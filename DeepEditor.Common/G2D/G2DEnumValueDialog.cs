using DeepCore.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DeepEditor.Common.G2D
{
    public partial class G2DEnumValueDialog : G2DForm
    {
        private static List<Type> EnumTypes;

        public EnumTypeListItem SelectedEnumType
        {
            get
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    var item = listView1.SelectedItems[0] as EnumTypeListItem;
                    return item;
                }
                return null;
            }
        }
        public EnumValueListItem SelectedEnumValue
        {
            get
            {
                if (listView2.SelectedItems.Count > 0)
                {
                    var item = listView2.SelectedItems[0] as EnumValueListItem;
                    return item;
                }
                return null;
            }
        }
        public EnumValue SelectedObject
        {
            get
            {
                if (SelectedEnumValue is EnumValueListItem item)
                {
                    try
                    {
                        return new EnumValue()
                        {
                            EnumType = item.EnumType,
                            Value = item.Int32Value,
                        };
                    }
                    catch
                    {

                    }
                }
                return null;
            }
        }

        private EnumTypeListItem lastSelectedType;
        private EnumValueListItem lastSelectedValue;

        public G2DEnumValueDialog(EnumValue selected)
        {
            InitializeComponent();
            if (EnumTypes == null)
            {
                EnumTypes = new List<Type>();
                var alltypes = ReflectionUtil.GetRuntimeTypes();
                foreach (var type in alltypes)
                {
                    if (type.IsEnum && type.TryGetAttribute<DescAttribute>(out var desc))
                    {
                        EnumTypes.Add(type);
                    }
                }
            }
            this.SuspendLayout();
            try
            {
                var selectedType = lastSelectedType;
                var adding = new List<EnumTypeListItem>();
                foreach (var type in EnumTypes)
                {
                    var item = new EnumTypeListItem(type);
                    adding.Add(item);
                    if (type == selected?.EnumType)
                    {
                        selectedType = item;
                    }
                }
                listView1.Items.AddRange(adding.ToArray());
                if (selectedType != null)
                {
                    selectedType.Selected = true;
                    SetSelectedType(selectedType.EnumType);
                    foreach (EnumValueListItem vt in listView2.Items)
                    {
                        if (vt.Int32Value == selected.Value)
                        {
                            vt.Selected = true;
                            this.lastSelectedValue = vt;
                            break;
                        }
                    }
                    this.lastSelectedType = selectedType;
                }
                this.listView1.SelectedIndexChanged += ListView1_SelectedIndexChanged;
                this.listView2.SelectedIndexChanged += ListView2_SelectedIndexChanged;
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedEnumType is EnumTypeListItem item)
            {
                SetSelectedType(item.EnumType);
            }
            this.lastSelectedType = SelectedEnumType;
        }
        private void ListView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lastSelectedValue = SelectedEnumValue;
        }

        private void SetSelectedType(Type etype)
        {
            if (lastSelectedType?.EnumType != etype || listView2.Items.Count==0)
            {
                try
                {
                    listView2.Items.Clear();
                    var values = Enum.GetValues(etype);
                    var adding = new List<EnumValueListItem>();
                    foreach (var value in values)
                    {
                        var int32 = EnumValue.ConvertToInt32(value);
                        var item = new EnumValueListItem(etype, value, int32);
                        adding.Add(item);
                    }
                    listView2.Items.AddRange(adding.ToArray());
                }
                catch (Exception ex)
                {
                    ex.ShowMessageBox();
                }
            }
        }

        public class EnumTypeListItem : ListViewItem
        {
            public Type EnumType { get; }
            public EnumTypeListItem(Type etype) : base(new string[] { etype.ToDesc(), etype.Name, })
            {
                this.EnumType = etype;
            }
        }
        public class EnumValueListItem : ListViewItem
        {
            public Type EnumType { get; }
            public object EnumValue { get; }
            public int Int32Value { get; }
            public EnumValueListItem(Type etype, object value, int int32) : base(new string[] { PropertyUtil.ToEnumDesc(value), int32.ToString(), value.ToString() })
            {
                this.EnumType = etype;
                this.EnumValue = value;
                this.Int32Value = int32;
            }
        }

    }
}
