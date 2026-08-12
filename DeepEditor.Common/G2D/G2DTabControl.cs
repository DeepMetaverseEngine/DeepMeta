using DeepEditor.Common.Properties;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public class G2DTabControl : TabControl
    {
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public Image CloseImage { get; set; }
        public event PageCloseClickedEventHandler PageCloseClicked;
        public event PageCloseClickedEventHandler PageClicked;
        public event PageCloseClickedEventHandler PageMouseDown;

        public G2DTabControl()
        {
            this.CloseImage = Resources.cancel;
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.Padding = new Point(10, 3);
        }
        public Rectangle GetCloseRectangle(Rectangle tabRect)
        {
            var cw = tabRect.Height - Padding.Y;
            return new Rectangle(tabRect.Right - 2 - cw, tabRect.Y, cw, cw);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            try
            {
                var rect = this.GetTabRect(e.Index);
                var page = TabPages[e.Index];
                var title = page.Text;
                if (e.Index == SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0x20, 0, 0, 255)), rect);
                    rect.X += 1;
                    rect.Y += 1;
                }
                e.Graphics.DrawImage(CloseImage, GetCloseRectangle(rect));
                e.Graphics.DrawString(title, page.Font, new SolidBrush(page.ForeColor), rect);
            }
            catch { }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                base.OnMouseDown(e);
                for (int index = 0; index < TabCount; index++)
                {
                    var rect = this.GetTabRect(index);
                    var page = TabPages[index];
                    if (rect.Contains(e.Location))
                    {
                        PageMouseDown?.Invoke(this, new TabPageMouseEventArgs(e, page));
                        return;
                    }
                }
            }
            catch { }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            try
            {
                base.OnMouseClick(e);
                if (SelectedIndex >= 0)
                {
                    var rect = this.GetTabRect(SelectedIndex);
                    var close = GetCloseRectangle(rect);
                    if (close.Contains(e.Location))
                    {
                        PageCloseClicked?.Invoke(this, new TabPageMouseEventArgs(e, TabPages[SelectedIndex]));
                    }
                    else
                    {
                        PageClicked?.Invoke(this, new TabPageMouseEventArgs(e, TabPages[SelectedIndex]));
                    }
                }
            }
            catch { }
        }

    }
    public class TabPageMouseEventArgs : MouseEventArgs
    {
        public TabPage Page { get; private set; }
        public TabPageMouseEventArgs(MouseEventArgs e, TabPage page) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            this.Page = page;
        }
    }
    public delegate void PageCloseClickedEventHandler(object sender, TabPageMouseEventArgs e);

}
