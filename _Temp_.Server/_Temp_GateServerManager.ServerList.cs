using Gate.Data;
using Gate.Server;

namespace _Temp_.Server
{
    public partial class _Temp_GateServerManager : MMOServerManager
    {
        //----------------------------------------------------------------------------------------------------------------

        protected override ServerListManager CreateServerList()
        {
            return new _Temp_ServerListManager();
        }

        public class _Temp_ServerListManager : ServerListManager
        {
       
        }

        //----------------------------------------------------------------------------------------------------------------

    }
}
