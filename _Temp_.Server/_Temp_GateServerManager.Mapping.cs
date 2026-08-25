using DeepCore.Threading;
using DeepCrystal.ORM.Generic;
using Gate.Data;
using Gate.Data.Protocol;
using Gate.Server;
using System;
using System.Threading.Tasks;

namespace _Temp_.Server
{
    public partial class _Temp_GateServerManager : MMOServerManager
    {
        //----------------------------------------------------------------------------------------------------------------
        protected override MappingManager CreateMapping()
        {
            return new _Temp_MappingManager();
        }

        public class _Temp_MappingManager : MappingManager
        {
        }
        //----------------------------------------------------------------------------------------------------------------

    }
}
