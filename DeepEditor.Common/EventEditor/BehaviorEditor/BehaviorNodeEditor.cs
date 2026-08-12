using DeepCore.EventTrigger;
using DeepCore.EventTrigger.Data;
using DeepCore.GUI.Data;
using DeepCore.Xml;
using ST.Library.UI.NodeEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DeepEditor.Common.EventEditor.BehaviorEditor
{
    public class BehaviorNodeEditor : ST.Library.UI.NodeEditor.STNodeEditor
    {
        public Image icon_var { get; set; }
        public Image icon_trigger { get; set; }
        public Image icon_condition { get; set; }
        public Image icon_action { get; set; }
        public Image icon_value { get; set; }
        public Image icon_question { get; set; }
        public STBehaviorNode SelectedNode { get => this.ActiveNode as STBehaviorNode; }

        public BehaviorNodeEditor()
        {
            this.DrawNodeAfter += NodeEditor_DrawNodeAfter;
            this.MouseDown += NodeEditor_MouseDown;
        }


        public void SelectAll()
        {
            this.SetSelectedNodes(this.Nodes.ToArray());
        }
        public List<STNode> SelectTree(STNode active)
        {
            var func = new SelectTreeNode((STNode src, STNodeOption srcP, STNode dst, STNodeOption dstP) =>
            {
                if (srcP.IsInput && dst is STValueNode) { return true; }
                if (src is STActionNode && srcP.IsOutput && dst is STActionNode) { return true; }
                if (src is STTriggerNode && srcP.IsOutput && dst is STActionNode) { return true; }
                return false;
            });
            return this.SelectTree(active, true, func);
        }
        public void AutoLayoutInputs(STNode active, bool select = true)
        {
            var func1 = new SelectTreeNode((STNode src, STNodeOption srcP, STNode dst, STNodeOption dstP) =>
            {
                if (srcP.IsInput && dst is STValueNode) { return true; }
                return false;
            });
            var list1 = this.AutoLayout(active, func1);
            if (select) { this.SelectTree(active, true, func1); }
        }
        public void AutoLayoutTree(STNode active, bool select = true)
        {
            var func1 = new SelectTreeNode((STNode src, STNodeOption srcP, STNode dst, STNodeOption dstP) =>
            {
                if (srcP.IsInput && dst is STValueNode) { return true; }
                if (src is STActionNode && srcP.IsOutput && dst is STActionNode) { return true; }
                if (src is STTriggerNode && srcP.IsOutput && dst is STActionNode) { return true; }
                return false;
            });
            var list1 = this.AutoLayout(active, func1);
            //NodeEditor.ExpandNodes(list1);
            if (select) { this.SelectTree(active, true, func1); }
        }


        public bool ConvertFromBehavior(IEventDataNode node)
        {
            ResetHelp(null);
            if (node != null)
            {
                try
                {
                    var converter = new EventBehaviorDataConveter();
                    var behaviorNodes = converter.ConvertTo(node);
                    var nodes = LoadNodes(behaviorNodes.Nodes);
                    if (nodes.Count > 0)
                    {
                        var main = nodes[0];
                        this.ClearSelected();
                        foreach (var n in nodes)
                        {
                            if (n is STLocalVarNode var)
                            {
                                AutoLayoutTree(var, false);
                            }
                            else if (n is STTriggerNode trigger)
                            {
                                main = trigger;
                                AutoLayoutTree(trigger, false);
                            }
                        }
                        this.ClearSelected();
                        SelectTree(main);
                        return true;
                    }
                }
                catch (Exception err)
                {
                    err.ShowMessageBox(this);
                }
            }
            return false;
        }

        private STBehaviorNode LoadFromBehavior(EventExternalizable data, List<LinkOption> inputs, List<LinkOption> outputs, Point? location, bool single)
        {
            ResetHelp(null);
            if (data != null)
            {
                try
                {
                    if (single)
                    {
                        var stnode = STBehaviorLayout.CreateNode(data, inputs, outputs);
                        LoadNode(stnode, location);
                        if (location != null)
                        {
                            var pt = this.ControlToCanvas(location.Value);
                            stnode.Left = pt.X;
                            stnode.Top = pt.Y;
                        }
                        ;
                        return stnode;
                    }
                    else
                    {
                        var converter = new EventBehaviorDataConveter();
                        if (location != null)
                        {
                            var pt = this.ControlToCanvas(location.Value);
                            converter.StartX = pt.X;
                            converter.StartY = pt.Y;
                        }
                        ;
                        var behaviorNodes = converter.ConvertTo(data, out var main);
                        var nodes = LoadNodes(behaviorNodes.Nodes);
                        AutoLayoutTree(nodes[0], false);
                        return nodes[0];
                    }
                }
                catch (Exception err)
                {
                    err.ShowMessageBox(this);
                }
            }
            return null;
        }
        public void LoadEventBehavior(IEventDataNode node)
        {
            Nodes.Clear();
            LoadNodes(node.EventBehavior.Nodes);
        }
        public void Clear()
        {
            Nodes.Clear();
        }
        public List<STBehaviorNode> LoadNodes(ICollection<EventBehaviorNode> nodes, bool select = false)
        {
            var ret = new List<STBehaviorNode>();
            foreach (var bnode in nodes)
            {
                if (bnode?.EventData != null)
                {
                    var node = STBehaviorLayout.CreateNode(bnode);
                    LoadNode(node, null);
                    ret.Add(node);
                }
            }
            // after build relations
            foreach (STBehaviorNode stnode in ret)
            {
                stnode.Load(this, select);
            }
            RefreshDocking();
            return ret;
        }
        public void LoadNode(STBehaviorNode node, Point? location)
        {
            if (node != null)
            {
                if (TryGetNodeByGUID(node.Guid.ToString(), out var exist))
                {
                    node.NewGuid();
                }
                Point pt = new Point(20, 20);
                if (location.HasValue)
                {
                    pt = location.Value;
                }
                //pt = stNodeEditor1.PointToClient(pt);
                pt = this.ControlToCanvas(pt);
                node.Left = pt.X; node.Top = pt.Y;
                if (node.NodeData.EventData is EventLocalVar)
                {
                    node.Icon = icon_var;
                }
                else if (node.NodeData.EventData is AbstractTrigger)
                {
                    node.Icon = icon_trigger;
                }
                else if (node.NodeData.EventData is AbstractCondition)
                {
                    node.Icon = icon_condition;
                }
                else if (node.NodeData.EventData is AbstractAction)
                {
                    node.Icon = icon_action;
                }
                else if (node.NodeData.EventData is AbstractValue)
                {
                    node.Icon = icon_value;
                }
                node.HelpIcon = this.icon_question;
                this.Nodes.Add(node);
            }
        }

        public bool TryGetNodeByGUID(string guid, out STBehaviorNode node)
        {
            foreach (STBehaviorNode stnode in this.Nodes)
            {
                if (stnode.Guid.ToString().Equals(guid))
                {
                    node = stnode;
                    return true;
                }
            }
            node = null;
            return false;
        }
        public bool TryGetNodeFieldByGUID(string guid, string fieldName, LinkDock fieldDock, out STBehaviorNode node, out STBehaviorOption option)
        {
            foreach (STBehaviorNode stnode in this.Nodes)
            {
                if (stnode.Guid.ToString().Equals(guid))
                {
                    node = stnode;
                    if (stnode.TryGetNodeFieldOption(fieldDock, fieldName, out option))
                    {
                        return true;
                    }
                }
            }
            option = null;
            node = null;
            return false;
        }
        public STBehaviorNode AddNode(EventExternalizable nodeData, Point? location, bool single = false)
        {
            var node = LoadFromBehavior(nodeData, null, null, location, single);
            //var node = STBehaviorLayout.CreateNode(nodeData);
            this.ClearSelected();
            SelectTree(node);
            return node;
        }
        public STBehaviorNode AddNode(EventExternalizable nodeData, List<LinkOption> inputs, List<LinkOption> outputs, Point? location, bool single = false)
        {
            var node = LoadFromBehavior(nodeData, inputs, outputs, location, single);
            //var node = STBehaviorLayout.CreateNode(nodeData);
            this.ClearSelected();
            SelectTree(node);
            return node;
        }
        public STBehaviorNode AddGroup(string text, Point? location)
        {
            var node = LoadFromBehavior(new BehaviorGroup() { Title = text }, null, null, location, true) as STGroupNode;
            //var node = STBehaviorLayout.CreateNode(nodeData);
            this.ClearSelected();
            SelectTree(node);
            return node;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        public EventBehaviorData DumpToData()
        {
            var list = new List<EventBehaviorNode>();
            foreach (STBehaviorNode node in this.Nodes)
            {
                node.Save(this, true);
                var cnode = XmlUtil.CloneObject(node.NodeData);
                list.Add(cnode);
            }
            var data = new EventBehaviorData() { Nodes = list };
            return data;
        }
        public EventBehaviorData DumpToData(List<STNode> selected)
        {
            var list = new List<EventBehaviorNode>();
            var pt = this.GetMouseCanvasPoint();
            foreach (STBehaviorNode node in selected)
            {
                node.Save(this);
                var cnode = XmlUtil.CloneObject(node.NodeData);
                cnode.ForEachOptions(0, (st, link, dock) =>
                {
                    if (!selected.Exists(st => link.NextGUID == st.GUID))
                    {
                        if (dock == LinkDock.Input)
                        {
                            cnode.Inputs.Remove(link);
                        }
                        else if (dock == LinkDock.Output)
                        {
                            cnode.Outputs.Remove(link);
                        }
                    }
                });
                cnode.EditorX = node.Left - pt.X;
                cnode.EditorY = node.Top - pt.Y;
                list.Add(cnode);
            }
            var data = new EventBehaviorData() { Nodes = list };
            data.ReHash();
            return data;
        }
        public void ResetDump(EventBehaviorData dump)
        {
            var mtx = this.SaveCanvasTransform();
            this.SuspendLayout();
            try
            {
                ResetHelp(null);
                this.Nodes.Clear();
                this.LoadNodes(dump.Nodes, true);
            }
            finally
            {
                this.ResumeLayout();
                this.SetCanvasTransform(mtx);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Helper ---------------------------------------------------------------------------------------------

        private STBehaviorNode helpNode;
        private Bitmap helpBuffer;
        private Rectangle helpBounds;
        public void ResetHelp(STNode st)
        {
            helpNode = null;
            helpBuffer?.Dispose();
            helpBuffer = null;
            if (st is STBehaviorNode activeNode)
            {
                var func = new SelectTreeNode((STNode src, STNodeOption srcP, STNode dst, STNodeOption dstP) =>
                {
                    if (srcP.IsInput && dst is STValueNode)
                    {
                        return true;
                    }
                    if (src is STActionNode && srcP.IsOutput && dst is STActionNode)
                    {
                        return true;
                    }
                    if (src is STActionNode && srcP.IsOutput && dst is STTriggerNode)
                    {
                        return true;
                    }
                    if (src is STTriggerNode && srcP.IsOutput && dst is STActionNode)
                    {
                        return true;
                    }
                    if (src is STTriggerNode && srcP.IsOutput && dst is STTriggerNode)
                    {
                        return true;
                    }
                    return false;
                });
                var list = activeNode.GetTreeNodes(func);
                var data = DumpToData(list);
                var main = data.Nodes[0];
                var asm = new EventBehaviorAssembly().Init(data);
                if (asm.TryGetNode(main.GUID, out var mainNode))
                {
                    var font = st.Font;
                    var camera = this.CameraBounds;
                    this.helpBuffer = DeepCore.GUI.Win32.Win32RichTextLayer.CreateAttributeTextBuffer(
                        this.Width - 20,
                        EventStringBuilder.FunctionDocument(mainNode),
                        font,
                        DeepCore.GUI.Display.Text.RichTextAlignment.taLEFT,
                        22,
                        Color.Black,
                        TextFontStyle.Plain);
                    this.helpNode = activeNode;
                    var bounds = new Rectangle(new Point(helpNode.Left + helpNode.Width + 10, helpNode.Top), new Size(helpBuffer.Width, helpBuffer.Height));
                    if (bounds.Right >= camera.Right)
                    {
                        bounds.X = Math.Max(helpNode.Left - helpBuffer.Width - 10, camera.Left);
                    }
                    if (bounds.Bottom >= camera.Bottom)
                    {
                        bounds.Y = Math.Max(helpNode.Top - helpBuffer.Height - 10, camera.Top);
                    }
                    this.helpBounds = bounds;
                    this.Invalidate();
                }
            }
        }

        private void NodeEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                var hit = this.GetNodeByMousePoint(e.Location);
                if (hit is STBehaviorNode activeNode)
                {
                    var mouse = this.GetMouseCanvasPoint();
                    if (activeNode.HelpBounds.Contains(mouse))
                    {
                        ResetHelp(activeNode);
                        return;
                    }
                }
                ResetHelp(null);
            }
        }
        private void NodeEditor_DrawNodeAfter(object sender, PaintEventArgs e)
        {
            if (helpBuffer != null && helpNode != null)
            {
                var g = e.Graphics;
                g.DrawRectangle(Pens.White, helpNode.HelpBounds);
                g.FillRectangle(new SolidBrush(Color.FromArgb(240, Color.White)), helpBounds);
                g.DrawImage(helpBuffer, helpBounds);
                g.DrawRectangle(new Pen(Color.FromArgb(255, Color.Black)), helpBounds);
            }
        }

        #endregion


    }
}
