using DeepCore;
using DeepCore.Game3D.Host;
using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Host.ZoneEditor;
using DeepCore.Game3D.Host.ZoneRuntime;
using DeepCore.Game3D.Slave;
using DeepCore.Game3D.Slave.Runtime;
using DeepCore.Log;
using DeepCrystal;
using DeepCrystal.NetServer;
using DeepCrystal.Server;
using DeepEditor.Common.G2D;
using DeepEditor.Plugin3D.BattleClient;
using DeepEditor.Plugin3D.BattleServer.Slave;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Message;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using DeepMetaGame.ZoneServer.Message;
using DeepMetaGame.ZoneServer.Server;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeepEditor.Plugin3D.BattleServer.Host
{
    public partial class FormServer : G2DBaseForm
    {
        //private ConsoleOutput console;
        private ServerNode server;
        private EditorScene zone;
        private ClientLoader client_loader;
        private static DirectoryInfo dataDir;
        public bool IsServerVisible => chk_ServerVisible.Checked;
        private ZoneHostFactory hostFactory;
        private ZoneSlaveFactory slaveFactory;
        public ZoneHostFactory HostFactory => hostFactory;
        public ZoneSlaveFactory SlaveFactory => slaveFactory;
        public FormServer(DirectoryInfo dataDir, int sceneID, ZoneHostFactory hostFactory, ZoneSlaveFactory slaveFactory)
        {
            FormServer.dataDir = dataDir;
            this.hostFactory = hostFactory;
            this.slaveFactory = slaveFactory;
            var templates = ZoneDataFactory.Factory.CreateEditorTemplates(dataDir.FullName);
            //templates.UseMMP = config.use_mmp;
            templates.LoadAllTemplates();
            InitializeComponent();
            //this.console = new ConsoleOutput();
            hostFactory.BindLogger(LoggerFactory.GetLogger("FormServer"));

            this.Shown += FormServer_Shown;
            this.FormClosing += FormServer_FormClosing;
            this.FormClosed += FormServer_FormClosed;
            this.Disposed += FormServer_Disposed;

            var port = NetUtils.GetAvaliablePort(14000);
            this.server = new ServerNode(templates, sceneID, new ServerConfig() { Host = "127.0.0.1", Port = port }, hostFactory);
            this.client_loader = new ClientLoader(this, server);
            this.server.Node.EnableAOI = chk_EnableAOI.Checked;
            //this.server.Node.EnableSyncPos = config.record_server;
            this.server.Node.OnZoneStart += Node_OnZoneStart;
            this.server.Node.OnZoneStop += Node_OnZoneStop;

            this.battlePanel.timerInfo.Tick += TimerInfo_Tick;
        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            this.server.StartAsync();
        }
        private void FormServer_Disposed(object sender, EventArgs e)
        {
            server.Dispose();
            client_loader = null;
            server = null;
            zone = null;
        }
        private void FormServer_Shown(object sender, EventArgs e)
        {
            //             console.DockTo = this;
            //             console.Show();
        }
        private void FormServer_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
        private void FormServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            client_loader.DisposeClients();
            server.Dispose();
            e.Cancel = true;
        }
        private void Node_OnZoneStart(InstanceZone zone)
        {
            this.zone = zone as EditorScene;
            zone.UUID = System.Guid.NewGuid().ToString();
            this.Invoke(new System.Action(() =>
            {
                battlePanel.Init(server.DataRoot, new BattleView(this, zone), new BattleRuntime(server, zone, slaveFactory));
                battlePanel.BattleView.Layer.LayerInit += (layer) =>
                {
                    client_loader.AddRandomClient(this.zone);
                    battlePanel.BattleView.SetCameraMode(BattleView3D.CameraMode.Mode2D);
                    //                     battlePanel.BattleView.PostPaintTask(g =>
                    //                    {
                    //                    });
                };
            }));
        }
        private void Node_OnZoneStop(InstanceZone zone)
        {
            this.Invoke(new System.Action(() =>
            {
                //this.console.Close();
                this.Dispose();
            }));
        }

        private void TimerInfo_Tick(object sender, EventArgs e)
        {
            if (server != null)
            {
                if (server.Running)
                {
                    this.Text = "FormServer : " + server.Server.ClientConnectString;
                }
            }
        }


        //----------------------------------------------------------------------------------------------------------------------

        internal class BattleRuntime : AbstractBattle, InstanceZoneListener
        {
            private EditorScene mZone;
            public SceneData SceneData => mZone.SceneData;
            public bool IsLocalBattle => false;
            public BattleRuntime(ServerNode server, InstanceZone zone, ZoneSlaveFactory slaveFactory)
                : base(server.Node.DataRoot, slaveFactory)
            {
                this.Layer.ActorSyncMode = SyncMode.ForceByServer;
                this.QueueTask(static t =>
                {
                    var local = t as BattleRuntime;
                    local.OnStart?.Invoke(local);
                });
                this.mZone = zone as EditorScene;
                {
                    var enter = zone.ObjectPool.Alloc<ClientEnterScene>().Init(
                        mZone.UUID,
                        mZone.Data.ID,
                        zone.SpaceDivSizeW,
                        zone.Gravity,
                        zone.Terrain3D.StepIntercept,
                        zone.Templates.ResourceVersion,
                        mZone.GetLayerInitData());
                    enter.sender = zone;
                    this.Layer.QueueMessage(enter);
                }
            }
            public EditorScene Zone
            {
                get { return mZone; }
            }
            private int mRecvPack = 0;
            private int mSendPack = 0;
            private InstanceUnit mSender;
            public override long RecvPackages { get { return mRecvPack; } }
            public override long SendPackages { get { return mSendPack; } }
            public override bool IsNet { get { return false; } }
            public bool IsPause
            {
                get => Pause;
                set => Pause = value;
            }
            public override void Start()
            {
            }
            protected override void Disposing()
            {
                this.OnEnd?.Invoke(this);
                base.Disposing();
                this.mSender = null;
                this.OnStart = null;
                this.OnEnd = null;
                this.OnError = null;
            }
            public override void BeginUpdate(float intervalMS)
            {
                base.BeginUpdate(intervalMS);
            }
            public override void Update()
            {
                base.Update();
            }
            public override bool TryLoadSceneData(ClientEnterScene msg, out SceneData sdata)
            {
                sdata = mZone.Data;
                return true;
            }
            public override void SendAction(BattleAction action)
            {
            }
            void InstanceZoneListener.OnCreateZone(InstanceZone zone)
            {
            }
            public void OnEventHandler(IReadOnlyList<BattleNotify> events)
            {
                for (var i = 0; i < events.Count; i++)
                {
                    mRecvPack++;
                    Layer.QueueMessage(events[i]);
                }
            }

            public override event BattleStart OnStart;
            public override event BattleEnd OnEnd;
            public override event BattleError OnError;
        }
        internal class BattleView : BattleView3D
        {
            private FormServer server;
            private BattleCodec mCodec;
            private InstanceZone zone;
            public BattleView(FormServer server, InstanceZone zone) : base(server.battlePanel.glControl, server.battlePanel.TimerMain)
            {
                base.ShowGuardRange = false;
                base.ShowAttackRange = false;
                base.ShowDamageRange = false;
                base.ShowObjectsName = true;
                this.ShowFlagMesh3D = true;
                this.ShowPathMesh3D = false;
                this.ShowTerrain3D = false;
                base.ShowHP = true;
                this.mCodec = new BattleCodec(server.server.Node.DataRoot.Templates);
                this.server = server;
                this.zone = zone;
                zone.OnPostEvent += zone_OnPostEvent;
                zone.OnUpdate += zone_OnUpdate;
            }
            private void zone_OnPostEvent(InstanceZone zone, IEnumerable<BattleNotify> events)
            {
                var client_events = new List<BattleNotify>();
                {
                    ArraySegment<byte> bin;
                    object msg;
                    foreach (var e in events)
                    {
                        if (mCodec.DoEncode(e, out bin))
                        {
                            if (mCodec.DoDecode(bin, out msg))
                            {
                                if (msg is BattleNotify evt)
                                {
                                    client_events.Add(evt);
                                }
                            }
                        }
                    }
                }
                if (Client is BattleRuntime battle)
                {
                    battle.OnEventHandler(client_events);
                }
            }
            private void zone_OnUpdate(InstanceZone zone)
            {
                //if (server.IsServerVisible)
                {
                    var syncPos = zone.AllocSyncPosEvent();
                    if (syncPos != null)
                    {
                        if (mCodec.DoEncode(syncPos, out var bin))
                        {
                            if (mCodec.DoDecode(bin, out var msg))
                            {
                                syncPos = msg as SyncPosEvent;
                                this.Client.QueueTask(t =>
                                {
                                    if (syncPos.ReadUnitPosList != null)//uha
                                    {
                                        foreach (var pos in syncPos.ReadUnitPosList)
                                        {
                                            if (pos != null)
                                            {
                                                var co = Client.Layer.GetObject(pos.ID);
                                                if (co != null)
                                                {
                                                    co.SyncPos(pos);
                                                }
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
            }

            protected override void GlControl_Paint(object sender, PaintEventArgs e)
            {
                base.GlControl_Paint(sender, e);
            }

            protected override void render_SpaceDiv(Graphics g)
            {
                if (SpaceDivW > 0)
                {
                    {
                        if (zone != null)
                        {
                            var cs = SpaceDivW;
                            var sw = CMath.RoundMod(zone.Terrain3D.TotalSizeX, SpaceDivW);
                            var sh = CMath.RoundMod(zone.Terrain3D.TotalSizeY, SpaceDivW);
                            var tw = sw * cs;
                            var th = sh * cs;
                            for (var x = 0; x <= sw; x++)
                            {
                                float dx = x * cs;
                                for (var y = 0; y <= sh; y++)
                                {
                                    float dy = y * cs;
                                    var cell = zone.GetSpaceCellNode(dx, dy);
                                    if (cell != null)
                                    {
                                        var screenPos = this.Camera.WorldToScreen(new OpenTK.Mathematics.Vector3(dx, 0, dy));
                                        g.DrawString($"P={cell.NearPlayerCount}",
                                            this.GLControl.Font,
                                            Brushes.White,
                                            screenPos.X, screenPos.Y);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------


        //----------------------------------------------------------------------------------------------------------------------


        private class ClientLoader
        {
            private static int id_gen = 0;
            private readonly FormServer form;
            private readonly ServerNode server;
            private Random random = new Random();
            private List<FormClient> client_list = new List<FormClient>();

            public ClientLoader(FormServer form, ServerNode server)
            {
                this.form = form;
                this.server = server;
            }

            public CreateUnitInfoR2B GenUnitInfoR2B(int unitID)
            {
                var ret = new CreateUnitInfoR2B();
                ret.UnitTemplateID = unitID;
                return ret;
            }
            public Type ZoneFactoryType
            {
                get { return ZoneDataFactory.Factory.GetType(); }
            }

            public void DisposeClients()
            {
                foreach (var f in client_list)
                {
                    f.Dispose();
                }
                client_list.Clear();
            }

            public void AddRandomClient(EditorScene zone)
            {
                var z = zone;
                if (z.TryGetTestUnit(out var region, out var start, out var info, random))
                {
                    var enter = GenUnitInfoR2B(info.ID);
                    enter.Force = (byte)start.START_Force;
                    FormLauncher.StartLauncher(
                      zone.DataRoot,
                      form.hostFactory,
                      form.slaveFactory,
                      info.Name + "_p_" + id_gen++, // player_uuid
                      server.Server.ClientConnectString,
                      server.Node.SceneID,
                      enter, null, (launcher, client) =>
                      {
                          client_list.Add(client);
                          launcher.Dispose();
                      }).Show();
                }
            }
            public void AddTestClient(EditorScene zone)
            {
                var z = zone;
                if (z.TryGetTestUnit(out var region, out var start, out var info, random))
                {
                    var enter = GenUnitInfoR2B(info.ID);
                    enter.Force = (byte)start.START_Force;
                    FormLauncher.StartLauncher(
                       zone.DataRoot,
                      form.hostFactory,
                      form.slaveFactory,
                       info.Name + "_p_" + id_gen++, // player_uuid
                       server.Server.ClientConnectString,
                       server.Node.SceneID,
                       enter, (launcher) =>
                       {
                           var client = new FormClient();
                           client.Init(launcher);
                           client_list.Add(client);
                           client.Show();
                           launcher.Dispose();
                       }).Show();
                }
            }
            public void AddTestClientStandalone(EditorScene zone)
            {
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        private void btn_AddPlayer_Click(object sender, EventArgs e)
        {
            client_loader.AddTestClient(this.zone);
        }
        private void btn_addPlayerStandalone_Click(object sender, EventArgs e)
        {
            client_loader.AddTestClientStandalone(this.zone);
        }

        private void btn_EmulateDelay_Click(object sender, EventArgs e)
        {
            //             int min, max;
            //             server.Server.GetEmulateLaggingMS(out min, out max);
            //             string text = G2DTextDialog.Show(string.Format("{0} - {1}", min, max), "模拟网络延时");
            //             try
            //             {
            //                 var kv = text.Split(new char[] { '-' }, 2, StringSplitOptions.RemoveEmptyEntries);
            //                 if (kv.Length == 2)
            //                 {
            //                     min = int.Parse(kv[0]);
            //                     max = int.Parse(kv[1]);
            //                     server.Server.SetEmulateLaggingMS(min, max);
            //                 }
            //                 else
            //                 {
            //                     min = int.Parse(text);
            //                     server.Server.SetEmulateLaggingMS(min, min);
            //                 }
            //             }
            //             catch (Exception err)
            //             {
            //                 MessageBox.Show(err.Message);
            //             }
        }

        private void btn_Bots_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo start = new ProcessStartInfo(
                    Application.StartupPath + @"\CommonAIServer.Connector.exe",
                    string.Format("FactoryClass={0} DataRoot=\"{1}\" ConnectString={2}",
                    ZoneDataFactory.Factory.GetType().FullName,
                    dataDir.FullName,
                    server.Server.ClientConnectString
                    ));
                start.WorkingDirectory = Application.StartupPath;
                Process.Start(start);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btn_ShowMsgBytes_Click(object sender, EventArgs e)
        {
            var msg_bytes_rec = new FormMsgBytes(server.Codec);
            msg_bytes_rec.Show();
            msg_bytes_rec.BringToFront();
        }

        private void btn_GC_Click(object sender, EventArgs e)
        {
            System.GC.Collect();
        }

        private void btn_disconnectAll_Click(object sender, EventArgs e)
        {
            foreach (var session in server.Server.GetSessions())
            {
                session.Disconnect(true);
            }
        }

        private void chk_EnableAOI_Click(object sender, EventArgs e)
        {
            server.Node.EnableAOI = chk_EnableAOI.Checked;
        }

        private void chk_ServerVisible_Click(object sender, EventArgs e)
        {
            this.battlePanel.glControl.Visible = chk_ServerVisible.Checked;
        }
    }
}
