using DeepCore;
using DeepCore.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.Controls
{
    public class DropDownFieldMaskGenerator : Disposable
    {
        private object genObjectOwner;
        private ToolStripDropDownItem drop;
        private ListDictionary<MemberInfo, ToolStripItem> dropItems = new ListDictionary<MemberInfo, ToolStripItem>();
        private HashMap<string, object> s_default_type;
        private static HashMap<Type, HashMap<string, object>> s_default_value = new();

        public DropDownFieldMaskGenerator(object genObjectOwner, ToolStripDropDownItem drop, string descCategory)
        {
            this.drop = drop;
            this.genObjectOwner = genObjectOwner;
            this.s_default_type = s_default_value.GetOrNew(genObjectOwner.GetType());
            var list = new List<ToolStripItem>();
            foreach (var field in genObjectOwner.GetType().GetFields())
            {
                var desc = PropertyUtil.GetAttribute<DescAttribute>(field);
                if (desc != null)
                {
                    if (field.FieldType == typeof(bool) && desc.Category == descCategory)
                    {
                        try
                        {
                            if (s_default_type.TryGetValue(field.Name, out var default_value))
                            {
                                field.SetValue(genObjectOwner, default_value);
                            }
                        }
                        catch { }
                        ToolStripMenuItem chk = new ToolStripMenuItem();
                        chk.Size = new System.Drawing.Size(152, 22);
                        chk.CheckOnClick = true;
                        chk.Checked = (bool)field.GetValue(genObjectOwner);
                        chk.Text = desc.Desc;
                        chk.Tag = field;
                        chk.Click += new System.EventHandler(this.chk_Click);
                        chk.CheckedChanged += Chk_CheckedChanged;
                        list.Add(chk);
                        dropItems.Add(field, chk);
                    }
                    else if (desc.Desc == "-")
                    {
                        list.Add(new ToolStripSeparator());
                    }
                }
            }
            foreach (var property in genObjectOwner.GetType().GetProperties())
            {
                var desc = PropertyUtil.GetAttribute<DescAttribute>(property);
                if (desc != null)
                {
                    if (property.PropertyType == typeof(bool) && desc.Category == descCategory)
                    {
                        try
                        {
                            if (s_default_type.TryGetValue(property.Name, out var default_value))
                            {
                                property.SetValue(genObjectOwner, default_value);
                            }
                        }
                        catch { }
                        ToolStripMenuItem chk = new ToolStripMenuItem();
                        chk.Size = new System.Drawing.Size(152, 22);
                        chk.CheckOnClick = true;
                        chk.Checked = (bool)property.GetValue(genObjectOwner);
                        chk.Text = desc.Desc;
                        chk.Tag = property;
                        chk.Click += new System.EventHandler(this.chk_Click);
                        chk.CheckedChanged += Chk_CheckedChanged;
                        list.Add(chk);
                        dropItems.Add(property, chk);
                    }
                    else if (desc.Desc == "-")
                    {
                        list.Add(new ToolStripSeparator());
                    }
                }
            }
            drop.DropDownItems.AddRange(list.ToArray());
            this.changeEnable();
        }

        protected override void Disposing()
        {
            Click = null;
            CheckChanged = null;
            drop.DropDownItems.Clear();
            genObjectOwner = null;
            foreach (var d in dropItems)
            {
                d.Value.Dispose();
            }
            dropItems.Clear();
            CheckChanged = null;
            Click = null;
        }
        public void RefreshFromOwner()
        {
            foreach (var sub in dropItems.Values)
            {
                if (sub is ToolStripMenuItem tool)
                {
                    if (sub.Tag is FieldInfo subField)
                    {
                        var check = (bool)subField.GetValue(genObjectOwner);
                        if (tool.Checked != check)
                        {
                            tool.Checked = check;
                        }
                    }
                    else if (sub.Tag is PropertyInfo subProperty)
                    {
                        var check = (bool)subProperty.GetValue(genObjectOwner);
                        if (tool.Checked != check)
                        {
                            tool.Checked = check;
                        }
                    }
                }
            }
            changeEnable();
        }
        public void RefreshToOwner()
        {
            foreach (var sub in dropItems.Values)
            {
                if (sub is ToolStripMenuItem tool)
                {
                    if (sub.Tag is FieldInfo subField)
                    {
                        subField.SetValue(genObjectOwner, tool.Checked);
                    }
                    else if (sub.Tag is PropertyInfo subProperty)
                    {
                        subProperty.SetValue(genObjectOwner, tool.Checked);
                    }
                }
            }
        }

        private void chk_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem chk)
            {
                if (chk.Tag is FieldInfo field)
                {
                    field.SetValue(genObjectOwner, chk.Checked);
                    s_default_type.Put(field.Name, chk.Checked);
                }
                else if (chk.Tag is PropertyInfo property)
                {
                    property.SetValue(genObjectOwner, chk.Checked);
                    s_default_type.Put(property.Name, chk.Checked);
                }
                this.changeEnable();
                Click?.Invoke(sender, e);
            }
        }

        private void Chk_CheckedChanged(object sender, EventArgs e)
        {
            RefreshToOwner();
            CheckChanged?.Invoke(sender, e);
        }

        private void changeEnable()
        {
            bool changed = false;
            do
            {
                changed = false;
                foreach (var sub in dropItems.Values)
                {
                    if (sub.Tag is FieldInfo subField)
                    {
                        var depends = subField.GetAttributes<DependOnPropertyAttribute>();
                        if (depends != null)
                        {
                            var isdepend = DependOnPropertyAttribute.IsDepend(depends, genObjectOwner);
                            if (sub.Enabled != isdepend)
                            {
                                sub.Enabled = isdepend;
                                changed = true;
                            }
                        }
                    }
                    else if (sub.Tag is PropertyInfo subProperty)
                    {
                        var depends = subProperty.GetAttributes<DependOnPropertyAttribute>();
                        if (depends != null)
                        {
                            var isdepend = DependOnPropertyAttribute.IsDepend(depends, genObjectOwner);
                            if (sub.Enabled != isdepend)
                            {
                                sub.Enabled = isdepend;
                                changed = true;
                            }
                        }
                    }
                }
            }
            while (changed);
        }

        public event EventHandler Click;
        public event EventHandler CheckChanged;
    }
}
