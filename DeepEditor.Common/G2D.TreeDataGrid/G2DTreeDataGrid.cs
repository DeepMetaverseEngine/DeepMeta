using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AdvancedDataGridView;
using System.Reflection;
using System.Collections;
using DeepCore.Reflection;

namespace DeepEditor.Common.Utils.TreeGrid
{
    public partial class G2DTreeDataGrid : UserControl
    {
        private G2DTreeGridDocument selected_doc ;

        public readonly Image img_icon_men;

        public G2DTreeDataGrid()
        {
            InitializeComponent();
            this.img_icon_men = imageList1.Images["icon_men.png"];
        }

        //-------------------------------------------------------------------------------------------

        public object SelectedObject
        {
            get
            {
                if (selected_doc != null)
                {
                    return selected_doc.Value;
                }
                return null;
            }
            set
            {
                SetSelectedObject(value);
            }
        }
        public G2DTreeGridValueNode CurrentValueNode
        {
            get
            {
                return treeGridView1.CurrentNode as G2DTreeGridValueNode;
            }
        }

        private void SetSelectedObject(object obj)
        {
            if (obj != null)
            {
                selected_doc = new G2DTreeGridDocument(this);
                treeGridView1.Nodes.Clear();
                treeGridView1.Nodes.Add(selected_doc);
                selected_doc.Value = (obj);
                treeGridView1.ExpandNode(selected_doc);
                treeGridView1.ExpandNode(selected_doc.DocumentElement);
            }
            else
            {
                selected_doc = null;
                treeGridView1.Nodes.Clear();
            }
        }

        private void SelectNode(int rowIndex)
        {
            try
            {
                TreeGridNode node = treeGridView1.GetNodeForRow(rowIndex);
                if (node != treeGridView1.CurrentNode)
                {
                    treeGridView1.ClearSelection();
                    node.Selected = true;
                    treeGridView1.CurrentCell = node.Cells[1];
                    treeGridView1.Invalidate();
                }
            }
            catch (Exception err) { }
        }

        //-------------------------------------------------------------------------------------------
        #region _Windows事件处理_

        // 选择 //
        private void treeGridView1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            SelectNode(e.RowIndex);
        }
        private void treeGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            SelectNode(e.RowIndex);
        }
        private void menuNode_Opening(object sender, CancelEventArgs e)
        {
            if (CurrentValueNode != null)
            {
                CurrentValueNode.OnMenuOpening(e);
            }
        }

        private void menuNode_New_Click(object sender, EventArgs e)
        {
            if (CurrentValueNode != null)
            {
                CurrentValueNode.OnNewContentClick();
            }
        }

        private void menuNode_Delete_Click(object sender, EventArgs e)
        {
            if (CurrentValueNode != null)
            {
                CurrentValueNode.OnDelContentClick();
            }
        }

        private void menuNode_Up_Click(object sender, EventArgs e)
        {
            if (CurrentValueNode != null)
            {
                CurrentValueNode.OnMoveListItemIndexClick(-1);
            }
        }

        private void menuNode_Down_Click(object sender, EventArgs e)
        {
            if (CurrentValueNode != null)
            {
                CurrentValueNode.OnMoveListItemIndexClick(1);
            }
        }


        #endregion


        //-------------------------------------------------------------------------------------------


    }

}
