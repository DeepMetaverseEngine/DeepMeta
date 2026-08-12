using DeepCore;
using DeepCore.Concurrent;
using DeepCore.FuncData;
using DeepCore.IO;
using DeepCore.Log;
using DeepCore.Reflection;
using DeepCore.Xml;
using DeepEditor.Common;
using DeepEditor.Common.G2D;
using DeepEditorConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DeepEditor.Common.G2D
{
    //------------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------------------


    public delegate void DuplicateAction(string srcPath, object src, string dstPath, object dst);

    public delegate void ErrorAction(Exception err);

    public delegate void NodeModifyAction(G2DTreeView tree, TreeNode tn);

    //------------------------------------------------------------------------------------------------------

    public class G2DTreeNodeBase : TreeNode
    {
        protected static Logger log = new LazyLogger(typeof(G2DTreeNodeBase));
        public string IconName { get; set; }
        public System.Drawing.Image Icon { get; set; }

        public void RemoveFromParent()
        {
            try
            {
                if (Parent != null)
                {
                    Parent.Nodes.Remove(this);
                }
            }
            catch { }
        }
        protected virtual void RefreshData() { }
        public virtual void Refresh()
        {
            RefreshData();
        }
    }
    //------------------------------------------------------------------------------------------------------

    public class G2DTreeNode : G2DTreeNodeBase
    {
        readonly protected static Encoding UTF8 = new UTF8Encoding(false);
        private object mData;
        private Type mDataType;
        public string SavedXmlMD5 { get; private set; }
        public int SavedXmlLength { get; private set; }
        public FileInfo FilePath { get; internal set; }
        public G2DTreeNode(object data)
        {
            this.mData = data;
            this.mDataType = data.GetType();
            this.Text = data.ToString();
            this.Tag = mData;
            this.Name = this.TextID;
        }
        public G2DTreeNode()
        {
        }

        public void Load(Type dataType, byte[] input, IExternalizableFactory factory)
        {
            this.mDataType = dataType;
            this.mData = LoadXml(factory, input, dataType);
            this.Text = mData.ToString();
            this.Tag = mData;
            this.Name = this.TextID;
            this.SavedXmlMD5 = CMD5.CalculateMD5(input);
            this.SavedXmlLength = input.Length;
        }
        public void SetData(object data)
        {
            this.mData = data;
            this.mDataType = data.GetType();
            this.Text = data.ToString();
            this.Tag = mData;
            this.Name = this.TextID;
            this.Refresh();
        }

        public static string GetDataTextID(object data)
        {
            if (data is IFuncTemplateData temp)
            {
                return temp.TemplateID;
            }
            var type = data.GetType();
            var tca = PropertyUtil.GetAttribute<TableClassAttribute>(type);
            if (tca == null)
                throw new Exception($"类型'{type.FullName}'不包含主键'{typeof(TableClassAttribute).Name}'属性");
            FieldInfo fi = type.GetField(tca.PrimaryKey);
            string id = Parser.ObjectToString(fi.GetValue(data));
            return id;
        }
        public object Data { get { return mData; } }
        public string TextID { get { return GetDataTextID(mData); } }
        public string TextName
        {
            get
            {
                if (mData is IFuncTemplateData temp)
                {
                    return temp.TemplateName;
                }
                return mData.ToString();
            }
        }
        public Type DataType { get => mData.GetType(); }
        public object DataID
        {
            get
            {
                var type = mData.GetType();
                var tca = PropertyUtil.GetAttribute<TableClassAttribute>(type);
                if (tca == null)
                    throw new Exception($"类型'{type.FullName}'不包含主键'{typeof(TableClassAttribute).Name}'属性");
                var fi = type.GetField(tca.PrimaryKey);
                return fi.GetValue(mData);
            }
        }

        public Type DataIDType
        {
            get
            {
                var type = mData.GetType();
                var tca = PropertyUtil.GetAttribute<TableClassAttribute>(type);
                if (tca == null)
                    throw new Exception($"类型'{type.FullName}'不包含主键'{typeof(TableClassAttribute).Name}'属性");
                FieldInfo fi = type.GetField(tca.PrimaryKey);
                return fi.FieldType;
            }
        }

        public virtual G2DTreeNode Clone(string newID)
        {
            var newData = XmlUtil.CloneObject(mData);
            var ret = DeepActivator.CreateInstance(GetType(), newData) as G2DTreeNode;
            ret.SetDataID(newID);
            return ret;
        }
        public void SetParent(TreeNode parent)
        {
            if (this.Parent != parent)
            {
                if (this.Parent != null) this.Parent.Nodes.Remove(this);
                parent.Nodes.Add(this);
            }
        }
        sealed public override void Refresh()
        {
            this.Text = mData.ToString();
            base.Refresh();
        }

        public void SetDataID(string id)
        {
            var type = mData.GetType();
            var tca = PropertyUtil.GetAttribute<TableClassAttribute>(type);
            if (tca == null)
                throw new Exception($"类型'{type.FullName}'不包含主键'{typeof(TableClassAttribute).Name}'属性");
            FieldInfo fi = mData.GetType().GetField(tca.PrimaryKey);
            if (Parser.TryStringToObject(id, fi.FieldType, out var tid))
            {
                fi.SetValue(mData, tid);
                this.Text = mData.ToString();
                this.Name = id;
            }
            else
            {
                throw new Exception($"无法转换 {id} 到 {fi.FieldType.Name}");
            }
        }
        public void SetDataID(string id, object mData)
        {
            var type = mData.GetType();
            var tca = PropertyUtil.GetAttribute<TableClassAttribute>(type);
            if (tca == null)
                throw new Exception($"类型'{type.FullName}'不包含主键'{typeof(TableClassAttribute).Name}'属性");
            FieldInfo fi = mData.GetType().GetField(tca.PrimaryKey);
            if (Parser.TryStringToObject(id, fi.FieldType, out var tid))
            {
                fi.SetValue(mData, tid);
                this.mData = mData;
                this.mDataType = mData.GetType();
                this.Text = mData.ToString();
                this.Tag = mData;
                this.Name = this.TextID;
                this.Text = mData.ToString();
                this.Name = id;
            }
            else
            {
                throw new Exception($"无法转换 {id} 到 {fi.FieldType.Name}");
            }
        }


        public byte[] SaveXML(IExternalizableFactory factory)
        {
            var old_md5 = this.SavedXmlMD5;
            byte[] xml_bin = SaveXML(factory, mData);
            this.SavedXmlMD5 = CMD5.CalculateMD5(xml_bin);
            this.SavedXmlLength = xml_bin.Length;
            return xml_bin;
        }
        public byte[] SaveBin(IExternalizableFactory factory)
        {
            return SaveBin(factory, mData);
        }

        public bool IsModified { get; private set; }
        public void MarkModified()
        {
            if (TreeView != null)
            {
                TreeView.Invoke(() =>
                {
                    InternalMarkModified();
                });
            }
            else
            {
                InternalMarkModified();
            }
        }
        internal void AtomicSave(G2DTreeNodeRoot root)
        {
            if (TreeView != null)
            {
                TreeView.Invoke(() =>
                {
                    InternalCleanModified();
                });
            }
            else
            {
                InternalCleanModified();
            }
        }

        private object internal_lock = new object();
        internal void InternalMarkModified()
        {
            lock (internal_lock)
            {
                if (IsModified == false)
                {
                    IsModified = true;
                }
                else
                {
                    return;
                }
            }
            if (TreeView is G2DTreeView g2d)
            {
                g2d.OnNodeModified(this);
            }
        }
        internal void InternalCleanModified()
        {
            lock (internal_lock)
            {
                if (IsModified == true)
                {
                    IsModified = false;
                }
                else
                {
                    return;
                }
            }
            if (TreeView is G2DTreeView g2d)
            {
                try
                {
                    g2d.OnNodeModified(this);
                }
                catch (Exception err)
                {
                    err.PrintStackTrace();
                }
            }
        }


        //--------------------------------------------------------------------------------------------------------------
        public static byte[] SaveXML(IExternalizableFactory factory, object mData)
        {
            //             using (DeepCore.IO.MemoryStream output = new DeepCore.IO.MemoryStream(1024 * 1024))
            //             {
            //                 Type type = mData.GetType();
            //                 XmlDocument doc = new XmlSerializer(false) { Factory = factory }.ObjectToXml(mData);
            //                 XmlWriterSettings settings = new XmlWriterSettings();
            //                 settings.Indent = true;
            //                 settings.Encoding = UTF8;
            //                 using (XmlWriter xml = XmlWriter.Create(output, settings))
            //                 {
            //                     doc.Save(xml);
            //                     xml.Flush();
            //                 }
            //                 output.Flush();
            //                 byte[] xml_bin = output.ToArray();
            //                 return xml_bin;
            //             }
            return XmlUtil.SaveTemplateXML(factory, mData);
        }
        public static byte[] SaveBin(IExternalizableFactory factory, object mData)
        {
            if (mData is ISerializable)
            {
                using (DeepCore.IO.MemoryStream ms = new DeepCore.IO.MemoryStream(1024 * 1024))
                {
                    OutputStream output = new OutputStream(ms, factory);
                    output.PutObj(mData);
                    ms.Flush();
                    byte[] bin = new byte[ms.Position];
                    Array.Copy(ms.GetBuffer(), bin, bin.Length);
                    return bin;
                }
            }
            else
            {
                throw new Exception($"{mData.GetType()} is not a ISerializable");
            }
        }
        public static object LoadXml(IExternalizableFactory factory, byte[] bin, Type type)
        {
            using (XmlReader xml = XmlReader.Create(new DeepCore.IO.MemoryStream(bin)))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xml);
                var data = new XmlSerializer(false) { Factory = factory }.XmlToObject(type, doc);
                return data;
            }
        }
        public static T LoadXml<T>(IExternalizableFactory factory, byte[] bin)
        {
            using (XmlReader xml = XmlReader.Create(new DeepCore.IO.MemoryStream(bin)))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xml);
                var data = new XmlSerializer(false) { Factory = factory }.XmlToObject<T>(doc);
                return data;
            }
        }
        //--------------------------------------------------------------------------------------------------------------
    }

    //------------------------------------------------------------------------------------------------------

    public class G2DTreeNodeGroup : G2DTreeNodeBase
    {
        public G2DTreeNodeGroup(string name)
        {
            this.Text = name;
            this.Name = name;
        }
        public void SetName(string name)
        {
            this.Text = name;
            this.Name = name;
        }

        public bool ContainsNode(TreeNode node)
        {
            return GetAllNodes(false).Contains(node);
        }

        public int GetAllNodesCount()
        {
            using (var list = new ArrayList<TreeNode>())
            {
                G2DTreeNodes.GetAllNodes(this, list);
                return list.Count;
            }
        }
        public List<TreeNode> GetAllNodes(bool sort)
        {
            List<TreeNode> ret = new List<TreeNode>();
            G2DTreeNodes.GetAllNodes(this, ret);
            if (sort) ret.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
            return ret;
        }

        public List<T> GetAllNodesT<T>(bool sort) where T : TreeNode
        {
            List<T> ret = new List<T>();
            var list = GetAllNodes(false);
            foreach (var n in list)
            {
                if (n is T)
                {
                    ret.Add(n as T);
                }
            }
            if (sort) ret.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
            return ret;
        }

        public bool TryAddG2DGroup(string gname, out G2DTreeNodeGroup group)
        {
            if (this.Nodes.ContainsKey(gname) && this.Nodes[gname] is G2DTreeNodeGroup gnode)
            {
                group = gnode;
                return true;
            }
            else
            {
                group = new G2DTreeNodeGroup(gname);
                group.ContextMenuStrip = this.ContextMenuStrip;
                group.SelectedImageKey = this.SelectedImageKey;
                group.ImageKey = this.ImageKey;
                this.Nodes.Add(group);
                return true;
            }
        }
        public bool TryAddG2DGroupDialog(string groupName, out G2DTreeNodeGroup group, string title = "添加分组")
        {
            groupName = CreateDefaultChildName(groupName);
            groupName = G2DTextDialog.Show(groupName, title);
            if (groupName != null)
            {
                groupName = groupName.Trim();
                if (TryAddG2DGroup(groupName, out group))
                {
                    this.TreeView.SelectedNode = group;
                    Expand();
                    return true;
                }
                else if (group != null)
                {
                    this.TreeView.SelectedNode = group;
                    Expand();
                    return true;
                }
                else
                {
                    MessageBox.Show("不能创建分组: " + groupName);
                    return false;
                }
            }
            group = null;
            return false;
        }
        /*
        public string GetTreeInfo()
        {
            var doc = new XmlDocument();
            var e = doc.CreateElement("node");
            doc.AppendChild(e);
            GetTreeInfo(e, this);
            return XmlUtil.ToString(doc);
        }
        public static void GetTreeInfo(XmlElement e, TreeNode node)
        {
            e.SetAttribute("Name", node.Name);
            e.SetAttribute("IsExpanded", node.IsExpanded.ToString());
            e.SetAttribute("Index", node.Index.ToString());
            e.SetAttribute("IsSelected", node.IsSelected.ToString());
            foreach (TreeNode sub in node.Nodes)
            {
                var ts = sub.GetType();
                var xs = e.OwnerDocument.CreateElement("node");
                e.AppendChild(xs);
                GetTreeInfo(xs, sub);
            }
        }
        public void SetTreeInfo(string xmltext, bool removeEmptyGroup = false)
        {
            var doc = XmlUtil.FromString(xmltext);
            var e = doc.DocumentElement;
            SetTreeInfo(e, this, removeEmptyGroup);
        }
        public static void SetTreeInfo(XmlElement e, TreeNode node, bool removeEmptyGroup = false)
        {
            if (e != null && e.GetAttribute("Name") == node.Name)
            {
                bool expand;
                if (XmlUtil.TryGetAttributeAs(e, "IsExpanded", out expand))
                {
                    if (expand)
                    {
                        node.Expand();
                    }
                    else
                    {
                        node.Collapse();
                    }
                }
                int index;
                if (XmlUtil.TryGetAttributeAs(e, "Index", out index))
                {
                    if (node.Index != index)
                    {
                        var parent = node.Parent;
                        parent.Nodes.Remove(node);
                        parent.Nodes.Insert(index, node);
                    }
                }
                bool selected;
                if (XmlUtil.TryGetAttributeAs(e, "IsSelected", out selected))
                {
                    if (selected && node.TreeView != null)
                    {
                        node.TreeView.SelectedNode = node;
                    }
                }
            }
            foreach (TreeNode sub in new ArrayList(node.Nodes))
            {
                if (e != null)
                {
                    var se = XmlUtil.FindChild<XmlElement>(e, (s) => { return s.GetAttribute("Name") == sub.Name; });
                    SetTreeInfo(se, sub, removeEmptyGroup);
                }
                else
                {
                    SetTreeInfo(null, sub, removeEmptyGroup);
                }
            }
            if (e == null && (node is G2DTreeNodeGroup) && node.Nodes.Count == 0)
            {
                if (removeEmptyGroup) node.Remove();
            }
        }
        */

        public G2DTreeNodeGroup GetOrCreateGroup(string path, char splitChar = '/')
        {
            if (!string.IsNullOrEmpty(path))
            {
                string[] paths = path.Split(splitChar);
                G2DTreeNodeGroup node = this;
                foreach (string sub in paths)
                {
                    if (string.IsNullOrEmpty(sub))
                    {
                        continue;
                    }
                    else if (node.TryFindNodeByText<G2DTreeNodeGroup>(sub, out var tn, false))
                    {
                        node = tn;
                    }
                    else if (node.TryAddG2DGroup(sub, out tn))
                    {
                        node = tn;
                    }
                    else if (tn != null)
                    {
                        node = tn;
                    }
                    else
                    {
                        MessageBox.Show("不能创建分组: " + sub);
                    }
                }
                return node;
            }
            return this;
        }

        public string CreateDefaultChildName(string name)
        {
            int index = 1;
            var dname = name;
            do
            {
                if (!this.Nodes.ContainsKey(dname)) { return dname; }
                dname = name + index.ToString();
                index++;
            } while (true);
        }

        public int GetDataCount(TreeNode node)
        {
            int ret = 0;
            foreach (TreeNode tn in node.Nodes)
            {
                if (tn is G2DTreeNode)
                {
                    ret++;
                }
                else if (tn.Nodes.Count > 0)
                {
                    ret += GetDataCount(tn);
                }
            }
            return ret;
        }

        public string GetSavePath(TreeNode node, bool include_node_name = false)
        {
            return G2DTreeNodes.GetSavePath(this, node, include_node_name);
        }
    }

    //------------------------------------------------------------------------------------------------------

    public class G2DTreeNodeRoot : G2DTreeNodeGroup
    {
        public static event ErrorAction OnError;


        public event LoadingAction OnLoading;
        public event LoadedAction OnLoaded;
        public event SavingAction OnSaving;
        public event SavedAction OnSaved;
        public event DuplicateAction OnDuplicate;

        public static bool ENABLE_BINARY = true;
        readonly private static Encoding UTF8 = new UTF8Encoding(false);
        readonly private string dir;
        readonly private string setting_dir;
        private HashMap<string, byte[]> savedBin = new HashMap<string, byte[]>();
        private HashMap<string, string> savedMd5 = new HashMap<string, string>();
        private HashMap<string, int> savedSize = new HashMap<string, int>();

        public ContextMenuStrip ChildsContextMenuStrip;
        public string ChildsImageKey;
        public string Dir { get { return dir; } }
        public string SettingDir { get { return setting_dir; } }

        public G2DTreeNodeRoot(string name, string dir, string set_dir)
            : base(name)
        {
            this.dir = dir;
            this.setting_dir = set_dir;
        }
        delegate void Delegate0();

        public bool AddG2DNode(G2DTreeNode tn, TreeNode parent = null)
        {
            if (!ContainsNode(tn) && !ContainsG2DNodeID(tn.TextID))
            {
                if (parent == null || !ContainsNode(parent))
                {
                    parent = this;
                }
                tn.ImageKey = ChildsImageKey;
                tn.SelectedImageKey = ChildsImageKey;
                tn.ContextMenuStrip = ChildsContextMenuStrip;
                parent.Nodes.Add(tn);
                return true;
            }
            return false;
        }

        private void AddG2DNode(G2DTreeNode tn, string path)
        {
            TreeNode parent = GetOrCreateGroup(path);
            tn.ImageKey = ChildsImageKey;
            tn.SelectedImageKey = ChildsImageKey;
            tn.ContextMenuStrip = ChildsContextMenuStrip;
            parent.Nodes.Add(tn);
        }
        private bool SetG2DNodePath(G2DTreeNode tn, string path)
        {
            TreeNode parent = GetOrCreateGroup(path);
            if (tn.Parent != parent)
            {
                tn.RemoveFromParent();
                parent.Nodes.Add(tn);
            }
            return true;
        }

        public void Invoke(Action action)
        {
            if (this.TreeView != null && this.TreeView.InvokeRequired)
            {
                this.TreeView.Invoke(new Delegate0(() =>
                {
                    action.Invoke();
                }));
            }
            else
            {
                action.Invoke();
            }
        }

        public bool SetG2DNodeID(G2DTreeNode tn, string id)
        {
            var ret = GetG2DList();
            if (ret.Contains(tn))
            {
                foreach (var node in ret)
                {
                    if (node.TextID.Equals(id))
                    {
                        return false;
                    }
                }
                tn.SetDataID(id);
                return true;
            }
            return false;
        }
        public G2DTreeNode GetNodeWithID(string id)
        {
            var ret = GetG2DList();
            foreach (var node in ret)
            {
                if (node.TextID.Equals(id))
                {
                    return node;
                }
            }
            return null;
        }
        public bool ContainsG2DNodeID(string id)
        {
            var ret = GetG2DList();
            foreach (var node in ret)
            {
                if (node.TextID.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }
        public int GetDataCount()
        {
            return GetDataCount(this);
        }

        public List<G2DTreeNode> GetG2DList()
        {
            var ret = new List<G2DTreeNode>();
            GetG2DList(this, ret);
            return ret;
        }

        private void GetG2DList(TreeNode node, List<G2DTreeNode> ret)
        {
            foreach (TreeNode tn in node.Nodes)
            {
                if (tn is G2DTreeNode)
                {
                    ret.Add(tn as G2DTreeNode);
                }
                else if (tn.Nodes.Count > 0)
                {
                    GetG2DList(tn, ret);
                }
            }
        }


        public G2DTreeNode FindNode(string id)
        {
            var ret = GetG2DList();
            foreach (var node in ret)
            {
                if (node.TextID.Equals(id))
                {
                    return node;
                }
            }
            return null;
        }
        public G2DTreeNode FindNode<ST>(ST st, BreakPredicate<ST, G2DTreeNode> func)
        {
            var ret = GetG2DList();
            foreach (var node in ret)
            {
                if (func(st, node))
                {
                    return node;
                }
            }
            return null;
        }

        public override void Refresh()
        {
            base.Refresh();
            var ret = GetG2DList();
            foreach (var node in ret)
            {
                node.Refresh();
            }
        }

        public virtual string GetSaveXmlPath(G2DTreeNode sub)
        {
            string xmlpath = dir + "/" + sub.TextID + ".xml";
            return xmlpath;
        }

        public virtual G2DTreeNode CreateDataNode(object data)
        {
            var ret = new G2DTreeNode(data);
            return ret;
        }
        public virtual object CreateNodeData(byte[] bin, IExternalizableFactory factory)
        {
            return G2DTreeNode.LoadXml(factory, bin, null);
        }




        protected virtual void GenMD5(G2DTreeNode sub)
        {
            savedMd5.Put(sub.TextID, sub.SavedXmlMD5);
            savedSize.Put(sub.TextID, sub.SavedXmlLength);
        }


        public void LoadState(TreeStateInfoConfig cfg)
        {
            try
            {
                if (setting_dir != null)
                {
                    if (File.Exists(setting_dir + ListTreeExt))
                    {
                        this.SetTreeInfo(File.ReadAllText(setting_dir + ListTreeExt, UTF8), cfg);
                    }
                }
            }
            catch (Exception) { }
        }
        public void SaveState()
        {
            if (setting_dir != null)
            {
                CFiles.CreateDir(setting_dir);
                File.WriteAllText(setting_dir + ListTreeExt, this.GetTreeInfo(), UTF8);
            }
        }
        public const string ListFileExt = "/dir.list";
        public const string ListTreeExt = "/dir.tree";
        public const string ListMd5Ext = "/dir.md5";

        public delegate bool TryGetPathHandler(G2DTreeNode node, out string path);
        public event TryGetPathHandler TryLoadListPath;
        public delegate bool TrySetPathHandler(G2DTreeNode node, out string path);
        public event TrySetPathHandler TrySaveListPath;

        public int GetTryLoadCount()
        {
            //             if (File.Exists(dir + ListFileExt))
            //             {
            //                 string[] list = File.ReadAllLines(dir + ListFileExt, UTF8);
            //                 return list.Length;
            //             }
            return Directory.GetFiles(dir).Length;
        }
        public void LoadAll(IExternalizableFactory factory, LoadingAction loading = null, LoadedAction loaded = null, IRangeValue progress = null)
        {
            if (File.Exists(dir + ListFileExt))
            {
                var list = File.ReadAllLines(dir + ListFileExt, UTF8);
                var idpath = new HashMap<string, string>();
                foreach (string sub in list)
                {
                    try
                    {
                        string[] kv = sub.Split(';');
                        idpath.Add(kv[1], kv[0]);
                    }
                    catch (Exception) { }
                }
                foreach (string subname in Directory.GetFiles(dir))
                {
                    var sub = new FileInfo(subname);
                    if (sub.Extension.EndsWith(".xml"))
                    {
                        if (progress != null) progress.SetText(sub.FullName);
                        loading?.Invoke(sub);
                        var tn = AtomicLoad(sub, factory, loaded);
                        if (tn != null)
                        {
                            string id = tn.TextID;
                            string path = idpath.Get(id);
                            if (TryLoadListPath != null && TryLoadListPath.Invoke(tn, out var _path))
                            {
                                path = _path;
                            }
                            this.Invoke(() =>
                            {
                                AddG2DNode(tn, path);
                            });
                        }
                    }
                    if (progress != null) progress.Add(1);
                }

            }
        }
        public void ReloadAll(IExternalizableFactory factory, LoadingAction loading = null, LoadedAction loaded = null, IRangeValue progress = null)
        {
            if (File.Exists(dir + ListFileExt))
            {
                var list = File.ReadAllLines(dir + ListFileExt, UTF8);
                var idpath = new HashMap<string, string>();
                foreach (string sub in list)
                {
                    try
                    {
                        string[] kv = sub.Split(';');
                        idpath.Add(kv[1], kv[0]);
                    }
                    catch (Exception) { }
                }
                foreach (var subname in Directory.GetFiles(dir))
                {
                    var sub = new FileInfo(subname);
                    sub.Refresh();
                    if (sub.Extension.EndsWith(".xml"))
                    {
                        if (progress != null) progress.SetText(sub.FullName);
                        loading?.Invoke(sub);
                        var exist = FindNode(sub, (sub, node) => CFiles.FileEquals(node.FilePath, sub));
                        if (exist != null)
                        {
                            AtomicReload(sub, exist, factory, loaded);
                            if (exist != null)
                            {
                                string id = exist.TextID;
                                string path = idpath.Get(id);
                                if (TryLoadListPath != null && TryLoadListPath.Invoke(exist, out var _path))
                                {
                                    path = _path;
                                }
                                this.Invoke(() =>
                                {
                                    SetG2DNodePath(exist, path);
                                });
                            }
                        }
                        else
                        {
                            var tn = AtomicLoad(sub, factory, loaded);
                            if (tn != null)
                            {
                                string id = tn.TextID;
                                string path = idpath.Get(id);
                                if (TryLoadListPath != null && TryLoadListPath.Invoke(tn, out var _path))
                                {
                                    path = _path;
                                }
                                this.Invoke(() =>
                                {
                                    AddG2DNode(tn, path);
                                });
                            }
                        }

                    }
                    if (progress != null) progress.Add(1);
                }

            }
        }

        public List<G2DTreeNode> SaveList()
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var ret = GetG2DList();
            StringBuilder savelist = new StringBuilder();
            ret.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
            foreach (var sub in ret)
            {
                var path = GetSavePath(sub);
                if (TrySaveListPath != null && TrySaveListPath.Invoke(sub, out var _path))
                {
                    path = _path;
                }
                savelist.AppendLine(path + ";" + sub.TextID);
            }
            File.WriteAllText(dir + ListFileExt, savelist.ToString(), UTF8);
            Invoke(() =>
            {
                SaveState();
            });
            return ret;
        }

        public FileInfo GetListFile()
        {
            return new FileInfo(dir + ListFileExt);
        }
        public FileInfo GetMd5File()
        {
            return new FileInfo(dir + ListMd5Ext);
        }
        public List<FileInfo> ListSavedFiles()
        {
            var list = GetG2DList();
            var ret = new List<FileInfo>(list.Count);
            foreach (var sub in list)
            {
                string path = GetSaveXmlPath(sub);
                ret.Add(new FileInfo(path));
            }
            ret.Add(new FileInfo(dir + ListFileExt));
            return ret;
        }
        public void SaveModified(IExternalizableFactory factory, SavingAction saving, SavedAction saved)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var ret = SaveList();
            try
            {
                foreach (var node in ret)
                {
                    if (node is G2DTreeNode g2D && g2D.IsModified)
                    {
                        AtomicSave(g2D, factory, true, saving, saved);
                    }
                }
            }
            catch (Exception err)
            {
                log.Error(err);
                OnError?.Invoke(err);
            }
        }
        public void SaveOne(G2DTreeNode node, IExternalizableFactory factory, SavingAction saving, SavedAction saved)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var ret = SaveList();
            try
            {
                AtomicSave(node, factory, true, saving, saved);
            }
            catch (Exception err)
            {
                log.Error(err);
                OnError?.Invoke(err);
            }
        }
        public void SaveAll(IExternalizableFactory factory, bool force, SavingAction saving, SavedAction saved, IRangeValue progress = null)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var ret = SaveList();
            var savedfiles = new HashMap<string, G2DTreeNode>();
            savedfiles.Add("dir", null);
            foreach (var sub in ret)
            {
                try
                {
                    if (progress != null) progress.Text = $"保存:{sub.Data}";
                    savedfiles.Add(sub.TextID, sub);
                    AtomicSave(sub, factory, force, saving, saved);
                }
                catch (Exception err)
                {
                    log.Error(err);
                    OnError?.Invoke(err);
                }
                finally
                {
                    if (progress != null) progress.Add(1);
                }
            }
            // clean
            foreach (string sub in Directory.GetFiles(dir))
            {
                string filename = CFiles.GetFileNameWithoutExtensions(sub);
                if (!savedfiles.ContainsKey(filename))
                {
                    FileSystem.DeleteToRecycleBin(sub);
                }
            }
            // save md5
            try
            {
                StringBuilder sb = new StringBuilder();
                ret.Sort((a, b) =>
                {
                    if (a.DataID is IComparable ac && b.DataID is IComparable bc)
                    {
                        return ac.CompareTo(bc);
                    }
                    return a.TextID.CompareTo(b.TextID);
                });
                foreach (var sub in ret)
                {
                    string md5 = savedMd5.Get(sub.TextID);
                    int size = savedSize.Get(sub.TextID);
                    sb.AppendLine(string.Format(string.Format("{0} : {1,12} : {2}", md5, size, sub.TextID + ".xml")));
                }
                File.WriteAllText(dir + ListMd5Ext, sb.ToString(), UTF8);
            }
            catch (Exception err)
            {
                log.Error(err);
                OnError?.Invoke(err);
            }
        }



        //------------------------------------------------------------------------------------------------
        protected virtual void AtomicSave(
            G2DTreeNode sub,
            IExternalizableFactory factory,
            bool force,
            SavingAction saving,
            SavedAction saved)
        {
            try
            {

                var xmlpath = new FileInfo(GetSaveXmlPath(sub));
                sub.FilePath = xmlpath;
                saving?.Invoke(xmlpath, sub.Data);
                OnSaving?.Invoke(xmlpath, sub.Data);
                if (ENABLE_BINARY)
                {
                    var binpath = new FileInfo(xmlpath.FullName + ".bin");
                    byte[] bin = sub.SaveBin(factory);
                    if (bin != null)
                    {
                        if (!force)
                        {
                            byte[] oldbin = savedBin.Get(sub.TextID);
                            if (oldbin != null && CUtils.ArraysEqual(oldbin, bin))
                            {
                                saved?.Invoke(xmlpath, sub.Data);
                                return;
                            }
                        }
                        savedBin.Put(sub.TextID, bin);
                        File.WriteAllBytes(binpath.FullName, bin);
                    }
                }
                var old_md5 = sub.SavedXmlMD5;
                byte[] xml = sub.SaveXML(factory);
                FileSystemWorkSpace.WriteAllBytes(xmlpath, xml);
                GenMD5(sub);
                saved?.Invoke(xmlpath, sub.Data);
                OnSaved?.Invoke(xmlpath, sub.Data);
            }
            finally
            {
                sub.AtomicSave(this);
            }
        }

        protected virtual G2DTreeNode AtomicLoad(FileInfo path, IExternalizableFactory factory, LoadedAction loaded = null)
        {
            OnLoading?.Invoke(path);
            byte[] xml = FileSystemWorkSpace.ReadAllBytes(path);
            try
            {
                var nodeData = CreateNodeData(xml, factory);
                var node = CreateDataNode(nodeData);
                if (node != null)
                {
                    node.FilePath = path;
                    loaded?.Invoke(path, nodeData);
                    OnLoaded?.Invoke(path, node.Data);
                    node.Refresh();
                    return node;
                }
            }
            catch (Exception err)
            {
                log.Error("Load File Error : " + path, err);
                OnError?.Invoke(err);
                //MessageBox.Show(err.Message);
            }
            return null;
        }
        protected virtual void AtomicReload(FileInfo path, G2DTreeNode node, IExternalizableFactory factory, LoadedAction loaded = null)
        {
            OnLoading?.Invoke(path);
            byte[] xml = FileSystemWorkSpace.ReadAllBytes(path);
            try
            {
                var nodeData = CreateNodeData(xml, factory);
                if (nodeData != null)
                {
                    node.FilePath = path;
                    loaded?.Invoke(path, nodeData);
                    OnLoaded?.Invoke(path, nodeData);
                    var tree = this.TreeView;
                    if (tree != null)
                    {
                        tree.Invoke(() =>
                        {
                            node.SetData(nodeData);
                        });
                    }
                    else
                    {
                        node.SetData(nodeData);
                    }
                }
            }
            catch (Exception err)
            {
                log.Error("Load File Error : " + path, err);
                OnError?.Invoke(err);
                //MessageBox.Show(err.Message);
            }
        }

        //------------------------------------------------------------------------------------------------
    }

    //------------------------------------------------------------------------------------------------------


    public class G2DTreeNode<T> : G2DTreeNode
    {
        public G2DTreeNode(T data) : base(data) { }
        public G2DTreeNode() { }
        //public G2DTreeNode(byte[] input, IExternalizableFactory factory) : base(typeof(T), input, factory) { }
        new public T Data { get { return (T)base.Data; } }
        public override G2DTreeNode Clone(string newID)
        {
            T newData = XmlUtil.CloneObject<T>(Data);
            G2DTreeNode<T> ret = new G2DTreeNode<T>(newData);
            ret.SetDataID(newID);
            return ret;
        }
    }

    public class G2DTreeNodeRoot<T> : G2DTreeNodeRoot where T : class, new()
    {
        public G2DTreeNodeRoot(string name, string dir, string set_dir)
            : base(name, dir, set_dir)
        {
        }
        public override G2DTreeNode CreateDataNode(object data)
        {
            var ret = new G2DTreeNode<T>((T)data);
            return ret;
        }
        public override object CreateNodeData(byte[] bin, IExternalizableFactory factory)
        {
            return G2DTreeNode.LoadXml(factory, bin, typeof(T));
        }
        //         sealed public override G2DTreeNode CreateNodeData(byte[] bin, IExternalizableFactory factory)
        //         {
        //             var mData = G2DTreeNode.LoadXml(factory, bin, typeof(T));
        //             return CreateDataNode(mData);
        //         }
        new public List<G2DTreeNode<T>> GetG2DList()
        {
            return base.GetG2DList().ConvertAll(a => (G2DTreeNode<T>)a);
        }
        new public G2DTreeNode<T> FindNode(string id)
        {
            return base.FindNode(id) as G2DTreeNode<T>;
        }
        new public List<G2DTreeNode<T>> SaveList()
        {
            return base.SaveList().ConvertAll(a => (G2DTreeNode<T>)a);
        }
        //         public void SaveOne(T data, IExternalizableFactory factory, SavingAction saving, SavedAction saved)
        //         {
        //             base.SaveOne(data, factory,
        //                 (a) => saving?.Invoke((T)a),
        //                 (b, src, dst) => saved?.Invoke((T)b, src, dst));
        //         }
        //         public void SaveAll(IExternalizableFactory factory, bool force, AtomicInteger progress, SavingAction saving, SavedAction saved)
        //         {
        //             base.SaveAll(factory, force, progress,
        //                 (a) => saving?.Invoke((T)a),
        //                 (b, src, dst) => saved?.Invoke((T)b, src, dst));
        //         }
    }

    //------------------------------------------------------------------------------------------------------

    public class G2DDuplicateTreeNode : TreeNode
    {
        public TreeNode SrcNode { get; }
        public G2DDuplicateTreeNode(TreeNode tn) : base(tn.Text)
        {
            this.SrcNode = tn;
            this.SelectedImageKey = tn.SelectedImageKey;
            this.SelectedImageIndex = tn.SelectedImageIndex;
            this.ImageIndex = tn.ImageIndex;
            this.ImageKey = tn.ImageKey;
            this.Tag = tn.Tag;
            this.ForeColor = tn.ForeColor;
            this.BackColor = tn.BackColor;
        }

    }

    //------------------------------------------------------------------------------------------------------
    public class G2DTreeNodeComparer : IComparer<TreeNode>, IComparer
    {
        public int Compare(TreeNode x, TreeNode y)
        {
            if (x is G2DTreeNodeGroup && y is G2DTreeNodeGroup)
            {
                return x.Text.CompareTo(y.Text);
            }
            if (x is G2DTreeNodeGroup)
            {
                return -1;
            }
            if (y is G2DTreeNodeGroup)
            {
                return 1;
            }
            if (x is G2DTreeNode tx && y is G2DTreeNode ty)
            {
                if (tx.DataID is IComparable ta && ty.DataID is IComparable tb)
                {
                    if (ta != null && tb != null)
                    {
                        return ta.CompareTo(tb);
                    }
                }
                return tx.TextID.CompareTo(ty.TextID);
            }
            return x.Text.CompareTo(y.Text);
        }

        public int Compare(object x, object y)
        {
            return Compare((TreeNode)x, (TreeNode)y);
        }
    }
    //------------------------------------------------------------------------------------------------------
    public struct TreeStateInfoConfig
    {
        public bool removeEmptyGroup = false;
        public bool reIndex = false;
        public bool select = false;
        public TreeStateInfoConfig()
        {
        }
    }
    //------------------------------------------------------------------------------------------------------
}

namespace System.Windows.Forms
{
    public static class G2DTreeNodes
    {
        //-------------------------------------------------------------------------------------------------------
        #region TreeStateInfo -------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------
        public static XmlDocument GetTreeInfoXML(this TreeView node)
        {
            var doc = new XmlDocument();
            var e = doc.CreateElement("tree");
            doc.AppendChild(e);
            foreach (TreeNode sub in node.Nodes)
            {
                var ts = sub.GetType();
                var xs = e.OwnerDocument.CreateElement("node");
                e.AppendChild(xs);
                GetTreeInfoXML(sub, xs);
            }
            return doc;
        }
        public static XmlDocument GetTreeInfoXML(this TreeNode node)
        {
            var doc = new XmlDocument();
            var e = doc.CreateElement("node");
            doc.AppendChild(e);
            GetTreeInfoXML(node, e);
            return doc;
        }
        public static string GetTreeInfo(this TreeView node)
        {
            return XmlUtil.ToString(GetTreeInfoXML(node));
        }
        public static string GetTreeInfo(this TreeNode node)
        {
            return XmlUtil.ToString(GetTreeInfoXML(node));
        }
        public static void GetTreeInfoXML(this TreeNode node, XmlElement e)
        {
            e.SetAttribute("Name", node.Name);
            e.SetAttribute("IsExpanded", node.IsExpanded.ToString());
            e.SetAttribute("Index", node.Index.ToString());
            e.SetAttribute("IsSelected", node.IsSelected.ToString());
            foreach (TreeNode sub in node.Nodes)
            {
                var ts = sub.GetType();
                var xs = e.OwnerDocument.CreateElement("node");
                e.AppendChild(xs);
                GetTreeInfoXML(sub, xs);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------
        public static void SetTreeInfo(this TreeView node, XmlDocument doc, TreeStateInfoConfig cfg)
        {
            var e = doc.DocumentElement;
            foreach (TreeNode sub in new ArrayList(node.Nodes))
            {
                if (e != null)
                {
                    var se = XmlUtil.FindChild<XmlElement>(e, (s) => { return s.GetAttribute("Name") == sub.Name; });
                    SetTreeInfoXML(sub, se, cfg);
                }
                else
                {
                    SetTreeInfoXML(sub, null, cfg);
                }
            }
        }
        public static void SetTreeInfo(this TreeNode node, XmlDocument doc, TreeStateInfoConfig cfg)
        {
            var e = doc.DocumentElement;
            SetTreeInfoXML(node, e, cfg);
        }
        public static void SetTreeInfo(this TreeView node, string xmltext, TreeStateInfoConfig cfg)
        {
            var doc = XmlUtil.FromString(xmltext);
            SetTreeInfo(node, doc, cfg);
        }
        public static void SetTreeInfo(this TreeNode node, string xmltext, TreeStateInfoConfig cfg)
        {
            var doc = XmlUtil.FromString(xmltext);
            SetTreeInfo(node, doc, cfg);
        }
        public static void SetTreeInfoXML(this TreeNode node, XmlElement e, TreeStateInfoConfig cfg)
        {
            if (e != null && e.GetAttribute("Name") == node.Name)
            {
                if (XmlUtil.TryGetAttributeAs(e, "IsExpanded", out bool expand))
                {
                    if (expand)
                    {
                        if (!node.IsExpanded)
                            node.Expand();
                    }
                    else
                    {
                        if (node.IsExpanded)
                            node.Collapse();
                    }
                }
                if (cfg.reIndex)
                {
                    int index;
                    if (XmlUtil.TryGetAttributeAs(e, "Index", out index))
                    {
                        if (node.Index != index)
                        {
                            var parent = node.Parent;
                            if (parent != null)
                            {
                                parent.Nodes.Remove(node);
                                parent.Nodes.Insert(index, node);
                            }
                        }
                    }
                }
                if (cfg.select)
                {
                    bool selected;
                    if (XmlUtil.TryGetAttributeAs(e, "IsSelected", out selected))
                    {
                        if (selected && node.TreeView != null)
                        {
                            node.TreeView.SelectedNode = node;
                        }
                    }
                }
            }
            foreach (TreeNode sub in new ArrayList(node.Nodes))
            {
                if (e != null)
                {
                    var se = XmlUtil.FindChild<XmlElement>(e, (s) => { return s.GetAttribute("Name") == sub.Name; });
                    SetTreeInfoXML(sub, se, cfg);
                }
                else
                {
                    SetTreeInfoXML(sub, null, cfg);
                }
            }
            if (e == null && (node is G2DTreeNodeGroup) && node.Nodes.Count == 0)
            {
                if (cfg.removeEmptyGroup) node.Remove();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------

        public static string SaveState(this TreeView node)
        {
            try
            {
                return node.GetTreeInfo();
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
                throw;
            }
        }
        public static void LoadState(this TreeView node, string state, TreeStateInfoConfig cfg)
        {
            try
            {
                node.SetTreeInfo(state, cfg);
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        public static void SaveState(this TreeView node, FileInfo file)
        {
            try
            {
                CFiles.CreateFile(file);
                File.WriteAllText(file.FullName, node.GetTreeInfo(), CUtils.UTF8);
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        public static void SaveState(this TreeNode node, FileInfo file)
        {
            try
            {
                CFiles.CreateFile(file);
                File.WriteAllText(file.FullName, node.GetTreeInfo(), CUtils.UTF8);
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        public static void LoadState(this TreeView node, FileInfo file, TreeStateInfoConfig cfg)
        {
            try
            {
                if (File.Exists(file.FullName))
                {
                    node.SetTreeInfo(File.ReadAllText(file.FullName, CUtils.UTF8), cfg);
                }
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }
        public static void LoadState(this TreeNode node, FileInfo file, TreeStateInfoConfig cfg)
        {
            try
            {
                if (File.Exists(file.FullName))
                {
                    node.SetTreeInfo(File.ReadAllText(file.FullName, CUtils.UTF8), cfg);
                }
            }
            catch (Exception err) { err.PrintStackTrace(); }
        }

        #endregion
        //------------------------------------------------------------------------------------------------------------------------------------------------
        public static T FindNode<T>(this TreeNode parent, Predicate<T> text, bool searchAllChildren = true) where T : TreeNode
        {
            if (parent != null)
                return FindNode<T>(parent.Nodes, text, searchAllChildren);
            return default(T);
        }
        public static bool TryFindNode<T>(this TreeNode parent, Predicate<T> text, out T tnode, bool searchAllChildren = true) where T : TreeNode
        {
            if (parent != null)
                return TryFindNode(parent.Nodes, text, out tnode, searchAllChildren);
            tnode = default(T);
            return false;
        }
        public static T FindNodeByText<T>(this TreeNode parent, string text, bool searchAllChildren = true) where T : TreeNode
        {
            if (parent != null)
                return FindNodeByText<T>(parent.Nodes, text, searchAllChildren);
            return default(T);
        }
        public static bool TryFindNodeByText<T>(this TreeNode parent, string text, out T tnode, bool searchAllChildren = true) where T : TreeNode
        {
            if (parent != null)
                return TryFindNodeByText<T>(parent.Nodes, text, out tnode, searchAllChildren);
            tnode = default(T);
            return false;
        }
        public static bool ContainsChild(this TreeNode parent, TreeNode node, bool searchAllChildren = true)
        {
            return ContainsChild(parent.Nodes, node, searchAllChildren);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------
        public static T FindNode<T>(this TreeNodeCollection parent, Predicate<T> text, bool searchAllChildren = true) where T : TreeNode
        {
            foreach (TreeNode tn in parent)
            {
                if (tn is T t && text(t))
                {
                    return t;
                }
                if (searchAllChildren)
                {
                    T stn = FindNode<T>(tn.Nodes, text, true);
                    if (stn != null)
                    {
                        return (T)stn;
                    }
                }
            }
            return default(T);
        }
        public static bool TryFindNode<T>(this TreeNodeCollection parent, Predicate<T> text, out T tnode, bool searchAllChildren = true) where T : TreeNode
        {
            foreach (TreeNode tn in parent)
            {
                if (tn is T t && text(t))
                {
                    tnode = t;
                    return true;
                }
                if (searchAllChildren)
                {
                    T stn = FindNode<T>(tn.Nodes, text, true);
                    if (stn != null)
                    {
                        tnode = (T)stn;
                        return true;
                    }
                }
            }
            tnode = default(T);
            return false;
        }
        public static T FindNodeByText<T>(this TreeNodeCollection parent, string text, bool searchAllChildren = true) where T : TreeNode
        {
            foreach (TreeNode tn in parent)
            {
                if (typeof(T).IsAssignableFrom(tn.GetType()) && tn.Text.Equals(text))
                {
                    return (T)tn;
                }
                if (searchAllChildren)
                {
                    T stn = FindNodeByText<T>(tn.Nodes, text, true);
                    if (stn != null)
                    {
                        return (T)stn;
                    }
                }
            }
            return default(T);
        }
        public static bool TryFindNodeByText<T>(this TreeNodeCollection parent, string text, out T tnode, bool searchAllChildren = true) where T : TreeNode
        {
            foreach (TreeNode tn in parent)
            {
                if (typeof(T).IsAssignableFrom(tn.GetType()) && tn.Text.Equals(text))
                {
                    tnode = (T)tn;
                    return true;
                }
                if (searchAllChildren)
                {
                    T stn = FindNodeByText<T>(tn.Nodes, text, true);
                    if (stn != null)
                    {
                        tnode = (T)stn;
                        return true;
                    }
                }
            }
            tnode = default(T);
            return false;
        }
        public static bool ContainsChild(this TreeNodeCollection parent, TreeNode node, bool searchAllChildren = true)
        {
            foreach (TreeNode tn in parent)
            {
                if (tn == node)
                {
                    return true;
                }
                if (searchAllChildren)
                {
                    if (ContainsChild(tn.Nodes, node, true))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------

        public static TreeNode GetNodeByPath(this TreeNode node, string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                string[] paths = path.Split('/');
                foreach (string sub in paths)
                {
                    TreeNode tn = FindNodeByText<TreeNode>(node, sub, false);
                    if (tn != null)
                    {
                        node = tn;
                    }
                    else
                    {
                        return null;
                    }
                }
                return node;
            }
            return node;
        }

        public static TreeNode GetNodeByPath(this TreeNodeCollection view, string path)
        {
            foreach (TreeNode tn in view)
            {
                var result = tn.GetNodeByPath(path);
                if (result != null) return result;
            }
            return null;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        public static TreeNode GetRoot(this TreeNode node)
        {
            var root = node;
            while (root.Parent != null)
            {
                root = root.Parent;
            }
            return root;
        }
        public static string GetSavePath(this TreeNode node, bool include_node_name = false)
        {
            return GetSavePath(node.GetRoot(), node, include_node_name);
        }
        public static string GetSavePath(this TreeNode root, TreeNode node, bool include_node_name = false)
        {
            if (node == root)
            {
                return "" + (include_node_name ? node.Text : "");
            }
            if (node.Parent == root)
            {
                return (include_node_name ? node.Text : "");
            }
            string ret = node.Parent.Text + (include_node_name ? "/" + node.Text : "");
            node = node.Parent;
            while (node != null)
            {
                if (node.Parent == root)
                {
                    break;
                }
                ret = node.Parent.Text + "/" + ret;
                node = node.Parent;
            }
            return ret;
        }

        public static TreeNode[] ToArray(this TreeNodeCollection tc, bool sort = false)
        {
            var ret = new List<TreeNode>();
            foreach (TreeNode tn in tc)
            {
                ret.Add(tn);
            }
            if (sort) ret.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
            return ret.ToArray();
        }
        public static List<TreeNode> GetAllNodes(this TreeNodeCollection tc, bool sort = false)
        {
            var ret = new List<TreeNode>();
            foreach (TreeNode tn in tc)
            {
                GetAllNodes(tn, ret);
            }
            if (sort) ret.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
            return ret;
        }
        public static List<TreeNode> GetAllNodes(this TreeView tv, bool sort = false)
        {
            return GetAllNodes(tv.Nodes, sort);
        }
        public static List<TreeNode> GetAllNodes(this TreeNode tn, bool sort = false)
        {
            var ret = new List<TreeNode>();
            GetAllNodes(tn, ret);
            if (sort) ret.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
            return ret;
        }
        public static void GetAllNodes(this TreeNode node, List<TreeNode> ret)
        {
            ret.Add(node);
            foreach (TreeNode tn in node.Nodes)
            {
                GetAllNodes(tn, ret);
            }
        }

        public static List<T> GetAllNodesAs<T>(this TreeNodeCollection tc, bool sort = false)
        {
            var ret = new List<T>();
            foreach (TreeNode tn in tc)
            {
                GetAllNodesAs<T>(tn, ret);
            }
            if (sort) ret.Sort((a, b) =>
            {
                if (a is TreeNode ta && b is TreeNode tb)
                {
                    return ta.Name.CompareTo(tb.Name);
                }
                return a.ToString().CompareTo(b.ToString());
            });
            return ret;
        }
        public static List<T> GetAllNodesAs<T>(this TreeView tv, bool sort = false)
        {
            return GetAllNodesAs<T>(tv.Nodes, sort);
        }
        public static List<T> GetAllNodesAs<T>(this TreeNode node, bool sort = false)
        {
            var ret = new List<T>();
            GetAllNodesAs<T>(node, ret);
            if (sort) ret.Sort((a, b) =>
            {
                if (a is TreeNode ta && b is TreeNode tb)
                {
                    return ta.Name.CompareTo(tb.Name);
                }
                return a.ToString().CompareTo(b.ToString());
            });
            return ret;
        }
        public static void GetAllNodesAs<T>(this TreeNode node, List<T> ret)
        {
            if (node is T tn) { ret.Add(tn); }
            foreach (TreeNode sub in node.Nodes)
            {
                GetAllNodesAs(sub, ret);
            }
        }

        public static List<T> GetNodesAs<T>(this TreeNode node, bool sort = false)
        {
            var ret = new List<T>();
            foreach (TreeNode sub in node.Nodes)
            {
                if (sub is T tn)
                {
                    ret.Add(tn);
                }
            }
            if (sort) ret.Sort((a, b) =>
            {
                if (a is TreeNode ta && b is TreeNode tb)
                {
                    return ta.Name.CompareTo(tb.Name);
                }
                return a.ToString().CompareTo(b.ToString());
            });
            return ret;
        }
        public static List<TreeNode> TreeNodeDuplicate(this TreeNodeCollection root, Func<TreeNode, G2DDuplicateTreeNode> clone)
        {
            var Nodes = new List<TreeNode>();
            foreach (TreeNode tn in root)
            {
                if (tn is G2DDuplicateTreeNode tr)
                {
                }
                else
                {
                    tr = tn.TreeNodeDuplicate(clone) as G2DDuplicateTreeNode;
                }
                if (tr != null)
                {
                    Nodes.Add(tr);
                    if (tn.IsExpanded)
                    {
                        tr.Expand();
                    }
                    else
                    {
                        tr.Collapse();
                    }
                }
            }
            return Nodes;
        }
        public static G2DDuplicateTreeNode TreeNodeDuplicate(this TreeNode root, Func<TreeNode, G2DDuplicateTreeNode> clone)
        {
            G2DDuplicateTreeNode node = clone(root);// new TreeNode(root.Text);
            if (node != null)
            {
                node.Tag = root.Tag;
                node.ImageIndex = node.SelectedImageIndex = root.ImageIndex;
                node.ImageKey = node.SelectedImageKey = root.ImageKey;
                foreach (TreeNode tn in root.Nodes)
                {
                    TreeNode ctn = TreeNodeDuplicate(tn, clone);
                    if (ctn != null)
                    {
                        node.Nodes.Add(ctn);
                    }
                }
                if (root.IsExpanded)
                {
                    node.Expand();
                }
                else
                {
                    node.Collapse();
                }
            }
            return node;
        }

        public static TreeNode TreeNodeDuplicate(this TreeNode root)
        {
            var node = new G2DDuplicateTreeNode(root);
            node.Tag = root.Tag;
            node.ImageIndex = node.SelectedImageIndex = root.ImageIndex;
            node.ImageKey = node.SelectedImageKey = root.ImageKey;
            foreach (TreeNode tn in root.Nodes)
            {
                var ctn = TreeNodeDuplicate(tn);
                if (ctn != null) node.Nodes.Add(ctn);
            }
            if (root.IsExpanded)
            {
                node.Expand();
            }
            else
            {
                node.Collapse();
            }
            return node;
        }

        public static void MoveTreeNode(this TreeNode node, int d)
        {
            TreeNode parent = node.Parent;
            if (d < 0)
            {
                TreeNode prev = node.PrevNode;
                if (prev != null)
                {
                    node.Remove();
                    parent.Nodes.Insert(prev.Index, node);
                }
            }
            else if (d > 0)
            {
                TreeNode next = node.NextNode;
                if (next != null)
                {
                    node.Remove();
                    parent.Nodes.Insert(next.Index + 1, node);
                }
            }
        }
        public static TreeNode GetOrCreate(this TreeNode tv, string name)
        {
            var node = tv.Nodes[name];
            if (node == null)
            {
                node = tv.Nodes.Add(name);
            }
            return node;
        }
        public static TreeNode GetOrCreate(this TreeNodeCollection tv, string name)
        {
            var node = tv[name];
            if (node == null)
            {
                node = tv.Add(name);
                node.Name = name;
            }
            return node;
        }


        public delegate TreeNode CreateTreeNode(string name, TreeNode parent);

        public static TreeNode GetOrCreateNodeWithPath(this TreeNode tv, CreateTreeNode createNode, params string[] paths)
        {
            return GetOrCreateNodeWithPath(tv, createNode, createNode, paths);
        }
        public static TreeNode GetOrCreateNodeWithPath(this TreeNode tv, CreateTreeNode createNode, CreateTreeNode createDirectory, params string[] paths)
        {
            var root = tv;
            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                if (string.IsNullOrEmpty(path)) { continue; }
                if (root == null)
                {
                    if (i == paths.Length - 1)
                    {
                        root = createNode(path, null);
                    }
                    else
                    {
                        root = createDirectory(path, null);
                    }
                }
                else
                {
                    var node = root.Nodes[path];
                    if (node == null)
                    {
                        if (i == paths.Length - 1)
                        {
                            node = createNode(path, root);
                        }
                        else
                        {
                            node = createDirectory(path, root);
                        }
                        root.Nodes.Add(node);
                    }
                    root = node;
                }
            }
            return root;
        }

        public static TreeNode GetOrCreateNodeWithPath(this TreeNodeCollection tv, CreateTreeNode createNode, params string[] paths)
        {
            return GetOrCreateNodeWithPath(tv, createNode, createNode, paths);
        }
        public static TreeNode GetOrCreateNodeWithPath(this TreeNodeCollection tv, CreateTreeNode createNode, CreateTreeNode createDirectory, params string[] paths)
        {
            TreeNode root = null;
            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                if (string.IsNullOrEmpty(path)) { continue; }
                if (root == null)
                {
                    var node = tv[path];
                    if (node == null)
                    {
                        if (i == paths.Length - 1)
                        {
                            node = createNode(path, root);
                        }
                        else
                        {
                            node = createDirectory(path, root);
                        }
                        node.Name = path;
                        tv.Add(node);
                    }
                    root = node;
                }
                else
                {
                    var node = root.Nodes[path];
                    if (node == null)
                    {
                        if (i == paths.Length - 1)
                        {
                            node = createNode(path, root);
                        }
                        else
                        {
                            node = createDirectory(path, root);
                        }
                        node.Name = path;
                        root.Nodes.Add(node);
                    }
                    root = node;
                }
            }
            return root;
        }



        public delegate TreeNode CreateTreePathNode(FileSystemInfo name, TreeNode parent);
        public static TreeNode GetOrCreateNode(this TreeNode tv, DirectoryInfo dir, FileSystemInfo sub, CreateTreePathNode createNode, CreateTreePathNode createDirectory)
        {
            var paths = CFiles.GetPathList(dir, sub);
            var root = tv;
            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                var node = root.Nodes[path.Name];
                if (node == null)
                {
                    if (i == paths.Length - 1)
                    {
                        node = createNode(path, root);
                    }
                    else
                    {
                        node = createDirectory(path, root);
                    }
                    root.Nodes.Add(node);
                }
                root = node;
            }
            return root;
        }
        public static TreeNode GetOrCreateNode(this TreeNode tv, DirectoryInfo root, DirectoryInfo sub, CreateTreePathNode createNode)
        {
            return GetOrCreateNode(tv, root, sub, createNode, createNode);
        }
        public static TreeNode GetOrCreateNode(this TreeNode tv, DirectoryInfo root, FileInfo sub, CreateTreePathNode createNode)
        {
            return GetOrCreateNode(tv, root, sub, createNode, createNode);
        }
        //-------------------------------------------------------------------------------------------------------

        public static void ShowSearchDialog(this TreeView view)
        {
            G2DSearchDialog find = new G2DSearchDialog();
            TreeNode last_find_object = null;
            if (view.TopNode != null)
            {
                find.SetTitle("查找" + view.TopNode.Text);
            }
            find.FindPrevClicked += (string text) =>
            {
                TreeNode finded = FormUtils.FindLastTreeNodeByText(view.Nodes, text, last_find_object);
                if (finded != null)
                {
                    view.SelectedNode = finded;
                    finded.Expand();
                    last_find_object = finded;
                    finded.EnsureVisible();
                }
                return finded;
            };
            find.FindNextClicked += (string text) =>
            {
                TreeNode finded = FormUtils.FindTreeNodeByText(view.Nodes, text, last_find_object);
                if (finded == null)
                {
                    //从头再查一次，这样就可以循环搜索了
                    finded = FormUtils.FindTreeNodeByText(view.Nodes, text, null);
                }
                if (finded != null)
                {
                    view.SelectedNode = finded;
                    finded.Expand();
                    last_find_object = finded;
                    finded.EnsureVisible();
                }
                return finded;
            };
            find.FindClicked += (string text) =>
            {
                TreeNode finded = FormUtils.FindTreeNodeByText(view.Nodes, text, last_find_object, true);
                if (finded != null)
                {
                    view.SelectedNode = finded;
                    finded.Expand();
                    last_find_object = finded;
                    find.Close();
                    finded.EnsureVisible();
                }
                return finded;
            };
            find.Show();
        }


        public static TreeNode ShowSelectDialog(this TreeView view, TreeNode defaultNode = null)
        {
            var dialog = new G2DListSelectEditor(null, view.Nodes, view.ImageList, defaultNode);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedSrc as TreeNode;
            }
            return null;
        }

        public static bool SetIndex(this TreeNode node, int newIndex)
        {
            var parent = node.Parent;
            if (parent != null && node.Index != newIndex && newIndex < parent.Nodes.Count)
            {
                var oldIndex = node.Index;
                var exist = parent.Nodes[newIndex];
                parent.Nodes.Insert(newIndex, node);
                parent.Nodes.Insert(oldIndex, exist);
                return true;
            }
            return false;
        }
    }
}
