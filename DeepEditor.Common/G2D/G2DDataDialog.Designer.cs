namespace DeepEditor.Common.G2D
{
    partial class G2DDataDialog
    {      /// <summary>
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            buttonOK = new G2DBaseButton();
            buttonCancel = new G2DBaseButton();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            g2DPropertyGrid1 = new DataGrid.G2DPropertyGrid();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
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
            buttonOK.Location = new System.Drawing.Point(690, 88);
            buttonOK.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            buttonOK.MouseState = MaterialSkin.MouseState.HOVER;
            buttonOK.Name = "buttonOK";
            buttonOK.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonOK.Size = new System.Drawing.Size(111, 53);
            buttonOK.TabIndex = 1;
            buttonOK.Text = "OK";
            buttonOK.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonOK.UseAccentColor = false;
            buttonOK.UseVisualStyleBackColor = false;
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
            buttonCancel.Location = new System.Drawing.Point(541, 88);
            buttonCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            buttonCancel.MouseState = MaterialSkin.MouseState.HOVER;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonCancel.Size = new System.Drawing.Size(111, 53);
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
            splitContainer1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            splitContainer1.Location = new System.Drawing.Point(3, 24);
            splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel1.Controls.Add(g2DPropertyGrid1);
            splitContainer1.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel2.Controls.Add(buttonOK);
            splitContainer1.Panel2.Controls.Add(buttonCancel);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            splitContainer1.Size = new System.Drawing.Size(827, 651);
            splitContainer1.SplitterDistance = 482;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 5;
            // 
            // g2DPropertyGrid1
            // 
            g2DPropertyGrid1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            g2DPropertyGrid1.CategoryForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            g2DPropertyGrid1.CategorySplitterColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            g2DPropertyGrid1.CommandsBackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            g2DPropertyGrid1.CommandsBorderColor = System.Drawing.Color.FromArgb(242, 242, 242);
            g2DPropertyGrid1.CommandsForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            g2DPropertyGrid1.CustomBackColor = null;
            g2DPropertyGrid1.CustomForeColor = null;
            g2DPropertyGrid1.DescriptionAreaHeight = 59;
            g2DPropertyGrid1.DescriptionAreaLineCount = 2;
            g2DPropertyGrid1.DisabledItemForeColor = System.Drawing.Color.Gray;
            g2DPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            g2DPropertyGrid1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            g2DPropertyGrid1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            g2DPropertyGrid1.HelpBackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            g2DPropertyGrid1.HelpBorderColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            g2DPropertyGrid1.HelpForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            g2DPropertyGrid1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            g2DPropertyGrid1.LineColor = System.Drawing.Color.LightGray;
            g2DPropertyGrid1.Location = new System.Drawing.Point(0, 0);
            g2DPropertyGrid1.Margin = new System.Windows.Forms.Padding(2);
            g2DPropertyGrid1.MinDescriptionAreaLineCount = 5;
            g2DPropertyGrid1.Name = "g2DPropertyGrid1";
            g2DPropertyGrid1.Size = new System.Drawing.Size(827, 482);
            g2DPropertyGrid1.TabIndex = 0;
            g2DPropertyGrid1.ViewBackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            g2DPropertyGrid1.ViewBorderColor = System.Drawing.Color.FromArgb(242, 242, 242);
            g2DPropertyGrid1.ViewForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            // 
            // G2DDataDialog
            // 
            AcceptButton = buttonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(832, 677);
            Controls.Add(splitContainer1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            Name = "G2DDataDialog";
            Padding = new System.Windows.Forms.Padding(3, 24, 2, 2);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "设置";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion


        private DeepEditor.Common.G2D.G2DBaseButton buttonOK;
        private DeepEditor.Common.G2D.G2DBaseButton buttonCancel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DataGrid.G2DPropertyGrid g2DPropertyGrid1;
    }
}