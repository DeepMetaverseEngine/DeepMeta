using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace DeepEditor.Common.G3D.GLTF
{
    public abstract class Material
    {
        public PipelineState state;
        // public Uniforms uniforms;
        public Dictionary<ProgramStageMask,Uniforms> programUniform = new Dictionary<ProgramStageMask, Uniforms>();
        public bool IsMultiStage()
        {
            return !programUniform.ContainsKey(ProgramStageMask.AllShaderBits);
        }

        public string name;
        public Material()
        {
            state = new PipelineState();
            state.DepthTest = true;
            state.blendFactorSrc = BlendingFactor.SrcAlpha;
            state.blendFactorDst = BlendingFactor.OneMinusSrcAlpha;
        }

        public static Vector4 ToVector(float[] f)
        {
            Vector4 vector = Vector4.Zero;
            vector.X = f[0];
            vector.Y = f[1];
            vector.Z = f[2];
            if(f.Length>3)vector.W = f[3];
            return vector;
        }
        
        public void SetupPipelineState()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(state.blendFactorSrc,state.blendFactorDst);
            if(state.DepthTest)
                GL.Enable(EnableCap.DepthTest);
            else
                GL.Disable(EnableCap.DepthTest);

            GL.PolygonMode(MaterialFace.FrontAndBack,OpenTK.Graphics.OpenGL4.PolygonMode.Fill);
        }

        protected void Regiseter(GPUProgram p)
        {
            var mask = p.GetStageMask();
            if(programUniform.ContainsKey(mask))
            {
                Console.WriteLine("registered GPUProgram already");
                var u = programUniform[mask];
                u.ShaderProgram = p;
            }
            else
            {
                programUniform.Add(mask,new Uniforms(p));
            }

        }

        public abstract void UpdateUniforms();

        public void ReplaceProgram(GPUProgram p)
        {
            var mask = p.GetStageMask();
            if(programUniform.ContainsKey(mask))
            {
                var u = programUniform[mask];
                if(u.ShaderProgram!=p)
                {
                    u.ShaderProgram = p;
                    u.ReloadLocations();
                }
            }
        }

        public void PrintValidUniforms()
        {
            foreach(var p in programUniform)
            foreach(var e in p.Value.kv)
            {
                if(e.Value>=0)
                {
                    Console.WriteLine(e.Key);
                }
            }
        }
        public Uniforms FindUniform(ProgramStageMask mask)
        {
            if(!programUniform.ContainsKey(mask))
            {
                if(!programUniform.ContainsKey(ProgramStageMask.AllShaderBits))
                    return null;
                return programUniform[ProgramStageMask.AllShaderBits];
            }
            return programUniform[mask];
        }
    }

    public static class MaterialFactory
    {
        static Dictionary<Type,IStageMask> kv = new Dictionary<Type, IStageMask>();
        public static void RegPBRMaterial(IStageMask program)
        {
            kv.Add(typeof(PBRMaterial),program);
        }

        public static void RegPBR_IBLMaterial(IStageMask program)
        {
            kv.Add(typeof(PBR_IBLMaterial),program);
        }
        public static void RegSkyBoxMaterial(IStageMask program)
        {
            kv.Add(typeof(SkyBoxMaterial),program);
        }

        public static PBRMaterial CreatePBRMaterial()
        {
            var pbrmtl = new PBRMaterial();
            if(!kv.ContainsKey(typeof(PBRMaterial)))return null;
            IStageMask sm = kv[typeof(PBRMaterial)];
            if(sm is ProgramGroup)
            {
                ProgramGroup gp = sm as ProgramGroup;
                pbrmtl.RegisterTransform(gp.GetStageProgram(ProgramStageMask.VertexShaderBit));
                pbrmtl.RegisterPBR(gp.GetStageProgram(ProgramStageMask.FragmentShaderBit));

            }
            else if(sm is GPUProgram)
            {
                GPUProgram p = sm as GPUProgram;
                pbrmtl.RegisterTransform(p);
                pbrmtl.RegisterPBR(p);
            }
            return pbrmtl;
        }

        public static PBR_IBLMaterial CreatePBR_IBLMaterial()
        {
            var iblmtl = new PBR_IBLMaterial();
            if(!kv.ContainsKey(typeof(PBR_IBLMaterial)))return null;
            IStageMask sm = kv[typeof(PBR_IBLMaterial)];
            if(sm is ProgramGroup)
            {
                ProgramGroup gp = sm as ProgramGroup;
                iblmtl.RegisterTransform(gp.GetStageProgram(ProgramStageMask.VertexShaderBit));
                
                iblmtl.RegisterPBR(gp.GetStageProgram(ProgramStageMask.FragmentShaderBit));
                iblmtl.RegisterIBL(gp.GetStageProgram(ProgramStageMask.FragmentShaderBit));

            }
            else if(sm is GPUProgram)
            {
                GPUProgram p = sm as GPUProgram;
                iblmtl.RegisterTransform(p);
                iblmtl.RegisterPBR(p);
                iblmtl.RegisterIBL(p);
            }
            return iblmtl;
        }

        public static SkyBoxMaterial CreateSkyboxMaterial()
        {
            var skyboxmtl = new SkyBoxMaterial();
            if(!kv.ContainsKey(typeof(SkyBoxMaterial)))return null;
            IStageMask sm = kv[typeof(SkyBoxMaterial)];
            if(sm is ProgramGroup)
            {
                ProgramGroup gp = sm as ProgramGroup;
                skyboxmtl.RegisterTransform(gp.GetStageProgram(ProgramStageMask.VertexShaderBit));
                skyboxmtl.RegisterCubeMap(gp.GetStageProgram(ProgramStageMask.FragmentShaderBit));
            }
            else if(sm is GPUProgram)
            {
                GPUProgram p = sm as GPUProgram;
                skyboxmtl.RegisterTransform(p);
                skyboxmtl.RegisterCubeMap(p);
            }
            return skyboxmtl;
        }
    }

    public class MeshMaterial: Material
    {
        public Matrix4 matModel;
        public Matrix4 matView;
        public Matrix4 matProjection;
        public Matrix4 matMVP;

        public MeshMaterial():base()
        {
        }

        public void RegisterTransform(GPUProgram p)
        {
            var mask = p.GetStageMask();
            if(p.HasStageMask(ProgramStageMask.VertexShaderBit))
                Regiseter(p);
            programUniform[mask].RegName("Model");
            programUniform[mask].RegName("View");
            programUniform[mask].RegName("Projection");
            programUniform[mask].RegName("MVP");
        }
        public void UpdateTransform()
        {
            Uniforms u = FindUniform(ProgramStageMask.VertexShaderBit);
            if(u==null)
            {
                Console.WriteLine("{0} require VertexShaderBit",this.GetType());
                return;
            }
            u.UpdateUniform("MVP", matMVP);
            u.UpdateUniform("Model", matModel);
            u.UpdateUniform("View", matView);
            u.UpdateUniform("Projection", matProjection);
        }

        public override void UpdateUniforms()
        {
            UpdateTransform();
        }
    }

    public class PBRMaterial : MeshMaterial
    {
        protected List<MaterialGLTFExtension> extensions = new List<MaterialGLTFExtension>();
        
        //31-24,extension
        //23-0,bit set. there some param is conflict. like BaseColorTexture and BaseColor
        T GetExtensionByType<T>() where T: class
        {
            foreach(var e in extensions)
            {
                if(e is T)
                return e as T;
            }    
            return null;
        }

        public int TextureMask()
        {
            int result = 0;
            //31-24 extensions,23-16 ext texture,15-8 gltf default texture, 7-0 common texture
            var SpecularGlossiness = GetExtensionByType<PBRSpecularGlossinessMaterial>();
            if(SpecularGlossiness!=null)
            {
                result |= 0x01000000;//extension id = 1<<24
                if(SpecularGlossiness.diffuseTexture!=null)result |= 0x10000;
                if(SpecularGlossiness.specularGlossinessTexture!=null)result |= 0x20000;
            }
            else
            {
                if(BaseColorTexture!=null)result |= 0x100;
                if(MetallicRoughnessTexture!=null)result |= 0x200;
            }
            if(NormalTexture!=null)result |= 0x1;
            if(EmissiveTexture!=null)result |= 0x2;
            if(OcclusionTexture!=null)result |= 0x4;
            return result;
        }

        public PBRMaterial():base()
        {
        }

        public void RegisterPBR(GPUProgram p)
        {
            var mask = p.GetStageMask();
            if(p.HasStageMask(ProgramStageMask.FragmentShaderBit))
                Regiseter(p);
            var u = programUniform[mask];

            u.RegName("NormalTexture");
            u.RegName("BaseColorTexture");
            u.RegName("MetallicRoughnessTexture");
            u.RegName("OcclusionTexture");
            u.RegName("EmissiveTexture");
            u.RegName("BaseColor");////default:[1,1,1,1]
            u.RegName("RoughnessFactor");//default:1
            u.RegName("MetallicFactor");//default:1
            u.RegName("OcclusionStrength");//default:1
            u.RegName("EmissiveColor");//default:[0,0,0]
            u.RegName("AlphaCutoff");//default:0.5
            u.RegName("DoubleSided");//default:false
            u.RegName("AlphaMode");//default: Opaque

            u.RegName("CameraPosition");
            u.RegName("Light");
            u.RegName("LightColor");
        }

        public void InitExtension(MaterialGLTFExtension mtlExt)
        {
            Uniforms u = FindUniform(ProgramStageMask.FragmentShaderBit);
            if(u==null) return;

            mtlExt.InitUniform(u);
            extensions.Add(mtlExt);
        }

        public int AlphaMode;//output. Opaque = 0, Mask = 1, Blend = 2 
        public bool DoubleSided;//lighting. 
        public Texture2D NormalTexture;
        public Texture2D BaseColorTexture;
        public Vector4 BaseColor = new Vector4(1,1,1,1);
        public float RoughnessFactor = 1;
        public float MetallicFactor = 1;
        public Texture2D MetallicRoughnessTexture;
        public Texture2D OcclusionTexture;
        public float OcclusionStrength = 1; 
        public Texture2D EmissiveTexture;
        public Vector4 EmissiveColor = Vector4.Zero;
        public float AlphaCutoff = 0.5f;

        //
        public Vector3 CameraPosition;
        public Vector4 Light;
        public Vector3 LightColor;

        public override void UpdateUniforms()
        {
            //base.UpdateUniforms();//useprogram
            UpdateTransform();
            
            Uniforms u = FindUniform(ProgramStageMask.FragmentShaderBit);
            if(u==null)
            {
                Console.WriteLine("{0} require FragmentShaderBit",this.GetType());
                return;
            }
            u.UpdateUniformTexture("BaseColorTexture",BaseColorTexture);
            u.UpdateUniformTexture("NormalTexture",NormalTexture);
            u.UpdateUniformTexture("MetallicRoughnessTexture",MetallicRoughnessTexture);
            u.UpdateUniformTexture("OcclusionTexture",OcclusionTexture);
            u.UpdateUniformTexture("EmissiveTexture",EmissiveTexture);
            u.UpdateUniform("BaseColor",BaseColor);
            u.UpdateUniform("RoughnessFactor",RoughnessFactor);
            u.UpdateUniform("MetallicFactor",MetallicFactor);
            u.UpdateUniform("OcclusionStrength",OcclusionStrength);
            u.UpdateUniform("EmissiveColor",EmissiveColor);
            u.UpdateUniform("AlphaCutoff",AlphaCutoff);
            
            u.UpdateUniform("Light",Light);
            u.UpdateUniform("LightColor",LightColor);
            u.UpdateUniform("CameraPosition",CameraPosition);
            foreach(var e in extensions)
            {
                e.UpdateUniformsExt(u);
            }
        }
        
        //for deferred pre pass
        public void UpdateTextureMask(GPUProgram program)
        {
            int uTextureMask = GL.GetUniformLocation(program.ID,"TextureMask");
            GL.ProgramUniform1(program.ID,uTextureMask,TextureMask());
        }

        public void CopyTo(PBRMaterial target)
        {
            //父类成员直接拷贝，反序列化或反射都可以
            //还就就是一开始就直接用子类
            target.DoubleSided = DoubleSided;
            target.AlphaMode = AlphaMode;
            target.AlphaCutoff = AlphaCutoff;
            target.BaseColor = BaseColor;
            target.BaseColorTexture = BaseColorTexture;
            target.CameraPosition = CameraPosition;
            target.EmissiveColor = EmissiveColor;
            target.EmissiveTexture = EmissiveTexture;
            target.Light = Light;
            target.LightColor = LightColor;
            target.matModel = matModel;
            target.matMVP = matMVP;
            target.NormalTexture = NormalTexture;
            target.MetallicFactor = MetallicFactor;
            target.MetallicRoughnessTexture = MetallicRoughnessTexture;
            target.OcclusionStrength = OcclusionStrength;
            target.OcclusionTexture = OcclusionTexture;
            target.RoughnessFactor = RoughnessFactor;

            // target.extensions = extensions;
            if(extensions.Count>0)
            {
                foreach(var e in extensions)
                {
                    target.InitExtension(e);
                }
            }
        }
    }

    public class PBR_IBLMaterial:PBRMaterial
    {
        public PBR_IBLMaterial():base()
        {
            
        }

        public void RegisterIBL(GPUProgram p)
        {
            var mask = p.GetStageMask();
            if(p.HasStageMask(ProgramStageMask.FragmentShaderBit))
                Regiseter(p);
            var u = programUniform[mask];
            u.RegName("IrrMap");
            u.RegName("PrefilterMap");
            u.RegName("brdfLUT");
        }

        public TextureCube IrrMap;
        public TextureCube PrefilterMap;
        public Texture2D brdfLUT;
        public override void UpdateUniforms()
        {
            base.UpdateUniforms();
            Uniforms u = FindUniform(ProgramStageMask.FragmentShaderBit);
            if(u==null)
            {
                Console.WriteLine("{0} require FragmentShaderBit",this.GetType());
                return;
            }

            u.UpdateUniformTexture("IrrMap",IrrMap);
            u.UpdateUniformTexture("PrefilterMap",PrefilterMap);
            u.UpdateUniformTexture("brdfLUT",brdfLUT);
        }

    }

    public class SkyBoxMaterial:MeshMaterial
    {
        public SkyBoxMaterial():base()
        {
        }

        public void RegisterCubeMap(GPUProgram p)
        {
            var mask = p.GetStageMask();
            if(p.HasStageMask(ProgramStageMask.FragmentShaderBit))
                Regiseter(p);
            
            //RegisterTransform(p);
            var u = programUniform[mask];
            u.RegName("CubeMap");
        }

        public TextureCube CubeMap;
        public override void UpdateUniforms()
        {
            //base.UpdateUniforms();
            UpdateTransform();
            Uniforms u = FindUniform(ProgramStageMask.FragmentShaderBit);
            if(u==null)
            {
                Console.WriteLine("{0} require FragmentShaderBit",this.GetType());
                return;
            }
            u.UpdateUniformTexture("CubeMap",CubeMap);
        }
        
    }
}