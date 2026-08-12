using DeepEditor.Common.G2D;

namespace DeepEditor.Common.EventEditor
{
    partial class EventEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventEditor));
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            treeViewEvents = new G2DTreeViewControl();
            imageList1 = new System.Windows.Forms.ImageList(components);
            toolStripEdit = new G2DBaseToolStrip();
            tool_Edit = new G2DBaseToolStripDropDownButton();
            tool_Save = new System.Windows.Forms.ToolStripMenuItem();
            btn_Copy = new G2DBaseToolStripMenuItem();
            btn_Paste = new G2DBaseToolStripMenuItem();
            btn_CopyToClipboard = new G2DBaseToolStripMenuItem();
            btn_Delete = new G2DBaseToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            btn_Save = new System.Windows.Forms.ToolStripButton();
            btn_EnvVars = new G2DBaseToolStripButton();
            btn_Run = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            tabControlEvent = new MaterialSkin.Controls.MaterialTabControl();
            tabPage4 = new System.Windows.Forms.TabPage();
            behaviorPanel1 = new BehaviorEditor.BehaviorPanel();
            tabPage1 = new System.Windows.Forms.TabPage();
            awardPanel1 = new AwardEditor.AwardPanel();
            tabPage2 = new System.Windows.Forms.TabPage();
            txt_EventComment = new G2DBaseRichTextBox();
            tabPage3 = new System.Windows.Forms.TabPage();
            chk_EnableEvent = new G2DBaseCheckBox();
            label4 = new G2DBaseLabel();
            materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            groupMenuStrip = new G2DBaseContextMenuStrip(components);
            menu_AddZoneEvent = new G2DBaseToolStripMenuItem();
            menu_AddGroup = new G2DBaseToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            menu_Enable = new G2DBaseToolStripMenuItem();
            menu_RenameZoneEvent = new G2DBaseToolStripMenuItem();
            menu_CopyZoneEvent = new G2DBaseToolStripMenuItem();
            menu_ParseZoneEvent = new G2DBaseToolStripMenuItem();
            menu_DeleteZoneEvent = new G2DBaseToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            menu_ConvertToBehavior = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            menu_OpenAll = new G2DBaseToolStripMenuItem();
            menu_CloseAll = new G2DBaseToolStripMenuItem();
            menu_DeleteAll = new G2DBaseToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            toolStripEdit.SuspendLayout();
            tabControlEvent.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            groupMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Location = new System.Drawing.Point(5, 34);
            splitContainer1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel1.Controls.Add(treeViewEvents);
            splitContainer1.Panel1.Controls.Add(toolStripEdit);
            splitContainer1.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel2.Controls.Add(tabControlEvent);
            splitContainer1.Panel2.Controls.Add(materialTabSelector1);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Size = new System.Drawing.Size(2976, 1374);
            splitContainer1.SplitterDistance = 401;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 1;
            // 
            // treeViewEvents
            // 
            treeViewEvents.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            treeViewEvents.CheckBoxes = true;
            treeViewEvents.CustomBackColor = null;
            treeViewEvents.CustomForeColor = null;
            treeViewEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewEvents.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            treeViewEvents.EnableCopyPaste = true;
            treeViewEvents.EnableDragDrop = true;
            treeViewEvents.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            treeViewEvents.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            treeViewEvents.FullRowSelect = true;
            treeViewEvents.HideSelection = false;
            treeViewEvents.ImageKey = "icon_Group";
            treeViewEvents.ImageList = imageList1;
            treeViewEvents.ItemHeight = 32;
            treeViewEvents.LineColor = System.Drawing.Color.DarkGray;
            treeViewEvents.Location = new System.Drawing.Point(0, 33);
            treeViewEvents.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            treeViewEvents.Name = "treeViewEvents";
            treeViewEvents.SelectedImageKey = "icon_Group";
            treeViewEvents.SelectedNode = null;
            treeViewEvents.ShowNodeToolTips = true;
            treeViewEvents.Size = new System.Drawing.Size(401, 1341);
            treeViewEvents.TabIndex = 0;
            treeViewEvents.TreeViewNodeSorter = null;
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
            imageList1.Images.SetKeyName(59, "Question.png");
            // 
            // toolStripEdit
            // 
            toolStripEdit.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripEdit.CustomBackColor = null;
            toolStripEdit.CustomForeColor = null;
            toolStripEdit.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            toolStripEdit.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            toolStripEdit.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripEdit.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStripEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tool_Edit, toolStripSeparator2, btn_Save, btn_EnvVars, btn_Run, toolStripSeparator4 });
            toolStripEdit.Location = new System.Drawing.Point(0, 0);
            toolStripEdit.Name = "toolStripEdit";
            toolStripEdit.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            toolStripEdit.Size = new System.Drawing.Size(401, 33);
            toolStripEdit.TabIndex = 0;
            toolStripEdit.Text = "toolStrip2";
            // 
            // tool_Edit
            // 
            tool_Edit.AutoSize = false;
            tool_Edit.CustomBackColor = null;
            tool_Edit.CustomForeColor = null;
            tool_Edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tool_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tool_Save, btn_Copy, btn_Paste, btn_CopyToClipboard, btn_Delete });
            tool_Edit.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_Edit.ImageOrigin = null;
            tool_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            tool_Edit.Name = "tool_Edit";
            tool_Edit.Size = new System.Drawing.Size(50, 28);
            tool_Edit.Text = "编辑";
            // 
            // tool_Save
            // 
            tool_Save.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_Save.Image = (System.Drawing.Image)resources.GetObject("tool_Save.Image");
            tool_Save.Name = "tool_Save";
            tool_Save.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            tool_Save.Size = new System.Drawing.Size(284, 34);
            tool_Save.Text = "保存";
            tool_Save.Click += btn_Save_Click;
            // 
            // btn_Copy
            // 
            btn_Copy.AutoSize = false;
            btn_Copy.CustomBackColor = null;
            btn_Copy.CustomForeColor = null;
            btn_Copy.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Copy.ImageOrigin = null;
            btn_Copy.Name = "btn_Copy";
            btn_Copy.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            btn_Copy.Size = new System.Drawing.Size(254, 24);
            btn_Copy.Text = "复制";
            btn_Copy.Click += btn_Copy_Click;
            // 
            // btn_Paste
            // 
            btn_Paste.AutoSize = false;
            btn_Paste.CustomBackColor = null;
            btn_Paste.CustomForeColor = null;
            btn_Paste.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Paste.ImageOrigin = null;
            btn_Paste.Name = "btn_Paste";
            btn_Paste.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            btn_Paste.Size = new System.Drawing.Size(254, 24);
            btn_Paste.Text = "粘贴";
            btn_Paste.Click += btn_Paste_Click;
            // 
            // btn_CopyToClipboard
            // 
            btn_CopyToClipboard.AutoSize = false;
            btn_CopyToClipboard.CustomBackColor = null;
            btn_CopyToClipboard.CustomForeColor = null;
            btn_CopyToClipboard.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_CopyToClipboard.ImageOrigin = null;
            btn_CopyToClipboard.Name = "btn_CopyToClipboard";
            btn_CopyToClipboard.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.C;
            btn_CopyToClipboard.Size = new System.Drawing.Size(254, 24);
            btn_CopyToClipboard.Text = "拷贝作为文本";
            btn_CopyToClipboard.Click += btn_CopyToClipboard_Click;
            // 
            // btn_Delete
            // 
            btn_Delete.AutoSize = false;
            btn_Delete.CustomBackColor = null;
            btn_Delete.CustomForeColor = null;
            btn_Delete.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Delete.ImageOrigin = null;
            btn_Delete.Name = "btn_Delete";
            btn_Delete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            btn_Delete.Size = new System.Drawing.Size(254, 24);
            btn_Delete.Text = "删除";
            btn_Delete.Click += btn_Delete_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // btn_Save
            // 
            btn_Save.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_Save.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Save.Image = (System.Drawing.Image)resources.GetObject("btn_Save.Image");
            btn_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Save.Name = "btn_Save";
            btn_Save.Size = new System.Drawing.Size(34, 28);
            btn_Save.Text = "Save";
            btn_Save.Click += btn_Save_Click;
            // 
            // btn_EnvVars
            // 
            btn_EnvVars.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_EnvVars.CustomBackColor = null;
            btn_EnvVars.CustomForeColor = null;
            btn_EnvVars.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_EnvVars.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_EnvVars.Image = (System.Drawing.Image)resources.GetObject("btn_EnvVars.Image");
            btn_EnvVars.ImageOrigin = null;
            btn_EnvVars.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_EnvVars.Name = "btn_EnvVars";
            btn_EnvVars.Size = new System.Drawing.Size(34, 28);
            btn_EnvVars.Text = "环境变量";
            btn_EnvVars.Click += btn_EnvVars_Click;
            // 
            // btn_Run
            // 
            btn_Run.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_Run.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_Run.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Run.Image = (System.Drawing.Image)resources.GetObject("btn_Run.Image");
            btn_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Run.Name = "btn_Run";
            btn_Run.Size = new System.Drawing.Size(34, 28);
            btn_Run.Text = "运行测试";
            btn_Run.Click += btn_Run_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripSeparator4.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 33);
            // 
            // tabControlEvent
            // 
            tabControlEvent.Controls.Add(tabPage4);
            tabControlEvent.Controls.Add(tabPage1);
            tabControlEvent.Controls.Add(tabPage2);
            tabControlEvent.Controls.Add(tabPage3);
            tabControlEvent.Depth = 0;
            tabControlEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlEvent.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            tabControlEvent.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tabControlEvent.Location = new System.Drawing.Point(0, 45);
            tabControlEvent.MouseState = MaterialSkin.MouseState.HOVER;
            tabControlEvent.Multiline = true;
            tabControlEvent.Name = "tabControlEvent";
            tabControlEvent.SelectedIndex = 0;
            tabControlEvent.Size = new System.Drawing.Size(2570, 1329);
            tabControlEvent.TabIndex = 2;
            // 
            // tabPage4
            // 
            tabPage4.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            tabPage4.Controls.Add(behaviorPanel1);
            tabPage4.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tabPage4.Location = new System.Drawing.Point(4, 29);
            tabPage4.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            tabPage4.Size = new System.Drawing.Size(2562, 1296);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "行为";
            // 
            // behaviorPanel1
            // 
            behaviorPanel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            behaviorPanel1.CustomBackColor = null;
            behaviorPanel1.CustomForeColor = null;
            behaviorPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            behaviorPanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            behaviorPanel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            behaviorPanel1.Location = new System.Drawing.Point(3, 1);
            behaviorPanel1.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            behaviorPanel1.Name = "behaviorPanel1";
            behaviorPanel1.Size = new System.Drawing.Size(2556, 1294);
            behaviorPanel1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            tabPage1.Controls.Add(awardPanel1);
            tabPage1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tabPage1.Location = new System.Drawing.Point(4, 29);
            tabPage1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            tabPage1.Size = new System.Drawing.Size(2563, 1295);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "代码";
            // 
            // awardPanel1
            // 
            awardPanel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            awardPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            awardPanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            awardPanel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            awardPanel1.Location = new System.Drawing.Point(5, 4);
            awardPanel1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            awardPanel1.Name = "awardPanel1";
            awardPanel1.Size = new System.Drawing.Size(2553, 1287);
            awardPanel1.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            tabPage2.Controls.Add(txt_EventComment);
            tabPage2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tabPage2.Location = new System.Drawing.Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            tabPage2.Size = new System.Drawing.Size(2563, 1295);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "注释";
            // 
            // txt_EventComment
            // 
            txt_EventComment.AcceptsTab = true;
            txt_EventComment.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            txt_EventComment.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txt_EventComment.CustomBackColor = null;
            txt_EventComment.CustomForeColor = null;
            txt_EventComment.Dock = System.Windows.Forms.DockStyle.Fill;
            txt_EventComment.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            txt_EventComment.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            txt_EventComment.Location = new System.Drawing.Point(3, 3);
            txt_EventComment.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            txt_EventComment.MaxLength = 50;
            txt_EventComment.Multiline = false;
            txt_EventComment.Name = "txt_EventComment";
            txt_EventComment.Size = new System.Drawing.Size(2557, 1289);
            txt_EventComment.TabIndex = 0;
            txt_EventComment.Text = "";
            txt_EventComment.TextChanged += txt_EventComment_TextChanged;
            // 
            // tabPage3
            // 
            tabPage3.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            tabPage3.Controls.Add(chk_EnableEvent);
            tabPage3.Controls.Add(label4);
            tabPage3.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tabPage3.Location = new System.Drawing.Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new System.Drawing.Size(2563, 1295);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "杂项";
            // 
            // chk_EnableEvent
            // 
            chk_EnableEvent.AutoSize = true;
            chk_EnableEvent.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            chk_EnableEvent.CustomBackColor = null;
            chk_EnableEvent.CustomForeColor = null;
            chk_EnableEvent.Depth = 0;
            chk_EnableEvent.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            chk_EnableEvent.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            chk_EnableEvent.Location = new System.Drawing.Point(16, 14);
            chk_EnableEvent.Margin = new System.Windows.Forms.Padding(0);
            chk_EnableEvent.MouseLocation = new System.Drawing.Point(-1, -1);
            chk_EnableEvent.MouseState = MaterialSkin.MouseState.HOVER;
            chk_EnableEvent.Name = "chk_EnableEvent";
            chk_EnableEvent.ReadOnly = false;
            chk_EnableEvent.Ripple = true;
            chk_EnableEvent.Size = new System.Drawing.Size(67, 37);
            chk_EnableEvent.TabIndex = 1;
            chk_EnableEvent.Text = "开启";
            chk_EnableEvent.UseVisualStyleBackColor = false;
            chk_EnableEvent.CheckedChanged += chk_EnableEvent_CheckedChanged;
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label4.AutoSize = true;
            label4.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            label4.CustomBackColor = null;
            label4.CustomForeColor = null;
            label4.Depth = 0;
            label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label4.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            label4.Location = new System.Drawing.Point(1098, 79);
            label4.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            label4.MouseState = MaterialSkin.MouseState.HOVER;
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(107, 21);
            label4.TabIndex = 3;
            label4.Text = "延迟执行(毫秒)";
            // 
            // materialTabSelector1
            // 
            materialTabSelector1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            materialTabSelector1.BaseTabControl = tabControlEvent;
            materialTabSelector1.CharacterCasing = MaterialSkin.Controls.MaterialTabSelector.CustomCharacterCasing.Normal;
            materialTabSelector1.Depth = 0;
            materialTabSelector1.Dock = System.Windows.Forms.DockStyle.Top;
            materialTabSelector1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialTabSelector1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            materialTabSelector1.Location = new System.Drawing.Point(0, 0);
            materialTabSelector1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            materialTabSelector1.Name = "materialTabSelector1";
            materialTabSelector1.Size = new System.Drawing.Size(2570, 45);
            materialTabSelector1.TabIndex = 3;
            materialTabSelector1.Text = "materialTabSelector1";
            // 
            // groupMenuStrip
            // 
            groupMenuStrip.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            groupMenuStrip.CustomBackColor = null;
            groupMenuStrip.CustomForeColor = null;
            groupMenuStrip.Depth = 0;
            groupMenuStrip.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            groupMenuStrip.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            groupMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            groupMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menu_AddZoneEvent, menu_AddGroup, toolStripMenuItem1, menu_Enable, menu_RenameZoneEvent, menu_CopyZoneEvent, menu_ParseZoneEvent, menu_DeleteZoneEvent, toolStripMenuItem3, menu_ConvertToBehavior, toolStripMenuItem2, menu_OpenAll, menu_CloseAll, menu_DeleteAll });
            groupMenuStrip.MouseState = MaterialSkin.MouseState.HOVER;
            groupMenuStrip.Name = "groupMenuStrip";
            groupMenuStrip.Size = new System.Drawing.Size(249, 387);
            groupMenuStrip.Opening += groupMenuStrip_Opening;
            // 
            // menu_AddZoneEvent
            // 
            menu_AddZoneEvent.AutoSize = false;
            menu_AddZoneEvent.CustomBackColor = null;
            menu_AddZoneEvent.CustomForeColor = null;
            menu_AddZoneEvent.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_AddZoneEvent.Image = (System.Drawing.Image)resources.GetObject("menu_AddZoneEvent.Image");
            menu_AddZoneEvent.ImageOrigin = null;
            menu_AddZoneEvent.Name = "menu_AddZoneEvent";
            menu_AddZoneEvent.Size = new System.Drawing.Size(168, 30);
            menu_AddZoneEvent.Text = "添加事件";
            menu_AddZoneEvent.Click += menu_AddZoneEvent_Click;
            // 
            // menu_AddGroup
            // 
            menu_AddGroup.AutoSize = false;
            menu_AddGroup.CustomBackColor = null;
            menu_AddGroup.CustomForeColor = null;
            menu_AddGroup.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_AddGroup.Image = (System.Drawing.Image)resources.GetObject("menu_AddGroup.Image");
            menu_AddGroup.ImageOrigin = null;
            menu_AddGroup.Name = "menu_AddGroup";
            menu_AddGroup.Size = new System.Drawing.Size(168, 30);
            menu_AddGroup.Text = "添加过滤器类别";
            menu_AddGroup.Click += menu_AddGroupToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripMenuItem1.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(245, 6);
            // 
            // menu_Enable
            // 
            menu_Enable.AutoSize = false;
            menu_Enable.Checked = true;
            menu_Enable.CheckOnClick = true;
            menu_Enable.CheckState = System.Windows.Forms.CheckState.Checked;
            menu_Enable.CustomBackColor = null;
            menu_Enable.CustomForeColor = null;
            menu_Enable.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_Enable.ImageOrigin = null;
            menu_Enable.Name = "menu_Enable";
            menu_Enable.Size = new System.Drawing.Size(168, 30);
            menu_Enable.Text = "开启";
            menu_Enable.CheckStateChanged += menu_EnableItem_CheckStateChanged;
            // 
            // menu_RenameZoneEvent
            // 
            menu_RenameZoneEvent.AutoSize = false;
            menu_RenameZoneEvent.CustomBackColor = null;
            menu_RenameZoneEvent.CustomForeColor = null;
            menu_RenameZoneEvent.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_RenameZoneEvent.ImageOrigin = null;
            menu_RenameZoneEvent.Name = "menu_RenameZoneEvent";
            menu_RenameZoneEvent.Size = new System.Drawing.Size(168, 30);
            menu_RenameZoneEvent.Text = "重命名";
            menu_RenameZoneEvent.Click += menu_RenameZoneEvent_Click;
            // 
            // menu_CopyZoneEvent
            // 
            menu_CopyZoneEvent.AutoSize = false;
            menu_CopyZoneEvent.CustomBackColor = null;
            menu_CopyZoneEvent.CustomForeColor = null;
            menu_CopyZoneEvent.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_CopyZoneEvent.ImageOrigin = null;
            menu_CopyZoneEvent.Name = "menu_CopyZoneEvent";
            menu_CopyZoneEvent.Size = new System.Drawing.Size(168, 30);
            menu_CopyZoneEvent.Text = "复制";
            menu_CopyZoneEvent.Click += menu_CopyZoneEvent_Click;
            // 
            // menu_ParseZoneEvent
            // 
            menu_ParseZoneEvent.AutoSize = false;
            menu_ParseZoneEvent.CustomBackColor = null;
            menu_ParseZoneEvent.CustomForeColor = null;
            menu_ParseZoneEvent.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_ParseZoneEvent.ImageOrigin = null;
            menu_ParseZoneEvent.Name = "menu_ParseZoneEvent";
            menu_ParseZoneEvent.Size = new System.Drawing.Size(168, 30);
            menu_ParseZoneEvent.Text = "粘贴";
            menu_ParseZoneEvent.Click += menu_ParseZoneEvent_Click;
            // 
            // menu_DeleteZoneEvent
            // 
            menu_DeleteZoneEvent.AutoSize = false;
            menu_DeleteZoneEvent.CustomBackColor = null;
            menu_DeleteZoneEvent.CustomForeColor = null;
            menu_DeleteZoneEvent.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_DeleteZoneEvent.ImageOrigin = null;
            menu_DeleteZoneEvent.Name = "menu_DeleteZoneEvent";
            menu_DeleteZoneEvent.Size = new System.Drawing.Size(168, 30);
            menu_DeleteZoneEvent.Text = "删除";
            menu_DeleteZoneEvent.Click += menu_DeleteZoneEvent_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripMenuItem3.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(245, 6);
            // 
            // menu_ConvertToBehavior
            // 
            menu_ConvertToBehavior.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_ConvertToBehavior.Name = "menu_ConvertToBehavior";
            menu_ConvertToBehavior.Size = new System.Drawing.Size(248, 32);
            menu_ConvertToBehavior.Text = "转换为行为树";
            menu_ConvertToBehavior.Click += menu_ConvertToBehavior_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripMenuItem2.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(245, 6);
            // 
            // menu_OpenAll
            // 
            menu_OpenAll.AutoSize = false;
            menu_OpenAll.CustomBackColor = null;
            menu_OpenAll.CustomForeColor = null;
            menu_OpenAll.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_OpenAll.ImageOrigin = null;
            menu_OpenAll.Name = "menu_OpenAll";
            menu_OpenAll.Size = new System.Drawing.Size(168, 30);
            menu_OpenAll.Text = "全部启用";
            menu_OpenAll.Click += menu_OpenAll_Click;
            // 
            // menu_CloseAll
            // 
            menu_CloseAll.AutoSize = false;
            menu_CloseAll.CustomBackColor = null;
            menu_CloseAll.CustomForeColor = null;
            menu_CloseAll.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_CloseAll.ImageOrigin = null;
            menu_CloseAll.Name = "menu_CloseAll";
            menu_CloseAll.Size = new System.Drawing.Size(168, 30);
            menu_CloseAll.Text = "全部禁用";
            menu_CloseAll.Click += menu_CloseAll_Click;
            // 
            // menu_DeleteAll
            // 
            menu_DeleteAll.AutoSize = false;
            menu_DeleteAll.CustomBackColor = null;
            menu_DeleteAll.CustomForeColor = null;
            menu_DeleteAll.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu_DeleteAll.ImageOrigin = null;
            menu_DeleteAll.Name = "menu_DeleteAll";
            menu_DeleteAll.Size = new System.Drawing.Size(168, 30);
            menu_DeleteAll.Text = "全部删除";
            menu_DeleteAll.Click += menu_DeleteAll_Click;
            // 
            // EventEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(2986, 1412);
            Controls.Add(splitContainer1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            Name = "EventEditor";
            Padding = new System.Windows.Forms.Padding(5, 34, 5, 4);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "SceneEventEditor";
            Load += EventEditor_Load;
            Validating += EventEditor_Validating;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            toolStripEdit.ResumeLayout(false);
            toolStripEdit.PerformLayout();
            tabControlEvent.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            groupMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private G2DBaseToolStrip toolStripEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private G2DTreeViewControl treeViewEvents;
        private G2DBaseRichTextBox txt_EventComment;
        private G2DBaseCheckBox chk_EnableEvent;
        private System.Windows.Forms.ImageList imageList1;
        private G2DBaseContextMenuStrip groupMenuStrip;
        private G2DBaseToolStripMenuItem menu_AddZoneEvent;
        private G2DBaseToolStripMenuItem menu_AddGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private G2DBaseToolStripMenuItem menu_CopyZoneEvent;
        private G2DBaseToolStripMenuItem menu_ParseZoneEvent;
        private G2DBaseToolStripMenuItem menu_DeleteZoneEvent;
        private G2DBaseToolStripMenuItem menu_RenameZoneEvent;
        private G2DBaseToolStripDropDownButton tool_Edit;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private G2DBaseLabel label4;
        public G2DBaseToolStripMenuItem btn_CopyToClipboard;
        public G2DBaseToolStripMenuItem btn_Copy;
        public G2DBaseToolStripMenuItem btn_Paste;
        public G2DBaseToolStripMenuItem btn_Delete;
        private G2DBaseToolStripMenuItem menu_OpenAll;
        private G2DBaseToolStripMenuItem menu_CloseAll;
        private G2DBaseToolStripMenuItem menu_DeleteAll;
        private MaterialSkin.Controls.MaterialTabControl tabControlEvent;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private G2DBaseToolStripMenuItem menu_Enable;
        private G2DBaseToolStripButton btn_EnvVars;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private System.Windows.Forms.TabPage tabPage4;
        private AwardEditor.AwardPanel awardPanel1;
        public System.Windows.Forms.ToolStripButton btn_Save;
        public System.Windows.Forms.ToolStripMenuItem tool_Save;
        private DeepEditor.Common.EventEditor.BehaviorEditor.BehaviorPanel behaviorPanel1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem menu_ConvertToBehavior;
        public System.Windows.Forms.ToolStripButton btn_Run;
    }
}