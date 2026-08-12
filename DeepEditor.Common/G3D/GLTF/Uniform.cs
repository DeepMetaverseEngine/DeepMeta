using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

//Material{Program,Uniform,TextureResource}
//Program commit to pipeline,
//Uniform location relate to Program.
namespace DeepEditor.Common.G3D.GLTF
{
    public struct UBTransform
    {
        Matrix4 MVP;
        Matrix4 Model;
    }

    public struct UBCamera
    {
        Vector3 camPosition;
        Vector3 camDirection;
    }

    public struct UBLight
    {
        Vector4 lightsource;
        Vector3 lightcolor;
    }

    public class Uniforms
    {
        GPUProgram gpuProgram;
        public GPUProgram ShaderProgram
        {
            get
            {
                if(gpuProgram==null)
                    gpuProgram = ShadersProgram.Default;
                return gpuProgram;
            }
            set
            {
                if(gpuProgram!=null)
                {
                    gpuProgram.UnregisterNoitfication(weakReference);
                }
                gpuProgram = value;
                if(value!=null)
                {
                    value.RegisterNoitfication(weakReference);
                }
            }
        }

        public Dictionary<string,int> kv;
        public List<string> textureSlot;
        WeakReference<Uniforms> weakReference;
        public Uniforms()
        {
            weakReference = new WeakReference<Uniforms>(this);
            kv = new Dictionary<string, int>();
            textureSlot = new List<string>();
        }

        public Uniforms(GPUProgram program)
        {
            weakReference = new WeakReference<Uniforms>(this);
            kv = new Dictionary<string, int>();
            textureSlot = new List<string>();
            ShaderProgram = program;
        }

        //Finalizer be invoked automatically.
        // ~Uniforms()
        // {
        //     Dispose();
        // }

        public Uniforms CopyWith(GPUProgram p)
        {
            Uniforms u = new Uniforms(p);
            foreach(var e in kv)
            {
                u.RegName(e.Key);
            }
            return u;
        }

        int GetTextureSlot(string name)
        {
            int i = textureSlot.IndexOf(name);
            if(i>=0) return i;
            textureSlot.Add(name);
            return textureSlot.Count - 1;
        }
        public void UpdateUniformTexture(string name, Texture tex)
        {
            if(!kv.ContainsKey(name))return;
            if(kv[name]==-1)return;

            int slot = GetTextureSlot(name);
            GL.ActiveTexture(TextureUnit.Texture0 + slot);

            if(tex==null)
            {
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            else
            {
                GL.BindTexture(tex.textureType,tex.ID);
            }

            GL.ProgramUniform1(ShaderProgram.ID,kv[name],slot);
            // GL.Uniform1(kv[name],slot);
        }

        List<string> keys = null;
        public void ReloadLocations()
        {
            if(keys==null)
            {
                keys = new List<string>();
                foreach(var e in kv)
                {
                    keys.Add(e.Key);
                }
            }
            foreach(var k in keys)
            {
                kv[k] = GL.GetUniformLocation(ShaderProgram.ID,k);
            }
        }

        public int RegName(string name)
        {
            if(ShaderProgram.ID==0)
            {
                System.Console.WriteLine("invalid uniform. current program = 0");
                return -1;
            }
            //GL.UseProgram(ShaderProgram.ID);
            int l = GL.GetUniformLocation(ShaderProgram.ID, name);
            if(kv.ContainsKey(name))
            {
                kv[name] = l;
            }else{
                kv.Add(name,l);
            }
            return l;
        }
        
        public void UpdateUniform(string name, Texture tex)
        {
            UpdateUniformTexture(name,tex);
        }

        public void UpdateUniform(string name, float v)
        {
            // GL.Uniform1(kv[name],v);
            GL.ProgramUniform1(ShaderProgram.ID,kv[name],v);
        }

        public void UpdateUniform(string name, int i)
        {
            // GL.Uniform1(kv[name],i);
            GL.ProgramUniform1(ShaderProgram.ID,kv[name],i);
        }
        
        public void UpdateUniform(string name, Vector2 v2)
        {
            // GL.Uniform2(kv[name],v2);
            GL.ProgramUniform2(ShaderProgram.ID,kv[name],v2);
        }

        public void UpdateUniform(string name, Vector3 v3)
        {
            //GL.Uniform3(kv[name],v3);
            GL.ProgramUniform3(ShaderProgram.ID,kv[name],v3);
        }

        public void UpdateUniform(string name, Vector4 v4)
        {
            // GL.Uniform4(kv[name],v4);
            GL.ProgramUniform4(ShaderProgram.ID,kv[name],v4);
        }

        public void UpdateUniform(string name, Color4 c)
        {
            // GL.Uniform4(kv[name], new Vector4(c.R,c.G,c.B,c.A));
            GL.ProgramUniform4(ShaderProgram.ID,kv[name], new Vector4(c.R,c.G,c.B,c.A));
        }

        public void UpdateUniform(string name, Matrix4 m44)
        {
            // GL.UniformMatrix4(kv[name],false,ref m44);
            GL.ProgramUniformMatrix4(ShaderProgram.ID,kv[name],false,ref m44);
        }
    }

    public class BufferBindingPoint
    {
        //1 binding point -- 1 ubo -- multi BlockIndex
        public static int maxUBO = 1024;
        public static BufferBindingPoint[] UBOs = new BufferBindingPoint[1024];

        public BufferObject UBO
        {
            get;
            private set;
        }

        public static Dictionary<UniformBlock,BufferBindingPoint> blockBindings = new Dictionary<UniformBlock,BufferBindingPoint>();
        private BufferBindingPoint()
        {}

        public static BufferBindingPoint GetBindingPoint(int index)
        {
            if(index>=0&&index<maxUBO)
            {
                if(UBOs[index]==null)
                    UBOs[index] = new BufferBindingPoint();
                return UBOs[index];
            }
            return null;
        }

        public static bool BindUBO(int bindingindex, BufferObject ubo)
        {
            if(bindingindex>=0&&bindingindex<maxUBO)
            {
                GL.BindBufferBase(BufferRangeTarget.UniformBuffer,bindingindex,ubo.ID);
                BufferBindingPoint bp = GetBindingPoint(bindingindex);
                bp.UBO = ubo;
                return true;
            }
            return false;
        }
        

        public static bool BindBlock(int bindingindex, UniformBlock block)
        {
            if(bindingindex>=0&&bindingindex<maxUBO)
            {
                GL.UniformBlockBinding(block.Program.ID,block.BlockIndex,bindingindex);
                BufferBindingPoint bp = GetBindingPoint(bindingindex);
                blockBindings.Add(block,bp);
                return true;
            }
            return false;
        }

        public static void ChangeUniformBlockBinding(GPUProgram p, string uniformBlockName, int bindingindex)
        {
            int blockIndex = GL.GetUniformBlockIndex(p.ID,uniformBlockName);
            GL.UniformBlockBinding(p.ID,blockIndex,bindingindex);
        }

        public static void ChangeShaderStorageBlockBinding(GPUProgram p, string shaderStorageBlockName, int bindingindex)
        {
            int blockIndex = GL.GetProgramResourceIndex(p.ID,ProgramInterface.ShaderStorageBlock,shaderStorageBlockName);
            GL.ShaderStorageBlockBinding(p.ID,blockIndex,bindingindex);
        }

    }


    //constraint
    //https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters
    /*
    unmanage(struct(new()))
    */
    //fixed
    //https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/unsafe-code-pointers/fixed-size-buffers
    public class UniformBlock
    {
        /*
        in glsl.
        binding = 0, means use a binding point.

        for vairable.
        location = 0, means an address. pointer to header of strcut or array
        index = 0, mean offset of pointer. if index specified, location must be specified.
        
        for block.
        it doesn't name location, just has (block index). and binding

        we can assume buffer is static. or bindingpoint is static.
        but let bindingpoint is static better. the binding of uniformblock always bind a binding point.
        instead of connecting with UBO(uniform buffer) directly.
        if we assume that buffer is static, then bindingpiont is dynamic. that will be more trouble.

        we can change buffers data by MapBuffer, BufferData.
        we can change buffer object on bindingpoint. by BindBufferBase
        we can alsos change binding of Program uniform block or shader storage block. by 
        Buffers     bingding points     GPU-Programs                        
        A           0                   program0.transform(index = 0)       
        B           1                   program0.camera(index = 1)
        C           2                   program1.transform(index = 0)
                                        program2.lighting(index = 1)
                                        program2.camera(index = 2)
        */
    
        public int BlockIndex
        {
            get;
            protected set;
        }

        public GPUProgram Program
        {
            get;
            protected set;
        }
        
        public string name
        {
            get;
            private set;
        }

        public UniformBlock(GPUProgram p, string n)
        {
            BlockIndex = GL.GetUniformBlockIndex(p.ID,n);
        }

    }
    
    public class UBO<T>:StructBufferObject<T>  where T:unmanaged
    {
        public UBO(T data):base(BufferTarget.UniformBuffer,data)
        {
        }

        public unsafe T ReadBufferData()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer,ID);
            T* ptr = (T*)GL.MapBuffer(BufferTarget.UniformBuffer,BufferAccess.ReadWrite);
            bufferData = (*ptr);
            GL.UnmapBuffer(BufferTarget.UniformBuffer);
            return *ptr;
        }

        public T GetBufferData()
        {
            return bufferData;
        }

        public unsafe void UpdateBufferData(T data)
        {
            bufferData = data;
            GL.BindBuffer(BufferTarget.UniformBuffer,ID);
            T* ptr = (T*)GL.MapBuffer(BufferTarget.UniformBuffer,BufferAccess.ReadWrite);
            *ptr = bufferData;
            GL.UnmapBuffer(BufferTarget.UniformBuffer);
        }
    }

    public static class ShaderResourceHelper
    {
        public static int GetUniformIndex(int program,string name)
        {
            return GL.GetProgramResourceIndex(program,ProgramInterface.Uniform,name);
        }
        public static int[] GetUniforms(int program,string[] names)
        {
            int size = names.Length;
            int[] indices = new int[size];
            GL.GetUniformIndices(program,size,names,indices);
            return indices;
        }

        public static int GetUniformBlockIndex(int program,string name)
        {
            //return GL.GetUniformBlockIndex(program,name);
            return GL.GetProgramResourceIndex(program,ProgramInterface.UniformBlock,name);
        }

        public static int GetUniformLocation(int program,string name)
        {
            // return GL.GetUniformLocation(program,name);
            return GL.GetProgramResourceLocation(program,ProgramInterface.Uniform,name);
        }
    }

}