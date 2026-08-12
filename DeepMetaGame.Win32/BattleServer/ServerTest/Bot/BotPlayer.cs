using DeepCore;
using DeepCore.GameData.Zone.ZoneEditor;
using DeepCore.Log;
using DeepCore.Net.Sockets;
using DeepEditor.Plugin.ServerTest.Client;
using System;
using System.Collections.Generic;

namespace DeepEditor.Plugin.ServerTest.Bot
{

    public class BotPlayer : IDisposable
    {
        private static Random random = new Random();
        private readonly Logger log;
        private readonly string mName;
        private readonly NetTestClient mClient;
        private readonly int mFixedIntervalMS;

        private System.Threading.Timer mTimer;
        private long mLastUpdateTime;


        public string Name { get { return mName; } }

        public bool IsRunning { get { return mClient.Client.Session.IsConnected; } }

        public BotPlayer(string name, string roomID, DeepCore.GameData.Zone.UnitInfo unit, int force, EditorTemplates dataroot, string connectString)
        {
            this.log = LoggerFactory.GetLogger("Bot[" + name + "]");

            DeepCore.GameData.ZoneServer.CreateUnitInfoR2B ret = new DeepCore.GameData.ZoneServer.CreateUnitInfoR2B();
            ret.UnitTemplateID = unit.TemplateID;
            ret.Force = (byte)force;

            this.mFixedIntervalMS = (1000 / dataroot.Templates.CFG.SYSTEM_FPS);
            this.mName = name;
            this.mClient = new NetTestClient(name,
                roomID, typeof(NetSession).FullName,
                connectString,
                mFixedIntervalMS,
                20, ret, false, null, dataroot);
            this.mClient.Client.Layer.ActorAdded += Layer_ActorAdded;
            this.mClient.Client.Disconnectd += Client_Disconnectd;

        }

        public void Start()
        {
            this.mClient.Start();
            this.mTimer = new System.Threading.Timer(update, this, mFixedIntervalMS, mFixedIntervalMS);
        }
        public void Stop()
        {
            try
            {
                mClient.Stop();
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
            }
        }
        public void SendLeaveRoom()
        {
            try
            {
                mClient.Client.SendLeaveRoom();
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
            }
        }
        public void Dispose()
        {
            try
            {
                lock (this)
                {
                    mTimer.Dispose();
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
            }
            try
            {
                mClient.Stop();
                mClient.Dispose();
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
            }
        }




        private void Client_Disconnectd(DeepCore.GameHost.Local.Client.BattleClient bc)
        {
            this.Dispose();
        }

        private void Layer_ActorAdded(DeepCore.GameSlave.ZoneLayer layer, DeepCore.GameSlave.ZoneActor actor)
        {
            actor.SendUnitGuard(true);
            startMoveToTarget();
        }

        private void update(object state)
        {
            lock (this)
            {
                long curTime = CUtils.CurrentTimeMS;
                if (mLastUpdateTime == 0)
                {
                    mLastUpdateTime = curTime;
                }
                int intervalMS = (int)(curTime - mLastUpdateTime);
                this.mLastUpdateTime = curTime;
                try
                {
                    updateInTarget(intervalMS);
                    this.mClient.Update(intervalMS);
                }
                catch (Exception err)
                {
                    log.Error(err.Message, err);
                }
            }
        }

        private TimeExpire<RegionData> target_expire;

        private void startMoveToTarget()
        {
            var list = new List<RegionData>(mClient.Client.Layer.Data.Regions);
            CUtils.RandomList(random, list);
            foreach (var rd in list)
            {
                foreach (var ab in rd.GetAbilities())
                {
                    if (ab is SpawnUnitAbilityData)
                    {
                        var suab = ab as SpawnUnitAbilityData;
                        if (suab.Force != mClient.Client.Actor.Force)
                        {
                            target_expire = new TimeExpire<RegionData>(rd, random.Next(10000, 100000));
                            mClient.Client.Actor.SendUnitAttackMoveTo(rd.X, rd.Y,0, true);
                            return;
                        }
                    }
                }
            }
        }
        private void updateInTarget(int intervalMS)
        {
            if (target_expire != null && target_expire.Update(intervalMS))
            {
                var rg = target_expire.Tag;
                var actor = mClient.Client.Actor;
                startMoveToTarget();
            }
        }
    }
}
