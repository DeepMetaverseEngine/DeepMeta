using DeepCrystal.RPC;
using Gate.Server.Service.Logic;
using _Temp_.Battle.Data;
using System;

namespace _Temp_.Service.Logic
{
    public class _Temp_LogicService : MMOLogicService
    {
        public Random random { get; } = new Random();

        public _Temp_LogicService(ServiceStartInfo start) : base(start)
        {

        }

        //----------------------------------------------------------------------------------------------------
        #region Modules

        protected override void OnClearModules()
        {
            base.OnClearModules();
        }
        /// <summary>
        /// 注册所有模块
        /// </summary>
        protected override void OnInitModules()
        {
            base.OnInitModules();



        }


        #endregion
        //----------------------------------------------------------------------------------------------------
    }

    public abstract class _Temp_LogicModule : MMOLogicModule<_Temp_LogicService>
    {
        public static _Temp_DataCenter DataCenter => _Temp_DataCenter.Instance;
        public IRemoteService Session => Service.Session;
        public _Temp_LogicModule(_Temp_LogicService service) : base(service)
        {
        }


    }

}
