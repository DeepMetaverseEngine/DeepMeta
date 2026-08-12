using DeepCore;
using DeepEditor.Common.G3D;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Drawing;
using static System.Windows.Forms.DataFormats;

namespace DeepEditor.Common.G3D
{
    public abstract class GLDrawingObject : Disposable
    {
        public const int VAO_POINTS_LOCATION = 0;
        public const int VAO_COLORS_LOCATION = 1;
        public const int VAO_NORMAL_LOCATION = 2;
        public const int VAO_UV_LOCATION = 3;

        protected readonly PrimitiveType objectType;
        protected Shader _shader;
        protected Shader _ower_shader;
        public GLDrawingObject(PrimitiveType type)
        {
            this.objectType = type;
        }
        protected override void Disposing()
        {
            _ower_shader?.Dispose();
        }
        public virtual Shader SetShader(Shader shader)
        {
            this._shader = shader;
            return shader;
        }
        public virtual Shader CreateDefaultShader()
        {
            return null;
        }
        public abstract void Draw();
        public abstract void Draw(PaintEventArgs3D e);
        public abstract void Flush();
    }

    public struct Vertex
    {
        public Vector3 Position;
        public Color4 Color;
        public Vector3 Normal;
        public Vector2 UV;
    }
    public class VertexArrayObject<T> : GLDrawingObject where T : unmanaged
    {
        protected List<T> vertices = new List<T>();
        private int VAO = 0;
        private int VBO = 0;
        private int arrayLength = 0;
        public int ArrayLength { get => arrayLength; }
        public VertexArrayObject(PrimitiveType type) : base(type)
        {
        }
        protected override void Disposing()
        {
            CleanVAO();
            base.Disposing();
        }
        public override void Flush()
        {
            vertices.Clear();
            CleanVAO();
        }
        public virtual void Add(T vec)
        {
            vertices.Add(vec);
        }
        public virtual void AddRange(IEnumerable<T> vecs)
        {
            vertices.AddRange(vecs);
        }
        sealed public override void Draw()
        {
            BeginDraw();
            if (VAO > 0) OnDraw();
        }
        sealed public override void Draw(PaintEventArgs3D e)
        {
            BeginDraw();
            if (VAO > 0) OnDraw(e);
        }
        protected virtual void CleanVAO()
        {
            if (VAO > 0 || VBO > 0)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindVertexArray(0);
                if (VAO > 0) GL.DeleteVertexArray(VAO);
                if (VBO > 0) GL.DeleteBuffer(VBO);
                VAO = 0;
                VBO = 0;
            }
        }
        protected virtual void BeginDraw()
        {
            if (vertices.Count > 0)
            {
                this.arrayLength = vertices.Count;
                try
                {
                    if (this._shader == null)
                    {
                        this._shader = this._ower_shader = CreateDefaultShader();
                    }
                    this.CleanVAO();
                    this.VAO = GL.GenVertexArray();
                    GL.BindVertexArray(VAO);
                    var toffset = 0;
                    this.OnInitVAO(ref toffset);
                    GL.BindVertexArray(0);
                }
                finally
                {
                    vertices.Clear();
                    vertices.TrimExcess();
                }
            }
        }
        unsafe protected virtual void OnInitVAO(ref int toffset)
        {
            var tsize = sizeof(T);
            this.VBO = GL.GenBuffer();
            // 2. 绑定 VBO 并传输数据
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            var arrayPTR = vertices.ToArray();
            GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * tsize, arrayPTR, BufferUsageHint.StaticDraw);
            if (tsize >= toffset + sizeof(Vector3))
            {
                // 3. 设置位置属性 (Location = 0)
                GL.VertexAttribPointer(VAO_POINTS_LOCATION, sizeof(Vector3) / sizeof(float), VertexAttribPointerType.Float, false, sizeof(T), toffset);
                GL.EnableVertexAttribArray(VAO_POINTS_LOCATION);
                toffset += sizeof(Vector3);
            }
            if (tsize >= toffset + sizeof(Color4))
            {
                // 4. 设置颜色属性 (Location = 1)
                GL.VertexAttribPointer(VAO_COLORS_LOCATION, sizeof(Color4) / sizeof(float), VertexAttribPointerType.Float, false, sizeof(T), toffset);
                GL.EnableVertexAttribArray(VAO_COLORS_LOCATION);
                toffset += sizeof(Color4);
            }
            if (tsize >= toffset + sizeof(Vector3))
            {
                // 4. 设置 Normal 属性 (Location = 2)
                GL.VertexAttribPointer(VAO_NORMAL_LOCATION, sizeof(Vector3) / sizeof(float), VertexAttribPointerType.Float, false, sizeof(T), toffset);
                GL.EnableVertexAttribArray(VAO_NORMAL_LOCATION);
                toffset += sizeof(Vector3);
            }
            if (tsize >= toffset + sizeof(Vector2))
            {
                // 5. 设置 UV 属性 (Location = 3)
                GL.VertexAttribPointer(VAO_UV_LOCATION, sizeof(Vector2) / sizeof(float), VertexAttribPointerType.Float, false, sizeof(T), toffset);
                GL.EnableVertexAttribArray(VAO_UV_LOCATION);
                toffset += sizeof(Vector2);
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        protected virtual void OnDraw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindVertexArray(VAO);
            GL.DrawArrays(objectType, 0, arrayLength);
            GL.BindVertexArray(0);
        }
        protected virtual void OnDraw(PaintEventArgs3D e)
        {
            if (_shader != null)
            {
                try
                {
                    _shader.Use(e);
                    _shader.SetMatrix4("model", e.ModelMatrix);
                    _shader.SetMatrix4("view", e.Camera.ViewMatrix);
                    _shader.SetMatrix4("projection", e.Camera.ProjectionMatrix);
                    this.OnDraw();
                }
                finally
                {
                    _shader.EndUse();
                }
            }
            else
            {
                OnDraw();
            }
        }
    }



    //------------------------------------------------------------------------------------------------
    #region OLD

    public abstract class VertexBufferObject : GLDrawingObject
    {
        public bool EnableColor = false;
        public bool EnableNormal = false;
        public bool EnableUV = false;
        protected int vertexArrayObject = 0;
        protected int vertexBufferObject_Vertices = 0;
        protected int vertexBufferObject_Colors = 0;
        protected int vertexBufferObject_Normals = 0;
        protected int vertexBufferObject_UV = 0;
        protected int elementBufferObject = 0;
        private int arrayLength = 0;
        public int ArrayLength { get => arrayLength; }
        public VertexBufferObject(PrimitiveType type) : base(type)
        {
        }
        protected override void Disposing()
        {
            Flush();
            base.Disposing();
        }
        public override void Flush()
        {
            this.OnClean();
            if (vertexArrayObject > 0) GL.DeleteVertexArray(vertexArrayObject);
            if (vertexBufferObject_Vertices > 0) GL.DeleteBuffer(vertexBufferObject_Vertices);
            if (vertexBufferObject_Colors > 0) GL.DeleteBuffer(vertexBufferObject_Colors);
            if (vertexBufferObject_Normals > 0) GL.DeleteBuffer(vertexBufferObject_Normals);
            if (vertexBufferObject_UV > 0) GL.DeleteBuffer(vertexBufferObject_UV);
            if (elementBufferObject > 0) GL.DeleteBuffer(elementBufferObject);
            this.vertexArrayObject = 0;
            this.vertexBufferObject_Vertices = 0;
            this.vertexBufferObject_Colors = 0;
            this.vertexBufferObject_Normals = 0;
            this.vertexBufferObject_UV = 0;
            this.elementBufferObject = 0;
            this.arrayLength = 0;
            this.OnFlush();
        }
        private void InternalInit()
        {
            if (OnCheckDirty(out var len))
            {
                this.arrayLength = len;
                if (this._shader == null)
                {
                    this._shader = this._ower_shader = CreateDefaultShader();
                }
                OnInit(arrayLength);
                OnClean();
            }
        }
        sealed public override void Draw()
        {
            InternalInit();
            if (ArrayLength > 0)
            {
                OnDraw();
            }
        }
        sealed public override void Draw(PaintEventArgs3D e)
        {
            InternalInit();
            if (ArrayLength > 0)
            {
                OnDraw(e);
            }
        }
        protected abstract void OnFlush();
        protected abstract void OnClean();
        protected abstract bool OnCheckDirty(out int arrayLength);
        protected abstract void OnInit(int arrayLength);
        protected abstract void OnDraw();
        protected abstract void OnDraw(PaintEventArgs3D e);
    }

    public class VertexArrayObject : VertexBufferObject
    {
        protected Color4 color;
        protected Vector3 normal;
        protected Vector2 coord;
        protected List<Vector3> vertices = new List<Vector3>();
        protected List<Vector3> normals = new List<Vector3>();
        protected List<Vector2> coords = new List<Vector2>();
        protected List<Color4> colors = new List<Color4>();

        public VertexArrayObject(PrimitiveType type, Color4 color) : base(type)
        {
            this.color = color;
        }
        public VertexArrayObject(PrimitiveType type) : base(type)
        {
            this.color = Color4.White;
        }
        public void SetColor(Color4 color)
        {
            this.color = color;
        }
        public void SetNormal(Vector3 normal)
        {
            this.normal = normal;
        }
        public void SetTextureCoords(Vector2 coord)
        {
            this.coord = coord;
        }
        public void Add(float x, float y, float z)
        {
            this.Add(new Vector3(x, y, z));
        }
        public virtual void Add(Vector3 vec)
        {
            vertices.Add(vec);
            if (EnableColor) colors.Add(color);
            if (EnableNormal) normals.Add(normal);
            if (EnableUV) coords.Add(coord);
        }
        protected override void OnFlush()
        {
        }
        protected override bool OnCheckDirty(out int arrayLength)
        {
            arrayLength = vertices.Count;
            return vertices.Count > 0;
        }
        protected override void OnInit(int arrayLength)
        {
            this.vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            {
                this.vertexBufferObject_Vertices = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_Vertices);
                GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * 3 * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(VAO_POINTS_LOCATION, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(VAO_POINTS_LOCATION);
                if (EnableColor)
                {
                    this.vertexBufferObject_Colors = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_Colors);
                    GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * 4 * sizeof(float), colors.ToArray(), BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(VAO_COLORS_LOCATION, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
                    GL.EnableVertexAttribArray(VAO_COLORS_LOCATION);
                }
                if (EnableNormal)
                {
                    this.vertexBufferObject_Normals = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_Normals);
                    GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * 3 * sizeof(float), normals.ToArray(), BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(VAO_NORMAL_LOCATION, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                    GL.EnableVertexAttribArray(VAO_NORMAL_LOCATION);
                }
                if (EnableUV)
                {
                    this.vertexBufferObject_UV = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_UV);
                    GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * 2 * sizeof(float), coords.ToArray(), BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(VAO_UV_LOCATION, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
                    GL.EnableVertexAttribArray(VAO_UV_LOCATION);
                }
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
            GL.BindVertexArray(0);
        }

        protected override void OnClean()
        {
            this.vertices.Clear();
            this.vertices.TrimExcess();
            this.colors.Clear();
            this.colors.TrimExcess();
            this.normals.Clear();
            this.normals.TrimExcess();
            this.coords.Clear();
            this.coords.TrimExcess();
        }
        protected override void OnDraw()
        {
            GL.Color4(color);
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArrays(objectType, 0, ArrayLength);
            GL.BindVertexArray(0);
        }
        protected override void OnDraw(PaintEventArgs3D e)
        {
            if (_shader != null)
            {
                try
                {
                    _shader.Use(e);
                    _shader.SetMatrix4("model", e.ModelMatrix);
                    _shader.SetMatrix4("view", e.Camera.ViewMatrix);
                    _shader.SetMatrix4("projection", e.Camera.ProjectionMatrix);
                    GL.BindVertexArray(vertexArrayObject);
                    GL.DrawArrays(objectType, 0, ArrayLength);
                    GL.BindVertexArray(0);
                }
                finally
                {
                    _shader.EndUse();
                }
            }
            else
            {
                OnDraw();
            }
        }
    }

    public class VertexElementObject : VertexBufferObject
    {
        protected Vector3[] _vertices;
        protected Vector3[] _normals;
        protected Vector2[] _uv;
        protected Color4[] _colors;
        protected uint[] _indices;
        private int _elementCount;
        public VertexElementObject(
            PrimitiveType type,
            Vector3[] vertices,
            Vector3[] normals,
            Vector2[] uv,
            Color4[] colors,
            uint[] indices) : base(type)
        {
            SetMesh(vertices, normals, uv, colors, indices);
        }
        public VertexElementObject(PrimitiveType type) : base(type)
        {
        }
        public void SetMesh(
            Vector3[] vertices,
            Vector3[] normals,
            Vector2[] uv,
            Color4[] colors,
            uint[] indices)
        {
            this._vertices = vertices;
            this._normals = normals;
            this._uv = uv;
            this._colors = colors;
            this._indices = indices;
        }
        protected override void OnFlush()
        {
            _elementCount = 0;
        }
        protected override bool OnCheckDirty(out int arrayLength)
        {
            if (_vertices != null)
            {
                _elementCount = _indices.Length;
                arrayLength = _vertices.Length;
                return true;
            }
            arrayLength = 0;
            return false;
        }
        protected override void OnInit(int arrayLength)
        {
            this.vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            {
                this.vertexBufferObject_Vertices = GL.GenBuffer();
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_Vertices);
                    GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * 3 * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(VAO_POINTS_LOCATION, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                    GL.EnableVertexAttribArray(VAO_POINTS_LOCATION);
                }
                if (EnableColor)
                {
                    this.vertexBufferObject_Colors = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_Colors);
                    GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * 4 * sizeof(float), _colors, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(VAO_COLORS_LOCATION, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
                    GL.EnableVertexAttribArray(VAO_COLORS_LOCATION);
                }
                if (EnableNormal)
                {
                    this.vertexBufferObject_Normals = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_Normals);
                    GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * 3 * sizeof(float), _normals, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(VAO_NORMAL_LOCATION, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                    GL.EnableVertexAttribArray(VAO_NORMAL_LOCATION);
                }
                if (EnableUV)
                {
                    this.vertexBufferObject_UV = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_UV);
                    GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * 2 * sizeof(float), _uv, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(VAO_UV_LOCATION, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
                    GL.EnableVertexAttribArray(VAO_UV_LOCATION);
                }
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
            {
                this.elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }
            GL.BindVertexArray(0);
        }
        protected override void OnClean()
        {
            _vertices = null;
            _normals = null;
            _uv = null;
            _colors = null;
            _indices = null;
        }
        protected override void OnDraw()
        {
            GL.BindVertexArray(vertexArrayObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.DrawElements(objectType, _elementCount, DrawElementsType.UnsignedInt, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
        protected override void OnDraw(PaintEventArgs3D e)
        {
            if (_shader != null)
            {
                try
                {
                    _shader.Use(e);
                    _shader.SetMatrix4("model", e.ModelMatrix);
                    _shader.SetMatrix4("view", e.Camera.ViewMatrix);
                    _shader.SetMatrix4("projection", e.Camera.ProjectionMatrix);
                    GL.BindVertexArray(vertexArrayObject);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
                    GL.DrawElements(objectType, _elementCount, DrawElementsType.UnsignedInt, 0);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    GL.BindVertexArray(0);
                }
                finally
                {
                    _shader.EndUse();
                }
            }
            else
            {
                OnDraw();
            }
        }

    }

    #endregion
    //------------------------------------------------------------------------------------------------
}


