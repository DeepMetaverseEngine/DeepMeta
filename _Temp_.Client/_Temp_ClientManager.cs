using DeepCore;
using DeepCore.Concurrent;
using Gate.Client;
using System.Threading.Tasks;

namespace _Temp_.Client
{
    public class _Temp_ClientManager : MMOClientManager
    {
        public static ETemplateLangKey LangKey = ETemplateLangKey.en_US;
        static _Temp_ClientManager()
        {
        }
        public _Temp_ClientManager()
        {
        }

        protected override async Task<Properties> LoadLangPropertiesAsync(string langPath)
        {
            langPath = $"{Config.BattleEditorDir}/templates/lang/{LangKey}/lang.properties";
            Log.Info($"LoadLangPropertiesAsync1 {langPath}");
            return await Properties.LoadFromResourceAsync(langPath);
        }

        protected override async Task OnInitAsync(IRangeValue range)
        {
            await base.OnInitAsync(range);
            {
                Log.Info($"GameConfig Load");

                // var DataCenter = new ZumaDataCenter($"{Config.BattleEditorDir}/templates/json");
                // DataCenter.SetLangProperties(LangProperties);                
                //                 this.DataCenter.SetLangProperties(LangProperties);
                //                 GameConfig.Load($"{Config.BattleEditorDir}/templates/{nameof(GameConfig)}.properties");
            }
        }

        protected override async Task OnInitEndAsync(IRangeValue p)
        {
            await base.OnInitEndAsync(p);
            //             if (DataCenter != null)
            //             {
            //                 Log.Info($"DataCenter ReloadAllAsync");
            //                 await DataCenter.ReloadAllAsync(p);
            //             }
        }

        protected override void Disposing()
        {
            base.Disposing();
            //DataCenter?.Dispose();
        }
        public override GateClient CreateGateClient()
        {
            return new _Temp_Client();
        }
        //         public override IClientAdapter CreateNetClientAdapter(string address, GateNetClient client)
        //         {
        //             if (address.StringStartWithIgnoreCase("ws://") || address.StringStartWithIgnoreCase("wss://"))
        //             {
        //                 return WSNetClientFactory.WSInstance.CreateAdapter(client);
        //             }
        //             else
        //             {
        //                 return PomeloClientFactory.IOInstance.CreateAdapter(client);
        //             }
        //         }

    }
    public enum ETemplateLangKey
    {
        zh_CN, zh_TW, en_US, ja_JP, ko_KR, fr_FR, de_DE, es_ES, pt_PT, ru_RU,
    }
}
