using DeepCrystal.ORM;
using DeepCrystal.RPC;
using Gate.Data.Sample;
using Gate.Server.Protocol;
using Gate.Server.Service.Logic;
using System;
using System.Threading.Tasks;

namespace _Temp_.Service.Logic.Modules
{
    public class SampleModule : _Temp_LogicModule, ILogicModule
    {
        // RPC 对象
        private IRemoteService areaManager;

        // ORM 对象
        //private SampleORMMapping sampleMapping;

        public SampleModule(_Temp_LogicService service) : base(service)
        {
            //this.sampleMapping = new SampleORMMapping("sampleMapping", this.Service);
        }
        public override async Task OnStartAsync()
        {
            await base.OnStartAsync();

            //获得另外一个服务（AreaManager）的Proxy
            this.areaManager = await Service.Provider.GetAsync("AreaManager");

            //创建定时器
            this.Service.Provider.CreateTimer(timer_tick, this, TimeSpan.FromSeconds(5));

            //初始化默认数据
//             await this.sampleMapping.LoadOrCreateDataAsync(() =>
//             {
//                 return new SampleORM()
//                 {
//                     structMapping = new SampleStructMapping(),
//                     structWrapper = new SampleStructWrapper(),
//                     roleMap = new DeepCore.HashMap<string, Gate.Data.RoleIDSnap>(),
//                     subMap = new DeepCore.HashMap<string, SampleSubMapping>(),
//                     subMapping = new SampleSubMapping() { },
//                 };
//             });
        }


        public Task OnClientEnterGameAsync()
        {
            return Task.CompletedTask;
        }
         public async Task OnSessionDisconnectAsync(SessionDisconnectNotify notify)
         {
             ///这个字段改变即立刻写入
          //   await this.sampleMapping.save_saveImmediately_async("imm");
         }
 
         public async Task OnSessionReconnectAsync(SessionReconnectNotify notify)
         {
             //两个字段改变
           //  this.sampleMapping.subMapping.userSource2 = notify.ToString();
           //  this.sampleMapping.subMapping.userSource1 = DateTime.UtcNow.ToString();
             //立即回写到数据库
           //  await this.sampleMapping.FlushAsync();
         }

        /// <summary>
        /// 没隔一段时间触发一次的存储过程
        /// </summary>
        /// <param name="trans"></param>
        public void OnSaveData(IObjectTransaction trans)
        {
//             this.sampleMapping.structWrapper.deviceId += 0.1f;
//             //所有脏字段全部刷新入库
//             this.sampleMapping.BatchFlush(trans);
        }

        /// <summary>
        /// 定时器回调
        /// </summary>
        /// <param name="st"></param>
        private void timer_tick(object st)
        {
            var tick = DateTime.UtcNow;
            log.Info($"timer tick : {tick}");
            //主动推送给客户端
            this.Service.Session.Invoke(new SampleNotify() { time = tick });
        }

//         /// <summary>
//         /// 收到客户端请求
//         /// (由链接服务RPC到此)
//         /// </summary>
//         /// <param name="ping"></param>
//         /// <returns></returns>
//         [RpcHandler]
//         public async Task<SamplePong> rpc_PingAsync(SamplePing ping)
//         {
//             log.Info(ping);
// 
//             //获得另外一个服务（AreaManager）的Proxy
//             this.areaManager = await Service.Provider.GetAsync("AreaManager");
// 
//             //这里只内存赋值，同时也标记了字段脏
//             this.sampleMapping.subMapping.rawData = ping.rawdata;
// 
//             //向另外一个服务（AreaManager）通信（RPC调用）
//             var rsp = await this.areaManager.CallAsync<GetRolePositionResponse>(
//                 new GetRolePositionRequest() { roleUUID = Service.RoleID });
//             if (rsp.IsSuccess)
//             {
//                 return new SamplePong()
//                 {
//                     s2c_code = rsp.s2c_code,
//                     s2c_msg = rsp.zoneUUID,
//                 };
//             }
//             return new SamplePong()
//             {
//                 s2c_code = Response.CODE_ERROR,
//             };
//         }
    }
}
