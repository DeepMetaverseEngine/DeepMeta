namespace DeepTools.MPQ
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnOpenMPQ = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExtractTo = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExtractSelectedTo = new System.Windows.Forms.ToolStripMenuItem();
            this.entriesPanel1 = new DeepTools.MPQ.EntriesPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1499, 31);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenMPQ,
            this.btnExtractTo,
            this.btnExtractSelectedTo});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(58, 28);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // btnOpenMPQ
            // 
            this.btnOpenMPQ.Name = "btnOpenMPQ";
            this.btnOpenMPQ.Size = new System.Drawing.Size(272, 30);
            this.btnOpenMPQ.Text = "Open MPQ";
            this.btnOpenMPQ.Click += new System.EventHandler(this.btnOpenMPQ_Click);
            // 
            // btnExtractTo
            // 
            this.btnExtractTo.Name = "btnExtractTo";
            this.btnExtractTo.Size = new System.Drawing.Size(272, 30);
            this.btnExtractTo.Text = "Extract To ...";
            this.btnExtractTo.Click += new System.EventHandler(this.btnExtractTo_Click);
            // 
            // btnExtractSelectedTo
            // 
            this.btnExtractSelectedTo.Name = "btnExtractSelectedTo";
            this.btnExtractSelectedTo.Size = new System.Drawing.Size(272, 30);
            this.btnExtractSelectedTo.Text = "Extract Selected To ...";
            this.btnExtractSelectedTo.Click += new System.EventHandler(this.btnExtractSelectedTo_Click);
            // 
            // entriesPanel1
            // 
            this.entriesPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entriesPanel1.Location = new System.Drawing.Point(0, 31);
            this.entriesPanel1.Name = "entriesPanel1";
            this.entriesPanel1.Size = new System.Drawing.Size(1499, 852);
            this.entriesPanel1.TabIndex = 2;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1499, 883);
            this.Controls.Add(this.entriesPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MPQ";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EntriesPanel entriesPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem btnOpenMPQ;
        private System.Windows.Forms.ToolStripMenuItem btnExtractTo;
        private System.Windows.Forms.ToolStripMenuItem btnExtractSelectedTo;
    }
}