using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DeepCore.Reflection.Modeling;

namespace DeepEditor.Common.G2D.DataGrid
{
    public partial class G2DDataGridEditor : Form
    {
        private G2DDataGrid dataGrid;
        private bool isDataChanged = false;

        public G2DDataGridEditor(IList<object> list)
        {
            InitializeComponent();

            this.SuspendLayout();
            this.dataGrid = new G2DDataGrid(list);
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(0, 0);
            this.dataGrid.Size = new System.Drawing.Size(986, 547);
            this.dataGrid.TabIndex = 0;
            this.dataGrid.OnDataChanged += new G2DDataGrid.DataChangedHandler(dataGrid_OnDataChanged);
            this.panel1.Controls.Add(this.dataGrid);
            this.ResumeLayout(false);
        }

        public bool IsDataChanged
        {
            get { return isDataChanged; }
        }

        private void dataGrid_OnDataChanged(G2DDataGrid sender, UmlValueNode cell, object new_value)
        {
            isDataChanged = true;
            sender.SaveAll();
        }

    }
}
