using DeepCrystal.RPC;
using Gate.Server.Launcher;
using _Temp_.Server;

namespace _Temp_.ServerMain
{
    /// <summary>
    /// 启动服务，用于拉起各个公共服务
    /// </summary>
    public class _Temp_LauncherService : MMOLauncherService
    {
        public _Temp_LauncherService(ServiceStartInfo start) : base(start) { }
//         protected override async Task OnStartAsync()
//         {
//             await base.OnStartAsync();
//             // //创建RaningService并注册到NameServer
//             // await Provider.CreateAsync(GetRankingService(SelfAddress.ServiceNode));
//             // //创建AdminService并注册到NameServer
//             // await Provider.CreateAsync(GetAdminService(SelfAddress.ServiceNode));
//             // //创建DungeonService并注册到NameServer
//             // await Provider.CreateAsync(GetDungeonService(SelfAddress.ServiceNode));
//             // //创建ChatService并注册到NameServer1
//             // await Provider.CreateAsync(GetChatService(SelfAddress.ServiceNode));
//             // //创建GuildService并注册到NameServer
//             // await Provider.CreateAsync(GetGuildService(SelfAddress.ServiceNode));
// 
// 
//             //             //创建SoicalService并注册到NameServer
//             // var svcProxy = await Provider.CreateAsync(GetSoicalService(SelfAddress.ServiceNode));
//             //             {
//             //                 //由其他服务发起，关闭一个服务
//             //                 await svcProxy.ShutdownAsync(reason: "测试卸载一个服务");
//             // 
//             //                 //也可以自己关闭自己
//             //                 this.ShutdownSelf(reason: "测试卸载自己");
//             //             }
// 
//         }

        public RemoteAddress GetRankingService(string svcNode = null)
        {
            return new RemoteAddress($"{ServiceNames.RANKING_SERVICE_TYPE}", svcNode, ServiceNames.RANKING_SERVICE_TYPE);
        }

        public RemoteAddress GetAdminService(string svcNode = null)
        {
            return new RemoteAddress($"{ServiceNames.ADMIN_SERVICE_TYPE}", svcNode, ServiceNames.ADMIN_SERVICE_TYPE);
        }

        public RemoteAddress GetChatService(string svcNode = null)
        {
            return new RemoteAddress($"{ServiceNames.CHAT_SERVICE_TYPE}", svcNode, ServiceNames.CHAT_SERVICE_TYPE);
        }

        public RemoteAddress GetSoicalService(string svcNode = null)
        {
            return new RemoteAddress($"{ServiceNames.SOCIAL_SERVICE_TYPE}", svcNode, ServiceNames.SOCIAL_SERVICE_TYPE);
        }

        public RemoteAddress GetDungeonService(string svcNode = null)
        {
            return new RemoteAddress($"{ServiceNames.DUNGEON_SERVICE_TYPE}", svcNode, ServiceNames.DUNGEON_SERVICE_TYPE);
        }

        public RemoteAddress GetGuildService(string svcNode = null)
        {
            return new RemoteAddress($"{ServiceNames.Guild_SERVICE_TYPE}", svcNode, ServiceNames.Guild_SERVICE_TYPE);
        }
    }
}
