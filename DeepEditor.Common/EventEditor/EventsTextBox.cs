using DeepCore.EventTrigger;
using DeepCore.GUI.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.EventEditor
{
    public partial class EventsTextBox : Form
    {
        public EventsTextBox()
        {
            InitializeComponent();
        }
        public EventsTextBox(IReadOnlyList<IEventDataNode> events)
        {
            InitializeComponent();
            SetEvents(events);
        }
        public void SetEvents(IReadOnlyList<IEventDataNode> events)
        {
            var doc = EventStringBuilder.BehaviorDocument(events);
            var atext = Win32RichTextLayer.DecodeAttributedString(doc,
                    this.richTextBox1.Font,
                    DeepCore.GUI.Display.Text.RichTextAlignment.taLEFT,
                    this.richTextBox1.Font.Size,
                    this.richTextBox1.ForeColor,
                    DeepCore.GUI.Data.TextFontStyle.Plain);
            Win32RichTextLayer.AppendAttributeText(richTextBox1, atext);
        }
    }
}
