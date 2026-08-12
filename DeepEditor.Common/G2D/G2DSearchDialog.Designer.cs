namespace DeepEditor.Common.G2D
{
    partial class G2DSearchDialog
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new G2DBaseRichTextBox();
            button_next = new G2DBaseButton();
            button_prev = new G2DBaseButton();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            button_close = new G2DBaseButton();
            button_find = new G2DBaseButton();
            statusStrip1 = new G2DBaseStatusStrip();
            lbl_status = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox1.CustomBackColor = null;
            textBox1.CustomForeColor = null;
            textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            textBox1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            textBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            textBox1.Location = new System.Drawing.Point(0, 0);
            textBox1.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(993, 197);
            textBox1.TabIndex = 0;
            textBox1.Text = "";
            // 
            // button_next
            // 
            button_next.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button_next.AutoSize = false;
            button_next.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button_next.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            button_next.CustomBackColor = null;
            button_next.CustomForeColor = null;
            button_next.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            button_next.Depth = 0;
            button_next.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            button_next.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            button_next.HighEmphasis = true;
            button_next.Icon = null;
            button_next.Location = new System.Drawing.Point(562, 68);
            button_next.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button_next.MouseState = MaterialSkin.MouseState.HOVER;
            button_next.Name = "button_next";
            button_next.NoAccentTextColor = System.Drawing.Color.Empty;
            button_next.Size = new System.Drawing.Size(162, 51);
            button_next.TabIndex = 1;
            button_next.Text = "下一个  (F3)";
            button_next.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            button_next.UseAccentColor = false;
            button_next.UseVisualStyleBackColor = false;
            button_next.Click += button_next_Click;
            // 
            // button_prev
            // 
            button_prev.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button_prev.AutoSize = false;
            button_prev.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button_prev.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            button_prev.CustomBackColor = null;
            button_prev.CustomForeColor = null;
            button_prev.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            button_prev.Depth = 0;
            button_prev.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            button_prev.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            button_prev.HighEmphasis = true;
            button_prev.Icon = null;
            button_prev.Location = new System.Drawing.Point(328, 68);
            button_prev.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button_prev.MouseState = MaterialSkin.MouseState.HOVER;
            button_prev.Name = "button_prev";
            button_prev.NoAccentTextColor = System.Drawing.Color.Empty;
            button_prev.Size = new System.Drawing.Size(228, 51);
            button_prev.TabIndex = 2;
            button_prev.Text = "上一个(Shift+F3)";
            button_prev.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            button_prev.UseAccentColor = false;
            button_prev.UseVisualStyleBackColor = false;
            button_prev.Click += button_prev_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Location = new System.Drawing.Point(5, 34);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel1.Controls.Add(textBox1);
            splitContainer1.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel2.Controls.Add(button_close);
            splitContainer1.Panel2.Controls.Add(button_find);
            splitContainer1.Panel2.Controls.Add(button_next);
            splitContainer1.Panel2.Controls.Add(button_prev);
            splitContainer1.Panel2.Controls.Add(statusStrip1);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Size = new System.Drawing.Size(993, 369);
            splitContainer1.SplitterDistance = 197;
            splitContainer1.TabIndex = 5;
            // 
            // button_close
            // 
            button_close.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            button_close.AutoSize = false;
            button_close.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button_close.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            button_close.CustomBackColor = null;
            button_close.CustomForeColor = null;
            button_close.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            button_close.Depth = 0;
            button_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            button_close.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            button_close.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            button_close.HighEmphasis = true;
            button_close.Icon = null;
            button_close.Location = new System.Drawing.Point(22, 68);
            button_close.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button_close.MouseState = MaterialSkin.MouseState.HOVER;
            button_close.Name = "button_close";
            button_close.NoAccentTextColor = System.Drawing.Color.Empty;
            button_close.Size = new System.Drawing.Size(148, 51);
            button_close.TabIndex = 4;
            button_close.Text = "关闭 (ESC)";
            button_close.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            button_close.UseAccentColor = false;
            button_close.UseVisualStyleBackColor = false;
            button_close.Click += button_close_Click;
            // 
            // button_find
            // 
            button_find.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button_find.AutoSize = false;
            button_find.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button_find.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            button_find.CustomBackColor = null;
            button_find.CustomForeColor = null;
            button_find.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            button_find.Depth = 0;
            button_find.DialogResult = System.Windows.Forms.DialogResult.OK;
            button_find.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            button_find.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            button_find.HighEmphasis = true;
            button_find.Icon = null;
            button_find.Location = new System.Drawing.Point(796, 68);
            button_find.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button_find.MouseState = MaterialSkin.MouseState.HOVER;
            button_find.Name = "button_find";
            button_find.NoAccentTextColor = System.Drawing.Color.Empty;
            button_find.Size = new System.Drawing.Size(171, 51);
            button_find.TabIndex = 3;
            button_find.Text = "确定(Enter)";
            button_find.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            button_find.UseAccentColor = false;
            button_find.UseVisualStyleBackColor = false;
            button_find.Click += button_find_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            statusStrip1.CustomBackColor = null;
            statusStrip1.CustomForeColor = null;
            statusStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            statusStrip1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lbl_status });
            statusStrip1.Location = new System.Drawing.Point(0, 146);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 14, 0);
            statusStrip1.Size = new System.Drawing.Size(993, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // lbl_status
            // 
            lbl_status.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            lbl_status.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            lbl_status.Name = "lbl_status";
            lbl_status.Size = new System.Drawing.Size(0, 15);
            // 
            // G2DSearchDialog
            // 
            AcceptButton = button_find;
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = button_close;
            ClientSize = new System.Drawing.Size(1001, 406);
            Controls.Add(splitContainer1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "G2DSearchDialog";
            Padding = new System.Windows.Forms.Padding(5, 34, 3, 3);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "查找";
            FormClosed += G2DSearchDialog_FormClosed;
            Shown += G2DSearchDialog_Shown;
            KeyDown += G2DSearchDialog_KeyDown;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DeepEditor.Common.G2D.G2DBaseRichTextBox textBox1;
        private DeepEditor.Common.G2D.G2DBaseButton button_next;
        private DeepEditor.Common.G2D.G2DBaseButton button_prev;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DeepEditor.Common.G2D.G2DBaseButton button_find;
        private DeepEditor.Common.G2D.G2DBaseButton button_close;
        private DeepEditor.Common.G2D.G2DBaseStatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbl_status;
    }
}