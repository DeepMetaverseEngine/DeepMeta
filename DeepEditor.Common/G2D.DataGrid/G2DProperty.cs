using System;
using System.Collections.Generic;

using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using DeepCore.Reflection;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using DeepCore;
using DeepCore.IO;
using System.IO;
using DeepCore.Components;
using DeepCore.FuncData;
using static DeepEditor.Common.G2D.DataGrid.G2DTypeDescriptor;
using System.Linq;

namespace DeepEditor.Common.G2D.DataGrid
{
    //--------------------------------------------------------------------------------------

    public delegate UITypeEditor GetEditor(MemberInfo field, Type fieldType, object ownerData);
    public delegate TypeConverter GetConverter(MemberInfo field, Type fieldType, object ownerData);
    public delegate bool AcceptField(G2DTypeDescriptor desc, MemberInfo field, object ownerData);
    public delegate void OnCommitHandler(G2DPropertyDescriptor prop, object component, object value);
    public delegate void OnSetValueHandler(G2DPropertyDescriptor prop, object component, object value);
    //--------------------------------------------------------------------------------------
    [Reflectible]
    public interface IG2DPropertyAdapter
    {
        UITypeEditor GetEditor(MemberInfo field, Type fieldType, object ownerData);
        TypeConverter GetConverter(MemberInfo field, Type fieldType, object ownerData);
        void OnSetValue(G2DPropertyDescriptor desc, object component, object value);
    }

    public enum G2DPropertyFieldDisplayStyle
    {
        FieldName,
        DescName,
        FieldName_DescName,
    }
    //--------------------------------------------------------------------------------------

    [Reflectible]
    public class G2DTypeDescriptor : ICustomTypeDescriptor
    {
        public static G2DPropertyFieldDisplayStyle SHOW_DISPLAY_NAME = G2DPropertyFieldDisplayStyle.FieldName_DescName;
        //-------------------------------------------------------------------------------------------------------
        #region StaticUtils

        public delegate bool TryGetCategory(object componentData, MemberInfo member, out string category);
        public delegate bool TryGetDisplayName(object componentData, MemberInfo member, out string displayName);
        public delegate bool TryGetEditor(object componentData, MemberInfo member, Type editorBaseType, out object editor);
        public delegate bool TryGetConverter(object componentData, MemberInfo member, out TypeConverter converter);
        private static TryGetCategory sevent_TryGetCategory;
        private static TryGetDisplayName sevent_TryGetDisplayName;
        private static TryGetEditor sevent_TryGetEditor;
        private static TryGetConverter sevent_TryGetConverter;
        public static void RegistDelegate(TryGetCategory evt) { sevent_TryGetCategory += evt; }
        public static void RegistDelegate(TryGetDisplayName evt) { sevent_TryGetDisplayName += evt; }
        public static void RegistDelegate(TryGetEditor evt) { sevent_TryGetEditor += evt; }
        public static void RegistDelegate(TryGetConverter evt) { sevent_TryGetConverter += evt; }

        private static List<IG2DPropertyAdapter> static_adapters = new List<IG2DPropertyAdapter>();
        public static void RegistPropertyAdapter(IG2DPropertyAdapter adapter)
        {
            if (!static_adapters.Contains(adapter))
            {
                static_adapters.Add(adapter);
            }
        }
        public static void RegistMemberAttributeEditor(Type attributeType, GetEditor editor)
        {
            RegistDelegate(new TryGetEditor((object componentData, MemberInfo member, Type editorBaseType, out object oeditor) =>
            {
                if (member.GetAttributeByType(attributeType) != null)
                {
                    oeditor = editor(member, editorBaseType, componentData);
                    return oeditor != null;
                }
                oeditor = null;
                return false;
            }));
        }
        public static void RegistMemberAttributeConverter(Type attributeType, GetConverter converter)
        {
            RegistDelegate(new TryGetEditor((object componentData, MemberInfo member, Type editorBaseType, out object oeditor) =>
            {
                if (member.GetAttributeByType(attributeType) != null)
                {
                    oeditor = converter(member, editorBaseType, componentData);
                    return oeditor != null;
                }
                oeditor = null;
                return false;
            }));
        }

        public static GridItemCollection GetAllGridEntries(PropertyGrid grid)
        {
            if (grid == null)
                throw new ArgumentNullException("grid");
            object view = grid.GetType().GetField(
                "gridView",
                BindingFlags.NonPublic | BindingFlags.Instance).GetValue(grid);
            return (GridItemCollection)view.GetType().InvokeMember(
                "GetAllGridEntries",
                BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                null, view, null);
        }

        public static void ToDescription(StringBuilder sb, Type fieldType, object fieldValue)
        {
            if (fieldValue != null)
            {
                //$"{Data.GetType().FullName}\r\n{Data.ToString()}";
                sb.AppendLine(fieldValue.GetType().ToTypeDefineFullName());
                if (fieldValue.GetType().IsArray)
                {
                    sb.AppendLine(CUtils.ArrayToString((Array)fieldValue));
                }
                else if (fieldValue.GetType().IsInterfaceOf(typeof(IDictionary)))
                {
                    sb.AppendLine(CUtils.MapToString((IDictionary)fieldValue));
                }
                else if (fieldValue.GetType().IsInterfaceOf(typeof(IList)))
                {
                    sb.AppendLine(CUtils.ListToString((IList)fieldValue));
                }
                else
                {
                    if (fieldValue.GetType().IsEnum)
                    {
                        DescAttribute eda = PropertyUtil.GetEnumAttribute<DescAttribute>((Enum)fieldValue);
                        if (eda != null)
                        {
                            sb.AppendLine(fieldValue + " : " + eda.Desc);
                        }
                        else
                        {
                            sb.AppendLine(fieldValue + "");
                        }
                    }
                    else
                    {
                        sb.AppendLine(fieldValue + "");
                    }
                }
            }
            else
            {
                sb.AppendLine(fieldType.FullName);
            }
        }


        #endregion
        //-------------------------------------------------------------------------------------------------------
        static G2DTypeDescriptor()
        {
            RegistPropertyAdapter(new InternalDataAdapters());
            //RegistPropertyAdapter(new FuncEditor.FuncDataPropertyAdapter());
            RegistPropertyAdapter(new EventEditor.EventTriggerDataAdapters());
        }
        public event OnCommitHandler OnCommit;
        public event OnSetValueHandler OnSetValue;
        private object Data;
        private List<IG2DPropertyAdapter> adapter_list = new List<IG2DPropertyAdapter>();
        private List<IG2DPropertyAdapter> add_adapters = new List<IG2DPropertyAdapter>();
        public static G2DTypeDescriptor CreateDescriptor(object obj, params IG2DPropertyAdapter[] adds)
        {
            if (obj == null) return null;
            return new G2DTypeDescriptor(obj, adds);
        }
        protected G2DTypeDescriptor(object obj, params IG2DPropertyAdapter[] adds)
        {
            this.EnablePropertyInfo = true;
            this.EnableFieldInfo = true;
            this.Data = obj;
            foreach (var sa in static_adapters)
            {
                if (sa != null)
                {
                    this.AddPropertyAdapter(sa);
                }
            }
            foreach (var sa in adds)
            {
                if (sa != null)
                {
                    add_adapters.Add(sa);
                    this.AddPropertyAdapter(sa);
                }
            }
        }

        public object EditData
        {
            get { return Data; }
            internal set { Data = value; }
        }
        public bool EnablePropertyInfo { get; set; }
        public bool EnableFieldInfo { get; set; }

        public override string ToString()
        {
            return Data.ToString();
        }

        public void AddPropertyAdapter(params IG2DPropertyAdapter[] adapters)
        {
            foreach (var adapter in adapters)
            {
                if (!adapter_list.Contains(adapter))
                {
                    adapter_list.Add(adapter);
                }
            }
        }

        public virtual bool IsNeedG2DEditor(Type listType)
        {
            if (listType.IsArray && listType.GetArrayRank() == 1)
            {
                return IsNeedG2DCollectionEditor(listType);
            }
            else if (typeof(IList).IsAssignableFrom(listType))
            {
                return IsNeedG2DCollectionEditor(listType);
            }
            //             else if (typeof(DataComponentCollection).IsAssignableFrom(listType))
            //             {
            //                 return IsNeedG2DCollectionEditor(listType);
            //             }
            else if (typeof(IDictionary).IsAssignableFrom(listType))
            {
                return IsNeedG2DCollectionEditor(listType);
            }
            if (listType.Equals(typeof(string)))
            {
                return false;
            }
            if (listType.Equals(typeof(DateTime)))
            {
                return false;
            }
            if (listType.Equals(typeof(TimeSpan)))
            {
                return false;
            }
            if (listType.IsArray)
            {
                return false;
            }
            if (listType.IsPrimitive)
            {
                return false;
            }
            return true;
        }

        public virtual bool IsNeedG2DCollectionEditor(Type listType)
        {
            if (listType.IsArray && listType.GetArrayRank() == 1)
            {
                Type elementType = listType.GetElementType();
                return IsNeedG2DEditor(elementType);
            }
            else if (listType.IsInterfaceOf(typeof(IList)))
            {
                //                 if (listType.IsGenericType)
                //                 {
                //                     Type elementType = listType.GetGenericArguments()[0];
                //                     if (elementType.Equals(typeof(string))) { return true; }
                //                     return IsNeedG2DEditor(elementType);
                //                 }
                //                 else
                {
                    return true;
                }
            }
            //             else if (typeof(DataComponentCollection).IsAssignableFrom(listType))
            //             {
            //                 //                 if (listType.IsGenericType)
            //                 //                 {
            //                 //                     Type elementType = listType.GetGenericArguments()[0];
            //                 //                     if (elementType.Equals(typeof(string))) { return true; }
            //                 //                     return IsNeedG2DEditor(elementType);
            //                 //                 }
            //                 //                 else
            //                 {
            //                     return true;
            //                 }
            //             }
            else if (listType.IsInterfaceOf(typeof(IDictionary)))
            {
                if (listType.IsGenericType)
                {
                    var gtypes = listType.GetGenericArguments();
                    if (gtypes[0].IsPrimitive || gtypes[0].IsEnum || gtypes[0] == typeof(string))
                    {
                        return true;//IsNeedG2DEditor(gtypes[1]);
                    }
                }
            }
            return false;
        }

        //------------------------------------------------------------------

        //------------------------------------------------------------------
        #region OptionalList
        private HashMap<string, OptionalList> optional_map = new HashMap<string, OptionalList>();

        public HashMap<string, List<object>> GetOptionalsMap()
        {
            var ret = new HashMap<string, List<object>>();
            foreach (var e in optional_map)
            {
                ret.Add(e.Key, new List<object>(e.Value.Values));
            }
            return ret;
        }
        public void AppendOptionals(IDictionary<string, OptionalList> optionals)
        {
            foreach (var e in optionals)
            {
                AppendOptionals(e);
            }
        }
        public void AppendOptionals(KeyValuePair<string, OptionalList> optionals)
        {
            AppendOptionals(optionals.Key, optionals.Value);
        }
        public void AppendOptionals(string fieldName, OptionalList optionals)
        {
            var list = optional_map.GetOrAdd(fieldName, e => new OptionalList());
            list.AddRange(optionals);
        }
        public void SetOptionalConverter(string fieldName, Func<MemberInfo, object, object> converter)
        {
            var list = optional_map.GetOrAdd(fieldName, e => new OptionalList());
            list.Converter = converter;
        }


        public void AppendOptionals(IDictionary<string, List<object>> optionals)
        {
            foreach (var e in optionals)
            {
                AppendOptionals(e);
            }
        }
        public void AppendOptionals(KeyValuePair<string, List<object>> optionals)
        {
            AppendOptionals(optionals.Key, optionals.Value);
        }
        public void AppendOptionals(string fieldName, List<object> optionals)
        {
            var list = optional_map.GetOrAdd(fieldName, e => new OptionalList());
            list.AddRange(optionals);
        }
        public void AppendOptionalsFromHistoryObject(object obj)
        {
            foreach (FieldInfo f in obj.GetType().GetFields())
            {
                if (f.FieldType.IsPrimitive || f.FieldType == typeof(string))
                {
                    AppendOptional(f.Name, f.GetValue(obj));
                }
            }
        }
        public void AppendOptional(string fieldName, object value)
        {
            var list = optional_map.GetOrAdd(fieldName, e => new OptionalList());
            list.Add(value);
        }


        #endregion
        //------------------------------------------------------------------

        #region ICustomTypeDescriptor 成员

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(Data, true);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(Data, true);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(Data, true);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(Data, true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(Data, true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(Data, true);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(Data, editorBaseType, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(Data, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(Data, true);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return ListPropertyDescriptors(Data, this, attributes);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ListPropertyDescriptors(Data, this, new Attribute[0]);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return Data;
        }

        #endregion

        //------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------------------------
        public event AcceptField TryAcceptField;
        public virtual PropertyDescriptorCollection ListPropertyDescriptors(object data, G2DTypeDescriptor g2ddesc, Attribute[] attributes)
        {
            var type = data.GetType();
            var ret = new List<PropertyDescriptor>();
            if (type.IsArray && type.GetArrayRank() == 1)
            {
                Array array = data as Array;
                Type elementType = type.GetElementType();
                for (int i = 0; i < array.Length; i++)
                {
                    ret.Add(new ArrayItemPropertyDescriptor(g2ddesc, array, i, attributes));
                }
            }
            else if (type.IsInterfaceOf(typeof(IList)))
            {
                IList list = data as IList;
                if (type.IsGenericType)
                {
                    Type elementType = type.GetGenericArguments()[0];
                    for (int i = 0; i < list.Count; i++)
                    {
                        ret.Add(new ListItemPropertyDescriptor(g2ddesc, list, i, attributes));
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        ret.Add(new ListItemPropertyDescriptor(g2ddesc, list, i, attributes));
                    }
                }
            }
            else if (type.IsInterfaceOf(typeof(IDictionary)))
            {
                IDictionary map = data as IDictionary;
                if (type.IsGenericType)
                {
                    foreach (var k in map.Keys)
                    {
                        ret.Add(new MapItemPropertyDescriptor(g2ddesc, map, k, attributes));
                    }
                }
            }
            else if ((data == Data) && (type.IsEnum))
            {
                ret.Add(new EnumPropertyDescriptor(g2ddesc, Data, attributes));
            }
            else
            {
                if (EnableFieldInfo)
                {
                    FieldInfo[] fields = type.GetFields();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        FieldInfo field = fields[i];
                        //                         if (FuncDataManager.ENABLE_FUNC_DATA == false)
                        //                         {
                        //                             if (typeof(FuncTable).IsAssignableFrom(field.FieldType))
                        //                             {
                        //                                 continue;
                        //                             }
                        //                         }
                        if (!field.IsStatic)
                        {
                            if (TryAcceptField == null || TryAcceptField.Invoke(this, field, data))
                            {
                                DescAttribute desc = PropertyUtil.GetDesc(field);
                                if (desc == null || desc.Editable)
                                {
                                    Type fieldType = field.FieldType;
                                    object fieldData = field.GetValue(data);
                                    if (fieldData != null)
                                    {
                                        fieldType = fieldData.GetType();
                                    }
                                    ret.Add(new FieldMemberDescriptor(g2ddesc, i, field, data, attributes));
                                }
                            }
                        }

                    }
                }
                if (EnablePropertyInfo)
                {
                    PropertyInfo[] props = type.GetProperties();
                    for (int i = 0; i < props.Length; i++)
                    {
                        PropertyInfo field = props[i];
                        //                         if (FuncDataManager.ENABLE_FUNC_DATA == false)
                        //                         {
                        //                             if (typeof(FuncTable).IsAssignableFrom(field.PropertyType))
                        //                             {
                        //                                 continue;
                        //                             }
                        //                         }
                        if (field.CanRead && !field.GetMethod.IsStatic)
                        {
                            if (TryAcceptField == null || TryAcceptField.Invoke(this, field, data))
                            {
                                DescAttribute desc = PropertyUtil.GetDesc(field);
                                if (desc != null && desc.Editable)
                                {
                                    Type fieldType = field.PropertyType;
                                    object fieldData = PropertyUtil.GetMemberValue(field, data); ;//field.GetValue(data);
                                    if (fieldData != null)
                                    {
                                        fieldType = fieldData.GetType();
                                    }
                                    ret.Add(new PropertyMemberDescriptor(g2ddesc, i, field, data, attributes));
                                }
                            }
                        }
                    }
                }
                ret.Sort((a, b) => a.DisplayName.CompareTo(b.DisplayName));
            }
            var result = new PropertyDescriptorCollection(ret.ToArray(), true);
            var propertyNames = new List<string>();
            foreach (PropertyDescriptor propertyAttributes in ret)
            {
                propertyNames.Add(propertyAttributes.DisplayName);
            }
            propertyNames.Sort();
            return result.Sort(propertyNames.ToArray());
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------
        #region FieldPropertyDescriptor--------------------------------------------------------------------------------------------------------------------------------
        void DoCommit(G2DPropertyDescriptor prop, object comp, object value)
        {
            OnCommit?.Invoke(prop, comp, value);
        }
        void DoSetValue(G2DPropertyDescriptor prop, object comp, object value)
        {
            OnSetValue?.Invoke(prop, comp, value);
        }
        //------------------------------------------------------------------
        public abstract class G2DPropertyDescriptor : PropertyDescriptor
        {
            public readonly G2DTypeDescriptor parent;
            protected G2DPropertyDescriptor(G2DTypeDescriptor parent, string name, Attribute[] attrs) : base(name, attrs)
            {
                this.parent = parent;
            }
            public abstract void OnSetValue(object component, object value);
            sealed public override void SetValue(object component, object value)
            {
                foreach (IG2DPropertyAdapter adapter in parent.adapter_list)
                {
                    adapter.OnSetValue(this, component, value);
                }
                parent.DoCommit(this, component, value);
                this.OnSetValue(component, value);
                parent.DoSetValue(this, component, value);
            }
        }
        public abstract class G2DOwnerPropertyDescriptor : G2DPropertyDescriptor
        {
            protected G2DOwnerPropertyDescriptor(G2DTypeDescriptor parent, string name, Attribute[] attrs) : base(parent, name, attrs)
            {
            }
            public abstract object ComponentData { get; }
            public abstract object FieldMember { get; }
            public abstract object FieldValue { get; }
            public abstract bool NotNull { get; }
            public abstract Type DecleardFieldType { get; }
            public abstract G2DFieldElementDesc ToFieldDesc(object root, GridItem grid);
            public G2DFieldDescValue ToFieldDesc()
            {
                return new G2DFieldDescValue()
                {
                    ComponentData = ComponentData,
                    FieldMember = FieldMember,
                    FieldValue = FieldValue,
                };
            }
        }
        //------------------------------------------------------------------
        public abstract class MemberPropertyDescriptor : G2DOwnerPropertyDescriptor
        {
            protected MemberPropertyDescriptor(G2DTypeDescriptor g2ddesc, string name, Attribute[] attrs) : base(g2ddesc, name, attrs)
            {
            }
            public override object FieldMember => Member;
            public abstract MemberInfo Member { get; }
            public abstract string GetFuncDesc(bool withType);
            public override G2DFieldElementDesc ToFieldDesc(object root, GridItem item)
            {
                return new G2DFieldElementDesc()
                {
                    RootData = root,
                    FieldDecleardType = DecleardFieldType,
                    FieldMember = Member,
                    FieldName = Member.Name,
                    FieldValue = FieldValue,
                    ComponentData = ComponentData,
                    Cell = item,
                };
            }
        }
        public abstract class MemberPropertyDescriptor<M> : MemberPropertyDescriptor where M : MemberInfo
        {
            public readonly int index;
            public readonly G2DTypeDescriptor g2ddesc;

            protected readonly M @field;
            protected readonly Type fieldType;

            protected readonly object componentData;
            protected readonly Type componentType;
            protected readonly bool isExpandable;

            private DescAttribute desc;
            private DependOnPropertyAttribute[] depends;
            private TypeConverterAttribute[] type_converters;
            private NotNullAttribute notNull;
            private DeepCore.Reflection.ReadOnlyAttribute _readonly;
            //private object editor;
            private TypeConverter converter;


            public MemberPropertyDescriptor(G2DTypeDescriptor g2ddesc, int index, M field, object componentData, Attribute[] attributes)
                : base(g2ddesc, field.Name, attributes)
            {
                this.g2ddesc = g2ddesc;
                this.index = index;

                this.componentData = componentData;
                this.componentType = componentData.GetType();

                this._readonly = field.GetAttribute<DeepCore.Reflection.ReadOnlyAttribute>();

                this.field = field;
                this.fieldType = DecleardFieldType;
                this.desc = PropertyUtil.GetDesc(field);
                object fieldValue = this.GetValue(componentData);
                if (fieldValue != null)
                {
                    fieldType = fieldValue.GetType();
                }

                this.depends = (DependOnPropertyAttribute[])field.GetCustomAttributes(typeof(DependOnPropertyAttribute), false);
                this.type_converters = (TypeConverterAttribute[])fieldType.GetCustomAttributes(typeof(TypeConverterAttribute), true);
                this.isExpandable = !fieldType.IsPrimitiveData();
                if (this.isExpandable)
                {
                    if (fieldType.TryGetAttributes<ExpandableAttribute>(out var expandables, true))
                    {
                        if (expandables.Any(e => !e.IsExpandable))
                        {
                            this.isExpandable = false;
                        }
                    }
                }
                if (this.isExpandable)
                {
                    if (field.TryGetAttributes<ExpandableAttribute>(out var expandables_f, true))
                    {
                        if (expandables_f.Any(e => !e.IsExpandable))
                        {
                            this.isExpandable = false;
                        }
                    }
                }
                this.notNull = PropertyUtil.GetAttribute<NotNullAttribute>(field);
            
            }

            public DescAttribute Desc { get => desc; }
            public override MemberInfo Member { get => @field; }
            public M Field { get { return @field; } }
            public override object ComponentData { get { return componentData; } }
            public override bool NotNull { get { return notNull != null; } }
            public override object FieldValue { get { return this.GetValue(componentData); } }
            public override Type ComponentType { get { return componentType; } }
            public override bool IsReadOnly
            {
                get { return (_readonly != null) || !DependOnPropertyAttribute.IsDepend(depends, componentData); }
            }
            public override Type PropertyType
            {
                get { return (IsReadOnly) ? typeof(string) : fieldType; }
            }
            public override string DisplayName
            {
                get
                {
                    if (sevent_TryGetDisplayName != null && sevent_TryGetDisplayName.Invoke(componentData, @field, out var displayName))
                    {
                        return displayName;
                    }
                    if (desc != null)
                    {
                        if (SHOW_DISPLAY_NAME == G2DPropertyFieldDisplayStyle.FieldName_DescName)
                        {
                            return base.DisplayName + "-" + desc.Desc;
                        }
                        if (SHOW_DISPLAY_NAME == G2DPropertyFieldDisplayStyle.DescName)
                        {
                            return desc.Desc;
                        }
                    }
                    return base.DisplayName;
                }
            }
            public override string Category
            {
                get
                {
                    if (sevent_TryGetCategory != null && sevent_TryGetCategory.Invoke(componentData, @field, out var category))
                    {
                        return category;
                    }
                    if (desc != null)
                    {
                        return desc.Category;
                    }
                    return base.Category;
                }
            }
            public override string Description
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    if (desc != null)
                    {
                        sb.AppendLine(desc.Desc);
                    }
                    object fieldValue = this.GetValue(componentData);
                    ToDescription(sb, DecleardFieldType, fieldValue);
                    sb.AppendLine(base.Description);
                    return sb.ToString();
                }
            }
            public override bool CanResetValue(object component)
            {
                return false;
            }
            public override void ResetValue(object component)
            {
            }
            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
            public override object GetEditor(Type editorBaseType)
            {
                object editor = null;
                if (field is FieldInfo ffield)
                {
                    if (ffield.FieldType.IsEnum)
                    {

                    }
                }
                if (IsReadOnly)
                {
                    editor = new G2DReadonlyEditor(field, componentData);
                    return editor;
                }
                if (editor != null)
                {
                    if (editor is G2DOptionalEditor opt)
                    {
                        opt.AddOptionals(g2ddesc.optional_map);
                    }
                    return editor;
                }
                else
                {
                    if (g2ddesc.optional_map.TryGetValue(field.Name, out var optionals) && optionals.Count > 0)
                    {
                        var opt = new G2DOptionalEditor(field, componentData);
                        opt.AddOptionals(g2ddesc.optional_map);
                        editor = opt;
                        return editor;
                    }
                    foreach (IG2DPropertyAdapter adapter in g2ddesc.adapter_list)
                    {
                        editor = adapter.GetEditor(field, DecleardFieldType, componentData);
                        if (editor != null)
                        {
                            if (editor is G2DOptionalEditor opt)
                            {
                                opt.AddOptionals(g2ddesc.optional_map);
                            }
                            return editor;
                        }
                    }
                    if (g2ddesc.IsNeedG2DCollectionEditor(fieldType))
                    {
                        editor = new G2DFieldMemberEditor(field, this.GetValue(componentData), true, g2ddesc.add_adapters.ToArray());
                    }
                    else if (g2ddesc.IsNeedG2DEditor(fieldType))
                    {
                        editor = new G2DFieldMemberEditor(field, this.GetValue(componentData), false, g2ddesc.add_adapters.ToArray());
                    }
                    if (editor != null)
                    {
                        return editor;
                    }
                }
                if (sevent_TryGetEditor != null && sevent_TryGetEditor.Invoke(componentData, field, editorBaseType, out var edit))
                {
                    editor = edit;
                    return edit;
                }
                editor = base.GetEditor(editorBaseType);
                return editor;
            }

            public override TypeConverter Converter
            {
                get
                {
                    if (IsReadOnly) return null;
                    if (converter != null) return converter;
                    if (type_converters.Length > 0)
                    {
                        return base.Converter;
                    }
                    foreach (IG2DPropertyAdapter adapter in g2ddesc.adapter_list)
                    {
                        converter = adapter.GetConverter(@field, DecleardFieldType, componentData);
                        if (converter != null)
                        {
                            return converter;
                        }
                    }
                    if (PropertyType.IsEnum)
                    {
                        converter = new G2DDescEnumConverter(PropertyType);
                    }
                    else if (g2ddesc.IsNeedG2DCollectionEditor(fieldType))
                    {
                        converter = new G2DUIFieldListConverter(g2ddesc);
                    }
                    else if (isExpandable)
                    {
                        converter = new G2DUIFieldExpandableConverter(g2ddesc);
                    }
                    if (converter != null) return converter;
                    if (sevent_TryGetConverter != null && sevent_TryGetConverter.Invoke(componentData, @field, out var edit))
                    {
                        converter = edit;
                        return converter;
                    }
                    return base.Converter;
                }
            }


        }
        public class FieldMemberDescriptor : MemberPropertyDescriptor<FieldInfo>
        {
            public FieldMemberDescriptor(G2DTypeDescriptor g2ddesc, int index, FieldInfo @field, object componentData, Attribute[] attributes)
                : base(g2ddesc, index, @field, componentData, attributes)
            {
            }
            public override object GetEditor(Type editorBaseType)
            {
                return base.GetEditor(editorBaseType);
            }
            public override Type DecleardFieldType
            {
                get { return Field.FieldType; }
            }
            public override object GetValue(object component)
            {
                return field.GetValue(component ?? componentData);
            }
            public override void OnSetValue(object component, object value)
            {
                field.SetValue(component ?? componentData, value);
            }
            public override string GetFuncDesc(bool withType)
            {
                return
                  (Desc != null ? Desc.Desc : field.Name) + Environment.NewLine +
                  "." + field.Name + (withType ? ("@" + componentData.GetType().Name) : string.Empty) + Environment.NewLine +
                  field.FieldType.Name + Environment.NewLine +
                  GetValue(componentData);
            }

        }
        public class PropertyMemberDescriptor : MemberPropertyDescriptor<PropertyInfo>
        {
            protected readonly MethodInfo set_op;
            protected readonly MethodInfo get_op;

            public PropertyMemberDescriptor(G2DTypeDescriptor g2ddesc, int index, PropertyInfo field, object componentData, Attribute[] attributes)
                : base(g2ddesc, index, field, componentData, attributes)
            {
                this.get_op = field.GetGetMethod();
                this.set_op = field.GetSetMethod();
            }
            public override bool IsReadOnly
            {
                get
                {
                    if (set_op == null) return true;
                    if (!@field.CanWrite) return true;
                    return base.IsReadOnly;
                }
            }
            public override Type DecleardFieldType
            {
                get { return Field.PropertyType; }
            }
            public override object GetValue(object component)
            {
                if (get_op != null) return get_op.Invoke(component, new object[] { });
                return null;
            }
            public override void OnSetValue(object component, object value)
            {
                if (set_op != null) set_op.Invoke(component, new object[] { value });
            }

            public override string GetFuncDesc(bool withType)
            {
                return
                (Desc != null ? Desc.Desc : field.Name) + Environment.NewLine +
                "." + field.Name + (withType ? ("@" + componentData.GetType().Name) : string.Empty) + Environment.NewLine +
                field.PropertyType.Name + Environment.NewLine +
                GetValue(componentData);
            }
        }
        //------------------------------------------------------------------
        public class EnumPropertyDescriptor : G2DPropertyDescriptor
        {
            private object value;
            private Type valueType;
            public EnumPropertyDescriptor(G2DTypeDescriptor g2d, object value, Attribute[] attributes) : base(g2d, "Value", attributes)
            {
                this.value = value;
                this.valueType = value.GetType();
            }
            public override Type ComponentType { get { return valueType; } }
            public override bool IsReadOnly { get { return false; } }
            public override Type PropertyType { get { return valueType; } }
            public override bool CanResetValue(object component)
            {
                return false;
            }
            public override object GetValue(object component)
            {
                return value;
            }
            public override void ResetValue(object component)
            {
            }
            public override void OnSetValue(object component, object value)
            {
                this.value = value;
                parent.EditData = value;
            }
            public override bool ShouldSerializeValue(object component)
            {
                return true;
            }
            public override object GetEditor(Type editorBaseType)
            {
                return base.GetEditor(editorBaseType);
            }
        }
        //------------------------------------------------------------------
        public abstract class CollectionItemPropertyDescriptor : G2DOwnerPropertyDescriptor
        {
            public override object FieldMember => Index;
            public int Index { get; private set; }
            public G2DTypeDescriptor Descriptor { get; private set; }
            public object ListOwner { get; private set; }
            public override object ComponentData { get { return ListOwner; } }
            public override object FieldValue { get { return ElementItem; } }
            public abstract object ElementItem { get; }
            public abstract Type ElementType { get; }

            private object editor;
            private TypeConverter converter;

            public CollectionItemPropertyDescriptor(G2DTypeDescriptor g2ddesc, object listOwner, int index, Attribute[] attributes)
                : base(g2ddesc, index.ToString(), attributes)
            {
                this.Descriptor = g2ddesc;
                this.Index = index;
                this.ListOwner = listOwner;
            }

            public override Type ComponentType { get { return ListOwner.GetType(); } }
            public override bool NotNull { get { return false; } }
            public override bool IsReadOnly { get { return false; } }
            public override Type PropertyType
            {
                get
                {
                    if (ElementItem != null) return ElementItem.GetType();
                    return ElementType;
                }
            }
            public override string Description
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    ToDescription(sb, DecleardFieldType, ElementItem);
                    sb.AppendLine(base.Description);
                    return sb.ToString();
                }
            }
            public override bool CanResetValue(object component)
            {
                return false;
            }
            public override void ResetValue(object component)
            {
            }
            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
            public override object GetEditor(Type editorBaseType)
            {
                if (editor != null) return editor;
                if (Descriptor.IsNeedG2DEditor(PropertyType))
                {
                    editor = new G2DCollectionItemEditor(ElementItem, ElementType, Descriptor.add_adapters.ToArray());
                    return editor;
                }
                return base.GetEditor(editorBaseType);
            }
            public override TypeConverter Converter
            {
                get
                {
                    if (converter != null) return converter;
                    if (PropertyType.IsClass || PropertyType.GetCustomAttributes(typeof(ExpandableAttribute), true).Length > 0)
                    {
                        converter = new G2DUIFieldExpandableConverter(Descriptor);
                        return converter;
                    }
                    return base.Converter;
                }
            }
            public override G2DFieldElementDesc ToFieldDesc(object root, GridItem item)
            {
                return new G2DFieldElementDesc()
                {
                    RootData = root,
                    FieldDecleardType = DecleardFieldType,
                    FieldMember = typeof(int),
                    FieldName = Index,
                    FieldValue = FieldValue,
                    ComponentData = ComponentData,
                    Cell = item,
                };
            }
        }
        //------------------------------------------------------------------
        public class ListItemPropertyDescriptor : CollectionItemPropertyDescriptor
        {
            private readonly Type genericItemType;
            public override object ElementItem { get { return GetValue(ListOwner); } }
            public override Type ElementType { get { return genericItemType; } }
            public override Type DecleardFieldType { get { return genericItemType; } }

            public ListItemPropertyDescriptor(G2DTypeDescriptor g2ddesc, IList listOwner, int index, Attribute[] attributes)
                : base(g2ddesc, listOwner, index, attributes)
            {
                Type listtype = listOwner.GetType();
                if (listtype.IsGenericType)
                {
                    this.genericItemType = listOwner.GetType().GetGenericArguments()[0];
                }
                else
                {
                    this.genericItemType = typeof(object);
                }
            }
            public override object GetValue(object component)
            {
                IList list = component as IList;
                if (list != null && Index >= 0 && Index < list.Count)
                {
                    return list[Index];
                }
                return null;
            }
            public override void OnSetValue(object component, object value)
            {
                IList list = component as IList;
                if (Index >= 0 && Index < list.Count)
                {
                    list[Index] = value;
                }
                //base.SetValue(component, value);
            }

        }
        //------------------------------------------------------------------
        //         public class ComponentItemPropertyDescriptor : CollectionItemPropertyDescriptor
        //         {
        //             private readonly Type genericItemType;
        //             public override object ElementItem { get { return GetValue(ListOwner); } }
        //             public override Type ElementType { get { return genericItemType; } }
        //             public override Type DecleardFieldType { get { return genericItemType; } }
        // 
        //             public ComponentItemPropertyDescriptor(G2DPropertyDescriptor g2ddesc, DataComponentCollection listOwner, int index, Attribute[] attributes)
        //                 : base(g2ddesc, listOwner, index, attributes)
        //             {
        //                 Type listtype = listOwner.GetType();
        //                 if (listtype.IsGenericType)
        //                 {
        //                     this.genericItemType = listOwner.GetType().GetGenericArguments()[0];
        //                 }
        //                 else
        //                 {
        //                     this.genericItemType = typeof(object);
        //                 }
        //             }
        //             public override object GetValue(object component)
        //             {
        //                 var list = component as DataComponentCollection;
        //                 if (list != null && Index >= 0 && Index < list.Count)
        //                 {
        //                     return list[Index];
        //                 }
        //                 return null;
        //             }
        //             public override void SetValue(object component, object value)
        //             {
        // //                 var list = component as DataComponentCollection;
        // //                 if (Index >= 0 && Index < list.Count)
        // //                 {
        // //                     list[Index] = value;
        // //                 }
        //                 //base.SetValue(component, value);
        //             }
        //         }

        //------------------------------------------------------------------
        public class ArrayItemPropertyDescriptor : CollectionItemPropertyDescriptor
        {
            private readonly Type genericItemType;
            public override object ElementItem { get { return GetValue(ListOwner); } }
            public override Type ElementType { get { return genericItemType; } }
            public override Type DecleardFieldType { get { return genericItemType; } }

            public ArrayItemPropertyDescriptor(G2DTypeDescriptor g2ddesc, IList listOwner, int index, Attribute[] attributes)
                : base(g2ddesc, listOwner, index, attributes)
            {
                this.genericItemType = listOwner.GetType().GetElementType();
            }
            public override object GetValue(object component)
            {
                Array list = component as Array;
                if (list != null && Index >= 0 && Index < list.Length)
                {
                    return list.GetValue(Index);
                }
                return null;
            }
            public override void OnSetValue(object component, object value)
            {
                Array list = component as Array;
                if (Index >= 0 && Index < list.Length)
                {
                    list.SetValue(value, Index);
                }
            }
        }
        //------------------------------------------------------------------  

        public class MapItemPropertyDescriptor : G2DOwnerPropertyDescriptor
        {
            private readonly G2DTypeDescriptor g2ddesc;
            private readonly IDictionary mapOwner;
            private readonly object key;
            private readonly Type[] genericArgs;
            private object editor;
            private TypeConverter converter;
            public override object FieldMember => key;
            public G2DTypeDescriptor Descriptor { get { return g2ddesc; } }
            public IDictionary MapOwner { get { return mapOwner; } }
            public override object ComponentData { get { return mapOwner; } }
            public override Type DecleardFieldType { get { return genericArgs[1]; } }
            public override object FieldValue { get { return GetValue(mapOwner); } }
            public override bool NotNull { get { return true; } }

            public object Key { get { return key; } }
            public object Value { get { return GetValue(mapOwner); } }

            public override Type ComponentType { get { return MapOwner.GetType(); } }
            public override bool IsReadOnly { get { return false; } }
            public override Type PropertyType
            {
                get
                {
                    var value = Value;
                    if (value != null) return value.GetType();
                    return DecleardFieldType;
                }
            }
            public override string Description
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    ToDescription(sb, DecleardFieldType, Value);
                    sb.AppendLine(base.Description);
                    return sb.ToString();
                }
            }


            public MapItemPropertyDescriptor(G2DTypeDescriptor g2ddesc, IDictionary mapOwner, object key, Attribute[] attributes)
                : base(g2ddesc, key.ToString(), attributes)
            {
                this.g2ddesc = g2ddesc;
                this.key = key;
                this.mapOwner = mapOwner;
                this.genericArgs = mapOwner.GetType().GetGenericArguments();
            }

            public override object GetValue(object component)
            {
                if (mapOwner != null)
                {
                    return mapOwner[key];
                }
                return null;
            }
            public override void OnSetValue(object component, object value)
            {
                if (mapOwner != null)
                {
                    mapOwner[key] = value;
                }
            }
            public override bool CanResetValue(object component)
            {
                return false;
            }
            public override void ResetValue(object component)
            {
            }
            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
            public override object GetEditor(Type editorBaseType)
            {
                if (editor != null) return editor;
                var value = Value;
                var keyType = key.GetType();
                var valueType = DecleardFieldType;
                if (value != null)
                {
                    valueType = value.GetType();
                }
                if (keyType.IsPrimitive || keyType == typeof(string))
                {
                    if (Descriptor.IsNeedG2DEditor(valueType))
                    {
                        editor = new G2DCollectionItemEditor(value, valueType, Descriptor.add_adapters.ToArray());
                    }
                }
                if (editor != null) return editor;
                return null;
            }
            public override TypeConverter Converter
            {
                get
                {
                    if (converter != null) return converter;
                    if (PropertyType.IsClass || PropertyType.GetCustomAttributes(typeof(ExpandableAttribute), true).Length > 0)
                    {
                        converter = new G2DUIFieldExpandableConverter(Descriptor);
                    }
                    if (converter != null) return converter;
                    return base.Converter;
                }
            }
            public override G2DFieldElementDesc ToFieldDesc(object root, GridItem item)
            {
                return new G2DFieldElementDesc()
                {
                    RootData = root,
                    FieldDecleardType = DecleardFieldType,
                    FieldMember = key.GetType(),
                    FieldName = key,
                    FieldValue = FieldValue,
                    ComponentData = ComponentData,
                    Cell = item,
                };
            }
        }
        #endregion
    }

    //--------------------------------------------------------------------------------------------------------------------------------
    #region Internal ---------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 内部DataAdapters
    /// </summary>
    public class InternalDataAdapters : IG2DPropertyAdapter
    {
        public void OnSetValue(G2DPropertyDescriptor desc, object component, object value)
        {

        }
        public UITypeEditor GetEditor(MemberInfo field, Type fieldType, object ownerData)
        {
            if (fieldType == typeof(DeepCore.GUI.Display.Color))
            {
                return new G2DColorTypeEditor(field, ownerData);
            }
            if (fieldType == typeof(EnumValue))
            {
                return new G2DEnumValueTypeEditor(field, ownerData);
            }
            if (PropertyUtil.GetAttribute<DirectoryPathAttribute>(field) != null && fieldType == typeof(string))
            {
                return new G2DDirectoryDialogEditor(field, ownerData);
            }
            if (PropertyUtil.TryGetAttribute<FilePathAttribute>(field, out var filePath) && fieldType == typeof(string))
            {
                return new G2DFileDialogEditor(field, ownerData, filePath);
            }
            if (PropertyUtil.GetAttribute<Int32ColorAttribute>(field) != null && fieldType == typeof(int))
            {
                return new G2DColorEditor(field, ownerData);
            }
            if (PropertyUtil.GetAttribute<Int32ColorAttribute>(field) != null && fieldType == typeof(uint))
            {
                return new G2DColorEditorU32(field, ownerData);
            }
            if (PropertyUtil.GetAttribute<OptionalValueAttribute>(field) != null)
            {
                return new G2DOptionalEditor(field, ownerData);
            }
            if (PropertyUtil.GetAttribute<LocalizationTextAttribute>(field) != null && fieldType == typeof(string))
            {
                return new G2DLocalizationTextEditor(field, ownerData);
            }
            return null;
        }
        public TypeConverter GetConverter(MemberInfo field, Type fieldType, object ownerData)
        {
            //             if (fieldType == typeof(EnumValue))
            //             {
            //                 return new G2DEnumValueTypeConverter();
            //             }
            if (PropertyUtil.GetAttribute<Int32ColorAttribute>(field) != null && fieldType == typeof(int))
            {
                return new G2DIntToHexTypeConverter();
            }
            if (PropertyUtil.GetAttribute<Int32ColorAttribute>(field) != null && fieldType == typeof(uint))
            {
                return new G2DUIntToHexTypeConverter();
            }
            if (PropertyUtil.GetAttribute<HexIntegerAttribute>(field) != null && fieldType == typeof(int))
            {
                return new G2DIntToHexTypeConverter();
            }
            if (PropertyUtil.GetAttribute<HexIntegerAttribute>(field) != null && fieldType == typeof(uint))
            {
                return new G2DUIntToHexTypeConverter();
            }
            //             else if (PropertyUtil.TryGetAttribute<FilePathAttribute>(field, out var attr) && attr.IsImage && fieldType == typeof(string))
            //             {
            //                 return new G2DImageConverter();
            //             }
            return null;
        }

    }

    public class OptionalList
    {
        private HashSet<object> values = new HashSet<object>();
        public Func<MemberInfo, object, object> Converter;
        public void AddRange(IEnumerable list)
        {
            foreach (var e in list)
            {
                values.Add(e);
            }
        }
        public void AddRange(OptionalList list)
        {
            foreach (var e in list.values)
            {
                values.Add(e);
            }
            if (list.Converter != null) { this.Converter = list.Converter; }
        }
        public void Add(object value)
        {
            values.Add(value);
        }
        public int Count { get => values.Count; }
        public IEnumerable<object> Values { get => values; }
    }

    //--------------------------------------------------------------------------------------------------------------------------------
    #region TypeConverter --------------------------------------------------------------------------------------------------------------------------------

    public class G2DDescEnumConverter : System.ComponentModel.EnumConverter
    {
        public Type FieldType { get; }
        public G2DDescEnumConverter(Type fieldType) : base(fieldType)
        {
            FieldType = fieldType;
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var attr = PropertyUtil.GetEnumAttribute<DescAttribute>((Enum)value);
            if (attr != null)
            {
                return base.ConvertTo(context, culture, value, destinationType) + " :" + attr.Desc;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string strvalue)
            {
                if (strvalue.TryIndexOf(" :", out var index))
                {
                    return base.ConvertFrom(context, culture, strvalue.Substring(0, index));
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }


    public class G2DUIFieldListConverter : TypeConverter
    {
        private readonly G2DTypeDescriptor g2ddesc;
        public G2DUIFieldListConverter(G2DTypeDescriptor g2d)
        {
            g2ddesc = g2d;
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType.IsInterfaceOf(typeof(ICollection)))
            {
                return true;
            }
            if (destinationType.IsArray)
            {
                return true;
            }
            return false;
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is ICollection)
            {
                ICollection list = (ICollection)value;
                return "集合:[" + list.Count + "]";
            }
            if (destinationType.IsArray)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("数组:");
                Array array = value as Array;
                int[] ranks = new int[array.Rank];
                foreach (int len in ranks)
                {
                    sb.Append("[" + len + "]");
                }
                return sb.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return g2ddesc.ListPropertyDescriptors(value, g2ddesc, attributes);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    //--------------------------------------------------------------------------------------


    public class G2DUIFieldExpandableConverter : ExpandableObjectConverter
    {
        private readonly G2DTypeDescriptor g2ddesc;
        public G2DUIFieldExpandableConverter(G2DTypeDescriptor g2d)
        {
            g2ddesc = g2d;
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return g2ddesc.ListPropertyDescriptors(value, g2ddesc, attributes);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    // --------------------------------------------------------------------------------------

    public class G2DIntToHexTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(int))
            {
                return string.Format("0x{0:X8}", value);
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string input = (string)value;

                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(2);
                }

                return int.Parse(input, NumberStyles.HexNumber, culture);
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }

    public class G2DUIntToHexTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(uint))
            {
                return string.Format("0x{0:X8}", value);
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string input = (string)value;

                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(2);
                }

                return uint.Parse(input, NumberStyles.HexNumber, culture);
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }

    public class G2DEnumValueTypeConverter : TypeConverter
    {
        public G2DEnumValueTypeConverter()
        {

        }
    }



    //--------------------------------------------------------------------------------------

    #endregion
    //--------------------------------------------------------------------------------------------------------------------------------

    #region UITypeEditor --------------------------------------------------------------------------------------------------------------------------------

    //     public class G2DDescEnumEditor : UITypeEditor
    //     {
    // 
    //     }

    public class G2DFieldMemberEditor : UITypeEditor
    {
        private readonly MemberInfo fieldInfo;
        private readonly Type fieldType;
        private readonly IG2DPropertyAdapter[] adapters;

        private object fieldValue;

        private readonly ListDescAttribute listDesc;
        private readonly NotNullAttribute notNull;
        private readonly bool isList;

        public G2DFieldMemberEditor(MemberInfo field, object fieldValue, bool isList, IG2DPropertyAdapter[] adapters)
        {
            this.isList = isList;
            this.fieldInfo = field;
            if (field is FieldInfo)
            {
                this.fieldType = (field as FieldInfo).FieldType;
            }
            else if (field is PropertyInfo)
            {
                this.fieldType = (field as PropertyInfo).PropertyType;
            }
            this.fieldValue = fieldValue;
            this.adapters = adapters;
            if (this.fieldValue != null)
            {
                fieldType = fieldValue.GetType();
            }
            this.listDesc = PropertyUtil.GetListDesc(field);
            this.notNull = PropertyUtil.GetAttribute<NotNullAttribute>(field);
        }
        //         public override void PaintValue(PaintValueEventArgs e)
        //         {
        //             e.Graphics.FillRectangle(Brushes.Blue, e.Bounds);
        //         }
        //         public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        //         {
        //             return true;
        //         }
        /// <summary>
        /// 编辑属性值时，在右侧显示...更多按钮 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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
            var edSvc = provider.GetService(typeof(IWindowsFormsEditorService))
                as IWindowsFormsEditorService;
            if (edSvc != null)
            {
                if (isList)
                {
                    var editor = new G2DCollectionEditor(
                        fieldType,
                        value,
                        listDesc?.GetElementTypes(fieldType),
                        adapters);
                    if (editor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        value = editor.GetEditCompleteData();
                    }
                    return value;
                }
                else if (fieldType.IsValueType)
                {
                    var editor = new G2DXmlEditor(fieldType, value, (notNull == null));
                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        value = editor.EditObject;
                    }
                    return value;
                }
                else
                {
                    var editor = new G2DFieldEditor(fieldType, value, (notNull == null), adapters);
                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        value = editor.EditObject;
                    }
                    return value;
                }
            }
            return base.EditValue(context, provider, value);
        }
    }

    public class G2DCollectionItemEditor : UITypeEditor
    {
        public object ElementItem { get; private set; }
        public Type ElementType { get; private set; }

        private IG2DPropertyAdapter[] adapters;


        public G2DCollectionItemEditor(object fieldValue, Type fieldType, IG2DPropertyAdapter[] adapters)
        {
            this.ElementItem = fieldValue;
            this.ElementType = fieldType;
            this.adapters = adapters;
        }

        /// <summary>
        /// 编辑属性值时，在右侧显示...更多按钮 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (ElementType.IsPrimitive)
            {
                return UITypeEditorEditStyle.None;
            }
            else if (ElementType.IsEnum)
            {
                return UITypeEditorEditStyle.None;
            }
            else if (ElementType.Equals(typeof(string)))
            {
                return UITypeEditorEditStyle.None;
            }
            else if (ElementType.IsClass)
            {
                return UITypeEditorEditStyle.Modal;
            }
            else if (ElementType.IsArray)
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
            G2DFieldEditor editor = new G2DFieldEditor(ElementType, value, false, adapters);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                value = editor.EditObject;
                return value;
            }
            return base.EditValue(context, provider, value);
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------

    public abstract class G2DMemberUITypeEditor : UITypeEditor
    {
        protected readonly MemberInfo fieldInfo;
        protected readonly Type fieldType;
        protected readonly DescAttribute desc;
        protected readonly object ownerData;

        public G2DMemberUITypeEditor(MemberInfo field, object ownerData)
        {
            this.desc = PropertyUtil.GetAttribute<DescAttribute>(field);
            this.fieldInfo = field;
            this.ownerData = ownerData;
            if (fieldInfo is FieldInfo)
            {
                fieldType = (fieldInfo as FieldInfo).FieldType;
            }
            else if (fieldInfo is PropertyInfo)
            {
                fieldType = (fieldInfo as PropertyInfo).PropertyType;
            }
            else
            {
                fieldType = null;
            }
        }
        public object GetMemberValue()
        {
            return PropertyUtil.GetMemberValue(fieldInfo, ownerData);
        }
        public void SetMemberValue(object value)
        {
            PropertyUtil.SetMemberValue(fieldInfo, ownerData, value);
        }
    }
    public abstract class G2DMemberUITypeEditor<FT, FV> : G2DMemberUITypeEditor where FT : MemberInfo
    {
        public FT Field { get => base.fieldInfo as FT; }
        protected G2DMemberUITypeEditor(FT field, object ownerData) : base(field, ownerData)
        {
        }

        new public FV GetMemberValue()
        {
            return (FV)PropertyUtil.GetMemberValue(fieldInfo, ownerData);
        }
        public void SetMemberValue(FV value)
        {
            PropertyUtil.SetMemberValue(fieldInfo, ownerData, value);
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------
    public class G2DDirectoryDialogEditor : G2DMemberUITypeEditor
    {
        public G2DDirectoryDialogEditor(MemberInfo field, object ownerData)
            : base(field, ownerData)
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(
            ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
        {
            string path = (string)base.GetMemberValue();
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.SelectedPath = path;
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    return fd.SelectedPath;
                }
            }
            return base.EditValue(context, provider, value);
        }
    }

    public class G2DFileDialogEditor : G2DMemberUITypeEditor
    {
        public FilePathAttribute Attr { get; }
        private Bitmap image;
        public G2DFileDialogEditor(MemberInfo field, object ownerData, FilePathAttribute attr)
            : base(field, ownerData)
        {
            this.Attr = attr;
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(
            ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
        {
            string path = (string)base.GetMemberValue();
            {
                path = ShowResourceDialog(path);
            }
            return base.EditValue(context, provider, value);
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            base.PaintValue(e);
            if (Attr.IsImage)
            {
                var value = this.GetMemberValue();
                if (image == null && value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                {
                    try
                    {
                        var imagePath = GetResourceFullPath(value.ToString());
                        image = new Bitmap(imagePath);
                    }
                    catch { }
                }
                if (image != null)
                {
                    e.Graphics.DrawImage(image, e.Bounds);
                }
            }
        }
        public static string ShowResourceDialog(string path)
        {
            path = GetResourceFullPath(path);
            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = path;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                return GetResourceSubPath(fd.FileName);
            }
            return null;
        }
        public static string GetResourceFullPath(string path)
        {
            string fullPath = Resource.PathRoot + "\\" + path;
            fullPath = fullPath.Replace('/', '\\');
            fullPath = fullPath.Replace("\\\\", "\\");
            return fullPath;
        }
        public static string GetResourceSubPath(string fullName)
        {
            var droot = new DirectoryInfo(Resource.PathRoot);
            var fdinfo = new FileInfo(fullName);
            if (fdinfo.FullName.StartsWith(droot.FullName))
            {
                var id = fdinfo.FullName.Substring(droot.FullName.Length);
                id = id.Replace('\\', '/');
                return id;
            }
            else
            {
                throw new Exception("无法引用编辑器目录之外的资源 : " + fullName);
            }
        }
    }

    public class G2DColorEditor : G2DMemberUITypeEditor
    {
        private SolidBrush brush = new SolidBrush(Color.Green);
        public G2DColorEditor(MemberInfo field, object ownerData)
            : base(field, ownerData)
        {
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            int color = (int)base.GetMemberValue();
            brush.Color = Color.FromArgb(color);
            e.Graphics.FillRectangle(brush, e.Bounds);
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(
            ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
        {
            int color = (int)base.GetMemberValue();
            {
                ColorDialog cd = new ColorDialog();
                cd.Color = Color.FromArgb(color);
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    return cd.Color.ToArgb();
                }
            }
            return base.EditValue(context, provider, value);
        }
    }
    public class G2DColorEditorU32 : G2DMemberUITypeEditor
    {
        private SolidBrush brush = new SolidBrush(Color.Green);
        public G2DColorEditorU32(MemberInfo field, object ownerData)
            : base(field, ownerData)
        {
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            uint color = (uint)base.GetMemberValue();
            brush.Color = Color.FromArgb((int)color);
            e.Graphics.FillRectangle(brush, e.Bounds);
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(
            ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
        {
            uint color = (uint)base.GetMemberValue();
            {
                ColorDialog cd = new ColorDialog();
                cd.Color = Color.FromArgb((int)color);
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    return (uint)cd.Color.ToArgb();
                }
            }
            return base.EditValue(context, provider, value);
        }
    }
    public class G2DColorTypeEditor : G2DMemberUITypeEditor
    {
        private SolidBrush brush = new SolidBrush(Color.Green);
        public G2DColorTypeEditor(MemberInfo field, object ownerData)
            : base(field, ownerData)
        {
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            var color = (DeepCore.GUI.Display.Color)base.GetMemberValue();
            brush.Color = Color.FromArgb(color.sARGB);
            e.Graphics.FillRectangle(brush, e.Bounds);
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var color = (DeepCore.GUI.Display.Color)base.GetMemberValue();
            {
                var cd = new ColorDialog();
                cd.Color = Color.FromArgb(color.sARGB);
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    var sARGB = cd.Color.ToArgb();
                    return DeepCore.GUI.Display.Color.FromARGB(sARGB);
                }
            }
            return base.EditValue(context, provider, value);
        }
    }

    public class G2DEnumValueTypeEditor : G2DMemberUITypeEditor
    {
        public G2DEnumValueTypeEditor(MemberInfo field, object ownerData)
            : base(field, ownerData)
        {
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var enumValue = base.GetMemberValue();
            {
                var dialog = new G2DEnumValueDialog(enumValue as EnumValue);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    enumValue = dialog.SelectedObject;
                    return enumValue;
                }
            }
            return base.EditValue(context, provider, value);
        }
    }


    public class G2DOptionalEditor : G2DMemberUITypeEditor
    {
        private OptionalValueAttribute optional;
        private OptionalList optional_ext = new OptionalList();

        public G2DOptionalEditor(MemberInfo field, object ownerData)
            : base(field, ownerData)
        {
            this.optional = PropertyUtil.GetAttribute<OptionalValueAttribute>(fieldInfo);
        }
        public void AddOptionals(IDictionary<string, OptionalList> opts)
        {
            if (opts.TryGetValue(base.fieldInfo.Name, out var list))
            {
                optional_ext.AddRange(list);
            }
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue(
            ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
        {
            IWindowsFormsEditorService wfes =
               provider.GetService(typeof(IWindowsFormsEditorService)) as
               IWindowsFormsEditorService;
            if (wfes != null)
            {
                var list = new ListBox();
                if (optional != null)
                {
                    foreach (var o in optional.Values)
                    {
                        if (o != null)
                        {
                            list.Items.Add(o);
                        }
                    }
                }
                foreach (var o in optional_ext.Values)
                {
                    if (o != null)
                    {
                        list.Items.Add(o);
                    }
                }
                list.SelectedValueChanged += (s, e) =>
                {
                    wfes.CloseDropDown();
                };
                wfes.DropDownControl(list);
                if (list.SelectedItems.Count > 0)
                {
                    value = list.SelectedItem;
                }
                list.Dispose();
            }
            if (optional_ext.Converter != null)
            {
                value = optional_ext.Converter(fieldInfo, value);
            }
            return value;
        }

    }

    public class G2DLocalizationTextEditor : G2DMemberUITypeEditor
    {
        public const string KEY = "译";
        private static Font KEY_FONT;

        public G2DLocalizationTextEditor(MemberInfo field, object ownerData) : base(field, ownerData)
        {
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            if (KEY_FONT == null)
            {
                KEY_FONT = new Font(Form.DefaultFont.FontFamily, 9);
            }
            var font = KEY_FONT;
            var tsize = e.Graphics.MeasureString(KEY, font);
            var bounds = e.Bounds;
            e.Graphics.FillRectangle(Brushes.Blue, bounds);
            e.Graphics.DrawString(KEY, font, Brushes.White, bounds.X + (bounds.Width - tsize.Width) / 2, bounds.Y + (bounds.Height - tsize.Height) / 2);
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    public class G2DReadonlyEditor : G2DMemberUITypeEditor
    {
        public const string KEY = "[R]";
        private static Font KEY_FONT;

        public G2DReadonlyEditor(MemberInfo field, object ownerData) : base(field, ownerData)
        {
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            if (KEY_FONT == null)
            {
                KEY_FONT = new Font(Form.DefaultFont.FontFamily, 9);
            }
            var font = KEY_FONT;
            var tsize = e.Graphics.MeasureString(KEY, font);
            var bounds = e.Bounds;
            e.Graphics.FillRectangle(Brushes.Gray, bounds);
            e.Graphics.DrawString(KEY, font, Brushes.White, bounds.X + (bounds.Width - tsize.Width) / 2, bounds.Y + (bounds.Height - tsize.Height) / 2);
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.None;
        }
    }

    #endregion
    //--------------------------------------------------------------------------------------------------------------------------------

    #endregion
}
