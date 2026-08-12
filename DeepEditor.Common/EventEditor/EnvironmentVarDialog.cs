using DeepCore.EventTrigger;
using DeepCore.EventTrigger.Data;
using DeepCore.Xml;
using DeepEditor.Common.EventEditor.DescAttributeEdit;
using DeepEditor.Common.G2D;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DeepEditor.Common.EventEditor
{


    public partial class EnvironmentVarDialog : G2DBaseForm
    {
        readonly private IEventEditorProvider Provider;
        readonly private EnvironmentVarEditor varEdit;
        readonly private bool IsAdd;
        readonly private IEnvironmentVar SrcVar;
        private IEnvironmentVar mData;
        public IEnvironmentVar Value
        {
            get
            {
                return mData;
            }
        }

        public EnvironmentVarDialog(IEventEditorProvider provider, EnvironmentVarEditor vedit, IEnvironmentVar var = null)
        {
            InitializeComponent();
            this.varEdit = vedit;
            this.Provider = provider;
            this.IsAdd = var == null;
            this.SrcVar = var;
            this.comboBox1.Items.AddRange(provider.NameSpace.ValueTypes.ToArray());

            if (var != null && var.Value != null)
            {
                this.mData = XmlUtil.CloneObject<IEnvironmentVar>(var);
                this.comboBox1.Enabled = false;
            }
            else
            {
                this.mData = provider.CreateEnvironmentVar();
                this.mData.Key = varEdit.GenVarKey();
                this.mData.Value = new IntegerValue.VALUE();
                if (var != null)
                {
                    this.mData.SyncToClient = var.SyncToClient;
                }
            }
            this.comboBox1.SelectedItem = provider.NameSpace.GetValueType(mData.Value.GetType());
            this.richTextBox1.Text = this.mData.Value.ToString();
            this.textBox1.Text = this.mData.Key;
            this.chk_Sync.Checked = this.mData.SyncToClient;


            this.comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            this.textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text.Trim();
            this.mData.Key = text;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var baseType = this.comboBox1.SelectedItem as ValueTypeNameSpace.ValueTypeDefine;
            if (baseType != null)
            {
                this.mData.Value = Provider.NameSpace.MakeDefault(baseType.ValueType.OwnerType);
                this.richTextBox1.Text = this.mData.Value.ToString();
            }
        }
        private void chk_Sync_CheckedChanged(object sender, EventArgs e)
        {
            this.mData.SyncToClient = chk_Sync.Checked;
        }


        private void SceneVarDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string text = textBox1.Text.Trim();
                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("名字不能为空");
                    e.Cancel = true;
                }
                else if (IsAdd)
                {
                    if (varEdit.ContainsVarKey(Value.Key))
                    {
                        MessageBox.Show("变量名重复");
                        e.Cancel = true;
                    }
                }
                else if (!Value.Key.Equals(SrcVar.Key))
                {
                    if (varEdit.ContainsVarKey(Value.Key))
                    {
                        MessageBox.Show("变量名重复");
                        e.Cancel = true;
                    }
                }
            }
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            var baseType = this.comboBox1.SelectedItem as ValueTypeNameSpace.ValueTypeDefine;
            if (baseType != null)
            {
                object result = ValueTypeDialog.ShowEditDialog(this,
                    baseType.ValueType.OwnerType,
                    mData.Value,
                    Provider.PropertyAdapters);
                if (result != null)
                {
                    this.mData.Value = result;
                    this.richTextBox1.Text = result.ToString();
                }
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {

        }




    }
}
