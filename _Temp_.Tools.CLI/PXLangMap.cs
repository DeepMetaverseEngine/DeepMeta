using DeepCore.IO;
using DeepCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuMa.Tools
{
    public class PXLangMap
    {
        public readonly HashMap<int, string> lang;
        public PXLangMap(DirectoryInfo editorRootDir, string name) : this(new FileInfo($"{editorRootDir}/templates/xoutres/Lang/{name}.json")) { }
        public PXLangMap(FileInfo file)
        {
            var langJson = Resource.LoadAllText($"{file}");
            this.lang = Newtonsoft.Json.JsonConvert.DeserializeObject<HashMap<int, string>>(langJson);
        }
        public bool TryGet(int id, out string ret)
        {
            if (lang.TryGetValue(id, out ret))
            {
                ret = CleanTag(ret);
                return true;
            }
            return false;
        }
        public string Get(int id)
        {
            if (TryGet(id, out var ret))
            {
                return ret;
            }
            return string.Empty;
        }
        private static string CleanTag(string src)
        {
            while (src.TryIndexOf('[', out var left) && src.TryIndexOf(']', out var right, left))
            {
                if (left > 0)
                {
                    src = src.Substring(0, left) + src.Substring(right + 1);
                }
                else
                {
                    src = src.Substring(right + 1);
                }
            }
            return src;
        }
    }
}
