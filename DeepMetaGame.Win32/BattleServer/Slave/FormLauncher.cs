using DeepCore;
using DeepCore.Game3D.Host;
using DeepCore.Game3D.Host.ZoneServer;
using DeepCore.Game3D.Slave;
using DeepCore.MinaClient.Sockets;
using DeepEditor.Common.G2D;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using DeepMetaGame.ZoneServer.Message;

namespace DeepEditor.Plugin3D.BattleServer.Slave
{
    public partial class FormLauncher : G2DBaseForm
    {
        private FormLauncher(
            EditorTemplates templates,
            ZoneHostFactory hostFactory,
            ZoneSlaveFactory slaveFactory,
            string data_root,
            string player_uuid,
            string room_id,
            string connect_string,
            int scene_id,
            CreateUnitInfoR2B enter,
            FormLauncher.OnLaunchOverHandler handler,
            FormLauncher.OnAutoLaunchHandler auto)
        {
            this.InitializeComponent();
            this.Templates = templates;
            this.HostFactory = hostFactory;
            this.SlaveFactory = slaveFactory;
            var cfg = templates.Templates.DefaultConfig;
            this.UnitInfo = enter;
            this.txt_GameDataRoot.Text = data_root;
            this.txt_PlayerUUID.Text = player_uuid;
            this.txt_RoomID.Text = room_id;
            this.txt_ConnectString.Text = connect_string;
            this.num_IntervalMS.Value = 1000 / cfg.SYSTEM_FPS;
            this.num_SyncRange.Value = cfg.CLIENT_SYNC_UNIT_MIN_RANGE;
            this.txt_UnitTemplateID.Text = enter.UnitTemplateID.ToString();
            this.txt_Force.Text = enter.Force.ToString();
            this.txt_SceneID.Text = scene_id.ToString();
            if (auto != null)
            {
                this.OnAutoLaunch += auto;
            }
            if (handler != null)
            {
                this.OnLaunchOver += handler;
            }
            this.IsAutoLaunch = (handler == null);
        }
        public static FormLauncher StartLauncher(
            string data_root,
            ZoneHostFactory hostFactory,
            ZoneSlaveFactory slaveFactory,
            string player_uuid,
            string connect_string,
            int scene_id,
            CreateUnitInfoR2B enter,
             FormLauncher.OnLaunchOverHandler handler = null, FormLauncher.OnAutoLaunchHandler auto = null)
        {
            FormLauncher ret = new FormLauncher(null, hostFactory,slaveFactory, data_root,
                player_uuid, scene_id.ToString(),
                connect_string, scene_id, enter, handler, auto);
            return ret;
        }
        public static FormLauncher StartLauncher(
            EditorTemplates templates,
            ZoneHostFactory hostFactory,
            ZoneSlaveFactory slaveFactory,
            string player_uuid,
            string connect_string,
            int scene_id,
            CreateUnitInfoR2B enter,
            FormLauncher.OnLaunchOverHandler handler = null, FormLauncher.OnAutoLaunchHandler auto = null)
        {
            FormLauncher ret = new FormLauncher(templates, hostFactory, slaveFactory, "",
                player_uuid, scene_id.ToString(),
                connect_string, scene_id, enter, handler, auto);
            return ret;
        }
        //-------------------------------------------------------------------------------------
        public EditorTemplates Templates { get; private set; }
        public ZoneHostFactory HostFactory { get; private set; }
        public ZoneSlaveFactory SlaveFactory { get; private set; }
        public bool IsAutoLaunch { get; private set; }
        public CreateUnitInfoR2B UnitInfo { get; private set; }
        public string DataDir { get => txt_GameDataRoot.Text; }
        public string PlayerUUID { get { return txt_PlayerUUID.Text; } }
        public string RoomID { get { return txt_RoomID.Text; } }
        public string ConnectString { get { return txt_ConnectString.Text; } }
        public int IntervalMS { get { return (int)num_IntervalMS.Value; } }
        public int SyncRange { get { return (int)num_SyncRange.Value; } }
        public bool IsProxy { get => this.chk_IsProxy.Checked; }
        public string ProxyConnectString { get => this.txt_ProxyConnectString.Text; }
        public int UnitTemplateID
        {
            get
            {
                int result = 0;
                Parser.TryParseInt(txt_UnitTemplateID.Text, out result);
                return result;
            }
        }
        public byte Force
        {
            get
            {
                int result = 0;
                Parser.TryParseInt(txt_Force.Text, out result);
                return (byte)result;
            }
        }
        public int SceneID
        {
            get
            {
                int result = 0;
                Parser.TryParseInt(txt_SceneID.Text, out result);
                return result;
            }
        }
        //-------------------------------------------------------------------------------------------

        private void FormLauncher_Load(object sender, EventArgs e)
        {
            if (Templates != null)
            {
                txt_GameDataRoot.Enabled = false;
            }
            txt_UnitTemplateID.Focus();
            if (IsAutoLaunch)
            {
                launchWin32();
            }
        }
        private void FormLauncher_Shown(object sender, EventArgs e)
        {
            txt_UnitTemplateID.Focus();
        }
        private void FormLauncher_FormClosed(object sender, FormClosedEventArgs e)
        {
            event_OnLaunchOver = null;
            event_OnAutoLaunch = null;
        }
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            if (Templates == null)
            {
                this.Templates = ZoneDataFactory.Factory.CreateEditorTemplates(txt_GameDataRoot.Text);
                this.Templates.LoadAllTemplates();
            }
            this.UnitInfo.UnitTemplateID = Parser.ParseInt(txt_UnitTemplateID.Text);
            this.UnitInfo.Force = Parser.ParseByte(txt_Force.Text);
            if (event_OnLaunchOver != null)
            {
                event_OnLaunchOver.Invoke(this);
            }
            else
            {
                launchWin32();
            }
        }
        private void Chk_IsProxy_CheckedChanged(object sender, EventArgs e)
        {
            txt_ProxyConnectString.Enabled = chk_IsProxy.Checked;
        }
        //-------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private FormClient launchWin32()
        {
            try
            {
                FormClient form = new FormClient();
                form.Init(this);
                form.FormClosed += new FormClosedEventHandler((object sender2, FormClosedEventArgs e2) =>
                {
                    if (!this.IsDisposed) this.Visible = true;
                    else this.Dispose();
                });
                form.Disposed += new EventHandler((object sender2, EventArgs e2) =>
                {
                    this.Dispose();
                });
                form.Shown += new EventHandler((object sender2, EventArgs e2) =>
                {
                    if (!this.IsDisposed) this.Visible = false;
                });
                form.Show();
                event_OnAutoLaunch?.Invoke(this, form);
                return form;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return null;
            }
        }

        public delegate void OnLaunchOverHandler(FormLauncher launcher);
        public delegate void OnAutoLaunchHandler(FormLauncher launcher, FormClient client);
        private OnLaunchOverHandler event_OnLaunchOver;
        private OnAutoLaunchHandler event_OnAutoLaunch;
        public event OnLaunchOverHandler OnLaunchOver { add { event_OnLaunchOver += value; } remove { event_OnLaunchOver -= value; } }
        public event OnAutoLaunchHandler OnAutoLaunch { add { event_OnAutoLaunch += value; } remove { event_OnAutoLaunch -= value; } }

        //--------------------------------------------------------------------------------------------

    }
}
