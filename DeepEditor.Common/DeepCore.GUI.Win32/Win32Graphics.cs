using DeepCore.Geometry;
using DeepCore.GUI.Data;
using DeepCore.GUI.Display;
using System;
using System.Collections.Generic;
using System.Drawing;
using Graphics = DeepCore.GUI.Display.Graphics;
using Image = DeepCore.GUI.Display.Image;
using RectangleF = DeepCore.Geometry.RectangleF;
using Vector2 = DeepCore.Geometry.Vector2;

namespace DeepCore.GUI.Win32
{
    public class Win32Graphics : Graphics
    {
        private const float ANGLE_90 = (float)(90);
        private const float ANGLE_180 = (float)(180);
        private const float ANGLE_270 = (float)(270);

        internal readonly System.Drawing.Graphics gfx;
        internal readonly System.Drawing.Drawing2D.Matrix gfx_init;
        //-----------------------------------------------------------------------
        public System.Drawing.Graphics G { get => gfx; }
        public Win32Graphics(System.Drawing.Graphics gfx, bool smooth = false)
        {
            if (smooth)
            {
                gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            }
            this.gfx = gfx;
            this.gfx_init = gfx.Transform;
        }
        public override void Dispose()
        {
            this.gfx.Dispose();
        }
        //-----------------------------------------------------------------------
        #region Transform
        private Stack<System.Drawing.Drawing2D.Matrix> stack_trans = new Stack<System.Drawing.Drawing2D.Matrix>();
        private void transAnchor(AlignmentStyle anchor, float w, float h)
        {
            float tx = 0, ty = 0;
            if ((anchor & AlignmentStyle.MASK_CENTER) != 0)
            {
                tx = -w / 2;
            }
            else if ((anchor & AlignmentStyle.MASK_RIGHT) != 0)
            {
                tx = -w;
            }

            if ((anchor & AlignmentStyle.MASK_MIDDLE) != 0)
            {
                ty = -h / 2;
            }
            else if ((anchor & AlignmentStyle.MASK_BOTTOM) != 0)
            {
                ty = -h;
            }
            gfx.TranslateTransform(tx, ty);
        }
        private void transAnchor(AlignmentStyle anchor, float sw, float sh, float dw, float dh)
        {
            float dx = 0;
            float dy = 0;
            switch (anchor)
            {
                case AlignmentStyle.TopLeft:
                    break;
                case AlignmentStyle.TopCenter:
                    dx = (dw - sw) / 2;
                    break;
                case AlignmentStyle.TopRight:
                    dx = (dw - sw);
                    break;
                case AlignmentStyle.MiddleLeft:
                    dy = (dh - sh) / 2;
                    break;
                case AlignmentStyle.MiddleCenter:
                    dx = (dw - sw) / 2;
                    dy = (dh - sh) / 2;
                    break;
                case AlignmentStyle.MiddleRight:
                    dx = (dw - sw);
                    dy = (dh - sh) / 2;
                    break;
                case AlignmentStyle.BottomLeft:
                    dy = (dh - sh);
                    break;
                case AlignmentStyle.BottomCenter:
                    dx = (dw - sw) / 2;
                    dy = (dh - sh);
                    break;
                case AlignmentStyle.BottomRight:
                    dx = (dw - sw);
                    dy = (dh - sh);
                    break;
            }
            gfx.TranslateTransform(dx, dy);
        }
        private void transTans(Trans trans, float w, float h)
        {
            switch (trans)
            {
                case Trans.TRANS_ROT90:
                    gfx.TranslateTransform(h, 0, 0);
                    gfx.RotateTransform(ANGLE_90);
                    break;

                case Trans.TRANS_ROT180:
                    gfx.TranslateTransform(w, h, 0);
                    gfx.RotateTransform(ANGLE_180);
                    break;

                case Trans.TRANS_ROT270:
                    gfx.TranslateTransform(0, w, 0);
                    gfx.RotateTransform(ANGLE_270);
                    break;

                case Trans.TRANS_MIRROR:
                    gfx.TranslateTransform(w, 0);
                    gfx.ScaleTransform(-1, 1);
                    break;

                case Trans.TRANS_MIRROR_ROT90:
                    gfx.TranslateTransform(h, 0);
                    gfx.RotateTransform(ANGLE_90);
                    gfx.TranslateTransform(w, 0);
                    gfx.ScaleTransform(-1, 1);
                    break;

                case Trans.TRANS_MIRROR_ROT180:
                    gfx.TranslateTransform(w, h);
                    gfx.RotateTransform(ANGLE_180);
                    gfx.TranslateTransform(w, 0, 0);
                    gfx.ScaleTransform(-1, 1);
                    break;

                case Trans.TRANS_MIRROR_ROT270:
                    gfx.TranslateTransform(0, w);
                    gfx.RotateTransform(ANGLE_270);
                    gfx.TranslateTransform(w, 0);
                    gfx.ScaleTransform(-1, 1);
                    break;
            }
        }
        public override void MultiplyTransform(in Matrix m)
        {
            //var wt = new System.Drawing.Drawing2D.Matrix(m.M11, m.M12, m.M21, m.M22, m.M41, m.M42);
            var wt = new System.Drawing.Drawing2D.Matrix(m.M11, m.M12, m.M21, m.M22, m.M41, m.M42);
            //             var mt = m.Translation;
            //             var ms = m.Scale;
            //             var mr = m.Rotation;
            //             wt.Translate(mt.X, mt.Y); 
            //             wt.Scale(ms.X, ms.Y);
            //             wt.Rotate(CMath.To360(mr.Z));
            gfx.MultiplyTransform(wt);

        }
        public override void Translate(in Vector2 pos)
        {
            gfx.TranslateTransform(pos.X, pos.Y);

        }
        public override void Rotate(float angle)
        {
            gfx.RotateTransform(CMath.RadianToAngle(angle));

        }
        public override void Scale(in Vector2 scale)
        {
            gfx.ScaleTransform(scale.X, scale.Y);

        }
        public override void PushTransform()
        {
            stack_trans.Push(gfx.Transform);

        }
        public override void PopTransform()
        {
            if (stack_trans.Count > 0)
            {
                System.Drawing.Drawing2D.Matrix mtx = stack_trans.Pop();
                gfx.Transform = mtx;
            }

        }
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------
        #region Status

        private System.Drawing.SolidBrush cur_brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        private System.Drawing.Pen cur_pen = new System.Drawing.Pen(System.Drawing.Color.Black);
        private System.Drawing.Font cur_Font = new System.Drawing.Font("Microsoft YaHei UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

        public System.Drawing.Font CurrentFont { get { return cur_Font; } }
        public void SetFont(Font font)
        {
            this.cur_Font = font;
        }

        protected override void InternalSetBlend(Blend blen)
        {

        }
        protected override void InternalSetPen(float size, DashStyle style)
        {
            cur_pen.Width = size;
            cur_pen.DashStyle = (System.Drawing.Drawing2D.DashStyle)style;

        }
        protected override void InternalSetClip(in RectangleF rect)
        {
            if (!rect.IsEmpty)
            {
                System.Drawing.Drawing2D.Matrix save = gfx.Transform;
                {
                    gfx.Transform = gfx_init;
                    gfx.SetClip(new System.Drawing.RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
                }
                gfx.Transform = save;
            }
            else
            {
                gfx.ResetClip();
            }

        }
        protected override void InternalSetColor(in Display.Color color, float alpha)
        {
            //Display.Color.DecodeRGBA(color, out byte r, out var g, out var b, out var a);
            cur_brush.Color = System.Drawing.Color.FromArgb(
                (byte)Math.Min(color.A8 * alpha, 255),
                color.R8,
                color.G8,
                color.B8);
            cur_pen.Color = cur_brush.Color;
        }
        protected override void InternalSetFont(float size, TextFontStyle style)
        {
            cur_Font = Win32Driver.CreateFont(cur_Font.FontFamily, size, style);

        }
        public override Vector2 MeasureString(string text)
        {
            var b = gfx.MeasureString(text, cur_Font);
            return new Vector2(MathF.Ceiling(b.Width), MathF.Ceiling(b.Height));
        }
        public override Vector2 MeasureString(string text, int width)
        {
            var b = gfx.MeasureString(text, cur_Font, width);
            return new Vector2(MathF.Ceiling(b.Width), MathF.Ceiling(b.Height));
        }
        public override Vector2 MeasureString(string text, RectangleF width)
        {
            var b = gfx.MeasureString(text, cur_Font, (int)width.Width);
            return new Vector2(MathF.Ceiling(b.Width), MathF.Ceiling(b.Height));
        }

        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------
        #region DrawImage

        private Win32Image cur_img;
        public override void BeginImage(Image image, RectangleF? srcRect)
        {
            this.cur_img = (Win32Image)image;

        }
        public override void DrawVertex(VertexBuffer vertex)
        {
            throw new NotImplementedException();
        }
        public override void DrawVertex(VertexBuffer vertex, int[] indices, VertexTopology mode)
        {
            throw new NotImplementedException();
        }
        public override void DrawVertexSequence(VertexBuffer vertex, VertexTopology mode)
        {
            throw new NotImplementedException();
        }

        public override void DrawImageEllipse(in RectangleF rect, float startAngle, float endAngle)
        {
            System.Drawing.Drawing2D.GraphicsPath region = new System.Drawing.Drawing2D.GraphicsPath();
            region.AddPie(rect.X - rect.Width / 2, rect.Y - rect.Height / 2, rect.Width * 2, rect.Height * 2, startAngle - 90, endAngle);
            System.Drawing.Drawing2D.GraphicsState gs = gfx.Save();
            gfx.SetClip(region, System.Drawing.Drawing2D.CombineMode.Intersect);
            gfx.DrawImage(cur_img.src, 0, 0);
            gfx.Restore(gs);

        }
        public override void DrawImageZoom(in RectangleF rect)
        {
            Win32Image wimg = (Win32Image)cur_img;
            gfx.DrawImage(wimg.src, rect.X, rect.Y, rect.Width, rect.Height);

        }
        public override void DrawImage(in Vector2 pos)
        {
            Win32Image wimg = (Win32Image)cur_img;
            gfx.DrawImage(wimg.src, pos.X, pos.Y, wimg.Width, wimg.Height);

        }
        public override void DrawImage(in Vector2 pos, AlignmentStyle anchor)
        {
            Win32Image wimg = (Win32Image)cur_img;
            PushTransform();
            try
            {
                transAnchor(anchor, wimg.Width, wimg.Height);
                gfx.DrawImage(wimg.src, pos.X, pos.Y, wimg.Width, wimg.Height);
            }
            finally
            {
                PopTransform();
            }

        }
        public override void DrawImageTrans(in Vector2 pos, Trans trans)
        {
            Win32Image wimg = (Win32Image)cur_img;
            PushTransform();
            {
                gfx.TranslateTransform(pos.X, pos.Y);
                transTans(trans, wimg.Width, wimg.Height);
                gfx.DrawImage(wimg.src, 0, 0, wimg.Width, wimg.Height);
            }
            PopTransform();

        }

        public override void DrawImageRegion(in RectangleF srcRect, in Vector2 dst)
        {
            Win32Image wimg = (Win32Image)cur_img;
            {
                System.Drawing.RectangleF sRect = new System.Drawing.RectangleF(
                    srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height);
                System.Drawing.RectangleF dRect = new System.Drawing.RectangleF(
                    dst.X, dst.Y, srcRect.Width, srcRect.Height);
                gfx.DrawImage(wimg.src, dRect, sRect, GraphicsUnit.Pixel);
            }
        }
        public override void DrawImageRegion(in RectangleF srcRect, in RectangleF dstRect)
        {
            Win32Image wimg = (Win32Image)cur_img;
            {
                System.Drawing.RectangleF sRect = new System.Drawing.RectangleF(
                    srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height);
                System.Drawing.RectangleF dRect = new System.Drawing.RectangleF(
                   dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height);
                gfx.DrawImage(wimg.src, dRect, sRect, System.Drawing.GraphicsUnit.Pixel);
            }
        }

        public override void DrawImageRegion(in RectangleF srcRect, Trans tx, in Vector2 dst)
        {
            Win32Image wimg = (Win32Image)cur_img;
            PushTransform();
            {
                gfx.TranslateTransform(dst.X, dst.Y);
                transTans(tx, srcRect.Width, srcRect.Height);
                System.Drawing.RectangleF sRect = new System.Drawing.RectangleF(
                    srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height);
                gfx.DrawImage(wimg.src, 0, 0, sRect, System.Drawing.GraphicsUnit.Pixel);
            }
            PopTransform();

        }
        public override void DrawImageRegion(in RectangleF srcRect, Trans tx, in RectangleF dstRect)
        {
            Win32Image wimg = (Win32Image)cur_img;
            PushTransform();
            {
                gfx.TranslateTransform(dstRect.X, dstRect.Y);
                transTans(tx, srcRect.Width, srcRect.Height);
                System.Drawing.RectangleF sRect = new System.Drawing.RectangleF(
                    srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height);
                System.Drawing.RectangleF dRect = new System.Drawing.RectangleF(
                   0, 0, dstRect.Width, dstRect.Height);
                gfx.DrawImage(wimg.src, dRect, sRect, System.Drawing.GraphicsUnit.Pixel);
                //Draw red rect to test bounds
                //gfx.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), dx, dy, dw, dh);
            }
            PopTransform();

        }

        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------
        #region DrawString
        public override void DrawString(string text, in Vector2 pos, AlignmentStyle anchor)
        {
            anchor.ToStringAlignment(out var ax, out var ay);
            gfx.DrawString(text, cur_Font, cur_brush, pos.ToGDI(), new StringFormat()
            {
                Alignment = ax,
                LineAlignment = ay,
            });

        }
        public override void DrawString(string text, in RectangleF region, AlignmentStyle anchor)
        {
            anchor.ToStringAlignment(out var ax, out var ay);
            gfx.DrawString(text, cur_Font, cur_brush, region.ToGDI(), new StringFormat()
            {
                Alignment = ax,
                LineAlignment = ay,
            });

        }

        public override void DrawTextLayer(TextLayer text, in RectangleF rect, AlignmentStyle alignment)
        {
            //             Win32TextLayer wtxt = (Win32TextLayer)text;
            //             //wtxt.Refresh(this);
            //             //PushTransform();
            //             {
            //                 //gfx.DrawImage(wtxt.src, rect.X, rect.Y);
            //                 wtxt.Render(this, rect, alignment);
            //             }
            //             //PopTransform();
            var wtxt = (Win32TextLayer)text;
            SetFont(wtxt.cur_font);
            wtxt.Refresh(this);
            this.DrawStringBounds(wtxt.Text, wtxt.FontColor, wtxt.BorderColor, wtxt.BorderTime, alignment, rect);
        }

        public override void DrawStringBorder(string text, TextBorderStyle bt, in Display.Color bodyBrush, in Display.Color bbrush, in RectangleF rect, AlignmentStyle anchor)
        {
            var expectRect = rect;
            var bounds = MeasureString(text, expectRect);
            switch (anchor)
            {
                case DeepCore.GUI.Data.AlignmentStyle.TopLeft:
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.MiddleLeft:
                    expectRect.Y = (expectRect.Y + expectRect.Height / 2) - bounds.Y / 2;
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.BottomLeft:
                    expectRect.Y = (expectRect.Y + expectRect.Height) - bounds.Y;
                    break;

                case DeepCore.GUI.Data.AlignmentStyle.TopCenter:
                    expectRect.X = (expectRect.X + expectRect.Width / 2) - bounds.X / 2;
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.MiddleCenter:
                    expectRect.X = (expectRect.X + expectRect.Width / 2) - bounds.X / 2;
                    expectRect.Y = (expectRect.Y + expectRect.Height / 2) - bounds.Y / 2;
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.BottomCenter:
                    expectRect.X = (expectRect.X + expectRect.Width / 2) - bounds.X / 2;
                    expectRect.Y = (expectRect.Y + expectRect.Height) - bounds.Y;
                    break;

                case DeepCore.GUI.Data.AlignmentStyle.TopRight:
                    expectRect.X = (expectRect.X + expectRect.Width) - bounds.X;
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.MiddleRight:
                    expectRect.X = (expectRect.X + expectRect.Width) - bounds.X;
                    expectRect.Y = (expectRect.Y + expectRect.Height / 2) - bounds.Y / 2;
                    break;
                case DeepCore.GUI.Data.AlignmentStyle.BottomRight:
                    expectRect.X = (expectRect.X + expectRect.Width) - bounds.X;
                    expectRect.Y = (expectRect.Y + expectRect.Height) - bounds.Y;
                    break;
            }
            expectRect.Size = bounds;
            SetColor(bbrush);
            switch (bt)
            {
                case DeepCore.GUI.Data.TextBorderStyle.Border_4:
                    for (int i = 0; i < 4; i++)
                    {
                        gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X + offset_4[i, 0], expectRect.Y + offset_4[i, 1], expectRect.width, expectRect.height));
                    }
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Border:
                    for (int i = 0; i < 8; i++)
                    {
                        gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X + offset_8[i, 0], expectRect.Y + offset_8[i, 1], expectRect.width, expectRect.height));
                    }
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow:
                    gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X + 1, expectRect.Y + 1, expectRect.width, expectRect.height));
                    break;

                case DeepCore.GUI.Data.TextBorderStyle.Shadow_L_T:
                    gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X - 1, expectRect.Y - 1, expectRect.width, expectRect.height));
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_C_T:
                    gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X + 0, expectRect.Y - 1, expectRect.width, expectRect.height));
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_R_T:
                    gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X + 1, expectRect.Y - 1, expectRect.width, expectRect.height));
                    break;

                case DeepCore.GUI.Data.TextBorderStyle.Shadow_L_C:
                    gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X - 1, expectRect.Y + 0, expectRect.width, expectRect.height));
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_C_C:
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_R_C:
                    gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X + 1, expectRect.Y + 0, expectRect.width, expectRect.height));
                    break;

                case DeepCore.GUI.Data.TextBorderStyle.Shadow_L_B:
                    gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X - 1, expectRect.Y + 1, expectRect.width, expectRect.height));
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_C_B:
                    gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X + 0, expectRect.Y + 1, expectRect.width, expectRect.height));
                    break;
                case DeepCore.GUI.Data.TextBorderStyle.Shadow_R_B:
                    gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X + 1, expectRect.Y + 1, expectRect.width, expectRect.height));
                    break;
            }
            SetColor(bodyBrush);
            gfx.DrawString(text, cur_Font, cur_brush, new System.Drawing.RectangleF(expectRect.X, expectRect.Y, expectRect.width, expectRect.height));
        }
        private static float[,] offset_8 =
        {
                    { -1, -1},{ 0,-1},{ 1, -1},
                    { -1,  0},/*0, 0*/{ 1,  0},
                    { -1,  1},{ 0, 1},{ 1,  1}
        };
        private static float[,] offset_4 =
        {
                    /*0, 0*/{ 0,-1},/*2, 0*/
                    {-1, 0},/*1, 1*/{1, 0},
                    /*0, 2*/{ 0, 1},/*2,2*/
        };
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------
        #region Shape
        public override void DrawLine(in Line2 line)
        {
            gfx.DrawLine(cur_pen, line.P.X, line.P.Y, line.Q.X, line.Q.Y);

        }
        public override void FillRect(in RectangleF rect)
        {
            gfx.FillRectangle(cur_brush, rect.X, rect.Y, rect.Width, rect.Height);

        }
        public override void DrawRect(in RectangleF rect)
        {
            gfx.DrawRectangle(cur_pen, rect.X, rect.Y, rect.Width, rect.Height);

        }
        public override void FillArc(in RectangleF rect, float startAngle, float arcAngle)
        {
            gfx.FillPie(cur_brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, arcAngle);

        }
        public override void DrawArc(in RectangleF rect, float startAngle, float arcAngle)
        {
            gfx.DrawArc(cur_pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, arcAngle);

        }
        public override void FillRect4Color(in RectangleF rect, uint[] argb_4)
        {
            gfx.FillRectangle(cur_brush, rect.X, rect.Y, rect.Width, rect.Height);

        }
        public override void FillRoundRect(in RectangleF rect, float rx, float ry)
        {
            gfx.FillRectangle(cur_brush, rect.X, rect.Y, rect.Width, rect.Height);

        }
        public override void DrawRoundRect(in RectangleF rect, float rx, float ry)
        {
            gfx.DrawRectangle(cur_pen, rect.X, rect.Y, rect.Width, rect.Height);

        }
        public override void DrawPolygon(Vector2[] points)
        {
            gfx.DrawPolygon(cur_pen, Array.ConvertAll(points, p => new PointF(p.X, p.Y)));

        }
        public override void FillPolygon(Vector2[] points)
        {
            gfx.FillPolygon(cur_brush, Array.ConvertAll(points, p => new PointF(p.X, p.Y)));

        }
        public override void FillRectEllipse(in RectangleF rect, float startAngle, float endAngle)
        {
            System.Drawing.Drawing2D.GraphicsPath region = new System.Drawing.Drawing2D.GraphicsPath();
            region.AddPie(rect.X - rect.Width / 2, rect.Y - rect.Height / 2, rect.Width * 2, rect.Height * 2, startAngle - 90, endAngle);
            System.Drawing.Drawing2D.GraphicsState gs = gfx.Save();
            gfx.SetClip(region, System.Drawing.Drawing2D.CombineMode.Intersect);
            gfx.FillRectangle(cur_brush, rect.X, rect.Y, rect.Width, rect.Height);
            gfx.Restore(gs);
        }

        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------
    }
}
