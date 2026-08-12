
namespace DeepEditor.Common.Voxel.DisplaySCVX
{
    partial class FormStreamingVoxelViewer
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
            this.voxelViewer1 = new StreamingVoxelViewer();
            this.SuspendLayout();
            // 
            // voxelViewer1
            // 
            this.voxelViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.voxelViewer1.Location = new System.Drawing.Point(0, 0);
            this.voxelViewer1.Name = "voxelViewer1";
            this.voxelViewer1.Size = new System.Drawing.Size(1371, 962);
            this.voxelViewer1.TabIndex = 0;
            // 
            // FormVoxelViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 962);
            this.Controls.Add(this.voxelViewer1);
            this.Name = "FormStreamingVoxelViewer";
            this.Text = "FormStreamingVoxelViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private StreamingVoxelViewer voxelViewer1;
    }
}