using DeepCore.IO;
using DeepCore.Voxel.Data;
using DeepCore.Voxel.Extensions.MagicaVoxel;
using DeepEditor.Common.Voxel.Display3D;
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
    public partial class FormVoxelViewer : DeepEditor.Common.G2D.G2DBaseForm
    {
        public PanelVoxelViewer Viewer { get => voxelViewer1; }
        public FormVoxelViewer()
        {
            InitializeComponent();
        }
        public void LoadVoxelWorld(VoxelWorld wd)
        {
            this.voxelViewer1.LoadVoxelWorld(wd);
        }
        public bool TryLoadVoxelWorld(string file)
        {
            if (this.voxelViewer1.TryLoadVoxelWorld(file))
            {
                return true;
            }
            return false;
        }
    }
}
