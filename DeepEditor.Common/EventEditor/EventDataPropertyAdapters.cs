using DeepCore.EventTrigger;
using DeepCore.Reflection;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G2D.DataGrid;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using static DeepEditor.Common.G2D.DataGrid.G2DTypeDescriptor;
using static DeepEditor.Common.G2D.G2DTypeFieldDialog;

namespace DeepEditor.Common.EventEditor
{
    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    public class EventTriggerDataAdapters : IG2DPropertyAdapter
    {
        public void OnSetValue(G2DPropertyDescriptor desc, object component, object value)
        {

        }
        public UITypeEditor GetEditor(MemberInfo field, Type fieldType, object ownerData)
        {
            if (field is FieldInfo)
            {
                if (field.TryGetAttribute<GetObjectMemberNameAttribute>(out var get))
                {
                    return new ObjectMemberNameEditor(field as FieldInfo, ownerData, get.BaseOwnerType, get.FieldType, RW.Read);
                }
                if (field.TryGetAttribute<SetObjectMemberNameAttribute>(out var set))
                {
                    return new ObjectMemberNameEditor(field as FieldInfo, ownerData, set.BaseOwnerType, set.FieldType, RW.Write);
                }
            }
            return null;
        }

        public TypeConverter GetConverter(MemberInfo field, Type fieldType, object ownerData)
        {
            return null;
        }
        //-------------------------------------------------------------------------------------
        /// <summary>
        /// AbstractValue 获取对象字段
        /// </summary>
        public class ObjectMemberNameEditor : UITypeEditor
        {
            private readonly FieldInfo fieldInfo;
            private readonly object ownerData;
            private readonly Type fieldType;
            private readonly Type baseType;
            private readonly RW rw;

            public ObjectMemberNameEditor(FieldInfo field, object ownerData, Type baseOwnerType, Type fieldType, RW rw)
            {
                this.baseType = baseOwnerType;
                this.fieldInfo = field;
                this.ownerData = ownerData;
                this.fieldType = fieldType;
                this.rw = rw;
            }
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (baseType != null)
                {
                    var dialog = new G2DTypeFieldDialog(baseType, fieldType, ValueTypeNameSpace.GetCompatibilityTypes(fieldType), rw);
                    //dialog.SelectedType = member.BaseOwnerType;
                    dialog.SelectedField = value?.ToString();
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        value = dialog.SelectedField;
                        return value;
                    }
                    //                     FieldsMap fm = GameFields.Manager.GetFields(baseType, member.FieldType);
                    //                     if (fm != null)
                    //                     {
                    //                         G2DListSelectEditor<MemberDescAttribute> dialog = new G2DListSelectEditor<MemberDescAttribute>(
                    //                             fm.ListFields, fm.GetFieldDesc(value + ""));
                    //                         if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    //                         {
                    //                             value = dialog.SelectedObject.DataField.Name;
                    //                             return value;
                    //                         }
                    //                     }
                }
                return base.EditValue(context, provider, value);
            }
        }
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    public class EventDataAdapters : IG2DPropertyAdapter
    {
        public readonly EventEditor evtEditor;

        public EventDataAdapters(EventEditor evtEditor)
        {
            this.evtEditor = evtEditor;
        }
        public void OnSetValue(G2DPropertyDescriptor desc, object component, object value)
        {

        }


        public UITypeEditor GetEditor(MemberInfo field, Type fieldType, object ownerData)
        {
            if (field is FieldInfo)
            {

                if (PropertyUtil.GetAttribute<EventIDAttribute>(field) != null)
                {
                    return new EventIDEditor(evtEditor, field as FieldInfo, ownerData);
                }
                else if (PropertyUtil.GetAttribute<EnvironmentVarIDAttribute>(field) != null)
                {
                    return new EnvironmentVarIDEditor(evtEditor, field as FieldInfo, ownerData);
                }
                else if (PropertyUtil.GetAttribute<LocalVarTypeAttribute>(field) != null)
                {
                    return new LocalVarIDEditor(evtEditor, field as FieldInfo, ownerData);
                }
            }
            return null;
        }

        public TypeConverter GetConverter(MemberInfo field, Type fieldType, object ownerData)
        {
            return null;
        }


        public class EventIDEditor : UITypeEditor
        {
            private EventEditor scene;
            private FieldInfo fieldInfo;
            private object ownerData;
            private string objName;

            public EventIDEditor(EventEditor scene, FieldInfo field, object ownerData)
            {
                this.scene = scene;
                this.fieldInfo = field;
                this.ownerData = ownerData;
                this.objName = fieldInfo.GetValue(ownerData) as string;
            }
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                var soid = PropertyUtil.GetAttribute<EventIDAttribute>(fieldInfo);
                var sobj = scene.ShowSelectEvent(objName);
                if (sobj != null)
                {
                    return sobj.EventName;
                }
                return base.EditValue(context, provider, value);
            }
        }


        public class EnvironmentVarIDEditor : UITypeEditor
        {
            private EventEditor scene;
            private FieldInfo fieldInfo;
            private object ownerData;
            private string objName;

            public EnvironmentVarIDEditor(EventEditor scene, FieldInfo field, object ownerData)
            {
                this.scene = scene;
                this.fieldInfo = field;
                this.ownerData = ownerData;
                this.objName = fieldInfo.GetValue(ownerData) as string;
            }
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                var svid = PropertyUtil.GetAttribute<EnvironmentVarIDAttribute>(fieldInfo);
                var vobj = scene.Provider.ShowSelectEnvironmentVar(objName, svid.VarType);
                if (vobj != null)
                {
                    return vobj.Key;
                }
                return base.EditValue(context, provider, value);
            }
        }


        public class LocalVarIDEditor : UITypeEditor
        {
            private EventEditor scene;
            private FieldInfo fieldInfo;
            private object ownerData;
            private string objName;

            public LocalVarIDEditor(EventEditor scene, FieldInfo field, object ownerData)
            {
                this.scene = scene;
                this.fieldInfo = field;
                this.ownerData = ownerData;
                this.objName = fieldInfo.GetValue(ownerData) as string;
            }
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                var svid = PropertyUtil.GetAttribute<LocalVarTypeAttribute>(fieldInfo);
                var vobj = scene.ShowSelectLocalVar(objName, svid.VarType);
                if (vobj != null)
                {
                    return vobj.Key;
                }
                return base.EditValue(context, provider, value);
            }
        }




    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
}
