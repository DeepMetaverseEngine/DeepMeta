using DeepCore;
using DeepCore.IO;
using DeepCore.Reflection;
using DeepCore.Xml;
using DeepEditor.Common.Windows;
using MaterialSkin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static AntdUI.GridPanel;

namespace DeepEditor.Common.G2D
{
    public class G2DTreeView : TreeView, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        public bool EnableCopyPaste { get; set; } = false;

        public G2DTreeView()
        {
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }
        protected override void Dispose(bool disposing)
        {
            this.FetchNodeIcon = null;
            base.Dispose(disposing);
        }

        //------------------------------------------------------------------------------------------------------
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.ShowSearchDialog();
                return;
            }
            if (EnableCopyPaste)
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    CopyToClipboard(SelectedNode);
                    return;
                }
                if (e.Control && e.KeyCode == Keys.V)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    PasteFromClipboard();
                    return;
                }
            }
            base.OnKeyDown(e);
        }

        public T FindParentNodeAs<T>(TreeNode selected, Predicate<T> predicate = null) where T : TreeNode
        {
            while (selected != null)
            {
                if (selected is T g2d)
                {
                    if (predicate == null || predicate(g2d))
                    {
                        return g2d;
                    }
                }
                selected = selected.Parent;
            }
            return null;
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            var selected = this.GetNodeAt(e.Location);
            this.SelectedNode = selected;
        }
        //------------------------------------------------------------------------------------------------------
        #region Display

        public delegate void FetchNodeIconHandler(TreeNode node);
        public event FetchNodeIconHandler FetchNodeIcon;

        private bool no_draw = false;
        public void BeginNoDraw() { no_draw = true; }
        public void EndNoDraw() { no_draw = false; }

        public void RefreshAllNode()
        {
            this.SuspendLayout();
            BeginNoDraw();
            try
            {
                foreach (TreeNode node in this.Nodes)
                {
                    if (node is G2DTreeNode g2d)
                    {
                        g2d.Refresh();
                    }
                }
                if (SelectedNode is G2DTreeNode tn)
                {
                    tn.Refresh();
                }
                //propertyGrid.Refresh();
            }
            finally
            {
                EndNoDraw();
                this.ResumeLayout();
                Invalidate();
            }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            foreach (var c in this.GetAllNodes())
            {
                if (c is G2DTreeNode g2D)
                {
                    g2D.Refresh();
                }
            }
        }
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            if (no_draw) return;
            //var font = e.Node.NodeFont ?? e.Node.TreeView.Font;           
            //             if (e.Node is G2DTreeNode g2d && g2d.IsModified)
            //             {
            //                 TextRenderer.DrawText(e.Graphics, "*", font,
            //                     new Rectangle(e.Bounds.X, e.Bounds.Y, 20, 20), 
            //                     Color.Red, TextFormatFlags.GlyphOverhangPadding);
            //             }
            FetchNodeIcon?.Invoke(e.Node);
            //             if (DrawMode == TreeViewDrawMode.OwnerDrawText)
            //             {
            //                 this.OwnerDrawText(e);
            //             }
            //             else if (DrawMode == TreeViewDrawMode.OwnerDrawAll)
            //             {
            //                 this.OwnerDrawAll(e);
            //             }
            e.DrawDefault = false;
            base.OnDrawNode(e);
            //             if (e.Node is G2DTreeNode tn && tn.IsModified)
            //             {
            //                 var r = e.Bounds;
            //                 e.Graphics.SmoothingMode= System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //                 e.Graphics.FillEllipse(Brushes.Red, new RectangleF(r.X,r.Y,4,4));
            //             }
            //             else if (e.Node is G2DTreeNodeRoot tr && tr.IsModified)
            //             {
            //                 var r = e.Bounds;
            //                 e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //                 e.Graphics.FillEllipse(Brushes.Red, new RectangleF(r.X, r.Y, 4, 4));
            //             }
            //               if (dragOverEvent != null)
            //               {
            //                   var pos = PointToClient(new Point(dragOverEvent.X, dragOverEvent.Y));
            //                   var dropNode = this.GetNodeAt(pos);
            //                   if (dropNode == e.Node)
            //                   {
            //                       e.Graphics.DrawRectangle(new Pen(SkinManager.TextHighEmphasisColor), e.Bounds);
            //                   }
            //               }
            //             if (e.Node.BackColor.A != 0)
            //             {
            //                 e.Graphics.FillRectangle(new SolidBrush(e.Node.BackColor), e.Bounds);
            //             }
            var node = e.Node;
            {
                var g = e.Graphics;
                Rectangle bounds = node.Bounds;
                Size textSize = TextRenderer.MeasureText(node.Text, node.TreeView!.Font);
                Point textLoc = new(bounds.X - 1, bounds.Y); // required to center the text
                bounds = new Rectangle(textLoc, new Size(textSize.Width, bounds.Height));

                // Simulate default text drawing here
                TreeNodeStates curState = e.State;
                Font font = node.NodeFont ?? node.TreeView.Font;
                Color color = (((curState & TreeNodeStates.Selected) == TreeNodeStates.Selected) && node.TreeView.Focused) ?
                    SystemColors.HighlightText :
                    (node.ForeColor != Color.Empty) ? node.ForeColor : node.TreeView.ForeColor;

                // Draw the actual node.
                if ((curState & TreeNodeStates.Selected) == TreeNodeStates.Selected)
                {
                    if (node.BackColor.A != 0)
                    {
                        g.FillRectangle(SystemBrushes.Highlight, bounds);
                        using (var brush = node.BackColor.GetCachedSolidBrushScope())
                        {
                            g.FillRectangle(brush, bounds.X + 3, bounds.Y + 3, bounds.Width - 6, bounds.Height - 6);
                        }
                    }
                    else
                    {
                        g.FillRectangle(SystemBrushes.Highlight, bounds);
                    }
                    ControlPaint.DrawFocusRectangle(g, bounds, color, SystemColors.Highlight);
                    TextRenderer.DrawText(g, node.Text, font, bounds, color, TextFormatFlags.Default);
                }
                else
                {
                    if (node.BackColor.A != 0)
                    {
                        using (var brush = node.BackColor.GetCachedSolidBrushScope())
                        {
                            g.FillRectangle(brush, e.Bounds);
                        }
                    }
                    else
                    {
                        using (var brush = BackColor.GetCachedSolidBrushScope())
                        {
                            g.FillRectangle(brush, bounds);
                        }
                    }

                    TextRenderer.DrawText(g, node.Text, font, bounds, color, TextFormatFlags.Default);
                }
            }
        }

        protected virtual void OwnerDrawText(DrawTreeNodeEventArgs e)
        {
            var state = e.State;
            var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
            var fore = e.Node.ForeColor;
            if (fore == Color.Empty) fore = e.Node.TreeView.ForeColor;
            if (e.Node == e.Node.TreeView.SelectedNode)
            {
                fore = SystemColors.HighlightText;
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, fore, SystemColors.Highlight);
            }
            else
            {
                //e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
            }
            //             if (DrawModeTowLineText)
            //             {
            //                 if (e.Node is G2DTreeNode dnode)
            //                 {
            //                     TextRenderer.DrawText(e.Graphics, dnode.TextID + Environment.NewLine + dnode.TextName, font, e.Bounds, fore, TextFormatFlags.VerticalCenter);
            //                 }
            //                 else if (e.Node is G2DTreeNodeGroup gnode)
            //                 {
            //                     TextRenderer.DrawText(e.Graphics, gnode.Text, font, e.Bounds, fore, TextFormatFlags.VerticalCenter);
            //                 }
            //                 else
            //                 {
            //                     TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, fore, TextFormatFlags.GlyphOverhangPadding);
            //                 }
            //             }
            //             else
            {
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, fore, TextFormatFlags.GlyphOverhangPadding);
            }
        }
        protected virtual void OwnerDrawAll(DrawTreeNodeEventArgs e)
        {
            var imageList = this.ImageList;
            var tn = e.Node as TreeNode;
            if (tn == null)
            {
                return;
            }
            Point ptbigimage = new Point(tn.Bounds.X - 5, tn.Bounds.Y + 5);
            Point ptsmallimage = new Point(tn.Bounds.X - 5, tn.Bounds.Y + 5);
            Size szbig = new Size(16, 16);//打开收缩图片
            Size szsmall = new Size(16, 16);//叶子图片
                                            //Rectangle bcimage = new Rectangle(ptimage, new Size(33, 23));
                                            //根据节点增加图片
                                            //             if (tn.IsExpanded)
                                            //             {
                                            //                 e.Graphics.DrawImage(imageList1.Images[ExpandImageIndex], new Rectangle(ptbigimage, szbig));
                                            //             }
                                            //             else if (tn.Nodes.Count > 0)
                                            //             {
                                            //                 e.Graphics.DrawImage(imageList1.Images[CollapseImageIndex], new Rectangle(ptbigimage, szbig));
                                            //             }
                                            //             else
                                            //             {
                                            //                 e.Graphics.DrawImage(imageList1.Images[LeafImageIndex], new Rectangle(ptsmallimage, szsmall));
                                            //             }
                                            //增加背景颜色
            Point pt = new Point(tn.Bounds.X + 11, tn.Bounds.Y);
            Brush bccolor = new SolidBrush(System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(229)))), ((int)(((byte)(242))))));
            Brush ybccolor = new SolidBrush(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))));
            Rectangle bcrt = new Rectangle(pt, new Size(Width - e.Bounds.X, e.Bounds.Height));//使用Width充满整行
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                e.Graphics.FillRectangle(bccolor, bcrt);
            }
            else
            {
                e.Graphics.FillRectangle(ybccolor, bcrt);
            }
            //增加格线
            using (Pen focusPen = new Pen(System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))))))
            {
                focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                e.Graphics.DrawRectangle(focusPen, bcrt);
            }
            //设置文本颜色和字体
            Point ptft = new Point(tn.Bounds.X + 16, tn.Bounds.Y + 7);
            Rectangle rt = new Rectangle(ptft, new Size(e.Bounds.Width, e.Bounds.Height));
            Brush bfcolor = new SolidBrush(System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59))))));
            var nf = tn.NodeFont == null ? ((TreeView)this).Font : tn.NodeFont;
            e.Graphics.DrawString(e.Node.Text, nf, bfcolor, Rectangle.Inflate(rt, 1, 4));
        }


        #endregion

        //------------------------------------------------------------------------------------------------------
        #region CopyPaste

        //         private static string copyName;
        //         private static object copyData;
        private static CopyPaste s_copying = new CopyPaste(typeof(G2DTreeView).FullName);

        public void CopyToClipboard(TreeNode node)
        {
            if (node is G2DTreeNode g2d)
            {
                // var copyName = g2d.TextID;
                // copyData = XmlUtil.CloneObject(g2d.Data);
                s_copying.Copy(g2d.TextID, g2d.Data);
                try
                {
                    Win32.SetClipboard(g2d.TextID);
                }
                catch { }
            }
            else if (node != null)
            {
                try
                {
                    Win32.SetClipboard(node.ToString());
                }
                catch { }
            }
        }
        public void PasteFromClipboard()
        {
            //if (copyData != null)
            if (s_copying.TryPaste(out var copyName, out var copyData))
            {
                var node = DuplicateNode(copyData, copyName);
                if (node != null)
                {
                    this.SelectedNode = node;
                }
            }
        }

        public TreeNode DuplicateNode(object data, string name)
        {
            if (data != null)
            {
                var selectedGroup = FindParentNodeAs<G2DTreeNodeGroup>(SelectedNode);
                var selectedRoot = FindParentNodeAs<G2DTreeNodeRoot>(SelectedNode);
                if (selectedRoot != null && selectedGroup != null)
                {
                    var newdata = XmlUtil.CloneObject(data);
                    var newname = name + "(1)";
                    var newnode = selectedRoot.CreateDataNode(newdata);
                    if (newnode.DataIDType.IsNumberType())
                    {
                        newname = newnode.DataIDType.TextNumberAdd(name, 1).ToString();
                    }
                    while (newname != null)
                    {
                        newname = G2DTextDialog.Show(newname, "复制");
                        if (newname != null)
                        {
                            if (selectedRoot.ContainsG2DNodeID(newname))
                            {
                                MessageBox.Show(newname + " 已存在！");
                                continue;
                            }
                            try
                            {
                                newnode.SetDataID(newname);
                                if (!selectedRoot.AddG2DNode(newnode, selectedGroup))
                                {
                                    MessageBox.Show($"无法添加 {newdata}");
                                }
                                return newnode;
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show(err.Message);
                            }
                        }
                        return null;
                    }
                }
            }
            return null;
        }

        #endregion
        //------------------------------------------------------------------------------------------------------
        public event NodeModifyAction NodeModifyed;
        internal void OnNodeModified(TreeNode tn)
        {
            NodeModifyed?.Invoke(this, tn);
        }
        public void LoadState(FileInfo sateFile, TreeStateInfoConfig cfg)
        {
            try
            {
                if (sateFile.Exists)
                {
                    this.SetTreeInfo(File.ReadAllText(sateFile.FullName, CUtils.UTF8), cfg);
                    this.SelectedNode?.EnsureVisible();
                }
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        public void SaveState(FileInfo sateFile)
        {
            try
            {
                CFiles.CreateFile(sateFile);
                File.WriteAllText(sateFile.FullName, this.GetTreeInfo(), CUtils.UTF8);
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
    }
    //------------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------------------


    public class G2DTreeViewAdapter<T>
    {
        public delegate void TreeNodeAddedHandler(G2DTreeNode<T> node);
        public delegate void TreeNodeRemovedHandler(G2DTreeNode<T> node);
        public delegate void TreeNodeRenamedHandler(G2DTreeNode<T> node);

        public event TreeNodeAddedHandler OnTreeNodeAdded;
        public event TreeNodeRemovedHandler OnTreeNodeRemoved;
        public event TreeNodeRenamedHandler OnTreeNodeRenamed;

        private TreeView treeView;
        private G2DTreeNodeGroup treeRoot;

        public G2DTreeViewAdapter(
            TreeView treeView,
            G2DTreeNodeGroup root)
        {
            this.treeView = treeView;
            this.treeRoot = root;

            this.treeView.ItemDrag += new ItemDragEventHandler(treeView1_ItemDrag);
            this.treeView.DragDrop += new DragEventHandler(treeView1_DragDrop);
            this.treeView.DragOver += new DragEventHandler(treeView1_DragOver);
            this.treeView.DragEnter += new DragEventHandler(treeView1_DragEnter);
        }

        public G2DTreeNodeGroup GetSelectedGroup()
        {
            if (treeView.SelectedNode is G2DTreeNodeGroup)
            {
                return treeView.SelectedNode as G2DTreeNodeGroup;
            }
            if (treeView.SelectedNode is G2DTreeNode<T>)
            {
                return treeView.SelectedNode.Parent as G2DTreeNodeGroup;
            }
            return null;
        }

        public G2DTreeNode<T> GetSelectedObject()
        {
            if (treeView.SelectedNode is G2DTreeNode<T>)
            {
                return treeView.SelectedNode as G2DTreeNode<T>;
            }
            return null;
        }

        public void RemoveSelectedObject()
        {
            G2DTreeNode<T> node = GetSelectedObject();
            if (node != null)
            {
                if (MessageBox.Show("确定要删除: " + node.TextID, "确认",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    node.Parent.Nodes.Remove(node);
                    if (OnTreeNodeRemoved != null)
                    {
                        OnTreeNodeRemoved.Invoke(node);
                    }
                }
            }
        }

        public void DuplicateSelectedObject()
        {
            G2DTreeNode<T> node = GetSelectedObject();
            if (node != null)
            {
                G2DTreeNodeGroup parent = node.Parent as G2DTreeNodeGroup;

                T copy = XmlUtil.CloneObject(node.Data);
                G2DTreeNode<T> copy_node = new G2DTreeNode<T>(copy);
                copy_node.ImageKey = node.ImageKey;
                copy_node.SelectedImageKey = node.SelectedImageKey;
                copy_node.ContextMenuStrip = node.ContextMenuStrip;

                string name = copy_node.TextID;

                while (!string.IsNullOrEmpty(name))
                {
                    name = G2DTextDialog.Show(name, "复制 " + node.Name);
                    if (name != null)
                    {
                        if (ContainsObject(name))
                        {
                            MessageBox.Show("\"" + name + "\" 已存在!");
                        }
                        else
                        {
                            copy_node.SetDataID(name);
                            parent.Nodes.Add(copy_node);
                            parent.Expand();
                            treeView.SelectedNode = copy_node;
                            if (OnTreeNodeAdded != null)
                            {
                                OnTreeNodeAdded.Invoke(copy_node);
                            }
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        public void RenameObject()
        {
            G2DTreeNode<T> node = GetSelectedObject();
            if (node != null)
            {
                string name = node.TextID;
                while (!string.IsNullOrEmpty(name))
                {
                    name = G2DTextDialog.Show(name, "重命名 " + name);
                    if (name != null)
                    {
                        if (ContainsObject(name))
                        {
                            MessageBox.Show("\"" + name + "\" 已存在!");
                        }
                        else
                        {
                            string src_name = node.TextID;
                            node.SetDataID(name);
                            if (OnTreeNodeRenamed != null)
                            {
                                OnTreeNodeRenamed.Invoke(node);
                            }
                            return;
                        }
                    }
                }
            }
        }

        public G2DTreeNodeGroup GetTreeRoot()
        {
            return treeRoot;
        }

        public bool ContainsObject(string name)
        {
            TreeNode tn = GetTreeRoot().FindNodeByText<G2DTreeNode<T>>(name, true);
            return tn != null;
        }

        //         public void AddGroup()
        //         {
        //             G2DTreeNodeGroup parent = GetSelectedGroup();
        //             if (parent != null)
        //             {
        //                 string name = G2DTextDialog.Show("分组", "添加过滤器");
        //                 if (name != null)
        //                 {
        //                     if (parent.TryAddG2DGroup(name, out var group))
        //                     {
        //                         parent.Expand();
        //                         treeView.SelectedNode = group;
        //                     }
        //                     else
        //                     {
        //                         MessageBox.Show("不能创建分组: " + name);
        //                     }
        //                 }
        //             }
        //         }

        public G2DTreeNode<T> NewObject(T data)
        {
            G2DTreeNodeGroup parent = GetSelectedGroup();
            if (parent != null)
            {
                string name = data.GetType().Name + GetTreeRoot().GetAllNodesCount();
                while (!string.IsNullOrEmpty(name))
                {
                    name = G2DTextDialog.Show(name, "添加:" + name);
                    if (name != null)
                    {
                        if (ContainsObject(name))
                        {
                            MessageBox.Show("\"" + name + "\" 已存在!");
                        }
                        else
                        {
                            G2DTreeNode<T> node = new G2DTreeNode<T>(data);
                            node.SetDataID(name);
                            parent.Nodes.Add(node);
                            parent.Expand();
                            treeView.SelectedNode = node;
                            if (OnTreeNodeAdded != null)
                            {
                                OnTreeNodeAdded.Invoke(node);
                            }
                            return node;
                        }
                    }
                }
            }
            return null;
        }

        #region Delegate


        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            treeView.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            Point pos = treeView.PointToClient(new Point(e.X, e.Y));
            TreeNode dropNode = this.treeView.GetNodeAt(pos);
            G2DTreeNodeBase child_node = (G2DTreeNodeBase)e.Data.GetData(typeof(G2DTreeNode<T>));
            G2DTreeNodeBase group_node = (G2DTreeNodeBase)e.Data.GetData(typeof(G2DTreeNodeGroup));
            if (dropNode is G2DTreeNodeGroup)
            {
                var group = dropNode as G2DTreeNodeGroup;
                if (child_node == null && group_node == null)
                {
                    MessageBox.Show("error");
                }
                else if (child_node != null)
                {
                    child_node.RemoveFromParent();
                    group.Nodes.Add(child_node);
                    group.Expand();
                    //treeView1.SelectedNode = child_node;
                }
                else if (group_node != group)
                {
                    if (!group_node.ContainsChild(group, true))
                    {
                        group_node.RemoveFromParent();
                        group.Nodes.Add(group_node);
                        group.Expand();
                        //treeView1.SelectedNode = group_node;
                    }
                }
            }
            else if (dropNode.Parent is G2DTreeNodeGroup)
            {
                var group = dropNode.Parent as G2DTreeNodeGroup;
                if (child_node == null && group_node == null)
                {
                    MessageBox.Show("error");
                }
                else if (child_node != null)
                {
                    child_node.RemoveFromParent();
                    group.Nodes.Add(child_node);
                    group.Expand();
                    //treeView1.SelectedNode = child_node;
                }
                else if (group_node != group)
                {
                    if (!group_node.ContainsChild(group, true))
                    {
                        group_node.RemoveFromParent();
                        group.Nodes.Add(group_node);
                        group.Expand();
                        //treeView1.SelectedNode = group_node;
                    }
                }
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(G2DTreeNode<T>)))
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

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            Point pos = treeView.PointToClient(new Point(e.X, e.Y));
            TreeNode dropNode = this.treeView.GetNodeAt(pos);
            if (dropNode is G2DTreeNodeBase)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        #endregion
    }

    public class SelectTreeNodeCollection : IReadOnlyList<TreeNode>
    {
        private readonly List<TreeNode> selectedNodeList;
        public SelectTreeNodeCollection(List<TreeNode> list) { this.selectedNodeList = list; }
        public TreeNode this[int index] => ((IReadOnlyList<TreeNode>)selectedNodeList)[index];
        public int Count => ((IReadOnlyCollection<TreeNode>)selectedNodeList).Count;
        public IEnumerator<TreeNode> GetEnumerator()
        {
            return ((IEnumerable<TreeNode>)selectedNodeList).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)selectedNodeList).GetEnumerator();
        }
    }
    //------------------------------------------------------------------------------------------------------
#if false
    ///<summary>
    /// 导航树控件
    ///</summary>
    [DesignTimeVisible(true)]
    [Serializable]
    public class GTreeView : TreeView
    {
        #region 成员变量

        private List<TreeNode> selectedNodeList = new List<TreeNode>();

        ///<summary>
        /// 当前节点
        ///</summary>
        private TreeNode currentNode = null;

        #endregion

        #region 属性

        ///<summary>
        /// 是否是多选
        ///</summary>
        public bool IsMultiSelect { get; set; } = false;

        ///<summary>   
        ///   选择的节点的集合
        ///</summary>
        public SelectTreeNodeCollection SelectedNodeList
        {
            get { return new SelectTreeNodeCollection(selectedNodeList); }
        }

        #endregion

        #region Delegate & Event

        ///<summary>
        /// 节点被拖动后要处理事件的Delegate
        ///</summary>
        ///<param name="sourceNode">被拖动的节点</param>
        ///<param name="targetNode">目标节点</param>
        public delegate void OnDragNodeSucceed(TreeNode sourceNode, TreeNode targetNode);

        ///<summary>
        /// 判断目标节点是否接受拖动的Delegate
        ///</summary>
        ///<param name="targetNode"></param>
        ///<returns></returns>
        public delegate bool IsNodeCanAcceptDrag(TreeNode targetNode);

        ///<summary>
        /// 节点被拖动后要处理事件
        ///</summary>
        public event OnDragNodeSucceed TreeNodeCanAcceptDragedHandler;

        ///<summary>
        /// 判断目标节点是否接受拖动的事件处理
        ///</summary>
        public event IsNodeCanAcceptDrag IsNodeCanAcceptDragHandler;

        #endregion

        #region 类函数

        ///<summary>
        /// 构造函数
        ///</summary>
        public GTreeView() : base()
        {
            //this.DrawMode = TreeViewDrawMode.OwnerDrawText;
        }

        ///<summary>
        /// 鼠标单击事件
        ///</summary>
        ///<param name="e">TreeNodeMouseClickEventArgs对象类</param>
        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            //this.SelectedNode = e.Node;
            var isSingleSelected = false;
            // 如果是多选，则根据按钮情况设置节点的选择状态
            if (IsMultiSelect)
            {
                //                 if (!(SelectedNodeList.Count == 1 && SelectedNodeList[0] == SelectedNode))
                //                 {
                //                     if ((Control.ModifierKeys & Keys.Control) != 0 || e.Button == MouseButtons.Right)
                //                     {
                //                         ctrlMultiSelectNodes(SelectedNode, e.Button == MouseButtons.Right);
                //                     }
                //                     else if ((Control.ModifierKeys & Keys.Shift) != 0)
                //                     {
                //                         shiftMultiSelectNodes(SelectedNode, e.Button == MouseButtons.Right);
                //                     }
                //                     else
                //                     {
                //                         isSingleSelected = true;
                //                         singleSelectNode(SelectedNode);
                //                     }
                //                 }
                //                 isSingleSelected = true;
                //                 this.Invalidate();
                base.OnNodeMouseClick(e);
            }
            else
            {
                base.OnNodeMouseClick(e);
            }
        }


        ///<summary>
        /// 重绘,主要是在Checkbox/RadioButton前面有图片
        ///</summary>
        ///<param name="e">DrawTreeNodeEventArgs对象类</param>
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            if (IsMultiSelect)
            {
                e.DrawDefault = false;
                if (e.Bounds.X == -1)
                    return;

                e.DrawDefault = false;

                Font font = this.Font;
                if (e.Node.NodeFont != null) font = e.Node.NodeFont;

                Color color = this.ForeColor;
                if (selectedNodeList.Contains(e.Node))
                {
                    color = SystemColors.HighlightText;
                }
                else if (e.Node.ForeColor != Color.Empty)
                {
                    color = e.Node.ForeColor;
                }

                Graphics g = e.Graphics;
                Rectangle textBounds = new Rectangle();

                var extNode = e.Node as TreeNode;

                textBounds.X = e.Bounds.X;
                textBounds.Y = e.Bounds.Y;
                textBounds.Width = e.Bounds.Width;
                textBounds.Height = e.Bounds.Height;

                // 绘制节点的文本
                if (selectedNodeList.Contains(e.Node))
                {
                    g.FillRectangle(SystemBrushes.Highlight, textBounds);
                    ControlPaint.DrawFocusRectangle(g, textBounds, color, SystemColors.Highlight);
                    TextRenderer.DrawText(g, e.Node.Text, font, textBounds, color, TextFormatFlags.Default);
                }
                else
                {
                    g.FillRectangle(SystemBrushes.Window, textBounds);
                    TextRenderer.DrawText(g, e.Node.Text, font, textBounds, color, TextFormatFlags.Default);
                }
            }
            else
            {
                base.OnDrawNode(e);
            }
        }

        #endregion

        #region 鼠标拖动节点移动
#if false
        ///<summary>
        /// 拖动节点移动,在鼠标拖放操作结束时发生
        ///</summary>
        ///<param name="drgevent">DragEventArgs对象</param>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);

            var moveNode = (TreeNode)drgevent.Data.GetData(typeof(TreeNode));

            //根据鼠标坐标确定要移动到的目标节点 
            Point point = this.PointToClient(new Point(drgevent.X, drgevent.Y));
            var targetNode = this.GetNodeAt(point);

            // 如果目标节点不接受拖动，则返回
            if (IsNodeCanAcceptDragHandler != null)
            {
                if (!IsNodeCanAcceptDragHandler(targetNode))
                {
                    return;
                }
            }

            // 确定落下的节点不是被拖拽节点本身或者被拖拽节点的子节点
            if (!moveNode.Equals(targetNode) && !containsNode(moveNode, targetNode))
            {
                var newMoveNode = (TreeNode)moveNode.Clone();
                targetNode.Nodes.Insert(targetNode.Index, newMoveNode);

                //更新当前拖动的节点选择 
                this.SelectedNode = newMoveNode;

                //移除拖放的节点 
                moveNode.Remove();
                moveNode = newMoveNode;
                newMoveNode.Expand();

                if (TreeNodeCanAcceptDragedHandler != null)
                {
                    TreeNodeCanAcceptDragedHandler(moveNode, targetNode);
                }
            }
        }

        ///<summary>
        /// 拖动节点移动,在用鼠标将某项拖动到该控件的工作区时发生
        ///</summary>
        ///<param name="drgevent">DragEventArgs对象</param>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);

            if (drgevent.Data.GetDataPresent(typeof(TreeNode)))
            {
                drgevent.Effect = DragDropEffects.Move;
            }
            else
            {
                drgevent.Effect = DragDropEffects.None;
            }
        }

        ///<summary>
        /// 拖动节点移动，在用户开始拖动项时发生
        ///</summary>
        ///<param name="e">ItemDragEventArgs对象</param>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);

            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }
#endif
    #endregion 鼠标拖动节点移动

        #region Private Methods

        ///<summary>   
        ///   按ctrl键多选的方法 
        ///</summary>   
        ///<param   name="node"></param>   
        ///<param   name="mustSelect"></param>
        private void ctrlMultiSelectNodes(TreeNode node, bool mustSelect)
        {
            if (selectedNodeList.Contains(node) && !mustSelect)
            {
                selectedNodeList.Remove(node);
                setCurrentNode((TreeNode)SelectedNodeList[SelectedNodeList.Count - 1]);
            }
            else if (!mustSelect)
            {
                selectedNodeList.Add(node);
                setCurrentNode(node);
            }
        }

        ///<summary>
        /// 按shift键多选的方法 
        ///</summary>
        ///<param name="node"></param>
        ///<param name="mustSelect"></param>
        private void shiftMultiSelectNodes(TreeNode node, bool mustSelect)
        {
            if (mustSelect)
            {
                return;
            }
            if (selectedNodeList.Contains(node))
            {
                selectedNodeList.Remove(node);
                setCurrentNode((TreeNode)SelectedNodeList[SelectedNodeList.Count - 1]);
            }
            else
            {
                if (node.Parent == currentNode.Parent)
                {

                    TreeNode addNode = node;
                    for (int i = System.Math.Abs(currentNode.Index - node.Index); i > 0; i--)
                    {
                        if (!selectedNodeList.Contains(addNode))
                        {
                            selectedNodeList.Add(addNode);
                        }

                        addNode = currentNode.Index > node.Index ? addNode.NextNode : addNode.PrevNode;
                    }

                    setCurrentNode(node);
                }
                else
                {
                    singleSelectNode(SelectedNode);
                }
            }


        }

        ///<summary>   
        /// single select   
        ///</summary>   
        ///<param   name="node"></param>
        private void singleSelectNode(TreeNode node)
        {
            selectedNodeList.Clear();
            selectedNodeList.Add(node);
            setCurrentNode(node);
        }

        ///<summary>   
        ///   Set current node   
        ///</summary>   
        ///<param   name="node"></param>
        private void setCurrentNode(TreeNode node)
        {
            //if (isMulSelect)
            //    SelectedNode = null;
            if (currentNode != node)
            {
                currentNode = node as TreeNode;
            }
        }


        ///<summary>
        /// 确定一个节点是否是另一个节点的祖先节点
        ///</summary>
        ///<param name="parentNode"></param>
        ///<param name="childNode"></param>
        ///<returns></returns>
        private bool containsNode(TreeNode parentNode, TreeNode childNode)
        {
            if (childNode.Parent == null) return false;
            if (childNode.Parent.Equals(parentNode)) return true;

            return containsNode(parentNode, childNode.Parent);
        }

        #endregion
    }
#endif
}

