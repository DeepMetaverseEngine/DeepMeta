
namespace DeepEditor.Common.G2D
{
    partial class G2DTreeViewDataPanel
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(G2DTreeViewDataPanel));
            treeViewControl = new G2DTreeViewControl();
            imageList = new System.Windows.Forms.ImageList(components);
            groupMenu = new G2DBaseContextMenuStrip(components);
            groupBtn_AddNode = new G2DBaseToolStripMenuItem();
            groupBtn_AddGroup = new G2DBaseToolStripMenuItem();
            groupBtn_SendTo = new G2DBaseToolStripMenuItem();
            groupBtn_Rename = new G2DBaseToolStripMenuItem();
            groupBtn_EditAll = new G2DBaseToolStripMenuItem();
            groupBtn_Balance = new G2DBaseToolStripMenuItem();
            group_Delete = new G2DBaseToolStripMenuItem();
            childMenu = new G2DBaseContextMenuStrip(components);
            childBtn_SetID = new G2DBaseToolStripMenuItem();
            childBtn_Duplicate = new G2DBaseToolStripMenuItem();
            childBtn_EditGrid = new G2DBaseToolStripMenuItem();
            childBtn_Delete = new G2DBaseToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            showInFolderToolStripMenuItem = new G2DBaseToolStripMenuItem();
            childBtn_SendTo = new G2DBaseToolStripMenuItem();
            childBtn_CopyProperties = new G2DBaseToolStripMenuItem();
            childBtn_PastePorpertie = new G2DBaseToolStripMenuItem();
            groupMenu.SuspendLayout();
            childMenu.SuspendLayout();
            SuspendLayout();
            // 
            // treeViewControl
            // 
            treeViewControl.AllowDrop = true;
            treeViewControl.CheckBoxes = true;
            treeViewControl.CustomBackColor = null;
            treeViewControl.CustomForeColor = null;
            treeViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewControl.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            treeViewControl.EnableCopyPaste = true;
            treeViewControl.EnableDragDrop = true;
            treeViewControl.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            treeViewControl.FullRowSelect = true;
            treeViewControl.HideSelection = false;
            treeViewControl.ImageKey = "icon_Group";
            treeViewControl.ImageList = null;
            treeViewControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            treeViewControl.ItemHeight = 32;
            treeViewControl.LineColor = System.Drawing.Color.DarkGray;
            treeViewControl.Location = new System.Drawing.Point(0, 0);
            treeViewControl.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            treeViewControl.Name = "treeViewControl";
            treeViewControl.SelectedImageKey = "icon_Group";
            treeViewControl.SelectedNode = null;
            treeViewControl.ShowNodeToolTips = true;
            treeViewControl.Size = new System.Drawing.Size(500, 1000);
            treeViewControl.TabIndex = 0;
            treeViewControl.TreeViewNodeSorter = null;
            // 
            // imageList
            // 
            imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList.ImageStream");
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            imageList.Images.SetKeyName(0, "icon_Node");
            imageList.Images.SetKeyName(1, "icon_Group");
            // 
            // groupMenu
            // 
            groupMenu.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            groupMenu.CustomBackColor = null;
            groupMenu.CustomForeColor = null;
            groupMenu.Depth = 0;
            groupMenu.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            groupMenu.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            groupMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            groupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { groupBtn_AddNode, groupBtn_AddGroup, groupBtn_SendTo, groupBtn_Rename, groupBtn_EditAll, groupBtn_Balance, group_Delete });
            groupMenu.MouseState = MaterialSkin.MouseState.HOVER;
            groupMenu.Name = "groupMenu";
            groupMenu.Size = new System.Drawing.Size(179, 228);
            // 
            // groupBtn_AddNode
            // 
            groupBtn_AddNode.CustomBackColor = null;
            groupBtn_AddNode.CustomForeColor = null;
            groupBtn_AddNode.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            groupBtn_AddNode.Image = (System.Drawing.Image)resources.GetObject("groupBtn_AddNode.Image");
            groupBtn_AddNode.ImageOrigin = (System.Drawing.Image)resources.GetObject("groupBtn_AddNode.ImageOrigin");
            groupBtn_AddNode.Name = "groupBtn_AddNode";
            groupBtn_AddNode.Size = new System.Drawing.Size(178, 32);
            groupBtn_AddNode.Text = "添加";
            groupBtn_AddNode.Click += groupBtn_AddNode_Click;
            // 
            // groupBtn_AddGroup
            // 
            groupBtn_AddGroup.CustomBackColor = null;
            groupBtn_AddGroup.CustomForeColor = null;
            groupBtn_AddGroup.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            groupBtn_AddGroup.Image = (System.Drawing.Image)resources.GetObject("groupBtn_AddGroup.Image");
            groupBtn_AddGroup.ImageOrigin = null;
            groupBtn_AddGroup.Name = "groupBtn_AddGroup";
            groupBtn_AddGroup.Size = new System.Drawing.Size(178, 32);
            groupBtn_AddGroup.Text = "添加过滤器";
            groupBtn_AddGroup.Click += groupBtn_AddGroup_Click;
            // 
            // groupBtn_SendTo
            // 
            groupBtn_SendTo.CustomBackColor = null;
            groupBtn_SendTo.CustomForeColor = null;
            groupBtn_SendTo.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            groupBtn_SendTo.Image = (System.Drawing.Image)resources.GetObject("groupBtn_SendTo.Image");
            groupBtn_SendTo.ImageOrigin = null;
            groupBtn_SendTo.Name = "groupBtn_SendTo";
            groupBtn_SendTo.Size = new System.Drawing.Size(178, 32);
            groupBtn_SendTo.Text = "发送到";
            groupBtn_SendTo.Click += groupBtn_SendTo_Click;
            // 
            // groupBtn_Rename
            // 
            groupBtn_Rename.CustomBackColor = null;
            groupBtn_Rename.CustomForeColor = null;
            groupBtn_Rename.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            groupBtn_Rename.Image = (System.Drawing.Image)resources.GetObject("groupBtn_Rename.Image");
            groupBtn_Rename.ImageOrigin = (System.Drawing.Image)resources.GetObject("groupBtn_Rename.ImageOrigin");
            groupBtn_Rename.Name = "groupBtn_Rename";
            groupBtn_Rename.Size = new System.Drawing.Size(178, 32);
            groupBtn_Rename.Text = "重命名";
            groupBtn_Rename.Click += groupBtn_Rename_Click;
            // 
            // groupBtn_EditAll
            // 
            groupBtn_EditAll.CustomBackColor = null;
            groupBtn_EditAll.CustomForeColor = null;
            groupBtn_EditAll.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            groupBtn_EditAll.Image = (System.Drawing.Image)resources.GetObject("groupBtn_EditAll.Image");
            groupBtn_EditAll.ImageOrigin = (System.Drawing.Image)resources.GetObject("groupBtn_EditAll.ImageOrigin");
            groupBtn_EditAll.Name = "groupBtn_EditAll";
            groupBtn_EditAll.Size = new System.Drawing.Size(178, 32);
            groupBtn_EditAll.Text = "编辑所有";
            // 
            // groupBtn_Balance
            // 
            groupBtn_Balance.CustomBackColor = null;
            groupBtn_Balance.CustomForeColor = null;
            groupBtn_Balance.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            groupBtn_Balance.Image = (System.Drawing.Image)resources.GetObject("groupBtn_Balance.Image");
            groupBtn_Balance.ImageOrigin = (System.Drawing.Image)resources.GetObject("groupBtn_Balance.ImageOrigin");
            groupBtn_Balance.Name = "groupBtn_Balance";
            groupBtn_Balance.Size = new System.Drawing.Size(178, 32);
            groupBtn_Balance.Text = "配平数据";
            groupBtn_Balance.Click += groupBtn_Balance_Click;
            // 
            // group_Delete
            // 
            group_Delete.CustomBackColor = null;
            group_Delete.CustomForeColor = null;
            group_Delete.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            group_Delete.Image = (System.Drawing.Image)resources.GetObject("group_Delete.Image");
            group_Delete.ImageOrigin = (System.Drawing.Image)resources.GetObject("group_Delete.ImageOrigin");
            group_Delete.Name = "group_Delete";
            group_Delete.Size = new System.Drawing.Size(178, 32);
            group_Delete.Text = "删除";
            group_Delete.Click += group_Delete_Click;
            // 
            // childMenu
            // 
            childMenu.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            childMenu.CustomBackColor = null;
            childMenu.CustomForeColor = null;
            childMenu.Depth = 0;
            childMenu.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            childMenu.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            childMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            childMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { childBtn_SetID, childBtn_Duplicate, childBtn_EditGrid, childBtn_Delete, toolStripMenuItem1, showInFolderToolStripMenuItem, childBtn_SendTo, childBtn_CopyProperties, childBtn_PastePorpertie });
            childMenu.MouseState = MaterialSkin.MouseState.HOVER;
            childMenu.Name = "childMenu";
            childMenu.Size = new System.Drawing.Size(249, 299);
            // 
            // childBtn_SetID
            // 
            childBtn_SetID.CustomBackColor = null;
            childBtn_SetID.CustomForeColor = null;
            childBtn_SetID.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            childBtn_SetID.Image = (System.Drawing.Image)resources.GetObject("childBtn_SetID.Image");
            childBtn_SetID.ImageOrigin = (System.Drawing.Image)resources.GetObject("childBtn_SetID.ImageOrigin");
            childBtn_SetID.Name = "childBtn_SetID";
            childBtn_SetID.Size = new System.Drawing.Size(301, 32);
            childBtn_SetID.Text = "设置ID";
            childBtn_SetID.Click += childBtn_SetID_Click;
            // 
            // childBtn_Duplicate
            // 
            childBtn_Duplicate.CustomBackColor = null;
            childBtn_Duplicate.CustomForeColor = null;
            childBtn_Duplicate.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            childBtn_Duplicate.Image = (System.Drawing.Image)resources.GetObject("childBtn_Duplicate.Image");
            childBtn_Duplicate.ImageOrigin = (System.Drawing.Image)resources.GetObject("childBtn_Duplicate.ImageOrigin");
            childBtn_Duplicate.Name = "childBtn_Duplicate";
            childBtn_Duplicate.Size = new System.Drawing.Size(301, 32);
            childBtn_Duplicate.Text = "复制";
            childBtn_Duplicate.Click += childBtn_Duplicate_Click;
            // 
            // childBtn_EditGrid
            // 
            childBtn_EditGrid.CustomBackColor = null;
            childBtn_EditGrid.CustomForeColor = null;
            childBtn_EditGrid.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            childBtn_EditGrid.Image = (System.Drawing.Image)resources.GetObject("childBtn_EditGrid.Image");
            childBtn_EditGrid.ImageOrigin = (System.Drawing.Image)resources.GetObject("childBtn_EditGrid.ImageOrigin");
            childBtn_EditGrid.Name = "childBtn_EditGrid";
            childBtn_EditGrid.Size = new System.Drawing.Size(301, 32);
            childBtn_EditGrid.Text = "编辑";
            // 
            // childBtn_Delete
            // 
            childBtn_Delete.CustomBackColor = null;
            childBtn_Delete.CustomForeColor = null;
            childBtn_Delete.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            childBtn_Delete.Image = (System.Drawing.Image)resources.GetObject("childBtn_Delete.Image");
            childBtn_Delete.ImageOrigin = (System.Drawing.Image)resources.GetObject("childBtn_Delete.ImageOrigin");
            childBtn_Delete.Name = "childBtn_Delete";
            childBtn_Delete.Size = new System.Drawing.Size(301, 32);
            childBtn_Delete.Text = "删除";
            childBtn_Delete.Click += childBtn_Delete_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripMenuItem1.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(298, 6);
            // 
            // showInFolderToolStripMenuItem
            // 
            showInFolderToolStripMenuItem.CustomBackColor = null;
            showInFolderToolStripMenuItem.CustomForeColor = null;
            showInFolderToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            showInFolderToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("showInFolderToolStripMenuItem.Image");
            showInFolderToolStripMenuItem.ImageOrigin = null;
            showInFolderToolStripMenuItem.Name = "showInFolderToolStripMenuItem";
            showInFolderToolStripMenuItem.Size = new System.Drawing.Size(301, 32);
            showInFolderToolStripMenuItem.Text = "打开文件夹查看";
            showInFolderToolStripMenuItem.Click += showInFolder_Click;
            // 
            // childBtn_SendTo
            // 
            childBtn_SendTo.CustomBackColor = null;
            childBtn_SendTo.CustomForeColor = null;
            childBtn_SendTo.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            childBtn_SendTo.Image = (System.Drawing.Image)resources.GetObject("childBtn_SendTo.Image");
            childBtn_SendTo.ImageOrigin = null;
            childBtn_SendTo.Name = "childBtn_SendTo";
            childBtn_SendTo.Size = new System.Drawing.Size(301, 32);
            childBtn_SendTo.Text = "发送到";
            childBtn_SendTo.Click += childBtn_SendTo_Click;
            // 
            // childBtn_CopyProperties
            // 
            childBtn_CopyProperties.CustomBackColor = null;
            childBtn_CopyProperties.CustomForeColor = null;
            childBtn_CopyProperties.Image = Properties.Resources.export;
            childBtn_CopyProperties.ImageOrigin = Properties.Resources.export;
            childBtn_CopyProperties.Name = "childBtn_CopyProperties";
            childBtn_CopyProperties.Size = new System.Drawing.Size(248, 32);
            childBtn_CopyProperties.Text = "复制所有属性";
            childBtn_CopyProperties.Click += childBtn_CopyProperties_Click;
            // 
            // childBtn_PastePorpertie
            // 
            childBtn_PastePorpertie.CustomBackColor = null;
            childBtn_PastePorpertie.CustomForeColor = null;
            childBtn_PastePorpertie.Image = Properties.Resources.import;
            childBtn_PastePorpertie.ImageOrigin = Properties.Resources.import;
            childBtn_PastePorpertie.Name = "childBtn_PastePorpertie";
            childBtn_PastePorpertie.Size = new System.Drawing.Size(248, 32);
            childBtn_PastePorpertie.Text = "粘贴所有属性";
            childBtn_PastePorpertie.Click += childBtn_PastePorpertie_Click;
            // 
            // G2DTreeViewDataPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(treeViewControl);
            Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            Name = "G2DTreeViewDataPanel";
            Size = new System.Drawing.Size(500, 1000);
            groupMenu.ResumeLayout(false);
            childMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private G2DTreeViewControl treeViewControl;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem groupBtn_AddNode;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem groupBtn_AddGroup;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem groupBtn_Rename;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem groupBtn_EditAll;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem groupBtn_Balance;
        private DeepEditor.Common.G2D.G2DBaseContextMenuStrip groupMenu;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem childBtn_SetID;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem childBtn_Duplicate;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem childBtn_EditGrid;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem childBtn_Delete;
        private DeepEditor.Common.G2D.G2DBaseContextMenuStrip childMenu;
        private System.Windows.Forms.ImageList imageList;
        private G2DBaseToolStripMenuItem group_Delete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private G2DBaseToolStripMenuItem showInFolderToolStripMenuItem;
        private G2DBaseToolStripMenuItem groupBtn_SendTo;
        private G2DBaseToolStripMenuItem childBtn_SendTo;
        private G2DBaseToolStripMenuItem childBtn_CopyProperties;
        private G2DBaseToolStripMenuItem childBtn_PastePorpertie;
    }
}
