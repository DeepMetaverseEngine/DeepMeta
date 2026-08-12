using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel.Display3D;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using DeepEditor.Common.Voxel;
using System;
using System.Drawing;

namespace DeepEditor.Plugin3D.Display3D
{
    public class WorldCamera3D : FreeCameraControl3D, IWorldCamera
    {
        public void DrawObjectBoundsHUD(DisplayZoneObject obj, Color4 color)
        {
            var op0 = obj.Position;
            var opT = obj.Position + new Vector3(0, 0, Math.Max(obj.Height, obj.Size * 2));
            var sp0 = WorldToScreen(op0.ObjectToWorld());
            var spT = WorldToScreen(opT.ObjectToWorld());
            var sw = Math.Abs(sp0.Y - spT.Y);
            DrawingHUD.DrawRect(PrimitiveType.LineLoop, color, spT.X - sw / 2, spT.Y, sw, sw);
        }

        public Vector2 GetDrawStartOffsetHUD(DisplayZoneObject obj)
        {
            var wp = obj.Position.ObjectToWorld();
            wp.Y += obj.Height;
            var offset = WorldToScreen(wp);
            return offset.Xy;
        }
    }

    public class WorldCamera2D : FreeCameraControl2D, IWorldCamera
    {
        public void DrawObjectBoundsHUD(DisplayZoneObject obj, Color4 color)
        {
            var sp0 = WorldToScreen(obj.Position.ObjectToWorld());
            var spW = WorldToScreenSize(obj.Size);
            DrawingHUD.DrawRect(PrimitiveType.LineLoop, color, sp0.X - spW, sp0.Y - spW, spW * 2, spW * 2);
        }
        public Vector2 GetDrawStartOffsetHUD(DisplayZoneObject obj)
        {
            var offset = WorldToScreen(obj.Position.ObjectToWorld());
            offset.Y -= obj.ScreenSize;
            return offset.Xy;
        }
    }
    public interface IWorldCamera
    {
        void DrawObjectBoundsHUD(DisplayZoneObject obj, Color4 color);
        Vector2 GetDrawStartOffsetHUD(DisplayZoneObject obj);
    }
}
