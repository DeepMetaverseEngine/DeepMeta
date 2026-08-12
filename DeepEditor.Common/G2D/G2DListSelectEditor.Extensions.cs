using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

using DeepCore;
using DeepCore.Reflection;

namespace DeepEditor.Common.G2D
{
    public class G2DListSelectEditor<TAG> : G2DListSelectEditor
    {
        new public TAG SelectedTag { get => (TAG)base.SelectedTag; }
        new public List<TAG> SelectedTags
        {
            get
            {
                var objs = base.SelectedTags;
                var ret = new List<TAG>();
                foreach (var o in objs)
                {
                    if (typeof(TAG).IsInstanceOfType(o))
                    {
                        ret.Add((TAG)o);
                    }
                }
                return ret;
            }
        }
        public G2DListSelectEditor(List<TAG> list, object selected = null) : base(typeof(TAG), list, selected) { }
        public G2DListSelectEditor(TAG[] list, object selected = null) : base(typeof(TAG), list, selected) { }
        public G2DListSelectEditor(TreeNode root, ImageList imageList, object selected) : base(typeof(TAG), root, imageList, selected) { }
        public G2DListSelectEditor(TreeNodeCollection root, ImageList imageList, object selected) : base(typeof(TAG), root, imageList, selected) { }

        public G2DListSelectEditor(List<TAG> list, Predicate<ListViewItem> selected) : base(typeof(TAG), list, selected) { }
        public G2DListSelectEditor(TAG[] list, Predicate<ListViewItem> selected) : base(typeof(TAG), list, selected) { }
        public G2DListSelectEditor(TreeNode root, ImageList imageList, Predicate<G2DDuplicateTreeNode> selected) : base(typeof(TAG), root, imageList, selected) { }
        public G2DListSelectEditor(TreeNodeCollection root, ImageList imageList, Predicate<G2DDuplicateTreeNode> selected) : base(typeof(TAG), root, imageList, selected) { }

        public G2DListSelectEditor(TreeNode root, ImageList imageList, string path) : base(typeof(TAG), root, imageList, path) { }

        public G2DListSelectEditor(TreeView root, Predicate<G2DDuplicateTreeNode> selected) : base(typeof(TAG), root, selected) { }
        public G2DListSelectEditor(TreeView root) : base(typeof(TAG), root, tag => false) { }
        public G2DListSelectEditor(TreeNodeCollection root, ImageList imageList) : base(typeof(TAG), root, imageList, tag => false) { }
        public G2DListSelectEditor(IEnumerable<TreeNode> root, ImageList imageList) : base(typeof(TAG), root, imageList, tag => false) { }

    }

    public class G2DEnumSelectEditor : G2DListSelectEditor
    {
        class EnumItem
        {
            public Type EnumType;
            public object EnumValue;
            public override string ToString()
            {
                var attr = PropertyUtil.GetEnumAttribute<DescAttribute>(EnumValue);
                if (attr != null)
                {
                    return $"{EnumValue} : {attr.Desc}";
                }
                return EnumValue.ToString();
            }
            public override bool Equals(object obj)
            {
                if (obj is EnumItem item)
                {
                    return this.EnumValue.Equals(item.EnumValue);
                }
                return base.Equals(obj);
            }
            public override int GetHashCode()
            {
                return EnumValue.GetHashCode();
            }
        }
        static List<EnumItem> ToEnumList(Type enumType)
        {
            var ret = new List<EnumItem>();
            foreach (var e in Enum.GetValues(enumType))
            {
                ret.Add(ToEnumItem(enumType, e));
            }
            return ret;
        }
        static EnumItem ToEnumItem(Type enumType, object enumValue)
        {
            return new EnumItem() { EnumType = enumType, EnumValue = enumValue };
        }
        public G2DEnumSelectEditor(Type enumType, object selected = null) :
            base(typeof(EnumItem), ToEnumList(enumType), ToEnumItem(enumType, selected ?? Enum.GetValues(enumType).GetValue(0)))
        {
            this.listView1.FullRowSelect = true;
            this.listView1.View = View.List;
        }
        protected override string ElementToString(object arg)
        {
            return base.ElementToString(arg);
        }
        public object SelectedEnumValue
        {
            get
            {
                if (SelectedTag is EnumItem item)
                {
                    return item.EnumValue;
                }
                return null;
            }
        }
    }

    public class G2DCreateInstanceDialog : G2DListSelectEditor<G2DCreateInstanceDialog.ClassDefine>
    {
        private G2DCreateInstanceDialog(List<G2DCreateInstanceDialog.ClassDefine> list, ClassDefine selected)
            : base(list, selected)
        {
        }

        public static object ShowCreateInstanceDialog(Type type, IWin32Window owner = null)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                List<Type> types = ReflectionUtil.GetNoneVirtualSubTypes(type, true);
                if (types.Count > 0)
                {
                    if (types.Count == 1)
                    {
                        return ReflectionUtil.CreateInstance(types[0]);
                    }
                    List<ClassDefine> defines = new List<ClassDefine>();
                    foreach (Type subType in types)
                    {
                        defines.Add(new ClassDefine(subType));
                    }
                    G2DCreateInstanceDialog dialog = new G2DCreateInstanceDialog(defines, null);
                    if (dialog.ShowDialog(owner) == DialogResult.OK)
                    {
                        ClassDefine define = dialog.SelectedTag;
                        return ReflectionUtil.CreateInstance(define.ValueType);
                    }
                }
                return null;
            }
            else
            {
                return ReflectionUtil.CreateInstance(type);
            }
        }

        public class ClassDefine
        {
            public Type ValueType { get; private set; }
            public DescAttribute Desc { get; private set; }
            public ClassDefine(Type subType)
            {
                ValueType = subType;
                Desc = PropertyUtil.GetAttribute<DescAttribute>(subType);
            }
            public override string ToString()
            {
                if (Desc != null)
                {
                    return Desc.Desc;
                }
                else
                {
                    return ValueType.Name;
                }
            }
        }
    }
}
