using DeepCore;
using DeepCore.Reflection;
using DeepEditor.Common.G2D;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace DeepEditor.Common.EventEditor.DescAttributeEdit
{
    public class ValueTypesTreeViewControl : G2DTreeViewControl
    {
        private HashMap<Type, TypeNode> nodeMaps = new HashMap<Type, TypeNode>();
        public void Init(Type baseType)
        {
            Init(baseType, null);
        }
        public void Init(Type baseType, TreeNode parent)
        {
            this.SuspendLayout();
            {
                var baseTypes = ReflectionUtil.GetNoneVirtualSubTypes(baseType);
                foreach (var type in baseTypes)
                {
                    try
                    {
                        if (type.TryGetAttribute<DescAttribute>(out var desc) && desc.Editable)
                        {
                            var valueType = new TypeDescAttribute(type);
                            var catgory = desc.Category;
                            //var catgoryGroup = new CatgoryGroupNode(catgory);
                            var catgoryGroup = default(TreeNode);
                            var paths = catgory.Split('/');
                            if (parent != null)
                            {
                                catgoryGroup = G2DTreeNodes.GetOrCreateNodeWithPath(parent, (text, parent) => new CatgoryGroupNode(text), paths);
                                //catgoryGroup.ForeColor = parent.ForeColor;
                                //parent.Nodes.Add(catgoryGroup);
                            }
                            else
                            {
                                catgoryGroup = G2DTreeNodes.GetOrCreateNodeWithPath(this.Nodes, (text, parent) => new CatgoryGroupNode(text), paths);
                                //this.Nodes.Add(catgoryGroup);
                            }
                            if (catgoryGroup == null)
                            {
                                catgoryGroup = new CatgoryGroupNode("");
                            }
                            if (catgoryGroup != null)
                            {
                                var tn = new TypeNode(valueType);
                                catgoryGroup.Nodes.Add(tn);
                                nodeMaps.Add(valueType.OwnerType, tn);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                //this.SelectedNode = lastSelectNode;
                this.TreeViewNodeSorter = new ValueComparer<TreeNode>((a, b) =>
                {
                    //if (a.Parent == null && b.Parent == null) return 0;
                    if (a is CatgoryGroupNode && b is CatgoryGroupNode) { return a.Text.CompareTo(b.Text); }
                    if (a is TypeNode && b is CatgoryGroupNode) { return 1; }
                    if (b is TypeNode && a is CatgoryGroupNode) { return -1; }
                    return a.Text.CompareTo(b.Text);
                });
                this.Sort();
                this.CheckBoxes = false;
                this.ItemHeight = 22;
                //this.TreeView.AfterSelect += this.treeView1_AfterSelect;
            }
            this.ResumeLayout();
        }
        public bool TryGetTypeNode(Type type, out TypeNode node)
        {
            return this.nodeMaps.TryGetValue(type, out node);
        }
        private static List<string> favorites = new List<string>();
    }



    public class CatgoryGroupNode : G2DTreeNodeGroup
    {
        public string Catgory { get; private set; }
        public CatgoryGroupNode(string catgory) : base(catgory)
        {
            this.Name = catgory;
            this.Catgory = catgory;
            this.ImageKey = SelectedImageKey = "icons_tool_bar2.png";
        }
    }
    public class TypeNode : EventTypeNode
    {
        public TypeDescAttribute TypeDesc { get; private set; }
        public Type ValueType { get; private set; }
        public TypeNode(TypeDescAttribute valueType) : base(valueType.OwnerType)
        {
            this.TypeDesc = valueType;
            this.ValueType = valueType.OwnerType;
            this.Text = valueType.Desc.Desc;
            this.ToolTipText = "按住Ctrl拖动全节点";
        }
    }
}
