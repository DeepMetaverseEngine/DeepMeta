using DeepCore.Game3D.Slave.Layer;
using DeepCore.Game3D.Slave.War3;
using DeepMetaGame.Data.ZoneEditor;

namespace _Temp_.Battle.Slave
{
    public class _Temp_ZoneSlaveFactory : War3ZoneSlaveFactory
    {
        public static _Temp_ZoneSlaveFactory Factory { get; private set; }
        public _Temp_ZoneSlaveFactory() { Factory = this; }
        protected override LayerZone CreateClientZoneLayer(EditorTemplates templates, ILayerZoneListener listener)
        {
            return new _Temp_ZoneSlave(templates, this, listener);
        }
    }
}
