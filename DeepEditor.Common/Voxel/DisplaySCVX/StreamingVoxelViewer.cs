using DeepEditor.Common.Controls;
using System;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.DisplaySCVX
{
    public partial class StreamingVoxelViewer : UserControl
    {
        public StreamingVoxelCanvas Canvas { get; private set; }
        public StreamingVoxelViewer()
        {
            InitializeComponent();
            this.btn_AddPlayer.Click += Btn_AddPlayer_Click;
            this.btn_SetStaticObject.Click += Btn_SetStaticObject_Click;
            Canvas = new StreamingVoxelCanvas(this.glControl1, this.timer1);
            new DropDownFieldMaskGenerator(Canvas, menu_View, "show");
        }
        private void Btn_SetStaticObject_Click(object sender, EventArgs e)
        {
        }
        private void Btn_AddPlayer_Click(object sender, EventArgs e)
        {
        }
        private void chk_Pause_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void chk_Camera2D_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Camera2D.Checked)
            {
                this.Canvas.SetCameraControlWithType(G3D.CameraType.Camera2D);
            }
            else
            {
                this.Canvas.SetCameraControlWithType(G3D.CameraType.Camera3D);
            }
        }

    }
}
