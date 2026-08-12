using DeepCore;
using DeepCore.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.Controls
{
    public abstract class ExclusionButtonGroup<C> where C : Component
    {
        private ListDictionary<C, Field> btns = new ListDictionary<C, Field>();
        public IEnumerable<C> Buttons { get => btns.Keys; }

        public void Invalidate()
        {
            foreach (var btn in Buttons)
            {
                if (btn is Control c)
                {
                    c.Invalidate();
                }
            }
        }

        public void AddButton(C btn)
        {
            btns.Add(btn, null);
            RegistClick(btn);
        }
        /// <summary>
        /// 绑定某个类的bool字段
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="ownerType"></param>
        /// <param name="fieldName"></param>
        public void AddButton(C btn, Type ownerType, string fieldName)
        {
            var fi = ownerType.GetField(fieldName);
            btns.Add(btn, new Field() { ownerType = ownerType, field = fi });
            RegistClick(btn);
        }
        /// <summary>
        /// 绑定某个类的bool字段
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="owner"></param>
        /// <param name="fieldName"></param>
        public void AddButton(C btn, object owner, string fieldName)
        {
            var ownerType = owner.GetType();
            var fi = ownerType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (fi.TryGetAttribute<DescAttribute>(out var desc))
            {
                if (btn is ToolStripItem ts)
                {
                    ts.Text = desc.Desc;
                    ts.Text = $"{desc.Desc}\r\n{desc.Detail}";
                }
            }
            btns.Add(btn, new Field() { ownerType = ownerType, owner = owner, field = fi });
            RegistClick(btn);
        }

        public void AddButtons(params C[] btns)
        {
            foreach (var btn in btns)
            {
                this.AddButton(btn);
            }
        }

        public void RefreshFromOwner()
        {
            foreach (var btn in btns)
            {
                try
                {
                    var field = btn.Value;
                    if (field != null && field.field != null)
                    {
                        var value = field.field.GetValue(field.owner);
                        SetChecked(btn.Key, (bool)value);
                    }
                }
                catch { }
            }
        }
        public void RefreshToOwner()
        {
            foreach (var btn in btns)
            {
                try
                {
                    var field = btn.Value;
                    if (field != null && field.field != null)
                    {
                        var value = GetChecked(btn.Key);
                        field.field.SetValue(field.owner, value);
                    }
                }
                catch { }
            }
        }
        public void RefreshToOwner(C btn)
        {
            try
            {
                if (btns.TryGetValue(btn, out var field))
                {
                    if (field != null && field.field != null)
                    {
                        var value = GetChecked(btn);
                        field.field.SetValue(field.owner, value);
                    }
                }
            }
            catch { }
        }

        protected void OnButtonClick(object sender, EventArgs e)
        {
            foreach (var btn in btns)
            {
                if (sender != btn.Key)
                {
                    SetChecked(btn.Key, false);
                }
            }
            Click?.Invoke(sender, e);
        }
        protected void OnButtonCheckChanged(object sender, EventArgs e)
        {
            if (sender is C c)
            {
                RefreshToOwner(c);
            }
            CheckChanged?.Invoke(sender, e);
       
        }

        protected abstract void RegistClick(C btn);
        protected abstract void SetChecked(C btn, bool @checked);
        protected abstract bool GetChecked(C btn);

        public event EventHandler Click;
        public event EventHandler CheckChanged;

        private class Field
        {
            public Type ownerType;
            public object owner;
            public FieldInfo field;
        }

    }

    public class ExclusionToolStripButtonGroup : ExclusionButtonGroup<ToolStripButton>
    {
        public ExclusionToolStripButtonGroup(params ToolStripButton[] btns)
        {
            AddButtons(btns);
        }

        protected override void SetChecked(ToolStripButton btn, bool @checked)
        {
            if (btn.Checked != @checked)
            {
                btn.Checked = @checked;
            }
        }
        protected override bool GetChecked(ToolStripButton btn)
        {
            return btn.Checked;
        }
        protected override void RegistClick(ToolStripButton btn)
        {
            btn.Click += base.OnButtonClick;
            btn.CheckedChanged += base.OnButtonCheckChanged;
        }
    }
    public class ExclusionToolStripMenuItemGroup : ExclusionButtonGroup<ToolStripMenuItem>
    {
        public ExclusionToolStripMenuItemGroup(params ToolStripMenuItem[] btns)
        {
            AddButtons(btns);
        }
        protected override void SetChecked(ToolStripMenuItem btn, bool @checked)
        {
            if (btn.Checked != @checked)
            {
                btn.Checked = @checked;
            }
        }
        protected override bool GetChecked(ToolStripMenuItem btn)
        {
            return btn.Checked;
        }
        protected override void RegistClick(ToolStripMenuItem btn)
        {
            btn.Click += base.OnButtonClick;
            btn.CheckedChanged += base.OnButtonCheckChanged;
        }
    }

}
