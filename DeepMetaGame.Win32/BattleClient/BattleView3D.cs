using DeepCore;
using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Host.ZoneRuntime;
using DeepCore.Game3D.Slave;
using DeepCore.Game3D.Slave.Layer;
using DeepCore.Game3D.Slave.Runtime;
using DeepCore.GUI.Win32;
using DeepCore.Protocol;
using DeepCore.Reflection;
using DeepCore.Space;
using DeepCore.Voxel.Data;
using DeepEditor.Common.Drawing;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel;
using DeepEditor.Plugin3D.Display3D;
using DeepGameEditor3D.Common;
using DeepMetaGame.Data.Message;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using DeepMetaGame.Win32;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System.Text;
using static OpenTK.Audio.OpenAL.ALC;
using static System.Net.Mime.MediaTypeNames;

namespace DeepEditor.Plugin3D.BattleClient
{
    public class BattleView3D : DisplayZoneWorld3D
    {
        //--------------------------------------------------------------------
        [Desc("显示警戒范围", "show", true)]
        public bool ShowGuardRange = false;

        [Desc("显示单位移动路径", "show", false)]
        public bool ShowUnitMoveAI = false;

        [Desc("显示攻击范围", "show", true)]
        public bool ShowAttackRange = true;

        [Desc("显示受击范围", "show", true)]
        public bool ShowDamageRange = false;

        [Desc("显示AOI范围", "show", true)]
        public bool ShowAOI = false;

        [Desc("显示光环范围", "show", true)]
        public bool ShowAura = true;

        [Desc("显示Flag名字", "show", true)]
        public bool ShowFlagName = false;

        [Desc("显示HP", "show", true)]
        public bool ShowHP = false;
        [Desc("显示MP", "show", true)]
        public bool ShowMP = true;

        [Desc("显示法术名字", "show", true)]
        public bool ShowSpellName = false;

        [Desc("显示Log", "show", true)]
        public bool ShowLog = true;

        [Desc("显示所有Log", "show", true)]
        public bool ShowLogAll = false;
        //--------------------------------------------------------------------
        public readonly Random random = new Random();
        private AbstractBattle client;
        private ZoneInfo terrainZone;

        public AbstractBattle Client { get { return client; } }
        public LayerZone Layer { get { return client?.Layer; } }
        public LayerPlayer Actor { get { return client?.Layer?.Actor; } }
        public SceneData SceneData { get { return client?.Layer?.Data; } }
        public bool Pause
        {
            get
            {
                if (client == null) return true;
                return client.Pause;
            }
            set
            {
                if (client != null)
                {
                    client.Pause = value;
                }
            }
        }
        public float TurboX { get; set; } = 1;
        public bool FixedUpdate { get; set; } = false;
        public bool Step1 { get; set; } = false;
        public override ZoneInfo TerrainZone { get => terrainZone; }

        public BattleView3D(GLControl control, System.Windows.Forms.Timer timer) : base(control, timer)
        {
            base.OnUpdate += BattleView3D_OnUpdate;
            base.OnEndRender += BattleView3D_OnEndRender;
            base.OnRenderHUD += BattleView3D_OnRenderHUD;
            base.OnPaintGDI += BattleView3D_OnPaintHUD;
            this.HUDRootNode = new Win32DisplayRoot(new GLViewCanvas(this))
            {
                EnableDrawHUD = false,
                EnableCamera = false,
                EnableMouseRightMoveCamera = false,
                EnableMouseWheelZoomCamera = false,
                EnableDragMoveNode = true,
                EnableDragResizeNode = false,
                RepaintOnMouseHold = false,
            };
        }

        protected override void Disposing()
        {
            this.DisposeEvents();
            this.ClearLogs();
            this.ClearPopupTextHUD();
            this.DisposeObjects();
            if (client != null)
            {
                this.client.Dispose();
                this.client = null;
            }
            this.camera2D?.Dispose();
            this.camera2D = null;
            this.camera3D?.Dispose();
            this.camera3D = null;
            this.HUDRootNode?.Dispose();
            this.HUDRootNode = null;
            this.HUDRuntime.Dispose();
            base.Disposing();
        }

        public virtual void InitBattle(AbstractBattle bt)
        {
            if (client == null)
            {
                this.client = bt;
                this.Layer.LayerInit += OnLayerInit;
                this.Layer.ObjectEnter += OnObjectEnter;
                this.Layer.ObjectLeave += OnObjectLeave;
                this.Layer.MessageReceived += OnMessageReceived;
                this.Layer.ObjectMessageReceived += OnObjectMessageReceived;
                //base.InitTerrain(bt.Layer.TerrainSrc);
                this.camera2D = new ActorCamera2D(this);
                this.camera3D = new ActorCamera3D(this);
                this.SetCameraControl(camera3D);
                {
                    this.HUDRuntime.Init(this, bt.DataRoot);

                    //                     else if (bt is ThreadBattle t)
                    //                     {
                    //                         t.QueueZoneTask(z =>
                    //                         {
                    //                             z.GUIRuntime = this.HUDRuntime;
                    //                         });
                    //                     }
                }
            }
        }

        protected void UpateBattle(float intervalMS)
        {
            if (FixedUpdate)
            {
                intervalMS = 1000 / FixedFPS;
            }
            if (client != null)
            {
                //if (!Pause || Step1)
                {
                    var endPause = Pause;
                    if (Step1)
                    {
                        Pause = false;
                    }
                    var totalMS = (float)(intervalMS * this.TurboX);
                    while (totalMS > 0)
                    {
                        var tick = Math.Min(totalMS, intervalMS);
                        UpdateClient(tick);
                        totalMS -= tick;
                    }
                    if (Step1)
                    {
                        Pause = endPause;
                    }
                    Step1 = false;
                }
            }
        }
        private ActorCamera2D camera2D;
        private ActorCamera3D camera3D;
        private CameraMode cameraMode = CameraMode.Mode3D;
        public enum CameraMode { Mode2D, Mode3D, }
        public CameraMode ActorCameraMode
        {
            get => this.cameraMode;
        }
        public void SetCameraMode(CameraMode mode)
        {
            if (client != null)
            {
                switch (mode)
                {
                    case CameraMode.Mode2D:
                        SetCameraControl(camera2D);
                        break;
                    case CameraMode.Mode3D:
                        SetCameraControl(camera3D);
                        break;
                }
            }
        }
        protected override void OnSetCameraControl(CameraControl c)
        {
            if (c is ActorCamera2D c2d)
            {
                this.cameraMode = CameraMode.Mode2D;
                this.camera2D = c2d;
            }
            if (c is ActorCamera3D c3d)
            {
                this.cameraMode = CameraMode.Mode3D;
                this.camera3D = c3d;
            }
            base.OnSetCameraControl(c);
        }
        public override void ResetCameraPos(CameraControl camera)
        {
            base.ResetCameraPos(camera);
            if (TerrainZone != null)
            {
                camera.SetTarget(ZoneToWorld(new Vector3(TerrainZone.TotalWidth / 2f, TerrainZone.TotalHeight / 2f, 0f)));
            }
        }
        public void FocusCamera(LayerObject obj)
        {
            this.camera2D.FocusCamera(obj);
            this.camera3D.FocusCamera(obj);
        }

        public DeepCore.Geometry.Vector3? RayCastVoxelLandFromScreen(PointF point)
        {
            return (this.Camera as IActorCamera)?.RayCastVoxelLandFromScreen(point);
        }

        //         protected override void DrawSpaceDiv2DGrids()
        //         {
        //             base.DrawSpaceDiv2DGrids();
        //          
        //         }

        //--------------------------------------------------------------------------------------------------------------------
        #region UpdateAndDrawingHUD
        public Win32ZoneGUIRuntime HUDRuntime { get; } = new Win32ZoneGUIRuntime();
        public Win32DisplayRoot HUDRootNode { get; private set; }
        public bool AutoUpdateBattleClient { get; set; } = true;
        private VoxelLayer raycastLayer;
        private Vector3 raycastLayerTouch;
        private Glu.Ray raycast;
        private void BattleView3D_OnUpdate(GLView sender, TimeSpan interval)
        {
            try
            {
                var mouse = sender.GLControl.PointToClient(Control.MousePosition);
                this.raycast = this.Camera.ScreenToWorldRay(new Vector2(mouse.X, mouse.Y));
                this.raycastLayer = base.RayCastVoxel(raycast, out raycastLayerTouch);
                if (AutoUpdateBattleClient)
                {
                    var intervalMS = (float)interval.TotalMilliseconds;
                    UpateBattle(intervalMS);
                }
            }
            catch (Exception err)
            {
                this.Pause = true;
                event_OnError?.Invoke(sender, err);
                err.ShowMessageBox();
            }
        }

        public virtual void UpdateClient(float intervalMS)
        {
            if (client != null)
            {
                client.BeginUpdate(intervalMS);
                client.Update();
            }
        }

        protected virtual void BattleView3D_OnEndRender(GLView sender, PaintEventArgs3D e)
        {
            if (raycastLayer != null)
            {
                var terrain = this.VoxelTerrain;
                var opos = raycastLayerTouch.WorldToObject();
                opos.Z += 0.2f;
                DrawingVoxelObject.DrawCycle(Color.Yellow, opos, VoxelTerrain.GridCellRadius);
                var color = Color4.White;
                color.A = (float)Math.Abs(Math.Sin(this.TotalTimeSEC * 2f));
                DrawingVoxelObject.FillRectW(
                    color,
                    raycastLayer.X * terrain.GridCellSize,
                    raycastLayer.Y * terrain.GridCellSize,
                    terrain.GridCellSize,
                    terrain.GridCellSize,
                    raycastLayer.Upward + PATH_Y_OFFSET);
            }
        }

        protected virtual void BattleView3D_OnRenderHUD(object sender, PaintEventArgs3D e)
        {
            DrawLogsHUD(e);
            DrawPopUpTextHUD(e);
        }
        protected virtual void BattleView3D_OnPaintHUD(object sender, PaintEventArgs e)
        {
            if (Layer == null) return;
            using (var g = new DrawableGraphics(e.Graphics, glControl))
            {
                try
                {
                    if (ShowSpaceDiv)
                    {
                        render_SpaceDiv(g.g);
                    }
                    if (Layer.Actor != null && !Layer.Actor.IsDisposed)
                    {
                        render_UnitHUD(g, 0, GLControl.Height, AnchorStyles.Bottom | AnchorStyles.Left, Layer.Actor);
                    }
                    if (this.SelectedObject is LayerZoneUnit3D unit3D && !unit3D.IsDisposed)
                    {
                        render_UnitHUD(g, GLControl.Width / 2, 0, AnchorStyles.Top | AnchorStyles.Left, unit3D.ZUnit);
                    }
                    float sw = GLControl.Width / 2;
                    float sy = 1;
                    float sh = 24;
                    var rect_text = new TextRect();
                    {
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(" ZoneUUID=" + this.Layer.ZoneUUID);
                            sb.Append(" LastServerTime=" + TimeSpan.FromMilliseconds(Layer.LastServerTimeMS));
                            sb.Append(" Paused=" + (this.Pause));
                            sb.Append("\n LayerObjects=" + this.ObjectsCount);
                            sb.Append("\n LayerPoolStack=" + this.Layer.ObjectPool.StackCount);
                            if (client is LocalBattle local)
                            {
                                sb.Append("\n HostObjects=" + local.Zone.AllObjectsCount);
                                sb.Append("\n HostPoolStack=" + local.Zone.ObjectPool.StackCount);
                            }
                            sy += rect_text.SetText(sb.ToString()).Draw(g, 1, sy, sw, sh).Height;
                        }
                        {
                            string mem =
                                $" SZ={InstanceZone.ActiveZoneCount}/{InstanceZone.AllocZoneCount}" +
                                $" SO={InstanceZoneObject.ActiveObjectCount}/{InstanceZoneObject.AllocObjectCount}" +
                                $" CZ={LayerZone.ActiveZoneLayerCount}/{LayerZone.AllocZoneLayerCount}" +
                                $" CO={LayerObject.ActiveObjectCount}/{LayerObject.AllocObjectCount}" +
                                $" ST={InstanceUnit.State.ActiveObjectCount}/{InstanceUnit.State.AllocObjectCount}" +
                                $" BM={BattleMessage.ActiveObjectCount}/{BattleMessage.AllocObjectCount}";
                            sy += rect_text.SetText(mem).Draw(g, 1, sy, sw, sh).Height;
                        }
                    }
                    if (client.IsNet)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(" Send=" + client.SendPackages);
                        sb.Append(" Recv=" + client.RecvPackages);
                        sb.Append(" Ping=" + client.CurrentPing);
                        sb.Append(" NetPing=" + client.NetPing);
                        //                     if (client.KickMessage != null)
                        //                     {
                        //                         sb.Append(" Kicked=" + client.KickMessage.message);
                        //                     }
                        sy += rect_text.SetText(sb.ToString()).Draw(g, 1, sy, sw, sh).Height;
                    }
                    {
                        var wp = this.RayToWorldPos(raycast);
                        var sp = this.Camera.WorldToScreen(wp);
                        var sb = new StringBuilder();
                        sb.Append($" FPS={this.CurrentFPS}");
                        sb.Append($" World({wp.X.ToString("#0.0")}, {wp.Y.ToString("#0.0")}, {wp.Z.ToString("#0.0")})");
                        sb.Append($" Screen({sp.X.ToString("#0.0")}, {sp.Y.ToString("#0.0")})  ");
                        if (raycastLayer != null)
                        {
                            sb.Append($"\n Voxel({raycastLayer.X}, {raycastLayer.Y}, down={raycastLayer.Downward.ToString("#0.0")} up={raycastLayer.Upward.ToString("#0.0")}");
                        }
                        sy += rect_text.SetText(sb.ToString()).Draw(g, 1, sy, sw, sh).Height;
                    }
                    if (client.Layer.ServerStatus != null)
                    {
                        var state = client.Layer.ServerStatus;
                        List<string> sb = new List<string>();
                        sb.Add("ServerStatus:");
                        sb.Add(string.Format("   ActiveGameObjectCount = {0}/{1}", state.ActiveGameObjectCount, state.AllocGameObjectCount));
                        sb.Add(string.Format(" ActiveInstanceZoneCount = {0}/{1}", state.ActiveInstanceZoneCount, state.AllocInstanceZoneCount));
                        foreach (var line in sb)
                        {
                            sy += rect_text.SetText(line).Draw(g, 1, sy, sw, sh).Height;
                        }
                    }
                    {
                        sy += rect_text.SetText(" 环境变量:").Draw(g, 1, sy, sw, sh).Height;
                        foreach (string evk in Layer.ListEnvironmentVars())
                        {
                            sy += rect_text.SetText(string.Format(" {0} = {1}", evk, Layer.GetEnvironmentVar(evk))).Draw(g, 1, sy, sw, sh).Height;
                        }
                        if (Layer.Actor != null)
                        {
                            sy += rect_text.SetText(" 单位环境变量:").Draw(g, 1, sy, sw, sh).Height;
                            foreach (string evk in Layer.Actor.ListEnvironmentVars())
                            {
                                sy += rect_text.SetText(string.Format(" {0} = {1}", evk, Layer.Actor.GetEnvironmentVar(evk))).Draw(g, 1, sy, sw, sh).Height;
                            }
                            sy += rect_text.SetText(" 玩家环境变量:").Draw(g, 1, sy, sw, sh).Height;
                            foreach (string evk in Layer.Actor.ListPlayerEnvironmentVars())
                            {
                                sy += rect_text.SetText(string.Format(" {0} = {1}", evk, Layer.Actor.GetPlayerEnvironmentVar(evk))).Draw(g, 1, sy, sw, sh).Height;
                            }
                        }
                    }
                    event_OnDrawHUD?.Invoke(this, g.g);
                }
                finally
                {
                }
            }
        }
        protected virtual void render_UnitHUD(DrawableGraphics g, float start_x, float start_y, AnchorStyles anc, LayerUnit unit)
        {
            float sy = start_y;
            var text_line = new TextLine() { anchor = anc };
            var gauge_line = new GaugeStrip() { anchor = anc };
            {
                float sh = 24;
                var text = LayerDisplay.ToStatusText(unit);
                sy += text_line.SetText(text.ToString()).Draw(g, start_x, sy, 0, sh).Height;
            }

            using (var skills = unit.ObjectPool.AllocList<LayerUnit.SkillState>())
            {
                unit.GetSkillStatus(skills);
                if (skills.Count > 0)
                {
                    var gauge_fan = new GaugeRectFan() { anchor = anc };
                    float sw = 50;
                    float sh = 50;
                    float sx = start_x;
                    float dy = 0;
                    int i = 0;
                    foreach (var ss in skills)
                    {
                        gauge_fan.text_brush = ss.IsActive ? Brushes.White : Brushes.Red;
                        gauge_fan.ToolTips =
                            $"Skill: {ss.Data}\n" +
                            $"  level: {ss.Level}\n" +
                            //$"  pass: {TimeSpan.FromMilliseconds(ss.PassTimeMS)}\n" +
                            $"  expire: {TimeSpan.FromMilliseconds(ss.ExpireTimeMS)}\n" +
                            $"  action index: {ss.CurrentActionID}\n" +
                            $"  speed: {ss.ActionSpeed}\n" +
                            $"  active state: {ss.ActiveState}";
                        dy = gauge_fan
                             .SetText(ss.Data.Name, (ss.IsActive ? ToSkillShortKey(i) : null))
                             .SetAmount(ss.CDAmount)
                             .Draw(g, sx, sy, sw, sh).Height;
                        sx += sw;
                        i++;
                    }
                    sy += dy;
                }
            }


            if (unit is LayerPlayer actor)
            {
                using (var items = unit.ObjectPool.AllocList<LayerPlayer.ItemSlot>())
                {
                    actor.GetItemSlots(items);
                    if (items.Count > 0)
                    {
                        var gauge_fan = new GaugeRectFan() { anchor = anc };
                        float sw = 40;
                        float sh = 40;
                        float sx = start_x;
                        float dy = 0;
                        int i = 0;
                        foreach (var item in items)
                        {
                            if (!item.IsEmpty)
                            {
                                string text1 = null;
                                string text2 = null;
                                float pct = 0;
                                text1 = item.Data.Name;
                                text2 = "x" + item.Count;
                                var cd = actor.GetCoolDownItem(item.Data.ID);
                                if (cd != null) pct = cd.Amount;
                                gauge_fan.ToolTips =
                                    $"Item: {item.Data}\n" +
                                    $"  count: {item.Count}\n" +
                                    $"  expire: {(cd != null ? TimeSpan.FromMilliseconds(cd.ExpireTimeMS) : string.Empty)}";
                                dy = gauge_fan
                                    .SetText(text1, text2)
                                    .SetAmount(pct)
                                    .Draw(g, sx, sy, sw, sh).Height;
                                sx += sw;
                            }
                            i++;
                        }
                        sy += dy;
                    }
                }
            }

            using (var buffs = unit.ObjectPool.AllocList<LayerUnit.BuffState>())
            {
                unit.GetBuffStatus(buffs);
                if (buffs.Count > 0)
                {
                    var gauge_fan = new GaugeRectFan() { anchor = anc };
                    float sw = 40;
                    float sh = 40;
                    float sx = start_x;
                    float dy = 0;
                    int i = 0;
                    foreach (var bs in buffs)
                    {
                        gauge_fan.ToolTips =
                            $"Buff: {bs.Data}\n" +
                            $"  level: {bs.BuffLevel}\n" +
                            $"  expire: {TimeSpan.FromMilliseconds(bs.ExpireTimeMS)}\n" +
                            $"  overlay level: {bs.OverlayLevel}\n" +
                            $"  is equip: {bs.isEquip}\n" +
                            $"  sender id: {bs.SenderID}\n" +
                            $"  ";
                        dy = gauge_fan.SetText(bs.Data.Name, (bs.OverlayLevel != 0) ? bs.OverlayLevel.ToString() : string.Empty).
                            SetAmount(bs.CDAmount).Draw(g, sx, sy, sw, sh).Height;
                        if (bs.OverlayLevel != 0)
                        {

                        }
                        sx += sw;
                        i++;
                    }
                    sy += dy;
                }
            }
            using (var cards = unit.ObjectPool.AllocList<LayerUnit.CardSlot>())
            {
                unit.GetCards(cards);
                if (cards.Count > 0)
                {
                    var gauge_fan = new TextRectBody() { anchor = anc };
                    float sw = 40;
                    float sh = 40;
                    float sx = start_x;
                    float dy = 0;
                    int i = 0;
                    foreach (var bs in cards)
                    {
                        gauge_fan.ToolTips =
                            $"Card: {bs.Card}\n" +
                            $"  level: {bs.Level}";
                        dy = gauge_fan.SetText($"{bs.Card.Name}", $"{bs.Level}").Draw(g, sx, sy, sw, sh).Height;
                        sx += sw;
                        i++;
                    }
                    sy += dy;
                }
            }
            {

                float sh = 24;
                if (unit.ChantingSkill != null)
                {
                    var ss = unit.ChantingSkill;
                    float pct = (unit.ChantingSkillPassMS / (float)unit.ChantingSkillTotalMS);
                    sy += gauge_line
                        .SetText("吟唱：" + ss.Data.Name)
                        .SetAmount(pct)
                        .Draw(g, start_x + 4, sy, 200, sh).Height;
                }
                if (unit.CurrentSkillAction != null)
                {
                    var skill = unit.CurrentSkillAction;
                    sy += gauge_line
                        .SetText("引导：" + skill.SkillData.Name + "(" + skill.CurrentActionIndex + ")")
                        .SetAmount(1 - skill.ExpirePercent)
                        .Draw(g, start_x + 4, sy, 200, sh).Height;
                }
                if (unit.PickEvent != null)
                {
                    var pick = unit.PickEvent;
                    sy += gauge_line
                        .SetText("检取：" + pick.Tag)
                        .SetAmount(pick.Amount)
                        .Draw(g, start_x + 4, sy, 200, sh).Height;
                }
                {
                    sy += text_line.SetText(unit.DisplayName).Draw(g, start_x, sy, 0, sh).Height;
                }
            }
        }
        protected virtual void render_SpaceDiv(Graphics g)
        {
            if (Space != null)
            {
                if (Client is LocalBattle localClient)
                {
                    var zone = localClient.Zone;
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
                                    var screenPos = this.Camera.WorldToScreen(new Vector3(dx, 0, dy));
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
        #endregion

        //--------------------------------------------------------------------------------------------------------------------
        #region LayerEvents
        protected virtual void OnLayerInitFinished(LayerZone layer)
        {
            // Override this method to handle additional initialization after the layer is set up.
        }
        private void OnLayerInit(LayerZone layer)
        {
            //             PostPaintTask(arg =>
            //             {
            //             });
            if (layer.Sender is InstanceZone zone)
            {
                var wd = zone.TerrainWorld;
                if (wd is VoxelWorld vworld)
                {
                    this.InitVoxelWorld(vworld);
                    this.ResetCameraPos();
                }
            }
            else if (layer.Terrain3D is VoxelClientTerrain3D voxel3d)
            {
                var wd = voxel3d.World;
                if (wd is VoxelWorld vworld)
                {
                    this.InitVoxelWorld(vworld);
                    this.ResetCameraPos();
                }
            }

            this.SpaceDivW = layer.SpaceDivSizeW;
            this.Space = layer.SpaceDiv;
            if (layer.Data.Terrain is TerrainData terrain)
            {
                this.terrainZone = layer.Data.ZoneData;
            }

            foreach (var dt in layer.Data.Decorations)
            {
                var zed = layer.GetFlag<LayerEditorDecoration>(dt.Name);
                var gf = ZoneWin32Factory.Instance.CreateFlagView(this, zed);//new LayerZoneDecoration3D(this, zed);
                flags.Add(dt.Name, gf);
                base.AddDisplayObject(gf);
            }
            foreach (var dt in layer.Data.Regions)
            {
                var zed = layer.GetFlag<LayerEditorRegion>(dt.Name);
                var gf = ZoneWin32Factory.Instance.CreateFlagView(this, zed);//new LayerZoneRegion3D(this, zed);
                flags.Add(dt.Name, gf);
                base.AddDisplayObject(gf);
            }
            foreach (var dt in layer.Data.Areas)
            {
                var zed = layer.GetFlag<LayerEditorArea>(dt.Name);
                var gf = ZoneWin32Factory.Instance.CreateFlagView(this, zed);//new LayerZoneArea3D(this, zed);
                flags.Add(dt.Name, gf);
                base.AddDisplayObject(gf);
            }
            foreach (var dt in layer.Data.Points)
            {
                var zed = layer.GetFlag<LayerEditorPoint>(dt.Name);
                var gf = ZoneWin32Factory.Instance.CreateFlagView(this, zed);//new LayerZonePoint3D(this, zed);
                flags.Add(dt.Name, gf);
                base.AddDisplayObject(gf);
            }
            this.ResetCameraPos(camera2D);
            this.ResetCameraPos(camera3D);

            this.OnLayerInitFinished(layer);
        }
        private void OnObjectEnter(LayerZone layer, LayerZoneObject obj)
        {
            //             PostPaintTask(g =>
            //             {
            //                 
            //             });
            if (obj is LayerUnit unit)
            {
                var u = ZoneWin32Factory.Instance.CreateUnitView(this, unit);//new LayerZoneUnit3D(this, unit);
                objects.Add(obj.ObjectID, u);
                base.AddDisplayObject(u);
                if (client.Actor != null && obj.ObjectID == client.Actor.ObjectID)
                {
                    FocusCamera(client.Actor);
                    //                     client.Actor.OnMoneyChanged += (u, oldM, newM) =>
                    //                     {
                    //                         //showLog((newM - oldM).ToString(), unit.X, unit.Y, Color.Yellow);
                    //                     };
                    //                     client.Actor.OnHPChanged += (u, oldM, newM) =>
                    //                     {
                    //                     };
                    //                     client.Actor.OnMPChanged += (u, oldM, newM) =>
                    //                     {
                    //                     };
                }
            }
            else if (obj is LayerSpell spell)
            {
                var u = ZoneWin32Factory.Instance.CreateSpellView(this, spell);//new LayerZoneSpell3D(this, spell);
                objects.Add(obj.ObjectID, u);
                base.AddDisplayObject(u);
            }
            else if (obj is LayerItem item)
            {
                //var u = new LayerZoneItem3D(this, item);
                var u = ZoneWin32Factory.Instance.CreateItemView(this, item);
                objects.Add(obj.ObjectID, u);
                base.AddDisplayObject(u);
            }
        }

        private void OnObjectLeave(LayerZone layer, LayerZoneObject obj)
        {
            var r = objects.RemoveByKey(obj.ObjectID);
            if (r != null)
            {
                base.RemoveDisplayObject(r);
            }
            //             PostPaintTask(g =>
            //             {
            //               
            //             });

            //  showLog("ObjectLeave: " + obj.Name, obj.X, obj.Y);
        }
        protected virtual void OnMessageReceived(LayerZone layer, IBattleMessage msg)
        {
            if (msg is AddEffectEvent effect)
            {
                //AddLog(effect.effect.Name, effect.pos.ToGL().ObjectToWorld());
            }
            else if (msg is BubbleTalkNotify talkNotify)
            {
                AddPopupText(talkNotify);
            }
        }
        protected virtual void OnObjectMessageReceived(LayerZone layer, IBattleMessage msg, LayerZoneObject obj)
        {
            if (msg is UnitDoActionEvent)
            {
                AddObjectLog("UnitDoAction: " + obj.Name + " - " + (msg as UnitDoActionEvent).ActionName, obj);
            }
            else if (ShowLogAll)
            {
                AddObjectLog(msg.ToString(), obj);
            }
        }


        protected virtual void DisposeEvents()
        {
            event_OnDrawHUD = null;
            event_OnError = null;
        }
        private Action<GLView, Graphics> event_OnDrawHUD;
        private Action<GLView, Exception> event_OnError;
        public event Action<GLView, Graphics> OnDrawHUD
        {
            add { event_OnDrawHUD += value; }
            remove { event_OnDrawHUD -= value; }
        }
        public event Action<GLView, Exception> OnError
        {
            add { event_OnError += value; }
            remove { event_OnError -= value; }
        }
        #endregion
        //--------------------------------------------------------------------------------------------------------------------
        #region LayerObjects

        private HashMap<uint, LayerZoneObject3D> objects = new HashMap<uint, LayerZoneObject3D>();
        private HashMap<string, LayerZoneFlag3D> flags = new HashMap<string, LayerZoneFlag3D>();

        public int LayerObjectsCount { get { return client.Layer.ObjectsCount; } }
        public LayerZoneObject3D SelectedObject { get; private set; }

        public LayerZoneObject3D GetObject(uint id)
        {
            return objects.Get(id);
        }
        public LayerZoneFlag3D GetFlag(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return flags.Get(name) as LayerZoneFlag3D;
        }
        public LayerZoneUnit3D GetUnitByName(string name)
        {
            foreach (var u in objects.Values)
            {
                if (u is LayerZoneUnit3D unit && name.Equals(unit.ZUnit.Name))
                {
                    return unit;
                }
            }
            return null;
        }

        private void DisposeObjects()
        {
            SelectedObject = null;
            foreach (var obj in objects.Values)
            {
                obj.Dispose();
            }
            objects.Clear();
            foreach (var obj in flags.Values)
            {
                obj.Dispose();
            }
            flags.Clear();

        }


        #endregion
        //--------------------------------------------------------------------------------------------------------------------
        #region Interaction

        public string ToSkillShortKey(int index)
        {
            index++;
            if (index >= 1 && index <= 9) { return (index).ToString(); }
            if (index == 10) { return "0"; }
            if (index >= 11 && index <= 19) { return "num" + (index - 10).ToString(); }
            if (index == 20) { return "num0"; }
            return "";
        }

        public int ToSkillIndex(Keys key)
        {
            if (key >= Keys.D1 && key <= Keys.D9)
            {
                int i = (key - Keys.D1);
                return i;
            }
            if (key == Keys.D0) { return 9; }
            if (key >= Keys.NumPad1 && key <= Keys.NumPad9)
            {
                int i = (key - Keys.NumPad1);
                return 10 + i;
            }
            if (key == Keys.NumPad0) { return 19; }
            return int.MaxValue;
        }

        public void ActorLaunchSkill(KeyEventArgs e, Glu.Ray ray)
        {
            var actor = Layer.Actor;
            if (actor != null)
            {
                var status = actor.GetSkillStatus();
                int i = ToSkillIndex(e.KeyCode);
                if (i < status.Count)
                {
                    var ss = status[i];
                    var target = base.RayCastVoxel(ray, out var touch);
                    if (target != null)
                    {
                        actor.SendUnitLaunchSkill(ss.Data.ID, touch.WorldToObject().ToGeometry());
                    }
                    else
                    {
                        actor.SendUnitLaunchSkill(ss.Data.ID);
                    }
                }
            }
        }
        public void ActorLaunchSkill(KeyEventArgs e, LayerZoneObject target)
        {
            var actor = Layer.Actor;
            if (actor != null)
            {
                var status = actor.GetSkillStatus();
                int i = ToSkillIndex(e.KeyCode);
                if (i < status.Count)
                {
                    var ss = status[i];
                    actor.SendUnitLaunchSkill(ss.Data.ID, target.ObjectID);
                }
            }
        }

        public void ActorUseItem(KeyEventArgs e)
        {
            var actor = Layer.Actor;
            if (actor != null)
            {
                var items = actor.GetItemSlots();
                int i = ToSkillIndex(e.KeyCode);
                if (i < items.Count)
                {
                    var slot = items[i];
                    actor.SendUnitUseItem(i);
                }
            }
        }
        public virtual LayerItem CheckPickItem(Glu.Ray ray)
        {
            if (Actor != null)
            {
                //this.Cursor = Cursors.Default;
                var item = Layer.GetNearPickableItem(Actor, Actor.AGuard ? Actor.AGuard.GuardRange : Actor.BodyBlockSize);
                if (item != null)
                {
                    var item3D = GetObject(item.ObjectID);
                    //if (CMath.IncludeRoundPoint(item.X, item.Y, item.Info.BodySize, mouseX, mouseY))
                    if (item3D != null && item3D.TryRayCast(ray, out var wdpos))
                    {
                        //this.Cursor = Cursors.Hand;
                        return item;
                    }
                }
            }
            return null;
        }

        public LayerZoneObject3D PickObject3D(Glu.Ray ray, out Vector3 wd_pos)
        {
            foreach (var u in objects.Values)
            {
                if (u.IsPickable)
                {
                    if (u.TryRayCast(ray, out wd_pos))
                    {
                        SelectedObject = (u);
                        return u;
                    }
                }
            }
            wd_pos = Vector3.Zero;
            SelectedObject = null;
            return null;
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------------
        #region HudLogs

        private List<DisplayLog> logs = new List<DisplayLog>();

        private void DrawLogsHUD(PaintEventArgs3D e)
        {
            var tickMS = (int)e.ELapsed.TotalMilliseconds;
            for (int i = logs.Count - 1; i >= 0; --i)
            {
                var u = logs[i];
                if (u.Draw(e, tickMS))
                {
                    u.Dispose();
                    logs.RemoveAt(i);
                }
            }
        }
        private void ClearLogs()
        {
            for (int i = logs.Count - 1; i >= 0; --i)
            {
                logs[i].Dispose();
            }
            logs.Clear();
        }

        public void AddLog(string text, Color4 color, Vector3 worldPos)
        {
            if (ShowLog)
            {
                var pos = this.Camera.WorldToScreen(worldPos);
                logs.Add(new DisplayLog(text, color, pos.Xy));
            }
        }
        public void AddLog(string text, Vector3 worldPos)
        {
            AddLog(text, Color4.White, worldPos);
        }
        public void AddObjectLog(string text, LayerObject obj)
        {
            AddObjectLog(text, Color4.White, obj);
        }
        public void AddObjectLog(string text, Color4 color, LayerObject obj)
        {
            var pos = (obj.Position + new DeepCore.Geometry.Vector3(0, 0, obj.BodyHeight));
            AddLog(text, color, pos.ToGL().ObjectToWorld());
        }

        class DisplayLog : IDisposable
        {
            private long timeMS = 0;
            private Vector2 pos;
            private readonly GLTextTexture2D texture;
            internal DisplayLog(string text, Color4 color, Vector2 pos)
            {
                this.pos = pos;
                this.texture = new GLTextTexture2D(FontStyle.Regular, 16, color);
                this.texture.Text = text;
            }
            public void Dispose()
            {
                texture.Dispose();
            }
            internal bool Draw(PaintEventArgs3D g, int tickMS)
            {
                texture.DrawQuards2D(g, pos.X, pos.Y - ((timeMS / (float)LOG_MAX_TIME_MS) * LOG_MAX_HEIGHT), GLTextureAnchor.C_B);
                timeMS += tickMS;
                if (timeMS < LOG_MAX_TIME_MS)
                {
                    return false;
                }
                return true;
            }
        }
        public static int LOG_MAX_TIME_MS = 1000;
        public static int LOG_MAX_HEIGHT = 20;
        #endregion
        //--------------------------------------------------------------------------------------------------------------------
        #region PopUpText
        private HashMap<uint, PopupText> popTexts = new HashMap<uint, PopupText>();
        private void DrawPopUpTextHUD(PaintEventArgs3D e)
        {
            var tickMS = (int)e.ELapsed.TotalMilliseconds;
            foreach (var txt in popTexts.ToArray())
            {
                var u = txt.Value;
                if (u.Draw(e, tickMS))
                {
                    u.Dispose();
                    popTexts.Remove(txt.Key);
                }
            }
        }
        private void ClearPopupTextHUD()
        {
            foreach (var txt in popTexts)
            {
                var u = txt.Value;
                u.Dispose();
            }
            popTexts.Clear();
        }

        public void AddPopupText(BubbleTalkNotify ntf)
        {
            if (ntf.TalkInfos != null)
            {
                foreach (var txt in ntf.TalkInfos)
                {
                    var obj = Layer.GetObject(txt.TalkUnit);
                    var pop = new PopupText(this, txt.TalkContent, Color.White, txt.TalkKeepTimeMS, obj);
                    uint key = obj != null ? obj.ObjectID : 0;
                    if (popTexts.TryRemove(key, out var old))
                    {
                        old.Dispose();
                    }
                    popTexts.Put(key, pop);
                }
            }
        }

        class PopupText : IDisposable
        {
            private readonly BattleView3D view;
            private readonly long totalTimeMS;
            private readonly LayerZoneObject obj;
            private readonly GLTextTexture2D texture;
            private long timeMS = 0;
            internal PopupText(BattleView3D view, string text, Color4 color, int totalTimeMS, LayerZoneObject obj)
            {
                this.view = view;
                this.timeMS = 0;
                this.totalTimeMS = totalTimeMS;
                this.obj = obj;
                this.texture = new GLTextTexture2D(FontStyle.Regular, 20, color);
                this.texture.Text = text;
                this.texture.ExpectSize = new SizeF(300, 200);
                this.texture.BackColor = Color.DarkGray;
                this.texture.BackBorderColor = Color.LightGray;
                this.texture.BorderColor = Color.Black;
                obj?.Retain();
            }
            public void Dispose()
            {
                obj?.Release();
                texture.Dispose();
            }
            internal bool Draw(PaintEventArgs3D g, int tickMS)
            {
                if (obj != null)
                {
                    if (!obj.IsEnable)
                    {
                        return true;
                    }
                    var pos = (obj.Position + new DeepCore.Geometry.Vector3(0, 0, obj.BodyHeight)).ToGL().ObjectToWorld();
                    pos = view.Camera.WorldToScreen(pos);
                    texture.DrawQuards2D(g, pos.X, pos.Y - 50, GLTextureAnchor.C_B);
                }
                else
                {
                    texture.DrawQuards2D(g, view.Width / 2, view.Height / 2, GLTextureAnchor.C_B);
                }
                if (totalTimeMS > 0)
                {
                    this.timeMS += tickMS;
                    if (timeMS > totalTimeMS)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion
        //--------------------------------------------------------------------------------------------------------------------
    }
}
