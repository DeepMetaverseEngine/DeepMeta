// See https://aka.ms/new-console-template for more information
using DeepCore;
using DeepCore.IO;
using DeepCore.Log;
using DeepCore.Net.Http;
using DeepFrozen.RPC.Launcher;
using DeepFrozen.RPC.Remote.InAppImpl;
using _Temp_.Codec;
using _Temp_.ServerMain;

static class Program
{
    [STAThread]
    static async Task Main(string[] args)
    {
        //ReflectionUtil.LoadDlls();
        new _Temp_Types();
        _Temp_Server.MainLoop(args);
    }
}

public static class _Temp_Server
{
    public static void MainLoop(string[] args)
    {

        var pargs = Properties.ParseArgs(args);
        if (!pargs.TryGetValue("root", out var editor_root))
        {
            if (!CFiles.TryFindParentDirectory(Environment.CurrentDirectory, Path.Combine("GameEditor"), out editor_root))
            {
                if (!CFiles.TryFindParentDirectory(Environment.CurrentDirectory, Path.Combine("GameEditor"), out editor_root))
                {
                }
            }
        }
        {
            var UseShellExecuteDB = true;
            //LoggerFactory.SetFactory(logfactory);
            if (System.OperatingSystem.IsWindows())
            {
                UseShellExecuteDB = false;
                // Windows 下使用 BILogger 的 Console 输出
                LoggerFactory.CurrentFactory.SetLevelFlag(LoggerLevel.ALL);
                RpcAppFactory.DEBUG_ON(true);
            }
            else
            {
                // Linux 下使用  的 Console 输出
                LoggerFactory.CurrentFactory.SetLevelFlag(LoggerLevel.RELEASE);
                RpcAppFactory.DEBUG_ON(false);
            }
            new HttpResourceLoader();
            TemplateDataCenter.ENABLE_LOAD_FROM_BIN = true;
            if (!pargs.TryGetValue("GateListenHost", out var host))
            {
                // host = "ws://127.0.0.1";
                host = "127.0.0.1";
            }
            if (!pargs.TryGetAsInt("GateListenPort", out var port))
            {
                port = 19300;
            }
            if (pargs.TryGetAsBool("UseShellExecuteDB", out var db))
            {
                UseShellExecuteDB = db;
            }
            if (pargs.TryGetAsBool("ws", out var ws) && ws)
            {
                host = "ws://" + host;
            }
            //WSServer.TRACE_PROTOCOL = true;
            Console.Title = $"_Temp_Server: {host}:{port}";
            var main = new _Temp_GateMainLoop()
            {
                GateListenHost = host,
                GateListenPort = port,
                UseShellExecuteDB = UseShellExecuteDB,
                BattleRoot = editor_root + "/",
                RedisDumpMaintainceTime = TimeSpan.FromDays(7),
                RpcAppFactoryType = typeof(InAppRpcAppFactory),
            };
            main.MainLoopGateTest(pargs);
        }
    }
}
