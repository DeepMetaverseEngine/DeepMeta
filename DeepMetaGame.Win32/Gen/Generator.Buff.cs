using DeepCore;
using DeepCore.Reflection;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Editor.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepMetaGame.Win32.Gen
{
    partial class Generator
    {
        [GeneratorMethod(typeof(BuffTemplate),  "创建减速BUFF", "基础")]
        public static BuffTemplate CreateSpeedDownBuff()
        {
            var ret = new BuffTemplate();
            ret.Name = "减速BUFF";
            ret.Abilities = new()
            {
                new BuffSpeedChangeAbility()
                {
                    FastActionRate = 0.5f,
                    FastCastRate = 0.5f,
                    FastMoveRate = 0.5f,
                }
            };
            return ret;
        }
        [GeneratorMethod(typeof(BuffTemplate), "创建加速BUFF", "基础")]
        public static BuffTemplate CreateSpeedUpBuff()
        {
            var ret = new BuffTemplate();
            ret.Name = "加速BUFF";
            ret.Abilities = new()
            {
                new BuffSpeedChangeAbility()
                {
                    FastActionRate = 1.5f,
                    FastCastRate = 1.5f,
                    FastMoveRate = 1.5f,
                }
            };
            return ret;
        }
        [GeneratorMethod(typeof(BuffTemplate), "创建无敌BUFF", "基础")]
        public static BuffTemplate CreateInvincibleBuff()
        {
            var ret = new BuffTemplate();
            ret.Name = "无敌BUFF";
            ret.Abilities = new() { new BuffStateChangeAbility() { IsInvincible = true, } };
            return ret;
        }
        [GeneratorMethod(typeof(BuffTemplate), "创建隐身BUFF", "基础")]
        public static BuffTemplate CreateInvisibleBuff()
        {
            var ret = new BuffTemplate();
            ret.Name = "隐身UFF";
            ret.Abilities = new() { new BuffStateChangeAbility() { IsInvisible = true, } };
            return ret;
        }
        [GeneratorMethod(typeof(BuffTemplate), "创建眩晕BUFF", "基础")]
        public static BuffTemplate CreateStunBuff()
        {
            var ret = new BuffTemplate();
            ret.Name = "眩晕UFF";
            ret.Abilities = new() { new BuffStateChangeAbility() { MakeStun = true, } };
            return ret;
        }
        [GeneratorMethod(typeof(BuffTemplate), "创建地锁BUFF", "基础")]
        public static BuffTemplate CreateLockMotionBuff()
        {
            var ret = new BuffTemplate();
            ret.Name = "地锁UFF";
            ret.Abilities = new() { new BuffStateChangeAbility() { IsLockMotion = true, } };
            return ret;
        }
        [GeneratorMethod(typeof(BuffTemplate), "创建沉默BUFF", "基础")]
        public static BuffTemplate CreateSilentBuff()
        {
            var ret = new BuffTemplate();
            ret.Name = "沉默BUFF";
            ret.Abilities = new() { new BuffStateChangeAbility() { IsSilent = true, } };
            return ret;
        }
        [GeneratorMethod(typeof(BuffTemplate), "创建无伤BUFF", "基础")]
        public static BuffTemplate CreateNoDamageBuff()
        {
            var ret = new BuffTemplate();
            ret.Name = "无伤BUFF";
            ret.Abilities = new() { new BuffStateChangeAbility() { IsNoDamage = true, } };
            return ret;
        }
        [GeneratorMethod(typeof(BuffTemplate), "创建霸体BUFF", "基础")]
        public static BuffTemplate CreateNoneBlockBuff()
        {
            var ret = new BuffTemplate();
            ret.Name = "霸体BUFF";
            ret.Abilities = new() { new BuffStateChangeAbility() { IsNoneBlock = true, } };
            return ret;
        }
    }
}
