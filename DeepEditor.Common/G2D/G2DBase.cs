using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MaterialSkin.MaterialSkinManager;

namespace DeepEditor.Common.G2D
{
    internal class G2DBase
    {
    }

    public interface IG2DBaseComponent
    {
        Color? CustomForeColor { get; set; }
        Color? CustomBackColor { get; set; }
        MaterialSkinManager SkinManager { get; }
    }

    public interface IG2DBaseToolStripItem
    {
        Image Image { get; set; }
        Image ImageOrigin { get; set; }
    }

    public interface IG2DSkinListener
    {
        void OnSkinChanged(MaterialSkinManager manager, Control control, Themes themes);
    }
}
