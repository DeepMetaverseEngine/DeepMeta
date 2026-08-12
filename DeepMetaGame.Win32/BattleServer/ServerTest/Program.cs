using DeepCore;
using DeepCore.Reflection;
using DeepEditor.Plugin.ServerTest.Bot;
using System;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Plugin.ServerTest
{
    class Program
    {
        public static string Usage
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"App=Bots [OR Launcher]");
                sb.AppendLine(@"DataFactoryClass=ZeusCommon.ZeusBattleFactory");
                sb.AppendLine(@"ZoneFactoryClass=ZeusCommon.ZeusBattleFactory");
                sb.AppendLine(@"ClientFactoryClass=ZeusCommon.ZeusBattleFactory");
                sb.AppendLine(@"DataRoot=C:\Editors\GameEditor\data");
                sb.AppendLine(@"ConnectString=127.0.0.1:19001");
                sb.AppendLine(@"UUID=string");
                sb.AppendLine(@"RoomID=string");
                sb.AppendLine(@"IntervalMS=33");
                sb.AppendLine(@"SyncRange=12");
                sb.AppendLine(@"UnitTemplateID=10001");
                sb.AppendLine(@"Force=1");
                sb.AppendLine(@"SceneID=109");
                return sb.ToString();
            }
        }
        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                ReflectionUtil.LoadDlls(new System.IO.DirectoryInfo(Application.StartupPath));
                var prop = DeepCore.Properties.ParseArgs(args);
                string app = prop["App"];
                string dataFactoryClass = prop["DataFactoryClass"];
                string zoneFactoryClass = prop["ZoneFactoryClass"];
                string clientFactoryClass = prop["ClientFactoryClass"];
                string dataRoot = prop["DataRoot"];
                string connectString = prop["ConnectString"];
                BotRunner server = new BotRunner(dataFactoryClass, zoneFactoryClass, clientFactoryClass, dataRoot, connectString);
                server.Start();
                Console.WriteLine("*************************************************");
                Console.WriteLine("* 启动完毕 ");
                Console.WriteLine("*************************************************");
                switch (app.ToLower())
                {
                    case "bots":
                        new BotConsoleCommand().Run();
                        break;
                    case "launcher":
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(server.AddTestClient(
                            prop["UUID"],
                            prop["RoomID"],
                            int.Parse(prop["IntervalMS"]),
                            int.Parse(prop["SyncRange"]),
                            int.Parse(prop["UnitTemplateID"]),
                            int.Parse(prop["Force"]),
                            int.Parse(prop["SceneID"])));
                        break;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message + "\n" + err.StackTrace);
                Console.WriteLine(CUtils.ArrayToString(args));
                Console.WriteLine(Usage);
                Console.In.ReadLine();
                return -1;
            }
            return 0;
        }


    }
}
