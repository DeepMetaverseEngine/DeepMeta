using DeepCore.Geometry;
using DeepCore.GUI.Data;
using DeepCore.GUI.Display;
using System;
using System.Security.Policy;

namespace DeepCore.GUI.Win32
{
    public class Win32TextLayer : TextLayer
    {
        internal System.Drawing.Font cur_font;
        //         private System.Drawing.Bitmap src;
        //         private Win32Image buffer;
        //         public Win32Image Buffer { get => buffer; }
        internal Win32TextLayer(string text, object fontName, TextFontStyle style, float size)
            : base(text, (int)size, style)
        {
            this.Text = (text);
            this.isDirty = true;
            if (fontName is System.Drawing.Font f)
            {
                this.cur_font = Win32Driver.CreateFont(f.FontFamily, mFontSize, mFontStyle);
            }
            else
            {
                this.cur_font = Win32Driver.CreateFont(mFontSize, mFontStyle);
            }
            SetSize(cur_font);
        }

        internal void SetSize(System.Drawing.Font f)
        {
            var size = Win32Driver.GetTextBounds(mText, f, mBorderTime, 0);
            this.mBounds.Width = (size.Width);
            this.mBounds.Height = (size.Height);
        }

        internal void Refresh(Win32Graphics g)
        {
            if (this.isDirty)
            {
                this.isDirty = false;
                //                   if (buffer != null)
                //                   {
                //                       buffer.Dispose();
                //                   }
                SetSize(cur_font);
            }
        }
        protected override void Disposing()
        {
            //             if (src != null)
            //             {
            //                 src.Dispose();
            //             }
            if (cur_font != null)
            {
                cur_font.Dispose();
            }
        }
        //         public override Image GetBuffer()
        //         {
        //             Refresh(Win32Driver.TestGFX);
        //             return buffer;
        //         }
//         public override void Render(Graphics g, Geometry.RectangleF rect, AlignmentStyle alignment)
//         {
//             var dg = g as Win32Graphics;
//             //SetSize(cur_font);
//             dg.SetFont(this.cur_font);
//             Refresh(dg);
// //             rect.Width += 1;
// //             rect.Height += 1;
//             dg.DrawStringBounds(mText, mFontColorRGBA, mBorderColorRGBA, mBorderTime, alignment, rect);
//             //dg.DrawRect(rect.x, rect.y, rect.width - 2, rect.height - 2);
//         }
    }
}
