using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

using OpenTK;
using OpenTK.Mathematics;

namespace DeepEditor.Common.G3D.GLTF
{

    public struct Plane
    {
        //ax+by+cz+d=0;
        Vector4 abcd;
        public Plane(Vector4 v)
        {
            abcd = v;
        }

        public Vector3 Normal
        {
            get
            {
                return abcd.Xyz;
            }
        }

        public float DistanceToOrigin
        {
            get
            {
                return abcd.W;
            }
        }

        public static Plane FromPointNormal(Vector3 p,Vector3 n)
        {
            n.Normalize();
            float d0 = p.Length;
            float d1 = Vector3.Dot(p,n);
            return new Plane(new Vector4(n.Xzy,d1));
        }

        public float Distance(Vector3 p)
        {
            return Vector3.Dot(abcd.Xyz,p) + abcd.W;
        }

    }

    public static class Converts
    {
        public static Matrix4 Floats2Mat4Coloum(float[] data)
        {
            Matrix4 m = default;
            for(int x = 0;x<4;x++)
            for(int y = 0;y<4;y++)
                m[x,y] = data[x*4+y];
            return m;
        }
        
        public static Matrix4 Floats2Mat4Row(float[] data)
        {
            Matrix4 m = default;
            for(int x = 0;x<4;x++)
            for(int y = 0;y<4;y++)
                m[x,y] = data[y*4+x];
            return m;
        }
    }

    public static class Helpers
    {
        public static FilesWatcher shaderWatcher;
        public static Queue<string> shaderChanged;
        static void OnFileChanged(string name, byte[] data)
        {
            Console.WriteLine(name);
            shaderChanged.Enqueue(name);
        }

        public static void InitShaderWatcher(string dir)
        {
            var fsWatcher = new FileSystemWatcher(dir);
            if(shaderWatcher==null)
            {
                shaderWatcher = new FilesWatcher(fsWatcher);
                shaderChanged = new Queue<string>();
                shaderWatcher.fileChangedHandler = OnFileChanged;
            }
            
            fsWatcher.Changed += shaderWatcher.FileChanged;
            fsWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fsWatcher.IncludeSubdirectories = true;
            fsWatcher.EnableRaisingEvents = true;
        }

        public static List<Drawable> FrustrumCulling(Matrix4 mtxView, Matrix4 mtxProjection, List<Drawable> drawables)
        {
            List<Drawable> culled = new List<Drawable>();
            return culled;
        }

        public static void Orthogonal(Vector3 v, out Vector3 a, out Vector3 b)
        {
            if(MathF.Abs(Vector3.Dot(v,Vector3.UnitY))<0.001)
                a = Vector3.Cross(v,Vector3.UnitX);
            else
                a = Vector3.Cross(v,Vector3.UnitY);
            b = Vector3.Cross(v,a);
        }
        
        public static Plane[]GetPlanes(Matrix4 view, Matrix4 projection)
        {
            //https://cloud.tencent.com/developer/article/1119398
            //经过透视变换后的p',p'.w / p'.z = p.z
            Plane[] result = new Plane[6];

            //(default),like depth, nearZ=-1,farZ=1
            //projection.M34=1;//or convert projection coordinate like opengl coordinate. nearZ=1, farZ=-1.
            var vp = view*projection;
            Matrix4 Inv_VP = vp.Inverted();
            // Vector4 v0 = vp * new Vector4(0,10,5,1);
            // Vector4 v1 = new Vector4(0,10,5,1) * vp;
            // var v2 = Vector4.Transform(new Vector4(0,10,5,1),vp);
            Vector3[] pts = 
            {
                new Vector3(-1,1,-1),//near
                new Vector3(1,1,-1),
                new Vector3(1,-1,-1),
                new Vector3(-1,-1,-1),
                new Vector3(-1,1,1),//far
                new Vector3(1,1,1),
                new Vector3(1,-1,1),
                new Vector3(-1,-1,1)
            };
            int[] ni = {
                1,5,6,//+x
                0,3,7,//-x
                5,1,0,//+y
                7,3,2,//-y
                0,1,2,//+z
                7,6,5};//-z

            for(int i=0;i<8;i++)
            {
                var uvp = (new Vector4(pts[i],1) * Inv_VP);
                // Console.WriteLine(uvp.ToString());
                pts[i] = uvp.Xyz / uvp.W;

            }
            Vector3[] n = new Vector3[6];
            for(int i=0;i<6;i++)
            {
                int a = ni[i*3+0];
                int b = ni[i*3+1];
                int c = ni[i*3+2];
                n[i] = Vector3.Cross(pts[b]-pts[a],pts[c]-pts[a]).Normalized();
                result[i] = Plane.FromPointNormal(pts[a],n[i]);
            }
            return result;
        }

        public static Matrix4 GetLightVP(Vector4 light,BoundingBoxGroup boxs)
        {
            if(light.W==0)
            {
                return GetDirectionalLightVPMatrix(light.Xyz,boxs);
            }
            else
            {
                Matrix4 result = Matrix4.Zero;
                GetPointLightVPMatrix(light.Xyz, boxs,ref result);
                return result;
            }

        }

        public static bool GetPointLightVPMatrix(Vector3 lightPos, BoundingBoxGroup boxs, ref Matrix4 mat)
        {
            float maxDepth = float.NegativeInfinity;
            AABB combo = null;
            foreach(var e in boxs.Boxes)
            {
                var box = e.Key;
                AABB aabb = box as AABB;
                if(aabb!=null)
                {
                    if(combo==null)
                        combo = new AABB(aabb);
                    combo.Combine(aabb);
                }
            }
            if(combo.IsIn(lightPos))
                return false;

            Vector3 dir = combo.Center - lightPos;
            Vector3 u, v ;
            Helpers.Orthogonal(dir,out u,out v);
            float size = combo.DiagonalLength;//;
            float r = size /2;
            
            float fovy = (float)Math.Asin(r/dir.Length) * 2;
            var view = Matrix4.LookAt(lightPos, combo.Center, u);
            var prjection = Matrix4.CreatePerspectiveFieldOfView(fovy,1,0.1f,dir.Length+r);
            mat = view *prjection;
            return true;
        }
        
        public static Matrix4 GetDirectionalLightVPMatrix(Vector3 lightDir,  BoundingBoxGroup boxs)
        {
            float maxDepth = float.NegativeInfinity;
            AABB combo = null;
            foreach(var e in boxs.Boxes)
            {
                var box = e.Key;
                AABB aabb = box as AABB;
                if(aabb!=null)
                {
                    float mp = aabb.MaxProjection(lightDir);
                    if(mp>maxDepth) maxDepth = mp;

                    if(combo==null)
                        combo = new AABB(aabb);
                    combo.Combine(aabb);
                }
            }
            Plane plane = new Plane(new Vector4(lightDir,maxDepth));
            Vector3 u, v ;
            Helpers.Orthogonal(lightDir,out u,out v);
            // Console.WriteLine(combo.Center);
            float size = combo.DiagonalLength;// / 1.4142f;//sqrt(2)
            var view = Matrix4.LookAt(combo.Center - lightDir, combo.Center, u);
            var projection = Matrix4.CreateOrthographic(size,size,-size,size);
            return view * projection;
        }
    
        //http://extremelearning.com.au/how-to-generate-uniformly-random-points-on-n-spheres-and-n-balls/
        public static Vector3[] SphereSamples(int samples)
        {
            Vector3[] result = new Vector3[samples];
            Random r = new Random();
            int k = 0;
            do
            {
                //float phi = r.Next(65536) / 65536f * MathF.PI * 2f;
                float u = r.Next(65536) / 65536f * 2 - 1;
                float phi = r.Next(65536) / 65536f * MathF.PI * 2f;
                float z = u;
                float t =  MathF.Sqrt((1 - z*z));
                float x = MathF.Cos(phi) * t;
                float y = MathF.Sin(phi) * t;
                var v = new Vector3(x,y,z);
                result[k] = v.Normalized() * 10;
                k++;
            }while(k<samples);
            return result;
        }

        public static float NormalDistribution1D(float x, float range, float center)
        {
            var r = 1 / (Math.Sqrt(2 * Math.PI) * range) * 
                Math.Pow(Math.E, -(x - center) * (x - center) / (2 * range * range));
            return (float)r;
        }

        public static float NormalDistribution2D(float x, float y, float range, float centerX,float centerY)
        {
            var r = 1 / ((2 * Math.PI) * Math.Pow(range,2)) * 
                Math.Pow(Math.E, -((x - centerX) * (x - centerX) + (y  - centerY) * (y - centerY)) / (2 * range * range));
            return (float)r;
        }

        public static Vector3[] SphereSamplesMuller(int samples)
        {
            Vector3[] result = new Vector3[samples];
            MathNet.Numerics.Distributions.Normal  nd = new MathNet.Numerics.Distributions.Normal();
            // Random r = new Random();
            int k = 0;
            do
            {
                //float phi = r.Next(65536) / 65536f * MathF.PI * 2f;
                float u = (float)nd.RandomSource.NextDouble() * 2 - 1;
                float v = (float)nd.RandomSource.NextDouble() * 2 - 1;
                float w = (float)nd.RandomSource.NextDouble() * 2 - 1;
                float norm = MathF.Sqrt(u*u+v*v+w*w);
                float x = u/norm;
                float y = v/norm;
                float z = w/norm;
                var pos = new Vector3(x,y,z);
                result[k] = pos.Normalized() * 10;
                k++;
            }while(k<samples);
            return result;
        }
    }

    public class SphericalHarmonic
    {
        public void LegendrePolynomial()
        {
        }

        //AssiociatedLegendrePolynomial
        public void ALP()
        {

        }
    }

    public delegate void FileChangedHandler(string name, byte[] data);
    public class FilesWatcher
    {
        MD5 md5;
        public Dictionary<string,byte[]> kv;
        FileSystemWatcher watcher;
        public FilesWatcher(FileSystemWatcher fsw)
        {
            md5 = MD5.Create();
            kv = new Dictionary<string, byte[]>();
            watcher = fsw;
        }

        public FileChangedHandler fileChangedHandler;
        public void FileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo(e.FullPath);
                string fullname = fi.FullName;
                byte[] data = File.ReadAllBytes(fullname);
                byte[] code = md5.ComputeHash(data);
                if(!HashMatch(fullname,code))
                {
                    fileChangedHandler(fullname,data);
                    kv[fullname] = code;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error FileChanged:{0}", ex.Message);
            }
        }

        public bool HashMatch(string file, byte[] code)
        {
            byte[] hash;
            if(kv.TryGetValue(file,out hash))
            {
                if(code.Length!=hash.Length)return false;
                for(int i =0;i<hash.Length;i++)
                {
                    if(hash[i]!=code[i])return false;
                }
                return true;
            }
            return false;
        }

        public void RegisterFile(string file,out string fullName)
        {
            FileInfo fi = new FileInfo(file);
            fullName = fi.FullName;
            byte[] data = File.ReadAllBytes(file);
            byte[] code = md5.ComputeHash(data);
            if(!kv.ContainsKey(fullName))
            kv.Add(fullName,code);
        }

        //低差异序列,该序列是把十进制数字的二进制表示镜像翻转到小数点右边而得
        float Van_Der_Corpus_Sequence(uint bits)
        {
            bits = (bits << 16) | (bits >> 16);
            bits = ((bits & 0x55555555u) << 1) | ((bits & 0xAAAAAAAAu) >> 1);
            bits = ((bits & 0x33333333u) << 2) | ((bits & 0xCCCCCCCCu) >> 2);
            bits = ((bits & 0x0F0F0F0Fu) << 4) | ((bits & 0xF0F0F0F0u) >> 4);
            bits = ((bits & 0x00FF00FFu) << 8) | ((bits & 0xFF00FF00u) >> 8);
            return (float)bits * 2.3283064365386963e-10f; // / 0x100000000
        }

        // vec2 Hammersley(uint i, uint N)
        // {
        //     return vec2(float(i)/float(N), RadicalInverse_VdC(i));
        // } 

        float half(ushort d)
        {
            int sign = d>>15;//sign
            int exponent = (d>>10)&31 - 15;
            int fraction = d&1023;
            return 0;
        }

    //monte-carlo intergration
    //for [a,b], integration function f(x)
    //assume number of the sample N.
    //probability density function PDF.
    //it equvliants to 1 / N * sum_0^{N-1}(f(x)/pdf(x))

    //but what is the PDF.

    }
}