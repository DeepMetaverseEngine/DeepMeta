using DeepEditor.Common.Windows;
using MaterialSkin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static DeepEditor.Common.G2D.G2DTreeViewControl;
using static System.Net.Mime.MediaTypeNames;

namespace DeepEditor.Common.G2D
{
    public partial class G2DTreeViewControl : UserControl, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }

        [Browsable(true)] public G2DTreeView TreeView { get => treeView; }
        [Browsable(true)] public G2DBaseToolStrip ToolStrip { get => toolStrip; }
        [Browsable(true)] public ImageList ImageList { get => this.TreeView.ImageList; set => this.TreeView.ImageList = value; }
        [Browsable(true)] public TreeViewDrawMode DrawMode { get => this.TreeView.DrawMode; set => this.TreeView.DrawMode = value; }
        [Browsable(true)] public bool EnableCopyPaste { get => this.TreeView.EnableCopyPaste; set => this.TreeView.EnableCopyPaste = value; }
        [Browsable(true)] public bool FullRowSelect { get => this.TreeView.FullRowSelect; set => this.TreeView.FullRowSelect = value; }
        [Browsable(true)] public bool HideSelection { get => this.TreeView.HideSelection; set => this.TreeView.HideSelection = value; }
        [Browsable(true)] public bool ShowNodeToolTips { get => this.TreeView.ShowNodeToolTips; set => this.TreeView.ShowNodeToolTips = value; }
        [Browsable(true)] public string ImageKey { get => this.TreeView.ImageKey; set => this.TreeView.ImageKey = value; }
        [Browsable(true)] public string SelectedImageKey { get => this.TreeView.SelectedImageKey; set => this.TreeView.SelectedImageKey = value; }
        [Browsable(true)] public Color LineColor { get => this.TreeView.LineColor; set => this.TreeView.LineColor = value; }
        [Browsable(true)] public bool CheckBoxes { get => this.TreeView.CheckBoxes; set { this.TreeView.CheckBoxes = value; this.chk_CheckON.Checked = value; } }
        [Browsable(true)] public int ItemHeight { get => this.TreeView.ItemHeight; set { this.TreeView.ItemHeight = value; } }



        [Browsable(false)] public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        [Browsable(false)] public TreeNodeCollection Nodes => this.TreeView.Nodes;
        [Browsable(false)] public TreeNode SelectedNode { get => this.TreeView.SelectedNode; set => this.TreeView.SelectedNode = value; }
        [Browsable(false)] public IComparer TreeViewNodeSorter { get => this.TreeView.TreeViewNodeSorter; set => this.TreeView.TreeViewNodeSorter = value; }


        [Browsable(true)] public event TreeViewEventHandler AfterSelect { add => this.TreeView.AfterSelect += value; remove => this.TreeView.AfterSelect -= value; }

        [Browsable(true)] public event TreeViewEventHandler SelectionChanged;

        public G2DBaseToolStripButton CheckOnButton { get => chk_CheckON; }

        public G2DTreeViewControl()
        {
            InitializeComponent();
            this.btn_RefreshTree.Click += this.RefreshTree_Click;
            this.btn_ExpandALL.Click += this.Expand_Click;
            this.btn_CollapseAll.Click += this.Collapse_Click;
            this.chk_CheckON.CheckedChanged += this.CheckON_CheckedChanged;

            this.treeView.AfterCheck += this.TreeViewOnAfterCheck;
            this.treeView.ItemDrag += this.TreeViewOnItemDrag;
            this.treeView.DragEnter += this.TreeViewOnDragEnter;
            this.treeView.DragOver += this.TreeViewOnDragOver;
            this.treeView.DragDrop += this.TreeViewOnDragDrop;
            this.treeView.DragLeave += this.TreeViewOnDragLeave;
            this.treeView.QueryContinueDrag += TreeView_QueryContinueDrag;
            this.treeView.GiveFeedback += TreeView_GiveFeedback;

            this.treeView.MouseDown += TreeView_MouseDown;
            this.treeView.AfterSelect += TreeView_AfterSelect;

            this.toolStrip.Resize += ToolStrip_Resize;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ResetSearch();
        }


        private void ToolStrip_Resize(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void ResetSearch()
        {
            var size = this.txtFilter.Size;
            {
                var startX = toolStripSeparatorLeft.Bounds.X + toolStripSeparatorLeft.Width;
                var endX = toolStrip.Width - btn_Find.Width - toolStripSeparatorLeft.Width;
                size.Width = endX - startX - 6;
            }
            this.txtFilter.Size = size;
        }

        public void Sort()
        {
            TreeView.Sort();
        }


        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }
        private void TreeView_MouseDown(object sender, MouseEventArgs e)
        {
            var node = this.treeView.GetNodeAt(e.Location);
            if (node != treeView.SelectedNode)
            {
                treeView.SelectedNode = node;
                //SelectionChanged?.Invoke(sender, new TreeViewEventArgs(node));
            }
        }

        protected virtual void RefreshTree_Click(object sender, EventArgs e)
        {
            treeView.Sort();
            this.treeView.RefreshAllNode();
        }
        protected virtual void Collapse_Click(object sender, EventArgs e)
        {
            this.treeView.CollapseAll();
            this.treeView.TopNode.Expand();
            this.treeView.RefreshAllNode();
        }
        protected virtual void Expand_Click(object sender, EventArgs e)
        {
            this.treeView.ExpandAll();
            this.treeView.RefreshAllNode();
        }
        protected virtual void CheckON_CheckedChanged(object sender, EventArgs e)
        {
            treeView.CheckBoxes = chk_CheckON.Checked;
        }
        //         protected override void OnBeforeCheck(TreeViewCancelEventArgs e)
        //         {
        //             if (EnableDragDrop)
        //             {
        //                 if (DraggingGroupType.IsInstanceOfType(e.Node))
        //                 {
        //                     e.Cancel = true;
        //                     return;
        //                 }
        //             }
        //             base.OnBeforeCheck(e);
        //         }

        //------------------------------------------------------------------------------------------------------

        #region DragDrop

        private StartDragTreeNodeEvent startDragEvent;
        [Browsable(true)] public bool EnableDragDrop { get; set; } = false;
        [Browsable(false)] public Type DraggingNodeType { get; private set; } = typeof(G2DTreeNode);
        [Browsable(false)] public Type DraggingGroupType { get; private set; } = typeof(G2DTreeNodeGroup);

        private bool EnableDragDropChecked { get => CheckBoxes; }
        public class DragNodeData : IDataObject
        {
            public readonly TreeNode Data;
            public DragNodeData(TreeNode node)
            {
                this.Data = node;
            }
            public object GetData(string format, bool autoConvert) => this.Data;
            public object GetData(string format) => this.Data;
            public object GetData(Type format) => format.IsInstanceOfType(this.Data) ? this.Data : null;
            public bool GetDataPresent(string format, bool autoConvert) => true;
            public bool GetDataPresent(string format) => true;
            public bool GetDataPresent(Type format) => format.IsInstanceOfType(this.Data);
            public string[] GetFormats(bool autoConvert) => ["TreeNode"];
            public string[] GetFormats() => ["TreeNode"];
            public void SetData(string format, bool autoConvert, object data) { }
            public void SetData(string format, object data) { }
            public void SetData(Type format, object data) { }
            public void SetData(object data) { }
        }
        protected virtual void TreeViewOnAfterCheck(object sender, TreeViewEventArgs e)
        {
            if (EnableDragDrop)
            {
                if (DraggingGroupType.IsAssignableFrom(e.Node.GetType()))
                {
                    foreach (var tn in e.Node.GetAllNodes())
                    {
                        if (tn != e.Node)
                        {
                            tn.Checked = e.Node.Checked;
                        }
                    }
                }
            }
        }
        private void TreeView_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

        }

        private void TreeView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move && startDragEvent != null)
            {
                e.UseDefaultCursors = false;
                Cursor.Current = startDragEvent.Cursor;
            }
            else
            {
                e.UseDefaultCursors = true;
            }
        }
        public static bool IsCheckedAlone(TreeNode tn)
        {
            if (tn.Checked)
            {
                var parent = tn.Parent;
                while (parent != null)
                {
                    if (parent.Checked)
                    {
                        return false;
                    }
                    parent = parent.Parent;
                }
                return true;
            }
            return false;
        }
        protected virtual void TreeViewOnItemDrag(object sender, ItemDragEventArgs e)
        {
            if (EnableDragDrop)
            {
                if (e.Item is TreeNode dragging)
                {
                    this.startDragEvent = new StartDragTreeNodeEvent(treeView, dragging)
                    {
                        e = e,
                    };
                    this.startDragEvent.CheckedNodes.Add(dragging);
                    if (EnableDragDropChecked)
                    {
                        foreach (var tn in TreeView.GetAllNodes())
                        {
                            if (IsCheckedAlone(tn) && DraggingNodeType.IsAssignableFrom(tn.GetType()))
                            {
                                startDragEvent.CheckedNodes.Add(tn);
                            }
                        }
                        //DraggingNodeType = e.Item.GetType();
                        TreeView.DoDragDrop(new DragNodeData(dragging), DragDropEffects.Move);
                    }
                }
            }
        }
        protected virtual void TreeViewOnDragEnter(object sender, DragEventArgs e)
        {
            if (EnableDragDrop)
            {
                var pos = TreeView.PointToClient(new Point(e.X, e.Y));
                var dropNode = TreeView.GetNodeAt(pos);
                if (dropNode != null && dropNode.TreeView == this.TreeView)
                {
                    var child_node = (TreeNode)e.Data.GetData(DraggingNodeType);
                    var group_node = (TreeNode)e.Data.GetData(DraggingGroupType);
                    if (DraggingGroupType.IsAssignableFrom(dropNode.GetType()))
                    {
                        if (child_node != null && child_node.TreeView == this.TreeView)
                        {
                            e.Effect = DragDropEffects.Move;
                        }
                        else if (group_node != null && group_node.TreeView == this.TreeView && group_node != dropNode)
                        {
                            if (group_node.ContainsChild(dropNode, true))
                            {
                                e.Effect = DragDropEffects.None;
                            }
                            else
                            {
                                e.Effect = DragDropEffects.Move;
                            }
                        }
                        else
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                    else if (DraggingNodeType.IsAssignableFrom(dropNode.GetType()))
                    {
                        e.Effect = DragDropEffects.None;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
                //                     if (e.Data.GetDataPresent(DraggingNodeType))
                //                 {
                //                     e.Effect = DragDropEffects.Move;
                //                 }
                //                 else if (e.Data.GetDataPresent(DraggingGroupType))
                //                 {
                //                     e.Effect = DragDropEffects.Move;
                //                 }
                //                 else
                //                 {
                //                     e.Effect = DragDropEffects.None;
                //                 }
            }
        }
        protected virtual void TreeViewOnDragOver(object sender, DragEventArgs e)
        {
            if (EnableDragDrop)
            {
                var pos = TreeView.PointToClient(new Point(e.X, e.Y));
                var dropNode = TreeView.GetNodeAt(pos);
                if (dropNode != null && dropNode.TreeView == this.TreeView)
                {
                    var child_node = (TreeNode)e.Data.GetData(DraggingNodeType);
                    var group_node = (TreeNode)e.Data.GetData(DraggingGroupType);
                    if (DraggingGroupType.IsAssignableFrom(dropNode.GetType()))
                    {
                        if (child_node != null && child_node.TreeView == this.TreeView)
                        {
                            e.Effect = DragDropEffects.Move;
                        }
                        else if (group_node != null && group_node.TreeView == this.TreeView && group_node != dropNode)
                        {
                            if (group_node.ContainsChild(dropNode, true))
                            {
                                e.Effect = DragDropEffects.None;
                            }
                            else
                            {
                                e.Effect = DragDropEffects.Move;
                            }
                        }
                        else
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                    else if (DraggingNodeType.IsAssignableFrom(dropNode.GetType()))
                    {
                        e.Effect = DragDropEffects.None;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }
        protected virtual void TreeViewOnDragDrop(object sender, DragEventArgs e)
        {
            this.startDragEvent?.Dispose();
            this.startDragEvent = null;
            if (EnableDragDrop)
            {
                var pos = TreeView.PointToClient(new Point(e.X, e.Y));
                var dropNode = TreeView.GetNodeAt(pos);
                if (dropNode != null && dropNode.TreeView == this.TreeView)
                {
                    var child_node = (TreeNode)e.Data.GetData(DraggingNodeType);
                    var group_node = (TreeNode)e.Data.GetData(DraggingGroupType);
                    if (DraggingGroupType.IsAssignableFrom(dropNode.GetType()))
                    {
                        if (child_node == null && group_node == null)
                        {
                            return;
                        }
                        if (child_node != null && child_node.TreeView != this.TreeView)
                        {
                            return;
                        }
                        if (group_node != null && group_node.TreeView != this.TreeView)
                        {
                            return;
                        }
                        {
                            var dropEvent = new DragDropTreeNodeToGroupEvent()
                            {
                                DraggingNode = child_node ?? group_node,
                                DropNode = dropNode,
                                DragDrop = e,
                            };
                            try
                            {
                                if (EnableDragDropChecked)
                                {
                                    foreach (var tn in TreeView.GetAllNodes())
                                    {
                                        if (tn.Checked && DraggingNodeType.IsAssignableFrom(tn.GetType()))
                                        {
                                            dropEvent.CheckedNodes.Add(tn);
                                        }
                                    }
                                }
                                DragDropTreeNodeToGroup?.Invoke(this, dropEvent);
                                if (!dropEvent.Cancel)
                                {
                                    if (child_node != null)
                                    {
                                        if (EnableDragDropChecked)
                                        {
                                            foreach (var tn in dropEvent.CheckedNodes)
                                            {
                                                tn.Remove();
                                                dropNode.Nodes.Add(tn);
                                            }
                                        }
                                        child_node.Remove();
                                        dropNode.Nodes.Add(child_node);
                                        TreeView.SelectedNode = child_node;
                                    }
                                    else if (group_node != dropNode)
                                    {
                                        if (!group_node.ContainsChild(dropNode, true))
                                        {
                                            group_node.Remove();
                                            dropNode.Nodes.Add(group_node);
                                            TreeView.SelectedNode = group_node;
                                        }
                                    }
                                    DragDropTreeNodeToGroupComplete?.Invoke(this, dropEvent);
                                }
                            }
                            finally
                            {
                                foreach (var tn in TreeView.GetAllNodes())
                                {
                                    if (tn.Checked) tn.Checked = false;
                                }
                            }
                        }
                    }
                    else if (DraggingNodeType.IsAssignableFrom(dropNode.GetType()))
                    {

                    }
                }
                else
                {
                }
            }
        }
        protected virtual void TreeViewOnDragLeave(object sender, EventArgs e)
        {
        }

        public class StartDragTreeNodeEvent : EventArgs, IDisposable
        {
            public readonly TreeView TreeView;
            public readonly HashSet<TreeNode> CheckedNodes = new HashSet<TreeNode>();
            public readonly TreeNode DraggingNode;
            public ItemDragEventArgs e;
            private Bitmap startDragBitmap;
            private Cursor cursor;
            public StartDragTreeNodeEvent(TreeView treeView, TreeNode dragging)
            {
                this.TreeView = treeView;
                this.DraggingNode = dragging;
            }
            public Cursor Cursor
            {
                get
                {
                    if (cursor == null)
                    {
                        cursor = CursorUtil.CreateCursor(DragBitmap, 0, 0);
                    }
                    return cursor;
                }
            }
            public Bitmap DragBitmap
            {
                get
                {
                    if (startDragBitmap == null)
                    {
                        var text = this.ToString();
                        var bounds = TextRenderer.MeasureText(text, TreeView.Font);
                        var bitmap = new Bitmap(bounds.Width + 10, bounds.Height + 10);
                        using (var g = Graphics.FromImage(bitmap))
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(192, Color.Black)),
                                new Rectangle(10, 10, bounds.Width, bounds.Height));
                            g.DrawRectangle(new Pen(Color.WhiteSmoke) { DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot },
                                new Rectangle(10, 10, bounds.Width - 1, bounds.Height - 1));
                            TextRenderer.DrawText(g, text, TreeView.Font, new Point(12, 12), Color.White);
                            Cursors.Hand.Draw(g, new Rectangle(0, 0, 32, 32));
                        }
                        this.startDragBitmap = bitmap;
                    }
                    return startDragBitmap;
                }
            }
            public void Dispose()
            {
                this.cursor?.Dispose();
                this.cursor = null;
                this.startDragBitmap?.Dispose();
                this.startDragBitmap = null;
            }
            public override string ToString()
            {
                var sb = new StringBuilder();
                foreach (var dragging in GetDraggingNodes())
                {
                    sb.AppendLine(dragging.Text);
                }
                return sb.ToString();
            }
            public TreeNode[] GetDraggingNodes()
            {
                var ret = new List<TreeNode>();
                if (DraggingNode != null)
                {
                    ret.Add(DraggingNode);
                }
                foreach (var tn in CheckedNodes)
                {
                    if (!ret.Contains(tn))
                    {
                        ret.Add(tn);
                    }
                }
                return ret.ToArray();
            }

        }

        public delegate void OnDragDropTreeNodeToGroup(object sender, DragDropTreeNodeToGroupEvent e);
        public event OnDragDropTreeNodeToGroup DragDropTreeNodeToGroup;
        public event OnDragDropTreeNodeToGroup DragDropTreeNodeToGroupComplete;
        public class DragDropTreeNodeToGroupEvent : EventArgs
        {
            public DragEventArgs DragDrop;
            public HashSet<TreeNode> CheckedNodes = new HashSet<TreeNode>();
            public TreeNode DraggingNode;
            public TreeNode DropNode;
            public bool Cancel = false;

            public TreeNode[] GetDraggingNodes()
            {
                var ret = new List<TreeNode>();
                if (DraggingNode != null)
                {
                    ret.Add(DraggingNode);
                }
                foreach (var tn in CheckedNodes)
                {
                    if (!ret.Contains(tn))
                    {
                        ret.Add(tn);
                    }
                }
                return ret.ToArray();
            }
        }

        #endregion
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            //             var text = txtFilter.Text;
            // 
            //             if (string.IsNullOrEmpty(text))
            //             {
            //                 foreach (var node in treeView.GetAllNodes())
            //                 { node.IsVisible = }
            //             }
            //             else
            //             {
            //             }
        }
        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Find_Click(sender, e);
            }
        }
        private void btn_Find_Click(object sender, EventArgs e)
        {
            var text = txtFilter.Text;
            if (!string.IsNullOrEmpty(text))
            {
                TreeNode finded = FormUtils.FindTreeNodeByText(treeView.Nodes, text, last_find_object);
                if (finded == null)
                {
                    //从头再查一次，这样就可以循环搜索了
                    finded = FormUtils.FindTreeNodeByText(treeView.Nodes, text, null);
                }
                if (finded != null)
                {
                    treeView.SelectedNode = finded;
                    last_find_object = finded;
                    finded.EnsureVisible();
                }
            }
            else
            {
                treeView.ShowSearchDialog();
            }

        }

        TreeNode last_find_object = null;
        //------------------------------------------------------------------------------------------------------

    }

}
