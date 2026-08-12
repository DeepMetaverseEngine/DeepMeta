using DeepCore.Game3D.Slave.Layer;
using DeepEditor.Common.G2D;
using System.IO;
using System.Windows.Forms;
using static DeepEditor.Plugin3D.BattleServer.Host.FormServer;

namespace DeepEditor.Plugin3D.BattleClient
{
    public partial class FormBattleView3D : G2DForm
    {
        public PanelBattleView3D BattlePanel
        {
            get => panel;
        }
        public FormBattleView3D()
        {
            InitializeComponent();
            this.Disposed += FormBattleView3D_Disposed;
        }
        private void FormBattleView3D_Disposed(object sender, EventArgs e)
        {
            panel = null;
        }
        public bool Init(PanelBattleView3D.BattleConfig cfg)
        {
            if (this.panel.Init(cfg))
            {

                panel.BattleView.Layer.GameOver += Layer_GameOver; 
                this.Text = $"{panel.Title}";
                return true;
            }
            return false;
        }

        private void Layer_GameOver(LayerZone layer, int winForce, string msg)
        {
            MessageBox.Show(msg, "Winner is " + winForce);
            panel.BattleView.Pause = true;
        }
      

    }
}
