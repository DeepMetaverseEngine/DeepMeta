using DeepCore.GUI.Data;
using DeepCore.GUI.Display;
using DeepCore.IO;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepCore.GUI.Win32
{
    public class Win32Driver : GraphicsDriver
    {
        //--------------------------------------------------------------------------
        public static new Win32Driver Instance { get; private set; }
        public static Win32Graphics TestGFX { get { return Instance.testGFX; } }
        public static FontFamily DefaultFont { get; set; } = FontFamily.GenericSansSerif;


        private System.Drawing.Bitmap testBuffer;
        private PrivateFontCollection loadFonts;
        private Win32Graphics testGFX;

        public Win32Driver()
        {
            Instance = this;
            this.testBuffer = new System.Drawing.Bitmap(100, 100);
            this.testGFX = new Win32Graphics(System.Drawing.Graphics.FromImage(testBuffer));
            this.loadFonts = new PrivateFontCollection();
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------
        #region Impl

        public override void Assert(bool cond, string msg)
        {
            if (!cond)
            {
                MessageBox.Show(msg);
            }
        }

        public override void ReloadImage(Display.Image img)
        {

        }
        public override async Task<Display.Image> CreateImageAsync(string resource)
        {
            byte[] data = await Resource.LoadDataAsync(resource);
            if (data != null)
            {
                using (DeepCore.IO.MemoryStream ms = new DeepCore.IO.MemoryStream(data))
                {
                    System.Drawing.Image src = System.Drawing.Image.FromStream(ms);
                    if (src != null)
                    {
                        return new Win32Image(src, resource);
                    }
                }
            }
            return null;
        }
        public override Display.Image CreateImage(string resource)
        {
            byte[] data = Resource.LoadData(resource);
            if (data != null)
            {
                using (DeepCore.IO.MemoryStream ms = new DeepCore.IO.MemoryStream(data))
                {
                    System.Drawing.Image src = System.Drawing.Image.FromStream(ms);
                    if (src != null)
                    {
                        return new Win32Image(src, resource);
                    }
                }
            }
            return null;
        }

        public override Display.Image CreateImage(System.IO.Stream stream)
        {
            System.Drawing.Image src = System.Drawing.Image.FromStream(stream);
            if (src != null)
            {
                return new Win32Image(src, null);
            }
            return null;
        }

        public override Display.Image CreateImage(byte[] imageData, int imageOffset, int imageLength)
        {
            Stream input = new DeepCore.IO.MemoryStream(imageData, imageOffset, imageLength);
            System.Drawing.Image src = System.Drawing.Image.FromStream(input);
            if (src != null)
            {
                return new Win32Image(src, null);
            }
            return null;
        }

        public override Display.Image CreateRGBImage(int width, int height, uint[] rgba)
        {
            if (rgba != null)
            {
                System.Drawing.Bitmap src = new System.Drawing.Bitmap(width, height,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                if (src != null)
                {
                    int i = 0;
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++, i++)
                        {
                            uint RGBA = rgba[i];
                            src.SetPixel(x, y,
                                System.Drawing.Color.FromArgb(
                                    (int)((RGBA & 0x000000ff)),
                                    (int)((RGBA & 0xff000000) >> 24),
                                    (int)((RGBA & 0x00ff0000) >> 16),
                                    (int)((RGBA & 0x0000ff00) >> 8)
                                ));
                        }
                    }
                    return new Win32Image(src, null);
                }
            }
            else
            {
                System.Drawing.Bitmap src = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                if (src != null)
                {
                    System.Drawing.Color c = System.Drawing.Color.FromArgb(0);
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            src.SetPixel(x, y, c);
                        }
                    }
                    return new Win32Image(src, null);
                }
            }
            return null;
        }

        public override TextLayer CreateTextLayer(string text, object fontName, float size, TextFontStyle style, TextBorderStyle border)
        {
            Win32TextLayer ret = new Win32TextLayer(text, fontName, style, size);
            ret.Refresh(Instance.testGFX);
            return ret;
        }

        public override VertexBuffer CreateVertexBuffer(int capacity)
        {
            throw new NotImplementedException();
        }

        public override bool TestTextLineBreak(string text, object fontName, float size, TextFontStyle style, TextBorderStyle borderTime, float testWidth, out float realWidth, out float realHeight)
        {
            realWidth = 0;
            realHeight = 0;
            testWidth = (int)Math.Ceiling(testWidth);
            using (var cur_font = CreateFont(fontName, size, style))
            {
                var max = GetTextBounds(text, cur_font, borderTime, 0);
                realWidth = max.Width;
                realHeight = max.Height;
                if (testWidth > 0 && max.Width > testWidth)
                {
                    System.Drawing.SizeF min = GetTextBounds(text, cur_font, borderTime, testWidth);
                    realWidth = min.Width;
                    realHeight = min.Height;
                    return true;
                }
            }
            return false;
        }

        #endregion
        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static void LoadFont(string filepath)
        {
            try
            {
                Instance.loadFonts.AddFontFile(filepath);
                DefaultFont = Instance.loadFonts.Families[0];
            }
            catch { }
        }
        public static void LoadFontBinary(byte[] data)
        {
            Instance.loadFonts.AddMemoryFont(CUtils.ToIntPtr(data), data.Length);
            DefaultFont = Instance.loadFonts.Families[0];
        }
        public static System.Drawing.Font CreateFont(System.Drawing.FontFamily family, float size, TextFontStyle style)
        {
            return new System.Drawing.Font(family, size * 0.75f, style.ToFontStyle(), System.Drawing.GraphicsUnit.Pixel, 127);
        }
        public static System.Drawing.Font CreateFont(float size, TextFontStyle style)
        {
            return CreateFont(DefaultFont, size, style);
        }
        public static System.Drawing.Font CreateFont(object fontName, float size, TextFontStyle style)
        {
            var family = DefaultFont;
            if (fontName is System.Drawing.Font font)
            {
                family = font.FontFamily;
            }
            return CreateFont(family, size, style);
        }
        public static System.Drawing.SizeF GetTextBounds(string text, System.Drawing.Font font, TextBorderStyle borderTime, float expectWidth = 0)
        {
            System.Drawing.SizeF size;
            if (expectWidth > 0)
            {
                size = Instance.testGFX.G.MeasureString(text, font, (int)MathF.Ceiling(expectWidth), DefaultFormat);
            }
            else
            {
                size = Instance.testGFX.G.MeasureString(text, font);
            }
            size.Width = (int)MathF.Ceiling(size.Width);
            size.Height = (int)MathF.Ceiling(size.Height);
            return size;
        }
        public static StringFormat DefaultFormat
        {
            get
            {
                var format = StringFormat.GenericTypographic.Clone() as StringFormat;
                format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                return format;
            }
        }
        public static System.Drawing.Bitmap GenStringBuffer(int w, int h, string text, System.Drawing.Font font, uint fontColor, TextBorderStyle borderTime, uint borderColor)
        {
            System.Drawing.Bitmap src = new System.Drawing.Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (System.Drawing.Graphics gfx = System.Drawing.Graphics.FromImage(src))
            {
                gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;

                System.Drawing.SolidBrush bbrush = new System.Drawing.SolidBrush(
                    System.Drawing.Color.FromArgb((int)Display.Color.ToARGB(borderColor)));
                System.Drawing.SolidBrush fbrush = new System.Drawing.SolidBrush(
                    System.Drawing.Color.FromArgb((int)Display.Color.ToARGB(fontColor)));

                //test board
                //gfx.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), 0, 0, w - 1, h - 1);

                float[,] offset_8 = {
                    { 0, 0},{ 1, 0},{ 2, 0},
                    { 0, 1},/*1, 1*/{ 2, 1},
                    { 0, 2},{ 1, 2},{ 2, 2} };
                float[,] offset_4 =  { 
                    /*0, 0*/{ 1, 0},/*2, 0*/
                    { 0, 1},/*1, 1*/{ 2, 1},
                    /*0, 2*/{ 1, 2},/*2, 2*/};

                System.Drawing.RectangleF expectRect = new System.Drawing.RectangleF(1f, 1f, w - 1f, h - 1f);

                if (borderTime >= TextBorderStyle.Border_4)
                {
                    DrawString(text, gfx, font, bbrush, expectRect, offset_4[0, 0], offset_4[0, 1]);
                    DrawString(text, gfx, font, bbrush, expectRect, offset_4[1, 0], offset_4[1, 1]);
                    DrawString(text, gfx, font, bbrush, expectRect, offset_4[2, 0], offset_4[2, 1]);
                    DrawString(text, gfx, font, bbrush, expectRect, offset_4[3, 0], offset_4[3, 1]);
                }
                if (borderTime >= TextBorderStyle.Border)
                {
                    DrawString(text, gfx, font, bbrush, expectRect, offset_8[0, 0], offset_8[0, 1]);
                    DrawString(text, gfx, font, bbrush, expectRect, offset_8[1, 0], offset_8[1, 1]);
                    DrawString(text, gfx, font, bbrush, expectRect, offset_8[2, 0], offset_8[2, 1]);
                    DrawString(text, gfx, font, bbrush, expectRect, offset_8[3, 0], offset_8[3, 1]);
                }
                DrawString(text, gfx, font, fbrush, expectRect, 1, 1);
                //DrawString(text, gfx, font, fbrush, expectRect, 1, 1);
            }
            return src;
        }
        public static void DrawString(string text, System.Drawing.Graphics gfx, System.Drawing.Font font, System.Drawing.SolidBrush brush, System.Drawing.RectangleF expectRect, float x, float y)
        {
            gfx.TranslateTransform(x, y);
            gfx.DrawString(text, font, brush, expectRect, Win32Driver.DefaultFormat);
            gfx.TranslateTransform(-x, -y);
        }
        //         public static System.Drawing.Bitmap GenLayoutSnap(UILayoutMeta meta)
        //         {
        //             var layout = UIEditor.Instance.CreateLayout(meta);
        //             return GenLayoutSnap(layout);
        //         }
        //         public static System.Drawing.Bitmap GenLayoutSnap(UILayout layout)
        //         {
        //             if (layout != null && layout.ImageRegion != null)
        //             {
        //                 int w = (int)(layout.ImageRegion.width + layout.ClipSize);
        //                 int h = (int)(layout.ImageRegion.height + layout.ClipSize);
        //                 var bitmap = ((Win32Image)Instance.createRGBImage(w, h)).Src;
        //                 using (var gfx = new Win32Graphics(System.Drawing.Graphics.FromImage(bitmap)))
        //                 {
        //                     layout.Render(gfx, w, h);
        //                 }
        //                 return bitmap;
        //             }
        //             return null;
        //         }

    }
}
