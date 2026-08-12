using DeepCore.IO;
using DeepEditor.Common.Net;
using DeepEditor.Plugin3D.BattleClient;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using DeepMetaGame.ZoneServer.Client;
using DeepMetaGame.ZoneServer.Message;

namespace DeepEditor.Plugin3D.BattleServer.Slave
{
    public class FormClient : BattleClient.FormBattleView3D
    {
        private DeepMetaGame.ZoneServer.Client.BattleClient mClient;
        private EventHandler mAppExit;
        private FormNetSession mSessionView;
        private int mClientSyncRange;
        private EditorTemplates mDataRoot;
        public EditorTemplates DataRoot { get { return mDataRoot; } }
        //--------------------------------------------------------------------------------------------
        public void Init(FormLauncher launcher)
        {
            Console.WriteLine("   GameDataRoot = " + launcher.DataDir);
            Console.WriteLine("     PlayerUUID = " + launcher.PlayerUUID);
            Console.WriteLine("         RoomID = " + launcher.RoomID);
            Console.WriteLine("  ConnectString = " + launcher.ConnectString);
            Console.WriteLine("     IntervalMS = " + launcher.IntervalMS);
            Console.WriteLine("      SyncRange = " + launcher.SyncRange);
            Console.WriteLine(" UnitTemplateID = " + launcher.UnitTemplateID);
            Console.WriteLine("          Force = " + launcher.Force);
            Console.WriteLine("        IsProxy = " + launcher.IsProxy);

            var unit = launcher.UnitInfo;
            this.mDataRoot = launcher.Templates;
            if (unit.UnitPropData == null)
            {
                UnitInfo temp = mDataRoot.Templates.GetUnit(unit.UnitTemplateID);
                unit.UnitPropData = IOUtil.Clone(ZoneDataFactory.Factory.PersistCodec, temp.Properties);
            }

            PlayerWillConnectResponseB2R room = new PlayerWillConnectResponseB2R();
            room.PlayerUUID = launcher.PlayerUUID;
            room.Room = new RoomInfo();
            room.Room.ClientConnectString = launcher.ConnectString;
            room.Room.Dummy = 0;
            room.Room.RoomID = launcher.RoomID;

            PlayerWillConnectRequestR2B enter = new PlayerWillConnectRequestR2B();
            {
                enter.PlayerUUID = launcher.PlayerUUID;
                enter.PlayerDisplayName = launcher.PlayerUUID;
                enter.Token = launcher.PlayerUUID;
                enter.TokenValidTimeSec = int.MaxValue;
                enter.RoomID = launcher.RoomID;
                enter.Data = unit; //IOUtil.ObjectToBin(factory, unit);
            }

            MessageFactoryGenerator factory = ZoneDataFactory.Factory.MessageCodec as MessageFactoryGenerator;
            if (launcher.IsProxy)
            {
                this.mClient = new DeepMetaGame.ZoneServer.Client.BattleClientProxy(
                    new TestProxySession(),
                    launcher.ProxyConnectString,
                    mDataRoot, launcher.SlaveFactory, factory, room, enter, launcher.PlayerUUID);
            }
            else
            {
                this.mClient = new DeepMetaGame.ZoneServer.Client.BattleClientDirect(
                    mDataRoot, launcher.SlaveFactory, factory, room, enter, launcher.PlayerUUID);
            }
            this.mClientSyncRange = launcher.SyncRange;

            this.mSessionView = new FormNetSession(mClient.Session);
            this.mSessionView.ShowInTaskbar = false;
            this.mSessionView.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                if (this.Visible)
                {
                    e.Cancel = true;
                    mSessionView.Hide();
                }
            };
            this.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                mSessionView.Close();
                mClient.Stop();
            };
            base.BattlePanel.btn_Exit.Visible = true;
            base.BattlePanel.btn_Exit.Click += (object sender, EventArgs e) =>
            {
                this.mClient.SendLeaveRoom();
                this.Close();
            };
            base.BattlePanel.btn_NetView.Click += (object sender, EventArgs e) =>
            {
                mSessionView.Show();
            };
            base.BattlePanel.timerInfo.Tick += (object sender, EventArgs e) =>
            {
                string conn = mClient.Session.IsConnected ? "已连接" : "未连接";
                this.Text = mClient.PlayerUUID + " - [" + conn + "]";
            };

            base.BattlePanel.LoadTemplates += (DirectoryInfo dataRoot) =>
            {
                return mDataRoot;
            };
            base.BattlePanel.CreateAbstractBattle += (cfg) =>
            {
                return mClient;
            };

            base.Init(new PanelBattleView3D.BattleConfig()
            {

                hostFactory = launcher.HostFactory,
                slaveFactory = launcher.SlaveFactory,
            });

            base.BattlePanel.BattleView.SetFPS(DataRoot.Templates.DefaultConfig.SYSTEM_FPS);

            this.mClient.Start();
        }

        private void Clear()
        {
            if (mClient != null)
            {
                mClient.Stop();
                mClient.Dispose();
                mClient = null;
            }
        }
        //--------------------------------------------------------------------------------------------

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            this.Clear();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.mAppExit = new EventHandler(Application_ApplicationExit);
            Application.ApplicationExit += mAppExit;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Application.ApplicationExit -= mAppExit;
                this.Clear();
                this.mSessionView.Dispose();
            }
            base.Dispose(disposing);
        }

        //--------------------------------------------------------------------------------------------

        public class TestProxySession : BattleClientProxy.ProxyNetSession
        {
            public override bool IsMessageTypeGS(object msg)
            {
                return false;
            }
        }


    }

}
