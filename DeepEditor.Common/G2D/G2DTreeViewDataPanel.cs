using DeepCore;
using DeepCore.Concurrent;
using DeepCore.FuncData;
using DeepCore.IO;
using DeepCore.Reflection;
using DeepCore.Reflection.Modeling;
using DeepCore.Xml;
using DeepEditor.Common.G2D.DataGrid;
using DeepEditorConsole;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{

    public partial class G2DTreeViewDataPanel : UserControl, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public DeepEditor.Common.G2D.G2DBaseToolStrip ToolStrip { get => treeViewControl.ToolStrip; }
        public G2DTreeViewControl TreeControl => this.treeViewControl;
        public G2DTreeView TreeView { get => treeViewControl.TreeView; }
        // public DataGrid.G2DPropertyGrid PropertyGrid { get => propertyGrid; }
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem GroupBtn_AddNode { get => groupBtn_AddNode; }
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem GroupBtn_AddGroup { get => groupBtn_AddGroup; }
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem GroupBtn_Rename { get => groupBtn_Rename; }
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem GroupBtn_EditAll { get => groupBtn_EditAll; }
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem GroupBtn_Balance { get => groupBtn_Balance; }
        public DeepEditor.Common.G2D.G2DBaseContextMenuStrip GroupMenu { get => groupMenu; }
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem ChildBtn_SetID { get => childBtn_SetID; }
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem ChildBtn_Duplicate { get => childBtn_Duplicate; }
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem ChildBtn_EditGrid { get => childBtn_EditGrid; }
        public DeepEditor.Common.G2D.G2DBaseToolStripMenuItem ChildBtn_Delete { get => childBtn_Delete; }
        public DeepEditor.Common.G2D.G2DBaseContextMenuStrip ChildMenu { get => childMenu; }
        public System.Windows.Forms.ImageList ImageList { get => imageList; }

        public Size IconSize { get; set; } = new Size(22, 22);

        private Random random = new Random();
        private IExternalizableFactory codec;
        private string categoryText;
        private Type dataType;
        private G2DTreeNodeRoot rootNode;

        public IExternalizableFactory Codec { get => codec; }
        public Type DataType { get => dataType; }
        public G2DTreeNodeRoot TreeRoot { get => rootNode; }
        public string CategoryText { get => categoryText; }
        public string SaveDir { get => rootNode.Dir; }
        public string ImageKey { get; private set; }

        public string GroupImageKey { get; private set; }
        public string ChildImageKey { get; private set; }
        //         public FieldAttributeValue SelectedField
        //         {
        //             get
        //             {
        //                 var grid = this.propertyGrid.SelectedGridItem;
        //                 if (grid != null && grid.PropertyDescriptor is G2DTypeDescriptor.FieldMemberDescriptor fieldDesc)
        //                 {
        //                     return new FieldAttributeValue(fieldDesc.Field, fieldDesc.Desc, fieldDesc.FieldValue, fieldDesc.ComponentData);
        //                 }
        //                 return null;
        //             }
        //             set
        //             {
        //                 if (value != null)
        //                 {
        //                     try
        //                     {
        //                         var g = this.propertyGrid.FindGridItem(grid =>
        //                         {
        //                             if (grid.PropertyDescriptor is G2DTypeDescriptor.FieldMemberDescriptor fieldDesc)
        //                             {
        //                                 if (fieldDesc.Field == value.Field)
        //                                 {
        //                                     fieldDesc.SetValue(grid.Parent.Value, value.FieldValue);
        //                                     return true;
        //                                 }
        //                             }
        //                             return false;
        //                         });
        //                         this.propertyGrid.ExpandTo(g);
        //                         //this.propertyGrid.SelectedGridItem = g;
        //                     }
        //                     catch (Exception ex)
        //                     {
        // 
        //                     }
        //                 }
        //             }
        //         }
        public System.Drawing.Image Image { get; private set; }
        public G2DTreeViewDataPanel()
        {
            InitializeComponent();
            this.BackColor = Color.Transparent;
        }
        public virtual void Init(
            IExternalizableFactory codec,
            Type dataType,
            string category,
            string dir,
            string set_dir,
            ImageList imageList = null,
            string groupImageKey = "",
            string childImageKey = "")
        {
            if (this.codec != null) throw new Exception("Already Init!!!");
            this.codec = codec;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!Directory.Exists(set_dir))
            {
                Directory.CreateDirectory(set_dir);
            }


            this.dataType = dataType;
            this.categoryText = category;

            this.groupBtn_AddNode.Text = ("添加 " + category);

            this.rootNode = CreateRoot(dir, set_dir);
            this.rootNode.ContextMenuStrip = groupMenu;
            this.rootNode.ChildsContextMenuStrip = childMenu;
            this.TreeView.TreeViewNodeSorter = new G2DTreeNodeComparer();
            this.TreeView.Nodes.Add(rootNode);

            if (imageList != null)
            {
                this.imageList = imageList.Clone();
            }
            else
            {
                this.imageList = new ImageList();
            }
            this.imageList.ImageSize = IconSize;
            if (string.IsNullOrEmpty(groupImageKey))
            {
                groupImageKey = "icon_Group";
            }
            if (string.IsNullOrEmpty(childImageKey))
            {
                childImageKey = "icon_Node";
            }
            this.ImageKey = childImageKey;
            this.GroupImageKey = groupImageKey;
            this.ChildImageKey = childImageKey;
            this.TreeView.ImageList = this.imageList;
            this.rootNode.ChildsImageKey = childImageKey;
            this.rootNode.ImageKey = groupImageKey;
            this.TreeView.SelectedImageKey = groupImageKey;
            this.TreeView.ImageKey = groupImageKey;
            this.TreeView.AfterSelect += treeView_AfterSelect;
            this.TreeView.MouseDown += treeView_MouseDown;
            this.TreeView.KeyDown += TreeView_KeyDown;
            this.treeViewControl.DragDropTreeNodeToGroupComplete += TreeViewControl_DragDropTreeNodeToGroupComplete;
            //treeView.ItemDrag += treeView_ItemDrag;
            //treeView.DragDrop += treeView_DragDrop;
            //treeView.DragEnter += treeView_DragEnter;
            //treeView.DragOver += treeView_DragOver;

            this.treeViewControl.EnableDragDrop = true;
            //this.treeViewControl.DraggingNodeType = GetDataNodeType();

            this.Image = this.imageList.Images[childImageKey];
        }

        private void TreeViewControl_DragDropTreeNodeToGroupComplete(object sender, G2DTreeViewControl.DragDropTreeNodeToGroupEvent e)
        {
            foreach (var node in e.GetDraggingNodes())
            {
                if (node is G2DTreeNode g2D)
                {
                    g2D.MarkModified();
                }
            }
            TreeView.Invalidate();
        }

        //-------------------------------------------------------------------------------------------
        virtual public object CreateData()
        {
            return DeepActivator.CreateInstance(dataType);
        }
        virtual protected G2DTreeNodeRoot CreateRoot(string dir, string set_dir)
        {
            return new G2DTreeNodeRoot(categoryText, dir, set_dir);
        }
        //         virtual protected G2DTreeNode CreateDataNode(object data)
        //         {
        //             var ret = new G2DTreeNode(data);
        //             return ret;
        //         }
        virtual protected Type GetDataNodeType()
        {
            return typeof(G2DTreeNode);
        }
        //-------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------
        public int GetTryLoadCount()
        {
            return rootNode.GetTryLoadCount();
        }
        public void LoadAll(IRangeValue progress, LoadingAction loading = null, LoadedAction loaded = null)
        {
            this.rootNode.LoadAll(codec, loading, loaded, progress);
            this.rootNode.Invoke(() =>
            {
                this.rootNode.Expand();
                this.TreeView.Sort();
            });
        }
        public void ReloadAll(IRangeValue progress, LoadingAction loading, LoadedAction loaded)
        {
            this.rootNode.ReloadAll(codec, loading, loaded, progress);
        }
        public void LoadState()
        {
            this.rootNode.LoadState(new TreeStateInfoConfig());
        }
        public void SetEnableDataGrid(bool e)
        {
            groupBtn_Balance.Visible = e;
            groupBtn_EditAll.Visible = e;
            groupBtn_Balance.Enabled = e;
            groupBtn_EditAll.Enabled = e;
        }
        public TreeView GetTreeView()
        {
            return TreeView;
        }

        public void AddChildMenuItem(int index, ToolStripItem append)
        {
            childMenu.Items.Insert(index, append);
        }
        public void AddChildMenuItem(ToolStripItem append)
        {
            childMenu.Items.Add(append);
        }
        public void AddGroupMenuItem(int index, ToolStripItem append)
        {
            groupMenu.Items.Insert(index, append);
        }
        public void AddGroupMenuItem(ToolStripItem append)
        {
            groupMenu.Items.Add(append);
        }


        public void SaveEditorStatus()
        {
            rootNode.SaveState();
        }

        public bool TryGetNodeData(string id, out G2DTreeNode node, out object data)
        {
            var cn = rootNode.FindNode(id);
            if (cn != null)
            {
                node = cn;
                data = cn.Data;
                return true;
            }
            node = null;
            data = null;
            return false;
        }
        public object GetNodeData(string id)
        {
            var cn = rootNode.FindNode(id);
            if (cn != null)
            {
                return cn.Data;
            }
            return null;
        }
        public TreeNode GetNode(string id)
        {
            var cn = rootNode.FindNode(id);
            return cn;
        }
        public object FocusData(string id)
        {
            var tn = GetNode(id);
            if (tn != null)
            {
                TreeView.SelectedNode = tn;
                if (tn is G2DTreeNode g2d)
                {
                    return g2d.Data;
                }
            }
            return null;
        }

        public void ForEachNodes(Action<TreeView, TreeNode> action)
        {
            GetAllDataNode().ForEach(n =>
            {
                action(TreeView, n);
            });
        }

        public List<object> GetAllNodeData()
        {
            var ret = new List<object>();
            foreach (var cn in rootNode.GetG2DList())
            {
                ret.Add(cn.Data);
            }
            return ret;
        }
        public List<G2DTreeNode> GetAllDataNode()
        {
            var ret = new List<G2DTreeNode>();
            foreach (var cn in rootNode.GetG2DList())
            {
                ret.Add(cn);
            }
            return ret;
        }
        public FileInfo SaveAll(IRangeValue progress, string checkDir, SavingAction saving = null, SavedAction saved = null)
        {
            foreach (var node in GetAllDataNode())
            {
                //node.Data.EditorPath = node.FullPath;
                event_OnSetDataTreePath?.Invoke(node);
            }
            rootNode.SaveAll(codec, !string.IsNullOrEmpty(checkDir), saving, saved, progress);
            if (!string.IsNullOrEmpty(checkDir))
            {
                foreach (var node in GetAllDataNode())
                {
                    string srcxml;
                    string retxml;
                    if (!XmlUtil.ValidateBin(node.Data, codec, out srcxml, out retxml))
                    {
                        string sfile = checkDir + $"/{CategoryText}" + "_conflict_" + node.TextID + ".src.txt";
                        string dfile = checkDir + $"/{CategoryText}" + "_conflict_" + node.TextID + ".bin.txt";
                        CFiles.WriteAllText(sfile, srcxml, CUtils.UTF8);
                        CFiles.WriteAllText(dfile, retxml, CUtils.UTF8);
                        Console.WriteLine(checkDir + "/" + node.TextID + ".xml" + " : Save Load 二进制序列化不匹配 ！" +
                            node.Data.GetType() +
                            "\n比较文件已存储到: " + dfile);
                    }
                }
            }
            return GetMd5File();
        }

        public void SaveModified(SavingAction saving = null, SavedAction saved = null)
        {
            foreach (var node in GetAllDataNode())
            {
                if (node is G2DTreeNode g2d && g2d.IsModified)
                {
                    event_OnSetDataTreePath?.Invoke(node);
                }
            }
            rootNode.SaveModified(codec, saving, saved);
            rootNode.SaveState();
        }
        public void SaveNode(G2DTreeNode node, SavingAction saving = null, SavedAction saved = null)
        {
            event_OnSetDataTreePath?.Invoke(node);
            rootNode.SaveOne(node, codec, saving, saved);
        }

        public void RefreshData()
        {
            this.SuspendLayout();
            TreeView.BeginNoDraw();
            try
            {
                if (TreeView.SelectedNode is G2DTreeNode tn)
                {
                    tn.Refresh();
                }
                rootNode.Refresh();
                //propertyGrid.Refresh();
            }
            finally
            {
                TreeView.EndNoDraw();
                this.ResumeLayout();
                TreeView.Invalidate();
            }
        }

        public List<FileInfo> GetSavedFiles()
        {
            return rootNode.ListSavedFiles();
        }
        public FileInfo GetListFile()
        {
            return rootNode.GetListFile();
        }
        public FileInfo GetMd5File()
        {
            return rootNode.GetMd5File();
        }

        public object GetSelectedData()
        {
            var cn = GetChildMenuNode();
            if (cn != null)
            {
                return cn.Data;
            }
            return null;
        }
        //         public object GetSelectedField()
        //         {
        //             return propertyGrid.SelectedGridItem.Value;
        //         }
        public G2DTreeNode GetSelectedNode()
        {
            return GetChildMenuNode();
        }
        public G2DTreeNodeGroup GetSelectedGroup()
        {
            return GetGroupMenuNode();
        }

        private G2DTreeNode GetChildMenuNode()
        {
            TreeNode nd = TreeView.SelectedNode;
            if (nd is G2DTreeNode)
            {
                return nd as G2DTreeNode;
            }
            return null;
        }

        private G2DTreeNodeGroup GetGroupMenuNode()
        {
            TreeNode nd = TreeView.SelectedNode;
            if (nd is G2DTreeNodeGroup)
            {
                return nd as G2DTreeNodeGroup;
            }
            return null;
        }


        public object ShowSelectTemplateDialog(object obj)
        {
            G2DListSelectEditor dialog = new G2DListSelectEditor(
                dataType,
                   this.TreeRoot,
                   TreeView.ImageList,
                   obj);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedTag;
            }
            return null;
        }
        //         public bool ShowSelectTemplateIDDialog(object obj, out int id)
        //         {
        //             G2DListSelectEditor<T> dialog = new G2DListSelectEditor<T>(
        //                 this.TreeRoot, treeView.ImageList, obj);
        //             if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //             {
        //                 if (dialog.SelectedObject != null)
        //                 {
        //                     id = dialog.SelectedObject.TemplateID;
        //                     return true;
        //                 }
        //             }
        //             id = 0;
        //             return false;
        //         }

        public G2DTreeNode AddNodeWithData(G2DTreeNodeGroup parent, object data)
        {
            var tn = TreeRoot.CreateDataNode(data);
            if (rootNode.AddG2DNode(tn, parent))
            {
                parent?.Expand();
                TreeView.SelectedNode = tn;
                TreeView.Invalidate();
                tn.MarkModified();
                return tn;
            }
            return null;
        }
        public G2DTreeNode AddNodeWithData(G2DTreeNodeGroup parent, string id, object data)
        {
            var tn = TreeRoot.CreateDataNode(data);
            tn.SetDataID(id);
            if (rootNode.AddG2DNode(tn, parent))
            {
                parent?.Expand();
                TreeView.SelectedNode = tn;
                TreeView.Invalidate();
                tn.MarkModified();
                return tn;
            }
            return null;
        }
        public G2DTreeNode AddNodeWithData(string path, object data)
        {
            var parent = TreeRoot.GetOrCreateGroup(path);
            return AddNodeWithData(parent, data);
        }
        public G2DTreeNode AddOrUpdateData(string path, string id, object data)
        {
            var exist = TreeRoot.GetNodeWithID(id.ToString());
            if (exist != null)
            {
                exist.SetData(data);
                exist.MarkModified();
                return exist;
            }
            else
            {
                var parent = TreeRoot.GetOrCreateGroup(path);
                return AddNodeWithData(parent, data);
            }
        }
        public void UpdateData(string id, Func<object, object> func)
        {
            var exist = TreeRoot.GetNodeWithID(id.ToString());
            if (exist != null)
            {
                var data = func(exist.Data);
                exist.SetData(data);
                exist.MarkModified();
                TreeView.Invalidate();
            }
        }
        public G2DTreeNode GetDataNodeWithID(string id)
        {
            return TreeRoot.GetNodeWithID(id.ToString());
        }

        private string InternalNewID(G2DTreeNodeGroup group, Func<string, bool> testExist)
        {
            var id = random.Next();
            if (group != null && group.TreeView == this.TreeView)
            {
                var childs = group.GetNodesAs<G2DTreeNode>();
                if (childs.Count > 0)
                {
                    id = 1;
                    foreach (var child in childs)
                    {
                        if (int.TryParse(child.TextID, out var cid))
                        {
                            id = Math.Max(id, cid + 1);
                            while (testExist != null && testExist(id.ToString()))
                            {
                                id++;
                            }
                            while (this.GetDataNodeWithID(id.ToString()) != null)
                            {
                                id++;
                            }
                        }
                    }
                }
                else
                {
                    childs = group.GetAllNodesAs<G2DTreeNode>();
                    if (childs.Count > 0)
                    {
                        id = 1;
                        foreach (var child in childs)
                        {
                            if (int.TryParse(child.TextID, out var cid))
                            {
                                id = Math.Max(id, cid + 1);
                                while (testExist != null && testExist(id.ToString()))
                                {
                                    id++;
                                }
                                while (this.GetDataNodeWithID(id.ToString()) != null)
                                {
                                    id++;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var childs = GetAllDataNode();
                if (childs.Count > 0)
                {
                    id = 1;
                    foreach (var child in childs)
                    {
                        if (int.TryParse(child.TextID, out var cid))
                        {
                            id = Math.Max(id, cid + 1);
                            while (testExist != null && testExist(id.ToString()))
                            {
                                id++;
                            }
                            while (this.GetDataNodeWithID(id.ToString()) != null)
                            {
                                id++;
                            }
                        }
                    }
                }
            }
            while (testExist != null && testExist(id.ToString()))
            {
                id++;
            }
            while (this.GetDataNodeWithID(id.ToString()) != null)
            {
                id++;
            }
            return id.ToString();
        }

        public virtual string NewID(G2DTreeNodeGroup group = null, Func<string, bool> testExist = null)
        {
            return InternalNewID(group, testExist);
        }
        public bool TryNewID(out string id, G2DTreeNodeGroup group = null, Func<string, bool> testExist = null)
        {
            id = G2DTextDialog.Show(InternalNewID(group, testExist), "添加" + categoryText);
            while (!string.IsNullOrEmpty(id))
            {
                if (this.GetDataNodeWithID(id) != null)
                {
                    id = G2DTextDialog.Show(InternalNewID(group, testExist), "添加" + categoryText);
                }
                else
                {
                    break;
                }
            }
            return id != null;
        }
        //-------------------------------------------------------------------------------------------
        #region Events

        public delegate void SetDataTreePathHandler(G2DTreeNode node);
        public delegate void SetIDCompleteHandler(G2DTreeNode node, object srcID, object dstID);

        public delegate bool TryParseTextIDHandler(string textID, out object srcID);
        public event SetDataTreePathHandler OnSetDataTreePath
        {
            add { event_OnSetDataTreePath += value; }
            remove { event_OnSetDataTreePath -= value; }
        }
        public event SetIDCompleteHandler OnSetIDComplete
        {
            add { event_OnSetIDComplete += value; }
            remove { event_OnSetIDComplete -= value; }
        }
        public event TryParseTextIDHandler TryParseTextID
        {
            add { event_TryParseTextID += value; }
            remove { event_TryParseTextID -= value; }
        }

        private SetDataTreePathHandler event_OnSetDataTreePath;
        private SetIDCompleteHandler event_OnSetIDComplete;
        private TryParseTextIDHandler event_TryParseTextID;

        #endregion
        //-------------------------------------------------------------------------------------------

        #region EventHandlers
        protected virtual void showInFolder_Click(object sender, EventArgs e)
        {
            var node = GetChildMenuNode();
            if (node != null)
            {
                var file = new FileInfo(rootNode.GetSaveXmlPath(node));
                Win32.ShowInFolder(file);
            }
        }
        private void groupBtn_AddNode_Click(object sender, EventArgs e)
        {
            G2DTreeNodeGroup parent = GetGroupMenuNode();
            if (parent != null)
            {
                if (TryNewID(out var id, parent))
                {
                    try
                    {
                        var tn = TreeRoot.CreateDataNode(CreateData());
                        tn.SetDataID(id);
                        tn.Refresh();
                        if (rootNode.AddG2DNode(tn, parent))
                        {
                            parent.Expand();
                            tn.MarkModified();
                            TreeView.Sort();
                            TreeView.Invalidate();
                            TreeView.SelectedNode = tn;
                        }
                        else
                        {
                            MessageBox.Show("无法添加");
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
            }
        }
        private void groupBtn_AddGroup_Click(object sender, EventArgs e)
        {
            G2DTreeNodeGroup parent = GetGroupMenuNode();
            if (parent != null)
            {
                parent.TryAddG2DGroupDialog("分组", out var group);
                TreeView.Sort();
            }
        }
        private void groupBtn_Rename_Click(object sender, EventArgs e)
        {
            G2DTreeNodeGroup parent = GetGroupMenuNode();
            if (parent != null)
            {
                string gname = G2DTextDialog.Show(parent.Text, "重命名");
                if (gname != null)
                {
                    parent.Text = gname;
                    foreach (var sub in parent.GetAllNodes())
                    {
                        if (sub is G2DTreeNode g2d)
                        {
                            g2d.MarkModified();
                        }
                    }
                    TreeView.Invalidate();
                    TreeView.Sort();
                }
            }
        }
        private void groupBtn_Balance_Click(object sender, EventArgs e)
        {

            G2DTreeNodeGroup parent = GetGroupMenuNode();
            if (parent != null)
            {
                StringBuilder sb = new StringBuilder();
                List<object> datas = new List<object>();
                foreach (TreeNode tn in parent.Nodes)
                {
                    if (tn is G2DTreeNode g2d)
                    {
                        var data = g2d.Data;
                        datas.Add(data);
                        sb.AppendLine("[" + data.ToString() + "]");
                        g2d.MarkModified();
                    }
                }
                var result = MessageBox.Show(
                    "确定要配平数据?\n" + sb.ToString(),
                    "集合内所有数据结构将会一致！",
                    MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    sb.Clear();

                    int count = 0;
                    foreach (object src in datas)
                    {
                        foreach (object dst in datas)
                        {
                            try
                            {
                                if (UmlUtils.StructEquationBalancer(src, dst))
                                {
                                    sb.AppendLine(src + " -> " + dst);
                                    count++;
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show(err.Message);
                            }
                        }
                    }
                    if (count > 0)
                    {
                        TreeView.Invalidate();
                        MessageBox.Show(string.Format("{0}个数据已配平!\n{1}", count, sb.ToString()));
                    }
                }
            }
        }
        private void group_Delete_Click(object sender, EventArgs e)
        {
            var parent = GetSelectedGroup();
            if (parent != null)
            {
                var result = MessageBox.Show("删除目录下所有数据？", "删除?", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    parent.RemoveFromParent();
                }
            }
        }
        private void childBtn_SetID_Click(object sender, EventArgs e)
        {
            var node = GetChildMenuNode();
            object srcID = node.TextID;
            if (node != null && (event_TryParseTextID == null || event_TryParseTextID.Invoke(node.TextID, out srcID)))
            {
                string id = G2DTextDialog.Show(node.TextID, node.Text);
                object dstID = id;
                if (event_TryParseTextID == null || event_TryParseTextID.Invoke(id, out dstID))
                {
                    try
                    {
                        if (rootNode.SetG2DNodeID(node, id))
                        {
                            event_OnSetIDComplete?.Invoke(node, srcID, dstID);
                            //Editor.Instance.RenameID(dataType, srcID, dstID);
                            node.MarkModified();
                            node.Refresh();
                            TreeView.Sort();
                            TreeView.Invalidate();
                        }
                        else
                        {
                            MessageBox.Show("无法重设ID，有冲突！");
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
            }
        }
        private void childBtn_Duplicate_Click(object sender, EventArgs e)
        {
            var node = GetChildMenuNode();
            if (node != null)
            {
                var copy = TreeView.DuplicateNode(node.Data, node.TextID);
                if (copy != null)
                {
                    if (copy is G2DTreeNode treeNode)
                    {
                        treeNode.MarkModified();
                        treeNode.Refresh();
                    }
                    TreeView.Sort();
                    TreeView.Invalidate();
                }
            }
        }
        private void childBtn_Delete_Click(object sender, EventArgs e)
        {
            var node = GetChildMenuNode();
            if (node != null)
            {
                DialogResult res = MessageBox.Show(
                    "确认删除: " + node.Text,
                    "确认",
                    MessageBoxButtons.OKCancel);
                if (res == DialogResult.OK)
                {
                    node.RemoveFromParent();
                }
            }
        }
        private object copingProperties = null;
        private void childBtn_CopyProperties_Click(object sender, EventArgs e)
        {
            var node = GetChildMenuNode();
            if (node != null)
            {
                copingProperties = XmlUtil.CloneObject(node.Data);
            }
        }

        private void childBtn_PastePorpertie_Click(object sender, EventArgs e)
        {
            var node = GetChildMenuNode();
            if (node != null && copingProperties != null && copingProperties.GetType() == node.Data.GetType())
            {
                var srcID = node.TextID;

                var copy = XmlUtil.CloneObject(copingProperties);
                if (node.Data is IFuncTemplateData srctemp && copy is IFuncTemplateData dsttemp)
                {
                    dsttemp.TemplateName = srctemp.TemplateName;
                    //dsttemp.TemplateID = srctemp.TemplateID;
                }
                node.SetDataID(node.TextID, copy);
                node.Refresh();
            }
        }
        protected virtual void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            var selected = TreeView.GetNodeAt(e.Location);
            TreeView.SelectedNode = selected;
        }
        protected virtual void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }
        protected virtual void TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (GetChildMenuNode() != null)
                {
                    childBtn_Delete_Click(childBtn_Delete, e);
                }
                else if (GetGroupMenuNode() != null)
                {
                    group_Delete_Click(group_Delete, e);
                }
            }
        }


        #endregion

        //-------------------------------------------------------------------------------------------
        #region SendTo
        public bool TrySendTo()
        {
            TreeNode nd = TreeView.SelectedNode;
            if (ShowSelectGroupDialog(out var group))
            {
                if (nd.ContainsChild(group, true))
                {
                    MessageBox.Show($"'{group.Text}'是'{nd.Text}'的孩子！！！");
                }
                else
                {
                    var oldp = nd.Parent;
                    nd.Remove();
                    try
                    {
                        group.Nodes.Add(nd);
                        if (nd is G2DTreeNode ndd)
                        {
                            ndd.MarkModified();
                        }
                        else if (nd.FindNode<G2DTreeNode>(t => t is G2DTreeNode) is G2DTreeNode ndd2)
                        {
                            ndd2.MarkModified();
                        }
                        return true;
                    }
                    catch (Exception err)
                    {
                        err.ShowMessageBox();
                        oldp.Nodes.Add(nd);
                    }
                }
            }
            return false;
        }
        private void groupBtn_SendTo_Click(object sender, EventArgs e)
        {
            TrySendTo();
        }

        private void childBtn_SendTo_Click(object sender, EventArgs e)
        {
            TrySendTo();
        }

        public bool ShowSelectGroupDialog(out G2DTreeNodeGroup group)
        {
            var nodes = this.TreeView.Nodes.TreeNodeDuplicate(t => t is G2DTreeNodeGroup ? new G2DDuplicateTreeNode(t) : null);
            var dialog = new G2DListSelectEditor<G2DTreeNodeGroup>(nodes, TreeView.ImageList);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (dialog.SelectedSrcNode is G2DTreeNodeGroup _group)
                {
                    group = _group;
                    return true;
                }
                if (dialog.SelectedSrcNode?.Parent is G2DTreeNodeGroup _group2)
                {
                    group = _group2;
                    return true;
                }
            }
            group = null;
            return false;
        }

        #endregion
        //-------------------------------------------------------------------------------------------
    }


}
