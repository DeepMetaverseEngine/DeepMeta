using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace DeepEditor.Common.G3D.GLTF
{
    public class FramebufferObject
    {
        public int width;
        public int height;
        public int samples;
        public int id
        {
            get;
            private set;
        }
        
        private static FramebufferObject fboDefault;
        public static FramebufferObject Default
        {
            get
            {
                if(fboDefault==null)
                {
                    fboDefault = new FramebufferObject();
                }
                return fboDefault;
            }
        }

        public static void GetViewport(out int w,out int h)
        {
            int[] vp ={0,0,0,0};
            GL.GetInteger(GetPName.Viewport,vp);
            w = vp[2];
            h = vp[3];
        }
        public static void GetDeafultSize(out int w, out int h)
        {
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer,0);
            GL.GetFramebufferParameter(FramebufferTarget.Framebuffer,FramebufferDefaultParameter.FramebufferDefaultWidth,out w);
            GL.GetFramebufferParameter(FramebufferTarget.Framebuffer,FramebufferDefaultParameter.FramebufferDefaultHeight,out h);
        }

        public FramebufferObject(int w, int h,int s = 1)
        {
            id = GL.GenFramebuffer();
            width = w;
            height = h;
            samples = s;
        }

        private FramebufferObject()
        {
            id = 0;
            // GL.BindFramebuffer(FramebufferTarget.Framebuffer,0);
            int[] viewport = {0,0,0,0};
            GL.GetInteger(GetPName.Viewport,viewport);
            width= viewport[2];
            height = viewport[3];
            //GL.GetInteger(GetPName.SampleBuffers,out samples);
            GL.GetInteger(GetPName.Samples,out samples);
            int sb;
            GL.GetInteger(GetPName.SampleBuffers,out sb);
        }
        
        public Dictionary<FramebufferAttachment,int> rboAttachments = new Dictionary<FramebufferAttachment,int>();
        public Dictionary<FramebufferAttachment,Texture> texAttachments = new Dictionary<FramebufferAttachment,Texture>();

        public int InitRBO(RenderbufferStorage storage, FramebufferAttachment attachTo = FramebufferAttachment.ColorAttachment0)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer,id);
            GL.Enable(EnableCap.Multisample);
            int rbo = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer,rbo);
            //GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,storage,width,height);
            if(samples>0)
            {
                GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer,samples,storage,width,height);
            }
            else
            {
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,storage,width,height);
            }
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,attachTo,
            RenderbufferTarget.Renderbuffer,rbo);
            CheckFramebufferStatus();
            int sb;
            GL.GetInteger(GetPName.SampleBuffers,out sb);
            rboAttachments.Add(attachTo,id);
            return rbo;
        }

        public int InitTextureTarget2D(SizedInternalFormat format,FramebufferAttachment attachTo = FramebufferAttachment.ColorAttachment0)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer,id);
            GL.Enable(EnableCap.Multisample);
            Texture tex;
            if(samples > 0)
            {
                //texture is bind Texture2DMultisample. and TexStorage2DMultisample fixedsamplelocation = true;
                //if fbo is mixed by texture and RBO. fixedsamplelocation must be all true;
                tex = new Texture2DMS((uint)width,(uint)height, samples, format);
            }
            else
            {
                tex = new Texture2D((uint)width, (uint)height, format);
            }

            GL.FramebufferTexture(FramebufferTarget.Framebuffer,attachTo,tex.ID, 0);
            //GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,attachTo,TextureTarget.Texture2DMultisample,tex,0);
            CheckFramebufferStatus();
            texAttachments.Add(attachTo,tex);
            return tex.ID;
        }

        public void UpdateTextureToFBO(Texture texture, FramebufferAttachment attachTo = FramebufferAttachment.ColorAttachment0)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer,id);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer,attachTo,texture.ID, 0);
            CheckFramebufferStatus();
            if(texAttachments.ContainsKey(attachTo))
                texAttachments[attachTo] = texture;
            else
                texAttachments.Add(attachTo,texture);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer,0);
        }

        public FramebufferErrorCode CheckFramebufferStatus()
        {
            var errorCode = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if(errorCode != FramebufferErrorCode.FramebufferComplete)
            Console.WriteLine("FBO[{0}]:Frame buffer status incorrect:{1}",id,errorCode);
            return errorCode;
        }
        
        public void CopyToTarget(FramebufferObject fboTarget)
        {
            //R-SampleBuffer>0 D-SampleBUffer=0. OK(convert). window samples = 0, good quality
            //R-SampleBuffer=0 D-SampleBUffer>0. OK(replicated) no recommended
            //R-SampleBuffer>0 D-SampleBUffer>0. differrent formats or rectangle. Error
            //R-SampleBuffer>0 D-SampleBUffer>0. idnetical. correct. but hard.

            int minWidth = Math.Min(fboTarget.width,width);
            int minHeight = Math.Min(fboTarget.height,height);
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, id);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, fboTarget.id);
            GL.BlitFramebuffer(0, 0, minWidth, minHeight, 0, 0, minWidth, minHeight, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear); // 如果用到其他的bufferbit 过滤就不能用linear
            //GL.BlitFramebuffer(0, 0, width, height, 0, 0, fboTarget.width, fboTarget.height, ClearBufferMask.DepthBufferBit, BlitFramebufferFilter.Nearest);
            //GL.BlitFramebuffer(0, 0, width, height, 0, 0, fboTarget.width, fboTarget.height, ClearBufferMask.StencilBufferBit, BlitFramebufferFilter.Nearest);
        }

    }

    public delegate void OnPassRender(RenderPass rp);

    public class RenderPass
    {
        public static RenderPass shadowPass;
        public static RenderPass deferredPrePass;
        public static RenderPass defferedPass;
        public static RenderPass deferLightPass;
        public static RenderPass postEffectPass;
        
        static RenderPass defaultPass;
        public static RenderPass DefaultDraw
        {
            get
            {
                if(defaultPass==null)
                {
                    FramebufferObject.Default.CheckFramebufferStatus();
                    defaultPass = new RenderPass(FramebufferObject.Default);
                    defaultPass.clearBufferMask = ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit;
                    defaultPass.clearColor = new Color4(0,0,0,1);
                    defaultPass.clearDepth = 1.0f;
                }
                return defaultPass;
            }
        }

        public List<Drawable> drawables = new List<Drawable>();
        public GroupMesh groupMesh;
        public FramebufferObject FBO
        {
            get;
            private set;
        }

        public RenderPass(FramebufferObject f, List<Drawable> d = null)
        {
            FBO = f;
            if(d!=null)
                drawables.AddRange(d);
        }

        public Color4 clearColor;
        public float clearDepth;
        public ClearBufferMask clearBufferMask;
        
        public OnPassRender PassRenderFunc;
        public void Render()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer,FBO.id);
            GL.Viewport(0,0,FBO.width,FBO.height);
            PassRenderFunc(this);
            GL.Flush();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer,0);
        }

        public void Display()
        {
            FBO.CopyToTarget(FramebufferObject.Default);
        }
    }
    
}