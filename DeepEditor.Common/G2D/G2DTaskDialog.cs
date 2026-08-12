using MaterialSkin.Controls; using DeepEditor.Common.G2D;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public delegate Task<T> TransactionAsync<T>(G2DTaskDialog sender);
    public delegate Task TransactionAsync(G2DTaskDialog sender);

    public partial class G2DTaskDialog : G2DBaseForm
    {
        public readonly RichTextBoxLogger log;
        private TransactionAsync action;
        private CancellationTokenSource cancellationToken;
        private DialogResult result = DialogResult.Cancel;
        public G2DTaskDialog(string title, TransactionAsync action)
        {
            InitializeComponent();
            this.Text = title;
            this.log = new RichTextBoxLogger(richTextBox1, "");
            this.action = action;
            this.cancellationToken = new CancellationTokenSource();
            this.btn_cancel.Visible = false;
            this.btn_close.Visible = false;
            this.Load += FormSendTransaction_Load;
        }
        public CancellationToken GetCancellationToken()
        {
            this.btn_cancel.Visible = true;
            return cancellationToken.Token;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.DialogResult = result;
            base.OnFormClosing(e);
        }
        private void FormSendTransaction_Load(object sender, EventArgs e)
        {
            this.result = DialogResult.Cancel;
            this.btn_close.Visible = false;
            Task.Run(async () =>
            {
                try
                {
                    await action(this);
                    Invoke(() => { this.result = DialogResult.OK; });
                }
                catch (Exception ex)
                {
                    this.log.Error(ex);
                    Invoke(() => { this.result = DialogResult.Abort; });
                }
                finally
                {
                    Invoke(() => { this.btn_close.Visible = true; });
                }
            });
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.cancellationToken.Cancel();
        }
        //----------------------------------------------------------------------------------------------------------------------------------------
        public static DialogResult ShowDialog(string title, TransactionAsync action)
        {
            var form = new G2DTaskDialog(title, action);
            return form.ShowDialog((IWin32Window)null);
        }
        public static DialogResult ShowDialog(string title, IWin32Window win, TransactionAsync action)
        {
            var form = new G2DTaskDialog(title, action);
            return form.ShowDialog(win);
        }
        public static T ShowDialog<T>(string title, IWin32Window win, TransactionAsync<T> action)
        {
            T hash = default(T);
            var form = new G2DTaskDialog(title, async (sender) =>
            {
                hash = await action(sender);
            });
            if (form.ShowDialog(win) == DialogResult.OK)
            {
                return hash;
            }
            return hash;
        }
        public static T ShowDialog<T>(string title, TransactionAsync<T> action)
        {
            return ShowDialog<T>(title, null, action);
        }
        //----------------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------
    }
}
