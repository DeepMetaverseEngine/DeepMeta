namespace DeepEditor.Common.G2D
{
    partial class G2DRichToolTipForm
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
            g2dBaseRichTextBox1 = new G2DBaseRichTextBox();
            SuspendLayout();
            // 
            // g2dBaseRichTextBox1
            // 
            g2dBaseRichTextBox1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            g2dBaseRichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            g2dBaseRichTextBox1.CustomBackColor = null;
            g2dBaseRichTextBox1.CustomForeColor = null;
            g2dBaseRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            g2dBaseRichTextBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            g2dBaseRichTextBox1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            g2dBaseRichTextBox1.Location = new System.Drawing.Point(0, 0);
            g2dBaseRichTextBox1.Name = "g2dBaseRichTextBox1";
            g2dBaseRichTextBox1.ReadOnly = true;
            g2dBaseRichTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal;
            g2dBaseRichTextBox1.Size = new System.Drawing.Size(486, 723);
            g2dBaseRichTextBox1.TabIndex = 0;
            g2dBaseRichTextBox1.Text = "";
            // 
            // G2DRichToolTipForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            BackColor = System.Drawing.SystemColors.MenuText;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            ClientSize = new System.Drawing.Size(486, 723);
            ControlBox = false;
            Controls.Add(g2dBaseRichTextBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "G2DRichToolTipForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "G2DRichToolTipForm";
            TopMost = true;
            TransparencyKey = System.Drawing.Color.Black;
            ResumeLayout(false);
        }

        #endregion

        private G2DBaseRichTextBox g2dBaseRichTextBox1;
    }
}