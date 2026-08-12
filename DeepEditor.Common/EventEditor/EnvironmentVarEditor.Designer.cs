namespace DeepEditor.Common.EventEditor
{
    partial class EnvironmentVarEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnvironmentVarEditor));
            listView1 = new G2D.G2DBaseListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            imageList1 = new System.Windows.Forms.ImageList(components);
            toolStrip1 = new G2D.G2DBaseToolStrip();
            btn_Add = new G2D.G2DBaseToolStripButton();
            btn_Remove = new G2D.G2DBaseToolStripButton();
            btn_Edit = new G2D.G2DBaseToolStripButton();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.AutoSizeTable = true;
            listView1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            listView1.CustomBackColor = null;
            listView1.CustomForeColor = null;
            listView1.Depth = 0;
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.Font = new System.Drawing.Font("微软雅黑", 9F);
            listView1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            listView1.FullRowSelect = true;
            listView1.LargeImageList = imageList1;
            listView1.Location = new System.Drawing.Point(9, 89);
            listView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            listView1.MinimumSize = new System.Drawing.Size(314, 141);
            listView1.MouseLocation = new System.Drawing.Point(-1, -1);
            listView1.MouseState = MaterialSkin.MouseState.OUT;
            listView1.Name = "listView1";
            listView1.OwnerDraw = true;
            listView1.Size = new System.Drawing.Size(904, 513);
            listView1.SmallImageList = imageList1;
            listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            listView1.StateImageList = imageList1;
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.MouseDoubleClick += listView1_MouseDoubleClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "变量名";
            columnHeader1.Width = 130;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "类型";
            columnHeader2.Width = 124;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "默认值";
            columnHeader3.Width = 127;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "网络协议";
            columnHeader4.Width = 100;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "touch_marker.png");
            imageList1.Images.SetKeyName(1, "refresh.png");
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStrip1.CustomBackColor = null;
            toolStrip1.CustomForeColor = null;
            toolStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            toolStrip1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_Add, btn_Remove, btn_Edit });
            toolStrip1.Location = new System.Drawing.Point(9, 56);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            toolStrip1.Size = new System.Drawing.Size(904, 33);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // btn_Add
            // 
            btn_Add.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_Add.CustomBackColor = null;
            btn_Add.CustomForeColor = null;
            btn_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_Add.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Add.Image = (System.Drawing.Image)resources.GetObject("btn_Add.Image");
            btn_Add.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_Add.ImageOrigin");
            btn_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Add.Name = "btn_Add";
            btn_Add.Size = new System.Drawing.Size(34, 28);
            btn_Add.Text = "添加";
            btn_Add.Click += btn_Add_Click;
            // 
            // btn_Remove
            // 
            btn_Remove.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_Remove.CustomBackColor = null;
            btn_Remove.CustomForeColor = null;
            btn_Remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_Remove.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Remove.Image = (System.Drawing.Image)resources.GetObject("btn_Remove.Image");
            btn_Remove.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_Remove.ImageOrigin");
            btn_Remove.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Remove.Name = "btn_Remove";
            btn_Remove.Size = new System.Drawing.Size(34, 28);
            btn_Remove.Text = "删除";
            btn_Remove.Click += btn_Remove_Click;
            // 
            // btn_Edit
            // 
            btn_Edit.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_Edit.CustomBackColor = null;
            btn_Edit.CustomForeColor = null;
            btn_Edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_Edit.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Edit.Image = (System.Drawing.Image)resources.GetObject("btn_Edit.Image");
            btn_Edit.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_Edit.ImageOrigin");
            btn_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Edit.Name = "btn_Edit";
            btn_Edit.Size = new System.Drawing.Size(34, 28);
            btn_Edit.Text = "编辑变量";
            btn_Edit.Click += btn_Edit_Click;
            // 
            // EnvironmentVarEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(922, 610);
            Controls.Add(listView1);
            Controls.Add(toolStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "EnvironmentVarEditor";
            Padding = new System.Windows.Forms.Padding(9, 56, 9, 8);
            Text = "编辑场景变量";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private DeepEditor.Common.G2D.G2DBaseToolStrip toolStrip1;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton btn_Add;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton btn_Remove;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton btn_Edit;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ImageList imageList1;
        private DeepEditor.Common.G2D.G2DBaseListView listView1;
    }
}