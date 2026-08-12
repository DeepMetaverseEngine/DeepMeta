using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DeepCore;
using DeepCore.Reflection;
using DeepCore.SharpZipLib;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G3D;
using OpenTK; using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace G3D.ObjRenderer
{


    public class ObjLoaderConfig
    {
        [Desc("渲染类型", "GL")]
        public PrimitiveType PrimitiveType = PrimitiveType.Triangles;

        [Desc("矩阵：缩放X", "GL")]
        public float ScaleX = 1f;
        [Desc("矩阵：缩放Y", "GL")]
        public float ScaleY = 1f;
        [Desc("矩阵：缩放Z", "GL")]
        public float ScaleZ = 1f;

        [Desc("矩阵：旋转X(Angle360)", "GL")]
        public float RotationX = 0f;
        [Desc("矩阵：旋转Y(Angle360)", "GL")]
        public float RotationY = 0f;
        [Desc("矩阵：旋转Z(Angle360)", "GL")]
        public float RotationZ = 0f;

        [Desc("矩阵：位移X", "GL")]
        public float TranslationX = 0f;
        [Desc("矩阵：位移Y", "GL")]
        public float TranslationY = 0f;
        [Desc("矩阵：位移Z", "GL")]
        public float TranslationZ = 0f;

        [Desc("可寻路颜色", "体素")]
        [Int32Color]
        public uint Color = 0xFF80FFFF;
        [Desc("场景总宽", "体素")]
        public float TerrainWidth = 0;
        [Desc("场景总高", "体素")]
        public float TerrainHeight = 0;
        [Desc("场景寻路权重", "体素")]
        public float PathWeight = 100;

        public Vector3 Scale { get => new Vector3(ScaleX, ScaleY, ScaleZ); }
        public Vector3 Rotation { get => new Vector3(CMath.AngleToRadian(RotationX), CMath.AngleToRadian(RotationY), CMath.AngleToRadian(RotationZ)); }
        public Vector3 Translation { get => new Vector3(TranslationX, TranslationY, TranslationZ); }
        public Matrix4 Transform
        {
            get
            {
                return Matrix4.CreateTranslation(Translation) * Matrix4.CreateScale(Scale) *
                      (Rotation.X == 0 ? Matrix4.Identity : Matrix4.CreateRotationX(Rotation.X)) *
                      (Rotation.Y == 0 ? Matrix4.Identity : Matrix4.CreateRotationY(Rotation.Y)) *
                      (Rotation.Z == 0 ? Matrix4.Identity : Matrix4.CreateRotationZ(Rotation.Z));
            }
        }
        public Color4 TintColor
        {
            get => GLUtils.Argb2Color4(Color);
        }
    }

    public static class ObjLoader
    {
        public static Mesh Load(string path, ObjLoaderConfig config = null)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to open \"" + path + "\", does not exist.");
            }
            using (var stream = File.OpenRead(path))
            {
                return Load(stream, config);
            }
        }

        public static Mesh Load(Stream stream, ObjLoaderConfig config = null)
        {
            List<Vector4> vertices = new List<Vector4>();
            List<Vector3> textureVertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<uint> vertexIndices = new List<uint>();
            List<uint> textureIndices = new List<uint>();
            List<uint> normalIndices = new List<uint>();

            using (StreamReader streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    List<string> words = new List<string>(streamReader.ReadLine().ToLower().Split(' '));
                    words.RemoveAll(s => s == string.Empty);

                    if (words.Count == 0)
                        continue;

                    string type = words[0];
                    words.RemoveAt(0);

                    switch (type)
                    {
                        // vertex
                        case "v":
                            vertices.Add(new Vector4(
                                Parser.ParseFloat(words[0]),
                                Parser.ParseFloat(words[1]),
                                Parser.ParseFloat(words[2]),
                                words.Count < 4 ? 1 : Parser.ParseFloat(words[3])));
                            break;

                        case "vt":
                            textureVertices.Add(new Vector3(
                                Parser.ParseFloat(words[0]),
                                Parser.ParseFloat(words[1]),
                                words.Count < 3 ? 0 : Parser.ParseFloat(words[2])));
                            break;

                        case "vn":
                            normals.Add(new Vector3(
                                Parser.ParseFloat(words[0]),
                                Parser.ParseFloat(words[1]),
                                Parser.ParseFloat(words[2])));
                            break;

                        // face
                        case "f":
                            foreach (string w in words)
                            {
                                if (w.Length == 0)
                                    continue;

                                string[] comps = w.Split('/');

                                // subtract 1: indices start from 1, not 0
                                vertexIndices.Add(Parser.ParseUInt(comps[0]) - 1);

                                if (comps.Length > 1 && comps[1].Length != 0)
                                    textureIndices.Add(Parser.ParseUInt(comps[1]) - 1);

                                if (comps.Length > 2)
                                    normalIndices.Add(Parser.ParseUInt(comps[2]) - 1);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }

            var ret = new Mesh(vertices, textureVertices, normals, vertexIndices, textureIndices, normalIndices);
            if (config != null)
            {
                ret.PrimitiveType = config.PrimitiveType;
                ret.TintColor = config.TintColor;
                ret.Transform = config.Transform;
            }
            return ret;
        }

        public static bool LoadMeshDialog(IWin32Window window, string initPath, ref ObjLoaderConfig cfg, out string path, out Mesh mesh)
        {
            var fd = new OpenFileDialog();
            fd.Multiselect = false;
            if (System.IO.Directory.Exists(initPath))
            {
                fd.InitialDirectory = initPath;
            }
            if (System.IO.File.Exists(initPath))
            {
                fd.InitialDirectory = System.IO.Path.GetDirectoryName(initPath);
            }
            if (fd.ShowDialog() == DialogResult.OK)
            {
                path = fd.FileName;
                var pdialog = new G2DDataDialog.G2DObjectDialog<ObjLoaderConfig>(cfg);
                if (pdialog.ShowDialog(window) == DialogResult.OK)
                {
                    cfg = pdialog.SelectedObject;
                    try
                    {
                        if (path.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                        {
                            var objstream = ZipUtil.LoadZipEntry(path, e => e.Name.EndsWith(".obj", StringComparison.OrdinalIgnoreCase));
                            if (objstream != null)
                            {
                                using (objstream)
                                {
                                    mesh = ObjLoader.Load(objstream, cfg);
                                    return true;
                                }
                            }
                        }
                        mesh = ObjLoader.Load(path, cfg);
                        return true;
                    }
                    catch (Exception err)
                    {
                        err.ShowMessageBox();
                    }
                }
            }
            path = null;
            mesh = null;
            return false;
        }
    }
}
