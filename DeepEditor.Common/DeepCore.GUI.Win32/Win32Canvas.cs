using DeepCore.Geometry;
using DeepCore.GUI.Input;
using DeepCore.GUI.SceneGraph;
using DeepCore.Threading;
using DeepEditor.Common.G3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepCore.GUI.Win32
{
    public class PictureBoxCanvas : Win32Canvas
    {
        public PictureBoxCanvas(PictureBox control) : base(control)
        {
            control.Paint += Canvas_Paint;
        }
    }
    public class GLViewCanvas : Win32Canvas
    {
        public GLView View { get; }
        public GLViewCanvas(GLView control) : base(control.GLControl)
        {
            this.View = control;
            control.OnPaintGDI += Canvas_Paint;
        }
    }

    public abstract class Win32Canvas : Disposable, IDisplayCanvas
    {
        private readonly static TypeAllocRecorder Alloc = new TypeAllocRecorder(typeof(Win32Canvas)) { };
        internal Win32DisplayRoot root;
        internal Control canvas;
        public Control Control => canvas;
        public Win32Canvas(Control control)
        {
            Alloc.RecordConstructor(GetType());
            this.invoking = new UpdateTaskQueue<IDisplayCanvas>(this);
            this.canvas = control;
            this.canvas.MouseDown += Canvas_MouseDown;
            this.canvas.MouseUp += Canvas_MouseUp;
            this.canvas.MouseMove += Canvas_MouseMove;
            this.canvas.MouseWheel += Canvas_MouseWheel;
            this.canvas.KeyDown += Canvas_KeyDown;
            this.canvas.KeyPress += Canvas_KeyPress;
            this.canvas.KeyUp += Canvas_KeyUp;
            this.canvas.Resize += Canvas_Resize;
        }
        ~Win32Canvas()
        {
            Alloc.RecordDestructor(GetType());
        }
        protected override void Disposing()
        {
            Alloc.RecordDispose(GetType());
            {
                this.IDisplayCanvas_KeyDown = null;
                this.IDisplayCanvas_KeyPress = null;
                this.IDisplayCanvas_KeyUp = null;
                this.IDisplayCanvas_MouseDown = null;
                this.IDisplayCanvas_MouseUp = null;
                this.IDisplayCanvas_MouseMove = null;
                this.IDisplayCanvas_MouseWheel = null;
                this.IDisplayCanvas_Update = null;
                this.IDisplayCanvas_Paint = null;
                this.IDisplayCanvas_Resize = null;
            }
            this.canvas.MouseDown -= Canvas_MouseDown;
            this.canvas.MouseUp -= Canvas_MouseUp;
            this.canvas.MouseMove -= Canvas_MouseMove;
            this.canvas.MouseWheel -= Canvas_MouseWheel;
            this.canvas.KeyDown -= Canvas_KeyDown;
            this.canvas.KeyPress -= Canvas_KeyPress;
            this.canvas.KeyUp -= Canvas_KeyUp;
            this.canvas.Resize -= Canvas_Resize;

            //this.updating.Clear();

            this.invoking.Dispose();
            this.canvas = null;
            this.root = null;
        }
        //-----------------------------------------------------------------------------------------------------
        public bool Focused
        {
            get
            {
                var f = canvas.FindForm()?.ContainsFocus;
                if (f.HasValue) return f.Value;
                return false;
            }
        }
        public MouseButton MouseButtons => (MouseButton)Control.MouseButtons;
        public KeyCode KeyStatus => (KeyCode)Control.ModifierKeys;
        public void RequestRepaint()
        {
            if (root.RepaintOnMouseHold)
            {
                canvas.Refresh();
            }
        }
        //         public void PostToEditor(DisplayNode e, object arg)
        //         {
        //             if (canvas is Win32PictureBox pictureBox)
        //             {
        //                 pictureBox.PostToEditor(e, arg);
        //             }
        //         }
        Vector2 IDisplayCanvas.Size
        {
            get => new Vector2(canvas.Width, canvas.Height);
        }
        Vector2 IDisplayCanvas.MouseLocation
        {
            get => canvas.GetMousePoint().ToGeometry();
        }

        //-----------------------------------------------------------------------------------------------------
        #region Events
        protected void Canvas_Resize(object sender, EventArgs e)
        {
            try
            {
                IDisplayCanvas_Resize?.Invoke(this, new Vector2(canvas.Width, canvas.Height));
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        protected void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                IDisplayCanvas_KeyDown?.Invoke(this, e.ToKeyArgs(root));
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        protected void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                IDisplayCanvas_KeyUp?.Invoke(this, e.ToKeyArgs(root));
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        protected void Canvas_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                IDisplayCanvas_KeyPress?.Invoke(this, e.ToKeyArgs(root));
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        protected void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                canvas.Focus();
                IDisplayCanvas_MouseDown?.Invoke(this, e.ToMouseArgs(root));
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        protected void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                IDisplayCanvas_MouseUp?.Invoke(this, e.ToMouseArgs(root));
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        protected void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //if (canvas.Focused)
            {
                try
                {
                    IDisplayCanvas_MouseMove?.Invoke(this, e.ToMouseArgs(root));
                }
                catch (Exception err) { err.PrintStackTrace(); }
            }
        }
        protected void Canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            //if (canvas.Focused)
            {
                try
                {
                    IDisplayCanvas_MouseWheel?.Invoke(this, e.ToMouseArgs(root));
                }
                catch (Exception err) { err.PrintStackTrace(); }
            }
        }
        protected void Canvas_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                //InvokingAsync().Wait();
                invoking.Update();
                IDisplayCanvas_Update?.Invoke(this);
                IDisplayCanvas_Paint?.Invoke(this, new Win32Graphics(e.Graphics));
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        //-----------------------------------------------------------------------------------------------------
        private CanvasKeyboardEvent IDisplayCanvas_KeyDown;
        private CanvasKeyboardEvent IDisplayCanvas_KeyPress;
        private CanvasKeyboardEvent IDisplayCanvas_KeyUp;
        private CanvasMouseEvent IDisplayCanvas_MouseDown;
        private CanvasMouseEvent IDisplayCanvas_MouseUp;
        private CanvasMouseEvent IDisplayCanvas_MouseMove;
        private CanvasMouseEvent IDisplayCanvas_MouseWheel;
        private CanvasUpdateEvent IDisplayCanvas_Update;
        private CanvasPaintEvent IDisplayCanvas_Paint;
        private CanvasResizeEvent IDisplayCanvas_Resize;

        event CanvasKeyboardEvent IDisplayCanvas.KeyDown { add { IDisplayCanvas_KeyDown += value; } remove { IDisplayCanvas_KeyDown -= value; } }
        event CanvasKeyboardEvent IDisplayCanvas.KeyPress { add { IDisplayCanvas_KeyPress += value; } remove { IDisplayCanvas_KeyPress -= value; } }
        event CanvasKeyboardEvent IDisplayCanvas.KeyUp { add { IDisplayCanvas_KeyUp += value; } remove { IDisplayCanvas_KeyUp -= value; } }
        event CanvasMouseEvent IDisplayCanvas.MouseDown { add { IDisplayCanvas_MouseDown += value; } remove { IDisplayCanvas_MouseDown -= value; } }
        event CanvasMouseEvent IDisplayCanvas.MouseUp { add { IDisplayCanvas_MouseUp += value; } remove { IDisplayCanvas_MouseUp -= value; } }
        event CanvasMouseEvent IDisplayCanvas.MouseMove { add { IDisplayCanvas_MouseMove += value; } remove { IDisplayCanvas_MouseMove -= value; } }
        event CanvasMouseEvent IDisplayCanvas.MouseWheel { add { IDisplayCanvas_MouseWheel += value; } remove { IDisplayCanvas_MouseWheel -= value; } }
        event CanvasUpdateEvent IDisplayCanvas.Update { add { IDisplayCanvas_Update += value; } remove { IDisplayCanvas_Update -= value; } }
        event CanvasPaintEvent IDisplayCanvas.Paint { add { IDisplayCanvas_Paint += value; } remove { IDisplayCanvas_Paint -= value; } }
        event CanvasResizeEvent IDisplayCanvas.Resize { add { IDisplayCanvas_Resize += value; } remove { IDisplayCanvas_Resize -= value; } }

        #endregion
        //-----------------------------------------------------------------------------------------------------
        #region MainThread
        private UpdateTaskQueue<IDisplayCanvas> invoking;
        //private List<Func<Task>> updating = new List<Func<Task>>();
        public void Invoke(Func<IDisplayCanvas, Task> invoke)
        {
            invoking.Add(invoke);
        }
        public void Invoke(Action<IDisplayCanvas> invoke)
        {
            invoking.Add(invoke);
        }
//         async Task InvokingAsync()
//         {
//             if (invoking.Count > 0)
//             {
//                 updating.AddRange(invoking);
//                 invoking.Clear();
//                 try
//                 {
//                     if (!IsDisposing)
//                     {
//                         foreach (var func in updating)
//                         {
//                             try
//                             {
//                                 await func();
//                             }
//                             catch (Exception err) { err.PrintStackTrace(); }
//                         }
//                     }
//                 }
//                 finally
//                 {
//                     updating.Clear();
//                 }
//             }
//         }
        #endregion
        //-----------------------------------------------------------------------------------------------------
    }
}
