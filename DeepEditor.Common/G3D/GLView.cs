using DeepCore;
using DeepCore.GUI.Data;
using DeepCore.GUI.Display;
using DeepCore.Threading;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static DeepCore.Colors;
using static System.Windows.Forms.DataFormats;

namespace DeepEditor.Common.G3D
{
    public class GLView : Disposable
    {
        private bool disposed = false;
        private DateTime lastRenderTime = DateTime.Now;
        private Exception last_Error;
        private ActionQueue<PaintEventArgs> paintQueue = new ActionQueue<PaintEventArgs>();
        private HashMap<CameraType, CameraControl> cameraStack = new HashMap<CameraType, CameraControl>();
        private GLBitmapTexture2D renderHudTexture;
        protected GLControl glControl { get; private set; }
        protected System.Windows.Forms.Timer timer { get; private set; }
        public ShaderLibrary Shaders { get; private set; } = new ShaderLibrary();
        public CameraControl Camera { get; private set; }
        public static System.Drawing.Color DefaultBackColor { get; set; } = System.Drawing.Color.FromArgb(0xff, 0x40, 0x40, 0x40);
        public Color4 BackColor { get; set; } = new Color4(DefaultBackColor.R, DefaultBackColor.G, DefaultBackColor.B, DefaultBackColor.A);
        public float LastIntervalMS { get; private set; }
        public double TotalTimeMS { get; private set; }
        public double TotalTimeSEC { get; private set; }
        public int CurrentFPS { get => (int)(1000 / LastIntervalMS); }
        public int FPS { get { return 1000 / timer.Interval; } }
        public int FixedFPS { get; set; }
        public GLControl GLControl { get => glControl; }
        public int Width { get => glControl.Width; }
        public int Height { get => glControl.Height; }
        public void SetFPS(int value)
        {
            this.timer.Interval = 1000 / value;
            this.FixedFPS = value;
        }

        public GLView(GLControl control, Timer timer)
        {
            GLFonts.Instance.ToString();
            this.glControl = control;
            this.glControl.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.timer = timer;
            {
                this.glControl.Load += GlControl_Load;
                this.glControl.Resize += GlControl_Resize;
                this.glControl.Paint += GlControl_Paint;
                this.glControl.HandleDestroyed += GlControl_Destroyed;
                this.glControl.KeyDown += GLControl_KeyDown;
                this.glControl.KeyUp += GLControl_KeyUp;
                this.glControl.MouseDown += GLControl_MouseDown;
                this.glControl.MouseUp += GLControl_MouseUp;
                this.glControl.MouseMove += GlControl_MouseMove;
                this.glControl.MouseWheel += GlControl_MouseWheel;
                this.timer.Tick += Timer_Tick;
            }
            this.RootObject = new GLViewObject3D.GLViewRootObject3D(this);
            this.SetCameraControl(new FreeCameraControl2D());
            this.SetCameraControl(new FreeCameraControl3D());
        }
        protected override void Disposing()
        {
            if (!disposed)
            {
                disposed = true;
                event_OnUpdate = null;
                event_OnBeginRender = null;
                event_OnRender = null;
                event_OnEndRender = null;
                event_OnRenderHUD = null;
                event_OnPaintHUDGDI = null;
                {
                    this.glControl.Load -= GlControl_Load;
                    this.glControl.Resize -= GlControl_Resize;
                    this.glControl.Paint -= GlControl_Paint;
                    this.glControl.HandleDestroyed -= GlControl_Destroyed;
                    this.glControl.KeyDown -= GLControl_KeyDown;
                    this.glControl.KeyUp -= GLControl_KeyUp;
                    this.glControl.MouseDown -= GLControl_MouseDown;
                    this.glControl.MouseUp -= GLControl_MouseUp;
                    this.glControl.MouseMove -= GlControl_MouseMove;
                    this.glControl.MouseWheel -= GlControl_MouseWheel;
                    this.timer.Tick -= Timer_Tick;
                }
                try
                {
                    glControl.MakeCurrent();
                    try
                    {
                        event_Destory?.Invoke(this);
                    }
                    catch { }
                }
                catch { }
                finally
                {
                    this.ClearObjects(true);
                    RootObject.Dispose();
                    RootObject = null;
                    paintQueue.Dispose();
                    Shaders.Dispose();
                    Shaders = null;
                    renderHudTexture?.Dispose();
                    renderHudTexture = null;
                    paintQueue.Dispose();
                    paintQueue = null;
                    foreach (var cam in cameraStack.Values)
                    {
                        cam.Dispose();
                    }
                    cameraStack.Clear();
                    cameraStack = null;
                    Camera.Dispose();
                    Camera = null;
                    this.glControl = null;
                    this.timer = null;
                    this.Shaders = null;
                }
            }
        }
        public void PostPaintTask(Action<PaintEventArgs> drawAction)
        {
            paintQueue.Enqueue(drawAction);
        }
        public void SetCameraControl(CameraControl c)
        {
            this.Camera = c;
            this.cameraStack.Put(c.CamType, c);
            this.OnSetCameraControl(c);
        }
        public CameraControl SetCameraControlWithType(CameraType c)
        {
            if (cameraStack.TryGetValue(c, out var cam))
            {
                this.SetCameraControl(cam);
                return cam;
            }
            return null;
        }

        protected virtual void OnCreateCameraControl(CameraControl c) { }
        protected virtual void OnSetCameraControl(CameraControl c) { }


        protected virtual void GlControl_Load(object sender, EventArgs e)
        {
            GlControl_Resize(sender, e);
        }
        protected virtual void GlControl_Destroyed(object sender, EventArgs e)
        {
            ((IDisposable)this).Dispose();
        }
        protected virtual void Timer_Tick(object sender, EventArgs e)
        {
            //this.glControl.MakeCurrent();
            this.glControl.Invalidate();
        }
        private void GLControl_KeyUp(object sender, KeyEventArgs e)
        {
            this.Camera.OnKeyUp(glControl, e);
        }
        private void GLControl_KeyDown(object sender, KeyEventArgs e)
        {
            this.Camera.OnKeyDown(glControl, e);
        }
        protected virtual void GLControl_MouseUp(object sender, MouseEventArgs e)
        {
            this.Camera.OnMouseUp(glControl, e);
        }
        protected virtual void GLControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.glControl.Focus();
            this.Camera.OnMouseDown(glControl, e);
        }
        protected virtual void GlControl_MouseMove(object sender, MouseEventArgs e)
        {
            this.Camera.OnMouseMove(glControl, e);
        }
        protected virtual void GlControl_MouseWheel(object sender, MouseEventArgs e)
        {
            this.Camera.OnMouseWheel(glControl, e);
        }
        protected virtual void GlControl_Resize(object sender, EventArgs e)
        {
            try
            {
                //glControl.MakeCurrent();
                if (glControl.ClientSize.Height == 0)
                    glControl.ClientSize = new System.Drawing.Size(glControl.ClientSize.Width, 1);
            }
            catch (Exception err) { }
        }
        protected virtual void DrawWorld(PaintEventArgs3D args)
        {
            this.event_OnBeginRender?.Invoke(this, args);
            this.RenderObjects(args);
            this.event_OnRender?.Invoke(this, args);
            this.event_OnEndRender?.Invoke(this, args);
        }
        protected virtual void DrawHUD(PaintEventArgs3D e)
        {
            this.RenderObjectsHUD(e);
            this.event_OnRenderHUD?.Invoke(this, e);
        }
        protected virtual void DrawHUDGDI(PaintEventArgs e)
        {
            event_OnPaintHUDGDI.Invoke(this, e);
        }
        private bool inited = false;
        protected virtual void OnInitGL()
        {
            this.event_Init?.Invoke(this);
        }
        protected virtual void GlControl_Paint(object sender, PaintEventArgs e)
        {
            var now = DateTime.Now;
            var elapsed = now - lastRenderTime;
            this.lastRenderTime = now;
            try
            {
                glControl.MakeCurrent();
                if (inited == false)
                {
                    inited = true;
                    this.OnInitGL();
                }
                this.paintQueue.ProcessMessages(e);
                this.UpdateObjects();
                this.Camera.Update(glControl, elapsed);
                this.event_OnUpdate?.Invoke(this, elapsed);
                this.LastIntervalMS = (float)elapsed.TotalMilliseconds;
                this.TotalTimeMS += (double)elapsed.TotalMilliseconds;
                this.TotalTimeSEC = TotalTimeMS / 1000f;
                if (glControl.Visible)
                {
                    this.Camera.ResetViewPort(this.glControl.ClientSize);
                    GL.ClearColor(BackColor);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.Enable(EnableCap.Blend);
                    GL.Enable(EnableCap.AlphaTest);
                    GL.BlendColor(1f, 1f, 1f, 1f);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    try
                    {
                        this.Camera.BeginLookAt(this.GLControl, elapsed);
                        using (var args = new PaintEventArgs3D(e, elapsed, this.Camera))
                        {
                            try
                            {
                                GL.Enable(EnableCap.DepthTest);
                                this.DrawWorld(args);
                            }
                            finally
                            {
                                this.Camera.EndLookAt();
                            }

                            GL.MatrixMode(MatrixMode.Projection);
                            GL.PushMatrix();
                            GL.LoadIdentity();
                            GL.Ortho(0, this.glControl.Width, 0, this.glControl.Height, -1, 1);
                            GL.MatrixMode(MatrixMode.Modelview);
                            GL.PushMatrix();
                            GL.LoadIdentity();
                            try
                            {
                                GL.Disable(EnableCap.DepthTest);
                                GL.Translate(new Vector3(0f, glControl.Height, 0f));
                                GL.Scale(new Vector3(1f, -1f, 1f));
                                try
                                {
                                    this.DrawHUD(args);
                                }
                                finally
                                {
                                }
                                if (event_OnPaintHUDGDI != null)
                                {
                                    try
                                    {
                                        if (this.renderHudTexture == null)
                                            this.renderHudTexture = new GLBitmapTexture2D(glControl.Size);
                                    }
                                    catch { renderHudTexture = null; }
                                    if (this.renderHudTexture != null)
                                    {
                                        var g = renderHudTexture.BeginGraphics(glControl.Size);
                                        var gs = g.Save();
                                        try
                                        {
                                            this.DrawHUDGDI(new PaintEventArgs(g, new System.Drawing.Rectangle(0, 0, Width, Height)));
                                        }
                                        finally
                                        {
                                            g.Restore(gs);
                                        }
                                        try
                                        {
                                            renderHudTexture.Flush();
                                            renderHudTexture.DrawQuards2D(args, 0, 0);
                                        }
                                        catch { renderHudTexture = null; }
                                    }
                                }
                            }
                            finally
                            {
                                GL.MatrixMode(MatrixMode.Modelview);
                                GL.PopMatrix();
                                GL.MatrixMode(MatrixMode.Projection);
                                GL.PopMatrix();
                            }
                        }
                    }
                    finally
                    {
                        GL.Flush();
                        this.glControl.SwapBuffers();
                    }
                }
            }
            catch (Exception err)
            {
                if (last_Error != null)
                {
                    if (err.StackTrace == last_Error.StackTrace && err.Message == last_Error.Message)
                    {
                        return;
                    }
                }
                last_Error = err;
                err.PrintStackTrace();
                err.ShowMessageBox();
            }
        }


        //-----------------------------------------------------------------------------------------------------------------
        #region Events
        private GLViewHandler event_Init;
        private GLViewHandler event_Destory;
        private UpdateHandler event_OnUpdate;
        private RenderHandler event_OnBeginRender;
        private RenderHandler event_OnRender;
        private RenderHandler event_OnEndRender;
        private RenderHandler event_OnRenderHUD;
        private PaintEventHandler event_OnPaintHUDGDI;
        public event GLViewHandler OnInit
        {
            add { event_Init += value; }
            remove { event_Init -= value; }
        }
        public event GLViewHandler OnDestory
        {
            add { event_Destory += value; }
            remove { event_Destory -= value; }
        }
        public event UpdateHandler OnUpdate
        {
            add { event_OnUpdate += value; }
            remove { event_OnUpdate -= value; }
        }
        public event RenderHandler OnBeginRender
        {
            add { event_OnBeginRender += value; }
            remove { event_OnBeginRender -= value; }
        }
        public event RenderHandler OnRender
        {
            add { event_OnRender += value; }
            remove { event_OnRender -= value; }
        }
        public event RenderHandler OnEndRender
        {
            add { event_OnEndRender += value; }
            remove { event_OnEndRender -= value; }
        }
        public event RenderHandler OnRenderHUD
        {
            add { event_OnRenderHUD += value; }
            remove { event_OnRenderHUD -= value; }
        }
        public event PaintEventHandler OnPaintGDI
        {
            add { event_OnPaintHUDGDI += value; }
            remove { event_OnPaintHUDGDI -= value; }
        }
        public delegate void GLViewHandler(GLView sender);
        public delegate void RenderHandler(GLView sender, PaintEventArgs3D e);
        public delegate void UpdateHandler(GLView sender, TimeSpan interval);

        #endregion
        //-----------------------------------------------------------------------------------------------------------------

        #region Objects
        public int ObjectsCount { get => RootObject.ChildrenCount; }
        public GLViewObject3D RootObject { get; private set; }

        public GLViewObject3D ForEachObjects(BreakPredicate<GLViewObject3D> action)
        {
            return RootObject.ForEachChildren(action);
        }
        public T ForEachObjects<T>(BreakPredicate<T> action) where T : GLViewObject3D
        {
            return RootObject.ForEachChildren<T>(action);
        }
        public void ClearObjects(bool dispose = true)
        {
            this.RootObject.ClearChildren(dispose);
        }
        public void AddDisplayObject(GLViewObject3D obj)
        {
            RootObject.AddChild(obj);
        }
        public void RemoveDisplayObject(GLViewObject3D obj)
        {
            RootObject.RemoveChild(obj);
        }
        protected virtual void UpdateObjects()
        {
            RootObject.Update();
        }
        protected virtual void RenderObjects(PaintEventArgs3D e)
        {
            RootObject.Render(e);
        }
        protected virtual void RenderObjectsHUD(PaintEventArgs3D e)
        {
            RootObject.RenderHUD(e);
        }
        #endregion

        public System.Drawing.Bitmap TakeSnap()
        {
            int xs = glControl.Width;
            int ys = glControl.Height;
            var raw = new byte[xs * ys * 4];
            GL.ReadPixels(0, 0, xs, ys, PixelFormat.Rgba, PixelType.UnsignedByte, raw);
            GL.Finish();
            var ret = new System.Drawing.Bitmap(xs, ys);

            for (int x = 0; x < xs; x++)
            {
                for (int y = 0; y < ys; y++)
                {
                    var pi = (y * xs + x) * 4;
                    var r = raw[pi + 0];
                    var g = raw[pi + 1];
                    var b = raw[pi + 2];
                    var a = raw[pi + 3];
                    ret.SetPixel(x, ys - y - 1, System.Drawing.Color.FromArgb(a, r, g, b));
                }
            }
            return ret;
        }
    }

    public static class GLViewExt
    {
        public static void DrawDebugTextHUD(this System.Drawing.Graphics g, string text, float x, float y)
        {
            g.DrawStringBounds(text, GLControl.DefaultFont, Brushes.White, Brushes.Black, DeepCore.GUI.Data.TextBorderStyle.Border, x, y);
        }
        public static void DrawDebugTextHUD(this System.Drawing.Graphics g, string text, AlignmentStyle anchor, RectangleF rect)
        {
            g.DrawStringBounds(text, GLControl.DefaultFont, Brushes.White, Brushes.Black, DeepCore.GUI.Data.TextBorderStyle.Border, anchor, rect);
        }
    }

    public class PaintEventArgs3D : EventArgs, IDisposable
    {
        public PaintEventArgs Paint { get; private set; }
        public TimeSpan ELapsed { get; private set; }
        public CameraControl Camera { get; private set; }
        public Matrix4 ModelMatrix { get; private set; }
        private Stack<Matrix4> matrix4s = new Stack<Matrix4>();
        public PaintEventArgs3D(PaintEventArgs e, TimeSpan elapsed, CameraControl camera)
        {
            this.Paint = e;
            this.ELapsed = elapsed;
            this.Camera = camera;
            this.ModelMatrix = Matrix4.Identity;
        }
        public Matrix4 MultiplyMatrix(in Matrix4 mtx)
        {
            ModelMatrix *= mtx;
            var t = ModelMatrix * Camera.ViewMatrix;
            GL.LoadMatrix(ref t);
            return ModelMatrix;
        }
        public Matrix4 PushMatrix()
        {
            GL.PushMatrix();
            matrix4s.Push(ModelMatrix);
            return ModelMatrix;
        }
        public Matrix4 PopMatrix()
        {
            ModelMatrix = matrix4s.Pop();
            GL.PopMatrix();
            return ModelMatrix;
        }
        public void Dispose()
        {
            Paint = null;
            Camera = null;
            ModelMatrix = Matrix4.Identity;
            matrix4s.Clear();
        }
    }



    public abstract class GLViewObject3D : Disposable
    {
        public GLView View { get; private set; }
        public GLViewObject3D Parent { get; private set; }
        public virtual bool IsVisible { get => true; }
        public Matrix4 Transform { get; set; } = Matrix4.Identity;

        protected override void Disposing()
        {
            this.ClearChildren(true);
        }
        internal void OnAdded(GLViewObject3D parent)
        {
            this.Parent = parent;
            this.View = parent.View;
            this.OnAdded();
        }
        internal void OnRemoved(GLViewObject3D parent)
        {
            this.OnRemoved();
            this.Parent = null;
        }
        internal void Update()
        {
            OnUpdate();
            UpdateChildren();
            OnEndUpdate();
        }
        internal void Render(PaintEventArgs3D e)
        {
            e.PushMatrix();
            try
            {
                e.MultiplyMatrix(this.Transform);
                OnRender(e);
                RenderChildren(e);
                OnEndRender(e);
            }
            finally
            {
                e.PopMatrix();
            }
        }
        internal void RenderHUD(PaintEventArgs3D e)
        {
            e.PushMatrix();
            try
            {
//                 var pos = this.Transform.ExtractTranslation();
//                 var offset = e.Camera.WorldToScreen(pos);
//                 e.MultiplyMatrix(Matrix4.CreateTranslation(offset.X, offset.Y, 0));
                OnRenderHUD(e);
                RenderChildrenHUD(e);
            }
            finally
            {
                e.PopMatrix();
            }
        }


        protected virtual void OnAdded() { }
        protected virtual void OnRemoved() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnEndUpdate() { }
        protected virtual void OnRender(PaintEventArgs3D e) { }
        protected virtual void OnRenderHUD(PaintEventArgs3D e) { }
        protected virtual void OnEndRender(PaintEventArgs3D e) { }
        public void RemoveFromParent(bool dispose = true)
        {
            if (Parent != null)
            {
                Parent.RemoveChild(this, dispose);
            }
        }

        #region Children
        public int ChildrenCount { get => objects.Count; }
        private LinkedListNode<GLViewObject3D> curChildNode;
        private LinkedList<GLViewObject3D> objects = new LinkedList<GLViewObject3D>();
        public GLViewObject3D ForEachChildren(BreakPredicate<GLViewObject3D> action, bool recursive = false)
        {
            var list = new List<GLViewObject3D>(objects);
            {
                foreach (var o in list)
                {
                    if (action(o)) { return o; }
                    if (recursive)
                    {
                        var ct = o.ForEachChildren(action);
                        if (ct != null) return ct;
                    }
                }
            }
            return null;
        }
        public T ForEachChildren<T>(BreakPredicate<T> action, bool recursive = false) where T : GLViewObject3D
        {
            var list = new List<GLViewObject3D>(objects);
            {
                foreach (var o in list)
                {
                    if (o is T t && action(t)) { return t; }
                    if (recursive)
                    {
                        var ct = o.ForEachChildren<T>(action);
                        if (ct != null) return ct;
                    }
                }
            }
            return null;
        }
        public T FindChild<T>() where T : GLViewObject3D
        {
            return ForEachChildren<T>(v => true, true);
        }
        public T FindChild<T>(Predicate<T> find) where T : GLViewObject3D
        {
            return ForEachChildren<T>(v => find(v), true);
        }
        public GLViewObject3D FindChild(Predicate<GLViewObject3D> find)
        {
            return ForEachChildren(v => find(v), true);
        }
        public void ClearChildren(bool dispose = true)
        {
            var list = new List<GLViewObject3D>(objects);
            {
                foreach (var o in list)
                {
                    RemoveChild(o, dispose);
                }
            }
        }
        public void AddChild(GLViewObject3D obj)
        {
            if (obj == this) throw new Exception();
            if (obj.Parent != null) throw new Exception();
            if (obj.curChildNode != null) throw new Exception();
            obj.Parent = this;
            obj.curChildNode = objects.AddLast(obj);
            obj.ForEachChildren(cc => { cc.View = this.View; return false; }, true);
            obj.OnAdded(this);
        }
        public void RemoveChild(GLViewObject3D obj, bool dispose = true)
        {
            if (obj == this) throw new Exception();
            if (obj.Parent != this) throw new Exception();
            if (obj.curChildNode == null) throw new Exception();
            try
            {
                objects.Remove(obj.curChildNode);
                obj.OnRemoved(this);
                if (dispose)
                {
                    obj.Dispose();
                }
            }
            finally
            {
                obj.curChildNode = null;
                obj.Parent = null;
            }
        }
        internal void UpdateChildren()
        {
            var list = new List<GLViewObject3D>(objects);
            {
                foreach (var o in list)
                {
                    o.Update();
                }
            }
        }
        internal void RenderChildren(PaintEventArgs3D e)
        {
            var list = new List<GLViewObject3D>(objects);
            {
                foreach (var o in list)
                {
                    if (o.IsVisible) o.Render(e);
                }
            }
        }
        internal void RenderChildrenHUD(PaintEventArgs3D e)
        {
            var list = new List<GLViewObject3D>(objects);
            {
                foreach (var o in list)
                {
                    if (o.IsVisible) o.RenderHUD(e);
                }
            }
        }
        #endregion

        public class GLViewRootObject3D : GLViewObject3D
        {
            public GLViewRootObject3D(GLView view)
            {
                this.View = view;
                this.Parent = this;
            }
        }

    }

    public class GLViewTextObject3D : GLViewObject3D
    {
        private bool isDirty = true;
        protected readonly GLTexture2D txt_head;
        private string m_text = "";
        private Color4 m_color = Color4.White;
        private Font m_font = GLControl.DefaultFont;
        public Vector3 Position { get; set; }
        public string Text { get => m_text; set { m_text = value; this.isDirty = true; } }
        public Color4 Color { get => m_color; set { m_color = value; this.isDirty = true; } }
        public Font Font { get => m_font; set { m_font = value; this.isDirty = true; } }
        public GLViewTextObject3D()
        {
            this.txt_head = new GLTextTexture2D();
        }
        protected override void Disposing()
        {
            base.Disposing();
            txt_head.Dispose();
        }
        protected override void OnRenderHUD(PaintEventArgs3D e)
        {
            var screen = e.Camera.WorldToScreen(Position);
            if (isDirty)
            {
                isDirty = false;
                txt_head.InitWithText($"{Text}", m_font.Size, m_color, 8, Color4.Black);
            }
            txt_head.DrawQuards2D(e, screen.X, screen.Y, GLTextureAnchor.C_C);
        }

    }
}
