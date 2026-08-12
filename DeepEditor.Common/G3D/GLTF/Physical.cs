using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using simd = System.Runtime.Intrinsics;
using numeric = System.Numerics;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace DeepEditor.Common.G3D.GLTF
{

    public abstract class BoundingBox
    {
        public enum BoundingBoxType
        {
            AABB,OBB,Sphere
        }
        public BoundingBoxType type;
        protected BoundingBox(BoundingBoxType t)
        {
            type = t;
        }
        public virtual void GetBoundingboxMeshPrimitive(ref MeshPrimitive mp){}

        public virtual bool IsIn(Vector3 point){return false;}
    }

    public class AABB : BoundingBox
    {
        public float minX;
        public float minY;
        public float minZ;
        public float maxX;
        public float maxY;
        public float maxZ;
        public void Reset()
        {
            minX = float.MaxValue;
            minY = float.MaxValue;
            minZ = float.MaxValue;
            maxX = float.MinValue;
            maxY = float.MinValue;
            maxZ = float.MinValue;
        }
        public AABB():base(BoundingBoxType.AABB)
        {
            Reset();
        }

        public AABB(AABB aabb):base(BoundingBoxType.AABB)
        {
            minX = aabb.minX;
            minY = aabb.minY;
            minZ = aabb.minZ;
            maxX = aabb.maxX;
            maxY = aabb.maxY;
            maxZ = aabb.maxZ;
        }
        ArrayBufferObject<float> ssboOutput;
        const int workload = 128;//of vertices / thread.
        const int maxGroup = 1024;//max vertices (32 * 1024 * 128) 4 millions
        const int maxOutBufferSize = maxGroup * 32 * 8 * 4;//local thread 32
        public void CalcAABBWithGPU(Drawable d,Matrix4 transform)
        {
            int vCount = d.meshPrimitive.VertexCount;
            int group = vCount / workload;
            if(vCount % workload!=0) group++;
            if(group>maxGroup)return;

            //float[] output = new float[group * 8];
            Computable computable = ComputableManager.accAABB;
            computable.SetBufferBinding(0,d.vboPosition);

            if(ssboOutput==null)
            {
                ssboOutput = new ArrayBufferObject<float>(BufferTarget.ShaderStorageBuffer,maxOutBufferSize);
            }
            
            computable.SetBufferBinding(1,ssboOutput);
            computable.parameters.UpdateUniformData("Workload",workload);
            computable.parameters.UpdateUniformData("Transform",transform);
            computable.parameters.UpdateUniformData("TotalCount",vCount);
            computable.SetupLocalWorkSize(32,1,1);
            computable.SetWorkGroup(group / 32,1,1);
            computable.Compute();

            unsafe
            {
                Reset();
                //float* fp = (float*)ssboOutput.MapBuffer(BufferAccess.ReadOnly);
                float* fp = (float*)ssboOutput.MapBufferRange(IntPtr.Zero,group*8*4);
                if((int)fp!=0)
                {
                    for(int i = 0;i<group;i++)
                    {
                        float xMin = fp[i*8+0];
                        float yMin = fp[i*8+1];
                        float zMin = fp[i*8+2];
                        float xMax = fp[i*8+3];
                        float yMax = fp[i*8+4];
                        float zMax = fp[i*8+5];

                        minX = Math.Min(minX,xMin);
                        minY = Math.Min(minY,yMin);
                        minZ = Math.Min(minZ,zMin);
                        maxX = Math.Max(maxX,xMax);
                        maxY = Math.Max(maxY,yMax);
                        maxZ = Math.Max(maxZ,zMax);
                        // if(xMin<minX)minX = xMin;
                        // if(yMin<minY)minY = yMin;
                        // if(zMin<minZ)minZ = zMin;
                        // if(xMax>maxX)maxX = xMax;
                        // if(yMax>maxY)maxY = yMax;
                        // if(zMax>maxZ)maxZ = zMax;
                    }
                }
                ssboOutput.UnMapBuffer();

            }
        }
    
        //https://software.intel.com/sites/landingpage/IntrinsicsGuide/#expand=2946,2946,2946,4085,2946,6111,6054,2537,2947,2946,2549,456,154,133,2946
        //https://github.com/dotnet/runtime/tree/master/src/libraries/System.Private.CoreLib/src/System/Runtime/Intrinsics
        public void CalcSIMD2(MeshPrimitive mp, Matrix4 t)
        {
            Reset();
            var m1 = simd.Vector128.Create(t.M11,t.M12,t.M13,t.M14);
            var m2 = simd.Vector128.Create(t.M21,t.M22,t.M23,t.M24);
            var m3 = simd.Vector128.Create(t.M31,t.M32,t.M33,t.M34);
           //var m4 = simd.Vector128.Create(t.M41,t.M42,t.M43,t.M44);

            var vMin = simd.Vector128.Create(minX,minY,minZ,0);
            var vMax = simd.Vector128.Create(maxX,maxY,maxZ,0);
            unsafe
            {
                fixed(float* fp = mp.positions)
                {
                    for(int  i =0;i<mp.positions.Length;i+=mp.comPosition)
                    {
                        //var v = simd.X86.Sse3.LoadVector128(fp+i);
                        var x = simd.Vector128.Create(mp.positions[i+0]);
                        var y = simd.Vector128.Create(mp.positions[i+1]);
                        var z = simd.Vector128.Create(mp.positions[i+2]);
                        var v1 = simd.X86.Sse3.Multiply(m1, x);
                        var v2 = simd.X86.Sse3.Multiply(m2, y);
                        var v3 = simd.X86.Sse3.Multiply(m3, z);
                        // var k1 = simd.X86.Sse3.HorizontalAdd(v1,v2);//a0 a1 b0 b1
                        // var k2 = simd.X86.Sse3.HorizontalAdd(v3,v3);//c0 c1 c0 c1
                        // var r = simd.X86.Sse3.HorizontalAdd(k1,k2);//a,b,c,c
                        v1 = simd.X86.Sse3.Add(v1,v2);
                        var r = simd.X86.Sse3.Add(v1,v3);

                        vMin = simd.X86.Fma.Min(vMin,r);
                        vMax = simd.X86.Fma.Max(vMax,r);
                    }
                }
            }
            
            minX = simd.Vector128.GetElement<float>(vMin,0);
            minY = simd.Vector128.GetElement<float>(vMin,1);
            minZ = simd.Vector128.GetElement<float>(vMin,2);
            maxX = simd.Vector128.GetElement<float>(vMax,0);
            maxY = simd.Vector128.GetElement<float>(vMax,1);
            maxZ = simd.Vector128.GetElement<float>(vMax,2);
        }

        public void CalcSIMD(MeshPrimitive mp, Matrix4 t)
        {
            Reset();

            numeric.Matrix4x4 m = new numeric.Matrix4x4(
                t.M11,t.M12,t.M13,t.M14,
                t.M21,t.M22,t.M23,t.M24,
                t.M31,t.M32,t.M33,t.M34,
                t.M41,t.M42,t.M43,t.M44);
            numeric.Vector4 vMin = new numeric.Vector4(minX,minY,minZ,0);
            numeric.Vector4 vMax = new numeric.Vector4(maxX,maxY,maxZ,0);
            for(int  i =0;i<mp.positions.Length;i+=mp.comPosition)
            {
                var px = (mp.positions[i+0]);
                var py = (mp.positions[i+1]);
                var pz = (mp.positions[i+2]);
                var v = new numeric.Vector4(px,py,pz,1);
                var r = numeric.Vector4.Transform(v,m);
                vMin = numeric.Vector4.Min(r,vMin);
                vMax = numeric.Vector4.Max(r,vMax);
            }
            
            minX = vMin.X;
            minY = vMin.Y;
            minZ = vMin.Z;
            maxX = vMax.X;
            maxY = vMax.Y;
            maxZ = vMax.Z;
        }

        public void Calc(MeshPrimitive mp)
        {//do it on GPU
            Reset();
            for(int i = 0;i<mp.positions.Length;i+=mp.comPosition)
            {
                float px = mp.positions[i+0];
                float py = mp.positions[i+1];
                float pz = mp.positions[i+2];
                maxX = Math.Max(px,maxX);
                minX = Math.Min(px,minX);
                maxY = Math.Max(py,maxY);
                minY = Math.Min(py,minY);
                maxZ = Math.Max(pz,maxZ);
                minZ = Math.Min(pz,minZ);
            }
        }

        public void Calc(MeshPrimitive mp, Matrix4 transform)
        {//do it on GPU
            Reset();
            Vector4 v = Vector4.Zero;
            for(int i = 0;i<mp.positions.Length;i+=mp.comPosition)
            {
                v.X = mp.positions[i+0];
                v.Y = mp.positions[i+1];
                v.Z = mp.positions[i+2];
                v = v * transform;
                maxX = Math.Max(v.X,maxX);
                minX = Math.Min(v.X,minX);
                maxY = Math.Max(v.Y,maxY);
                minY = Math.Min(v.Y,minY);
                maxZ = Math.Max(v.Z,maxZ);
                minZ = Math.Min(v.Z,minZ);
            }
        }

        public override void GetBoundingboxMeshPrimitive(ref MeshPrimitive mp)
        {
            mp.mode = PrimitiveType.Lines;
            mp.comPosition = 3;
            float[] p = {
                minX,minY,minZ,
                minX,minY,maxZ,
                minX,maxY,minZ,
                minX,maxY,maxZ,
                maxX,minY,minZ,
                maxX,minY,maxZ,
                maxX,maxY,minZ,
                maxX,maxY,maxZ
            };
            uint[] indices = {
                0,1, 2,3, 4,5, 6,7,
                0,4, 1,5, 2,6, 3,7,
                0,2, 1,3, 4,6, 5,7
            };
            mp.positions = p;
            mp.indices = indices;
        }

        public override bool IsIn(Vector3 point)
        {
            if(maxX<point.X)return false;
            if(maxY<point.Y)return false;
            if(maxZ<point.Z)return false;
            if(minX>point.X)return false;
            if(minY>point.Y)return false;
            if(minZ>point.Z)return false;
            return true;
        }

        public Vector3 Center
        {
            get
            {
                return new Vector3(minX+maxX,minY+maxY,minZ+maxZ)*0.5f;
            }
        }

        public float DiagonalLength
        {
            get
            {
                return MathF.Sqrt((maxX - minX)*(maxX - minX) +
                (maxY-minY)*(maxY-minY) + (maxZ-minZ)*(maxZ-minZ));
            }
        }


        public float MaxProjection(Vector3 dir)
        {
            Vector3 p = Extent;
            if(dir.X<0)p.X = -p.X;
            if(dir.Y<0)p.Y = -p.Y;
            if(dir.Z<0)p.Z = -p.Z;
            return Vector3.Dot(Center + p,dir);
        }

        public Vector3 Extent
        {
            get
            {
                return new Vector3(MathF.Abs(minX-maxX),
                MathF.Abs(minY-maxY),MathF.Abs(minZ-maxZ))*0.5f;
            }
        }

        public void Combine(AABB other)
        {
            minX = MathF.Min(minX,other.minX);
            minY = MathF.Min(minY,other.minY);
            minZ = MathF.Min(minY,other.minZ);
            maxX = MathF.Max(maxX,other.maxX);
            maxY = MathF.Max(maxY,other.maxY);
            maxZ = MathF.Max(maxZ,other.maxZ);
        }
    }

    public class SphereBox:BoundingBox
    {
        public Vector3 Center;
        public float Radius;
        public SphereBox():base(BoundingBoxType.Sphere){}
        public SphereBox(AABB aabb):base(BoundingBoxType.Sphere)
        {
            Center = aabb.Center;
            Radius = aabb.DiagonalLength;
        }
    }
    
    public class Physics
    {
        public static bool FrustumIntersect(Plane[] frustrum, AABB box)
        {
            for (int i = 0; i < 6; ++i)
            {
                var plane = frustrum[i];
                if (Vector3.Dot(box.Center, plane.Normal)
                    + box.Extent.X * Math.Abs(plane.Normal.X)
                    + box.Extent.Y * Math.Abs(plane.Normal.Y)
                    + box.Extent.Z * Math.Abs(plane.Normal.Z)
                    <= -plane.DistanceToOrigin)
                    return false;
            }
            return true;
        }
        
    }
}
