namespace DeepEditor.Common.G2D
{
    partial class G2DTypeFieldDialog
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
            comboBox1 = new G2DBaseComboBox();
            label1 = new G2DBaseLabel();
            listBox1 = new G2DBaseListBox();
            label2 = new G2DBaseLabel();
            btn_OK = new G2DBaseButton();
            btn_Cancel = new G2DBaseButton();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBox1.AutoResize = false;
            comboBox1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            comboBox1.CustomBackColor = null;
            comboBox1.CustomForeColor = null;
            comboBox1.Depth = 0;
            comboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            comboBox1.DropDownHeight = 174;
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.DropDownWidth = 121;
            comboBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            comboBox1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.IntegralHeight = false;
            comboBox1.ItemHeight = 43;
            comboBox1.Location = new System.Drawing.Point(96, 28);
            comboBox1.Margin = new System.Windows.Forms.Padding(4);
            comboBox1.MaxDropDownItems = 4;
            comboBox1.MouseState = MaterialSkin.MouseState.OUT;
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(603, 49);
            comboBox1.StartIndex = 0;
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            label1.CustomBackColor = null;
            label1.CustomForeColor = null;
            label1.Depth = 0;
            label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            label1.Location = new System.Drawing.Point(14, 28);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.MouseState = MaterialSkin.MouseState.HOVER;
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(65, 21);
            label1.TabIndex = 1;
            label1.Text = "实体类型";
            // 
            // listBox1
            // 
            listBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listBox1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            listBox1.CustomBackColor = null;
            listBox1.CustomForeColor = null;
            listBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            listBox1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new System.Drawing.Point(96, 84);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(604, 164);
            listBox1.TabIndex = 2;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            label2.CustomBackColor = null;
            label2.CustomForeColor = null;
            label2.Depth = 0;
            label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label2.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            label2.Location = new System.Drawing.Point(14, 84);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.MouseState = MaterialSkin.MouseState.HOVER;
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(65, 21);
            label2.TabIndex = 3;
            label2.Text = "成员列表";
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
            btn_OK.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btn_OK.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            btn_OK.HighEmphasis = true;
            btn_OK.Icon = null;
            btn_OK.Location = new System.Drawing.Point(540, 269);
            btn_OK.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btn_OK.MouseState = MaterialSkin.MouseState.HOVER;
            btn_OK.Name = "btn_OK";
            btn_OK.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_OK.Size = new System.Drawing.Size(160, 58);
            btn_OK.TabIndex = 4;
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
            btn_Cancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btn_Cancel.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            btn_Cancel.HighEmphasis = true;
            btn_Cancel.Icon = null;
            btn_Cancel.Location = new System.Drawing.Point(340, 269);
            btn_Cancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btn_Cancel.MouseState = MaterialSkin.MouseState.HOVER;
            btn_Cancel.Name = "btn_Cancel";
            btn_Cancel.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_Cancel.Size = new System.Drawing.Size(160, 58);
            btn_Cancel.TabIndex = 5;
            btn_Cancel.Text = "Cancel";
            btn_Cancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btn_Cancel.UseAccentColor = false;
            btn_Cancel.UseVisualStyleBackColor = false;
            // 
            // G2DTypeFieldDialog
            // 
            AcceptButton = btn_OK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btn_OK;
            ClientSize = new System.Drawing.Size(720, 346);
            Controls.Add(btn_Cancel);
            Controls.Add(btn_OK);
            Controls.Add(label2);
            Controls.Add(listBox1);
            Controls.Add(label1);
            Controls.Add(comboBox1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "G2DTypeFieldDialog";
            Padding = new System.Windows.Forms.Padding(3, 24, 2, 2);
            Text = "G2DTypeFieldDialog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DeepEditor.Common.G2D.G2DBaseComboBox comboBox1;
        private DeepEditor.Common.G2D.G2DBaseLabel label1;
        private DeepEditor.Common.G2D.G2DBaseListBox listBox1;
        private DeepEditor.Common.G2D.G2DBaseLabel label2;
        private DeepEditor.Common.G2D.G2DBaseButton btn_OK;
        private DeepEditor.Common.G2D.G2DBaseButton btn_Cancel;
    }
}