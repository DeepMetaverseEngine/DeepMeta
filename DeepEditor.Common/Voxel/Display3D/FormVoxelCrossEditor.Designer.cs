

namespace DeepEditor.Common.Voxel.Display3D
{
    partial class FormVoxelCrossEditor
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVoxelCrossEditor));
            glControl1 = new OpenTK.WinForms.GLControl();
            timer1 = new System.Windows.Forms.Timer(components);
            toolStrip1 = new DeepEditor.Common.G2D.G2DBaseToolStrip();
            menu_File = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            btn_Save = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_OptimizeSave = new System.Windows.Forms.ToolStripMenuItem();
            btn_Optimize = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            btn_LoadMeshDX = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_LoadMesh = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_ClearMesh = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_CombineMeshWeight = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            btn_Load = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_Properties = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            optimizeVoxels_KeepUpward = new System.Windows.Forms.ToolStripMenuItem();
            btn_makeVoxel2DPlane = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripDropDownButton1 = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            btn_Undo = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_Redo = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            menu_Meshs = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            menu_View = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            chk_2D = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            btn_Undo2 = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            btn_Redo2 = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            chk_MoveBrush = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            chk_MoveEraser = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            chk_Test = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            statusStrip1 = new DeepEditor.Common.G2D.G2DBaseStatusStrip();
            txt_State = new System.Windows.Forms.ToolStripStatusLabel();
            timer2 = new System.Windows.Forms.Timer(components);
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            flowLayoutPanel1 = new System.Windows.Forms.Panel();
            g2DPropertyGrid1 = new DeepEditor.Common.G2D.DataGrid.G2DPropertyGrid();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // glControl1
            // 
            glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl1.APIVersion = new System.Version(3, 3, 0, 0);
            glControl1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            glControl1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            glControl1.IsDesignRender = false;
            glControl1.IsEventDriven = true;
            glControl1.Location = new System.Drawing.Point(0, 0);
            glControl1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            glControl1.Name = "glControl1";
            glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            glControl1.Size = new System.Drawing.Size(1412, 973);
            glControl1.TabIndex = 0;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 33;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStrip1.CustomBackColor = null;
            toolStrip1.CustomForeColor = null;
            toolStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            toolStrip1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menu_File, toolStripSeparator1, toolStripDropDownButton1, toolStripSeparator5, menu_Meshs, toolStripSeparator7, menu_View, toolStripSeparator2, chk_2D, toolStripSeparator4, btn_Undo2, btn_Redo2, toolStripSeparator3, chk_MoveBrush, chk_MoveEraser, chk_Test, toolStripSeparator6 });
            toolStrip1.Location = new System.Drawing.Point(5, 34);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            toolStrip1.Size = new System.Drawing.Size(1851, 33);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // menu_File
            // 
            menu_File.CustomBackColor = null;
            menu_File.CustomForeColor = null;
            menu_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_Save, btn_OptimizeSave, btn_Optimize, toolStripMenuItem2, btn_LoadMeshDX, btn_LoadMesh, btn_ClearMesh, btn_CombineMeshWeight, toolStripMenuItem1, btn_Load, btn_Properties, optimizeVoxels_KeepUpward, btn_makeVoxel2DPlane });
            menu_File.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            menu_File.Image = (System.Drawing.Image)resources.GetObject("menu_File.Image");
            menu_File.ImageOrigin = null;
            menu_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            menu_File.Name = "menu_File";
            menu_File.Size = new System.Drawing.Size(50, 28);
            menu_File.Text = "File";
            // 
            // btn_Save
            // 
            btn_Save.CustomBackColor = null;
            btn_Save.CustomForeColor = null;
            btn_Save.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_Save.ImageOrigin = null;
            btn_Save.Name = "btn_Save";
            btn_Save.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            btn_Save.Size = new System.Drawing.Size(374, 34);
            btn_Save.Text = "Save";
            btn_Save.Click += Btn_Save_Click;
            // 
            // btn_OptimizeSave
            // 
            btn_OptimizeSave.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_OptimizeSave.Name = "btn_OptimizeSave";
            btn_OptimizeSave.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S;
            btn_OptimizeSave.Size = new System.Drawing.Size(374, 34);
            btn_OptimizeSave.Text = "Optimize Voxels And Save";
            btn_OptimizeSave.Click += btn_OptimizeSave_Click;
            // 
            // btn_Optimize
            // 
            btn_Optimize.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_Optimize.Name = "btn_Optimize";
            btn_Optimize.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D;
            btn_Optimize.Size = new System.Drawing.Size(374, 34);
            btn_Optimize.Text = "Optimize Voxels";
            btn_Optimize.Click += btn_optimizeVoxelsToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripMenuItem2.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(371, 6);
            // 
            // btn_LoadMeshDX
            // 
            btn_LoadMeshDX.CustomBackColor = null;
            btn_LoadMeshDX.CustomForeColor = null;
            btn_LoadMeshDX.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_LoadMeshDX.ImageOrigin = null;
            btn_LoadMeshDX.Name = "btn_LoadMeshDX";
            btn_LoadMeshDX.Size = new System.Drawing.Size(374, 34);
            btn_LoadMeshDX.Text = "Load Mesh DX";
            btn_LoadMeshDX.Click += btn_LoadMeshDX_Click;
            // 
            // btn_LoadMesh
            // 
            btn_LoadMesh.CustomBackColor = null;
            btn_LoadMesh.CustomForeColor = null;
            btn_LoadMesh.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_LoadMesh.ImageOrigin = null;
            btn_LoadMesh.Name = "btn_LoadMesh";
            btn_LoadMesh.Size = new System.Drawing.Size(374, 34);
            btn_LoadMesh.Text = "Load Mesh";
            btn_LoadMesh.Click += btn_LoadMesh_Click;
            // 
            // btn_ClearMesh
            // 
            btn_ClearMesh.CustomBackColor = null;
            btn_ClearMesh.CustomForeColor = null;
            btn_ClearMesh.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_ClearMesh.ImageOrigin = null;
            btn_ClearMesh.Name = "btn_ClearMesh";
            btn_ClearMesh.Size = new System.Drawing.Size(374, 34);
            btn_ClearMesh.Text = "Clear Mesh";
            btn_ClearMesh.Click += btn_ClearMesh_Click;
            // 
            // btn_CombineMeshWeight
            // 
            btn_CombineMeshWeight.CustomBackColor = null;
            btn_CombineMeshWeight.CustomForeColor = null;
            btn_CombineMeshWeight.Enabled = false;
            btn_CombineMeshWeight.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_CombineMeshWeight.ImageOrigin = null;
            btn_CombineMeshWeight.Name = "btn_CombineMeshWeight";
            btn_CombineMeshWeight.Size = new System.Drawing.Size(374, 34);
            btn_CombineMeshWeight.Text = "Combine Mesh Weight";
            btn_CombineMeshWeight.Click += btn_CombineMeshWeight_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripMenuItem1.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(371, 6);
            // 
            // btn_Load
            // 
            btn_Load.CustomBackColor = null;
            btn_Load.CustomForeColor = null;
            btn_Load.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_Load.ImageOrigin = null;
            btn_Load.Name = "btn_Load";
            btn_Load.Size = new System.Drawing.Size(374, 34);
            btn_Load.Text = "Load XML";
            btn_Load.Click += btn_Load_Click;
            // 
            // btn_Properties
            // 
            btn_Properties.CustomBackColor = null;
            btn_Properties.CustomForeColor = null;
            btn_Properties.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_Properties.ImageOrigin = null;
            btn_Properties.Name = "btn_Properties";
            btn_Properties.Size = new System.Drawing.Size(374, 34);
            btn_Properties.Text = "Build Config";
            btn_Properties.Click += btn_Properties_Click;
            // 
            // optimizeVoxels_KeepUpward
            // 
            optimizeVoxels_KeepUpward.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            optimizeVoxels_KeepUpward.Name = "optimizeVoxels_KeepUpward";
            optimizeVoxels_KeepUpward.Size = new System.Drawing.Size(374, 34);
            optimizeVoxels_KeepUpward.Text = "Optimize Voxels (Keep Upward)";
            optimizeVoxels_KeepUpward.Click += btn_optimizeVoxels_KeepUpward_Click;
            // 
            // btn_makeVoxel2DPlane
            // 
            btn_makeVoxel2DPlane.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_makeVoxel2DPlane.Name = "btn_makeVoxel2DPlane";
            btn_makeVoxel2DPlane.Size = new System.Drawing.Size(374, 34);
            btn_makeVoxel2DPlane.Text = "Make Voxel 2D Plane";
            btn_makeVoxel2DPlane.Click += btn_makeVoxel2DPlane_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.CustomBackColor = null;
            toolStripDropDownButton1.CustomForeColor = null;
            toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_Undo, btn_Redo, toolStripMenuItem3 });
            toolStripDropDownButton1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageOrigin = null;
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(53, 28);
            toolStripDropDownButton1.Text = "Edit";
            // 
            // btn_Undo
            // 
            btn_Undo.CustomBackColor = null;
            btn_Undo.CustomForeColor = null;
            btn_Undo.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_Undo.Image = Properties.Resources.undo;
            btn_Undo.ImageOrigin = null;
            btn_Undo.Name = "btn_Undo";
            btn_Undo.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z;
            btn_Undo.Size = new System.Drawing.Size(196, 34);
            btn_Undo.Text = "Undo";
            // 
            // btn_Redo
            // 
            btn_Redo.CustomBackColor = null;
            btn_Redo.CustomForeColor = null;
            btn_Redo.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_Redo.Image = Properties.Resources.redo;
            btn_Redo.ImageOrigin = null;
            btn_Redo.Name = "btn_Redo";
            btn_Redo.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y;
            btn_Redo.Size = new System.Drawing.Size(196, 34);
            btn_Redo.Text = "Redo";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripMenuItem3.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(193, 6);
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripSeparator5.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 33);
            // 
            // menu_Meshs
            // 
            menu_Meshs.CustomBackColor = null;
            menu_Meshs.CustomForeColor = null;
            menu_Meshs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menu_Meshs.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            menu_Meshs.Image = (System.Drawing.Image)resources.GetObject("menu_Meshs.Image");
            menu_Meshs.ImageOrigin = null;
            menu_Meshs.ImageTransparentColor = System.Drawing.Color.Magenta;
            menu_Meshs.Name = "menu_Meshs";
            menu_Meshs.Size = new System.Drawing.Size(70, 28);
            menu_Meshs.Text = "Meshs";
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripSeparator7.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(6, 33);
            // 
            // menu_View
            // 
            menu_View.CustomBackColor = null;
            menu_View.CustomForeColor = null;
            menu_View.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menu_View.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            menu_View.Image = (System.Drawing.Image)resources.GetObject("menu_View.Image");
            menu_View.ImageOrigin = null;
            menu_View.ImageTransparentColor = System.Drawing.Color.Magenta;
            menu_View.Name = "menu_View";
            menu_View.Size = new System.Drawing.Size(59, 28);
            menu_View.Text = "View";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // chk_2D
            // 
            chk_2D.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            chk_2D.CheckOnClick = true;
            chk_2D.CustomBackColor = null;
            chk_2D.CustomForeColor = null;
            chk_2D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            chk_2D.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            chk_2D.Image = (System.Drawing.Image)resources.GetObject("chk_2D.Image");
            chk_2D.ImageOrigin = null;
            chk_2D.ImageTransparentColor = System.Drawing.Color.Magenta;
            chk_2D.Name = "chk_2D";
            chk_2D.Size = new System.Drawing.Size(34, 28);
            chk_2D.Text = "2D";
            chk_2D.Click += chk_2D_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripSeparator4.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 33);
            // 
            // btn_Undo2
            // 
            btn_Undo2.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btn_Undo2.CustomBackColor = null;
            btn_Undo2.CustomForeColor = null;
            btn_Undo2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_Undo2.Enabled = false;
            btn_Undo2.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_Undo2.Image = Properties.Resources.undo;
            btn_Undo2.ImageOrigin = null;
            btn_Undo2.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Undo2.Name = "btn_Undo2";
            btn_Undo2.Size = new System.Drawing.Size(34, 28);
            btn_Undo2.Text = "Undo";
            // 
            // btn_Redo2
            // 
            btn_Redo2.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btn_Redo2.CustomBackColor = null;
            btn_Redo2.CustomForeColor = null;
            btn_Redo2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btn_Redo2.Enabled = false;
            btn_Redo2.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btn_Redo2.Image = Properties.Resources.redo;
            btn_Redo2.ImageOrigin = null;
            btn_Redo2.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Redo2.Name = "btn_Redo2";
            btn_Redo2.Size = new System.Drawing.Size(34, 28);
            btn_Redo2.Text = "Redo";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // chk_MoveBrush
            // 
            chk_MoveBrush.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            chk_MoveBrush.Checked = true;
            chk_MoveBrush.CheckOnClick = true;
            chk_MoveBrush.CheckState = System.Windows.Forms.CheckState.Checked;
            chk_MoveBrush.CustomBackColor = null;
            chk_MoveBrush.CustomForeColor = null;
            chk_MoveBrush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            chk_MoveBrush.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            chk_MoveBrush.Image = Properties.Resources.下载__1_;
            chk_MoveBrush.ImageOrigin = null;
            chk_MoveBrush.ImageTransparentColor = System.Drawing.Color.Magenta;
            chk_MoveBrush.Name = "chk_MoveBrush";
            chk_MoveBrush.Size = new System.Drawing.Size(34, 28);
            chk_MoveBrush.Text = "抹平行走面";
            chk_MoveBrush.Click += Chk_MoveBrush_Click;
            // 
            // chk_MoveEraser
            // 
            chk_MoveEraser.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            chk_MoveEraser.CheckOnClick = true;
            chk_MoveEraser.CustomBackColor = null;
            chk_MoveEraser.CustomForeColor = null;
            chk_MoveEraser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            chk_MoveEraser.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            chk_MoveEraser.Image = Properties.Resources.th__2_;
            chk_MoveEraser.ImageOrigin = null;
            chk_MoveEraser.ImageTransparentColor = System.Drawing.Color.Magenta;
            chk_MoveEraser.Name = "chk_MoveEraser";
            chk_MoveEraser.Size = new System.Drawing.Size(34, 28);
            chk_MoveEraser.Text = "清除行走面";
            chk_MoveEraser.Click += Chk_MoveEraser_Click;
            // 
            // chk_Test
            // 
            chk_Test.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            chk_Test.CheckOnClick = true;
            chk_Test.CustomBackColor = null;
            chk_Test.CustomForeColor = null;
            chk_Test.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            chk_Test.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            chk_Test.Image = Properties.Resources.icon_run;
            chk_Test.ImageOrigin = null;
            chk_Test.ImageTransparentColor = System.Drawing.Color.Magenta;
            chk_Test.Name = "chk_Test";
            chk_Test.Size = new System.Drawing.Size(34, 28);
            chk_Test.Text = "测试";
            chk_Test.Click += chk_Test_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            toolStripSeparator6.ForeColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(6, 33);
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            statusStrip1.CustomBackColor = null;
            statusStrip1.CustomForeColor = null;
            statusStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            statusStrip1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { txt_State });
            statusStrip1.Location = new System.Drawing.Point(5, 1040);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 17, 0);
            statusStrip1.Size = new System.Drawing.Size(1851, 27);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // txt_State
            // 
            txt_State.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            txt_State.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            txt_State.Name = "txt_State";
            txt_State.Size = new System.Drawing.Size(41, 20);
            txt_State.Text = "        ";
            // 
            // timer2
            // 
            timer2.Enabled = true;
            timer2.Interval = 3000;
            timer2.Tick += timer2_Tick;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            splitContainer1.Location = new System.Drawing.Point(5, 67);
            splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            splitContainer1.Panel1.Controls.Add(glControl1);
            splitContainer1.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            splitContainer1.Size = new System.Drawing.Size(1851, 973);
            splitContainer1.SplitterDistance = 1412;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            flowLayoutPanel1.Controls.Add(g2DPropertyGrid1);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            flowLayoutPanel1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(434, 973);
            flowLayoutPanel1.TabIndex = 6;
            // 
            // g2DPropertyGrid1
            // 
            g2DPropertyGrid1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            g2DPropertyGrid1.CategoryForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            g2DPropertyGrid1.CategorySplitterColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            g2DPropertyGrid1.CommandsBackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            g2DPropertyGrid1.CommandsBorderColor = System.Drawing.Color.FromArgb(50, 50, 50);
            g2DPropertyGrid1.CommandsForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            g2DPropertyGrid1.CustomBackColor = null;
            g2DPropertyGrid1.CustomForeColor = null;
            g2DPropertyGrid1.DescriptionAreaHeight = 88;
            g2DPropertyGrid1.DescriptionAreaLineCount = 4;
            g2DPropertyGrid1.DisabledItemForeColor = System.Drawing.Color.Gray;
            g2DPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            g2DPropertyGrid1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            g2DPropertyGrid1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            g2DPropertyGrid1.HelpBackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            g2DPropertyGrid1.HelpBorderColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            g2DPropertyGrid1.HelpForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            g2DPropertyGrid1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            g2DPropertyGrid1.LineColor = System.Drawing.Color.FromArgb(80, 80, 80);
            g2DPropertyGrid1.Location = new System.Drawing.Point(0, 0);
            g2DPropertyGrid1.Margin = new System.Windows.Forms.Padding(8);
            g2DPropertyGrid1.MinDescriptionAreaLineCount = 5;
            g2DPropertyGrid1.Name = "g2DPropertyGrid1";
            g2DPropertyGrid1.SelectedElementDesc = null;
            g2DPropertyGrid1.SelectedField = null;
            g2DPropertyGrid1.SelectedFieldDesc = null;
            g2DPropertyGrid1.SelectedRootObject = null;
            g2DPropertyGrid1.Size = new System.Drawing.Size(434, 973);
            g2DPropertyGrid1.TabIndex = 4;
            g2DPropertyGrid1.ViewBackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            g2DPropertyGrid1.ViewBorderColor = System.Drawing.Color.FromArgb(50, 50, 50);
            g2DPropertyGrid1.ViewForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            // 
            // FormVoxelCrossEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1861, 1071);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "FormVoxelCrossEditor";
            Padding = new System.Windows.Forms.Padding(5, 34, 5, 4);
            Text = "FormVoxelView3D";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OpenTK.WinForms.GLControl glControl1;
        private System.Windows.Forms.Timer timer1;
        private DeepEditor.Common.G2D.G2DBaseToolStrip toolStrip1;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_View;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_File;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Load;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Properties;
        private DeepEditor.Common.G2D.G2DBaseStatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel txt_State;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton chk_MoveBrush;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton btn_Redo2;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton btn_Undo2;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Undo;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Redo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel flowLayoutPanel1;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton chk_MoveEraser;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton chk_2D;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_LoadMeshDX;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_LoadMesh;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_ClearMesh;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_Meshs;
        private DeepEditor.Common.G2D.G2DBaseToolStripButton chk_Test;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private Common.G2D.DataGrid.G2DPropertyGrid g2DPropertyGrid1;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_CombineMeshWeight;
        private System.Windows.Forms.ToolStripMenuItem btn_Optimize;
        private System.Windows.Forms.ToolStripMenuItem btn_makeVoxel2DPlane;
        private System.Windows.Forms.ToolStripMenuItem optimizeVoxels_KeepUpward;
        private System.Windows.Forms.ToolStripMenuItem btn_OptimizeSave;
    }
}