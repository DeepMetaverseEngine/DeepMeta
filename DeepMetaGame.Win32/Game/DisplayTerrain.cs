using DeepCore;
using DeepCore.GameData.Data;
using DeepCore.GameData.Zone;
using DeepCore.GUI.Win32;
using DeepCore.Reflection;
using DeepEditor.Common.Drawing;
using DeepMetaGame.Data.Template;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DeepEditor.Common.Game
{
    //-----------------------------------------------------------------------------------------------

    abstract public class DisplayTerrain : IDisposable
    {
        //-----------------------------------------------------------------------------------------------------------------
        #region ShowTags

        [Desc("显示Z高度", "show")]
        public bool ShowZ = true;
        [Desc("显示日志", "show")]
        public bool ShowLog = false;
        [Desc("显示地形", "show")]
        public bool ShowTerrain = true;
        public void GenShowItems(ToolStripDropDownButton drop)
        {
            List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();
            foreach (var field in GetType().GetFields())
            {
                DescAttribute desc = PropertyUtil.GetAttribute<DescAttribute>(field);
                if (desc != null && field.FieldType == typeof(bool) && desc.Category == "show")
                {
                    ToolStripMenuItem chk = new ToolStripMenuItem();
                    chk.Size = new System.Drawing.Size(152, 22);
                    chk.CheckOnClick = true;
                    chk.Checked = (bool)field.GetValue(this);
                    chk.Text = desc.Desc;
                    chk.Tag = field;
                    chk.Click += new System.EventHandler(this.chk_Click);
                    list.Add(chk);
                }
            }
            drop.DropDownItems.AddRange(list.ToArray());
        }

        private void chk_Click(object sender, EventArgs e)
        {
            var chk = sender as ToolStripMenuItem;
            var field = chk.Tag as FieldInfo;
            field.SetValue(this, chk.Checked);
        }

        #endregion
        //-----------------------------------------------------------------------------------------------------------------

        private ZoneInfo mZoneInfo;
        private List<DisplayLog> logs = new List<DisplayLog>();

        //-----------------------------------------------------------------------------------------------------------------

        public abstract void Dispose();

        //-----------------------------------------------------------------------------------------------------------------
        public virtual bool UseMapBuffer { get => true; }
        public ZoneInfo Terrain { get { return mZoneInfo; } }
        protected virtual void InitTerrain(ZoneInfo zoneInfo)
        {
            this.mZoneInfo = zoneInfo;
            CreateMapBuffer();
            UpdateMapBuffer(0, 0, zoneInfo.XCount, zoneInfo.YCount);
        }
        public int Width { get { if (mZoneInfo != null) return mZoneInfo.TotalWidth; return 1; } }
        public int Height { get { if (mZoneInfo != null) return mZoneInfo.TotalHeight; return 1; } }
        public int CellW { get { if (mZoneInfo != null) return mZoneInfo.GridCellW; return 1; } }
        public int CellH { get { if (mZoneInfo != null) return mZoneInfo.GridCellH; return 1; } }
        public int XCount { get { if (mZoneInfo != null) return mZoneInfo.XCount; return 1; } }
        public int YCount { get { if (mZoneInfo != null) return mZoneInfo.YCount; return 1; } }


        //-----------------------------------------------------------------------------------------------------------------

        private QueryKey queryKey;

        public void BindMessageFilter(PictureBox control)
        {
            if (queryKey == null)
            {
                this.queryKey = new QueryKey(control);
            }
        }
        public void UnbindMessageFilter()
        {
            if (queryKey != null)
            {
                queryKey.Dispose();
                queryKey = null;
            }
        }
        public bool IsMouseDown(MouseButtons btn)
        {
            if (queryKey != null)
            {
                return queryKey.IsMouseDown(btn);
            }
            return false;
        }
        public bool IsMouseUp(MouseButtons btn)
        {
            if (queryKey != null)
            {
                return queryKey.IsMouseUp(btn);
            }
            return false;
        }
        public bool IsMouseHold(MouseButtons btn)
        {
            if (queryKey != null)
            {
                return queryKey.IsMouseHold(btn);
            }
            return false;
        }

        //-----------------------------------------------------------------------------------------------------------------
        // windows

        private RectangleF mWindow = new RectangleF(0, 0, 512, 512);

        private float mouseX = 0;
        private float mouseY = 0;
        private float cameraX = 0;
        private float cameraY = 0;
        private float cameraScaleX = 1;
        private float cameraScaleY = 1;

        public float MouseX { get { return mouseX; } }
        public float MouseY { get { return mouseY; } }

        public float CameraX { get { return cameraX; } }
        public float CameraY { get { return cameraY; } }
        public float WindowW { get { return mWindow.Width; } }
        public float WindowH { get { return mWindow.Height; } }

        public RectangleF CameraScrollRect
        {
            get
            {
                return new RectangleF(
                            screenToWorldX(0),
                            screenToWorldY(0),
                            screenToWorldSizeX(mWindow.Width),
                            screenToWorldSizeY(mWindow.Height));
            }
        }

        public void setWindow(RectangleF window)
        {
            this.mWindow.X = window.X;
            this.mWindow.Y = window.Y;
            this.mWindow.Width = window.Width;
            this.mWindow.Height = window.Height;
        }

        public float screenToWorldSizeX(float s)
        {
            return s / cameraScaleX;
        }
        public float screenToWorldSizeY(float s)
        {
            return s / cameraScaleY;
        }

        public float worldToScreenSizeX(float s)
        {
            return s * cameraScaleX;
        }
        public float worldToScreenSizeY(float s)
        {
            return s * cameraScaleY;
        }

        public float screenToWorldX(float x)
        {
            return (x - mWindow.Width / 2) / cameraScaleX + cameraX;
        }
        public float screenToWorldY(float y)
        {
            return (y - mWindow.Height / 2) / cameraScaleY + cameraY;
        }
        public float worldToScreenX(float x)
        {
            return (x - cameraX) * cameraScaleX + mWindow.Width / 2;
        }
        public float worldToScreenY(float y)
        {
            return (y - cameraY) * cameraScaleY + mWindow.Height / 2;
        }

        public void setCamera(float x, float y)
        {
            this.cameraX = x;
            this.cameraY = y;
        }

        public void setCameraScale(float scaleX, float scaleY)
        {
            cameraScaleX = scaleX;
            cameraScaleX = Math.Max(cameraScaleX, 0.01f);
            cameraScaleY = scaleY;
            cameraScaleY = Math.Max(cameraScaleY, 0.01f);
        }

        public float getCameraScaleX()
        {
            return cameraScaleX;
        }
        public float getCameraScaleY()
        {
            return cameraScaleY;
        }

        public float getCameraScale()
        {
            return Math.Max(cameraScaleX, cameraScaleY);
        }


        //-----------------------------------------------------------------------------------------------------------------

        virtual public void update(int intervalMS)
        {
        }
        virtual public void update_flush()
        {
            if (queryKey != null)
            {
                queryKey.Update();
                this.mouseX = queryKey.MouseX;
                this.mouseY = queryKey.MouseY;
            }
            for (int i = logs.Count - 1; i >= 0; --i)
            {
                DisplayLog u = logs[i];
                u.update();
                if (!u.Alive)
                {
                    logs.RemoveAt(i);
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------------

        #region _render_

        public static Font font = new Font(@"Microsoft YaHei UI", 9, FontStyle.Regular);

        protected Bitmap mapBuffer;
        protected System.Drawing.Imaging.ImageAttributes mapBufferAttr;
        protected void CreateMapBuffer()
        {
            if (UseMapBuffer)
            {
                this.mapBuffer = new Bitmap(Terrain.XCount, Terrain.YCount);
                this.mapBufferAttr = new System.Drawing.Imaging.ImageAttributes();
            }
        }
        public void UpdateMapBuffer(int sx, int sy, int sw, int sh)
        {
            if (UseMapBuffer)
            {
                for (int x = sx + sw; x >= sx; --x)
                {
                    if (x < mZoneInfo.XCount && x >= 0)
                    {
                        for (int y = sy + sh; y >= sy; --y)
                        {
                            if (y < mZoneInfo.YCount && y >= 0)
                            {
                                int flag = mZoneInfo.GetFlag(x, y);
                                mapBuffer.SetPixel(x, y, Color.FromArgb(flag));
                            }
                        }
                    }
                }
            }
        }

        public void UpdateObjBuffer() { }

        private float begin_pen_scale = 1f;
        public float PenScale { get { return begin_pen_scale; } }

        protected readonly SolidBrush brush_cell = new SolidBrush(Color.FromArgb(0xff, 0x20, 0x40, 0x20));
        protected readonly Pen pen_z = new Pen(Color.White);
        protected readonly Pen pen_grid = new Pen(Color.FromArgb(0x80, 0xff, 0xff, 0xff));

        virtual public void render(Graphics g)
        {

            RectangleF worldBounds = new RectangleF(
                    screenToWorldX(0),
                    screenToWorldY(0),
                    screenToWorldSizeX(mWindow.Width),
                    screenToWorldSizeY(mWindow.Height));
            var gs = g.Save();
            try
            {
                g.Reset();
                //                 g.CompositingMode = (System.Drawing.Drawing2D.CompositingMode.SourceOver);
                //                 g.PageUnit = GraphicsUnit.Pixel;
                //                 g.CompositingQuality = (System.Drawing.Drawing2D.CompositingQuality.HighSpeed);
                //                 g.SmoothingMode = (System.Drawing.Drawing2D.SmoothingMode.None);
                //                 g.InterpolationMode = (System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
                //                 g.PixelOffsetMode = (System.Drawing.Drawing2D.PixelOffsetMode.Half);

                this.begin_pen_scale = 1f / getCameraScale();

                g.TranslateTransform(
                   mWindow.Width / 2,
                   mWindow.Height / 2);
                g.ScaleTransform(
                    cameraScaleX,
                    cameraScaleY);
                g.TranslateTransform(
                    -cameraX,
                    -cameraY);

                renderTerrain(g, worldBounds);

                renderObjects(g, worldBounds);
            }
            finally
            {
                g.Restore(gs);
            }
            renderScreen(g, worldBounds);
            if (ShowLog) renderLogs(g);
        }

        private void renderLogs(Graphics g)
        {

            for (int i = logs.Count - 1; i >= 0; --i)
            {
                DisplayLog u = logs[i];
                var state = g.Save();
                g.TranslateTransform(
                    worldToScreenX(u.pos.X),
                    worldToScreenY(u.pos.Y));
                u.render(g);
                g.Restore(state);
            }

        }

        protected abstract void renderObjects(Graphics g, RectangleF worldBounds);
        protected abstract void renderScreen(Graphics g, RectangleF worldBounds);
        protected virtual void renderTerrain(Graphics g, RectangleF worldBounds)
        {
            if (ShowTerrain && mZoneInfo != null)
            {
                if (UseMapBuffer)
                {
                    renderMapBuffer(g, mapBuffer, worldBounds);
                }
                else
                {
                    renderMapDirect(g, worldBounds);
                }
            }
        }
        protected virtual void renderMapBuffer(Graphics g2d, Bitmap map,RectangleF rect)
        {
            int sx = Math.Max(0, (int)(rect.X / CellW));
            int sy = Math.Max(0, (int)(rect.Y / CellH));
            int sw = (int)(rect.Width / CellW) + 2;
            int sh = (int)(rect.Height / CellH) + 2;
            int dx = Math.Min(mZoneInfo.XCount - 1, sx + sw);
            int dy = Math.Min(mZoneInfo.YCount - 1, sy + sh);
            sw = dx - sx + 1;
            sh = dy - sy + 1;
            g2d.DrawImage(map,
                new System.Drawing.Rectangle(sx * CellW, sy * CellH, sw * CellW, sh * CellH),
                sx, sy, sw, sh,
                GraphicsUnit.Pixel, mapBufferAttr);
        }
        protected virtual void renderMapDirect(Graphics g, RectangleF rect)
        {
            int sx = Math.Max(0, (int)(rect.X / CellW));
            int sy = Math.Max(0, (int)(rect.Y / CellH));
            int sw = (int)(rect.Width / CellW) + 2;
            int sh = (int)(rect.Height / CellH) + 2;
            int dx = Math.Min(mZoneInfo.XCount - 1, sx + sw);
            int dy = Math.Min(mZoneInfo.YCount - 1, sy + sh);
            for (int x = dx; x >= sx; --x)
            {
                for (int y = dy; y >= sy; --y)
                {
                    int flag = mZoneInfo.GetFlag(x, y);
                    if (flag != 0)
                    {
                        brush_cell.Color = Color.FromArgb(flag);
                        g.FillRectangle(brush_cell, x * CellW, y * CellH, CellW, CellH);
                    }
                }
            }
        }
        protected virtual void renderMapEnd(Graphics g, int sx, int sy, int dx, int dy)
        {

        }

        //----------------------------------------------------------------------------------------------------
        public delegate void DrawAction(Graphics g, RectangleF cameraBounds);

        public void drawInWorldSpace(Graphics g, DrawAction action)
        {
            RectangleF worldBounds = new RectangleF(
                    screenToWorldX(0),
                    screenToWorldY(0),
                    screenToWorldSizeX(mWindow.Width),
                    screenToWorldSizeY(mWindow.Height));
            var gs = g.Save();
            {
                this.begin_pen_scale = 1f / getCameraScale();

                g.TranslateTransform(
                   mWindow.Width / 2,
                   mWindow.Height / 2);
                g.ScaleTransform(
                    cameraScaleX,
                    cameraScaleY);
                g.TranslateTransform(
                    -cameraX,
                    -cameraY);
                action(g, worldBounds);
            }
            g.Restore(gs);
        }

        public void drawGrid(Graphics g, RectangleF rect, int cellW, int cellH)
        {
            pen_grid.Width = 1f / getCameraScale();
            int sx = (int)(rect.X / cellW);
            int sy = (int)(rect.Y / cellH);
            int sw = (int)(rect.Width / cellW) + 1;
            int sh = (int)(rect.Height / cellH) + 1;
            int lw = (int)(rect.Width + cellW * 2);
            int lh = (int)(rect.Height + cellH * 2);
            for (int x = sx + sw; x >= sx; --x)
            {
                g.DrawLine(pen_grid, x * cellW, sy * cellH, x * cellW, (sy * cellH + lh));
            }
            for (int y = sy + sh; y >= sy; --y)
            {
                g.DrawLine(pen_grid, sx * cellW, y * cellH, (sx * cellW + lw), y * cellH);
            }
        }

        protected void drawObjectes<T>(Graphics g, RectangleF rect, ICollection<T> units)
            where T : DisplayObject
        {
            float penscale = 1f / getCameraScale();

            foreach (DisplayObject u in units)
            {
                if (u.Visible && CMath.IntersectRectW(
                    rect.X,
                    rect.Y,
                    rect.Width,
                    rect.Height,
                    u.X + u.LocalBounds.X,
                    u.Y + u.LocalBounds.Y,
                    u.LocalBounds.Width,
                    u.LocalBounds.Height
                    ))
                {
                    var state = g.Save();
                    try
                    {
                        g.TranslateTransform(u.X, u.Y);
                        if (ShowZ && u.Z != 0)
                        {
                            pen_z.Width = penscale;
                            g.DrawLine(pen_z, 0, 0, 0, -u.Z);
                            g.ScaleTransform(1 + u.Z, 1 + u.Z);
                        }
                        u.render_begin(g);
                        u.render(g);
                        u.render_end(g);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message + "\n" + err.StackTrace);
                    }
                    finally
                    {
                        g.Restore(state);
                    }
                }
            }
        }

        #endregion

        //-----------------------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------------------

        public void showLog(string text, float x, float y)
        {
            if (ShowLog)
                logs.Add(new DisplayLog(text, x, y, Color.White));
        }
        public void showLog(string text, float x, float y, Color color)
        {
            if (ShowLog)
                logs.Add(new DisplayLog(text, x, y, color));
        }

        public class DisplayLog
        {
            public readonly PointF pos = new PointF();
            private Brush brush;
            private int time = 20;
            private readonly string text;

            internal DisplayLog(string text, float x, float y, Color color)
            {
                this.text = text;
                this.time = 20;
                this.pos.X = x;
                this.pos.Y = y;
                this.brush = new SolidBrush(color);
            }

            internal void update()
            {
                time--;
            }

            internal void render(Graphics g)
            {
                SizeF size = g.MeasureString(text, font);
                g.DrawString(
                    text,
                    font,
                    brush,
                    -size.Width / 2,
                    -size.Height / 2 - 20 + time,
                    Win32Driver.DefaultFormat);
            }

            internal bool Alive
            {
                get { return time > 0; }
            }


        }
    }

    //-----------------------------------------------------------------------------------------------

    abstract public class DisplayObject
    {
        public abstract bool Visible { get; }

        public abstract float X { get; }
        public abstract float Y { get; }
        public abstract float Z { get; }

        public abstract RectangleF LocalBounds { get; }
        public abstract void render(Graphics g);

        public virtual void render_begin(Graphics g) { }
        public virtual void render_end(Graphics g) { }
        public virtual void render_in_terrain(Graphics g) { }

        public virtual void renderHP(Graphics g) { }
        public virtual void renderName(Graphics g, Font font, Brush brush) { }
    }

    //-----------------------------------------------------------------------------------------------

}
