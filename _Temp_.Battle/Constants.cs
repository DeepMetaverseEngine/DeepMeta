using DeepMetaGame.Data.Template;

namespace _Temp_.Battle
{
    public static class Constants
    {
        public static byte FORCE_A = 1;
        public static byte FORCE_B = 2;
    }

    public enum MsgHead
    {
        //-----------------------------------------------------------
        START = 0x0bbb0000,
        _Temp_Global,
        _Temp_CFG,
        _Temp_SceneProperties,
        _Temp_UnitProp,
        _Temp_ItemProperties,
        _Temp_AttackProperties,
        _Temp_EffectProperties,
        _Temp_BuffProperties,
        _Temp_SkillProperties,
        _Temp_SpellProperties,
        _Temp_AuraProperties,
        _Temp_CardProperties,
        _Temp_EventProperties,
        _Temp_CustomKeyFrame,

        //-----------------------------------------------------------
        TABLE_START = 0x0bbb2000,

        //-----------------------------------------------------------
        SCENE_START = 0x0bbb3000,

        //-----------------------------------------------------------
        SCENE_Data = 0x0bbb4000,



        //-----------------------------------------------------------
        GUI = 0x0bbb5000,

        //-----------------------------------------------------------
        C2B = 0x0bbb7000,

        //-----------------------------------------------------------
    }

}
