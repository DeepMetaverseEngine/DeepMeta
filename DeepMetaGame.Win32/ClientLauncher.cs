using DeepCore.Game3D.Host;
using DeepCore.Game3D.Slave;
using DeepEditor.Plugin3D.BattleServer.Slave;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using DeepMetaGame.ZoneServer.Message;

namespace DeepGameEditor3D.Common
{
    public class ClientLauncher
    {

        public static void LaunchClient(DirectoryInfo dataDir, int sceneID, int port, ZoneHostFactory host, ZoneSlaveFactory slave)
        {
           var templates = ZoneDataFactory.Factory.CreateEditorTemplates(dataDir.FullName);
           templates.LoadAllTemplates();

            if (templates == null) throw new Exception("服务器未开启");
            var zone = templates.LoadScene(sceneID, false, true, false);
            if (zone.TryGetStartTestUnit(templates.Templates, out var region, out var start, out var info, new Random()))
            {
                var hostport = "127.0.0.1:" + port;
                var enter = new CreateUnitInfoR2B();
                enter.UnitTemplateID = info.ID;
                enter.Force = (byte)start.START_Force;
                FormLauncher.StartLauncher(templates, host,slave, Guid.NewGuid().ToString(), hostport, zone.ID, enter).Show();
            }
        }

        //         public static void LaunchClient()
        //         {
        //             if (templates == null) throw new Exception("服务器未开启");
        //             var zone = templates.LoadScene(sceneID, false, true, false);
        //             if (zone.TryGetStartTestUnit(templates.Templates, out var region, out var start, out var info, new Random()))
        //             {
        //                 var cfg = HostFactory.GetServerConfig();
        //                 cfg.GAME_UPDATE_INTERVAL_MS = 1000 / templates.Templates.DefaultConfig.SYSTEM_FPS;
        //                 cfg.CLIENT_SYNC_OBJECT_IN_RANGE = templates.Templates.DefaultConfig.CLIENT_SYNC_UNIT_MIN_RANGE;
        //                 cfg.CLIENT_SYNC_OBJECT_OUT_RANGE = templates.Templates.DefaultConfig.CLIENT_SYNC_UNIT_MAX_RANGE;
        // 
        //                 var hostport = "127.0.0.1:" + port;
        //                 var enter = new CreateUnitInfoR2B();
        //                 enter.UnitTemplateID = info.ID;
        //                 enter.Force = (byte)start.START_Force;
        //                 //FormLauncher.StartLauncher(templates, Guid.NewGuid().ToString(), hostport, zone.ID, cfg, enter).Show();
        //             }
        //         }
    }
}
