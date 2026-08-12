using DeepCore;
using DeepCore.IO;
using DeepCore.Voxel;
using DeepCore.Voxel.Data;
using DeepCore.Voxel.Extensions.MagicaVoxel;
using DeepEditor.Common.Controls;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel.Display3D;
using DeepTools.Voxel;
using OpenTK.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.Display3D
{
    public partial class PanelVoxelViewer : UserControl
    {
        private FileInfo lastLoad;
        private FileInfo lastSave;
        private DisplayVoxelWorld3D canvas;
        public DisplayVoxelWorld3D Canvas { get => canvas; }
        public VoxelWorld World { get => canvas.World3D; }
        public GLControl GLControl { get => glControl1; }
        public PanelVoxelViewer()
        {
            InitializeComponent();
            this.Load += VoxelViewer_Load;
        }
        private void VoxelViewer_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                this.canvas = new DisplayVoxelWorld3D(this.glControl1, this.timer1);
                this.canvas.ShowFlagMesh3D = false;
                this.canvas.ShowMeshObject = false;
                this.canvas.ShowObjects = false;
                this.canvas.ShowPathFinder = false;
                this.canvas.ShowPathMesh3D = false;
                this.canvas.ShowTerrain3D = true;
                this.canvas.ShowTerrain3DLines = false;
                new DropDownFieldMaskGenerator(canvas, menu_View, "show");
            }
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            if (Voxel3DPlugin.TryLoadNewVoxelWorldDialog(this, out var voxelXmlFile, out var voxelBinFile, out var world))
            {
                LoadVoxelWorld(world);
                this.Text = $"{voxelXmlFile}";
                this.lastLoad = new FileInfo(voxelXmlFile);
                this.lastSave = new FileInfo(voxelBinFile);
            }
        }

        private void btn_TakeSnap_Click(object sender, EventArgs e)
        {
            var fd = new SaveFileDialog();
            fd.InitialDirectory = Environment.CurrentDirectory;
            fd.DefaultExt = ".png";
            fd.FileName = CUtils.FormatTime(DateTime.Now);
            if (fd.ShowDialog(this) == DialogResult.OK)
            {
                var bitmap = this.canvas.TakeSnap();
                bitmap.Save(fd.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void btn_SaveWorld_Click(object sender, EventArgs e)
        {
            if (this.canvas.World3D != null)
            {
                var fd = new SaveFileDialog();
                fd.InitialDirectory = Environment.CurrentDirectory;
                if (lastLoad != null && lastLoad.Exists)
                {
                    fd.InitialDirectory = lastLoad.Directory.FullName;
                    fd.FileName = lastLoad.Name + VoxelWorld.FILE_EXT;
                }
                if (lastSave != null && lastSave.Exists)
                {
                    fd.InitialDirectory = lastSave.Directory.FullName;
                    fd.FileName = lastSave.Name;
                }
                if (fd.ShowDialog(this) == DialogResult.OK)
                {
                    VoxelWorld.SaveToFile(this.canvas.World3D, fd.FileName);
                }
            }
        }
        public void LoadVoxelWorld(VoxelWorld wd, string file = null)
        {
            this.canvas.InitVoxelWorld(wd);
            this.canvas.ResetCameraPos();
            if (file != null)
            {
                this.Text = $"{file}";
                this.lastLoad = new FileInfo(file);
                this.lastSave = null;
            }
        }
        public bool TryLoadVoxelWorld(string file)
        {
            try
            {
                using (var fs = Resource.OpenStream(file))
                {
                    var input = new InputStream(fs);
                    if (VoxelLoader.TryPickVoxelAsVoxelWorld(input, out var wd))
                    {
                        this.LoadVoxelWorld(wd, file);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.PrintStackTrace();
            }
            return false;
        }

    }
}
