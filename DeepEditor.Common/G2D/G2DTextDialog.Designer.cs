namespace DeepEditor.Common.G2D
{
    partial class G2DTextDialog
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
            textBox1 = new G2DBaseTextBox();
            buttonOkey = new G2DBaseButton();
            buttonCancel = new G2DBaseButton();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
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
            textBox1.HideSelection = false;
            textBox1.Location = new System.Drawing.Point(0, 0);
            textBox1.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            textBox1.MaxLength = 50;
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(954, 191);
            textBox1.TabIndex = 0;
            // 
            // buttonOkey
            // 
            buttonOkey.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonOkey.AutoSize = false;
            buttonOkey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            buttonOkey.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            buttonOkey.CustomBackColor = null;
            buttonOkey.CustomForeColor = null;
            buttonOkey.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonOkey.Depth = 0;
            buttonOkey.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOkey.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            buttonOkey.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            buttonOkey.HighEmphasis = true;
            buttonOkey.Icon = null;
            buttonOkey.Location = new System.Drawing.Point(785, 40);
            buttonOkey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            buttonOkey.MouseState = MaterialSkin.MouseState.HOVER;
            buttonOkey.Name = "buttonOkey";
            buttonOkey.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonOkey.Size = new System.Drawing.Size(145, 51);
            buttonOkey.TabIndex = 1;
            buttonOkey.Text = "OK";
            buttonOkey.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonOkey.UseAccentColor = false;
            buttonOkey.UseVisualStyleBackColor = false;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonCancel.AutoSize = false;
            buttonCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            buttonCancel.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            buttonCancel.CustomBackColor = null;
            buttonCancel.CustomForeColor = null;
            buttonCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonCancel.Depth = 0;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            buttonCancel.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            buttonCancel.HighEmphasis = true;
            buttonCancel.Icon = null;
            buttonCancel.Location = new System.Drawing.Point(618, 40);
            buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            buttonCancel.MouseState = MaterialSkin.MouseState.HOVER;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonCancel.Size = new System.Drawing.Size(145, 51);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "Cancel";
            buttonCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonCancel.UseAccentColor = false;
            buttonCancel.UseVisualStyleBackColor = false;
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
            splitContainer1.Panel2.Controls.Add(buttonOkey);
            splitContainer1.Panel2.Controls.Add(buttonCancel);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Size = new System.Drawing.Size(954, 302);
            splitContainer1.SplitterDistance = 191;
            splitContainer1.TabIndex = 5;
            // 
            // G2DTextDialog
            // 
            AcceptButton = buttonOkey;
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(962, 339);
            Controls.Add(splitContainer1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "G2DTextDialog";
            Padding = new System.Windows.Forms.Padding(5, 34, 3, 3);
            Text = "输入名称";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DeepEditor.Common.G2D.G2DBaseTextBox textBox1;
        private DeepEditor.Common.G2D.G2DBaseButton buttonOkey;
        private DeepEditor.Common.G2D.G2DBaseButton buttonCancel;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}