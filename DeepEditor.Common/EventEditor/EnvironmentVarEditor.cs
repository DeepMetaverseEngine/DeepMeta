using DeepCore.EventTrigger;
using DeepEditor.Common.G2D.DataGrid;
using MaterialSkin.Controls;
using DeepEditor.Common.G2D;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DeepEditor.Common.EventEditor
{
    public partial class EnvironmentVarEditor : G2DBaseForm
    {
        readonly private IEventEditorProvider Provider;
        public EnvironmentVarEditor(IEventEditorProvider provider)
        {
            InitializeComponent();
            this.Provider = provider;
            this.Text = provider + " - 变量编辑器";

            LoadData();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            this.SaveData();
        }
        private void LoadData()
        {
            foreach (var var in Provider.LoadEnvironmentVars())
            {
                if (var != null)
                {
                    this.listView1.Items.Add(ToItem(var));
                }
            }
        }

        public void SaveData()
        {
            var saved = new List<IEnvironmentVar>();
            foreach (ListViewItem item in listView1.Items)
            {
                saved.Add(item.Tag as IEnvironmentVar);
            }
            Provider.SaveEnvironmentVars(saved);
        }

        public bool ContainsVarKey(string key)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (key.Equals((item.Tag as IEnvironmentVar).Key))
                {
                    return true;
                }
            }
            return false;
        }

        public string GenVarKey()
        {
            int i = 0;
            while (true)
            {
                string name = "VarName" + i;
                if (!ContainsVarKey(name))
                {
                    return name;
                }
                i++;
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            EnvironmentVarDialog dialog = new EnvironmentVarDialog(Provider, this);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IEnvironmentVar var = dialog.Value;
                this.listView1.Items.Add(ToItem(var));
            }
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in SelectedItems())
            {
                listView1.Items.Remove(item);
            }
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in SelectedItems())
            {
                EnvironmentVarDialog dialog = new EnvironmentVarDialog(Provider, this, item.Tag as IEnvironmentVar);
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SetItem(dialog.Value, item);
                }
                return;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView1.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                EnvironmentVarDialog dialog = new EnvironmentVarDialog(Provider, this, item.Tag as IEnvironmentVar);
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SetItem(dialog.Value, item);
                }
            }
        }

        private ListViewItem[] SelectedItems()
        {
            ListViewItem[] ret = new ListViewItem[listView1.SelectedItems.Count];
            listView1.SelectedItems.CopyTo(ret, 0);
            return ret;
        }

        private ListViewItem ToItem(IEnvironmentVar var)
        {
            ListViewItem item = new ListViewItem(new string[] {
                var.Key,
                Provider.NameSpace.GetValueType(var.Value.GetType()).ToString(),
                var.Value.ToString(),
                var.SyncToClient?"是":"否"
            });
            item.Tag = var;
            item.ImageIndex = var.SyncToClient ? 1 : 0;
            return item;
        }

        private void SetItem(IEnvironmentVar var, ListViewItem item)
        {
            item.SubItems[0].Text = var.Key;
            item.SubItems[1].Text = Provider.NameSpace.GetValueType(var.Value.GetType()).ToString();
            item.SubItems[2].Text = var.Value.ToString();
            item.SubItems[3].Text = var.SyncToClient ? "是" : "否";
            item.Tag = var;
            item.ImageIndex = var.SyncToClient ? 1 : 0;
        }

    }
}
