using DeepCore.IO;
using DeepCore.Reflection;
using DeepMetaGame.Data.ZoneEditor;

namespace _Temp_.Battle.Data
{
    //----------------------------------------------------------------------------------------------
    public partial class _Temp_DataCenter : EditorDataCenter
    {
        public static _Temp_DataCenter Instance { get; private set; }
        public _Temp_DataCenter(string editorRoot) : base("scope", $"{editorRoot}/templates/json/")
        {
            Instance = this;
        }     
    }
    //----------------------------------------------------------------------------------------------
}
