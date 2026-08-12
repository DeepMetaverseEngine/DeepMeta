
namespace DeepEditor.Common.Voxel.DisplaySCVX
{
    partial class StreamingVoxelViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StreamingVoxelViewer));
            this.glControl1 = new OpenTK.WinForms.GLControl ();
            this.toolStrip1 = new DeepEditor.Common.G2D.G2DBaseToolStrip();
            this.menu_View = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chk_Camera2D = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            this.btn_AddPlayer = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.btn_SetStaticObject = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new DeepEditor.Common.G2D.G2DBaseStatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.chk_Pause = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.glControl1.APIVersion = new System.Version(3, 3, 0, 0);
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.glControl1.IsEventDriven = true;
            this.glControl1.Location = new System.Drawing.Point(0, 33);
            this.glControl1.Name = "glControl1";
            this.glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.glControl1.Size = new System.Drawing.Size(1600, 925);
            this.glControl1.TabIndex = 0;
            this.glControl1.Text = "glControl1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_View,
            this.toolStripSeparator1,
            this.chk_Camera2D,
            this.chk_Pause,
            this.toolStripSeparator2,
            this.toolStripDropDownButton1,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
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
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // chk_Camera2D
            // 
            this.chk_Camera2D.CheckOnClick = true;
            this.chk_Camera2D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chk_Camera2D.Image = ((System.Drawing.Image)(resources.GetObject("chk_Camera2D.Image")));
            this.chk_Camera2D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chk_Camera2D.Name = "chk_Camera2D";
            this.chk_Camera2D.Size = new System.Drawing.Size(39, 28);
            this.chk_Camera2D.Text = "2D";
            this.chk_Camera2D.CheckedChanged += new System.EventHandler(this.chk_Camera2D_CheckedChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_AddPlayer,
            this.btn_SetStaticObject});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(64, 28);
            this.toolStripDropDownButton1.Text = "功能";
            // 
            // btn_AddPlayer
            // 
            this.btn_AddPlayer.Name = "btn_AddPlayer";
            this.btn_AddPlayer.Size = new System.Drawing.Size(253, 34);
            this.btn_AddPlayer.Text = "Add Player";
            // 
            // btn_SetStaticObject
            // 
            this.btn_SetStaticObject.Name = "btn_SetStaticObject";
            this.btn_SetStaticObject.Size = new System.Drawing.Size(253, 34);
            this.btn_SetStaticObject.Text = "Set Static Object";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 958);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 22, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1600, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 15);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // chk_Pause
            // 
            this.chk_Pause.CheckOnClick = true;
            this.chk_Pause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.chk_Pause.Image = global::DeepEditor.Common.Properties.Resources.Image2;
            this.chk_Pause.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.chk_Pause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chk_Pause.Name = "chk_Pause";
            this.chk_Pause.Size = new System.Drawing.Size(34, 28);
            this.chk_Pause.Text = "Pause";
            this.chk_Pause.CheckedChanged += new System.EventHandler(this.chk_Pause_CheckedChanged);
            // 
            // StreamingVoxelViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "StreamingVoxelViewer";
            this.Size = new System.Drawing.Size(1600, 980);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.WinForms.GLControl  glControl1;
        private DeepEditor.Common.G2D.G2DBaseToolStrip toolStrip1;
        private System.Windows.Forms.Timer timer1;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_View;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton chk_Camera2D;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton toolStripDropDownButton1;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_AddPlayer;
        private DeepEditor.Common.G2D.G2DBaseStatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_SetStaticObject;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton chk_Pause;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
