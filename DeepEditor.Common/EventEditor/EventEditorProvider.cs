using DeepCore.EventTrigger;
using DeepCore.EventTrigger.Data;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G2D.DataGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepEditor.Common.EventEditor
{

    public interface IEventEditorProvider
    {
        string EditorName { get; }
        ValueTypeNameSpace NameSpace { get; }
        IG2DPropertyAdapter[] PropertyAdapters { get; }


        IEnvironmentVar CreateEnvironmentVar();
        void SaveEnvironmentVars(List<IEnvironmentVar> vars);
        List<IEnvironmentVar> LoadEnvironmentVars();


        IEventDataNode CreateEventDataNode();
        List<IEventDataNode> LoadEventDataNodes();
        void SaveEventDataNodes(List<IEventDataNode> events);

    }

    public interface IEventNodeEditor
    {
        void ListEventLocalVar(Action<EventLocalVar> vars);
    }

    public static class EventEditorProviderExt
    {
        public static IEventDataNode GetEventData(this IEventEditorProvider p, string name)
        {
            if (name != null)
            {
                foreach (var evt in p.LoadEventDataNodes())
                {
                    if (evt.EventName.Equals(name))
                    {
                        return evt;
                    }
                }
            }
            return null;
        }
        public static IEventDataNode ShowSelectEvent(this IEventEditorProvider p, string srcName)
        {
            var src = GetEventData(p, srcName);
            var dialog = new G2DListSelectEditor<IEventDataNode>(
                    p.LoadEventDataNodes(), src);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedTag as IEventDataNode;
            }
            return null;
        }
        public static IEnvironmentVar GetEnvironmentVarData(this IEventEditorProvider p, string name)
        {
            if (name != null)
            {
                foreach (var var in p.LoadEnvironmentVars())
                {
                    if (var.Key.Equals(name))
                    {
                        return var;
                    }
                }
            }
            return null;
        }
        public static List<IEnvironmentVar> GetEnvironmentVarDatasAsType(this IEventEditorProvider p, Type valueType)
        {
            if (valueType == null)
            {
                var ret = new List<IEnvironmentVar>();
                foreach (var var in p.LoadEnvironmentVars())
                {
                    ret.Add(var);
                }
                return ret;
            }
            else
            {
                var ret = new List<IEnvironmentVar>();
                foreach (var var in p.LoadEnvironmentVars())
                {
                    if (valueType.IsInstanceOfType(var.Value))
                    {
                        ret.Add(var);
                    }
                }
                return ret;
            }
        }
        public static IEnvironmentVar ShowSelectEnvironmentVar(this IEventEditorProvider p, string srcKey, Type valueType)
        {
            var src = GetEnvironmentVarData(p, srcKey);
            var dialog = new G2DListSelectEditor<IEnvironmentVar>(
                GetEnvironmentVarDatasAsType(p, valueType), src);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedTag;
            }
            return null;
        }

    }
}
