using DeepCore.GUI.M3Z;
using DeepCore.MPQ;
using System.IO;
using System.Windows.Forms;

namespace DeepTools.MPQUpdate
{
    public partial class FormEntry : Form
    {
        public FormEntry(MPQFileSystem.MPQFileEntry e, MPQFileSystem fs)
        {
            InitializeComponent();

            this.Text = e.Key;

            if (e.Key.ToLower().EndsWith(".m3z"))
            {
                byte[] data = fs.GetData(e.Key);
                using (MemoryStream stream = new MemoryStream(data))
                {
                    var m3z = new M3ZHeaderMeta<M3ZTrunkMeta>(stream);
                    this.propertyGrid1.SelectedObject = m3z;
                }
            }
            else
            {
                this.propertyGrid1.SelectedObject = e;
            }
        }
    }
}
