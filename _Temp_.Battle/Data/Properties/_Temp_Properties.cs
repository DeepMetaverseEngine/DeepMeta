using DeepCore.IO;
using DeepCore.Reflection;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Template;

namespace _Temp_.Battle.Data.Properties
{
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_Global"), MessageType(MsgHead._Temp_Global)]
    public class _Temp_Global : IGlobalConfig
    {
        public static _Temp_Global Instance => TemplateManager.Instance.GlobalConfig as _Temp_Global;      
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_Config"), MessageType(MsgHead._Temp_CFG)]
    public class _Temp_CFG : IBaseFuncData, ICommonConfig
    {
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_场景扩展"), MessageType(MsgHead._Temp_SceneProperties)]
    public class _Temp_SceneProperties : IBaseFuncData, ISceneProperties
    {

    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_单位扩展"), MessageType(MsgHead._Temp_UnitProp)]
    public class _Temp_UnitProp : IBaseFuncData, IUnitProperties
    {
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_物品扩展"), MessageType(MsgHead._Temp_ItemProperties)]
    public class _Temp_ItemProperties : IBaseFuncData, IItemProperties
    {
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_攻击扩展"), MessageType(MsgHead._Temp_AttackProperties)]
    public class _Temp_AttackProperties : IBaseFuncData, IAttackProperties
    {
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_特效扩展"), MessageType(MsgHead._Temp_EffectProperties)]
    public class _Temp_EffectProperties : IBaseFuncData, IEffectProperties
    {
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_ Buff扩展"), MessageType(MsgHead._Temp_BuffProperties)]
    public class _Temp_BuffProperties : IBaseFuncData, IBuffProperties
    {
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_技能扩展"), MessageType(MsgHead._Temp_SkillProperties)]
    public class _Temp_SkillProperties : IBaseFuncData, ISkillProperties
    {
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_法术扩展"), MessageType(MsgHead._Temp_SpellProperties)]
    public class _Temp_SpellProperties : IBaseFuncData, ISpellProperties
    {
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_光环扩展"), MessageType(MsgHead._Temp_AuraProperties)]
    public class _Temp_AuraProperties : IBaseFuncData, IAuraProperties
    {

    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_词缀扩展"), MessageType(MsgHead._Temp_CardProperties)]
    public class _Temp_CardProperties : IBaseFuncData, ICardProperties
    {
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_事件扩展"), MessageType(MsgHead._Temp_EventProperties)]
    public class _Temp_EventProperties : IBaseFuncData, IEventProperties
    {
    }
    //----------------------------------------------------------------------------------------------
    [Desc(Category = "_Temp_", Desc = "_Temp_词缀扩展"), MessageType(MsgHead._Temp_CustomKeyFrame)]
    public class _Temp_CustomKeyFrame : IBaseFuncData, IKeyFrameProperties
    {
    }
    //----------------------------------------------------------------------------------------------
}
