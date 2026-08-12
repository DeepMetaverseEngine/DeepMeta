using DeepCore.GUI.Editor;
using glTFLoader.Schema;
using OpenTK.Compute.OpenCL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace DeepEditor.Common.G3D.GLTF
{
    public abstract class ModelLoader
    {
        public List<Mesh> meshes;
        public Scene scene;
        protected ModelLoader()
        {
            meshes = new List<Mesh>();
        }

    }

    // public class PerspectiveCamera
    // {
    //     Matrix4 matView;
    //     public PerspectiveCamera (Vector3 look,Vector3 at, Vector3 up)
    //     {
    //         matView = Matrix4.LookAt(look,at,up);
    //     }
    // }

    //dual node.
    public class Node<T, D>
    {
        public Node<T, D> parent
        { get; }
        public uint nodeID
        { get; set; }
        public T related
        { get; set; }//like local transform;
        public D content
        { get; set; }
        public Node(T t)
        {
            related = t;
            children = new List<Node<T, D>>();
        }

        public Node(T t, D d)
        {
            related = t;
            content = d;
            children = new List<Node<T, D>>();
        }

        protected Node(T t, D d, List<Node<T, D>> c)
        {
            related = t;
            content = d;
            children = c;
        }

        public List<Node<T, D>> children;
        public Node<T, D> FindNode(D d)
        {
            //System.Runtime.Intrinsics.Vector128
            Node<T, D> result = null;
            if (children == null) return null;
            //Breadth first search
            foreach (var e in children)
            {
                if (e.content.Equals(d))
                    return e;
            }
            foreach (var e in children)
            {
                return e.FindNode(d);
            }
            return result;
        }
    }

    //groupDrawable,groupAnimation ... single processing / single object type.
    //for DOP designing. ST-write, MT-read.
    public class GroupMesh : Node<Matrix4, Mesh>
    {
        public GroupMesh(Node<Matrix4, Mesh> c) : base(c.related, c.content, c.children)
        {
        }

        public void Recursive(Action<Matrix4, Mesh> processor, Matrix4 mtxParent,
        Node<Matrix4, Mesh> parent = null)
        {
            if (parent == null)
                parent = this;

            processor.Invoke(mtxParent, parent.content);
            foreach (var e in parent.children)
            {
                Recursive(processor, mtxParent * e.related, e);
            }
        }
    }

    public class Scene
    {
        public static Scene ActiveScene;
        public uint sceneID;
        public uint currentID;
        public uint NextID()
        {
            return currentID++;
        }

        public List<Scene> sub;
        public Scene()
        {
            sub = new List<Scene>();
            // groupPhysical = new Group<Matrix4, Physics>();
        }

        //matrix4 propably driven by physical->controller->animation.
        // public Group<Matrix4,Physics> groupPhysical;
        public GroupMesh groupMesh;
        // public Group<Matrix4,Animation> groupAnimation;
        void MergeInputMatrix()
        {
        }

        // public Group<Matrix2,UI> groupUI;

    }

    public class GLTFConverter
    {
        public glTFLoader.Schema.Gltf model;
        public List<byte[]> buffers;
        Vector4[] ReadVec4(byte[] data, int offset, int length)
        {
            Vector4[] result = new Vector4[length / 16];
            for (int i = 0; i < result.Length; i++)
            {
                result[i].X = BitConverter.ToSingle(data, offset + i * 16 + 0);
                result[i].Y = BitConverter.ToSingle(data, offset + i * 16 + 4);
                result[i].Z = BitConverter.ToSingle(data, offset + i * 16 + 8);
                result[i].W = BitConverter.ToSingle(data, offset + i * 16 + 12);
            }
            return result;
        }

        float[] ReadFloats(byte[] data, int offset, int length)
        {
            // int sizeT = sizeof(T);//where T: unmanaged
            float[] result = new float[length / 4];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToSingle(data, offset + i * 4);
            }
            return result;
        }
        uint[] ReadUInts(byte[] data, int offset, int length)
        {
            uint[] result = new uint[length / 4];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToUInt32(data, offset + i * 4);
            }
            return result;
        }
        ushort[] ReadUShorts(byte[] data, int offset, int length)
        {
            ushort[] result = new ushort[length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToUInt16(data, offset + i * 2);
            }
            return result;
        }
        short[] ReadShorts(byte[] data, int offset, int length)
        {
            short[] result = new short[length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToInt16(data, offset + i * 2);
            }
            return result;
        }

        int getNumComponentsOfVec(glTFLoader.Schema.Accessor.TypeEnum t)
        {
            if ((int)t >= 4)
                throw new Exception("invalid component value");
            return (int)t + 1;
        }
        public GLTFConverter(glTFLoader.Schema.Gltf model, List<byte[]> buffers)
        {
            this.model = model;
            this.buffers = buffers;
        }

        public SamplerInfo ConvertSampler(Sampler s)
        {
            SamplerInfo si = new SamplerInfo();
            si.MagFilter = (TextureMagFilter)(int)s.MagFilter;
            si.MinFilter = (TextureMinFilter)(int)s.MinFilter;
            si.WrapS = (TextureWrapMode)(int)s.WrapS;
            si.WrapT = (TextureWrapMode)(int)s.WrapT;
            return si;
        }

        void StoreImage(byte[] data, int imgindex)
        {
            glTFLoader.Schema.Image img = model.Images[imgindex];
            switch (img.MimeType)
            {
                case glTFLoader.Schema.Image.MimeTypeEnum.image_png:
                    System.IO.File.WriteAllBytes(string.Format(@".\{0}.png", imgindex), data);
                    break;
                case glTFLoader.Schema.Image.MimeTypeEnum.image_jpeg:
                    System.IO.File.WriteAllBytes(string.Format(@".\{0}.jpg", imgindex), data);
                    break;
            }
        }
        public Texture2D ConvertTexture(glTFLoader.Schema.Texture tex)
        {
            int iimage = (int)tex.Source;
            glTFLoader.Schema.Image img = model.Images[iimage];
            BufferView bvImg = model.BufferViews[(int)img.BufferView];
            byte[] buffer = buffers[bvImg.Buffer];
            byte[] imgdata = new byte[bvImg.ByteLength];
            Array.Copy(buffer, bvImg.ByteOffset, imgdata, 0, imgdata.Length);
            // StoreImage(imgdata,iimage);
            ImageData2D imageData = ImageLoader.Load(imgdata);
            Texture2D texture2D = new Texture2D(imageData);
            if (tex.Sampler != null)
            {
                Sampler s = model.Samplers[(int)tex.Sampler];
                texture2D.SetupSampler(ConvertSampler(s));
            }
            return texture2D;
        }

        public PBRMaterial ConvertMaterial(List<Texture2D> textures, glTFLoader.Schema.Material mtl)
        {
            PBRMaterial pbrMtl = MaterialFactory.CreatePBRMaterial();
            pbrMtl.AlphaMode = (int)mtl.AlphaMode;
            pbrMtl.DoubleSided = mtl.DoubleSided;

            pbrMtl.AlphaCutoff = mtl.AlphaCutoff;
            pbrMtl.EmissiveColor = Material.ToVector(mtl.EmissiveFactor);
            if (mtl.EmissiveTexture != null)
                pbrMtl.EmissiveTexture = textures[mtl.EmissiveTexture.Index];

            if (mtl.OcclusionTexture != null)
            {
                pbrMtl.OcclusionTexture = textures[mtl.OcclusionTexture.Index];
                pbrMtl.OcclusionStrength = mtl.OcclusionTexture.Strength;
            }

            if (mtl.PbrMetallicRoughness != null)
            {
                if (mtl.PbrMetallicRoughness.BaseColorTexture != null)
                    pbrMtl.BaseColorTexture = textures[mtl.PbrMetallicRoughness.BaseColorTexture.Index];
                if (mtl.PbrMetallicRoughness.MetallicRoughnessTexture != null)
                    pbrMtl.MetallicRoughnessTexture = textures[mtl.PbrMetallicRoughness.MetallicRoughnessTexture.Index];

                pbrMtl.MetallicFactor = mtl.PbrMetallicRoughness.MetallicFactor;
                pbrMtl.RoughnessFactor = mtl.PbrMetallicRoughness.RoughnessFactor;
                pbrMtl.BaseColor = Material.ToVector(mtl.PbrMetallicRoughness.BaseColorFactor);
            }
            if (mtl.NormalTexture != null)
                pbrMtl.NormalTexture = textures[mtl.NormalTexture.Index];

            //process extensions
            if (mtl.Extensions != null)
                foreach (var e in mtl.Extensions)
                {
                    Console.WriteLine(e.Key);
                    MaterialGLTFExtension extmtl = MaterialGLTFExtension.FatoryCreate(e.Key);
                    extmtl.SetupProperty(textures, e.Value);
                    pbrMtl.InitExtension(extmtl);
                }
            return pbrMtl;
        }


        Skin ConvertSkin(glTFLoader.Schema.Skin skin)
        {
            Skin sk = new Skin();
            Accessor accessor = model.Accessors[(int)skin.InverseBindMatrices];
            byte[] b = ReadAccesor(accessor);
            Matrix4[] mtxs = AccessorBytesToMatrix4F(b);
            Dictionary<int, Joint> table = new Dictionary<int, Joint>();
            for (int i = 0; i < skin.Joints.Length; i++)
            {
                int ni = skin.Joints[i];
                // var node = model.Nodes[ni];
                Joint j = new Joint();
                j.nodeID = ni;
                j.inverseBindMatrix = mtxs[i];
                sk.joints.Add(j);
                sk.matrices.Add(mtxs[i]);
                table.Add(ni, j);
            }
            sk.MakeSkeleton(model, table);
            return sk;
        }

        public unsafe Matrix4[] AccessorBytesToMatrix4F(byte[] b)
        {
            if (b.Length % 64 != 0)
                throw new Exception("Data has problem");
            int len = b.Length / 64;
            Matrix4[] mtxs = new Matrix4[len];
            for (int i = 0; i < len; i++)
            {
                Matrix4 m = default;
                for (int x = 0; x < 4; x++)
                    for (int y = 0; y < 4; y++)
                    {
                        m[x, y] = BitConverter.ToSingle(b, i * 64 + x * 16 + y * 4);
                    }
                mtxs[i] = m;
            }
            return mtxs;
        }

        public unsafe byte[] ReadAccesor(Accessor accessor)
        {
            int sizeOfComponent = 0, sizeOfType = 0;
            switch (accessor.Type)
            {
                case Accessor.TypeEnum.SCALAR:
                    sizeOfComponent = 1;
                    break;
                case Accessor.TypeEnum.VEC2:
                    sizeOfComponent = 2;
                    break;
                case Accessor.TypeEnum.VEC3:
                    sizeOfComponent = 3;
                    break;
                case Accessor.TypeEnum.VEC4:
                    sizeOfComponent = 4;
                    break;
                case Accessor.TypeEnum.MAT2:
                    sizeOfComponent = 4;
                    break;
                case Accessor.TypeEnum.MAT3:
                    sizeOfComponent = 9;
                    break;
                case Accessor.TypeEnum.MAT4:
                    sizeOfComponent = 16;
                    break;
            }
            switch (accessor.ComponentType)
            {
                case Accessor.ComponentTypeEnum.UNSIGNED_INT:
                    sizeOfType = 4;
                    break;
                case Accessor.ComponentTypeEnum.FLOAT:
                    sizeOfType = 4;
                    break;
                case Accessor.ComponentTypeEnum.UNSIGNED_SHORT:
                    sizeOfType = 2;
                    break;
                case Accessor.ComponentTypeEnum.SHORT:
                    sizeOfType = 2;
                    break;
                case Accessor.ComponentTypeEnum.BYTE:
                    sizeOfType = 1;
                    break;
                case Accessor.ComponentTypeEnum.UNSIGNED_BYTE:
                    sizeOfType = 1;
                    break;
            }
            int aCount = accessor.Count;
            byte[] result = new byte[sizeOfComponent * sizeOfType * aCount];
            int aOffset = accessor.ByteOffset;

            BufferView bufferView = model.BufferViews[(int)accessor.BufferView];
            int bvOffset = bufferView.ByteOffset;
            byte[] bufferdata = buffers[bufferView.Buffer];
            fixed (void* p = bufferdata)
            {
                Marshal.Copy((IntPtr)p + bvOffset + aOffset, result, 0, result.Length);
            }
            return result;
        }

        public SkinMesh ConvertSKinMesh(glTFLoader.Schema.Skin skin, glTFLoader.Schema.Mesh mesh)
        {
            // skin.InverseBindMatrices
            var sk = ConvertSkin(skin);
            var m = ConvertMesh(mesh);
            SkinMesh skm = new SkinMesh(sk, m);
            return skm;
        }

        public Mesh ConvertMesh(glTFLoader.Schema.Mesh mesh)
        {
            Console.WriteLine("Name:{0}", mesh.Name);
            Mesh result = new Mesh(mesh.Primitives.Length);
            for (int i = 0; i < mesh.Primitives.Length; i++)
            {
                var primitive = mesh.Primitives[i];
                MeshPrimitive mp = new MeshPrimitive();

                mp.mode = (PrimitiveType)primitive.Mode;
                int indicesIndex = (int)primitive.Indices;
                Accessor aIndices = model.Accessors[indicesIndex];

                byte[] indicesData = ReadAccesor(aIndices);
                if (aIndices.ComponentType == Accessor.ComponentTypeEnum.UNSIGNED_SHORT)
                    mp.indices = Array.ConvertAll(ReadUShorts(indicesData, 0, indicesData.Length),
                    new Converter<ushort, uint>((ushort a) => (uint)a));
                else if (aIndices.ComponentType == Accessor.ComponentTypeEnum.UNSIGNED_INT)
                    mp.indices = ReadUInts(indicesData, 0, indicesData.Length);
                Console.WriteLine("indices:{0}", indicesData.Length);
                foreach (var attrib in primitive.Attributes)
                {
                    string bufName = attrib.Key;
                    Accessor aAttribute = model.Accessors[attrib.Value];
                    byte[] attribData = ReadAccesor(aAttribute);
                    switch (bufName)
                    {
                        case "POSITION":
                            mp.positions = ReadFloats(attribData, 0, attribData.Length);
                            mp.comPosition = getNumComponentsOfVec(aAttribute.Type);
                            break;
                        case "NORMAL":
                            mp.normals = ReadFloats(attribData, 0, attribData.Length);
                            mp.comNormal = getNumComponentsOfVec(aAttribute.Type);
                            break;
                        case "TANGENT":
                            mp.tangents = ReadFloats(attribData, 0, attribData.Length);
                            mp.comTangent = getNumComponentsOfVec(aAttribute.Type);
                            break;
                        case "TEXCOORD_0":
                            mp.uvs = new List<float[]>();
                            mp.uvs.Add(ReadFloats(attribData, 0, attribData.Length));
                            break;
                        case "JOINTS_0":
                            mp.joints = ReadVec4(attribData, 0, attribData.Length);
                            break;
                        case "WEIGHTS_0":
                            mp.weights = ReadVec4(attribData, 0, attribData.Length);
                            break;
                    }
                }
                result.primitives[i] = mp;
            }
            result.weight = mesh.Weights;
            return result;
        }

        // public Drawable ConvertDrawable(List<Material> materials, glTFLoader.Schema.Mesh mesh)
        // {
        //     MaterialDrawable m = new MaterialDrawable();
        //     foreach (var primitive in mesh.Primitives)
        //     {
        //         m.primitiveType = (PrimitiveType)primitive.Mode;
        //         m.material = materials[(int)primitive.Material];
        //         int indicesIndex = (int)primitive.Indices;
        //         Accessor aIndices = model.Accessors[indicesIndex];
        //         BufferView bvIndices = model.BufferViews[(int)aIndices.BufferView];
        //         byte[] buffer = buffers[bvIndices.Buffer];
        //         int off = bvIndices.ByteOffset;
        //         int len = bvIndices.ByteLength;
        //         if(aIndices.ComponentType==Accessor.ComponentTypeEnum.UNSIGNED_SHORT)
        //             m.indices = ReadShorts(buffer, off, len);
        //         else if(aIndices.ComponentType==Accessor.ComponentTypeEnum.UNSIGNED_INT)
        //             m.indices = ReadInts(buffer,off, len);
        //         foreach (var attrib in primitive.Attributes)
        //         {
        //             string bufName = attrib.Key;
        //             Accessor aAttribute = model.Accessors[attrib.Value];
        //             BufferView bufView = model.BufferViews[(int)aAttribute.BufferView];
        //             buffer = buffers[bufView.Buffer];
        //             off = bufView.ByteOffset;
        //             len = bufView.ByteLength;
        //             switch (bufName)
        //             {
        //                 case "POSITION":
        //                     m.positions = ReadFloats(buffer, off, len);
        //                     m.comPosition = getNumComponentsOfVec(aAttribute.Type);
        //                     break;
        //                 case "NORMAL":
        //                     m.normals = ReadFloats(buffer, off, len);
        //                     m.comNormal = getNumComponentsOfVec(aAttribute.Type);
        //                     break;
        //                 case "TANGENT":
        //                     m.tangents = ReadFloats(buffer, off, len);
        //                     m.comTangent = getNumComponentsOfVec(aAttribute.Type);
        //                     break;
        //                 case "TEXCOORD_0":
        //                     m.uvs = new List<float[]>();
        //                     m.uvs.Add(ReadFloats(buffer, off, len));
        //                     break;
        //             }
        //         }
        //     }
        //     return m;
        // }
    }

    //https://github.com/KhronosGroup/glTF/tree/master/specification/2.0
    public class GLTFLoader : ModelLoader
    {
        //public int texDefault;
        public GLTFLoader() : base()
        {
            // texDefault = GL.GenTexture();
            // GL.BindTexture(TextureTarget.Texture2D,texDefault);
            // uint[] pixels = new uint[64*64];
            // for(int j =0 ;j<64;j++)
            // for(int i=0;i<64;i++)
            // {
            //     if((i+j)%2==0)
            //         pixels[i+64*j] = 0;
            //     else
            //         pixels[i+64*j] = 0xffffffff;
            // }
            // GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgba32f,64,64,0,PixelFormat.Rgba,PixelType.UnsignedByte,pixels);
            // GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapS,(int)TextureWrapMode.Repeat);
            // GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapT,(int)TextureWrapMode.Repeat);
            // GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Linear);
            // GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.Linear);
        }

        public List<Texture2D> textures;
        public List<Material> materials;
        public List<GroupMesh> roots = new List<GroupMesh>();


        Gltf model;
        GLTFConverter converter;
        public void LoadFile(string filepath)
        {
            FileStream fileGLTF = File.OpenRead(filepath);
            byte[] hMagic = new byte[4];
            fileGLTF.Read(hMagic, 0, hMagic.Length);
            byte[] hVersion = new byte[4];
            fileGLTF.Read(hVersion, 0, hVersion.Length);
            byte[] hLength = new byte[4];
            fileGLTF.Read(hLength, 0, hLength.Length);
            //load buffers
            List<byte[]> buffers = new List<byte[]>();
            do
            {
                byte[] cLength = new byte[4];
                byte[] cType = new byte[4];
                fileGLTF.Read(cLength, 0, cLength.Length);
                fileGLTF.Read(cType, 0, cType.Length);
                uint length = BitConverter.ToUInt32(cLength);
                uint type = BitConverter.ToUInt32(cType);

                byte[] buffer = new byte[length];
                fileGLTF.Read(buffer, 0, buffer.Length);
                if (type == 0x4e4f534a)
                {
                    //File.WriteAllBytes(filepath + ".json",buffer);
                    //Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer));
                    Console.WriteLine("json, at:{0}, len:{1}", fileGLTF.Position, length);
                }
                else if (type == 0x004e4942)
                {
                    Console.WriteLine("binary, at:{0}, len:{1}", fileGLTF.Position, length);
                    buffers.Add(buffer);
                }
                //fileGLTF.Position+=length;
            } while (fileGLTF.Position < fileGLTF.Length);
            fileGLTF.Close();

            model = glTFLoader.Interface.LoadModel(filepath);
            PrintGLTFModelInfo(model);
            converter = new GLTFConverter(model, buffers);
            //load texture image
            textures = new List<Texture2D>();
            foreach (var t in model.Textures)
            {
                textures.Add(converter.ConvertTexture(t));
            }
            // var scenes = model.Scenes[(int)model.Scene];
            materials = new List<Material>();
            foreach (var mtl in model.Materials)
            {
                PBRMaterial pbrMtl = converter.ConvertMaterial(textures, mtl);
                materials.Add(pbrMtl);
                pbrMtl.PrintValidUniforms();
            }

            //load geometry
            foreach (var mesh in model.Meshes)
            {
                Mesh m = converter.ConvertMesh(mesh);
                meshes.Add(m);
            }

            //load scene
            foreach (var s in model.Scenes)
            {
                roots.Add(new GroupMesh(CreateNodes(model.Scenes[0])));
            }
        }

        public Node<Matrix4, Mesh> CreateNodes(glTFLoader.Schema.Scene s,
        glTFLoader.Schema.Node parent = null)
        {
            Node<Matrix4, Mesh> nmesh = new Node<Matrix4, Mesh>(Matrix4.Identity);

            int[] pNodes = null;
            if (parent == null)
                pNodes = s.Nodes;
            else
                pNodes = parent.Children;

            if (pNodes == null)
                return null;
            foreach (var n in pNodes)
            {
                glTFLoader.Schema.Node node = model.Nodes[n];
                glTFLoader.Schema.Skin gskin = null;
                glTFLoader.Schema.Mesh gmesh = null;
                if (node.Skin != null)
                    gskin = model.Skins[(int)node.Skin];

                if (node.Mesh != null)
                {
                    gmesh = model.Meshes[(int)node.Mesh];
                    // if(gskin==null)
                    //     nmesh.content = ConvertMesh(gmesh);
                    // else
                    //     nmesh.content = ConvertSKinMesh(gskin,gmesh);

                    if (gskin != null)
                        meshes[(int)node.Mesh] = converter.ConvertSKinMesh(gskin, gmesh);
                    nmesh.content = meshes[(int)node.Mesh];
                }
                Matrix4 m = Matrix4.Zero;
                for (int x = 0; x < 4; x++)
                    for (int y = 0; y < 4; y++)
                    {
                        m[x, y] = node.Matrix[x * 4 + y];
                    }
                nmesh.related = m;
                if (node.Children == null)
                    continue;
                foreach (var c in node.Children)
                {
                    var childNode = model.Nodes[c];
                    nmesh.children.Add(CreateNodes(s, childNode));
                }
            }

            return nmesh;
        }

        public void ConvertMaterialsToIBL()
        {
            for (int i = 0; i < materials.Count; i++)
            {
                PBRMaterial mtl = materials[i] as PBRMaterial;
                PBR_IBLMaterial ibl = MaterialFactory.CreatePBR_IBLMaterial();
                mtl.CopyTo(ibl);
                materials[i] = ibl;
            }
        }

        public void PrintGLTFModelInfo(glTFLoader.Schema.Gltf model)
        {
            Console.WriteLine("Scenes:{0}", model.Scenes?.Length);
            Console.WriteLine("Nodes:{0}", model.Nodes?.Length);
            Console.WriteLine("Cameras:{0}", model.Cameras?.Length);
            Console.WriteLine("Meshes:{0}", model.Meshes?.Length);
            Console.WriteLine();
            Console.WriteLine("Animations:{0}", model.Animations?.Length);
            Console.WriteLine("Skins:{0}", model.Skins?.Length);
            if (model.Skins != null)
            {
                for (int i = 0; i < model.Skins.Length; i++)
                    Console.WriteLine("----skin[{0}],joints:{1}", i, model.Skins[i].Joints.Length);
            }
            Console.WriteLine();
            Console.WriteLine("Materials:{0}", model.Materials?.Length);
            Console.WriteLine("Textures:{0}", model.Textures?.Length);
            Console.WriteLine("Images:{0}", model.Images?.Length);
            Console.WriteLine("Samplers:{0}", model.Samplers?.Length);
            Console.WriteLine();
            Console.WriteLine("Accessors:{0}", model.Accessors?.Length);
            Console.WriteLine("BufferViews:{0}", model.BufferViews?.Length);
            Console.WriteLine("Buffers:{0}", model.Buffers?.Length);
            Console.WriteLine();
        }

        public void PrintInfo()
        {
            Console.WriteLine("Meshes:{0}", meshes.Count);
            Console.WriteLine("Materials:{0}", materials.Count);
            Console.WriteLine("Textures:{0}", textures.Count);
        }

        Dictionary<MeshPrimitive, Drawable> mpTable = new Dictionary<MeshPrimitive, Drawable>();
        //mesh{meshprimitive[]},drawble-meshprimitive
        public Drawable[] FindDrawablesByMesh(Mesh m)
        {
            Drawable[] drawables = new Drawable[m.primitives.Length];
            for (int i = 0; i < m.primitives.Length; i++)
            {
                drawables[i] = GetDrawableByMeshPrimitive(m.primitives[i]);
            }
            return drawables;
        }

        public Drawable GetDrawableByMeshPrimitive(MeshPrimitive mp)
        {
            return mpTable[mp];
        }

        public List<Drawable> CreateDrawables()
        {
            List<Drawable> result = new List<Drawable>();
            mpTable.Clear();
            for (int j = 0; j < meshes.Count; j++)
            {
                Mesh m = meshes[j];
                var m0 = model.Meshes[j];
                for (int i = 0; i < m.primitives.Length; i++)
                {
                    var mtl = materials[(int)m0.Primitives[i].Material];
                    MaterialDrawable md = new MaterialDrawable(mtl);
                    md.Update(m.primitives[i]);
                    result.Add(md);
                    mpTable.TryAdd(m.primitives[i], md);
                }
            }
            return result;
        }


        // public void CreateScene()
        // {
        //     scene = new Scene();
        //     foreach(var s in model.Scenes)
        //     {
        //         var subScene = new Scene();
        //         scene.sub.Add(subScene);

        //         foreach(var ni in s.Nodes)
        //         {
        //             Node node = model.Nodes[ni];
        //             Vector3 translation = new Vector3(node.Translation[0],node.Translation[1],node.Translation[2]);
        //             Quaternion rotation = new Quaternion(node.Rotation[0],node.Rotation[1],node.Rotation[2],node.Rotation[3]);
        //             Vector3 scale = new Vector3(node.Translation[0],node.Translation[1],node.Translation[2]);

        //             Matrix4 TRS = Matrix4.CreateTranslation(translation) * 
        //             Matrix4.CreateFromQuaternion(rotation) * 
        //             Matrix4.CreateScale(scale);

        //             float weight = node.Weights[0];
        //             int mi = (int)node.Mesh;
        //             subScene.groupDrawable.AddChild(0,subScene.NextID(),new Node<Matrix4, Drawable>(TRS,drawables[mi]));
        //         }
        //     }
        // }

        void CreateSubScene(Drawable[] drawables, Scene s)
        {

        }

    }
}
