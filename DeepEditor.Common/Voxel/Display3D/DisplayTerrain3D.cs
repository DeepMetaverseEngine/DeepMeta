#define TERRAIN_6D

using DeepCore;
using DeepCore.Geometry.Terrain;
using DeepCore.Reflection;
using DeepCore.Voxel.Data;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G3D;
using G3D.ObjRenderer;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.Display3D
{

    public class DisplayTerrain3D : GLView
    {
        //-----------------------------------------------------------------------------------------------------------------
        public static float DefaultVoxelAlphaScale = 1f;
        public static float DefaultVoxelGrayScale = 1f;

        private bool mShowTerrain3D = true;
        private bool mShowTerrain3DLines = false;
        private bool mShowFlagMesh3D = false;
        private bool mShowPathMesh3D = false;

        private float mBlendAlpha = DefaultVoxelAlphaScale;
        private float mBlendGray = DefaultVoxelGrayScale;


        [Desc("显示单位", "show", true)]
        public bool ShowObjects { get; set; } = true;
        [Desc("显示MeshObject", "show", true)]
        public bool ShowMeshObject { get; set; } = true;


        [Desc("显示地形3D", "show", true)]
        public bool ShowTerrain3D { get => mShowTerrain3D; set { if (value != mShowTerrain3D) { mShowTerrain3D = value; RebuildVoxelTerrain(); } } }
        [Desc("显示地形3D(网格)", "show", true)]
        public bool ShowTerrain3DLines { get => mShowTerrain3DLines; set { if (value != mShowTerrain3DLines) { mShowTerrain3DLines = value; RebuildVoxelTerrain(); } } }
        [Desc("显示行走面Flag", "show", true)]
        public bool ShowFlagMesh3D { get => mShowFlagMesh3D; set { if (value != mShowFlagMesh3D) { mShowFlagMesh3D = value; RebuildVoxelTerrain(); } } }
        [Desc("显示行走连通图", "show", true)]
        public bool ShowPathMesh3D { get => mShowPathMesh3D; set { if (value != mShowPathMesh3D) { mShowPathMesh3D = value; RebuildVoxelTerrain(); } } }


        public float BlendAlpha { get => mBlendAlpha; set { if (value != mBlendAlpha) { mBlendAlpha = value; RebuildVoxelTerrain(); } } }
        public float BlendGray { get => mBlendGray; set { if (value != mBlendGray) { mBlendGray = value; RebuildVoxelTerrain(); } } }

        //-----------------------------------------------------------------------------------------------------------------
        public DisplayTerrain3D(GLControl control, Timer timer) : base(control, timer)
        {
            this.OnRender += DisplayTerrain3D_OnRender;
        }
        protected override void Disposing()
        {
            UnbindMeshDropDownMenu();
            base.Disposing();
        }
        protected override void GlControl_Load(object sender, EventArgs e)
        {
            base.GlControl_Load(sender, e);
        }
        public void ResetCameraPos()
        {
            this.ResetCameraPos(this.Camera);
        }
        public void ResetCameraPos(SizeF size)
        {
            this.ResetCameraPos(this.Camera, size);
        }
        protected override void OnCreateCameraControl(CameraControl c)
        {
            this.ResetCameraPos(c);
            base.OnCreateCameraControl(c);
        }
        public virtual void ResetCameraPos(CameraControl camera)
        {
            if (TerrainSize.Width > 0)
            {
                var zoneSize = Math.Max(TerrainSize.Width, TerrainSize.Height);
                camera.ShiftAddSpeedRate = Math.Max(10, zoneSize / 100);
                camera.ResetCameraFar(Math.Max(this.Camera.CameraFar, TerrainSize.Width + TerrainSize.Height));
                if (VoxelTerrain != null)
                {
                    var box = VoxelTerrain.AABB;
                    var boxZ = box;
                    camera.SetTerrain(boxZ);
                }
                else
                {
                    camera.SetTerrain(TerrainSize.Width, TerrainSize.Height);
                }
            }
        }
        public virtual void ResetCameraPos(CameraControl camera, SizeF size)
        {
            var zoneSize = Math.Max(size.Width, size.Height);
            camera.ShiftAddSpeedRate = Math.Max(10, zoneSize / 100);
            camera.ResetCameraFar(Math.Max(this.Camera.CameraFar, size.Width + size.Height));
            if (VoxelTerrain != null)
            {
                var box = VoxelTerrain.AABB;
                var boxZ = box;
                camera.SetTerrain(boxZ);
            }
            else
            {
                camera.SetTerrain(size.Width, size.Height);
            }
        }
        protected virtual void DisplayTerrain3D_OnRender(GLView sender, PaintEventArgs3D e)
        {
            //             GL.Enable(EnableCap.Blend);
            //             GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            RenderVoxelTerrain(e);
            RenderMeshObject();
        }

        //-----------------------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------------------
        #region Mesh

        private static ObjLoaderConfig DefaultObjLoaderConfig = new ObjLoaderConfig();
        private SortedDictionary<string, Mesh> loadMeshMap = new SortedDictionary<string, Mesh>();
        private ToolStripDropDownItem bindMenu;
        public void BindMeshDropDownMenu(ToolStripDropDownItem menu)
        {
            UnbindMeshDropDownMenu();
            this.bindMenu = menu;
            this.bindMenu.DropDownOpening += BindMenu_DropDownOpening;
        }
        private void UnbindMeshDropDownMenu()
        {
            loadMeshMap?.Clear();
            if (this.bindMenu != null)
            {
                foreach (ToolStripMenuItem drop in bindMenu.DropDownItems)
                {
                    drop.Dispose();
                }
                bindMenu.DropDownItems.Clear();
                this.bindMenu.DropDownOpening -= BindMenu_DropDownOpening;
            }
        }

        private void BindMenu_DropDownOpening(object sender, EventArgs e)
        {
            var bindMenu = sender as ToolStripDropDownItem;
            foreach (ToolStripMenuItem drop in bindMenu.DropDownItems)
            {
                drop.Dispose();
            }
            bindMenu.DropDownItems.Clear();
            foreach (var me in loadMeshMap)
            {
                var mmenu = new ToolStripMenuItem(Path.GetFileName(me.Key));
                mmenu.Tag = me;
                var mremove = mmenu.DropDownItems.Add("Remove");
                var mconfig = mmenu.DropDownItems.Add("Config");
                mremove.Click += (rs, re) =>
                {
                    this.RemoveMesh(me.Key);
                };
                mconfig.Click += (cs, ce) =>
                {
                    if (this.VoxelTerrain != null)
                    {
                        var cfg = DefaultObjLoaderConfig;
                        var mesh = me.Value;
                        cfg.TerrainWidth = (this.VoxelTerrain?.TotalSizeX) ?? 0;
                        cfg.TerrainHeight = (this.VoxelTerrain?.TotalSizeY) ?? 0;
                        var pdialog = new G2DDataDialog.G2DObjectDialog<ObjLoaderConfig>(cfg);
                        if (pdialog.ShowDialog() == DialogResult.OK)
                        {
                            cfg = DefaultObjLoaderConfig = pdialog.SelectedObject;
                            mesh.PrimitiveType = cfg.PrimitiveType;
                            mesh.TintColor = cfg.TintColor;
                            mesh.Transform = cfg.Transform;
                        }
                    }
                };
                bindMenu.DropDownItems.Add(mmenu);
            }
        }


        public Mesh LoadMeshDialog(ObjLoaderConfig cfg = null)
        {
            return LoadMeshDialog(null, null, cfg);
        }
        public Mesh LoadMeshDialog(string initPath, ObjLoaderConfig cfg = null)
        {
            return LoadMeshDialog(null, initPath, cfg);
        }
        public Mesh LoadMeshDialog(IWin32Window window, string initPath, ObjLoaderConfig cfg = null)
        {
            if (cfg == null)
            {
                cfg = DefaultObjLoaderConfig;
            }
            if (initPath == null)
            {
                initPath = Environment.CurrentDirectory;
            }
            cfg.TerrainWidth = (this.VoxelTerrain?.TotalSizeX) ?? 0;
            cfg.TerrainHeight = (this.VoxelTerrain?.TotalSizeY) ?? 0;
            if (ObjLoader.LoadMeshDialog(window, initPath, ref cfg, out var path, out var mesh))
            {
                DefaultObjLoaderConfig = cfg;
                AddMesh(path, mesh);
                return mesh;
            }
            return null;
        }
        public void AddMesh(string path, Mesh mesh)
        {
            if (this.loadMeshMap.TryGetValue(path, out var oldMesh))
            {
                this.loadMeshMap.Remove(path);
            }
            this.loadMeshMap.Add(path, mesh);
        }
        public string[] ListMeshs()
        {
            return loadMeshMap.Keys.ToArray();
        }
        public Mesh[] GetMeshs()
        {
            return loadMeshMap.Values.ToArray();
        }
        public Mesh GetMesh(string path)
        {
            loadMeshMap.TryGetValue(path, out var ret);
            return ret;
        }
        public Mesh RemoveMesh(string path)
        {
            if (this.loadMeshMap.TryGetValue(path, out var oldMesh))
            {
                this.loadMeshMap.Remove(path);
            }
            return oldMesh;
        }
        public void ClearMesh()
        {
            this.loadMeshMap.Clear();
        }
        private void RenderMeshObject()
        {
            if (ShowMeshObject)
            {
                if (loadMeshMap.Count > 0)
                {
                    foreach (var mesh in loadMeshMap.Values)
                    {
                        mesh.Draw();
                    }
                }
            }
        }

        #endregion
        //-----------------------------------------------------------------------------------------------------------------
        #region Terrain3D

        //-----------------------------------------------------------------------------------------------------------------
        public VoxelTerrain3D VoxelTerrain { get; private set; }
        public SizeF TerrainSize { get; private set; }
        public float GridSize { get; private set; }
        protected const float PATH_Y_OFFSET = 0.01f;

        private VertexArrayObject ShowPathMesh3D_VBO = new VertexArrayObject(PrimitiveType.Lines, Color4.LightBlue);
        private HashMap<uint, VertexArrayObject> ShowTerrain3DLines_VBO = new HashMap<uint, VertexArrayObject>();
        private HashMap<uint, VertexArrayObject> ShowFlagMesh3D_VBO = new HashMap<uint, VertexArrayObject>();
#if TERRAIN_6D
        private HashMap<uint, VertexArrayObject> ShowTerrain3D_VBO1 = new HashMap<uint, VertexArrayObject>();
        private HashMap<uint, VertexArrayObject> ShowTerrain3D_VBO2 = new HashMap<uint, VertexArrayObject>();
        private HashMap<uint, VertexArrayObject> ShowTerrain3D_VBO3 = new HashMap<uint, VertexArrayObject>();
        private HashMap<uint, VertexArrayObject> ShowTerrain3D_VBO4 = new HashMap<uint, VertexArrayObject>();
        private HashMap<uint, VertexArrayObject> ShowTerrain3D_VBO5 = new HashMap<uint, VertexArrayObject>();
        private HashMap<uint, VertexArrayObject> ShowTerrain3D_VBO6 = new HashMap<uint, VertexArrayObject>();
#else
        private VertexArrayObject ShowTerrain3D_VBO = new VertexArrayObject(PrimitiveType.Quads);
#endif   
        protected virtual void RebuildVoxelTerrain()
        {
            InitVoxelTerrain(this.VoxelTerrain);
        }
        public virtual void InitVoxelTerrain(VoxelTerrain3D zone)
        {
            if (zone == null) return;
            this.TerrainSize = new SizeF(zone.TotalSizeX, zone.TotalSizeY);
            this.GridSize = zone.GridCellSize;
            this.VoxelTerrain = zone;
            this.ShowFlagMesh3D_VBO.RunAndClear(e => { e.Value.Flush(); });
#if TERRAIN_6D
            this.ShowTerrain3D_VBO1.RunAndClear(e => { e.Value.Flush(); });
            this.ShowTerrain3D_VBO2.RunAndClear(e => { e.Value.Flush(); });
            this.ShowTerrain3D_VBO3.RunAndClear(e => { e.Value.Flush(); });
            this.ShowTerrain3D_VBO4.RunAndClear(e => { e.Value.Flush(); });
            this.ShowTerrain3D_VBO5.RunAndClear(e => { e.Value.Flush(); });
            this.ShowTerrain3D_VBO6.RunAndClear(e => { e.Value.Flush(); });
#else
            this.ShowTerrain3D_VBO.Flush();
            this.ShowTerrain3D_VBO.SetShader(Shaders.GetOrAdd("tint", n =>
            {
                var lightShader = new LightingShader();
                lightShader.OnShaderBegin += (e, s) =>
                {
                    lightShader.LightPosition = this.Camera.CamPosition;
                };
                return lightShader;
            }));
#endif
            this.ShowPathMesh3D_VBO.Flush();
            if (this.VoxelTerrain != null)
            {
                var voxel = this.VoxelTerrain;
                var cellsize = voxel.GridCellSize;
                var cellr = cellsize / 2f;
                for (int x = voxel.XCount - 1; x >= 0; --x)
                {
                    for (int y = voxel.YCount - 1; y >= 0; --y)
                    {
                        var cell = voxel.GetVoxelCell(x, y);
                        if (cell != null)
                        {
                            for (int la = 0; la < cell.LayerCount; la++)
                            {
                                var layer = cell.GetLayer(la);
                                var color = GLUtils.Argb2Color4(layer.Color.ARGB);
                                color.A *= BlendAlpha;
                                color.R *= BlendGray;
                                color.G *= BlendGray;
                                color.B *= BlendGray;
                                var min = new Vector3(x * cellsize, y * cellsize, layer.Downward);
                                var max = new Vector3(min.X + cellsize, min.Y + cellsize, layer.Upward);
                                var top = new Vector3(x * cellsize + cellr, y * cellsize + cellr, layer.Upward);
                                var bot = new Vector3(top.X, top.Y, layer.Downward);
                                // flags
                                if (ShowFlagMesh3D)
                                {
                                    var list = ShowFlagMesh3D_VBO.GetOrAdd(layer.Color, flag => new VertexArrayObject(PrimitiveType.Quads, color));
                                    list.AddPlane2D(min.To2D(), max.To2D(), max.Z);
                                }
                                // terrain
                                {
                                    if (!layer.IsPlane)
                                    {
                                        if (ShowTerrain3DLines)
                                        {
                                            var list = ShowTerrain3DLines_VBO.GetOrAdd(layer.Color, flag => new VertexArrayObject(PrimitiveType.Lines, color));
                                            list.Add(new Vector3(min.X, max.Z, min.Y)); list.Add(new Vector3(max.X, max.Z, min.Y));
                                            list.Add(new Vector3(max.X, max.Z, min.Y)); list.Add(new Vector3(max.X, max.Z, max.Y));
                                            list.Add(new Vector3(max.X, max.Z, max.Y)); list.Add(new Vector3(min.X, max.Z, max.Y));
                                            list.Add(new Vector3(min.X, max.Z, max.Y)); list.Add(new Vector3(min.X, max.Z, min.Y));
                                            list.Add(new Vector3(min.X, max.Z, min.Y)); list.Add(new Vector3(max.X, max.Z, max.Y));
                                            list.Add(new Vector3(max.X, max.Z, min.Y)); list.Add(new Vector3(min.X, max.Z, max.Y));
                                            list.Add(new Vector3(top.X, top.Z, top.Y));
                                            list.Add(new Vector3(bot.X, bot.Z, bot.Y));
                                        }
                                        if (ShowTerrain3D)
                                        {
                                            var c = color;
#if TERRAIN_6D
                                            var list1 = ShowTerrain3D_VBO1.GetOrAdd(layer.Color, flag => new VertexArrayObject(PrimitiveType.Quads, c.Mul(1)));
                                            var list2 = ShowTerrain3D_VBO2.GetOrAdd(layer.Color, flag => new VertexArrayObject(PrimitiveType.Quads, c.Mul(0.4f)));
                                            var list3 = ShowTerrain3D_VBO3.GetOrAdd(layer.Color, flag => new VertexArrayObject(PrimitiveType.Quads, c.Mul(0.8f)));
                                            var list4 = ShowTerrain3D_VBO4.GetOrAdd(layer.Color, flag => new VertexArrayObject(PrimitiveType.Quads, c.Mul(0.8f)));
                                            var list5 = ShowTerrain3D_VBO5.GetOrAdd(layer.Color, flag => new VertexArrayObject(PrimitiveType.Quads, c.Mul(0.6f)));
                                            var list6 = ShowTerrain3D_VBO6.GetOrAdd(layer.Color, flag => new VertexArrayObject(PrimitiveType.Quads, c.Mul(0.6f)));
                                            list1.AddBox2D(min, max, BoxFlag.TOP);
                                            list2.AddBox2D(min, max, BoxFlag.BOTTOM);
                                            list3.AddBox2D(min, max, BoxFlag.FORTH);
                                            list4.AddBox2D(min, max, BoxFlag.BACK);
                                            list5.AddBox2D(min, max, BoxFlag.LEFT);
                                            list6.AddBox2D(min, max, BoxFlag.RIGHT);
#else
                                            ShowTerrain3D_VBO.SetColor(c);
                                            ShowTerrain3D_VBO.AddBox2D(min, max);
#endif
                                        }
                                    }
                                }
                                if (ShowPathMesh3D)
                                {
                                    // path
                                    layer.ForEachNextNodes(0, (next, _) =>
                                    {
                                        ShowPathMesh3D_VBO.Add(new Vector3(top.X, top.Z + PATH_Y_OFFSET, top.Y));
                                        ShowPathMesh3D_VBO.Add(new Vector3(next.X * cellsize + cellr, next.Upward + PATH_Y_OFFSET, next.Y * cellsize + cellr));
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }
        public virtual void RefreshVoxelTerrainPath()
        {
            this.ShowPathMesh3D_VBO.Flush();
            if (ShowPathMesh3D && this.VoxelTerrain != null)
            {
                var voxel = this.VoxelTerrain;
                var cellsize = voxel.GridCellSize;
                var cellr = cellsize / 2f;
                for (int x = voxel.XCount - 1; x >= 0; --x)
                {
                    for (int y = voxel.YCount - 1; y >= 0; --y)
                    {
                        var cell = voxel.GetVoxelCell(x, y);
                        if (cell != null)
                        {
                            for (int la = 0; la < cell.LayerCount; la++)
                            {
                                var layer = cell.GetLayer(la);
                                var color = GLUtils.Argb2Color4(layer.Color);
                                color.A *= BlendAlpha;
                                var min = new Vector3(x * cellsize, y * cellsize, layer.Downward);
                                var max = new Vector3(min.X + cellsize, min.Y + cellsize, layer.Upward);
                                var top = new Vector3(x * cellsize + cellr, y * cellsize + cellr, layer.Upward);
                                var bot = new Vector3(top.X, top.Y, layer.Downward);
                                // path
                                layer.ForEachNextNodes(0, (next, _) =>
                                {
                                    ShowPathMesh3D_VBO.Add(new Vector3(top.X, top.Z + PATH_Y_OFFSET, top.Y));
                                    ShowPathMesh3D_VBO.Add(new Vector3(next.X * cellsize + cellr, next.Upward + PATH_Y_OFFSET, next.Y * cellsize + cellr));
                                });
                            }
                        }
                    }
                }
            }
        }


        protected virtual void DrawTerrain3D(PaintEventArgs3D e)
        {
            GL.Disable(EnableCap.CullFace);
            if (VoxelTerrain != null)
            {
                DrawingVoxelObject.DrawBounds(Color4.White, 0, 0, VoxelTerrain.TotalSizeX, VoxelTerrain.TotalSizeY, VoxelTerrain.GridCellSize);
            }
            if (ShowTerrain3D)
            {
#if TERRAIN_6D
                foreach (var list in ShowTerrain3D_VBO1.Values) { list.Draw(); }
                foreach (var list in ShowTerrain3D_VBO2.Values) { list.Draw(); }
                foreach (var list in ShowTerrain3D_VBO3.Values) { list.Draw(); }
                foreach (var list in ShowTerrain3D_VBO4.Values) { list.Draw(); }
                foreach (var list in ShowTerrain3D_VBO5.Values) { list.Draw(); }
                foreach (var list in ShowTerrain3D_VBO6.Values) { list.Draw(); }
#else
                ShowTerrain3D_VBO.Draw(e);
#endif
            }
            if (ShowTerrain3DLines)
            {
                foreach (var list in ShowTerrain3DLines_VBO.Values) { list.Draw(); }
            }
            if (ShowFlagMesh3D)
            {
                foreach (var list in ShowFlagMesh3D_VBO.Values) { list.Draw(); }
            }
            if (ShowPathMesh3D)
            {
                ShowPathMesh3D_VBO.Draw();
            }
        }
        private void RenderVoxelTerrain(PaintEventArgs3D e)
        {
            DrawTerrain3D(e);
        }
        public VoxelLayer RayCastVoxelFromMouse(System.Drawing.Point mouse, out Vector3 touch)
        {
            var ray = Camera.ScreenToWorldRay(new Vector2(mouse.X, mouse.Y));
            return RayCastVoxel(ray, out touch);
        }
        public VoxelLayer RayCastVoxel(Glu.Ray ray, out Vector3 touch)
        {
            if (VoxelTerrain != null)
            {
                var pos = ray.center.ToGeometry().XZ;
                var dir = CMath.GetDegree(ray.normal.X, ray.normal.Z);
                var sqrt = (float)Math.Sqrt(VoxelTerrain.TotalSizeX * VoxelTerrain.TotalSizeX + VoxelTerrain.TotalSizeY * VoxelTerrain.TotalSizeY);
                var len = CMath.GetDistance(0, 0, ray.normal.X, ray.normal.Z) * sqrt;
                VoxelLayer ret = null;
                Vector3 ray_touch = Vector3.Zero;
                VoxelTerrain.ForEachCellsRayStepPloar(ref ray, ref pos, dir, len, (t, cell, cx, cy, current) =>
                {
                    if (cell != null)
                    {
                        var pp = new Vector3(current.X, 0, current.Y);
                        for (int i = cell.LayerCount - 1; i >= 0; --i)
                        {
                            var layer = cell.GetLayer(i);
                            pp.Y = layer.Upward;
                            ray_touch = Glu.RayPlaneIntersection(ray, new Glu.Plane(pp, Vector3.UnitY));
                            if (CMath.IncludeRectPointW(cell.X * GridSize, cell.Y * GridSize, GridSize, GridSize, ray_touch.X, ray_touch.Z))
                            {
                                ret = layer;
                                return true;
                            }
                        }
                    }
                    return false;
                }, false);
                touch = ray_touch;
                return ret;
            }
            touch = Vector3.Zero;
            return null;
        }
        public VoxelLayer RayCastVoxelBounding(Glu.Ray ray, out Vector3 touch)
        {
            if (VoxelTerrain != null)
            {
                var pos = ray.center.ToGeometry().XZ;
                var dir = CMath.GetDegree(ray.normal.X, ray.normal.Z);
                var sqrt = (float)Math.Sqrt(VoxelTerrain.TotalSizeX * VoxelTerrain.TotalSizeX + VoxelTerrain.TotalSizeY * VoxelTerrain.TotalSizeY);
                var len = CMath.GetDistance(0, 0, ray.normal.X, ray.normal.Z) * sqrt;
                VoxelLayer ret = null;
                Vector3? ray_touch = null;
                VoxelTerrain.ForEachCellsRayStepPloar(ref ray, ref pos, dir, len, (t, cell, cx, cy, current) =>
                {
                    if (cell != null)
                    {
                        var pp = new Vector3(current.X, 0, current.Y);
                        for (int i = cell.LayerCount - 1; i >= 0; --i)
                        {
                            var layer = cell.GetLayer(i);
                            pp.Y = layer.Upward;
                            var geoBox = layer.GetBlockBoundingBox();
                            ray_touch = Glu.RayBoundingBoxIntersection(ray, new Glu.BoundingBox(
                                geoBox.Min.ToGL().ObjectToWorld(),
                                geoBox.Max.ToGL().ObjectToWorld()));
                            if (ray_touch != null)
                            {
                                ret = layer;
                                return true;
                            }
                            //                         ray_touch = Glu.RayPlaneIntersection(ray, new Glu.Plane(pp, Vector3.UnitY));
                            //                         if (CMath.IncludeRectPointW(cell.X * GridSize, cell.Y * GridSize, GridSize, GridSize, ray_touch.X, ray_touch.Z))
                            //                         {
                            //                             ret = layer;
                            //                             return true;
                            //                         }
                        }
                    }
                    return false;
                }, false);
                if (ray_touch != null)
                {
                    touch = ray_touch.Value;
                    return ret;
                }
            }
            touch = Vector3.Zero;
            return null;
        }

        #endregion
        //-----------------------------------------------------------------------------------------------------------------
        protected override void RenderObjects(PaintEventArgs3D e)
        {
            if (ShowObjects)
            {
                base.RenderObjects(e);
            }
        }
    }



}
