using DeepEditor.Common.G2D;

namespace DeepEditor.Common.EventEditor.AwardEditor
{
    partial class AwardPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AwardPanel));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("临时变量");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("事件开端");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("条件");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("动作");
            eventMenuStrip = new G2DBaseContextMenuStrip(components);
            dataMenu_AddLocalVar = new G2DBaseToolStripMenuItem();
            dataMenu_AddTrigger = new G2DBaseToolStripMenuItem();
            dataMenu_AddCondition = new G2DBaseToolStripMenuItem();
            dataMenu_AddAction = new G2DBaseToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            dataMenu_UP = new G2DBaseToolStripMenuItem();
            dataMenu_Down = new G2DBaseToolStripMenuItem();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            dataMenu_Copy = new G2DBaseToolStripMenuItem();
            dataMenu_Paste = new G2DBaseToolStripMenuItem();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            dataMenu_Delete = new G2DBaseToolStripMenuItem();
            imageList1 = new System.Windows.Forms.ImageList(components);
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            treeView2 = new G2DTreeView();
            txt_EventFunction = new G2DBaseRichTextBox();
            toolStripAward = new G2DBaseToolStrip();
            btn_moveAwardItemUP = new G2DBaseToolStripButton();
            btn_moveAwardItemDown = new G2DBaseToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            btn_Font = new G2DBaseToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            eventMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            toolStripAward.SuspendLayout();
            SuspendLayout();
            // 
            // eventMenuStrip
            // 
            eventMenuStrip.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            eventMenuStrip.CustomBackColor = null;
            eventMenuStrip.CustomForeColor = null;
            eventMenuStrip.Depth = 0;
            eventMenuStrip.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            eventMenuStrip.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            eventMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            eventMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { dataMenu_AddLocalVar, dataMenu_AddTrigger, dataMenu_AddCondition, dataMenu_AddAction, toolStripMenuItem3, dataMenu_UP, dataMenu_Down, toolStripMenuItem5, dataMenu_Copy, dataMenu_Paste, toolStripMenuItem4, dataMenu_Delete });
            eventMenuStrip.MouseState = MaterialSkin.MouseState.HOVER;
            eventMenuStrip.Name = "eventMenuStrip";
            eventMenuStrip.Size = new System.Drawing.Size(197, 292);
            eventMenuStrip.Opening += eventMenuStrip_Opening;
            // 
            // dataMenu_AddLocalVar
            // 
            dataMenu_AddLocalVar.AutoSize = false;
            dataMenu_AddLocalVar.CustomBackColor = null;
            dataMenu_AddLocalVar.CustomForeColor = null;
            dataMenu_AddLocalVar.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            dataMenu_AddLocalVar.Image = (System.Drawing.Image)resources.GetObject("dataMenu_AddLocalVar.Image");
            dataMenu_AddLocalVar.ImageOrigin = null;
            dataMenu_AddLocalVar.Name = "dataMenu_AddLocalVar";
            dataMenu_AddLocalVar.Size = new System.Drawing.Size(156, 30);
            dataMenu_AddLocalVar.Text = "添加临时变量";
            dataMenu_AddLocalVar.Click += dataMenu_AddLocalVar_Click;
            // 
            // dataMenu_AddTrigger
            // 
            dataMenu_AddTrigger.AutoSize = false;
            dataMenu_AddTrigger.CustomBackColor = null;
            dataMenu_AddTrigger.CustomForeColor = null;
            dataMenu_AddTrigger.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            dataMenu_AddTrigger.Image = (System.Drawing.Image)resources.GetObject("dataMenu_AddTrigger.Image");
            dataMenu_AddTrigger.ImageOrigin = null;
            dataMenu_AddTrigger.Name = "dataMenu_AddTrigger";
            dataMenu_AddTrigger.Size = new System.Drawing.Size(156, 30);
            dataMenu_AddTrigger.Text = "添加事件开端";
            dataMenu_AddTrigger.Click += dataMenu_AddTrigger_Click;
            // 
            // dataMenu_AddCondition
            // 
            dataMenu_AddCondition.AutoSize = false;
            dataMenu_AddCondition.CustomBackColor = null;
            dataMenu_AddCondition.CustomForeColor = null;
            dataMenu_AddCondition.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            dataMenu_AddCondition.Image = (System.Drawing.Image)resources.GetObject("dataMenu_AddCondition.Image");
            dataMenu_AddCondition.ImageOrigin = null;
            dataMenu_AddCondition.Name = "dataMenu_AddCondition";
            dataMenu_AddCondition.Size = new System.Drawing.Size(156, 30);
            dataMenu_AddCondition.Text = "添加条件";
            dataMenu_AddCondition.Click += dataMenu_AddCondition_Click;
            // 
            // dataMenu_AddAction
            // 
            dataMenu_AddAction.AutoSize = false;
            dataMenu_AddAction.CustomBackColor = null;
            dataMenu_AddAction.CustomForeColor = null;
            dataMenu_AddAction.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            dataMenu_AddAction.Image = (System.Drawing.Image)resources.GetObject("dataMenu_AddAction.Image");
            dataMenu_AddAction.ImageOrigin = null;
            dataMenu_AddAction.Name = "dataMenu_AddAction";
            dataMenu_AddAction.Size = new System.Drawing.Size(156, 30);
            dataMenu_AddAction.Text = "添加动作";
            dataMenu_AddAction.Click += dataMenu_AddAction_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripMenuItem3.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(193, 6);
            // 
            // dataMenu_UP
            // 
            dataMenu_UP.AutoSize = false;
            dataMenu_UP.CustomBackColor = null;
            dataMenu_UP.CustomForeColor = null;
            dataMenu_UP.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            dataMenu_UP.Image = (System.Drawing.Image)resources.GetObject("dataMenu_UP.Image");
            dataMenu_UP.ImageOrigin = null;
            dataMenu_UP.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            dataMenu_UP.Name = "dataMenu_UP";
            dataMenu_UP.Size = new System.Drawing.Size(156, 30);
            dataMenu_UP.Text = "向上";
            dataMenu_UP.Click += dataMenu_UP_Click;
            // 
            // dataMenu_Down
            // 
            dataMenu_Down.AutoSize = false;
            dataMenu_Down.CustomBackColor = null;
            dataMenu_Down.CustomForeColor = null;
            dataMenu_Down.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            dataMenu_Down.Image = (System.Drawing.Image)resources.GetObject("dataMenu_Down.Image");
            dataMenu_Down.ImageOrigin = null;
            dataMenu_Down.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            dataMenu_Down.Name = "dataMenu_Down";
            dataMenu_Down.Size = new System.Drawing.Size(156, 30);
            dataMenu_Down.Text = "向下";
            dataMenu_Down.Click += dataMenu_Down_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripMenuItem5.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new System.Drawing.Size(193, 6);
            // 
            // dataMenu_Copy
            // 
            dataMenu_Copy.AutoSize = false;
            dataMenu_Copy.CustomBackColor = null;
            dataMenu_Copy.CustomForeColor = null;
            dataMenu_Copy.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            dataMenu_Copy.ImageOrigin = null;
            dataMenu_Copy.Name = "dataMenu_Copy";
            dataMenu_Copy.Size = new System.Drawing.Size(156, 30);
            dataMenu_Copy.Text = "复制";
            dataMenu_Copy.Click += dataMenu_Copy_Click;
            // 
            // dataMenu_Paste
            // 
            dataMenu_Paste.AutoSize = false;
            dataMenu_Paste.CustomBackColor = null;
            dataMenu_Paste.CustomForeColor = null;
            dataMenu_Paste.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            dataMenu_Paste.ImageOrigin = null;
            dataMenu_Paste.Name = "dataMenu_Paste";
            dataMenu_Paste.Size = new System.Drawing.Size(156, 30);
            dataMenu_Paste.Text = "粘贴";
            dataMenu_Paste.Click += dataMenu_Paste_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripMenuItem4.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(193, 6);
            // 
            // dataMenu_Delete
            // 
            dataMenu_Delete.AutoSize = false;
            dataMenu_Delete.CustomBackColor = null;
            dataMenu_Delete.CustomForeColor = null;
            dataMenu_Delete.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            dataMenu_Delete.ImageOrigin = null;
            dataMenu_Delete.Name = "dataMenu_Delete";
            dataMenu_Delete.Size = new System.Drawing.Size(156, 30);
            dataMenu_Delete.Text = "删除";
            dataMenu_Delete.Click += dataMenu_Delete_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "icon_error.png");
            imageList1.Images.SetKeyName(1, "close12.png");
            imageList1.Images.SetKeyName(2, "icon_action.png");
            imageList1.Images.SetKeyName(3, "icon_affect.png");
            imageList1.Images.SetKeyName(4, "icon_camera.png");
            imageList1.Images.SetKeyName(5, "icon_condition.png");
            imageList1.Images.SetKeyName(6, "icon_cpj.png");
            imageList1.Images.SetKeyName(7, "icon_cpj_48.png");
            imageList1.Images.SetKeyName(8, "icon_edit.png");
            imageList1.Images.SetKeyName(9, "icon_edit_48.png");
            imageList1.Images.SetKeyName(10, "icon_error.png");
            imageList1.Images.SetKeyName(11, "icon_event.png");
            imageList1.Images.SetKeyName(12, "icon_grid.png");
            imageList1.Images.SetKeyName(13, "icon_hd.png");
            imageList1.Images.SetKeyName(14, "icon_layer.png");
            imageList1.Images.SetKeyName(15, "icon_quest.ico");
            imageList1.Images.SetKeyName(16, "icon_quest.png");
            imageList1.Images.SetKeyName(17, "icon_quest_condition.png");
            imageList1.Images.SetKeyName(18, "icon_quest_event.png");
            imageList1.Images.SetKeyName(19, "icon_quest_group.png");
            imageList1.Images.SetKeyName(20, "icon_quest_result.png");
            imageList1.Images.SetKeyName(21, "icon_refresh.png");
            imageList1.Images.SetKeyName(22, "icon_res.png");
            imageList1.Images.SetKeyName(23, "icon_res_1.png");
            imageList1.Images.SetKeyName(24, "icon_res_2.png");
            imageList1.Images.SetKeyName(25, "icon_res_3.png");
            imageList1.Images.SetKeyName(26, "icon_res_4.png");
            imageList1.Images.SetKeyName(27, "icon_res_5.png");
            imageList1.Images.SetKeyName(28, "icon_res_6.png");
            imageList1.Images.SetKeyName(29, "icon_res_7.png");
            imageList1.Images.SetKeyName(30, "icon_res_8.png");
            imageList1.Images.SetKeyName(31, "icon_res_9.png");
            imageList1.Images.SetKeyName(32, "icon_run.png");
            imageList1.Images.SetKeyName(33, "icon_scene.ico");
            imageList1.Images.SetKeyName(34, "icon_scene.png");
            imageList1.Images.SetKeyName(35, "icon_scene_graph.png");
            imageList1.Images.SetKeyName(36, "icon_talk.png");
            imageList1.Images.SetKeyName(37, "icon_trigger.png");
            imageList1.Images.SetKeyName(38, "icon_var.ico");
            imageList1.Images.SetKeyName(39, "icons_bar.png");
            imageList1.Images.SetKeyName(40, "icons_tool_bar1.png");
            imageList1.Images.SetKeyName(41, "icons_tool_bar3.png");
            imageList1.Images.SetKeyName(42, "img_item_info.png");
            imageList1.Images.SetKeyName(43, "img_job_trainer.png");
            imageList1.Images.SetKeyName(44, "img_mail.png");
            imageList1.Images.SetKeyName(45, "img_npc_bank.png");
            imageList1.Images.SetKeyName(46, "img_quest_info.png");
            imageList1.Images.SetKeyName(47, "img_quest_info2.png");
            imageList1.Images.SetKeyName(48, "img_script.png");
            imageList1.Images.SetKeyName(49, "img_sell_item.png");
            imageList1.Images.SetKeyName(50, "img_skill_trainer.png");
            imageList1.Images.SetKeyName(51, "img_talk.png");
            imageList1.Images.SetKeyName(52, "img_transport.png");
            imageList1.Images.SetKeyName(53, "light64.png");
            imageList1.Images.SetKeyName(54, "lock.png");
            imageList1.Images.SetKeyName(55, "splash.jpg");
            imageList1.Images.SetKeyName(56, "icon_value.png");
            imageList1.Images.SetKeyName(57, "icons_tool_bar2.png");
            imageList1.Images.SetKeyName(58, "icon_var.png");
            // 
            // splitContainer2
            // 
            splitContainer2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer2.Location = new System.Drawing.Point(0, 35);
            splitContainer2.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer2.Panel1.Controls.Add(treeView2);
            splitContainer2.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer2.Panel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer2.Panel2.Controls.Add(txt_EventFunction);
            splitContainer2.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer2.Panel2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer2.Size = new System.Drawing.Size(1526, 897);
            splitContainer2.SplitterDistance = 584;
            splitContainer2.SplitterWidth = 13;
            splitContainer2.TabIndex = 2;
            // 
            // treeView2
            // 
            treeView2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            treeView2.ContextMenuStrip = eventMenuStrip;
            treeView2.CustomBackColor = null;
            treeView2.CustomForeColor = null;
            treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView2.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            treeView2.EnableCopyPaste = false;
            treeView2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            treeView2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            treeView2.FullRowSelect = true;
            treeView2.HideSelection = false;
            treeView2.ImageIndex = 0;
            treeView2.ImageList = imageList1;
            treeView2.LineColor = System.Drawing.Color.DarkGray;
            treeView2.Location = new System.Drawing.Point(0, 0);
            treeView2.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            treeView2.Name = "treeView2";
            treeNode1.ContextMenuStrip = eventMenuStrip;
            treeNode1.ImageKey = "icon_var.png";
            treeNode1.Name = "RootLocalVar";
            treeNode1.SelectedImageKey = "icon_var.png";
            treeNode1.Text = "临时变量";
            treeNode2.ContextMenuStrip = eventMenuStrip;
            treeNode2.ImageKey = "icon_trigger.png";
            treeNode2.Name = "RootTrigger";
            treeNode2.SelectedImageKey = "icon_trigger.png";
            treeNode2.Text = "事件开端";
            treeNode3.ContextMenuStrip = eventMenuStrip;
            treeNode3.ImageKey = "icon_condition.png";
            treeNode3.Name = "RootCondition";
            treeNode3.SelectedImageKey = "icon_condition.png";
            treeNode3.Text = "条件";
            treeNode4.ContextMenuStrip = eventMenuStrip;
            treeNode4.ImageKey = "icon_run.png";
            treeNode4.Name = "RootAction";
            treeNode4.SelectedImageKey = "icon_run.png";
            treeNode4.Text = "动作";
            treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { treeNode1, treeNode2, treeNode3, treeNode4 });
            treeView2.PathSeparator = "/";
            treeView2.SelectedImageIndex = 0;
            treeView2.Size = new System.Drawing.Size(584, 897);
            treeView2.TabIndex = 0;
            treeView2.AfterSelect += treeView2_AfterSelect;
            treeView2.NodeMouseClick += treeView2_NodeMouseClick;
            treeView2.NodeMouseDoubleClick += treeView2_NodeMouseDoubleClick;
            treeView2.KeyDown += treeView2_KeyDown;
            treeView2.KeyPress += treeView2_KeyPress;
            // 
            // txt_EventFunction
            // 
            txt_EventFunction.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            txt_EventFunction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txt_EventFunction.CustomBackColor = System.Drawing.SystemColors.Control;
            txt_EventFunction.CustomForeColor = System.Drawing.SystemColors.ControlText;
            txt_EventFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            txt_EventFunction.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            txt_EventFunction.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            txt_EventFunction.Location = new System.Drawing.Point(0, 0);
            txt_EventFunction.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            txt_EventFunction.MaxLength = 50000;
            txt_EventFunction.MinimumSize = new System.Drawing.Size(200, 200);
            txt_EventFunction.Name = "txt_EventFunction";
            txt_EventFunction.ReadOnly = true;
            txt_EventFunction.Size = new System.Drawing.Size(929, 897);
            txt_EventFunction.TabIndex = 0;
            txt_EventFunction.Text = "";
            // 
            // toolStripAward
            // 
            toolStripAward.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripAward.CustomBackColor = null;
            toolStripAward.CustomForeColor = null;
            toolStripAward.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            toolStripAward.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            toolStripAward.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripAward.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStripAward.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_moveAwardItemUP, btn_moveAwardItemDown, toolStripSeparator1, btn_Font, toolStripSeparator3 });
            toolStripAward.Location = new System.Drawing.Point(0, 0);
            toolStripAward.Name = "toolStripAward";
            toolStripAward.Size = new System.Drawing.Size(1526, 35);
            toolStripAward.TabIndex = 0;
            toolStripAward.Text = "toolStrip1";
            // 
            // btn_moveAwardItemUP
            // 
            btn_moveAwardItemUP.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_moveAwardItemUP.CustomBackColor = null;
            btn_moveAwardItemUP.CustomForeColor = null;
            btn_moveAwardItemUP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_moveAwardItemUP.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_moveAwardItemUP.Image = (System.Drawing.Image)resources.GetObject("btn_moveAwardItemUP.Image");
            btn_moveAwardItemUP.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_moveAwardItemUP.ImageOrigin");
            btn_moveAwardItemUP.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_moveAwardItemUP.Name = "btn_moveAwardItemUP";
            btn_moveAwardItemUP.Size = new System.Drawing.Size(34, 30);
            btn_moveAwardItemUP.Text = "上移";
            btn_moveAwardItemUP.Click += btn_moveAwardUP_Click;
            // 
            // btn_moveAwardItemDown
            // 
            btn_moveAwardItemDown.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_moveAwardItemDown.CustomBackColor = null;
            btn_moveAwardItemDown.CustomForeColor = null;
            btn_moveAwardItemDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_moveAwardItemDown.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_moveAwardItemDown.Image = (System.Drawing.Image)resources.GetObject("btn_moveAwardItemDown.Image");
            btn_moveAwardItemDown.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_moveAwardItemDown.ImageOrigin");
            btn_moveAwardItemDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_moveAwardItemDown.Name = "btn_moveAwardItemDown";
            btn_moveAwardItemDown.Size = new System.Drawing.Size(34, 30);
            btn_moveAwardItemDown.Text = "下移";
            btn_moveAwardItemDown.Click += btn_moveAwardDown_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
            // 
            // btn_Font
            // 
            btn_Font.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_Font.CustomBackColor = null;
            btn_Font.CustomForeColor = null;
            btn_Font.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btn_Font.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btn_Font.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Font.Image = (System.Drawing.Image)resources.GetObject("btn_Font.Image");
            btn_Font.ImageOrigin = null;
            btn_Font.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Font.Name = "btn_Font";
            btn_Font.Size = new System.Drawing.Size(40, 30);
            btn_Font.Text = "Aa";
            btn_Font.ToolTipText = "改变字体";
            btn_Font.Click += btn_Font_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 35);
            // 
            // AwardPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainer2);
            Controls.Add(toolStripAward);
            Name = "AwardPanel";
            Size = new System.Drawing.Size(1526, 932);
            eventMenuStrip.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            toolStripAward.ResumeLayout(false);
            toolStripAward.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private G2DBaseRichTextBox txt_EventFunction;
        private G2DTreeView treeView2;
        private System.Windows.Forms.ImageList imageList1;
        private G2DBaseContextMenuStrip eventMenuStrip;
        private G2DBaseToolStripMenuItem dataMenu_AddTrigger;
        private G2DBaseToolStripMenuItem dataMenu_AddAction;
        private G2DBaseToolStripMenuItem dataMenu_Delete;
        private G2DBaseToolStripMenuItem dataMenu_AddCondition;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private G2DBaseToolStripMenuItem dataMenu_Copy;
        private G2DBaseToolStripMenuItem dataMenu_Paste;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private G2DBaseToolStripMenuItem dataMenu_AddLocalVar;
        private G2DBaseToolStripMenuItem dataMenu_UP;
        private G2DBaseToolStripMenuItem dataMenu_Down;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private G2DBaseToolStrip toolStripAward;
        private G2DBaseToolStripButton btn_moveAwardItemUP;
        private G2DBaseToolStripButton btn_moveAwardItemDown;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private G2DBaseToolStripButton btn_Font;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}