using DeepCore;
using DeepCrystal;
using DeepCrystal.ORM.Redis;
using DeepFrozen.Server.SSocket2.WebSocket;
using Gate.Server;
using Gate.Server.Launcher;
using PomeloServer.NetUV;
using _Temp_.Battle.Data;
using _Temp_.Client;
using _Temp_.Codec;
using _Temp_.Server;
using _Temp_.Service.Logic;
using System.Diagnostics;
using static Gate.Server.GateServerManager;

namespace _Temp_.ServerMain;

public class _Temp_GateMainLoop : MMOMainLoop
{
    public bool UseLocalDb = true;
    public string MysqlHost = "localhost";
    public string MysqlUser = "root";
    public string MysqlPassword = "123456";
    public string RedisHost = "127.0.0.1";
    public string RedisPassword = "";
    public bool GmActive = true;
    public bool SdkAuth = false;
    public string PlatformLoginProvider = "";
    public string ServerOpenAt = "2025-9-1T00:00:00Z";
    public string GamelogMysqlHost = "localhost";
    public int GamelogMysqlPort = 3306;
    public string GamelogMysqlUser = "root";
    public string GamelogMysqlPassword = "123456";
    public string GamelogMysqlDbName = "gamelog";
    public bool IsProd = false;

    public _Temp_GateMainLoop()
    {
        // 替换表结构，符合 IGG 规范
        RedisDump.CMD_CREATE_MAPPING = @"
            CREATE TABLE IF NOT EXISTS `mapping_object` (
                `id` BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY COMMENT '主键',
                `key` VARCHAR(255) NOT NULL COMMENT '键',
                `type` VARCHAR(255) NOT NULL COMMENT '值类型',
                `time` DATETIME NOT NULL COMMENT '归档时间',
                `value` LONGTEXT NOT NULL COMMENT '值',
                UNIQUE KEY `idx1` (`key`)
            ) DEFAULT CHARSET=utf8mb4 COMMENT='冷数据'";

        UseShellExecuteDB = false;
        StartServiceName /*        */ = nameof(_Temp_LauncherService);
        StartServiceType /*        */ = typeof(_Temp_LauncherService);
        GateClientManagerType /*   */ = typeof(_Temp_ClientManager);
        BattleCodec /*             */ = typeof(_Temp_BattleCodec);
        ClientCodec /*             */ = typeof(_Temp_ClientCodec);
        GateServerManagerType /*   */ = typeof(_Temp_GateServerManager);
        ServerCodec /*             */ = typeof(_Temp_ClientCodec);
        BattleDataFactory /*       */ = typeof(_Temp_ZoneDataFactory);
        GateListenPort = 19300;
        ServiceMapping.Put($"{ServerNameManager.GateServerType}", typeof(_Temp_GateService).FullName);
        ServiceMapping.Put($"{ServerNameManager.LogicServiceType}", typeof(_Temp_LogicService).FullName);
        ServiceMapping.Put($"{ServerNameManager.SessionServiceType}", typeof(_Temp_SessionService).FullName);
    }

    public override void MainLoopGateTest(Properties pargs)
    {
        pargs.LoadFields(this);
        // 这里切换TCP服务器还是WS服务器
        if (GateListenHost.StringStartWithIgnoreCase("ws://") || GateListenHost.StringStartWithIgnoreCase("wss://"))
        {
            new WSServerFactory();
            //WSServer.TRACE_PROTOCOL = true;
            GateServerManager.EnableProxyProtocolV2 = false;
            this.ServerConfig.ClientHostFactoryClass = typeof(WSServerFactory).FullName;
        }
        else
        {
            new UVPomeloServerFactory();
            GateServerManager.EnableProxyProtocolV2 = true;
            this.ServerConfig.ClientHostFactoryClass = typeof(UVPomeloServerFactory).FullName;
        }
        try
        {
            var myPool = "Pooling=true;ConnectionTimeout=60;MaxPoolSize=200;MinPoolSize=10;";
            if (UseLocalDb)
            {
                using (var redis = new RedisLauncher() { Port = RedisPort, UseShellExecute = UseShellExecuteDB }.Start_Redis_EXE(CurrentDir))
                using (var mysql = new MySQLLauncher() { Port = MysqlPort, UseShellExecute = UseShellExecuteDB }.Start_MySQL_EXE(CurrentDir))
                {
                    //this.RedisDumpMaintainceTime /* */= TimeSpan.FromDays(7);
                    this.RedisConnectionString = $"{RedisHost}:{RedisPort},password={RedisPassword},allowAdmin=true,syncTimeout=30000,responseTimeout=30000,connectTimeout=30000;db=2";
                    this.MySQLConnectionString/*            */ = $"Host={MysqlHost};Port={MysqlPort};User ID={MysqlUser};Password={MysqlPassword};{myPool}CharSet=utf8mb4;database=orm;";
                    this.ServerConfig.MySQLConnectorString/**/ = $"Host={MysqlHost};Port={MysqlPort};User ID={MysqlUser};Password={MysqlPassword};{myPool}CharSet=utf8mb4;database=gate;";
                    //new GameLog(false, "");
                    // StartServerGo(pargs);// 服务器直接管理 C# 跟 Go 两个进程
                    this.MainLoopWithProperties(pargs);// 注意：到这中断 main()
                }
                Console.WriteLine("done");
            }
            else
            {
                this.RedisConnectionString = $"{RedisHost}:{RedisPort},password={RedisPassword},allowAdmin=true,syncTimeout=30000,responseTimeout=30000,connectTimeout=30000;db=2";
                this.MySQLConnectionString/*            */ = $"Host={MysqlHost};Port={MysqlPort};User ID={MysqlUser};Password={MysqlPassword};{myPool}CharSet=utf8mb4;database=orm;";
                this.ServerConfig.MySQLConnectorString/**/ = $"Host={MysqlHost};Port={MysqlPort};User ID={MysqlUser};Password={MysqlPassword};{myPool}CharSet=utf8mb4;database=gate;";
                //new GameLogAsync($"Host={GamelogMysqlHost};Port={GamelogMysqlPort};User ID={GamelogMysqlUser};Password={GamelogMysqlPassword};{myPool}CharSet=utf8mb4;database={GamelogMysqlDbName};", IsProd);
                this.MainLoopWithProperties(pargs);// 注意：到这中断 main()
                Console.WriteLine("done");
            }
        }
        catch (Exception err)
        {
            err.PrintStackTrace();
        }
    }

}
