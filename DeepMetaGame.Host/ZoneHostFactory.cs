using DeepCore.Game3D.Host.FuncData;
using DeepCore.Game3D.Host.Helper;
using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Host.Instance.Abilities;
using DeepCore.Game3D.Host.Instance.Components;
using DeepCore.Game3D.Host.ZoneEditor;
using DeepCore.Game3D.Host.ZoneEditor.EventTrigger;
using DeepCore.Game3D.Host.ZonePreview;
using DeepCore.Game3D.Host.ZoneRuntime;
using DeepCore.Game3D.Host.ZoneServer;
using DeepCore.Game3D.Host.ZoneServer.Interface;
using DeepCore.Game3D.Slave;
using DeepCore.GameData.EventTrigger;
using DeepCore.Geometry.Terrain;
using DeepCore.GUI.Data;
using DeepCore.Log;
using DeepCore.Reflection;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Message;
using DeepMetaGame.Data.Misc;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeepCore.Game3D.Host
{
    [Reflectible]
    public abstract class ZoneHostFactory : IBattleFactory
    {
        //         private static ZoneHostFactory instance;
        //         public static ZoneHostFactory Factory
        //         {
        //             get
        //             {
        //                 if (instance == null)
        //                 {
        //                     instance = new Simple.SimpleZoneHostFactory();
        //                 }
        //                 return instance;
        //             }
        //         }
        /// <summary>
        /// 编辑器根地址
        /// </summary>
        //public string GameEditorRoot { get=>DataFactory.GameEditorRoot; }
        public ZoneDataFactory DataFactory { get; }
        //public static bool IsXmlAlias { get; set; } = false;
        //---------------------------------------------------------------------------------------------

        protected ZoneHostFactory()
        {
            this.DataFactory = ZoneDataFactory.Factory;
            //instance = this;
            new ZoneValueTypeNameSpace();
            //ZoneDataFactory.Factory.OnEditorCheckExistDatas += Factory_OnCheckExistDatas;
            //ZoneDataFactory.Factory.OnEditorSaveSceneData += Factory_OnEditorSaveSceneData;
            //GameFields.SetInstanceType();
            //             if (IsXmlAlias)
            //             {
            //                 XmlSerializer.SetTypeAlias("DeepCore.GameHostEditor.ZoneVar", "DeepCore.GameData.Zone.ZoneEditor.ZoneVar");
            //                 XmlSerializer.SetTypeAlias("DeepCore.GameHost.ZoneEditor.ZoneEvent", "DeepCore.GameData.Zone.ZoneEditor.ZoneEvent");
            //                 XmlSerializer.SetTypeAlias("DeepCore.GameHost.UnitEvent", "DeepCore.GameData.Zone.ZoneEditor.UnitEvent");
            //                 XmlSerializer.SetTypeAliasPrefix("DeepCore.GameHost.EventTrigger.", "DeepCore.GameData.EventTrigger.");
            //                 XmlSerializer.SetTypeAliasPrefix("DeepCore.GameHost.ZoneEditor.EventTrigger.", "DeepCore.GameData.Zone.ZoneEditor.EventTrigger.");
            //                 XmlSerializer.SetTypeAliasPrefix("CommonServer.Plugin.Editor.", "DeepCore.GameData.Zone.ZoneEditor.EventTrigger.");
            //             }
        }
        /// <summary>
        /// 绑定日志系统
        /// </summary>
        /// <param name="log"></param>
        public virtual void BindLogger(Logger log)
        {

        }
        //-------------------------------------------------------------------------------------------------------------------------------
        #region Instance


        public EditorScene CreateZone(InstanceZoneListener listener, EditorTemplates dataroot, SceneData data)
        {
            var z = this.CreateEditorScene(listener, dataroot, data);
            OnZoneCreate?.Invoke(z);
            return z;
        }

        public delegate void OnZoneCreateHandler(EditorScene zone);
        public event OnZoneCreateHandler OnZoneCreate;

        /// <summary>
        /// 创建场景
        /// </summary>
        /// <param name="templates"></param>
        /// <param name="listener"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual EditorScene CreateEditorScene(InstanceZoneListener listener, EditorTemplates dataroot, SceneData data)
        {
            return new EditorScene(listener, this, dataroot, data, DateTime.Now.Millisecond);
        }

        public virtual InstanceFlag CreateFlag(InstanceZone zone, SceneObjectData data)
        {
            if (data is RegionData regionData)
            {
                return new ZoneRegion(zone, regionData);
            }
            if (data is PointData pointData)
            {
                return new ZoneWayPoint(zone, pointData);
            }
            if (data is DecorationData decorationData)
            {
                return new ZoneDecoration(zone, decorationData);
            }
            if (data is AreaData areaData)
            {
                return new ZoneArea(zone, areaData);
            }
            return null;
        }
        public virtual InstanceSpell CreateSpell(InstanceZone zone, TAddSpell add)
        {
            return InstanceSpell.Alloc(zone, add);
        }
        public virtual InstanceItem CreateItem(InstanceZone zone, TAddItem add)
        {
            return new InstanceItem(zone, add);
        }
        public virtual InstanceUnit CreateUnit(InstanceZone zone, TAddUnit add)
        {
            InstanceUnit ret = null;
            var utype = add.info.UType;
            if (add.overrideType.HasValue && add.overrideType != UnitType.TYPE_NA)
            {
                utype = add.overrideType.Value;
            }
            if (utype == UnitType.TYPE_PLAYER)
            {
                ret = new InstancePlayer(zone, add);
            }
            else if (utype == UnitType.TYPE_PET)
            {
                ret = new InstancePet(zone, add);
            }
            else if (utype == UnitType.TYPE_SUMMON)
            {
                ret = new InstanceSummon(zone, add);
            }
            else if (utype == UnitType.TYPE_BUILDING)
            {
                ret = new InstanceBuilding(zone, add);
            }
            else if (utype == UnitType.TYPE_MANUAL)
            {
                ret = new InstanceManual(zone, add);
            }
            else if (utype == UnitType.TYPE_BEHAVIOR_TREE)
            {
                ret = new InstanceBehaviorUnit(zone, add);
            }
            else if (utype == UnitType.TYPE_ATTACHMENT)
            {
                ret = new InstanceAttachment(zone, add);
            }
            else if (utype == UnitType.TYPE_NEUTRALITY)
            {
                ret = new InstanceNature(zone, add);
            }
            else
            {
                ret = new InstanceGuard(zone, add);
            }
            return ret;
        }

        public virtual InstanceUnit.EquipSkill CreateUnitSkillState(InstanceUnit owner, SkillTemplate data, LaunchSkill skill)
        {
            return InstanceUnit.EquipSkill.Alloc(owner, data, skill);
        }
        public virtual InstanceUnit.EquipBuff CreateUnitBuffState(InstanceUnit owner, TAddBuff addbuff)
        {
            return InstanceUnit.EquipBuff.Alloc(owner, addbuff);
        }
        public virtual InstanceUnit.EquipAura CreateUnitAuraState(InstanceUnit owner, AuraTemplate aura, int level, InstanceUnit.EquipSkill fromSkillID)
        {
            return InstanceUnit.EquipAura.Alloc(owner, aura, level, fromSkillID);
        }
        public virtual InstanceUnit.InventorySlot CreateUnitInventorySlot(int index, InstanceUnit owner)
        {
            return InstanceUnit.InventorySlot.Alloc(index, owner);
        }
        public virtual InstanceUnit.DockingContext CreateDockingContext(InstanceUnit unit, InstanceZoneObject parent, DockingOffset offset)
        {
            return InstanceUnit.DockingContext.Alloc(unit, parent, offset);
        }

        //---------------------------------------------------------------------------------------------
        public virtual InstanceZoneFormula CreateFormula(InstanceZone zone)
        {
            return new InstanceZoneFormula(zone);
        }
        public virtual InstanceUnitFormula CreateFormula(InstanceUnit unit)
        {
            return new InstanceUnitFormula(unit);
        }

        //---------------------------------------------------------------------------------------------
        /// <summary>
        /// 创建单位仇恨系统
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public virtual HateSystem CreateHateSystem(InstanceUnit owner) => HateSystem.Alloc(owner);
        public virtual ObjectAoiStatus CreateAOI(InstancePlayer player) => new ObjectAoiStatus(player);

        /// <summary>
        /// 创建弹药库
        /// </summary>
        /// <param name="templates"></param>
        /// <returns></returns>
        public virtual UnitCartridge CreateCartridge(in TAddUnit add, InstanceUnit owner)
        {
            return new UnitCartridge();
        }


        /// <summary>
        /// 创建任务事件接口
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public virtual IQuestAdapter CreateQuestAdapter(InstanceZone zone) => new IQuestAdapter(zone);



        #endregion
        //-------------------------------------------------------------------------------------------------------------------------------
        #region Abilities
        public virtual Ability CreateAbility(EditorAbilityData data, InstanceAttributes obj)
        {
            if (data is SpawnUnitAbilityData && obj is ISpawnContainer)
            {
                return CreateSpawnUnit(data as SpawnUnitAbilityData, obj as ISpawnContainer);
            }
            if (data is SpawnItemAbilityData && obj is ISpawnContainer)
            {
                return CreateSpawnItem(data as SpawnItemAbilityData, obj as ISpawnContainer);
            }
            if (data is UnitTransportAbilityData && obj is ZoneRegion)
            {
                return CreateUnitTransport(data as UnitTransportAbilityData, obj as ZoneRegion);
            }
            if (data is SceneTransportAbilityData && obj is ZoneRegion)
            {
                return CreateSceneTransport(data as SceneTransportAbilityData, obj as ZoneRegion);
            }
            return null;
        }

        protected virtual SpawnUnitAbility CreateSpawnUnit(SpawnUnitAbilityData data, ISpawnContainer region)
        {
            SpawnUnitAbility tg = new SpawnUnitAbility(region.Zone, data);
            //             tg.setDelayTime(data.StartTimeDelayMS);
            //             tg.setSpawnInterval(data.IntervalMS);
            //             tg.setSpawnCount(data.OnceCount);
            //             tg.setSpawnEffect(data.SpawnEffect);
            //             tg.setLimitedAliveCount(data.AliveLimit);
            //             tg.setLimitedSpawnCount(data.TotalLimit);
            //             tg.setSpawnWithoutAlive(data.WithoutAlive);
            //             tg.setUnitTag(data.UnitTag);
            //             tg.setUnitName(data.UnitName);
            //             tg.setUnitForce(data.Force);
            //             //tg.addUnits(data.UnitTemplatesID.ToArray(), data.UnitLevel);
            //             if (data.UnitTemplates != null)
            //             {
            //                 foreach (var spawn in data.UnitTemplates)
            //                 {
            //                     tg.addUnitInfo(spawn.UnitTemplateID, spawn.UnitLevel, spawn.Percent);
            //                 }
            //             }
            //             if (data.UnitGroup != null)
            //             {
            //                 using (var array = region.Zone.ObjectPool.AllocList<UnitInfo>())
            //                 {
            //                     region.Zone.Templates.GetAllUnitsByPath(data.UnitGroup.UnitGroupPath, array);
            //                     foreach (var unit in array)
            //                     {
            //                         tg.addUnitInfo(unit.ID, data.UnitGroup.UnitLevel);
            //                     }
            //                 }
            //             }
            //             tg.setTeamFormation(data.TFormation);
            //             tg.setStartPath(region.Zone, data.StartPointName, data.StartPathHoldMinTimeMS, data.StartPathHoldMaxTimeMS);
            //             tg.setStartDirection(data.StartDirection);
            //             tg.setResetOnWithoutAlive(data.ResetOnWithoutAlive);
            return tg;
        }
        protected virtual SpawnItemAbility CreateSpawnItem(SpawnItemAbilityData data, ISpawnContainer region)
        {
            SpawnItemAbility tg = new SpawnItemAbility(region.Zone, data);
            //             tg.setDelayTime(data.StartTimeDelayMS);
            //             tg.setSpawnInterval(data.IntervalMS);
            //             tg.setSpawnCount(data.OnceCount);
            //             tg.setSpawnEffect(data.SpawnEffect);
            //             tg.setLimitedAliveCount(data.AliveLimit);
            //             tg.setLimitedSpawnCount(data.TotalLimit);
            //             tg.setSpawnWithoutAlive(data.WithoutAlive);
            //             tg.setUnitTag(data.UnitTag);
            //             tg.setUnitName(data.UnitName);
            //             tg.setUnitForce(data.Force);
            //             tg.setStartDirection(data.StartDirection);
            //             if (data.ItemTemplates != null)
            //             {
            //                 foreach (var spawn in data.ItemTemplates)
            //                 {
            //                     tg.addItemInfo(spawn.ItemTemplateID, spawn.Percent);
            //                 }
            //             }
            //             tg.setResetOnWithoutAlive(data.ResetOnWithoutAlive);
            return tg;
        }
        protected virtual TransportUnitAbility CreateUnitTransport(UnitTransportAbilityData data, ZoneRegion region)
        {
            TransportUnitAbility tg = new TransportUnitAbility(region.Parent, data);
            return tg;
        }
        protected virtual TransportSceneAbility CreateSceneTransport(SceneTransportAbilityData data, ZoneRegion region)
        {
            TransportSceneAbility tg = new TransportSceneAbility(region.Parent, data);
            return tg;
        }

        #endregion
        //-------------------------------------------------------------------------------------------------------------------------------


        #region Preview
        public LocalBattle CreatePreview(EditorTemplates dataroot, ZoneSlaveFactory slave, SceneData sd = null)
        {
            if (sd == null)
            {
                if (dataroot.Templates.DefaultConfig.PREVIEW_SCENE != 0)
                {
                    sd = dataroot.LoadScene(dataroot.Templates.DefaultConfig.PREVIEW_SCENE);
                }
            }
            if (sd == null)
            {
                sd = new SceneData();
            }
            var z = this.CreatePreviewBattle(dataroot, slave, sd);
            return z;
        }
        protected virtual LocalBattle CreatePreviewBattle(EditorTemplates dataroot, ZoneSlaveFactory slave, SceneData sd)
        {
            return new PreviewBattle<ZonePreviewComponent>(dataroot, this, slave, sd);
        }

        #endregion


        //-------------------------------------------------------------------------------------------------------------------------------

        public virtual ZoneNode CreateServerZoneNode(IZoneNodeServer server, EditorTemplates data_root)
        {
            return new ZoneNode(server, this, data_root);
        }
        public virtual IPostChannel CreateChannel(object owner)
        {
            return null;//new ZoneNodeChannel(owner);
        }
        public virtual ZoneSpaceDivision CreateSpaceDivision(InstanceZone zone)
        {
            return new ZoneSpaceDivision(zone);
        }

        //---------------------------------------------------------------------------------------------
        public virtual ZoneEventTriggerCollection CreateZoneEventCollection(EditorScene mScene)
        {
            return mScene.ObjectPool.Alloc<ZoneEventTriggerCollection>().Init (mScene);
        }
        public virtual GUIEventTriggerCollection CreateGUIEventCollection(InstanceZone.HostGUIForm form)
        {
            return form.Zone.ObjectPool.Alloc<GUIEventTriggerCollection>().Init(form);
        }
        public virtual UnitEventTriggerCollection CreateUnitEventCollection(InstanceUnit unit, UnitEventTemplate ue)
        {
            return unit.ObjectPool.Alloc<UnitEventTriggerCollection>().Init(unit, ue);
        }
        public virtual CustomUnitEventTriggerCollection CreateCustomUnitEventCollection(InstanceUnit unit, CustomEventTemplateData ue)
        {
            return unit.ObjectPool.Alloc<CustomUnitEventTriggerCollection>().Init(unit, ue);
        }
        //---------------------------------------------------------------------------------------------


        public virtual object EncodeZoneVar(object value)
        {
            if (value is InstanceZoneObject obj)
            {
                return new ZoneVarObject() { ObjID = obj.ID };
            }
            if (value is InstanceUnit.EquipBuff buff)
            {
                return new ZoneVarObjectBuff() { ObjID = buff.Owner.ID, BuffID = buff.ID, };
            }
            if (value is InstanceUnit.EquipSkill skill)
            {
                return new ZoneVarObjectSkill() { ObjID = skill.Owner.ID, SkillID = skill.ID, };
            }
            if (value is InstanceUnit.EquipAura aura)
            {
                return new ZoneVarObjectAura() { ObjID = aura.Owner.ID, AuraID = aura.ID, };
            }
            if (value is TemplateData temp)
            {
                return new ZoneVarTemplate() { TemplateType = temp.GetType(), TemplateID = temp.ID, };
            }
            return value;
        }
    }
    //---------------------------------------------------------------------------------------------


    //---------------------------------------------------------------------------------------------

    //---------------------------------------------------------------------------------------------
}
