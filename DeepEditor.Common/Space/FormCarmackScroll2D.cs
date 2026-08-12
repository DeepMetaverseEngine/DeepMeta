using DeepCore;
using DeepCore.Geometry;
using DeepCore.Space;
using DeepEditor.Common.Properties;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.Space
{
    public partial class FormCarmackScroll2D : DeepEditor.Common.G2D.G2DBaseForm
    {
        static Font nfont;
        Bitmap image;
        IScrollView<Bitmap> scroll2D;

        public FormCarmackScroll2D()
        {
            InitializeComponent();
            FormCarmackScroll2D.nfont = new Font(this.Font.FontFamily, 6);
            this.KeyDown += FormCarmackScroll2D_KeyDown;
        }

        public void SetScrollImage(Image bitmap)
        {
            this.image = bitmap.AsBitmap();
#if false
            this.scroll2D = new CarmackScroll<Bitmap>(
                new TilesCarmackScrollMap(image, 32),
                new DeepCore.Geometry.Vector3(320, 320, 0));
#else       
            this.scroll2D = new LookRangeScroll<Bitmap>(
               new TilesCarmackScrollMap(image, 64),
               new DeepCore.Geometry.Vector3(320, 320, 0),
               2);
#endif
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var viewPos = scroll2D.ViewPosition;
            var viewSize = scroll2D.ViewSize;
            var gridSize = scroll2D.GridSize;
            var mousePos = this.GetMousePoint().ToGeometry();
            // draw src
            {
                e.Graphics.DrawImage(image, 0, 0, image.Width, image.Height);
                e.Graphics.DrawRectangle(Pens.Yellow,
                    viewPos.X - viewSize.X / 2,
                    viewPos.Y - viewSize.Y / 2,
                    viewSize.X,
                    viewSize.Y);
                e.Graphics.DrawGridLines(
                    new Pen(Color.FromArgb(64, 255, 255, 255)), 0, 0,
                    scroll2D.Meta.GridSize.X,
                    scroll2D.Meta.GridSize.Y,
                    scroll2D.Meta.Length.X,
                    scroll2D.Meta.Length.Y);
                e.Graphics.FillRectangle(Brushes.Yellow, viewPos.X - 2, viewPos.Y - 2, 4, 4);
            }
            // raycast
            {
                foreach (var loc in rayCastsLoc)
                {
                    e.Graphics.FillRectangle(Brushes.Gray, loc.X * gridSize.X, loc.Y * gridSize.Y, gridSize.X, gridSize.Y);
                }
                foreach (var cast in rayCasts)
                {
                    e.Graphics.FillRectangle(Brushes.MediumAquamarine, cast.X - 4, cast.Y - 4, 8, 8);
                }
                var start = raycast.center;
                var end = raycast.center + (raycast.normal * raycast.distance);
                e.Graphics.DrawLine(Pens.White, start.X, start.Y, end.X, end.Y);
            }
            // draw buffer in world
            {
                var st = e.Graphics.Save();
                e.Graphics.TranslateTransform(image.Width, 0);
                scroll2D.ForEachWorldBuffer((b, x, y, z) =>
                {
                    e.Graphics.DrawImage(b,
                        x * gridSize.X,
                        y * gridSize.Y,
                        gridSize.X,
                        gridSize.Y);
                    return false;
                });
                e.Graphics.DrawRectangle(Pens.Yellow,
                    viewPos.X - viewSize.X / 2,
                    viewPos.Y - viewSize.Y / 2,
                    viewSize.X,
                    viewSize.Y);

                e.Graphics.Restore(st);
            }
            // draw get clip in map buffer
            {
                var st = e.Graphics.Save();
                e.Graphics.TranslateTransform(32, image.Height + 32);
                if (scroll2D.TryGetMapBuffByPos(mousePos, out var pickBuff))
                {
                    e.Graphics.DrawImage(pickBuff, 0, 0, gridSize.X, gridSize.Y);
                }
                e.Graphics.Restore(st);
            }
            // draw camera
            {
                var st = e.Graphics.Save();
                e.Graphics.TranslateTransform(32 + 128, image.Height + 32);
                e.Graphics.SetClip(0, 0, viewSize.X, viewSize.Y);
                scroll2D.Visit((b, pos) =>
                {
                    e.Graphics.DrawImage(b, pos.X, pos.Y, gridSize.X, gridSize.Y);
                    return false;
                });
                e.Graphics.DrawRectangle(Pens.Yellow, 0, 0, viewSize.X, viewSize.Y);
                e.Graphics.Restore(st);
            }
            // draw foreach buffer
            {
                var st = e.Graphics.Save();
                e.Graphics.TranslateTransform(viewSize.X + 128 + 128, image.Height + 32);
                scroll2D.ForEachBuffer((b, x, y, z) =>
                {
                    e.Graphics.DrawImage(b,
                        x * gridSize.X,
                        y * gridSize.Y,
                        gridSize.X,
                        gridSize.Y);
                });
                e.Graphics.Restore(st);
            }
        }
        private void FormCarmackScroll2D_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    scroll2D.MoveViewPos(new Vector2(0, -1));
                    break;
                case Keys.Down:
                case Keys.S:
                    scroll2D.MoveViewPos(new Vector2(0, 1));
                    break;
                case Keys.Left:
                case Keys.A:
                    scroll2D.MoveViewPos(new Vector2(-1, 0));
                    break;
                case Keys.Right:
                case Keys.D:
                    scroll2D.MoveViewPos(new Vector2(1, 0));
                    break;
            }
        }


        List<Vector2> rayCasts = new List<Vector2>();
        List<Location3D> rayCastsLoc = new List<Location3D>();
        RayCast raycast;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var pos = e.Location.ToGeometry();
            if (e.Button == MouseButtons.Right)
            {
                scroll2D.SetViewPos(pos);
            }
            else if (e.Button == MouseButtons.Left)
            {
                rayCasts.Clear(); rayCastsLoc.Clear();
                raycast.center = new Vector2(e.X, e.Y);
                raycast.normal = Vector3.Zero;
                raycast.distance = 0;
                if (scroll2D.TryGetMapBuffByPos(pos, out var pickBuff))
                {

                }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                raycast.normal = Vector3.Normalize(new Vector3(e.X, e.Y, 0) - raycast.center);
                raycast.distance = Vector3.Distance(new Vector3(e.X, e.Y, 0), raycast.center);
                scroll2D.TryRayCastMap(0,CollectionPool.Shared, raycast, (clip, loc, pos,st) =>
                {
                     rayCastsLoc.Add(loc);
                     rayCasts.Add(pos);
                     return false;
                }, out var touched);
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var pos = e.Location.ToGeometry();
                scroll2D.SetViewPos(pos);
            }
        }

        class TilesCarmackScrollMap : IScrollMap<Bitmap>
        {
            public Vector3 GridSize { get; }
            public Size3D Length { get; }
            public bool IsInfinity => true;
            public bool IsSycMap => false;

            private int clip;
            private Bitmap[,] clips;
            public TilesCarmackScrollMap(Bitmap src, int clip)
            {
                GridSize = new Vector3(clip, clip, 1);
                Length = new Size3D(
                    CMath.RoundMod(src.Width, clip),
                    CMath.RoundMod(src.Height, clip),
                    1);
                this.clip = clip;
                this.clips = new Bitmap[Length.X, Length.Y];
                this.clips.InitArray2D(0,(st, x, y) =>
                {
                    return SubMap(src, x * clip, y * clip, clip, clip);
                });

            }
            public Bitmap GetMetaData(int x, int y, int z)
            {
                if (x >= 0 && y >= 0 && x < Length.X && y < Length.Y)
                {
                    return clips[x, y];
                }
                else
                {
                    var num = new Bitmap(clip, clip);
                    using (var g = Graphics.FromImage(num))
                    {
                        g.FillRectangle(Brushes.Gray, new System.Drawing.Rectangle(0, 0, clip, clip));
                        g.DrawRectangle(Pens.Yellow, new System.Drawing.Rectangle(0, 0, clip - 1, clip - 1));
                        g.DrawString($"{x}\n{y}", nfont, Brushes.White, new System.Drawing.Rectangle(0, 0, clip, clip));
                    }
                    return num;
                }
            }
            public Bitmap SubMap(Bitmap src, int x, int y, int w, int h)
            {
                var clip = new Bitmap(w, h);
                using (var g = Graphics.FromImage(clip))
                {
                    g.DrawImage(src,
                        new System.Drawing.Rectangle(0, 0, w, h),
                        new System.Drawing.Rectangle(x, y, w, h),
                        GraphicsUnit.Pixel);
                }
                return clip;
            }
        }

    }
}
