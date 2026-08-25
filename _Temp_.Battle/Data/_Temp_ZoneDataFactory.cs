using DeepCore.Reflection;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Misc;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using _Temp_.Battle.Data.Properties;

namespace _Temp_.Battle.Data
{
    //----------------------------------------------------------------------------------------------

    [Desc("_Temp_数据扩展")]
    public class _Temp_ZoneDataFactory : ZoneDataFactory
    {
        new public static _Temp_ZoneDataFactory Factory { get; private set; }
        public _Temp_ZoneDataFactory()
        {
            Factory = this;
            base.RegistPropertiesType(
                 typeof(_Temp_Global),
                 typeof(_Temp_CFG),
                 typeof(_Temp_SceneProperties),
                 typeof(_Temp_UnitProp),
                 typeof(_Temp_ItemProperties),
                 typeof(_Temp_SkillProperties),
                 typeof(_Temp_SpellProperties),
                 typeof(_Temp_BuffProperties),
                 typeof(_Temp_AuraProperties),
                 typeof(_Temp_CardProperties),
                 typeof(_Temp_EventProperties),
                 typeof(_Temp_AttackProperties),
                 typeof(_Temp_EffectProperties),
                 typeof(_Temp_CustomKeyFrame)
                 );
        }
        public override ICommonConfig CreateCommonCFG() => new _Temp_CFG();
        public override IGlobalConfig CreateGlobalCFG() => new _Temp_Global();
        protected override IPropertiesData CreateProperties(IPropertiesOwner owner, Type type)
        {
            if (owner is SceneData scene) return new _Temp_SceneProperties();
            if (owner is UnitInfo unit) return new _Temp_UnitProp();
            if (owner is ItemTemplate item) return new _Temp_ItemProperties();
            if (owner is SkillTemplate skill) return new _Temp_SkillProperties();
            if (owner is SpellTemplate spell) return new _Temp_SpellProperties();
            if (owner is BuffTemplate buff) return new _Temp_BuffProperties();
            if (owner is AuraTemplate aura) return new _Temp_AuraProperties();
            if (owner is CardTemplate card) return new _Temp_CardProperties();
            if (owner is UnitEventTemplate uevent) return new _Temp_EventProperties();
            if (owner is AttackProp attack) return new _Temp_AttackProperties();
            if (owner is LaunchEffect effect) return new _Temp_EffectProperties();
            if (owner is BaseKeyFrame kf) return new _Temp_CustomKeyFrame();
            throw new NotImplementedException();
        }
        public override EditorDataCenter CreateDataCenter(EditorTemplates root)
        {
            return new _Temp_DataCenter(root.EditorRoot);
        }
    }

}
