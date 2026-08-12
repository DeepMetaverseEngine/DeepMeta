namespace DeepEditor.Common.G2D
{
    partial class G2DEnumValueDialog
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
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            listView1 = new G2DBaseListView();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            listView2 = new G2DBaseListView();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            btn_OK = new G2DBaseButton();
            btn_Cancel = new G2DBaseButton();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Location = new System.Drawing.Point(12, 12);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel1.Controls.Add(listView1);
            splitContainer1.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel2.Controls.Add(listView2);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Size = new System.Drawing.Size(1087, 663);
            splitContainer1.SplitterDistance = 483;
            splitContainer1.SplitterWidth = 8;
            splitContainer1.TabIndex = 0;
            // 
            // listView1
            // 
            listView1.AutoSizeTable = true;
            listView1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader2, columnHeader1 });
            listView1.CustomBackColor = null;
            listView1.CustomForeColor = null;
            listView1.Depth = 0;
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            listView1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            listView1.FullRowSelect = true;
            listView1.Location = new System.Drawing.Point(0, 0);
            listView1.MinimumSize = new System.Drawing.Size(200, 100);
            listView1.MouseLocation = new System.Drawing.Point(-1, -1);
            listView1.MouseState = MaterialSkin.MouseState.OUT;
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.OwnerDraw = true;
            listView1.Size = new System.Drawing.Size(483, 663);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "描述";
            columnHeader2.Width = 120;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "枚举";
            columnHeader1.Width = 200;
            // 
            // listView2
            // 
            listView2.AutoSizeTable = true;
            listView2.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            listView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader4, columnHeader5, columnHeader3 });
            listView2.CustomBackColor = null;
            listView2.CustomForeColor = null;
            listView2.Depth = 0;
            listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            listView2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            listView2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            listView2.FullRowSelect = true;
            listView2.Location = new System.Drawing.Point(0, 0);
            listView2.MinimumSize = new System.Drawing.Size(200, 100);
            listView2.MouseLocation = new System.Drawing.Point(-1, -1);
            listView2.MouseState = MaterialSkin.MouseState.OUT;
            listView2.MultiSelect = false;
            listView2.Name = "listView2";
            listView2.OwnerDraw = true;
            listView2.Size = new System.Drawing.Size(596, 663);
            listView2.TabIndex = 1;
            listView2.UseCompatibleStateImageBehavior = false;
            listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "描述";
            columnHeader4.Width = 200;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "值";
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "枚举";
            columnHeader3.Width = 200;
            // 
            // btn_OK
            // 
            btn_OK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btn_OK.AutoSize = false;
            btn_OK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btn_OK.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_OK.CustomBackColor = null;
            btn_OK.CustomForeColor = null;
            btn_OK.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btn_OK.Depth = 0;
            btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btn_OK.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btn_OK.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_OK.HighEmphasis = true;
            btn_OK.Icon = null;
            btn_OK.Location = new System.Drawing.Point(921, 699);
            btn_OK.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btn_OK.MouseState = MaterialSkin.MouseState.HOVER;
            btn_OK.Name = "btn_OK";
            btn_OK.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_OK.Size = new System.Drawing.Size(177, 54);
            btn_OK.TabIndex = 1;
            btn_OK.Text = "OK";
            btn_OK.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btn_OK.UseAccentColor = false;
            btn_OK.UseVisualStyleBackColor = false;
            // 
            // btn_Cancel
            // 
            btn_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btn_Cancel.AutoSize = false;
            btn_Cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btn_Cancel.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_Cancel.CustomBackColor = null;
            btn_Cancel.CustomForeColor = null;
            btn_Cancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btn_Cancel.Depth = 0;
            btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btn_Cancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btn_Cancel.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_Cancel.HighEmphasis = true;
            btn_Cancel.Icon = null;
            btn_Cancel.Location = new System.Drawing.Point(717, 699);
            btn_Cancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btn_Cancel.MouseState = MaterialSkin.MouseState.HOVER;
            btn_Cancel.Name = "btn_Cancel";
            btn_Cancel.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_Cancel.Size = new System.Drawing.Size(177, 54);
            btn_Cancel.TabIndex = 2;
            btn_Cancel.Text = "Cancel";
            btn_Cancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btn_Cancel.UseAccentColor = false;
            btn_Cancel.UseVisualStyleBackColor = false;
            // 
            // G2DEnumValueDialog
            // 
            AcceptButton = btn_OK;
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btn_Cancel;
            ClientSize = new System.Drawing.Size(1111, 768);
            Controls.Add(btn_Cancel);
            Controls.Add(btn_OK);
            Controls.Add(splitContainer1);
            Name = "G2DEnumValueDialog";
            Text = "G2DEnumValueDialog";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private G2DBaseButton btn_OK;
        private G2DBaseButton btn_Cancel;
        private G2DBaseListView listView1;
        private G2DBaseListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}