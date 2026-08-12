using DeepCore;
using DeepCore.GameData.Zone;
using DeepCore.Reflection;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel.Display3D;
using DeepEditor.Common.Voxel;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.WinForms;
using DeepMetaGame.Data.Template;
using DeepCore.Space;

namespace DeepEditor.Plugin3D.Display3D
{
    public abstract class DisplayZoneWorld3D : DisplayVoxelWorld3D
    {
        [Desc("显示地形2D", "show", true)] public bool ShowTerrain2D { get; set; } = false;
        [Desc("显示地形2D(网格)", "show", true)] public bool ShowTerrain2D_Line { get; set; } = false;
        [Desc("显示2D参考网线", "edit", true)] public bool ShowTerrainGrids2D { get; set; } = false;
        [Desc("显示单位名字", "show", true)] public bool ShowObjectsName { get; set; } = true;
        [Desc("显示单位海拔", "show", true)] public bool ShowObjectsAltitude { get; set; } = false;
        [Desc("显示空间分割", "show", true)] public bool ShowSpaceDiv { get; set; } = false;
        //-----------------------------------------------------------------------------------------------------------------
        public string ShowNameFilterText = string.Empty;
        //-----------------------------------------------------------------------------------------------------------------

        public DisplayZoneWorld3D(GLControl control, System.Windows.Forms.Timer timer) : base(control, timer)
        {
            this.OnRenderHUD += DrawHUD;
        }
        public virtual void OnInit()
        {
        }
//         protected override void Disposing()
//         {
//             base.Disposing();
//         }

        //-----------------------------------------------------------------------------------------------------------------
        public IWorldCamera WorldCamera { get => this.Camera as IWorldCamera; }
        public abstract ZoneInfo TerrainZone { get; }
        public float SpaceDivW { get; protected set; }
        public SpaceDivision Space { get; protected set; }
        protected override void DrawTerrain3D(PaintEventArgs3D e)
        {
            if (TerrainZone != null)
            {
                {
                    float tw = TerrainZone.TotalWidth;
                    float th = TerrainZone.TotalHeight;
                    DrawingVoxelObject.DrawBounds(Color4.Yellow, 0, 0, tw, th, TerrainZone.GridCellW);
                }
                if (ShowTerrain2D)
                {
                    DrawTerrain2DQuards();
                }
                if (ShowTerrain2D_Line)
                {
                    DrawTerrain2DLines();
                }
                if (ShowTerrainGrids2D)
                {
                    DrawTerrain2DGrids();
                }
            }
            if (ShowSpaceDiv)
            {
                DrawSpaceDiv2DGrids();
            }
            base.DrawTerrain3D(e);
        }


        protected virtual void DrawTerrain2DQuards()
        {
            if (TerrainZone != null)
            {
                var mZoneInfo = this.TerrainZone;
                if (mZoneInfo != null)
                {
                    float cw = mZoneInfo.GridCellW;
                    float ch = mZoneInfo.GridCellH;
                    float tw = mZoneInfo.TotalWidth;
                    float th = mZoneInfo.TotalHeight;
                    GL.Begin(PrimitiveType.Quads);
                    for (int x = mZoneInfo.XCount - 1; x >= 0; --x)
                    {
                        for (int y = mZoneInfo.YCount - 1; y >= 0; --y)
                        {
                            int flag = mZoneInfo[x, y];
                            var c = GLUtils.Argb2Color4(flag);
                            if (c.A != 0)
                            {
                                float dz = 0;// mZoneInfo.mHeightMap[x, y];
                                float dx = x * cw;
                                float dy = y * ch;
                                GL.Color4(c);
                                GL.Vertex3(dx,/* */ dz, dy/* */);
                                GL.Vertex3(dx + cw, dz, dy/* */);
                                GL.Vertex3(dx + cw, dz, dy + ch);
                                GL.Vertex3(dx,/* */ dz, dy + ch);
                            }
                        }
                    }
                    GL.End();
                }
            }
        }
        protected virtual void DrawTerrain2DLines()
        {
            if (TerrainZone != null)
            {
                var mZoneInfo = this.TerrainZone;
                if (mZoneInfo != null)
                {
                    float cw = mZoneInfo.GridCellW;
                    float ch = mZoneInfo.GridCellH;
                    float tw = mZoneInfo.TotalWidth;
                    float th = mZoneInfo.TotalHeight;
                    GL.Begin(PrimitiveType.Lines);
                    for (int x = mZoneInfo.XCount - 1; x >= 0; --x)
                    {
                        for (int y = mZoneInfo.YCount - 1; y >= 0; --y)
                        {
                            int flag = mZoneInfo[x, y];
                            var c = GLUtils.Argb2Color4(flag);
                            if (c.A != 0)
                            {
                                float dz = 0;//mZoneInfo.mHeightMap[x, y];
                                float dx = x * cw;
                                float dy = y * ch;
                                GL.Color4(c);
                                GL.Vertex3(dx,/* */ dz, dy/* */); GL.Vertex3(dx + cw, dz, dy/* */);
                                GL.Vertex3(dx + cw, dz, dy/* */); GL.Vertex3(dx + cw, dz, dy + ch);
                                GL.Vertex3(dx + cw, dz, dy + ch); GL.Vertex3(dx,/* */ dz, dy + ch);
                                GL.Vertex3(dx,/* */ dz, dy + ch); GL.Vertex3(dx,/* */ dz, dy/* */);
                            }
                        }
                    }
                    GL.End();
                }
            }
        }
        protected virtual void DrawTerrain2DGrids()
        {
            if (TerrainZone != null)
            {
                var mZoneInfo = this.TerrainZone;
                float cw = mZoneInfo.GridCellW;
                float ch = mZoneInfo.GridCellH;
                float tw = mZoneInfo.TotalWidth;
                float th = mZoneInfo.TotalHeight;
                float ox = 0;
                float oy = 0;
                GL.Begin(PrimitiveType.Lines);
                GL.Color4(Color4.White.SetAlpha(0.2f));
                for (int x = mZoneInfo.XCount - 1; x >= 0; --x)
                {
                    float dx = x * cw + ox;
                    GL.Vertex3(dx, 0, oy);
                    GL.Vertex3(dx, 0, oy + th);
                }
                for (int y = mZoneInfo.YCount - 1; y >= 0; --y)
                {
                    float dy = y * ch + oy;
                    GL.Vertex3(ox, 0, dy);
                    GL.Vertex3(ox + tw, 0, dy);
                }
                GL.End();
            }
        }
        protected virtual void DrawSpaceDiv2DGrids()
        {
            if (VoxelTerrain != null && SpaceDivW > 0)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Color4(Color4.Magenta);
                var cs = SpaceDivW;
                var sw = CMath.RoundMod(this.VoxelTerrain.TotalSizeX, SpaceDivW);
                var sh = CMath.RoundMod(this.VoxelTerrain.TotalSizeY, SpaceDivW);
                var tw = sw * cs;
                var th = sh * cs;
                for (var x = 0; x <= sw; x++)
                {
                    float dx = x * cs;
                    GL.Vertex3(dx, 0.5f, 0);
                    GL.Vertex3(dx, 0.5f, th);
                }
                for (var y = 0; y <= sh; y++)
                {
                    float dy = y * cs;
                    GL.Vertex3(0, 0.5f, dy);
                    GL.Vertex3(tw, 0.5f, dy);
                }
                GL.End();
            }
        }
        public virtual Vector3 WorldToZone(Vector3 ret)
        {
            return new Vector3(ret.X, ret.Z, ret.Y);
        }
        public virtual Vector3 ZoneToWorld(Vector3 vec)
        {
            return new Vector3(vec.X, vec.Z, vec.Y);
        }
        public virtual Vector3 RayToWorldPos(Glu.Ray ray)
        {
            return Glu.RayPlaneIntersection(ray, new Glu.Plane(Vector3.Zero, Vector3.UnitY));
        }


        protected virtual void DrawHUD(GLView sender, PaintEventArgs3D e)
        {
            if (ShowObjects)
            {
                base.ForEachObjects<DisplayZoneObject>((u) =>
                {
                    if (u.IsVisible) { u.OnRenderObjectHUD(e); }
                    return false;
                });
            }
            //             if (SelectedObject != null)
            //             {
            //                 SelectedObject.OnRenderHUD(e);
            //             }
        }

        public T RayCastObject<T>(Glu.Ray ray, out Vector3 wd_pos) where T : DisplayZoneObject
        {
            var ret_pos = Vector3.Zero;
            var ret = base.ForEachObjects<T>((u) =>
            {
                if (u.TryRayCast(ray, out ret_pos))
                {
                    return true;
                }
                return false;
            });
            wd_pos = ret_pos;
            return ret;
        }


    }
    public abstract class DisplayZoneObject : GLViewObject3D
    {
        private readonly DisplayZoneWorld3D world;
        protected readonly GLTexture2D txt_head;
        protected readonly GLTexture2D txt_display;
        protected RectangleF? txt_head_bounds;

        public DisplayZoneObject(DisplayZoneWorld3D wd)
        {
            this.world = wd;
            this.txt_head = new GLTextTexture2D();
            this.txt_display = new GLTextTexture2D();
        }
        protected override void Disposing()
        {
            base.Disposing();
            txt_head.Dispose();
            txt_display.Dispose();
        }
        public abstract float Direction { get; }
        public abstract float Height { get; }
        public abstract Vector3 Position { get; }
        public abstract float Size { get; }

        new public DisplayZoneWorld3D Parent { get => world; }
        public Vector3 WorldPos { get { var pos = this.Position; return new Vector3(pos.X, pos.Z, pos.Y); } }
        public Vector2 ScreenPos
        {
            get
            {
                var wp = this.Position.ObjectToWorld();
                var screen = world.Camera.WorldToScreen(wp);
                return new Vector2(screen.X, screen.Y);
            }
        }
        public virtual float ScreenSize { get => this.WorldToScreenSize(this.Size); }

        public void SetText(string name, float fontSize, Color4 fontColor)
        {
            this.txt_head.InitWithText($"{name}", fontSize, fontColor, 8, Color4.Black);
        }
        public void SetDisplayText(string name, float fontSize, Color4 fontColor)
        {
            this.txt_display.InitWithText($"{name}", fontSize, fontColor, 8, Color4.Black);
        }
        public virtual bool IsInCamera(CameraControl cam)
        {
            return cam.IsObjectInCamera((Position).ObjectToWorld(), Size) ||
                   cam.IsObjectInCamera((Position + new Vector3(0, 0, Height)).ObjectToWorld(), Size);
        }
        protected override void OnRender(PaintEventArgs3D e)
        {
            if (IsInCamera(world.Camera))
            {
                DrawBody(e);
            }
        }
        protected override void OnRenderHUD(PaintEventArgs3D e)
        {
            base.OnRenderHUD(e);
        }
        protected internal virtual void OnRenderObjectHUD(PaintEventArgs3D e)
        {
            var offset = Parent.WorldCamera.GetDrawStartOffsetHUD(this);
            DrawHUD(e, ref offset);
        }
        protected virtual void DrawHUD(PaintEventArgs3D e, ref Vector2 offset)
        {
            this.txt_head_bounds = null;
            if (!string.IsNullOrEmpty(Parent.ShowNameFilterText))
            {
                var text = this.txt_head.Text;
                if (string.IsNullOrEmpty(text) || !text.Contains(Parent.ShowNameFilterText))
                {
                    return;
                }
            }
            if (world.ShowObjectsName)
            {
                if (this.txt_head.DrawQuards2D(e, offset.X - txt_head.Width / 2f, offset.Y - (txt_head.Height)))
                {
                    offset.Y -= (txt_head.Height);
                    this.txt_head_bounds = new RectangleF(offset.X - txt_head.Width / 2f, offset.Y, txt_head.Width, txt_head.Height);
                }
                if (!string.IsNullOrEmpty(this.txt_display.Text) && !string.Equals(this.txt_display.Text, this.txt_head.Text))
                {
                    if (this.txt_display.DrawQuards2D(e, offset.X - txt_display.Width / 2f, offset.Y - (txt_display.Height)))
                    {
                        offset.Y -= (txt_display.Height);
                    }
                }
            }
            DrawGUI(e, ref offset);
        }
        protected virtual void DrawGUI(PaintEventArgs3D e, ref Vector2 offset) { }
        protected virtual void DrawBody(PaintEventArgs3D e) { }

        protected virtual bool TryPickPlane2D(Vector2 obj_pos)
        {
            var size = this.Size;
            var pos = this.Position;
            return CMath.IncludeRoundPoint(pos.X, pos.Y, size, obj_pos.X, obj_pos.Y);
        }
        public virtual bool TryRayCast(Glu.Ray ray, out Vector3 wd_pos)
        {
            wd_pos = RayCastPlaneOffset(ray);
            if (TryPickPlane2D(new Vector2(wd_pos.X, wd_pos.Z)))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 射线和脚底板焦点
        /// </summary>
        public virtual Vector3 RayCastPlaneOffset(Glu.Ray ray, float height = 0)
        {
            var owp = Position.ObjectToWorld();
            owp.Y += height;
            return Glu.RayPlaneIntersection(ray, new Glu.Plane(owp, Vector3.UnitY));
        }
        public virtual float WorldToScreenSize(float size)
        {
            var s1 = world.Camera.WorldToScreen(this.Position.ObjectToWorld());
            var s2 = world.Camera.WorldToScreen(this.Position.ObjectToWorld() + new Vector3(size, 0, 0));
            return Vector2.Distance(s1.Xy, s2.Xy);
        }
        public virtual float ScreenToWorldSize(float size)
        {
            var s1 = world.Camera.WorldToScreen(this.Position.ObjectToWorld());
            var plane = new Glu.Plane(Position.ObjectToWorld(), Vector3.UnitY);
            var wp1 = RayCastPlaneOffset(world.Camera.ScreenToWorldRay(s1.Xy));
            var wp2 = RayCastPlaneOffset(world.Camera.ScreenToWorldRay(s1.Xy + new Vector2(size, 0)));
            return Vector3.Distance(wp1, wp2);
        }
    }



}
