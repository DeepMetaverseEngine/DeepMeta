using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Collections;
using DeepCore.Reflection;
using DeepCore.Xml;
using DeepCore;
using DeepCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MaterialSkin.Controls;
using DeepEditor.Common.G2D;

namespace DeepEditor.Common.G2D.DataGrid
{
    public partial class G2DCollectionEditor : G2DBaseForm
    {
        private readonly Type listType;
        private readonly Type[] listTypeGnericArgs;
        private HashMap<Type, Type> elementTypes = new HashMap<Type, Type>();
        private IG2DPropertyAdapter[] adapters;

        public G2DCollectionEditor(Type dataType, object data, Type[] ElementTypes, params IG2DPropertyAdapter[] adapters)
        {
            InitializeComponent();

            if (data == null)
            {
                data = ReflectionUtil.CreateInstance(dataType);
            }

            this.adapters = adapters;
            this.listType = data.GetType();
            this.listTypeGnericArgs = listType.IsGenericType ? listType.GetGenericArguments() : null;
            {
                this.Text = $"集合：{listType.Name}";
            }
            if (ElementTypes != null)
            {
                foreach (var e in ElementTypes)
                {
                    elementTypes.Put(e, e);
                }
            }
            if (listType.IsArray && listType.GetArrayRank() == 1)
            {
                var array = (Array)data;
                int index = 0;
                foreach (object obj in array)
                {
                    listView1.Items.Add(CreateListViewItem(index.ToString(), obj));
                    index++;
                }
                elementTypes.Put(listType.GetElementType(), listType.GetElementType());
            }
            else if (listType.IsInterfaceOf(typeof(IList)))
            {
                var list = (IList)data;
                int index = 0;
                foreach (object obj in list)
                {
                    listView1.Items.Add(CreateListViewItem(index.ToString(), obj));
                    index++;
                }
                if (listType.IsGenericList())
                {
                    elementTypes.Put(listTypeGnericArgs[0], listTypeGnericArgs[0]);
                }
            }
            //             else if (typeof(DataComponentCollection).IsAssignableFrom(listType))
            //             {
            //                 var list = (DataComponentCollection)data;
            //                 list.ForEachComponent((index, obj) =>
            //                 {
            //                     listView1.Items.Add(CreateListViewItem(index.ToString(), obj));
            //                 });
            //                 elementTypes.Put(listTypeGnericArgs[0], listTypeGnericArgs[0]);
            //             }
            else if (listType.IsInterfaceOf(typeof(IDictionary)))
            {
                var map = (IDictionary)data;
                map.ForEachDictionary((e) =>
                {
                    listView1.Items.Add(CreateListViewItem(e.Key.ToString(), CreateMapElement(e.Key, e.Value)));
                });
                if (listType.IsGenericMap())
                {
                    elementTypes.Put(listTypeGnericArgs[1], listTypeGnericArgs[1]);
                }
            }
            else
            {
                throw new Exception("仅支持一维数组和IList类型！");
            }
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
            }
            foreach (var e in new HashMap<Type, Type>(elementTypes))
            {
                if (e.Key.IsAbstract)
                {
                    elementTypes.Remove(e.Key);
                    foreach (var sub in ReflectionUtil.GetNoneVirtualSubTypes(e.Key))
                    {
                        elementTypes.Put(sub, sub);
                    }
                }
            }
        }

        private ListViewItem CreateListViewItem(string name, object element)
        {
            ListViewItem item = new ListViewItem(name);
            item.Tag = element;
            //DescAttribute desc = PropertyUtil.GetDesc(element.GetType());
            item.SubItems.Add(element + "");
            return item;
        }

        private ListViewItem CloneListViewItem(ListViewItem src)
        {
            ListViewItem item = new ListViewItem("");
            item.Tag = XmlUtil.CloneObject(src.Tag);
            //DescAttribute desc = PropertyUtil.GetDesc(item.Tag.GetType());
            item.SubItems.Add(item.Tag + "");
            return item;
        }

        public object CreateMapElement(object key, object value)
        {
            if (listTypeGnericArgs != null)
            {
                var gtype = typeof(MapElement<,>).MakeGenericType(listTypeGnericArgs);
                var ret = DeepActivator.CreateInstance(gtype) as IMapElement;
                ret.Key = key;
                ret.Value = value;
                return ret;
            }
            else
            {
                return new MapElement() { Key = key, Value = value };
            }
        }
        public object NewItemData()
        {
            try
            {
                if (listType.IsInterfaceOf(typeof(IDictionary)))
                {
                    var key = CreateItemInstance(listTypeGnericArgs[0], "输入Key值");
                    if (key == null)
                    {
                        return null;
                    }
                    if (elementTypes.Count == 0)
                    {
                        MessageBox.Show("无法确定列表元素类型！");
                        return null;
                    }
                    else if (elementTypes.Count == 1)
                    {
                        Type etype = elementTypes.Keys.ToArray()[0];
                        object edata = this.CreateItemInstance(etype, "输入Value值");
                        return CreateMapElement(key, edata);
                    }
                    else
                    {
                        List<TypeDescAttribute> list = new List<TypeDescAttribute>();
                        foreach (Type type in elementTypes.Keys)
                        {
                            list.Add(new TypeDescAttribute(type));
                        }
                        list.Sort();
                        G2DListSelectEditor<TypeDescAttribute> dialog = new G2DListSelectEditor<TypeDescAttribute>(list, null);
                        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            TypeDescAttribute selected = dialog.SelectedTag;
                            if (selected != null)
                            {
                                object edata = this.CreateItemInstance(selected.OwnerType, "输入Value值");
                                return CreateMapElement(key, edata);
                            }
                        }
                    }

                }
                else
                {
                    if (elementTypes.Count == 0)
                    {
                        MessageBox.Show("无法确定列表元素类型！");
                        return null;
                    }
                    else if (elementTypes.Count == 1)
                    {
                        Type etype = elementTypes.Keys.ToArray()[0];
                        object edata = this.CreateItemInstance(etype);
                        return edata;
                    }
                    else
                    {
                        List<TypeDescAttribute> list = new List<TypeDescAttribute>();
                        foreach (Type type in elementTypes.Keys)
                        {
                            list.Add(new TypeDescAttribute(type));
                        }
                        list.Sort();
                        G2DListSelectEditor<TypeDescAttribute> dialog = new G2DListSelectEditor<TypeDescAttribute>(list, null);
                        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            TypeDescAttribute selected = dialog.SelectedTag;
                            if (selected != null)
                            {
                                object edata = this.CreateItemInstance(selected.OwnerType);
                                return edata;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            return null;
        }

        public virtual object CreateItemInstance(Type etype, string tip = null)
        {
            try
            {
                if (etype.Equals(typeof(string)))
                {
                    if (tip == null) tip = "输入字符";
                    return G2DTextDialog.Show("", tip);
                }
                else if (etype.IsPrimitive)
                {
                    if (tip == null) tip = "输入值(" + etype.Name + ")";
                    var txt = G2DTextDialog.Show("", tip);
                    return DeepCore.Parser.StringToObject(txt, etype);
                }
                else if (etype.IsClass)
                {
                    return ReflectionUtil.CreateInstance(etype);
                }
                else if (etype.IsEnum)
                {
                    var ebox = new G2DEnumSelectEditor(etype);
                    ebox.ShowDialog();
                    return ebox.SelectedEnumValue;
                }
                else
                {
                    if (tip == null) tip = "输入值(" + etype.Name + ")";
                    var txt = G2DTextDialog.Show("", tip);
                    return DeepCore.Parser.StringToObject(txt, etype);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            return ReflectionUtil.CreateInstance(etype);
        }

        public object GetEditCompleteData()
        {
            if (listType.IsArray)
            {
                int count = listView1.Items.Count;
                var mirror_data = new ArrayList(count);
                foreach (ListViewItem item in listView1.Items)
                {
                    mirror_data.Add(item.Tag);
                }
                if (listType.GetElementType().GetInterface(nameof(IComparable)) != null)
                {
                    mirror_data.Sort();
                }
                var array = Array.CreateInstance(listType.GetElementType(), count);
                mirror_data.CopyTo(array);
                return array;
            }
            else if (listType.IsInterfaceOf(typeof(IList)))
            {
                int count = listView1.Items.Count;
                var mirror_data = new ArrayList(count);
                foreach (ListViewItem item in listView1.Items)
                {
                    mirror_data.Add(item.Tag);
                }
                if (listTypeGnericArgs != null && listTypeGnericArgs[0].GetInterface(nameof(IComparable)) != null)
                {
                    mirror_data.Sort();
                }
                var list = (IList)ReflectionUtil.CreateInstance(listType);
                foreach (var e in mirror_data) list.Add(e);
                return list;
            }
            //             else if (typeof(DataComponentCollection).IsAssignableFrom(listType))
            //             {
            //                 var list = (DataComponentCollection)ReflectionUtil.CreateInstance(listType);
            //                 foreach (ListViewItem item in listView1.Items)
            //                 {
            //                     list.AddComponent(item.Tag);
            //                 }
            //                 return list;
            //             }
            else if (listType.IsInterfaceOf(typeof(IDictionary)))
            {
                var map = (IDictionary)ReflectionUtil.CreateInstance(listType);
                foreach (ListViewItem item in listView1.Items)
                {
                    var e = item.Tag as IMapElement;
                    map[e.Key] = e.Value;
                }
                return map;
            }
            else
            {
                return null;
            }
        }

        public void RefreshList()
        {
            if (listType.IsInterfaceOf(typeof(IDictionary)))
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    var e = item.Tag as IMapElement;
                    item.Text = e.Key + "";
                }
            }
            else
            {
                int index = 0;
                foreach (ListViewItem item in listView1.Items)
                {
                    item.Text = index.ToString();
                    index++;
                }
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.propertyGrid1.SelectedObject = G2DTypeDescriptor.CreateDescriptor(e.Item.Tag, adapters);
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                var desc = this.propertyGrid1.SelectedObject as G2DTypeDescriptor;
                if (desc.EditData.GetType().IsEnum)
                {
                    listView1.SelectedItems[0].Tag = e.ChangedItem.Value;
                }
            }
            catch { }

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                item.SubItems[1].Text = item.Tag + "";
            }
            this.listView1.Invalidate();
        }

        private void btn_AddItem_Click(object sender, EventArgs e)
        {
            object edata = NewItemData();
            if (edata != null)
            {
                int index = listView1.Items.Count;
                if (listView1.SelectedItems.Count > 0)
                {
                    index = listView1.SelectedIndices[0] + 1;
                }
                if (edata is IMapElement mape)
                {
                    ListViewItem item = CreateListViewItem(mape.Key.ToString(), edata);
                    listView1.Items.Insert(index, item);
                    RefreshList();
                    item.Selected = true;
                }
                else
                {
                    ListViewItem item = CreateListViewItem(index.ToString(), edata);
                    listView1.Items.Insert(index, item);
                    RefreshList();
                    item.Selected = true;
                }
            }
        }

        private void btn_DelItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                listView1.Items.Remove(item);
            }
            RefreshList();
        }

        private void btn_MoveUpItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int curIndex = listView1.SelectedIndices[0];
                if (curIndex > 0)
                {
                    ListViewItem item = listView1.SelectedItems[0];
                    listView1.Items.Remove(item);
                    listView1.Items.Insert(curIndex - 1, item);
                    RefreshList();
                    item.Selected = true;
                }
            }
        }

        private void btn_MoveDownItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int curIndex = listView1.SelectedIndices[0];
                if (curIndex < listView1.Items.Count - 1)
                {
                    ListViewItem item = listView1.SelectedItems[0];
                    listView1.Items.Remove(item);
                    listView1.Items.Insert(curIndex + 1, item);
                    RefreshList();
                    item.Selected = true;
                }
            }
        }

        private void btn_DuplicateItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                ListViewItem dst = CloneListViewItem(item);
                listView1.Items.Insert(item.Index + 1, dst);
                RefreshList();
                dst.Selected = true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        public interface IMapElement
        {
            object Key { get; set; }
            object Value { get; set; }
        }
        public class MapElement : IMapElement
        {
            [Desc("Key")]
            public object Key;
            [Desc("Value")]
            public object Value;
            object IMapElement.Key { get => this.Key; set { this.Key = value; } }
            object IMapElement.Value { get => this.Value; set { this.Value = value; } }
            public override string ToString()
            {
                return $"{Value}";
            }
        }
        public class MapElement<K, V> : IMapElement
        {
            [Desc("Key")]
            public K Key;
            [Desc("Value")]
            public V Value;
            object IMapElement.Key { get => this.Key; set { this.Key = (K)value; } }
            object IMapElement.Value { get => this.Value; set { this.Value = (V)value; } }
            public override string ToString()
            {
                return $"{Value}";
            }
        }
    }
}
