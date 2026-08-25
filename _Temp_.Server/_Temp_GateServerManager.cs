using DeepCore.Template.NewtonJson;
using Gate.Server;

namespace _Temp_.Server
{
    public partial class _Temp_GateServerManager : MMOServerManager
    {
        public new static _Temp_GateServerManager Instance { get; private set; }
        static _Temp_GateServerManager()
        {
            new NewtonJsonTemplateLoader(true);
        }
        public _Temp_GateServerManager(GateServerConfig cfg) : base(cfg)
        {
            Instance = this;
        }
        //----------------------------------------------------------------------------------------------------------------
        // private ZumaDataCenter DataCenter;
        protected override void OnInit()
        {
            base.OnInit();
        }
        //----------------------------------------------------------------------------------------------------------------
        protected override void OnInitEnd()
        {
            base.OnInitEnd();
        }
        protected override void Disposing()
        {
            base.Disposing();
        }
    }
}
