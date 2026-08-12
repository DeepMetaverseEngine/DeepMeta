using DeepCore.Reflection;
using MaterialSkin.Controls;
using DeepEditor.Common.G2D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeepCore;

namespace DeepEditor.Common.G2D
{
    public partial class G2DTypeFieldDialog : G2DBaseForm
    {
        public enum RW
        {
            Read, Write,
        }
        private readonly RW rw;
        public G2DTypeFieldDialog(Type baseType, Type fieldType, Type[] compatibilityTypes, RW rw)
        {
            InitializeComponent();
            this.rw = rw;
            var subTypes = ReflectionUtil.GetNoneVirtualSubTypes(baseType);
            if (subTypes.Count > 0)
            {
                subTypes.Sort((a, b) =>
                {
                    if (a.IsSubclassOf(b)) return 1;
                    if (b.IsSubclassOf(a)) return -1;
                    return 0;
                });
                foreach (var type in subTypes)
                {
                    this.comboBox1.Items.Add(new FieldsType(type, CUtils.ArrayAppend(compatibilityTypes, fieldType)));
                }
            }
        }
        public Type SelectedType
        {
            get { return (comboBox1.SelectedItem as FieldsType)?.ObjectType; }
            set
            {
                if (value == null)
                {
                    comboBox1.SelectedItem = null;
                    return;
                }
                foreach (FieldsType type in comboBox1.Items)
                {
                    if (type.ObjectType == value)
                    {
                        comboBox1.SelectedItem = type;
                        return;
                    }
                }
            }
        }
        public string SelectedField
        {
            get { return (listBox1.SelectedItem as FieldsType.MemberDescAttribute)?.Name; }
            set
            {
                if (value == null)
                {
                    listBox1.SelectedItem = null;
                    return;
                }
                foreach (FieldsType.MemberDescAttribute desc in listBox1.Items)
                {
                    if (desc.Name == value)
                    {
                        listBox1.SelectedItem = desc;
                        return;
                    }
                }
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (SelectedType == null)
            {
                foreach (FieldsType type in comboBox1.Items)
                {
                    comboBox1.SelectedItem = type;
                    return;
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = (comboBox1.SelectedItem as FieldsType);
            var oldField = SelectedField;
            listBox1.Items.Clear();
            if (selected != null)
            {
                var list = new List<FieldsType.MemberDescAttribute>(rw == RW.Read ? selected.GetFields.Values : selected.SetFields.Values);
                list.Sort((ma, mb) =>
                {
                    var a = ma.DeclaringType;
                    var b = mb.DeclaringType;
                    if (a.IsSubclassOf(b)) return 1;
                    if (b.IsSubclassOf(a)) return -1;
                    return ma.CompareTo(mb);
                });
                foreach (var f in list)
                {
                    listBox1.Items.Add(f);
                }
                SelectedField = oldField;
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
