using DeepCore;
using DeepCore.Game3D.Slave.Layer;
using DeepEditor.Common;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel;
using DeepEditor.Plugin3D.Display3D;
using DeepMetaGame.Data.Message.UI;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using static DeepCore.GameData.Zone.ZoneEditor.EventTrigger.ZoneIntegerValue;

namespace DeepEditor.Plugin3D.BattleClient
{
    public interface IActorCamera
    {
        DeepCore.Geometry.Vector3? RayCastVoxelLandFromScreen(PointF point);
        void FocusCamera(LayerObject obj);
    }

    public class ActorCamera3D : WorldCamera3D, IActorCamera
    {
        private float backDistanec = 20f;
        private float backYaw = 0f;
        private float backPitch = -MathHelper.Pi / 4;
        private float camDir;
        private float moveDir;
        private float moveDistance;
        private Vector3 targetTo = Vector3.Zero;
        private BattleView3D battle;
        public LayerPlayer Actor { get => battle.Actor; }


        public DeepCore.Geometry.Vector3? RayCastVoxelLandFromScreen(PointF point)
        {
            var ray = battle.Camera.ScreenToWorldRay(new Vector2(point.X, point.Y));
            var target = battle.RayCastVoxel(ray, out var touch);
            if (target != null)
            {
                var ret = touch.WorldToObject().ToGeometry();
                return ret;
            }
            return null;
        }
        public void FocusCamera(LayerObject Actor)
        {
            this.backYaw = Actor.Direction - CMath.PI_F / 2f;
            {
                var actor_pos = Actor.Position;
                actor_pos.Z += Actor.BodyHeight;
                Vector3 front;
                front.X = backDistanec * (float)(-Math.Sin(backYaw));
                front.Y = backDistanec * (float)(Math.Sin(backPitch));
                front.Z = backDistanec * (float)(-Math.Cos(backPitch) * Math.Cos(backYaw));
                var camRight = Vector3.Cross(front, Vector3.UnitY).Normalized();
                var camUp = Vector3.Cross(camRight, front).Normalized();
                var target = actor_pos.ToGL().ObjectToWorld();
                //var td = Vector3.Distance(target, targetTo);
                this.targetTo = Vector3.Lerp(targetTo, target, 0.25f);
                this.targetTo.X = target.X;
                this.targetTo.Z = target.Z;
                var camPos = targetTo - front;
                this.mtx_modelview = Matrix4.LookAt(camPos, targetTo, camUp);
                this.mCamPosition = camPos;
                this.mCamPitch = backPitch + MathHelper.Pi;
                this.mCamYaw = backYaw + MathHelper.Pi;
                this.camDir = CMath.GetDegree(targetTo.X - camPos.X, targetTo.Z - camPos.Z);
                //GL.LoadMatrix(ref mtx_modelview);
            }
        }
        public ActorCamera3D(BattleView3D battle)
        {
            this.battle = battle;
            this.battle.Layer.ActorAdded += Layer_ActorAdded;
        }
        protected override void Disposing()
        {
            base.Disposing();
            this.battle = null;
        }
        private void Layer_ActorAdded(LayerZone layer, LayerPlayer actor)
        {
            FocusCamera(actor);
        }

        protected override void InternalLookAt()
        {
            if (Actor != null)
            {
                var actor_pos = Actor.Position;
                actor_pos.Z += Actor.BodyHeight;
                if (Actor.Parent.CameraOffset != null)
                {
                    var offset = Actor.Parent.CameraOffset;
                    var pos = new DeepCore.Geometry.Vector3();
                    DeepCore.Geometry.VectorHelper.MovePolar(ref pos, offset.Angle, offset.Radius);
                    pos.Z = offset.OffsetZ;
                    actor_pos += pos;
                }
                Vector3 front;
                front.X = backDistanec * (float)(-Math.Sin(backYaw));
                front.Y = backDistanec * (float)(Math.Sin(backPitch));
                front.Z = backDistanec * (float)(-Math.Cos(backPitch) * Math.Cos(backYaw));
                var camRight = Vector3.Cross(front, Vector3.UnitY).Normalized();
                var camUp = Vector3.Cross(camRight, front).Normalized();
                var target = actor_pos.ToGL().ObjectToWorld();
                //var td = Vector3.Distance(target, targetTo);
                this.targetTo = Vector3.Lerp(targetTo, target, 0.25f);
                this.targetTo.X = target.X;
                this.targetTo.Z = target.Z;
                var camPos = targetTo - front;
                this.mtx_modelview = Matrix4.LookAt(camPos, targetTo, camUp);
                this.mCamPosition = camPos;
                this.mCamPitch = backPitch + MathHelper.Pi;
                this.mCamYaw = backYaw + MathHelper.Pi;
                this.camDir = CMath.GetDegree(targetTo.X - camPos.X, targetTo.Z - camPos.Z);
                GL.LoadMatrix(ref mtx_modelview);
            }
            else
            {
                base.InternalLookAt();
            }
        }
        protected override void ProcessMouseDrag(GLControl control, MouseEventArgs e, MouesMoveArgs args)
        {
            if (Actor != null)
            {
                var o = args.ScreenOffset * MouseSensitivity;
                this.backYaw += o.X;
                this.backPitch += o.Y;
            }
            else
            {
                base.ProcessMouseDrag(control, e, args);
            }
        }
        protected override void ProcessQueryKey(GLControl control, TimeSpan elapsed)
        {
            if (Actor != null && Actor.IsActive)
            {
                int dx = 0, dy = 0, dz = 0;
                float angle = 0;
                float distance = 0;
                if (Mouse.IsMouseDown(MouseButtons.Left) && Mouse.IsMouseDown(MouseButtons.Right))
                {
                    angle = 0;
                    distance = 1;
                }
                else
                {
                    if (Keyboard.IsKeyDown(Keys.W)) { dx += 1; }
                    if (Keyboard.IsKeyDown(Keys.S)) { dx -= 1; }
                    if (Keyboard.IsKeyDown(Keys.A)) { dy -= 1; }
                    if (Keyboard.IsKeyDown(Keys.D)) { dy += 1; }
                    if (Keyboard.IsKeyDown(Keys.Q)) { dz += 1; }
                    if (Keyboard.IsKeyDown(Keys.E)) { dz -= 1; }
                    angle = CMath.GetDegree(dx, dy);
                    distance = CMath.GetDistance(0, 0, dx, dy);
                }
                //                 if (Keyboard.IsKeyDown(Keys.Space))
                //                 {
                //                 }
                //                 else
                {
                    this.moveDistance = distance;
                    this.moveDir = camDir + angle;
                    {
                        if (dz != 0)
                        {
                            Actor.SendUnit3DAxisAngle(moveDir, distance, camDir, CMath.GetDirect(dz) * Actor.MoveSpeedSEC);
                        }
                        else if (distance > 0)
                        {
                            Actor.SendUnitAxisAngle(moveDir, distance, camDir);
                        }
                        else
                        {
                            Actor.SendUnitAxisAngle(0, 0, camDir);
                        }

                    }
                }
                {
                    var mouse = control.GetMousePoint();
                    var ray = battle.Camera.ScreenToWorldRay(new Vector2(mouse.X, mouse.Y));
                    var item = battle.CheckPickItem(ray);
                    if (item != null)
                    {
                        control.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        control.Cursor = Cursors.Arrow;
                    }
                }
            }
            else
            {
                base.ProcessQueryKey(control, elapsed);
            }
        }
        protected override void ProcessKeyDown(GLControl control, KeyEventArgs e)
        {
            if (Actor != null && Actor.IsActive)
            {
                if (e.KeyCode == Keys.Space)
                {
                    Actor.SendJump(moveDir, moveDistance);
                }
                if (e.KeyCode == Keys.E)
                {
                    Actor.SendFall(moveDir, moveDistance);
                }
                if (e.KeyCode == Keys.ShiftKey)
                {
                    battle.ActorUseItem(e);
                }

                {
                    if (battle.SelectedObject != null)
                    {
                        battle.ActorLaunchSkill(e, battle.SelectedObject.ZObject);
                    }
                    else
                    {
                        var mouse = control.GetMousePoint();
                        var ray = battle.Camera.ScreenToWorldRay(new Vector2(mouse.X, mouse.Y));
                        battle.ActorLaunchSkill(e, ray);
                    }
                }
            }
            else
            {
                base.ProcessKeyDown(control, e);
            }
        }
        protected override void ProcessMouseDown(GLControl control, MouseEventArgs e)
        {
            if (Actor != null && Actor.IsActive)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Keyboard.IsShiftDown)
                    {
                        Actor.SendUnitLaunchNormalAttack();
                    }
                    else
                    {
                        var ray = battle.Camera.ScreenToWorldRay(new Vector2(e.X, e.Y));
                        var selected = battle.PickObject3D(ray, out var wd_pos);
                        if (selected != null)
                        {
                            Actor.Parent.SendAction(Actor.ObjectPool.AllocInit<MouseSelectObjectAction>((t) =>
                            {
                                t.HitObjectID = selected.ZObject.ObjectID;
                            }));
                            if (selected is LayerZoneItem3D item3D)
                            {
                                if (battle.Layer.IsPickableItem(Actor, item3D.ZItem))
                                {
                                    Actor.SendUnitPickObject(item3D.ZObject.ObjectID);
                                }
                            }
                            else if (selected is LayerZoneUnit3D unit3D)
                            {
                                if (unit3D.ZUnit.Force != Actor.Force)
                                {
                                    Actor.SendUnitFocuseTarget(unit3D.ZUnit.ObjectID);
                                }
                                else
                                {
                                    Actor.SendUnitPickObject(unit3D.ZUnit.ObjectID);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var ray = battle.Camera.ScreenToWorldRay(new Vector2(e.X, e.Y));
                var selected = battle.PickObject3D(ray, out var wd_pos);
                base.ProcessMouseDown(control, e);
            }
        }
        protected override void ProcessMouseWheel(GLControl control, MouseEventArgs e, float delta)
        {
            if (Actor != null && Actor.IsActive)
            {
                var add = Keyboard.IsShiftDown ? ShiftAddSpeedRate : 1.0f;
                this.backDistanec -= (DeepCore.CMath.GetDirect(delta) * MovementSpeed * add);
                if (backDistanec < 1)
                {
                    backDistanec = 1;
                }
            }
            else
            {
                base.ProcessMouseWheel(control, e, delta);
            }
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------------------------------
    public class ActorCamera2D : WorldCamera2D, IActorCamera
    {
        private BattleView3D battle;
        private float moveDir;
        private float moveDistance;
        public LayerPlayer Actor { get => battle.Layer.Actor; }
        public ActorCamera2D(BattleView3D battle)
        {
            this.battle = battle;
        }
        protected override void Disposing()
        {
            base.Disposing();
            this.battle = null;
        }

        public DeepCore.Geometry.Vector3? RayCastVoxelLandFromScreen(PointF point)
        {
            var ray = battle.Camera.ScreenToWorldRay(new Vector2(point.X, point.Y));
            var target = battle.RayCastVoxel(ray, out var touch);
            if (target != null)
            {
                var ret = touch.WorldToObject().ToGeometry();
                return ret;
            }
            return null;
        }
        public void FocusCamera(LayerObject obj)
        {
            var pos = obj.Position;
            pos.Z += obj.BodyHeight;
            base.SetTarget(pos.ToGL().ObjectToWorld());
        }
        public override void BeginLookAt(GLControl control, TimeSpan elapsed)
        {
            if (Actor != null && Actor.IsActive)
            {
                FocusCamera(Actor);
            }
            base.BeginLookAt(control, elapsed);
        }

        protected override void ProcessQueryKey(GLControl control, TimeSpan elapsed)
        {
            if (Actor != null && Actor.IsActive)
            {
                var mouse = control.PointToClient(Control.MousePosition);
                var ray = ScreenToWorldRay(new Vector2(mouse.X, mouse.Y));
                var delta = ray.center - base.CamPosition;
                var dx = delta.X;
                var dy = delta.Z;
                var angle = CMath.GetDegree(dx, dy);
                var distance = CMath.GetDistance(0, 0, dx, dy);

                if (Mouse.IsMouseDown(MouseButtons.Right))
                {
                    this.moveDir = angle;
                    this.moveDistance = distance;
                    //                     if (Keyboard.IsKeyDown(Keys.Space))
                    //                     {
                    //                         Actor.SendJump(angle, distance);
                    //                     }
                    //                     else
                    if (control.ContainsMousePoint())
                    {
                        Actor.SendUnitAxisAngle(angle, distance, angle);
                    }
                    else
                    {
                        Actor.SendUnitAxisAngle(0, 0, angle);
                    }
                }
                else
                {
                    dx = 0;
                    dy = 0;
                    if (Keyboard.IsKeyDown(Keys.W)) { dy -= 1; }
                    if (Keyboard.IsKeyDown(Keys.S)) { dy += 1; }
                    if (Keyboard.IsKeyDown(Keys.A)) { dx -= 1; }
                    if (Keyboard.IsKeyDown(Keys.D)) { dx += 1; }
                    var move_angle = CMath.GetDegree(dx, dy);
                    var move_distance = CMath.GetDistance(0, 0, dx, dy);

                    this.moveDir = move_angle;
                    this.moveDistance = move_distance;
                    //                     if (Keyboard.IsKeyDown(Keys.Space))
                    //                     {
                    //                         Actor.SendJump(move_angle, move_distance);
                    //                     }
                    if (control.ContainsMousePoint())
                    {
                        if (move_distance > 0)
                        {
                            Actor.SendUnitAxisAngle(move_angle, move_distance, angle);
                        }
                        else
                        {
                            Actor.SendUnitAxisAngle(0, 0, angle);
                        }
                    }
                    //                     else
                    //                     {
                    //                         Actor.SendUnitAxisAngle(0, 0, angle);
                    //                     }
                }
                {
                    var item = battle.CheckPickItem(ray);
                    if (item != null)
                    {
                        control.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        control.Cursor = Cursors.Arrow;
                    }
                }
            }
            else
            {
                base.ProcessQueryKey(control, elapsed);
            }
        }
        protected override void ProcessKeyDown(GLControl control, KeyEventArgs e)
        {
            if (Actor != null && Actor.IsActive)
            {
                if (e.KeyCode == Keys.Space)
                {
                    Actor.SendJump(moveDir, moveDistance);
                }
                if (e.KeyCode == Keys.E)
                {
                    Actor.SendFall(moveDir, moveDistance);
                }
                if (e.KeyCode == Keys.ShiftKey)
                {
                    battle.ActorUseItem(e);
                }

                {
                    if (battle.SelectedObject != null)
                    {
                        battle.ActorLaunchSkill(e, battle.SelectedObject.ZObject);
                    }
                    else
                    {
                        var mouse = control.GetMousePoint();
                        var ray = battle.Camera.ScreenToWorldRay(new Vector2(mouse.X, mouse.Y));
                        battle.ActorLaunchSkill(e, ray);
                    }
                }
            }
            else
            {
                base.ProcessKeyDown(control, e);
            }
        }
        protected override void ProcessMouseDown(GLControl control, MouseEventArgs e)
        {
            if (Actor != null && Actor.IsActive)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Keyboard.IsShiftDown)
                    {
                        Actor.SendUnitLaunchNormalAttack();
                    }
                    else
                    {
                        var ray = battle.Camera.ScreenToWorldRay(new Vector2(e.X, e.Y));
                        var selected = battle.PickObject3D(ray, out var wd_pos);
                        if (selected != null)
                        {
                            Actor.Parent.SendAction(Actor.ObjectPool.AllocInit < MouseSelectObjectAction>((t)=>
                            {
                                t.HitObjectID = selected.ZObject.ObjectID;
                            }));
                            if (selected is LayerZoneItem3D item3D)
                            {
                                if (battle.Layer.IsPickableItem(Actor, item3D.ZItem))
                                {
                                    Actor.SendUnitPickObject(item3D.ZObject.ObjectID);
                                }
                            }
                            else if (selected is LayerZoneUnit3D unit3D)
                            {
                                if (unit3D.ZUnit.Force != Actor.Force)
                                {
                                    Actor.SendUnitFocuseTarget(unit3D.ZUnit.ObjectID);
                                }
                                else
                                {
                                    Actor.SendUnitPickObject(unit3D.ZUnit.ObjectID);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                base.ProcessMouseDown(control, e);
            }
        }
        protected override void ProcessMouseDrag(GLControl control, MouseEventArgs e, MouesMoveArgs args)
        {
            if (Actor != null && Actor.IsActive)
            {
            }
            else
            {
                base.ProcessMouseDrag(control, e, args);
            }
        }
    }
}
