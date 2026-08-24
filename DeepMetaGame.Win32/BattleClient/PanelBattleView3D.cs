using DeepCore;
using DeepCore.Concurrent;
using DeepCore.Event.Debug;
using DeepCore.Game3D.Host;
using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Host.ZoneRuntime;
using DeepCore.Game3D.Slave;
using DeepCore.Game3D.Slave.Agent;
using DeepCore.Game3D.Slave.Layer;
using DeepCore.Game3D.Slave.Runtime;
using DeepCore.GameData.Zone;
using DeepCore.GameData.Zone.ZoneEditor;
using DeepCore.GUI;
using DeepCore.GUI.Cell;
using DeepCore.GUI.Input;
using DeepCore.IO;
using DeepCore.Net;
using DeepCrystal;
using DeepEditor.Common.Controls;
using DeepEditor.Common.EventDebug;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel;
using DeepEditor.Plugin3D.Display3D;
using DeepGameEditor3D.Common;
using DeepMetaGame.Data;
using DeepMetaGame.Data.Message;
using DeepMetaGame.Data.Message.UI;
using DeepMetaGame.Data.Misc;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using DeepMetaGame.Win32;
using G3D.ObjRenderer;
using PomeloServer.NetUV;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Windows.Forms;
using static DeepEditor.Common.Voxel.Display3D.FormVoxelCrossEditor;

namespace DeepEditor.Plugin3D.BattleClient
{
    public partial class PanelBattleView3D : UserControl
    {
        //private readonly static TypeAllocRecorder Alloc = new TypeAllocRecorder(typeof(PanelBattleView3D));
        private static System.Drawing.Color back_color = System.Drawing.Color.Black;
        public delegate void BattleViewBeginInit(PanelBattleView3D view);
        public delegate void BattleViewEndInit(PanelBattleView3D view);
        public delegate AbstractBattle CreateBattle(PanelBattleView3D view, EditorTemplates templates, SceneData sceneData);
        public static event BattleViewBeginInit OnBattleViewBeginInit;
        public static event BattleViewEndInit OnBattleViewEndInit;
        public static event CreateBattle OnCreateLocalBattle;
        public static event Func<EventDebugHost, EventDebugForm> OnCreateEventDebug;
        //---------------------------------------------------------------------------
        private EditorTemplates templates;
        private bool selfTemplates = true;
        private BattleView3D worldView;
        private DropDownFieldMaskGenerator canvasViewManager;
        //---------------------------------------------------------------------------
        public EventDebugHost EventDebug { get; private set; }
        public EventDebugForm EventDebugWindow { get; private set; }
        //---------------------------------------------------------------------------
        public System.Windows.Forms.Timer TimerMain { get; }
        public BattleView3D BattleView
        {
            get => worldView;
        }
        public EditorTemplates Templates
        {
            get => templates;
        }
#pragma warning disable WFO1000
        [Browsable(false)]
        public int RenderFPS
        {
            get => worldView != null ? worldView.FPS : 1;
        }

        //---------------------------------------------------------------------------
        public PanelBattleView3D()
        {
            //Alloc.RecordConstructor(this.GetType());
            InitializeComponent();
            this.Disposed += FormBattleView3D_Disposed;
            this.TimerMain = new System.Windows.Forms.Timer(this.components);
            this.TimerMain.Enabled = true;
            this.TimerMain.Interval = 33;
        }

        ~PanelBattleView3D()
        {
            //Alloc.RecordDestructor(this.GetType());
        }
        private void FormBattleView3D_Disposed(object sender, EventArgs e)
        {
            EventDebugWindow?.Dispose();
            try
            {
                EventDebug?.StopAsync().Wait();
            }
            catch { }
            EventDebug?.Dispose();
            EventDebug = null;
            //Alloc.RecordDispose(this.GetType());
            this.TimerMain.Dispose();
            this.event_LoadTemplates = null;
            this.event_CreateAbstractBattle = null;
            this.event_CreateAbstractBattle = null;
            {
                this.worldView?.Dispose();
                this.worldView = null;
                this.canvasViewManager?.Dispose();
                this.canvasViewManager = null;
                if (selfTemplates)
                {
                    this.templates?.Dispose();
                }
                this.templates = null;
            }
        }
        public enum LocalExecuteType
        {
            Local,
            Thread,
            Node,
            Preview,
        }
        public class BattleConfig
        {
            public ZoneHostFactory hostFactory;
            public ZoneSlaveFactory slaveFactory;
            public LocalExecuteType exeType;
            public DirectoryInfo dataDir = null;
            public int sceneID = 0;
        }
        public static bool USE_META_TEMPLATE = false;
        public string Title { get; private set; }
        public bool Init(BattleConfig cfg)
        {
            try
            {
                var templates = event_LoadTemplates?.Invoke(cfg.dataDir);
                if (templates == null)
                {
                    selfTemplates = true;

                    templates = ZoneDataFactory.Factory.CreateEditorTemplates(cfg.dataDir.FullName);
                    var progressDlg = new G2DProgressDialog("LoadTemplates", p =>
                    {
                        if (USE_META_TEMPLATE && CFiles.TryFindParentFile(cfg.dataDir, "data.bytes", out var metaBytesFile))
                        {
                            templates.LoadAllTemplatesMeta();
                        }
                        else
                        {
                            templates.LoadAllTemplates(true, p);
                        }
                        //templates.LoadAllTemplatesAsync(true, p).Wait();//WIN32编辑器调试可以用这个方法
                    });
                    progressDlg.ShowDialog(this);
                }
                else
                {
                    selfTemplates = false;
                }
                var world = event_CreateBattleView?.Invoke();
                if (world == null)
                {
                    world = ZoneWin32Factory.Instance.CreateBattleView(this.glControl, this.TimerMain);
                }
                var battle = event_CreateAbstractBattle?.Invoke(cfg);
                if (battle == null)
                {
                    var sceneData = templates.LoadScene(cfg.sceneID);
                    this.Title = $"{sceneData}";
                    if (OnCreateLocalBattle != null)
                    {
                        battle = OnCreateLocalBattle(this, templates, sceneData);// new BattleLocalPlaySingle(templates, sceneData);
                    }
                    else if (ZoneWin32Factory.Instance.CreateBattle(templates, cfg, sceneData) is InstanceBattle zbattle)
                    {
                        zbattle.OnCrateZone += (b, z) =>
                        {
                            Factory_OnCreateZone(z);
                        };
                        zbattle.OnZoneStart += (t, z) =>
                        {
                            zbattle.QueueTask(() =>
                            {
                                OnInstanceZoneInit(z);
                            });
                        };
                        battle = zbattle;
                    }
                    else if (cfg.exeType == LocalExecuteType.Thread)
                    {
                        var single = new ThreadBattleSinglePlay(templates, cfg.hostFactory, cfg.slaveFactory, sceneData);
                        single.OnCrateZone += (b, z) =>
                        {
                            Factory_OnCreateZone(z);
                        };
                        single.OnZoneStart += (t, z) =>
                        {
                            single.QueueTask(() =>
                            {
                                OnInstanceZoneInit(z);
                            });
                        };
                        battle = single;
                    }
                    else if (cfg.exeType == LocalExecuteType.Node)
                    {
                        var single = new ZoneNodeBattle(templates, cfg.hostFactory, cfg.slaveFactory, sceneData);
                        single.OnCrateZone += (b, z) =>
                        {
                            Factory_OnCreateZone(z);
                        };
                        single.OnZoneStart += (t, z) =>
                        {
                            single.QueueTask(() =>
                            {
                                OnInstanceZoneInit(z);
                            });
                        };
                        battle = single;
                    }
                    else
                    {
                        var single = new LocalBattleSinglePlay(templates, cfg.hostFactory, cfg.slaveFactory, sceneData);
                        single.OnCrateZone += (b, z) =>
                        {
                            Factory_OnCreateZone(z);
                        };
                        single.OnStart += (z) =>
                        {
                            single.QueueTask(() =>
                            {
                                OnInstanceZoneInit(single.Zone);
                            });
                        };

                        battle = single;
                    }
                }
                if (battle != null)
                {
                    if (battle is LocalBattle local)
                    {
                        world.FixedUpdate = true;
                    }
                    battle.Start();
                }
                this.Init(templates, world, battle);

                fixedUpdateToolStripMenuItem.Checked = world.FixedUpdate;
                return true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "\n" + err.StackTrace);
                return false;
            }
        }
        public void Init(EditorTemplates templates, BattleView3D view, AbstractBattle battle)
        {
            OnBattleViewBeginInit?.Invoke(this);
            try
            {
                this.templates = templates;
                this.worldView = view;
                this.worldView.OnBeginRender += Canvas_OnBeginRender;
                this.worldView.OnInit();
                this.worldView.InitBattle(battle);
                this.worldView.Layer.LayerInit += Layer_LayerInit;
                this.worldView.Layer.ActorAdded += Layer_ActorAdded;
                this.worldView.ShowObjects = true;
                this.worldView.ShowTerrain3D = true;
                this.worldView.ShowPathMesh3D = false;
                this.worldView.ShowPathFinder = false;
                this.worldView.ShowFlagMesh3D = false;
                this.worldView.OnError += World_OnError;
                this.worldView.Layer.MessageReceived += Layer_MessageReceived;
                this.worldView.BindMeshDropDownMenu(this.menu_Meshs);
                this.canvasViewManager = new DropDownFieldMaskGenerator(worldView, menu_View, "show");
                foreach (SyncMode mode in Enum.GetValues(typeof(SyncMode)))
                {
                    ToolStripButton item = new ToolStripButton(mode.ToString());
                    item.Tag = mode;
                    item.Click += item_SyncMode_Click;
                    item.CheckOnClick = true;
                    drop_SyncMode.DropDownItems.Add(item);
                    if (worldView.Layer.ActorSyncMode == mode)
                    {
                        item.Checked = true;
                    }
                }
                this.worldView.SetFPS(templates.Templates.DefaultConfig.SYSTEM_FPS);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "\n" + err.StackTrace);
            }
            OnBattleViewEndInit?.Invoke(this);
        }
        private void Factory_OnCreateZone(InstanceZone zone)
        {
            var port = NetUtils.GetAvaliablePort(new Random().Next(14000, 15000));
            var config = new Properties
            {
                { "Listen", port.ToString() }
            };
            this.EventDebug = new EventDebugHost(zone, new UVPomeloServer(new DeepCrystal.Server.ServerConfig()
            {
                Port = port,
                Config = config,
            }, new EventDebugProtocolFactory(ZoneDataFactory.Codec)));
        }
        private void OnInstanceZoneInit(InstanceZone z)
        {
            this.EventDebug.StartAsync().Wait(1000);
            this.Invoke(() =>
            {
                btn_EventDebug.Enabled = true;
            });
        }

        public void SetPause(bool pause)
        {
            if (worldView != null)
            {
                worldView.Pause = pause;
            }
            btn_Running.Checked = !pause;
            btn_RunPauseTool.Checked = !pause;
            if (EventDebugWindow != null)
            {
                EventDebugWindow.btn_Play.Checked = !pause;
                EventDebugWindow.btn_RunTool.Checked = !pause;
            }
        }
        public void SetStep()
        {
            SetPause(true);
            worldView.Step1 = true;
        }

        private void World_OnError(Common.G3D.GLView arg1, Exception arg2)
        {
            SetPause(true);
        }
        private void Layer_MessageReceived(LayerZone layer, IBattleMessage msg)
        {
            if (msg is ChatNotify msg2)
            {
                Console.WriteLine($"{msg2.To}:{msg2.Message}");
            }
        }

        private void Layer_LayerInit(DeepCore.Game3D.Slave.Layer.LayerZone layer)
        {
            this.chk_2D.Checked = false;
            this.worldView.ResetCameraPos();
            //             this.BattleView.PostPaintTask(g =>
            //             {
            //             });
        }


        private void Layer_ActorAdded(LayerZone layer, LayerPlayer actor)
        {
            layer.TaskQueue.Enqueue(actor, static (z, actor) => actor.SendReady());
            this.txt_ActorInfo.Text = actor.ToStatusText();
        }

        //------------------------------------------------------------------------------------
        #region QuestEmulate

        protected virtual void btn_QuestAccpetR2B_Click(object sender, EventArgs e)
        {
            if (worldView.Client is LocalBattle local)
            {
                if (worldView.Layer.Actor != null)
                {
                    string quest = G2DTextDialog.Show("任务ID", "模拟【游戏服->】接取任务");
                    if (quest != null)
                    {
                        local.Zone.QuestAdapter.OnQuestAcceptedHandler(worldView.Layer.Actor.PlayerUUID, quest);
                    }
                }
            }
        }
        protected virtual void btn_QuestStatusChangeR2B_Click(object sender, EventArgs e)
        {
            if (worldView.Client is LocalBattle local)
            {
                if (worldView.Layer.Actor != null)
                {
                    string quest = G2DTextDialog.Show("任务ID\r\nKey\r\nValue", "模拟【游戏服->】任务状态改变");
                    if (quest != null)
                    {
                        string[] kvs = quest.Split(new char[] { '\n' }, 3, StringSplitOptions.RemoveEmptyEntries);
                        if (kvs.Length >= 3)
                        {
                            local.Zone.QuestAdapter.OnQuestStatusChangedHandler(worldView.Layer.Actor.PlayerUUID, kvs[0], kvs[1], kvs[2]);
                        }
                    }
                }
            }
        }
        protected virtual void btn_QuestCommitR2B_Click(object sender, EventArgs e)
        {
            if (worldView.Client is LocalBattle local)
            {
                if (worldView.Layer.Actor != null)
                {
                    string quest = G2DTextDialog.Show("任务ID", "模拟【游戏服->】提交任务");
                    if (quest != null)
                    {
                        local.Zone.QuestAdapter.OnQuestCommittedHandler(worldView.Layer.Actor.PlayerUUID, quest);
                    }
                }
            }
        }
        protected virtual void btn_QuestDropR2B_Click(object sender, EventArgs e)
        {
            if (worldView.Client is LocalBattle local)
            {
                if (worldView.Layer.Actor != null)
                {
                    string quest = G2DTextDialog.Show("任务ID", "模拟【游戏服->】放弃任务");
                    if (quest != null)
                    {
                        local.Zone.QuestAdapter.OnQuestDroppedHandler(worldView.Layer.Actor.PlayerUUID, quest);
                    }
                }
            }
        }

        #endregion
        //------------------------------------------------------------------------------------
        #region Turbo

        private void btn_1X_Click(object sender, EventArgs e)
        {
            worldView.TurboX = 1;
            menu_Turbo.Text = "加速x" + worldView.TurboX;
        }
        private void btn_2X_Click(object sender, EventArgs e)
        {
            worldView.TurboX = 2;
            menu_Turbo.Text = "加速x" + worldView.TurboX;
        }
        private void btn_3X_Click(object sender, EventArgs e)
        {
            worldView.TurboX = 3;
            menu_Turbo.Text = "加速x" + worldView.TurboX;
        }
        private void btn_4X_Click(object sender, EventArgs e)
        {
            worldView.TurboX = 4;
            menu_Turbo.Text = "加速x" + worldView.TurboX;
        }
        private void btn_5X_Click(object sender, EventArgs e)
        {
            worldView.TurboX = 5;
            menu_Turbo.Text = "加速x" + worldView.TurboX;
        }
        private void btn_10X_Click(object sender, EventArgs e)
        {
            worldView.TurboX = 10;
            menu_Turbo.Text = "加速x" + worldView.TurboX;
        }
        private void btn_0_5X_Click(object sender, EventArgs e)
        {
            worldView.TurboX = 0.5f;
            menu_Turbo.Text = "加速x" + worldView.TurboX;
        }
        private void btn_0_1X_Click(object sender, EventArgs e)
        {
            worldView.TurboX = 0.1f;
            menu_Turbo.Text = "加速x" + worldView.TurboX;
        }

        private void btn_TurboX_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string turbo = G2DTextDialog.Show(worldView.TurboX.ToString(), "设置加速倍率");
            if (turbo != null && Parser.TryParseFloat(turbo.Trim(), out var turbox))
            {
                worldView.TurboX = turbox;
                menu_Turbo.Text = "加速x" + worldView.TurboX;
            }
        }
        public float TurboX { get => worldView.TurboX; }
        public void SetTurboX(float x)
        {
            worldView.TurboX = x;
            menu_Turbo.Text = "加速x" + worldView.TurboX;
        }

        private void fixedUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fixedUpdateToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            worldView.FixedUpdate = fixedUpdateToolStripMenuItem.Checked;
        }
        private void fixedTimeIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fps = G2DTextDialog.Show(worldView.FPS.ToString(), "设置FPS");
            if (fps != null && Parser.TryParseInt(fps.Trim(), out var _fps))
            {
                worldView.FixedFPS = (_fps);
            }
        }
        #endregion
        //------------------------------------------------------------------------------------
        #region Events

        //------------------------------------------------------------------------------------

        private Func<DirectoryInfo, EditorTemplates> event_LoadTemplates;
        private Func<BattleConfig, AbstractBattle> event_CreateAbstractBattle;
        private Func<BattleView3D> event_CreateBattleView;

        public event Func<DirectoryInfo, EditorTemplates> LoadTemplates
        {
            add { event_LoadTemplates += value; }
            remove { event_LoadTemplates -= value; }
        }
        public event Func<BattleView3D> CreateBattleView
        {
            add { event_CreateBattleView += value; }
            remove { event_CreateBattleView -= value; }
        }
        public event Func<BattleConfig, AbstractBattle> CreateAbstractBattle
        {
            add { event_CreateAbstractBattle += value; }
            remove { event_CreateAbstractBattle -= value; }
        }

        #endregion
        //------------------------------------------------------------------------------------
        #region Handler

        private void Canvas_OnBeginRender(Common.G3D.GLView sender, Common.G3D.PaintEventArgs3D e)
        {
            var form = (this.TopLevelControl as Form);
            if (form != null)
            {
                if ((!form.Visible) || (form.WindowState == FormWindowState.Minimized))
                {
                    SetPause(true);
                }
            }
        }

        public Raycast GetRaycastAction(MouseEventArgs e)
        {
            //var ret = new Raycast();
            var ret = this.BattleView.Layer.ObjectPool.Alloc<Raycast>();
            {
                var ray = this.BattleView.Camera.ScreenToWorldRay(e.Location);
                ret.screen = ray.screen.Value.ToGeometry();
                ret.normal = ray.normal.GLToVoxel().ToGeometry();
                ret.origin = ray.center.GLToVoxel().ToGeometry();

                //射到地图
                var raycastLayer = this.BattleView.RayCastVoxel(ray, out var raycastLayerTouch);
                if (raycastLayer != null)
                {
                    ret.IsHitTerrain = true;
                    ret.HitTerrainPosition = raycastLayerTouch.GLToVoxel().ToGeometry();
                }
                //射到物件
                var raycastObject = this.BattleView.RayCastObject<LayerObject3D>(ray, out var pos);
                if (raycastObject != null)
                {
//                     ret.HitObjectPlanePosition = DeepCore.Geometry.RayCast.RayPlaneIntersection(
//                         ret.origin,
//                         ret.normal,
//                         raycastObject.LayerObject.Position,
//                         DeepCore.Geometry.Vector3.UnitZ);

                    ret.HitObjectID = (raycastObject is LayerZoneUnit3D unit) ? unit.ZUnit.ObjectID : 0;
                    ret.HitFlagName = (raycastObject is LayerZoneFlag3D flag) ? flag.ZFlag.Name : null;
                    ret.HitObjectPosition = pos.GLToVoxel().ToGeometry();
                }
            }
            return ret;
        }

        private void TimerInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                if (worldView != null)
                {
                    chk_2D.Checked = worldView?.ActorCameraMode == BattleView3D.CameraMode.Mode2D;
                    if (worldView != null && worldView.Actor != null && btn_AutoAttack.Checked != worldView.Actor.IsGuard)
                    {
                        btn_AutoAttack.Checked = worldView.Actor.IsGuard;
                    }
                    SetPause(this.BattleView.Pause);
                }
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
            //             if (this.world.Layer?.Actor != null)
            //             {
            //                 var act = this.world.Layer.Actor;
            //                 txt_ActorInfo.Text = $"{act.Name} Pos=({act.X.ToString("#0.0")},{act.Y.ToString("#0.0")},{act.Z.ToString("#0.0")}) Dir={act.Direction.ToString("#0.00")}";
            //             }
        }
        public static float MOUSE_CLICK_DISTANCE = 10;
        private MouseEventArgs begin_mouse_down;
        private void GlControl1_MouseDown(object sender, MouseEventArgs e)
        {
            begin_mouse_down = e;
            if (worldView != null && worldView.Layer != null)
            {
                if (worldView.HUDRootNode.RayCastWithMouse() == null)
                {
                    this.worldView.Layer.SendAction(worldView.Layer.ObjectPool.AllocInit<MouseDownAction>((t) =>
                    {
                        t.SenderObjectID = this.worldView.Layer.Actor != null ? this.worldView.Layer.Actor.ObjectID : 0;
                        t.Button = (MouseButton)e.Button;
                        t.Clicks = e.Clicks;
                        t.Delta = e.Delta;
                        t.raycast = GetRaycastAction(e);
                    }));
                }

            }
        }
        private void GlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (worldView != null && worldView.Layer != null)
            {
                if (begin_mouse_down != null || worldView.HUDRootNode.RayCastWithMouse() == null)
                {
                    this.worldView.Layer.SendAction(worldView.Layer.ObjectPool.AllocInit<MouseMoveAction>((t) =>
                    {
                        t.SenderObjectID = this.worldView.Layer.Actor != null ? this.worldView.Layer.Actor.ObjectID : 0;
                        t.Button = (MouseButton)e.Button;
                        t.Clicks = e.Clicks;
                        t.Delta = e.Delta;
                        t.raycast = GetRaycastAction(e);
                    }));
                }
            }
        }
        private void GlControl1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (worldView != null && worldView.Layer != null)
                {
                    if (begin_mouse_down != null || worldView.HUDRootNode.RayCastWithMouse() == null)
                    {

                        this.worldView.Layer.SendAction(worldView.Layer.ObjectPool.AllocInit<MouseUpAction>((t) =>
                        {
                            t.SenderObjectID = this.worldView.Layer.Actor != null ? this.worldView.Layer.Actor.ObjectID : 0;
                            t.Button = (MouseButton)e.Button;
                            t.Clicks = e.Clicks;
                            t.Delta = e.Delta;
                            t.raycast = GetRaycastAction(e);
                        }));
                    }
                }
            }
            finally
            {
                begin_mouse_down = null;
            }
        }
        private void GlControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (worldView != null && worldView.Layer != null)
            {
                if (begin_mouse_down != null && CMath.GetDistance(begin_mouse_down.Location.X, begin_mouse_down.Location.Y, e.Location.X, e.Location.Y) <= MOUSE_CLICK_DISTANCE)
                {
                    //if (worldView.HUDRootNode.RayCastWithMouse() == null)
                    {
                        this.worldView.Layer.SendAction(worldView.Layer.ObjectPool.AllocInit<MouseClickAction>((t) =>
                        {
                            t.SenderObjectID = this.worldView.Layer.Actor != null ? this.worldView.Layer.Actor.ObjectID : 0;
                            t.Button = (MouseButton)e.Button;
                            t.Clicks = e.Clicks;
                            t.Delta = e.Delta;
                            t.raycast = GetRaycastAction(e);
                        }));
                    }
                }
            }
        }

        private void GlControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (worldView != null && worldView.Layer != null)
            {
                this.worldView.Layer.SendAction(worldView.Layer.ObjectPool.AllocInit<KeyUpAction>((t) =>
                {
                    t.SenderObjectID = this.worldView.Layer.Actor != null ? this.worldView.Layer.Actor.ObjectID : 0;
                    t.Key = (KeyCode)e.KeyCode;
                    t.Modifiers = (KeyCode)e.Modifiers;
                }));
            }
        }
        private void GlControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (worldView != null && worldView.Layer != null)
            {
                this.worldView.Layer.SendAction(worldView.Layer.ObjectPool.AllocInit<KeyDownAction>((t) =>
                {
                    t.SenderObjectID = this.worldView.Layer.Actor != null ? this.worldView.Layer.Actor.ObjectID : 0;
                    t.Key = (KeyCode)e.KeyCode;
                    t.Modifiers = (KeyCode)e.Modifiers;
                }));
            }
            if (e.KeyCode == Keys.Enter)
            {
                var chat = G2DTextDialog.Show("", "Input Chat");
                if (!string.IsNullOrEmpty(chat))
                {
                    this.worldView.Layer.SendAction(worldView.Layer.ObjectPool.AllocInit<TextMessage>((t) =>
                    {
                        t.Message = chat;
                    }));
                }
            }
        }

        private void btn_Running_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem run)
            {
                SetPause(!run.Checked);
            }
            else if (sender is ToolStripButton btn)
            {
                SetPause(!btn.Checked);
            }
        }

        protected virtual void btn_Step_Click(object sender, EventArgs e)
        {
            SetStep();
        }

        private void Chk_2D_Click(object sender, EventArgs e)
        {
            if (worldView.Client != null)
            {
                if (chk_2D.Checked)
                {
                    worldView.SetCameraMode(BattleView3D.CameraMode.Mode2D);
                    //world.SetCameraControl(camera2D);
                }
                else
                {
                    worldView.SetCameraMode(BattleView3D.CameraMode.Mode3D);
                    //world.SetCameraControl(camera3D);
                }
            }
        }

        protected virtual void btn_SkipClientEvent_Click(object sender, EventArgs e)
        {
            if (worldView != null)
            {
                // canvas.SkipClientEvent();
            }
        }

        protected virtual void btn_AutoAttack_Click(object sender, EventArgs e)
        {
            if (worldView.Actor != null)
            {
                worldView.Client.Actor.SendUnitGuard(btn_AutoAttack.Checked);
            }
        }
        protected virtual void btn_IsAutoFocusTarget_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (worldView.Actor != null)
            {
                worldView.Layer.Actor.IsSkillAutoFocusTarget = btn_IsAutoFocusTarget.Checked;
            }
        }

        protected virtual void btn_CleanBuff_Click(object sender, EventArgs e)
        {
            if (worldView.Actor != null)
            {
                using (var list = new ArrayList<LayerUnit.BuffState>())
                {
                    worldView.Layer.Actor.GetBuffStatus(list);
                    foreach (LayerUnit.BuffState bs in list)
                    {
                        worldView.Layer.Actor.SendCancelBuff(bs.Data.ID);
                    }
                }
            }
        }

        protected virtual void pickUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (worldView.Actor != null)
            {
                if (worldView.SelectedObject != null && worldView.SelectedObject is LayerZoneUnit3D su)
                {
                    worldView.Client.Actor.SendUnitPickObject(su.ZUnit.ObjectID);
                }
            }
        }
        private void PickItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (worldView.Actor != null)
            {
                if (worldView.SelectedObject != null && worldView.SelectedObject is LayerZoneItem3D su)
                {
                    worldView.Client.Actor.SendUnitPickObject(su.ZItem.ObjectID);
                }
            }
        }
        private void PickNearItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (worldView.Actor != null)
            {
                var near = worldView.Layer.GetNearPickableItem(worldView.Actor, worldView.Actor.BodyBlockSize);
                if (near != null)
                {
                    worldView.Client.Actor.SendUnitPickObject(near.ObjectID);
                }
            }
        }

        private void followUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (worldView.Actor != null)
            {
                showFollowDialog(false);
            }
        }
        private void followUnitGuardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (worldView.Actor != null)
            {
                showFollowDialog(true);
            }
        }

        public void showFollowDialog(bool attack)
        {
            var slot = G2DTextDialog.Show<int[]>("0, 2, 5, 15", Parser.TryStringToObject<int[]>, "SlotIndex");
            if (slot != null)
            {
                var targetID = worldView.SelectedObject != null ? worldView.SelectedObject.ZObject.ObjectID : 0;
                worldView.Client.Actor.SendUnitFolloweTarget(targetID, attack,
                    slot.Length > 1 ? slot[1] : 0,
                    slot.Length > 2 ? slot[2] : 0,
                    slot.Length > 3 ? slot[3] : 0,
                    slot[0]);
            }
        }

        protected virtual void item_SyncMode_Click(object sender, EventArgs e)
        {
            ToolStripButton item = sender as ToolStripButton;
            if (item != null)
            {
                worldView.Layer.ActorSyncMode = (SyncMode)item.Tag;
            }
            foreach (ToolStripButton btn in drop_SyncMode.DropDownItems)
            {
                btn.Checked = ((SyncMode)btn.Tag == worldView.Layer.ActorSyncMode);
            }
        }

        private void gCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.GC.Collect();
        }

        private void btn_IgnoreScriptEvent_Click(object sender, EventArgs e)
        {
            //    RuntimeGameLocal.IsShowScriptEvent = !btn_IgnoreScriptEvent.Checked;
        }

        private void btn_TestClientCustom_Click(object sender, EventArgs e)
        {
            if (worldView.Actor != null)
            {
                worldView.Actor.AddAgent(new TestClientCustomAgent());
            }
        }

        public class TestClientCustomAgent : AbstractAgent
        {
            private float keepTimeMS = 5000;
            private DeepCore.Geometry.Vector3 pos;
            private float dir;
            public override bool IsDuplicate => false;
            public override bool IsEnd { get => keepTimeMS <= 0 && !Owner.IsInAir; }
            protected override void OnInit(LayerPlayer actor)
            {
                base.OnInit(actor);
                dir = actor.Direction;
                pos = actor.Position;
                actor.SendCustomAction("test");
            }
            protected override void BeginUpdate(float intervalMS)
            {
                base.BeginUpdate(intervalMS);
                if (keepTimeMS > 0)
                {
                    keepTimeMS -= intervalMS;
                    pos.Z += 0.1f;
                    Owner.SendUpdatePos(pos, dir, dir, UnitActionStatus.ClientCustom);
                }
                else
                {
                    Owner.SendJump(dir, 1f);
                }
            }
            protected override void OnDispose()
            {
                base.OnDispose();
                this.Owner.SendUnitStopMove();
            }
        }

        private void btn_LoadMesh_Click(object sender, EventArgs e)
        {
            worldView.LoadMeshDialog();
        }

        private void btn_LoadMeshDX_Click(object sender, EventArgs e)
        {
            worldView.LoadMeshDialog(new ObjLoaderConfig()
            {
                ScaleX = -1,
                ScaleZ = -1,
                TranslationZ = -(this.worldView?.VoxelTerrain?.TotalSizeY) ?? 0,
            });
        }
        private void menu_Meshs_Click(object sender, EventArgs e)
        {

        }
        private void txt_Filter_TextChanged(object sender, EventArgs e)
        {
            worldView.ShowNameFilterText = txt_Filter.Text;
        }
        private void btn_EventDebug_Click(object sender, EventArgs e)
        {
            if (EventDebugWindow == null)
            {
                if (EventDebug != null)
                {
                    if (OnCreateEventDebug != null)
                    {
                        EventDebugWindow = OnCreateEventDebug.Invoke(EventDebug);
                    }
                    else
                    {
                        var address = $"127.0.0.1:{EventDebug?.Server?.ListenPort}";
                        EventDebugWindow = new EventDebugForm(ZoneDataFactory.Codec)
                        {
                            Text = $"{this.worldView.SceneData} : EventDebug({address})",
                        };
                        EventDebugWindow.FormClosing += (s, e) =>
                        {
                            e.Cancel = true;
                            EventDebugWindow.Hide();
                        };
                        SetPause(worldView.Pause);
                        EventDebugWindow.btn_Play.Click += btn_Running_Click;
                        EventDebugWindow.btn_RunTool.Click += btn_Running_Click;
                        EventDebugWindow.btn_Step.Click += btn_Step_Click;
                        EventDebugWindow.btn_StepTool.Click += btn_Step_Click;
                        EventDebugWindow.Start(address);
                    }
                }
            }
            EventDebugWindow?.Show();
        }
        private void btn_NetView_Click(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
