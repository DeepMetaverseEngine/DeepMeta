using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace DeepEditor.Common.G3D.GLTF
{
    //fxaa
    //dof
    //ssao
    //ssr
    //god ray (volume light)
    //outline
    //motion blur
    //glow

    /*
    PostProcess Effect处理顺序
    Anti-aliasing
    Blur
    Builtins:
    DepthOfField
    Uber Effects:

    AutoExposure
    LensDistortion
    CHromaticAberration
    Bloom
    Vignette
    Grain
    ColorGrading(tonemapping)
    */

    public class Computable
    {
        public ShadersProgram shaderProgram;
        public int workgroupX{get;private set;}
        public int workgroupY{get;private set;}
        public int workgroupZ{get;private set;}
        MemoryBarrierFlags barrierFlags;
        public ProgramParameters parameters;
        Dictionary<int,BufferObject> ssboBindings;
        public Computable(ShadersProgram p, MemoryBarrierFlags b = MemoryBarrierFlags.AllBarrierBits)
        {
            shaderProgram = p;
            barrierFlags = b;
            parameters = new ProgramParameters(p);
            ssboBindings = new Dictionary<int,BufferObject>();
        }

        public void SetBufferBinding(int index, BufferObject bo)
        {
            if(ssboBindings.ContainsKey(index))
                ssboBindings[index] = bo;
            else
                ssboBindings.Add(index,bo);
        }
        
        int localWorkSizeX,localWorkSizeY,localWorkSizeZ;
        public void SetupLocalWorkSize(int lx, int ly,int lz=1)
        {
            localWorkSizeX = lx;
            localWorkSizeY = ly;
            localWorkSizeZ = lz;
        }

        public void SetWorkGroup(int x,int y,int z=1)
        {
            // int gx = x / localWorkSizeX;
            // int gy = y / localWorkSizeY;
            // int gz = z / localWorkSizeZ;
            // if(x%localWorkSizeX!=0)gx++;
            // if(y%localWorkSizeY!=0)gy++;
            // if(z%localWorkSizeZ!=0)gz++;

            workgroupX = x;
            workgroupY = y;
            workgroupZ = z;
        }

        public void Compute()
        {
            GL.UseProgram(shaderProgram.ID);
            parameters.UpdateUniforms();
            foreach(var e in ssboBindings)
            {
                GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer,e.Key,e.Value.ID);
            }
            ImageBindingPoint.UpdateImages();
            GL.DispatchCompute(workgroupX,workgroupY,workgroupZ);
            GL.MemoryBarrier(barrierFlags);
        }
    }


    public static class ComputableManager
    {
        static Dictionary<string,Computable> acc = new Dictionary<string, Computable>();
        public static void Init()
        {
            InitAABB();
            InitDepthSAT();
        }

        public static Computable accAABB;
        public static Computable depthSAT;
        static void InitAABB()
        {
            var aabb = ShaderSourceManager.LoadCSShaderFile(@"shaders\Acc\aabb.glcs");
            Computable c = new Computable(aabb);
            c.SetupLocalWorkSize(32,1,1);
            acc.Add("AABB",c);
            accAABB = c;
        }

        static void InitDepthSAT()
        {
            var sat = ShaderSourceManager.LoadCSShaderFile(@"shaders\Acc\depthSAT.glcs");
            Computable c = new Computable(sat);
            c.SetupLocalWorkSize(64,1,1);
            acc.Add("depthSAT",c);
            depthSAT = c;
        }

        public static Computable Find(string name)
        {
            if(acc.ContainsKey(name))
                return acc[name];
            return null;
        }
    }

    public class ImageBindingPoint
    {
        public static int maxImages = 16;
        public static ImageBindingPoint[] Images = new ImageBindingPoint[maxImages];
        public Texture texture
        {
            get;
            private set;
        }

        private static ImageBindingPoint GetBindingPoint(int index)
        {
            if(index>=0&&index<maxImages)
            {
                if(Images[index]==null)
                    Images[index] = new ImageBindingPoint();
                return Images[index];
            }
            return null;
        }

        public static bool BindImage(int bindingindex,Texture tex)
        {
            if(bindingindex>=0&&bindingindex<maxImages)
            {
                ImageBindingPoint bp = GetBindingPoint(bindingindex);
                bp.texture = tex;
                return true;
            }
            return false;
        }

        public static bool UpdateImage(int bindingindex,Texture tex)
        {
            if(bindingindex>=0&&bindingindex<maxImages)
            {
//                 if(tex.Immutable)
//                 {
//                     GL.BindImageTexture(bindingindex,tex.ID,0,
//                     tex.Layered,0,tex.Accessable,(SizedInternalFormat)tex.internalFormat);
//                 }
//                 else
                {
                    GL.BindImageTexture(bindingindex,tex.ID,0,
                    tex.Layered,0,TextureAccess.ReadOnly,(SizedInternalFormat)tex.internalFormat);
                }
                ImageBindingPoint bp = GetBindingPoint(bindingindex);
                bp.texture = tex;
                return true;
            }
            return false;
        }

        public static void UpdateImages()
        {
            for(int i =0;i<maxImages;i++)
            {
                if(Images[i]==null)continue;
                var tex = Images[i].texture;
//                 if(tex.Immutable)
//                 {
//                     GL.BindImageTexture(i,tex.ID,0,
//                     tex.Layered,0,tex.Accessable,(SizedInternalFormat)tex.internalFormat);
//                 }
//                 else
                {
                    GL.BindImageTexture(i,tex.ID,0,
                    tex.Layered,0,TextureAccess.ReadOnly,(SizedInternalFormat)tex.internalFormat);
                }
            }
        }
    }

    public class PostEffect
    {//computable + framebuffer
        public static RenderPass Pass
        {
            get
            {
                if(RenderPass.postEffectPass==null)
                {
                    int w, h;
                    FramebufferObject.GetViewport(out w, out h);
                    RenderPass.postEffectPass = new RenderPass(new FramebufferObject(w,h));
                    int v;
                    GL.GetInteger(GetPName.MaxTextureImageUnits,out v);
                    Console.WriteLine("MaxTextureImageUnits:{0}",v);
                    GL.GetInteger(GetPName.MaxUniformBufferBindings,out v);
                    Console.WriteLine("MaxUniformBufferBindings:{0}",v);
                    GL.GetInteger(GetPName.MaxCombinedImageUniforms,out v);
                    Console.WriteLine("MaxCombinedImageUniforms:{0}",v);
                    GL.GetInteger(GetPName.MaxComputeImageUniforms,out v);
                    Console.WriteLine("MaxComputeImageUniforms:{0}",v);
                }
                return RenderPass.postEffectPass;
            }
        }
        public static void InitFx()
        {
            InitGlow();
            InitSSAO();
            InitDof();
            InitSSR();
        }

        public static PostEffect GlowFX{get;private set;}
        static void InitGlow()
        {//考虑用mipmap优化
            if(GlowFX==null)
            {
                var spGlow = ShaderSourceManager.LoadCSShaderFile(@"shaders\FX\glow.glcs");
                int w = FramebufferObject.Default.width;
                int h = FramebufferObject.Default.height;
                var fx = new PostEffect(w,h ,spGlow);
                fx.texOuput = fx.InitOutputImage(SizedInternalFormat.Rgba8,0);
                GlowFX = fx;
            }
        }

        public static PostEffect SSAOFX{get;private set;}
        static void InitSSAO()
        {
            if(SSAOFX==null)
            {
                var spSSAO = ShaderSourceManager.LoadCSShaderFile(@"shaders\FX\ssao.glcs");
                int w = FramebufferObject.Default.width;
                int h = FramebufferObject.Default.height;
                var fx = new PostEffect(w,h ,spSSAO);
                var samples =  MakeHalfSphereSamples(1024,Vector3.UnitZ);
                fx.co.parameters.UpdateUniformData("samplesize",64);
                fx.co.parameters.UpdateUniformData("radius",1.0f);
                fx.co.parameters.UpdateUniformData("Projection",Matrix4.Identity);
                fx.texOuput = fx.InitOutputImage(SizedInternalFormat.R16f,0);
                // fx.co.parameters.UpdateUniformData("samples",samples);

                samples.internalFormat = PixelInternalFormat.Rgba32f;
                fx.SetupInputImage(samples,1);
                SSAOFX = fx;
            }
        }

        static Texture1D MakeHalfSphereSamples(int s,Vector3 normal)
        {
            Vector3[] vector = Helpers.SphereSamples(s);
            for(int i =0;i<vector.Length;i++)
            {
                var v = vector[i];
                if(Vector3.Dot(v,normal)<0)
                vector[i] = -v;
            }
            //test
            // float[] t = new float[s * 3];
            // for(int i = 0;i<s;i++)
            // {
            //     t[i*3+0]=0;
            //     t[i*3+1]=1;
            //     t[i*3+2]=0;
            // }
            
            Texture1D t1d = null;
            unsafe
            {
                fixed(void* p = vector)
                {
                    t1d = new Texture1D(vector.Length,PixelInternalFormat.Rgba32f,PixelFormat.Rgb,(IntPtr)p);
                }
            }
            SamplerInfo si = new SamplerInfo();
            si.MagFilter = TextureMagFilter.Linear;
            si.MinFilter = TextureMinFilter.Linear;
            si.WrapR = TextureWrapMode.ClampToEdge;
            si.WrapS = TextureWrapMode.ClampToEdge;
            si.WrapT = TextureWrapMode.ClampToEdge;
            t1d.SetupSampler(si);
            return t1d;
        }

        static void InitSSR()
        {

        }

        static void InitDof()
        {

        }

        Computable co;
        int width;
        int height;
        public PostEffect(int w,int h,ShadersProgram sp)
        {
            width = w;
            height = h;
            co = new Computable(sp);
            co.SetupLocalWorkSize(8,8);
            co.SetWorkGroup(w/8,h/8);
        }
        
        public static Texture gAlbedo
        {get =>RenderPass.deferredPrePass.FBO.texAttachments[FramebufferAttachment.ColorAttachment0];}
        public static Texture gPosition
        {get =>RenderPass.deferredPrePass.FBO.texAttachments[FramebufferAttachment.ColorAttachment1];}
        public static Texture gNormal
        {get =>RenderPass.deferredPrePass.FBO.texAttachments[FramebufferAttachment.ColorAttachment2];}
        public static Texture gMetallicRoughness
        {get =>RenderPass.deferredPrePass.FBO.texAttachments[FramebufferAttachment.ColorAttachment3];}
        public static Texture gEmbientAO
        {get =>RenderPass.deferredPrePass.FBO.texAttachments[FramebufferAttachment.ColorAttachment4];}
        public static Texture gColor
        {get => RenderPass.defferedPass.FBO.texAttachments[FramebufferAttachment.ColorAttachment0];}

        Dictionary<Texture,int> imgBinding = new Dictionary<Texture, int>();
        void SetupInputImage(Texture tex, int bindingindex)
        {
            imgBinding.Add(tex,bindingindex);
        }

        Texture InitOutputImage(SizedInternalFormat format,int bindingindex)
        {
            Texture2D output = new Texture2D((uint)width, (uint)height,format); 
            imgBinding.Add(output,bindingindex);
            return output;
        }

        public Texture texOuput
        {
            get;private set;
        }

        public void SetCustomParameters<T>(string name, T data)
        {
            co.parameters.UpdateUniformData(name,data);
        }

        void UpdateData()
        {
            foreach(var p in imgBinding)
            {
                ImageBindingPoint.BindImage(p.Value,p.Key);
            }

            co.parameters.UpdateUniformData("gAlbedo",gAlbedo);
            co.parameters.UpdateUniformData("gPosition",gPosition);
            co.parameters.UpdateUniformData("gNormal",gNormal);
            co.parameters.UpdateUniformData("gMetallicRoughness",gMetallicRoughness);
            co.parameters.UpdateUniformData("gEmbientAO",gEmbientAO);
            co.parameters.UpdateUniformData("gColor",gColor);

            co.parameters.UpdateUniforms();
        }

        public void Process()
        {
            UpdateData();
            co.Compute();
        }

        public void UpdateToPostFxFBO()
        {
            PostEffect.Pass.FBO.UpdateTextureToFBO(texOuput,FramebufferAttachment.ColorAttachment0);
        }
    }

}
