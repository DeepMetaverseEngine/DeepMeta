using DeepCore;
using DeepCore.Space;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel;
using DeepMetaGame.Data.Misc;
using DeepMetaGame.Data.Template;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace DeepEditor.Plugin3D.Display3D
{
    public static class DrawingObjectZone
    {
        //---------------------------------------------------------------------------------------------------------------------
        public static void DrawAttackShape(Color4 color, AttackShape shape,
            Vector3 localPos,
            float bodyHeight,
            float direction,
            float size,
            float distance,
            float fan_angle,
            float strip_wide,
            Vector3? targetPos = null)
        {
            //             var pos1 = localPos;
            //             var pos2 = localPos;
            //             switch (anchor)
            //             {
            //                 case VoxelAnchor.Flooring:
            //                     if (bodyHeight != 0)
            //                     {
            //                         DrawingVoxelObject.DrawHightZ(color, localPos, bodyHeight);
            //                         pos2.Z += bodyHeight;
            //                     }
            //                     break;
            //                 case VoxelAnchor.Floating:
            //                     if (bodyHeight != 0)
            //                     {
            //                         DrawingVoxelObject.DrawHightZ(color, localPos + new Vector3(0, 0, -bodyHeight / 2f), bodyHeight);
            //                         pos2.Z -= bodyHeight / 2f;
            //                         pos1.Z += bodyHeight / 2f;
            //                     }
            //                     break;
            //                 case VoxelAnchor.Ceiling:
            //                     if (bodyHeight != 0)
            //                     {
            //                         DrawingVoxelObject.DrawHightZ(color, localPos, -bodyHeight);
            //                         pos2.Z -= bodyHeight;
            //                     }
            //                     break;
            //             }
            switch (shape)
            {
                case AttackShape.Single:
                    {
                        var p0 = localPos;
                        var p1 = localPos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, direction, distance);
                        DrawingVoxelObject.DrawLine(color, p0, p1);
                    }
                    break;
                case AttackShape.Round:
                    DrawingVoxelObject.DrawBodyMesh3D(color, localPos, bodyHeight, size);
                    //                     DrawingVoxelObject.DrawCycle(color, pos1, size);
                    //                     DrawingVoxelObject.DrawCycle(color, pos2, size);
                    //g.DrawEllipse(pen, new RectangleF(-size, -size, size * 2, size * 2));
                    break;
                case AttackShape.Circle:
                    DrawingVoxelObject.DrawBody3D(Color4.Transparent, color, Color4.Transparent, localPos, bodyHeight, size);
                    //                     DrawingVoxelObject.DrawCycle(color, pos1, size);
                    //                     DrawingVoxelObject.DrawCycle(color, pos2, size);
                    //g.DrawEllipse(pen, new RectangleF(-size, -size, size * 2, size * 2));
                    float sr = size - strip_wide;
                    DrawingVoxelObject.DrawBody3D(Color4.Transparent, color, Color4.Transparent, localPos, bodyHeight, sr);
                    //                     DrawingVoxelObject.DrawCycle(color, pos1, sr);
                    //                     DrawingVoxelObject.DrawCycle(color, pos2, sr);
                    //g.DrawEllipse(pen, new RectangleF(-sr, -sr, sr * 2, sr * 2));
                    break;
                case AttackShape.Fan:
                    //g.DrawFan(pen, direction, size, angle);
                    //                     DrawingVoxelObject.DrawFan(color, pos1, direction, fan_angle, size);
                    //                     DrawingVoxelObject.DrawFan(color, pos2, direction, fan_angle, size);
                    DrawingVoxelObject.DrawFan3D(color, localPos, direction, fan_angle, bodyHeight, size);
                    break;
                case AttackShape.Strip:
                    {
                        float d_width = strip_wide / 2f;
                        float d_distance = distance / 2f;
                        var p0 = localPos;
                        var p1 = localPos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p0.X, ref p0.Y, direction, -d_distance);
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, direction, +d_distance);
                        DrawingVoxelObject.DrawStripe(PrimitiveType.LineLoop, color, p0.X, p0.Y, p1.X, p1.Y, d_width, localPos.Z);
                        DrawingVoxelObject.DrawStripe(PrimitiveType.LineLoop, color, p0.X, p0.Y, p1.X, p1.Y, d_width, localPos.Z + bodyHeight);
                    }
                    break;
                case AttackShape.StripRay:
                case AttackShape.StripRayTouchEnd:
                    {
                        float d_width = strip_wide / 2f;
                        var p1 = localPos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, direction, distance);
                        DrawingVoxelObject.DrawStripe(PrimitiveType.LineLoop, color, localPos.X, localPos.Y, p1.X, p1.Y, d_width, localPos.Z);
                        DrawingVoxelObject.DrawStripe(PrimitiveType.LineLoop, color, localPos.X, localPos.Y, p1.X, p1.Y, d_width, localPos.Z + bodyHeight);
                    }
                    break;
                case AttackShape.RectStrip:
                    {
                        float d_width = strip_wide / 2f;
                        float d_distance = distance / 2f;
                        var p0 = localPos;
                        var p1 = localPos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p0.X, ref p0.Y, direction, -d_distance);
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, direction, +d_distance);
                        DrawingVoxelObject.DrawStripeRect(PrimitiveType.LineLoop, color, p0.X, p0.Y, p1.X, p1.Y, d_width, localPos.Z);
                        DrawingVoxelObject.DrawStripeRect(PrimitiveType.LineLoop, color, p0.X, p0.Y, p1.X, p1.Y, d_width, localPos.Z + bodyHeight);
                    }
                    break;
                case AttackShape.RectStripRay:
                    {
                        float d_width = strip_wide / 2f;
                        var p1 = localPos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, direction, distance);
                        DrawingVoxelObject.DrawStripeRect(PrimitiveType.LineLoop, color, localPos.X, localPos.Y, p1.X, p1.Y, d_width, localPos.Z);
                        DrawingVoxelObject.DrawStripeRect(PrimitiveType.LineLoop, color, localPos.X, localPos.Y, p1.X, p1.Y, d_width, localPos.Z + bodyHeight);
                    }
                    break;
                case AttackShape.WideStrip:
                    {
                        float d_width = distance / 2f;
                        float d_distance = strip_wide / 2f;
                        var p0 = localPos;
                        var p1 = localPos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p0.X, ref p0.Y, direction, -d_distance);
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, direction, +d_distance);
                        DrawingVoxelObject.DrawStripeRect(PrimitiveType.LineLoop, color, p0.X, p0.Y, p1.X, p1.Y, d_width, localPos.Z);
                        DrawingVoxelObject.DrawStripeRect(PrimitiveType.LineLoop, color, p0.X, p0.Y, p1.X, p1.Y, d_width, localPos.Z + bodyHeight);
                    }
                    break;
                case AttackShape.LineToTarget:
                case AttackShape.LineToTargetPos:
                    if (targetPos != null)
                    {
                        var p1 = localPos;
                        var p2 = targetPos.Value;//target.Value.Center + new DeepCore.Geometry.Vector3(0, 0, target.Value.Height / 2);
                        DrawingVoxelObject.DrawCursor(color, p1, p2, strip_wide / 2, strip_wide, strip_wide);
                    }
                    break;
                case AttackShape.LineToStart:
                    if (targetPos != null)
                    {
                        var p1 = localPos;
                        var p2 = targetPos.Value;
                        DrawingVoxelObject.DrawCursor(color, p1, p2, strip_wide / 2, strip_wide, strip_wide);
                    }
                    break;
                case AttackShape.LineToSender:
                    if (targetPos != null)
                    {
                        var p1 = localPos;
                        var p2 = targetPos.Value;
                        DrawingVoxelObject.DrawCursor(color, p1, p2, strip_wide / 2, strip_wide, strip_wide);
                    }
                    break;
            }

        }

        public static void DrawZoneShape(Color4 color, Vector3 pos, IZoneShape shape)
        {
            if (shape is ShapeRound round)
            {
                DrawingVoxelObject.DrawCycle(color, pos, round.r);
            }
            else if (shape is ShapeRect rect)
            {
                DrawingVoxelObject.DrawRect(color, pos, rect.w, rect.h);
            }
            else if (shape is ShapeStripWidth strip)
            {
                DrawingVoxelObject.DrawStripeRect(color, strip.sx, strip.sy, strip.dx, strip.dy, strip.r_wide, pos.Z);
            }
        }
    }

}
