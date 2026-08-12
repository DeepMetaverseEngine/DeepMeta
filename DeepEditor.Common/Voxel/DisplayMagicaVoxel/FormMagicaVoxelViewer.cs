using DeepCore.Voxel.Extensions.MagicaVoxel;
using DeepEditor.Common.Voxel.DisplayMagicaVoxel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.DisplayMagicaVoxel
{
    public partial class FormMagicaVoxelViewer : DeepEditor.Common.G2D.G2DBaseForm
    {
        public MagicaVoxelViewer Viewer { get => magicaVoxelViewer1; }
        public FormMagicaVoxelViewer()
        {
            InitializeComponent();
        }
        public void InitVoxel(MagicaVoxelFile vox)
        {
            this.Viewer.Canvas.InitVOX(vox);
        }
    }
}
