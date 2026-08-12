using DeepCore.GUI.Data;
using DeepCore.GUI.Display;
using DeepCore.GUI.Display.Text;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
namespace DeepCore.GUI.Win32

{
    public static class Win32RichTextLayer
    {
        static Win32RichTextLayer()
        {
            if (Win32Driver.Instance == null)
            {
                new Win32Driver();
            }
        }
        public static Bitmap CreateAttributeTextBuffer(
            int width,
            AttributedString atext,
            Font defaultFont = null,
            RichTextAlignment anchor = RichTextAlignment.taLEFT)
        {
            try
            {
                var buffer = new Bitmap(width, width);
                var g = System.Drawing.Graphics.FromImage(buffer);
                try
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    var richText = new RichTextLayer(width, anchor);
                    richText.SetEnableMultiline(true);
                    richText.SetString(atext);
                    var cw = (int)richText.ContentWidth + 4;
                    var ch = (int)richText.ContentHeight + 4;
                    if (ch != buffer.Height || cw != buffer.Width)
                    {
                        g.Dispose();
                        buffer.Dispose();
                        buffer = new Bitmap(cw, ch);
                        g = System.Drawing.Graphics.FromImage(buffer);
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    }
                    var gfx = new Win32Graphics(g);
                    gfx.SetFont(defaultFont);
                    richText.Render(gfx, 2, 2);
                }
                finally
                {
                    g.Dispose();
                }
                return buffer;
            }
            catch
            {
                return null;
            }
        }
        public static Bitmap CreateAttributeTextBuffer(
            int width,
            XmlDocument xml,
            Font defaultFont = null,
            RichTextAlignment anchor = RichTextAlignment.taLEFT,
            float? defaultFontSize = null,
            System.Drawing.Color? defaultColor = null,
            TextFontStyle defaultStyle = TextFontStyle.Plain)
        {
            var dsize = defaultFontSize.HasValue ? defaultFontSize.Value : 11;
            var dcolor = defaultColor.HasValue ? DeepCore.GUI.Display.Color.EncodeRGBA(defaultColor.Value.R, defaultColor.Value.G, defaultColor.Value.B, defaultColor.Value.A) : 0xFF000000;
            var atext = new AttributedStringDecoder().CreateFromXML(xml, new TextAttribute(dcolor, dsize, defaultFont, defaultStyle));
            return CreateAttributeTextBuffer(width, atext, defaultFont, anchor);
        }

        public static Size MeasureAttributeText(this System.Drawing.Graphics g, AttributedString atext)
        {
            return MeasureAttributeText(g, atext, int.MaxValue);
        }
        public static Size MeasureAttributeText(this System.Drawing.Graphics g, AttributedString atext, int width)
        {
            using (var richText = new RichTextLayer(width, RichTextAlignment.taLEFT))
            {
                richText.SetEnableMultiline(true);
                richText.SetString(atext);
                return new Size(
                    (int)Math.Ceiling(richText.ContentWidth),
                    (int)Math.Ceiling(richText.ContentHeight));
            }
        }
        public static void DrawAttributeText(this System.Drawing.Graphics g, AttributedString atext, int width, RichTextAlignment anchor = RichTextAlignment.taLEFT)
        {
            using (var richText = new RichTextLayer(width, anchor))
            {
                richText.SetEnableMultiline(true);
                richText.SetString(atext);
                using (var gfx = new Win32Graphics(g))
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    richText.Render(gfx, 0, 0);
                }
            }
        }

        public static AttributedString DecodeAttributedString(
           XmlDocument xml,
           Font defaultFont = null,
           RichTextAlignment anchor = RichTextAlignment.taLEFT,
           float? defaultFontSize = null,
           System.Drawing.Color? defaultColor = null,
           TextFontStyle defaultStyle = TextFontStyle.Plain)
        {
            var dsize = defaultFontSize.HasValue ? defaultFontSize.Value : 11;
            var dcolor = defaultColor.HasValue ? DeepCore.GUI.Display.Color.EncodeRGBA(defaultColor.Value.R, defaultColor.Value.G, defaultColor.Value.B, defaultColor.Value.A) : 0xFF000000;
            var atext = new AttributedStringDecoder().CreateFromXML(xml, new TextAttribute(dcolor, dsize, defaultFont, defaultStyle));
            return atext;
        }

        public static void AppendAttributeText(this RichTextBox rt, AttributedString atext)
        {
            atext.ForEachAttributesText((start, count, attr) =>
            {
                var color = attr.fontColor;
                rt.AppendText(atext.Substring(start, count), System.Drawing.Color.FromArgb(color.sARGB));
            });

        }
    }
}
