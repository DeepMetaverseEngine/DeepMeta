using DeepCore.GameData.Zone;
using DeepCore.GameData.Zone.ZoneEditor;
using DeepCore.GameData.ZoneServer;
using DeepCore.GameHost.Local.Client;
using DeepCore.GameSlave.Client;
using DeepCore.IO;
using DeepEditor.Common.Drawing;
using DeepEditor.Common.Net;
using DeepEditor.Plugin.Win32.BattleClient;
using DeepEditor.Plugin.Win32.Runtime;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DeepEditor.Plugin.ServerTest
{
    public class FormClient : FormAbstractClient
    {
        // -----------------------------------------------------------------------------------------------------------------
        
        private BattleClient mClient;
        private EventHandler mAppExit;
        private FormNetSession mSessionView;
        private int mClientSyncRange;
        private EditorTemplates mDataRoot;

        protected readonly Pen RangePen = new Pen(Color.FromArgb(128, 255, 255, 255));

        public void Init(
            string gameDataRoot,
            string playerUUID,
            string roomID,
            string netDirver,
            string connectString,
            int intervalMS,
            int syncRange,
            CreateUnitInfoR2B unit,
            bool isProxy = false,
            string proxyConnectString = null)
        {
            Console.WriteLine("   GameDataRoot = " + gameDataRoot);
            Console.WriteLine("     PlayerUUID = " + playerUUID);
            Console.WriteLine("         RoomID = " + roomID);
            Console.WriteLine("      NetDirver = " + netDirver);
            Console.WriteLine("  ConnectString = " + connectString);
            Console.WriteLine("     IntervalMS = " + intervalMS);
            Console.WriteLine("      SyncRange = " + syncRange);
            Console.WriteLine(" UnitTemplateID = " + unit.UnitTemplateID);
            Console.WriteLine("          Force = " + unit.Force);
            Console.WriteLine("        IsProxy = " + isProxy);
            
            if (FormLauncher.Templates != null)
            {
                this.mDataRoot = FormLauncher.Templates;
            }
            else
            {
                this.mDataRoot = TemplateManager.DataFactory.CreateEditorTemplates(gameDataRoot);
                this.mDataRoot.LoadAllTemplates();
            }

            if (unit.UnitPropData == null)
            {
                UnitInfo temp = mDataRoot.Templates.GetUnit(unit.UnitTemplateID);
                unit.UnitPropData = IOUtil.Clone(TemplateManager.DataFactory.PersistCodec, temp.Properties);
            }

            PlayerWillConnectResponseB2R room = new PlayerWillConnectResponseB2R();
            room.PlayerUUID = playerUUID;
            room.Room = new RoomInfo();
            room.Room.ClientConnectString = connectString;
            room.Room.NetDriverString = netDirver;
            room.Room.Dummy = 0;
            room.Room.RoomID = roomID;

            PlayerWillConnectRequestR2B enter = new PlayerWillConnectRequestR2B();
            {
                enter.PlayerUUID = playerUUID;
                enter.PlayerDisplayName = playerUUID;
                enter.Token = playerUUID;
                enter.TokenValidTimeSec = int.MaxValue;
                enter.RoomID = roomID;
                enter.Data = unit; //IOUtil.ObjectToBin(factory, unit);
            }

            MessageFactoryGenerator factory = TemplateManager.DataFactory.MessageCodec as MessageFactoryGenerator;
            if (isProxy)
            {
                this.mClient = new BattleClientProxy(
                    new TestProxySession(),
                    proxyConnectString,
                    mDataRoot, factory, room, enter, playerUUID);
            }
            else
            {
                this.mClient = new BattleClientDirect(
                    mDataRoot, factory, room, enter, playerUUID);
            }
            this.mClientSyncRange = syncRange;

            this.mSessionView = new FormNetSession(mClient.Session);
            this.mSessionView.ShowInTaskbar = false;
            this.mSessionView.FormClosing += (object sender, FormClosingEventArgs e)=>
            {
                if (this.Visible)
                {
                    e.Cancel = true;
                    mSessionView.Hide();
                }
            };
            this.FormClosed += FormClient_FormClosed;
            base.BattlePanel.RenderFPS = DataRoot.Templates.CFG.SYSTEM_FPS;
            base.BattlePanel.btn_Exit.Visible = true;
            base.BattlePanel.OnExitClicked += BattlePanel_OnExitClicked;
        }

        private void BattlePanel_OnExitClicked()
        {
            this.mClient.SendLeaveRoom();
            this.Close();
        }

        private void FormClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            mSessionView.Close();
            mClient.Stop();
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            mClient.Stop();
        }

        //--------------------------------------------------------------------------------------------
        public override EditorTemplates DataRoot
        {
            get { return mDataRoot; }
        }
        public override AbstractBattle GenBattle()
        {
            return mClient;
        }
        public override DisplayLayerWorld GenDisplay(PictureBox control)
        {
            var ret = new DisplayLayerWorld();
            ret.ShowAOI = true;
            return ret;
        }

        protected override void OnLoaded()
        {
            this.mClient.Start();
            this.mAppExit = new EventHandler(Application_ApplicationExit);
            Application.ApplicationExit += mAppExit;
        }
        protected override void OnClose()
        {
            Application.ApplicationExit -= mAppExit;
            this.mSessionView.Dispose();
            mClient.Stop();
        }
        protected override void OnUpdate(int intervalMS)
        {
            string conn = mClient.Session.IsConnected ? "已连接" : "未连接";
            this.Text = mClient.PlayerUUID + " - [" + conn + "]";
        }
        protected override void DisplayActor_OnRender(Graphics g, DisplayGameUnit unit)
        {
//             RangePen.Width = 1f / BattlePanel.DisplayWorld.getCameraScale();
//             g.DrawArc(RangePen,
//                 -mClientSyncRange,
//                 -mClientSyncRange,
//                 mClientSyncRange << 1,
//                 mClientSyncRange << 1,
//                 0, 360);
        }
        protected override void OnNetworkViewClicked()
        {
            mSessionView.Show();
        }
        //--------------------------------------------------------------------------------------------

        class TestProxySession : BattleClientProxy.ProxyNetSession
        {
            public override bool IsMessageTypeGS(object msg)
            {
                return false;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BattlePanel
            // 
            this.BattlePanel.Size = new System.Drawing.Size(961, 642);
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.ClientSize = new System.Drawing.Size(961, 642);
            this.Name = "FormClient";
            this.ResumeLayout(false);

        }
    }

}
