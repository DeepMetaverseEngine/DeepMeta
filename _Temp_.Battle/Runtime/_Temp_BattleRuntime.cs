using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Host.ZoneEditor;
using DeepCore.Game3D.Host.ZoneRuntime;
using DeepMetaGame.Data.ZoneEditor;
using _Temp_.Battle.Host;
using _Temp_.Battle.Slave;

namespace _Temp_.Battle.Runtime
{
    public class _Temp_BattleRuntime : LocalBattleSinglePlay
    {
        public static _Temp_BattleRuntime Instance { get; private set; }
        public _Temp_BattleRuntime(EditorTemplates data_root, SceneData sd)
            : base(data_root, _Temp_ZoneHostFactory.Factory, _Temp_ZoneSlaveFactory.Factory, sd)
        {
            Instance = this;
        }

        protected override void OnAddLocalPlayer(InstancePlayer actor)
        {
            base.OnAddLocalPlayer(actor);
        }

        protected override EditorScene CreateZone()
        {
            return HostFactory.CreateZone(this, DataRoot, base.SceneData);
        }
        protected override void Disposing()
        {
            base.Disposing();
        }
    }
}
