
namespace DeepEditor.Plugin3D.BattleClient
{
    partial class PanelBattleView3D
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelBattleView3D));
            glControl = new OpenTK.WinForms.GLControl();
            toolStrip1 = new DeepEditor.Common.G2D.G2DBaseToolStrip();
            menu_View = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            toolStripSeparator2 = new ToolStripSeparator();
            menu_Mesh = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            btn_LoadMesh = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_LoadMeshDX = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            menu_Meshs = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            toolStripSeparator9 = new ToolStripSeparator();
            chk_2D = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btn_Running = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            btn_Step = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            btn_AutoAttack = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            menu_Turbo = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            btn_0_1X = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_0_5X = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            btn_1X = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_2X = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_3X = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_4X = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_5X = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_10X = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            btn_TurboX = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            fixedUpdateToolStripMenuItem = new ToolStripMenuItem();
            fixedTimeIntervalToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            btn_RunPauseTool = new ToolStripMenuItem();
            btn_StepTool = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            drop_SyncMode = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            toolStripSeparator6 = new ToolStripSeparator();
            btn_SkipClientEvent = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripSeparator11 = new ToolStripSeparator();
            btn_EventDebug = new ToolStripButton();
            toolStripSeparator7 = new ToolStripSeparator();
            menu_Function = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            btn_IsAutoFocusTarget = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_IgnoreScriptEvent = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_CleanBuff = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            pickUnitToolStripMenuItem = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            pickItemToolStripMenuItem = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            pickNearItemToolStripMenuItem = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            followUnitToolStripMenuItem = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            followUnitGuardToolStripMenuItem = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_TestClientCustom = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_QuestTest = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_QuestAccpetR2B = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_QuestStatusChangeR2B = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_QuestCommitR2B = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_QuestDropR2B = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            gCToolStripMenuItem = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_NetView = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_Help = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripSeparator8 = new ToolStripSeparator();
            btn_Exit = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripLabel1 = new ToolStripLabel();
            txt_Filter = new ToolStripTextBox();
            toolStripSeparator10 = new ToolStripSeparator();
            statusStripActor = new DeepEditor.Common.G2D.G2DBaseStatusStrip();
            txt_ActorInfo = new ToolStripStatusLabel();
            timerInfo = new System.Windows.Forms.Timer(components);
            splitGLView = new SplitContainer();
            toolStrip1.SuspendLayout();
            statusStripActor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitGLView).BeginInit();
            splitGLView.Panel1.SuspendLayout();
            splitGLView.SuspendLayout();
            SuspendLayout();
            // 
            // glControl
            // 
            glControl.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl.APIVersion = new Version(3, 3, 0, 0);
            glControl.BackColor = Color.Black;
            glControl.Dock = DockStyle.Fill;
            glControl.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl.IsDesignRender = false;
            glControl.IsEventDriven = true;
            glControl.Location = new Point(0, 0);
            glControl.Margin = new Padding(5, 6, 5, 6);
            glControl.Name = "glControl";
            glControl.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            glControl.Size = new Size(1798, 1287);
            glControl.TabIndex = 0;
            glControl.KeyDown += GlControl1_KeyDown;
            glControl.KeyUp += GlControl1_KeyUp;
            glControl.MouseClick += GlControl1_MouseClick;
            glControl.MouseDown += GlControl1_MouseDown;
            glControl.MouseMove += GlControl1_MouseMove;
            glControl.MouseUp += GlControl1_MouseUp;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.FromArgb(242, 242, 242);
            toolStrip1.CustomBackColor = null;
            toolStrip1.CustomForeColor = null;
            toolStrip1.Font = new Font("Microsoft YaHei UI", 9F);
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { menu_View, toolStripSeparator2, menu_Mesh, toolStripSeparator9, chk_2D, toolStripSeparator1, btn_Running, btn_Step, toolStripSeparator3, btn_AutoAttack, toolStripSeparator4, menu_Turbo, toolStripSeparator5, drop_SyncMode, toolStripSeparator6, btn_SkipClientEvent, toolStripSeparator11, btn_EventDebug, toolStripSeparator7, menu_Function, btn_Help, toolStripSeparator8, btn_Exit, toolStripLabel1, txt_Filter, toolStripSeparator10 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(0, 0, 3, 0);
            toolStrip1.Size = new Size(1798, 33);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // menu_View
            // 
            menu_View.CustomBackColor = null;
            menu_View.CustomForeColor = null;
            menu_View.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menu_View.Image = (Image)resources.GetObject("menu_View.Image");
            menu_View.ImageOrigin = null;
            menu_View.ImageTransparentColor = Color.Magenta;
            menu_View.Name = "menu_View";
            menu_View.Size = new Size(69, 28);
            menu_View.Text = "View";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 33);
            // 
            // menu_Mesh
            // 
            menu_Mesh.CustomBackColor = null;
            menu_Mesh.CustomForeColor = null;
            menu_Mesh.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menu_Mesh.DropDownItems.AddRange(new ToolStripItem[] { btn_LoadMesh, btn_LoadMeshDX, menu_Meshs });
            menu_Mesh.Image = (Image)resources.GetObject("menu_Mesh.Image");
            menu_Mesh.ImageOrigin = null;
            menu_Mesh.ImageTransparentColor = Color.Magenta;
            menu_Mesh.Name = "menu_Mesh";
            menu_Mesh.Size = new Size(75, 28);
            menu_Mesh.Text = "Mesh";
            // 
            // btn_LoadMesh
            // 
            btn_LoadMesh.CustomBackColor = null;
            btn_LoadMesh.CustomForeColor = null;
            btn_LoadMesh.ImageOrigin = null;
            btn_LoadMesh.Name = "btn_LoadMesh";
            btn_LoadMesh.Size = new Size(241, 34);
            btn_LoadMesh.Text = "加载OBJ";
            btn_LoadMesh.Click += btn_LoadMesh_Click;
            // 
            // btn_LoadMeshDX
            // 
            btn_LoadMeshDX.CustomBackColor = null;
            btn_LoadMeshDX.CustomForeColor = null;
            btn_LoadMeshDX.ImageOrigin = null;
            btn_LoadMeshDX.Name = "btn_LoadMeshDX";
            btn_LoadMeshDX.Size = new Size(241, 34);
            btn_LoadMeshDX.Text = "加载OBJ（DX）";
            btn_LoadMeshDX.Click += btn_LoadMeshDX_Click;
            // 
            // menu_Meshs
            // 
            menu_Meshs.CustomBackColor = null;
            menu_Meshs.CustomForeColor = null;
            menu_Meshs.ImageOrigin = null;
            menu_Meshs.Name = "menu_Meshs";
            menu_Meshs.Size = new Size(241, 34);
            menu_Meshs.Text = "Meshs";
            menu_Meshs.Click += menu_Meshs_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(6, 33);
            // 
            // chk_2D
            // 
            chk_2D.CheckOnClick = true;
            chk_2D.CustomBackColor = null;
            chk_2D.CustomForeColor = null;
            chk_2D.DisplayStyle = ToolStripItemDisplayStyle.Text;
            chk_2D.Image = (Image)resources.GetObject("chk_2D.Image");
            chk_2D.ImageOrigin = null;
            chk_2D.ImageTransparentColor = Color.Magenta;
            chk_2D.Name = "chk_2D";
            chk_2D.Size = new Size(39, 28);
            chk_2D.Text = "2D";
            chk_2D.Click += Chk_2D_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 33);
            // 
            // btn_Running
            // 
            btn_Running.AutoSize = false;
            btn_Running.CheckOnClick = true;
            btn_Running.CustomBackColor = null;
            btn_Running.CustomForeColor = null;
            btn_Running.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_Running.Image = (Image)resources.GetObject("btn_Running.Image");
            btn_Running.ImageOrigin = (Image)resources.GetObject("btn_Running.ImageOrigin");
            btn_Running.ImageTransparentColor = Color.Magenta;
            btn_Running.Name = "btn_Running";
            btn_Running.Size = new Size(28, 28);
            btn_Running.Text = "Running（F9）";
            btn_Running.Click += btn_Running_Click;
            // 
            // btn_Step
            // 
            btn_Step.CustomBackColor = null;
            btn_Step.CustomForeColor = null;
            btn_Step.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_Step.Image = (Image)resources.GetObject("btn_Step.Image");
            btn_Step.ImageOrigin = (Image)resources.GetObject("btn_Step.ImageOrigin");
            btn_Step.ImageTransparentColor = Color.Magenta;
            btn_Step.Name = "btn_Step";
            btn_Step.Size = new Size(34, 28);
            btn_Step.Text = "单步执行（F10）";
            btn_Step.Click += btn_Step_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 33);
            // 
            // btn_AutoAttack
            // 
            btn_AutoAttack.CheckOnClick = true;
            btn_AutoAttack.CustomBackColor = null;
            btn_AutoAttack.CustomForeColor = null;
            btn_AutoAttack.Image = (Image)resources.GetObject("btn_AutoAttack.Image");
            btn_AutoAttack.ImageOrigin = null;
            btn_AutoAttack.ImageScaling = ToolStripItemImageScaling.None;
            btn_AutoAttack.ImageTransparentColor = Color.Magenta;
            btn_AutoAttack.Name = "btn_AutoAttack";
            btn_AutoAttack.Size = new Size(66, 28);
            btn_AutoAttack.Text = "托管";
            btn_AutoAttack.Click += btn_AutoAttack_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 33);
            // 
            // menu_Turbo
            // 
            menu_Turbo.CustomBackColor = null;
            menu_Turbo.CustomForeColor = null;
            menu_Turbo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menu_Turbo.DropDownItems.AddRange(new ToolStripItem[] { btn_0_1X, btn_0_5X, toolStripMenuItem1, btn_1X, btn_2X, btn_3X, btn_4X, btn_5X, btn_10X, btn_TurboX, toolStripMenuItem2, fixedUpdateToolStripMenuItem, fixedTimeIntervalToolStripMenuItem, toolStripMenuItem3, btn_RunPauseTool, btn_StepTool });
            menu_Turbo.ImageOrigin = null;
            menu_Turbo.ImageTransparentColor = Color.Magenta;
            menu_Turbo.Name = "menu_Turbo";
            menu_Turbo.Size = new Size(64, 28);
            menu_Turbo.Text = "加速";
            // 
            // btn_0_1X
            // 
            btn_0_1X.CustomBackColor = null;
            btn_0_1X.CustomForeColor = null;
            btn_0_1X.ImageOrigin = null;
            btn_0_1X.Name = "btn_0_1X";
            btn_0_1X.Size = new Size(270, 34);
            btn_0_1X.Text = "0.1X";
            btn_0_1X.Click += btn_0_1X_Click;
            // 
            // btn_0_5X
            // 
            btn_0_5X.CustomBackColor = null;
            btn_0_5X.CustomForeColor = null;
            btn_0_5X.ImageOrigin = null;
            btn_0_5X.Name = "btn_0_5X";
            btn_0_5X.Size = new Size(270, 34);
            btn_0_5X.Text = "0.5X";
            btn_0_5X.Click += btn_0_5X_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(267, 6);
            // 
            // btn_1X
            // 
            btn_1X.CustomBackColor = null;
            btn_1X.CustomForeColor = null;
            btn_1X.ImageOrigin = null;
            btn_1X.Name = "btn_1X";
            btn_1X.Size = new Size(270, 34);
            btn_1X.Text = "1X";
            btn_1X.Click += btn_1X_Click;
            // 
            // btn_2X
            // 
            btn_2X.CustomBackColor = null;
            btn_2X.CustomForeColor = null;
            btn_2X.ImageOrigin = null;
            btn_2X.Name = "btn_2X";
            btn_2X.Size = new Size(270, 34);
            btn_2X.Text = "2X";
            btn_2X.Click += btn_2X_Click;
            // 
            // btn_3X
            // 
            btn_3X.CustomBackColor = null;
            btn_3X.CustomForeColor = null;
            btn_3X.ImageOrigin = null;
            btn_3X.Name = "btn_3X";
            btn_3X.Size = new Size(270, 34);
            btn_3X.Text = "3X";
            btn_3X.Click += btn_3X_Click;
            // 
            // btn_4X
            // 
            btn_4X.CustomBackColor = null;
            btn_4X.CustomForeColor = null;
            btn_4X.ImageOrigin = null;
            btn_4X.Name = "btn_4X";
            btn_4X.Size = new Size(270, 34);
            btn_4X.Text = "4X";
            btn_4X.Click += btn_4X_Click;
            // 
            // btn_5X
            // 
            btn_5X.CustomBackColor = null;
            btn_5X.CustomForeColor = null;
            btn_5X.ImageOrigin = null;
            btn_5X.Name = "btn_5X";
            btn_5X.Size = new Size(270, 34);
            btn_5X.Text = "5X";
            btn_5X.Click += btn_5X_Click;
            // 
            // btn_10X
            // 
            btn_10X.CustomBackColor = null;
            btn_10X.CustomForeColor = null;
            btn_10X.ImageOrigin = null;
            btn_10X.Name = "btn_10X";
            btn_10X.Size = new Size(270, 34);
            btn_10X.Text = "10X";
            btn_10X.Click += btn_10X_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(267, 6);
            // 
            // btn_TurboX
            // 
            btn_TurboX.CustomBackColor = null;
            btn_TurboX.CustomForeColor = null;
            btn_TurboX.ImageOrigin = null;
            btn_TurboX.Name = "btn_TurboX";
            btn_TurboX.Size = new Size(270, 34);
            btn_TurboX.Text = "自定义速度倍率";
            btn_TurboX.Click += btn_TurboX_ToolStripMenuItem_Click;
            // 
            // fixedUpdateToolStripMenuItem
            // 
            fixedUpdateToolStripMenuItem.CheckOnClick = true;
            fixedUpdateToolStripMenuItem.Name = "fixedUpdateToolStripMenuItem";
            fixedUpdateToolStripMenuItem.Size = new Size(270, 34);
            fixedUpdateToolStripMenuItem.Text = "Fixed Update";
            fixedUpdateToolStripMenuItem.CheckedChanged += fixedUpdateToolStripMenuItem_CheckedChanged;
            fixedUpdateToolStripMenuItem.Click += fixedUpdateToolStripMenuItem_Click;
            // 
            // fixedTimeIntervalToolStripMenuItem
            // 
            fixedTimeIntervalToolStripMenuItem.Name = "fixedTimeIntervalToolStripMenuItem";
            fixedTimeIntervalToolStripMenuItem.Size = new Size(270, 34);
            fixedTimeIntervalToolStripMenuItem.Text = "Fixed FPS";
            fixedTimeIntervalToolStripMenuItem.Click += fixedTimeIntervalToolStripMenuItem_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(267, 6);
            // 
            // btn_RunPauseTool
            // 
            btn_RunPauseTool.CheckOnClick = true;
            btn_RunPauseTool.Name = "btn_RunPauseTool";
            btn_RunPauseTool.ShortcutKeys = Keys.F9;
            btn_RunPauseTool.Size = new Size(270, 34);
            btn_RunPauseTool.Text = "Run/Pasuse";
            btn_RunPauseTool.Click += btn_Running_Click;
            // 
            // btn_StepTool
            // 
            btn_StepTool.Name = "btn_StepTool";
            btn_StepTool.ShortcutKeys = Keys.F10;
            btn_StepTool.Size = new Size(270, 34);
            btn_StepTool.Text = "Step";
            btn_StepTool.Click += btn_Step_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 33);
            // 
            // drop_SyncMode
            // 
            drop_SyncMode.CustomBackColor = null;
            drop_SyncMode.CustomForeColor = null;
            drop_SyncMode.DisplayStyle = ToolStripItemDisplayStyle.Text;
            drop_SyncMode.Image = (Image)resources.GetObject("drop_SyncMode.Image");
            drop_SyncMode.ImageOrigin = null;
            drop_SyncMode.ImageTransparentColor = Color.Magenta;
            drop_SyncMode.Name = "drop_SyncMode";
            drop_SyncMode.Size = new Size(100, 28);
            drop_SyncMode.Text = "同步模式";
            drop_SyncMode.Click += item_SyncMode_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 33);
            // 
            // btn_SkipClientEvent
            // 
            btn_SkipClientEvent.CustomBackColor = null;
            btn_SkipClientEvent.CustomForeColor = null;
            btn_SkipClientEvent.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btn_SkipClientEvent.Image = (Image)resources.GetObject("btn_SkipClientEvent.Image");
            btn_SkipClientEvent.ImageOrigin = null;
            btn_SkipClientEvent.ImageTransparentColor = Color.Magenta;
            btn_SkipClientEvent.Name = "btn_SkipClientEvent";
            btn_SkipClientEvent.Size = new Size(136, 28);
            btn_SkipClientEvent.Text = "跳过Client事件";
            btn_SkipClientEvent.Click += btn_SkipClientEvent_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(6, 33);
            // 
            // btn_EventDebug
            // 
            btn_EventDebug.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_EventDebug.Enabled = false;
            btn_EventDebug.Image = (Image)resources.GetObject("btn_EventDebug.Image");
            btn_EventDebug.ImageTransparentColor = Color.Magenta;
            btn_EventDebug.Name = "btn_EventDebug";
            btn_EventDebug.Size = new Size(34, 28);
            btn_EventDebug.Text = "Event Debug";
            btn_EventDebug.Click += btn_EventDebug_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(6, 33);
            // 
            // menu_Function
            // 
            menu_Function.CustomBackColor = null;
            menu_Function.CustomForeColor = null;
            menu_Function.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menu_Function.DropDownItems.AddRange(new ToolStripItem[] { btn_IsAutoFocusTarget, btn_IgnoreScriptEvent, btn_CleanBuff, pickUnitToolStripMenuItem, pickItemToolStripMenuItem, pickNearItemToolStripMenuItem, followUnitToolStripMenuItem, followUnitGuardToolStripMenuItem, btn_TestClientCustom, btn_QuestTest, toolStripMenuItem4, gCToolStripMenuItem, btn_NetView });
            menu_Function.Image = (Image)resources.GetObject("menu_Function.Image");
            menu_Function.ImageOrigin = null;
            menu_Function.ImageTransparentColor = Color.Magenta;
            menu_Function.Name = "menu_Function";
            menu_Function.Size = new Size(64, 28);
            menu_Function.Text = "功能";
            // 
            // btn_IsAutoFocusTarget
            // 
            btn_IsAutoFocusTarget.Checked = true;
            btn_IsAutoFocusTarget.CheckOnClick = true;
            btn_IsAutoFocusTarget.CheckState = CheckState.Checked;
            btn_IsAutoFocusTarget.CustomBackColor = null;
            btn_IsAutoFocusTarget.CustomForeColor = null;
            btn_IsAutoFocusTarget.ImageOrigin = null;
            btn_IsAutoFocusTarget.Name = "btn_IsAutoFocusTarget";
            btn_IsAutoFocusTarget.Size = new Size(255, 34);
            btn_IsAutoFocusTarget.Text = "自动瞄准";
            btn_IsAutoFocusTarget.Click += btn_IsAutoFocusTarget_ToolStripMenuItem_Click;
            // 
            // btn_IgnoreScriptEvent
            // 
            btn_IgnoreScriptEvent.CustomBackColor = null;
            btn_IgnoreScriptEvent.CustomForeColor = null;
            btn_IgnoreScriptEvent.ImageOrigin = null;
            btn_IgnoreScriptEvent.Name = "btn_IgnoreScriptEvent";
            btn_IgnoreScriptEvent.Size = new Size(255, 34);
            btn_IgnoreScriptEvent.Text = "忽略ScriptEvent";
            btn_IgnoreScriptEvent.Click += btn_IgnoreScriptEvent_Click;
            // 
            // btn_CleanBuff
            // 
            btn_CleanBuff.CustomBackColor = null;
            btn_CleanBuff.CustomForeColor = null;
            btn_CleanBuff.ImageOrigin = null;
            btn_CleanBuff.Name = "btn_CleanBuff";
            btn_CleanBuff.Size = new Size(255, 34);
            btn_CleanBuff.Text = "清除BUFF";
            btn_CleanBuff.Click += btn_CleanBuff_Click;
            // 
            // pickUnitToolStripMenuItem
            // 
            pickUnitToolStripMenuItem.CustomBackColor = null;
            pickUnitToolStripMenuItem.CustomForeColor = null;
            pickUnitToolStripMenuItem.ImageOrigin = null;
            pickUnitToolStripMenuItem.Name = "pickUnitToolStripMenuItem";
            pickUnitToolStripMenuItem.Size = new Size(255, 34);
            pickUnitToolStripMenuItem.Text = "PickUnit";
            pickUnitToolStripMenuItem.Click += pickUnitToolStripMenuItem_Click;
            // 
            // pickItemToolStripMenuItem
            // 
            pickItemToolStripMenuItem.CustomBackColor = null;
            pickItemToolStripMenuItem.CustomForeColor = null;
            pickItemToolStripMenuItem.ImageOrigin = null;
            pickItemToolStripMenuItem.Name = "pickItemToolStripMenuItem";
            pickItemToolStripMenuItem.Size = new Size(255, 34);
            pickItemToolStripMenuItem.Text = "PickItem";
            pickItemToolStripMenuItem.Click += PickItemToolStripMenuItem_Click;
            // 
            // pickNearItemToolStripMenuItem
            // 
            pickNearItemToolStripMenuItem.CustomBackColor = null;
            pickNearItemToolStripMenuItem.CustomForeColor = null;
            pickNearItemToolStripMenuItem.ImageOrigin = null;
            pickNearItemToolStripMenuItem.Name = "pickNearItemToolStripMenuItem";
            pickNearItemToolStripMenuItem.Size = new Size(255, 34);
            pickNearItemToolStripMenuItem.Text = "PickNearItem";
            pickNearItemToolStripMenuItem.Click += PickNearItemToolStripMenuItem_Click;
            // 
            // followUnitToolStripMenuItem
            // 
            followUnitToolStripMenuItem.CustomBackColor = null;
            followUnitToolStripMenuItem.CustomForeColor = null;
            followUnitToolStripMenuItem.ImageOrigin = null;
            followUnitToolStripMenuItem.Name = "followUnitToolStripMenuItem";
            followUnitToolStripMenuItem.Size = new Size(255, 34);
            followUnitToolStripMenuItem.Text = "FollowUnit";
            followUnitToolStripMenuItem.Click += followUnitToolStripMenuItem_Click;
            // 
            // followUnitGuardToolStripMenuItem
            // 
            followUnitGuardToolStripMenuItem.CustomBackColor = null;
            followUnitGuardToolStripMenuItem.CustomForeColor = null;
            followUnitGuardToolStripMenuItem.ImageOrigin = null;
            followUnitGuardToolStripMenuItem.Name = "followUnitGuardToolStripMenuItem";
            followUnitGuardToolStripMenuItem.Size = new Size(255, 34);
            followUnitGuardToolStripMenuItem.Text = "FollowUnitGuard";
            followUnitGuardToolStripMenuItem.Click += followUnitGuardToolStripMenuItem_Click;
            // 
            // btn_TestClientCustom
            // 
            btn_TestClientCustom.CustomBackColor = null;
            btn_TestClientCustom.CustomForeColor = null;
            btn_TestClientCustom.ImageOrigin = null;
            btn_TestClientCustom.Name = "btn_TestClientCustom";
            btn_TestClientCustom.Size = new Size(255, 34);
            btn_TestClientCustom.Text = "模拟轻功";
            btn_TestClientCustom.Click += btn_TestClientCustom_Click;
            // 
            // btn_QuestTest
            // 
            btn_QuestTest.CustomBackColor = null;
            btn_QuestTest.CustomForeColor = null;
            btn_QuestTest.DropDownItems.AddRange(new ToolStripItem[] { btn_QuestAccpetR2B, btn_QuestStatusChangeR2B, btn_QuestCommitR2B, btn_QuestDropR2B });
            btn_QuestTest.ImageOrigin = null;
            btn_QuestTest.Name = "btn_QuestTest";
            btn_QuestTest.Size = new Size(255, 34);
            btn_QuestTest.Text = "模拟任务";
            // 
            // btn_QuestAccpetR2B
            // 
            btn_QuestAccpetR2B.CustomBackColor = null;
            btn_QuestAccpetR2B.CustomForeColor = null;
            btn_QuestAccpetR2B.ImageOrigin = null;
            btn_QuestAccpetR2B.Name = "btn_QuestAccpetR2B";
            btn_QuestAccpetR2B.Size = new Size(272, 34);
            btn_QuestAccpetR2B.Text = "游戏服接取任务";
            btn_QuestAccpetR2B.Click += btn_QuestAccpetR2B_Click;
            // 
            // btn_QuestStatusChangeR2B
            // 
            btn_QuestStatusChangeR2B.CustomBackColor = null;
            btn_QuestStatusChangeR2B.CustomForeColor = null;
            btn_QuestStatusChangeR2B.ImageOrigin = null;
            btn_QuestStatusChangeR2B.Name = "btn_QuestStatusChangeR2B";
            btn_QuestStatusChangeR2B.Size = new Size(272, 34);
            btn_QuestStatusChangeR2B.Text = "游戏服改变任务状态";
            btn_QuestStatusChangeR2B.Click += btn_QuestStatusChangeR2B_Click;
            // 
            // btn_QuestCommitR2B
            // 
            btn_QuestCommitR2B.CustomBackColor = null;
            btn_QuestCommitR2B.CustomForeColor = null;
            btn_QuestCommitR2B.ImageOrigin = null;
            btn_QuestCommitR2B.Name = "btn_QuestCommitR2B";
            btn_QuestCommitR2B.Size = new Size(272, 34);
            btn_QuestCommitR2B.Text = "游戏服提交任务";
            btn_QuestCommitR2B.Click += btn_QuestCommitR2B_Click;
            // 
            // btn_QuestDropR2B
            // 
            btn_QuestDropR2B.CustomBackColor = null;
            btn_QuestDropR2B.CustomForeColor = null;
            btn_QuestDropR2B.ImageOrigin = null;
            btn_QuestDropR2B.Name = "btn_QuestDropR2B";
            btn_QuestDropR2B.Size = new Size(272, 34);
            btn_QuestDropR2B.Text = "游戏服放弃任务";
            btn_QuestDropR2B.Click += btn_QuestDropR2B_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(252, 6);
            // 
            // gCToolStripMenuItem
            // 
            gCToolStripMenuItem.CustomBackColor = null;
            gCToolStripMenuItem.CustomForeColor = null;
            gCToolStripMenuItem.ImageOrigin = null;
            gCToolStripMenuItem.Name = "gCToolStripMenuItem";
            gCToolStripMenuItem.ShortcutKeys = Keys.F2;
            gCToolStripMenuItem.Size = new Size(255, 34);
            gCToolStripMenuItem.Text = "GC";
            gCToolStripMenuItem.Click += gCToolStripMenuItem_Click;
            // 
            // btn_NetView
            // 
            btn_NetView.CustomBackColor = null;
            btn_NetView.CustomForeColor = null;
            btn_NetView.ImageOrigin = null;
            btn_NetView.Name = "btn_NetView";
            btn_NetView.Size = new Size(255, 34);
            btn_NetView.Text = "NetView";
            btn_NetView.Click += btn_NetView_Click;
            // 
            // btn_Help
            // 
            btn_Help.CustomBackColor = null;
            btn_Help.CustomForeColor = null;
            btn_Help.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btn_Help.Enabled = false;
            btn_Help.Image = (Image)resources.GetObject("btn_Help.Image");
            btn_Help.ImageOrigin = null;
            btn_Help.ImageTransparentColor = Color.Magenta;
            btn_Help.Name = "btn_Help";
            btn_Help.Size = new Size(50, 28);
            btn_Help.Text = "帮助";
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(6, 33);
            // 
            // btn_Exit
            // 
            btn_Exit.Alignment = ToolStripItemAlignment.Right;
            btn_Exit.CustomBackColor = null;
            btn_Exit.CustomForeColor = null;
            btn_Exit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_Exit.Image = (Image)resources.GetObject("btn_Exit.Image");
            btn_Exit.ImageOrigin = null;
            btn_Exit.ImageTransparentColor = Color.Magenta;
            btn_Exit.Name = "btn_Exit";
            btn_Exit.Size = new Size(34, 28);
            btn_Exit.Text = "Disconnect And Exit";
            btn_Exit.Visible = false;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(58, 28);
            toolStripLabel1.Text = "Filter:";
            // 
            // txt_Filter
            // 
            txt_Filter.Name = "txt_Filter";
            txt_Filter.Size = new Size(200, 33);
            txt_Filter.TextChanged += txt_Filter_TextChanged;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(6, 33);
            // 
            // statusStripActor
            // 
            statusStripActor.BackColor = Color.FromArgb(242, 242, 242);
            statusStripActor.CustomBackColor = null;
            statusStripActor.CustomForeColor = null;
            statusStripActor.Font = new Font("Microsoft YaHei UI", 9F);
            statusStripActor.ImageScalingSize = new Size(24, 24);
            statusStripActor.Items.AddRange(new ToolStripItem[] { txt_ActorInfo });
            statusStripActor.Location = new Point(0, 1320);
            statusStripActor.Name = "statusStripActor";
            statusStripActor.Padding = new Padding(2, 0, 17, 0);
            statusStripActor.Size = new Size(1798, 31);
            statusStripActor.TabIndex = 2;
            statusStripActor.Text = "statusStrip1";
            // 
            // txt_ActorInfo
            // 
            txt_ActorInfo.Name = "txt_ActorInfo";
            txt_ActorInfo.Size = new Size(72, 24);
            txt_ActorInfo.Text = "ACTOR";
            // 
            // timerInfo
            // 
            timerInfo.Enabled = true;
            timerInfo.Interval = 1000;
            timerInfo.Tick += TimerInfo_Tick;
            // 
            // splitGLView
            // 
            splitGLView.Dock = DockStyle.Fill;
            splitGLView.FixedPanel = FixedPanel.Panel2;
            splitGLView.Location = new Point(0, 33);
            splitGLView.Name = "splitGLView";
            splitGLView.Orientation = Orientation.Horizontal;
            // 
            // splitGLView.Panel1
            // 
            splitGLView.Panel1.Controls.Add(glControl);
            splitGLView.Panel1MinSize = 250;
            splitGLView.Panel2Collapsed = true;
            splitGLView.Panel2MinSize = 250;
            splitGLView.Size = new Size(1798, 1287);
            splitGLView.SplitterDistance = 250;
            splitGLView.TabIndex = 3;
            // 
            // PanelBattleView3D
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitGLView);
            Controls.Add(statusStripActor);
            Controls.Add(toolStrip1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "PanelBattleView3D";
            Size = new Size(1798, 1351);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStripActor.ResumeLayout(false);
            statusStripActor.PerformLayout();
            splitGLView.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitGLView).EndInit();
            splitGLView.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public OpenTK.WinForms.GLControl glControl;
        public DeepEditor.Common.G2D.G2DBaseToolStrip toolStrip1;
        public DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_View;
        public ToolStripSeparator toolStripSeparator1;
        public ToolStripSeparator toolStripSeparator2;
        public DeepEditor.Common.G2D.G2DBaseToolStripButton chk_2D;
        public ToolStripSeparator toolStripSeparator3;
        public DeepEditor.Common.G2D.G2DBaseToolStripButton btn_AutoAttack;
        public ToolStripSeparator toolStripSeparator4;
        public DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_Turbo;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_0_1X;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_0_5X;
        public ToolStripSeparator toolStripMenuItem1;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_1X;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_2X;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_3X;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_4X;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_5X;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_10X;
        public ToolStripSeparator toolStripMenuItem2;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_TurboX;
        public ToolStripSeparator toolStripSeparator5;
        public DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton drop_SyncMode;
        public ToolStripSeparator toolStripSeparator6;
        public DeepEditor.Common.G2D.G2DBaseToolStripButton btn_SkipClientEvent;
        public ToolStripSeparator toolStripSeparator7;
        public DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_Function;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_IsAutoFocusTarget;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_IgnoreScriptEvent;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_CleanBuff;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem pickUnitToolStripMenuItem;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_QuestTest;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_QuestAccpetR2B;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_QuestStatusChangeR2B;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_QuestCommitR2B;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_QuestDropR2B;
        public ToolStripSeparator toolStripMenuItem4;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem gCToolStripMenuItem;
        public DeepEditor.Common.G2D.G2DBaseToolStripButton btn_Help;
        public DeepEditor.Common.G2D.G2DBaseStatusStrip statusStripActor;
        public ToolStripStatusLabel txt_ActorInfo;
        public System.Windows.Forms.Timer timerInfo;
        public ToolStripSeparator toolStripSeparator8;
        public DeepEditor.Common.G2D.G2DBaseToolStripButton btn_Exit;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_NetView;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem pickItemToolStripMenuItem;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem pickNearItemToolStripMenuItem;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem followUnitToolStripMenuItem;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem followUnitGuardToolStripMenuItem;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_TestClientCustom;
        public DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton menu_Mesh;
        public ToolStripSeparator toolStripSeparator9;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem menu_Meshs;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_LoadMesh;
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_LoadMeshDX;
        public SplitContainer splitGLView;
        private ToolStripMenuItem fixedUpdateToolStripMenuItem;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox txt_Filter;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripButton btn_EventDebug;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem btn_StepTool;
        private ToolStripMenuItem btn_RunPauseTool;
        private ToolStripSeparator toolStripMenuItem3;
        private Common.G2D.G2DBaseToolStripButton btn_Step;
        private Common.G2D.G2DBaseToolStripButton btn_Running;
        private ToolStripMenuItem fixedTimeIntervalToolStripMenuItem;
    }
}