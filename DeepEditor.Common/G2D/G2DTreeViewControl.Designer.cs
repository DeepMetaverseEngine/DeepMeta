using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    partial class G2DTreeViewControl
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(G2DTreeViewControl));
            toolStrip = new G2DBaseToolStrip();
            btn_RefreshTree = new G2DBaseToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btn_CollapseAll = new G2DBaseToolStripButton();
            btn_ExpandALL = new G2DBaseToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            chk_CheckON = new G2DBaseToolStripButton();
            toolStripSeparatorLeft = new ToolStripSeparator();
            txtFilter = new G2DBaseToolStripTextBox();
            btn_Find = new G2DBaseToolStripButton();
            toolStripSeparatorRight = new ToolStripSeparator();
            treeView = new G2DTreeView();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStrip.CanOverflow = false;
            toolStrip.CustomBackColor = null;
            toolStrip.CustomForeColor = null;
            toolStrip.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip.Items.AddRange(new ToolStripItem[] { btn_RefreshTree, toolStripSeparator1, btn_CollapseAll, btn_ExpandALL, toolStripSeparator2, chk_CheckON, toolStripSeparatorLeft, txtFilter, btn_Find, toolStripSeparatorRight });
            toolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            toolStrip.Location = new System.Drawing.Point(0, 0);
            toolStrip.MinimumSize = new System.Drawing.Size(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new System.Drawing.Size(381, 33);
            toolStrip.TabIndex = 2;
            toolStrip.Text = "toolStrip1";
            // 
            // btn_RefreshTree
            // 
            btn_RefreshTree.CustomBackColor = null;
            btn_RefreshTree.CustomForeColor = null;
            btn_RefreshTree.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_RefreshTree.Image = (System.Drawing.Image)resources.GetObject("btn_RefreshTree.Image");
            btn_RefreshTree.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_RefreshTree.ImageOrigin");
            btn_RefreshTree.ImageScaling = ToolStripItemImageScaling.None;
            btn_RefreshTree.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_RefreshTree.Name = "btn_RefreshTree";
            btn_RefreshTree.Size = new System.Drawing.Size(34, 28);
            btn_RefreshTree.Text = "Refresh";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // btn_CollapseAll
            // 
            btn_CollapseAll.CustomBackColor = null;
            btn_CollapseAll.CustomForeColor = null;
            btn_CollapseAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_CollapseAll.Image = (System.Drawing.Image)resources.GetObject("btn_CollapseAll.Image");
            btn_CollapseAll.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_CollapseAll.ImageOrigin");
            btn_CollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_CollapseAll.Name = "btn_CollapseAll";
            btn_CollapseAll.Size = new System.Drawing.Size(34, 28);
            btn_CollapseAll.Text = "收缩所有";
            // 
            // btn_ExpandALL
            // 
            btn_ExpandALL.CustomBackColor = null;
            btn_ExpandALL.CustomForeColor = null;
            btn_ExpandALL.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_ExpandALL.Image = (System.Drawing.Image)resources.GetObject("btn_ExpandALL.Image");
            btn_ExpandALL.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_ExpandALL.ImageOrigin");
            btn_ExpandALL.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_ExpandALL.Name = "btn_ExpandALL";
            btn_ExpandALL.Size = new System.Drawing.Size(34, 28);
            btn_ExpandALL.Text = "展开所有";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // chk_CheckON
            // 
            chk_CheckON.Checked = true;
            chk_CheckON.CheckOnClick = true;
            chk_CheckON.CheckState = CheckState.Checked;
            chk_CheckON.CustomBackColor = null;
            chk_CheckON.CustomForeColor = null;
            chk_CheckON.DisplayStyle = ToolStripItemDisplayStyle.Image;
            chk_CheckON.Image = (System.Drawing.Image)resources.GetObject("chk_CheckON.Image");
            chk_CheckON.ImageOrigin = (System.Drawing.Image)resources.GetObject("chk_CheckON.ImageOrigin");
            chk_CheckON.ImageTransparentColor = System.Drawing.Color.Magenta;
            chk_CheckON.Name = "chk_CheckON";
            chk_CheckON.Size = new System.Drawing.Size(34, 28);
            chk_CheckON.Text = "多选";
            // 
            // toolStripSeparatorLeft
            // 
            toolStripSeparatorLeft.Name = "toolStripSeparatorLeft";
            toolStripSeparatorLeft.Size = new System.Drawing.Size(6, 23);
            // 
            // txtFilter
            // 
            txtFilter.BorderStyle = BorderStyle.FixedSingle;
            txtFilter.CustomBackColor = null;
            txtFilter.CustomForeColor = null;
            txtFilter.Name = "txtFilter";
            txtFilter.Size = new System.Drawing.Size(50, 30);
            txtFilter.KeyDown += txtFilter_KeyDown;
            txtFilter.TextChanged += txtFilter_TextChanged;
            // 
            // btn_Find
            // 
            btn_Find.Alignment = ToolStripItemAlignment.Right;
            btn_Find.CustomBackColor = null;
            btn_Find.CustomForeColor = null;
            btn_Find.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_Find.Image = (System.Drawing.Image)resources.GetObject("btn_Find.Image");
            btn_Find.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_Find.ImageOrigin");
            btn_Find.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Find.Name = "btn_Find";
            btn_Find.Size = new System.Drawing.Size(34, 28);
            btn_Find.Text = "Search";
            btn_Find.Click += btn_Find_Click;
            // 
            // toolStripSeparatorRight
            // 
            toolStripSeparatorRight.Alignment = ToolStripItemAlignment.Right;
            toolStripSeparatorRight.Name = "toolStripSeparatorRight";
            toolStripSeparatorRight.Size = new System.Drawing.Size(6, 23);
            // 
            // treeView
            // 
            treeView.AllowDrop = true;
            treeView.CheckBoxes = true;
            treeView.CustomBackColor = null;
            treeView.CustomForeColor = null;
            treeView.Dock = DockStyle.Fill;
            treeView.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeView.EnableCopyPaste = true;
            treeView.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            treeView.FullRowSelect = true;
            treeView.HideSelection = false;
            treeView.ImageKey = "icon_Group";
            treeView.ImeMode = ImeMode.NoControl;
            treeView.ItemHeight = 32;
            treeView.LineColor = System.Drawing.Color.DarkGray;
            treeView.Location = new System.Drawing.Point(0, 33);
            treeView.Margin = new Padding(3, 4, 3, 4);
            treeView.Name = "treeView";
            treeView.PathSeparator = "/";
            treeView.SelectedImageKey = "icon_Group";
            treeView.ShowNodeToolTips = true;
            treeView.Size = new System.Drawing.Size(381, 736);
            treeView.TabIndex = 3;
            // 
            // G2DTreeViewControl
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            Controls.Add(treeView);
            Controls.Add(toolStrip);
            Name = "G2DTreeViewControl";
            Size = new System.Drawing.Size(381, 769);
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private G2DBaseToolStrip toolStrip;
        private G2DBaseToolStripButton btn_RefreshTree;
        private ToolStripSeparator toolStripSeparator1;
        private G2DBaseToolStripButton btn_CollapseAll;
        private G2DBaseToolStripButton btn_ExpandALL;
        private ToolStripSeparator toolStripSeparator2;
        private G2DBaseToolStripButton chk_CheckON;
        private ToolStripSeparator toolStripSeparatorLeft;
        private G2DTreeView treeView;
        private G2DBaseToolStripTextBox txtFilter;
        private ToolStripSeparator toolStripSeparatorRight;
        private G2DBaseToolStripButton btn_Find;
    }
}
