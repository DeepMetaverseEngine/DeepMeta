using DeepCore;
using DeepCore.EventTrigger;
using DeepCore.EventTrigger.Data;
using DeepCore.Xml;
using DeepEditor.Common.EventEditor.AwardEditor;
using DeepEditor.Common.EventEditor.DescAttributeEdit;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G2D.DataGrid;
using DeepEditor.Common.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

namespace DeepEditor.Common.EventEditor
{
    public partial class EventEditor : G2DBaseForm
    {
        public IEventEditorProvider Provider { get; private set; }
        private G2DTreeNodeGroup rootEvents;
        private EventTreeNode editEventNode;

        //private static IEventDataNode copyEventNode;
        private static CopyPaste s_copying = new CopyPaste(typeof(EventEditor));
        private EnvironmentVarEditor envEditor;

        public G2DTreeNodeGroup EventTreeRoot { get => rootEvents; }
        public ImageList EventTreeImageList { get => treeViewEvents.TreeView.ImageList; }
        public AwardPanel Award { get => awardPanel1; }
        public BehaviorEditor.BehaviorPanel Behavior { get => behaviorPanel1; }
        public object RootObject { get; set; }
        //----------------------------------------------------------------------------------------

        public EventEditor()
        {
            InitializeComponent();

            this.ShowIcon = true;

            this.rootEvents = new G2DTreeNodeGroup(this.Text);
            this.rootEvents.ImageKey = "icons_tool_bar2.png";
            this.rootEvents.SelectedImageKey = "icons_tool_bar2.png";
            this.rootEvents.ContextMenuStrip = groupMenuStrip;
            this.treeViewEvents.TreeView.Nodes.Add(rootEvents);

            this.treeViewEvents.EnableDragDrop = true;
            this.treeViewEvents.TreeView.Enter += treeView_Enter;
            this.treeViewEvents.TreeView.KeyDown += treeViewEvents_KeyDown;
            this.treeViewEvents.TreeView.KeyPress += treeViewEvents_KeyPress;
            this.treeViewEvents.TreeView.Leave += treeView_Leave;
            this.treeViewEvents.SelectionChanged += TreeViewEvents_SelectionChanged;

            //             treeViewEvents.ItemDrag += treeViewEvents_ItemDrag;
            //             treeViewEvents.DragDrop += treeViewEvents_DragDrop;
            //             treeViewEvents.DragEnter += treeViewEvents_DragEnter;
            //             treeViewEvents.DragOver += treeViewEvents_DragOver;
            this.Load += EventEditor_Load1;
        }

        private void EventEditor_Load1(object sender, EventArgs e)
        {
            this.Award.Init(this);
            this.Behavior.Init(this);
        }

        public virtual void InitProvider(IEventEditorProvider provider)
        {
            this.Provider = provider;
            try
            {
                this.Text = provider.EditorName;
                this.Provider.CreateEnvironmentVar();
            }
            catch
            {
                this.btn_EnvVars.Visible = this.btn_EnvVars.Enabled = false;
            }
        }
        //----------------------------------------------------------------------------------------
        private Action<EventEditor> event_OnSaving;
        private Action<EventEditor> event_OnDataSaved;
        private Action<EventEditor> event_OnDataLoaded;
        private Action<EventEditor> event_OnExecute;
        public event Action<EventEditor> OnSaving { add { event_OnSaving += value; } remove { event_OnSaving -= value; } }
        public event Action<EventEditor> OnDataSaved { add { event_OnDataSaved += value; } remove { event_OnDataSaved -= value; } }
        public event Action<EventEditor> OnDataLoaded { add { event_OnDataLoaded += value; } remove { event_OnDataLoaded -= value; } }
        public event Action<EventEditor> OnExecute { add { event_OnExecute += value; } remove { event_OnExecute -= value; } }

        //----------------------------------------------------------------------------------------
        //         #region ABSTRACT
        // 
        //         public abstract List<IEventDataNode> LoadEventDataNodes();
        // 
        //         public abstract void OnSave(List<IEventDataNode> events);
        // 
        //         public abstract IEventDataNode CreateEventDataNode();
        // 
        //         public abstract IG2DPropertyAdapter[] CreatePropertyAdapters();
        // 
        //         #endregion
        //----------------------------------------------------------------------------------------


        public IG2DPropertyAdapter[] CreatePropertyAdapters()
        {
            return Provider.PropertyAdapters.ArrayAppend(new EventDataAdapters(this));
        }
        public EventExternalizable ShowAddDialog(Type dataType)
        {
            var result = ValueTypeDialog.ShowAddDialog(this, dataType, CreatePropertyAdapters());
            if (result is EventExternalizable data)
            {
                return data;
            }
            return null;
        }
        public T ShowAddDialog<T>() where T : EventExternalizable
        {
            var result = ValueTypeDialog.ShowAddDialog<T>(this, CreatePropertyAdapters());
            return result;
        }
        public EventExternalizable ShowEditDialog(Type dataType, EventExternalizable edit)
        {
            var result = ValueTypeDialog.ShowEditDialog(this, dataType, edit, CreatePropertyAdapters());
            if (result is EventExternalizable data)
            {
                return data;
            }
            return null;
        }

        /*
          private void AddEventLocalVar(Point? location)
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowAddDialog<EventLocalVar>(this, eventEditor.CreatePropertyAdapters());
                if (result != null)
                {
                    AddNode(result, location);
                }
            }
        }
        private void AddEventTrigger(Point? location)
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowAddDialog<AbstractTrigger>(this, eventEditor.CreatePropertyAdapters());
                if (result != null)
                {
                    AddNode(result, location);
                }
            }
        }
        private void AddEventAction(Point? location)
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowAddDialog<AbstractAction>(this, eventEditor.CreatePropertyAdapters());
                if (result != null)
                {
                    AddNode(result, location);
                }
            }
        }
        private void AddEventValue(Point? location)
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowAddDialog<AbstractValue>(this, eventEditor.CreatePropertyAdapters());
                if (result != null)
                {
                    AddNode(result, location);
                }
            }
        }
        private void AddEventValue(Type valueType, Point? location)
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowAddDialog(this, valueType, eventEditor.CreatePropertyAdapters());
                if (result is AbstractValue value)
                {
                    AddNode(value, location);
                }
            }
        }
        private void AddEventCondition(Point? location)
        {
            if (editEventNode != null)
            {
                var result = ValueTypeDialog.ShowAddDialog<AbstractCondition>(this, eventEditor.CreatePropertyAdapters());
                if (result is AbstractCondition value)
                {
                    AddNode(value, location);
                }
            }
        }
         
         */

        private void EventEditor_Load(object sender, EventArgs e)
        {
            LoadData();
            this.rootEvents.Text = "事件列表";
            //             this.treeViewEvents.ExpandAll();
            //             this.treeView2.ExpandAll();
            this.rootEvents.Expand();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            //this.SaveData();
            envEditor?.Close();
        }
        private void EventEditor_Validating(object sender, CancelEventArgs e)
        {
        }

        private void LoadData()
        {
            foreach (var evt in Provider.LoadEventDataNodes())
            {
                LoadEvent(evt);
            }
            event_OnDataLoaded?.Invoke(this);
        }
        public List<IEventDataNode> SaveData()
        {
            //envEditor?.SaveData();
            envEditor?.SaveData();
            var nodes = GetEventDataNodes();
            Provider.SaveEventDataNodes(nodes);
            event_OnDataSaved?.Invoke(this);
            return nodes;
        }
        private List<IEventDataNode> GetEventDataNodes()
        {
            OnSaveEditEventNode();
            var events = new List<IEventDataNode>();
            foreach (TreeNode tn in rootEvents.GetAllNodes(false))
            {
                if (tn is EventTreeNode)
                {
                    EventTreeNode otn = tn as EventTreeNode;
                    otn.Data.EventTreePath = rootEvents.GetSavePath(otn);
                    events.Add(otn.Data);
                }
            }
            return events;
        }

        public EnvironmentVarEditor ShowEnvironmentVarEditor()
        {
            if (envEditor == null)
            {
                envEditor = new EnvironmentVarEditor(this.Provider);
                envEditor.FormClosing += (sender, evt) =>
                {
                    evt.Cancel = true;
                    envEditor.SaveData();
                    envEditor.Hide();
                };
                envEditor.Show();
            }
            else
            {
                envEditor.Show();
                FormUtils.SwithToThisForm(envEditor, true);
            }
            return envEditor;
        }



        //----------------------------------------------------------------------------------------
        #region EventTree
        public void LoadTreeInfo(XmlElement events)
        {
            if (events != null)
            {
                rootEvents.SetTreeInfoXML(events, new TreeStateInfoConfig());
            }
        }
        public void SaveTreeInfo(XmlElement events)
        {
            rootEvents.GetTreeInfoXML(events);
        }
        public class EventTreeNode : G2DTreeNode<IEventDataNode>
        {
            private UndoRedoManager cmd_queue = new UndoRedoManager(100);
            public EventEditor Editor { get; }
            public UndoRedoManager CmdQueue { get => cmd_queue; }
            public EventTreeNode(IEventDataNode data, EventEditor editor)
                : base(data)
            {
                this.Editor = editor;
                this.ImageKey = "icon_quest.png";
                this.SelectedImageKey = "icon_quest.png";
                this.ContextMenuStrip = editor.groupMenuStrip;
                this.cmd_queue.OnListChanged += Cmd_queue_OnListChanged;
                RefreshData();
            }
            private void Cmd_queue_OnListChanged()
            {
                Editor.behaviorPanel1.RefreshStatus();
            }
            protected override void RefreshData()
            {
                base.RefreshData();
                if (!Data.EventIsActive)
                {
                    this.ForeColor = GlobalSkinManager.TextDisabledColor;
                }
                else
                {
                    this.ForeColor = GlobalSkinManager.TextHighEmphasisColor;
                }
            }
        }

        //----------------------------------------------------------------------------------------

        private void AddGroup()
        {
            var parent = GetSelectedGroup();
            if (parent != null)
            {
                parent.TryAddG2DGroupDialog("分组", out var group);
            }
        }
        private void RenameGroup()
        {
            var group = GetSelectedGroup();
            if (group != null)
            {
                string name = G2DTextDialog.Show(group.Name, "重命名过滤器");
                if (name != null)
                {
                    group.SetName(name);
                }
            }
        }
        private void LoadEvent(IEventDataNode data)
        {
            if (data == null) return;
            if (ContainsEvent(data.EventName))
            {
                MessageBox.Show("\"" + data.EventName + "\" 已存在!");
            }
            else
            {
                EventTreeNode node = new EventTreeNode(data, this);
                G2DTreeNodeGroup parent = rootEvents.GetOrCreateGroup(data.EventTreePath);
                parent.Nodes.Add(node);
            }
        }
        private void AddEvent()
        {
            G2DTreeNodeGroup parent = GetSelectedGroup();
            if (parent != null)
            {
                string name = "未命名事件" + rootEvents.GetAllNodesCount();
                while (!string.IsNullOrEmpty(name))
                {
                    name = G2DTextDialog.Show(name, "新建事件");
                    if (name != null)
                    {
                        if (ContainsEvent(name))
                        {
                            MessageBox.Show("\"" + name + "\" 已存在!");
                        }
                        else
                        {
                            IEventDataNode data = Provider.CreateEventDataNode();
                            data.EventName = name;
                            EventTreeNode node = new EventTreeNode(data, this);
                            parent.Nodes.Add(node);
                            parent.Expand();
                            treeViewEvents.TreeView.SelectedNode = node;
                            return;
                        }
                    }
                }
            }
        }

        private string BestNewName(string name)
        {
            if (ContainsEvent(name))
            {
                int i = name.Length - 1;
                for (; i >= 0; --i)
                {
                    char ch = name[i];
                    if (ch >= '0' && ch <= '9')
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                name = name.Substring(0, i + 1);
            }
            return name + rootEvents.GetAllNodesCount();
        }
        private void CloneEvent(IEventDataNode data)
        {
            G2DTreeNodeGroup parent = GetSelectedGroup();
            int index = parent.Nodes.Count;
            TreeNode selected = GetSelectedEvent();
            if (selected != null)
            {
                index = parent.Nodes.IndexOf(selected) + 1;
            }
            if (data != null && parent != null)
            {
                data = XmlUtil.CloneObject<IEventDataNode>(data);
                string name = data.EventName;
                if (ContainsEvent(name))
                {
                    name = BestNewName(name);
                }
                while (!string.IsNullOrEmpty(name))
                {
                    name = G2DTextDialog.Show(name, "复制 " + data.EventName);
                    if (name != null)
                    {
                        if (ContainsEvent(name))
                        {
                            MessageBox.Show("\"" + name + "\" 已存在!");
                        }
                        else
                        {
                            data.EventName = name;
                            EventTreeNode node = new EventTreeNode(data, this);
                            parent.Nodes.Insert(index, node);
                            parent.Expand();
                            treeViewEvents.TreeView.SelectedNode = node;
                            return;
                        }
                    }
                }
            }
        }
        private void RenameEvent()
        {
            EventTreeNode node = GetSelectedEvent();
            if (node != null)
            {
                string name = node.TextID;
                while (!string.IsNullOrEmpty(name))
                {
                    name = G2DTextDialog.Show(name, "重命名 " + name);
                    if (name != null)
                    {
                        if (ContainsEvent(name))
                        {
                            MessageBox.Show("\"" + name + "\" 已存在!");
                        }
                        else
                        {
                            node.SetDataID(name);
                            return;
                        }
                    }
                }
            }
        }
        private void RemoveEvent()
        {
            EventTreeNode node = GetSelectedEvent();
            if (node != null)
            {
                node.RemoveFromParent();
            }
        }

        private G2DTreeNodeGroup GetSelectedGroup()
        {
            if (treeViewEvents.TreeView.SelectedNode is G2DTreeNodeGroup)
            {
                return treeViewEvents.TreeView.SelectedNode as G2DTreeNodeGroup;
            }
            if (treeViewEvents.TreeView.SelectedNode is EventTreeNode)
            {
                return (treeViewEvents.TreeView.SelectedNode as EventTreeNode).Parent as G2DTreeNodeGroup;
            }
            return rootEvents;
        }
        private EventTreeNode GetSelectedEvent()
        {
            if (treeViewEvents.TreeView.SelectedNode is EventTreeNode)
            {
                return treeViewEvents.TreeView.SelectedNode as EventTreeNode;
            }
            return null;
        }
        private bool ContainsEvent(string name)
        {
            TreeNode tn = rootEvents.FindNodeByText<EventTreeNode>(name, true);
            return tn != null;
        }

        private void SetGroupActive(G2DTreeNodeGroup group, bool active)
        {
            if (group != null)
            {
                var list = group.GetAllNodesT<EventTreeNode>(false);
                foreach (var evt in list)
                {
                    evt.Data.EventIsActive = active;
                    evt.Refresh();
                }
            }
        }

        private void RemoveGroupEvents(G2DTreeNodeGroup group)
        {
            if (group != null)
            {
                var list = group.GetAllNodesT<EventTreeNode>(false);
                foreach (var evt in list)
                {
                    evt.RemoveFromParent();
                }
                if (group.Parent != null)
                {
                    group.Parent.Nodes.Remove(group);
                }
            }
        }

        //--------------------------------------------------------------------------------------

        private void treeView_Enter(object sender, EventArgs e)
        {
            //             if (treeViewEvents.Focused)
            //             {
            //                 tool_Edit.Enabled = true;
            //             }
            //             else if (treeView2.Focused)
            //             {
            //                 tool_Edit.Enabled = true;
            //             }
            //             else
            //             {
            //                 tool_Edit.Enabled = false;
            //             }
        }
        private void treeView_Leave(object sender, EventArgs e)
        {
            //             if (treeViewEvents.Focused)
            //             {
            //                 tool_Edit.Enabled = true;
            //             }
            //             else if (treeView2.Focused)
            //             {
            //                 tool_Edit.Enabled = true;
            //             }
            //             else
            //             {
            //                 tool_Edit.Enabled = false;
            //             }
        }

        /*
        private void treeViewEvents_ItemDrag(object sender, ItemDragEventArgs e)
        {
            treeViewEvents.DoDragDrop(e.Item, DragDropEffects.Move);
        }
        private void treeViewEvents_DragDrop(object sender, DragEventArgs e)
        {
            Point pos = treeViewEvents.PointToClient(new Point(e.X, e.Y));
            TreeNode dropNode = this.treeViewEvents.GetNodeAt(pos);
            G2DTreeNodeBase child_node = (G2DTreeNodeBase)e.Data.GetData(typeof(EventTreeNode));
            G2DTreeNodeBase group_node = (G2DTreeNodeBase)e.Data.GetData(typeof(G2DTreeNodeGroup));
            if (dropNode is G2DTreeNodeGroup)
            {
                if (child_node == null && group_node == null)
                {
                    MessageBox.Show("error");
                }
                else if (child_node != null)
                {
                    child_node.RemoveFromParent();
                    dropNode.Nodes.Add(child_node);
                    dropNode.Expand();
                    //treeViewEvents.SelectedNode = child_node;
                }
                else if (group_node != dropNode)
                {
                    if (!G2DTreeNodeBase.ContainsChild(group_node, dropNode, true))
                    {
                        group_node.RemoveFromParent();
                        dropNode.Nodes.Add(group_node);
                        dropNode.Expand();
                        //treeViewEvents.SelectedNode = group_node;
                    }
                }
            }
        }
        private void treeViewEvents_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(EventTreeNode)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent(typeof(G2DTreeNodeGroup)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void treeViewEvents_DragOver(object sender, DragEventArgs e)
        {
            Point pos = treeViewEvents.PointToClient(new Point(e.X, e.Y));
            TreeNode dropNode = this.treeViewEvents.GetNodeAt(pos);
            if (dropNode is G2DTreeNodeBase)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        */

        private void TreeViewEvents_SelectionChanged(object sender, TreeViewEventArgs e)
        {
            if (e.Node is EventTreeNode)
            {
                //this.eventMenuStrip.Enabled = true;
                OnSelectChangeEventObject(e.Node as EventTreeNode);
            }
            else
            {
                //this.eventMenuStrip.Enabled = false;
                OnSelectChangeEventObject(null);
            }
        }
        //         private void treeViewEvents_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        //         {
        //             //treeViewEvents.TreeView.SelectedNode = e.Node;
        //         }
        //         private void treeViewEvents_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        //         {
        //             //treeViewEvents.TreeView.SelectedNode = e.Node;
        //         }
        //         private void treeViewEvents_AfterSelect(object sender, TreeViewEventArgs e)
        //         {
        //             if (e.Node is EventTreeNode)
        //             {
        //                 //this.eventMenuStrip.Enabled = true;
        //                 OnSelectChangeEventObject(e.Node as EventTreeNode);
        //             }
        //             else
        //             {
        //                 //this.eventMenuStrip.Enabled = false;
        //                 OnSelectChangeEventObject(null);
        //             }
        //         }

        private void treeViewEvents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (treeViewEvents.SelectedNode != null)
                {
                    if (MessageBox.Show("确认删除事件 " + GetSelectedEvent(), "确认删除", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        RemoveEvent();
                    }
                }
            }
        }
        private void treeViewEvents_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            event_OnSaving?.Invoke(this);
            SaveData();
        }


        private void btn_CopyToClipboard_Click(object sender, EventArgs e)
        {
            if (treeViewEvents.TreeView.Focused)
            {
                TreeNode node = treeViewEvents.TreeView.SelectedNode;
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

        private void btn_Copy_Click(object sender, EventArgs e)
        {
            if (treeViewEvents.TreeView.Focused)
            {
                TreeNode node = treeViewEvents.TreeView.SelectedNode;
                if (node != null)
                {
                    try
                    {
                        Win32.SetClipboard(node.Text);
                    }
                    catch { }
                }
                EventTreeNode tn = GetSelectedEvent();
                if (tn != null)
                {
                    var copyEventNode = XmlUtil.CloneObject<IEventDataNode>(tn.Data);
                    s_copying.Copy(copyEventNode);
                }
            }
        }
        private void btn_Paste_Click(object sender, EventArgs e)
        {
            if (treeViewEvents.TreeView.Focused)
            {
                if (s_copying.TryPaste(out IEventDataNode copyEventNode))
                {
                    CloneEvent(copyEventNode);
                }
            }
        }
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (treeViewEvents.TreeView.Focused)
            {
                if (MessageBox.Show(
                "确认删除事件: " + GetSelectedEvent(),
                "确认删除",
                MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    RemoveEvent();
                }
            }
        }

        private void btn_EnvVars_Click(object sender, EventArgs e)
        {
            ShowEnvironmentVarEditor();
        }

        private void chk_EnableEvent_CheckedChanged(object sender, EventArgs e)
        {
            EventTreeNode evt = GetSelectedEvent();
            if (evt != null)
            {
                evt.Data.EventIsActive = chk_EnableEvent.Checked;
                evt.Refresh();
            }
        }
        private void txt_EventComment_TextChanged(object sender, EventArgs e) { }


        private void btn_Run_Click(object sender, EventArgs e)
        {
            event_OnExecute?.Invoke(this);
        }

        #endregion
        //--------------------------------------------------------------------------------------
        #region GroupMenu

        private void groupMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            bool single = (treeViewEvents.TreeView.SelectedNode is EventTreeNode);
            bool group = !single;

            menu_Enable.Enabled = single;

            menu_AddGroup.Enabled = group;
            menu_AddZoneEvent.Enabled = group;

            menu_CopyZoneEvent.Enabled = single;
            menu_DeleteZoneEvent.Enabled = single;
            menu_RenameZoneEvent.Enabled = true;

            menu_ParseZoneEvent.Enabled = true;

            menu_ConvertToBehavior.Enabled = single;

            menu_OpenAll.Enabled = group;
            menu_CloseAll.Enabled = group;
            menu_DeleteAll.Enabled = group;

            var node = (treeViewEvents.TreeView.SelectedNode as EventTreeNode);
            if (node != null)
            {
                menu_Enable.Checked = node.Data.EventIsActive;
            }
        }

        private void menu_AddZoneEvent_Click(object sender, EventArgs e)
        {
            AddEvent();
        }
        private void menu_RenameZoneEvent_Click(object sender, EventArgs e)
        {
            if (treeViewEvents.TreeView.SelectedNode is EventTreeNode single)
            {
                RenameEvent();
            }
            else if (treeViewEvents.TreeView.SelectedNode is G2DTreeNodeGroup group)
            {
                RenameGroup();
            }
        }
        private void menu_AddGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGroup();
        }
        private void menu_CopyZoneEvent_Click(object sender, EventArgs e)
        {
            EventTreeNode node = GetSelectedEvent();
            if (node != null)
            {
              var  copyEventNode = XmlUtil.CloneObject<IEventDataNode>(node.Data);
                s_copying.Copy(copyEventNode);
            }
        }
        private void menu_ParseZoneEvent_Click(object sender, EventArgs e)
        {
            if (s_copying.TryPaste(out IEventDataNode copyEventNode))
            {
                CloneEvent(copyEventNode);
            }
        }
        private void menu_DeleteZoneEvent_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "确认删除事件: " + GetSelectedEvent(),
                "确认删除",
                MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                RemoveEvent();
            }
        }
        private void menu_OpenAll_Click(object sender, EventArgs e)
        {
            var group = GetSelectedGroup();
            if (group != null)
            {
                SetGroupActive(group, true);
            }
        }
        private void menu_CloseAll_Click(object sender, EventArgs e)
        {
            var group = GetSelectedGroup();
            if (group != null)
            {
                SetGroupActive(group, false);
            }
        }
        private void menu_DeleteAll_Click(object sender, EventArgs e)
        {
            var group = GetSelectedGroup();
            if (group != null)
            {
                if (MessageBox.Show(
                  "确认删除所有事件: " + group.Text,
                  "确认删除",
                  MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    RemoveGroupEvents(group);
                }
            }
        }

        private void menu_EnableItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (chk_EnableEvent.Checked != menu_Enable.Checked)
            {
                chk_EnableEvent.Checked = menu_Enable.Checked;
            }
        }

        private void menu_ConvertToBehavior_Click(object sender, EventArgs e)
        {
            if (treeViewEvents.TreeView.SelectedNode is EventTreeNode et)
            {
                if (Behavior.DoLoadFromBehavior(et.Data))
                {
                    if (MessageBox.Show(
                        $"转换完成！保留原始事件\"{et.Data}\"？",
                        "保留原始事件？", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        this.Award.ClearEventNodes();
                    }
                }
            }
        }

        #endregion

        //-------------------------------------------------------------------------------------------------------


        /// <summary>
        /// 事件节点选择
        /// 会将右边的数据保存，并初始化新的右边编辑栏
        /// </summary>
        /// <param name="node"></param>
        private void OnSelectChangeEventObject(EventTreeNode node)
        {
            if (editEventNode != null)
            {
                editEventNode.Data.EventIsActive = chk_EnableEvent.Checked;
                editEventNode.Data.EventComment = txt_EventComment.Text;
            }
            editEventNode = node;
            if (node != null)
            {
                this.chk_EnableEvent.Checked = node.Data.EventIsActive;
                this.txt_EventComment.Text = node.Data.EventComment;

                this.Award.SetData(node);
                this.Behavior.SetData(node);
            }
            else
            {
                this.chk_EnableEvent.Checked = false;
                this.txt_EventComment.Clear();

                this.Award.SetData(null);
                this.Behavior.SetData(null);
            }
        }
        private void OnSaveEditEventNode()
        {
            if (editEventNode != null)
            {
                editEventNode.Data.EventIsActive = chk_EnableEvent.Checked;
                editEventNode.Data.EventComment = txt_EventComment.Text;

                this.Award.OnSaveEditEventNode(editEventNode.Data);
                this.Behavior.OnSaveEditEventNode(editEventNode.Data);
            }
        }

        //----------------------------------------------------------------------------------------

        public EventLocalVar GetSelectedEventLocalVar(string key)
        {
            if (editEventNode != null)
            {
                foreach (EventLocalVar lcv in editEventNode.Data.EventLocalVars)
                {
                    if (string.Equals(key, lcv.Key))
                    {
                        return lcv;
                    }
                }
                if (editEventNode.Data.EventBehavior != null)
                {
                    foreach (EventLocalVar lcv in editEventNode.Data.EventBehavior.GetEventLocalVars())
                    {
                        if (string.Equals(key, lcv.Key))
                        {
                            return lcv;
                        }
                    }
                }
            }
            return null;
        }

        public EventLocalVar ShowSelectLocalVar(string srcKey, Type valueType)
        {
            if (editEventNode != null)
            {
                List<EventLocalVar> vars = new List<EventLocalVar>();
                //                 foreach (EventLocalVar lcv in editEventNode.Data.EventLocalVars)
                //                 {
                //                     if (valueType == null || valueType.IsAssignableFrom(lcv.ValueType))
                //                     {
                //                         vars.Add(lcv);
                //                     }
                //                 }
                //                 if (editEventNode.Data.EventBehaviors != null)
                //                 {
                //                     foreach (EventLocalVar lcv in editEventNode.Data.EventBehaviors.GetEventLocalVars())
                //                     {
                //                         if (valueType == null || valueType.IsAssignableFrom(lcv.ValueType))
                //                         {
                //                             vars.Add(lcv);
                //                         }
                //                     }
                //                 }
                this.awardPanel1.ListEventLocalVar(lcv =>
                {
                    if (valueType == null || valueType.IsAssignableFrom(lcv.ValueType))
                    {
                        vars.Add(lcv);
                    }
                }); 
                this.behaviorPanel1.ListEventLocalVar(lcv =>
                {
                    if (valueType == null || valueType.IsAssignableFrom(lcv.ValueType))
                    {
                        vars.Add(lcv);
                    }
                });
                EventLocalVar src = GetSelectedEventLocalVar(srcKey);
                G2DListSelectEditor<EventLocalVar> dialog = new G2DListSelectEditor<EventLocalVar>(vars, src);
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return dialog.SelectedTag;
                }
            }
            return null;
        }

        public IEventDataNode ShowSelectEvent(string srcName)
        {
            var events = GetEventDataNodes();
            var src = events.Find(t => srcName == t.EventName);
            var dialog = new G2DListSelectEditor<IEventDataNode>(
                    events, src);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedTag as IEventDataNode;
            }
            return null;
        }
        public string ShowSelectEventGroup(string path)
        {
            var droot = EventTreeRoot.TreeNodeDuplicate();
            var dialog = new G2DListSelectEditor<IEventDataNode>(droot, EventTreeImageList, path);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedTreeSavePath;
            }
            return null;
        }

        //------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------


    }
}