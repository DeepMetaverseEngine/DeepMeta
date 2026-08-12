using DeepCore;
using DeepCore.Geometry;
using DeepCore.GUI.Win32;
using OpenTK;
using System.Collections.Generic;

namespace System.Drawing
{
    public static class GraphicsEXT
    {
        public static StringFormat TextFormat = Win32Driver.DefaultFormat;
        public static void Reset(this System.Drawing.Graphics g)
        {
            g.CompositingMode = (System.Drawing.Drawing2D.CompositingMode.SourceOver);
            g.PageUnit = GraphicsUnit.Pixel;
            g.CompositingQuality = (System.Drawing.Drawing2D.CompositingQuality.HighSpeed);
            g.SmoothingMode = (System.Drawing.Drawing2D.SmoothingMode.None);
            g.InterpolationMode = (System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
            g.PixelOffsetMode = (System.Drawing.Drawing2D.PixelOffsetMode.Half);
        }
        public static void SetClip(this System.Drawing.Graphics g, int x, int y, int w, int h)
        {
            g.SetClip(new Rectangle(x, y, w, h));
        }
        public static void SetClip(this System.Drawing.Graphics g, float x, float y, float w, float h)
        {
            g.SetClip(new RectangleF(x, y, w, h));
        }
        public static void DrawCross(this Graphics g, Pen pen, float cx, float cy, float cr)
        {
            g.DrawLine(pen, cx - cr, cy, cx + cr, cy);
            g.DrawLine(pen, cx, cy - cr, cx, cy + cr);
        }

        public static void DrawFan(this Graphics g, Pen pen, float direction, float range, float angle)
        {
            if (angle != 0)
            {
                float startRadians = direction - angle / 2;
                float endRadians = direction + angle / 2;
                g.DrawArc(pen,
                    -range,
                    -range,
                    range * 2,
                    range * 2,
                    CMath.RadianToAngle(startRadians),
                    CMath.RadianToAngle(angle));
                g.DrawLine(pen, 0, 0,
                    (float)Math.Cos(startRadians) * range,
                    (float)Math.Sin(startRadians) * range);
                g.DrawLine(pen, 0, 0,
                    (float)Math.Cos(endRadians) * range,
                    (float)Math.Sin(endRadians) * range);
            }
            else
            {
                g.DrawLine(pen, 0, 0,
                    (float)Math.Cos(direction) * range,
                    (float)Math.Sin(direction) * range);
            }
        }
        public static void FillFan(this Graphics g, Brush pen, float direction, float range, float angle)
        {
            float startRadians = direction - angle / 2;
            float endRadians = direction + angle / 2;
            g.FillPie(pen,
                -range,
                -range,
                range * 2,
                range * 2,
                CMath.RadianToAngle(startRadians),
                CMath.RadianToAngle(angle));
        }

        /// <summary>
        /// 画粗线条
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="width"></param>
        public static void DrawLineRect(this Graphics g, Pen pen, float sx, float sy, float dx, float dy, float line_r)
        {
            float direction = VectorHelper.GetDegree(sx, sy, dx, dy);
            Vector2 s_l = new Vector2(sx, sy);
            Vector2 s_r = new Vector2(sx, sy);
            Vector2 d_l = new Vector2(dx, dy);
            Vector2 d_r = new Vector2(dx, dy);
            VectorHelper.MovePolar(ref s_l, direction + CMath.PI_DIV_2, line_r);
            VectorHelper.MovePolar(ref d_l, direction + CMath.PI_DIV_2, line_r);
            VectorHelper.MovePolar(ref s_r, direction - CMath.PI_DIV_2, line_r);
            VectorHelper.MovePolar(ref d_r, direction - CMath.PI_DIV_2, line_r);
            g.DrawLine(pen, s_l.X, s_l.Y, d_l.X, d_l.Y);
            g.DrawLine(pen, s_r.X, s_r.Y, d_r.X, d_r.Y);
            g.DrawLine(pen, s_l.X, s_l.Y, s_r.X, s_r.Y);
            g.DrawLine(pen, d_l.X, d_l.Y, d_r.X, d_r.Y);
        }
        public static void FillLineRect(this Graphics g, Brush brush, float sx, float sy, float dx, float dy, float line_r)
        {
            float direction = VectorHelper.GetDegree(sx, sy, dx, dy);
            Vector2 s_l = new Vector2(sx, sy);
            Vector2 s_r = new Vector2(sx, sy);
            Vector2 d_l = new Vector2(dx, dy);
            Vector2 d_r = new Vector2(dx, dy);
            VectorHelper.MovePolar(ref s_l, direction + CMath.PI_DIV_2, line_r);
            VectorHelper.MovePolar(ref d_l, direction + CMath.PI_DIV_2, line_r);
            VectorHelper.MovePolar(ref s_r, direction - CMath.PI_DIV_2, line_r);
            VectorHelper.MovePolar(ref d_r, direction - CMath.PI_DIV_2, line_r);
            g.FillPolygon(brush,
                new PointF[] {
                    new PointF(s_l.X, s_l.Y),
                    new PointF(s_r.X, s_r.Y),
                    new PointF(d_r.X, d_r.Y),
                    new PointF(d_l.X, d_l.Y)
                },
                System.Drawing.Drawing2D.FillMode.Winding);
        }


        /// <summary>
        /// 画圆角粗线条
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="size"></param>
        public static void DrawLineRoundRect(this Graphics g, Pen pen, float sx, float sy, float dx, float dy, float line_r)
        {
            float direction = VectorHelper.GetDegree(sx, sy, dx, dy);
            float r = line_r;
            float size = r * 2;
            Vector2 s_l = new Vector2(sx, sy);
            Vector2 s_r = new Vector2(sx, sy);
            Vector2 d_l = new Vector2(dx, dy);
            Vector2 d_r = new Vector2(dx, dy);
            VectorHelper.MovePolar(ref s_l, direction + CMath.PI_DIV_2, r);
            VectorHelper.MovePolar(ref d_l, direction + CMath.PI_DIV_2, r);
            VectorHelper.MovePolar(ref s_r, direction - CMath.PI_DIV_2, r);
            VectorHelper.MovePolar(ref d_r, direction - CMath.PI_DIV_2, r);
            g.DrawLine(pen, s_l.X, s_l.Y, d_l.X, d_l.Y);
            g.DrawLine(pen, s_r.X, s_r.Y, d_r.X, d_r.Y);
            float angle = CMath.RadianToAngle(direction);
            g.DrawArc(pen, sx - r, sy - r, size, size, angle + 180 - 90, 180);
            g.DrawArc(pen, dx - r, dy - r, size, size, angle + 360 - 90, 180);
        }

        public static void FillLineRoundRect(this Graphics g, Brush brush, float sx, float sy, float dx, float dy, float line_r)
        {
            float direction = VectorHelper.GetDegree(sx, sy, dx, dy);
            float r = line_r;
            float size = r * 2;
            Vector2 s_l = new Vector2(sx, sy);
            Vector2 s_r = new Vector2(sx, sy);
            Vector2 d_l = new Vector2(dx, dy);
            Vector2 d_r = new Vector2(dx, dy);
            VectorHelper.MovePolar(ref s_l, direction + CMath.PI_DIV_2, r);
            VectorHelper.MovePolar(ref d_l, direction + CMath.PI_DIV_2, r);
            VectorHelper.MovePolar(ref s_r, direction - CMath.PI_DIV_2, r);
            VectorHelper.MovePolar(ref d_r, direction - CMath.PI_DIV_2, r);
            g.FillPolygon(brush,
                new PointF[] {
                    new PointF(s_l.X, s_l.Y),
                    new PointF(s_r.X, s_r.Y),
                    new PointF(d_r.X, d_r.Y),
                    new PointF(d_l.X, d_l.Y)
                },
                System.Drawing.Drawing2D.FillMode.Winding);
            float angle = CMath.RadianToAngle(direction);
            g.FillPie(brush, sx - r, sy - r, size, size, angle + 180 - 90, 180);
            g.FillPie(brush, dx - r, dy - r, size, size, angle + 360 - 90, 180);
        }

        public static PointF[] GenCursor(float sx, float sy, float dx, float dy, float width, float cursor_width, float cursor_height)
        {
            if (cursor_width < width)
                cursor_width = width;

            float ox = sx - dx;
            float oy = sy - dy;
            float ds = CMath.GetDistance(sx, sy, dx, dy);
            float hw = width / 2f;
            float cw = cursor_width / 2;
            float od = (float)Math.Atan2(oy, ox);

            PointF sC = new PointF(0, 0);
            PointF sL = new PointF(-hw, 0);
            PointF sR = new PointF(+hw, 0);

            PointF dC = new PointF(0, ds);
            PointF dL = new PointF(-hw, ds - cursor_height);
            PointF dR = new PointF(+hw, ds - cursor_height);
            PointF dLL = new PointF(-cw, ds - cursor_height);
            PointF dRR = new PointF(+cw, ds - cursor_height);

            PointF[] points = new PointF[] { sC, sL, dL, dLL, dC, dRR, dR, sR, sC };

            for (int i = 0; i < points.Length; i++)
            {
                float x = points[i].X;
                float y = points[i].Y;
                VectorHelper.Rotate(ref x, ref y, 0, 0, od + CMath.PI_DIV_2);
                points[i].X = x + sx;
                points[i].Y = y + sy;
            }
            return points;
        }

        public static void FillCursor(this Graphics g, Brush brush, float sx, float sy, float dx, float dy, float width, float cursor_width = 0, float cursor_height = 0)
        {
            PointF[] points = GenCursor(sx, sy, dx, dy, width, cursor_width, cursor_height);
            var gs = g.Save();
            try
            {
                g.FillPolygon(brush, points);
            }
            finally
            {
                g.Restore(gs);
            }
        }
        public static void DrawCursor(this Graphics g, Pen pen, float sx, float sy, float dx, float dy, float width, float cursor_width = 0, float cursor_height = 0)
        {
            PointF[] points = GenCursor(sx, sy, dx, dy, width, cursor_width, cursor_height);
            var gs = g.Save();
            try
            {
                g.DrawPolygon(pen, points);
            }
            finally
            {
                g.Restore(gs);
            }
        }

        public static void DrawStringAlignment(this Graphics gfx, string text, Font font, Brush bodyBrush, DeepCore.GUI.Data.AlignmentStyle anchor, System.Drawing.RectangleF expectRect)
        {
            var bounds = gfx.MeasureString(text, font);
            switch (anchor)
            {
                case DeepCore.GUI.Data.AlignmentStyle.TopLeft:
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.MiddleLeft:
                    expectRect.Y = (expectRect.Y + expectRect.Height / 2) - bounds.Height / 2;
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.BottomLeft:
                    expectRect.Y = (expectRect.Y + expectRect.Height) - bounds.Height;
                    break;

                case DeepCore.GUI.Data.AlignmentStyle.TopCenter:
                    expectRect.X = (expectRect.X + expectRect.Width / 2) - bounds.Width / 2;
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.MiddleCenter:
                    expectRect.X = (expectRect.X + expectRect.Width / 2) - bounds.Width / 2;
                    expectRect.Y = (expectRect.Y + expectRect.Height / 2) - bounds.Height / 2;
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.BottomCenter:
                    expectRect.X = (expectRect.X + expectRect.Width / 2) - bounds.Width / 2;
                    expectRect.Y = (expectRect.Y + expectRect.Height) - bounds.Height;
                    break;

                case DeepCore.GUI.Data.AlignmentStyle.TopRight:
                    expectRect.X = (expectRect.X + expectRect.Width) - bounds.Width;
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.MiddleRight:
                    expectRect.X = (expectRect.X + expectRect.Width) - bounds.Width;
                    expectRect.Y = (expectRect.Y + expectRect.Height / 2) - bounds.Height / 2;
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.BottomRight:
                    expectRect.X = (expectRect.X + expectRect.Width) - bounds.Width;
                    expectRect.Y = (expectRect.Y + expectRect.Height) - bounds.Height;
                    break;
            }
            gfx.DrawString(text, font, bodyBrush, expectRect);
        }


        public static SizeF MeasureBoundsString(this Graphics gfx, string text, Font font, float bsize = 2)
        {
            var bounds = gfx.MeasureString(text, font);
            bounds.Width += bsize;
            bounds.Height += bsize;
            return bounds;
        }
        public static SizeF MeasureBoundsString(this Graphics gfx, string text, Font font, int width, float bsize = 2)
        {
            var bounds = gfx.MeasureString(text, font, width);
            bounds.Width += bsize;
            bounds.Height += bsize;
            return bounds;
        }
        public static SizeF DrawStringBounds(this Graphics gfx, string text, Font font, Brush bodyBrush, Brush borderBrush,
            DeepCore.GUI.Data.TextBorderStyle borderTime,
            float x, float y)
        {
            return DrawStringBounds(gfx, text, font, bodyBrush, borderBrush, borderTime, DeepCore.GUI.Data.AlignmentStyle.TopLeft, x, y);
        }
        public static SizeF DrawStringBounds(this Graphics gfx, string text, Font font, Brush bodyBrush, Brush borderBrush,
            DeepCore.GUI.Data.TextBorderStyle borderTime,
            DeepCore.GUI.Data.AlignmentStyle anchor,
            float x, float y)
        {
            var rect = new System.Drawing.RectangleF(x, y, 0, 0);
            return DrawStringBounds(gfx, text, font, bodyBrush, borderBrush, borderTime, anchor, rect);
        }
        public static SizeF DrawStringBounds(this Graphics gfx, string text, Font font, Brush bodyBrush, Brush borderBrush,
            DeepCore.GUI.Data.TextBorderStyle borderTime,
            DeepCore.GUI.Data.AlignmentStyle anchor,
            System.Drawing.RectangleF expectRect)
        {
            var bounds = gfx.MeasureBoundsString(text, font, (int)(expectRect.Width));
            if ((anchor & DeepCore.GUI.Data.AlignmentStyle.MASK_MIDDLE) != 0)
            {
                expectRect.Y = (expectRect.Y + expectRect.Height / 2) - bounds.Height / 2;
            }
            else if ((anchor & DeepCore.GUI.Data.AlignmentStyle.MASK_BOTTOM) != 0)
            {
                expectRect.Y = (expectRect.Y + expectRect.Height) - bounds.Height;
            }
            if ((anchor & DeepCore.GUI.Data.AlignmentStyle.MASK_CENTER) != 0)
            {
                expectRect.X = (expectRect.X + expectRect.Width / 2) - bounds.Width / 2;
            }
            else if ((anchor & DeepCore.GUI.Data.AlignmentStyle.MASK_RIGHT) != 0)
            {
                expectRect.X = (expectRect.X + expectRect.Width) - bounds.Width;
            }
            var bbrush = borderBrush;
            var fbrush = bodyBrush;
            var bt = borderTime;
            switch (bt)
            {
                case DeepCore.GUI.Data.TextBorderStyle.Border_4:
                    for (int i = 0; i < 4; i++)
                    {
                        DrawString(gfx, text, font, bbrush, offset_4[i, 0], offset_4[i, 1], expectRect);
                    }
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Border:
                    for (int i = 0; i < 8; i++)
                    {
                        DrawString(gfx, text, font, bbrush, offset_8[i, 0], offset_8[i, 1], expectRect);
                    }
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow:
                    DrawString(gfx, text, font, bbrush, 1, 2, expectRect);
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_L_T:
                    DrawString(gfx, text, font, bbrush, 0, 0, expectRect);
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_C_T:
                    DrawString(gfx, text, font, bbrush, 1, 0, expectRect);
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_R_T:
                    DrawString(gfx, text, font, bbrush, 2, 0, expectRect);
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_L_C:
                    DrawString(gfx, text, font, bbrush, 0, 1, expectRect);
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_C_C:
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_R_C:
                    DrawString(gfx, text, font, bbrush, 2, 1, expectRect);
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_L_B:
                    DrawString(gfx, text, font, bbrush, 0, 2, expectRect);
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_C_B:
                    DrawString(gfx, text, font, bbrush, 1, 2, expectRect);
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_R_B:
                    DrawString(gfx, text, font, bbrush, 2, 2, expectRect);
                    break;
            }
            DrawString(gfx, text, font, fbrush, 1, 1, expectRect);
            return bounds;
        }

        private static void DrawString(this Graphics gfx, string text, Font font, Brush brush, float ox, float oy, RectangleF rect)
        {
            gfx.TranslateTransform(ox, oy);
            //if (rect.Width <= 0 || rect.Height <= 0)
            {
                gfx.DrawString(text, font, brush, rect.X, rect.Y, TextFormat);
            }
//             else
//             {
//                 gfx.DrawString(text, font, brush, rect, TextFormat);
//             }
            gfx.TranslateTransform(-ox, -oy);
        }

        public static void DrawGridLines(this Graphics gfx, Pen color, float startX, float startY, float gridW, float gridH, int xcount, int ycount)
        {
            gfx.TranslateTransform(startX, startY);
            var tw = xcount * gridW;
            var th = ycount * gridH;
            for (var x = 0; x <= xcount; x++)
            {
                float dx = x * gridW;
                gfx.DrawLine(color, dx, 0, dx, th);
            }
            for (var y = 0; y <= ycount; y++)
            {
                float dy = y * gridH;
                gfx.DrawLine(color, 0, dy, tw, dy);
            }
            gfx.TranslateTransform(-startX, -startY);
        }


        #region constans
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
        #endregion

        #region color
        public static Color InverseColor(this Color c)
        {
            return Color.FromArgb(c.A, (byte)(255 - c.R), (byte)(255 - c.G), (byte)(255 - c.B));
        }
        public static Color SetAlpha(this Color c, float a)
        {
            return Color.FromArgb((byte)(255 * a), c);
        }
        public static Color ToAlpha(this Color src, float alpha)
        {
            return Color.FromArgb(Math.Min((int)(src.A * alpha), 255), src);
        }
        public static System.Drawing.Color ToDark(this Color src, float dark)
        {
            return Color.FromArgb(src.A,
                Math.Min((int)(src.R * dark), 255),
                Math.Min((int)(src.G * dark), 255),
                Math.Min((int)(src.B * dark), 255));
        }
        public static System.Drawing.Color ToAlphaAndDark(this Color src, float alpha, float dark)
        {
            return Color.FromArgb(
                Math.Min((int)(src.A * alpha), 255),
                Math.Min((int)(src.R * dark), 255),
                Math.Min((int)(src.G * dark), 255),
                Math.Min((int)(src.B * dark), 255));
        }
        public class StackSolidBrush : Disposable
        {
            public readonly SolidBrush Brush;
            public StackSolidBrush(SolidBrush bBrush)
            {
                this.Brush = bBrush;
            }
            protected override void Disposing()
            {
                colorBrushes.Push(this);
            }
            public static implicit operator SolidBrush(in StackSolidBrush value)
            {
                return value.Brush;
            }
        }

        private static Stack<StackSolidBrush> colorBrushes = new Stack<StackSolidBrush>();
        public static StackSolidBrush GetCachedSolidBrushScope(this Color color)
        {
            if (colorBrushes.Count > 0)
            {
                var b = colorBrushes.Pop();
                b.Brush.Color = color;
                return b;
            }
            else
            {
                return new StackSolidBrush(new SolidBrush(color));
            }
        }
        #endregion


    }
}
