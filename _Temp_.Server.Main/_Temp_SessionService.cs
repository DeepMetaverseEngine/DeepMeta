using DeepCrystal.RPC;
using Gate.Data.Protocol;
using Gate.Server;
using Gate.Server.Service.Session;

namespace _Temp_.ServerMain;

public class _Temp_SessionService : SessionService
{
    public _Temp_SessionService(ServiceStartInfo start) : base(start)
    {
    }

    public override async Task<ClientCreateRoleResponse> client_rpc_Handle(ClientCreateRoleRequest req)
    {
        var response = await base.client_rpc_Handle(req);
        if (response.IsSuccess && response.s2c_role != null)
        {
        }
        return response;
    }

}
