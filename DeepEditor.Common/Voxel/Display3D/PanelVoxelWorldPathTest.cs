using DeepCore;
using DeepCore.Astar;
using DeepCore.Geometry.Terrain;
using DeepCore.Geometry;
using DeepCore.Reflection;
using DeepCore.Voxel.Data;
using DeepEditor.Common.Controls;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G3D;
using G3D.ObjRenderer;
using OpenTK.WinForms;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace DeepEditor.Common.Voxel.Display3D
{
    public partial class PanelVoxelWorldPathTest : UserControl
    {
        private static Random random = new Random();
        private DisplayVoxelWorld3D canvas;
        public DisplayVoxelWorld3D Canvas => canvas;
        public GLControl GLControl { get => glControl1; }
        public PanelVoxelWorldPathTest()
        {
            InitializeComponent();
            this.Load += PanelVoxelView3D_Load;
            this.Disposed += PanelVoxelView3D_Disposed;
        }

        private void PanelVoxelView3D_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                this.canvas = new DisplayVoxelWorld3D(this.glControl1, this.timer1);
                this.canvas.ShowTerrain3D = true;
                this.canvas.ShowPathFinder = true;
                this.canvas.OnUpdate += Canvas_OnUpdate;
                this.canvas.OnEndRender += Canvas_OnEndRender;
                this.canvas.OnRenderHUD += Canvas_OnRenderHUD;
                this.canvas.BindMeshDropDownMenu(menu_Meshs);
                new DropDownFieldMaskGenerator(canvas, menu_View, "show");
                this.glControl1.MouseDown += GlControl1_MouseDown;
                this.glControl1.MouseMove += GlControl1_MouseMove;
                this.g2DPropertyGrid1.SetSelectedObject(new TestFindPathParams());
            }
        }
        private void PanelVoxelView3D_Disposed(object sender, EventArgs e)
        {
            drawText.Dispose();
        }
        //--------------------------------------------------------------------------------------------------------------------------
        #region FileSaveLoad

        private VoxelWorld currentWorld;
        private FileInfo lastVoxelBinFile;
        private FileInfo lastVoxelXmlFile;
        private VoxelBuildConfig prop = Voxel3DPlugin.Instance.CreateVoxelBuildConfig();

        public VoxelWorld CurrentWorld { get => currentWorld; }

        protected void SetConfig(VoxelBuildConfig prop)
        {
            this.prop = prop;
            this.Reload();
        }
        protected void LoadVoxelData(string xmlFile, VoxelTerrainData data)
        {
            if (Voxel3DPlugin.TryConvertWorldDialog(this, xmlFile, data, prop, out var binFile, out var world))
            {
                this.LoadVoxelWorld(world);
                lastVoxelXmlFile = new FileInfo(xmlFile);
                lastVoxelBinFile = new FileInfo(binFile);
            }
        }
        protected void LoadNewVoxelWorld()
        {
            if (Voxel3DPlugin.TryLoadNewVoxelWorldDialog(this, out var xmlFile, out var binFile, out var world))
            {
                this.LoadVoxelWorld(world);
                lastVoxelXmlFile = new FileInfo(xmlFile);
                lastVoxelBinFile = new FileInfo(binFile);
            }
        }
        protected void LoadVoxelWorld(VoxelWorld wd)
        {
            this.currentWorld = wd;
            this.prop = wd.Terrain.BuildConfig;
            this.canvas.InitVoxelWorld(this.currentWorld);
            this.canvas.ResetCameraPos();
            this.txt_State.Text = currentWorld.Terrain.ToString();
        }
        public void LoadVoxelWorld(VoxelWorld wd, string voxelBinFile)
        {
            this.LoadVoxelWorld(wd);
            this.lastVoxelBinFile = new FileInfo(voxelBinFile);
        }
        //         public void LoadVoxelBin(byte[] bin, string voxelBinFile)
        //         {
        //             try
        //             {
        //                 this.LoadVoxelWorld(VoxelWorld.LoadFromBin(bin));
        //                 this.lastVoxelBinFile = new FileInfo(voxelBinFile);
        //             }
        //             catch (Exception err)
        //             {
        //                 err.ShowMessageBox();
        //             }
        //         }
        public VoxelWorld LoadVoxelFile(string voxelBinFile)
        {
            //             var bin = File.ReadAllBytes(voxelBinFile);
            //             var wd = VoxelWorld.LoadFromBin(bin);
            var wd = VoxelWorld.LoadFromFile(voxelBinFile);
            LoadVoxelWorld(wd);
            lastVoxelBinFile = new FileInfo(voxelBinFile);
            return wd;
        }
        public void LoadVoxelXML(string voxelXmlFile)
        {
            try
            {
                if (Voxel3DPlugin.TryLoadTerrainDataDialog(voxelXmlFile, out var data))
                {
                    this.LoadVoxelData(voxelXmlFile, data);
                }
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
        }
        private void SaveVoxel(string path)
        {
            VoxelWorld.SaveToFile(currentWorld, path);
            //             var bin = currentWorld.SaveToBin();
            //             File.WriteAllBytes(path, bin);
            this.canvas.InitVoxelWorld(this.currentWorld);
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
            LoadNewVoxelWorld();
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
                if (this.lastVoxelBinFile != null && this.lastVoxelBinFile.Exists)
                {
                    this.SaveVoxel(this.lastVoxelBinFile.FullName);
                }
                else
                {
                    btn_SaveToBin_Click(sender, e);
                }
            }
        }
        private void btn_LoadFromBin_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.Filter = $"vox|*{VoxelWorld.FILE_EXT}";
            if (fd.ShowDialog(this) == DialogResult.OK)
            {
                //var bin = File.ReadAllBytes(fd.FileName);
                LoadVoxelFile(fd.FileName);
            }
        }
        private void btn_SaveToBin_Click(object sender, EventArgs e)
        {
            if (this.currentWorld != null)
            {
                var fd = new SaveFileDialog();
                fd.DefaultExt = VoxelWorld.FILE_EXT;
                if (fd.ShowDialog(this) == DialogResult.OK)
                {
                    this.SaveVoxel(fd.FileName);
                    this.lastVoxelBinFile = new FileInfo(fd.FileName);
                }
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

        private void btn_ImportMagicaVoxel_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.Filter = "All Files|*.*|MagicaVoxel (vox)|*.vox";
            if (fd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var file = new FileInfo(fd.FileName);
                    var vox = DeepCore.Voxel.Extensions.MagicaVoxel.MagicaVoxelFile.Load(file);
                }
                catch (Exception err)
                {
                    err.ShowMessageBox();
                }
            }
        }

        private void btn_GenRandomTerrain_Click(object sender, EventArgs e)
        {

        }

        #endregion
        //--------------------------------------------------------------------------------------------------------------------------
        #region Terrain
        public class TestFindPathParams : FindPathParams
        {
            [Desc("测试机器人移动速度")]
            public float BotMoveSpeed = 3f;
        }
        private const float PATH_Y_OFFSET = 0.02f;
        private VoxelLayer mouse_PathBegin;
        private VoxelLayer mouse_PathEnd;
        private DeepCore.Geometry.Vector3 mouse_PathBeginPos;
        private DeepCore.Geometry.Vector3 mouse_PathEndPos;
        private IVoxelWayPoint mouse_FindPath;
        private void DrawPathFinder()
        {
            var terrain = this.canvas.VoxelTerrain;
            if (terrain != null)
            {
                if (chk_ShowMousePath.Checked)
                {
                    if (mouse_FindPath != null)
                    {
                        var color1 = Color4.DarkBlue;
                        var color2 = Color4.Magenta;
                        color1.A = color2.A = (float)Math.Abs(Math.Sin(canvas.TotalTimeSEC * 2f));
                        DrawingVoxelObject.DrawWayPoint(
                            color1,
                            color2,
                            mouse_FindPath,
                            canvas.VoxelTerrain.GridCellSize,
                            new Vector3(0, 0, PATH_Y_OFFSET));
                    }
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
                mouse_PathBeginPos = raycastLayerTouch.WorldToObject().ToGeometry();
                mouse_PathEnd = null;
                mouse_FindPath = null;
            }
            else if (mouse_PathEnd == null)
            {
                mouse_PathEnd = raycastLayer;
                mouse_PathEndPos = raycastLayerTouch.WorldToObject().ToGeometry();
            }
            if (mouse_PathBegin != null && mouse_PathEnd != null)
            {
                var param = g2DPropertyGrid1.GetSelectedValue() as TestFindPathParams;
                var watch = Stopwatch.StartNew();
                canvas.World3D.PathFinder.FindPathStepLimit = param.StepLimit;
                mouse_FindPath = canvas.World3D.PathFinder.FindPathByPos(mouse_PathBeginPos, mouse_PathEndPos);
                watch.Stop();
                drawText.Text = $"FindPath={watch.ElapsedMilliseconds}ms";
                if (mouse_FindPath != null)
                {
                    var bot = new TestVoxelPathBotAutoDispose(mouse_FindPath);
                    bot.Speed = param.BotMoveSpeed;
                    bot.VObject.Transport(mouse_PathBeginPos);
                    AddDisplayObject(bot);
                }
            }
        }

        private Vector3 raycastLayerTouch;
        private VoxelLayer raycastLayer;
        private GLTextTexture2D drawText;

        private void Canvas_OnUpdate(Common.G3D.GLView sender, TimeSpan interval)
        {
            var mouse = sender.GLControl.PointToClient(Control.MousePosition);
            this.raycastLayer = canvas.RayCastVoxelFromMouse(mouse, out raycastLayerTouch);
        }
        private void Canvas_OnEndRender(Common.G3D.GLView sender, Common.G3D.PaintEventArgs3D e)
        {
            if (raycastLayer != null)
            {
                var terrain = this.canvas.VoxelTerrain;
                if (terrain != null)
                {
                    var opos = raycastLayerTouch.WorldToObject();
                    opos.Z += PATH_Y_OFFSET;
                    DrawingVoxelObject.DrawCycle(Color4.Yellow, opos, terrain.GridCellRadius);
                    var color = Color4.White;
                    color.A = (float)Math.Abs(Math.Sin(canvas.TotalTimeSEC * 2f));
                    DrawingVoxelObject.FillRectW(
                        color,
                        raycastLayer.X * terrain.GridCellSize,
                        raycastLayer.Y * terrain.GridCellSize,
                        terrain.GridCellSize,
                        terrain.GridCellSize,
                        raycastLayer.Upward + PATH_Y_OFFSET);
                }
            }
            {
                DrawPathFinder();
            }
            // drawSpacePath();
        }
        private void Canvas_OnRenderHUD(GLView sender, PaintEventArgs3D e)
        {
            if (drawText == null)
            {
                drawText = new GLTextTexture2D(System.Drawing.FontStyle.Regular, 12, Color4.White);
            }
            drawText.DrawQuards2D(e, 0, 0);
        }
        private void GlControl1_MouseDown(object sender, MouseEventArgs e)
        {
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    ClickPathFinder();
                }
            }
        }
        private void GlControl1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        protected virtual void timer2_Tick(object sender, EventArgs e)
        {
            this.txt_Objects.Text = $"Objects={this.canvas?.ObjectsCount}";
        }
        private void Chk_MoveBrush_Click(object sender, EventArgs e)
        {

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


        //         SpaceAstarGenerator.SpaceInfo[] spaces;
        // 
        //         private void saveSpacePathFinderToolStripMenuItem_Click(object sender, EventArgs e)
        //         {
        //             spaces = GamePlugin3D.GenSpaceAstarDialog(this, this.currentWorld.Terrain);
        //         }
        // 
        //         private void drawSpacePath()
        //         {
        //             if (spaces != null)
        //             {
        //                 var gs = currentWorld.Terrain.GridCellSize;
        //                 foreach (var space in spaces)
        //                 {
        //                     var anchor = space.Root.UpwardCenterPos;
        //                     var rect = space.Range;
        //                     var cw = rect.Width * gs;
        //                     var ch = rect.Height * gs;
        //                     var cx = rect.X * gs + cw / 2f;
        //                     var cy = rect.Y * gs + ch / 2f;
        //                     //DrawingObject.DrawArc(Color4.Pink, new Vector3(cx, cy, space.Root.UpwardCenterPos.Z + 0.1f), cw, ch, 0, 360);
        //                     DrawingObject.DrawRectW(Color4.Cyan, rect.X * gs, rect.Y * gs, rect.Width * gs, rect.Height * gs, anchor.Z + 0.1f);
        //                 }
        //             }
        //         }

        #endregion
        //-----------------------------------------------------------------------------------------------------------
        #region TestAddActor

        public void AddDisplayObject<T>(DisplayVoxelObject3D<T> actor) where T : VoxelObject
        {
            this.canvas.AddDisplayVoxel(actor);
        }
        public void AddActor<T>(DisplayVoxelActor3D<T> actor) where T : VoxelObject
        {
            Canvas.SetCameraControl(new LockActorCamera2D(actor));
            Canvas.SetCameraControl(new LockActorCamera3D(actor));
            this.AddDisplayObject(actor);
        }


        public virtual TestVoxelBot AddRandomTestBot()
        {
            var actor = TestVoxelBot.CreateRandomBot();
            this.AddDisplayObject(actor);
            return actor;
        }
        public virtual DisplayVoxelActor3D<VoxelObject> AddMainActor()
        {
            var actor = new DisplayVoxelActor3D<VoxelObject>(new VoxelObject());
            var campos = Canvas.Camera.CamPosition;
            actor.VObject.Transport(new DeepCore.Geometry.Vector3(campos.X, campos.Z, 0));
            AddActor(actor);
            return actor;
        }

        private void btn_AddActor_Click(object sender, EventArgs e)
        {
            AddMainActor();
        }

        private void btn_AddTestActor_Click(object sender, EventArgs e)
        {
            if (currentWorld != null) { AddRandomTestBot(); }
        }
        private void btn_AddTestActor10_Click(object sender, EventArgs e)
        {
            if (currentWorld != null)
            {
                for (int i = 0; i < 10; i++) { AddRandomTestBot(); }
            }
        }
        private void btn_AddTestActor100_Click(object sender, EventArgs e)
        {
            if (currentWorld != null)
            {
                for (int i = 0; i < 100; i++) { AddRandomTestBot(); }
            }
        }

        #endregion
        //-----------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------
        public abstract class TestVoxelBot : DisplayVoxelObject3D<VoxelObject>
        {
            public readonly static Random random = new Random();
            public float Speed { get; set; } = 1.0f;
            public TestVoxelBot() : base(new VoxelObject())
            {
                this.VObject.Height = 1.8f + 0.5f * random.NextFloat();
                this.Size = 0.2f + 0.3f * random.NextFloat();
                this.Speed = 1f + random.NextFloat() * 10f;
            }
            protected override void OnRender(PaintEventArgs3D e)
            {
                base.OnRender(e);
            }

            public static TestVoxelBot CreateRandomBot()
            {
                var type = random.GetRandomInArray(new Type[] {
                typeof(TestVoxelWalkBot),
                typeof(TestVoxelJumpBot),
                typeof(TestVoxelPathBot),
            });
                return ReflectionUtil.CreateInterface<TestVoxelBot>(type);
                // return new TestVoxelJumpBot();
            }
        }
        //----------------------------------------------------------------------------------------------------------
        public class TestVoxelWalkBot : TestVoxelBot
        {
            private float direction = 0;
            public override Color4 Color => Color4.Red;
            public TestVoxelWalkBot()
            {
                this.direction = CMath.RADIANS_360 * random.NextFloat();
            }
            protected override void OnAdded()
            {
                base.OnAdded();
                var layer = VObject.Terrain.GetRandomLayer(random);
                this.VObject.Transport(layer);
            }
            protected override void OnUpdate()
            {
                base.OnUpdate();
                float distance = MotionHelper.GetDistance(View.LastIntervalMS, Speed);
                float dx = (float)(Math.Cos(direction) * distance);
                float dy = (float)(Math.Sin(direction) * distance);
                if (VObject.TryMoveOffset(new DeepCore.Geometry.Vector2(dx, dy), true) == AgentMoveResult.Blocked)
                {
                    this.direction = CMath.RADIANS_360 * random.NextFloat();
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------
        public class TestVoxelJumpBot : TestVoxelBot
        {
            protected float direction = 0;
            public override Color4 Color => Color4.Green;
            public TestVoxelJumpBot()
            {
                this.direction = CMath.RADIANS_360 * random.NextFloat();
            }
            protected override void OnAdded()
            {
                base.OnAdded();
                var layer = VObject.Terrain.GetRandomLayer(random);
                this.VObject.Transport(layer);
            }
            protected override void OnUpdate()
            {
                base.OnUpdate();
                float distance = MotionHelper.GetDistance(View.LastIntervalMS, Speed);
                float dx = (float)(Math.Cos(direction) * distance);
                float dy = (float)(Math.Sin(direction) * distance);
                if (VObject.TryMoveOffset(new DeepCore.Geometry.Vector2(dx, dy), false) != AgentMoveResult.MoveSmooth)
                {
                    if (!this.VObject.IsInTheAir)
                    {
                        var pct = random.Next() % 100;
                        if (pct >= 0 && pct < 10)
                        {
                            VObject.Jump(10f + 10f * random.NextFloat());
                        }
                        else
                        {
                            this.direction = CMath.RADIANS_360 * random.NextFloat();
                        }
                    }
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------
        public class TestVoxelPathBot : TestVoxelBot
        {
            public override Color4 Color => Color4.Yellow;
            private IVoxelWayPoint path;
            public TestVoxelPathBot()
            {
            }
            protected override void OnAdded()
            {
                base.OnAdded();
                var layer = VObject.Terrain.GetRandomLayer(random);
                this.VObject.Transport(layer);
            }
            protected override void OnUpdate()
            {
                base.OnUpdate();
                if (path != null)
                {
                    float step = MotionHelper.GetDistance(View.LastIntervalMS, Speed);
                    VObject.TryMoveToPath(ref path, step, false);
                }
                else if (View.VoxelTerrain != null)
                {
                    int sx = random.Next() % View.VoxelTerrain.XCount;
                    int sy = random.Next() % View.VoxelTerrain.YCount;
                    if (View.VoxelTerrain.TryGetVoxelCell(sx, sy, out var cell))
                    {
                        var la = random.Next() % cell.LayerCount;
                        var tgt = cell.GetLayer(la);
                        if (VObject.CurrentLayer != null)
                        {
                            path = VObject.World.PathFinder.FindPathByLayer(VObject.CurrentLayer, tgt);
                            if (path == null)
                            {
                                VObject.Transport(tgt);
                            }
                        }
                        else
                        {
                            VObject.Transport(tgt);
                        }
                    }
                }
            }
            protected override void OnRender(PaintEventArgs3D e)
            {
                base.OnRender(e);
                if (path != null && path.Next != null)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Color4(this.Color);
                    foreach (var p in path)
                    {
                        var pos = p.Position;
                        GL.Vertex3(pos.X, pos.Z, pos.Y);
                    }
                    GL.End();
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------
        public class TestVoxelPathBotAutoDispose : TestVoxelBot
        {
            public override Color4 Color => Color4.Purple;
            private IVoxelWayPoint path;
            public TestVoxelPathBotAutoDispose(IVoxelWayPoint path)
            {
                this.path = path;
                this.Speed = 5f + random.NextFloat() * 5f;
            }
            protected override void OnAdded()
            {
                base.OnAdded();
            }
            protected override void OnUpdate()
            {
                base.OnUpdate();
                if (path != null)
                {
                    float step = MotionHelper.GetDistance(View.LastIntervalMS, Speed);
                    VObject.TryMoveToPath(ref path, step, false);
                }
                else
                {
                    this.RemoveFromParent();
                }
            }
            protected override void OnRender(PaintEventArgs3D e)
            {
                base.OnRender(e);
                if (path != null && path.Next != null)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    GL.Color4(this.Color);
                    foreach (var p in path)
                    {
                        var pos = p.Position;
                        GL.Vertex3(pos.X, pos.Z, pos.Y);
                    }
                    GL.End();
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------}
    }
}