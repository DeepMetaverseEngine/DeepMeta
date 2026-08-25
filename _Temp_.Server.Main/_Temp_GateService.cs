using DeepCore.Log;
using DeepCrystal.NetServer;
using DeepCrystal.RPC;
using Gate.Server.Service.Gate;

namespace _Temp_.ServerMain;

public class _Temp_GateService : GateServer
{
    protected Dictionary<string, int> clientCount = [];

    public _Temp_GateService(ServiceStartInfo start) : base(start) { }

    protected override ViewSession Acceptor_CreateViewSession(ISession session)
    {
        return new _Temp_GateViewSession(this, session, log);
    }

    private sealed class _Temp_GateViewSession : ViewSession
    {
        private const int MaxPlatformDiagnosticLength = 256;

        public _Temp_GateViewSession(GateServer server, ISession session, Logger log)
            : base(server, session, log) { }

    }

//     [RpcHandler(typeof(SyncConnectToGateNotify))]
//     public override void rpc_OnHandleConnector(SyncConnectToGateNotify msg)
//     {
//         clientCount[msg.connectServiceAddress] = msg.clientNumber;
//         bool wasReady = isReady;
//         base.rpc_OnHandleConnector(msg);
//         if (!wasReady && isReady)
//         {
//             ServerOpenFlag = true;
//             log.Info("Gate Service Is Open after the first Connector became ready.");
//         }
//     }
// 
//     [RpcHandler(typeof(GameServerOnlineCountRequest), typeof(GameServerOnlineCountResponse))]
//     public virtual Task<GameServerOnlineCountResponse> rpc_OnOnlineCount(GameServerOnlineCountRequest reqest)
//     {
//         var rsp = new GameServerOnlineCountResponse()
//         {
//             Count = clientCount.Sum(e => e.Value),
//         };
//         return Task.FromResult(rsp);
//     }
}
