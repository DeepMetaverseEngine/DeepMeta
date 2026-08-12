using DeepCore.MinaClient;
using DeepCore.Net;
using DeepEditor.Common.Drawing;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Common.Net
{
    public partial class FormNetSession : DeepEditor.Common.G2D.G2DBaseForm
    {
        private INetSession mSession;

        private BytesSecondRateTracker mTrackerRecv;
        private BytesSecondRateTracker mTrackerSend;

        private StringBuilder textBuffer = new StringBuilder();
        
        public FormNetSession(INetSession session)
        {
            InitializeComponent();

            this.mSession = session;
            if (mSession is IMinaClientSession netsession)
            {
                netsession.OnMessageReceived += this.onMessageReceived;
                netsession.OnMessageSent += this.onMessageSent;
            }
            this.mTrackerRecv = new BytesSecondRateTracker(600, this.Font, new Pen(Color.Blue), new SolidBrush(Color.Blue));
            this.mTrackerRecv.Title = "Recv";
            this.mTrackerSend = new BytesSecondRateTracker(600, this.Font, new Pen(Color.Red), new SolidBrush(Color.Red));
            this.mTrackerSend.Title = "Send";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mTrackerRecv.Record(mSession.TotalRecvBytes);
            mTrackerSend.Record(mSession.TotalSentBytes);
            pictureBox1.Refresh();

            lock (textBuffer)
            {
                if (textBuffer.Length > 0)
                {
                    richTextBox1.AppendText(textBuffer.ToString());
                    textBuffer.Clear();
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                float px = 0;
                float py = 0;
                float pw = pictureBox1.Width;
                float ph = pictureBox1.Height / 2;
                mTrackerRecv.DrawGrap(e.Graphics,  px + 1, py + 1, pw - 2, ph - 2);
                py += ph;
                mTrackerSend.DrawGrap(e.Graphics, px + 1, py + 1, pw - 2, ph - 2);
            }
            catch { }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void onMessageReceived(IMinaClientSession session, object msg)
        {
            lock(textBuffer)
            {
                textBuffer.AppendLine(DateTime.Now + " : Recv <- " + msg);
            }
        }
        private void onMessageSent(IMinaClientSession session, object msg)
        {
            lock (textBuffer)
            {
                textBuffer.AppendLine(DateTime.Now + " : Send -> " + msg);
            }
        }

    }
}
