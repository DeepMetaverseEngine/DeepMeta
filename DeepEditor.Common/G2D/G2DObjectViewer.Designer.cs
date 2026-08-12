
namespace DeepEditor.Common.G2D
{
    partial class G2DObjectViewer
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
            treeView1 = new System.Windows.Forms.TreeView();
            SuspendLayout();
            // 
            // treeView1
            // 
            treeView1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            treeView1.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            treeView1.LineColor = System.Drawing.Color.DarkGray;
            treeView1.Location = new System.Drawing.Point(3, 24);
            treeView1.Margin = new System.Windows.Forms.Padding(2);
            treeView1.Name = "treeView1";
            treeView1.Size = new System.Drawing.Size(850, 502);
            treeView1.TabIndex = 0;
            // 
            // G2DObjectViewer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(855, 528);
            Controls.Add(treeView1);
            Margin = new System.Windows.Forms.Padding(2);
            Name = "G2DObjectViewer";
            Padding = new System.Windows.Forms.Padding(3, 24, 2, 2);
            Text = "G2DObjectViewer";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
    }
}