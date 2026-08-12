using DeepCore;
using DeepCore.Reflection;
using DeepCore.Xml;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G2D.DataGrid;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace DeepEditor.Common.EventEditor.DescAttributeEdit
{
    /// <summary>
    /// 用于选择或创建，所有对应其子类的实例
    /// 该选择界面先对注释为 [DescAttribute] 的 Catgory 分类，再进行子类划分
    /// </summary>
    public partial class ValueTypeDialog : G2DBaseForm
    {
        private readonly Type baseType;
        // private Catgorys baseTypes;
        private object editValue;
        private TypeNode lastSelectNode = null;
        private HashMap<Type, object> editHistory = new HashMap<Type, object>();

        public ValueTypeDialog(Type baseType, object edit, IG2DPropertyAdapter[] adapters)
        {
            this.baseType = baseType;
            //             this.baseTypes = Catgorys.GetCatgory(baseType);

            InitializeComponent();

            this.SuspendLayout();
            {
                if (edit != null)
                {
                    edit = XmlUtil.CloneObject<object>(edit);
                }
                this.editValue = edit;
                this.valuePanel1.SetAdapters(adapters);
                //  this.lastSelectNode = null;
                //                 foreach (string catgory in baseTypes.CatgoryNames)
                //                 {
                //                     var catgoryGroup = new CatgoryGroupNode(catgory);
                //                     treeView1.Nodes.Add(catgoryGroup);
                //                     foreach (var valueType in baseTypes.GetCatgoryTypes(catgory))
                //                     {
                //                         var tn = new TypeNode(valueType);
                //                         catgoryGroup.Nodes.Add(tn);
                //                         if (editValue != null)
                //                         {
                //                             if (tn.ValueType == editValue.GetType())
                //                             {
                //                                 catgoryGroup.ExpandAll();
                //                                 lastSelectNode = tn;
                //                             }
                //                         }
                //                         else if (lastSelectNode == null)
                //                         {
                //                             lastSelectNode = tn;
                //                         }
                //                     }
                //                 }

                treeView1.Init(baseType);
                //                 treeView1.TreeViewNodeSorter = new ValueComparer<TreeNode>((a, b) =>
                //                 {
                //                     return a.Text.CompareTo(b.Text);
                //                 });

                //                 treeView1.Sort();
                //                 treeView1.CheckBoxes = false;
                //                 treeView1.ItemHeight = 22;
            }
            this.ResumeLayout();
            try
            {
                var file = new System.IO.FileInfo($"{Application.UserAppDataPath}\\{$"{this.GetType().Name}.{baseType.ToSaveFullName()}.tree"}");
                treeView1.TreeView.LoadState(file, new TreeStateInfoConfig()
                {
                    removeEmptyGroup = true,
                    reIndex = false,
                    select = false,
                });
            }
            catch (Exception ex)
            {
                ex.PrintStackTrace();
            }
            if (editValue != null)
            {
                if (treeView1.TryGetTypeNode(editValue.GetType(), out lastSelectNode))
                {
                    treeView1.SelectedNode = lastSelectNode;
                    treeView1.SelectedNode.EnsureVisible();
                }
                SetEditValue(editValue, false);
            }
            //this.treeView1.Sort();
            treeView1.AfterSelect += this.treeView1_AfterSelect;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            {
                //                 else if (treeView1.DefaultNode != null)
                //                 {
                //                     SetEditValue(treeView1.DefaultNode.ValueType);
                //                     lastSelectNode = treeView1.DefaultNode;
                //                 }
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                var file = new System.IO.FileInfo($"{Application.UserAppDataPath}\\{$"{this.GetType().Name}.{baseType.ToSaveFullName()}.tree"}");
                treeView1.TreeView.SaveState(file);
            }
            catch (Exception ex)
            {
                ex.PrintStackTrace();
            }
            base.OnFormClosing(e);
        }

        //         public TypeDescAttribute SelectedTypeDesc
        //         {
        //             get { return lastSelectNode.TypeDesc; }
        //         }
        //         public Type SelectedType
        //         {
        //             get { return lastSelectNode.ValueType; }
        //         }
        public object EditValue
        {
            get { return editValue; }
        }

        private void SetEditValue(Type selected)
        {
            bool is_new = false;
            if (!editHistory.TryGetValue(selected, out var value))
            {
                is_new = true;
                value = ReflectionUtil.CreateInstance(selected);
            }
            SetEditValue(value, is_new);
        }
        private void SetEditValue(object value, bool is_new)
        {
            this.editValue = value;
            valuePanel1.SetValue(editValue);
            editHistory.Put(editValue.GetType(), editValue);
            this.Text = value == null ? baseType.Name : value.ToString();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is TypeNode valueType)
            {
                if (lastSelectNode == null || lastSelectNode.ValueType != valueType.ValueType)
                {
                    SetEditValue(valueType.ValueType);
                }
                lastSelectNode = valueType;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }
        public static T ShowAddDialog<T>(IWin32Window owner, params IG2DPropertyAdapter[] adapters) where T : class
        {
            ValueTypeDialog form = new ValueTypeDialog(typeof(T), null, adapters);
            if (form.ShowDialog(owner) == DialogResult.OK)
            {
                return (T)form.EditValue;
            }
            return null;
        }
        public static object ShowAddDialog(IWin32Window owner, Type baseType, params IG2DPropertyAdapter[] adapters)
        {
            ValueTypeDialog form = new ValueTypeDialog(baseType, null, adapters);
            if (form.ShowDialog(owner) == DialogResult.OK)
            {
                return form.EditValue;
            }
            return null;
        }

        public static object ShowEditDialog(IWin32Window owner, Type baseType, object edit, params IG2DPropertyAdapter[] adapters)
        {
            ValueTypeDialog form = new ValueTypeDialog(baseType, edit, adapters);
            if (form.ShowDialog(owner) == DialogResult.OK)
            {
                return form.EditValue;
            }
            return null;
        }

        public static object ShowValueDialog(IWin32Window owner, Type baseType, object edit, params IG2DPropertyAdapter[] adapters)
        {
            ValueTypeDialog form = new ValueTypeDialog(baseType, edit, adapters);
            if (form.ShowDialog(owner) == DialogResult.OK)
            {
                return form.EditValue;
            }
            return null;
        }

        //         class FieldHistory
        //         {
        //             class Entry
        //             {
        //                 Type type;
        //                 string fieldName;
        //                 int fieldIndex;
        //                 object fieldValue;
        //             }
        // 
        //             public void FindFieldValue(FieldInfo field)
        //             {
        // 
        //             }
        //         }

    }
}
