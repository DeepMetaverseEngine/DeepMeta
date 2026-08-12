using DeepCore.Voxel.StreamingVoxel.Data;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.DisplaySCVX
{
    public partial class FormStreamingVoxelViewer : DeepEditor.Common.G2D.G2DBaseForm
    {
        public StreamingVoxelViewer Viewer { get => voxelViewer1; }
        public FormStreamingVoxelViewer()
        {
            InitializeComponent();
        }
        public void InitVoxelAdapter(StreamingChunk adapter)
        {
            this.Viewer.Canvas.InitWorld(adapter);
        }
    }
}
