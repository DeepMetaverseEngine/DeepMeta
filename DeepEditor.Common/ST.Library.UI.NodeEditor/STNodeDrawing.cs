using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Library.UI.NodeEditor
{
    public class STNodeDrawing
    {
        public virtual Image CreateBorderImage(Color clr, int bsize = 12)
        {
            Image img = new Bitmap(bsize, bsize);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                using (GraphicsPath gp = new GraphicsPath())
                {
                    gp.AddEllipse(new Rectangle(0, 0, bsize - 1, bsize - 1));
                    using (PathGradientBrush b = new PathGradientBrush(gp))
                    {
                        b.CenterColor = Color.FromArgb(200, clr);
                        b.SurroundColors = new Color[] { Color.FromArgb(10, clr) };
                        g.FillPath(b, gp);
                    }
                }
            }
            return img;
        }
        public virtual Image CreateSolidImage(Color clr, int bsize = 12)
        {
            Image img = new Bitmap(bsize, bsize);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.FillRectangle(new SolidBrush(clr), 0, 0, bsize, bsize);
            }
            return img;
        }
        public virtual void RenderBorder(Graphics g, Rectangle rect, Image img, int bsize = 5)
        {
            bsize = Math.Min(bsize, img.Width / 2 - 1);
            var size = bsize * 2;
            var clip = bsize;
            //填充四个角
            g.DrawImage(img,
                new Rectangle(rect.X - clip, rect.Y - clip, clip, clip),
                new Rectangle(0, 0, clip, clip), GraphicsUnit.Pixel);
            g.DrawImage(img,
                new Rectangle(rect.Right, rect.Y - clip, clip, clip),
                new Rectangle(img.Width - clip, 0, clip, clip), GraphicsUnit.Pixel);
            g.DrawImage(img,
                new Rectangle(rect.X - clip, rect.Bottom, clip, clip),
                new Rectangle(0, img.Height - clip, clip, clip), GraphicsUnit.Pixel);
            g.DrawImage(img,
                new Rectangle(rect.Right, rect.Bottom, clip, clip),
                new Rectangle(img.Width - clip, img.Height - clip, clip, clip), GraphicsUnit.Pixel);
            //四边
            g.DrawImage(img,
                new Rectangle(rect.X - clip, rect.Y, clip, rect.Height),
                new Rectangle(0, clip, clip, img.Height - size), GraphicsUnit.Pixel);
            g.DrawImage(img,
                new Rectangle(rect.X, rect.Y - clip, rect.Width, clip),
                new Rectangle(clip, 0, img.Width - size, clip), GraphicsUnit.Pixel);
            g.DrawImage(img,
                new Rectangle(rect.Right, rect.Y, clip, rect.Height),
                new Rectangle(img.Width - clip, clip, clip, img.Height - size), GraphicsUnit.Pixel);
            g.DrawImage(img,
                new Rectangle(rect.X, rect.Bottom, rect.Width, clip),
                new Rectangle(clip, img.Height - clip, img.Width - size, clip), GraphicsUnit.Pixel);
        }


        public float Curvature = 0.3f;

        public virtual void DrawBezier(Graphics g, Pen p, PointF ptStart, PointF ptEnd, float f)
        {
            this.DrawBezier(g, p, ptStart.X, ptStart.Y, ptEnd.X, ptEnd.Y, f);
        }

        public virtual void DrawBezier(Graphics g, Pen p, float x1, float y1, float x2, float y2, float f)
        {
            float n = (Math.Abs(x1 - x2) * f);
            if (this.Curvature != 0 && n < 30) n = 30;
            g.DrawBezier(p,
                x1, y1,
                x1 + n, y1,
                x2 - n, y2,
                x2, y2);
        }

        public virtual GraphicsPath CreateBezierPath(float x1, float y1, float x2, float y2, float f)
        {
            GraphicsPath gp = new GraphicsPath();
            float n = (Math.Abs(x1 - x2) * f);
            if (this.Curvature != 0 && n < 30) n = 30;
            gp.AddBezier(
                x1, y1,
                x1 + n, y1,
                x2 - n, y2,
                x2, y2
                );
            return gp;
        }
    }
}
