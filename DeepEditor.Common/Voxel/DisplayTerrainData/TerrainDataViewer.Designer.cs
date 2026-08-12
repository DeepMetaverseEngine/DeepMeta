
namespace DeepEditor.Common.Voxel.DisplayTerrainData
{
    partial class TerrainDataViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerrainDataViewer));
            this.glControl1 = new OpenTK.WinForms.GLControl();
            this.toolStrip1 = new DeepEditor.Common.G2D.G2DBaseToolStrip();
            this.menu_View = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.glControl1.APIVersion = new System.Version(3, 3, 0, 0);
            this.glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.glControl1.IsEventDriven = true;
            this.glControl1.Location = new System.Drawing.Point(3, 28);
            this.glControl1.Name = "glControl1";
            this.glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.glControl1.Size = new System.Drawing.Size(1594, 949);
            this.glControl1.TabIndex = 0;
            this.glControl1.Text = "glControl1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_View,
            this.toolStripSeparator1,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1600, 33);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menu_View
            // 
            this.menu_View.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menu_View.Image = ((System.Drawing.Image)(resources.GetObject("menu_View.Image")));
            this.menu_View.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menu_View.Name = "menu_View";
            this.menu_View.Size = new System.Drawing.Size(69, 28);
            this.menu_View.Text = "View";
            this.menu_View.Click += new System.EventHandler(this.menu_View_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            // 
            // TerrainDataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.glControl1);
            this.Name = "TerrainDataViewer";
            this.Size = new System.Drawing.Size(1600, 980);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.WinForms.GLControl  glControl1;
        private DeepEditor.Common.G2D.G2DBaseToolStrip toolStrip1;
        private System.Windows.Forms.Timer timer1;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_View;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
