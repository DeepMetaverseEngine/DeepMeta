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
    public partial class PanelVoxelWorldViewer : UserControl
    {
        private static Random random = new Random();
        private DisplayVoxelWorld3D canvas;
        public DisplayVoxelWorld3D Canvas => canvas;
        public GLControl GLControl { get => glControl1; }
        public PanelVoxelWorldViewer()
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

        private Vector3 raycastLayerTouch;
        private VoxelLayer raycastLayer;
        private GLTextTexture2D drawText;
        private const float PATH_Y_OFFSET = 0.02f;

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

        #endregion
        //-----------------------------------------------------------------------------------------------------------

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

        //-----------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------}
    }
}