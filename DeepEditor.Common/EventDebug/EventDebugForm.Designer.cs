namespace DeepEditor.Common.EventDebug
{
    partial class EventDebugForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventDebugForm));
            ST.Library.UI.NodeEditor.STNodeDrawing stNodeDrawing1 = new ST.Library.UI.NodeEditor.STNodeDrawing();
            g2dBaseToolStrip1 = new G2D.G2DBaseToolStrip();
            menu = new System.Windows.Forms.ToolStripDropDownButton();
            btn_RunTool = new System.Windows.Forms.ToolStripMenuItem();
            btn_StepTool = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            btn_AlwaysTop = new G2D.G2DBaseToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            btn_Play = new G2D.G2DBaseToolStripButton();
            btn_Step = new G2D.G2DBaseToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            treeView1 = new G2D.G2DTreeView();
            behaviorNodeEditor1 = new EventEditor.BehaviorEditor.BehaviorNodeEditor();
            imageList1 = new System.Windows.Forms.ImageList(components);
            timer1 = new System.Windows.Forms.Timer(components);
            g2dBaseToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // g2dBaseToolStrip1
            // 
            g2dBaseToolStrip1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            g2dBaseToolStrip1.CustomBackColor = null;
            g2dBaseToolStrip1.CustomForeColor = null;
            g2dBaseToolStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            g2dBaseToolStrip1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            g2dBaseToolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            g2dBaseToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menu, toolStripSeparator3, btn_AlwaysTop, toolStripSeparator1, btn_Play, btn_Step, toolStripSeparator2 });
            g2dBaseToolStrip1.Location = new System.Drawing.Point(0, 0);
            g2dBaseToolStrip1.Name = "g2dBaseToolStrip1";
            g2dBaseToolStrip1.Size = new System.Drawing.Size(1752, 33);
            g2dBaseToolStrip1.TabIndex = 0;
            g2dBaseToolStrip1.Text = "g2dBaseToolStrip1";
            // 
            // menu
            // 
            menu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_RunTool, btn_StepTool });
            menu.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            menu.Image = (System.Drawing.Image)resources.GetObject("menu.Image");
            menu.ImageTransparentColor = System.Drawing.Color.Magenta;
            menu.Name = "menu";
            menu.Size = new System.Drawing.Size(67, 28);
            menu.Text = "Menu";
            // 
            // btn_RunTool
            // 
            btn_RunTool.CheckOnClick = true;
            btn_RunTool.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_RunTool.Name = "btn_RunTool";
            btn_RunTool.ShortcutKeys = System.Windows.Forms.Keys.F9;
            btn_RunTool.Size = new System.Drawing.Size(270, 34);
            btn_RunTool.Text = "Run/Pause";
            // 
            // btn_StepTool
            // 
            btn_StepTool.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_StepTool.Name = "btn_StepTool";
            btn_StepTool.ShortcutKeys = System.Windows.Forms.Keys.F10;
            btn_StepTool.Size = new System.Drawing.Size(270, 34);
            btn_StepTool.Text = "Step";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // btn_AlwaysTop
            // 
            btn_AlwaysTop.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_AlwaysTop.CheckOnClick = true;
            btn_AlwaysTop.CustomBackColor = null;
            btn_AlwaysTop.CustomForeColor = null;
            btn_AlwaysTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_AlwaysTop.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_AlwaysTop.Image = (System.Drawing.Image)resources.GetObject("btn_AlwaysTop.Image");
            btn_AlwaysTop.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_AlwaysTop.ImageOrigin");
            btn_AlwaysTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_AlwaysTop.Name = "btn_AlwaysTop";
            btn_AlwaysTop.Size = new System.Drawing.Size(34, 28);
            btn_AlwaysTop.Text = "总在最前";
            btn_AlwaysTop.Click += btn_AlwaysTop_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // btn_Play
            // 
            btn_Play.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_Play.CheckOnClick = true;
            btn_Play.CustomBackColor = null;
            btn_Play.CustomForeColor = null;
            btn_Play.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_Play.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Play.Image = (System.Drawing.Image)resources.GetObject("btn_Play.Image");
            btn_Play.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_Play.ImageOrigin");
            btn_Play.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Play.Name = "btn_Play";
            btn_Play.Size = new System.Drawing.Size(34, 28);
            btn_Play.Text = "Play";
            // 
            // btn_Step
            // 
            btn_Step.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_Step.CustomBackColor = null;
            btn_Step.CustomForeColor = null;
            btn_Step.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_Step.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Step.Image = (System.Drawing.Image)resources.GetObject("btn_Step.Image");
            btn_Step.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_Step.ImageOrigin");
            btn_Step.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Step.Name = "btn_Step";
            btn_Step.Size = new System.Drawing.Size(34, 28);
            btn_Step.Text = "Step (F10)";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Location = new System.Drawing.Point(0, 33);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel1.Controls.Add(treeView1);
            splitContainer1.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel2.Controls.Add(behaviorNodeEditor1);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Size = new System.Drawing.Size(1752, 1046);
            splitContainer1.SplitterDistance = 452;
            splitContainer1.TabIndex = 1;
            // 
            // treeView1
            // 
            treeView1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            treeView1.CustomBackColor = null;
            treeView1.CustomForeColor = null;
            treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView1.EnableCopyPaste = false;
            treeView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            treeView1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            treeView1.LineColor = System.Drawing.Color.DarkGray;
            treeView1.Location = new System.Drawing.Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new System.Drawing.Size(452, 1046);
            treeView1.TabIndex = 0;
            // 
            // behaviorNodeEditor1
            // 
            behaviorNodeEditor1.AllowDrop = true;
            behaviorNodeEditor1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            behaviorNodeEditor1.Curvature = 0.3F;
            behaviorNodeEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            behaviorNodeEditor1.DragMoveDistance = 5;
            behaviorNodeEditor1.Drawing = stNodeDrawing1;
            behaviorNodeEditor1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            behaviorNodeEditor1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            behaviorNodeEditor1.icon_action = null;
            behaviorNodeEditor1.icon_condition = null;
            behaviorNodeEditor1.icon_question = null;
            behaviorNodeEditor1.icon_trigger = null;
            behaviorNodeEditor1.icon_value = null;
            behaviorNodeEditor1.icon_var = null;
            behaviorNodeEditor1.IsReadOnly = false;
            behaviorNodeEditor1.Location = new System.Drawing.Point(0, 0);
            behaviorNodeEditor1.LocationBackColor = System.Drawing.Color.FromArgb(120, 0, 0, 0);
            behaviorNodeEditor1.MarkBackColor = System.Drawing.Color.FromArgb(180, 0, 0, 0);
            behaviorNodeEditor1.MarkForeColor = System.Drawing.Color.FromArgb(180, 0, 0, 0);
            behaviorNodeEditor1.MinimumSize = new System.Drawing.Size(100, 100);
            behaviorNodeEditor1.Name = "behaviorNodeEditor1";
            behaviorNodeEditor1.ScaleMax = 10F;
            behaviorNodeEditor1.ScaleMin = 0.1F;
            behaviorNodeEditor1.Size = new System.Drawing.Size(1296, 1046);
            behaviorNodeEditor1.TabIndex = 0;
            behaviorNodeEditor1.Text = "behaviorNodeEditor1";
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
            imageList1.Images.SetKeyName(60, "event_2558944.png");
            imageList1.Images.SetKeyName(61, "icon_common_67.png");
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 10;
            timer1.Tick += timer1_Tick;
            // 
            // EventDebugForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1752, 1079);
            Controls.Add(splitContainer1);
            Controls.Add(g2dBaseToolStrip1);
            Name = "EventDebugForm";
            Text = "EventDebugForm";
            Load += EventDebugForm_Load;
            g2dBaseToolStrip1.ResumeLayout(false);
            g2dBaseToolStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private G2D.G2DBaseToolStrip g2dBaseToolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private G2D.G2DTreeView treeView1;
        private EventEditor.BehaviorEditor.BehaviorNodeEditor behaviorNodeEditor1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
        private G2D.G2DBaseToolStripButton btn_AlwaysTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public G2D.G2DBaseToolStripButton btn_Step;
        public G2D.G2DBaseToolStripButton btn_Play;
        private System.Windows.Forms.ToolStripDropDownButton menu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripMenuItem btn_StepTool;
        public System.Windows.Forms.ToolStripMenuItem btn_RunTool;
    }
}