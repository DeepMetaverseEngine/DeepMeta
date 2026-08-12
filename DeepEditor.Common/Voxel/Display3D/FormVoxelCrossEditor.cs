using DeepCore.Astar;
using DeepCore.Reflection;
using DeepCore.Voxel.Data;
using DeepCore.Xml;
using DeepEditor.Common.Controls;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G3D;
using G3D.ObjRenderer;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.Display3D
{
    public partial class FormVoxelCrossEditor : DeepEditor.Common.G2D.G2DBaseForm
    {
        private readonly static Random random = new Random();
        private readonly DisplayVoxelWorld3D canvas;
        private UndoRedoManager cmd_queue = new UndoRedoManager(100);
        public FormVoxelCrossEditor()
        {
            InitializeComponent();
            this.canvas = new DisplayVoxelWorld3D(this.glControl1, this.timer1);
            this.canvas.OnUpdate += Canvas_OnUpdate;
            this.canvas.OnEndRender += Canvas_OnEndRender;
            this.canvas.ShowFlagMesh3D = false;
            this.canvas.ShowPathFinder = false;
            this.canvas.ShowPathMesh3D = true;
            this.canvas.ShowTerrain3D = false;
            this.canvas.BindMeshDropDownMenu(menu_Meshs);
            new DropDownFieldMaskGenerator(canvas, menu_View, "show");
            this.glControl1.MouseDown += GlControl1_MouseDown;
            this.glControl1.MouseMove += GlControl1_MouseMove;
            this.cmd_queue.BindButton(btn_Redo, btn_Undo);
            this.cmd_queue.BindButton(btn_Redo2, btn_Undo2);
            new ExclusionToolStripButtonGroup(chk_MoveBrush, chk_MoveEraser, chk_Test);
            this.g2DPropertyGrid1.SetSelectedObject(new Config());

        }
        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        //--------------------------------------------------------------------------------------------------------------------------
        #region FileSaveLoad
        private bool resetCamera = true;
        private VoxelWorld currentWorld;
        private FileInfo lastVoxelBinFile;
        private FileInfo lastVoxelXmlFile;
        private VoxelBuildConfig prop = Voxel3DPlugin.Instance.CreateVoxelBuildConfig();

        protected void SetConfig(VoxelBuildConfig prop)
        {
            this.prop = prop;
            this.Reload();
        }
        protected VoxelWorld LoadVoxelData(string xmlFile, VoxelTerrainData data)
        {
            //             var terrain = new VoxelTerrain3D(data, prop);
            //             var astar = VoxelWorldManager.Instance.CreateVoxelAstar(terrain);
            if (Voxel3DPlugin.TryConvertWorldDialog(this, xmlFile, data, prop, out var binFile, out var world))
            {
                return this.LoadVoxelWorld(world);
            }
            return null;
        }

        protected VoxelWorld LoadVoxelWorld(VoxelWorld wd)
        {
            this.currentWorld = wd;
            this.prop = wd.Terrain.BuildConfig;
            this.canvas.InitVoxelWorld(this.currentWorld);
            if (resetCamera)
            {
                resetCamera = false;
                this.canvas.ResetCameraPos();
            }
            this.txt_State.Text = currentWorld.Terrain.ToString();
            return wd;
        }
        public VoxelWorld LoadVoxelWorld(VoxelWorld wd, string voxelBinFile)
        {
            this.LoadVoxelWorld(wd);
            this.lastVoxelBinFile = new FileInfo(voxelBinFile);
            return wd;
        }
        public VoxelWorld LoadVoxelFile(string voxelBinFile)
        {
            try
            {
                var wd = VoxelWorld.LoadFromFile(voxelBinFile);
                this.LoadVoxelWorld(wd, voxelBinFile);
                this.lastVoxelBinFile = new FileInfo(voxelBinFile);
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            return currentWorld;
        }
        public VoxelWorld LoadVoxelBin(string voxelBinFile)
        {
            //             var bin = File.ReadAllBytes(voxelBinFile);
            //             var wd = VoxelWorld.LoadFromBin(bin);
            var wd = VoxelWorld.LoadFromFile(voxelBinFile);
            LoadVoxelWorld(wd);
            lastVoxelBinFile = new FileInfo(voxelBinFile);
            return currentWorld;
        }
        public void LoadVoxelXML(string voxelXmlFile)
        {
            try
            {
                if (Voxel3DPlugin.TryLoadTerrainDataDialog(voxelXmlFile, out var data))
                {
                    var wd = this.LoadVoxelData(voxelXmlFile, data);
                    //VoxelWorld.SaveToFile(wd, voxelXmlFile + VoxelWorld.FILE_EXT);
                    lastVoxelXmlFile = new FileInfo(voxelXmlFile);
                    lastVoxelBinFile = new FileInfo(voxelXmlFile + VoxelWorld.FILE_EXT);
                }
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
        }
        private void SaveVoxel()
        {
            if (currentWorld != null && this.lastVoxelBinFile != null)
            {
                if (Voxel3DPlugin.TryRebuildWorldDialog(this, this.lastVoxelBinFile.Directory, this.currentWorld, out var newWorld))
                {
                    LoadVoxelWorld(newWorld);
                }
            }
            VoxelWorldManager.Instance.Clear();
        }
        public void Reload()
        {
            if (lastVoxelXmlFile != null && lastVoxelXmlFile.Exists)
            {
                LoadVoxelXML(lastVoxelXmlFile.FullName);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------

        private void btn_Load_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.Filter = "zip|*.zip|xml|*.xml";
            if (fd.ShowDialog(this) == DialogResult.OK)
            {
                LoadVoxelXML(fd.FileName);
            }
        }
        private void btn_SavePathCache_Click(object sender, EventArgs e)
        {
            if (currentWorld != null)
            {
                var fn = lastVoxelBinFile?.FullName;
                if (Path.GetExtension(fn) == VoxelWorld.FILE_EXT)
                {
                    fn = fn.Substring(0, fn.Length - 4) + ".path";
                }
                var fd = new SaveFileDialog();
                fd.FileName = Path.GetFileName(fn); ;
                fd.InitialDirectory = Path.GetDirectoryName(fn);
                fd.DefaultExt = ".path";
                if (fd.ShowDialog(this) == DialogResult.OK)
                {
                    this.canvas.GLControl.Visible = false;
                    //                     new G2DProgressDialog("save path cache", true, (p) =>
                    //                     {
                    //                         currentWorld.PathFinder.SaveFileCache(new FileInfo(fd.FileName), p);
                    //                     }).ShowDialog(this);
                    this.canvas.GLControl.Visible = true;
                }
            }
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (this.currentWorld != null)
            {
                this.SaveVoxel();
            }
        }

        private void btn_Properties_Click(object sender, EventArgs e)
        {
            var d = new G2DDataDialog.G2DObjectDialog<VoxelBuildConfig>(this.prop);
            if (d.ShowDialog(this) == DialogResult.OK)
            {
                this.SetConfig(d.SelectedObject);
            }
        }

        #endregion
        //--------------------------------------------------------------------------------------------------------------------------
        #region Terrain
        public class Config
        {
            [Desc("笔刷尺寸范围")]
            public int FillBrushSize = 2;
            [Desc("笔刷连通高度")]
            public float FillBrushStep = 3f;
            [Desc("寻路步数限制")]
            public int FindPathStepLimit = 10000;
            [Desc("仅抹除相同颜色")]
            public bool OnlyClearSameColor = true;
        }
        private const float PATH_Y_OFFSET = 0.02f;
        //--------------------------------------------------------------------------------------------------------------------------
        public bool IsFillTerrainMode { get => chk_MoveBrush.Checked; }
        public bool IsClearTerrainMode { get => chk_MoveEraser.Checked; }
        public bool IsTestMode { get => chk_Test.Checked; }
        //--------------------------------------------------------------------------------------------------------------------------
        public delegate bool CheckNextNode(VoxelLayer current, VoxelLayer next);
        //--------------------------------------------------------------------------------------------------------------------------
        private bool OnlyClearSameColor { get => g2DPropertyGrid1.GetSelectedValueAs<Config>().OnlyClearSameColor; }
        private int FillBrushSize { get => g2DPropertyGrid1.GetSelectedValueAs<Config>().FillBrushSize; }
        private float FillBrushStep { get => g2DPropertyGrid1.GetSelectedValueAs<Config>().FillBrushStep; }
        private int FindPathStepLimit { get => g2DPropertyGrid1.GetSelectedValueAs<Config>().FindPathStepLimit; }
        private void ForEachTerrain(VoxelLayer current, float stepMax, HashSet<VoxelLayer> exist, CheckNextNode testLink)
        {
            if (current != null)
            {
                if (!exist.Contains(current))
                {
                    exist.Add(current);
                    current.ForEachNearCell(0, (ox, oy, cell, st) =>
                    {
                        var near = cell.FindNearAltitude(current.Upward, stepMax);
                        if (near != null && testLink(current, near))
                        {
                            ForEachTerrain(near, stepMax, exist, testLink);
                        }
                    });
                }
            }
        }
        private void ForEachTerrainFillWalk(VoxelLayer current, float stepMax, int deep, HashSet<VoxelLayer> exist, CheckNextNode testLink)
        {
            if (current != null && deep > 0)
            {
                if (!exist.Contains(current))
                {
                    exist.Add(current);
                    current.ForEachNearCell(0, (ox, oy, cell, st) =>
                    {
                        var near = cell.FindNearAltitude(current.Upward, stepMax);
                        if (near != null && testLink(current, near))
                        {
                            ForEachTerrainFillWalk(near, stepMax, deep - 1, exist, testLink);
                        }
                    });
                }
            }
        }
        private void FillTerrainWalk(VoxelLayer current)
        {
            if (current == null) return;
            var list = new List<Tuple<VoxelLayer, VoxelLayer>>();
            ForEachTerrainFillWalk(current, FillBrushStep, FillBrushSize, new HashSet<VoxelLayer>(), (c, n) =>
            {
                if (c.TestLinkNextNode(n))
                {
                    list.Add(new Tuple<VoxelLayer, VoxelLayer>(c, n));
                }
                return true;
            });
            if (list.Count > 0)
            {
                cmd_queue.ExecuteAs((redo) =>
                {
                    var changed = false;
                    foreach (var n in redo)
                    {
                        changed |= n.Item1.TryLinkNextNode(n.Item2);
                    }
                    if (changed)
                        canvas.RefreshVoxelTerrainPath();
                }, (undo) =>
                {
                    var changed = false;
                    foreach (var n in undo)
                    {
                        changed |= n.Item1.TryUnlinkNextNode(n.Item2);
                    }
                    if (changed)
                        canvas.RefreshVoxelTerrainPath();
                }, list);
            }
        }
        private void ClearTerrainWalk(VoxelLayer current)
        {
            if (current == null) return;
            var clearColor = OnlyClearSameColor;
            var list = new List<Tuple<VoxelLayer, VoxelLayer>>();
            var baseColor = current.Color;
            ForEachTerrainFillWalk(current, FillBrushStep, FillBrushSize, new HashSet<VoxelLayer>(), (c, n) =>
            {
                if (c.TestUnlinkNextNode(n))
                {
                    if (clearColor)
                    {
                        if (c.Color == baseColor)
                        {
                            list.Add(new Tuple<VoxelLayer, VoxelLayer>(c, n));
                        }
                    }
                    else
                    {
                        list.Add(new Tuple<VoxelLayer, VoxelLayer>(c, n));
                    }
                }
                return true;
            });
            if (list.Count > 0)
            {
                cmd_queue.ExecuteAs((redo) =>
                {
                    var changed = false;
                    foreach (var n in redo)
                    {
                        changed |= n.Item1.TryUnlinkNextNode(n.Item2);
                    }
                    if (changed)
                        canvas.RefreshVoxelTerrainPath();
                }, (undo) =>
                {
                    var changed = false;
                    foreach (var n in undo)
                    {
                        changed |= n.Item1.TryLinkNextNode(n.Item2);
                    }
                    if (changed)
                        canvas.RefreshVoxelTerrainPath();
                }, list);
            }

        }

#if DrawTrangles
        private List<DeepCore.Geometry.Triangles> trangles = new List<DeepCore.Geometry.Triangles>();
        private void DrawTriangles()
        {
            foreach (var t in trangles)
            {
                t.ForEachTrangles((tg) =>
                {
                    DrawingObject.DrawStar(PrimitiveType.LineLoop, Color4.AliceBlue, tg.A.ToGL(), 1);
                    DrawingObject.DrawStar(PrimitiveType.LineLoop, Color4.AliceBlue, tg.B.ToGL(), 1);
                    DrawingObject.DrawStar(PrimitiveType.LineLoop, Color4.AliceBlue, tg.C.ToGL(), 1);
                });
            }
        }
        private void AddTriangles(DeepCore.Geometry.Triangles t)
        {
            trangles.Add(t);
        }
#else
        private void AddTriangles(DeepCore.Geometry.Triangles t) { }
        private void DrawTriangles() { }
#endif
        private void DrawPathFinder()
        {
            if (canvas.VoxelTerrain != null)
            {
                var terrain = this.canvas.VoxelTerrain;
                if (mouse_FindPath != null)
                {
                    var color1 = Color4.DarkBlue;
                    var color2 = Color4.Magenta;
                    color1.A = color2.A = (float)Math.Abs(Math.Sin(canvas.TotalTimeSEC * 2f));
                    DrawingVoxelObject.DrawWayPoint(color1, color2, mouse_FindPath, canvas.VoxelTerrain.GridCellSize,
                        new Vector3(0, 0, PATH_Y_OFFSET));
                }
                if (mouse_PathBegin != null)
                {
                    DrawingVoxelObject.DrawSphere3D(Color4.Pink, mouse_PathBegin.UpwardCenterPos.ToGL(), terrain.GridCellSize);
                }
                if (mouse_PathEnd != null)
                {
                    DrawingVoxelObject.DrawSphere3D(Color4.LightCyan, mouse_PathEnd.UpwardCenterPos.ToGL(), terrain.GridCellSize);
                }
            }
        }
        private void ClickPathFinder()
        {
            if (mouse_PathBegin == null || mouse_PathEnd != null)
            {
                mouse_PathBegin = raycastLayer;
                mouse_PathEnd = null;
                mouse_FindPath = null;
            }
            else if (mouse_PathEnd == null)
            {
                mouse_PathEnd = raycastLayer;
            }
            if (mouse_PathBegin != null && mouse_PathEnd != null)
            {
                //var param = g2DPropertyGrid1.GetSelectedValue() as FindPathParams;
                canvas.World3D.PathFinder.FindPathStepLimit = FindPathStepLimit;
                mouse_FindPath = canvas.World3D.PathFinder.FindPathByLayer(mouse_PathBegin, mouse_PathEnd);
            }
        }

        private Vector3 raycastLayerTouch;
        private VoxelLayer raycastLayer;

        private VoxelLayer mouse_PathBegin;
        private VoxelLayer mouse_PathEnd;
        private IVoxelWayPoint mouse_FindPath;
        //--------------------------------------------------------------------------------------------------------------------------
        private void Canvas_OnUpdate(Common.G3D.GLView sender, TimeSpan interval)
        {
            if (chk_Test.Checked)
            {
                var mouse = sender.GLControl.PointToClient(Control.MousePosition);
                this.raycastLayer = canvas.RayCastVoxelFromMouse(mouse, out raycastLayerTouch);
            }
            else
            {
                var mouse = sender.GLControl.PointToClient(Control.MousePosition);
                var ray = canvas.Camera.ScreenToWorldRay(new Vector2(mouse.X, mouse.Y));
                this.raycastLayer = canvas.RayCastVoxelBounding(ray, out raycastLayerTouch);
                //this.raycastLayer = canvas.RayCastVoxelFromMouse(mouse, out raycastLayerTouch);
            }
        }
        private void Canvas_OnEndRender(Common.G3D.GLView sender, Common.G3D.PaintEventArgs3D e)
        {
            var terrain = this.canvas.VoxelTerrain;
            if (terrain == null) { return; }
            if (IsTestMode)
            {
                if (raycastLayer != null)
                {
                    var opos = raycastLayerTouch.WorldToObject();
                    opos.Z += PATH_Y_OFFSET;

                    var color = Color4.Yellow;
                    color.A = (float)Math.Abs(Math.Sin(canvas.TotalTimeSEC * 3f));
                    DrawingVoxelObject.DrawVoxel(color, raycastLayer);

                    DrawingVoxelObject.DrawCycle(Color4.Yellow, opos, terrain.GridCellRadius);
                    color = Color4.White;
                    color.A = (float)Math.Abs(Math.Sin(canvas.TotalTimeSEC * 2f));
                    DrawingVoxelObject.FillRectW(
                        color,
                        raycastLayer.X * terrain.GridCellSize,
                        raycastLayer.Y * terrain.GridCellSize,
                        terrain.GridCellSize,
                        terrain.GridCellSize,
                        raycastLayer.Upward + PATH_Y_OFFSET);
                }
                DrawPathFinder();
            }
            else if (IsFillTerrainMode)
            {
                var color = Color4.Yellow;
                if (raycastLayer != null)
                {
                    var opos = raycastLayerTouch.WorldToObject();
                    opos.Z += PATH_Y_OFFSET;
                    DrawingVoxelObject.DrawCycle(Color4.Yellow, opos, terrain.GridCellRadius);
                    color.A = (float)Math.Abs(Math.Sin(canvas.TotalTimeSEC * 3f));
                    DrawingVoxelObject.DrawVoxel(color, raycastLayer);
                }
                color = Color4.LightGreen;
                color.A = (float)Math.Abs(Math.Sin(canvas.TotalTimeSEC * 4f));
                GL.Begin(PrimitiveType.Lines);
                GL.Color4(color);
                float stepMax = FillBrushStep;
                ForEachTerrainFillWalk(raycastLayer, FillBrushStep, FillBrushSize, new HashSet<VoxelLayer>(), (c, n) =>
                {
                    //if (c.TestLinkNextNode(n))
                    {
                        var srcP = c.UpwardCenterPos;
                        var dstP = n.UpwardCenterPos;
                        GL.Vertex3(new Vector3(srcP.X, srcP.Z + PATH_Y_OFFSET, srcP.Y));
                        GL.Vertex3(new Vector3(dstP.X, dstP.Z + PATH_Y_OFFSET, dstP.Y));
                    }
                    return true;
                });
                GL.End();
            }
            else if (IsClearTerrainMode)
            {
                var color = Color4.Yellow;
                if (raycastLayer != null)
                {
                    var opos = raycastLayerTouch.WorldToObject();
                    opos.Z += PATH_Y_OFFSET;
                    DrawingVoxelObject.DrawCycle(Color4.Yellow, opos, terrain.GridCellRadius);
                    color.A = (float)Math.Abs(Math.Sin(canvas.TotalTimeSEC * 3f));
                    DrawingVoxelObject.DrawVoxel(color, raycastLayer);
                }
                color = Color4.Magenta;
                color.A = (float)Math.Abs(Math.Sin(canvas.TotalTimeSEC * 4f));
                GL.Begin(PrimitiveType.Lines);
                GL.Color4(color);
                float stepMax = FillBrushStep;
                ForEachTerrainFillWalk(raycastLayer, FillBrushStep, FillBrushSize, new HashSet<VoxelLayer>(), (c, n) =>
                {
                    //if (c.TestUnlinkNextNode(n))
                    {
                        var srcP = c.UpwardCenterPos;
                        var dstP = n.UpwardCenterPos;
                        GL.Vertex3(new Vector3(srcP.X, srcP.Z + PATH_Y_OFFSET, srcP.Y));
                        GL.Vertex3(new Vector3(dstP.X, dstP.Z + PATH_Y_OFFSET, dstP.Y));
                    }
                    return true;
                });
                GL.End();
            }
            DrawTriangles();
        }
        private void GlControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsTestMode)
            {
                ClickPathFinder();
            }
            else if (IsFillTerrainMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    FillTerrainWalk(raycastLayer);
                }
            }
            else if (IsClearTerrainMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ClearTerrainWalk(raycastLayer);
                }
            }
        }
        private void GlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsFillTerrainMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    FillTerrainWalk(raycastLayer);
                }
            }
            else if (IsClearTerrainMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ClearTerrainWalk(raycastLayer);
                }
            }
        }

        private void Chk_MoveBrush_Click(object sender, EventArgs e)
        {
            mouse_FindPath = null;
            mouse_PathEnd = mouse_PathBegin = null;
        }
        private void Chk_MoveEraser_Click(object sender, EventArgs e)
        {
            mouse_FindPath = null;
            mouse_PathEnd = mouse_PathBegin = null;
        }
        private void chk_Test_Click(object sender, EventArgs e)
        {
            mouse_FindPath = null;
            mouse_PathEnd = mouse_PathBegin = null;
        }


        private void chk_2D_Click(object sender, EventArgs e)
        {
            if (canvas != null)
            {
                if (chk_2D.Checked)
                {
                    canvas.SetCameraControlWithType(CameraType.Camera2D);
                }
                else
                {
                    canvas.SetCameraControlWithType(CameraType.Camera3D);
                }
            }
        }
        private void btn_LoadMesh_Click(object sender, EventArgs e)
        {
            canvas.LoadMeshDialog();
        }
        private void btn_LoadMeshDX_Click(object sender, EventArgs e)
        {
            canvas.LoadMeshDialog(new ObjLoaderConfig()
            {
                ScaleX = -1,
                ScaleZ = -1,
                TranslationZ = -(this.canvas?.VoxelTerrain?.TotalSizeY) ?? 0,
            });
        }
        private void btn_ClearMesh_Click(object sender, EventArgs e)
        {
            canvas.ClearMesh();
        }

        private void btn_CombineMeshWeight_Click(object sender, EventArgs e)
        {
            new G2DProgressDialog("合并主路", progress =>
            {
                var meshs = canvas.GetMeshs();
                foreach (var mesh in meshs)
                {
                    var t = mesh.ToVoxelTriangles(canvas.VoxelTerrain);
                    canvas.VoxelPathMap.CombineMesh(t, 1);
                    AddTriangles(t);
                }
            }).ShowDialog(this);
            this.canvas.InitVoxelWorld(this.currentWorld);
        }

        private void btn_makeVoxel2DPlane_Click(object sender, EventArgs e)
        {
            if (currentWorld == null || this.lastVoxelBinFile == null)
            {
                return;
            }
            var src = currentWorld;
            var data = new G2DProgressDialog<VoxelTerrainData>(
              $"生成新地形",
              range =>
              {
                  var newTerrainData = new VoxelTerrainData()
                  {
                      MinX = src.Terrain.ResourceStartX,
                      MinY = src.Terrain.ResourceStartY,
                      MaxX = src.Terrain.ResourceStartX + src.Terrain.TotalSizeX,
                      MaxY = src.Terrain.ResourceStartY + src.Terrain.TotalSizeY,
                      XLength = src.Terrain.XCount,
                      YLength = src.Terrain.YCount,
                      GridSize = src.Terrain.GridCellSize,
                      Grids = new VoxelNodeData[src.Terrain.XCount, src.Terrain.YCount][],
                      MinHeight = src.Terrain.BuildConfig.VoxelMinHeight,
                  };
                  range.SetMax(src.Terrain.XCount * src.Terrain.YCount);
                  src.Terrain.ForEachCells(0, (cell, st) =>
                  {
                      if (cell != null && cell.LayerCount > 0)
                      {
                          var top = cell.TopLayer;
                          newTerrainData.Grids[cell.X, cell.Y] = new VoxelNodeData[] { new VoxelNodeData()
                          {
                              Color = top.Color.ARGB,
                              Downward = top.Upward - newTerrainData.MinHeight,
                              Upward = top.Upward
                          }};
                      }
                      range.Add(1);
                      return false;
                  });
                  return newTerrainData;
              }).ShowDialogWith(this);
            if (data == null) return;

            var dst = new G2DProgressDialog<VoxelWorld>(
                $"生成新体素",
                range =>
                {
                    var voxelXmlFile = src.FileName;
                    var cfg = XmlUtil.CloneObject(src.Terrain.BuildConfig);
                    cfg.FlipY = false;
                    var terrain = new VoxelTerrain3D(data, cfg, range);
                    var astar = VoxelWorldManager.Instance.CreateVoxelAstar(terrain, range);
                    var retWorld = new VoxelWorld(voxelXmlFile, terrain, astar);
                    return retWorld;
                }).ShowDialogWith(this);
            if (dst == null) return;
            LoadVoxelWorld(dst);
            //SaveVoxel();
        }

        //-----------------------------------------------------------------------------------------------------------

        private void btn_OptimizeSave_Click(object sender, EventArgs e)
        {
            var evt = new OptimizeVoxelsEventArgs() { Src = this.currentWorld, KeepUpward = false, };
            OnOptimizeVoxels?.Invoke(this, evt);
            if (evt.Dst != null)
            {
                LoadVoxelWorld(evt.Dst);
            }
            this.SaveVoxel();
        }
        private void btn_optimizeVoxelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var evt = new OptimizeVoxelsEventArgs() { Src = this.currentWorld, KeepUpward = false, };
            OnOptimizeVoxels?.Invoke(this, evt);
            if (evt.Dst != null)
            {
                LoadVoxelWorld(evt.Dst);
            }
        }
        private void btn_optimizeVoxels_KeepUpward_Click(object sender, EventArgs e)
        {
            var evt = new OptimizeVoxelsEventArgs() { Src = this.currentWorld, KeepUpward = true, };
            OnOptimizeVoxels?.Invoke(this, evt);
            if (evt.Dst != null)
            {
                LoadVoxelWorld(evt.Dst);
            }
        }




        public delegate void OptimizeVoxels(object sender, OptimizeVoxelsEventArgs e);
        public class OptimizeVoxelsEventArgs : EventArgs
        {
            public bool KeepUpward = false;
            public VoxelWorld Src;
            public VoxelWorld Dst;
        }
        public event OptimizeVoxels OnOptimizeVoxels;

        #endregion
        //-----------------------------------------------------------------------------------------------------------
    }
}
