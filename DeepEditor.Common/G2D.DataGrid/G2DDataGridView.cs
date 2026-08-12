using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D.DataGrid
{
    public partial class G2DDataGridView : DataGridView
    {
        public G2DDataGridView()
        {
            MaterialSkinManager.AddIgnoreControlType(typeof(DataGridView));
            MaterialSkinManager.AddIgnoreControlType(typeof(G2DDataGridView));
        }
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
        }
    }
}
