namespace DeepEditor.Common.G2D.DataGrid
{
    partial class G2DXmlEditor
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
            text = new System.Windows.Forms.RichTextBox();
            buttonClear = new G2DBaseButton();
            buttonCancel = new G2DBaseButton();
            buttonOK = new G2DBaseButton();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            splitContainer1.Location = new System.Drawing.Point(3, 24);
            splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel1.Controls.Add(text);
            splitContainer1.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel2.Controls.Add(buttonClear);
            splitContainer1.Panel2.Controls.Add(buttonCancel);
            splitContainer1.Panel2.Controls.Add(buttonOK);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            splitContainer1.Size = new System.Drawing.Size(841, 420);
            splitContainer1.SplitterDistance = 326;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 1;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
            // 
            // text
            // 
            text.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            text.Dock = System.Windows.Forms.DockStyle.Fill;
            text.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            text.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            text.Location = new System.Drawing.Point(0, 0);
            text.Name = "text";
            text.Size = new System.Drawing.Size(841, 326);
            text.TabIndex = 0;
            text.Text = "";
            // 
            // buttonClear
            // 
            buttonClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonClear.AutoSize = false;
            buttonClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            buttonClear.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            buttonClear.CustomBackColor = null;
            buttonClear.CustomForeColor = null;
            buttonClear.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonClear.Depth = 0;
            buttonClear.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonClear.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            buttonClear.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            buttonClear.HighEmphasis = true;
            buttonClear.Icon = null;
            buttonClear.Location = new System.Drawing.Point(13, 38);
            buttonClear.Margin = new System.Windows.Forms.Padding(2);
            buttonClear.MouseState = MaterialSkin.MouseState.HOVER;
            buttonClear.Name = "buttonClear";
            buttonClear.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonClear.Size = new System.Drawing.Size(76, 38);
            buttonClear.TabIndex = 2;
            buttonClear.Text = "清除";
            buttonClear.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonClear.UseAccentColor = false;
            buttonClear.UseVisualStyleBackColor = false;
            buttonClear.Click += buttonClear_Click;
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
            buttonCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            buttonCancel.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            buttonCancel.HighEmphasis = true;
            buttonCancel.Icon = null;
            buttonCancel.Location = new System.Drawing.Point(676, 38);
            buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            buttonCancel.MouseState = MaterialSkin.MouseState.HOVER;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonCancel.Size = new System.Drawing.Size(76, 38);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "Cancel";
            buttonCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonCancel.UseAccentColor = false;
            buttonCancel.UseVisualStyleBackColor = false;
            // 
            // buttonOK
            // 
            buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonOK.AutoSize = false;
            buttonOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            buttonOK.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            buttonOK.CustomBackColor = null;
            buttonOK.CustomForeColor = null;
            buttonOK.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonOK.Depth = 0;
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOK.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            buttonOK.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            buttonOK.HighEmphasis = true;
            buttonOK.Icon = null;
            buttonOK.Location = new System.Drawing.Point(756, 38);
            buttonOK.Margin = new System.Windows.Forms.Padding(2);
            buttonOK.MouseState = MaterialSkin.MouseState.HOVER;
            buttonOK.Name = "buttonOK";
            buttonOK.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonOK.Size = new System.Drawing.Size(76, 38);
            buttonOK.TabIndex = 0;
            buttonOK.Text = "OK";
            buttonOK.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonOK.UseAccentColor = false;
            buttonOK.UseVisualStyleBackColor = false;
            buttonOK.Click += buttonOK_Click;
            // 
            // G2DXmlEditor
            // 
            AcceptButton = buttonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(847, 447);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            Name = "G2DXmlEditor";
            Text = "Json Editor";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DeepEditor.Common.G2D.G2DBaseButton buttonCancel;
        private DeepEditor.Common.G2D.G2DBaseButton buttonOK;
        private DeepEditor.Common.G2D.G2DBaseButton buttonClear;
        private System.Windows.Forms.RichTextBox text;
    }
}