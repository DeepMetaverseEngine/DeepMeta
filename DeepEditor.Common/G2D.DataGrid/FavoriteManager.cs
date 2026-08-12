using DeepCore;
using DeepCore.IO;
using DeepCore.Xml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeepEditor.Common.G2D.DataGrid
{
    public class GridFavoriteManager
    {
        public static GridFavoriteManager Instance { get; private set; } = new GridFavoriteManager();
        public IEnumerable<GridFavoriteData> AllFavorites => allFavo.Values;

        private ListDictionary<string, GridFavoriteData> allFavo = new ListDictionary<string, GridFavoriteData>();
        public GridFavoriteManager() { Instance = this; }
        public virtual void Load(DirectoryInfo dir)
        {
            try
            {
                if (Resource.ExistData($"{dir.FullName}/AllFavorites.xml"))
                {
                    var json = Resource.LoadAllText($"{dir.FullName}/AllFavorites.xml");
                    var all = XmlUtil.XmlTextToObject<GridFavoriteData[]>(json);
                    foreach (var desc in all)
                    {
                        allFavo.Put(desc.ToString(), desc);
                    }
                }
            }
            catch { }
        }
        public virtual void Save(DirectoryInfo dir)
        {
            var all = AllFavorites;
            var json = XmlUtil.ObjectToXmlString(all.ToArray());
            CFiles.WriteAllText($"{dir.FullName}/AllFavorites.xml", json);
        }
        public virtual void AddFavorite(G2DFieldElementDesc desc)
        {
            var data = new GridFavoriteData()
            {
                TopTemplateTypeFullName = desc.RootData.GetType().FullName,
                TopTemplateTypeName = desc.RootData.GetType().Name,
                OwnerTypeFullName = desc.ComponentData.GetType().FullName,
                OwnerTypeName = desc.ComponentData.GetType().Name,
                FieldName = $"{desc.FieldName}",
            };
            allFavo.Put(data.ToString(), data);
        }
        public virtual bool RemoveFavorite(GridFavoriteData data)
        {
            return allFavo.Remove(data.ToString());
        }
        public virtual bool TryGetFavorite(object top, object owner, string fieldName, out GridFavoriteData data)
        {
            var key = $"{top.GetType().FullName}-{owner.GetType().FullName}-{fieldName}";
            return allFavo.TryGetValue(key, out data);
        }
    }

    public class GridFavoriteData
    {
        public string TopTemplateTypeName;
        public string TopTemplateTypeFullName;
        public string OwnerTypeFullName;
        public string OwnerTypeName;
        public string FieldName;
        public override string ToString()
        {
            return $"{TopTemplateTypeFullName}-{OwnerTypeFullName}-{FieldName}";
        }
    }
}
