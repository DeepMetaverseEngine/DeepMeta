using DeepCore;
using DeepCore.EventTrigger;
using DeepCore.EventTrigger.Data;
using DeepCore.GUI.Data;
using DeepCore.Reflection;
using DeepCore.Xml;
using DeepEditor.Common.EventEditor.DescAttributeEdit;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G2D.DataGrid;
using DeepEditor.Common.Windows;
using MaterialSkin;
using ST.Library.UI.NodeEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeepEditor.Common.EventEditor.BehaviorEditor
{
    public partial class BehaviorPanel : UserControl, IG2DBaseComponent, IEventNodeEditor
    {
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public Color? CustomForeColor { get; set; }
        public Color? CustomBackColor { get; set; }
        private BehaviorNodeEditor NodeEditor { get => stNodeEditor1; }
        private ValueTypesTreeViewControl TypesTreeView { get => valueTypesTreeViewControl1; }
        private STBehaviorNode SelectedNode { get => NodeEditor.ActiveNode as STBehaviorNode; }

        private EventEditor eventEditor;
        private IEventDataNode editEventNode;
        private EventEditor.EventTreeNode editTreeNode;

        //         private Image icon_var;
        //         private Image icon_trigger;
        //         private Image icon_condition;
        //         private Image icon_action;
        //         private Image icon_value;
        //         private Image icon_question;

        public BehaviorPanel()
        {
            InitializeComponent();
            MaterialSkinManager.AddIgnoreControlType(this.stNodeEditor1.GetType());
        }

        public void ListEventLocalVar(Action<EventLocalVar> vars)
        {
            foreach (var node in this.NodeEditor.Nodes)
            {
                if (node is STLocalVarNode localVar)
                {
                    vars(localVar.EventData);
                }
            }
        }

        public void Init(EventEditor eventEditor)
        {
            // this.cmd_queue.OnListChanged += Cmd_queue_OnListChanged;
            this.eventEditor = eventEditor;
            this.nodeProp.SelectedRootObject = eventEditor.RootObject;
            {
                //MaterialSkinManager.Instance.AddIgnoreControlType(this.stNodeTreeView1.GetType());
                {
                    stNodeEditor1.GridToSize = this.chk_Grid.Checked;
                    stNodeEditor1.OptionConnected += (s, ea) => stNodeEditor1.ShowAlert(ea.Status.ToString(), Color.White, ea.Status == ConnectionStatus.Connected ? Color.FromArgb(125, Color.Green) : Color.FromArgb(125, Color.Red));
                    stNodeEditor1.CanvasScaled += (s, ea) => stNodeEditor1.ShowAlert(stNodeEditor1.CanvasScale.ToString("F2"), Color.White, Color.FromArgb(125, Color.Yellow));
                    stNodeEditor1.NodeAdded += (s, ea) => ea.Node.ContextMenuStrip = nodeMenu;
                }
                foreach (var valueType in ValueTypeNameSpace.Instance.ValueTypes)
                {
                    {
                        var vitem = new G2DBaseToolStripMenuItem()
                        {
                            Text = $"添加 {STBehaviorLayout.GetValueTypeName(valueType.ValueType.OwnerType)}",
                            BackColor = STBehaviorLayout.GetValueColor(valueType.ValueType.OwnerType),
                            Tag = valueType,
                        };
                        vitem.Click += btn_AddValue_Click;
                        btn_AddValue.DropDownItems.Add(vitem);
                    }
                    {
                        var vitem = new G2DBaseToolStripMenuItem()
                        {
                            Text = $"添加 {STBehaviorLayout.GetValueTypeName(valueType.ValueType.OwnerType)}",
                            BackColor = STBehaviorLayout.GetValueColor(valueType.ValueType.OwnerType),
                            Tag = valueType,
                        };
                        vitem.Click += tool_AddValue_Click;
                        tool_AddValue.DropDownItems.Add(vitem);
                    }
                }
                //this.stNodeEditor1.Nodes.Clear();
                this.stNodeEditor1.ShowGrid = false;
                this.menu_Items.Enabled = false;

                this.stNodeEditor1.ActiveChanged += StNodeEditor1_ActiveChanged;
                this.nodeProp.SelectedObjectsChanged += NodeProp_SelectedObjectsChanged;
                this.nodeProp.PropertyValueChanged += NodeProp_PropertyValueChanged;
                this.nodeProp.OnCommit += NodeProp_OnCommit;

                this.TypesTreeView.TreeView.MouseDown += TreeView_MouseDown;
                this.NodeEditor.DragEnter += NodeEditor_DragEnter;
                this.NodeEditor.DragDrop += NodeEditor_DragDrop;
                //                 this.NodeEditor.DrawNodeAfter += NodeEditor_DrawNodeAfter;
                this.NodeEditor.MouseWheel += NodeEditor_MouseWheel;
                //                 this.NodeEditor.MouseDown += NodeEditor_MouseDown;
                this.NodeEditor.NodesBeginMove += NodeEditor_NodesBeginMove;
                this.NodeEditor.NodesAfterMoved += NodeEditor_NodesAfterMoved;
            }
            {
                NodeEditor.icon_question = this.eventEditor.EventTreeImageList.Images["Question.png"];
                NodeEditor.icon_var = this.eventEditor.EventTreeImageList.Images["icon_var.png"];
                NodeEditor.icon_trigger = this.eventEditor.EventTreeImageList.Images["icon_trigger.png"];
                NodeEditor.icon_condition = this.eventEditor.EventTreeImageList.Images["icon_condition.png"];
                NodeEditor.icon_action = this.eventEditor.EventTreeImageList.Images["icon_run.png"];
                NodeEditor.icon_value = this.eventEditor.EventTreeImageList.Images["icon_value.png"];

                TypesTreeView.ImageList = this.eventEditor.EventTreeImageList;
                TypesTreeView.ImageKey = "icons_tool_bar2.png";
                TypesTreeView.SelectedImageKey = "icons_tool_bar2.png";

                {
                    var gLovalVar = TypesTreeView.Nodes.Add(typeof(EventLocalVar).FullName, "临时变量");
                    var gTrigger = TypesTreeView.Nodes.Add(typeof(AbstractTrigger).FullName, "事件开端");
                    var gCondition = TypesTreeView.Nodes.Add(typeof(AbstractCondition).FullName, "事件条件");
                    var gAction = TypesTreeView.Nodes.Add(typeof(AbstractAction).FullName, "事件动作");
                    var gValue = TypesTreeView.Nodes.Add(typeof(AbstractValue).FullName, "事件数据");
                    {
                        gLovalVar.ImageKey = "icon_var.png";
                        gLovalVar.SelectedImageKey = "icon_var.png";
                        //gTrigger.ForeColor = STBehaviorLayout.GetValueColor(typeof(AbstractTrigger));
                        TypesTreeView.Init(typeof(EventLocalVar), gLovalVar);
                        btn_AddLocalVar.Image = NodeEditor.icon_var;
                        tool_AddLocalVar.Image = NodeEditor.icon_var;
                    }
                    {
                        gTrigger.ImageKey = "icon_trigger.png";
                        gTrigger.SelectedImageKey = "icon_trigger.png";
                        //gTrigger.ForeColor = STBehaviorLayout.GetValueColor(typeof(AbstractTrigger));
                        TypesTreeView.Init(typeof(AbstractTrigger), gTrigger);
                        btn_AddTrigger.Image = NodeEditor.icon_trigger;
                        tool_AddTrigger.Image = NodeEditor.icon_trigger;
                    }
                    {
                        gCondition.ImageKey = "icon_condition.png";
                        gCondition.SelectedImageKey = "icon_condition.png";
                        //gAction.ForeColor = STBehaviorLayout.GetValueColor(typeof(AbstractAction));
                        TypesTreeView.Init(typeof(AbstractCondition), gCondition);
                        btn_AddCondition.Image = NodeEditor.icon_condition;
                        tool_AddCondition.Image = NodeEditor.icon_condition;
                    }
                    {
                        gAction.ImageKey = "icon_run.png";
                        gAction.SelectedImageKey = "icon_run.png";
                        //gAction.ForeColor = STBehaviorLayout.GetValueColor(typeof(AbstractAction));
                        TypesTreeView.Init(typeof(AbstractAction), gAction);
                        btn_AddAction.Image = NodeEditor.icon_action;
                        tool_AddAction.Image = NodeEditor.icon_action;
                    }
                    {
                        gValue.ImageKey = "icon_value.png";
                        gValue.SelectedImageKey = "icon_value.png";
                        //gValue.ForeColor = STBehaviorLayout.GetValueColor(typeof(AbstractValue));
                        //TypesTreeView.Init(typeof(AbstractValue), gValue);
                        btn_AddValue.Image = NodeEditor.icon_value;
                        tool_AddValue.Image = NodeEditor.icon_value;
                        foreach (var valueType in ValueTypeNameSpace.Instance.ValueTypes)
                        {
                            var vtext = STBehaviorLayout.GetValueTypeName(valueType.ValueType.OwnerType);
                            var vnode = gValue.Nodes.Add(valueType.ValueType.OwnerType.FullName, $"{vtext}");
                            vnode.ForeColor = STBehaviorLayout.GetValueColor(valueType.ValueType.OwnerType);
                            TypesTreeView.Init(valueType.ValueType.OwnerType, vnode);
                        }
                    }
                    gTrigger.Collapse();
                    gAction.Collapse();
                    gValue.Collapse();
                }
            }
            this.TypesTreeView.TreeView.LoadState(new FileInfo(Path.Combine(Application.UserAppDataPath, $"{this.GetType().Name}.{nameof(TypesTreeView)}.tree")), new TreeStateInfoConfig()
            {
                removeEmptyGroup = true,
                reIndex = false,
                select = true,
            });
            this.eventEditor.FormClosing += EventEditor_FormClosing;
            if (TypesTreeView.TreeView.SelectedNode != null)
            {
                TypesTreeView.TreeView.SelectedNode.EnsureVisible();
            }
        }


        //---------------------------------------------------------------------------------------------

        private void EventEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.TypesTreeView.TreeView.SaveState(new FileInfo(Path.Combine(Application.UserAppDataPath, $"{this.GetType().Name}.{nameof(TypesTreeView)}.tree")));
        }

        public void SetData(EventEditor.EventTreeNode enode)
        {
            NodeEditor.ResetHelp(null);
            this.editTreeNode = enode;
            this.stNodeEditor1.SuspendLayout();
            try
            {
                OnSaveEditEventNode(editEventNode);
                var node = enode?.Data;
                editEventNode = node;
                if (node?.EventBehavior?.Nodes != null)
                {
                    this.stNodeEditor1.Nodes.Clear();
                    NodeEditor.LoadNodes(node.EventBehavior.Nodes);
                    this.TypesTreeView.Enabled = true;
                    this.TypesTreeView.TreeView.Visible = true;
                    this.nodeProp.Enabled = true;
                    this.menu_Items.Enabled = true;
                    this.stNodeEditor1.ShowGrid = true;
                }
                else
                {
                    this.stNodeEditor1.Nodes.Clear();
                    this.TypesTreeView.Enabled = false;
                    this.TypesTreeView.TreeView.Visible = false;
                    this.nodeProp.SetSelectedObject(null);
                    this.nodeProp.Enabled = false;
                    this.menu_Items.Enabled = false;
                    this.stNodeEditor1.ShowGrid = false;
                }
                SetCMD(enode?.CmdQueue);
            }
            finally
            {
                this.stNodeEditor1.ResumeLayout();
            }
        }
        public void OnSaveEditEventNode(IEventDataNode node)
        {
            NodeEditor.ResetHelp(null);
            if (node?.EventBehavior?.Nodes != null)
            {
                node.EventBehavior.Nodes.Clear();
                foreach (STBehaviorNode stnode in stNodeEditor1.Nodes)
                {
                    stnode.Save(NodeEditor);
                    node.EventBehavior.Nodes.Add(stnode.NodeData);
                }
                node.EventBehavior.Nodes.Sort((a, b) => string.Compare(a.GUID, b.GUID));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        //         public EventBehaviorData DumpToData()
        //         {
        //             var list = new List<EventBehaviorNode>();
        //             foreach (STBehaviorNode node in stNodeEditor1.Nodes)
        //             {
        //                 node.Save(this, true);
        //                 var cnode = XmlUtil.CloneObject(node.NodeData);
        //                 list.Add(cnode);
        //             }
        //             var data = new EventBehaviorData() { Nodes = list };
        //             return data;
        //         }
        //         public EventBehaviorData DumpToData(List<STNode> selected)
        //         {
        //             var list = new List<EventBehaviorNode>();
        //             var pt = stNodeEditor1.GetMouseCanvasPoint();
        //             foreach (STBehaviorNode node in selected)
        //             {
        //                 node.Save(this);
        //                 var cnode = XmlUtil.CloneObject(node.NodeData);
        //                 cnode.ForEachOptions(0, (st, link, dock) =>
        //                 {
        //                     if (!selected.Exists(st => link.NextGUID == st.GUID))
        //                     {
        //                         if (dock == LinkDock.Input)
        //                         {
        //                             cnode.Inputs.Remove(link);
        //                         }
        //                         else if (dock == LinkDock.Output)
        //                         {
        //                             cnode.Outputs.Remove(link);
        //                         }
        //                     }
        //                 });
        //                 cnode.EditorX = node.Left - pt.X;
        //                 cnode.EditorY = node.Top - pt.Y;
        //                 list.Add(cnode);
        //             }
        //             var data = new EventBehaviorData() { Nodes = list };
        //             data.ReHash();
        //             return data;
        //         }
        //         private void ResetDump(EventBehaviorData dump)
        //         {
        //             var mtx = NodeEditor.SaveCanvasTransform();
        //             this.stNodeEditor1.SuspendLayout();
        //             try
        //             {
        //                 ResetHelp(null);
        //                 this.stNodeEditor1.Nodes.Clear();
        //                 this.LoadNodes(dump.Nodes, true);
        //             }
        //             finally
        //             {
        //                 this.stNodeEditor1.ResumeLayout();
        //                 NodeEditor.SetCanvasTransform(mtx);
        //             }
        //         }
        private bool InitBehavior(STBehaviorNode bnode, EventExternalizable src, EventExternalizable dst)
        {
            bnode.ForEachFieldLinks((dock, field, link) =>
            {
                if (link.ConnectionCount > 0)
                {
                    field.SetValue(dst, null);
                }
            });
            var converter = new EventBehaviorDataConveter();
            var pt = bnode.Location;
            converter.StartX = pt.X;
            converter.StartY = pt.Y;
            var behaviorNodes = converter.ConvertTo(dst, out var main);
            var fieldValues = new HashMap<string, EventBehaviorNode>();
            behaviorNodes.ForEachFields(main, (main, field, fieldNode) =>
            {
                fieldValues.Add(field.Field.Name, fieldNode);
            });
            behaviorNodes.Nodes.Remove(main);
            if (behaviorNodes.Nodes.Count > 0)
            {
                var nodes = NodeEditor.LoadNodes(behaviorNodes.Nodes);
                bnode.ForEachFieldLinks((dock, field, link) =>
                {
                    if (link.ConnectionCount == 0)
                    {
                        if (fieldValues.TryGetValue(field.Name, out var fdnode))
                        {
                            var fnode = nodes.Find(dn => dn.NodeData == fdnode);
                            if (fnode != null && bnode.TryGetNodeFieldOption(dock, field.Name, out var op))
                            {
                                if (dock == LinkDock.Input)
                                {
                                    op.ConnectOption(fnode.MainOutput);
                                }
                                else if (dock == LinkDock.Output)
                                {
                                    op.ConnectOption(fnode.MainInput);
                                }
                            }
                        }
                    }
                });
                return true;
            }
            return false;
        }
        private bool InitBehavior(STNode node)
        {
            if (node is STBehaviorNode bnode && bnode.NodeData?.EventData != null)
            {
                var src = bnode.NodeData?.EventData;
                var dst = DeepActivator.CreateInstance(bnode.NodeData.EventData.GetType()) as EventExternalizable;
                if (false)
                {
                    bnode.ForEachFieldLinks((dock, field, link) =>
                    {
                        if (link.ConnectionCount > 0)
                        {
                            field.SetValue(dst, null);
                        }
                    });
                    var converter = new EventBehaviorDataConveter();
                    var pt = node.Location;
                    converter.StartX = pt.X;
                    converter.StartY = pt.Y;
                    var behaviorNodes = converter.ConvertTo(dst, out var main);
                    var fieldValues = new HashMap<string, EventBehaviorNode>();
                    behaviorNodes.ForEachFields(main, (main, field, fieldNode) =>
                    {
                        fieldValues.Add(field.Field.Name, fieldNode);
                    });
                    behaviorNodes.Nodes.Remove(main);
                    if (behaviorNodes.Nodes.Count > 0)
                    {
                        var nodes = NodeEditor.LoadNodes(behaviorNodes.Nodes);
                        bnode.ForEachFieldLinks((dock, field, link) =>
                        {
                            if (link.ConnectionCount == 0)
                            {
                                if (fieldValues.TryGetValue(field.Name, out var fdnode))
                                {
                                    var fnode = nodes.Find(dn => dn.NodeData == fdnode);
                                    if (fnode != null && bnode.TryGetNodeFieldOption(dock, field.Name, out var op))
                                    {
                                        if (dock == LinkDock.Input)
                                        {
                                            op.ConnectOption(fnode.MainOutput);
                                        }
                                        else if (dock == LinkDock.Output)
                                        {
                                            op.ConnectOption(fnode.MainInput);
                                        }
                                    }
                                }
                            }
                        });
                        NodeEditor.AutoLayoutTree(node, false);
                        return true;
                    }
                }
                if (InitBehavior(bnode, src, dst))
                {
                    //NodeEditor.AutoLayoutTree(node, false);
                    return true;
                }
            }
            return false;
        }

        private bool ChangeBehavior(STNode node)
        {
            if (node is STBehaviorNode bnode && bnode.NodeData?.EventData != null)
            {
                var pos = node.Location;
                var src = bnode.NodeData?.EventData;
                if (src != null)
                {
                    var srcMainInput = bnode.MainInput?.ConnectedOption.ToArray();
                    var srcMainOutput = bnode.MainOutput?.ConnectedOption.ToArray();
                    var fieldsMap = new HashMap<string, List<STBehaviorNode>>();
                    var fieldsValues = new HashMap<string, object>();
                    bnode.ForEachFieldNodes((field, dock, node) =>
                    {
                        if (node != null)
                        {
                            fieldsMap.GetOrNew(field.Name).Add(node);
                        }
                    });
                    bnode.ForEachPrimitiveFields((field) =>
                    {
                        fieldsValues.Add(field.Name, field.GetValue(src));
                    });
                    bnode.ForEachBehaviorFields(f => { });
                    var dst = eventEditor.ShowEditDialog(src.BaseType, src);
                    if (dst != null)
                    {
                        stNodeEditor1.Nodes.Remove(bnode);
                        var dnode = NodeEditor.AddNode(dst, bnode.NodeData.Inputs, bnode.NodeData.Outputs, null, true);
                        {
                            dnode.NodeData.GUID = bnode.NodeData.GUID;
                            dnode.NodeData.EditorX = bnode.NodeData.EditorX;
                            dnode.NodeData.EditorY = bnode.NodeData.EditorY;
                            dnode.NodeData.EditorARGB = bnode.NodeData.EditorARGB;
                            dnode.NodeData.EditorTag = bnode.NodeData.EditorTag;
                        }
                        dnode.Location = pos;
                        dnode.ForEachBehaviorFields(f =>
                        {
                            if (fieldsMap.TryGetValue(f.Name, out var links))
                            {
                                foreach (var link in links)
                                {
                                    if (dnode.TryGetNodeFieldOption(f.Name, out var ff, out var fd, out var fop))
                                    {
                                        if (fd == LinkDock.Output)
                                        {
                                            fop.ConnectOption(link.MainInput);
                                        }
                                        else
                                        {
                                            fop.ConnectOption(link.MainOutput);
                                        }
                                    }
                                }
                            }
                        });
                        dnode.ForEachPrimitiveFields(f =>
                        {
                            if (fieldsValues.TryGetValue(f.Name, out var fieldValue))
                            {
                                try
                                {
                                    f.SetValue(dst, fieldValue);
                                }
                                catch { }
                            }
                        });
                        if (srcMainInput != null && dnode.MainInput != null)
                        {
                            foreach (var mainInput in srcMainInput)
                            {
                                dnode.MainInput.ConnectOption(mainInput);
                            }
                        }
                        if (srcMainOutput != null && dnode.MainOutput != null)
                        {
                            foreach (var mainOutput in srcMainOutput)
                            {
                                dnode.MainOutput.ConnectOption(mainOutput);
                            }
                        }
                        InitBehavior(dnode, src, XmlUtil.CloneObject(dst));
                        dnode.Load(NodeEditor, true);
                        StNodeEditor1_ActiveChanged(stNodeEditor1, new EventArgs());
                        return true;
                    }
                }
            }
            return false;
        }

        //         private STBehaviorNode LoadFromBehavior(EventExternalizable data, Point? location, bool single)
        //         {
        //             ResetHelp(null);
        //             if (data != null)
        //             {
        //                 try
        //                 {
        //                     if (single)
        //                     {
        //                         var stnode = STBehaviorLayout.CreateNode(data);
        //                         LoadNode(stnode, location);
        //                         if (location != null)
        //                         {
        //                             var pt = NodeEditor.ControlToCanvas(location.Value);
        //                             stnode.Left = pt.X;
        //                             stnode.Top = pt.Y;
        //                         };
        //                         return stnode;
        //                     }
        //                     else
        //                     {
        //                         var converter = new EventBehaviorDataConveter();
        //                         if (location != null)
        //                         {
        //                             var pt = NodeEditor.ControlToCanvas(location.Value);
        //                             converter.StartX = pt.X;
        //                             converter.StartY = pt.Y;
        //                         };
        //                         var behaviorNodes = converter.ConvertTo(data, out var main);
        //                         var nodes = LoadNodes(behaviorNodes.Nodes);
        //                         AutoLayoutTree(nodes[0], false);
        //                         return nodes[0];
        //                     }
        //                 }
        //                 catch (Exception err)
        //                 {
        //                     err.ShowMessageBox(this);
        //                 }
        //             }
        //             return null;
        //         }
        //         private bool LoadFromBehavior(IEventDataNode node)
        //         {
        //             ResetHelp(null);
        //             if (node != null)
        //             {
        //                 try
        //                 {
        //                     var converter = new EventBehaviorDataConveter();
        //                     var behaviorNodes = converter.ConvertTo(node);
        //                     var nodes = LoadNodes(behaviorNodes.Nodes);
        //                     if (nodes.Count > 0)
        //                     {
        //                         var main = nodes[0];
        //                         NodeEditor.ClearSelected();
        //                         foreach (var n in nodes)
        //                         {
        //                             if (n is STLocalVarNode var)
        //                             {
        //                                 AutoLayoutTree(var, false);
        //                             }
        //                             else if (n is STTriggerNode trigger)
        //                             {
        //                                 main = trigger;
        //                                 AutoLayoutTree(trigger, false);
        //                             }
        //                         }
        //                         NodeEditor.ClearSelected();
        //                         SelectTree(main);
        //                         return true;
        //                     }
        //                 }
        //                 catch (Exception err)
        //                 {
        //                     err.ShowMessageBox(this);
        //                 }
        //             }
        //             return false;
        //         }

        public bool DoLoadFromBehavior(IEventDataNode node)
        {
            return DoCMDFunc(() =>
            {
                return NodeEditor.ConvertFromBehavior(node);
            });
        }
        private void Search()
        {
            NodeEditor.ShowSearchDialog();
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Selection Layout ---------------------------------------------------------------------------------------------
        //         public void SelectAll()
        //         {
        //             NodeEditor.SetSelectedNodes(NodeEditor.Nodes.ToArray());
        //         }
        //         private List<STNode> SelectTree(STNode active)
        //         {
        //             var func = new SelectTreeNode((STNode src, STNodeOption srcP, STNode dst, STNodeOption dstP) =>
        //             {
        //                 if (srcP.IsInput && dst is STValueNode) { return true; }
        //                 if (src is STActionNode && srcP.IsOutput && dst is STActionNode) { return true; }
        //                 if (src is STTriggerNode && srcP.IsOutput && dst is STActionNode) { return true; }
        //                 return false;
        //             });
        //             return NodeEditor.SelectTree(active, true, func);
        //         }
        //         private void AutoLayoutInputs(STNode active, bool select = true)
        //         {
        //             var func1 = new SelectTreeNode((STNode src, STNodeOption srcP, STNode dst, STNodeOption dstP) =>
        //             {
        //                 if (srcP.IsInput && dst is STValueNode) { return true; }
        //                 return false;
        //             });
        //             var list1 = NodeEditor.AutoLayout(active, func1);
        //             if (select) { NodeEditor.SelectTree(active, true, func1); }
        //         }
        //         private void AutoLayoutTree(STNode active, bool select = true)
        //         {
        //             var func1 = new SelectTreeNode((STNode src, STNodeOption srcP, STNode dst, STNodeOption dstP) =>
        //             {
        //                 if (srcP.IsInput && dst is STValueNode) { return true; }
        //                 if (src is STActionNode && srcP.IsOutput && dst is STActionNode) { return true; }
        //                 if (src is STTriggerNode && srcP.IsOutput && dst is STActionNode) { return true; }
        //                 return false;
        //             });
        //             var list1 = NodeEditor.AutoLayout(active, func1);
        //             //NodeEditor.ExpandNodes(list1);
        //             if (select) { NodeEditor.SelectTree(active, true, func1); }
        //         }
        private void btn_AutoLayout_Click(object sender, EventArgs e)
        {
            tool_AutoLayoutTree_Click(sender, e);
        }
        private void btn_SelectAll_Click(object sender, EventArgs e)
        {
            NodeEditor.SelectAll();
        }
        private void tool_SelectTree_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.ActiveNode == null) return;
            NodeEditor.SelectTree(stNodeEditor1.ActiveNode);
        }
        private void tool_AutoLayout_Click(object sender, EventArgs e)
        {
            DoCMDAction(() => { NodeEditor.AutoLayoutInputs(stNodeEditor1.ActiveNode); });
        }
        private void tool_AutoLayoutTree_Click(object sender, EventArgs e)
        {
            DoCMDAction(() => { NodeEditor.AutoLayoutTree(stNodeEditor1.ActiveNode); });
        }
        private void tool_Clean_Click(object sender, EventArgs e)
        {
            //             DoCMDAction(() => { 
            //                 //AutoLayoutTree(stNodeEditor1.ActiveNode);
            //             });
        }


        private void stNodeEditor1_MouseUp(object sender, MouseEventArgs e)
        {
            if (editEventNode != null)
            {
                var selected = SelectedNode;
                if (selected == null && e.Button == MouseButtons.Right)
                {
                    if (stNodeEditor1.IsProcessMouseEvent)
                    {
                        nodeMenu.Show(stNodeEditor1, e.Location);
                    }
                }
            }
        }
        private void stNodeEditor1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tool_SelectTree_Click(sender, e);
        }
        private void stNodeEditor1_ActiveChanged_1(object sender, EventArgs e)
        {
            RefreshStatus();
        }
        private void stNodeEditor1_MouseMove(object sender, MouseEventArgs e)
        {
            RefreshStatus(e.Location);
        }
        private void StNodeEditor1_ActiveChanged(object sender, EventArgs e)
        {
            if (this.stNodeEditor1.ActiveNode is STBehaviorNode bnode)
            {
                var desc = G2DTypeDescriptor.CreateDescriptor(bnode.NodeData.EventData, eventEditor.CreatePropertyAdapters());
                desc.TryAcceptField += STBehaviorLayout.TryAcceptPropertyField;
                nodeProp.SetSelectedObject(desc);
            }
            else
            {
                nodeProp.SetSelectedObject(null);
            }
        }
        private EventBehaviorData beginMoveDump;
        private void NodeEditor_NodesBeginMove(object sender, STNodeEditorMoveNodeEventArgs e)
        {
            this.beginMoveDump = NodeEditor.DumpToData();
        }
        private void NodeEditor_NodesAfterMoved(object sender, STNodeEditorMoveNodeEventArgs e)
        {
            if (beginMoveDump != null)
            {
                DoCMDAction(beginMoveDump, () => { });
            }
        }


        private EventBehaviorData beginPropDump;
        private void NodeProp_OnCommit(object sender, G2DPropertyGrid.CommitEventArgs args)
        {
            beginPropDump = NodeEditor.DumpToData();
        }
        private void NodeProp_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (this.stNodeEditor1.ActiveNode is STBehaviorNode bnode)
            {
                DoCMDAction(beginPropDump, () =>
                {
                    bnode.Refresh();
                });
            }
        }
        private void NodeProp_SelectedObjectsChanged(object sender, EventArgs e)
        {
            if (this.stNodeEditor1.ActiveNode is STBehaviorNode bnode)
            {
                bnode.Refresh();
            }
        }

        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Add Remove Node ---------------------------------------------------------------------------------------------
        // 
        //         private List<STBehaviorNode> LoadNodes(ICollection<EventBehaviorNode> nodes, bool select = false)
        //         {
        //             var ret = new List<STBehaviorNode>();
        //             foreach (var bnode in nodes)
        //             {
        //                 if (bnode?.Data != null)
        //                 {
        //                     var node = STBehaviorLayout.CreateNode(bnode);
        //                     LoadNode(node, null);
        //                     ret.Add(node);
        //                 }
        //             }
        //             // after build relations
        //             foreach (STBehaviorNode stnode in ret)
        //             {
        //                 stnode.Load(this, select);
        //             }
        //             return ret;
        //         }
        //         private void LoadNode(STBehaviorNode node, Point? location)
        //         {
        //             if (node != null)
        //             {
        //                 if (TryGetNodeByGUID(node.Guid.ToString(), out var exist))
        //                 {
        //                     node.NewGuid();
        //                 }
        //                 Point pt = new Point(20, 20);
        //                 if (location.HasValue)
        //                 {
        //                     pt = location.Value;
        //                 }
        //                 //pt = stNodeEditor1.PointToClient(pt);
        //                 pt = stNodeEditor1.ControlToCanvas(pt);
        //                 node.Left = pt.X; node.Top = pt.Y;
        //                 if (node.NodeData.Data is EventLocalVar)
        //                 {
        //                     node.Icon = icon_var;
        //                 }
        //                 else if (node.NodeData.Data is AbstractTrigger)
        //                 {
        //                     node.Icon = icon_trigger;
        //                 }
        //                 else if (node.NodeData.Data is AbstractCondition)
        //                 {
        //                     node.Icon = icon_condition;
        //                 }
        //                 else if (node.NodeData.Data is AbstractAction)
        //                 {
        //                     node.Icon = icon_action;
        //                 }
        //                 else if (node.NodeData.Data is AbstractValue)
        //                 {
        //                     node.Icon = icon_value;
        //                 }
        //                 node.HelpIcon = this.icon_question;
        //                 this.stNodeEditor1.Nodes.Add(node);
        //             }
        //         }
        // 
        //         public bool TryGetNodeByGUID(string guid, out STBehaviorNode node)
        //         {
        //             foreach (STBehaviorNode stnode in stNodeEditor1.Nodes)
        //             {
        //                 if (stnode.Guid.ToString().Equals(guid))
        //                 {
        //                     node = stnode;
        //                     return true;
        //                 }
        //             }
        //             node = null;
        //             return false;
        //         }
        //         public bool TryGetNodeFieldByGUID(string guid, string fieldName, LinkDock fieldDock, out STBehaviorNode node, out STBehaviorOption option)
        //         {
        //             foreach (STBehaviorNode stnode in stNodeEditor1.Nodes)
        //             {
        //                 if (stnode.Guid.ToString().Equals(guid))
        //                 {
        //                     node = stnode;
        //                     if (stnode.TryGetNodeFieldOption(fieldDock, fieldName, out option))
        //                     {
        //                         return true;
        //                     }
        //                 }
        //             }
        //             option = null;
        //             node = null;
        //             return false;
        //         }
        //         private STBehaviorNode AddNode(EventExternalizable nodeData, Point? location, bool single = false)
        //         {
        //             var node = LoadFromBehavior(nodeData, location, single);
        //             //var node = STBehaviorLayout.CreateNode(nodeData);
        //             NodeEditor.ClearSelected();
        //             SelectTree(node);
        //             return node;
        //         }
        // 
        // 

        private void AddEventLocalVar(Point? location)
        {
            if (editEventNode != null)
            {
                var result = eventEditor.ShowAddDialog<EventLocalVar>();
                if (result != null)
                {
                    NodeEditor.AddNode(result, location);
                }
            }
        }
        private void AddEventTrigger(Point? location)
        {
            if (editEventNode != null)
            {
                var result = eventEditor.ShowAddDialog<AbstractTrigger>();
                if (result != null)
                {
                    NodeEditor.AddNode(result, location);
                }
            }
        }
        private void AddEventAction(Point? location)
        {
            if (editEventNode != null)
            {
                var result = eventEditor.ShowAddDialog<AbstractAction>();
                if (result != null)
                {
                    NodeEditor.AddNode(result, location);
                }
            }
        }
        private void AddEventValue(Point? location)
        {
            if (editEventNode != null)
            {
                var result = eventEditor.ShowAddDialog<AbstractValue>();
                if (result != null)
                {
                    NodeEditor.AddNode(result, location);
                }
            }
        }
        private void AddEventValue(Type valueType, Point? location)
        {
            if (editEventNode != null)
            {
                var result = eventEditor.ShowAddDialog(valueType);
                if (result is AbstractValue value)
                {
                    NodeEditor.AddNode(value, location);
                }
            }
        }
        private void AddEventCondition(Point? location)
        {
            if (editEventNode != null)
            {
                var result = eventEditor.ShowAddDialog<AbstractCondition>();
                if (result is AbstractCondition value)
                {
                    NodeEditor.AddNode(value, location);
                }
            }
        }
        private void AddGroup(Point? location)
        {
            if (editEventNode != null)
            {
                NodeEditor.AddGroup("GROUP", location);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------

        private void btn_AddLocalVar_Click(object sender, EventArgs e)
        {
            DoCMDAction(() => { AddEventLocalVar(null); });
        }
        private void btn_AddTrigger_Click(object sender, EventArgs e)
        {
            DoCMDAction(() => { AddEventTrigger(null); });
        }
        private void btn_AddCondition_Click(object sender, EventArgs e)
        {
            DoCMDAction(() => { AddEventCondition(null); });
        }
        private void btn_AddAction_Click(object sender, EventArgs e)
        {
            DoCMDAction(() => { AddEventAction(null); });
        }
        private void btn_AddValue_Click(object sender, EventArgs e)
        {
            DoCMDAction(() =>
            {
                if (sender is ToolStripMenuItem item && item.Tag is ValueTypeNameSpace.ValueTypeDefine valueType)
                {
                    AddEventValue(valueType.ValueType.OwnerType, null);
                }
                else
                {
                    AddEventValue(null);
                }
            });
        }

        private void btn_AddGroup_Click(object sender, EventArgs e)
        {
            DoCMDAction(() =>
            {
                AddGroup(null);
            });
        }

        private void tool_Init_Click(object sender, EventArgs e)
        {
            DoCMDCondition(() =>
            {
                return InitBehavior(stNodeEditor1.ActiveNode);
            });
        }

        private void tool_ChangeType_Click(object sender, EventArgs e)
        {
            DoCMDCondition(() =>
            {
                return ChangeBehavior(stNodeEditor1.ActiveNode);
            });
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------
        private void tool_AddLocalVar_Click(object sender, EventArgs e)
        {
            DoCMDAction(() =>
            {
                var pt = stNodeEditor1.GetMousePoint();
                AddEventLocalVar(pt);
            });
        }
        private void tool_AddTrigger_Click(object sender, EventArgs e)
        {
            DoCMDAction(() =>
            {
                var pt = stNodeEditor1.GetMousePoint();
                AddEventTrigger(pt);
            });
        }
        private void tool_AddCondition_Click(object sender, EventArgs e)
        {
            DoCMDAction(() =>
            {
                var pt = stNodeEditor1.GetMousePoint();
                AddEventCondition(pt);
            });
        }
        private void tool_AddAction_Click(object sender, EventArgs e)
        {
            DoCMDAction(() =>
            {
                var pt = stNodeEditor1.GetMousePoint();
                AddEventAction(pt);
            });
        }
        private void tool_AddValue_Click(object sender, EventArgs e)
        {
            DoCMDAction(() =>
            {
                var pt = stNodeEditor1.GetMousePoint();
                if (sender is ToolStripMenuItem item && item.Tag is ValueTypeNameSpace.ValueTypeDefine valueType)
                {
                    AddEventValue(valueType.ValueType.OwnerType, pt);
                }
                else
                {
                    AddEventValue(pt);
                }
            });
        }
        private void tool_AddGroup_Click(object sender, EventArgs e)
        {
            DoCMDAction(() =>
            {
                var pt = stNodeEditor1.GetMousePoint();
                AddGroup(pt);
            });
        }

        private void tool_Remove_Click(object sender, EventArgs e)
        {
            DoCMDCondition(() => RemoveSelected(stNodeEditor1.ActiveNode?.Title));
        }
        private void btn_ClearAll_Click(object sender, EventArgs e)
        {
            DoCMDAction(() => { NodeEditor.Nodes.Clear(); });
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------

        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Copy Paste ---------------------------------------------------------------------------------------------
        private bool RemoveSelected(string title = null)
        {
            var selected = stNodeEditor1.GetSelectedNode();
            if (selected.Length > 0)
            {
                //                 if (MessageBox.Show(
                //                     $"删除节点\"{title ?? selected[0].Title}\"\n以及选中节点({selected.Length})?",
                //                     $"删除节点（{selected.Length}）?", buttons: MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (var node in selected)
                    {
                        stNodeEditor1.Nodes.Remove(node);
                    }
                    return true;
                }
            }
            return false;
        }
        private bool CopySelected()
        {
            var nodes = stNodeEditor1.GetSelectedNode();
            if (nodes.Length > 0)
            {
                var selected = new List<STNode>(nodes);
                if (selected.Remove(stNodeEditor1.ActiveNode))
                {
                    selected.Insert(0, stNodeEditor1.ActiveNode);
                }
                var data = NodeEditor.DumpToData(selected);
                if (data != null)
                {
                    return s_copying.Copy(data);
                }
            }
            return false;
        }
        private bool PasteSelected()
        {
            if (s_copying.TryPaste(out EventBehaviorData data))
            {
                //var data = XmlUtil.XmlToObject<EventBehaviorData>(copying);
                data.ReHash();
                var pt = stNodeEditor1.GetMouseCanvasPoint();
                foreach (var nodeData in data.Nodes)
                {
                    nodeData.EditorX += pt.X + 20;
                    nodeData.EditorY += pt.Y + 20;
                }
                var nodes = NodeEditor.LoadNodes(data.Nodes);
                if (nodes != null && nodes.Count > 0)
                {
                    stNodeEditor1.SetSelectedNodes(nodes);
                    stNodeEditor1.SetActiveNode(nodes[0]);
                    return true;
                }
            }
            return false;
        }
        private bool ClipSelected()
        {
            if (CopySelected())
            {
                foreach (var node in stNodeEditor1.GetSelectedNode())
                {
                    stNodeEditor1.Nodes.Remove(node);
                }
                return true;
            }
            return false;
        }

        private static CopyPaste s_copying = new CopyPaste(typeof(BehaviorPanel));
        //private static XmlDocument st_copying;
        private void tool_Copy_Click(object sender, EventArgs e)
        {
            CopySelected();
        }
        private void tool_Paste_Click(object sender, EventArgs e)
        {
            DoCMDCondition(() => { return PasteSelected(); });
        }
        private void tool_Clip_Click(object sender, EventArgs e)
        {
            DoCMDCondition(() =>
            {
                return ClipSelected();
            });
        }
        private void tool_Duplicate_Click(object sender, EventArgs e)
        {
            DoCMDCondition(() =>
            {
                if (CopySelected() && PasteSelected())
                {
                    return true;
                }
                return false;
            });
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------


        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Helper ---------------------------------------------------------------------------------------------
        // 
        //         private STBehaviorNode helpNode;
        //         private Bitmap helpBuffer;
        //         private Rectangle helpBounds;
        public void RefreshStatus(Point? mouseLocation = null)
        {
            var selected = SelectedNode;
            var scale = (int)(NodeEditor.CanvasScale * 100);
            lbl_State.Text = $"缩放:{scale}% : 节点{stNodeEditor1.Nodes.Count}个 : GUID : ";
            if (selected != null)
            {
                lbl_State.Text += $"{selected.Guid} : ({selected.Location})";
            }
            if (mouseLocation != null)
            {
                txt_Mouse.Text = $"Mouse : {stNodeEditor1.ControlToCanvas(mouseLocation.Value)}";
            }
            if (editTreeNode?.CmdQueue != null)
            {
                this.btn_Undo.Enabled = editTreeNode.CmdQueue.CanUndo;
                this.btn_Redo.Enabled = editTreeNode.CmdQueue.CanRedo;
            }
        }
        private void NodeEditor_MouseWheel(object sender, MouseEventArgs e)
        {
            RefreshStatus(e.Location);
        }
        //         private void ResetHelp(STNode st)
        //         {
        //             helpNode = null;
        //             helpBuffer?.Dispose();
        //             helpBuffer = null;
        //             if (st is STBehaviorNode activeNode)
        //             {
        //                 var func = new SelectTreeNode((STNode src, STNodeOption srcP, STNode dst, STNodeOption dstP) =>
        //                 {
        //                     if (srcP.IsInput && dst is STValueNode)
        //                     {
        //                         return true;
        //                     }
        //                     if (src is STActionNode && srcP.IsOutput && dst is STActionNode)
        //                     {
        //                         return true;
        //                     }
        //                     if (src is STTriggerNode && srcP.IsOutput && dst is STActionNode)
        //                     {
        //                         return true;
        //                     }
        //                     return false;
        //                 });
        //                 var list = activeNode.GetTreeNodes(func);
        //                 var data = DumpToData(list);
        //                 var main = data.Nodes[0];
        //                 var asm = new EventBehaviorAssembly(data);
        //                 if (asm.TryGetNode(main.GUID, out var mainNode))
        //                 {
        //                     var font = st.Font;
        //                     var camera = NodeEditor.CameraBounds;
        //                     this.helpBuffer = DeepCore.GUI.Win32.Win32RichTextLayer.CreateAttributeTextBuffer(
        //                         NodeEditor.Width - 20,
        //                         EventStringBuilder.FunctionDocument(mainNode),
        //                         font,
        //                         DeepCore.GUI.Display.Text.RichTextAlignment.taLEFT,
        //                         22,
        //                         Color.Black,
        //                         TextFontStyle.Plain);
        //                     this.helpNode = activeNode;
        //                     var bounds = new Rectangle(new Point(helpNode.Left + helpNode.Width + 10, helpNode.Top), new Size(helpBuffer.Width, helpBuffer.Height));
        //                     if (bounds.Right >= camera.Right)
        //                     {
        //                         bounds.X = Math.Max(helpNode.Left - helpBuffer.Width - 10, camera.Left);
        //                     }
        //                     if (bounds.Bottom >= camera.Bottom)
        //                     {
        //                         bounds.Y = Math.Max(helpNode.Top - helpBuffer.Height - 10, camera.Top);
        //                     }
        //                     this.helpBounds = bounds;
        //                     this.NodeEditor.Refresh();
        //                 }
        //             }
        //         }

        // 
        //         private void NodeEditor_MouseDown(object sender, MouseEventArgs e)
        //         {
        //             if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
        //             {
        //                 var hit = NodeEditor.GetNodeByMousePoint(e.Location);
        //                 if (hit is STBehaviorNode activeNode)
        //                 {
        //                     var mouse = NodeEditor.GetMouseCanvasPoint();
        //                     if (activeNode.HelpBounds.Contains(mouse))
        //                     {
        //                         ResetHelp(activeNode);
        //                         return;
        //                     }
        //                 }
        //                 ResetHelp(null);
        //             }
        //         }
        //         private void NodeEditor_DrawNodeAfter(object sender, PaintEventArgs e)
        //         {
        //             if (helpBuffer != null && helpNode != null)
        //             {
        //                 var g = e.Graphics;
        //                 g.DrawRectangle(Pens.White, helpNode.HelpBounds);
        //                 g.FillRectangle(new SolidBrush(Color.FromArgb(240, Color.White)), helpBounds);
        //                 g.DrawImage(helpBuffer, helpBounds);
        //                 g.DrawRectangle(new Pen(Color.FromArgb(255, Color.Black)), helpBounds);
        //             }
        //         }
        // 
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        #region UndoRedo ---------------------------------------------------------------------------------------------

        private void SetCMD(UndoRedoManager cmd_queue)
        {
            RefreshStatus();
        }
        private void btn_Redo_Click(object sender, EventArgs e)
        {
            if (editTreeNode?.CmdQueue is UndoRedoManager cmd_queue)
            {
                cmd_queue.Redo();
            }
        }
        private void btn_Undo_Click(object sender, EventArgs e)
        {
            if (editTreeNode?.CmdQueue is UndoRedoManager cmd_queue)
            {
                cmd_queue.Undo();
            }
        }

        private void DoCMDAction(EventBehaviorData undo, Action action)
        {
            if (editTreeNode?.CmdQueue is UndoRedoManager cmd_queue)
            {
                action();
                var redo = NodeEditor.DumpToData();
                cmd_queue.ExecuteAs(
                  exe => { },
                  redo => { NodeEditor.ResetDump(redo); },
                  undo => { NodeEditor.ResetDump(undo); },
                  default(EventBehaviorData), redo, undo
              );
            }
        }
        private void DoCMDAction(Action action)
        {
            if (editTreeNode?.CmdQueue is UndoRedoManager cmd_queue)
            {
                var undo = NodeEditor.DumpToData();
                action();
                var redo = NodeEditor.DumpToData();
                cmd_queue.ExecuteAs(
                  exe => { },
                  redo => { NodeEditor.ResetDump(redo); },
                  undo => { NodeEditor.ResetDump(undo); },
                  default(EventBehaviorData), redo, undo
              );
            }
        }
        private R DoCMDFunc<R>(Func<R> action)
        {
            if (editTreeNode?.CmdQueue is UndoRedoManager cmd_queue)
            {
                var undo = NodeEditor.DumpToData();
                var ret = action();
                var redo = NodeEditor.DumpToData();
                cmd_queue.ExecuteAs(
                  exe => { },
                  redo => { NodeEditor.ResetDump(redo); },
                  undo => { NodeEditor.ResetDump(undo); },
                  default(EventBehaviorData), redo, undo
                );
                return ret;
            }
            return default;
        }
        private bool DoCMDCondition(Func<bool> action)
        {
            if (editTreeNode?.CmdQueue is UndoRedoManager cmd_queue)
            {
                var undo = NodeEditor.DumpToData();
                var ret = action();
                if (ret)
                {
                    var redo = NodeEditor.DumpToData();
                    cmd_queue.ExecuteAs(
                      exe => { },
                      redo => { NodeEditor.ResetDump(redo); },
                      undo => { NodeEditor.ResetDump(undo); },
                      default(EventBehaviorData), redo, undo
                    );
                }
                return ret;
            }
            return default;
        }
        private void Undo()
        {
            if (editTreeNode?.CmdQueue is UndoRedoManager cmd_queue)
            {
                cmd_queue.Undo();
            }
        }
        private void Redo()
        {
            if (editTreeNode?.CmdQueue is UndoRedoManager cmd_queue)
            {
                cmd_queue.Redo();
            }
        }

        //         private void Cmd_queue_OnListChanged()
        //         {
        //             RefreshStatus();
        //         }
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        #region DragDrop
        private void TreeView_MouseDown(object sender, MouseEventArgs e)
        {
            var node = TypesTreeView.TreeView.GetNodeAt(e.Location);
            if (node is TypeNode tnode)
            {
                DataObject d = new DataObject("STBehaviorNodeType", tnode.ValueType);
                this.DoDragDrop(d, DragDropEffects.Copy);
            }
        }
        private void NodeEditor_DragEnter(object sender, DragEventArgs e)
        {
            if (editEventNode == null)
                e.Effect = DragDropEffects.None;
            else if (e.Data.GetDataPresent("STBehaviorNodeType"))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void NodeEditor_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("STBehaviorNodeType"))
            {
                var dataType = e.Data.GetData("STBehaviorNodeType") as Type;
                if (dataType == null) return;
                if (!dataType.IsSubclassOf(typeof(EventExternalizable))) return;
                DoCMDCondition(() =>
                {
                    try
                    {
                        var data = (EventExternalizable)DeepActivator.CreateInstance(dataType);
                        NodeEditor.AddNode(data, NodeEditor.PointToClient(new Point(e.X, e.Y)), !Keyboard.IsCtrlDown);
                        return true;
                    }
                    catch (Exception err)
                    {
                        err.ShowMessageBox(this);
                        return false;
                    }
                });
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Control ---------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------------------------------------
        private void btn_ZoomIn_Click(object sender, EventArgs e)
        {
            NodeEditor.ZoomIn();
        }
        private void btn_ZoomOut_Click(object sender, EventArgs e)
        {
            NodeEditor.ZoomOut();
        }
        private void btn_Zoom1_Click(object sender, EventArgs e)
        {
            NodeEditor.Zoom(1f);
        }
        private void btn_GetCanvasImage_Click(object sender, EventArgs e)
        {
            try
            {
                var bitmap = NodeEditor.GetCanvasImage();
                if (bitmap != null)
                {
                    Clipboard.SetImage(bitmap);
                    NodeEditor.ShowAlert("图片已复制到剪贴板", Color.Yellow, Color.DarkGray);
                }
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
        }
        private void nodeMenu_Opening(object sender, CancelEventArgs e)
        {
            var selected = SelectedNode;
            tool_Paste.Enabled = s_copying.HasData;
            tool_Copy.Enabled = (selected != null);
            tool_Clip.Enabled = (selected != null);
            tool_Duplicate.Enabled = (selected != null);
            tool_Remove.Enabled = (selected != null);
        }
        private void stNodeEditor1_KeyDown(object sender, KeyEventArgs e)
        {
            NodeEditorOnHotKey(sender, e);
        }
        private void stNodeEditor1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            NodeEditorOnHotKey(sender, e);
        }
        private void NodeEditorOnHotKey(object sender, EventArgs args)
        {
            {
                if (args is PreviewKeyDownEventArgs e)
                {
                    if (e.Control)
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.A:
                            case Keys.C:
                            case Keys.V:
                            case Keys.X:
                            case Keys.D:
                            case Keys.Z:
                            case Keys.Y:
                            case Keys.F:
                                e.IsInputKey = true;
                                break;
                        }
                    }
                    switch (e.KeyCode)
                    {
                        case Keys.Delete:
                            e.IsInputKey = true;
                            break;
                    }
                }
            }
            {
                if (args is KeyEventArgs e)
                {
                    if (e.Control)
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.A: NodeEditor.SelectAll(); break;
                            case Keys.C: tool_Copy_Click(sender, e); break;
                            case Keys.V: tool_Paste_Click(sender, e); break;
                            case Keys.X: tool_Clip_Click(sender, e); break;
                            case Keys.D: tool_Duplicate_Click(sender, e); break;
                            case Keys.Z: Undo(); break;
                            case Keys.Y: Redo(); break;
                            case Keys.F: Search(); break;
                        }
                    }
                    switch (e.KeyCode)
                    {
                        case Keys.Delete: tool_Remove_Click(sender, e); break;
                    }
                }

            }
        }

        private void chk_Grid_CheckedChanged(object sender, EventArgs e)
        {
            NodeEditor.GridToSize = chk_Grid.Checked;
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            Search();
        }

        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
