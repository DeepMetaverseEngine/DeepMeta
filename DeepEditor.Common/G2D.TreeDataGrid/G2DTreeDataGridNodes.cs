using AdvancedDataGridView;
using DeepEditor.Common.G2D;
using DeepCore.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Common.Utils.TreeGrid
{
    public class G2DTreeGridDocument : TreeGridNode
    {
        private object mValue;
        public G2DTreeDataGrid OwnerTreeGrid { get; private set; }
        public G2DTreeGridValueNode DocumentElement { get; private set; }
        public object Value
        {
            get { return mValue; }
            set
            {
                this.mValue = value;
                this.Cells[1].Value = "Document";
                this.Cells[2].Value = "";
                this.Cells[3].Value = "";
                this.DocumentElement = CreateValueNode("Document", value.GetType(), null);
                this.Nodes.Clear();
                this.Nodes.Add(DocumentElement);
                this.DocumentElement.BindValue(value, null);
            }
        }

        public G2DTreeGridDocument(G2DTreeDataGrid tree)
        {
            this.OwnerTreeGrid = tree;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public G2DTreeGridValueNode CreateValueNode(string name, Type type, object owner)
        {
            if (type.IsArray)
            {
                return new G2DTreeGridArrayNode(this, name, type, owner);
            }
            else if (type.IsPrimitive)
            {
                return new G2DTreeGridLeafValueNode(this, name, type, owner);
            }
            else if (type.IsEnum)
            {
                return new G2DTreeGridLeafValueNode(this, name, type, owner);
            }
            else if (typeof(string).Equals(type))
            {
                return new G2DTreeGridLeafValueNode(this, name, type, owner);
            }
            else if (type.IsClass)
            {
                if (typeof(IDictionary).IsAssignableFrom(type))
                {
                    return new G2DTreeGridMapNode(this, name, type, owner);
                }
                else if (typeof(IList).IsAssignableFrom(type))
                {
                    return new G2DTreeGridListNode(this, name, type, owner);
                }
                else
                {
                    return new G2DTreeGridFieldsValueNode(this, name, type, owner);
                }
            }
            else
            {
                return new G2DTreeGridFieldsValueNode(this, name, type, owner);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 值类型节点基类
    /// </summary>
    public abstract class G2DTreeGridValueNode : TreeGridNode
    {
        public string Name { get; private set; }
        public G2DTreeGridDocument CurrentDocument { get; private set; }
        public G2DTreeGridValueNode ParentValueNode { get { return Parent as G2DTreeGridValueNode; } }
        public object OwnerIndexer { get { return mOwnerIndexer; } }
        public object OwnerObject { get { return mOwnerObject; } }
        public Type ValueType { get { return mValueType; } }
        public object Value { get { return mValue; } }
        public DescAttribute FieldDesc
        {
            get
            {
                if (OwnerIndexer is FieldInfo)
                {
                    return PropertyUtil.GetAttribute<DescAttribute>(OwnerIndexer as FieldInfo);
                }
                return null;
            }
        }
        public bool IsRootValue
        {
            get { return this.OwnerObject == null; }
        }
        public bool IsLeaf
        {
            get
            {
                if (mValueType.IsPrimitive)
                {
                    return true;
                }
                if (mValueType.IsEnum)
                {
                    return true;
                }
                if (mValueType.Equals(typeof(string)))
                {
                    return true;
                }
                if (mValueType.IsClass)
                {
                    return false;
                }
                return false;
            }
        }
        public bool IsCollection
        {
            get
            {
                if (mValueType.IsArray)
                {
                    return true;
                }
                if (typeof(IList).IsAssignableFrom(mValueType))
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsFieldReadonly
        {
            get
            {
                return !IsLeaf;
            }
        }

        private readonly Type mValueType;
        private readonly object mOwnerObject;
        private object mOwnerIndexer;
        private object mValue;

        public G2DTreeGridValueNode(G2DTreeGridDocument doc, string name, Type type, object owner)
        {
            this.CurrentDocument = doc;
            this.Name = name;
            this.mValueType = type;
            this.mOwnerObject = owner;
        }
        public G2DTreeGridValueNode GetChild(string key)
        {
            foreach (G2DTreeGridValueNode c in Nodes)
            {
                if (string.Equals(c.Name, key))
                {
                    return c;
                }
            }
            return null;
        }
        public void Refresh()
        {
            this.Image = ToFieldImage();
            this.Cells[1].Value = ToFieldString();
            this.Cells[1].ToolTipText = ToFieldToolTip();
            this.Cells[1].ContextMenuStrip = CurrentDocument.OwnerTreeGrid.menuNode;
          
            this.Cells[2].Value = ToValueString();
            this.Cells[2].ValueType = mValueType;
            this.Cells[2].ReadOnly = IsFieldReadonly;
            (this.Cells[2] as G2DTreeGridValueCell).SetValueEditType(ValueType);

            this.Cells[3].Value = ToFieldCatagory();
            this.Cells[3].ToolTipText = ToFieldToolTip();
        }
        protected void AppendChild(G2DTreeGridValueNode child)
        {
            if (child.ParentValueNode != null)
            {
                throw new Exception(string.Format("Child already have a parent : child={0} parent={1}", child.Name, child.ParentValueNode.Name));
            }
            this.Nodes.Add(child);
        }
        /// <summary>
        /// 如果是复合类，产生子节点
        /// </summary>
        protected abstract void GenChilds();
        /// <summary>
        /// 设置当前节点值（此操作 将改变当前节点结构）
        /// </summary>
        protected abstract void SetFieldValue(G2DTreeGridValueNode child, object value);

        internal void BindValue(object value, object key)
        {
            this.mValue = value;
            this.mOwnerIndexer = key;
            this.Nodes.Clear();
            this.GenChilds();
            this.Refresh();
        }

        /// <summary>
        /// 设置当前节点值（此操作 将改变当前节点结构）
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(object value)
        {
            if (ParentValueNode != null)
            {
                ParentValueNode.SetFieldValue(this, value);
            }
            this.Refresh();
        }

        //----------------------------------------------------------------------
        #region _Cells_
        public virtual string ToFieldString()
        {
            return Name;
        }
        public virtual Image ToFieldImage()
        {
            return CurrentDocument.OwnerTreeGrid.img_icon_men;
        }
        public virtual string ToFieldToolTip()
        {
            if (FieldDesc != null)
            {
                return FieldDesc.Desc;
            }
            return null;
        }
        public virtual string ToFieldCatagory()
        {
            if (FieldDesc != null)
            {
                return FieldDesc.Category;
            }
            return null;
        }
        public virtual string ToValueString()
        {
            return Value + "";
        }
        public virtual string ToTypeString()
        {
            return mValueType.ToTypeDefineName();
        }
        #endregion
        //----------------------------------------------------------------------
        #region _Operators_
        protected internal virtual void OnMenuOpening(CancelEventArgs e)
        {
            if (ParentValueNode == null)
            {
                e.Cancel = true;
                return;
            }

            CurrentDocument.OwnerTreeGrid.menuNode.SuspendLayout();
            try
            {
                foreach (ToolStripItem drop in CurrentDocument.OwnerTreeGrid.menuNode.Items)
                {
                    drop.Visible = false;
                }
                if (ParentValueNode.IsCollection)
                {
                    CurrentDocument.OwnerTreeGrid.menuNode_Delete.Visible = true;
                    CurrentDocument.OwnerTreeGrid.menuNode_Up.Visible = true;
                    CurrentDocument.OwnerTreeGrid.menuNode_Down.Visible = true;
                }
                else if (IsLeaf)
                {
                    e.Cancel = true;
                }
                else if (IsCollection)
                {
                    CurrentDocument.OwnerTreeGrid.menuNode_New.Visible = true;
                }
                else
                {
                    if (Value != null)
                    {
                        CurrentDocument.OwnerTreeGrid.menuNode_Delete.Visible = true;
                    }
                    else
                    {
                        CurrentDocument.OwnerTreeGrid.menuNode_New.Visible = true;
                    }
                }
            }
            finally
            {
                CurrentDocument.OwnerTreeGrid.menuNode.ResumeLayout();
            }
        }
        protected internal virtual void OnNewContentClick()
        {
            object value = G2DCreateInstanceDialog.ShowCreateInstanceDialog(ValueType);
            SetValue(value);
        }
        protected internal virtual void OnDelContentClick()
        {
            if (ParentValueNode != null)
            {
                if (ParentValueNode.IsCollection)
                {
                    ParentValueNode.OnRemoveListItemClick(this);
                }
                else if (!IsLeaf)
                {
                    SetValue(null);
                }
            }
        }
        protected internal virtual void OnMoveListItemIndexClick(int d) { }
        protected internal virtual void OnRemoveListItemClick(G2DTreeGridValueNode node) { }


        #endregion
        //----------------------------------------------------------------------
    }

    //---------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 表示基础类型
    /// </summary>
    public class G2DTreeGridLeafValueNode : G2DTreeGridValueNode
    {
        public G2DTreeGridLeafValueNode(G2DTreeGridDocument doc, string name, Type type, object owner)
            : base(doc, name, type, owner)
        {
        }
        protected override void GenChilds()
        {
        }
        protected override void SetFieldValue(G2DTreeGridValueNode child, object value)
        {
        }
    }

    //---------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 表示类
    /// </summary>
    public class G2DTreeGridFieldsValueNode : G2DTreeGridValueNode
    {
        public G2DTreeGridFieldsValueNode(G2DTreeGridDocument doc, string name, Type type, object owner)
            : base(doc, name, type, owner)
        {
        }

        protected override void GenChilds()
        {
            if (Value != null && !IsLeaf)
            {
                foreach (FieldInfo field in ValueType.GetFields())
                {
                    if (!field.IsStatic && !field.IsLiteral && !field.IsInitOnly && field.IsPublic)
                    {
                        Type ft = field.FieldType;
                        object fv = field.GetValue(Value);
                        if (fv != null) { ft = fv.GetType(); }
                        G2DTreeGridValueNode fe = CurrentDocument.CreateValueNode(field.Name, ft, Value);
                        AppendChild(fe);
                        fe.BindValue(fv, field);
                    }
                }
            }
        }
        protected override void SetFieldValue(G2DTreeGridValueNode child, object value)
        {
            child = GetChild(child.Name) as G2DTreeGridValueNode;
            if (child != null)
            {
                FieldInfo field = child.OwnerIndexer as FieldInfo;
                field.SetValue(this.Value, value);
                child.BindValue(value, field);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 表示数组
    /// </summary>
    public class G2DTreeGridArrayNode : G2DTreeGridValueNode
    {
        readonly public Type ElementType;
        readonly public int Rank;
        public ListDescAttribute ListDesc
        {
            get
            {
                if (OwnerIndexer is FieldInfo)
                {
                    return PropertyUtil.GetAttribute<ListDescAttribute>(OwnerIndexer as FieldInfo);
                }
                return null;
            }
        }
        public Array ArrayValue
        {
            get
            {
                if (Value != null) { return (Array)Value; } return null;
            }
        }
        public int ArrayLength
        {
            get
            {
                if (Value != null) { return ((Array)Value).Length; } return 0;
            }
        }

        public G2DTreeGridArrayNode(G2DTreeGridDocument doc, string name, Type type, object owner)
            : base(doc, name, type, owner)
        {
            this.ElementType = ValueType.GetElementType();
            this.Rank = ValueType.GetArrayRank();
        }
        protected override void GenChilds()
        {
            if (Value != null)
            {
                Array array = (Array)Value;
                int i = 0;
                foreach (object v in array)
                {
                    if (v != null)
                    {
                        Type vtype = v.GetType();
                        G2DTreeGridValueNode ei = CurrentDocument.CreateValueNode(string.Format("[{0}]", i), vtype, Value);
                        AppendChild(ei);
                        ei.BindValue(v, i);
                    }
                    i++;
                }
            }
        }
        protected override void SetFieldValue(G2DTreeGridValueNode child, object value)
        {
            child = GetChild(child.Name) as G2DTreeGridValueNode;
            if (child != null)
            {
                int i = (int)child.OwnerIndexer;
                Array array = (Array)this.Value;
                array.SetValue(value, i);
                child.BindValue(value, i);
            }
        }
        public override string ToValueString()
        {
            if (Value != null)
            {
                Array array = (Array)Value;
                return "Length="+array.GetLength(1).ToString();
            }
            return "null";
        }
        protected internal override void OnNewContentClick()
        {
            object item = G2DCreateInstanceDialog.ShowCreateInstanceDialog(ElementType);
            if (item != null)
            {
                Array dst = (Array)ReflectionUtil.CreateInstance(ValueType, ArrayLength + 1);
                if (ArrayValue != null)
                {
                    Array.Copy(ArrayValue, dst, ArrayLength);
                }
                dst.SetValue(item, ArrayLength);
                SetValue(dst);
            }
        }
        protected internal override void OnMoveListItemIndexClick(int d) { }
        protected internal override void OnRemoveListItemClick(G2DTreeGridValueNode node)
        {
            if (ArrayLength > 0)
            {
                Array src = ArrayValue;
                int newLen = ArrayLength - 1;
                Array dst = (Array)ReflectionUtil.CreateInstance(ValueType, newLen);
                for (int i = 0, j = 0; i < newLen && j < newLen; i++, j++)
                {
                    if (i.Equals(node.OwnerIndexer))
                    {
                        i++;
                        if (i >= newLen) { break; }
                    }
                    dst.SetValue(src.GetValue(i), j);
                }
            }
        }

    }

    //---------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 表示列表
    /// </summary>
    public class G2DTreeGridListNode : G2DTreeGridValueNode
    {
        readonly public Type GenericElementType;
        public ListDescAttribute ListDesc
        {
            get
            {
                if (OwnerIndexer is FieldInfo)
                {
                    return PropertyUtil.GetAttribute<ListDescAttribute>(OwnerIndexer as FieldInfo);
                }
                return null;
            }
        }
        public IList ListValue
        {
            get
            {
                if (Value != null) { return (IList)Value; } return null;
            }
        }
        public int ListCount
        {
            get
            {
                if (Value != null) { return ((IList)Value).Count; } return 0;
            }
        }

        public G2DTreeGridListNode(G2DTreeGridDocument doc, string name, Type type, object owner)
            : base(doc, name, type, owner)
        {
            if (ValueType.IsGenericType)
            {
                GenericElementType = ValueType.GetGenericArguments()[0];
            }
        }
        protected override void GenChilds()
        {
            if (Value != null)
            {
                IList list = (IList)Value;
                int i = 0;
                foreach (object v in list)
                {
                    if (v != null)
                    {
                        Type vtype = v.GetType();
                        G2DTreeGridValueNode ei = CurrentDocument.CreateValueNode(string.Format("[{0}]", i), vtype, Value);
                        AppendChild(ei);
                        ei.BindValue(v, i);
                    }
                    i++;
                }
            }
        }
        protected override void SetFieldValue(G2DTreeGridValueNode child, object value)
        {
            child = GetChild(child.Name) as G2DTreeGridValueNode;
            if (child != null)
            {
                int i = (int)child.OwnerIndexer;
                IList list = (IList)this.Value;
                list[i] = value;
                child.BindValue(value, i);
            }
        }
        public override string ToValueString()
        {
            if (Value != null)
            {
                IList array = (IList)Value;
                return "Count=" + array.Count.ToString();
            }
            return "null";
        }
        protected internal override void OnNewContentClick()
        {
            object item = G2DCreateInstanceDialog.ShowCreateInstanceDialog(GenericElementType);
            if (item != null)
            {
                IList dst = ListValue;
                if (dst == null)
                {
                    dst = (IList)ReflectionUtil.CreateInstance(ValueType);
                }
                dst.Add(item);
                SetValue(dst);
            }
        }
        protected internal override void OnMoveListItemIndexClick(int d) { }
        protected internal override void OnRemoveListItemClick(G2DTreeGridValueNode node)
        {
            IList dst = ListValue;
            if (dst != null)
            {
                dst.Remove(node.Value);
                SetValue(dst);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 表示字典
    /// </summary>
    public class G2DTreeGridMapNode : G2DTreeGridValueNode
    {
        readonly public Type GenericKeyType;
        readonly public Type GenericValueType;
        public IDictionary MapValue
        {
            get
            {
                if (Value != null) { return (IDictionary)Value; } return null;
            }
        }

        public G2DTreeGridMapNode(G2DTreeGridDocument doc, string name, Type type, object owner)
            : base(doc, name, type, owner)
        {
            if (ValueType.IsGenericType)
            {
                GenericKeyType = ValueType.GetGenericArguments()[0];
                GenericValueType = ValueType.GetGenericArguments()[1];
            }
            else
            {
                GenericKeyType = null;
                GenericValueType = null;
            }
        }

        protected override void GenChilds()
        {
            if (Value != null)
            {
                IDictionary map = (IDictionary)Value;
                foreach (object k in map.Keys)
                {
                    object v = map[k];
                    if (v != null)
                    {
                        Type vtype = v.GetType();
                        G2DTreeGridValueNode ei = CurrentDocument.CreateValueNode(string.Format("[{0}]", k), vtype, this.Value);
                        AppendChild(ei);
                        ei.BindValue(v, k);
                    }
                }
            }
        }
        protected override void SetFieldValue(G2DTreeGridValueNode child, object value)
        {
            child = GetChild(child.Name) as G2DTreeGridValueNode;
            if (child != null)
            {
                IDictionary map = (IDictionary)this.Value;
                map[child.OwnerIndexer] = value;
                child.BindValue(value, child.OwnerIndexer);
            }
        }
    }


    //---------------------------------------------------------------------------------------------------------------


}
