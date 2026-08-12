using DeepCore.GameData.Zone.ZoneEditor;
using DeepCore.GameData.ZoneServer;
using DeepEditor.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DeepEditor.Plugin.ServerTest
{
    public partial class FormLauncher : Form
    {
        public static EditorTemplates Templates { get; set; }

        private TestClientLoader mLoader;
        private bool mIsUnity;
        private Process mUnityProcess;
        private bool mIsAutoLaunch;

        public FormLauncher(string[] args, TestClientLoader loader, bool unity = false, bool autolaunch = false)
        {
            InitializeComponent();
            this.txt_GameDataRoot.Text = args[0];
            this.txt_PlayerUUID.Text = args[1];
            this.txt_RoomID.Text = args[2];
            this.txt_NetDirver.Text = args[3];
            this.txt_ConnectString.Text = args[4];
            this.num_IntervalMS.Value = int.Parse(args[5]);
            this.num_SyncRange.Value = int.Parse(args[6]);
            this.txt_UnitTemplateID.Text = args[7];
            this.txt_Force.Text = args[8];
            this.txt_SceneID.Text = args[9];
            this.mLoader = loader;
            this.mIsUnity = unity;
            this.mIsAutoLaunch = autolaunch;
        }
        public FormLauncher(
           string data_root,
           string player_uuid,
           string room_id,
           string net_driver,
           string connect_string,
           int interval_ms,
           int sync_range,
           int unit_template_id,
           int force,
           int scene_id,
           TestClientLoader loader,
           bool unity = false,
           bool autolaunch = false) :
           this(new string[] {
               data_root,
               player_uuid,
               room_id,
               net_driver,
               connect_string,
               interval_ms.ToString(),
               sync_range.ToString(),
               unit_template_id.ToString(),
               force.ToString(),
               scene_id.ToString(), }, loader, unity, autolaunch)
        {
            txt_NetDirver.Text = typeof(DeepCore.Net.Sockets.NetSession).FullName;
        }


        public string GameDataRoot
        {
            get { return this.txt_GameDataRoot.Text; }
        }
        public string PlayerUUID
        {
            get { return txt_PlayerUUID.Text; }
        }
        public string RoomID
        {
            get { return txt_RoomID.Text; }
        }
        public string NetDirver
        {
            get { return txt_NetDirver.Text; }
        }
        public string ConnectString
        {
            get { return txt_ConnectString.Text; }
        }
        public int IntervalMS
        {
            get { return (int)num_IntervalMS.Value; }
        }
        public int SyncRange
        {
            get { return (int)num_SyncRange.Value; }
        }
        public int UnitTemplateID
        {
            get
            {
                int result = 0;
                int.TryParse(txt_UnitTemplateID.Text, out result);
                return result;
            }
        }
        public byte Force
        {
            get
            {
                int result = 0;
                int.TryParse(txt_Force.Text, out result);
                return (byte)result;
            }
        }
        public int SceneID
        {
            get
            {
                int result = 0;
                int.TryParse(txt_SceneID.Text, out result);
                return result;
            }
        }
        public TestClientLoader Loader
        {
            get { return mLoader; }
        }
        public bool IsAutoLaunch
        {
            get { return mIsAutoLaunch; }
            set { mIsAutoLaunch = value; }
        }


        private void FormLauncher_Load(object sender, EventArgs e)
        {
            txt_UnitTemplateID.Focus();
            if (mIsAutoLaunch)
            {
                if (mIsUnity)
                {
                    launchUnity();
                }
                else
                {
                    launchWin32();
                }
            }
        }

        private void FormLauncher_Shown(object sender, EventArgs e)
        {
            txt_UnitTemplateID.Focus();
        }
        private void FormLauncher_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnLaunchOver = null;
        }
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            if (OnLaunchOver != null && OnLaunchOver.Invoke(this))
            {

            }
            else
            {
                if (mIsUnity)
                {
                    launchUnity();
                }
                else
                {
                    launchWin32();
                }
            }
        }
        //-------------------------------------------------------------------------------

        public delegate bool OnLaunchOverHandler(FormLauncher launcher);

        public event OnLaunchOverHandler OnLaunchOver;

        //-------------------------------------------------------------------------------
        #region Launch

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mUnityProcess != null)
            {
                if (mUnityProcess.HasExited)
                {
                    this.Show();
                    Regex regex = new Regex(@"\d{4}-\d{2}-\d{2}_\d*");
                    FileInfo exefile = new FileInfo(mUnityProcess.StartInfo.FileName);
                    foreach (DirectoryInfo dir in exefile.Directory.GetDirectories())
                    {
                        if (regex.IsMatch(dir.Name))
                        {
                            if (File.Exists(dir.FullName + Path.DirectorySeparatorChar + "crash.dmp"))
                            {
                                FileSystem.DeleteToRecycleBin(dir.FullName);
                            }
                        }
                    }
                    mUnityProcess = null;
                }
            }
        }

        private void launchUnity()
        {
            DirectoryInfo root_dir = new DirectoryInfo(txt_GameDataRoot.Text.Replace("file://", ""));
            string args = string.Format(
                "-CustomArgs:RunPlatform={0};" +
                "MapID={1};FolderPath={2};ResPath={3};UserID={4};Plugin={5};" +
                "IsNet=1;NetDriver={6};IP={7};UUID={8};Force={9};RoomID={10}",
                0,
                int.Parse(txt_SceneID.Text),
                root_dir.Parent.FullName + @"\",
                root_dir.Parent.FullName,
                int.Parse(txt_UnitTemplateID.Text),
                mLoader.ZoneFactoryType.FullName,

                txt_NetDirver.Text,
                txt_ConnectString.Text,
                txt_PlayerUUID.Text,
                int.Parse(txt_Force.Text),
                txt_RoomID.Text
                );
            FileInfo exefile = new FileInfo(Application.StartupPath + @"\U3DScene\U3DSceneRun.exe");
            ProcessStartInfo start = new ProcessStartInfo(exefile.FullName, args);
            start.WorkingDirectory = exefile.Directory.FullName;
            mUnityProcess = new Process();
            mUnityProcess.StartInfo = start;
            mUnityProcess.Exited += (object sender, EventArgs arg) =>
            {
                this.Visible = true;
            };
            mUnityProcess.Start();
            this.Visible = false;
        }


        private void launchWin32()
        {
            try
            {
                CreateUnitInfoR2B unit = mLoader.GenUnitInfoR2B(int.Parse(txt_UnitTemplateID.Text));
                {
                    unit.UnitTemplateID = int.Parse(txt_UnitTemplateID.Text);
                    unit.Force = byte.Parse(txt_Force.Text);
                    //unit.UnitPropData = prop_data;// (temp.Properties as HerosUnitProperties).ServerData;
                }
                FormClient form = new FormClient();
                form.Init(txt_GameDataRoot.Text,
                        txt_PlayerUUID.Text,
                         txt_RoomID.Text,
                        txt_NetDirver.Text,
                        txt_ConnectString.Text,
                        (int)num_IntervalMS.Value,
                        (int)num_SyncRange.Value,
                        unit,
                        chk_IsProxy.Checked,
                        txt_ProxyConnectString.Text);
                form.FormClosed += new FormClosedEventHandler((object sender2, FormClosedEventArgs e2) =>
                {
                    this.Visible = true;
                });
                form.Shown += new EventHandler((object sender2, EventArgs e2) =>
                {
                    this.Visible = false;
                });
                form.Show();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        #endregion
    }
}
