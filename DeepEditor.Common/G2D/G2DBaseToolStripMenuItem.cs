using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public class G2DBaseToolStripMenuItem : MaterialToolStripMenuItem, IG2DBaseComponent, IG2DBaseToolStripItem
    {
        private Image _Orginimage;
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(true)] public Image ImageOrigin { get => _Orginimage; set { _Orginimage = value; if (value != null) Image = value; } }
        [Browsable(false)] public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
    }

    public class G2DBaseToolStripDropDownButton : ToolStripDropDownButton, IG2DBaseComponent, IG2DBaseToolStripItem
    {
        private Image _Orginimage;
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(true)] public Image ImageOrigin { get => _Orginimage; set { _Orginimage = value; if (value != null) Image = value; } }
        [Browsable(false)] public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public G2DBaseToolStripDropDownButton()
        {
            //             AutoSize = false;
            //             Size = new Size(128, 32);
        }

        //         protected override ToolStripDropDown CreateDefaultDropDown()
        //         {
        //             var baseDropDown = base.CreateDefaultDropDown();
        //             if (DesignMode) return baseDropDown;
        // 
        //             var defaultDropDown = new G2DBaseToolStripDropDownMenu() { OwnerItem = this };
        //             defaultDropDown.Items.AddRange(baseDropDown.Items);
        //             defaultDropDown.MaximumSize = new Size(512, 512);
        //             return defaultDropDown;
        //         }
    }

    public class G2DBaseToolStripButton : ToolStripButton, IG2DBaseComponent, IG2DBaseToolStripItem
    {
        private Image _Orginimage;
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(true)] public Image ImageOrigin { get => _Orginimage; set { _Orginimage = value; if (value != null) Image = value; } }
        [Browsable(false)] public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Checked)
            {
                var rect = e.ClipRectangle;

                //                 rect.X += 1;
                //                 rect.Y += 1;
                //                 rect.Width -= 2;
                //                 rect.Height -= 2;
                //                 e.Graphics.DrawRectangle(Pens.LightGreen, rect);

                //                 rect.X += 1;
                //                 rect.Y += 1;
                //                 rect.Width -= 1;
                //                 rect.Height -= 1;
                rect.Y += (rect.Width - 5);
                rect.Height = 4;
                rect.X += 2;
                rect.Width -= 4;
                e.Graphics.FillRectangle(Brushes.Lime, rect);
            }
        }
    }

    public class G2DBaseToolStripTextBox : ToolStripTextBox, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(false)] public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

    }
    public class G2DBaseToolStripLabel : ToolStripLabel, IG2DBaseComponent, IG2DBaseToolStripItem
    {
        private Image _Orginimage;
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(true)] public Image ImageOrigin { get => _Orginimage; set { _Orginimage = value; if (value != null) Image = value; } }
        [Browsable(false)] public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        [Browsable(true)] public bool NeverDrawOverflow { get; set; } = false;
        protected override void OnPaint(PaintEventArgs e)
        {
//             if (NeverDrawOverflow)
//             {
//                 var clip = e.ClipRectangle;
//                 //clip.Width = int.MaxValue;
//                 //TextRenderer.DrawText(e.Graphics, this.Text, this.Font, clip, this.ForeColor);
//                 e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(ForeColor), 0, 0);
//             }
//             else
            {
                base.OnPaint(e);
            }
        }

    }
    //--------------------------------------------------------------------------------------------------------------------------
    #region Menu
    public class G2DBaseContextMenuStrip : MaterialContextMenuStrip, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        public G2DBaseContextMenuStrip()
        {
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        }
        public G2DBaseContextMenuStrip(IContainer container) : base(container)
        {
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        }

    }

    public class G2DBaseToolStripDropDownMenu : MaterialContextMenuStrip, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        public G2DBaseToolStripDropDownMenu()
        {
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        }

    }

    public class G2DBaseToolStrip : ToolStrip, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public G2DBaseToolStrip()
        {
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Renderer = new MaterialToolStripRender();
            BackColor = SkinManager.BackdropColor;
        }
        protected override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
        {
            //return base.CreateDefaultItem(text, image, onClick);
            if (text == "-")
            {
                return new ToolStripSeparator();
            }
            else
            {
                var item = new G2DBaseToolStripButton() { Text = text, Image = image };
                item.Click += onClick;
                return item;
            }
        }
    }

    public class G2DBaseStatusStrip : StatusStrip, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        public G2DBaseStatusStrip()
        {
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Renderer = new MaterialToolStripRender();
            BackColor = SkinManager.BackdropColor;
        }
    }

    #endregion
    //--------------------------------------------------------------------------------------------------------------------------
}
