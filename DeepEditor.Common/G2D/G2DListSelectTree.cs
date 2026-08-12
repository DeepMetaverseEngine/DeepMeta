using System;
using System.IO;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace DeepEditor.Common.G2D
{
    public partial class G2DListSelectTree : UserControl
    {
        public TreeView TreeView { get => this.g2dTreeViewControl1.TreeView; }
        public G2DTreeViewControl TreeViewControl { get => this.g2dTreeViewControl1; }
        public G2DDuplicateTreeNode SelectedTreeNode => TreeView.SelectedNode as G2DDuplicateTreeNode;
        public TreeNode SelectedSrcNode => SelectedTreeNode?.SrcNode as TreeNode;

        public G2DListSelectTree()
        {
            InitializeComponent();
        }
        public G2DDuplicateTreeNode SetSelected(Predicate<G2DDuplicateTreeNode> selected)
        {
            if (selected != null)
            {
                foreach (TreeNode tn in this.TreeView.GetAllNodes(false))
                {
                    if (tn is G2DDuplicateTreeNode gtn && selected(gtn))
                    {
                        this.TreeView.SelectedNode = gtn;
                        tn.Parent?.Expand();
                        return gtn;
                    }
                }
            }
            return null;
        }
        public G2DDuplicateTreeNode SetSelectedWithPath(string path)
        {
            return SetSelected((tn) => tn.GetSavePath(true) == path);
        }
        public G2DDuplicateTreeNode SetSelectedWithTag(object tag)
        {
            return SetSelected((tn) => tag == tn.Tag);
        }
        public void Init(TreeNodeCollection root, ImageList imageList, Func<TreeNode, G2DDuplicateTreeNode> clone, Predicate<G2DDuplicateTreeNode> selected)
        {
            if (root != null)
            {
                this.TreeView.ImageList = imageList;
                foreach (TreeNode tn in root)
                {
                    G2DDuplicateTreeNode tr = tn.TreeNodeDuplicate(clone);
                    if (tr != null)
                    {
                        this.TreeView.Nodes.Add(tr);
                        this.TreeView.CollapseAll();
                        tr.Expand();
                    }
                }
            }
            SetSelected(selected);
        }
        public void Init(TreeNode root, ImageList imageList, Func<TreeNode, G2DDuplicateTreeNode> clone, Predicate<G2DDuplicateTreeNode> selected)
        {
            this.TreeView.SuspendLayout();
            try
            {
                if (root != null)
                {
                    this.TreeView.ImageList = imageList;
                    G2DDuplicateTreeNode tr = root.TreeNodeDuplicate(clone);
                    if (tr != null)
                    {
                        this.TreeView.Nodes.Add(tr);
                        this.TreeView.CollapseAll();
                        tr.Expand();
                    }
                }
                SetSelected(selected);
            }
            finally
            {
                this.TreeView.SuspendLayout();
            }
        }
        public void Init(TreeNodeCollection root, ImageList imageList, Func<TreeNode, G2DDuplicateTreeNode> clone)
        {
            Init(root, imageList, clone, (tn) => false);
        }
        public void Init(TreeNode root, ImageList imageList, Func<TreeNode, G2DDuplicateTreeNode> clone)
        {
            Init(root, imageList, clone, (tn) => false);
        }
        public void Init(TreeNodeCollection root, ImageList imageList, Func<TreeNode, G2DDuplicateTreeNode> clone, string path)
        {
            Init(root, imageList, clone, (tn) => tn.GetSavePath(true) == path);
        }
        public void Init(TreeNode root, ImageList imageList, Func<TreeNode, G2DDuplicateTreeNode> clone, string path)
        {
            Init(root, imageList, clone, (tn) => tn.GetSavePath(true) == path);
        }

        public void Init(TreeNodeCollection root, ImageList imageList, Predicate<G2DDuplicateTreeNode> selected)
        {
            Init(root, imageList, tn => new G2DDuplicateTreeNode(tn), selected);
        }
        public void Init(TreeNode root, ImageList imageList, Predicate<TreeNode> selected)
        {
            Init(root, imageList, tn => new G2DDuplicateTreeNode(tn), selected);
        }
        public void Init(TreeNodeCollection root, ImageList imageList, string path)
        {
            Init(root, imageList, tn => new G2DDuplicateTreeNode(tn), (tn) => tn.GetSavePath(true) == path);
        }
        public void Init(TreeNode root, ImageList imageList, string path)
        {
            Init(root, imageList, tn => new G2DDuplicateTreeNode(tn), (tn) => tn.GetSavePath(true) == path);
        }
        public void Init(TreeNodeCollection root, ImageList imageList)
        {
            Init(root, imageList, (tn) => false);
        }
        public void Init(TreeNode root, ImageList imageList)
        {
            Init(root, imageList, (tn) => false);
        }
        public void Init(TreeView tree)
        {
            Init(tree.Nodes, tree.ImageList, (tn) => tree.SelectedNode?.Tag == tn.Tag);
        }
        public void Init(TreeView tree, Func<TreeNode, G2DDuplicateTreeNode> clone)
        {
            Init(tree.Nodes, tree.ImageList, clone, (tn) => tree.SelectedNode?.Tag == tn.Tag);
        }
    }
}
