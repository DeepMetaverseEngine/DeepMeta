
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DeepEditor.Common.G3D.GLTF
{
    public class PipelineState
    {
        public CullFaceMode cullFace = CullFaceMode.Back;
        public FrontFaceDirection FrontFaceDirection = FrontFaceDirection.Ccw;
        public bool Blending;
        public bool DepthTest;
        public PolygonMode PolygonModeFront = PolygonMode.Fill;
        public PolygonMode PolygonModeBack = PolygonMode.Line;
        public bool WriteDepth;
        public DepthFunction depthFunc = DepthFunction.Less;
        public BlendEquationMode blendEquation = BlendEquationMode.FuncAdd;
        public BlendingFactor blendFactorSrc = BlendingFactor.SrcAlpha;
        public BlendingFactor blendFactorDst = BlendingFactor.OneMinusSrcAlpha;
        public void SetupStateAll()
        {
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.CullFace(cullFace);
            //Depth
            if(DepthTest)
            {
               GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(depthFunc);
            }
            else
                GL.Disable(EnableCap.DepthTest);

            if(WriteDepth)
                GL.DepthMask(true);
            else
                GL.DepthMask(false);

            //Polygon mode
            GL.PolygonMode(MaterialFace.Front,PolygonModeFront);
            GL.PolygonMode(MaterialFace.Back,PolygonModeBack);

            //blending
            GL.Enable(EnableCap.Blend);
            if(Blending)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendEquation(blendEquation);
                GL.BlendFunc(blendFactorSrc,blendFactorDst);
            }
            else
                GL.Disable(EnableCap.Blend);
        }

    }

    public interface IPipelineActive
    {
        void Active();
    }

    public interface IStageMask
    {
        ProgramStageMask GetStageMask();
        bool HasStageMask(ProgramStageMask mask);
    }

    public class ShaderSourceManager
    {
        static ShaderSourceManager shaderSourceManager;
        public static ShaderSourceManager Singleton
        {
            get
            {
                if(shaderSourceManager==null)
                shaderSourceManager = new ShaderSourceManager();
                return shaderSourceManager;
            }
        }
        
        public static ShadersProgram LoadShaderFile(string vs,string fs)
        {
            string vsFullName = FullName(vs),fsFullName = FullName(fs);
            ShaderSource vss = ShaderSource.LoadFile(ShaderType.VertexShader, vsFullName);
            ShaderSource fss = ShaderSource.LoadFile(ShaderType.FragmentShader, fsFullName);
            ShadersProgram sp = ShadersProgram.LoadShader(vss,fss);
            Singleton.sources.Add(fsFullName,sp);
            Singleton.sources.Add(vsFullName,sp);
            return sp;
        }

        public static ShadersProgram LoadShaderFile(string vs,string fs,string gs)
        {
            string vsFullName = FullName(vs),fsFullName = FullName(fs),gsFullName = FullName(gs);
            ShaderSource vss = ShaderSource.LoadFile(ShaderType.VertexShader, vsFullName);
            ShaderSource fss = ShaderSource.LoadFile(ShaderType.FragmentShader, fsFullName);
            ShaderSource gss = ShaderSource.LoadFile(ShaderType.GeometryShader, gsFullName);
            ShadersProgram sp = ShadersProgram.LoadShader(vss,gss,fss);
            Singleton.sources.Add(fsFullName,sp);
            Singleton.sources.Add(vsFullName,sp);
            Singleton.sources.Add(gsFullName,sp);
            return sp;
        }
        
        public static ShadersProgram LoadCSShaderFile(string cs)
        {
            string csFullName = FullName(cs);
            ShaderSource css = ShaderSource.LoadFile(ShaderType.ComputeShader, csFullName);
            ShadersProgram sp = ShadersProgram.LoadShaderCS(css);
            Singleton.sources.Add(csFullName,sp);
            return sp;
        }
        
        public static ShadersProgram LoadTessShaderFile(string vs,string tcs, string tes ,string fs)
        {
            string vsFullName = FullName(vs),fsFullName = FullName(fs),tcsFullName = FullName(tcs),tesFullName = FullName(tes);
            ShaderSource vss = ShaderSource.LoadFile(ShaderType.VertexShader, vsFullName);
            ShaderSource tcss = ShaderSource.LoadFile(ShaderType.TessControlShader, tcsFullName);
            ShaderSource tess = ShaderSource.LoadFile(ShaderType.TessEvaluationShader, tesFullName);
            ShaderSource fss = ShaderSource.LoadFile(ShaderType.FragmentShader, fsFullName);
            ShadersProgram sp = ShadersProgram.LoadShaderTess(vss,tcss,tess,fss);
            Singleton.sources.Add(vsFullName,sp);
            Singleton.sources.Add(tcsFullName,sp);
            Singleton.sources.Add(tesFullName,sp);
            Singleton.sources.Add(fsFullName,sp);
            return sp;
        }
        public static ProgramGroup GroupLoadShader(string vs,string fs)
        {
            ProgramGroup pg = new ProgramGroup();
            string vsFullName = FullName(vs),fsFullName = FullName(fs);
            ShaderSource vss = ShaderSource.LoadFile(ShaderType.VertexShader, vsFullName);
            ShaderSource fss = ShaderSource.LoadFile(ShaderType.FragmentShader, fsFullName);
            SeparableShaderProgram vssp = new SeparableShaderProgram(vss);
            SeparableShaderProgram fssp = new SeparableShaderProgram(fss);
            pg.SetProgram(vssp);
            pg.SetProgram(fssp);
            Singleton.sources.Add(vsFullName,vssp);
            Singleton.sources.Add(fsFullName,fssp);
            return pg;
        }

        public static ProgramGroup GroupLoadShader(string vs,string gs,string fs)
        {
            ProgramGroup pg = new ProgramGroup();
            string vsFullName = FullName(vs), gsFullName = FullName(gs),
            fsFullName = FullName(fs);
            ShaderSource vss = ShaderSource.LoadFile(ShaderType.VertexShader, vsFullName);
            ShaderSource fss = ShaderSource.LoadFile(ShaderType.FragmentShader, fsFullName);
            ShaderSource gss = ShaderSource.LoadFile(ShaderType.GeometryShader,gsFullName);
            SeparableShaderProgram vssp = new SeparableShaderProgram(vss);
            SeparableShaderProgram fssp = new SeparableShaderProgram(fss);
            SeparableShaderProgram gssp = new SeparableShaderProgram(gss);
            pg.SetProgram(vssp);
            pg.SetProgram(fssp);
            pg.SetProgram(gssp);
            Singleton.sources.Add(vsFullName,vssp);
            Singleton.sources.Add(gsFullName,gssp);
            Singleton.sources.Add(fsFullName,fssp);
            return pg;
        }

        public static ProgramGroup GroupLoadShaderTess(string vs,string tcs, string tes,string fs)
        {
            ProgramGroup pg = new ProgramGroup();
            string vsFullName = FullName(vs),fsFullName = FullName(fs),
            tcsFullName = FullName(tcs),tesFullName = FullName(tes);
            ShaderSource vss = ShaderSource.LoadFile(ShaderType.VertexShader, vsFullName);
            ShaderSource fss = ShaderSource.LoadFile(ShaderType.FragmentShader, fsFullName);
            ShaderSource tcss = ShaderSource.LoadFile(ShaderType.TessControlShader,tcsFullName);
            ShaderSource tess = ShaderSource.LoadFile(ShaderType.TessEvaluationShader,tesFullName);
            SeparableShaderProgram vssp = new SeparableShaderProgram(vss);
            SeparableShaderProgram fssp = new SeparableShaderProgram(fss);
            SeparableShaderProgram tcssp = new SeparableShaderProgram(tcss);
            SeparableShaderProgram tessp = new SeparableShaderProgram(tess);
            pg.SetProgram(vssp);
            pg.SetProgram(fssp);
            pg.SetProgram(tcssp);
            pg.SetProgram(tessp);
            Singleton.sources.Add(vsFullName,vssp);
            Singleton.sources.Add(tcsFullName,tcssp);
            Singleton.sources.Add(tesFullName,tessp);
            Singleton.sources.Add(fsFullName,fssp);
            return pg;
        }



        public static ProgramPipeline PipelineLoadShader(string vs,string fs)
        {
            ProgramPipeline pipeline = new ProgramPipeline();
            string vsFullName = FullName(vs),fsFullName = FullName(fs);
            ShaderSource vss = ShaderSource.LoadFile(ShaderType.VertexShader, vsFullName);
            ShaderSource fss = ShaderSource.LoadFile(ShaderType.FragmentShader, fsFullName);
            SeparableShaderProgram vssp = new SeparableShaderProgram(vss);
            SeparableShaderProgram fssp = new SeparableShaderProgram(fss);
            pipeline.BindProgramStage(vssp);
            pipeline.BindProgramStage(fssp);
            Singleton.sources.Add(vsFullName,vssp);
            Singleton.sources.Add(fsFullName,fssp);
            return pipeline;
        }

        public static ProgramPipeline PipelineLoadShader(string vs,string gs,string fs)
        {
            ProgramPipeline pipeline = new ProgramPipeline();
            string vsFullName = FullName(vs), gsFullName = FullName(gs),
            fsFullName = FullName(fs);
            ShaderSource vss = ShaderSource.LoadFile(ShaderType.VertexShader, vsFullName);
            ShaderSource fss = ShaderSource.LoadFile(ShaderType.FragmentShader, fsFullName);
            ShaderSource gss = ShaderSource.LoadFile(ShaderType.GeometryShader,gsFullName);
            SeparableShaderProgram vssp = new SeparableShaderProgram(vss);
            SeparableShaderProgram fssp = new SeparableShaderProgram(fss);
            SeparableShaderProgram gssp = new SeparableShaderProgram(gss);
            pipeline.BindProgramStage(vssp);
            pipeline.BindProgramStage(gssp);
            pipeline.BindProgramStage(fssp);
            Singleton.sources.Add(vsFullName,vssp);
            Singleton.sources.Add(gsFullName,gssp);
            Singleton.sources.Add(fsFullName,fssp);
            return pipeline;
        }

        public static ProgramPipeline PipelineLoadShaderTess(string vs,string tcs, string tes,string fs)
        {
            ProgramPipeline pipeline = new ProgramPipeline();
            string vsFullName = FullName(vs),fsFullName = FullName(fs),
            tcsFullName = FullName(tcs),tesFullName = FullName(tes);
            ShaderSource vss = ShaderSource.LoadFile(ShaderType.VertexShader, vsFullName);
            ShaderSource fss = ShaderSource.LoadFile(ShaderType.FragmentShader, fsFullName);
            ShaderSource tcss = ShaderSource.LoadFile(ShaderType.TessControlShader,tcsFullName);
            ShaderSource tess = ShaderSource.LoadFile(ShaderType.TessEvaluationShader,tesFullName);
            SeparableShaderProgram vssp = new SeparableShaderProgram(vss);
            SeparableShaderProgram fssp = new SeparableShaderProgram(fss);
            SeparableShaderProgram tcssp = new SeparableShaderProgram(tcss);
            SeparableShaderProgram tessp = new SeparableShaderProgram(tess);
            pipeline.BindProgramStage(vssp);
            pipeline.BindProgramStage(tcssp);
            pipeline.BindProgramStage(tessp);
            pipeline.BindProgramStage(fssp);
            Singleton.sources.Add(vsFullName,vssp);
            Singleton.sources.Add(tcsFullName,tcssp);
            Singleton.sources.Add(tesFullName,tessp);
            Singleton.sources.Add(fsFullName,fssp);
            return pipeline;
        }

        public Dictionary<string, GPUProgram> sources = new Dictionary<string, GPUProgram>();
        public List<IPipelineActive>  pipelines = new List<IPipelineActive>();
        public static FilesWatcher ShaderWatcher
        {
            get;set;
        }
        
        static string FullName(string file)
        {
            if(ShaderWatcher!=null)
            {
                string fullname;
                ShaderWatcher.RegisterFile(file,out fullname);
                return fullname;
                //return fullname.ToLower();
            }
            else
            {
                FileInfo fi = new FileInfo(file);
                return fi.FullName;
                // return fi.FullName.ToLower();
            }
        }

        public ShaderSourceManager()
        {
        }
    }

    public class ShaderSource : IDisposable
    {
        public int id;
        public ShaderType type;
        public string source;
        public string filepath;
        public ShaderSource(ShaderType t, string s,string filename = null)
        {
            type = t;
            source = s;
            if(filename!=null)
            {
                filepath = filename;
            }
            id = GL.CreateShader(type);
            GL.ShaderSource(id,source);
            GL.CompileShader(id);
            Console.WriteLine("shader id:{0}\n{1}",id,GL.GetShaderInfoLog(id));
        }

        public void Dispose()
        {
            GL.DeleteShader(id);
        }

        public void Reload()
        {
            source = File.ReadAllText(filepath);
            if(source=="")
            {
                Console.WriteLine("reload shader error");
                return;
            }
            GL.ShaderSource(id,source);
            GL.CompileShader(id);
            Console.WriteLine("shader id:{0}\n{1}",id,GL.GetShaderInfoLog(id));
        }

        public static ShaderSource LoadFile(ShaderType type, string filename)
        {
            string source = File.ReadAllText(filename);
            if(source=="")
            {
                Console.WriteLine("load shader error");
                return null;
            }
            return new ShaderSource(type,source,filename);
        }
        
    }

    public abstract class GPUProgram : IDOjbect, IDisposable, IStageMask
    {
        protected ProgramStageMask StageMask = ProgramStageMask.AllShaderBits;

        public ProgramStageMask GetStageMask()
        {
            return StageMask;
        }

        public static Dictionary<int,GPUProgram> programs = new Dictionary<int, GPUProgram>();
        protected GPUProgram()
        {
            ID = GL.CreateProgram();
            programs.Add(ID,this);
        }

        public HashSet<WeakReference<Uniforms>> Notifications = new HashSet<WeakReference<Uniforms>>();
        public void RegisterNoitfication(WeakReference<Uniforms> u)
        {
            Notifications.Add(u);
            if(Notifications.Count>1024)
            {
                GC.Collect();
                ClearInvalidNotifications();
            }
        }

        public void UnregisterNoitfication(WeakReference<Uniforms> u)
        {
            Notifications.Remove(u);
        }

        public void ClearInvalidNotifications()
        {
            Notifications.RemoveWhere(new Predicate<WeakReference<Uniforms>>
            ((WeakReference<Uniforms> wr)=>{
                Uniforms u;
                return !wr.TryGetTarget(out u);
            }));
        }

        public virtual void Dispose()
        {
            GL.DeleteProgram(ID);
        }
        
        public bool HasStageMask(ProgramStageMask mask)
        {
            return ((int)StageMask&(int)mask) > 0;
        }

        public abstract void ReloadProgram();
        public void RelocationUniforms()
        {
            foreach(var e in Notifications)
            {
                Uniforms u;
                bool b = e.TryGetTarget(out u);
                if(b)u.ReloadLocations();
            }
        }
    }

    public class ShadersProgram : GPUProgram, IPipelineActive
    {
        static ShadersProgram zero;
        public static ShadersProgram Default
        {
            get
            {
                if(zero==null)
                {
                    zero = new ShadersProgram();
                }
                return zero;
            }
        }
        //对于漫反射，需要一张辐照度贴图。
        //通过Enviroment的CubeMap，产生一张Irradiance的CubeMap。
        //意思是采样一个方向，代替了辐照度Irradiance在半球的积分。(Output Cubemap)
        public static ShadersProgram GenIrradianceMap;
        //对于镜面反射，需要一张预过滤环境贴图
        //通过Enviroment的CubeMap，和粗糙度，产生一张不同粗糙度下，镜面反射的积分。(Output Cubemap with Mipmap)
        //通过采样一个反射方向，得到在镜面反射在一个粗糙度下，即不同的镜面波瓣（specular lobe)的积分。
        public static ShadersProgram GenPrefilterEnvMap;
        //
        public static ShadersProgram GenBRDF_LUT;
        public static ShadersProgram ProcessEquirectangularMap;

        public ShaderSource vs;
        public ShaderSource gs;
        public ShaderSource fs;
        public ShaderSource cs;
        public ShaderSource tcs;
        public ShaderSource tes;

        private ShadersProgram()
        {}

        private ShadersProgram(ShaderSource v,ShaderSource g,ShaderSource f,ShaderSource c,ShaderSource tc = null, ShaderSource te = null):base()
        {
            vs = v;
            gs = g;
            fs = f;
            cs = c;
            tcs = tc;
            tes = te;
            
            Attach(vs);
            Attach(fs);
            Attach(tcs);
            Attach(tes);
            Attach(gs);
            Attach(cs);
            GL.LinkProgram(ID);
            Console.WriteLine(GL.GetProgramInfoLog(ID));
        }

        private void Reload(ShaderSource ss)
        {
            if(ss!=null)
                ss.Reload();
        }

        private void Detach(ShaderSource ss)
        {
            if(ss!=null)
                GL.DetachShader(ID,ss.id);
        }
        private void Attach(ShaderSource ss)
        {
            if(ss!=null)
                GL.AttachShader(ID,ss.id);
        }

        public override void Dispose()
        {
            GL.UseProgram(0);
            Detach(vs);
            Detach(fs);
            Detach(tcs);
            Detach(tes);
            Detach(gs);
            Detach(cs);
            GL.DeleteProgram(ID);
        }

        static string ShaderSource(string filename)
        {
            string data = " ";
            try
            {
                data = File.ReadAllText(filename,Encoding.UTF8);
            }
            catch(Exception ex)
            {
                Console.WriteLine("error Shader source:", ex.ToString());
            }
            return data;
        }

        public override void ReloadProgram()
        {
            Detach(vs);
            Detach(fs);
            Detach(tcs);
            Detach(tes);
            Detach(gs);
            Detach(cs);
            GL.UseProgram(0);

            Reload(vs);
            Reload(fs);
            Reload(tcs);
            Reload(tes);
            Reload(gs);
            Reload(cs);

            Attach(vs);
            Attach(fs);
            Attach(tcs);
            Attach(tes);
            Attach(gs);
            Attach(cs);

            GL.LinkProgram(ID);
            Console.WriteLine(GL.GetProgramInfoLog(ID));
            GL.UseProgram(ID);

            foreach(var e in Notifications)
            {
                Uniforms u;
                bool b = e.TryGetTarget(out u);
                if(b)u.ReloadLocations();
            }
        }

        public static ShadersProgram LoadShader(ShaderSource vss,ShaderSource fss)
        {
            return new ShadersProgram(vss,null,fss,null);
        }
        
        public static ShadersProgram LoadShader(ShaderSource vss, ShaderSource gss, ShaderSource fss)
        {
            return new ShadersProgram(vss,null,fss,null);
        }

        public static ShadersProgram LoadShaderCS(ShaderSource css)
        {
            return new ShadersProgram(null,null,null,css);
        }

        public static ShadersProgram LoadShaderTess(ShaderSource vss, ShaderSource tcss, ShaderSource tess, ShaderSource fss)
        {
            return new ShadersProgram(vss,null,fss,null,tcss,tess);
        }

        public void Active()
        {
            GL.UseProgram(ID);
        }
    }

    public class SeparableShaderProgram : GPUProgram
    {
        ShaderSource shaderSource;
        public SeparableShaderProgram(ShaderSource ss):base()
        {
            shaderSource = ss;

            // string[] code = {ss.source};
            // Program = GL.CreateShaderProgram(ss.type,1,code);
            GL.ProgramParameter(ID,ProgramParameterName.ProgramSeparable,1);
            GL.AttachShader(ID,ss.id);
            GL.LinkProgram(ID);
            string linkInfo = GL.GetProgramInfoLog(ID);
            if(linkInfo!="")
                Console.WriteLine(linkInfo);
            SetStageMask();
        }

        public void Active(int pipeline)
        {
            GL.ActiveShaderProgram(pipeline,ID);
            GL.UseProgramStages(pipeline,ProgramStageMask.AllShaderBits,ID);
        }

        public override void ReloadProgram()
        {
            GL.DetachShader(ID,shaderSource.id);
            shaderSource.Reload();
            GL.AttachShader(ID,shaderSource.id);
            
            GL.LinkProgram(ID);
            string linkInfo = GL.GetProgramInfoLog(ID);
            if(linkInfo!="")
                Console.WriteLine(linkInfo);
            SetStageMask();
        }

        void SetStageMask()
        {
            switch(shaderSource.type)
            {
                case ShaderType.ComputeShader:
                    StageMask = ProgramStageMask.ComputeShaderBit;
                    break;
                case ShaderType.VertexShader:
                StageMask = ProgramStageMask.VertexShaderBit;
                    break;
                case ShaderType.TessControlShader:
                    StageMask = ProgramStageMask.TessControlShaderBit;
                    break;
                case ShaderType.TessEvaluationShader:
                    StageMask = ProgramStageMask.TessEvaluationShaderBit;
                    break;
                case ShaderType.GeometryShader:
                    StageMask = ProgramStageMask.GeometryShaderBit;
                    break;
                case ShaderType.FragmentShader:
                    StageMask = ProgramStageMask.FragmentShaderBit;
                    break;
                default:
                    StageMask = ProgramStageMask.AllShaderBits;
                    break;
            }
        }

        public int StageIndex()
        {
            switch(shaderSource.type)
            {
                case ShaderType.ComputeShader:
                    return 5;
                case ShaderType.VertexShader:
                    return 0;
                case ShaderType.TessControlShader:
                    return 3;
                case ShaderType.TessEvaluationShader:
                    return 4;
                case ShaderType.GeometryShader:
                    return 2;
                case ShaderType.FragmentShader:
                    return 1;
                default:
                    return -1;
            }
        }
        public static int StageIndex(ProgramStageMask mask)
        {
            switch(mask)
            {
                case ProgramStageMask.ComputeShaderBit:
                    return 5;
                case ProgramStageMask.VertexShaderBit:
                    return 0;
                case ProgramStageMask.TessControlShaderBit:
                    return 3;
                case ProgramStageMask.TessEvaluationShaderBit:
                    return 4;
                case ProgramStageMask.GeometryShaderBit:
                    return 2;
                case ProgramStageMask.FragmentShaderBit:
                    return 1;
                default:
                    return -1;
            }
        }
    }

    public class SPIRVProgram : GPUProgram
    {
        public SPIRVProgram(byte[] spirv)
        {
            int cs = GL.CreateShader(ShaderType.ComputeShader);
            //GL.ShaderBinary(1,cs,(OpenTK.Graphics.OpenGL4.BinaryFormat)ArbGlSpirv.ShaderBinaryFormatSpirVArb,spirv,spirv.Length);
            // GL.SpecializeShader()
            GL.ProgramBinary(ID,(OpenTK.Graphics.OpenGL4.BinaryFormat) ArbGlSpirv.ShaderBinaryFormatSpirVArb,spirv,spirv.Length);
        }
        public override void ReloadProgram()
        {
        }
    }

    public class ProgramGroup: IStageMask
    {
        public ProgramGroup()
        {
        }
        public void SetProgram(SeparableShaderProgram program)
        {
            if(programs[program.StageIndex()]!=null)
            {
                Console.WriteLine("stage:{0},has been set",program.GetStageMask());
            }
            programs[program.StageIndex()] = program;
        }
        internal SeparableShaderProgram[] programs = new SeparableShaderProgram[6];
        
        public ProgramStageMask GetStageMask()
        {
            ProgramStageMask mask = (ProgramStageMask)0;
            foreach(SeparableShaderProgram e in programs)
            {
                if(e!=null)
                    mask |= e.GetStageMask();
            }
            return mask;
        }

        public bool HasStageMask(ProgramStageMask mask)
        {
            for(int i =0;i<6;i++)
            {
                if(((int)mask&(1<<i))>0 && programs[i]!=null)
                    continue;
                else
                    return false;
            }
            return true;
        }

        public SeparableShaderProgram GetStageProgram(ProgramStageMask mask)
        {
            int index = SeparableShaderProgram.StageIndex(mask);
            if(index<0)
                return null;
            return programs[index];
        }
    }

    public abstract class ParameterData
    {
        //public int Location
        // {get; private set;}
        public Type type
        {
            get;
            protected set;
        }

        public virtual uint Size{get;}
        public int Offset{get;set;}
        public abstract dynamic GetData();
    }

    public class ParameterData<T>:ParameterData
    {
        public override uint Size
        {
            get
            {
                return (uint)Marshal.SizeOf<T>();
            }
        }

        public static implicit operator ParameterData<T>(T data)
        {
            return new ParameterData<T>(data);
        }

        public T data{get;set;}
        public ParameterData(T d)
        {
            type = typeof(T);
            data = d;
        }

        public override dynamic GetData()
        {
            return data;
        }
    }

    //像材质一样的东西，但是不像材质material，不同实例有独立不一样的参数
    //参数不会附加到材质上去，只要使用指定的Program，用固定的参数。
    public class ProgramParameters
    {
        Dictionary<string, UniformBlock> blocks = new Dictionary<string, UniformBlock>();
        public GPUProgram Program
        {
            get;
            private set;
        }

        Dictionary<string, ParameterData> uniformData = new Dictionary<string, ParameterData>();
        Uniforms uniforms;
        public ProgramParameters(GPUProgram p)
        {
            Program = p;
            uniforms = new Uniforms(p);
        }

        public bool UpdateUniformData<T>(string n, T data)
        {
            if(uniformData.ContainsKey(n))
            {
                ParameterData<T> tmp = uniformData[n] as ParameterData<T>;
                if(tmp!=null)
                {
                    tmp.data = data;
                    return true;
                }
            }
            else
            {
                uniforms.RegName(n);
                uniformData.Add(n,new ParameterData<T>(data));
                return true;
            }
            return false;
        }

        bool VerifyParameterType(ParameterData pd)
        {
            dynamic data = pd.GetData();
            if(data is float)return true;
            if(data is int)return true;
            if(data is Vector2)return true;
            if(data is Vector3)return true;
            if(data is Vector4)return true;
            if(data is Matrix4)return true;
            if(data is Texture)return true;
            return false;
        }

        public void UpdateUniforms()
        {
            foreach(var p in uniformData)
            {
                var value = uniformData[p.Key];
                if(VerifyParameterType(value))
                {
                    uniforms.UpdateUniform(p.Key,value.GetData());
                }
            }
        }

        public void RegUBO<T>(int bindingIndex,T data) where T: unmanaged
        {
            UBO<T> ubo = new UBO<T>(data);
            BufferBindingPoint.BindUBO(bindingIndex,ubo);
        }

        public UniformBlock RegBlock<T>(string blockname, T data)
        {
            if(blocks.ContainsKey(blockname))return null;
            var ub = new UniformBlock(Program,blockname);
            blocks.Add(blockname,ub);
            return ub;
        }

        public UniformBlock FindBlock(string blockname)
        {
            if(!blocks.ContainsKey(blockname))return null;
            return blocks[blockname];
        }
    }

    public class ProgramPipeline : IDOjbect, IDisposable, IPipelineActive
    {
        public static ProgramPipeline Default;
        public static ProgramPipeline Shadow;
        public static ProgramPipeline DeferredPre;

        public ProgramGroup BoundStage
        {
            get;
            private set;
            }

        public ProgramPipeline()
        {
            ID = GL.GenProgramPipeline();
            BoundStage = new ProgramGroup();
        }

        public void BindProgramGroup(ProgramGroup gp)
        {
            // var vp = gp.GetStageProgram(ProgramStageMask.VertexShaderBit).Program;
            // var fp = gp.GetStageProgram(ProgramStageMask.FragmentShaderBit).Program;
            // GL.BindAttribLocation(vp,0,"Position");
            // GL.BindFragDataLocation(fp,0,"fragcolor");
            // //Nvidia linking tips.
            // //C7592: ARB_separate_shader_objects requires built-in block gl_PerVertex to be redeclared before accessing its members
            // GL.LinkProgram(vp);
            // Console.WriteLine(GL.GetProgramInfoLog(vp));
            // GL.LinkProgram(fp);
            // Console.WriteLine(GL.GetProgramInfoLog(fp));

            BoundStage = gp;
            foreach(SeparableShaderProgram p in gp.programs)
            {
                if(p==null)continue;
                BindProgramStage(p);
            }
        }
        
        public void Prepare(ProgramStageMask stages, Material mtl)
        {
            GL.UseProgram(0);
            GL.BindProgramPipeline(ID);
            for(int i =0;i<6;i++)
            {
                Uniforms u = mtl.FindUniform(stages&(ProgramStageMask)(1<<i));//single stage
                if(u!=null)
                    BindProgramStage(u.ShaderProgram as SeparableShaderProgram);
            }
        }

        public void Prepare(ProgramStageMask stages, ProgramGroup pg)
        {
            GL.UseProgram(0);
            GL.BindProgramPipeline(ID);
            BindProgramStage(pg.GetStageProgram(stages));
        }

        public void BindProgramStage(SeparableShaderProgram ssp)
        {
            int index = ssp.StageIndex();
            BoundStage.programs[index] = ssp;

            var mask = ssp.GetStageMask();
            GL.UseProgramStages(ID,mask,ssp.ID);
        }

        public void Dispose()
        {
            GL.DeleteProgramPipeline(ID);
        }

        public void Active()
        {
            GL.UseProgram(0);
            GL.BindProgramPipeline(ID);
        }

    }

}

