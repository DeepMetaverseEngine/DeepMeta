using DeepCore;
using DeepCore.FuncData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DeepEditor.Common.FuncEditor
{
    public partial class FuncTableView : UserControl
    {
        private List<FuncListViewItem> hideList = new List<FuncListViewItem>();
        private Type currentFuncType;
        public ListView FuncListView
        {
            get => listView1;
        }
        public FuncListViewItem[] FuncListItems
        {
            get => FuncListView.Items.ToArray<FuncListViewItem>();
        }
        public FuncTableView(FuncDataTemplate temp)
        {
            InitializeComponent();

            using (var gfx = this.CreateGraphics())
            {
                foreach (var cell in temp.FieldsTable)
                {
                    foreach (var fc in cell.Value)
                    {
                        var ckey = cell.Key + (string.IsNullOrEmpty(fc.FieldType) ? string.Empty : "@" + fc.FieldType);
                        if (!FuncDataManager.Instance.ForEachFuncCellData(fc, (index, value) =>
                        {
                            var column = new FuncColumnHeader(ckey + $"[{index}]", index, fc);
                            column.Width = (int)gfx.MeasureString(column.Key, this.Font).Width + HeadSizeSpace;
                            listView1.Columns.Add(column);
                        }))
                        {
                            var column = new FuncColumnHeader(ckey, -1, fc);
                            column.Width = (int)gfx.MeasureString(column.Key, this.Font).Width + HeadSizeSpace;
                            listView1.Columns.Add(column);
                        }
                    }
                }
            }

            this.listView1.ListViewItemSorter = new FuncListViewSorter();
        }
        public void OnClose()
        {
            currentFuncType = null;
            foreach (var item in hideList)
            {
                item.Checked = false;
                foreach (var sitem in item.SubItems)
                {
                    if (sitem is FuncListViewSubItem sub)
                    {
                        sub.Reset();
                    }
                }
            }
            foreach (var item in FuncListItems)
            {
                item.Checked = false;
                foreach (var sitem in item.SubItems)
                {
                    if (sitem is FuncListViewSubItem sub)
                    {
                        sub.Reset();
                    }
                }
            }
        }
        public void RefreshOnlyShowID(HashMap<string, FuncDataTemplate> exists)
        {
            if (exists != null)
            {
                foreach (var item in FuncListItems)
                {
                    if (exists.ContainsKey(item.FuncTemplate.FuncID))
                    {
                        item.Remove();
                        item.Checked = false;
                        hideList.Add(item);
                    }
                    else
                    {
                        exists.Add(item.FuncTemplate.FuncID, item.FuncTemplate);
                    }
                }
            }
            else
            {
                FuncListView.Items.AddRange(hideList.ToArray());
                FuncListView.Sort();
                hideList.Clear();
            }
        }
        public void SetSelectFieldIndex(IFuncData funcData)
        {
            this.currentFuncType = funcData?.GetType();
            this.FuncListView.SelectedItems.Clear();
            foreach (FuncListViewItem item in FuncListView.Items)
            {
                var temp = item.Tag as FuncDataTemplate;
                if (funcData != null)
                {
                    var dataType = funcData.GetType();
                    var func = funcData.FuncID;
                    var fid = func?.GetFuncFields(temp.FuncID);
                    item.Checked = fid != null;
                    foreach (var sitem in item.SubItems)
                    {
                        if (sitem is FuncListViewSubItem sub)
                        {
                            sub.SetFieldIndex(fid, dataType);
                        }
                    }
                }
            }
        }
        public void SetSelectOwnerFuncs(IDictionary<string, int> ret)
        {
            foreach (FuncListViewItem item in FuncListItems)
            {
                if (ret != null && ret.TryGetValue(item.FuncTemplate.FuncID, out var level))
                {
                    item.Checked = item.FuncTemplate.FuncLevel == level;
                }
            }
        }
        public void SetSelectFillFuncs(ICollection ret, Type elementType)
        {
            this.currentFuncType = elementType;
            if (ret != null)
            {
                foreach (var data in ret)
                {
                    if (data is IFuncData fundata && fundata.FuncID != null)
                    {
                        foreach (FuncListViewItem item in FuncListItems)
                        {
                            if (fundata.FuncID.TryGetFuncFields(item.FuncTemplate.FuncID, item.FuncTemplate.FuncLevel, out var fields))
                            {
                                item.Checked = true;
                            }
                        }
                    }
                }
            }
        }
        public void SetSelectFillFuncs(object data)
        {
            this.currentFuncType = data?.GetType();
            if (data != null)
            {
                if (data is IFuncData fundata && fundata.FuncID != null)
                {
                    foreach (FuncListViewItem item in FuncListItems)
                    {
                        if (fundata.FuncID.TryGetFuncFields(item.FuncTemplate.FuncID, item.FuncTemplate.FuncLevel, out var fields))
                        {
                            item.Checked = true;
                        }
                    }
                }
            }
        }
        public void GetSelectOwnerFuncs(IDictionary<string, int> ret)
        {
            foreach (FuncListViewItem item in FuncListItems)
            {
                if (item.Checked)
                {
                    var func = item.FuncTemplate;
                    ret[func.FuncID] = func.FuncLevel;
                }
            }
        }
        public void GetSelectFields(Type dataType, IList<FuncTable.FuncFields> ret)
        {
            foreach (FuncListViewItem item in FuncListItems)
            {
                if (item.Checked)
                {
                    var func = item.FuncTemplate;
                    var ffields = new FuncTable.FuncFields()
                    {
                        ID = func.FuncID,
                        Level = func.FuncLevel,
                        Fields = new HashMap<string, FuncTable.FuncFieldIndex>(),
                    };
                    foreach (var sitem in item.SubItems)
                    {
                        if (sitem is FuncListViewSubItem sub && dataType != null)
                        {
                            var dataField = dataType.GetField(sub.FuncCell.FieldName);
                            if (dataField != null && dataField.FieldType.IsPrimitiveData())
                            {
                                if (sub.IsExclude || sub.IsInclude || sub.IsOP)
                                {
                                    var findex = new FuncTable.FuncFieldIndex();
                                    findex.Index = sub.FieldIndex;
                                    findex.IsInclude = sub.IsInclude;
                                    findex.IsExclude = sub.IsExclude;
                                    findex.OP = sub.FieldOP;
                                    ffields.Fields.Put(sub.FuncCell.FieldName, findex);
                                }
                            }
                        }
                    }
                    ret.Add(ffields);
                }
            }
        }
        public void GetSelectFields(Type dataType, IDictionary<string, FuncTable.FuncFields> ret)
        {
            var list = new List<FuncTable.FuncFields>();
            GetSelectFields(dataType, list);
            foreach (var func in list)
            {
                ret[func.ID] = func;
            }
        }
        public void GetSelectFields(IFuncData funcData, IDictionary<string, FuncTable.FuncFields> ret)
        {
            var dataType = funcData?.GetType();
            GetSelectFields(dataType, ret);
        }

        public FuncListViewItem CreateFuncItem(FuncDataTemplate temp)
        {
            var item = new FuncListViewItem(temp, FuncListView);
            return item;
        }

        private void ToogleFieldInclude(FuncListViewItem item, FuncListViewSubItem sub)
        {
            if (sub.HasIndex)
            {
                sub.IsInclude = !sub.IsInclude;
                foreach (var sitem in item.SubItems)
                {
                    if (sitem is FuncListViewSubItem subo)
                    {
                        if (subo != sub && subo.FuncCell == sub.FuncCell)
                        {
                            subo.IsInclude = false;
                        }
                    }
                }
                listView1.Refresh();
            }
        }
        private void ToogleFieldExclude(FuncListViewItem item, FuncListViewSubItem sub)
        {
            sub.IsExclude = !sub.IsExclude;
            if (sub.IsExclude)
            {
                sub.IsInclude = false;
                sub.FieldOP = FuncTable.FieldOperation.SET;
            }
            foreach (var sitem in item.SubItems)
            {
                if (sitem is FuncListViewSubItem subo)
                {
                    if (subo != sub && subo.FuncCell == sub.FuncCell)
                    {
                        subo.IsExclude = sub.IsExclude;
                        if (sub.IsExclude)
                        {
                            subo.IsInclude = false;
                            subo.FieldOP = FuncTable.FieldOperation.SET;
                        }
                    }
                }
            }
            listView1.Refresh();
        }
        //------------------------------------------------------------------------------------------------
        public const int HeadSizeSpace = 22;
        public const string IncludeKeyChar = "[✔]";
        public const string ExcludeKeyChar = "[✘]";
        public const string AddKeyChar = "[＋]";
        public const string SubKeyChar = "[－]";
        //------------------------------------------------------------------------------------------------------------------

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (currentFuncType != null && e.Header is FuncColumnHeader header)
            {
                var funcType = currentFuncType;
                if (funcType.GetField(header.FuncCell.FieldName) == null)
                {
                    e.Graphics.DrawString(header.Text, this.Font, Brushes.Gray, e.Bounds);
                    return;
                }
            }
            e.DrawDefault = true;
        }
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = currentFuncType == null;
        }
        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (currentFuncType != null && e.SubItem is FuncListViewSubItem sub)
            {
                e.DrawDefault = false;
                var funcType = currentFuncType;
                if (funcType.GetField(sub.FuncCell.FieldName) == null)
                {
                    e.Graphics.DrawString(sub.Text, this.Font, Brushes.Gray, e.Bounds);
                }
                else if (sub.IsExclude)
                {
                    var ssize = e.Graphics.MeasureString(IncludeKeyChar, this.Font);
                    var bounds2 = new RectangleF(e.Bounds.X + ssize.Width, e.Bounds.Y, e.Bounds.Width - ssize.Width, e.Bounds.Height);
                    e.Graphics.DrawString(sub.Text, this.Font, Brushes.Gray, bounds2);
                    e.Graphics.DrawString(ExcludeKeyChar, this.Font, Brushes.Red, e.Bounds);
                }
                else
                {
                    if (e.Item.Selected)
                    {
                        e.DrawFocusRectangle(e.Bounds);
                    }
                    var ssize = e.Graphics.MeasureString(IncludeKeyChar, this.Font);
                    var bounds2 = new RectangleF(e.Bounds.X + ssize.Width, e.Bounds.Y, e.Bounds.Width - ssize.Width, e.Bounds.Height);
                    e.Graphics.DrawString(sub.Text, this.Font, Brushes.Black, bounds2);
                    if (sub.IsInclude)
                    {
                        e.Graphics.DrawString(IncludeKeyChar, this.Font, Brushes.Green, e.Bounds);
                    }
                    switch (sub.FieldOP)
                    {
                        case FuncTable.FieldOperation.SET: break;
                        case FuncTable.FieldOperation.ADD:
                            e.Graphics.DrawString(AddKeyChar, this.Font, Brushes.Green, e.Bounds, new StringFormat(StringFormatFlags.DirectionRightToLeft));
                            break;
                        case FuncTable.FieldOperation.SUB:
                            e.Graphics.DrawString(SubKeyChar, this.Font, Brushes.Green, e.Bounds, new StringFormat(StringFormatFlags.DirectionRightToLeft));
                            break;
                    }
                }
                return;
            }
            e.DrawDefault = true;
        }
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (currentFuncType != null)
            {
                var item = listView1.GetItemAt(e.X, e.Y) as FuncListViewItem;
                if (item != null)
                {
                    var sub = item.GetSubItemAt(e.X, e.Y) as FuncListViewSubItem;
                    if (sub != null)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            ToogleFieldInclude(item, sub);
                        }
                        else if (e.Button == MouseButtons.Right)
                        {
                            if (sub.Enable)
                            {
                                menu_Field.Tag = new Tuple<FuncListViewItem, FuncListViewSubItem>(item, sub);
                                menu_Field_Exclude.Checked = sub.IsExclude;
                                menu_FieldOP.Enabled = !sub.IsExclude;
                                menu_Field_OP_Set.Checked = sub.FieldOP == FuncTable.FieldOperation.SET;
                                menu_Field_OP_Add.Checked = sub.FieldOP == FuncTable.FieldOperation.ADD;
                                menu_Field_OP_Sub.Checked = sub.FieldOP == FuncTable.FieldOperation.SUB;
                                menu_Field.Show(this, e.Location);
                            }
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------------
        private void menu_Field_Exclude_Click(object sender, EventArgs e)
        {
            if (menu_Field.Tag is Tuple<FuncListViewItem, FuncListViewSubItem> tag)
            {
                ToogleFieldExclude(tag.Item1, tag.Item2);
            }
        }
        private void menu_Field_OP_Set_Click(object sender, EventArgs e)
        {
            if (menu_Field.Tag is Tuple<FuncListViewItem, FuncListViewSubItem> tag)
            {
                tag.Item2.FieldOP = FuncTable.FieldOperation.SET;
                listView1.Refresh();
            }
        }
        private void menu_Field_OP_Add_Click(object sender, EventArgs e)
        {
            if (menu_Field.Tag is Tuple<FuncListViewItem, FuncListViewSubItem> tag)
            {
                tag.Item2.FieldOP = FuncTable.FieldOperation.ADD;
                listView1.Refresh();
            }
        }
        private void menu_Field_OP_Sub_Click(object sender, EventArgs e)
        {
            if (menu_Field.Tag is Tuple<FuncListViewItem, FuncListViewSubItem> tag)
            {
                tag.Item2.FieldOP = FuncTable.FieldOperation.SUB;
                listView1.Refresh();
            }
        }
        //------------------------------------------------------------------------------------------------
        public class FuncListViewSubItem : ListViewItem.ListViewSubItem
        {
            public FuncFieldCellData FuncCell { get; private set; }
            public object FieldValue { get; private set; }
            public int FieldIndex { get; private set; }

            public bool Enable { get; internal set; } = true;
            public bool IsExclude { get; internal set; } = false;
            public bool IsInclude { get; internal set; } = false;
            public FuncTable.FieldOperation FieldOP { get; internal set; } = FuncTable.FieldOperation.SET;
            public bool IsOP { get => FieldOP != FuncTable.FieldOperation.SET; }
            public bool HasIndex { get => FieldIndex >= 0; }

            internal FuncListViewSubItem(ListViewItem owner) : base(owner, "") { }
            internal void SetFieldValue(FuncFieldCellData cell, object value, int index)
            {
                this.FuncCell = cell;
                this.FieldValue = value;
                this.FieldIndex = index;
                this.Name = this.Text = value.ToString();
                this.Tag = value;
            }
            internal void Reset()
            {
                this.Enable = true;
                this.IsExclude = false;
                this.IsInclude = false;
                this.FieldOP = FuncTable.FieldOperation.SET;
            }
            internal void SetFieldIndex(FuncTable.FuncFields fid, Type dataType)
            {
                if (dataType != null && dataType.GetField(this.FuncCell.FieldName) == null)
                {
                    this.Enable = false;
                }
                else if (fid != null)
                {
                    if (fid.TryGetFieldIndex(this.FuncCell.FieldName, out var fieldIndex))
                    {
                        this.Enable = true;
                        this.IsExclude = fieldIndex.IsExclude;
                        if (IsExclude)
                        {
                            this.IsInclude = false;
                            this.FieldOP = FuncTable.FieldOperation.SET;
                        }
                        else
                        {
                            this.IsInclude = !fieldIndex.IsExclude && fieldIndex.IsInclude && (this.FieldIndex == fieldIndex.Index);
                            this.FieldOP = fieldIndex.OP;
                        }
                    }
                    else
                    {
                        this.Reset();
                    }
                }
                else
                {
                    this.Reset();
                }
            }
        }
        public class FuncListViewItem : ListViewItem
        {
            public FuncDataTemplate FuncTemplate { get; }
            internal FuncListViewItem(FuncDataTemplate temp, ListView ListView1) : base(new string[] {
                    temp.FuncID.ToString(),
                    temp.FuncLevel.ToString(),
                    temp.FuncName,
                    temp.FuncDesc
                })
            {
                this.FuncTemplate = temp;
                for (int i = ListView1.Columns.Count - 1 - 4; i >= 0; --i)
                {
                    SubItems.Add(new FuncListViewSubItem(this));
                }
                foreach (var cell in temp.FieldsTable)
                {
                    foreach (var fc in cell.Value)
                    {
                        var ckey = fc.ToString();
                        if (!FuncDataManager.Instance.ForEachFuncCellData(fc, (index, value) =>
                        {
                            var column = ListView1.Columns[ckey + $"[{index}]"];
                            var subitem = SubItems[column.DisplayIndex] as FuncListViewSubItem;
                            subitem.SetFieldValue(fc, value, index);
                        }))
                        {
                            var column = ListView1.Columns[ckey];
                            var subitem = SubItems[column.DisplayIndex] as FuncListViewSubItem;
                            subitem.SetFieldValue(fc, fc.FieldData.ToString(), -1);
                        }
                    }
                }
                this.Tag = temp;
            }
        }
        public class FuncColumnHeader : ColumnHeader
        {
            public string Key { get; }
            public bool IsIndex { get; }
            public int FieldIndex { get; }
            public FuncFieldCellData FuncCell { get; }
            internal FuncColumnHeader(string text, int fieldIndex, FuncFieldCellData cell)
            {
                this.Key = text;
                this.Text = text;
                this.IsIndex = fieldIndex >= 0;
                this.FieldIndex = fieldIndex;
                this.FuncCell = cell;
                this.Name = this.Text;
                this.Tag = cell;
            }
        }
        public class FuncListViewSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x is FuncListViewItem fx && y is FuncListViewItem fy)
                {
                    return fx.FuncTemplate.FuncID.CompareTo(fy.FuncTemplate.FuncID);
                }
                else if (x is FuncListViewSubItem sx && y is FuncListViewSubItem sy)
                {
                    return sx.Text.CompareTo(sy.Text);
                }
                else if (x is ListViewItem ix && y is ListViewItem iy)
                {
                    return ix.Text.CompareTo(iy.Text);
                }
                else
                {
                    return x.ToString().CompareTo(y.ToString());
                }
            }
        }

    }
}
