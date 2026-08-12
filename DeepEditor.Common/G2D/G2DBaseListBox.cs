using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public class G2DBaseListBox :  ListBox, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public G2DBaseListBox()
        {
            this.AutoSize = false; 
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

        }
    }
}
