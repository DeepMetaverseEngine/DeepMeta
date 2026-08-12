using DeepCore.Concurrent;
using DeepEditor.Common.G2D;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace DeepEditor.Common.G2D
{
    public partial class G2DProgressDialog : G2DBaseForm
    {
        private IProgress Progress;
        private Thread mThread;
        private bool mIsClose = false;
        private bool mIsFinish = false;
        private AtomicReference<Exception> mRunError = new AtomicReference<Exception>(null);
        private DialogResult result = DialogResult.OK;

        public G2DProgressDialog(string title, Action<IRangeValue> threadRun)
            : this(new DefaultProgress(title, (r) => { threadRun(r); return null; }, false)) { }
        public G2DProgressDialog(string title, Func<IRangeValue, object> threadRun)
            : this(new DefaultProgress(title, threadRun, false)) { }
        public G2DProgressDialog(string title, bool canbreak, Action<IRangeValue> threadRun)
            : this(new DefaultProgress(title, (r) => { threadRun(r); return null; }, canbreak)) { }
        public G2DProgressDialog(string title, bool canbreak, Func<IRangeValue, object> threadRun)
            : this(new DefaultProgress(title, threadRun, canbreak)) { }

        public G2DProgressDialog(string title, Func<IRangeValue, Task> threadRun)
            : this(new DefaultProgress(title, (r) => { threadRun(r).Wait(); return null; }, false)) { }
        public G2DProgressDialog(string title, Func<IRangeValue, Task<object>> threadRun)
            : this(new DefaultProgress(title, (r) => { return threadRun(r).WaitForResult(); }, false)) { }
        public G2DProgressDialog(string title, bool canbreak, Func<IRangeValue, Task> threadRun)
            : this(new DefaultProgress(title, (r) => { threadRun(r).Wait(); return null; }, canbreak)) { }
        public G2DProgressDialog(string title, bool canbreak, Func<IRangeValue, Task<object>> threadRun)
            : this(new DefaultProgress(title, (r) => { return threadRun(r).WaitForResult(); }, canbreak)) { }

        public G2DProgressDialog(IProgress progress) : this() { Init(progress); }
        public G2DProgressDialog()
        {
            InitializeComponent();
            this.Load += G2DProgressDialog_Load;
        }
        public void Init(string title, Action<IRangeValue> threadRun)
        {
            this.Init(new DefaultProgress(title, (r) => { threadRun(r); return null; }, false));
        }
        public void Init(string title, Func<IRangeValue, object> threadRun)
        {
            this.Init(new DefaultProgress(title, threadRun, false));
        }
        public void Init(string title, bool canbreak, Action<IRangeValue> threadRun)
        {
            this.Init(new DefaultProgress(title, (r) => { threadRun(r); return null; }, canbreak));
        }
        public void Init(string title, bool canbreak, Func<IRangeValue, object> threadRun)
        {
            this.Init(new DefaultProgress(title, threadRun, canbreak));
        }
        public void Init(IProgress progress)
        {
            this.Progress = progress;
        }
        private void G2DProgressDialog_Load(object sender, EventArgs e)
        {
            this.Text = Progress.Title;
            this.lbl_Title.Text = Progress.Title;
            this.textBox1.Text = Progress.Text;
            this.progressBar1.Maximum = (int)Progress.Maximum;
            this.progressBar1.Minimum = (int)Progress.Minimum;
            this.progressBar1.Value = (int)Progress.Value;
            this.btnClose.Visible = Progress.CanBreakOnClose;
            Progress.main_Begin();
            timer1.Start();
            mThread = new Thread(new ThreadStart(run));
            mThread.IsBackground = true;
            mThread.Start();
        }
        public void SetSplash(Image image)
        {
            this.pictureBox1.Image = image;
            this.Height += image.Height;
        }
        private void run()
        {
            try
            {
                Progress.thread_Run(this);
            }
            catch (Exception err)
            {
                mRunError.Value = err;
            }
            finally
            {
                mIsFinish = true;
            }
        }
        internal void RefreshBar()
        {
            long value = Progress.Value;
            value = Math.Min(value, Progress.Maximum);
            value = Math.Max(value, Progress.Minimum);
            this.progressBar1.Maximum = (int)Progress.Maximum;
            this.progressBar1.Minimum = (int)Progress.Minimum;
            this.progressBar1.Value = (int)value;
            this.textBox1.Text = Progress.Text;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.mIsClose)
            {
                timer1.Stop();
                try
                {
                    this.Progress.main_Done();
                }
                catch (Exception err)
                {
                    err.ShowMessageBox(this);
                }
                this.Close();
                return;
            }
            if (mRunError.CompareAndSet(o => o != null, out var error, null))
            {
                this.mIsClose = true;
                this.result = DialogResult.No;
                error.ShowMessageBox(this);
            }
            if (Progress.IsDone || mIsFinish)
            {
                this.Invalidate();
                this.mIsClose = true;
                this.progressBar1.Invalidate();
            }
            {
                long value = Progress.Value;
                value = Math.Min(value, Progress.Maximum);
                value = Math.Max(value, Progress.Minimum);
                this.progressBar1.Maximum = (int)Progress.Maximum;
                this.progressBar1.Minimum = (int)Progress.Minimum;
                this.progressBar1.Value = (int)value;
                this.textBox1.Text = Progress.Text;
            }
        }
        private void G2DProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            mThread.Join();
            this.DialogResult = result;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Progress.main_CloseClick();
            //this.Close();
        }
        public DialogResult ShowDialog(IWin32Window owner, out object product)
        {
            var ret = base.ShowDialog(owner);
            if (Progress.Product != null)
            {
                product = Progress.Product;
            }
            else
            {
                product = null;
            }
            return ret;
        }
        public DialogResult ShowDialog(out object product)
        {
            var ret = base.ShowDialog();
            if (Progress.Product != null)
            {
                product = Progress.Product;
            }
            else
            {
                product = null;
            }
            return ret;
        }
        public object ShowDialogWith(IWin32Window owner)
        {
            var result = this.ShowDialog(owner, out var ret);
            if (result == DialogResult.OK)
            {
                return ret;
            }
            else
            {
                return null;
            }
        }
        public object ShowDialogWith()
        {
            var result = this.ShowDialog(out var ret);
            if (result == DialogResult.OK)
            {
                return ret;
            }
            else
            {
                return null;
            }
        }


        #region static

        public static T ShowDialogAs<T>(string title, Func<IRangeValue, T> threadRun, bool canbreak = false)
            => ShowDialogAs<T>(null, title, threadRun, canbreak);
        public static DialogResult TryShowDialogAs<T>(string title, Func<IRangeValue, T> threadRun, out T product, bool canbreak = false)
            => TryShowDialogAs<T>(null, title, threadRun, out product, canbreak);

        public static DialogResult TryShowDialogAs<T>(IWin32Window owner, string title, Func<IRangeValue, T> threadRun, out T product, bool canbreak = false)
        {
            var dialog = new G2DProgressDialog<T>(title, canbreak, threadRun);
            var result = dialog.ShowDialog(owner, out var ret);
            if (result == DialogResult.OK)
            {
                product = (T)ret;
            }
            else
            {
                product = default(T);
            }
            return result;
        }
        public static T ShowDialogAs<T>(IWin32Window owner, string title, Func<IRangeValue, T> threadRun, bool canbreak = false)
        {
            var dialog = new G2DProgressDialog<T>(title, canbreak, threadRun);
            var result = dialog.ShowDialog(owner, out var ret);
            if (result == DialogResult.OK)
            {
                return (T)ret;
            }
            else
            {
                return default(T);
            }
        }



        #endregion

    }

    //--------------------------------------------------------------------------------------------------------

    public partial class G2DProgressDialog<T> : G2DProgressDialog
    {
        public G2DProgressDialog(string title, Func<IRangeValue, T> threadRun)
            : base(new DefaultProgress(title, (p) => threadRun(p), false)) { }
        public G2DProgressDialog(string title, bool canbreak, Func<IRangeValue, T> threadRun)
            : base(new DefaultProgress(title, (p) => threadRun(p), canbreak)) { }

        public G2DProgressDialog(IProgress progress) : base(progress)
        {

        }
        public DialogResult ShowDialog(IWin32Window owner, out T product)
        {
            var result = base.ShowDialog(owner, out var ret);
            if (result == DialogResult.OK)
            {
                product = (T)ret;
            }
            else
            {
                product = default(T);
            }
            return result;
        }
        public DialogResult ShowDialog(out T product)
        {
            var result = base.ShowDialog(out var ret);
            if (result == DialogResult.OK)
            {
                product = (T)ret;
            }
            else
            {
                product = default(T);
            }
            return result;
        }
        new public T ShowDialogWith(IWin32Window owner)
        {
            var result = base.ShowDialog(owner, out var ret);
            if (result == DialogResult.OK)
            {
                return (T)ret;
            }
            else
            {
                return default(T);
            }
        }
        new public T ShowDialogWith()
        {
            var result = base.ShowDialog(out var ret);
            if (result == DialogResult.OK)
            {
                return (T)ret;
            }
            else
            {
                return default(T);
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------

    public abstract class IProgress
    {
        public virtual string Title { get; protected set; }
        public virtual string Text { get; protected set; }
        public virtual long Maximum { get; protected set; }
        public virtual long Minimum { get; protected set; }
        public virtual long Value { get; protected set; }
        public virtual object Product { get; protected set; }
        public virtual bool CanBreakOnClose { get; protected set; } = false;
        public bool IsDone { get; private set; }
        public G2DProgressDialog Dialog;
        public IProgress()
        {
            this.Title = "";
            this.Text = "";
            this.IsDone = false;
            this.Maximum = 1;
            this.Minimum = 0;
            this.Value = 0;
        }
        public void thread_Run(G2DProgressDialog d)
        {
            try
            {
                Dialog = d;
                ThreadRun();
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
            finally
            {
                IsDone = true;
            }
        }
        public void main_CloseClick() { MainOnCloseClick(); }
        public void main_Begin() { MainOnBegin(); MainBegin?.Invoke(); }
        public void main_Done() { MainOnDone(); MainFinish?.Invoke(); }

        protected abstract void ThreadRun();
        protected virtual void MainOnCloseClick() { }
        protected virtual void MainOnBegin() { }
        protected virtual void MainOnDone() { }

        public event Action MainBegin;
        public event Action MainFinish;
    }

    public class DefaultProgress : IProgress
    {
        public override long Maximum { get => progress.Max; protected set { } }
        public override long Minimum { get => progress.Min; protected set { } }
        public override long Value { get => progress.Value; protected set { } }
        public override string Text { get => $"{progress.Value}/{progress.Max} {(progress.Rate * 100).ToString("0.00")}% : {progress.Text}"; protected set { } }
        private readonly PRangeValue progress;
        private readonly Func<IRangeValue, object> threadRun;
        public DefaultProgress(string title, Func<IRangeValue, object> threadRun, bool canBreak)
        {
            this.Title = title;
            this.threadRun = threadRun;
            this.CanBreakOnClose = canBreak;
            this.progress = new PRangeValue(this);
        }
        protected override void ThreadRun()
        {
            this.Product = threadRun(progress);
        }
        class PRangeValue : AtomicRangeValue
        {
            readonly DefaultProgress p;
            public PRangeValue(DefaultProgress p) : base(0, 0, 1)
            {
                this.p = p;
            }
            public override IRangeValue Update()
            {
                if (p.Dialog != null)
                {
                    p.Dialog.Invoke(() =>
                    {
                        p.Dialog.RefreshBar();
                    });
                }
                return this;
            }
        }
    }

    public class StreamProgress : IProgress
    {
        public override long Minimum { get => 0; protected set { } }
        public override long Maximum { get => this.stream == null ? 1 : (Math.Max(1, stream.Length / 1024)); protected set { } }
        public override long Value { get => this.stream == null ? 1 : (stream.Position / 1024); protected set { } }
        public override string Text { get => $"{Value}/{Maximum} {(Value / Maximum * 100).ToString("0.00")}%"; protected set { } }
        public override bool CanBreakOnClose => false;
        private Stream stream;
        private Func<Stream, object> threadRun;
        public StreamProgress(string title, Stream stream, Func<Stream, object> threadRun)
        {
            this.Title = title;
            this.stream = stream;
            this.threadRun = threadRun;
        }
        public StreamProgress(string title, Stream stream, Action<Stream> threadRun)
        {
            this.Title = title;
            this.stream = stream;
            this.threadRun = (s) => { threadRun(s); return null; };
        }
        protected override void ThreadRun()
        {
            this.Product = threadRun(stream);
            this.stream = null;
        }
    }
    //--------------------------------------------------------------------------------------------------------
}
