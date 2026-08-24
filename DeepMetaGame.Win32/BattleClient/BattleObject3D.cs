using DeepCore;
using DeepCore.Game3D.Host.Helper;
using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Slave.Layer;
using DeepCore.Space;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel;
using DeepEditor.Plugin3D.Display3D;
using DeepMetaGame.Data.Message;
using DeepMetaGame.Data.Misc;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using NetUV.Core.Requests;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using System.Drawing;
using System.IO;
using static DeepCore.GUI.Display.Text.BaseRichTextLayer;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeepEditor.Plugin3D.BattleClient
{
    //--------------------------------------------------------------------------------------------------------------------
    #region Abstract 

    public abstract class LayerObject3D : DisplayZoneObject
    {
        protected readonly LayerObject zoneObj;
        public LayerObject LayerObject => zoneObj;
        new public BattleView3D Parent { get; private set; }
        public abstract bool IsDirectionality { get; }
        public abstract Color4 Color { get; }
        public override Vector3 Position
        {
            get
            {
                var zpos = this.zoneObj.Position;
                return new Vector3(zpos.X, zpos.Y, zpos.Z);
            }
        }
        public override float Size
        {
            get { return zoneObj.BodyBlockSize; }
        }
        public LayerObject3D(BattleView3D parent, LayerObject obj) : base(parent)
        {
            this.Parent = parent;
            this.zoneObj = obj;
            //             if ((obj is LayerZoneObject unit))
            //             {
            //                 var text = $"{obj.DisplayName}({unit.ObjectID})";
            //                 if (unit is LayerUnit layerUnit)
            //                 {
            //                     text = $"Lv{layerUnit.Level} " + text;
            //                 }
            //                 base.SetText(text, 16f, Color4.White);
            //             }
            //             else
            //             {
            //             }
            base.SetText(obj.Name, 16f, Color4.White);
        }
        sealed protected override void OnRender(PaintEventArgs3D e)
        {
            if (zoneObj == null || zoneObj.IsDisposing) return;
            base.OnRender(e);
        }
        sealed protected internal override void OnRenderObjectHUD(PaintEventArgs3D e)
        {
            if (zoneObj == null || zoneObj.IsDisposing) return;
            base.OnRenderObjectHUD(e);
        }

        protected override void DrawBody(PaintEventArgs3D e)
        {
            var pos2D = this.Position + new Vector3(0, 0, this.Height / 2);
            DrawingVoxelObject.DrawCycle(Color, Position, Size);
            if (IsDirectionality)
            {
                DrawingVoxelObject.DrawDirection(Color, pos2D, Direction, Size * 2f);
            }
            if (Parent.ShowObjectsAltitude)
            {
                DrawingVoxelObject.DrawHightZ(Color4.Cyan, this.Position);
            }
            DrawNexts();
        }
        protected virtual void ForEachNexts(Action<string> action) { }
        protected virtual void DrawNexts()
        {
            ForEachNexts(name => DrawNext(name));
        }
        protected virtual void DrawNext(string nextName)
        {
            var nextobb = Parent.GetFlag(nextName);
            if (nextobb != null)
            {
                var size = Math.Min(this.Size, nextobb.Size) / 2f;
                float dw = size / 2;
                float cw = size * 2;
                float ch = size * 3;
                DrawingVoxelObject.DrawLine(Color,
                    Position + new Vector3(0, 0, this.Height / 2f),
                    nextobb.Position + new Vector3(0, 0, nextobb.Height / 2f));
            }
        }

    }
    public abstract class LayerZoneObject3D : LayerObject3D
    {
        public LayerZoneObject ZObject { get; private set; }
        public abstract bool IsPickable { get; }
        public override float Direction { get => ZObject.Direction; }
        public override Vector3 Position => new Vector3(ZObject.X, ZObject.Y, ZObject.Z);
        public override float Size => ZObject.RadiusSize;
        public override bool IsVisible => true;
        public override bool IsDirectionality => true;
        public LayerZoneObject3D(BattleView3D parent, LayerZoneObject obj) : base(parent, obj)
        {
            this.ZObject = obj;
        }
        public override bool TryRayCast(Glu.Ray ray, out Vector3 wd_pos)
        {
            if (base.TryRayCast(ray, out wd_pos))
            {
                return true;
            }
            if (this.Height != 0)
            {
                wd_pos = RayCastPlaneOffset(ray, this.Height);
                if (TryPickPlane2D(new Vector2(wd_pos.X, wd_pos.Z)))
                {
                    return true;
                }
            }
            var wp = Position.ObjectToWorld();
            wd_pos = Glu.RayPlaneIntersection(ray, new Glu.Plane(wp, new Vector3(ray.center.X, wp.Y, ray.center.Z) - wp));
            var bounding = new DeepCore.Geometry.VoxelCylinder(Position.ToGeometry(), Size, Height);
            return bounding.Intersects(wd_pos.WorldToObject().ToGeometry());
        }
        protected override void DrawHUD(PaintEventArgs3D e, ref Vector2 offset)
        {
            base.DrawHUD(e, ref offset);
            if (Parent.SelectedObject == this)
            {
                Parent.WorldCamera.DrawObjectBoundsHUD(this, Color4.White);
                //                 var op0 = this.Position;
                //                 var opT = this.Position + new Vector3(0, 0, Math.Max(this.Height, this.Size * 2));
                //                 var sp0 = Parent.Camera.WorldToScreen(op0.ObjectToWorld());
                //                 var spT = Parent.Camera.WorldToScreen(opT.ObjectToWorld());
                //                 var sw = Math.Abs(sp0.Y - spT.Y);
                //                 DrawingHUD.DrawRect(PrimitiveType.LineLoop, Color4.White, spT.X - sw / 2, spT.Y, sw, sw);
            }
        }
    }
    public abstract class LayerZoneFlag3D : LayerObject3D
    {
        public LayerFlag ZFlag { get; }
        public override bool IsVisible => true;
        public override Vector3 Position => new Vector3(ZFlag.X, ZFlag.Y, ZFlag.Z);
        public override float Size => ZFlag.BodyBlockSize;
        public override float Height => 0f;
        public override Color4 Color { get; }
        public LayerZoneFlag3D(BattleView3D parent, LayerFlag flag) : base(parent, flag)
        {
            this.ZFlag = flag;
            this.Color = GLUtils.Argb2Color4(flag.EditorData.Color);
        }
        protected override void DrawHUD(PaintEventArgs3D e, ref Vector2 offset)
        {
            if (Parent.ShowFlagName)
            {
                base.DrawHUD(e, ref offset);
            }
        }
        protected override void ForEachNexts(Action<string> action)
        {
            if (ZFlag.EditorData is SceneVirtualObjectData vdata)
            {
                if (vdata.NextNames != null)
                {
                    foreach (string next in vdata.NextNames)
                    {
                        action(next);
                    }
                }
            }
        }
    }
    public abstract class LayerZoneFlag3D<T> : LayerZoneFlag3D where T : LayerFlag
    {
        new public T ZFlag { get => base.ZFlag as T; }
        public LayerZoneFlag3D(BattleView3D parent, T flag) : base(parent, flag) { }
    }

    #endregion
    //--------------------------------------------------------------------------------------------------------------------

    public class LayerZoneUnit3D : LayerZoneObject3D
    {
        private SkillTemplate mBaseSkill;

        public LayerUnit ZUnit { get; private set; }
        public override float Height => ZUnit.BodyHeight;
        public override Color4 Color { get; }
        public override bool IsPickable => true;

        public LayerZoneUnit3D(BattleView3D parent, LayerUnit obj) : base(parent, obj)
        {
            base.SetText($"{obj.Name}", 16f, Color4.White);
            base.SetDisplayText($"{obj.DisplayName}", 16f, Color4.Yellow);
            this.ZUnit = obj;
            this.Color = color_forces[CMath.CycNum(obj.Force, color_forces.Length)];
            if (ZUnit.ASkill?.BaseSkillID != null)
            {
                var ss = obj.GetSkillState(ZUnit.ASkill.BaseSkillID.SkillID);
                if (ss != null)
                {
                    mBaseSkill = ss.Data;
                }
            }
            this.ZUnit.OnMessageReceived += ZUnit_OnDoEvent;
        }

        private void ZUnit_OnDoEvent(LayerZoneObject obj, ObjectNotify e)
        {
            if (e is UnitLaunchSkillEvent)
            {
                UnitLaunchSkillEvent me = (UnitLaunchSkillEvent)e;
            }
            else if (e is UnitHitEvent)
            {
                UnitHitEvent me = (UnitHitEvent)e;
                Parent.AddObjectLog(me.hp.ToString(), obj);
            }
            else if (e is UnitDeadEvent)
            {
                UnitDeadEvent me = (UnitDeadEvent)e;
            }
            base.SetDisplayText(obj.DisplayName, 16f, Color4.Yellow);
        }

        protected override void DrawHUD(PaintEventArgs3D e, ref Vector2 offset)
        {
            base.DrawHUD(e, ref offset);
        }

        protected override void DrawGUI(PaintEventArgs3D e, ref Vector2 offset)
        {
            base.DrawGUI(e, ref offset);
            if (Common.Keyboard.IsAltDown || Parent.ShowHP || Parent.Layer.Actor == this.ZUnit || Parent.SelectedObject == this)
            {
                if (Parent.ShowMP)
                {
                    DrawingHUD.DrawGauge(Color4.Blue, Color4.Black, 100f * ZUnit.MP / ZUnit.MaxMP, offset.X - 30, offset.Y - 6, 60, 6);
                    offset.Y -= 7;
                }
                DrawingHUD.DrawGauge(Color4.LightGreen, Color4.Black, 100f * ZUnit.HP / ZUnit.MaxHP, offset.X - 30, offset.Y - 6, 60, 6);
                offset.Y -= 7;
            }
        }
        protected override void DrawBody(PaintEventArgs3D e)
        {
            if (ZUnit.IsVisible == false)
                return;
            float bs = ZUnit.BodyBlockSize;
            var pos2D = this.Position + new Vector3(0, 0, this.Height / 2);
            DrawingVoxelObject.DrawDirection(this.Color, pos2D, this.Direction, bs * 2);
            DrawingVoxelObject.DrawDirectionRect(this.Color, pos2D, this.ZUnit.BodyDirection, bs);

            var shape = ZUnit.ZoneShape;
            if (shape != null && Parent.VoxelTerrain != null)
            {
                DrawingObjectZone.DrawZoneShape(Color, Position, shape);
                var gridsize = Parent.VoxelTerrain.GridCellSize;
                //var radius = Parent.VoxelTerrain.GridCellRadius;
                Parent.VoxelTerrain.ForEachByShape<Color4>(shape, this.Color, (color, layer) =>
                {
                    var pos = layer.UpwardTopLeft.ToGL();
                    DrawingVoxelObject.DrawBoundingBox(color, pos, pos + new Vector3(gridsize, gridsize, this.Height));
                    // DrawingVoxelObject.DrawVoxel(color,layer);
                    //                             var pos = new Vector3(
                    //                                 bx * mmap.GridSize,
                    //                                 by * mmap.GridSize,
                    //                                 ed.Data.Z);
                    //                            DrawingVoxelObject.DrawBoundingBox(color, pos, pos + new Vector3(mmap.GridSize, mmap.GridSize, ed.Data.Height));
                    // DrawingVoxelObject.DrawRect(color, pos + new Vector3(0, 0, this.Data.Height/10), mmap.GridSize, mmap.GridSize);
                    // DrawingVoxelObject.DrawRect(color, pos + new Vector3(0, 0, this.Data.Height), mmap.GridSize, mmap.GridSize);
                    //DrawingVoxelObject.DrawHightZ(color, pos, this.Data.Height);
                    return false;
                });
            }
            if (bs > 0)
            {
                var color = this.Color;
                if (color_status.TryGetValue(ZUnit.CurrentState, out var sc))
                {
                    color = sc;
                }
                if (ZUnit.IsPaused)
                {
                    if (color_status.TryGetValue(UnitActionStatus.Pause, out sc))
                    {
                        color = sc;
                    }
                }
                DrawingVoxelObject.DrawBody3D(color, this.Color, this.Color, this.Position, this.Height, bs);
            }
            if (Parent.ShowObjectsAltitude)
            {
                DrawingVoxelObject.DrawHightZ(Color4.Cyan, this.Position);
            }
            if (Parent.ShowDamageRange)
            {
                if (ZUnit.BodyHitSize > 0)
                {
                    DrawingVoxelObject.DrawCycle(color_hit, pos2D, ZUnit.BodyHitSize);
                }
            }
            if (Parent.ShowGuardRange && ZUnit.AGuard)
            {
                if (ZUnit.AGuard.GuardRange > 0)
                    DrawingVoxelObject.DrawCycle(color_guard, pos2D, ZUnit.AGuard.GuardRange);
                //                 if (ZUnit.AGuard.GuardRangeLimitAppend > 0)
                //                     DrawingVoxelObject.DrawCycle(color_guard, pos2D, ZUnit.AGuard.GuardRange + ZUnit.AGuard.GuardRangeLimitAppend);
            }
            if (ZUnit is LayerPlayer actor)
            {
                if (Parent.ShowAOI)
                {
                    DrawingVoxelObject.DrawCycle(color_aoi, pos2D, actor.LoginData.ClientSyncObjectRange);
                    DrawingVoxelObject.DrawCycle(color_aoi, pos2D, actor.LoginData.ClientSyncObjectOutRange);
                }
                if (Parent.ShowAura)
                {
                    using (var auras = actor.ObjectPool.AllocList<LayerUnit.AuraState>())
                    {
                        actor.GetAuraStatus(auras);
                        foreach (var aura in auras)
                        {
                            var temp = aura.Data;
                            DrawingVoxelObject.DrawCycle(Color4.LightBlue, pos2D, aura.Range);
                        }
                    }
                }
                if (Parent.VoxelTerrain != null)
                {
                    if (Parent.VoxelTerrain.TryGetVoxelLayerByPos(this.ZUnit.Position, out var cell, out var layer, true))
                    {
                        DrawingVoxelObject.FillCycle(Color4.Black, new Vector3(pos2D.X, pos2D.Y, layer.Upward + 0.01f), bs);
                    }
                }

            }
            if (Parent.ShowAttackRange)
            {
                var ashape = ZUnit.CurrentAttackShape;
                if (ashape != null)
                {
                    var pos = this.Position + new Vector3(0, 0, this.Height / 2);
                    if (ashape.OffsetRadius != 0)
                    {
                        DeepCore.Geometry.VectorHelper.MovePolar(ref pos.X, ref pos.Y, this.Direction, ashape.OffsetRadius);
                    }
                    DrawingObjectZone.DrawAttackShape(
                        color: color_attack,
                        shape: (AttackShape)ashape.AShape,
                        localPos: pos,
                        bodyHeight: this.Height,
                        direction: this.Direction,
                        size: ZUnit.GetSkillAttackRange(ashape.AttackRange),
                        distance: ZUnit.GetSkillAttackRange(ashape.AttackRange),
                        fan_angle: ashape.AttackAngle,
                        strip_wide: ashape.StripWide * ZUnit.BodyScale);
                }
                else
                {
                    if (ZUnit.CurrentSkillActionData != null)
                    {
                        float rg = ZUnit.GetSkillAttackRange(ZUnit.CurrentSkillAction.SkillData);
                        DrawingVoxelObject.DrawFan(color_attack, pos2D, this.Direction, ZUnit.CurrentSkillAction.SkillData.AttackAngle, rg);
                    }
                    else if (mBaseSkill != null)
                    {
                        float rg = ZUnit.GetSkillAttackRange(mBaseSkill);
                        DrawingVoxelObject.DrawFan(color_attack, pos2D, this.Direction, mBaseSkill.AttackAngle, rg);
                    }
                }
            }
            if (ZUnit.EventSender is InstanceUnit instanceUnit)
            {
                if (instanceUnit.Position != ZUnit.Position)
                {
                    if (bs > 0)
                    {
                        var rpos = instanceUnit.Position;
                        var rcolor = Color4.Cyan;
                        DrawingVoxelObject.DrawBody3D(rcolor, rcolor, rcolor, rpos.ToGL(), this.Height, bs);
                    }
                }
                DrawInstance(e, instanceUnit);
            }
        }
        protected virtual void DrawInstance(PaintEventArgs3D e, InstanceUnit unit)
        {
            // draw instance unit debug //
            //if (Parent.SelectedObject == this)
            {
                if (Parent.ShowUnitMoveAI)
                {
                    if (unit.CurrentMoveAI is MoveAI moveAI)
                    {
                        var srcPos = unit.Position.ToGL();
                        var nextPos = moveAI.NextStepTarget;
                        if (nextPos.HasValue)
                        {
                            var nextP = nextPos.Value.ToGL();
                            DrawingVoxelObject.DrawLine(Color4.Purple, srcPos, nextP);
                            if (moveAI.Target != null)
                            {
                                var targetPos = moveAI.Target.Pos.ToGL();
                                DrawingVoxelObject.DrawLine(Color4.Blue, srcPos, targetPos);
                            }
                            if (moveAI.NextPath != null)
                            {
                                DrawingVoxelObject.DrawWayPoint(Color4.Yellow, Color4.Magenta.SetAlpha(0.5f),
                                    moveAI.NextPath,
                                    this.Parent.VoxelTerrain.GridCellSize,
                                    new Vector3(0, 0, 0.1f));
                            }
                        }

                    }
                }
                //                 if (unit is InstanceGuard guard)
                //                 {
                //                     if (guard.CurrentState is InstanceUnit.StateFollowAndAttack tracing)
                //                     {
                //                         var path = tracing.NextPath;
                //                         if (path != null)
                //                         {
                //                             DrawingVoxelObject.DrawWayPoint(Color4.Blue, Color4.Black, path, this.Parent.VoxelTerrain.GridCellRadius, new Vector3(0, 0, 0.1f));
                //                         }
                //                     }
                //                 }
            }
        }

        static LayerZoneUnit3D()
        {
            color_status[UnitActionStatus.Spawn] = GLUtils.Argb2Color4(0x80, 0x40, 0x40, 0x40);
            color_status[UnitActionStatus.Idle] = GLUtils.Argb2Color4(0x80, 0, 0, 0xFF);
            color_status[UnitActionStatus.Damage] = GLUtils.Argb2Color4(0x80, 0xFF, 0, 0);
            color_status[UnitActionStatus.Dead] = GLUtils.Argb2Color4(0x80, 0x40, 0x40, 0x40);
            color_status[UnitActionStatus.Move] = GLUtils.Argb2Color4(0x80, 0x40, 0x40, 0xFF);
            color_status[UnitActionStatus.Walk] = GLUtils.Argb2Color4(0x80, 0x40, 0x40, 0xFF);
            color_status[UnitActionStatus.Skill] = GLUtils.Argb2Color4(0x80, 0xFF, 0xFF, 0);
            color_status[UnitActionStatus.Stun] = GLUtils.Argb2Color4(0x80, 0xFF, 0xFF, 0xFF);
            color_status[UnitActionStatus.Chaos] = GLUtils.Argb2Color4(0x80, 0xFF, 0x80, 0x80);
            color_status[UnitActionStatus.Escape] = GLUtils.Argb2Color4(0x80, 0xFF, 0x80, 0x80);
            color_status[UnitActionStatus.Pause] = GLUtils.Argb2Color4(0x80, 0x80, 0x80, 0x80);
            color_status[UnitActionStatus.Pick] = GLUtils.Argb2Color4(0x80, 0, 0xFF, 0xFF);
        }
        private static HashMap<UnitActionStatus, Color4> color_status = new HashMap<UnitActionStatus, Color4>();
        private static Color4[] color_forces = new Color4[] { Color4.Blue, Color4.Green, Color4.Red, Color4.Magenta, Color4.Cyan, Color4.Brown, Color4.Purple };
        private static Color4 color_dead = GLUtils.Argb2Color4(128, 255, 255, 255);
        private static Color4 color_attack = GLUtils.Argb2Color4(0x80, 0xff, 0x00, 0x00);
        private static Color4 color_guard = GLUtils.Argb2Color4(0x80, 0xff, 0xff, 0x00);
        private static Color4 color_aoi = GLUtils.Argb2Color4(0x80, 0xff, 0xff, 0xFF);
        private static Color4 color_path = GLUtils.Argb2Color4(0xff, 0x80, 0xff, 0x80);
        private static Color4 color_hit = GLUtils.Argb2Color4(0xff, 0x80, 0xff, 0x80);
        private static Color4 color_hp = GLUtils.Argb2Color4(0xff, 0, 0xff, 0);
        private static Color4 color_mp = GLUtils.Argb2Color4(0xff, 0x80, 0x80, 0xff);
    }

    //--------------------------------------------------------------------------------------------------------------------

    public class LayerZoneSpell3D : LayerZoneObject3D
    {
        public LayerSpell ZSpell { get; private set; }
        public override float Height => ZSpell.Info.BodyHeight;
        public override Color4 Color => Color4.Yellow;
        public override float Direction { get => ZSpell.Direction; }
        public override bool IsPickable => false;
        public LayerZoneSpell3D(BattleView3D parent, LayerSpell obj) : base(parent, obj)
        {
            this.ZSpell = obj;
            base.SetText($"{obj.Info}", 16f, Color4.White);
        }
        public override bool IsInCamera(CameraControl cam)
        {
            var zpos1 = this.ZSpell.Position;
            var zpos2 = this.ZSpell.DistancePos;
            return cam.IsObjectInCamera((zpos1.ToGL()).ObjectToWorld(), Size) ||
                   cam.IsObjectInCamera((zpos2.ToGL()).ObjectToWorld(), Size);
        }
        protected override void DrawHUD(PaintEventArgs3D e, ref Vector2 offset)
        {
            if (Parent.ShowSpellName)
            {
                base.DrawHUD(e, ref offset);
            }
        }
        protected override void DrawBody(PaintEventArgs3D e)
        {
            {
                var pos = this.Position;
                var target = ZSpell.Target as LayerZoneObject;
                var height = this.Height;
                pos = ZSpell.Info.AdjustVoxelAnchor(pos.ToGeometry(), ref height).ToGL();
                switch (ZSpell.Info.BodyShape)
                {
                    case SpellTemplate.Shape.LineToSender:
                        target = ZSpell.Sender;
                        break;
                }
                //             switch (ZSpell.Info.BodyHitVoxelAnchor)
                //             {
                //                 case SpellTemplate.HitVoxelAnchor.NA:
                //                     switch (ZSpell.Info.BodyVoxelAnchor)
                //                     {
                //                         case VoxelAnchor.Floating:
                //                             pos.Z -= height / 2f;
                //                             break;
                //                         case VoxelAnchor.Flooring:
                //                             break;
                //                         case VoxelAnchor.Ceiling:
                //                             pos.Z -= height;
                //                             break;
                //                     }
                //                     break;
                //                 case SpellTemplate.HitVoxelAnchor.Up:
                //                     break;
                //                 case SpellTemplate.HitVoxelAnchor.Middle:
                //                     pos.Z -= height / 2f;
                //                     break;
                //                 case SpellTemplate.HitVoxelAnchor.Down:
                //                     pos.Z -= height;
                //                     break;
                //             }
                var tpos = target?.WaistPosition.ToGL();
                if (tpos == null && this.ZSpell.TargetPos.HasValue)
                {
                    tpos = this.ZSpell.TargetPos.Value.ToGL();
                }
                if (ZSpell.Info.BodyShape == SpellTemplate.Shape.LineToStart)
                {
                    tpos = this.ZSpell.StartPos.ToGL();
                }
                DrawingObjectZone.DrawAttackShape(
                    this.Color,
                    (AttackShape)ZSpell.Info.BodyShape,
                    pos, height,
                    this.Direction,
                    ZSpell.BodySize,
                    ZSpell.Distance,
                    ZSpell.Info.FanAngle,
                    ZSpell.Info.RectWide,
                    tpos);
                //var pos2D = this.Position + new Vector3(0, 0, ZSpell.FloatZ) + new Vector3(0, 0, this.Height / 2);
                if (IsDirectionality)
                {
                    DrawingVoxelObject.DrawDirection(Color4.Blue, this.Position, this.Direction, Size * 2f);
                }
            }

            //             if (ZSpell.Info.ParabolaHeight > 0)
            //             {
            //                 DrawingVoxelObject.DrawLine(Color4.Blue, this.Position, this.Position + new Vector3(0, 0, ZSpell.FloatZ));
            //                 //                 var len = 0f;
            //                 //                 var cur = 0f;
            //                 //                 if (ZSpell.TargetPos != null)
            //                 //                 {
            //                 //                     cur = DeepCore.Geometry.Vector3.Distance(this.ZSpell.StartPos.ToGeometry3(), ZSpell.Position);
            //                 //                     len = DeepCore.Geometry.Vector3.Distance(this.ZSpell.StartPos.ToGeometry3(), ZSpell.TargetPos.ToGeometry3());
            //                 //                 }
            //                 //                 else if (ZSpell.Target != null)
            //                 //                 {
            //                 //                     cur = DeepCore.Geometry.Vector3.Distance(this.ZSpell.StartPos.ToGeometry3(), ZSpell.Position);
            //                 //                     len = DeepCore.Geometry.Vector3.Distance(this.ZSpell.StartPos.ToGeometry3(), ZSpell.Target.Position);
            //                 //                 }
            //                 //                 if (cur > 0)
            //                 //                 {
            //                 //                     var h = (float)Math.Sin(CMath.PI_F * cur / len) * ZSpell.Info.ParabolaHeight;
            //                 //                     pos.Z += h;
            //                 //                     DrawingObjectZone.DrawAttackShape(
            //                 //                         this.Color.SetAlpha(0.5f), (AttackShape)ZSpell.Info.BodyShape,
            //                 //                         pos, spo, this.Height, ZSpell.Info.BodyVoxelAnchor,
            //                 //                         this.Direction,
            //                 //                         ZSpell.BodySize, ZSpell.Distance, ZSpell.Info.FanAngle, ZSpell.Info.RectWide,
            //                 //                         ZSpell.Target?.VoxelBody);
            //                 //                 }
            //             }
            if (ZSpell.EventSender is InstanceSpell instanceSpell)
            {
                if (instanceSpell.Position != ZSpell.Position)
                {
                    if (Size > 0)
                    {
                        var rpos = instanceSpell.Position;
                        var height = instanceSpell.BodyHeight;
                        var rcolor = Color4.Cyan;
                        rpos = ZSpell.Info.AdjustVoxelAnchor(rpos, ref height);
                        DrawingVoxelObject.DrawBody3D(rcolor, rcolor, rcolor, rpos.ToGL(), this.Height, Size);
                    }
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------------

    public class LayerZoneItem3D : LayerZoneObject3D
    {
        public LayerItem ZItem { get; private set; }
        public override float Height => ZItem.Info.BodyHeight;
        public override Color4 Color => Color4.DarkGreen;
        public override bool IsPickable => true;
        public LayerZoneItem3D(BattleView3D parent, LayerItem obj) : base(parent, obj)
        {
            this.ZItem = obj;
        }
        protected override void DrawBody(PaintEventArgs3D e)
        {
            float bs = ZItem.Info.BodySize;
            var pos2D = this.Position + new Vector3(0, 0, this.Height / 2);
            DrawingVoxelObject.DrawDirection(this.Color, pos2D, this.Direction, bs * 2);
            if (bs > 0)
            {
                DrawingVoxelObject.DrawBody3D(this.Color, this.Color, this.Color, this.Position, this.Height, bs);
            }
            if (Parent.ShowObjectsAltitude)
            {
                DrawingVoxelObject.DrawHightZ(Color4.Cyan, this.Position);
            }
        }
        protected override void DrawHUD(PaintEventArgs3D e, ref Vector2 offset)
        {
            base.DrawHUD(e, ref offset);
        }
    }

    //--------------------------------------------------------------------------------------------------------------------

    public class LayerZoneRegion3D : LayerZoneFlag3D<LayerEditorRegion>
    {
        public LayerEditorRegion ZRegion { get; private set; }
        public override float Direction { get => ZRegion.Direction; }
        public override bool IsDirectionality { get => true; }
        public override float Size
        {
            get
            {
                switch (ZRegion.Data.RegionType)
                {
                    case RegionData.Shape.STRIP:
                        return ZRegion.Data.Radius;
                    case RegionData.Shape.ROUND:
                        return ZRegion.Data.R;
                    case RegionData.Shape.RECTANGLE:
                    default:
                        return Math.Max(ZRegion.Data.W, ZRegion.Data.H);
                }
            }
        }
        public LayerZoneRegion3D(BattleView3D parent, LayerEditorRegion flag) : base(parent, flag)
        {
            this.ZRegion = flag;
        }
        protected override void ForEachNexts(Action<string> action)
        {
            base.ForEachNexts(action);
            foreach (var ab in ZRegion.EditorData.GetAbilities())
            {
                if (ab is SpawnUnitAbilityData spawn)
                {
                    action(spawn.StartPointName);
                }
            }
        }
        protected override void DrawBody(PaintEventArgs3D e)
        {
            if (ZRegion.Enable)
            {
                var Data = ZRegion.Data;
                DrawingVoxelObject.DrawDirection(Color, Position, Data.Direction, this.Size);
                var shape = Data.ToZoneShape();
                if (shape != null)
                {
                    DrawingObjectZone.DrawZoneShape(Color, Position, shape);
                }
                else
                {
                    DrawingVoxelObject.DrawCycle(Color, Position, Data.R);
                }
                DrawNexts();
            }
        }
        //         protected override void DrawBody(PaintEventArgs3D e)
        //         {
        //             if (ZRegion.Enable)
        //             {
        //                 var pos2D = this.Position + new Vector3(0, 0, this.Height / 2);
        //                 DrawingVoxelObject.DrawDirection(this.Color, pos2D, this.Direction, this.Size);
        //                 var Data = ZRegion.Data;
        //                 if (Data.RegionType == RegionData.Shape.RECTANGLE)
        //                 {
        //                     DrawingVoxelObject.DrawRect(Color, Position, Data.W, Data.H);
        //                 }
        //                 else if (Data.RegionType == RegionData.Shape.ROUND)
        //                 {
        //                     DrawingVoxelObject.DrawCycle(Color, Position, Data.R);
        //                 }
        //                 DrawNexts();
        //             }
        //         }
    }

    public class LayerZoneDecoration3D : LayerZoneFlag3D<LayerEditorDecoration>
    {
        public LayerEditorDecoration ZDecoration { get; private set; }
        public override float Direction { get => ZDecoration.Data.Direction; }
        public override bool IsDirectionality { get => true; }
        public override float Size
        {
            get
            {
                switch (ZDecoration.Data.RegionType)
                {
                    case DecorationData.Shape.STRIP:
                        return ZDecoration.Data.Radius;
                    case DecorationData.Shape.ROUND:
                        return ZDecoration.Data.R;
                    case DecorationData.Shape.RECTANGLE:
                    default:
                        return Math.Max(ZDecoration.Data.W, ZDecoration.Data.H);
                }
            }
        }
        public LayerZoneDecoration3D(BattleView3D parent, LayerEditorDecoration flag) : base(parent, flag)
        {
            this.ZDecoration = flag;
        }
        protected override void DrawBody(PaintEventArgs3D e)
        {
            if (ZDecoration.Enable)
            {
                var Data = ZDecoration.Data;
                DrawingVoxelObject.DrawDirection(Color, Position, Data.Direction, this.Size);
                var shape = Data.ToZoneShape();
                if (shape != null)
                {
                    DrawingObjectZone.DrawZoneShape(Color, Position, shape);
                    if (ZDecoration.IsStaticBlock)
                    {
                        var gridsize = Parent.VoxelTerrain.GridCellSize;
                        var color = GLUtils.Argb2Color4(Data.Color);
                        Parent.VoxelTerrain.ForEachByShape(shape, color, (color, layer) =>
                        {
                            var pos = layer.UpwardTopLeft.ToGL();
                            DrawingVoxelObject.DrawBoundingBox(color, pos, pos + new Vector3(gridsize, gridsize, Data.Height));
                            return false;
                        });
                    }
                }
                else
                {
                    DrawingVoxelObject.DrawCycle(Color, Position, Data.R);
                }
                DrawNexts();
            }
        }
    }

    public class LayerZonePoint3D : LayerZoneFlag3D<LayerEditorPoint>
    {
        public LayerEditorPoint ZPoint { get; private set; }
        public override float Direction { get => ZPoint.Direction; }
        public override bool IsDirectionality { get => true; }
        public override float Size { get { return ZPoint.RadiusSize; } }
        public LayerZonePoint3D(BattleView3D parent, LayerEditorPoint flag) : base(parent, flag)
        {
            this.ZPoint = flag;
        }
        protected override void DrawBody(PaintEventArgs3D e)
        {
            if (ZPoint.Enable)
            {
                var pos2D = this.Position + new Vector3(0, 0, this.Height / 2);
                DrawingVoxelObject.DrawDirection(this.Color, pos2D, this.Direction, this.Size);
                var Data = ZPoint.Data;
                DrawingVoxelObject.DrawCycle(Color, Position, Data.Radius);
                DrawNexts();
            }
        }
    }
    public class LayerZoneArea3D : LayerZoneFlag3D<LayerEditorArea>
    {
        public LayerEditorArea ZArea { get; private set; }
        public override float Direction { get => ZArea.Data.Direction; }
        public override bool IsDirectionality { get => false; }
        public override float Size { get { return Math.Max(ZArea.Data.W, ZArea.Data.H); } }
        public LayerZoneArea3D(BattleView3D parent, LayerEditorArea flag) : base(parent, flag)
        {
            this.ZArea = flag;
        }
        protected override void DrawBody(PaintEventArgs3D e)
        {
            if (ZArea.Enable)
            {
                var Data = ZArea.Data;
                DrawingVoxelObject.DrawRect(Color, Position, Data.W, Data.H);
                DrawNexts();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------------

}
