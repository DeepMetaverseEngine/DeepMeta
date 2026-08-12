namespace DeepEditor.Common.Utils
{
    partial class ConsoleOutput
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
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new DeepEditor.Common.G2D.G2DBaseRichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new DeepEditor.Common.G2D.G2DBaseToolStrip();
            this.toolDock = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 33);
            this.textBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox1.MaxLength = 400000;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(1426, 743);
            this.textBox1.TabIndex = 0;
            this.textBox1.WordWrap = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolDock});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1426, 33);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolDock
            // 
            this.toolDock.Checked = true;
            this.toolDock.CheckOnClick = true;
            this.toolDock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolDock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolDock.Image = global::DeepEditor.Common.Properties.Resources.Image34;
            this.toolDock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDock.Name = "toolDock";
            this.toolDock.Size = new System.Drawing.Size(34, 28);
            this.toolDock.Text = "Dock";
            // 
            // ConsoleOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 776);
            this.ControlBox = false;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "ConsoleOutput";
            this.ShowInTaskbar = false;
            this.Text = "ConsoleOutput";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DeepEditor.Common.G2D.G2DBaseRichTextBox textBox1;
        private System.Windows.Forms.Timer timer1;
        private DeepEditor.Common.G2D.G2DBaseToolStrip toolStrip1;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton toolDock;
    }
}