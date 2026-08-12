using DeepCore;
using DeepCore.Reflection;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Editor.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepMetaGame.Data.Misc;

namespace DeepMetaGame.Win32.Gen
{
    partial class Generator
    {
        //-------------------------------------------------------------------------------
        [GeneratorMethod(typeof(SpellTemplate), "创建直线子弹","基础")]
        public static SpellTemplate CreateStraight()
        {
            var ret = new SpellTemplate();
            ret.Name = "直线子弹";

            ret.MType = SpellTemplate.MotionType.Straight;
            ret.MSpeedSEC = 10;

            ret.LifeTimeMS = 10000;

            ret.BodySize = 0.25f;
            ret.BodyHeight = 0.125f;

            ret.HitIntervalMS = 0;
            ret.OnceHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {

                }
            };

            return ret;
        }
        
        [GeneratorMethod(typeof(SpellTemplate), "创建朝向子弹", "基础")]
        public static SpellTemplate CreateForward()
        {
            var ret = new SpellTemplate();
            ret.Name = "朝向子弹";

            ret.MType = SpellTemplate.MotionType.Forward;
            ret.MSpeedSEC = 10;
            ret.LifeTimeMS = 1000;
            ret.BodySize = 0.25f;
            ret.BodyHeight = 0.125f;
            ret.HitIntervalMS = 0;
            ret.OnceHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {

                }
            };

            return ret;
        }
        //-------------------------------------------------------------------------------
        [GeneratorMethod(typeof(SpellTemplate), "创建跟踪弹", "基础")]
        public static SpellTemplate CreateMissile()
        {
            var ret = new SpellTemplate();
            ret.Name = "跟踪弹";

            ret.MType = SpellTemplate.MotionType.Missile;
            ret.MSpeedSEC = 10;

            ret.LifeTimeMS = 10000;

            ret.BodySize = 0.25f;
            ret.BodyHeight = 0.125f;

            ret.HitOnExplosion = true;
            ret.HitOnExplosionKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {

                }
            };
            return ret;
        }
        //-------------------------------------------------------------------------------
        [GeneratorMethod(typeof(SpellTemplate), "创建跟踪弹（自动锁敌，不需要指定目标）", "基础")]
        public static SpellTemplate CreateSeekerMissile()
        {
            var ret = new SpellTemplate();
            ret.Name = "跟踪弹-自动锁敌";

            ret.MType = SpellTemplate.MotionType.SeekerMissile;
            ret.MSpeedSEC = 10;

            ret.LifeTimeMS = 10000;

            ret.BodySize = 0.25f;
            ret.BodyHeight = 0.125f;

            ret.SeekingCooldownMS = 1000; // 自动锁敌冷却时间
            ret.HitOnExplosion = true;
            ret.HitOnExplosionKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {

                }
            };
            return ret;
        }
        //-------------------------------------------------------------------------------
        [GeneratorMethod(typeof(SpellTemplate), "创建范围攻击", "基础")]
        public static SpellTemplate CreateRange()
        {
            var ret = new SpellTemplate();
            ret.Name = "范围攻击";

            ret.MType = SpellTemplate.MotionType.Immovability;
            ret.MSpeedSEC = 10;

            ret.LifeTimeMS = 5000;

            ret.BodySize = 3f;
            ret.BodyHeight = 1f;

            ret.HitIntervalMS = 1000;
            ret.IntervalHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {

                }
            };

            return ret;
        }
        //-------------------------------------------------------------------------------
        [GeneratorMethod(typeof(SpellTemplate), "绑定角色AOE范围攻击（以施法者为中心，向外扩散）", "基础")]
        public static SpellTemplate CreateAOE()
        {
            var ret = new SpellTemplate();
            ret.Name = "角色AOE";

            ret.MType = SpellTemplate.MotionType.Binding;

            ret.MSpeedSEC = 3f;
            ret.LifeTimeMS = 3000;

            ret.BodySize = 1f;
            ret.BodyHeight = 1f;
            ret.AOEMType = SpellTemplate.AoeMotionType.Linear;

            ret.HitIntervalMS = 0;
            ret.OnceHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {

                }
            };

            return ret;
        }
        //-------------------------------------------------------------------------------
        [GeneratorMethod(typeof(SpellTemplate), "绑定角色绕圈（绑定在施法者身边绕圈）", "基础")]
        public static SpellTemplate CreateRotate()
        {
            var ret = new SpellTemplate();
            ret.Name = "角色AOE";

            ret.MType = SpellTemplate.MotionType.Binding;

            ret.MSpeedSEC = 3f;
            ret.LifeTimeMS = 30000;

            ret.RotateSpeedSEC = CMath.PI_MUL_2;
            ret.OrbitDistance = 3f;
            ret.IsBindingDirection = false;
            ret.IsBindingOrbit = true;

            ret.BodySize = 0.25f;
            ret.BodyHeight = 0.5f;

            ret.HitIntervalMS = 0;
            ret.CleanHitIntervalMS = 1000; // 每隔1秒清除一次命中状态
            ret.OnceHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {

                }
            };

            return ret;
        }
        //-------------------------------------------------------------------------------

        [GeneratorMethod(typeof(SpellTemplate), "创建直线子弹，并增加弹射", "Spell产生Spell")]
        public static TemplateGroup CreateStraightAndSplit()
        {
            var ret = CreateStraight();
            var sub = CreateStraight();
            sub.Name = "直线子弹-弹射";
            sub.ID = 33333;
            ret.OnceHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {
                    Spell = new Data.Misc.LaunchSpell()
                    {
                        SpellID = sub.ID,
                        SenderUnit = Data.Misc.LaunchSpell.LaunchSpellSenderUnit.Sender,
                        InheritDamageTargetList = true,
                        IsAutoSeekingTarget = true,
                        SeekingTargetRange = 12,
                    },
                }
            };
            return new TemplateGroup() { Main = ret, Subs = [sub] };
        }
        //-------------------------------------------------------------------------------


        [GeneratorMethod(typeof(SpellTemplate), "创建直线子弹，并增加减速BUFF", "Spell产生Buff")]
        public static TemplateGroup CreateStraightAndSpeedDown()
        {
            var ret = CreateStraight();
            ret.Name = "直线子弹-减速BUFF";
            var sub = CreateSpeedDownBuff();
            sub.ID = 33333;
            ret.OnceHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {                   
                    Buff = new Data.Misc.LaunchBuff()
                    {
                        BuffID = sub.ID,
                    },
                }
            };
            return new TemplateGroup() { Main = ret, Subs = [sub] };
        }
        
        
        [GeneratorMethod(typeof(SpellTemplate), "创建跟踪子弹，命中后爆开（圆形）", "Spell产生Spell")]
        public static TemplateGroup CreateSeekMissleAndExplosionCircle()
        {
            var ret = CreateMissile();
            var sub = CreateForward();
            sub.Name = "跟踪子弹-爆炸圆形";
            sub.ID = 33333;
            ret.OnceHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {
                    Spell = new Data.Misc.LaunchSpell()
                    {
                        Count = 6,
                        SpellID = sub.ID,
                        PType = LaunchSpell.PosType.POS_TYPE_CYCLE,
                        SenderUnit = Data.Misc.LaunchSpell.LaunchSpellSenderUnit.Sender,
                        InheritDamageTargetList = true,
                    },
                }
            };
            return new TemplateGroup() { Main = ret, Subs = [sub] };
        }
        [GeneratorMethod(typeof(SpellTemplate), "创建跟踪子弹，命中后爆开(扇形)", "Spell产生Spell")]
        public static TemplateGroup CreateSeekMissleAndExplosionFan()
        {
            var ret = CreateMissile();
            var sub = CreateForward();
            sub.Name = "跟踪子弹-爆炸扇形";
            sub.ID = 33333;
            ret.OnceHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {
                    Spell = new Data.Misc.LaunchSpell()
                    {
                        Count = 3,
                        SpellID = sub.ID,
                        Angle360 = 180,
                        PType = LaunchSpell.PosType.POS_TYPE_FAN,
                        SenderUnit = Data.Misc.LaunchSpell.LaunchSpellSenderUnit.Sender,
                        InheritDamageTargetList = true,
                    },
                }
            };
            return new TemplateGroup() { Main = ret, Subs = [sub] };
        }
        
        [GeneratorMethod(typeof(SpellTemplate), "创建跟踪子弹，命中后先直行后跟踪寻怪", "Spell产生Spell")]
        public static TemplateGroup CreateSeekMissleAndExplosionSeek()
        {
            var ret = CreateMissile();
            var sub = CreateSeekerMissile();
            sub.Name = "跟踪子弹-先直行后跟踪寻怪";
            sub.ID = 33333;
           
            ret.OnceHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {
                    Spell = new Data.Misc.LaunchSpell()
                    {
                        Count = 3,
                        SpellID = sub.ID,
                        Angle360 = 180,
                        PType = LaunchSpell.PosType.POS_TYPE_FAN,
                        SenderUnit = Data.Misc.LaunchSpell.LaunchSpellSenderUnit.Sender,
                        InheritDamageTargetList = true,
                    },
                }
            };
            //不能用OncehitkeyFrame 一定要开启hitOnExplosion 
            sub.HitOnExplosion = true;
            ret.HitOnExplosionKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {
                    
                }
            };
            return new TemplateGroup() { Main = ret, Subs = [sub] };
        }
        
        //-------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------
    }
}
