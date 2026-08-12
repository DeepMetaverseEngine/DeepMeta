using DeepCore.EventTrigger;
using DeepCore.EventTrigger.Data;
using DeepCore.Xml;
using DeepEditor.Common.EventEditor.DescAttributeEdit;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G2D.DataGrid;
using DeepEditor.Common.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DeepEditor.Common.EventEditor.AwardEditor
{
    public partial class AwardPanel : UserControl, IEventNodeEditor
    {
        private EventEditor eventEditor;
        private IEventDataNode editEventNode;
        private TreeNode rootTrigger;
        private TreeNode rootCondition;
        private TreeNode rootAction;
        private TreeNode rootLocalVar;

        public ImageList EventTreeImageList { get => imageList1; }
        //----------------------------------------------------------------------------------------

        public AwardPanel()
        {
            InitializeComponent();
            this.rootLocalVar = treeView2.Nodes["RootLocalVar"];
            this.rootTrigger = treeView2.Nodes["RootTrigger"];
            this.rootCondition = treeView2.Nodes["RootCondition"];
            this.rootAction = treeView2.Nodes["RootAction"];
            this.treeView2.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.treeView2.DrawNode += TreeView2_DrawNode;
            this.txt_EventFunction.Font = EventEditorSettings.Default.EventFunctionFont;
        }
        public void ListEventLocalVar(Action<EventLocalVar> vars)
        {
            foreach (var node in this.rootLocalVar.Nodes)
            {
                if (node is EventAwardNode anode && anode.Data is EventLocalVar var)
                {
                    vars(var);
                }
            }
        }

        private void TreeView2_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node is EventAwardNode)
            {
                //(e.Node as EventAwardNode).Draw(e);
                e.DrawDefault = true;
            }
            else
            {
                e.DrawDefault = true;
            }
        }


        private EventAwardNode GetSelectedEventAwardNode()
        {
            return treeView2.SelectedNode as EventAwardNode;
        }

        public void Init(EventEditor eventEditor)
        {
            this.eventEditor = eventEditor;
            this.eventEditor.btn_CopyToClipboard.Click += this.btn_CopyToClipboard_Click;
            this.eventEditor.btn_Copy.Click += this.btn_Copy_Click;
            this.eventEditor.btn_Paste.Click += this.btn_Paste_Click;
            this.eventEditor.btn_Delete.Click += this.btn_Delete_Click;
        }
        public void ClearEventNodes()
        {
            this.txt_EventFunction.SuspendLayout();
            this.treeView2.SuspendLayout();
            try
            {
                if (editEventNode != null)
                {
                    editEventNode.EventLocalVars.Clear();
                    editEventNode.EventTriggers.Clear();
                    editEventNode.EventConditions.Clear();
                    editEventNode.EventActions.Clear();
                }
                this.txt_EventFunction.Clear();
                this.rootTrigger.Nodes.Clear();
                this.rootCondition.Nodes.Clear();
                this.rootAction.Nodes.Clear();
                this.rootLocalVar.Nodes.Clear();
                this.treeView2.ExpandAll();
            }
            finally
            {
                this.treeView2.ResumeLayout();
                this.txt_EventFunction.ResumeLayout();
            }
        }
        public void SetData(EventEditor.EventTreeNode enode)
        {
            this.txt_EventFunction.SuspendLayout();
            this.treeView2.SuspendLayout();
            try
            {
                OnSaveEditEventNode(editEventNode);
                var node = enode?.Data;
                editEventNode = node;
                if (node != null)
                {
                    this.eventMenuStrip.Enabled = true;
                    try
                    {
                        this.txt_EventFunction.SetAttributedString(EventStringBuilder.FunctionDocument(node));
                    }
                    catch (Exception err)
                    {
                        this.txt_EventFunction.Text = err.Message;
                    }
                    this.rootLocalVar.Nodes.Clear();
                    this.rootTrigger.Nodes.Clear();
                    this.rootCondition.Nodes.Clear();
                    this.rootAction.Nodes.Clear();

                    foreach (EventLocalVar data in node.EventLocalVars)
                    {
                        AddEvetAwardNode(data);
                    }
                    foreach (AbstractTrigger data in node.EventTriggers)
                    {
                        AddEvetAwardNode(data);
                    }
                    foreach (AbstractCondition data in node.EventConditions)
                    {
                        AddEvetAwardNode(data);
                    }
                    foreach (AbstractAction data in node.EventActions)
                    {
                        AddEvetAwardNode(data);
                    }
                }
                else
                {
                    this.eventMenuStrip.Enabled = false;
                    this.txt_EventFunction.Clear();
                    this.rootTrigger.Nodes.Clear();
                    this.rootCondition.Nodes.Clear();
                    this.rootAction.Nodes.Clear();
                    this.rootLocalVar.Nodes.Clear();
                }
                this.treeView2.ExpandAll();
            }
            finally
            {
                this.treeView2.ResumeLayout();
                this.txt_EventFunction.ResumeLayout();
            }
        }
        public void OnSaveEditEventNode(IEventDataNode editEventNode)
        {
            if (editEventNode != null)
            {
                editEventNode.EventLocalVars.Clear();
                editEventNode.EventTriggers.Clear();
                editEventNode.EventConditions.Clear();
                editEventNode.EventActions.Clear();
                foreach (TreeNode tn in treeView2.GetAllNodes(false))
                {
                    if (tn is EventAwardNode)
                    {
                        EventAwardNode ean = tn as EventAwardNode;
                        if (ean.Data is EventLocalVar)
                        {
                            editEventNode.EventLocalVars.Add(ean.Data as EventLocalVar);
                        }
                        else if (ean.Data is AbstractTrigger)
                        {
                            editEventNode.EventTriggers.Add(ean.Data as AbstractTrigger);
                        }
                        else if (ean.Data is AbstractCondition)
                        {
                            editEventNode.EventConditions.Add(ean.Data as AbstractCondition);
                        }
                        else if (ean.Data is AbstractAction)
                        {
                            editEventNode.EventActions.Add(ean.Data as AbstractAction);
                        }
                    }
                }
                try
                {
                    txt_EventFunction.SetAttributedString(EventStringBuilder.FunctionDocument(editEventNode));
                }
                catch (Exception err)
                {
                    this.txt_EventFunction.Text = err.Message;
                }
            }
        }





        private void AddEvetAwardNode(EventExternalizable data)
        {
            if (data != null)
            {
                EventAwardNode node = new EventAwardNode(data);
                if (data is EventLocalVar)
                {
                    node.ContextMenuStrip = rootTrigger.ContextMenuStrip;
                    rootLocalVar.Nodes.Add(node);
                }
                else if (data is AbstractTrigger)
                {
                    node.ContextMenuStrip = rootTrigger.ContextMenuStrip;
                    rootTrigger.Nodes.Add(node);
                }
                else if (data is AbstractCondition)
                {
                    node.ContextMenuStrip = rootCondition.ContextMenuStrip;
                    rootCondition.Nodes.Add(node);
                }
                else if (data is AbstractAction)
                {
                    node.ContextMenuStrip = rootAction.ContextMenuStrip;
                    rootAction.Nodes.Add(node);
                }
                else if (data is AbstractValue)
                {
                    node.ContextMenuStrip = rootAction.ContextMenuStrip;
                    rootAction.Nodes.Add(node);
                }
            }
        }
        private void AddEventLocalVar()
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowAddDialog<EventLocalVar>(this, eventEditor.CreatePropertyAdapters());
                if (result != null)
                {
                    AddEvetAwardNode(result);
                    OnSaveEditEventNode(editEventNode);
                }
            }
        }
        private void AddEventTrigger()
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowAddDialog<AbstractTrigger>(this, eventEditor.CreatePropertyAdapters());
                if (result != null)
                {
                    AddEvetAwardNode(result);
                    OnSaveEditEventNode(editEventNode);
                }
            }
        }
        private void AddEventCondition()
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowAddDialog<AbstractCondition>(this, eventEditor.CreatePropertyAdapters());
                if (result != null)
                {
                    AddEvetAwardNode(result);
                    OnSaveEditEventNode(editEventNode);
                }
            }
        }
        private void AddEventAction()
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowAddDialog<AbstractAction>(this, eventEditor.CreatePropertyAdapters());
                if (result != null)
                {
                    AddEvetAwardNode(result);
                    OnSaveEditEventNode(editEventNode);
                }
            }
        }
        private void OpenEventAwardNode(EventAwardNode node)
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowEditDialog(this, node.BaseDataType, node.Data, eventEditor.CreatePropertyAdapters());
                if (result != null)
                {
                    node.SetData(result);
                    OnSaveEditEventNode(editEventNode);
                }
            }
        }
        private void RemoveAward(EventAwardNode obj)
        {
            obj.Remove();
            treeView2.Invalidate();
            if (editEventNode != null)
            {
                OnSaveEditEventNode(editEventNode);
            }
        }

        private void MoveAward(EventAwardNode obj, int d)
        {
            try
            {
                treeView2.SuspendLayout();
                obj.MoveTreeNode(d);
                treeView2.ResumeLayout();
                treeView2.SelectedNode = obj;
                OnSaveEditEventNode(editEventNode);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //----------------------------------------------------------------------------------------





        //private static object copyEventAward;
        private static CopyPaste s_copying = new CopyPaste(typeof(AwardPanel).FullName);

        // tree view 2

        public void btn_CopyToClipboard_Click(object sender, EventArgs e)
        {
            if (treeView2.Focused)
            {
                TreeNode node = treeView2.SelectedNode;
                if (node != null)
                {
                    try
                    {
                        Win32.SetClipboard(node.Text);
                    }
                    catch { }
                }
            }
        }

        public void btn_Copy_Click(object sender, EventArgs e)
        {
            if (treeView2.Focused)
            {
                TreeNode node = treeView2.SelectedNode;
                if (node != null)
                {
                    try
                    {
                        Win32.SetClipboard(node.Text);
                    }
                    catch { }
                }
                EventAwardNode obj = GetSelectedEventAwardNode();
                if (obj != null)
                {
                    //copyEventAward = XmlUtil.CloneObject<object>(obj.Data);
                    s_copying.Copy(obj.Data);
                }
            }
        }
        public void btn_Paste_Click(object sender, EventArgs e)
        {
            if (treeView2.Focused)
            {
                //if (copyEventAward != null)
                if (s_copying.TryPaste<EventExternalizable>(out var copyEventAward))
                {
                    AddEvetAwardNode(copyEventAward);
                }
            }
        }
        public void btn_Delete_Click(object sender, EventArgs e)
        {
            if (treeView2.Focused)
            {
                EventAwardNode obj = GetSelectedEventAwardNode();
                if (obj != null)
                {
                    if (MessageBox.Show(
                      "确认删除: " + obj,
                      "确认删除",
                      MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        RemoveAward(obj);
                    }
                }
            }
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView2.SelectedNode = e.Node;
        }
        private void treeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            EventAwardNode obj = GetSelectedEventAwardNode();
            if (obj != null)
            {
                OpenEventAwardNode(obj);
            }
        }

        private void treeView2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void treeView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                EventAwardNode obj = GetSelectedEventAwardNode();
                if (obj != null)
                {
                    RemoveAward(obj);
                }
            }
        }

        // tree view 2 menu

        private void dataMenu_AddLocalVar_Click(object sender, EventArgs e)
        {
            if (editEventNode != null)
            {
                AddEventLocalVar();
            }
        }

        private void dataMenu_AddTrigger_Click(object sender, EventArgs e)
        {
            if (editEventNode != null)
            {
                AddEventTrigger();
            }
        }
        private void dataMenu_AddCondition_Click(object sender, EventArgs e)
        {
            if (editEventNode != null)
            {
                AddEventCondition();
            }
        }
        private void dataMenu_AddAction_Click(object sender, EventArgs e)
        {
            if (editEventNode != null)
            {
                AddEventAction();
            }
        }
        private void dataMenu_Copy_Click(object sender, EventArgs e)
        {
            EventAwardNode obj = GetSelectedEventAwardNode();
            if (obj != null)
            {
                var copyEventAward = XmlUtil.CloneObject<object>(obj.Data);
                s_copying.Copy(copyEventAward);
                G2DPropertyGrid.PushCopy(obj.BaseDataType, copyEventAward);
            }
        }
        private void dataMenu_Paste_Click(object sender, EventArgs e)
        {
            if (s_copying.TryPaste<EventExternalizable>(out var copyEventAward))
            {
                AddEvetAwardNode(copyEventAward);
            }
        }
        private void dataMenu_Delete_Click(object sender, EventArgs e)
        {
            EventAwardNode obj = GetSelectedEventAwardNode();
            if (obj != null)
            {
                RemoveAward(obj);
            }
        }

        private void btn_moveAwardUP_Click(object sender, EventArgs e)
        {
            EventAwardNode obj = GetSelectedEventAwardNode();
            if (obj != null)
            {
                MoveAward(obj, -1);
            }
        }

        private void btn_moveAwardDown_Click(object sender, EventArgs e)
        {
            EventAwardNode obj = GetSelectedEventAwardNode();
            if (obj != null)
            {
                MoveAward(obj, 1);
            }
        }

        private void dataMenu_UP_Click(object sender, EventArgs e)
        {
            EventAwardNode obj = GetSelectedEventAwardNode();
            if (obj != null)
            {
                MoveAward(obj, -1);
            }
        }

        private void dataMenu_Down_Click(object sender, EventArgs e)
        {
            EventAwardNode obj = GetSelectedEventAwardNode();
            if (obj != null)
            {
                MoveAward(obj, 1);
            }
        }

        private void eventMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            bool single = treeView2.SelectedNode is EventAwardNode;
            bool group = !single;

            dataMenu_AddLocalVar.Enabled = group;
            dataMenu_AddAction.Enabled = group;
            dataMenu_AddCondition.Enabled = group;
            dataMenu_AddTrigger.Enabled = group;

            dataMenu_UP.Enabled = single;
            dataMenu_Down.Enabled = single;
            dataMenu_Copy.Enabled = single;
            dataMenu_Delete.Enabled = single;

            dataMenu_Paste.Enabled = true;
        }

        private void btn_Font_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = this.txt_EventFunction.Font;//Properties.EditorSettings.Default.EventFunctionFont;
            if (fd.ShowDialog(this) == DialogResult.OK)
            {
                this.txt_EventFunction.Font = EventEditorSettings.Default.EventFunctionFont = fd.Font;
                EventEditorSettings.Default.Save();
                if (editEventNode != null)
                {
                    try
                    {
                        txt_EventFunction.SetAttributedString(EventStringBuilder.FunctionDocument(editEventNode));
                    }
                    catch (Exception err)
                    {
                        this.txt_EventFunction.Text = err.Message;
                    }
                }
            }
        }




        //------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------


    }
}