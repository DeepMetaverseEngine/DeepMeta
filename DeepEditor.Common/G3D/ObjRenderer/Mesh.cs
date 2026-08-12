using DeepCore.GUI.Data;
using DeepEditor.Common.G3D;
using OpenTK; using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace G3D.ObjRenderer
{
    public class Mesh
    {
        private readonly Vector4[] vertices;
        private readonly Vector3[] textureVertices;
        private readonly Vector3[] normals;
        private readonly uint[] vertexIndices;
        private readonly uint[] textureIndices;
        private readonly uint[] normalIndices;

        public Vector4[] Vertices { get => vertices; }
        public Vector3[] TextureVertices { get => textureVertices; }
        public Vector3[] Normals { get => normals; }
        public uint[] VertexIndices { get => vertexIndices; }
        public uint[] TextureIndices { get => textureIndices; }
        public uint[] NormalIndices { get => normalIndices; }
        public Color4 TintColor { get; set; } = new Color4(0.5f, 1f, 1f, 1f);
        public Matrix4 Transform { get; set; } = Matrix4.Identity;
        public PrimitiveType PrimitiveType { get; set; } = PrimitiveType.Triangles;
        public bool DrawTriangleLines { get; set; } = true;
        public Mesh(
            List<Vector4> vertices,
            List<Vector3> textureVertices,
            List<Vector3> normals,
            List<uint> vertexIndices,
            List<uint> textureIndices,
            List<uint> normalIndices)
        {
            this.vertices = vertices.ToArray();
            this.textureVertices = textureVertices.ToArray();
            this.normals = normals.ToArray();
            this.vertexIndices = vertexIndices.ToArray();
            this.textureIndices = textureIndices.ToArray();
            this.normalIndices = normalIndices.ToArray();
        }

        public void Draw()
        {
            var tm = Transform;
            GL.PushMatrix();
            GL.MultMatrix(ref tm);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.Color4(TintColor);
            GL.VertexPointer(4, VertexPointerType.Float, 0, vertices);
            if (PrimitiveType == PrimitiveType.Polygon)
            {
                var span = new Span<uint>(vertexIndices);
                for (int i = 0; i < vertexIndices.Length; i += 3)
                {
                    GL.DrawElements(PrimitiveType.LineLoop, 3, DrawElementsType.UnsignedInt, span.Slice(i, 3).ToArray());
                }
            }
            else
            {
                GL.DrawElements(PrimitiveType, vertexIndices.Length, DrawElementsType.UnsignedInt, vertexIndices);
            }
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.PopMatrix();

        }

    }
}
