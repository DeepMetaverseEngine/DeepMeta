using DeepCore;
using DeepCore.GUI.Win32;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Drawing;
using System.Drawing.Text;

namespace DeepEditor.Common.G3D
{
    public class GLFonts
    {
        private Bitmap s_testBuffer;
        private Graphics s_testGFX;
        private FontFamily s_testFontFamily;
        private PrivateFontCollection s_loadFonts;
        private static float[,] offset_8 =
        {
                    { 0, 0},{ 1, 0},{ 2, 0},
                    { 0, 1},/*1, 1*/{ 2, 1},
                    { 0, 2},{ 1, 2},{ 2, 2}
        };
        private static float[,] offset_4 =
        {
                    /*0, 0*/{ 1, 0},/*2, 0*/
                    { 0, 1},/*1, 1*/{ 2, 1},
                    /*0, 2*/{ 1, 2},/*2, 2*/
        };
        private static GLFonts s_Instance;
        public static GLFonts Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (typeof(GLFonts))
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new GLFonts();
                        }
                    }
                }
                return s_Instance;
            }
        }
        private GLFonts()
        {
            try
            {
                s_testBuffer = new System.Drawing.Bitmap(100, 100);
                s_testGFX = System.Drawing.Graphics.FromImage(s_testBuffer);
                s_testFontFamily = new System.Drawing.FontFamily("Microsoft YaHei UI");
                s_loadFonts = new PrivateFontCollection();
            }
            catch { }
        }
        public void LoadFont(string filepath)
        {
            try
            {
                s_loadFonts.AddFontFile(filepath);
                s_testFontFamily = s_loadFonts.Families[0];
            }
            catch { }
        }
        public void LoadFontBinary(byte[] data)
        {
            try
            {
                s_loadFonts.AddMemoryFont(CUtils.ToIntPtr(data), data.Length);
                s_testFontFamily = s_loadFonts.Families[0];
            }
            catch { }
        }

        public System.Drawing.Font CreateFont(float size, System.Drawing.FontStyle style)
        {
            if (size == 0)
            {
                size = 12;
            }
            return new System.Drawing.Font(s_testFontFamily, size, style, System.Drawing.GraphicsUnit.Pixel, 137);
        }

        public System.Drawing.SizeF GetTextBounds(string text, System.Drawing.Font font, int borderTime, float expectWidth = 0)
        {
            System.Drawing.SizeF size;
            if (expectWidth > 0)
            {
                size = s_testGFX.MeasureString(text, font, (int)(expectWidth), Win32Driver.DefaultFormat);
            }
            else
            {
                size = s_testGFX.MeasureString(text, font, int.MaxValue, Win32Driver.DefaultFormat);
            }
            size.Width = (int)Math.Ceiling(size.Width + 3f);
            size.Height = (int)Math.Ceiling(size.Height + 3f);
            return size;
        }

        public System.Drawing.Bitmap GenStringBuffer(int w, int h, string text, Font font, Color4 fontColor, int borderTime, Color4 borderColor, Color4 backColor, Color4 backBorderColor)
        {
            var src = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var gfx = System.Drawing.Graphics.FromImage(src))
            {
                //                 gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                //                 gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                //                 gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //                 gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                //                 gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                var expectRect = new RectangleF(1f, 1f, w - 1f, h - 1f);
                var fbrush = new SolidBrush(Color.FromArgb(
                    (int)(fontColor.A * 255),
                    (int)(fontColor.R * 255),
                    (int)(fontColor.G * 255),
                    (int)(fontColor.B * 255)));
                if (backColor.A != 0)
                {
                    var bbrush = new SolidBrush(Color.FromArgb(
                        (int)(backColor.A * 255),
                        (int)(backColor.R * 255),
                        (int)(backColor.G * 255),
                        (int)(backColor.B * 255)));
                    gfx.FillRectangle(bbrush, 0, 0, w, h);
                }
                if (backBorderColor.A != 0)
                {
                    var bbrush = new Pen(Color.FromArgb(
                        (int)(backBorderColor.A * 255),
                        (int)(backBorderColor.R * 255),
                        (int)(backBorderColor.G * 255),
                        (int)(backBorderColor.B * 255)));
                    gfx.DrawRectangle(bbrush, 0, 0, w - 1, h - 1);
                }
                if (borderColor.A != 0)
                {
                    var bbrush = new SolidBrush(Color.FromArgb(
                        (int)(borderColor.A * 255),
                        (int)(borderColor.R * 255),
                        (int)(borderColor.G * 255),
                        (int)(borderColor.B * 255)));
                    var bt = (DeepCore.GUI.Data.TextBorderStyle)borderTime;
                    switch (bt)
                    {
                        case DeepCore.GUI.Data.TextBorderStyle.Border_4:
                            for (int i = 0; i < 4; i++)
                            {
                                DrawString(text, gfx, font, bbrush, expectRect, offset_4[i, 0], offset_4[i, 1]);
                            }
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Border:
                            for (int i = 0; i < 8; i++)
                            {
                                DrawString(text, gfx, font, bbrush, expectRect, offset_8[i, 0], offset_8[i, 1]);
                            }
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Shadow:
                            DrawString(text, gfx, font, bbrush, expectRect, 1, 2);
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Shadow_L_T:
                            DrawString(text, gfx, font, bbrush, expectRect, 0, 0);
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Shadow_C_T:
                            DrawString(text, gfx, font, bbrush, expectRect, 1, 0);
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Shadow_R_T:
                            DrawString(text, gfx, font, bbrush, expectRect, 2, 0);
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Shadow_L_C:
                            DrawString(text, gfx, font, bbrush, expectRect, 0, 1);
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Shadow_C_C:
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Shadow_R_C:
                            DrawString(text, gfx, font, bbrush, expectRect, 2, 1);
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Shadow_L_B:
                            DrawString(text, gfx, font, bbrush, expectRect, 0, 2);
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Shadow_C_B:
                            DrawString(text, gfx, font, bbrush, expectRect, 1, 2);
                            break;
                        case DeepCore.GUI.Data.TextBorderStyle.Shadow_R_B:
                            DrawString(text, gfx, font, bbrush, expectRect, 2, 2);
                            break;
                    }
                }
                //test board
                //gfx.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), 0, 0, w - 1, h - 1);

                DrawString(text, gfx, font, fbrush, expectRect, 1, 1);
            }
            return src;
        }

        private void DrawString(string text, Graphics gfx, Font font, SolidBrush brush, RectangleF expectRect, float x, float y)
        {
            gfx.TranslateTransform(x, y);
            gfx.DrawString(text, font, brush, expectRect, Win32Driver.DefaultFormat);
            gfx.TranslateTransform(-x, -y);
        }

        //-------------------------------------------------------------------------------------------------------------------

    }
}
