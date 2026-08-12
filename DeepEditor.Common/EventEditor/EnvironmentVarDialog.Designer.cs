namespace DeepEditor.Common.EventEditor
{
    partial class EnvironmentVarDialog
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
            btn_OK = new G2D.G2DBaseButton();
            btn_Cancel = new G2D.G2DBaseButton();
            label1 = new G2D.G2DBaseLabel();
            textBox1 = new G2D.G2DBaseTextBox();
            comboBox1 = new G2D.G2DBaseComboBox();
            label2 = new G2D.G2DBaseLabel();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            chk_Sync = new G2D.G2DBaseCheckBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btn_OK
            // 
            btn_OK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
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
            btn_OK.Location = new System.Drawing.Point(522, 16);
            btn_OK.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            btn_OK.MouseState = MaterialSkin.MouseState.HOVER;
            btn_OK.Name = "btn_OK";
            btn_OK.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_OK.Size = new System.Drawing.Size(55, 30);
            btn_OK.TabIndex = 0;
            btn_OK.Text = "确定";
            btn_OK.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btn_OK.UseAccentColor = false;
            btn_OK.UseVisualStyleBackColor = false;
            btn_OK.Click += btn_OK_Click;
            // 
            // btn_Cancel
            // 
            btn_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
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
            btn_Cancel.Location = new System.Drawing.Point(447, 16);
            btn_Cancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            btn_Cancel.MouseState = MaterialSkin.MouseState.HOVER;
            btn_Cancel.Name = "btn_Cancel";
            btn_Cancel.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_Cancel.Size = new System.Drawing.Size(55, 30);
            btn_Cancel.TabIndex = 1;
            btn_Cancel.Text = "取消";
            btn_Cancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btn_Cancel.UseAccentColor = false;
            btn_Cancel.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            label1.CustomBackColor = null;
            label1.CustomForeColor = null;
            label1.Depth = 0;
            label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            label1.Location = new System.Drawing.Point(77, 8);
            label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label1.MouseState = MaterialSkin.MouseState.HOVER;
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(58, 21);
            label1.TabIndex = 2;
            label1.Text = "变量名 :";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox1.CustomBackColor = null;
            textBox1.CustomForeColor = null;
            textBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            textBox1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            textBox1.Location = new System.Drawing.Point(139, 7);
            textBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            textBox1.MaxLength = 50;
            textBox1.Multiline = false;
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(417, 50);
            textBox1.TabIndex = 3;
            textBox1.Text = "";
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
            comboBox1.Location = new System.Drawing.Point(139, 63);
            comboBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            comboBox1.MaxDropDownItems = 4;
            comboBox1.MouseState = MaterialSkin.MouseState.OUT;
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(417, 49);
            comboBox1.StartIndex = 0;
            comboBox1.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            label2.CustomBackColor = null;
            label2.CustomForeColor = null;
            label2.Depth = 0;
            label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label2.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            label2.Location = new System.Drawing.Point(61, 63);
            label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label2.MouseState = MaterialSkin.MouseState.HOVER;
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(74, 21);
            label2.TabIndex = 5;
            label2.Text = "变量类型 :";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            richTextBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            richTextBox1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            richTextBox1.Location = new System.Drawing.Point(2, 20);
            richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new System.Drawing.Size(601, 151);
            richTextBox1.TabIndex = 8;
            richTextBox1.Text = "";
            richTextBox1.Click += richTextBox1_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            splitContainer1.Location = new System.Drawing.Point(3, 64);
            splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel1.Controls.Add(chk_Sync);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(textBox1);
            splitContainer1.Panel1.Controls.Add(comboBox1);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel2.Controls.Add(groupBox1);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            splitContainer1.Size = new System.Drawing.Size(605, 355);
            splitContainer1.SplitterDistance = 179;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 9;
            // 
            // chk_Sync
            // 
            chk_Sync.AutoSize = true;
            chk_Sync.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            chk_Sync.CustomBackColor = null;
            chk_Sync.CustomForeColor = null;
            chk_Sync.Depth = 0;
            chk_Sync.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chk_Sync.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            chk_Sync.Location = new System.Drawing.Point(139, 117);
            chk_Sync.Margin = new System.Windows.Forms.Padding(2);
            chk_Sync.MouseLocation = new System.Drawing.Point(-1, -1);
            chk_Sync.MouseState = MaterialSkin.MouseState.HOVER;
            chk_Sync.Name = "chk_Sync";
            chk_Sync.ReadOnly = false;
            chk_Sync.Ripple = true;
            chk_Sync.Size = new System.Drawing.Size(210, 37);
            chk_Sync.TabIndex = 6;
            chk_Sync.Text = "网络协议 (同步到客户端)";
            chk_Sync.UseVisualStyleBackColor = false;
            chk_Sync.CheckedChanged += chk_Sync_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            groupBox1.Controls.Add(richTextBox1);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            groupBox1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Margin = new System.Windows.Forms.Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(2);
            groupBox1.Size = new System.Drawing.Size(605, 173);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "初始值";
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            panel1.Controls.Add(btn_Cancel);
            panel1.Controls.Add(btn_OK);
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            panel1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            panel1.Location = new System.Drawing.Point(3, 419);
            panel1.Margin = new System.Windows.Forms.Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(605, 56);
            panel1.TabIndex = 10;
            // 
            // EnvironmentVarDialog
            // 
            AcceptButton = btn_OK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btn_Cancel;
            ClientSize = new System.Drawing.Size(610, 477);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EnvironmentVarDialog";
            Padding = new System.Windows.Forms.Padding(3, 64, 2, 2);
            Text = "定义变量";
            FormClosing += SceneVarDialog_FormClosing;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DeepEditor.Common.G2D.G2DBaseButton btn_OK;
        private DeepEditor.Common.G2D.G2DBaseButton btn_Cancel;
        private DeepEditor.Common.G2D.G2DBaseLabel label1;
        private DeepEditor.Common.G2D.G2DBaseTextBox textBox1;
        private DeepEditor.Common.G2D.G2DBaseComboBox comboBox1;
        private DeepEditor.Common.G2D.G2DBaseLabel label2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private DeepEditor.Common.G2D.G2DBaseCheckBox chk_Sync;
    }
}