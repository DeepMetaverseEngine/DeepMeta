using DeepCore.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common
{
    public class RichTextBoxLogger : Logger
    {
        private RichTextBox textbox;
        public RichTextBoxLogger(RichTextBox text, object owner) : base(LoggerFactory.CurrentFactory, owner)
        {
            this.textbox = text;
        }
        protected override void PrintText(string text, LoggerLevel level)
        {
            base.PrintText(text, level);
            textbox.Invoke(() =>
            {
                if (level == LoggerLevel.ERROR)
                {
                    textbox.SelectionColor = System.Drawing.Color.Red;
                    textbox.AppendText(text);
                }
                else if (level == LoggerLevel.WARNNING)
                {
                    textbox.SelectionColor = System.Drawing.Color.Yellow;
                    textbox.AppendText(text);
                }
                else
                {
                    textbox.SelectionColor = textbox.ForeColor;
                    textbox.AppendText(text);
                }
            });
        }
    }
}
