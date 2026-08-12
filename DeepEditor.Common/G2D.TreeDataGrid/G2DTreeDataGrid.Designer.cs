using AdvancedDataGridView;

namespace DeepEditor.Common.Utils.TreeGrid
{
    partial class G2DTreeDataGrid
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(G2DTreeDataGrid));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeGridView1 = new AdvancedDataGridView.TreeGridView();
            this.ColumnIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColumnField = new AdvancedDataGridView.TreeGridColumn();
            this.ColumnFieldType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new DeepEditor.Common.G2D.G2DBaseToolStrip();
            this.menuNode = new DeepEditor.Common.G2D.G2DBaseContextMenuStrip(this.components);
            this.menuNode_New = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.menuNode_Delete = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuNode_Up = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.menuNode_Down = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView1)).BeginInit();
            this.menuNode.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1299, 1430);
            this.splitContainer1.SplitterDistance = 670;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 4;
            // 
            // treeGridView1
            // 
            this.treeGridView1.AllowUserToAddRows = false;
            this.treeGridView1.AllowUserToDeleteRows = false;
            this.treeGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.treeGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.treeGridView1.ColumnHeadersHeight = 34;
            this.treeGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnIcon,
            this.ColumnField,
            this.ColumnFieldType});
            this.treeGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.treeGridView1.ImageList = this.imageList1;
            this.treeGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.treeGridView1.Location = new System.Drawing.Point(0, 0);
            this.treeGridView1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.treeGridView1.MultiSelect = false;
            this.treeGridView1.Name = "treeGridView1";
            this.treeGridView1.RowHeadersVisible = false;
            this.treeGridView1.RowHeadersWidth = 62;
            this.treeGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.treeGridView1.ShowEditingIcon = false;
            this.treeGridView1.Size = new System.Drawing.Size(1299, 670);
            this.treeGridView1.TabIndex = 3;
            this.treeGridView1.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.treeGridView1_CellContextMenuStripNeeded);
            this.treeGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.treeGridView1_CellMouseDown);
            // 
            // ColumnIcon
            // 
            this.ColumnIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnIcon.FillWeight = 1F;
            this.ColumnIcon.HeaderText = "";
            this.ColumnIcon.MinimumWidth = 23;
            this.ColumnIcon.Name = "ColumnIcon";
            this.ColumnIcon.ReadOnly = true;
            this.ColumnIcon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnIcon.Width = 23;
            // 
            // ColumnField
            // 
            this.ColumnField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnField.DefaultNodeImage = global::DeepEditor.Common.Properties.Resources.icon_men;
            this.ColumnField.HeaderText = "字段名";
            this.ColumnField.MinimumWidth = 200;
            this.ColumnField.Name = "ColumnField";
            this.ColumnField.ReadOnly = true;
            this.ColumnField.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnField.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnFieldType
            // 
            this.ColumnFieldType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnFieldType.FillWeight = 10F;
            this.ColumnFieldType.HeaderText = "类型";
            this.ColumnFieldType.MinimumWidth = 80;
            this.ColumnFieldType.Name = "ColumnFieldType";
            this.ColumnFieldType.ReadOnly = true;
            this.ColumnFieldType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icon_men.png");
            this.imageList1.Images.SetKeyName(1, "icons_tool_bar1.png");
            this.imageList1.Images.SetKeyName(2, "icons_tool_bar2.png");
            this.imageList1.Images.SetKeyName(3, "icons_tool_bar3.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1299, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menuNode
            // 
            this.menuNode.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNode_New,
            this.menuNode_Delete,
            this.toolStripMenuItem1,
            this.menuNode_Up,
            this.menuNode_Down});
            this.menuNode.Name = "menuCollection";
            this.menuNode.Size = new System.Drawing.Size(135, 130);
            this.menuNode.Opening += new System.ComponentModel.CancelEventHandler(this.menuNode_Opening);
            // 
            // menuNode_New
            // 
            this.menuNode_New.Name = "menuNode_New";
            this.menuNode_New.Size = new System.Drawing.Size(134, 30);
            this.menuNode_New.Text = "新建项";
            this.menuNode_New.Click += new System.EventHandler(this.menuNode_New_Click);
            // 
            // menuNode_Delete
            // 
            this.menuNode_Delete.Name = "menuNode_Delete";
            this.menuNode_Delete.Size = new System.Drawing.Size(134, 30);
            this.menuNode_Delete.Text = "删除项";
            this.menuNode_Delete.Click += new System.EventHandler(this.menuNode_Delete_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(131, 6);
            // 
            // menuNode_Up
            // 
            this.menuNode_Up.Name = "menuNode_Up";
            this.menuNode_Up.Size = new System.Drawing.Size(134, 30);
            this.menuNode_Up.Text = "向上";
            this.menuNode_Up.Click += new System.EventHandler(this.menuNode_Up_Click);
            // 
            // menuNode_Down
            // 
            this.menuNode_Down.Name = "menuNode_Down";
            this.menuNode_Down.Size = new System.Drawing.Size(134, 30);
            this.menuNode_Down.Text = "向下";
            this.menuNode_Down.Click += new System.EventHandler(this.menuNode_Down_Click);
            // 
            // G2DTreeDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "G2DTreeDataGrid";
            this.Size = new System.Drawing.Size(1299, 1455);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView1)).EndInit();
            this.menuNode.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TreeGridView treeGridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DeepEditor.Common.G2D.G2DBaseToolStrip toolStrip1;
        public DeepEditor.Common.G2D.G2DBaseContextMenuStrip menuNode;
        public System.Windows.Forms.ImageList imageList1;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem menuNode_New;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem menuNode_Delete;
        public System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem menuNode_Up;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem menuNode_Down;
        private System.Windows.Forms.DataGridViewImageColumn ColumnIcon;
        private TreeGridColumn ColumnField;
        private G2DTreeGridValueColumn ColumnFieldValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFieldType;
    }
}
