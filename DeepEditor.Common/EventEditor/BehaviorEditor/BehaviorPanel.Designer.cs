using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.EventEditor.BehaviorEditor
{
    partial class BehaviorPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BehaviorPanel));
            ST.Library.UI.NodeEditor.STNodeDrawing stNodeDrawing2 = new ST.Library.UI.NodeEditor.STNodeDrawing();
            toolStrip1 = new DeepEditor.Common.G2D.G2DBaseToolStrip();
            menu_Items = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            btn_AddTrigger = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_AddCondition = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_AddAction = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_AddValue = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_AddLocalVar = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_AddGroup = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menu_Data = new DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton();
            btn_ClearAll = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_GetCanvasImage = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_AutoLayout = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            btn_SelectAll = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            btn_Undo = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            btn_Redo = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            btn_ZoomIn = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            btn_ZoomOut = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            btn_Zoom1 = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            chk_Grid = new DeepEditor.Common.G2D.G2DBaseToolStripButton();
            stNodeEditor1 = new BehaviorNodeEditor();
            nodeMenu = new DeepEditor.Common.G2D.G2DBaseContextMenuStrip(components);
            tool_AddTrigger = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_AddCondition = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_AddAction = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_AddValue = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_AddLocalVar = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_AddGroup = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            tool_ChangeType = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_Init = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            tool_SelectTree = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_AutoLayout = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_AutoLayoutTree = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_Clean = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            tool_Copy = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_Paste = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_Clip = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            tool_Duplicate = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            tool_Remove = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            valueTypesTreeViewControl1 = new DeepEditor.Common.EventEditor.DescAttributeEdit.ValueTypesTreeViewControl();
            nodeProp = new DeepEditor.Common.G2D.DataGrid.G2DPropertyGrid();
            statusStrip1 = new StatusStrip();
            lbl_State = new ToolStripStatusLabel();
            txt_Mouse = new ToolStripStatusLabel();
            btn_Search = new ToolStripMenuItem();
            toolStrip1.SuspendLayout();
            nodeMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.SystemColors.WindowFrame;
            toolStrip1.CustomBackColor = null;
            toolStrip1.CustomForeColor = null;
            toolStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { menu_Items, toolStripSeparator1, menu_Data, toolStripSeparator2, btn_Undo, btn_Redo, toolStripSeparator3, btn_ZoomIn, btn_ZoomOut, btn_Zoom1, toolStripSeparator4, chk_Grid });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1264, 33);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // menu_Items
            // 
            menu_Items.CustomBackColor = null;
            menu_Items.CustomForeColor = null;
            menu_Items.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menu_Items.DropDownItems.AddRange(new ToolStripItem[] { btn_AddTrigger, btn_AddCondition, btn_AddAction, btn_AddValue, btn_AddLocalVar, btn_AddGroup });
            menu_Items.ImageOrigin = null;
            menu_Items.ImageTransparentColor = System.Drawing.Color.Magenta;
            menu_Items.Name = "menu_Items";
            menu_Items.Size = new System.Drawing.Size(82, 28);
            menu_Items.Text = "指令集";
            // 
            // btn_AddTrigger
            // 
            btn_AddTrigger.CustomBackColor = null;
            btn_AddTrigger.CustomForeColor = null;
            btn_AddTrigger.ImageOrigin = null;
            btn_AddTrigger.Name = "btn_AddTrigger";
            btn_AddTrigger.Size = new System.Drawing.Size(223, 34);
            btn_AddTrigger.Text = "添加 事件开端";
            btn_AddTrigger.Click += btn_AddTrigger_Click;
            // 
            // btn_AddCondition
            // 
            btn_AddCondition.CustomBackColor = null;
            btn_AddCondition.CustomForeColor = null;
            btn_AddCondition.ImageOrigin = null;
            btn_AddCondition.Name = "btn_AddCondition";
            btn_AddCondition.Size = new System.Drawing.Size(223, 34);
            btn_AddCondition.Text = "添加 事件条件";
            btn_AddCondition.Click += btn_AddCondition_Click;
            // 
            // btn_AddAction
            // 
            btn_AddAction.CustomBackColor = null;
            btn_AddAction.CustomForeColor = null;
            btn_AddAction.ImageOrigin = null;
            btn_AddAction.Name = "btn_AddAction";
            btn_AddAction.Size = new System.Drawing.Size(223, 34);
            btn_AddAction.Text = "添加 事件动作";
            btn_AddAction.Click += btn_AddAction_Click;
            // 
            // btn_AddValue
            // 
            btn_AddValue.CustomBackColor = null;
            btn_AddValue.CustomForeColor = null;
            btn_AddValue.ImageOrigin = null;
            btn_AddValue.Name = "btn_AddValue";
            btn_AddValue.Size = new System.Drawing.Size(223, 34);
            btn_AddValue.Text = "添加 事件数据";
            // 
            // btn_AddLocalVar
            // 
            btn_AddLocalVar.CustomBackColor = null;
            btn_AddLocalVar.CustomForeColor = null;
            btn_AddLocalVar.ImageOrigin = null;
            btn_AddLocalVar.Name = "btn_AddLocalVar";
            btn_AddLocalVar.Size = new System.Drawing.Size(223, 34);
            btn_AddLocalVar.Text = "添加 临时变量";
            btn_AddLocalVar.Click += btn_AddLocalVar_Click;
            // 
            // btn_AddGroup
            // 
            btn_AddGroup.CustomBackColor = null;
            btn_AddGroup.CustomForeColor = null;
            btn_AddGroup.Image = (System.Drawing.Image)resources.GetObject("btn_AddGroup.Image");
            btn_AddGroup.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_AddGroup.ImageOrigin");
            btn_AddGroup.Name = "btn_AddGroup";
            btn_AddGroup.Size = new System.Drawing.Size(270, 34);
            btn_AddGroup.Text = "添加分组";
            btn_AddGroup.Click += btn_AddGroup_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // menu_Data
            // 
            menu_Data.CustomBackColor = null;
            menu_Data.CustomForeColor = null;
            menu_Data.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menu_Data.DropDownItems.AddRange(new ToolStripItem[] { btn_ClearAll, btn_GetCanvasImage, btn_AutoLayout, btn_SelectAll, btn_Search });
            menu_Data.ImageOrigin = null;
            menu_Data.ImageTransparentColor = System.Drawing.Color.Magenta;
            menu_Data.Name = "menu_Data";
            menu_Data.Size = new System.Drawing.Size(64, 28);
            menu_Data.Text = "数据";
            // 
            // btn_ClearAll
            // 
            btn_ClearAll.CustomBackColor = null;
            btn_ClearAll.CustomForeColor = null;
            btn_ClearAll.ImageOrigin = null;
            btn_ClearAll.Name = "btn_ClearAll";
            btn_ClearAll.Size = new System.Drawing.Size(270, 34);
            btn_ClearAll.Text = "清除所有节点";
            btn_ClearAll.Click += btn_ClearAll_Click;
            // 
            // btn_GetCanvasImage
            // 
            btn_GetCanvasImage.CustomBackColor = null;
            btn_GetCanvasImage.CustomForeColor = null;
            btn_GetCanvasImage.ImageOrigin = null;
            btn_GetCanvasImage.Name = "btn_GetCanvasImage";
            btn_GetCanvasImage.Size = new System.Drawing.Size(270, 34);
            btn_GetCanvasImage.Text = "复制截图";
            btn_GetCanvasImage.Click += btn_GetCanvasImage_Click;
            // 
            // btn_AutoLayout
            // 
            btn_AutoLayout.CustomBackColor = null;
            btn_AutoLayout.CustomForeColor = null;
            btn_AutoLayout.ImageOrigin = null;
            btn_AutoLayout.Name = "btn_AutoLayout";
            btn_AutoLayout.Size = new System.Drawing.Size(270, 34);
            btn_AutoLayout.Text = "自动排列";
            btn_AutoLayout.Click += btn_AutoLayout_Click;
            // 
            // btn_SelectAll
            // 
            btn_SelectAll.Name = "btn_SelectAll";
            btn_SelectAll.Size = new System.Drawing.Size(270, 34);
            btn_SelectAll.Text = "选择所有";
            btn_SelectAll.ToolTipText = "选择所有";
            btn_SelectAll.Click += btn_SelectAll_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // btn_Undo
            // 
            btn_Undo.CustomBackColor = null;
            btn_Undo.CustomForeColor = null;
            btn_Undo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_Undo.Image = (System.Drawing.Image)resources.GetObject("btn_Undo.Image");
            btn_Undo.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_Undo.ImageOrigin");
            btn_Undo.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Undo.Name = "btn_Undo";
            btn_Undo.Size = new System.Drawing.Size(34, 28);
            btn_Undo.Text = "Undo";
            btn_Undo.Click += btn_Undo_Click;
            // 
            // btn_Redo
            // 
            btn_Redo.CustomBackColor = null;
            btn_Redo.CustomForeColor = null;
            btn_Redo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_Redo.Image = (System.Drawing.Image)resources.GetObject("btn_Redo.Image");
            btn_Redo.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_Redo.ImageOrigin");
            btn_Redo.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Redo.Name = "btn_Redo";
            btn_Redo.Size = new System.Drawing.Size(34, 28);
            btn_Redo.Text = "Redo";
            btn_Redo.Click += btn_Redo_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // btn_ZoomIn
            // 
            btn_ZoomIn.CustomBackColor = null;
            btn_ZoomIn.CustomForeColor = null;
            btn_ZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_ZoomIn.Image = (System.Drawing.Image)resources.GetObject("btn_ZoomIn.Image");
            btn_ZoomIn.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_ZoomIn.ImageOrigin");
            btn_ZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_ZoomIn.Name = "btn_ZoomIn";
            btn_ZoomIn.Size = new System.Drawing.Size(34, 28);
            btn_ZoomIn.Text = "Zoom In";
            btn_ZoomIn.Click += btn_ZoomIn_Click;
            // 
            // btn_ZoomOut
            // 
            btn_ZoomOut.CustomBackColor = null;
            btn_ZoomOut.CustomForeColor = null;
            btn_ZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_ZoomOut.Image = (System.Drawing.Image)resources.GetObject("btn_ZoomOut.Image");
            btn_ZoomOut.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_ZoomOut.ImageOrigin");
            btn_ZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_ZoomOut.Name = "btn_ZoomOut";
            btn_ZoomOut.Size = new System.Drawing.Size(34, 28);
            btn_ZoomOut.Text = "Zoom Out";
            btn_ZoomOut.Click += btn_ZoomOut_Click;
            // 
            // btn_Zoom1
            // 
            btn_Zoom1.CustomBackColor = null;
            btn_Zoom1.CustomForeColor = null;
            btn_Zoom1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_Zoom1.Image = (System.Drawing.Image)resources.GetObject("btn_Zoom1.Image");
            btn_Zoom1.ImageOrigin = (System.Drawing.Image)resources.GetObject("btn_Zoom1.ImageOrigin");
            btn_Zoom1.ImageTransparentColor = System.Drawing.Color.Magenta;
            btn_Zoom1.Name = "btn_Zoom1";
            btn_Zoom1.Size = new System.Drawing.Size(34, 28);
            btn_Zoom1.Text = "Zoom Reset";
            btn_Zoom1.Click += btn_Zoom1_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 33);
            // 
            // chk_Grid
            // 
            chk_Grid.Checked = true;
            chk_Grid.CheckOnClick = true;
            chk_Grid.CheckState = CheckState.Checked;
            chk_Grid.CustomBackColor = null;
            chk_Grid.CustomForeColor = null;
            chk_Grid.DisplayStyle = ToolStripItemDisplayStyle.Image;
            chk_Grid.Image = (System.Drawing.Image)resources.GetObject("chk_Grid.Image");
            chk_Grid.ImageOrigin = (System.Drawing.Image)resources.GetObject("chk_Grid.ImageOrigin");
            chk_Grid.ImageTransparentColor = System.Drawing.Color.Magenta;
            chk_Grid.Name = "chk_Grid";
            chk_Grid.Size = new System.Drawing.Size(34, 28);
            chk_Grid.Text = "对齐到网格";
            chk_Grid.CheckedChanged += chk_Grid_CheckedChanged;
            // 
            // stNodeEditor1
            // 
            stNodeEditor1.AllowDrop = true;
            stNodeEditor1.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
            stNodeEditor1.Curvature = 0.3F;
            stNodeEditor1.Dock = DockStyle.Fill;
            stNodeEditor1.DragMoveDistance = 5;
            stNodeEditor1.Drawing = stNodeDrawing2;
            stNodeEditor1.FindingNode = null;
            stNodeEditor1.GridSize = 10;
            stNodeEditor1.GridToSize = true;
            stNodeEditor1.icon_action = null;
            stNodeEditor1.icon_condition = null;
            stNodeEditor1.icon_question = null;
            stNodeEditor1.icon_trigger = null;
            stNodeEditor1.icon_value = null;
            stNodeEditor1.icon_var = null;
            stNodeEditor1.IsReadOnly = false;
            stNodeEditor1.Location = new System.Drawing.Point(0, 33);
            stNodeEditor1.LocationBackColor = System.Drawing.Color.FromArgb(120, 0, 0, 0);
            stNodeEditor1.MarkBackColor = System.Drawing.Color.FromArgb(180, 0, 0, 0);
            stNodeEditor1.MarkForeColor = System.Drawing.Color.FromArgb(180, 0, 0, 0);
            stNodeEditor1.MinimumSize = new System.Drawing.Size(100, 100);
            stNodeEditor1.Name = "stNodeEditor1";
            stNodeEditor1.ScaleMax = 10F;
            stNodeEditor1.ScaleMin = 0.1F;
            stNodeEditor1.Size = new System.Drawing.Size(1264, 836);
            stNodeEditor1.TabIndex = 2;
            stNodeEditor1.Text = "stNodeEditor1";
            stNodeEditor1.ActiveChanged += stNodeEditor1_ActiveChanged_1;
            stNodeEditor1.KeyDown += stNodeEditor1_KeyDown;
            stNodeEditor1.MouseDoubleClick += stNodeEditor1_MouseDoubleClick;
            stNodeEditor1.MouseMove += stNodeEditor1_MouseMove;
            stNodeEditor1.MouseUp += stNodeEditor1_MouseUp;
            stNodeEditor1.PreviewKeyDown += stNodeEditor1_PreviewKeyDown;
            // 
            // nodeMenu
            // 
            nodeMenu.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            nodeMenu.CustomBackColor = null;
            nodeMenu.CustomForeColor = null;
            nodeMenu.Depth = 0;
            nodeMenu.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            nodeMenu.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            nodeMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            nodeMenu.Items.AddRange(new ToolStripItem[] { tool_AddTrigger, tool_AddCondition, tool_AddAction, tool_AddValue, tool_AddLocalVar, tool_AddGroup, toolStripMenuItem3, tool_ChangeType, tool_Init, toolStripMenuItem4, tool_SelectTree, tool_AutoLayout, tool_AutoLayoutTree, tool_Clean, toolStripMenuItem1, tool_Copy, tool_Paste, tool_Clip, tool_Duplicate, toolStripMenuItem2, tool_Remove });
            nodeMenu.MouseState = MaterialSkin.MouseState.HOVER;
            nodeMenu.Name = "contextMenuStrip1";
            nodeMenu.Size = new System.Drawing.Size(251, 572);
            nodeMenu.Opening += nodeMenu_Opening;
            // 
            // tool_AddTrigger
            // 
            tool_AddTrigger.CustomBackColor = null;
            tool_AddTrigger.CustomForeColor = null;
            tool_AddTrigger.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_AddTrigger.Image = (System.Drawing.Image)resources.GetObject("tool_AddTrigger.Image");
            tool_AddTrigger.ImageOrigin = null;
            tool_AddTrigger.Name = "tool_AddTrigger";
            tool_AddTrigger.Size = new System.Drawing.Size(250, 32);
            tool_AddTrigger.Text = "添加 事件开端";
            tool_AddTrigger.Click += tool_AddTrigger_Click;
            // 
            // tool_AddCondition
            // 
            tool_AddCondition.CustomBackColor = null;
            tool_AddCondition.CustomForeColor = null;
            tool_AddCondition.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_AddCondition.Image = (System.Drawing.Image)resources.GetObject("tool_AddCondition.Image");
            tool_AddCondition.ImageOrigin = null;
            tool_AddCondition.Name = "tool_AddCondition";
            tool_AddCondition.Size = new System.Drawing.Size(250, 32);
            tool_AddCondition.Text = "添加 事件条件";
            tool_AddCondition.Click += tool_AddCondition_Click;
            // 
            // tool_AddAction
            // 
            tool_AddAction.CustomBackColor = null;
            tool_AddAction.CustomForeColor = null;
            tool_AddAction.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_AddAction.Image = (System.Drawing.Image)resources.GetObject("tool_AddAction.Image");
            tool_AddAction.ImageOrigin = null;
            tool_AddAction.Name = "tool_AddAction";
            tool_AddAction.Size = new System.Drawing.Size(250, 32);
            tool_AddAction.Text = "添加 事件动作";
            tool_AddAction.Click += tool_AddAction_Click;
            // 
            // tool_AddValue
            // 
            tool_AddValue.CustomBackColor = null;
            tool_AddValue.CustomForeColor = null;
            tool_AddValue.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_AddValue.Image = (System.Drawing.Image)resources.GetObject("tool_AddValue.Image");
            tool_AddValue.ImageOrigin = null;
            tool_AddValue.Name = "tool_AddValue";
            tool_AddValue.Size = new System.Drawing.Size(250, 32);
            tool_AddValue.Text = "添加 事件数据";
            // 
            // tool_AddLocalVar
            // 
            tool_AddLocalVar.CustomBackColor = null;
            tool_AddLocalVar.CustomForeColor = null;
            tool_AddLocalVar.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_AddLocalVar.Image = (System.Drawing.Image)resources.GetObject("tool_AddLocalVar.Image");
            tool_AddLocalVar.ImageOrigin = null;
            tool_AddLocalVar.Name = "tool_AddLocalVar";
            tool_AddLocalVar.Size = new System.Drawing.Size(250, 32);
            tool_AddLocalVar.Text = "添加 临时变量";
            tool_AddLocalVar.Click += tool_AddLocalVar_Click;
            // 
            // tool_AddGroup
            // 
            tool_AddGroup.CustomBackColor = null;
            tool_AddGroup.CustomForeColor = null;
            tool_AddGroup.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_AddGroup.Image = (System.Drawing.Image)resources.GetObject("tool_AddGroup.Image");
            tool_AddGroup.ImageOrigin = (System.Drawing.Image)resources.GetObject("tool_AddGroup.ImageOrigin");
            tool_AddGroup.Name = "tool_AddGroup";
            tool_AddGroup.Size = new System.Drawing.Size(250, 32);
            tool_AddGroup.Text = "添加分组";
            tool_AddGroup.Click += tool_AddGroup_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripMenuItem3.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(247, 6);
            // 
            // tool_ChangeType
            // 
            tool_ChangeType.CustomBackColor = null;
            tool_ChangeType.CustomForeColor = null;
            tool_ChangeType.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_ChangeType.Image = (System.Drawing.Image)resources.GetObject("tool_ChangeType.Image");
            tool_ChangeType.ImageOrigin = (System.Drawing.Image)resources.GetObject("tool_ChangeType.ImageOrigin");
            tool_ChangeType.Name = "tool_ChangeType";
            tool_ChangeType.Size = new System.Drawing.Size(250, 32);
            tool_ChangeType.Text = "改变类型";
            tool_ChangeType.Click += tool_ChangeType_Click;
            // 
            // tool_Init
            // 
            tool_Init.CustomBackColor = null;
            tool_Init.CustomForeColor = null;
            tool_Init.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_Init.Image = (System.Drawing.Image)resources.GetObject("tool_Init.Image");
            tool_Init.ImageOrigin = (System.Drawing.Image)resources.GetObject("tool_Init.ImageOrigin");
            tool_Init.Name = "tool_Init";
            tool_Init.Size = new System.Drawing.Size(250, 32);
            tool_Init.Text = "初始化默认参数";
            tool_Init.Click += tool_Init_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripMenuItem4.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(247, 6);
            // 
            // tool_SelectTree
            // 
            tool_SelectTree.CustomBackColor = null;
            tool_SelectTree.CustomForeColor = null;
            tool_SelectTree.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_SelectTree.Image = (System.Drawing.Image)resources.GetObject("tool_SelectTree.Image");
            tool_SelectTree.ImageOrigin = (System.Drawing.Image)resources.GetObject("tool_SelectTree.ImageOrigin");
            tool_SelectTree.Name = "tool_SelectTree";
            tool_SelectTree.Size = new System.Drawing.Size(250, 32);
            tool_SelectTree.Text = "选择关系树";
            tool_SelectTree.Click += tool_SelectTree_Click;
            // 
            // tool_AutoLayout
            // 
            tool_AutoLayout.CustomBackColor = null;
            tool_AutoLayout.CustomForeColor = null;
            tool_AutoLayout.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_AutoLayout.ImageOrigin = null;
            tool_AutoLayout.Name = "tool_AutoLayout";
            tool_AutoLayout.Size = new System.Drawing.Size(250, 32);
            tool_AutoLayout.Text = "自动排列（输入）";
            tool_AutoLayout.Click += tool_AutoLayout_Click;
            // 
            // tool_AutoLayoutTree
            // 
            tool_AutoLayoutTree.CustomBackColor = null;
            tool_AutoLayoutTree.CustomForeColor = null;
            tool_AutoLayoutTree.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_AutoLayoutTree.ImageOrigin = null;
            tool_AutoLayoutTree.Name = "tool_AutoLayoutTree";
            tool_AutoLayoutTree.Size = new System.Drawing.Size(250, 32);
            tool_AutoLayoutTree.Text = "自动排列（关系树）";
            tool_AutoLayoutTree.Click += tool_AutoLayoutTree_Click;
            // 
            // tool_Clean
            // 
            tool_Clean.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_Clean.Name = "tool_Clean";
            tool_Clean.Size = new System.Drawing.Size(250, 32);
            tool_Clean.Text = "清理";
            tool_Clean.ToolTipText = "清理";
            tool_Clean.Click += tool_Clean_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripMenuItem1.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(247, 6);
            // 
            // tool_Copy
            // 
            tool_Copy.CustomBackColor = null;
            tool_Copy.CustomForeColor = null;
            tool_Copy.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_Copy.Image = (System.Drawing.Image)resources.GetObject("tool_Copy.Image");
            tool_Copy.ImageOrigin = (System.Drawing.Image)resources.GetObject("tool_Copy.ImageOrigin");
            tool_Copy.Name = "tool_Copy";
            tool_Copy.Size = new System.Drawing.Size(250, 32);
            tool_Copy.Text = "复制";
            tool_Copy.Click += tool_Copy_Click;
            // 
            // tool_Paste
            // 
            tool_Paste.CustomBackColor = null;
            tool_Paste.CustomForeColor = null;
            tool_Paste.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_Paste.Image = (System.Drawing.Image)resources.GetObject("tool_Paste.Image");
            tool_Paste.ImageOrigin = (System.Drawing.Image)resources.GetObject("tool_Paste.ImageOrigin");
            tool_Paste.Name = "tool_Paste";
            tool_Paste.Size = new System.Drawing.Size(250, 32);
            tool_Paste.Text = "粘贴";
            tool_Paste.Click += tool_Paste_Click;
            // 
            // tool_Clip
            // 
            tool_Clip.CustomBackColor = null;
            tool_Clip.CustomForeColor = null;
            tool_Clip.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_Clip.Image = (System.Drawing.Image)resources.GetObject("tool_Clip.Image");
            tool_Clip.ImageOrigin = (System.Drawing.Image)resources.GetObject("tool_Clip.ImageOrigin");
            tool_Clip.Name = "tool_Clip";
            tool_Clip.Size = new System.Drawing.Size(250, 32);
            tool_Clip.Text = "剪贴";
            tool_Clip.Click += tool_Clip_Click;
            // 
            // tool_Duplicate
            // 
            tool_Duplicate.CustomBackColor = null;
            tool_Duplicate.CustomForeColor = null;
            tool_Duplicate.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_Duplicate.ImageOrigin = null;
            tool_Duplicate.Name = "tool_Duplicate";
            tool_Duplicate.Size = new System.Drawing.Size(250, 32);
            tool_Duplicate.Text = "克隆";
            tool_Duplicate.Click += tool_Duplicate_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            toolStripMenuItem2.ForeColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(247, 6);
            // 
            // tool_Remove
            // 
            tool_Remove.CustomBackColor = null;
            tool_Remove.CustomForeColor = null;
            tool_Remove.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tool_Remove.Image = (System.Drawing.Image)resources.GetObject("tool_Remove.Image");
            tool_Remove.ImageOrigin = (System.Drawing.Image)resources.GetObject("tool_Remove.ImageOrigin");
            tool_Remove.Name = "tool_Remove";
            tool_Remove.Size = new System.Drawing.Size(250, 32);
            tool_Remove.Text = "删除";
            tool_Remove.Click += tool_Remove_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.SystemColors.WindowFrame;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(stNodeEditor1);
            splitContainer1.Panel2.Controls.Add(statusStrip1);
            splitContainer1.Panel2.Controls.Add(toolStrip1);
            splitContainer1.Size = new System.Drawing.Size(1700, 900);
            splitContainer1.SplitterDistance = 430;
            splitContainer1.SplitterWidth = 6;
            splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            splitContainer2.BorderStyle = BorderStyle.FixedSingle;
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.FixedPanel = FixedPanel.Panel2;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(valueTypesTreeViewControl1);
            splitContainer2.Panel1MinSize = 0;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(nodeProp);
            splitContainer2.Panel2MinSize = 0;
            splitContainer2.Size = new System.Drawing.Size(430, 900);
            splitContainer2.SplitterDistance = 350;
            splitContainer2.SplitterWidth = 6;
            splitContainer2.TabIndex = 0;
            // 
            // valueTypesTreeViewControl1
            // 
            valueTypesTreeViewControl1.CheckBoxes = false;
            valueTypesTreeViewControl1.CustomBackColor = null;
            valueTypesTreeViewControl1.CustomForeColor = null;
            valueTypesTreeViewControl1.Dock = DockStyle.Fill;
            valueTypesTreeViewControl1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            valueTypesTreeViewControl1.EnableCopyPaste = true;
            valueTypesTreeViewControl1.EnableDragDrop = false;
            valueTypesTreeViewControl1.FullRowSelect = true;
            valueTypesTreeViewControl1.HideSelection = false;
            valueTypesTreeViewControl1.ImageKey = "icon_Group";
            valueTypesTreeViewControl1.ImageList = null;
            valueTypesTreeViewControl1.ItemHeight = 32;
            valueTypesTreeViewControl1.LineColor = System.Drawing.Color.DarkGray;
            valueTypesTreeViewControl1.Location = new System.Drawing.Point(0, 0);
            valueTypesTreeViewControl1.MinimumSize = new System.Drawing.Size(100, 300);
            valueTypesTreeViewControl1.Name = "valueTypesTreeViewControl1";
            valueTypesTreeViewControl1.SelectedImageKey = "icon_Group";
            valueTypesTreeViewControl1.SelectedNode = null;
            valueTypesTreeViewControl1.ShowNodeToolTips = true;
            valueTypesTreeViewControl1.Size = new System.Drawing.Size(428, 348);
            valueTypesTreeViewControl1.TabIndex = 0;
            valueTypesTreeViewControl1.TreeViewNodeSorter = null;
            // 
            // nodeProp
            // 
            nodeProp.CustomBackColor = null;
            nodeProp.CustomForeColor = null;
            nodeProp.DescriptionAreaHeight = 88;
            nodeProp.DescriptionAreaLineCount = 3;
            nodeProp.Dock = DockStyle.Fill;
            nodeProp.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            nodeProp.ImeMode = ImeMode.NoControl;
            nodeProp.Location = new System.Drawing.Point(0, 0);
            nodeProp.MinDescriptionAreaLineCount = 5;
            nodeProp.MinimumSize = new System.Drawing.Size(100, 300);
            nodeProp.Name = "nodeProp";
            nodeProp.SelectedElementDesc = null;
            nodeProp.SelectedField = null;
            nodeProp.SelectedFieldDesc = null;
            nodeProp.SelectedRootObject = null;
            nodeProp.Size = new System.Drawing.Size(428, 542);
            nodeProp.TabIndex = 0;
            nodeProp.ViewBackColor = System.Drawing.SystemColors.WindowFrame;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lbl_State, txt_Mouse });
            statusStrip1.Location = new System.Drawing.Point(0, 869);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(1264, 31);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // lbl_State
            // 
            lbl_State.Name = "lbl_State";
            lbl_State.Size = new System.Drawing.Size(69, 24);
            lbl_State.Text = "GUID : ";
            // 
            // txt_Mouse
            // 
            txt_Mouse.Name = "txt_Mouse";
            txt_Mouse.Size = new System.Drawing.Size(72, 24);
            txt_Mouse.Text = "Mouse:";
            // 
            // btn_Search
            // 
            btn_Search.Name = "btn_Search";
            btn_Search.ShortcutKeys = Keys.F3;
            btn_Search.Size = new System.Drawing.Size(270, 34);
            btn_Search.Text = "查找";
            btn_Search.Click += btn_Search_Click;
            // 
            // BehaviorPanel
            // 
            Controls.Add(splitContainer1);
            Name = "BehaviorPanel";
            Size = new System.Drawing.Size(1700, 900);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            nodeMenu.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private G2D.G2DBaseToolStrip toolStrip1;
        private G2D.G2DBaseToolStripDropDownButton menu_Items;
        private G2D.G2DBaseToolStripMenuItem btn_AddTrigger;
        private G2D.G2DBaseToolStripMenuItem btn_AddAction;
        private G2D.G2DBaseToolStripMenuItem btn_AddValue;
        private G2D.G2DBaseToolStripMenuItem btn_AddCondition;
        private BehaviorNodeEditor stNodeEditor1;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private G2D.G2DBaseContextMenuStrip nodeMenu;
        private G2D.G2DBaseToolStripMenuItem tool_Remove;
        private G2D.DataGrid.G2DPropertyGrid nodeProp;
        private ToolStripSeparator toolStripMenuItem1;
        private G2D.G2DBaseToolStripMenuItem tool_Copy;
        private G2D.G2DBaseToolStripMenuItem tool_Paste;
        private G2D.G2DBaseToolStripMenuItem tool_Duplicate;
        private ToolStripSeparator toolStripMenuItem2;
        private G2D.G2DBaseToolStripMenuItem tool_Clip;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lbl_State;
        private G2D.G2DBaseToolStripMenuItem tool_AddTrigger;
        private G2D.G2DBaseToolStripMenuItem tool_AddAction;
        private G2D.G2DBaseToolStripMenuItem tool_AddValue;
        private ToolStripSeparator toolStripMenuItem3;
        private DescAttributeEdit.ValueTypesTreeViewControl valueTypesTreeViewControl1;
        private ToolStripStatusLabel txt_Mouse;
        private ToolStripSeparator toolStripSeparator1;
        private G2D.G2DBaseToolStripDropDownButton menu_Data;
        private ToolStripSeparator toolStripSeparator2;
        private G2D.G2DBaseToolStripMenuItem tool_AddCondition;
        private G2D.G2DBaseToolStripMenuItem btn_ClearAll;
        private G2D.G2DBaseToolStripMenuItem btn_GetCanvasImage;
        private G2D.G2DBaseToolStripMenuItem tool_SelectTree;
        private G2D.G2DBaseToolStripButton btn_ZoomIn;
        private G2D.G2DBaseToolStripButton btn_ZoomOut;
        private ToolStripSeparator toolStripSeparator3;
        private G2D.G2DBaseToolStripMenuItem btn_AddLocalVar;
        private G2D.G2DBaseToolStripMenuItem tool_AddLocalVar;
        private G2D.G2DBaseToolStripMenuItem btn_AutoLayout;
        private G2D.G2DBaseToolStripMenuItem tool_AutoLayout;
        private G2D.G2DBaseToolStripMenuItem tool_AutoLayoutTree;
        private G2D.G2DBaseToolStripButton btn_Zoom1;
        private G2D.G2DBaseToolStripButton btn_Undo;
        private G2D.G2DBaseToolStripButton btn_Redo;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem tool_Clean;
        private ToolStripMenuItem btn_SelectAll;
        private G2D.G2DBaseToolStripMenuItem tool_Init;
        private ToolStripSeparator toolStripMenuItem4;
        private G2D.G2DBaseToolStripMenuItem tool_ChangeType;
        private G2D.G2DBaseToolStripMenuItem btn_AddGroup;
        private G2D.G2DBaseToolStripMenuItem tool_AddGroup;
        private G2D.G2DBaseToolStripButton chk_Grid;
        private ToolStripMenuItem btn_Search;
    }
}
