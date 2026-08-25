using Gate.Client;
using System;

namespace _Temp_.Client
{
    public class _Temp_Client : MMOClient
    {
        public _Temp_Client()
        {
            this.ConnectTimeOut = TimeSpan.FromSeconds(15); // 设置连接超时时间为30秒
        }

        protected override void OnInitModules()
        {

        }

    }
}
