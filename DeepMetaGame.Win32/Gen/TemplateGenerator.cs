using AntdUI;
using DeepCore;
using DeepCore.Log;
using DeepCore.Reflection;
using DeepEditor.Common.G2D;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Misc;
using DeepMetaGame.Data.Template;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static DeepEditor.Common.G2D.DataGrid.G2DTypeDescriptor;
using static OpenTK.Graphics.OpenGL.GL;

namespace DeepMetaGame.Editor.Gen
{
    //--------------------------------------------------------------------------------------------------------------------------------------
    public class TemplateGroup
    {
        public TemplateData Main;
        public TemplateData[] Subs;
        public static implicit operator TemplateGroup(in TemplateData value)
        {
            return new TemplateGroup() { Main = value };
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------
    public abstract class GeneratorAttribute : Attribute
    {
        public string Name { get; }
        public string Catgory { get; }
        public GeneratorAttribute(string name, string catgory)
        {
            this.Name = name;
            this.Catgory = catgory;
        }
        public override string ToString()
        {
            return Name;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Method)]
    public class GeneratorMethodAttribute : GeneratorAttribute
    {
        public Type TempType { get; }
        public GeneratorMethodAttribute(Type tempType, string name, string catgory) : base(name, catgory)
        {
            this.TempType = tempType;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------

    [AttributeUsage(AttributeTargets.Method)]
    public class AppendMethodAttribute : GeneratorAttribute
    {
        public Type MainType { get; }
        public AppendMethodAttribute(Type mainType, string name, string catgory) : base(name, catgory)
        {
            this.MainType = mainType;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------

    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateGeneratorAttribute : Attribute
    {
        public TemplateGeneratorAttribute()
        {
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------
    public static partial class TemplateGenerator
    {
        #region Base--------------------------------------------------------------------------------------------------------------------
        private static Logger log = new LazyLogger("TemplateGenerator");
        private static HashMap<Type, List<AttributeMember<GeneratorMethodAttribute, MethodInfo>>> gen_list;
        private static HashMap<Type, List<AttributeMember<AppendMethodAttribute, MethodInfo>>> append_list;
        private static void Init()
        {
            lock (log)
            {
                if (gen_list == null)
                {
                    gen_list = new HashMap<Type, List<AttributeMember<GeneratorMethodAttribute, MethodInfo>>>();
                    append_list = new HashMap<Type, List<AttributeMember<AppendMethodAttribute, MethodInfo>>>();
                    var alltype = ReflectionUtil.RegisteredTypes;
                    foreach (var generator in alltype)
                    {
                        if (generator.TryGetAttribute<TemplateGeneratorAttribute>(out var gen))
                        {
                            foreach (var m in generator.GetMethods())
                            {
                                {
                                    if (IsValidCreateMethod(m, out var desc))
                                    {
                                        var ret = gen_list.GetOrNew(desc.TempType);
                                        ret.Add(new(desc, m));
                                        log.Info($"注册模板生成器：{desc.TempType}：{desc.Name}");
                                    }
                                }
                                {
                                    if (IsValidAppendMethod(m, out var desc))
                                    {
                                        var ret = append_list.GetOrNew(desc.MainType);
                                        ret.Add(new(desc, m));
                                        log.Info($"注册模板附加器：{desc.MainType}：{desc.Name}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static bool TryGetGenList(Type tempType, out List<AttributeMember<GeneratorMethodAttribute, MethodInfo>> ret)
        {
            ret = new List<AttributeMember<GeneratorMethodAttribute, MethodInfo>>();
            Init();
            if (gen_list.TryGetValue(tempType, out var list))
            {
                ret.AddRange(list);
            }
            return ret.Count > 0;
        }
        public static bool TryGetAppendList(Type tempType, out List<AttributeMember<AppendMethodAttribute, MethodInfo>> ret)
        {
            ret = new List<AttributeMember<AppendMethodAttribute, MethodInfo>>();
            Init();
            if (append_list.TryGetValue(tempType, out var list))
            {
                ret.AddRange(list);
            }
            return ret.Count > 0;
        }
        private static bool IsValidCreateMethod(MethodInfo m, out GeneratorMethodAttribute desc)
        {
            if (m.IsStatic &&
                m.ReturnType != null &&
                m.GetParameters().Length == 0 &&
                m.TryGetAttribute<GeneratorMethodAttribute>(out desc))
            {
                if (m.ReturnType.IsSubclassOf(typeof(TemplateData)) || m.ReturnType.Equals(typeof(TemplateGroup)))
                {
                    return true;
                }
            }
            desc = null;
            return false;
        }
        private static bool IsValidAppendMethod(MethodInfo m, out AppendMethodAttribute desc)
        {
            if (m.IsStatic &&
                m.ReturnType != null &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType.IsSubclassOf(typeof(TemplateData)) &&
                m.TryGetAttribute<AppendMethodAttribute>(out desc))
            {
                if (m.ReturnType.IsSubclassOf(typeof(TemplateData)) || m.ReturnType.Equals(typeof(TemplateGroup)))
                {
                    return true;
                }
            }
            desc = null;
            return false;
        }
        private static void BananceID(object allnodes, HashMap<Type, HashMap<int, int>> nameKV)
        {
            var templateID = PropertyUtil.CollectFieldAttributeValues<TemplateIDAttribute, int>(allnodes);
            foreach (var tid in templateID)
            {
                if (nameKV.TryGetValue(tid.AttributeData.TemplateType, out var old_new_id))
                {
                    if (old_new_id.TryGetValue(tid.FieldValue, out var new_id))
                    {
                        tid.Field.SetValue(tid.FieldOwner, new_id);
                    }
                }
            }
            var templatesID = PropertyUtil.CollectFieldAttributeValues<TemplatesIDAttribute, ArrayList<int>>(allnodes);
            foreach (var tid in templatesID)
            {
                if (nameKV.TryGetValue(tid.AttributeData.TemplateType, out var old_new_id))
                {
                    var list = tid.FieldValue;
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (old_new_id.TryGetValue(list[i], out var dstID))
                            {
                                list[i] = dstID;
                            }
                        }
                    }
                }
            }
        }

        public static TreeNode ConvertToTreeNodeList<A>(
            G2DTreeViewDataPanel panel,
            string rootName,
            List<AttributeMember<A, MethodInfo>> ret) where A : GeneratorAttribute
        {
            var root = new TreeNode(rootName) { ImageKey = panel.GroupImageKey, SelectedImageKey = panel.GroupImageKey, Name = rootName, };
            foreach (var attr in ret)
            {
                var paths = $"{attr.Attr.Catgory}/{attr.Attr.Name}".Split("/");
                root.GetOrCreateNodeWithPath(
                    (name, parent) => new TreeNode(attr.Attr.Name) { ImageKey = panel.ChildImageKey, SelectedImageKey = panel.ChildImageKey, Tag = attr, Name = attr.Attr.Name, },
                    (name, parent) => new TreeNode(name) { ImageKey = panel.GroupImageKey, SelectedImageKey = panel.GroupImageKey, Name = name, },
                    paths);
            }
            return root;
        }
        #endregion--------------------------------------------------------------------------------------------------------------------
        /*
        public static G2DTreeNode AddTemplate(object data, G2DTreeViewDataPanel panel, IReadOnlyDictionary<Type, G2DTreeViewDataPanel> panels, object root = null)
        {
            if (data != null)
            {
                if (data is TemplateData tdata)
                {
                    if (root is TemplateData rdata)
                    {
                        tdata.Name = $"{rdata.Name}-{tdata.Name}";
                    }
                    var group = panel.GetSelectedGroup();
                    var node = panel.AddNodeWithData(group, panel.NewID(group), tdata);
                    return node;
                }
                else if (data is TemplateGroup tgroup)
                {
                    var nameKV = new HashMap<Type, HashMap<int, int>>();
                    {
                        if (root is TemplateData rdata)
                        {
                            tgroup.Main.Name = $"{rdata.Name}-{tgroup.Main.Name}";
                        }
                        var kvs = nameKV.GetOrNew((tgroup.Main.GetType()));
                        var group = panel.GetSelectedGroup();
                        var newID = int.Parse(panel.NewID(group));
                        kvs.Put(tgroup.Main.ID, newID);
                        tgroup.Main.ID = newID;
                    }
                    foreach (var sub in tgroup.Subs ?? new TemplateData[0])
                    {
                        if (root is TemplateData rdata)
                        {
                            sub.Name = $"{rdata.Name}-{sub.Name}";
                        }
                        var kvs = nameKV.GetOrNew((sub.GetType()));
                        var p = panels[sub.GetType()];
                        var group = p.GetSelectedGroup();
                        var newID = int.Parse(p.NewID(group, id => kvs.ContainsValue(int.Parse(id))));
                        kvs.Put(sub.ID, newID);
                        sub.ID = newID;
                    }
                    BananceID(data, nameKV);
                    {
                        var node = panel.AddNodeWithData(panel.GetSelectedGroup(), tgroup.Main);
                        foreach (var sub in tgroup.Subs ?? new TemplateData[0])
                        {
                            var p = panels[sub.GetType()];
                            p.AddNodeWithData(p.GetSelectedGroup(), sub);
                        }
                        panel.TreeView.SelectedNode = node;
                        return node;
                    }
                }
            }
            return null;
        }
        */

        /// <summary>
        /// 直接创建模板数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panels"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryCreateTemplateFromGenerator<T>(IReadOnlyDictionary<Type, G2DTreeViewDataPanel> panels, out T data, object root = null) where T : TemplateData
        {
            var dtype = typeof(T);
            var panel = panels[dtype];
            var node = CreateTemplateFromGenerator(panel, panels, root);
            data = node?.Data as T;
            return data != null;
        }
        /// <summary>
        /// 直接创建模板数据
        /// </summary>
        /// <param name="dtype"></param>
        /// <param name="panels"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryCreateTemplateFromGenerator(Type dtype, IReadOnlyDictionary<Type, G2DTreeViewDataPanel> panels, out TemplateData data, object root = null)
        {
            var panel = panels[dtype];
            var node = CreateTemplateFromGenerator(panel, panels, root);
            data = node?.Data as TemplateData;
            return data != null;
        }
        /// <summary>
        /// 直接创建模板数据
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="panels"></param>
        /// <returns></returns>
        public static G2DTreeNode CreateTemplateFromGenerator(G2DTreeViewDataPanel panel, IReadOnlyDictionary<Type, G2DTreeViewDataPanel> panels, object root = null)
        {
            var dtype = panel.DataType;
            if (TemplateGenerator.TryGetGenList(dtype, out var genList))
            {
                var rootNode = ConvertToTreeNodeList(panel, "模板", genList);
                var dialog = new G2DListSelectEditor<AttributeMember<GeneratorMethodAttribute, MethodInfo>>(rootNode, panel.ImageList, default(object));
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var method = dialog.SelectedTag;
                    if (method != null)
                    {
                        try
                        {
                            var data = method.Member.Invoke(null, new object[0]);
                            if (data != null)
                            {
                                if (data is TemplateData tdata)
                                {
                                    if (root is TemplateData rdata)
                                    {
                                        tdata.Name = $"{rdata.Name}-{tdata.Name}";
                                    }
                                    var group = panel.GetSelectedGroup();
                                    var node = panel.AddNodeWithData(group, panel.NewID(group), tdata);
                                    return node;
                                }
                                else if (data is TemplateGroup tgroup)
                                {
                                    var nameKV = new HashMap<Type, HashMap<int, int>>();
                                    {
                                        if (root is TemplateData rdata)
                                        {
                                            tgroup.Main.Name = $"{rdata.Name}-{tgroup.Main.Name}";
                                        }
                                        var kvs = nameKV.GetOrNew((tgroup.Main.GetType()));
                                        var group = panel.GetSelectedGroup();
                                        var newID = int.Parse(panel.NewID(group));
                                        kvs.Put(tgroup.Main.ID, newID);
                                        tgroup.Main.ID = newID;
                                    }
                                    foreach (var sub in tgroup.Subs ?? new TemplateData[0])
                                    {
                                        if (root is TemplateData rdata)
                                        {
                                            sub.Name = $"{rdata.Name}-{sub.Name}";
                                        }
                                        var kvs = nameKV.GetOrNew((sub.GetType()));
                                        var p = panels[sub.GetType()];
                                        var group = p.GetSelectedGroup();
                                        var newID = int.Parse(p.NewID(group, id => kvs.ContainsValue(int.Parse(id))));
                                        kvs.Put(sub.ID, newID);
                                        sub.ID = newID;
                                    }
                                    BananceID(data, nameKV);
                                    {
                                        var node = panel.AddNodeWithData(panel.GetSelectedGroup(), tgroup.Main);
                                        foreach (var sub in tgroup.Subs ?? new TemplateData[0])
                                        {
                                            var p = panels[sub.GetType()];
                                            p.AddNodeWithData(p.GetSelectedGroup(), sub);
                                        }
                                        panel.TreeView.SelectedNode = node;
                                        return node;
                                    }
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            err.ShowMessageBox();
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 基于一个模板创建附加模板数据
        /// </summary>
        /// <param name="main"></param>
        /// <param name="panels"></param>
        /// <returns></returns>
        public static G2DTreeNode AppendTemplateFromGenerator(G2DTreeViewDataPanel basePanel, TemplateData main, IReadOnlyDictionary<Type, G2DTreeViewDataPanel> panels)
        {
            if (TemplateGenerator.TryGetAppendList(main.GetType(), out var appendList))
            {
                var rootNode = ConvertToTreeNodeList(basePanel, "模板", appendList);
                var dialog = new G2DListSelectEditor<AttributeMember<AppendMethodAttribute, MethodInfo>>(rootNode, basePanel.ImageList, default(object));
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var method = dialog.SelectedTag;
                    if (method != null)
                    {
                        try
                        {
                            var data = method.Member.Invoke(null, [main]);
                            if (data != null)
                            {
                                if (data is TemplateData tdata)
                                {
                                    tdata.Name = $"{main.Name}-{tdata.Name}";
                                    var panel = panels[tdata.GetType()];
                                    var group = panel.GetSelectedGroup();
                                    var node = panel.AddNodeWithData(group, panel.NewID(group), tdata);
                                    return node;
                                }
                                else if (data is TemplateGroup tgroup)
                                {
                                    var nameKV = new HashMap<Type, HashMap<int, int>>();
                                    foreach (var sub in tgroup.Subs ?? new TemplateData[0])
                                    {
                                        sub.Name = $"{main.Name}-{sub.Name}";
                                        var kvs = nameKV.GetOrNew((sub.GetType()));
                                        var p = panels[sub.GetType()];
                                        var group = p.GetSelectedGroup();
                                        var newID = int.Parse(p.NewID(group, id => kvs.ContainsValue(int.Parse(id))));
                                        kvs.Put(sub.ID, newID);
                                        sub.ID = newID;
                                    }
                                    BananceID(data, nameKV);
                                    {
                                        var node = default(G2DTreeNode);
                                        foreach (var sub in tgroup.Subs ?? new TemplateData[0])
                                        {
                                            var p = panels[sub.GetType()];
                                            node = node ?? p.AddNodeWithData(p.GetSelectedGroup(), sub);
                                        }
                                        return node;
                                    }
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            err.ShowMessageBox();
                        }
                    }
                }
            }
            return null;
        }


        public static bool BeginFillTemplateFromGenerator(G2DOwnerPropertyDescriptor prop)
        {
            var type = prop.DecleardFieldType;
            if (type == typeof(LaunchSpell))
            {
                return true;
            }
            if (type == typeof(LaunchSkill))
            {
                return true;
            }
            if (type == typeof(LaunchBuff))
            {
                return true;
            }
            if (type == typeof(LaunchAura))
            {
                return true;
            }
            if (prop is MemberPropertyDescriptor mprop)
            {
                if (mprop.Member.TryGetAttribute<TemplateIDAttribute>(out var tid))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool TryFillTemplateFromGenerator(object root, G2DOwnerPropertyDescriptor prop, IReadOnlyDictionary<Type, G2DTreeViewDataPanel> panels, out object value)
        {
            var type = prop.DecleardFieldType;
            if (type == typeof(LaunchSpell) && TryCreateTemplateFromGenerator<SpellTemplate>(panels, out var spell, root))
            {
                value = new LaunchSpell() { SpellID = spell.ID, };
                return true;
            }
            if (type == typeof(LaunchSkill) && TryCreateTemplateFromGenerator<SkillTemplate>(panels, out var skill, root))
            {
                value = new LaunchSkill() { SkillID = skill.ID, };
                return true;
            }
            if (type == typeof(LaunchBuff) && TryCreateTemplateFromGenerator<BuffTemplate>(panels, out var buff, root))
            {
                value = new LaunchBuff() { BuffID = buff.ID, };
                return true;
            }
            if (type == typeof(LaunchAura) && TryCreateTemplateFromGenerator<AuraTemplate>(panels, out var aura, root))
            {
                value = new LaunchAura() { AuraID = aura.ID, };
                return true;
            }
            if (prop is MemberPropertyDescriptor mprop)
            {
                if (mprop.Member.TryGetAttribute<TemplateIDAttribute>(out var tid))
                {
                    if (TryCreateTemplateFromGenerator(tid.TemplateType, panels, out var tdata, root))
                    {
                        value = tdata.ID;
                        return true;
                    }
                }
            }
            value = null;
            return false;
        }

    }
    //--------------------------------------------------------------------------------------------------------------------------------------
}
