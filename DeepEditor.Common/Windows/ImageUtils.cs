using DeepCore.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DeepEditor.Common
{
    public static class ImageUtils
    {

        public static System.Drawing.Imaging.ImageFormat GetImageFormat(string type)
        {
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;
            //
            if (type.Equals("png", StringComparison.CurrentCultureIgnoreCase))
                format = System.Drawing.Imaging.ImageFormat.Png;
            if (type.Equals("bmp", StringComparison.CurrentCultureIgnoreCase))
                format = System.Drawing.Imaging.ImageFormat.Bmp;
            if (type.Equals("jpg", StringComparison.CurrentCultureIgnoreCase))
                format = System.Drawing.Imaging.ImageFormat.Jpeg;
            if (type.Equals("gif", StringComparison.CurrentCultureIgnoreCase))
                format = System.Drawing.Imaging.ImageFormat.Gif;
            return format;
        }

        public static System.Drawing.Bitmap PremultiplyAlpha(this System.Drawing.Bitmap dimg)
        {
            return PremultiplyAlpha(dimg, System.Drawing.Color.FromArgb(0, 0, 0, 0));
        }
        public static System.Drawing.Bitmap PremultiplyAlpha(this System.Drawing.Bitmap dimg, System.Drawing.Color backcolor)
        {
            var image = new System.Drawing.Bitmap(dimg.Width, dimg.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            if (backcolor.A != 0)
            {
                var gfx = System.Drawing.Graphics.FromImage(image);
                gfx.FillRectangle(new System.Drawing.SolidBrush(backcolor), 0, 0, image.Width, image.Height);
                gfx.DrawImage(dimg, 0, 0);
                for (var x = image.Width - 1; x >= 0; --x)
                {
                    for (var y = image.Height - 1; y >= 0; --y)
                    {
                        if (backcolor.A != 0)
                        {
                            var spix = dimg.GetPixel(x, y);
                            var dpix = image.GetPixel(x, y);
                            dpix = System.Drawing.Color.FromArgb(spix.A, dpix);
                            if (dpix.A != byte.MaxValue)
                            {
                                dpix = PremultiplyAlpha(dpix);
                            }
                            image.SetPixel(x, y, dpix);
                        }
                        else
                        {
                            var spix = dimg.GetPixel(x, y);
                            if (spix.A != byte.MaxValue)
                            {
                                spix = PremultiplyAlpha(spix);
                            }
                            image.SetPixel(x, y, spix);
                        }
                    }
                }
            }
            else
            {
                var black = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                for (var x = image.Width - 1; x >= 0; --x)
                {
                    for (var y = image.Height - 1; y >= 0; --y)
                    {
                        image.SetPixel(x, y, black);
                    }
                }
                var gfx = System.Drawing.Graphics.FromImage(image);
                gfx.DrawImage(dimg, 0, 0);
            }
            return image;
        }
        public static System.Drawing.Color PremultiplyAlpha(this System.Drawing.Color pixel)
        {
            return System.Drawing.Color.FromArgb(
                PremultiplyAlpha_Component(pixel.A, pixel.A),
                PremultiplyAlpha_Component(pixel.R, pixel.A),
                PremultiplyAlpha_Component(pixel.G, pixel.A),
                PremultiplyAlpha_Component(pixel.B, pixel.A));
        }
        private static byte PremultiplyAlpha_Component(byte source, byte alpha)
        {
            return (byte)((source * alpha) / byte.MaxValue);
        }

        public static System.Drawing.Bitmap AsBitmap(this System.Drawing.Image src, bool clone = false)
        {
            if (!clone && src is System.Drawing.Bitmap)
            {
                return (System.Drawing.Bitmap)src;
            }
            return Clone(src, src.Width, src.Height);
        }
        public static System.Drawing.Bitmap ToSingleColor(this System.Drawing.Image src, Color color)
        {
            var image = Clone(src);
            for (var x = image.Width - 1; x >= 0; --x)
            {
                for (var y = image.Height - 1; y >= 0; --y)
                {
                    var sc = image.GetPixel(x, y);
                    image.SetPixel(x, y, Color.FromArgb(sc.A, color.R, color.G, color.B));
                }
            }
            return image;
        }

        public static System.Drawing.Bitmap Clone(this System.Drawing.Image src)
        {
            return Clone(src, src.Width, src.Height);
        }
        public static System.Drawing.Bitmap Clone(this System.Drawing.Image src, int width, int height)
        {
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(
                src,
                new System.Drawing.Rectangle(0, 0, width, height),
                new System.Drawing.Rectangle(0, 0, src.Width, src.Height),
                System.Drawing.GraphicsUnit.Pixel);
            return image;
        }

        public static Icon ToIcon(this System.Drawing.Image src)
        {
            Bitmap theBitmap = new Bitmap(src, new Size(src.Width, src.Height));
            IntPtr Hicon = theBitmap.GetHicon();// Get an Hicon for myBitmap.
            Icon newIcon = Icon.FromHandle(Hicon);// Create a new icon from the handle.
           // FileStream fs = new FileStream(@"c:\Icon\" + filename + ".ico", FileMode.OpenOrCreate);//Write Icon to File Stream
           return newIcon;
        }
    }

    public static class SystemIcons
    {

        [DllImport("Shell32.dll")]
        private extern static int ExtractIconEx(string libName, int iconIndex, IntPtr[] largeIcon, IntPtr[] smallIcon, int nIcons);

        private static Bitmap[] largeIcons;
        private static Bitmap[] smallIcons;

        private static void LoadSystemIcon()
        {
            if (largeIcons == null)
            {
                var largeIcon = new IntPtr[Count];
                var smallIcon = new IntPtr[Count];
                largeIcons = new Bitmap[Count];
                smallIcons = new Bitmap[Count];
                ExtractIconEx("shell32.dll", 0, largeIcon, smallIcon, Count);
                for (int i = 0; i < Count; ++i)
                {
                    try { largeIcons[i] = Icon.FromHandle(largeIcon[i]).ToBitmap(); } catch { }
                    try { smallIcons[i] = Icon.FromHandle(smallIcon[i]).ToBitmap(); } catch { }
                }
            }
        }

        public static int Count { get => 250; }

        public static List<Bitmap> GetLargeIcons()
        {
            LoadSystemIcon();
            return new List<Bitmap>(largeIcons);
        }
        public static List<Bitmap> GetSmallIcons()
        {
            LoadSystemIcon();
            return new List<Bitmap>(smallIcons);
        }
        public static Bitmap GetLargeIcon(int index)
        {
            LoadSystemIcon();
            index = Math.Max(index, 0);
            index = Math.Min(index, largeIcons.Length - 1);
            return largeIcons[index];
        }
        public static Bitmap GetSmallIcon(int index)
        {
            LoadSystemIcon();
            index = Math.Max(index, 0);
            index = Math.Min(index, smallIcons.Length - 1);
            return smallIcons[index];
        }
        public static void SaveAll(DirectoryInfo dir)
        {
            CFiles.CreateDir(dir);
            int index = 0;
            foreach (var icon in GetLargeIcons())
            {
                if (icon != null)
                {
                    icon.Save(dir.FullName + "\\" + index + ".png", ImageFormat.Png);
                }
                index++;
            }
            foreach (var icon in GetSmallIcons())
            {
                if (icon != null)
                {
                    icon.Save(dir.FullName + "\\" + index + ".png", ImageFormat.Png);
                }
                index++;
            }
        }
    }
}
