using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DeepCore.GUI.Display.Text;
using System.Xml;
using MaterialSkin.Controls;
using MaterialSkin;

namespace DeepEditor.Common.G2D
{
    public class G2DBaseTextBox : TextBox, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public G2DBaseTextBox()
        {
            this.AutoSize = false;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

        }

        //new public bool Password { get => base.Password; set => base.Password = value; }
        new public bool Multiline { get => base.Multiline; set => base.Multiline = value; }

    }
}
