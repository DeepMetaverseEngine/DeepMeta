using DeepCore;
using DeepEditor.Common.G2D;
using DeepMetaGame.ZoneServer.Server;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DeepEditor.Plugin3D.BattleServer.Host
{
    public partial class FormMsgBytes : G2DBaseForm
    {
        private ServerCodec codec;

        public FormMsgBytes(ServerCodec codec)
        {
            InitializeComponent();

            this.codec = codec;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshRecords();
        }

        public void RefreshRecords()
        {
            this.listView1.Items.Clear();
            foreach (var ve in codec.GetSentTypeBytes())
            {
                var item = new ListViewItem(ve.Key.Name);
                item.SubItems.Add(CUtils.ToBytesSizeString(ve.Value.Bytes));
                item.SubItems.Add(ve.Value.Count.ToString());
                this.listView1.Items.Add(item);
            }
        }
    }
}
