
namespace DeepEditor.Common.Voxel.DisplayTerrainData
{
    partial class FormVoxelCreater
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
            this.terrainDataViewer1 = new DeepEditor.Common.Voxel.DisplayTerrainData.TerrainDataViewer();
            this.SuspendLayout();
            // 
            // terrainDataViewer1
            // 
            this.terrainDataViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.terrainDataViewer1.Location = new System.Drawing.Point(0, 0);
            this.terrainDataViewer1.Name = "terrainDataViewer1";
            this.terrainDataViewer1.Size = new System.Drawing.Size(1614, 1278);
            this.terrainDataViewer1.TabIndex = 0;
            // 
            // FormVoxelCreater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1614, 1278);
            this.Controls.Add(this.terrainDataViewer1);
            this.Name = "FormVoxelCreater";
            this.Text = "FormVoxelCreater";
            this.ResumeLayout(false);

        }

        #endregion

        private TerrainDataViewer terrainDataViewer1;
    }
}