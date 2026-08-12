using DeepEditor.Common.Controls;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.DisplayMagicaVoxel
{
    public partial class MagicaVoxelViewer : UserControl
    {
        public MagicaVoxelCanvas Canvas { get; private set; }
        public MagicaVoxelViewer()
        {
            InitializeComponent();
            Canvas = new MagicaVoxelCanvas(this.glControl1, this.timer1);
            new DropDownFieldMaskGenerator(Canvas, menu_View, "show");
        }

    }
}
