using DeepCore;
using DeepCore.FuncData;
using DeepCore.Reflection;
using DeepEditor.Common.G2D.DataGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.FuncEditor
{
    public class FuncDataPropertyAdapter : IG2DPropertyAdapter
    {
        static FuncDataPropertyAdapter()
        {
            G2DPropertyDescriptor.RegistDelegate(new G2DPropertyDescriptor.TryGetDisplayName((object owner, MemberInfo member, out string displayName) =>
            {
                if (owner is IFuncData func && func.HasFuncID() && member is FieldInfo field)
                {
                    using (var sb = StringBuilderObjectPool.AllocAutoRelease())
                    {
                        int count = 0;
                        foreach (var fid in func.FuncID.FuncID)
                        {
                            if (fid.TryGetFieldIndex(field.Name, out var findex))
                            {
                                if (findex.IsExclude) { continue; }
                            }
                            var fd = FuncDataManager.Instance.GetTemplate(fid.ID);
                            if (fd != null && fd.AcceptField(owner.GetType(), member.Name))
                            {
                                sb.Write($"[{fd.FuncID}]");
                                count++;
                            }
                        }
                        if (count > 0)
                        {
                            sb.Write(" > ");
                            sb.Write(member.Name);
                            displayName = sb.ToString();
                            return true;
                        }
                    }
                }
                displayName = null;
                return false;
            }));
            G2DPropertyDescriptor.RegistDelegate(new G2DPropertyDescriptor.TryGetCategory((object owner, MemberInfo member, out string category) =>
            {
                if (owner is IFuncData func && func.HasFuncID() && member is FieldInfo field)
                {
                    var funcProps = owner.GetType().GetMember(nameof(func.FuncID));
                    foreach (var funcProp in funcProps)
                    {
                        var funcAttr = funcProp.GetAttribute<DescAttribute>();
                        foreach (var fid in func.FuncID.FuncID)
                        {
                            var fd = FuncDataManager.Instance.GetTemplate(fid.ID);
                            if (fd != null && fd.AcceptField(owner.GetType(), member.Name))
                            {
                                category = funcAttr != null ? funcAttr.Category : $"FuncID";
                                return true;
                            }
                        }
                    }
                }
                category = null;
                return false;
            }));
        }

        public UITypeEditor GetEditor(MemberInfo field, Type fieldType, object ownerData)
        {
            if (field.GetAttributeByType(typeof(OwnerFuncIDAttribute)) != null)
            {
                return new OwnerFuncIDEditor(fieldType);
            }
            else if (field.TryGetAttribute<FillFromFuncIDAttribute>(out var fill))
            {
                return new FillFuncIDEditor(fieldType, fill);
            }
            else if (FuncDataManager.Instance != null && ownerData is IFuncData)
            {
                if ((field is PropertyInfo pinfo && pinfo.PropertyType == typeof(FuncTable)) ||
                    (field is FieldInfo finfo && finfo.FieldType == typeof(FuncTable)))
                {
                    return new FuncIDEditor(field, ownerData as IFuncData);
                }
            }
            return null;
        }

        public TypeConverter GetConverter(MemberInfo field, Type fieldType, object ownerData)
        {
            if (field.GetAttributeByType(typeof(OwnerFuncIDAttribute)) != null)
            {
                return new OwnerFuncIDConverter(fieldType);
            }
            return null;
        }

        //-------------------------------------------------------------------------------------
        public class FuncIDEditor : UITypeEditor
        {
            public const string KEY = "表";
            private static Font KEY_FONT;
            private Brush brush = new SolidBrush(Color.Green);
            private MemberInfo fieldInfo;
            private IFuncData ownerData;

            public FuncIDEditor(MemberInfo field, IFuncData ownerData)
            {
                this.fieldInfo = field;
                this.ownerData = ownerData;
            }
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                return true;
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
                e.Graphics.FillRectangle(brush, bounds);
                e.Graphics.DrawString(KEY, font, Brushes.White, bounds.X + (bounds.Width - tsize.Width) / 2, bounds.Y + (bounds.Height - tsize.Height) / 2);
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                var src = ownerData as IFuncData;
                if (FormSelectFuncID.ShowDialog(src, out var funcID))
                {
                    src.FuncID = funcID;
                    FuncDataManager.Instance.FillFromFuncID(src);
                    return funcID;
                }
                return base.EditValue(context, provider, value);
            }
        }

        //-------------------------------------------------------------------------------------

        public class OwnerFuncIDEditor : UITypeEditor
        {
            public Type FieldType { get; }
            public OwnerFuncIDEditor(Type fieldType)
            {
                this.FieldType = fieldType;
            }
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (FormSelectFuncID.ShowOwnerDialog(value as HashMap<string, int>, out var funcID))
                {
                    return funcID;
                }
                return base.EditValue(context, provider, value);
            }
        }

        public class FillFuncIDEditor : UITypeEditor
        {
            public Type FieldType { get; }
            public FillFromFuncIDAttribute FieldAttribute { get; }
            public FillFuncIDEditor(Type fieldType, FillFromFuncIDAttribute fill)
            {
                this.FieldType = fieldType;
                this.FieldAttribute = fill;
            }
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (FieldType.IsGenericList())
                {
                    if (FormSelectFuncID.ShowFillDialog(value as IList, FieldType, FieldAttribute.ElementType, out var olist))
                    {
                        return olist;
                    }
                }
                else if (typeof(IFuncData).IsAssignableFrom(FieldType))
                {
                    if (FormSelectFuncID.ShowFillDialog(FieldType, ref value))
                    {
                        return value;
                    }
                }
                return base.EditValue(context, provider, value);
            }
        }

        public class OwnerFuncIDConverter : TypeConverter
        {
            public Type FieldType { get; }
            public OwnerFuncIDConverter(Type fieldType)
            {
                this.FieldType = fieldType;
            }
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return false;
            }
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    return true;
                }
                return false;
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is HashMap<string, int> map)
                {
                    using (var sb = StringBuilderObjectPool.AllocAutoRelease())
                    {
                        int i = 0;
                        foreach (var fid in map)
                        {
                            var fd = FuncDataManager.Instance.GetTemplate(fid.Key);
                            if (fd != null && !string.IsNullOrWhiteSpace(fd.FuncName))
                            {
                                sb.Write($"{fd.FuncID}(lv={fid.Value})");
                            }
                            else
                            {
                                sb.Write(fid);
                            }
                            i++;
                            if (i < map.Count)
                            {
                                sb.Write("; ");
                            }
                        }
                        return sb.ToString();
                    }
                }
                return null;
            }

        }
    }
}
