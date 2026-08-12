using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OldGL =  OpenTK.Graphics.OpenGL.GL;

using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace DeepEditor.Common.G3D.GLTF
{
    public abstract class IDOjbect
    {
        public int ID
        {
            get;
            protected set;
        }
    }

    public class BufferObject : IDOjbect, IDisposable
    {        
        public BufferTarget BufferTarget
        {
            get;
            protected set;
        }
        protected BufferUsageHint usageHint;
        public int BufferSize
        {get;protected set;}
        protected BufferObject(BufferTarget target, BufferUsageHint hint)
        {
            BufferTarget = target;
            usageHint = hint;
            ID = GL.GenBuffer();
        }
        private BufferObject(int id,BufferTarget target, BufferUsageHint hint)
        {
            ID = id;
            BufferTarget = target;
            usageHint = hint;
        }

        public static void Bind(BufferObject obj)
        {
            GL.BindBuffer(obj.BufferTarget, obj.ID);
        }
        
        public void Dispose()
        {
            GL.BindBuffer(BufferTarget,0);
            GL.DeleteBuffer(ID);
        }

        public IntPtr MapBuffer(BufferAccess access)
        {
            GL.BindBuffer(BufferTarget,ID);
            return GL.MapBuffer(BufferTarget,access);
        }

        public IntPtr MapBufferRange()
        {
            //MapPersistentBit for datastorage
            GL.BindBuffer(BufferTarget,ID);
            return GL.MapBufferRange(BufferTarget,IntPtr.Zero,BufferSize,
            BufferAccessMask.MapReadBit);
        }
        
        public IntPtr MapBufferRange(IntPtr offset,int size)
        {
            //MapPersistentBit for datastorage
            GL.BindBuffer(BufferTarget,ID);
            return GL.MapBufferRange(BufferTarget,offset,size,
            BufferAccessMask.MapReadBit);
        }
        
        public void UnMapBuffer()
        {
            GL.BindBuffer(BufferTarget,ID);
            GL.UnmapBuffer(BufferTarget);
        }
    }

    public class ArrayBufferObject<T> : BufferObject where T : struct
    {
        public ArrayBufferObject(BufferTarget target, T[] data, BufferUsageHint hint = BufferUsageHint.StaticDraw):
        base(target,hint)
        {
            BufferSize = Marshal.SizeOf<T>() * data.Length;
            GL.BindBuffer(target, ID);
            GL.BufferData(target, Marshal.SizeOf<T>() * data.Length,data, hint);
        }

        public ArrayBufferObject(BufferTarget target, int size, BufferUsageHint hint = BufferUsageHint.StaticDraw):
        base(target,hint)
        {
            BufferSize = size;
            BufferTarget = target;
            usageHint = hint;
            GL.BindBuffer(target, ID);
            GL.BufferData(target, size, IntPtr.Zero, hint);
        }

        public void BufferData(T[] data)
        {
            BufferSize = data.Length * Marshal.SizeOf<T>();
            GL.BindBuffer(BufferTarget, ID);
            GL.BufferData(BufferTarget, data.Length * Marshal.SizeOf<T>(), data, usageHint);
        }

    }

    public class StructBufferObject<T> : BufferObject, IDisposable where T : unmanaged
    {
        protected T bufferData;
        public StructBufferObject(BufferTarget target, T data, BufferUsageHint hint = BufferUsageHint.StaticDraw):
        base(target,hint)
        {
            BufferSize = Marshal.SizeOf<T>();
            GL.BindBuffer(target, ID);
            T[] d = {data};
            GL.BufferData(target, Marshal.SizeOf<T>(), d, hint);
            GL.BindBuffer(target, 0);
        }

        public StructBufferObject(BufferTarget target, int size, BufferUsageHint hint = BufferUsageHint.StaticDraw):
        base(target,hint)
        {
            BufferSize = size;
            GL.BindBuffer(target, ID);
            GL.BufferData(target, size, IntPtr.Zero, hint);
            GL.BindBuffer(target, 0);
        }

        public void BufferData(T data)
        {
            GL.BindBuffer(BufferTarget, ID);
            T[] d = {data};
            GL.BufferData(BufferTarget, Marshal.SizeOf<T>(), d, usageHint);
            GL.BindBuffer(BufferTarget, 0);
        }
    }

    public class VBO<T> : ArrayBufferObject<T> where T :struct
    {
        public VBO(T[] data): base(BufferTarget.ArrayBuffer,data)
        {}
    }

    public class Drawable
    {
        BoundingBox box;
        static Drawable cube;
        public static Drawable Cube
        {
            get
            {
                if(cube==null)
                {
                    cube = new Drawable();
                    cube.Update(MeshPrimitive.Cube());
                }
                return cube;
            }
        }

        public int vao;
        public Drawable()
        {
            vao = GL.GenVertexArray();
        }
        //int vboPosition = 0,vboNormal=0,vboTangent=0,vboTexcoord0=0;
        //int ibo=0;
        public ArrayBufferObject<float> vboPosition{get;private set;}
        public ArrayBufferObject<float> vboNormal{get;private set;}
        public ArrayBufferObject<float> vboTangent{get;private set;}
        public ArrayBufferObject<float> vboTexcoord0{get;private set;}
        public ArrayBufferObject<uint> ibo{get;private set;}
        public int numIndices;

        public MeshPrimitive meshPrimitive
        {get;private set;}
        public AABB GetAABBBox()
        {
            AABB aabb = box as AABB;
            if(aabb==null)
            {
                aabb = new AABB();
                box = aabb;
            }

            if(meshPrimitive.comPosition==3)
                aabb.CalcAABBWithGPU(this,Matrix4.Identity);
            else
                aabb.Calc(meshPrimitive);
            return aabb;
        }
        
        public AABB GetAABBBox(Matrix4 model)
        {
            AABB aabb = box as AABB;
            if(aabb==null)
            {
                aabb = new AABB();
                box = aabb;
            }
            
            if(meshPrimitive.comPosition==3)
                aabb.CalcAABBWithGPU(this,model);
            else
                aabb.CalcSIMD(meshPrimitive,model);
            return aabb;
        }

        public void Update(MeshPrimitive m)
        {
            if(meshPrimitive==m)
                return;
            meshPrimitive = m;
            primitiveType = m.mode;
            GL.BindVertexArray(vao);
            if(m.positions!=null)
            {
                if(vboPosition==null)
                    vboPosition = new ArrayBufferObject<float>(BufferTarget.ArrayBuffer,m.positions.Length * 4);
                vboPosition.BufferData(m.positions);
                GL.VertexAttribPointer(0,m.comPosition,VertexAttribPointerType.Float,false,0,IntPtr.Zero);
                GL.EnableVertexAttribArray(0);
            }
            
            if(m.normals!=null)
            {
                if(vboNormal==null)vboNormal = new ArrayBufferObject<float>(BufferTarget.ArrayBuffer,m.normals.Length*4);
                vboNormal.BufferData(m.normals);
                GL.VertexAttribPointer(1,m.comNormal,VertexAttribPointerType.Float,false,0,IntPtr.Zero);
                GL.EnableVertexAttribArray(1);
            }
            if(m.tangents!=null)
            {
                if(vboTangent==null)vboTangent = new ArrayBufferObject<float>(BufferTarget.ArrayBuffer,m.tangents.Length*4);
                vboTangent.BufferData(m.tangents);
                GL.BufferData(BufferTarget.ArrayBuffer,m.tangents.Length * 4,m.tangents,BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(2,m.comTangent,VertexAttribPointerType.Float,false,0,IntPtr.Zero);
                GL.EnableVertexAttribArray(2);
            }
            if(m.uvs!=null&&m.uvs.Count>0)
            {
                if(vboTexcoord0==null)vboTexcoord0 = new ArrayBufferObject<float>(BufferTarget.ArrayBuffer,m.uvs[0].Length*4);
                vboTexcoord0.BufferData(m.uvs[0]);
                GL.BufferData(BufferTarget.ArrayBuffer,m.uvs[0].Length * 4,m.uvs[0],BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(3,2,VertexAttribPointerType.Float,false,0,IntPtr.Zero);
                GL.EnableVertexAttribArray(3);
            }

            if(m.indices!=null)
            {
                numIndices = m.indices.Length;
                if(ibo==null)ibo = new ArrayBufferObject<uint>(BufferTarget.ElementArrayBuffer,numIndices*4);
                ibo.BufferData(m.indices);
            }
            else
                Console.WriteLine("Mesh doesn't have indices");
            
            GL.BindVertexArray(0);
        }

        public PrimitiveType primitiveType;
        public virtual void Draw()
        {
            // int[] cp = {0};
            // GL.GetInteger(GetPName.CurrentProgram, cp);
            GL.BindVertexArray(vao);
            if(ibo!=null)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer,ibo.ID);
                GL.DrawElements(meshPrimitive.mode,numIndices,DrawElementsType.UnsignedInt,IntPtr.Zero);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
            }
            else
            {
                GL.DrawArrays(meshPrimitive.mode,0,meshPrimitive.positions.Length / meshPrimitive.comPosition);
            }
            GL.BindVertexArray(0);
        }

        //提供一个Indices的范围，进行绘制
        public bool DrawRangeElements(int start,int end)
        {
            if(ibo==null)return false;
            if(end-start+1>numIndices)return false;
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer,ibo.ID);
            GL.DrawRangeElements(meshPrimitive.mode,start,end,numIndices,DrawElementsType.UnsignedInt,IntPtr.Zero);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
            GL.BindVertexArray(0);
            return true;
        }

        //对Indices的所有index，进行一个baseVertex偏移。[0,1,2,3]=>[1,2,3,4]。用法，流水灯移动，顶点动画
        public bool DrawElementsBaseVertex(int baseVertex)
        {
            if(ibo==null)return false;
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer,ibo.ID);
            GL.DrawElementsBaseVertex(meshPrimitive.mode,numIndices,DrawElementsType.UnsignedInt,IntPtr.Zero, baseVertex);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
            GL.BindVertexArray(0);
            return true;
        }

        //对同一一个Mesh进行多次绘制（部分）。用法：多次绘制不同的子区域
        public bool MultiDrawArray(int[] first,int[] count)
        {
            if(first.Length!=count.Length) return false;
            GL.BindVertexArray(vao);
            GL.MultiDrawArrays(meshPrimitive.mode, first, count, count.Length);
            GL.BindVertexArray(0);
            return true;
        }

        //对一个Mesh进行多次绘制。用法：多次绘制不同的子区域。Array of Array [][]. 2D array[,].
        //逻辑上是 AOA, opentk的api是2D array.
        public bool MultiDrawElements(uint[][] indicesGroup,int[] count)
        {
            if(indicesGroup.Length!=indicesGroup.Length) return false;
            if(ibo==null) return false;
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
            var p = Marshal.UnsafeAddrOfPinnedArrayElement(indicesGroup,0);
            GL.MultiDrawElements(meshPrimitive.mode,count,DrawElementsType.UnsignedInt,p, count.Length);
            GL.BindVertexArray(0);
            return true;
        }

        public bool MultiDrawElements(uint[,] indicesGroup,int[] count)
        {
            if(indicesGroup.Length!=indicesGroup.Length) return false;
            if(ibo==null) return false;
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
            GL.MultiDrawElements(meshPrimitive.mode,count,DrawElementsType.UnsignedInt,indicesGroup, count.Length);
            GL.BindVertexArray(0);
            return true;
        }
        
        //提供实例个数进行绘制
        public virtual void DrawInstanced(int instance)
        {
            GL.BindVertexArray(vao);
            if(ibo!=null)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer,ibo.ID);
                GL.DrawElementsInstanced(meshPrimitive.mode,numIndices,DrawElementsType.UnsignedInt,IntPtr.Zero,instance);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
            }
            else
            {
                GL.DrawArraysInstanced(meshPrimitive.mode,0,meshPrimitive.positions.Length/meshPrimitive.comPosition,instance);
            }
            GL.BindVertexArray(0);
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct DrawArraysIndirectCommand
    {
        public uint count;
        public uint instanceCount;
        public uint first;
        public uint baseInstanced;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct DrawElementsIndirectCommand
    {
        public uint count;
        public uint instanceCount;
        public uint firstIndex;
        public int baseVertex;
        public uint baseInstance;
    };

    public class DrawableIndirectArray
    {
        PrimitiveType mode;
        ArrayBufferObject<DrawArraysIndirectCommand> indirectDraw;
        List<DrawArraysIndirectCommand> commands;
        public DrawableIndirectArray(int capacity)
        {
            commands = new List<DrawArraysIndirectCommand>(capacity);
        }

        public void AddDrawArrayCommand(DrawArraysIndirectCommand cmd)
        {
            commands.Add(cmd);
        }

        public void UpdateCommand()
        {
            indirectDraw.BufferData(commands.ToArray());
        }

        public virtual void DrawIndirect()
        {
            GL.BindBuffer(BufferTarget.DrawIndirectBuffer,indirectDraw.ID);
            GL.DrawArraysIndirect(mode,IntPtr.Zero);
            GL.BindBuffer(BufferTarget.DrawIndirectBuffer,0);
        }
    }

    public class DrawableIndirectElement
    {
        PrimitiveType mode;
        ArrayBufferObject<DrawElementsIndirectCommand> indirectDraw;
        List<DrawElementsIndirectCommand> commands;
        public DrawableIndirectElement(int capacity)
        {
            commands = new List<DrawElementsIndirectCommand>(capacity);
        }

        public void AddDrawArrayCommand(DrawElementsIndirectCommand cmd)
        {
            commands.Add(cmd);
        }

        public void UpdateCommand()
        {
            indirectDraw.BufferData(commands.ToArray());
        }

        public virtual void DrawIndirect()
        {
            GL.BindBuffer(BufferTarget.DrawIndirectBuffer,indirectDraw.ID);
            GL.DrawElementsIndirect(mode,DrawElementsType.UnsignedInt,IntPtr.Zero);
            GL.BindBuffer(BufferTarget.DrawIndirectBuffer,0);
        }
    }

    //indirect draw可以把CPU的提交压力降低，降低驱动验证次数。
    //但是仍然产生了间接绘制的调用，产生了第二类开销。
    //对于多次调用，可以用indirect draw. 相当于cache
    //子网格，一般按照相同的材质，不同的模型区分。
    //对于绘制不同的子网格，还是用下面的函数进行合并，来减少drawcall
    //multidraw*，*baseVertex, *instanced.

    public class BoundingBoxGroup
    {
        public Dictionary<BoundingBox,MeshPrimitive> Boxes
        {
            get;
            private set;
        } = new Dictionary<BoundingBox,MeshPrimitive>();
        Drawable drawble;
        public ShadersProgram program
        {
            get;
            private set;
        }
        public BoundingBoxGroup(ShadersProgram p)
        {
            program = p;
            drawble = new Drawable();
        }

        public AABB Combined
        {get;private set;} = new AABB();
        public void Combine()
        {
            Combined.Reset();
            foreach(var e in Boxes.Keys)
            {
                AABB aabb = e as AABB;
                if(aabb!=null)
                Combined.Combine(aabb);
            }
        }

        public void CalcBoundingBoxPrimitive(BoundingBox box)
        {
            if(!Boxes.ContainsKey(box))
            {
                Boxes.Add(box, new MeshPrimitive());
            }
            MeshPrimitive mp = Boxes[box];
            if(mp!=null)
                box.GetBoundingboxMeshPrimitive(ref mp);
        }

        public void Draw(Matrix4 model,Matrix4 mvp,Vector4 c)
        {
            program.Active();
            int uMVP = GL.GetUniformLocation(program.ID,"MVP");
            int uModel = GL.GetUniformLocation(program.ID,"Model");
            int ucolor = GL.GetUniformLocation(program.ID,"color");
            GL.ProgramUniformMatrix4(program.ID,uMVP,false,ref mvp);
            GL.ProgramUniformMatrix4(program.ID,uModel,false,ref model);
            GL.ProgramUniform4(program.ID,ucolor,c);
            foreach(var e in Boxes)
            {
                drawble.Update(e.Value);
                drawble.Draw();
            }
        }
    }

    public class MaterialDrawable : Drawable
    {
        public Material material;
        
        public MaterialDrawable(Material mtl):base()
        {
            material = mtl;
        }

        //using material internal program.
        public override void Draw()
        {
            Uniforms u = material.FindUniform(ProgramStageMask.AllShaderBits);
            if(u==null)
            {
                Console.WriteLine("invalid uniforms");
                return;
            }
            (u.ShaderProgram as ShadersProgram)?.Active();
            material.SetupPipelineState();
            material.UpdateUniforms();
            base.Draw();
        }
        
        //using material internal program and attach to pipeline
        public void Draw(ProgramPipeline pipeline)
        {
            foreach(var e in material.programUniform)
            {
                SeparableShaderProgram ssp = e.Value.ShaderProgram as SeparableShaderProgram;
                if(ssp!=null)
                    pipeline.BindProgramStage(ssp);
            }
            pipeline.Active();
            material.SetupPipelineState();
            material.UpdateUniforms();
            base.Draw();
        }

        //Replace ShadersProgram or a SeparableProgram. using material external program
        Dictionary<ProgramStageMask,Uniforms> newv = new Dictionary<ProgramStageMask, Uniforms>();
        public void DrawWith(GPUProgram program)
        {
            var oldv = material.programUniform;
            newv.Clear();
            foreach(var p in material.programUniform)
            {
                if(p.Key==program.GetStageMask())
                    newv.Add(p.Key,p.Value.CopyWith(program));
            }
            material.programUniform = newv;

            (program as ShadersProgram)?.Active();
            //material.SetupPipelineState();//pipeline state control by pipeline
            material.UpdateUniforms();
            base.Draw();

            material.programUniform = oldv;
        }

        //Replace Multi stage. using material external program and attach to pipeline.
        public void DrawWith(ProgramPipeline pipeline)
        {
            var oldv = material.programUniform;
            newv.Clear();
            foreach(var p in material.programUniform)
            {
                var sp = pipeline.BoundStage.GetStageProgram(p.Key);
                if(sp!=null)
                    newv.Add(p.Key,p.Value.CopyWith(sp));
            }
            material.programUniform = newv;
            
            pipeline.Active();
            //material.SetupPipelineState();//pipeline state control by pipeline
            material.UpdateUniforms();
            base.Draw();

            material.programUniform = oldv;
        }
    }
    
    public class DebugOuput
    {
        static void OnDebugMessage(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            string msg = Marshal.PtrToStringAnsi(message,length);
            var old = Console.ForegroundColor;
            switch(type)
            {
                case DebugType.DebugTypeError:
                Console.ForegroundColor= ConsoleColor.Red;
                break;  
                case DebugType.DebugTypeDeprecatedBehavior:
                Console.ForegroundColor= ConsoleColor.Yellow;
                break;
                case DebugType.DebugTypeUndefinedBehavior:
                Console.ForegroundColor= ConsoleColor.Blue;
                break;
                default:
                Console.ForegroundColor= ConsoleColor.Green;
                break;
            }
            Console.WriteLine("source:{0}, type:{1}, id:{2}, severity:{4} \n msg:{4}\n",source.ToString(),type.ToString(),id,severity,msg);
            Console.ForegroundColor = old;
        }

        static DebugProc dp;//avoid gc collect OnDebugMessage.
        public static void Init()
        {
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            dp = new DebugProc(OnDebugMessage);
            // GC.KeepAlive(dp);
            GL.DebugMessageCallback(dp,IntPtr.Zero);
            int[] ids= {};
            GL.DebugMessageControl(DebugSourceControl.DontCare,DebugTypeControl.DontCare,DebugSeverityControl.DontCare,0,ids,true);
        }
    }


}