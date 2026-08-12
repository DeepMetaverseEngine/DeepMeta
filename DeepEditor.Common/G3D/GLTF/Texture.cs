using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DeepEditor.Common.G3D.GLTF
{
    public class SamplerInfo
    {
        public TextureMagFilter MagFilter;
        public TextureMinFilter MinFilter;
        public TextureWrapMode WrapS;
        public TextureWrapMode WrapT;
        public TextureWrapMode WrapR;

        static SamplerInfo self = null;
        public static SamplerInfo GetDefault()
        {
            if(self==null)
            {
                self= new SamplerInfo();
                self.WrapR = TextureWrapMode.ClampToEdge;//clamp to edge. or it will has gap.
                self.WrapS = TextureWrapMode.ClampToEdge;
                self.WrapT = TextureWrapMode.ClampToEdge;
                self.MagFilter = TextureMagFilter.Linear;
                self.MinFilter = TextureMinFilter.Linear;
            }
            return self;
        }
    }

    public abstract class Texture : IDOjbect
    {
        public bool Layered{get;private set;}
        public TextureTarget textureType{get;private set;}
        public PixelInternalFormat internalFormat;
        public int levels = 1;
        public Texture(TextureTarget t,bool l)
        {
            textureType = t;
            ID = GL.GenTexture();
            Layered = l;
        }
        public SamplerInfo samplerInfo;
        public virtual void SetupSampler(SamplerInfo sampler)
        {
            throw new NotImplementedException();
        }
        
        public virtual void GenerateMipmap(){}
        public static int MaxMipmapLevel(int minWidthHeight)
        {
            uint[] t = {0,1,2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384};
            for(int i =0;i<t.Length;i++)
            {
                if(minWidthHeight<t[i])
                return i - 1;
            }
            return 0;
        }
    }

    //texture1d from curve
    public class Texture1D:Texture
    {
        public ImageData1D imageData;
        public Texture1D(ImageData1D img):base(TextureTarget.Texture1D,false)
        {
            imageData = img;
            GL.BindTexture(TextureTarget.Texture1D,ID);
            GL.TexImage1D(TextureTarget.Texture1D,0,PixelInternalFormat.Srgb,(int)img.width,
            0,imageData.GLPixelFormat,PixelType.UnsignedByte,img.pixels);
            // GL.GenerateMipmap(GenerateMipmapTarget.Texture1D);
        }

        public Texture1D(int size,PixelInternalFormat internalFormat, PixelFormat f,IntPtr t):base(TextureTarget.Texture1D,false)
        {
            GL.BindTexture(TextureTarget.Texture1D,ID);
            GL.TexImage1D(TextureTarget.Texture1D,0,internalFormat,size,0,f,PixelType.Float,t);
        }
        
        public override void SetupSampler(SamplerInfo sampler)
        {
            samplerInfo = sampler;
            GL.BindTexture(TextureTarget.Texture1D,ID);
            GL.TexParameter(TextureTarget.Texture1D,TextureParameterName.TextureWrapS,(int)sampler.WrapS);
            GL.TexParameter(TextureTarget.Texture1D,TextureParameterName.TextureMagFilter,(int)sampler.MagFilter);
            GL.TexParameter(TextureTarget.Texture1D,TextureParameterName.TextureMinFilter,(int)sampler.MinFilter);
        }
    }

    public class Texture2D:Texture
    {
        public ImageData2D imageData;

        public Texture2D(uint width, uint height, System.Drawing.Imaging.PixelFormat format):
        base(TextureTarget.Texture2D,false)
        {
            imageData = new ImageData2D(width,height,format);
            GL.BindTexture(TextureTarget.Texture2D,ID);
            GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Srgb,(int)width,(int)height,
            0,imageData.GLPixelFormat,PixelType.UnsignedByte,IntPtr.Zero);
        }

        public Texture2D(ImageData2D img):base(TextureTarget.Texture2D,false)
        {
            imageData = img;
            ///GL.Enable(EnableCap.Texture2D);
            ErrorCode err = GL.GetError();
            GL.BindTexture(TextureTarget.Texture2D,ID);
            GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Srgb,(int)img.width,(int)img.height,
            0,imageData.GLPixelFormat,PixelType.UnsignedByte,img.pixels);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            err = GL.GetError();

            // GL.TexSubImage2D(TextureTarget.Texture2D,0,0,0,(int)imageData.width,(int)imageData.height,PixelFormat.RgbaInteger,PixelType.UnsignedByte,img.pixels);
            // err = GL.GetError();
        }

        public Texture2D(uint width, uint height, SizedInternalFormat f, int level = 1):
        base(TextureTarget.Texture2D,false)
        {
            //imageData = new ImageData2D(width,height,System.Drawing.Imaging.PixelFormat.DontCare);
            GL.BindTexture(TextureTarget.Texture2D,ID);
            GL.TexStorage2D(TextureTarget2d.Texture2D,level,f,(int)width,(int)height);
            internalFormat = (PixelInternalFormat)f;
        }

        public override void SetupSampler(SamplerInfo sampler)
        {
            samplerInfo = sampler;
            GL.BindTexture(TextureTarget.Texture2D,ID);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapS,(int)sampler.WrapS);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapT,(int)sampler.WrapT);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)sampler.MagFilter);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)sampler.MinFilter);//LinearMipmapNearest需要使用GenerateMipmap。纹理采样为0,0,0,0
            // GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Linear);
            // GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.Linear);
        }

        public static Texture2D GenerateBRDF_LUT(int size)
        {
            Texture2D tex = new Texture2D((uint)size,(uint)size,SizedInternalFormat.Rg16f);
            GL.BindTexture(TextureTarget.Texture2D,tex.ID);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapS,(int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapT,(int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Linear);

            int program = ShadersProgram.GenBRDF_LUT.ID;
            ShadersProgram.GenBRDF_LUT.Active();
        
            GL.BindImageTexture(0,tex.ID,0,true,0,TextureAccess.ReadWrite,SizedInternalFormat.Rg16f);
            GL.DispatchCompute(size / 16,size / 16, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.ShaderImageAccessBarrierBit);
            
            return tex;
        }
    }

    public class Texture2DMS:Texture
    {
        public Texture2DMS(uint width, uint height, int sample, SizedInternalFormat f):
        base(TextureTarget.Texture2DMultisample,false)
        {
            //imageData = new ImageData2D(width,height,System.Drawing.Imaging.PixelFormat.DontCare);
            GL.BindTexture(TextureTarget.Texture2DMultisample,ID);
            GL.TexStorage2DMultisample(TextureTargetMultisample2d.Texture2DMultisample,sample,f,(int)width,(int)height,true);
            internalFormat = (PixelInternalFormat)f;
        }
    }

    public class TextureCube:Texture
    {
        public ImageData2D[] imgCube;
        public TextureCube(ImageData2D xp,ImageData2D xn,ImageData2D yp,ImageData2D yn,ImageData2D zp,ImageData2D zn):
        base(TextureTarget.TextureCubeMap,true)
        {
            imgCube = new ImageData2D[6];
            imgCube[0] = xp;
            imgCube[1] = xn;
            imgCube[2] = yp;
            imgCube[3] = yn;
            imgCube[4] = zp;
            imgCube[5] = zn;   
            GL.BindTexture(TextureTarget.TextureCubeMap,ID);
            PixelInternalFormat pif = PixelInternalFormat.Rgba16f;//fixed internal format rgba16f
            for(int i=0;i<6;i++)
            {
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i,0,pif,(int)imgCube[i].width,(int)imgCube[i].height,
                0,imgCube[i].GLPixelFormat,PixelType.UnsignedByte,imgCube[i].pixels);
            }
        }

        private TextureCube(int size):base(TextureTarget.TextureCubeMap,true)
        {
            imgCube = new ImageData2D[6];
            GL.BindTexture(TextureTarget.TextureCubeMap,ID);
            int numMipMap = Texture.MaxMipmapLevel(size);
            GL.TexStorage2D(TextureTarget2d.TextureCubeMap,numMipMap,SizedInternalFormat.Rgba16f,size,size);
            internalFormat = (PixelInternalFormat)SizedInternalFormat.Rgba16f;
            // for(int i =0;i<6;i++)
            // GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX+i,0,PixelInternalFormat.Rgba16f,size,size,0,PixelFormat.Rgba,PixelType.Float,IntPtr.Zero);
        }
        
        public static TextureCube ProccessEquirectangularMap(ImageData2D imgHDR, int cubeSize)
        {
            TextureCube tCube = new TextureCube(cubeSize);
            ShadersProgram.ProcessEquirectangularMap.Active();
            int program = ShadersProgram.ProcessEquirectangularMap.ID;
            
            int texHDR = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D,texHDR);
            GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgba16f,
            (int)imgHDR.width,(int)imgHDR.height,0,
            PixelFormat.Rgb,PixelType.Float,imgHDR.pixels);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapS,(int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapT,(int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)TextureMagFilter.Linear);

            int usize = GL.GetUniformLocation(program,"size");
            int uCEquirectangularMap = GL.GetUniformLocation(program,"EquirectangularMap");
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D,texHDR);
            GL.Uniform1(uCEquirectangularMap,0);
            GL.Uniform1(usize,cubeSize);

            GL.BindImageTexture(0,tCube.ID,0,true,0,TextureAccess.ReadWrite,SizedInternalFormat.Rgba16f);

            GL.DispatchCompute(cubeSize/16 ,cubeSize/16, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.ShaderImageAccessBarrierBit);

            GL.BindTexture(TextureTarget.TextureCubeMap,tCube.ID);
            for(int i = 0;i<6;i++)
            {
                byte[] pixels = new byte[cubeSize * cubeSize * 4];
                GL.GetTexImage(TextureTarget.TextureCubeMapPositiveX+i,0,PixelFormat.Rgba,PixelType.UnsignedByte,pixels);
                ImageData2D img = new ImageData2D((uint)cubeSize,(uint)cubeSize,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                img.width = (uint)cubeSize;
                img.height = (uint)cubeSize;
                img.imgformat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                img.pixels = pixels;
                tCube.imgCube[i] = img;
            }
            GL.BindTexture(TextureTarget.Texture2D,0);
            GL.DeleteTexture(texHDR);
            return tCube;
        }

        public override void SetupSampler(SamplerInfo sampler)
        {
            samplerInfo = sampler;
            GL.BindTexture(TextureTarget.TextureCubeMap,ID);
            GL.TexParameter(TextureTarget.TextureCubeMap,TextureParameterName.TextureWrapS,(int)sampler.WrapS);
            GL.TexParameter(TextureTarget.TextureCubeMap,TextureParameterName.TextureWrapT,(int)sampler.WrapT);
            GL.TexParameter(TextureTarget.TextureCubeMap,TextureParameterName.TextureWrapR,(int)sampler.WrapR);
            GL.TexParameter(TextureTarget.TextureCubeMap,TextureParameterName.TextureMinFilter,(int)sampler.MinFilter);//LinearMipmapNearest需要使用GenerateMipmap。纹理采样为0,0,0,0
            GL.TexParameter(TextureTarget.TextureCubeMap,TextureParameterName.TextureMagFilter,(int)sampler.MagFilter);//LinearMipmapNearest需要使用GenerateMipmap。纹理采样为0,0,0,0            
        }

        float[] GetPixels(int size)
        {
            float[] result = new float[size*size*4];
            for(int i=0;i<size;i++)
            for(int j=0;j<size;j++)
            {
                int id = i+j*size;
                result[id*4+0] = 0.5f;
                result[id*4+1] = (float)i/size;
                result[id*4+2] = (float)j/size;
                result[id*4+3] = 1;
            }
            return result;
        }

        public override void GenerateMipmap()
        {
            GL.BindTexture(TextureTarget.TextureCubeMap,ID);
            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
        }

        public TextureCube GenerateIrradianceMap(int cubesize)
        {
            TextureCube irr = new TextureCube(cubesize);
            int query = GL.GenQuery();
            GL.BeginQuery(QueryTarget.TimeElapsed,query);

            ShadersProgram.GenIrradianceMap.Active();
            int program = ShadersProgram.GenIrradianceMap.ID;
            int uCubeSize = GL.GetUniformLocation(program,"CubeSize");
            int uCubeMap = GL.GetUniformLocation(program,"CubeMap");
            GL.Uniform1(uCubeSize,cubesize);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap,ID);
            GL.Uniform1(uCubeMap,0);
            //int[] tt = {irr.ID,ID};
            //GL.BindImageTextures(0,2,tt);
            GL.BindImageTexture(0,irr.ID,0,true,0,TextureAccess.ReadWrite,SizedInternalFormat.Rgba16f);
            //GL.BindImageTexture(1,ID,0,true,0,TextureAccess.ReadWrite,SizedInternalFormat.Rgba16f);
            // GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer,0,ssboDebug);

            // float[] data = new float[imageXPlus.width*imageXPlus.height*4];
            // GL.BindTexture(TextureTarget.Texture2D,test2d);
            // GL.GetTexImage(TextureTarget.Texture2D,0,PixelFormat.Rgba,PixelType.Float,data);

            GL.DispatchCompute(cubesize/16 ,cubesize/16,1);
            GL.MemoryBarrier(MemoryBarrierFlags.ShaderImageAccessBarrierBit);
            
            // float[] debugdata = new float[cubesize * cubesize];
            // GL.BindBuffer(BufferTarget.ShaderStorageBuffer,ssboDebug);
            // GL.GetBufferSubData(BufferTarget.ShaderStorageBuffer,IntPtr.Zero,debugdata.Length,debugdata);

            GL.EndQuery(QueryTarget.TimeElapsed);
            int[] time ={0};
            GL.GetQueryObject(query,GetQueryObjectParam.QueryResult,time);
            Console.WriteLine("GenerateIrradianceMap size:{0}x{0},time(us):{1}",cubesize, time[0] *0.001f);

            // float[] px = new float[4 * size * size];
            // GL.BindTexture(TextureTarget.TextureCubeMap,irr.ID);
            // GL.GetTexImage(TextureTarget.TextureCubeMapPositiveX,0,PixelFormat.Rgba,PixelType.Float,px);
            return irr;
        }

        //镜面IBL,预滤波环境贴图 基于重要假设，视角方向V=镜面反射方向R=输出采样方向N。
        //所以才使得预过滤环境贴图不需要关心视角方向，可以先拿出来做预计算
        public TextureCube GeneratePreFilteredEnviromentMap(int size)
        {
            TextureCube pfem = new TextureCube(size);
            GL.BindTexture(TextureTarget.TextureCubeMap,pfem.ID);
            GL.TexParameter(TextureTarget.TextureCubeMap,TextureParameterName.TextureWrapS,(int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap,TextureParameterName.TextureWrapT,(int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap,TextureParameterName.TextureWrapR,(int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.TextureCubeMap,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Linear);
            //pfem.GenerateMipmap();
            
            int program = ShadersProgram.GenPrefilterEnvMap.ID;
            int curSize;
            int numMipMap;
            // GL.GetTexParameter(TextureTarget.TextureCubeMap,GetTextureParameter.TextureMaxLevel,out numMipMap);
            GL.GetTexParameter(TextureTarget.TextureCubeMap,GetTextureParameter.TextureMaxLevel,out numMipMap);
            numMipMap = Texture.MaxMipmapLevel(size);
            ShadersProgram.GenPrefilterEnvMap.Active();
            for(int i = 0;i<numMipMap;i++)
            {
                //GL.GetTexLevelParameter(TextureTarget.TextureCubeMapPositiveX,i,GetTextureParameter.TextureWidth,out curSize);
                curSize = size>>i;
                if(curSize<8)break;
                int uCubeMap = GL.GetUniformLocation(program,"CubeMap");
                int uCubeSize = GL.GetUniformLocation(program,"CubeSize");
                int uRoughness = GL.GetUniformLocation(program,"Roughness");
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.TextureCubeMap,ID);
                GL.Uniform1(uCubeMap,0);
                GL.Uniform1(uCubeSize,curSize);
                GL.Uniform1(uRoughness, (float)i / (numMipMap-4));

                GL.BindImageTexture(0,pfem.ID,i,true,0,TextureAccess.ReadWrite,SizedInternalFormat.Rgba16f);
                GL.DispatchCompute(size / 8,size / 8, 1);
                GL.MemoryBarrier(MemoryBarrierFlags.ShaderImageAccessBarrierBit);
            }
            return pfem;
        }
    }
    
    public class Texture3D:Texture
    {
        public ImageData3D imageData;
        public Texture3D(ImageData3D img):base(TextureTarget.Texture3D,false)
        {
            imageData = img;
            GL.BindTexture(TextureTarget.Texture3D,ID);
            GL.TexImage3D(TextureTarget.Texture3D,0,PixelInternalFormat.Srgb,(int)img.width,(int)img.height,(int)img.depth,
            0,imageData.GLPixelFormat,PixelType.UnsignedByte,img.pixels);
            //GL.GenerateMipmap(GenerateMipmapTarget.Texture3D);
        }

    }

    public class Texture2DArray: Texture
    {
        public ImageData3D imageData;
        public Texture2DArray(ImageData3D img):base(TextureTarget.Texture2DArray,true)
        {
            imageData = img;
            GL.BindTexture(TextureTarget.Texture2DArray,ID);
            GL.TexImage3D(TextureTarget.Texture2DArray,0,PixelInternalFormat.Srgb,(int)img.width,(int)img.height,(int)img.depth,
            0,imageData.GLPixelFormat,PixelType.UnsignedByte,img.pixels);
            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
        }
        
    }

    
    public class ImmutableTexture : Texture
    {
        public TextureAccess Accessable{get;set;}
        protected ImmutableTexture(TextureTarget t,bool l):base(t,l)
        {
            Accessable = TextureAccess.ReadWrite;
        }
    }

    public class ImmutableTexture2D : ImmutableTexture
    {
        public ImmutableTexture2D(int width,int height,int levels,SizedInternalFormat format):
        base(TextureTarget.Texture2D,false)
        {
            this.levels = levels;
            this.internalFormat = (PixelInternalFormat)format;
            GL.BindTexture(TextureTarget.Texture2D,ID);
            GL.TexStorage2D(TextureTarget2d.Texture2D,levels,format,width,height);
            GL.BindTexture(TextureTarget.Texture2D,0);
        }

        public void Clear()
        {
            GL.BindTexture(TextureTarget.Texture2D,ID);
            for(int i =0;i<levels;i++)
            {
                float[] zero = {0,0,0,0};
                GL.ClearTexImage(ID,i,PixelFormat.Rgba,PixelType.Float,zero);
            }
            GL.BindTexture(TextureTarget.Texture2D,0);
        }
    }

    public class BindlessTexture
    {
        public bool Resident
        {get; private set;}
        public long Handle
        {get; private set;}
        
        Texture bindlessTex;
        public BindlessTexture(Texture tex,bool resident = false)
        {
            Resident = resident;
            bindlessTex = tex;
            Handle = GL.Arb.GetTextureHandle(tex.ID);
            if(resident)
                GL.Arb.MakeTextureHandleResident(Handle);
            else
                GL.Arb.MakeTextureHandleNonResident(Handle);
        }

        public void Update(int program,int location)
        {
            GL.Arb.ProgramUniformHandle(program,location,Handle);
        }
    }
}