using DeepCore.Reflection;
using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public partial class G2DObjectViewer : G2DBaseForm
    {
        public G2DObjectViewer(object data)
        {
            InitializeComponent();
            var root = CreateTreeNode(data, null);
            root.ExpandAll();
            treeView1.Nodes.Add(root);
        }

        #region TreeView
        protected virtual bool AcceptChildMember(object data, object memberInfo)
        {
            if (memberInfo is FieldInfo m)
            {
                if (m.IsPublic && !m.IsStatic && !m.IsLiteral)
                {
                    return true;
                }
            }
            if (memberInfo is PropertyInfo p)
            {
                if (p.GetMethod.IsPublic && !p.GetMethod.IsStatic)
                {
                    return true;
                }
            }
            return false;
        }
        protected virtual DataTreeNode CreateTreeNode(object data, object memberInfo)
        {
            var node = new DataTreeNode(data, memberInfo);
            if (data == null)
            {

            }
            else if (data.GetType().IsPrimitiveData())
            {
            }
            else if (data.GetType().IsArray)
            {
                var array = (Array)data;
                for (int i = 0; i < array.Length; i++)
                {
                    node.Nodes.Add(CreateTreeNode(array.GetValue(i), i));
                }
            }
            else if (typeof(IList).IsInstanceOfType(data))
            {
                var list = (IList)data;
                for (int i = 0; i < list.Count; i++)
                {
                    node.Nodes.Add(CreateTreeNode(list[i], i));
                }
            }
            else if (typeof(IDictionary).IsInstanceOfType(data))
            {
                var map = (IDictionary)data;
                foreach (var k in map.Keys)
                {
                    node.Nodes.Add(CreateTreeNode(map[k], k));
                }
            }
            else
            {
                foreach (var m in data.GetType().GetFields())
                {
                    try
                    {
                        var fv = m.GetValue(data);
                        if (AcceptChildMember(fv, m))
                        {
                            node.Nodes.Add(CreateTreeNode(fv, m));
                        }
                    }
                    catch (Exception ex)
                    {
                        node.Nodes.Add(CreateTreeNode(ex.Message, m));
                    }
                }
                foreach (var m in data.GetType().GetProperties())
                {
                    try
                    {
                        var fv = m.GetValue(data); if (AcceptChildMember(fv, m))
                        {
                            node.Nodes.Add(CreateTreeNode(fv, m));
                        }
                    }
                    catch (Exception ex)
                    {
                        node.Nodes.Add(CreateTreeNode(ex.Message, m));
                    }
                }
            }
            return node;
        }

        public class DataTreeNode : TreeNode
        {
            public object Data { get; private set; }
            public object MemberInfo { get; private set; }
            public object ParentData { get => ParentDataNode.Data; }
            public DataTreeNode ParentDataNode { get => Parent as DataTreeNode; }
            public DataTreeNode(object data, object memberInfo)
            {
                Data = data;
                MemberInfo = memberInfo;
                var sb = new StringBuilder();
                if (memberInfo != null)
                {
                    sb.Append(memberInfo.ToString()).Append(" : ");
                }
                if (data != null)
                {
                    sb.Append(data.ToString());
                }
                else
                {
                    sb.Append("NULL");
                }
                this.Text = sb.ToString();
            }
        }



        #endregion
    }

}
