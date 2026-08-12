namespace DeepEditor.Common.EventEditor.DescAttributeEdit
{
    partial class ValueTypeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValueTypeDialog));
            groupBox1 = new System.Windows.Forms.GroupBox();
            valuePanel1 = new ValuePropertyPanel();
            buttonOK = new DeepEditor.Common.G2D.G2DBaseButton();
            panel1 = new System.Windows.Forms.Panel();
            buttonCancel = new DeepEditor.Common.G2D.G2DBaseButton();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            treeView1 = new ValueTypesTreeViewControl();
            imageList1 = new System.Windows.Forms.ImageList(components);
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            groupBox1.Controls.Add(valuePanel1);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            groupBox1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox1.Size = new System.Drawing.Size(1087, 733);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "事件文本";
            // 
            // valuePanel1
            // 
            valuePanel1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            valuePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            valuePanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            valuePanel1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            valuePanel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            valuePanel1.Location = new System.Drawing.Point(3, 22);
            valuePanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            valuePanel1.Name = "valuePanel1";
            valuePanel1.Size = new System.Drawing.Size(1081, 707);
            valuePanel1.TabIndex = 0;
            // 
            // buttonOK
            // 
            buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonOK.AutoSize = false;
            buttonOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            buttonOK.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            buttonOK.CustomBackColor = null;
            buttonOK.CustomForeColor = null;
            buttonOK.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonOK.Depth = 0;
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOK.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            buttonOK.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            buttonOK.HighEmphasis = true;
            buttonOK.Icon = null;
            buttonOK.Location = new System.Drawing.Point(1633, 8);
            buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            buttonOK.MouseState = MaterialSkin.MouseState.HOVER;
            buttonOK.Name = "buttonOK";
            buttonOK.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonOK.Size = new System.Drawing.Size(152, 61);
            buttonOK.TabIndex = 4;
            buttonOK.Text = "OK";
            buttonOK.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonOK.UseAccentColor = false;
            buttonOK.UseVisualStyleBackColor = false;
            buttonOK.Click += buttonOK_Click;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            panel1.Controls.Add(buttonCancel);
            panel1.Controls.Add(buttonOK);
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            panel1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            panel1.Location = new System.Drawing.Point(5, 823);
            panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1801, 78);
            panel1.TabIndex = 7;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonCancel.AutoSize = false;
            buttonCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            buttonCancel.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            buttonCancel.CustomBackColor = null;
            buttonCancel.CustomForeColor = null;
            buttonCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonCancel.Depth = 0;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            buttonCancel.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            buttonCancel.HighEmphasis = true;
            buttonCancel.Icon = null;
            buttonCancel.Location = new System.Drawing.Point(1419, 8);
            buttonCancel.MouseState = MaterialSkin.MouseState.HOVER;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonCancel.Size = new System.Drawing.Size(152, 61);
            buttonCancel.TabIndex = 5;
            buttonCancel.Text = "Cancel";
            buttonCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonCancel.UseAccentColor = false;
            buttonCancel.UseVisualStyleBackColor = false;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // splitContainer2
            // 
            splitContainer2.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer2.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            splitContainer2.Location = new System.Drawing.Point(5, 90);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            splitContainer2.Panel1.Controls.Add(treeView1);
            splitContainer2.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer2.Panel1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            splitContainer2.Panel2.Controls.Add(groupBox1);
            splitContainer2.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer2.Panel2.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            splitContainer2.Size = new System.Drawing.Size(1801, 733);
            splitContainer2.SplitterDistance = 709;
            splitContainer2.SplitterWidth = 5;
            splitContainer2.TabIndex = 8;
            // 
            // treeView1
            // 
            treeView1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            treeView1.CheckBoxes = true;
            treeView1.CustomBackColor = null;
            treeView1.CustomForeColor = null;
            treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            treeView1.EnableCopyPaste = false;
            treeView1.EnableDragDrop = false;
            treeView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            treeView1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            treeView1.FullRowSelect = true;
            treeView1.HideSelection = false;
            treeView1.ImageKey = "icons_tool_bar2.png";
            treeView1.ImageList = imageList1;
            treeView1.ItemHeight = 32;
            treeView1.LineColor = System.Drawing.Color.Gray;
            treeView1.Location = new System.Drawing.Point(0, 0);
            treeView1.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            treeView1.Name = "treeView1";
            treeView1.SelectedImageKey = "icons_tool_bar2.png";
            treeView1.SelectedNode = null;
            treeView1.ShowNodeToolTips = true;
            treeView1.Size = new System.Drawing.Size(709, 733);
            treeView1.TabIndex = 0;
            treeView1.TreeViewNodeSorter = null;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
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
            imageList1.Images.SetKeyName(38, "icons_bar.png");
            imageList1.Images.SetKeyName(39, "icons_tool_bar1.png");
            imageList1.Images.SetKeyName(40, "icons_tool_bar3.png");
            imageList1.Images.SetKeyName(41, "img_item_info.png");
            imageList1.Images.SetKeyName(42, "img_job_trainer.png");
            imageList1.Images.SetKeyName(43, "img_mail.png");
            imageList1.Images.SetKeyName(44, "img_npc_bank.png");
            imageList1.Images.SetKeyName(45, "img_quest_info.png");
            imageList1.Images.SetKeyName(46, "img_quest_info2.png");
            imageList1.Images.SetKeyName(47, "img_script.png");
            imageList1.Images.SetKeyName(48, "img_sell_item.png");
            imageList1.Images.SetKeyName(49, "img_skill_trainer.png");
            imageList1.Images.SetKeyName(50, "img_talk.png");
            imageList1.Images.SetKeyName(51, "img_transport.png");
            imageList1.Images.SetKeyName(52, "light64.png");
            imageList1.Images.SetKeyName(53, "lock.png");
            imageList1.Images.SetKeyName(54, "splash.jpg");
            imageList1.Images.SetKeyName(55, "icon_value.png");
            imageList1.Images.SetKeyName(56, "icons_tool_bar2.png");
            imageList1.Images.SetKeyName(57, "icon_var.png");
            // 
            // ValueTypeDialog
            // 
            AcceptButton = buttonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(1809, 904);
            Controls.Add(splitContainer2);
            Controls.Add(panel1);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "ValueTypeDialog";
            Padding = new System.Windows.Forms.Padding(5, 90, 3, 3);
            Text = "FormTriggers";
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private DeepEditor.Common.G2D.G2DBaseButton buttonOK;
        private System.Windows.Forms.Panel panel1;
        private ValuePropertyPanel valuePanel1;
        private DeepEditor.Common.G2D.G2DBaseButton buttonCancel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private ValueTypesTreeViewControl treeView1;
        private System.Windows.Forms.ImageList imageList1;
    }
}