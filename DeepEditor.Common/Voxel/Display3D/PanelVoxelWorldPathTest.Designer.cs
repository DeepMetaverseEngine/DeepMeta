
namespace DeepEditor.Common.Voxel.Display3D
{
    partial class PanelVoxelWorldPathTest
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelVoxelWorldPathTest));
            glControl1 = new OpenTK.WinForms.GLControl();
            timer1 = new System.Windows.Forms.Timer(components);
            toolStrip1 = new G2D.G2DBaseToolStrip();
            menu_File = new G2D.G2DBaseToolStripDropDownButton();
            btn_Save = new G2D.G2DBaseToolStripMenuItem();
            btn_LoadFromBin = new G2D.G2DBaseToolStripMenuItem();
            btn_SaveToBin = new G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            btn_LoadMeshDX = new G2D.G2DBaseToolStripMenuItem();
            btn_LoadMesh = new G2D.G2DBaseToolStripMenuItem();
            btn_ClearMesh = new G2D.G2DBaseToolStripMenuItem();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            btn_Import = new G2D.G2DBaseToolStripMenuItem();
            btn_ImportMagicaVoxel = new G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            btn_Load = new G2D.G2DBaseToolStripMenuItem();
            btn_Properties = new G2D.G2DBaseToolStripMenuItem();
            btn_SavePathCache = new G2D.G2DBaseToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            menu_Meshs = new G2D.G2DBaseToolStripDropDownButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            menu_View = new G2D.G2DBaseToolStripDropDownButton();
            chk_ShowMousePath = new G2D.G2DBaseToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            menu_Object = new G2D.G2DBaseToolStripDropDownButton();
            btn_AddTestActor1 = new G2D.G2DBaseToolStripMenuItem();
            btn_AddTestActor10 = new G2D.G2DBaseToolStripMenuItem();
            btn_AddTestActor100 = new G2D.G2DBaseToolStripMenuItem();
            btn_AddActor = new G2D.G2DBaseToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            chk_2D = new G2D.G2DBaseToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            statusStrip1 = new G2D.G2DBaseStatusStrip();
            txt_State = new System.Windows.Forms.ToolStripStatusLabel();
            txt_Objects = new System.Windows.Forms.ToolStripStatusLabel();
            timer2 = new System.Windows.Forms.Timer(components);
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            g2DPropertyGrid1 = new G2D.DataGrid.G2DPropertyGrid();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // glControl1
            // 
            glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl1.APIVersion = new System.Version(3, 3, 0, 0);
            glControl1.BackColor = System.Drawing.Color.Black;
            glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl1.IsDesignRender = false;
            glControl1.IsEventDriven = true;
            glControl1.Location = new System.Drawing.Point(0, 0);
            glControl1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            glControl1.Name = "glControl1";
            glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            glControl1.Size = new System.Drawing.Size(953, 814);
            glControl1.TabIndex = 0;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 33;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStrip1.CustomBackColor = null;
            toolStrip1.CustomForeColor = null;
            toolStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menu_File, toolStripSeparator1, menu_Meshs, toolStripSeparator5, menu_View, toolStripSeparator2, menu_Object, toolStripSeparator4, chk_2D, toolStripSeparator3, toolStripSeparator6 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            toolStrip1.Size = new System.Drawing.Size(1235, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // menu_File
            // 
            menu_File.CustomBackColor = null;
            menu_File.CustomForeColor = null;
            menu_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_Save, btn_LoadFromBin, btn_SaveToBin, toolStripMenuItem2, btn_LoadMeshDX, btn_LoadMesh, btn_ClearMesh, toolStripSeparator7, btn_Import, toolStripMenuItem1, btn_Load, btn_Properties, btn_SavePathCache });
            menu_File.Image = (System.Drawing.Image)resources.GetObject("menu_File.Image");
            menu_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            menu_File.Name = "menu_File";
            menu_File.Size = new System.Drawing.Size(40, 22);
            menu_File.Text = "File";
            // 
            // btn_Save
            // 
            btn_Save.CustomBackColor = null;
            btn_Save.CustomForeColor = null;
            btn_Save.Enabled = false;
            btn_Save.Name = "btn_Save";
            btn_Save.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            btn_Save.Size = new System.Drawing.Size(171, 22);
            btn_Save.Text = "Save";
            btn_Save.Visible = false;
            btn_Save.Click += Btn_Save_Click;
            // 
            // btn_LoadFromBin
            // 
            btn_LoadFromBin.CustomBackColor = null;
            btn_LoadFromBin.CustomForeColor = null;
            btn_LoadFromBin.Name = "btn_LoadFromBin";
            btn_LoadFromBin.Size = new System.Drawing.Size(171, 22);
            btn_LoadFromBin.Text = "Load From Bin";
            btn_LoadFromBin.Click += btn_LoadFromBin_Click;
            // 
            // btn_SaveToBin
            // 
            btn_SaveToBin.CustomBackColor = null;
            btn_SaveToBin.CustomForeColor = null;
            btn_SaveToBin.Enabled = false;
            btn_SaveToBin.Name = "btn_SaveToBin";
            btn_SaveToBin.Size = new System.Drawing.Size(171, 22);
            btn_SaveToBin.Text = "Save To Bin";
            btn_SaveToBin.Visible = false;
            btn_SaveToBin.Click += btn_SaveToBin_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(168, 6);
            // 
            // btn_LoadMeshDX
            // 
            btn_LoadMeshDX.CustomBackColor = null;
            btn_LoadMeshDX.CustomForeColor = null;
            btn_LoadMeshDX.Name = "btn_LoadMeshDX";
            btn_LoadMeshDX.Size = new System.Drawing.Size(171, 22);
            btn_LoadMeshDX.Text = "Load Mesh DX";
            btn_LoadMeshDX.Click += btn_LoadMeshDX_Click;
            // 
            // btn_LoadMesh
            // 
            btn_LoadMesh.CustomBackColor = null;
            btn_LoadMesh.CustomForeColor = null;
            btn_LoadMesh.Name = "btn_LoadMesh";
            btn_LoadMesh.Size = new System.Drawing.Size(171, 22);
            btn_LoadMesh.Text = "Load Mesh";
            btn_LoadMesh.Click += btn_LoadMesh_Click;
            // 
            // btn_ClearMesh
            // 
            btn_ClearMesh.CustomBackColor = null;
            btn_ClearMesh.CustomForeColor = null;
            btn_ClearMesh.Name = "btn_ClearMesh";
            btn_ClearMesh.Size = new System.Drawing.Size(171, 22);
            btn_ClearMesh.Text = "Clear Mesh";
            btn_ClearMesh.Click += btn_ClearMesh_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(168, 6);
            // 
            // btn_Import
            // 
            btn_Import.CustomBackColor = null;
            btn_Import.CustomForeColor = null;
            btn_Import.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_ImportMagicaVoxel });
            btn_Import.Name = "btn_Import";
            btn_Import.Size = new System.Drawing.Size(171, 22);
            btn_Import.Text = "Import";
            // 
            // btn_ImportMagicaVoxel
            // 
            btn_ImportMagicaVoxel.CustomBackColor = null;
            btn_ImportMagicaVoxel.CustomForeColor = null;
            btn_ImportMagicaVoxel.Name = "btn_ImportMagicaVoxel";
            btn_ImportMagicaVoxel.Size = new System.Drawing.Size(186, 22);
            btn_ImportMagicaVoxel.Text = "MagicaVoxel (.vox)";
            btn_ImportMagicaVoxel.Click += btn_ImportMagicaVoxel_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(168, 6);
            // 
            // btn_Load
            // 
            btn_Load.CustomBackColor = null;
            btn_Load.CustomForeColor = null;
            btn_Load.Enabled = false;
            btn_Load.Name = "btn_Load";
            btn_Load.Size = new System.Drawing.Size(171, 22);
            btn_Load.Text = "Load XML";
            btn_Load.Visible = false;
            btn_Load.Click += btn_Load_Click;
            // 
            // btn_Properties
            // 
            btn_Properties.CustomBackColor = null;
            btn_Properties.CustomForeColor = null;
            btn_Properties.Enabled = false;
            btn_Properties.Name = "btn_Properties";
            btn_Properties.Size = new System.Drawing.Size(171, 22);
            btn_Properties.Text = "Build Config";
            btn_Properties.Visible = false;
            btn_Properties.Click += btn_Properties_Click;
            // 
            // btn_SavePathCache
            // 
            btn_SavePathCache.CustomBackColor = null;
            btn_SavePathCache.CustomForeColor = null;
            btn_SavePathCache.Name = "btn_SavePathCache";
            btn_SavePathCache.Size = new System.Drawing.Size(171, 22);
            btn_SavePathCache.Text = "Save Path Cache";
            btn_SavePathCache.Click += btn_SavePathCache_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // menu_Meshs
            // 
            menu_Meshs.CustomBackColor = null;
            menu_Meshs.CustomForeColor = null;
            menu_Meshs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menu_Meshs.Image = (System.Drawing.Image)resources.GetObject("menu_Meshs.Image");
            menu_Meshs.ImageTransparentColor = System.Drawing.Color.Magenta;
            menu_Meshs.Name = "menu_Meshs";
            menu_Meshs.Size = new System.Drawing.Size(59, 22);
            menu_Meshs.Text = "Meshs";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // menu_View
            // 
            menu_View.CustomBackColor = null;
            menu_View.CustomForeColor = null;
            menu_View.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { chk_ShowMousePath });
            menu_View.Image = (System.Drawing.Image)resources.GetObject("menu_View.Image");
            menu_View.ImageTransparentColor = System.Drawing.Color.Magenta;
            menu_View.Name = "menu_View";
            menu_View.Size = new System.Drawing.Size(48, 22);
            menu_View.Text = "View";
            // 
            // chk_ShowMousePath
            // 
            chk_ShowMousePath.Checked = true;
            chk_ShowMousePath.CheckOnClick = true;
            chk_ShowMousePath.CheckState = System.Windows.Forms.CheckState.Checked;
            chk_ShowMousePath.CustomBackColor = null;
            chk_ShowMousePath.CustomForeColor = null;
            chk_ShowMousePath.Name = "chk_ShowMousePath";
            chk_ShowMousePath.Size = new System.Drawing.Size(148, 22);
            chk_ShowMousePath.Text = "显示鼠标寻路";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // menu_Object
            // 
            menu_Object.CustomBackColor = null;
            menu_Object.CustomForeColor = null;
            menu_Object.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menu_Object.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btn_AddTestActor1, btn_AddTestActor10, btn_AddTestActor100, btn_AddActor });
            menu_Object.Image = (System.Drawing.Image)resources.GetObject("menu_Object.Image");
            menu_Object.ImageTransparentColor = System.Drawing.Color.Magenta;
            menu_Object.Name = "menu_Object";
            menu_Object.Size = new System.Drawing.Size(59, 22);
            menu_Object.Text = "Object";
            // 
            // btn_AddTestActor1
            // 
            btn_AddTestActor1.CustomBackColor = null;
            btn_AddTestActor1.CustomForeColor = null;
            btn_AddTestActor1.Name = "btn_AddTestActor1";
            btn_AddTestActor1.ShortcutKeys = System.Windows.Forms.Keys.F2;
            btn_AddTestActor1.Size = new System.Drawing.Size(205, 22);
            btn_AddTestActor1.Text = "Add TestActor 1";
            btn_AddTestActor1.Click += btn_AddTestActor_Click;
            // 
            // btn_AddTestActor10
            // 
            btn_AddTestActor10.CustomBackColor = null;
            btn_AddTestActor10.CustomForeColor = null;
            btn_AddTestActor10.Name = "btn_AddTestActor10";
            btn_AddTestActor10.ShortcutKeys = System.Windows.Forms.Keys.F3;
            btn_AddTestActor10.Size = new System.Drawing.Size(205, 22);
            btn_AddTestActor10.Text = "Add TestActor 10";
            btn_AddTestActor10.Click += btn_AddTestActor10_Click;
            // 
            // btn_AddTestActor100
            // 
            btn_AddTestActor100.CustomBackColor = null;
            btn_AddTestActor100.CustomForeColor = null;
            btn_AddTestActor100.Name = "btn_AddTestActor100";
            btn_AddTestActor100.ShortcutKeys = System.Windows.Forms.Keys.F4;
            btn_AddTestActor100.Size = new System.Drawing.Size(205, 22);
            btn_AddTestActor100.Text = "Add TestActor 100";
            btn_AddTestActor100.Click += btn_AddTestActor100_Click;
            // 
            // btn_AddActor
            // 
            btn_AddActor.CustomBackColor = null;
            btn_AddActor.CustomForeColor = null;
            btn_AddActor.Name = "btn_AddActor";
            btn_AddActor.Size = new System.Drawing.Size(205, 22);
            btn_AddActor.Text = "Add Actor";
            btn_AddActor.Click += btn_AddActor_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // chk_2D
            // 
            chk_2D.CheckOnClick = true;
            chk_2D.CustomBackColor = null;
            chk_2D.CustomForeColor = null;
            chk_2D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            chk_2D.Image = (System.Drawing.Image)resources.GetObject("chk_2D.Image");
            chk_2D.ImageTransparentColor = System.Drawing.Color.Magenta;
            chk_2D.Name = "chk_2D";
            chk_2D.Size = new System.Drawing.Size(28, 22);
            chk_2D.Text = "2D";
            chk_2D.Click += chk_2D_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            statusStrip1.CustomBackColor = null;
            statusStrip1.CustomForeColor = null;
            statusStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { txt_State, txt_Objects });
            statusStrip1.Location = new System.Drawing.Point(0, 839);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 11, 0);
            statusStrip1.Size = new System.Drawing.Size(1235, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // txt_State
            // 
            txt_State.Name = "txt_State";
            txt_State.Size = new System.Drawing.Size(40, 17);
            txt_State.Text = "        ";
            // 
            // txt_Objects
            // 
            txt_Objects.Name = "txt_Objects";
            txt_Objects.Size = new System.Drawing.Size(15, 17);
            txt_Objects.Text = "0";
            // 
            // timer2
            // 
            timer2.Enabled = true;
            timer2.Interval = 3000;
            timer2.Tick += timer2_Tick;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.Location = new System.Drawing.Point(0, 25);
            splitContainer1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(glControl1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(g2DPropertyGrid1);
            splitContainer1.Size = new System.Drawing.Size(1235, 814);
            splitContainer1.SplitterDistance = 953;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 3;
            // 
            // g2DPropertyGrid1
            // 
            g2DPropertyGrid1.CustomBackColor = null;
            g2DPropertyGrid1.CustomForeColor = null;
            g2DPropertyGrid1.DescriptionAreaHeight = 59;
            g2DPropertyGrid1.DescriptionAreaLineCount = 3;
            g2DPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            g2DPropertyGrid1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            g2DPropertyGrid1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            g2DPropertyGrid1.Location = new System.Drawing.Point(0, 0);
            g2DPropertyGrid1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            g2DPropertyGrid1.MinDescriptionAreaLineCount = 5;
            g2DPropertyGrid1.Name = "g2DPropertyGrid1";
            g2DPropertyGrid1.Size = new System.Drawing.Size(279, 814);
            g2DPropertyGrid1.TabIndex = 0;
            // 
            // PanelVoxelWorldPathTest
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            Name = "PanelVoxelWorldPathTest";
            Size = new System.Drawing.Size(1235, 861);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_AddTestActor1;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_AddTestActor10;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_AddTestActor100;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Load;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_LoadFromBin;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Properties;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Save;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_SavePathCache;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_SaveToBin;
        private DeepEditor.Common.G2D.DataGrid.G2DPropertyGrid g2DPropertyGrid1;
        private OpenTK.WinForms.GLControl glControl1;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_File;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_Object;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_View;
        private System.Windows.Forms.SplitContainer splitContainer1;
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

        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_AddActor;
    }
}