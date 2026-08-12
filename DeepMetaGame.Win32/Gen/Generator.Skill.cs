using DeepCore;
using DeepCore.Reflection;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Editor.Gen;
using glTFLoader.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepMetaGame.Win32.Gen
{
    partial class Generator
    {
        //-------------------------------------------------------------------------------
        [GeneratorMethod(typeof(SkillTemplate), "创建近战扇形判定技能", "基础")]
        public static SkillTemplate CreateMeleeFanSkill()
        {
            var main = new SkillTemplate();
            main.Name = "近战扇形判定技能";
            main.AttackShape = new UnitActionData.AttackShape()
            {
                AShape = UnitActionData.AttackShape.Shape.Fan,
                AttackAngle360 = 45,
                AttackRange = 3f,
            };
            main.ActionQueue = new ArrayList<UnitActionData>()
            {
                new UnitActionData()
                {
                    TotalTimeMS = 1000,
                    KeyFrames = new ArrayList<UnitActionData.KeyFrame>()
                    {
                        new UnitActionData.KeyFrame()
                        {
                             FrameMS = 500,
                             Attack = new  Data.Misc.AttackProp()
                             {

                             }
                        }
                    }
                }
            };
            return main;
        }
        //-------------------------------------------------------------------------------
        [GeneratorMethod(typeof(SkillTemplate), "创建发射子弹技能", "基础")]
        public static TemplateGroup CreateSpellSkill()
        {
            var main = new SkillTemplate();
            main.Name = "发射子弹技能";

            var sub = CreateStraight();
            sub.Name = "直线子弹";
            sub.ID = 33333;
            sub.BodySize = 0.5f;
            sub.HitIntervalMS = 0;
            sub.OnceHitKeyFrame = new SpellTemplate.KeyFrame()
            {
                Attack = new Data.Misc.AttackProp()
                {
                    
                }
            };

            main.AttackShape = new UnitActionData.AttackShape()
            {
                AShape = UnitActionData.AttackShape.Shape.Fan,
                AttackAngle360 = 45,
                AttackRange = 3f,
            };
            main.ActionQueue = new ArrayList<UnitActionData>()
            {
                new UnitActionData()
                {
                    TotalTimeMS = 1000,
                    KeyFrames = new ArrayList<UnitActionData.KeyFrame>()
                    {
                        new UnitActionData.KeyFrame()
                        {
                             FrameMS = 500,
                             Spell = new Data.Misc.LaunchSpell()
                             {
                                 SpellID = sub.ID,
                             }
                        }
                    }
                }
            };
            return new TemplateGroup() { Main = main, Subs = [sub] };
        }
        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        [AppendMethod(typeof(SkillTemplate), "创建命中增加弹射", "弹射")]
        public static TemplateGroup AppendHitSplitSpell(SkillTemplate skill)
        {
            var sub = CreateStraight();
            sub.Name = "直线子弹-弹射";
            sub.ID = 33333;

            if (skill.ActionQueue != null)
            {
                foreach (var action in skill.ActionQueue)
                {
                    if (action.KeyFrames != null)
                    {
                        foreach (var kf in action.KeyFrames)
                        {

                            if (kf.Attack != null)
                            {
                                kf.Attack.Spell = new Data.Misc.LaunchSpell()
                                {
                                    SpellID = sub.ID,
                                    SenderUnit = Data.Misc.LaunchSpell.LaunchSpellSenderUnit.Sender,
                                    InheritDamageTargetList = true,
                                    IsAutoSeekingTarget = true,
                                    SeekingTargetRange = 12,
                                };
                            }
                        }
                    }
                }
            }

            return new TemplateGroup() { Main = skill, Subs = [sub] };
        }
        //-------------------------------------------------------------------------------
    }
}
