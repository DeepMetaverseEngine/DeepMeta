
namespace DeepEditor.Common.Voxel.Display3D
{
    partial class PanelVoxelViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelVoxelViewer));
            this.glControl1 = new OpenTK.WinForms.GLControl ();
            this.toolStrip1 = new DeepEditor.Common.G2D.G2DBaseToolStrip();
            this.menu_File = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            this.btn_Load = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.btn_SaveWorld = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_View = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btn_TakeSnap = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
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
            this.glControl1.Location = new System.Drawing.Point(3, 36);
            this.glControl1.Name = "glControl1";
            this.glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.glControl1.Size = new System.Drawing.Size(805, 671);
            this.glControl1.TabIndex = 0;
            this.glControl1.Text = "glControl1";


            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_File,
            this.toolStripSeparator1,
            this.menu_View});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(811, 33);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menu_File
            // 
            this.menu_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_Load,
            this.btn_SaveWorld,
            this.btn_TakeSnap});
            this.menu_File.Image = ((System.Drawing.Image)(resources.GetObject("menu_File.Image")));
            this.menu_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menu_File.Name = "menu_File";
            this.menu_File.Size = new System.Drawing.Size(58, 28);
            this.menu_File.Text = "File";
            // 
            // btn_Load
            // 
            this.btn_Load.Name = "btn_Load";
            this.btn_Load.Size = new System.Drawing.Size(270, 34);
            this.btn_Load.Text = "Load";
            this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
            // 
            // btn_SaveWorld
            // 
            this.btn_SaveWorld.Name = "btn_SaveWorld";
            this.btn_SaveWorld.Size = new System.Drawing.Size(270, 34);
            this.btn_SaveWorld.Text = "Save To World";
            this.btn_SaveWorld.Click += new System.EventHandler(this.btn_SaveWorld_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // menu_View
            // 
            this.menu_View.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menu_View.Image = ((System.Drawing.Image)(resources.GetObject("menu_View.Image")));
            this.menu_View.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menu_View.Name = "menu_View";
            this.menu_View.Size = new System.Drawing.Size(69, 28);
            this.menu_View.Text = "View";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            // 
            // btn_TakeSnap
            // 
            this.btn_TakeSnap.Name = "btn_TakeSnap";
            this.btn_TakeSnap.Size = new System.Drawing.Size(270, 34);
            this.btn_TakeSnap.Text = "Snap";
            this.btn_TakeSnap.Click += new System.EventHandler(this.btn_TakeSnap_Click);
            // 
            // VoxelViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "VoxelViewer";
            this.Size = new System.Drawing.Size(811, 710);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.WinForms.GLControl  glControl1;
        private DeepEditor.Common.G2D.G2DBaseToolStrip toolStrip1;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_View;
        private System.Windows.Forms.Timer timer1;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_File;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Load;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_SaveWorld;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_TakeSnap;
    }
}
