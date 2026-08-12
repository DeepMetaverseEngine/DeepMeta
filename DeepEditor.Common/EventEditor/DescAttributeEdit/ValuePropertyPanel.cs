using DeepCore;
using DeepCore.Reflection;
using DeepEditor.Common.G2D.DataGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using static DeepEditor.Common.G2D.DataGrid.G2DTypeDescriptor;

namespace DeepEditor.Common.EventEditor.DescAttributeEdit
{
    public partial class ValuePropertyPanel : UserControl
    {
        public object EditValue { get; private set; }

        private List<IG2DPropertyAdapter> value_adapters = new List<IG2DPropertyAdapter>();


        public ValuePropertyPanel()
        {
            InitializeComponent();
            this.propertyGrid1.PropertySort = PropertySort.Categorized;
        }

        public void SetAdapters(IG2DPropertyAdapter[] adapters)
        {
            foreach (var ad in adapters)
            {
                if (ad != null)
                {
                    value_adapters.Add(ad);
                }
            }
        }

        public void SetValue(object obj)
        {
            this.EditValue = obj;
            if (obj == null)
            {
                this.propertyGrid1.SelectedObject = null;
            }
            else
            {
                this.propertyGrid1.SelectedObject = G2DTypeDescriptor.CreateDescriptor(obj, value_adapters.ToArray());
            }
        }

        class ValuePanelAdapter : IG2DPropertyAdapter
        {
            private IG2DPropertyAdapter[] adapters;
            private Type eventClass = typeof(DeepCore.EventTrigger.EventExternalizable);
            public ValuePanelAdapter(IG2DPropertyAdapter[] adapters)
            {
                this.adapters = adapters;
            }
            public void OnSetValue(G2DPropertyDescriptor desc, object component, object value)
            {

            }
            public UITypeEditor GetEditor(MemberInfo field, Type fieldType, object ownerData)
            {
                if (fieldType.IsSubclassOf(eventClass))
                {
                    return new ValuePanelUITypeEditor(adapters, field, ownerData);
                }
                if (fieldType.IsGenericList())
                {
                    if (fieldType.GetGenericArguments()[0].IsSubclassOf(eventClass))
                    {
                        return new ValuePanelUITypeEditor(adapters, field, ownerData);
                    }
                }
                if (fieldType.IsArray)
                {
                    if (fieldType.GetElementType().IsSubclassOf(eventClass))
                    {
                        return new ValuePanelUITypeEditor(adapters, field, ownerData);
                    }
                }
                return null;
            }
            public TypeConverter GetConverter(MemberInfo field, Type fieldType, object ownerData)
            {
                return null;
            }
            class ValuePanelUITypeEditor : G2DMemberUITypeEditor
            {
                private IG2DPropertyAdapter[] adapters;
                //private DescAttribute desc;

                public ValuePanelUITypeEditor(IG2DPropertyAdapter[] adapters, MemberInfo field, object ownerData)
                    : base(field, ownerData)
                {
                    this.adapters = CUtils.ArrayLink<IG2DPropertyAdapter>(adapters, new ValuePanelAdapter(adapters));
                    //this.desc = PropertyUtil.GetAttribute<DescAttribute>(field);
                }
                public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
                {
                    if (fieldType.IsPrimitive)
                    {
                        return UITypeEditorEditStyle.None;
                    }
                    else if (fieldType.IsEnum)
                    {
                        return UITypeEditorEditStyle.None;
                    }
                    else if (fieldType.Equals(typeof(string)))
                    {
                        return UITypeEditorEditStyle.None;
                    }
                    else if (fieldType.IsClass)
                    {
                        return UITypeEditorEditStyle.Modal;
                    }
                    else if (fieldType.IsArray)
                    {
                        return UITypeEditorEditStyle.Modal;
                    }
                    return UITypeEditorEditStyle.Modal;
                }
                public override object EditValue(
                    ITypeDescriptorContext context,
                    IServiceProvider provider,
                    object value)
                {
                    ListDescAttribute listattr = PropertyUtil.GetAttribute<ListDescAttribute>(fieldInfo);
                    if (listattr != null)
                    {
                        G2DCollectionEditor editor = new G2DCollectionEditor(
                            fieldType,
                            value,
                            listattr.GetElementTypes(fieldType),
                            adapters);
                        if (editor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            return editor.GetEditCompleteData();
                        }
                    }
                    else
                    {
                        object result = ValueTypeDialog.ShowValueDialog(
                            null, fieldType, value, adapters);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                    return base.EditValue(context, provider, value);
                }
            }
        }

    }
}
