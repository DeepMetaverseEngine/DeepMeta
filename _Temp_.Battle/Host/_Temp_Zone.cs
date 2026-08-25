using DeepCore;
using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Host.Instance.Abilities;
using DeepCore.Game3D.Host.ZoneEditor;
using DeepCore.Geometry;
using DeepCore.Geometry.Terrain;
using DeepCore.Reflection;
using DeepMetaGame.Data.Helper;
using DeepMetaGame.Data.Misc;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;

namespace _Temp_.Battle.Host
{
    public class _Temp_Zone : EditorScene
    {

        public _Temp_Zone(InstanceZoneListener listener, _Temp_ZoneHostFactory hostFactory, EditorTemplates dataroot, SceneData data, int randomSeed = 1)
            : base(listener, hostFactory, dataroot, data, randomSeed)
        {
        }
        protected override void Disposing()
        {
            base.Disposing();
        }
    }
}
