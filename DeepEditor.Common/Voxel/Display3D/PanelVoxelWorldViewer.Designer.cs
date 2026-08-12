
namespace DeepEditor.Common.Voxel.Display3D
{
    partial class PanelVoxelWorldViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelVoxelWorldViewer));
            this.glControl1 = new OpenTK.WinForms.GLControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new DeepEditor.Common.G2D.G2DBaseToolStrip();
            this.menu_File = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            this.btn_Save = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.btn_LoadFromBin = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.btn_SaveToBin = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_LoadMeshDX = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.btn_LoadMesh = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.btn_ClearMesh = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_Import = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.btn_ImportMagicaVoxel = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_Load = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.btn_Properties = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.btn_SavePathCache = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_Meshs = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_View = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            this.chk_ShowMousePath = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.chk_2D = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new DeepEditor.Common.G2D.G2DBaseStatusStrip();
            this.txt_State = new System.Windows.Forms.ToolStripStatusLabel();
            this.txt_Objects = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.glControl1.APIVersion = new System.Version(3, 3, 0, 0);
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.glControl1.IsDesignRender = false;
            this.glControl1.IsEventDriven = true;
            this.glControl1.Location = new System.Drawing.Point(0, 33);
            this.glControl1.Margin = new System.Windows.Forms.Padding(8);
            this.glControl1.Name = "glControl1";
            this.glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.glControl1.Size = new System.Drawing.Size(1941, 1152);
            this.glControl1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 33;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_File,
            this.toolStripSeparator1,
            this.menu_Meshs,
            this.toolStripSeparator5,
            this.menu_View,
            this.toolStripSeparator2,
            this.toolStripSeparator4,
            this.chk_2D,
            this.toolStripSeparator3,
            this.toolStripSeparator6});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1941, 33);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menu_File
            // 
            this.menu_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_Save,
            this.btn_LoadFromBin,
            this.btn_SaveToBin,
            this.toolStripMenuItem2,
            this.btn_LoadMeshDX,
            this.btn_LoadMesh,
            this.btn_ClearMesh,
            this.toolStripSeparator7,
            this.btn_Import,
            this.toolStripMenuItem1,
            this.btn_Load,
            this.btn_Properties,
            this.btn_SavePathCache});
            this.menu_File.Image = ((System.Drawing.Image)(resources.GetObject("menu_File.Image")));
            this.menu_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menu_File.Name = "menu_File";
            this.menu_File.Size = new System.Drawing.Size(58, 28);
            this.menu_File.Text = "File";
            // 
            // btn_Save
            // 
            this.btn_Save.Enabled = false;
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.btn_Save.Size = new System.Drawing.Size(250, 34);
            this.btn_Save.Text = "Save";
            this.btn_Save.Visible = false;
            this.btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // btn_LoadFromBin
            // 
            this.btn_LoadFromBin.Name = "btn_LoadFromBin";
            this.btn_LoadFromBin.Size = new System.Drawing.Size(250, 34);
            this.btn_LoadFromBin.Text = "Load From Bin";
            this.btn_LoadFromBin.Click += new System.EventHandler(this.btn_LoadFromBin_Click);
            // 
            // btn_SaveToBin
            // 
            this.btn_SaveToBin.Enabled = false;
            this.btn_SaveToBin.Name = "btn_SaveToBin";
            this.btn_SaveToBin.Size = new System.Drawing.Size(250, 34);
            this.btn_SaveToBin.Text = "Save To Bin";
            this.btn_SaveToBin.Visible = false;
            this.btn_SaveToBin.Click += new System.EventHandler(this.btn_SaveToBin_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(247, 6);
            // 
            // btn_LoadMeshDX
            // 
            this.btn_LoadMeshDX.Name = "btn_LoadMeshDX";
            this.btn_LoadMeshDX.Size = new System.Drawing.Size(250, 34);
            this.btn_LoadMeshDX.Text = "Load Mesh DX";
            this.btn_LoadMeshDX.Click += new System.EventHandler(this.btn_LoadMeshDX_Click);
            // 
            // btn_LoadMesh
            // 
            this.btn_LoadMesh.Name = "btn_LoadMesh";
            this.btn_LoadMesh.Size = new System.Drawing.Size(250, 34);
            this.btn_LoadMesh.Text = "Load Mesh";
            this.btn_LoadMesh.Click += new System.EventHandler(this.btn_LoadMesh_Click);
            // 
            // btn_ClearMesh
            // 
            this.btn_ClearMesh.Name = "btn_ClearMesh";
            this.btn_ClearMesh.Size = new System.Drawing.Size(250, 34);
            this.btn_ClearMesh.Text = "Clear Mesh";
            this.btn_ClearMesh.Click += new System.EventHandler(this.btn_ClearMesh_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(247, 6);
            // 
            // btn_Import
            // 
            this.btn_Import.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_ImportMagicaVoxel});
            this.btn_Import.Name = "btn_Import";
            this.btn_Import.Size = new System.Drawing.Size(250, 34);
            this.btn_Import.Text = "Import";
            // 
            // btn_ImportMagicaVoxel
            // 
            this.btn_ImportMagicaVoxel.Name = "btn_ImportMagicaVoxel";
            this.btn_ImportMagicaVoxel.Size = new System.Drawing.Size(271, 34);
            this.btn_ImportMagicaVoxel.Text = "MagicaVoxel (.vox)";
            this.btn_ImportMagicaVoxel.Click += new System.EventHandler(this.btn_ImportMagicaVoxel_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(247, 6);
            // 
            // btn_Load
            // 
            this.btn_Load.Enabled = false;
            this.btn_Load.Name = "btn_Load";
            this.btn_Load.Size = new System.Drawing.Size(250, 34);
            this.btn_Load.Text = "Load XML";
            this.btn_Load.Visible = false;
            this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
            // 
            // btn_Properties
            // 
            this.btn_Properties.Enabled = false;
            this.btn_Properties.Name = "btn_Properties";
            this.btn_Properties.Size = new System.Drawing.Size(250, 34);
            this.btn_Properties.Text = "Build Config";
            this.btn_Properties.Visible = false;
            this.btn_Properties.Click += new System.EventHandler(this.btn_Properties_Click);
            // 
            // btn_SavePathCache
            // 
            this.btn_SavePathCache.Name = "btn_SavePathCache";
            this.btn_SavePathCache.Size = new System.Drawing.Size(250, 34);
            this.btn_SavePathCache.Text = "Save Path Cache";
            this.btn_SavePathCache.Click += new System.EventHandler(this.btn_SavePathCache_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // menu_Meshs
            // 
            this.menu_Meshs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menu_Meshs.Image = ((System.Drawing.Image)(resources.GetObject("menu_Meshs.Image")));
            this.menu_Meshs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menu_Meshs.Name = "menu_Meshs";
            this.menu_Meshs.Size = new System.Drawing.Size(83, 28);
            this.menu_Meshs.Text = "Meshs";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 33);
            // 
            // menu_View
            // 
            this.menu_View.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chk_ShowMousePath});
            this.menu_View.Image = ((System.Drawing.Image)(resources.GetObject("menu_View.Image")));
            this.menu_View.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menu_View.Name = "menu_View";
            this.menu_View.Size = new System.Drawing.Size(69, 28);
            this.menu_View.Text = "View";
            // 
            // chk_ShowMousePath
            // 
            this.chk_ShowMousePath.Checked = true;
            this.chk_ShowMousePath.CheckOnClick = true;
            this.chk_ShowMousePath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_ShowMousePath.Name = "chk_ShowMousePath";
            this.chk_ShowMousePath.Size = new System.Drawing.Size(218, 34);
            this.chk_ShowMousePath.Text = "显示鼠标寻路";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 33);
            // 
            // chk_2D
            // 
            this.chk_2D.CheckOnClick = true;
            this.chk_2D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chk_2D.Image = ((System.Drawing.Image)(resources.GetObject("chk_2D.Image")));
            this.chk_2D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chk_2D.Name = "chk_2D";
            this.chk_2D.Size = new System.Drawing.Size(39, 28);
            this.chk_2D.Text = "2D";
            this.chk_2D.Click += new System.EventHandler(this.chk_2D_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 33);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txt_State,
            this.txt_Objects});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1185);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 17, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1941, 31);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // txt_State
            // 
            this.txt_State.Name = "txt_State";
            this.txt_State.Size = new System.Drawing.Size(50, 24);
            this.txt_State.Text = "        ";
            // 
            // txt_Objects
            // 
            this.txt_Objects.Name = "txt_Objects";
            this.txt_Objects.Size = new System.Drawing.Size(21, 24);
            this.txt_Objects.Text = "0";
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 3000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // PanelVoxelWorldViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PanelVoxelWorldViewer";
            this.Size = new System.Drawing.Size(1941, 1216);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Load;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_LoadFromBin;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Properties;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Save;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_SavePathCache;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_SaveToBin;
        private OpenTK.WinForms.GLControl  glControl1;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_File;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_View;
        private DeepEditor.Common.G2D.G2DBaseStatusStrip statusStrip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private DeepEditor.Common.G2D.G2DBaseToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripStatusLabel txt_Objects;
        private System.Windows.Forms.ToolStripStatusLabel txt_State;

        private DeepEditor.Common.G2D.G2DBaseToolStripButton chk_2D;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_LoadMesh;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_LoadMeshDX;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_ClearMesh;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_Meshs;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem chk_ShowMousePath;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Import;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_ImportMagicaVoxel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        #endregion
    }
}