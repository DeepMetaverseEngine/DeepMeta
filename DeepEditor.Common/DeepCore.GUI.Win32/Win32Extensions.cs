using DeepCore;
using DeepCore.GUI;
using DeepCore.GUI.Cell;
using DeepCore.GUI.Data;
using DeepCore.GUI.Display;
using DeepCore.GUI.SceneGraph;
using DeepEditor.Common;
using DeepCore.GUI.Input;

namespace DeepCore.GUI.Win32
{
    public static class EXT
    {

        public static System.Drawing.FontStyle ToFontStyle(this TextFontStyle style)
        {
            return (System.Drawing.FontStyle)style;
        }

        public static System.Drawing.Bitmap GetTileBitmap(this CPJAtlas atlas, int index)
        {
            var image = atlas.GetTile(index);
            if (image is Win32.Win32Image wimg)
            {
                return wimg.src;
            }
            return null;
        }

        //         public static void ToStringAlignment(this PivotStyle anchor, out System.Drawing.StringAlignment alignment, out System.Drawing.StringAlignment lineAlignment)
        //         {
        //             alignment = System.Drawing.StringAlignment.Near;
        //             lineAlignment = System.Drawing.StringAlignment.Near;
        //  
        //             if ((anchor & PivotStyle.ANCHOR_TOP) != 0)
        //                 lineAlignment = System.Drawing.StringAlignment.Near;
        //             if ((anchor & PivotStyle.ANCHOR_VCENTER) != 0)
        //                 lineAlignment = System.Drawing.StringAlignment.Center;
        //             if ((anchor & PivotStyle.ANCHOR_BOTTOM) != 0)
        //                 lineAlignment = System.Drawing.StringAlignment.Far;
        //  
        //             if ((anchor & PivotStyle.ANCHOR_LEFT) != 0)
        //                 alignment = System.Drawing.StringAlignment.Near;
        //             if ((anchor & PivotStyle.ANCHOR_HCENTER) != 0)
        //                 alignment = System.Drawing.StringAlignment.Center;
        //             if ((anchor & PivotStyle.ANCHOR_RIGHT) != 0)
        //                 alignment = System.Drawing.StringAlignment.Far;
        //         }
        public static void ToStringAlignment(this AlignmentStyle anchor, out System.Drawing.StringAlignment alignment, out System.Drawing.StringAlignment lineAlignment)
        {
            alignment = System.Drawing.StringAlignment.Near;
            lineAlignment = System.Drawing.StringAlignment.Near;

            if ((anchor & AlignmentStyle.MASK_TOP) != 0)
            {
                lineAlignment = System.Drawing.StringAlignment.Near;
            }
            else if ((anchor & AlignmentStyle.MASK_MIDDLE) != 0)
            {
                lineAlignment = System.Drawing.StringAlignment.Center;
            }
            else if ((anchor & AlignmentStyle.MASK_BOTTOM) != 0)
            {
                lineAlignment = System.Drawing.StringAlignment.Far;
            }

            if ((anchor & AlignmentStyle.MASK_LEFT) != 0)
            {
                alignment = System.Drawing.StringAlignment.Near;
            }
            else if ((anchor & AlignmentStyle.MASK_CENTER) != 0)
            {
                alignment = System.Drawing.StringAlignment.Center;
            }
            else if ((anchor & AlignmentStyle.MASK_RIGHT) != 0)
            {
                alignment = System.Drawing.StringAlignment.Far;
            }
        }

    }
}

namespace System.Windows.Forms
{
    public static class EXT
    {
        public static MouseArgs ToMouseArgs(this MouseEventArgs e, DisplayRoot root)
        {
            return new MouseArgs(root)
            {
                Button = (MouseButton)e.Button,
                IsCtrlDown = Keyboard.IsCtrlDown,
                Delta = e.Delta,
                Location = new DeepCore.Geometry.Vector2(e.X, e.Y),
                Clicks = e.Clicks,
            };
        }
        public static KeyboardArgs ToKeyArgs(this KeyEventArgs e, DisplayRoot root)
        {
            return new KeyboardArgs(root)
            {
                Alt = e.Alt,
                Control = e.Control,
                Handled = e.Handled,
                KeyCode = (KeyCode)e.KeyCode,
                KeyData = (KeyCode)e.KeyData,
                KeyValue = e.KeyValue,
                Modifiers = (KeyCode)e.Modifiers,
                Shift = e.Shift,
                SuppressKeyPress = e.SuppressKeyPress,
            };
        }
        public static KeyboardArgs ToKeyArgs(this KeyPressEventArgs e, DisplayRoot root)
        {
            return new KeyboardArgs(root)
            {
                KeyChar = e.KeyChar,
                Handled = e.Handled,
            };
        }
    }
}

namespace System.Drawing
{
    public static class EXT
    {
        public static void DrawInClip(this Graphics g, RectangleF bounds, Action<Graphics> draw)
        {
            var clip = g.Clip;
            try
            {
                g.SetClip(bounds);
                draw(g);
            }
            finally
            {
                g.Clip = clip;
            }
        }



        public static DeepCore.Geometry.RectangleF ToGeometry(this RectangleF e)
        {
            return new DeepCore.Geometry.RectangleF(e.X, e.Y, e.Width, e.Height);
        }
        public static DeepCore.Geometry.RectangleF ToGeometry(this Rectangle e)
        {
            return new DeepCore.Geometry.RectangleF(e.X, e.Y, e.Width, e.Height);
        }
        public static DeepCore.Geometry.Vector2 ToGeometry(this PointF e)
        {
            return new DeepCore.Geometry.Vector2(e.X, e.Y);
        }
        public static DeepCore.Geometry.Vector2 ToGeometry(this Point e)
        {
            return new DeepCore.Geometry.Vector2(e.X, e.Y);
        }

        public static System.Drawing.RectangleF ToGDI(this DeepCore.Geometry.RectangleF e)
        {
            return new System.Drawing.RectangleF(e.X, e.Y, e.Width, e.Height);
        }
        public static System.Drawing.RectangleF ToGDI(this DeepCore.Geometry.Rectangle e)
        {
            return new System.Drawing.RectangleF(e.X, e.Y, e.Width, e.Height);
        }
        public static System.Drawing.PointF ToGDI(this DeepCore.Geometry.Vector2 e)
        {
            return new System.Drawing.PointF(e.X, e.Y);
        }


        public static string ToString(this Size image, string format)
        {
            return image.IsEmpty ? "N/A" : string.Format(format, image.Width, image.Height);
        }

        public static Size CalcTextureSize(this Size image, bool pow = false, bool sqr = false)
        {
            long w = image.Width;
            long h = image.Height;
            if (pow)
            {
                w = CMath.NextPOT(w);
                h = CMath.NextPOT(h);
            }
            if (sqr)
            {
                w = (h = Math.Max(w, h));
            }
            return new Size((int)w, (int)h);
        }

        public static long CalcTextureBytes(this Size image, bool pow = false, bool sqr = false)
        {
            long w = image.Width;
            long h = image.Height;
            if (pow)
            {
                w = CMath.NextPOT(w);
                h = CMath.NextPOT(h);
            }
            if (sqr)
            {
                w = (h = Math.Max(w, h));
            }
            return w * h * 4;
        }
    }
}
