using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DeepCore.IO;
using System.Text.RegularExpressions;
using DeepCore;

namespace DeepEditor.Common.G2D
{
    public partial class G2DDirectoryTreeView : G2DTreeView
    {
        private Regex filter;
        private DirectoryInfo root;
        private DirectoryTreeNode rootNode;

        public G2DDirectoryTreeView()
        {
            this.InitializeComponent();
        }
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            base.OnDrawNode(e);
        }

        //---------------------------------------------------------------------------------------------------------------

        public DirectoryInfo RootDir
        {
            get { return root; }
        }
        public FileInfo SelectedFile
        {
            get
            {
                var tn = this.SelectedNode as FileTreeNode; ;
                if (tn != null) { return tn.File; }
                return null;
            }
        }
        public DirectoryInfo SelectedDir
        {
            get
            {
                var tn = this.SelectedNode as DirectoryTreeNode; ;
                if (tn != null) { return tn.Dir; }
                return null;
            }
        }

        //---------------------------------------------------------------------------------------------------------------

        public void SetFilter(Regex regex)
        {
            this.filter = regex;
            this.RefreshFiles();
        }
        public void SetDirectory(DirectoryInfo dir)
        {
            this.Nodes.Clear();
            this.root = dir;
            this.rootNode = CreateTreeNode(root) as DirectoryTreeNode;
            this.Nodes.Add(rootNode);
            this.rootNode.Expand();
            this.RefreshFiles();
        }
        public void RefreshFiles()
        {
            this.RefreshFiles(rootNode);
        }

        protected void RefreshFiles(DirectoryTreeNode dirNode)
        {
            CUtils.SyncToDstList<FileSystemInfo, InfoTreeNode, TreeNodeCollection>(
                dirNode.Dir.GetFileSystemInfos(),
                dirNode.Nodes,
                (s, d) => { return s.FullName == d.Info.FullName; },
                (list, e) => { list.Add(CreateTreeNode(e)); },
                (list, e) => { list.Remove(e); });
            foreach (InfoTreeNode sub in new System.Collections.ArrayList(dirNode.Nodes))
            {
                if (IsNodeVisible(sub.Info) == false)
                {
                    dirNode.Nodes.Remove(sub);
                }
                else if (sub is DirectoryTreeNode)
                {
                    RefreshFiles(sub as DirectoryTreeNode);
                }
                else if (sub is FileTreeNode)
                {
                }
            }
            this.Invalidate();
        }
        protected virtual bool IsNodeVisible(FileSystemInfo info)
        {
            if (filter == null || filter.IsMatch(info.FullName))
            {
                return true;
            }
            return false;
        }
        protected virtual InfoTreeNode CreateTreeNode(FileSystemInfo info)
        {
            if (info is DirectoryInfo)
            {
                return new DirectoryTreeNode(info as DirectoryInfo);
            }
            else if (info is FileInfo)
            {
                return new FileTreeNode(info as FileInfo);
            }
            return null;
        }
        //---------------------------------------------------------------------------------------------------------------

        public abstract class InfoTreeNode : TreeNode
        {
            public FileSystemInfo Info { get; private set; }
            internal InfoTreeNode(FileSystemInfo info) : base(info.Name)
            {
                base.ToolTipText = info.FullName;
                this.Info = info;
            }
        }
        public class FileTreeNode : InfoTreeNode
        {
            public FileInfo File { get; private set; }
            internal FileTreeNode(FileInfo file) : base(file)
            {
                this.SelectedImageKey = this.ImageKey = "file";
                this.SelectedImageIndex = this.ImageIndex = 1;
                this.File = file;
            }
        }
        public class DirectoryTreeNode : InfoTreeNode
        {
            public DirectoryInfo Dir { get; private set; }
            internal DirectoryTreeNode(DirectoryInfo dir) : base(dir)
            {
                this.SelectedImageKey = this.ImageKey = "folder";
                this.SelectedImageIndex = this.ImageIndex = 0;
                this.Dir = dir;
            }
        }

    }
}
