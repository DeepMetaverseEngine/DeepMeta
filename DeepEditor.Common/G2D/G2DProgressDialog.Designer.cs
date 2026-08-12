namespace DeepEditor.Common.G2D
{
    partial class G2DProgressDialog
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
            progressBar1 = new System.Windows.Forms.ProgressBar();
            textBox1 = new G2DBaseTextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            btnClose = new G2DBaseButton();
            lbl_Title = new G2DBaseLabel();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            progressBar1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            progressBar1.Location = new System.Drawing.Point(3, 86);
            progressBar1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(1178, 32);
            progressBar1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox1.CustomBackColor = null;
            textBox1.CustomForeColor = null;
            textBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            textBox1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            textBox1.Location = new System.Drawing.Point(3, 126);
            textBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            textBox1.MaxLength = 50;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new System.Drawing.Size(1178, 43);
            textBox1.TabIndex = 1;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
            // 
            // btnClose
            // 
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnClose.AutoSize = false;
            btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnClose.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnClose.CustomBackColor = null;
            btnClose.CustomForeColor = null;
            btnClose.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnClose.Depth = 0;
            btnClose.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btnClose.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btnClose.HighEmphasis = true;
            btnClose.Icon = null;
            btnClose.Location = new System.Drawing.Point(993, 174);
            btnClose.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            btnClose.Name = "btnClose";
            btnClose.NoAccentTextColor = System.Drawing.Color.Empty;
            btnClose.Size = new System.Drawing.Size(182, 51);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnClose.UseAccentColor = false;
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lbl_Title
            // 
            lbl_Title.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            lbl_Title.CustomBackColor = null;
            lbl_Title.CustomForeColor = null;
            lbl_Title.Depth = 0;
            lbl_Title.Dock = System.Windows.Forms.DockStyle.Top;
            lbl_Title.Font = new System.Drawing.Font("Microsoft YaHei UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            lbl_Title.FontType = MaterialSkin.MaterialSkinManager.FontType.H6;
            lbl_Title.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            lbl_Title.Location = new System.Drawing.Point(3, 2);
            lbl_Title.Margin = new System.Windows.Forms.Padding(13, 0, 13, 0);
            lbl_Title.MouseState = MaterialSkin.MouseState.HOVER;
            lbl_Title.Name = "lbl_Title";
            lbl_Title.Size = new System.Drawing.Size(1178, 45);
            lbl_Title.TabIndex = 3;
            lbl_Title.Text = "g2dBaseLabel1";
            lbl_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            statusStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            statusStrip1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            statusStrip1.Location = new System.Drawing.Point(3, 240);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(1178, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pictureBox1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            pictureBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            pictureBox1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            pictureBox1.Location = new System.Drawing.Point(9, 66);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(1172, 13);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // G2DProgressDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1190, 270);
            ControlBox = false;
            Controls.Add(pictureBox1);
            Controls.Add(progressBar1);
            Controls.Add(textBox1);
            Controls.Add(statusStrip1);
            Controls.Add(lbl_Title);
            Controls.Add(btnClose);
            FormStyle = FormStyles.StatusAndActionBar_None;
            Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "G2DProgressDialog";
            Padding = new System.Windows.Forms.Padding(3, 2, 9, 8);
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "G2DProgressDialog";
            TopMost = true;
            FormClosing += G2DProgressDialog_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private DeepEditor.Common.G2D.G2DBaseTextBox textBox1;
        private System.Windows.Forms.Timer timer1;
        private DeepEditor.Common.G2D.G2DBaseButton btnClose;
        private G2DBaseLabel lbl_Title;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}