using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepEditor.Common.G2D
{
    public class G2DBaseComboBox : MaterialSkin.Controls.MaterialComboBox, IG2DBaseComponent
    {
        public G2DBaseComboBox()
        {
            this.AutoSize = false; 
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

        }
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
    }
}
