using MaterialSkin;
using System;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public partial class G2DForm : Form, IG2DBaseComponent
    {
        public static MaterialSkinManager GlobalSkinManager => MaterialSkinManager.Instance;
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        public G2DForm()
        {
            GlobalSkinManager.AddFormToManage(this);
        }
        protected override void Dispose(bool disposing)
        {
            GlobalSkinManager.RemoveFormToManage(this);
            base.Dispose(disposing);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ClassStyle |= G2DBaseForm.SystemShadow.CS_DropSHADOW;
                return cp;
            }
        }


   
    }
}
