namespace DeepEditor.Common.G2D
{
    partial class G2DListSelectTree
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
            g2dTreeViewControl1 = new G2DTreeViewControl();
            SuspendLayout();
            // 
            // g2dTreeViewControl1
            // 
            g2dTreeViewControl1.CheckBoxes = true;
            g2dTreeViewControl1.CustomBackColor = null;
            g2dTreeViewControl1.CustomForeColor = null;
            g2dTreeViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            g2dTreeViewControl1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            g2dTreeViewControl1.EnableCopyPaste = true;
            g2dTreeViewControl1.EnableDragDrop = false;
            g2dTreeViewControl1.FullRowSelect = true;
            g2dTreeViewControl1.HideSelection = false;
            g2dTreeViewControl1.ImageKey = "icon_Group";
            g2dTreeViewControl1.ImageList = null;
            g2dTreeViewControl1.ItemHeight = 32;
            g2dTreeViewControl1.LineColor = System.Drawing.Color.DarkGray;
            g2dTreeViewControl1.Location = new System.Drawing.Point(0, 0);
            g2dTreeViewControl1.Name = "g2dTreeViewControl1";
            g2dTreeViewControl1.SelectedImageKey = "icon_Group";
            g2dTreeViewControl1.SelectedNode = null;
            g2dTreeViewControl1.ShowNodeToolTips = true;
            g2dTreeViewControl1.Size = new System.Drawing.Size(480, 768);
            g2dTreeViewControl1.TabIndex = 0;
            g2dTreeViewControl1.TreeViewNodeSorter = null;
            // 
            // G2DListSelectTree
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(g2dTreeViewControl1);
            Name = "G2DListSelectTree";
            Size = new System.Drawing.Size(480, 768);
            ResumeLayout(false);
        }

        #endregion

        private G2DTreeViewControl g2dTreeViewControl1;
    }
}
