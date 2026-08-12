using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DeepCore.Log;
using System.IO;
using DeepCore;

using Microsoft.Win32;

namespace DeepEditor.Common.Utils
{
    public partial class ConsoleOutput : DeepEditor.Common.G2D.G2DBaseForm
    {
        private MyLogger log;
        private LoggerFactory old_factory;
        private Form m_DockTo;
        private Form m_OldActive;

        public bool IsDock
        {
            get { return toolDock.Checked; }
        }
        public Form DockTo
        {
            get { return m_DockTo; }
            set
            {
                m_DockTo = value;
            }
        }

        public ConsoleOutput(string name = "Output")
        {
            InitializeComponent();
            bool dock = true;
            if (RegistUtils.TryGetAppRegistry("ConsoleOutput.Dock", out dock))
            {
                this.toolDock.Checked = dock;
            }
            this.old_factory = LoggerFactory.CurrentFactory;
            this.log = new MyLogger(name);
            LoggerFactory.SetFactory(new MyLoggerFactory());
            this.Disposed += ConsoleOutput_Disposed;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string text = MyLogger.PopLines();
            if (!string.IsNullOrEmpty(text))
            {
                this.textBox1.AppendText(text);
            }
            if (this.IsDock && m_DockTo != null)
            {
                var dsize = m_DockTo.Bounds;
                this.Bounds = new System.Drawing.Rectangle(
                    dsize.X, dsize.Y + dsize.Height,
                    dsize.Width, this.Height);
                if (m_DockTo == Form.ActiveForm)
                {
                    if (m_OldActive != Form.ActiveForm)
                    {
                        this.Activate();
                        m_DockTo.Activate();
                    }
                }
                m_OldActive = Form.ActiveForm;
            }
        }


        private void ConsoleOutput_Disposed(object sender, EventArgs e)
        {
            RegistUtils.PutAppRegistry("ConsoleOutput.Dock", this.toolDock.Checked);
            LoggerFactory.SetFactory(old_factory);
        }

        public Logger GetLogger()
        {
            return log;
        }


        internal class MyLoggerFactory : LoggerFactory
        {
            override protected Logger CreateLogger(object name)
            {
                return new MyLogger(name);
            }
        }

        internal class MyLogger : Logger
        {
            static public List<string> lines = new List<string>();

            public MyLogger(object name) : base(name)
            {
            }
            protected override void Print(LoggerLevel level, object format, string text, Exception err)
            {
                base.Print(level, format, text, err);
                lock (lines)
                {
                    if (err != null)
                        lines.Add(text + " : " + err.Message + Environment.NewLine + err.StackTrace);
                    else
                        lines.Add(text);
                }
            }

            static public string PopLines()
            {
                StringBuilder sb = new StringBuilder();
                lock (lines)
                {
                    foreach (string line in lines)
                    {
                        sb.AppendLine(line);
                    }
                    lines.Clear();
                }
                return sb.ToString();
            }
        }
    }
}
