using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WeifenLuo.WinFormsUI.ThemeVS2013
{
    [ToolboxItem(false)]
    internal class VS2013SplitterControl : DockPane.SplitterControlBase
    {
        private int SplitterSize { get; }

        public VS2013SplitterControl(DockPane pane)
            : base(pane)
        {
            SplitterSize = pane.DockPanel.Theme.Measures.SplitterSize;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
               var _horizontalBrush = this.DockPane.DockPanel.Theme.PaintingService.GetBrush(this.DockPane.DockPanel.Theme.ColorPalette.MainWindowActive.Background);
                Rectangle rect = ClientRectangle;
                if (rect.Width <= 0 || rect.Height <= 0)
                    return;
                switch (Alignment)
                {
                    case DockAlignment.Right:
                    case DockAlignment.Left:
                        {
                            Debug.Assert(SplitterSize == rect.Width);
                            e.Graphics.FillRectangle(_horizontalBrush, rect);
                        }
                        break;
                    case DockAlignment.Bottom:
                    case DockAlignment.Top:
                        {
                            Debug.Assert(SplitterSize == rect.Height);
                            e.Graphics.FillRectangle(_horizontalBrush, rect);
                        }
                        break;
                }
            }
            catch { }
        }
    }
}