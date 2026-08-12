using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using OpenTK.Mathematics;

namespace DeepEditor.Common.G3D.GLTF
{
    //用来记录资源ID，方便调试
    public class ResourceID<T>
    {
        public uint id{get;private set;}
        public static uint counter = 0;
        public ResourceID()
        {
            id = counter;
            counter++;
        }
    } 

    public class MeshPrimitive:ResourceID<MeshPrimitive>
    {
        public MeshPrimitive():base(){}

        public int comPosition;
        public int comNormal;
        public int comTangent;
        public float[] positions;
        public float[] normals;
        public float[] tangents;
        public List<float[]> uvs;
        public uint[] indices;
        public PrimitiveType mode;

#region "for skined mesh"
        public Vector4[] joints;
        public Vector4[] weights;
#endregion      
        public int VertexCount
        {
            get
            {
                return positions.Length / comPosition;
            }
        }
        public int NormalCount
        {
            get
            {
                return normals.Length / comNormal;
            }
        }

        static MeshPrimitive surface;
        public static MeshPrimitive Surface()
        {
            float[] vts = {-1,1,0, 1,1,0, 1,-1,0,  1,-1,0, -1,-1,0, -1,1,0,};
            if(surface==null)
            {
                surface = new MeshPrimitive();
                surface.positions = vts;
                surface.comPosition = 3;
                surface.mode = PrimitiveType.Triangles;
            }
            return surface;
        }

        static MeshPrimitive cube;
        public static MeshPrimitive Cube(float factor = 1)
        {
            float[] pp = {
                -1,1,1, 1,1,1, -1,1,-1, -1,1,-1, 1,1,1, 1,1,-1,//top
                -1,-1,1, 1,-1,1, -1,-1,-1, -1,-1,-1, 1,-1,1, 1,-1,-1,//bottom
                1,1,1, 1,-1,1, 1,-1,-1, 1,-1,-1, 1,1,-1, 1,1,1,//right
                -1,1,1, -1,-1,1, -1,-1,-1, -1,-1,-1, -1,1,-1, -1,1,1,//left
                -1,1,1, 1,1,1, 1,-1,1, 1,-1,1, -1,-1,1 ,-1,1,1,//front
                -1,1,-1, 1,1,-1, 1,-1,-1, 1,-1,-1, -1,-1,-1 ,-1,1,-1//back
                };

            float[] nn = {
                0,1,0, 0,1,0, 0,1,0, 0,1,0, 0,1,0, 0,1,0,
                0,-1,0, 0,-1,0, 0,-1,0, 0,-1,0, 0,-1,0, 0,-1,0,
                1,0,0, 1,0,0, 1,0,0, 1,0,0, 1,0,0, 1,0,0,
                -1,0,0, -1,0,0, -1,0,0, -1,0,0, -1,0,0, -1,0,0,
                0,0,1, 0,0,1, 0,0,1, 0,0,1, 0,0,1, 0,0,1,
                0,0,-1, 0,0,-1, 0,0,-1, 0,0,-1, 0,0,-1, 0,0,-1
            };

            float[] tc = {
                0,0,1,0,0,1, 0,1,1,0,1,1,
                0,0,1,0,0,1, 0,1,1,0,1,1,
                0,1,0,0,1,0, 1,0,1,1,0,1,
                1,1,1,0,0,0, 0,0,0,1,1,1,
                0,1,1,1,1,0, 1,0,0,0,0,1,
                1,1,0,1,0,0, 0,0,1,0,0,1
            };
            
            for(int i = 0;i<pp.Length;i++)
                pp[i] *= factor;
                
            if(cube==null)
            {
                cube = new MeshPrimitive();
                cube.comPosition = 3;
                cube.comNormal = 3;
                cube.positions = pp;
                cube.normals = nn;
                cube.uvs = new List<float[]>();
                cube.uvs.Add(tc);
                cube.mode = PrimitiveType.Triangles;
            }
            return cube;
        }
    }

    public class Mesh
    {
        public MeshPrimitive[] primitives;
        public float[] weight;//contribution of morph target to final.根meshprimitive里的weight不同
        public Mesh()
        {

        }
        public Mesh(int numPrimitive)
        {
            primitives = new MeshPrimitive[numPrimitive];
        }
    }

    public class Joint
    {
        public int nodeID;
        public Matrix4 inverseBindMatrix;//局部的反响绑定矩阵
        //蒙皮网格根静态网格的坐标空间都是用局部空间。
        //globalTransform 由各自节点(Node)的局部变换的相乘结果，从根节点到各自独立节点的变换路径。
        //所表达的就是从局部到全局(世界)的变换。相反，从场景世界坐标到局部节点就是inverse(globalTransform)
        //globalJointTranform。类似于globalTransform 就是它是对于关节而言的。实际上Joint是一种Node

        //JointMatrix = inverse(globalTransform) * globalJointTranform * inverseBindMatrix
        //inverse(globalTransform)转换回绑定姿态时的局部坐标
        //globalJointTranform 对子关节,全局变换回根关节坐标空间
        
        public Joint[] children{get;private set;}
        public void AllocateChildren(uint size)
        {
            children = new Joint[size];
        }

        public void SetChildren(uint childID,Joint j)
        {
            children[childID] = j;
        }    
    }

    //表示一套蒙皮，其中包含了多个关节Joints
    //每一个关节有一个反向绑定矩阵，其中关节引用了一个节点ID
    public class Skin
    {
        public List<Matrix4> matrices = new List<Matrix4>();
        public List<Joint> joints = new List<Joint>();
        public Skin()
        {
        }

        //建立Joint之间的层级关系
        public void MakeSkeleton(glTFLoader.Schema.Gltf model,Dictionary<int,Joint> table)
        {
            foreach(var j in joints)
            {
                var n = model.Nodes[j.nodeID];
                if(n.Children!=null)
                {
                    j.AllocateChildren((uint)n.Children.Length);
                    for(uint i =0;i<n.Children.Length;i++)
                    {
                        int childID = n.Children[i];
                        j.SetChildren(i,table[childID]);
                    }
                }   
            }
        }
    }

    public class SkinMesh : Mesh
    {
        public Skin skin
        {get;private set;}

        public SkinMesh(Skin sk,Mesh m)
        {
            skin = sk;
            this.primitives = m.primitives;
            this.weight = m.weight;
        }
    }

    public class ImageData
    {
        public byte[] pixels;
        
        public System.Drawing.Imaging.PixelFormat imgformat;
        public OpenTK.Graphics.OpenGL4.PixelFormat GLPixelFormat
        {
            get
            {
                OpenTK.Graphics.OpenGL4.PixelFormat result = 0;
                switch (imgformat)
                {
                    case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                        result = OpenTK.Graphics.OpenGL4.PixelFormat.Bgra;
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                        result = OpenTK.Graphics.OpenGL4.PixelFormat.Bgr;
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                        result = OpenTK.Graphics.OpenGL4.PixelFormat.Bgra;
                        break;
                    default:
                        Console.WriteLine("inordinary pixel format");
                        break;
                }
                return result;
            }
        }
#if VULKAN
        public Vulkan.VkFormat VKFormat
        {
            get
            {
                var result = Vulkan.VkFormat.Undefined;
                switch (imgformat)
                {
                    case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                        result = Vulkan.VkFormat.B8g8r8a8Uint;
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                        result = Vulkan.VkFormat.B8g8r8Uint;
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                        result = Vulkan.VkFormat.B8g8r8a8Uint;
                        break;
                    default:
                        Console.WriteLine("inordinary pixel format");
                        break;
                }
                return result;
            }
        }
#endif
    }

    public class ImageData1D:ImageData
    {
        public uint width;
    }

    public class ImageData3D:ImageData
    {
        public uint width;
        public uint height;
        public uint depth;

    }

    public class ImageData2D:ImageData
    {
        public uint width;
        public uint height;

        uint stride;
        public uint Stride
        {
            get
            {
                if(stride!=0)return stride;
                uint result = 0;
                switch (imgformat)
                {
                    case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                        result = width * 4;
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                        result = width * 3;
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                        result = width * 4;
                        break;
                }
                if(result % 4 != 0)
                {
                    result = (result / 4 + 1) * 4;
                }
                return result;
            }
        }

        
        public ImageData2D(uint w,uint h, System.Drawing.Imaging.PixelFormat f)
        {
            width = w;
            height = h;
            imgformat = f;
            uint s = Stride;
            if(s > 0)
                pixels = new byte[s * h];
        }

        public ImageData2D(uint w,uint h, uint s)
        {
            width = w;
            height = h;
            stride = s;
            pixels = new byte[stride * h];
        }

        public void FillData(System.Drawing.Bitmap bmp)
        {
            System.Drawing.Imaging.BitmapData bmpdata = bmp.LockBits(new System.Drawing.Rectangle(0, 0, (int)width, (int)height),
            System.Drawing.Imaging.ImageLockMode.ReadOnly, imgformat);
            Marshal.Copy(bmpdata.Scan0, pixels, 0, (int)(Stride * height));
            bmp.UnlockBits(bmpdata);
        }
    }
}