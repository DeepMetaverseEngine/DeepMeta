
namespace DeepEditor.Common.Voxel.DisplayMagicaVoxel
{
    partial class FormMagicaVoxelViewer
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
            this.magicaVoxelViewer1 = new DeepEditor.Common.Voxel.DisplayMagicaVoxel.MagicaVoxelViewer();
            this.SuspendLayout();
            // 
            // magicaVoxelViewer1
            // 
            this.magicaVoxelViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magicaVoxelViewer1.Location = new System.Drawing.Point(0, 0);
            this.magicaVoxelViewer1.Name = "magicaVoxelViewer1";
            this.magicaVoxelViewer1.Size = new System.Drawing.Size(1333, 958);
            this.magicaVoxelViewer1.TabIndex = 0;
            // 
            // FormMagicaVoxelViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1333, 958);
            this.Controls.Add(this.magicaVoxelViewer1);
            this.Name = "FormMagicaVoxelViewer";
            this.Text = "FormMagicaVoxelViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private DisplayMagicaVoxel.MagicaVoxelViewer magicaVoxelViewer1;
    }
}