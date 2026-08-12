using DeepCore;
using DeepCore.Game3D.Host;
using DeepCore.Game3D.Host.ZoneServer;
using DeepCore.Game3D.ZoneServer;
using DeepCore.GameData;
using DeepCore.GameData.Zone;
using DeepCore.GameData.Zone.ZoneEditor;
using DeepCore.Log;
using DeepCore.Reflection;
using DeepCore.Threading;
using DeepCrystal.Command;
using DeepEditor.Common;
using DeepEditor.Plugin3D.BattleServer.Slave;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Plugin3D.BattleServer.Host
{
    /// <summary>
    /// 由编辑器调用
    /// </summary>
    public static class ServerNodeLauncher
    {
        private static DirectoryInfo dataDir;
        private static EditorTemplates templates;
        private static int sceneID;
        private static int port;

        public static Process StartProcess(DirectoryInfo dataDir, int sceneID, IntPtr mainWindow)
        {
            Random random = new Random();
            ServerNodeLauncher.dataDir = dataDir;
            ServerNodeLauncher.templates = TemplateManager.DataFactory.CreateEditorTemplates(dataDir.FullName);
            ServerNodeLauncher.templates.LoadAllTemplates();
            ServerNodeLauncher.sceneID = sceneID;
            ServerNodeLauncher.port = random.Next(10000, 60000);

            var zone = templates.LoadScene(sceneID, false, true, false);
            var cfg = ZoneHostFactory.Factory.GetServerConfig();
            cfg.GAME_UPDATE_INTERVAL_MS = 1000 / templates.Templates.DefaultConfig.SYSTEM_FPS;
            cfg.CLIENT_SYNC_OBJECT_IN_RANGE = templates.Templates.DefaultConfig.CLIENT_SYNC_UNIT_MIN_RANGE;
            cfg.CLIENT_SYNC_OBJECT_OUT_RANGE = templates.Templates.DefaultConfig.CLIENT_SYNC_UNIT_MAX_RANGE;

            Process p = new Process();
            p.StartInfo.FileName = typeof(ServerNodeMain).Assembly.Location;
            p.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            p.StartInfo.Arguments = $"{nameof(ServerNodeMain)} " +
                $"{ServerNodeMain.KEY_DATA_CLASS}={ZoneDataFactory.Factory.GetType().FullName} " +
                $"{ServerNodeMain.KEY_ZONE_CLASS}={ZoneHostFactory.Factory.GetType().FullName} " +
                $"{ServerNodeMain.KEY_NAME}={nameof(ServerNodeMain)} " +
                $"{ServerNodeMain.KEY_WORK_DIR}=\"{Environment.CurrentDirectory}\" " +
                $"{ServerNodeMain.KEY_DATA_DIR}=\"{dataDir.FullName}\" " +
                $"{ServerNodeMain.KEY_SCENE_ID}={sceneID} " +
                $"{ServerNodeMain.KEY_PORT}={port} ";
            p.StartInfo.Arguments +=
                $"{ServerNodeMain.SUB_KEY_NODE_CONFIG}{nameof(cfg.GAME_UPDATE_INTERVAL_MS)}={cfg.GAME_UPDATE_INTERVAL_MS} " +
                $"{ServerNodeMain.SUB_KEY_NODE_CONFIG}{nameof(cfg.CLIENT_SYNC_OBJECT_IN_RANGE)}={cfg.CLIENT_SYNC_OBJECT_IN_RANGE} " +
                $"{ServerNodeMain.SUB_KEY_NODE_CONFIG}{nameof(cfg.CLIENT_SYNC_OBJECT_OUT_RANGE)}={cfg.CLIENT_SYNC_OBJECT_OUT_RANGE} " +
                $"{ServerNodeMain.SUB_KEY_NODE_CONFIG}{nameof(cfg.CLIENT_IDLE_TIME_SEC)}={cfg.CLIENT_IDLE_TIME_SEC} ";
            File.WriteAllText(p.StartInfo.FileName + ".bat", p.StartInfo.FileName + " " + p.StartInfo.Arguments + Environment.NewLine + "pause");
            p.Start();
            Application.ApplicationExit += (app, evt) =>
            {
                try { p.CloseMainWindow(); } catch { }
            };
            p.WaitForExit(1000);
            FormUtils.InsertAfterZOrder(p.MainWindowHandle, mainWindow);

            return p;
        }

        public static void LaunchClient()
        {
            if (templates == null) throw new Exception("服务器未开启");
            var zone = templates.LoadScene(sceneID, false, true, false);
            if (zone.TryGetStartTestUnit(templates.Templates, out var region, out var start, out var info, new Random()))
            {
                var cfg = ZoneHostFactory.Factory.GetServerConfig();
                cfg.GAME_UPDATE_INTERVAL_MS = 1000 / templates.Templates.DefaultConfig.SYSTEM_FPS;
                cfg.CLIENT_SYNC_OBJECT_IN_RANGE = templates.Templates.DefaultConfig.CLIENT_SYNC_UNIT_MIN_RANGE;
                cfg.CLIENT_SYNC_OBJECT_OUT_RANGE = templates.Templates.DefaultConfig.CLIENT_SYNC_UNIT_MAX_RANGE;

                var hostport = "127.0.0.1:" + port;
                var enter = new CreateUnitInfoR2B();
                enter.UnitTemplateID = info.ID;
                enter.Force = (byte)start.START_Force;
                FormLauncher.StartLauncher(templates, Guid.NewGuid().ToString(), hostport, zone.ID, cfg, enter).Show();
            }
        }
    }

    /// <summary>
    /// 独立进程
    /// </summary>
    public static class ServerNodeMain
    {
        public const string KEY_DATA_CLASS = "data_class";
        public const string KEY_ZONE_CLASS = "zone_class";
        public const string KEY_NAME = "name";
        public const string KEY_WORK_DIR = "work";
        public const string KEY_DATA_DIR = "dir";
        public const string KEY_SCENE_ID = "scene";
        public const string KEY_PORT = "port";
        public const string SUB_KEY_NODE_CONFIG = "cfg.";

        public static ServerNode LaunchServer(string[] args)
        {
            var prop = DeepCore.Properties.ParseArgs(args, "=");
            var work = prop.Get(KEY_WORK_DIR);
            Environment.CurrentDirectory = work;
            try
            {
                string data_class = prop[KEY_DATA_CLASS] + "";
                string host_class = prop[KEY_ZONE_CLASS] + "";
                var data_factory = ReflectionUtil.CreateInterface<ZoneDataFactory>(data_class);
                if (data_factory == null) throw new Exception($"{nameof(ZoneDataFactory)} Not Exist : {data_class}");
                var host_factory = ReflectionUtil.CreateInterface<ZoneHostFactory>(host_class);
                if (host_factory == null) throw new Exception($"{nameof(ZoneHostFactory)} Not Exist : {host_class}");
            }
            catch (Exception err)
            {
                throw new Exception("编辑器插件初始化失败 : " + err.Message, err);
            }
            var name = prop.Get(KEY_NAME);
            var dir = prop.Get(KEY_DATA_DIR);
            var sceneID = prop.GetAsInt(KEY_SCENE_ID);
            var port = prop.Get(KEY_PORT);
            var pcfg = prop.SubProperties(SUB_KEY_NODE_CONFIG);
            var templates = TemplateManager.DataFactory.CreateEditorTemplates(dir);
            templates.LoadAllTemplates();
            ZoneHostFactory.Factory.BindLogger(log);
            var cfg = pcfg.CreateInstance<ZoneNodeConfig>();
            var server = new ServerNode(templates, cfg, sceneID, port);
            Console.Title = $"name={name} port={port}";
            return server;
        }
        //-------------------------------------------------------------------------------------------------------------------------
        public static string Usage
        {
            get
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"{KEY_DATA_CLASS}=ZoneDataFactory");
                sb.AppendLine($"{KEY_ZONE_CLASS}=ZoneHostFactory");
                sb.AppendLine($"{KEY_NAME}={nameof(ServerNodeMain)}");
                sb.AppendLine($"{KEY_WORK_DIR}=Work Dir");
                sb.AppendLine($"{KEY_DATA_DIR}=Data Dir");
                sb.AppendLine($"{KEY_SCENE_ID}=scene ID");
                sb.AppendLine($"{KEY_PORT}=listen Port");
                sb.AppendLine($"[{SUB_KEY_NODE_CONFIG}{nameof(ZoneNodeConfig.GAME_UPDATE_INTERVAL_MS)}=GAME_UPDATE_INTERVAL_MS]");
                sb.AppendLine($"[{SUB_KEY_NODE_CONFIG}{nameof(ZoneNodeConfig.CLIENT_SYNC_OBJECT_IN_RANGE)}=CLIENT_SYNC_OBJECT_IN_RANGE]");
                sb.AppendLine($"[{SUB_KEY_NODE_CONFIG}{nameof(ZoneNodeConfig.CLIENT_SYNC_OBJECT_OUT_RANGE)}=CLIENT_SYNC_OBJECT_OUT_RANGE]");
                sb.AppendLine($"[{SUB_KEY_NODE_CONFIG}{nameof(ZoneNodeConfig.CLIENT_IDLE_TIME_SEC)}=CLIENT_IDLE_TIME_SEC]");
                return sb.ToString();
            }
        }

        private static Logger log = LoggerFactory.GetLogger("ServerNodeMain");

        public static void Main(string[] args)
        {
            try
            {
                log.Info("----------------------------------------------------------------------");
                log.Info(Usage);
                log.Info("----------------------------------------------------------------------");
                var server = LaunchServer(args);
                var zone = server.StartAsync().WaitForResult();
                var console = new NodeConsoleCommand();
                console.MainLoop();
                server.StopAsync().Wait();
            }
            catch (Exception err)
            {
                OnError(err);
            }
        }

        private static void OnError(Exception err)
        {
            log.Error(err.Message, err);
            log.Error("Press Any Key To Exit !!!");
            Console.In.Read();
            Environment.ExitCode = -1;
        }

        //-------------------------------------------------------------------------------------------------------------------------
        class NodeConsoleCommand : ConsoleCommandList
        {
            [Desc("进程查看")]
            public class CMD_PI : AbstractCommand
            {
                public override string Key { get { return "pi"; } }
                public override void DoCommand(string arg, TextWriter output)
                {
                    TypeAllocRecorder.PrintProcessStatus(output, System.Diagnostics.Process.GetCurrentProcess(), " ", 32);
                }
            }
            [Desc("内存查看")]
            public class CMD_AC : AbstractCommand
            {
                public override string Key { get { return "ac"; } }
                public override void DoCommand(string arg, TextWriter output)
                {
                    output.PrintLineSeparator();
                    TypeAllocRecorder.PrintMemoryStatus(output);
                    output.PrintLineSeparator();
                }
            }
            [Desc("内存清理")]
            public class CMD_GC : AbstractCommand
            {
                public override string Key { get { return "gc"; } }
                public override string Help { get { return "gc <generation>"; } }
                public override void DoCommand(string arg, TextWriter output)
                {
                    if (int.TryParse(arg, out var gen)) { GC.Collect(gen); }
                    else { GC.Collect(); }
                    output.PrintLineSeparator();
                    TypeAllocRecorder.PrintMemoryStatus(output);
                    output.PrintLineSeparator();
                }
            }
            [Desc("缓存查看")]
            public class CMD_POOL : AbstractCommand
            {
                public override string Key { get { return "pool"; } }
                public override void DoCommand(string arg, TextWriter output)
                {
                    output.PrintLineSeparator();
                    ObjectPools.PrintStatus(output);
                    output.PrintLineSeparator();
                }
            }
            [Desc("缓存清理")]
            public class CMD_POOL_CLEAR : AbstractCommand
            {
                public override string Key { get { return "poolc"; } }
                public override void DoCommand(string arg, TextWriter output)
                {
                    ObjectPools.ClearPool();
                    output.PrintLineSeparator();
                    ObjectPools.PrintStatus(output);
                    output.PrintLineSeparator();
                }
            }
            [Desc("RPC协议统计")]
            public class CMD_ST : AbstractCommand
            {
                private Type etype = typeof(DeepFrozen.RPC.Invoker.RpcStatistics.SortField);
                public override string Key { get { return "st"; } }
                public override string Help
                {
                    get
                    {
                        return "st <sort(" + CUtils.ListToString(Enum.GetNames(etype)) + ")>";
                    }
                }
                public override void DoCommand(string arg, TextWriter output)
                {
                    var sort = DeepFrozen.RPC.Invoker.RpcStatistics.SortField.NAME;
                    if (CUtils.TryParseEnum(etype, arg, true, out var sortobj))
                    {
                        sort = (DeepFrozen.RPC.Invoker.RpcStatistics.SortField)sortobj;
                    }
                    DeepFrozen.RPC.Invoker.RpcStatistics.PrintStatus(output, sort, " ", 64, 150);
                }
            }
            [Desc("时间耗时统计")]
            public class CMD_STT : AbstractCommand
            {
                private Type etype = typeof(DeepCore.Statistics.TimeStatisticsRecoder.SortField);
                public override string Key { get { return "stt"; } }
                public override string Help
                {
                    get
                    {
                        return "stt <sort(" + CUtils.ListToString(Enum.GetNames(etype)) + ")>";
                    }
                }
                public override void DoCommand(string arg, TextWriter output)
                {
                    var sort = DeepCore.Statistics.TimeStatisticsRecoder.SortField.NAME;
                    if (CUtils.TryParseEnum(etype, arg, true, out var sortobj))
                    {
                        sort = (DeepCore.Statistics.TimeStatisticsRecoder.SortField)sortobj;
                    }
                    DeepCore.Statistics.TimeStatisticsRecoder.PrintAllStatus(output, sort, " ", 64, 150);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------
    }

}
