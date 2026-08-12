using DeepCore;
using MaterialSkin.Controls;
using DeepEditor.Common.G2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public partial class G2DListSelectEditor : G2DBaseForm
    {
        private static Rectangle last_open_rect = Rectangle.Empty;
        private static bool ListItemEquals(ListViewItem item, object selected)
        {
            return selected != null && (object.Equals(item.Tag, selected) || item == selected || (selected is ListViewItem sitem && sitem.Tag == item.Tag));
        }
        private static bool TreeNodeEquals(TreeNode tn, object selected)
        {
            return selected != null && (tn.Tag == selected || tn == selected || (selected is TreeNode stn && stn.Tag == tn.Tag));
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        private Type ElementType;
        public object SelectedTag { get; private set; }
        public List<object> SelectedTags { get; private set; }
        public object SelectedSrc { get; private set; }
        public TreeNode SelectedSrcNode => SelectedSrc as TreeNode;
        public List<object> SelectedSrcs { get; private set; }
        public string SelectedTreeSavePath { get; private set; }
        //------------------------------------------------------------------------------------------------------------------------------------

        public G2DListSelectEditor(Type etype, IList list, object selected = null) : this(etype, list, item => ListItemEquals(item, selected))
        { }
        public G2DListSelectEditor(Type etype, Array list, object selected = null) : this(etype, list, item => ListItemEquals(item, selected))
        { }
        public G2DListSelectEditor(Type etype, TreeNodeCollection root, ImageList imageList, object selected = null) : this(etype, root, imageList, tn => TreeNodeEquals(tn, selected))
        { }
        public G2DListSelectEditor(Type etype, IEnumerable<TreeNode> root, ImageList imageList, object selected = null) : this(etype, root, imageList, tn => TreeNodeEquals(tn, selected))
        { }
        public G2DListSelectEditor(Type etype, TreeNode root, ImageList imageList, object selected = null) : this(etype, root, imageList, tn => TreeNodeEquals(tn, selected))
        { }
        //------------------------------------------------------------------------------------------------------------------------------------
        public G2DListSelectEditor(Type etype, IList list, Predicate<DuplicateListViewItem> selected) : this(etype, list?.ToArray(etype), selected) { }
        public G2DListSelectEditor(Type etype, Array list, Predicate<DuplicateListViewItem> selected)
        {
            InitializeComponent();
            this.ElementType = etype;
            this.listView1.Visible = true;
            this.treeView1.Visible = false;
            this.Load += (object sender, EventArgs evt) =>
            {
                if (list != null)
                {
                    foreach (var e in list)
                    {
                        var text = ElementToString(e);
                        var item = new DuplicateListViewItem(e, text);
                        item.Tag = e;
                        listView1.Items.Add(item);
                    }
                }
                if (selected != null)
                {
                    listView1.SelectedItems.Clear();
                    foreach (DuplicateListViewItem item in listView1.Items)
                    {
                        if (selected(item))
                        {
                            item.Selected = true;
                            return;
                        }
                    }
                }
            };
        }
        public G2DListSelectEditor(Type etype, IEnumerable<TreeNode> root, ImageList imageList, Predicate<G2DDuplicateTreeNode> selected)
        {
            InitializeComponent();
            this.ElementType = etype;
            this.listView1.Visible = false;
            this.treeView1.Visible = true;
            this.Load += (object sender, EventArgs evt) =>
            {
                if (root != null)
                {
                    this.treeView1.ImageList = imageList;
                    foreach (TreeNode tn in root)
                    {
                        if (tn is G2DDuplicateTreeNode tr)
                        {
                        }
                        else
                        {
                            tr = tn.TreeNodeDuplicate(src => new G2DDuplicateTreeNode(src)) as G2DDuplicateTreeNode;
                        }
                        if (tr != null)
                        {
                            this.treeView1.Nodes.Add(tr);
                            this.treeView1.CollapseAll();
                            tr.Expand();
                        }
                    }
                }
                if (selected != null)
                {
                    foreach (G2DDuplicateTreeNode tn in treeView1.GetAllNodes(false))
                    {
                        if (selected(tn))
                        {
                            treeView1.SelectedNode = tn;
                            tn.Parent?.Expand();
                            return;
                        }
                    }
                }
            };
        }
        public G2DListSelectEditor(Type etype, TreeNodeCollection root, ImageList imageList, Predicate<G2DDuplicateTreeNode> selected)
        {
            InitializeComponent();
            this.ElementType = etype;
            this.listView1.Visible = false;
            this.treeView1.Visible = true;
            this.Load += (object sender, EventArgs evt) =>
            {
                if (root != null)
                {
                    this.treeView1.ImageList = imageList;
                    foreach (TreeNode tn in root)
                    {
                        if (tn is G2DDuplicateTreeNode tr)
                        {
                        }
                        else
                        {
                            tr = tn.TreeNodeDuplicate(src => new G2DDuplicateTreeNode(src)) as G2DDuplicateTreeNode;
                        }
                        if (tr != null)
                        {
                            this.treeView1.Nodes.Add(tr);
                            this.treeView1.CollapseAll();
                            tr.Expand();
                        }
                    }
                }
                if (selected != null)
                {
                    foreach (G2DDuplicateTreeNode tn in treeView1.GetAllNodes(false))
                    {
                        if (selected(tn))
                        {
                            treeView1.SelectedNode = tn;
                            tn.Parent?.Expand();
                            return;
                        }
                    }
                }
            };
        }
        public G2DListSelectEditor(Type etype, TreeNode root, ImageList imageList, Predicate<G2DDuplicateTreeNode> selected)
        {
            InitializeComponent();
            this.ElementType = etype;
            this.listView1.Visible = false;
            this.treeView1.Visible = true;
            this.Load += (object sender, EventArgs evt) =>
            {
                if (root != null)
                {
                    this.treeView1.ImageList = imageList;
                    if (root is G2DDuplicateTreeNode tr)
                    {
                    }
                    else
                    {
                        tr = root.TreeNodeDuplicate(src => new G2DDuplicateTreeNode(src)) as G2DDuplicateTreeNode;
                    }
                    if (tr != null)
                    {
                        this.treeView1.Nodes.Add(tr);
                        this.treeView1.CollapseAll();
                        tr.Expand();
                    }
                }
                if (selected != null)
                {
                    foreach (G2DDuplicateTreeNode tn in treeView1.GetAllNodes(false))
                    {
                        if (selected(tn))
                        {
                            treeView1.SelectedNode = tn;
                            tn.Parent?.Expand();
                            return;
                        }
                    }
                }
            };
        }
        public G2DListSelectEditor(Type etype, TreeNode root, ImageList imageList, string path)
        {
            InitializeComponent();
            this.ElementType = etype;
            this.listView1.Visible = false;
            this.treeView1.Visible = true;
            this.Load += (object sender, EventArgs evt) =>
            {
                if (root != null)
                {
                    this.treeView1.ImageList = imageList;
                    if (root is G2DDuplicateTreeNode tr)
                    {
                    }
                    else
                    {
                        tr = root.TreeNodeDuplicate(src => new G2DDuplicateTreeNode(src)) as G2DDuplicateTreeNode;
                    }
                    if (tr != null)
                    {
                        this.treeView1.Nodes.Add(tr);
                        this.treeView1.CollapseAll();
                        tr.Expand();
                    }
                }
                if (path != null)
                {
                    foreach (G2DDuplicateTreeNode tn in treeView1.GetAllNodes(false))
                    {
                        if (tn.GetSavePath(true) == path)
                        {
                            treeView1.SelectedNode = tn;
                            tn.Parent?.Expand();
                            return;
                        }
                    }
                }
            };
        }
        public G2DListSelectEditor(Type etype, TreeView root, Predicate<G2DDuplicateTreeNode> selected) : this(etype, root.Nodes, root.ImageList, selected)
        {

        }

        //------------------------------------------------------------------------------------------------------------------------------------

        protected virtual string ElementToString(object e) { return e.ToString(); }

        private void G2DListSelectEditor_Load(object sender, EventArgs e)
        {
            {
                this.Text = $"选择：{ElementType.Name}";
            }
            if (last_open_rect != Rectangle.Empty)
            {
                this.Bounds = last_open_rect;
            }
            if (treeView1.Visible && treeView1.Nodes.Count > 0)
            {
                treeView1.Nodes[0].Expand();
            }
        }

        private void G2DListSelectEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            SelectedTags = new List<object>();
            SelectedSrcs = new List<object>();
            if (listView1.Visible)
            {

                if (listView1.SelectedItems.Count > 0)
                {
                    SelectedTag = listView1.SelectedItems[0].Tag;
                    SelectedSrc = (listView1.SelectedItems[0] as DuplicateListViewItem).Src;
                    foreach (DuplicateListViewItem s in listView1.SelectedItems)
                    {
                        if (ElementType == null || ElementType.IsInstanceOfType(s.Tag))
                        {
                            SelectedTags.Add(s.Tag);
                        }
                        SelectedSrcs.Add(s.Src);
                    }
                }
            }
            else if (treeView1.Visible)
            {
                if (treeView1.SelectedNode != null)
                {
                    SelectedTreeSavePath = treeView1.SelectedNode.GetSavePath(true);
                    SelectedTag = treeView1.SelectedNode.Tag;
                    SelectedSrc = (treeView1.SelectedNode as G2DDuplicateTreeNode).SrcNode;
                    foreach (G2DDuplicateTreeNode s in treeView1.SelectedNode.GetAllNodes())
                    {
                        if (ElementType == null || ElementType.IsInstanceOfType(s.Tag))
                        {
                            SelectedTags.Add(s.Tag);
                        }
                        SelectedSrcs.Add(s.SrcNode);
                    }
                }
            }
            last_open_rect = this.Bounds;
        }


        //------------------------------------------------------------------------------------------------------------------------------------

        //         private ElementToStringHandler event_ElementToString;
        //         private FilterHandler event_Filter;
        // 
        //         public delegate string ElementToStringHandler(object item, object tag);
        //         //public delegate bool FilterHandler(object item, object tag);
        // 
        //         public event ElementToStringHandler ElementToString
        //         {
        //             add { event_ElementToString = value; }
        //             remove { event_ElementToString = value; }
        //         }
        //         public event FilterHandler Filter
        //         {
        //             add { event_Filter = value; }
        //             remove { event_Filter = value; }
        //         }
        //------------------------------------------------------------------------------------------------------------------------------------
    }

    public class DuplicateListViewItem : ListViewItem
    {
        public object Src { get; }
        public DuplicateListViewItem(object src, string text) : base(text)
        {
            this.Src = src;
        }
    }

}
