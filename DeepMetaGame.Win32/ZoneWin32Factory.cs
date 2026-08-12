using DeepCore.Game3D.Host.ZoneRuntime;
using DeepCore.Game3D.Slave.Layer;
using DeepCore.PomeloClient;
using DeepEditor.Plugin3D.BattleClient;
using DeepMetaGame.Data.ZoneEditor;
using OpenTK.WinForms;
using static DeepEditor.Plugin3D.BattleClient.PanelBattleView3D;

namespace DeepMetaGame.Win32
{
    public class ZoneWin32Factory
    {
        public static ZoneWin32Factory Instance { get; private set; } = new ZoneWin32Factory();
        public ZoneWin32Factory()
        {
            Instance = this;
        }
        public virtual BattleView3D CreateBattleView(GLControl control, System.Windows.Forms.Timer timer) => new BattleView3D(control, timer);
        public virtual LayerZoneUnit3D CreateUnitView(BattleView3D parent, LayerUnit obj) => new LayerZoneUnit3D(parent, obj);
        public virtual LayerZoneSpell3D CreateSpellView(BattleView3D parent, LayerSpell obj) => new LayerZoneSpell3D(parent, obj);
        public virtual LayerZoneItem3D CreateItemView(BattleView3D parent, LayerItem obj) => new LayerZoneItem3D(parent, obj);

        public virtual LayerZoneFlag3D CreateFlagView(BattleView3D parent, LayerFlag obj)
        {
            if (obj is LayerEditorDecoration zed) return new LayerZoneDecoration3D(parent, zed);
            if (obj is LayerEditorRegion zer) return new LayerZoneRegion3D(parent, zer);
            if (obj is LayerEditorArea zea) return new LayerZoneArea3D(parent, zea);
            if (obj is LayerEditorPoint zep) return new LayerZonePoint3D(parent, zep);
            return null;
        }

        public virtual InstanceBattle CreateBattle(EditorTemplates templates, BattleConfig cfg, SceneData sceneData)
        {
            if (cfg.exeType == LocalExecuteType.Thread)
            {
                return new ThreadBattleSinglePlay(templates, cfg.hostFactory, cfg.slaveFactory, sceneData);
            }
            else if (cfg.exeType == LocalExecuteType.Node)
            {
                return new ZoneNodeBattle(templates, cfg.hostFactory, cfg.slaveFactory, sceneData);
            }
            else if (cfg.exeType == LocalExecuteType.Preview)
            {
                return cfg.hostFactory.CreatePreview(templates, cfg.slaveFactory, sceneData);
            }
            else
            {
                return new LocalBattleSinglePlay(templates, cfg.hostFactory, cfg.slaveFactory, sceneData);
            }
        }
    }
}

