namespace DeepEditor.Common.G2D.DataGrid
{
    partial class G2DFieldEditor
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
            propertyGrid1 = new G2DPropertyGrid();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            buttonClear = new G2DBaseButton();
            buttonCancel = new G2DBaseButton();
            buttonOK = new G2DBaseButton();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // propertyGrid1
            // 
            propertyGrid1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            propertyGrid1.CategoryForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            propertyGrid1.CategorySplitterColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            propertyGrid1.CommandsBackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            propertyGrid1.CommandsBorderColor = System.Drawing.Color.FromArgb(50, 50, 50);
            propertyGrid1.CommandsForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            propertyGrid1.CustomBackColor = null;
            propertyGrid1.CustomForeColor = null;
            propertyGrid1.DescriptionAreaHeight = 88;
            propertyGrid1.DescriptionAreaLineCount = 4;
            propertyGrid1.DisabledItemForeColor = System.Drawing.Color.Gray;
            propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            propertyGrid1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            propertyGrid1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            propertyGrid1.HelpBackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            propertyGrid1.HelpBorderColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            propertyGrid1.HelpForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            propertyGrid1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            propertyGrid1.LineColor = System.Drawing.Color.FromArgb(80, 80, 80);
            propertyGrid1.Location = new System.Drawing.Point(0, 0);
            propertyGrid1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            propertyGrid1.MinDescriptionAreaLineCount = 5;
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.SelectedElementDesc = null;
            propertyGrid1.SelectedField = null;
            propertyGrid1.SelectedFieldDesc = null;
            propertyGrid1.SelectedRootObject = null;
            propertyGrid1.Size = new System.Drawing.Size(1321, 818);
            propertyGrid1.TabIndex = 0;
            propertyGrid1.ViewBackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            propertyGrid1.ViewBorderColor = System.Drawing.Color.FromArgb(50, 50, 50);
            propertyGrid1.ViewForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            propertyGrid1.Click += propertyGrid1_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            splitContainer1.Location = new System.Drawing.Point(5, 34);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            splitContainer1.Panel1.Controls.Add(propertyGrid1);
            splitContainer1.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            splitContainer1.Panel2.Controls.Add(buttonClear);
            splitContainer1.Panel2.Controls.Add(buttonCancel);
            splitContainer1.Panel2.Controls.Add(buttonOK);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            splitContainer1.Size = new System.Drawing.Size(1321, 953);
            splitContainer1.SplitterDistance = 818;
            splitContainer1.TabIndex = 1;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
            // 
            // buttonClear
            // 
            buttonClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonClear.AutoSize = false;
            buttonClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            buttonClear.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            buttonClear.CustomBackColor = null;
            buttonClear.CustomForeColor = null;
            buttonClear.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonClear.Depth = 0;
            buttonClear.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonClear.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            buttonClear.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            buttonClear.HighEmphasis = true;
            buttonClear.Icon = null;
            buttonClear.Location = new System.Drawing.Point(27, 53);
            buttonClear.MouseState = MaterialSkin.MouseState.HOVER;
            buttonClear.Name = "buttonClear";
            buttonClear.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonClear.Size = new System.Drawing.Size(119, 54);
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
            buttonCancel.Location = new System.Drawing.Point(1061, 53);
            buttonCancel.MouseState = MaterialSkin.MouseState.HOVER;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonCancel.Size = new System.Drawing.Size(119, 54);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "Cancel";
            buttonCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonCancel.UseAccentColor = false;
            buttonCancel.UseVisualStyleBackColor = false;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonOK
            // 
            buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
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
            buttonOK.Location = new System.Drawing.Point(1187, 53);
            buttonOK.MouseState = MaterialSkin.MouseState.HOVER;
            buttonOK.Name = "buttonOK";
            buttonOK.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonOK.Size = new System.Drawing.Size(119, 54);
            buttonOK.TabIndex = 0;
            buttonOK.Text = "OK";
            buttonOK.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonOK.UseAccentColor = false;
            buttonOK.UseVisualStyleBackColor = false;
            buttonOK.Click += buttonOK_Click;
            // 
            // G2DFieldEditor
            // 
            AcceptButton = buttonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(1329, 990);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "G2DFieldEditor";
            Padding = new System.Windows.Forms.Padding(5, 34, 3, 3);
            Text = "FieldEditor";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DeepEditor.Common.G2D.DataGrid.G2DPropertyGrid propertyGrid1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DeepEditor.Common.G2D.G2DBaseButton buttonCancel;
        private DeepEditor.Common.G2D.G2DBaseButton buttonOK;
        private DeepEditor.Common.G2D.G2DBaseButton buttonClear;
    }
}