using DeepCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.Windows
{
    /// <summary>
    /// 维护多个Form在一个线程里
    /// </summary>
    public class FormStack : Disposable
    {
        public event Action<Exception> OnError;
        private Control rootControl;
        private List<Form> formStack = new List<Form>();
        private Queue<Action> actionQueue = new Queue<Action>();
        private Thread thread;
        public FormStack(Control rootControl = null)
        {
            this.AsSynchronizedDisposing();
            this.rootControl = rootControl;
            this.thread = new Thread(ThreadMain);
            this.thread.Start();
        }
        //------------------------------------------------------------------------
        public List<Form> Forms
        {
            get
            {
                lock (formStack)
                {
                    return new List<Form>(formStack);
                }
            }
        }
        private void PushForm(Form form)
        {
            lock (formStack)
            {
                formStack.Add(form);
            }
            form.FormClosed += (sender, e) =>
            {
                RemoveForm(form);
            };
        }
        private void RemoveForm(Form form)
        {
            lock (formStack)
            {
                formStack.Remove(form);
            }
        }
        private bool TryPeekForm(out Form form)
        {
            lock (formStack)
            {
                if (formStack.Count > 0)
                {
                    form = formStack[0];
                    return true;
                }
                else
                {
                    form = null;
                    return false;
                }
            }
        }
        private bool TryDequeueMain(out Action action)
        {
            lock (actionQueue)
            {
                return actionQueue.TryDequeue(out action);
            }
        }
        protected override void Disposing()
        {
            InvokeMain((parent) =>
            {
                foreach (var f in Forms)
                {
                    try
                    {
                        f.Dispose();
                    }
                    catch (Exception err)
                    {
                        OnError?.Invoke(err);
                    }
                }
            });
            thread.Join();
        }
        //------------------------------------------------------------------------
        private void ThreadMain()
        {
            while (!IsDisposing)
            {
                while (TryDequeueMain(out var action))
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception err)
                    {
                        OnError?.Invoke(err);
                    }
                }
                Thread.Sleep(1);
                Thread.Yield();
            }
        }
        private void ThreadEnqueue(Action action)
        {
            lock (actionQueue)
            {
                actionQueue.Enqueue(action);
            }
        }
        //------------------------------------------------------------------------
        protected void InvokeMain(Action<Control> action)
        {
            if (rootControl != null)
            {
                try
                {
                    rootControl.Invoke(() => action(rootControl)); return;
                }
                catch { }
            }
            if (TryPeekForm(out var form))
            {
                form.Invoke(() => action(form));
            }
            else
            {
                ThreadEnqueue(() => action(null));
            }
        }
        public void Invoke(Action<Control> action)
        {
            if (IsDisposing) throw new Exception();
            if (rootControl != null)
            {
                try
                {
                    rootControl.Invoke(() => action(rootControl)); return;
                }
                catch { }
            }
            if (TryPeekForm(out var form))
            {
                form.Invoke(() => action(form));
            }
            else
            {
                ThreadEnqueue(() => action(null));
            }
        }
        public void Invoke(Action action)
        {
            if (IsDisposing) throw new Exception();
            if (rootControl != null)
            {
                try
                {
                    rootControl.Invoke(action); return;
                }
                catch { }
            }
            if (TryPeekForm(out var form))
            {
                form.Invoke(action);
            }
            else
            {
                ThreadEnqueue(action);
            }
        }
        //------------------------------------------------------------------------
        public void Run<T>(Func<Control, T> f) where T : Form
        {
            Invoke((parent) =>
            {
                try
                {
                    var form = f(parent);
                    PushForm(form);
                    if (parent != null)
                        form.Show();
                    else
                        Application.Run(form);
                }
                catch (Exception err)
                {
                    OnError?.Invoke(err);
                }
            });
        }
        public void Run<T>(Func<T> f) where T : Form
        {
            Run((parent) => f());
        }
        //------------------------------------------------------------------------
    }
}
