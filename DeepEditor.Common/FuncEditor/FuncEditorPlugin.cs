using DeepCore.FuncData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.FuncEditor
{
    public abstract class FuncEditorPlugin
    {
        public static FuncEditorPlugin Instance { get; private set; }

        public FuncEditorPlugin()
        {
            Instance = this;
        }

        public virtual ImageList GetTempaltesImageList()
        {
            return null;
        }
        public virtual List<IFuncTemplateData> GetEditorTemplatesData()
        {
            return new List<IFuncTemplateData>();
        }
        public virtual string GetImageKeyByTemplateType(Type type)
        {
            return "";
        }
        public virtual string GetFileTreeNodeImageKey()
        {
            return "icons_tool_bar2.png";
        }
        public virtual string GetSheetTreeNodeImageKey()
        {
            return "icons_tool_bar1.png";
        }
        public virtual void BuildFuncDatas()
        {

        }
    }
}
