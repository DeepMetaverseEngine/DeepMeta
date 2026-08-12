using DeepCore.GUI.SceneGraph;
using System;
using System.Windows.Forms;

namespace DeepCore.GUI.Win32
{
    public class Win32DisplayRoot : DisplayRoot
    {
        static Win32DisplayRoot()
        {
            new Win32Driver();
        }
        public bool RepaintOnMouseHold { get; set; } = true;
        private Win32Canvas canvas;
        private Control control;
        public Win32DisplayRoot(Win32Canvas canvas) : base(canvas)
        {
            this.canvas = canvas;
            this.canvas.root = this;
            this.control = canvas.Control;
            this.control.MouseHover += RootCanvas_MouseHover;
            this.control.MouseDown += RootCanvas_MouseDown;
            this.control.MouseMove += RootCanvas_MouseMove;
            this.control.MouseUp += RootCanvas_MouseUp;
        }
        protected override void OnDispose()
        {
            this.control.MouseHover -= RootCanvas_MouseHover;
            this.control.MouseDown -= RootCanvas_MouseDown;
            this.control.MouseMove -= RootCanvas_MouseMove;
            this.control.MouseUp -= RootCanvas_MouseUp;
            base.OnDispose();
            this.canvas.Dispose();
            this.canvas = null;
            this.control = null;
        }
        private void RootCanvas_MouseHover(object sender, EventArgs e)
        {
            if (RepaintOnMouseHold)
            {
                control.Invalidate();
            }
            control.Cursor = Win32DisplayRoot.UpdateCursor(DraggingDirection);
        }
        private void RootCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (RepaintOnMouseHold)
            {
                control.Invalidate();
            }
            control.Cursor = Win32DisplayRoot.UpdateCursor(DraggingDirection);
        }
        private void RootCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (RepaintOnMouseHold)
            {
                //if (e.Button != MouseButtons.None)
                {
                    control.Invalidate();
                }
                //Console.WriteLine("--------------------------------");
            }
            control.Cursor = Win32DisplayRoot.UpdateCursor(DraggingDirection);
        }
        private void RootCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (RepaintOnMouseHold)
            {
                control.Invalidate();
            }
            control.Cursor = Win32DisplayRoot.UpdateCursor(DraggingDirection);
        }


        public static Cursor UpdateCursor(DragDirection dir)
        {
            switch (dir)
            {
                case DragDirection.Left:
                case DragDirection.Right:
                    return Cursors.SizeWE;
                case DragDirection.Top:
                case DragDirection.Bottom:
                    return Cursors.SizeNS;
                case DragDirection.TopLeft:
                case DragDirection.BottomRight:
                    return Cursors.SizeNWSE;
                case DragDirection.TopRight:
                case DragDirection.BottomLeft:
                    return Cursors.SizeNESW;
                default:
                    break;
            }
            return Cursors.Default;
        }

    }
}
