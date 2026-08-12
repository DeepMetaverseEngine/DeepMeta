using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepEditor.Common.G2D
{
    public class G2DBaseButton : MaterialSkin.Controls.MaterialButton, IG2DBaseComponent
    {
        public G2DBaseButton()
        {
            this.AutoSize = false; 
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

        }

        public Color? CustomForeColor { get ; set; }
        public Color? CustomBackColor { get ; set ; }
    }
}
