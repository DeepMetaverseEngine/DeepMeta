using DeepCore;
using DeepCore.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DeepEditor.Common.G2D
{
    public class G2DTimer : System.Windows.Forms.Timer
    {
        // 内部主线程命令 //
        private readonly MessageActionQueue<G2DTimer> mTasks;
        public MessageActionQueue<G2DTimer> TaskQueue => mTasks;
        public G2DTimer()
        {
            mTasks = new MessageActionQueue<G2DTimer>();
        }
        public G2DTimer(IContainer container) : base(container)
        {
            mTasks = new MessageActionQueue<G2DTimer>();
        }


        /// <summary>
        /// 保证在Task内部执行的代码线程安全
        /// </summary>
        /// <param name="task"></param>
        public void QueueTask(Action task)
        {
            mTasks.Enqueue(task);
        }

        protected override void OnTick(EventArgs e)
        {
            base.OnTick(e);
            mTasks.ProcessMessages(this);
        }
    }
}
