using DeepCore.Game3D.Host;
using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Host.ZoneEditor;
using DeepCore.Game3D.Host.ZoneRuntime;
using DeepCore.Game3D.Slave;
using DeepMetaGame.Data.ZoneEditor;

namespace _Temp_.Battle.Host
{
    public class _Temp_ZoneHostFactory : ZoneHostFactory
    {
        public static _Temp_ZoneHostFactory Factory { get; private set; }
        public _Temp_ZoneHostFactory() { Factory = this; }
    }

}
