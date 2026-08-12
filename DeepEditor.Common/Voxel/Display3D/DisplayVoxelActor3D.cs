using DeepCore;
using DeepCore.Voxel.Data;
using DeepEditor.Common.G3D;
using OpenTK.Mathematics;

namespace DeepEditor.Common.Voxel.Display3D
{
    public class DisplayVoxelActor3D<T> : DisplayVoxelObject3D<T>, ILockCameraActor where T : VoxelObject
    {

        public DisplayVoxelActor3D(T obj) : base(obj)
        {
        }

        #region ILockCameraActor

        public float JumpSpeed { get; set; } = 12f;
        Vector3 ILockCameraActor.Position
        {
            get
            {
                var rv = VObject.Position;
                return new Vector3(rv.X, rv.Z, rv.Y);
            }
        }
        bool ILockCameraActor.IsActive => true;
        float ILockCameraActor.Direction => Direction;
        float ILockCameraActor.BodyHeight => Height;
        void ILockCameraActor.Jump()
        {
            VObject.Jump(JumpSpeed);
        }
        void ILockCameraActor.MoveAxis(Vector3 axis)
        {
            var tgt = new DeepCore.Geometry.Vector2(axis.X, axis.Z);
            var speed = MoveSpeed;
            if (Keyboard.IsShiftDown)
            {
                speed *= 2;
            }
            var step = DeepCore.Geometry.MotionHelper.GetDistance(View.LastIntervalMS, speed);
            var angle = CMath.GetDegree(tgt.X, tgt.Y);
            VObject.TryMoveLerp(angle, step, false);
        }
        void ILockCameraActor.FaceTo(float dir)
        {
            this.Direction = dir;
        }

        #endregion
    }

}
