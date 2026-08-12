using DeepCore;
using DeepCore.Concurrent;
using DeepCore.GameData;
using DeepCore.GameData.Zone;
using DeepCore.GameData.Zone.ZoneEditor;
using DeepCore.GameHost;
using DeepCore.GameSlave;
using DeepCore.IO;
using DeepCore.Log;
using DeepCore.Net.Sockets;
using DeepCore.Reflection;
using DeepEditor.Plugin.ServerTest.Bot;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepEditor.Plugin.ServerTest
{
    public class BotRunner : TestClientLoader
    {
        public static BotRunner Instance { get; private set; }
        public static ZoneDataFactory DataFactory { get; private set; }
        public static InstanceZoneFactory ZoneFactory { get; private set; }
        public static IExternalizableFactory MessageFactory { get; private set; }
        public static EditorTemplates Templates { get; private set; }
        public static string ConnectString { get; private set; }

        private Logger log = LoggerFactory.GetLogger("BotRunner");

        public bool IsRunning { get; private set; }

        public BotRunner(string dataFactory, string zoneFactory, string clientFactory, string dataRoot, string connectString)
        {
            log.Info("********************************************************");
            log.Info("# 初始化");
            log.Info("********************************************************");

            DataFactory = ReflectionUtil.CreateInterface<ZoneDataFactory>(dataFactory);
            ZoneFactory = ReflectionUtil.CreateInterface<InstanceZoneFactory>(dataFactory);

            log.Info(" 战斗编辑器插件 --> " + ZoneFactory);
            MessageFactory = TemplateManager.DataFactory.MessageCodec;
            log.Info("\r\n" + (MessageFactory as MessageFactoryGenerator).ListAll("  "));
            Templates = DataFactory.CreateEditorTemplates(dataRoot);
            EditorTemplates.DEFAULT_LOAD_FROM_BIN = true;
            Templates.LoadAllTemplates();
            ConnectString = connectString; ;
            Instance = this;
        }

        public void Start()
        {
            this.IsRunning = true;
        }
        public void Shutdown()
        {
            this.IsRunning = false;
        }

        //-------------------------------------------------------------------------------------------------
        #region Launcher

        public override DeepCore.GameData.ZoneServer.CreateUnitInfoR2B GenUnitInfoR2B(int unitID)
        {
            DeepCore.GameData.ZoneServer.CreateUnitInfoR2B ret = new DeepCore.GameData.ZoneServer.CreateUnitInfoR2B();
            ret.UnitTemplateID = unitID;
            return ret;
        }
        public override Type ZoneFactoryType
        {
            get { return TemplateManager.DataFactory.GetType(); }
        }

        public FormLauncher AddTestClient(
               string player_uuid,
               string room_id,
               int interval_ms,
               int sync_range,
               int unit_template_id,
               int force,
               int scene_id)
        {
            FormLauncher.Templates = Templates;
            FormLauncher launcher = new FormLauncher(
                 "",
                 player_uuid,
                 room_id,
                 typeof(NetSession).FullName,
                 ConnectString,
                 interval_ms,
                 sync_range,
                 unit_template_id,
                 force,
                 scene_id, this, false, false);
            return launcher;
        }

        #endregion
        //-------------------------------------------------------------------------------------------------
        #region Bots

        private static AtomicInteger test_player_indexer = new AtomicInteger(0);
        private HashMap<string, BotPlayer> Bots = new HashMap<string, BotPlayer>();
        private Random random = new Random();

        public List<BotPlayer> BotsList { get { lock (Bots) { return new List<BotPlayer>(Bots.Values); } } }
        public int BotsCount { get { lock (Bots) { return Bots.Count; } } }

        public string BotsStatus
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (BotPlayer bot in BotsList)
                {
                    sb.AppendLine(string.Format("Bot[{0}] IsRunning={1}", bot.Name, bot.IsRunning));
                }
                return sb.ToString();
            }
        }

        public BotPlayer AddBot(int force, List<int> templates)
        {
            int templateID = random.GetRandomInArray(templates );
            UnitInfo unit = Templates.Templates.GetUnit(templateID);
            if (unit == null)
            {
                log.Error("没有测试单位！");
                return null;
            }
            string name = unit.Name + "_" + test_player_indexer.IncrementAndGet();
            BotPlayer ret = null;
            lock (Bots)
            {
                if (Bots.ContainsKey(name))
                {
                    log.ErrorFormat("已包含单位[{0}]！", name);
                    return null;
                }
                ret = new BotPlayer(name, "", unit, force, Templates, ConnectString);
                Bots.Add(name, ret);
                log.InfoFormat("已添加单位: {0}", name);
            }
            ret.Start();
            return ret;
        }
        public void AddBots(int count, int force, List<int> templates = null)
        {
            for (int i = 0; i < count; i++)
            {
                AddBot(force, templates);
            }
        }
        public void CleanupBots()
        {
            foreach (BotPlayer bot in BotsList)
            {
                if (!bot.IsRunning)
                {
                    lock (Bots)
                    {
                        Bots.Remove(bot.Name);
                    }
                    bot.Dispose();
                }
            }
        }
        public void StopAllBots()
        {
            foreach (BotPlayer bot in BotsList)
            {
                bot.SendLeaveRoom();
            }
            CleanupBots();
        }
        public void StopBot(BotPlayer bot)
        {
            bot.SendLeaveRoom();
            CleanupBots();
        }

        #endregion
        //-------------------------------------------------------------------------------------------------

    }

}
