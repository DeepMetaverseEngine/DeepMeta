using DeepCore;
using DeepCore.Event.Debug;
using DeepCore.GUI.Win32;
using DeepCore.IO;
using DeepEditor.Common.EventEditor.BehaviorEditor;
using DeepEditor.Common.G2D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace DeepEditor.Common.EventDebug
{
    public partial class EventDebugForm : G2DForm
    {
        public BehaviorNodeEditor NodeEditor { get => this.behaviorNodeEditor1; }
        public ImageList NodeImageList { get => this.imageList1; }
        public G2DTreeView TreeView { get => treeView1; }
        public EventDebugSlave DebugSlave { get; private set; }
        private bool request_repaint = true;
        public EventDebugForm(IExternalizableFactory codec)
        {
            InitializeComponent();
            this.NodeEditor.IsReadOnly = true;
            this.NodeEditor.BackColor = Color.FromArgb(255, 64, 64, 64);
            this.DebugSlave = new EventDebugSlave(codec);
            this.DebugSlave.OnInit += DebugSlave_OnInit;
            this.DebugSlave.OnAddCollection += DebugSlave_OnAddCollection;
            this.DebugSlave.OnRemoveCollection += DebugSlave_OnRemoveCollection;
            this.DebugSlave.OnExecutorChanged += DebugSlave_OnExecutorChanged;
            this.DebugSlave.OnBeginTrace += DebugSlave_OnBeginTrace;
            this.DebugSlave.OnTrace += DebugSlave_OnTrace;
            this.treeView1.HideSelection = false;
            this.treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.treeView1.AfterSelect += TreeView1_AfterSelect;
            this.treeView1.DrawNode += TreeView1_DrawNode;
            this.FormClosed += EventDebugForm_FormClosed;
            this.Disposed += EventDebugForm_Disposed;
        }
        private void EventDebugForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DebugSlave?.Stop();
        }
        private void EventDebugForm_Disposed(object sender, EventArgs e)
        {
            this.DebugSlave?.Dispose();
            this.DebugSlave = null;
        }
        public void Start(string hostAddress)
        {
            this.DebugSlave.Start(hostAddress);
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected virtual void EventDebugForm_Load(object sender, EventArgs e)
        {
            this.treeView1.ImageList = this.NodeImageList;
            NodeEditor.icon_question = this.NodeImageList.Images["Question.png"];
            NodeEditor.icon_var = this.NodeImageList.Images["icon_var.png"];
            NodeEditor.icon_trigger = this.NodeImageList.Images["icon_trigger.png"];
            NodeEditor.icon_condition = this.NodeImageList.Images["icon_condition.png"];
            NodeEditor.icon_action = this.NodeImageList.Images["icon_run.png"];
            NodeEditor.icon_value = this.NodeImageList.Images["icon_value.png"];
        }
        protected virtual void timer1_Tick(object sender, EventArgs e)
        {
            this.DebugSlave?.Update();
            if (request_repaint)
            {
                request_repaint = false;
                this.NodeEditor.Invalidate();
                this.TreeView.Invalidate();
            }
        }
        protected virtual void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetCurrentExe(e.Node as TreeNodeEventExecutor);
        }
        protected virtual void TreeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = true;
            if (e.Node.IsVisible)
            {
                //var format = Win32Driver.DefaultFormat;
                //             if (e.Node.IsSelected)
                //             {
                //                 e.Graphics.FillRectangle(Brushes.DarkBlue, region);
                //             }
                //             e.Graphics.DrawString(e.Node.Name, treeView1.Font, SkinManager.TextHighEmphasisBrush, region, format);
                if (e.Node is TreeNodeEventExecutor exe && exe.TracingCount > 0)
                {
                    draw_tracing(e, exe.TracingCount);
                }
                //                 else if (e.Node is TreeNodeEventCollection group && group.TracingCount > 0)
                //                 {
                //                     draw_tracing(e, group.TracingCount);
                //                 }
            }
            static void draw_tracing(DrawTreeNodeEventArgs e, int tcount)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var region = e.Bounds;
                var font = e.Node.TreeView.Font;
                var pos = new PointF(region.X + region.Width + 1, region.Y);
                var count = $"{tcount}";
                var csize = e.Graphics.MeasureBoundsString(count, font);
                csize.Width += 5;
                var rect = new RectangleF(pos, csize);
                e.Graphics.FillPie(Brushes.Red, rect, 0, 360);
                e.Graphics.DrawStringAlignment(count, font,
                    Brushes.White,
                    DeepCore.GUI.Data.AlignmentStyle.MiddleCenter,
                    rect);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected virtual void DebugSlave_OnInit(EventDebugSlave arg1, EventRuntimeState arg2)
        {
            this.treeView1.Nodes.Clear();
            if (arg2?.Collections != null)
            {
                foreach (var evts in arg2.Collections)
                {
                    AddTreeNode(evts);
                }
            }
            foreach (TreeNode tn in treeView1.Nodes)
            {
                tn.Expand();
            }
            // treeView1.ExpandAll();
        }
        protected virtual void DebugSlave_OnAddCollection(EventDebugSlave arg1, AddCollectionNotify arg2)
        {
            var evts = arg2.Add;
            AddTreeNode(evts);
        }
        protected virtual void DebugSlave_OnRemoveCollection(EventDebugSlave arg1, RemoveCollectionNotify arg2)
        {
            RemoveTreeNode(arg2.GUID);
        }
        private void DebugSlave_OnExecutorChanged(EventDebugSlave arg1, ExecutorChangedNotify arg2)
        {
            ExecutorChanged(arg2);
        }
        private void DebugSlave_OnBeginTrace(EventDebugSlave arg1, EventBeginTraceNotify arg2)
        {
            BeginTrace(arg2);
        }
        protected virtual void DebugSlave_OnTrace(EventDebugSlave arg1, EventTraceData arg2)
        {
            Trace(arg2);
        }

        //----------------------------------------------------------------------------------------------------------------------
        private TreeNodeEventExecutor currentExe;
        private HashMap<string, TreeNodeEventCollection> treeNodes = new();
        public TreeNodeEventExecutor CurrentExecutor { get => currentExe; }

        void RepaintNodes()
        {
            this.request_repaint = true;
        }
        protected void Trace(EventTraceData e)
        {
            if (TryGetBehaviorNode(e.CollectionGUID, e.ExeName, out var events, out var exe))
            {
                exe.AddTracing(e.NodeGUID);
                if (CurrentExecutor == exe)
                {
                    if (NodeEditor.TryGetNodeByGUID(e.NodeGUID, out var node))
                    {
                        node.Highlight = true;
                    }
                }
                this.RepaintNodes();
            }
        }
        protected void BeginTrace(EventBeginTraceNotify e)
        {
            if (TryGetBehaviorNode(e.CollectionGUID, e.ExeName, out var events, out var exe))
            {
                exe.BeginTracing();
                if (CurrentExecutor == exe)
                {
                    foreach (var node in NodeEditor.Nodes)
                    {
                        if (node is STBehaviorNode bnode)
                        {
                            bnode.Highlight = false;
                        }
                    }
                }
                this.RepaintNodes();
            }
        }
        protected void ExecutorChanged(ExecutorChangedNotify e)
        {
            if (TryGetBehaviorNode(e.CollectionGUID, e.ExeData.Name, out var events, out var exe))
            {
                exe.Refresh(e.ExeData);
                if (CurrentExecutor == exe)
                {
                    foreach (var node in NodeEditor.Nodes)
                    {
                        if (node is STBehaviorNode bnode)
                        {
                            bnode.Highlight = exe.IsTracing(bnode.GUID);
                        }
                    }
                }
                this.RepaintNodes();
            }
        }
        protected void SetCurrentExe(TreeNodeEventExecutor executor)
        {
            this.currentExe = executor;
            if (executor != null)
            {
                NodeEditor.LoadEventBehavior(executor.Data.EventData);
                foreach (var node in NodeEditor.Nodes)
                {
                    if (node is STBehaviorNode bnode)
                    {
                        bnode.Highlight = executor.IsTracing(bnode.GUID);
                    }
                }
                this.RepaintNodes();
            }
            else
            {
                NodeEditor.Clear();
            }
        }
        protected virtual TreeNodeEventCollection AddTreeNode(EventCollectionData evts)
        {
            var tn = G2DTreeNodes.GetOrCreateNodeWithPath(treeView1.Nodes,
                 (tail, p) => new TreeNodeEventCollection(tail, evts),
                 (dir, p) => new TreeNodeEventDir(dir),
                 evts.Name.Split('/')) as TreeNodeEventCollection;
            treeNodes.Add(evts.GUID, tn);
            if (evts.Events != null)
            {
                foreach (var sub in evts.Events)
                {
                    var subtn = new TreeNodeEventExecutor(tn, sub);
                    tn.Nodes.Add(subtn);
                }
                //                tn.Expand();
                //                 if (tn.Parent != null)
                //                 {
                //                     tn.Parent.Expand();
                //                 }
            }
            var gname = tn.Parent?.Parent?.Name;
            if (gname != null)
            {
                var group = tn.Parent;
                if (gname == "Zone")
                {
                    group.ImageKey = group.SelectedImageKey = "icon_scene.ico";
                }
                else if (gname == "GUI")
                {
                    group.ImageKey = group.SelectedImageKey = "icon_common_67.png";
                }
                else if (gname == "Unit")
                {
                    group.ImageKey = group.SelectedImageKey = "icon_hd.png";
                }
            }
            return tn;
        }
        protected virtual TreeNodeEventCollection RemoveTreeNode(string guid)
        {
            if (treeNodes.TryRemove(guid, out var tn))
            {
                if (CurrentExecutor?.Parent == tn)
                {
                    NodeEditor.Clear();
                }
                var parent = tn.Parent;
                tn.Remove();
                while (parent != null && parent.Nodes.Count == 0)
                {
                    var n = parent;
                    parent = parent.Parent;
                    n.Remove();
                    if (parent is TreeNodeEventDir dir && dir.Parent == null)
                    {
                        break;
                    }

                }
            }
            return tn;
        }
        public bool TryGetBehaviorNode(string eventsGUID, string exeName, out TreeNodeEventCollection events, out TreeNodeEventExecutor exe)
        {
            if (treeNodes.TryGetValue(eventsGUID, out events))
            {
                if (events.Nodes[exeName] is TreeNodeEventExecutor treeNodeEventExecutor)
                {
                    exe = treeNodeEventExecutor;
                    return true;
                }
            }
            exe = null;
            return false;
        }
        //----------------------------------------------------------------------------------------------------------------------

        public class TreeNodeEventDir : TreeNode
        {
            public TreeNodeEventDir(string dir) : base(dir)
            {
                this.Name = dir;
                this.ImageKey = SelectedImageKey = "icons_tool_bar2.png";
            }
        }
        public class TreeNodeEventCollection : TreeNode
        {
            //public int TracingCount { get; private set; }
            public EventCollectionData Data { get; }
            public TreeNodeEventCollection(string tail, EventCollectionData events) : base(tail)
            {
                this.Name = events.GUID;
                this.ImageKey = SelectedImageKey = "icons_tool_bar2.png";
                this.Data = events;
            }
            //             public void Refresh()
            //             {
            //                 TracingCount = 0;
            //                 foreach (var sub in Nodes)
            //                 {
            //                     if (sub is TreeNodeEventExecutor exe)
            //                     {
            //                         TracingCount += exe.TracingCount;
            //                     }
            //                 }
            //             }
        }
        public class TreeNodeEventExecutor : TreeNode
        {
            private HashSet<string> tracing = new HashSet<string>();
            public EventExecutorData Data { get; private set; }
            public TreeNodeEventCollection Group { get; }
            public int TracingCount => tracing.Count;
            public TreeNodeEventExecutor(TreeNodeEventCollection group, EventExecutorData exe) : base(exe.Name)
            {
                this.Group = group;
                this.Name = exe.Name;
                this.ImageKey = SelectedImageKey = "event_2558944.png";
                Refresh(exe);
            }
            public void Refresh(EventExecutorData exe)
            {
                this.Data = exe;
                this.ForeColor = exe.IsActive ? GlobalSkinManager.TextHighEmphasisColor : GlobalSkinManager.TextDisabledColor;
                this.tracing.Clear();
                if (exe.TracingNodes != null)
                {
                    this.tracing.AddRange(exe.TracingNodes);
                }
                //this.Group.Refresh();
            }
            public void BeginTracing()
            {
                this.tracing.Clear();
                //this.Group.Refresh();
                //this.TreeView?.Invalidate();
            }
            public void AddTracing(string nodeGUID)
            {
                this.tracing.Add(nodeGUID);
                //this.Group.Refresh();
                //this.TreeView?.Invalidate();
            }
            public bool IsTracing(string nodeGUID)
            {
                return this.tracing.Contains(nodeGUID);
            }
        }
        private void btn_AlwaysTop_Click(object sender, EventArgs e)
        {
            this.TopMost = btn_AlwaysTop.Checked;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
