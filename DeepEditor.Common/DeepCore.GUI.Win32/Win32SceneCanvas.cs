using DeepCore.GUI.SceneGraph;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DeepCore.GUI.Win32
{
    public partial class Win32SceneCanvas : UserControl
    {
        public Win32DisplayRoot RootNode { get =>canvas.RootNode; }
        public Win32PictureBox RootCanvas { get => canvas; }
        public StatusStrip StatusBar { get => statusStrip1; }
        public ToolStripStatusLabel Info { get => txtStatusInfo; }
        public Timer Timer { get => timer; }
        [Browsable(false)]
        public int FPS { get { return timer.Interval / 1000; } }
        public Win32SceneCanvas()
        {
            InitializeComponent();
            this.RootCanvas.MouseHover += RootCanvas_MouseHover;
            this.RootCanvas.MouseDown += RootCanvas_MouseDown;
            this.RootCanvas.MouseMove += RootCanvas_MouseMove;
            this.RootCanvas.MouseUp += RootCanvas_MouseUp;
        }
        protected override void OnLoad(EventArgs e)
        {
            this.timer.Tick += TimerTick;
            base.OnLoad(e);
        }
        protected virtual void TimerTick(object sender, EventArgs e)
        {
            canvas.Invalidate();
        }
        public void SetFPS(int value)
        {
            timer.Interval = value / 1000;
        }
        //-----------------------------------------------------------------------------------------------------
        private void RootCanvas_MouseHover(object sender, EventArgs e)
        {
            RefreshStatusText();
        }
        private void RootCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            RefreshStatusText();
        }
        private void RootCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            RefreshStatusText();
        }
        private void RootCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            RefreshStatusText();
        }
        public void RefreshStatusText()
        {
            var pos = RootNode.RootMousePoint;
            this.txtStatusMouse.Text = $"Mouse:({(int)pos.X},{(int)pos.Y})";
        }

        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------



    }
}
