using DeepCore;
using DeepCore.GUI.Win32;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static OpenTK.Audio.OpenAL.ALC;

namespace DeepEditor.Common.Drawing
{

    public class DrawableGraphics : Disposable
    {
        private Pen s_mouse_holder = new Pen(Color.White);
        private Brush s_bg_brush = new SolidBrush(Color.FromArgb(192, Color.Black));
        private Brush s_text_brush = new SolidBrush(Color.White);
        private Brush s_border_brush = new SolidBrush(Color.Black);
        private DeepCore.GUI.Data.TextBorderStyle s_borderCount = DeepCore.GUI.Data.TextBorderStyle.Border;

        public readonly Graphics g;
        public readonly Control control;
        public readonly System.Drawing.Point mouse;
        public readonly Font font = Form.DefaultFont;
        private GraphicsState gs;
        public string LastToolTips { get; set; }
        public RectangleF? LastDrawable { get; set; }
        public DrawableGraphics(Graphics g, Control control)
        {
            this.g = g;
            this.control = control;
            this.mouse = control.GetMousePoint();
            this.font = control.Font;
            this.gs = g.Save();
            this.LastToolTips = null;
            this.LastDrawable = null;
        }
        protected override void Disposing()
        {
            g.Restore(gs);
            if (LastDrawable.HasValue)
            {
                var clip = LastDrawable.Value;
                g.DrawRectangle(s_mouse_holder, clip);
                if (!string.IsNullOrEmpty(LastToolTips))
                {
                    var tx = clip.X + clip.Width + 8;
                    var ty = clip.Y + clip.Height + 8;
                    var trect = g.MeasureString(LastToolTips, font, 300);
                    var s_textAnchor = DeepCore.GUI.Data.AlignmentStyle.None;
                    if (tx + trect.Width >= control.Width)
                    {
                        tx -= trect.Width + clip.Width + 8 + 8;
                        //s_textAnchor |= DeepCore.GUI.Data.AlignmentStyle.MASK_RIGHT;
                    }
                    if (ty + trect.Height > control.Height)
                    {
                        ty -= trect.Height + clip.Height + 8 + 8;
                        //s_textAnchor |= DeepCore.GUI.Data.AlignmentStyle.MASK_BOTTOM;
                    }
                    g.FillRectangle(s_bg_brush,
                        tx - 2, ty - 2, trect.Width + 4, trect.Height + 4);
                    g.DrawStringBounds(
                         text: LastToolTips,
                         font: font,
                         bodyBrush: s_text_brush,
                         borderBrush: s_border_brush,
                         borderTime: s_borderCount,
                         anchor: s_textAnchor,
                         expectRect: new System.Drawing.RectangleF(tx, ty, trect.Width, trect.Height));
                }
            }
        }
    }

    public abstract class Drawable
    {

        public int spacing = 2;
        public StringFormat format = Win32Driver.DefaultFormat;
        public Font font = Form.DefaultFont;
        public AnchorStyles anchor = AnchorStyles.Left | AnchorStyles.Top;
        public string ToolTips { get; set; }
        public bool EnableClip = true;
        public virtual SizeF Draw(DrawableGraphics gfx, float sx, float sy, float sw, float sh)
        {
            var g = gfx.g;
            sx = sx - (((anchor & AnchorStyles.Right) != 0) ? sw : 0);
            sy = sy - (((anchor & AnchorStyles.Bottom) != 0) ? sh : 0);
            var clip = new System.Drawing.RectangleF(sx, sy, sw, sh);
            float x = sx + spacing;
            float y = sy + spacing;
            float w = sw - (spacing << 1);
            float h = sh - (spacing << 1);
            if (EnableClip)
            {
                g.SetClip(clip);
            }
            try
            {
                var dsize = DrawSelf(g, x, y, w, h);
                clip.Width = dsize.Width;
                clip.Height = dsize.Height;
                if (clip.Contains(gfx.mouse))
                {
                    gfx.LastDrawable = clip;
                    gfx.LastToolTips = this.ToolTips;
                }
            }
            finally
            {
                if (EnableClip)
                {
                    g.ResetClip();
                }
            }
            if ((anchor & AnchorStyles.Bottom) != 0)
            {
                clip.Height = -clip.Height;
            }
            if ((anchor & AnchorStyles.Right) != 0)
            {
                clip.Width = -clip.Width;
            }
            return clip.Size;
        }
        protected abstract SizeF DrawSelf(Graphics g, float x, float y, float w, float h);
    }

    public class GaugeRectFan : Drawable
    {
        public Brush body_brush = new SolidBrush(Color.FromArgb(0xFF, 0x40, 0x40, 0x40));
        public Brush border_brush = new SolidBrush(Color.Gray);
        public Brush cd_brush = new SolidBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF));
        public Brush text_brush = new SolidBrush(Color.White);

        public int border = 1;
        public string text1;
        public string text2;
        public float percent;

        public GaugeRectFan SetText(string t1, string t2)
        {
            this.text1 = t1;
            this.text2 = t2;
            return this;
        }
        public GaugeRectFan SetAmount(float pct)
        {
            this.percent = pct;
            return this;
        }

        protected override SizeF DrawSelf(Graphics g, float x, float y, float sw, float sh)
        {
            var w = sw;
            var h = sh;
            g.FillRectangle(border_brush, x, y, w, h);
            x += border;
            y += border;
            w -= (border << 1);
            h -= (border << 1);
            g.FillRectangle(body_brush, x, y, w, h);
            if (percent > 0f && percent < 1f)
            {
                g.FillPie(cd_brush, x - w, y - h, w * 3, h * 3, -90, 360 * percent);
            }
            if (!string.IsNullOrEmpty(text1) && string.IsNullOrEmpty(text2))
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(text1, font, text_brush, x, y + border, Win32Driver.DefaultFormat);
            }
            else
            {
                x += 1;
                y += 1;
                w -= 2;
                h -= 2;
                if (!string.IsNullOrEmpty(text1))
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    g.DrawString(text1, font, text_brush, x, y + border, Win32Driver.DefaultFormat);
                }
                if (!string.IsNullOrEmpty(text2))
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    g.DrawString(text2, font, text_brush, new System.Drawing.RectangleF(x, y, w, h - border), format);
                }
            }
            return new SizeF(sw, sh);
        }
    }

    public class TextRectBody : Drawable
    {
        public Brush body_brush = new SolidBrush(Color.FromArgb(0xFF, 0x40, 0x40, 0x40));
        public Brush border_brush = new SolidBrush(Color.Gray);
        public Brush cd_brush = new SolidBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF));
        public Brush text_brush = new SolidBrush(Color.White);
        public int border = 1;
        public string text1;
        public string text2;

        public TextRectBody SetText(string t1, string t2)
        {
            this.text1 = t1;
            this.text2 = t2;
            return this;
        }
        protected override SizeF DrawSelf(Graphics g, float x, float y, float sw, float sh)
        {
            var w = sw;
            var h = sh;
            g.FillRectangle(border_brush, x, y, w, h);
            x += border;
            y += border;
            w -= (border << 1);
            h -= (border << 1);
            g.FillRectangle(body_brush, x, y, w, h);

            if (!string.IsNullOrEmpty(text1) && string.IsNullOrEmpty(text2))
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(text1, font, text_brush, x, y + border, Win32Driver.DefaultFormat);
            }
            else
            {
                x += 1;
                y += 1;
                w -= 2;
                h -= 2;
                if (!string.IsNullOrEmpty(text1))
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    g.DrawString(text1, font, text_brush, x, y + border, Win32Driver.DefaultFormat);
                }
                if (!string.IsNullOrEmpty(text2))
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    g.DrawString(text2, font, text_brush, new System.Drawing.RectangleF(x, y, w, h - border), format);
                }
            }
            return new SizeF(sw, sh);
        }
    }

    public class GaugeStrip : Drawable
    {
        public Brush back_brush = new SolidBrush(Color.FromArgb(0xFF, 0x40, 0x40, 0x40));
        public Brush cd_brush = new SolidBrush(Color.FromArgb(0xA0, 0x20, 0xFF, 0x20));
        public Brush text_brush = new SolidBrush(Color.White);

        public int border = 1;
        public string text;
        public float percent;

        public GaugeStrip SetText(string t)
        {
            this.text = t;
            return this;
        }
        public GaugeStrip SetAmount(float pct)
        {
            this.percent = pct;
            return this;
        }
        public override SizeF Draw(DrawableGraphics gfx, float sx, float sy, float sw, float sh)
        {
            var g = gfx.g;
            var trect = g.MeasureString(text, font);
            sw = Math.Max(sw, trect.Width + (spacing << 1) + 2);
            sh = Math.Max(sh, trect.Height + (spacing << 1) + 2);
            return base.Draw(gfx, sx, sy, sw, sh);
        }
        protected override SizeF DrawSelf(Graphics g, float x, float y, float sw, float sh)
        {
            var w = sw;
            var h = sh;
            g.FillRectangle(back_brush, x, y, w, h);
            x += border;
            y += border;
            w -= (border << 1);
            h -= (border << 1);
            if (percent > 0f && percent < 1f)
            {
                g.FillRectangle(cd_brush, x, y, w * percent, h);
            }
            g.DrawString(text, font, text_brush, x + 1, y + 1, format);

            return new SizeF(sw, sh);
        }
    }

    public class TextLine : Drawable
    {
        public string text;
        public Brush text_brush = new SolidBrush(Color.White);
        public Brush border_brush = new SolidBrush(Color.Black);
        public DeepCore.GUI.Data.TextBorderStyle borderCount = DeepCore.GUI.Data.TextBorderStyle.Border;
        public DeepCore.GUI.Data.AlignmentStyle textAnchor = DeepCore.GUI.Data.AlignmentStyle.TopLeft;
        public TextLine SetText(string t)
        {
            EnableClip = false;
            this.text = t;
            return this;
        }

        public override SizeF Draw(DrawableGraphics gfx, float sx, float sy, float sw, float sh)
        {
            var g = gfx.g;
            var trect = g.MeasureString(text, font);
            sw = Math.Max(sw, trect.Width + (spacing << 1));
            sh = Math.Max(sh, trect.Height + (spacing << 1));
            return base.Draw(gfx, sx, sy, sw, sh);
        }
        protected override SizeF DrawSelf(Graphics g, float x, float y, float w, float h)
        {
            //g.DrawString(text, font, text_brush, x, y, format);
            var textBounds = g.DrawStringBounds(
               text: text,
               font: font,
               bodyBrush: text_brush,
               borderBrush: border_brush,
               borderTime: borderCount,
               anchor: textAnchor,
               x: x,
               y: y);
            return textBounds;
        }
    }
    public class TextRect : Drawable
    {
        public string text;
        public Brush text_brush = new SolidBrush(Color.White);
        public Brush border_brush = new SolidBrush(Color.Black);
        public DeepCore.GUI.Data.TextBorderStyle borderCount = DeepCore.GUI.Data.TextBorderStyle.Border;
        public DeepCore.GUI.Data.AlignmentStyle textAnchor = DeepCore.GUI.Data.AlignmentStyle.None;
        public Brush back_brush;
        public Pen back_border;
        public TextRect SetText(string t)
        {
            EnableClip = false;
            this.text = t;
            return this;
        }

        public override SizeF Draw(DrawableGraphics gfx, float sx, float sy, float sw, float sh)
        {
            return base.Draw(gfx, sx, sy, sw, sh);
        }
        protected override SizeF DrawSelf(Graphics g, float x, float y, float w, float h)
        {
            if (back_brush != null)
            {
                g.FillRectangle(back_brush, new RectangleF(x, y, w, h));
            }
            if (back_border != null)
            {
                g.DrawRectangle(back_border, new RectangleF(x, y, w - 1, h - 1));
            }
            var trect = g.MeasureString(text, font, (int)w - (spacing << 1));
            //             if ((anchor | AnchorStyles.Right) != 0)
            //                 x = x + w - trect.Width;
            //             if ((anchor | AnchorStyles.Bottom) != 0)
            //                 y = y + h - trect.Height;
            //var sw = Math.Max(w, trect.Width + (spacing << 1));
            //var sh = Math.Max(h, trect.Height + (spacing << 1));
            //g.DrawString(text, font, text_brush, new System.Drawing.RectangleF(x, y, w, h), format);
            g.DrawStringBounds(
                    text: text,
                    font: font,
                    bodyBrush: text_brush,
                    borderBrush: border_brush,
                    borderTime: borderCount,
                    anchor: textAnchor,
                    expectRect: new System.Drawing.RectangleF(x, y, w, h));

            return trect;
        }
    }

    public static class DrawingUtil
    {

        public static Color RandomColor(this Random random)
        {
            var r = random.Next() % 0x100;
            var g = random.Next() % 0x100;
            var b = random.Next() % 0x100;
            return Color.FromArgb(0xFF, r, g, b);
        }
    }


}
