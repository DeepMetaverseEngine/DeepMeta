using DeepCore;
using DeepCore.Voxel.Data;
using DeepEditor.Common.G3D;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepEditor.Common.Voxel.Display3D
{


    public class DisplayVoxelObject3D<T> : DisplayObject3D where T : VoxelObject
    {
        private readonly T vobject;
        public T VObject { get => vobject; }
        public virtual float Height => vobject.Height;
        public virtual float Size { get; set; } = 0.5f;
        public virtual Color4 Color => Color4.Yellow;
        public virtual Vector3 VoxelPosition
        {
            get
            {
                var rv = vobject.Position;
                return new Vector3(rv.X, rv.Y, rv.Z);
            }
        }
        public float Direction { get; set; }
        public float MoveSpeed { get; set; } = 10f;

        public DisplayVoxelObject3D(T obj)
        {
            this.vobject = obj;
        }
        protected override void OnAdded()
        {
            vobject.EnterWorld((this.View as DisplayVoxelWorld3D).World3D);
            base.OnAdded();
        }
        protected override void OnRemoved()
        {
            vobject.LeaveWorld();
            base.OnRemoved();
        }
        protected override void OnUpdate()
        {
            this.vobject.Update(View.LastIntervalMS);
        }
        protected override void OnRender(PaintEventArgs3D e)
        {
            DrawingVoxelObject.DrawBody3D(this.Color, this.Color, this.Color, this.VoxelPosition, this.Height, this.Size);
        }


    }

}
