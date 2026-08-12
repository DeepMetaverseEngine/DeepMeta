using DeepEditor.Common.Controls;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.DisplayTerrainData
{
    public partial class TerrainDataViewer : UserControl
    {
        public TerrainDataCanvas Canvas { get; private set; }
        public TerrainDataViewer()
        {
            InitializeComponent();
            Canvas = new TerrainDataCanvas(this.glControl1, this.timer1);
            new DropDownFieldMaskGenerator(Canvas, menu_View, "show");
        }

        private void menu_View_Click(object sender, System.EventArgs e)
        {

        }
    }
}
