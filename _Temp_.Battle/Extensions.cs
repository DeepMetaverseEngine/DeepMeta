using _Temp_.Battle.Data;
using _Temp_.Battle.Data.Properties;
using _Temp_.Battle.Host;
using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Slave.Layer;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;

namespace _Temp_.Battle
{
    public static class _Temp_HostExt
    {
      extension(TemplateManager templates)
      {
          public _Temp_Global _Temp_Global { get => templates.GlobalConfig as _Temp_Global; }
      }
      extension(InstanceZone zone)
      {
          public _Temp_DataCenter _Temp_DataCenter => zone.Templates.DataCenter as _Temp_DataCenter;
          public _Temp_CFG _Temp_CFG { get => zone.ExtCFGAs<_Temp_CFG>(); }
      }
      extension(InstanceZoneObject o)
      {
          public _Temp_DataCenter _Temp_DataCenter => o.Templates.DataCenter as _Temp_DataCenter;
          public _Temp_Zone _Temp_Zone => o.Zone as _Temp_Zone;
          public _Temp_CFG _Temp_CFG { get => o.ExtCFGAs<_Temp_CFG>(); }
      }
 
      extension(LayerZone zone)
      {
          public _Temp_DataCenter _Temp_DataCenter => zone.Templates.DataCenter as _Temp_DataCenter;
          public _Temp_CFG _Temp_CFG { get => zone.ExtCFGAs<_Temp_CFG>(); }
          public _Temp_Zone HostZone => zone.Sender as _Temp_Zone;
      }
      extension(LayerObject o)
      {
          public _Temp_DataCenter _Temp_DataCenter => o.Templates.DataCenter as _Temp_DataCenter;
          public _Temp_CFG _Temp_CFG { get => o.Parent.ExtCFGAs<_Temp_CFG>(); }
      }
      extension(LayerUnit o)
      {
          public InstanceUnit HostUnit => o.EventSender as InstanceUnit;
      }
      extension(SceneData zone)
      {
          public _Temp_SceneProperties _Temp_Properties => zone.Properties as _Temp_SceneProperties;
      }
      extension(UnitInfo o)
      {
          public _Temp_UnitProp _Temp_Properties => o.Properties as _Temp_UnitProp;
      }
      extension(SkillTemplate o)
      {
          public _Temp_SkillProperties _Temp_Properties => o.Properties as _Temp_SkillProperties;
      }
      extension(SpellTemplate o)
      {
          public _Temp_SpellProperties _Temp_Properties => o.Properties as _Temp_SpellProperties;
      }
    }

}
