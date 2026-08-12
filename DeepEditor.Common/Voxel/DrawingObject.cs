using DeepCore;
using DeepCore.Geometry.Terrain;
using DeepCore.Voxel.Data;
using DeepEditor.Common.G3D;
using G3D.ObjRenderer;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DeepEditor.Common.Voxel
{
    public static class DrawingVoxelObject
    {

        public static Vector3 WorldToObject(this Vector3 pos)
        {
            return new Vector3(pos.X, pos.Z, pos.Y);
        }
        public static Vector3 ObjectToWorld(this Vector3 pos)
        {
            return new Vector3(pos.X, pos.Z, pos.Y);
        }
        public static Vector3 VoxelToGL(this Vector3 pos)
        {
            return new Vector3(pos.X, pos.Z, pos.Y);
        }
        public static Vector3 GLToVoxel(this Vector3 pos)
        {
            return new Vector3(pos.X, pos.Z, pos.Y);
        }
        public static Glu.BoundingBox VoxelToGL(this Glu.BoundingBox pos)
        {
            return new Glu.BoundingBox(pos.min.VoxelToGL(), pos.max.VoxelToGL());
        }
        public static Glu.BoundingBox GLToVoxel(this Glu.BoundingBox pos)
        {
            return new Glu.BoundingBox(pos.min.GLToVoxel(), pos.max.GLToVoxel());
        }
        public static void DrawVoxelAnchor(Vector3 pos, float len = 10f)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(Color4.Red);
            GL.Vertex3(pos);
            GL.Vertex3(pos + Vector3.UnitX * len);

            GL.Color4(Color4.Green);
            GL.Vertex3(pos);
            GL.Vertex3(pos + Vector3.UnitY * len);

            GL.Color4(Color4.Blue);
            GL.Vertex3(pos);
            GL.Vertex3(pos + Vector3.UnitZ * len);
            GL.End();
        }

        public static DeepCore.Geometry.Triangles ToVoxelTriangles(this Mesh mesh, VoxelTerrain3D terrain)
        {
            var flipY = terrain.BuildConfig.FlipY;
            var ret = new DeepCore.Geometry.Triangles() { Vertices = new DeepCore.Geometry.Triangle[mesh.VertexIndices.Length / 3] };
            var tm = mesh.Transform;
            for (int i = 0; i < mesh.VertexIndices.Length; i += 3)
            {
                var v0 = Vector3.TransformVector(mesh.Vertices[mesh.VertexIndices[i + 0]].To3D(), tm);
                var v1 = Vector3.TransformVector(mesh.Vertices[mesh.VertexIndices[i + 1]].To3D(), tm);
                var v2 = Vector3.TransformVector(mesh.Vertices[mesh.VertexIndices[i + 2]].To3D(), tm);
                var t = new DeepCore.Geometry.Triangle()
                {
                    A = v0.ObjectToWorld().ToGeometry(),
                    B = v1.ObjectToWorld().ToGeometry(),
                    C = v2.ObjectToWorld().ToGeometry(),
                };
                if (flipY)
                {
                    //yc - 1 - y
                    t.A.Y = t.A.Y + terrain.TotalSizeY;
                    t.B.Y = t.B.Y + terrain.TotalSizeY;
                    t.C.Y = t.C.Y + terrain.TotalSizeY;
                }
                ret.Vertices[i / 3] = t;
            }
            return ret;
        }

        public static DeepCore.Geometry.RectangleF GetRangeSize(this DeepCore.Geometry.Rectangle rect, float gridSize)
        {
            return new DeepCore.Geometry.RectangleF(rect.X * gridSize, rect.Y * gridSize, rect.Width * gridSize, rect.Height * gridSize);
        }
        //---------------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------------
        public static void FillRectW(Color4 color, float x, float y, float w, float h, float z)
        {
            DrawRectW(PrimitiveType.Quads, color, x, y, w, h, z);
        }
        public static void DrawRectW(Color4 color, float x, float y, float w, float h, float z)
        {
            DrawRectW(PrimitiveType.LineLoop, color, x, y, w, h, z);
        }
        public static void DrawRectW(PrimitiveType type, Color4 color, float x, float y, float w, float h, float z)
        {
            GL.Begin(type);
            GL.Color4(color);
            GL.Vertex3(x, z, y);
            GL.Vertex3(x + w, z, y);
            GL.Vertex3(x + w, z, y + h);
            GL.Vertex3(x, z, y + h);
            GL.End();
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void FillRect(Color4 color, Vector3 center, float w, float h)
        {
            DrawRect(PrimitiveType.Quads, color, center, w, h);
        }
        public static void DrawRect(Color4 color, Vector3 center, float w, float h)
        {
            DrawRect(PrimitiveType.LineLoop, color, center, w, h);
        }
        public static void DrawRect(PrimitiveType type, Color4 color, Vector3 center, float w, float h)
        {
            GL.Begin(type);
            var rw = w / 2;
            var rh = h / 2;
            GL.Color4(color);
            GL.Vertex3(center.X - rw, center.Z, center.Y - rh);
            GL.Vertex3(center.X + rw, center.Z, center.Y - rh);
            GL.Vertex3(center.X + rw, center.Z, center.Y + rh);
            GL.Vertex3(center.X - rw, center.Z, center.Y + rh);
            GL.End();
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void FillCycle(Color4 color, Vector3 center, float radius)
        {
            DrawCycle(PrimitiveType.TriangleFan, color, center, radius);
        }
        public static void DrawCycle(Color4 color, Vector3 center, float radius)
        {
            DrawCycle(PrimitiveType.LineLoop, color, center, radius);
        }
        public static void DrawCycle(PrimitiveType type, Color4 color, Vector3 center, float radius)
        {
            int count = (int)(16 * Math.Max(radius, 1));
            float rstep = CMath.RADIANS_360 / count;
            float degInRad = 0;
            GL.Begin(type);
            GL.Color4(color);
            for (int i = 0; i < count; i++)
            {
                float px = (float)(center.X + Math.Cos(degInRad) * radius);
                float py = (float)(center.Y + Math.Sin(degInRad) * radius);
                GL.Vertex3(new Vector3(px, center.Z, py));
                degInRad += rstep;
            }
            GL.End();
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void ForArc(Action<Vector3> list, Vector3 center, float startAngle, float endAngle, float radius)
        {
            var degrees = Math.Abs(endAngle - startAngle);
            var count = (int)(16 * Math.Max(radius, 1));
            float rstep = degrees / count;
            float degInRad = startAngle;
            for (int i = 0; i < count; i++)
            {
                float px = (float)(center.X + Math.Cos(degInRad) * radius);
                float py = (float)(center.Y + Math.Sin(degInRad) * radius);
                list(new Vector3(px, py, center.Z));
                degInRad += rstep;
            }
        }
        public static void ForArcW(Action<Vector3> list, Vector3 center, float w, float h, float startAngle, float arcAngle)
        {
            int point_count = 32;
            float sw = w / 2;
            float sh = h / 2;
            float degree_start = CMath.AngleToRadian(startAngle);
            float degree_delta = CMath.PI_F * 2 / point_count;
            point_count++;
            for (int i = 0; i < point_count; i++)
            {
                float idegree = degree_start + i * degree_delta;
                list(new Vector3(
                    (float)(center.X + Math.Cos(idegree) * sw),
                    (float)(center.Y + Math.Sin(idegree) * sh),
                    center.Z));
            }
        }
        public static void FillArc(Color4 color, Vector3 center, float w, float h, float startAngle, float arcAngle)
        {
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color4(color);
            GL.Vertex3(center.X, center.Z, center.Y);
            ForArcW(v3 => GL.Vertex3(v3.X, v3.Z, v3.Y), center, w, h, startAngle, arcAngle);
            GL.End();
        }
        public static void DrawArc(Color4 color, Vector3 center, float w, float h, float startAngle, float arcAngle)
        {
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(color);
            ForArcW(v3 => GL.Vertex3(v3.X, v3.Z, v3.Y), center, w, h, startAngle, arcAngle);
            GL.End();
        }
        public static void DrawArc(PrimitiveType type, Color4 color, Vector3 center, float w, float h, float startAngle, float arcAngle)
        {
            GL.Begin(type);
            GL.Color4(color);
            ForArcW(v3 => { GL.Vertex3(center.X, center.Z, center.Y); GL.Vertex3(v3.X, v3.Z, v3.Y); }, center, w, h, startAngle, arcAngle);
            GL.End();
        }

        //---------------------------------------------------------------------------------------------------------------------
        public static void FillFan(Color4 color, Vector3 center, float direction, float degrees, float radius)
        {
            DrawFan(PrimitiveType.TriangleFan, color, center, direction, degrees, radius);
        }
        public static void DrawFan(Color4 color, Vector3 center, float direction, float degrees, float radius)
        {
            DrawFan(PrimitiveType.LineLoop, color, center, direction, degrees, radius);
        }
        public static void DrawFan(PrimitiveType type, Color4 color, Vector3 center, float direction, float degrees, float radius)
        {
            if (degrees != 0)
            {
                int count = (int)(16 * Math.Max(radius, 1));
                float rstep = degrees / count;
                float degInRad = direction - degrees / 2;

                GL.Begin(type);
                GL.Color4(color);
                GL.Vertex3(new Vector3(center.X, center.Z, center.Y));
                for (int i = 0; i < count; i++)
                {
                    float px = (float)(center.X + Math.Cos(degInRad) * radius);
                    float py = (float)(center.Y + Math.Sin(degInRad) * radius);
                    GL.Vertex3(new Vector3(px, center.Z, py));
                    degInRad += rstep;
                }
                GL.End();
            }
            else
            {
                DrawDirection(color, center, direction, radius);
            }
        }

        public static void DrawFan3D(Color4 color, Vector3 center, float direction, float degrees, float height, float radius)
        {
            DrawFan3D(PrimitiveType.LineLoop, color, center, direction, degrees, height, radius);
        }
        public static void DrawFan3D(PrimitiveType type, in Color4 color, in Vector3 center, float direction, float degrees, float height, float radius)
        {
            if (degrees != 0)
            {
                int count = (int)(16 * Math.Max(radius, 1));
                float rstep = degrees / count;
                float headZ = center.Z + height;
                {
                    float degInRad = direction - degrees / 2;
                    GL.Begin(type);
                    GL.Color4(color);
                    GL.Vertex3(new Vector3(center.X, headZ, center.Y));
                    for (int i = 0; i < count; i++)
                    {
                        float px = (float)(center.X + Math.Cos(degInRad) * radius);
                        float py = (float)(center.Y + Math.Sin(degInRad) * radius);
                        GL.Vertex3(new Vector3(px, headZ, py));
                        degInRad += rstep;
                    }
                    GL.End();
                }
                {
                    float degInRad = direction - degrees / 2;
                    GL.Begin(type);
                    GL.Color4(color);
                    GL.Vertex3(new Vector3(center.X, center.Z, center.Y));
                    for (int i = 0; i < count; i++)
                    {
                        float px = (float)(center.X + Math.Cos(degInRad) * radius);
                        float py = (float)(center.Y + Math.Sin(degInRad) * radius);
                        GL.Vertex3(new Vector3(px, center.Z, py));
                        degInRad += rstep;
                    }
                    GL.End();
                }
                //if (count >= 2)
                {
                    float degInRad = direction - degrees / 2;
                    GL.Begin(PrimitiveType.Lines);
                    GL.Color4(color);
                    {
                        float px = (float)(center.X + Math.Cos(degInRad) * radius);
                        float py = (float)(center.Y + Math.Sin(degInRad) * radius);
                        GL.Vertex3(new Vector3(px, center.Z, py));
                        GL.Vertex3(new Vector3(px, headZ, py));
                    }
                    degInRad = direction + degrees / 2;
                    {
                        float px = (float)(center.X + Math.Cos(degInRad) * radius);
                        float py = (float)(center.Y + Math.Sin(degInRad) * radius);
                        GL.Vertex3(new Vector3(px, center.Z, py));
                        GL.Vertex3(new Vector3(px, headZ, py));
                    }
                    GL.End();
                }
            }
            else
            {
                DrawDirection(color, center, direction, radius);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void FillStripeRect(Color4 color, float sx, float sy, float dx, float dy, float line_r, float z)
        {
            DrawStripeRect(PrimitiveType.Quads, color, sx, sy, dx, dy, line_r, z);
        }
        public static void DrawStripeRect(Color4 color, float sx, float sy, float dx, float dy, float line_r, float z)
        {
            DrawStripeRect(PrimitiveType.LineLoop, color, sx, sy, dx, dy, line_r, z);
        }
        public static void DrawStripeRect(PrimitiveType type, Color4 color, float sx, float sy, float dx, float dy, float line_r, float z)
        {
            float direction = DeepCore.Geometry.VectorHelper.GetDegree(sx, sy, dx, dy);
            Vector2 s_l = new Vector2(sx, sy);
            Vector2 s_r = new Vector2(sx, sy);
            Vector2 d_l = new Vector2(dx, dy);
            Vector2 d_r = new Vector2(dx, dy);
            DeepCore.Geometry.VectorHelper.MovePolar(ref s_l.X, ref s_l.Y, direction + CMath.PI_DIV_2, line_r);
            DeepCore.Geometry.VectorHelper.MovePolar(ref d_l.X, ref d_l.Y, direction + CMath.PI_DIV_2, line_r);
            DeepCore.Geometry.VectorHelper.MovePolar(ref s_r.X, ref s_r.Y, direction - CMath.PI_DIV_2, line_r);
            DeepCore.Geometry.VectorHelper.MovePolar(ref d_r.X, ref d_r.Y, direction - CMath.PI_DIV_2, line_r);
            GL.Begin(type);
            GL.Color4(color);
            {
                GL.Vertex3(s_l.X, z, s_l.Y);
                GL.Vertex3(s_r.X, z, s_r.Y);
                GL.Vertex3(d_r.X, z, d_r.Y);
                GL.Vertex3(d_l.X, z, d_l.Y);
            }
            GL.End();
        }
        public static void DrawStripeRect3D(Color4 color, float sx, float sy, float dx, float dy, float line_r, float z, float height)
        {
            float direction = DeepCore.Geometry.VectorHelper.GetDegree(sx, sy, dx, dy);
            Vector2 s_l = new Vector2(sx, sy);
            Vector2 s_r = new Vector2(sx, sy);
            Vector2 d_l = new Vector2(dx, dy);
            Vector2 d_r = new Vector2(dx, dy);
            DeepCore.Geometry.VectorHelper.MovePolar(ref s_l.X, ref s_l.Y, direction + CMath.PI_DIV_2, line_r);
            DeepCore.Geometry.VectorHelper.MovePolar(ref d_l.X, ref d_l.Y, direction + CMath.PI_DIV_2, line_r);
            DeepCore.Geometry.VectorHelper.MovePolar(ref s_r.X, ref s_r.Y, direction - CMath.PI_DIV_2, line_r);
            DeepCore.Geometry.VectorHelper.MovePolar(ref d_r.X, ref d_r.Y, direction - CMath.PI_DIV_2, line_r);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(color);
            {
                GL.Vertex3(s_l.X, z, s_l.Y);
                GL.Vertex3(s_r.X, z, s_r.Y);
                GL.Vertex3(d_r.X, z, d_r.Y);
                GL.Vertex3(d_l.X, z, d_l.Y);
            }
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(color);
            {
                GL.Vertex3(s_l.X, z + height, s_l.Y);
                GL.Vertex3(s_r.X, z + height, s_r.Y);
                GL.Vertex3(d_r.X, z + height, d_r.Y);
                GL.Vertex3(d_l.X, z + height, d_l.Y);
            }
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(color);
            {
                GL.Vertex3(s_l.X, z, s_l.Y);
                GL.Vertex3(s_l.X, z + height, s_l.Y);
                GL.Vertex3(s_r.X, z, s_r.Y);
                GL.Vertex3(s_r.X, z + height, s_r.Y);
                GL.Vertex3(d_l.X, z, d_l.Y);
                GL.Vertex3(d_l.X, z + height, d_l.Y);
                GL.Vertex3(d_r.X, z, d_r.Y);
                GL.Vertex3(d_r.X, z + height, d_r.Y);
            }
            GL.End();
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void DrawStripe(PrimitiveType type, Color4 color, float sx, float sy, float dx, float dy, float line_r, float z)
        {
            DrawCycle(color, new Vector3(sx, sy, z), line_r);
            DrawCycle(color, new Vector3(dx, dy, z), line_r);
            GL.Begin(type);
            GL.Color4(color);
            {
                float direction = DeepCore.Geometry.VectorHelper.GetDegree(sx, sy, dx, dy);
                Vector2 s_l = new Vector2(sx, sy);
                Vector2 s_r = new Vector2(sx, sy);
                Vector2 d_l = new Vector2(dx, dy);
                Vector2 d_r = new Vector2(dx, dy);
                DeepCore.Geometry.VectorHelper.MovePolar(ref s_l.X, ref s_l.Y, direction + CMath.PI_DIV_2, line_r);
                DeepCore.Geometry.VectorHelper.MovePolar(ref d_l.X, ref d_l.Y, direction + CMath.PI_DIV_2, line_r);
                DeepCore.Geometry.VectorHelper.MovePolar(ref s_r.X, ref s_r.Y, direction - CMath.PI_DIV_2, line_r);
                DeepCore.Geometry.VectorHelper.MovePolar(ref d_r.X, ref d_r.Y, direction - CMath.PI_DIV_2, line_r);
                //float size = line_r * 2;
                GL.Vertex3(s_l.X, z, s_l.Y);
                GL.Vertex3(s_r.X, z, s_r.Y);
                //                 float angle = CMath.RadianToAngle(direction);
                //                 VertexUtil.ForArc(new Vector2(sx - line_r, sy - line_r), angle - MathHelper.PiOver2, angle + MathHelper.PiOver2, size, v => GL.Vertex3(v.X, z, v.Y));
                GL.Vertex3(d_r.X, z, d_r.Y);
                GL.Vertex3(d_l.X, z, d_l.Y);
                //                 angle = angle + MathHelper.TwoPi;
                //                 VertexUtil.ForArc(new Vector2(sx - line_r, sy - line_r), angle - MathHelper.PiOver2, angle + MathHelper.PiOver2, size, v => GL.Vertex3(v.X, z, v.Y));
            }
            GL.End();
        }

        //---------------------------------------------------------------------------------------------------------------------

        public static void DrawStar(Color4 color, Vector3 center, float radius, float rotate = 0)
        {
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(color);
            DeepCore.Geometry.VectorDrawing.ForStar(center.ToGeometry(), v3 => GL.Vertex3(v3.X, v3.Z, v3.Y), radius, rotate);
            GL.End();
        }

        //---------------------------------------------------------------------------------------------------------------------

        public static void DrawEquilateralTrangle(PrimitiveType type, Color4 color, Vector3 center, float rotate, float radius)
        {
            var delta = MathHelper.TwoPi / 3;
            var start = rotate - MathHelper.Pi / 2;
            var v0 = new Vector3();
            var v1 = new Vector3();
            var v2 = new Vector3();
            DeepCore.Geometry.VectorHelper.MovePolar(ref v0.X, ref v0.Y, start + delta * 0, radius);
            DeepCore.Geometry.VectorHelper.MovePolar(ref v1.X, ref v1.Y, start + delta * 1, radius);
            DeepCore.Geometry.VectorHelper.MovePolar(ref v2.X, ref v2.Y, start + delta * 2, radius);
            GL.Begin(type);
            GL.Color4(color);
            {
                GL.Vertex3(center.X + v0.X, center.Z, center.Y + v0.Y);
                GL.Vertex3(center.X + v1.X, center.Z, center.Y + v1.Y);
                GL.Vertex3(center.X + v2.X, center.Z, center.Y + v2.Y);
            }
            GL.End();
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void DrawCursor(Color4 color, Vector3 p1, Vector3 p2, float width, float cursor_width, float cursor_height)
        {
            if (cursor_width < width)
                cursor_width = width;

            float ox = p1.X - p2.X;
            float oy = p1.Y - p2.Y;
            float ds = CMath.GetDistance(p1.X, p1.Y, p2.X, p2.Y);
            float hw = width / 2f;
            float cw = cursor_width / 2;
            float od = (float)Math.Atan2(oy, ox);

            var sC = new Vector3(0, p1.Z, 0);
            var sL = new Vector3(-hw, p1.Z, 0);
            var sR = new Vector3(+hw, p1.Z, 0);
            var dC = new Vector3(0, p2.Z, ds);
            var dL = new Vector3(-hw, p2.Z, ds - cursor_height);
            var dR = new Vector3(+hw, p2.Z, ds - cursor_height);
            var dLL = new Vector3(-cw, p2.Z, ds - cursor_height);
            var dRR = new Vector3(+cw, p2.Z, ds - cursor_height);
            {
                var points = new Vector3[] { sC, sL, dL, dLL, dC, dRR, dR, sR, sC };
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color4(color);
                for (int i = 0; i < points.Length; i++)
                {
                    float x = points[i].X;
                    float y = points[i].Z;
                    DeepCore.Geometry.VectorHelper.Rotate(ref x, ref y, 0, 0, od + CMath.PI_DIV_2);
                    points[i].X = x + p1.X;
                    points[i].Z = y + p1.Y;
                    GL.Vertex3(points[i]);
                }
                GL.End();
            }
        }
        public static void FillCursor(Color4 color, Vector3 p1, Vector3 p2, float width, float cursor_width, float cursor_height)
        {
            if (cursor_width < width)
                cursor_width = width;

            float ox = p1.X - p2.X;
            float oy = p1.Y - p2.Y;
            float ds = CMath.GetDistance(p1.X, p1.Y, p2.X, p2.Y);
            float hw = width / 2f;
            float cw = cursor_width / 2;
            float od = (float)Math.Atan2(oy, ox);

            var sC = new Vector3(0, p1.Z, 0);
            var sL = new Vector3(-hw, p1.Z, 0);
            var sR = new Vector3(+hw, p1.Z, 0);
            var dC = new Vector3(0, p2.Z, ds);
            var dL = new Vector3(-hw, p2.Z, ds - cursor_height);
            var dR = new Vector3(+hw, p2.Z, ds - cursor_height);
            var dLL = new Vector3(-cw, p2.Z, ds - cursor_height);
            var dRR = new Vector3(+cw, p2.Z, ds - cursor_height);
            {
                var trangle = new Vector3[] { dC, dLL, dRR, };
                GL.Begin(PrimitiveType.Polygon);
                GL.Color4(color);
                for (int i = 0; i < trangle.Length; i++)
                {
                    float x = trangle[i].X;
                    float y = trangle[i].Z;
                    DeepCore.Geometry.VectorHelper.Rotate(ref x, ref y, 0, 0, od + CMath.PI_DIV_2);
                    trangle[i].X = x + p1.X;
                    trangle[i].Z = y + p1.Y;
                    GL.Vertex3(trangle[i]);
                }
                GL.End();
            }
            {
                var rectangle = new Vector3[] { sL, sR, dR, dL, };
                GL.Begin(PrimitiveType.Polygon);
                GL.Color4(color);
                for (int i = 0; i < rectangle.Length; i++)
                {
                    float x = rectangle[i].X;
                    float y = rectangle[i].Z;
                    DeepCore.Geometry.VectorHelper.Rotate(ref x, ref y, 0, 0, od + CMath.PI_DIV_2);
                    rectangle[i].X = x + p1.X;
                    rectangle[i].Z = y + p1.Y;
                    GL.Vertex3(rectangle[i]);
                }
                GL.End();
            }
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void DrawDirection(in Color4 color, in Vector3 center, float direction, float radius)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(color);
            var px = (float)(center.X + Math.Cos(direction) * radius);
            var py = (float)(center.Y + Math.Sin(direction) * radius);
            GL.Vertex3(new Vector3(center.X, center.Z, center.Y));
            GL.Vertex3(new Vector3(px, center.Z, py));
            GL.End();
        }
        public static void DrawDirectionRect(in Color4 color, in Vector3 center, float direction, float radius)
        {
            var px = (float)(Math.Cos(direction) * radius);
            var py = (float)(Math.Sin(direction) * radius);
            DrawStripeRect(color, center.X - px, center.Y - py, center.X + px, center.Y + py, radius / 2f, center.Z);
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void DrawCross(in Color4 color, in Vector3 center, float radius)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(color);
            GL.Vertex3(center.X - radius, center.Z, center.Y);
            GL.Vertex3(center.X + radius, center.Z, center.Y);
            GL.Vertex3(center.X, center.Z, center.Y + radius);
            GL.Vertex3(center.X, center.Z, center.Y - radius);
            GL.End();
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void DrawBody3D(in Color4 color, in Vector3 center, float height, float radius)
        {
            DrawBody3D(in color, in color, in color, in center, height, radius);
        }
        public static void DrawBody3D(in Color4 headColor, in Color4 bodyColor, in Color4 footColor, in Vector3 center, float height, float radius)
        {
            int count = (int)(16 * Math.Max(radius, 1));
            float rstep = CMath.RADIANS_360 / count;
            float degInRad = 0;
            var foot = new List<Vector3>(count);
            var head = new List<Vector3>(count);
            var body = new List<Vector3>(count);
            {
                for (int i = 0; i < count; i++)
                {
                    float px = (float)(center.X + Math.Cos(degInRad) * radius);
                    float py = (float)(center.Y + Math.Sin(degInRad) * radius);
                    head.Add(new Vector3(px, center.Z + height, py));
                    foot.Add(new Vector3(px, center.Z, py));
                    body.Add(head.Last());
                    body.Add(foot.Last());
                    degInRad += rstep;
                }
                {
                    GL.Begin(PrimitiveType.TriangleFan);
                    GL.Color4(headColor);
                    GL.Vertex3(center.X, center.Z + height, center.Y);
                    for (int i = 0; i < count; i++) { GL.Vertex3(head[i]); }
                    if (head.Count > 0) GL.Vertex3(head[0]);
                    GL.End();
                }
                {
                    GL.Begin(PrimitiveType.Lines);
                    GL.Color4(bodyColor);
                    for (int i = 0; i < count; i++)
                    {
                        GL.Vertex3(body[i * 2 + 0]);
                        GL.Vertex3(body[i * 2 + 1]);
                    }
                    GL.End();
                }
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Color4(footColor);
                    for (int i = 0; i < count; i++) { GL.Vertex3(foot[i]); }
                    GL.End();
                }
            }
        }
        public static void DrawBodyMesh3D(in Color4 color, in Vector3 center, float height, float radius, int fans = 16)
        {
            int count = (int)(fans * Math.Max(radius, 1));
            float rstep = CMath.RADIANS_360 / count;
            float degInRad = 0;
            var top = center.VoxelToGL() + new Vector3(0, height, 0);
            var bot = center.VoxelToGL();
            var foot = new List<Vector3>(count);
            var head = new List<Vector3>(count);
            var body = new List<Vector3>(count);
            {
                for (int i = 0; i < count; i++)
                {
                    float px = (float)(center.X + Math.Cos(degInRad) * radius);
                    float py = (float)(center.Y + Math.Sin(degInRad) * radius);
                    head.Add(new Vector3(px, center.Z + height, py));
                    foot.Add(new Vector3(px, center.Z, py));
                    body.Add(head.Last());
                    body.Add(foot.Last());
                    degInRad += rstep;
                }
                //                 {
                //                     GL.Begin(PrimitiveType.TriangleFan);
                //                     GL.Color4(headColor);
                //                     GL.Vertex3(center.X, center.Z + height, center.Y);
                //                     for (int i = 0; i < count; i++) { GL.Vertex3(head[i]); }
                //                     if (head.Count > 0) GL.Vertex3(head[0]);
                //                     GL.End();
                //                 }
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Color4(color);
                    for (int i = 0; i < count; i++) { GL.Vertex3(head[i]); }
                    GL.End();
                }
                {
                    GL.Begin(PrimitiveType.Lines);
                    GL.Color4(color);
                    for (int i = 0; i < count; i++)
                    {
                        GL.Vertex3(top);
                        GL.Vertex3(body[i * 2 + 0]);

                        GL.Vertex3(body[i * 2 + 0]);
                        GL.Vertex3(body[i * 2 + 1]);

                        GL.Vertex3(body[i * 2 + 1]);
                        GL.Vertex3(bot);
                    }
                    GL.End();
                }
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Color4(color);
                    for (int i = 0; i < count; i++) { GL.Vertex3(foot[i]); }
                    GL.End();
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void DrawSphere3D(Color4 color, Vector3 center, float radius)
        {
            int count = (int)(16 * Math.Max(radius, 1));
            float rstep = CMath.RADIANS_360 / count;
            float ladeg = 0, lodeg = 0;
            for (int i = 0; i < count; i++)
            {
                //latitude纬度//
                {
                    float pw = (float)Math.Sin(ladeg) * radius;
                    float ph = (float)Math.Cos(ladeg) * radius;
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Color4(color);
                    lodeg = 0;
                    for (int j = 0; j < count; j++)
                    {
                        float px = (float)(center.X + Math.Cos(lodeg) * pw);
                        float py = (float)(center.Y + Math.Sin(lodeg) * pw);
                        GL.Vertex3(new Vector3(px, center.Z + ph, py));
                        lodeg += rstep;
                    }
                    GL.End();
                }
                //longitude经度//
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Color4(color);
                    lodeg = 0;
                    for (int j = 0; j < count; j++)
                    {
                        float r = (float)(Math.Cos(lodeg) * radius);
                        float py = (float)(Math.Sin(lodeg) * radius);
                        float px = (float)(Math.Cos(ladeg) * r);
                        float pz = (float)(Math.Sin(ladeg) * r);
                        GL.Vertex3(center.X + px, center.Z + py, center.Y + pz);
                        lodeg += rstep;
                    }
                    GL.End();
                }
                ladeg += rstep;
            }
        }
        //---------------------------------------------------------------------------------------------------------------------
        public static void DrawHightZ(Color4 color, Vector3 pos)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(color);
            GL.Vertex3(pos.X, pos.Z, pos.Y);
            GL.Vertex3(pos.X, 0, pos.Y);
            GL.End();
        }
        public static void DrawHightZ(Color4 color, Vector3 pos, float height)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(color);
            GL.Vertex3(pos.X, pos.Z, pos.Y);
            GL.Vertex3(pos.X, pos.Z + height, pos.Y);
            GL.End();
        }
        public static void DrawLine(Color4 color, Vector3 pos1, Vector3 pos2)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(color);
            GL.Vertex3(pos1.X, pos1.Z, pos1.Y);
            GL.Vertex3(pos2.X, pos2.Z, pos2.Y);
            GL.End();
        }
        //---------------------------------------------------------------------------------------------------------------------
        /*
        public static void DrawAttackShape(Color4 color, AttackShape shape, Vector3 pos, Vector3 startPos,
            float bodyHeight, DeepCore.GameData.Zone.VoxelAnchor anchor,
            float direction, float size, float distance, float fan_angle, float strip_wide,
            DeepCore.Geometry.VoxelCylinder? target = null)
        {
            switch (anchor)
            {
                case VoxelAnchor.Flooring:
                    if (bodyHeight != 0)
                    {
                        DrawHightZ(color, pos, bodyHeight);
                    }
                    break;
                case VoxelAnchor.Floating:
                    if (bodyHeight != 0)
                    {
                        DrawHightZ(color, pos + new Vector3(0, 0, -bodyHeight / 2), bodyHeight);
                    }
                    break;
                case VoxelAnchor.Ceiling:
                    if (bodyHeight != 0)
                    {
                        DrawHightZ(color, pos, -bodyHeight);
                    }
                    break;
            }
            switch (shape)
            {
                case AttackShape.Round:
                    DrawCycle(color, pos, size);
                    //g.DrawEllipse(pen, new RectangleF(-size, -size, size * 2, size * 2));
                    break;
                case AttackShape.Circle:
                    DrawCycle(color, pos, size);
                    //g.DrawEllipse(pen, new RectangleF(-size, -size, size * 2, size * 2));
                    float sr = size - strip_wide;
                    DrawCycle(color, pos, sr);
                    //g.DrawEllipse(pen, new RectangleF(-sr, -sr, sr * 2, sr * 2));
                    break;
                case AttackShape.Fan:
                    //g.DrawFan(pen, direction, size, angle);
                    DrawFan(color, pos, direction, fan_angle, size);
                    break;
                case AttackShape.Strip:
                    {
                        float d_width = strip_wide / 2f;
                        float d_distance = distance / 2f;
                        var p0 = pos;
                        var p1 = pos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p0.X, ref p0.Y, direction, -d_distance);
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, direction, +d_distance);
                        DrawStripe(PrimitiveType.LineLoop, color, p0.X, p0.Y, p1.X, p1.Y, d_width, pos.Z);
                    }
                    break;
                case AttackShape.StripRay:
                case AttackShape.StripRayTouchEnd:
                    {
                        float d_width = strip_wide / 2f;
                        var p1 = pos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, direction, distance);
                        DrawStripe(PrimitiveType.LineLoop, color, pos.X, pos.Y, p1.X, p1.Y, d_width, pos.Z);
                    }
                    break;
                case AttackShape.RectStrip:
                    {
                        float d_width = strip_wide / 2f;
                        float d_distance = distance / 2f;
                        var p0 = pos;
                        var p1 = pos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p0.X, ref p0.Y, direction, -d_distance);
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, direction, +d_distance);
                        DrawStripeRect(PrimitiveType.LineLoop, color, p0.X, p0.Y, p1.X, p1.Y, d_width, pos.Z);
                    }
                    break;
                case AttackShape.RectStripRay:
                    {
                        float d_width = strip_wide / 2f;
                        var p1 = pos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, direction, distance);
                        DrawStripeRect(PrimitiveType.LineLoop, color, pos.X, pos.Y, p1.X, p1.Y, d_width, pos.Z);
                    }
                    break;
                case AttackShape.WideStrip:
                    {
                        float d_width = strip_wide / 2f;
                        float d_angle = direction + CMath.PI_DIV_2;
                        float d_distance = distance / 2f;
                        var p0 = pos;
                        var p1 = pos;
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p0.X, ref p1.Y, d_angle, -d_width);
                        DeepCore.Geometry.VectorHelper.MovePolar(ref p1.X, ref p1.Y, d_angle, +d_width);
                        DrawStripe(PrimitiveType.LineLoop, color, p0.X, p0.Y, p1.X, p1.Y, d_width, pos.Z);
                    }
                    break;
                case AttackShape.LineToTarget:
                    if (target != null)
                    {
                        var p1 = pos;
                        var p2 = target.Value.Center + new DeepCore.Geometry.Vector3(0, 0, target.Value.Height / 2);
                        DrawCursor(PrimitiveType.LineLoop, color, p1, p2.ToGL(), strip_wide / 2, strip_wide, strip_wide);
                    }
                    break;
                case AttackShape.LineToStart:
                    {
                        var p1 = pos;
                        var p2 = startPos;
                        DrawCursor(PrimitiveType.LineLoop, color, p1, p2, strip_wide / 2, strip_wide, strip_wide);
                    }
                    break;
            }

        }
        */
        public static void DrawWayPoint(Color4 lineColor, Color4 quardsColor, ITerrainWayPoint wp, float gridSize, Vector3 offset)
        {
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color4(lineColor);
            foreach (var p in wp)
            {
                var pos = p.Position.ToGL() + offset + new Vector3(0, 0, 0.1f);
                GL.Vertex3(pos.X, pos.Z, pos.Y);
            }
            GL.End();
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(quardsColor);
            foreach (var p in wp)
            {
                var pos = p.Position.ToGL() + offset + new Vector3(0, 0, 0.1f);
                var bx = (int)(p.Position.X / gridSize);
                var by = (int)(p.Position.Y / gridSize);
                var range = new DeepCore.Geometry.Rectangle(bx, by, 1, 1);
                var rect = range.GetRangeSize(gridSize);
                GL.Vertex3(rect.X/*         */, pos.Z, rect.Y);
                GL.Vertex3(rect.X + rect.Width, pos.Z, rect.Y);
                GL.Vertex3(rect.X + rect.Width, pos.Z, rect.Y + rect.Height);
                GL.Vertex3(rect.X/*         */, pos.Z, rect.Y + rect.Height);
                //                 var pos = new Vector3(p.Position.X * gridSize, p.Position.Y * gridSize, p.Position.Z) + offset;
                //                 GL.Vertex3(pos.X, pos.Z, pos.Y);
                //                 GL.Vertex3(pos.X + gridSize, pos.Z, pos.Y);
                //                 GL.Vertex3(pos.X + gridSize, pos.Z, pos.Y + gridSize);
                //                 GL.Vertex3(pos.X, pos.Z, pos.Y + gridSize);
            }
            GL.End();
        }

        public static void DrawBoundingBox(Color4 color, DeepCore.Geometry.BoundingBox box)
        {
            DrawBoundingBox(color, box.Min.ToGL(), box.Max.ToGL());
        }
        public static void DrawBoundingBox(Color4 color, Vector3 min, Vector3 max)
        {
            GL.Color4(color);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(min.X, min.Z, min.Y);
            GL.Vertex3(max.X, min.Z, min.Y);
            GL.Vertex3(max.X, min.Z, max.Y);
            GL.Vertex3(min.X, min.Z, max.Y);
            GL.End();

            GL.Color4(color);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(min.X, max.Z, min.Y);
            GL.Vertex3(max.X, max.Z, min.Y);
            GL.Vertex3(max.X, max.Z, max.Y);
            GL.Vertex3(min.X, max.Z, max.Y);
            GL.End();

            GL.Color4(color);
            GL.Begin(PrimitiveType.Lines);

            GL.Vertex3(min.X, min.Z, min.Y);
            GL.Vertex3(min.X, max.Z, min.Y);

            GL.Vertex3(max.X, min.Z, min.Y);
            GL.Vertex3(max.X, max.Z, min.Y);

            GL.Vertex3(max.X, min.Z, max.Y);
            GL.Vertex3(max.X, max.Z, max.Y);

            GL.Vertex3(min.X, min.Z, max.Y);
            GL.Vertex3(min.X, max.Z, max.Y);

            GL.End();
        }


        public static void FillBoundingBox(Color4 color, DeepCore.Geometry.BoundingBox box)
        {
            FillBoundingBox(color, box.Min.ToGL(), box.Max.ToGL());
        }
        public static void FillBoundingBox(Color4 color, Vector3 min, Vector3 max)
        {
            // TOP
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color);
            GL.Vertex3(min.X, max.Z, min.Y);
            GL.Vertex3(max.X, max.Z, min.Y);
            GL.Vertex3(max.X, max.Z, max.Y);
            GL.Vertex3(min.X, max.Z, max.Y);
            GL.End();

            // Bottom
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color.Mul(0.4f));
            GL.Vertex3(min.X, min.Z, min.Y);
            GL.Vertex3(max.X, min.Z, min.Y);
            GL.Vertex3(max.X, min.Z, max.Y);
            GL.Vertex3(min.X, min.Z, max.Y);
            GL.End();

            // FORTH
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color.Mul(0.8f));
            GL.Vertex3(min.X, min.Z, max.Y);
            GL.Vertex3(max.X, min.Z, max.Y);
            GL.Vertex3(max.X, max.Z, max.Y);
            GL.Vertex3(min.X, max.Z, max.Y);
            GL.End();
            // BACK
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color.Mul(0.8f));
            GL.Vertex3(min.X, min.Z, min.Y);
            GL.Vertex3(max.X, min.Z, min.Y);
            GL.Vertex3(max.X, max.Z, min.Y);
            GL.Vertex3(min.X, max.Z, min.Y);
            GL.End();

            // LEFT
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color.Mul(0.6f));
            GL.Vertex3(min.X, min.Z, min.Y);
            GL.Vertex3(min.X, min.Z, max.Y);
            GL.Vertex3(min.X, max.Z, max.Y);
            GL.Vertex3(min.X, max.Z, min.Y);
            GL.End();
            // RIGHT
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color.Mul(0.6f));
            GL.Vertex3(max.X, min.Z, min.Y);
            GL.Vertex3(max.X, min.Z, max.Y);
            GL.Vertex3(max.X, max.Z, max.Y);
            GL.Vertex3(max.X, max.Z, min.Y);
            GL.End();
        }
        public static void DrawVoxel(Color4 color, VoxelLayer layer)
        {
            var box = layer.GetBlockBoundingBox();
            var min = box.Min.ToGL();
            var max = box.Max.ToGL();
            DrawBoundingBox(color, min, max);
        }
        public static void FillVoxel(Color4 color, VoxelLayer layer)
        {
            var box = layer.GetBlockBoundingBox();
            var min = box.Min.ToGL();
            var max = box.Max.ToGL();
            FillBoundingBox(color, min, max);
        }

        /*
        public static void DrawZoneShape(Color4 color, Vector3 pos, IZoneShape shape)
        {
            if (shape is ShapeRound round)
            {
                DrawingObject.DrawCycle(color, pos, round.r);
            }
            else if (shape is ShapeRect rect)
            {
                DrawingObject.DrawRect(color, pos, rect.w, rect.h);
            }
            else if (shape is ShapeStripWidth strip)
            {
                DrawingObject.DrawStripeRect(color, strip.sx, strip.sy, strip.dx, strip.dy, strip.r_wide, pos.Z);
            }
        }
        */
        public static void DrawBounds(Color4 color, float x, float y, float w, float h, float grid)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(color);
            GL.Vertex3(new Vector3(x - grid, 0, y)); GL.Vertex3(new Vector3(x + grid + w, 0, y));
            GL.Vertex3(new Vector3(x, 0, y - grid)); GL.Vertex3(new Vector3(x, 0, y + grid + h));
            GL.Vertex3(new Vector3(x + w, 0, y + h)); GL.Vertex3(new Vector3(x, 0, y + h));
            GL.Vertex3(new Vector3(x + w, 0, y + h)); GL.Vertex3(new Vector3(x + w, 0, y));
            GL.End();
        }

        public static void DrawGridLines(Color4 color, float startX, float startY, float gridW, float gridH, int xcount, int ycount)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(color);
            var tw = xcount * gridW;
            var th = ycount * gridH;
            for (var x = 0; x <= xcount; x++)
            {
                float dx = startX + x * gridW;
                GL.Vertex3(dx, 0.5f, startY);
                GL.Vertex3(dx, 0.5f, startY + th);
            }
            for (var y = 0; y <= ycount; y++)
            {
                float dy = startY + y * gridH;
                GL.Vertex3(startX, 0.5f, dy);
                GL.Vertex3(startX + tw, 0.5f, dy);
            }
            GL.End();
        }


        public static void DrawBezier(DeepCore.Geometry.IBezierCurve bezier, Color4 color)
        {
            // 三次贝塞尔示例
            var points = bezier.Sample(100);
            // 绘制曲线
            for (int i = 0; i < points.Count - 1; i++)
            {
                var p1 = points[i].ToGL();
                var p2 = points[i + 1].ToGL();
                DrawLine(color, (p1), (p2));
            }
        }
        public static void VertexPlane2DQuard(float dx, float dy, float dz, float cw, float ch)
        {
            GL.Vertex3(dx,/* */ dz, dy/* */);
            GL.Vertex3(dx + cw, dz, dy/* */);
            GL.Vertex3(dx + cw, dz, dy + ch);
            GL.Vertex3(dx,/* */ dz, dy + ch);
        }

    }
    public static class DrawingHUD
    {
        public static void DrawRect(PrimitiveType type, Color4 color, float x, float y, float w, float h)
        {
            GL.Begin(type);
            GL.Color4(color);
            GL.Vertex2(x, y);
            GL.Vertex2(x + w, y);
            GL.Vertex2(x + w, y + h);
            GL.Vertex2(x, y + h);
            GL.End();
        }
        public static void DrawGauge(Color4 color, Color4 back, float percent, float x, float y, float w, float h, int border = 1)
        {
            float gx = x + border, gy = y + border, gw = w - border * 2, gh = h - border * 2;
            DrawRect(PrimitiveType.Quads, back, x, y, w, h);
            DrawRect(PrimitiveType.Quads, color, gx, gy, gw * percent / 100f, gh);
        }
        public static void DrawGaugeFan(Color4 color, Color4 back, float percent, float x, float y, float r, float border = 1)
        {
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color4(back);
            DrawingVoxelObject.ForArc(v => GL.Vertex2(v.X, v.Y), new Vector3(x, 0, y), 0, MathHelper.TwoPi, r);
            GL.End();
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color4(color);
            DrawingVoxelObject.ForArc(v => GL.Vertex2(v.X, v.Y), new Vector3(x, 0, y), 0, MathHelper.TwoPi * percent / 100f, r - border);
            GL.End();
        }
        public static void FillCycle(Color4 color, Vector2 center, float radius)
        {
            DrawCycle(PrimitiveType.TriangleFan, color, center, radius);
        }
        public static void DrawCycle(Color4 color, Vector2 center, float radius)
        {
            DrawCycle(PrimitiveType.LineLoop, color, center, radius);
        }
        public static void DrawCycle(PrimitiveType type, Color4 color, Vector2 center, float radius)
        {
            int count = (int)(16 * Math.Max(radius, 1));
            float rstep = CMath.RADIANS_360 / count;
            float degInRad = 0;
            GL.Begin(type);
            GL.Color4(color);
            for (int i = 0; i < count; i++)
            {
                float px = (float)(center.X + Math.Cos(degInRad) * radius);
                float py = (float)(center.Y + Math.Sin(degInRad) * radius);
                GL.Vertex2(new Vector2(px, py));
                degInRad += rstep;
            }
            GL.End();
        }

        public static void DrawLine(Color4 color, Vector2 a, Vector2 b)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(color);
            GL.Vertex2(a);
            GL.Vertex2(b);
            GL.End();
        }

    }
    public enum BoxFlag
    {
        TOP = 0x01,
        BOTTOM = 0x02,
        FORTH = 0x04,
        BACK = 0x08,
        LEFT = 0x10,
        RIGHT = 0x20,
    }
    public static class DrawingObjectVertexArray
    {
        public static void AddLine(this VertexArrayObject buff, Vector3 src, Vector3 dst)
        {
            buff.Add(src);
            buff.Add(dst);
        }
        public static void Add2D(this VertexArrayObject buff, Vector3 vec)
        {
            buff.Add(vec.X, vec.Z, vec.Y);
        }
        public static void AddPlane2D(this VertexArrayObject buff, Vector2 min, Vector2 max, float z)
        {
            buff.Add(new Vector3(min.X, z, min.Y));
            buff.Add(new Vector3(max.X, z, min.Y));
            buff.Add(new Vector3(max.X, z, max.Y));
            buff.Add(new Vector3(min.X, z, max.Y));
        }
        public static void AddPlaneLines2D(this VertexArrayObject buff, Vector2 min, Vector2 max, float z)
        {
            buff.Add(new Vector3(min.X, z, min.Y));
            buff.Add(new Vector3(max.X, z, min.Y));
            buff.Add(new Vector3(max.X, z, min.Y));
            buff.Add(new Vector3(max.X, z, max.Y));
            buff.Add(new Vector3(max.X, z, max.Y));
            buff.Add(new Vector3(min.X, z, max.Y));
            buff.Add(new Vector3(min.X, z, max.Y));
            buff.Add(new Vector3(min.X, z, min.Y));
        }
        public static void AddBox2DShadow(this VertexArrayObject buff, Vector3 min, Vector3 max, Color4 color)
        {

            buff.SetColor(color.Mul(1.0f)); buff.AddBox2D(min, max, BoxFlag.TOP);
            buff.SetColor(color.Mul(0.4f)); buff.AddBox2D(min, max, BoxFlag.BOTTOM);
            buff.SetColor(color.Mul(0.8f)); buff.AddBox2D(min, max, BoxFlag.FORTH);
            buff.SetColor(color.Mul(0.8f)); buff.AddBox2D(min, max, BoxFlag.BACK);
            buff.SetColor(color.Mul(0.6f)); buff.AddBox2D(min, max, BoxFlag.LEFT);
            buff.SetColor(color.Mul(0.6f)); buff.AddBox2D(min, max, BoxFlag.RIGHT);
        }

        public static void AddBox2D(this VertexArrayObject buff, Vector3 min, Vector3 max)
        {
            AddBox2D(buff, min, max, (BoxFlag)0xFFFF);
        }
        public static void AddBox2D(this VertexArrayObject buff, Vector3 min, Vector3 max, BoxFlag flag)
        {
            // FORTH              
            if ((flag & BoxFlag.FORTH) != 0)
            {
                buff.SetNormal(new Vector3(0, 0, 1));
                buff.SetTextureCoords(new Vector2(0.0f, 0.0f)); buff.Add(new Vector3(min.X, min.Z, max.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 0.0f)); buff.Add(new Vector3(max.X, min.Z, max.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 1.0f)); buff.Add(new Vector3(max.X, max.Z, max.Y));
                buff.SetTextureCoords(new Vector2(0.0f, 1.0f)); buff.Add(new Vector3(min.X, max.Z, max.Y));
            }
            // BACK                   
            if ((flag & BoxFlag.BACK) != 0)
            {
                buff.SetNormal(new Vector3(0, 0, -1));
                buff.SetTextureCoords(new Vector2(0.0f, 0.0f)); buff.Add(new Vector3(min.X, min.Z, min.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 0.0f)); buff.Add(new Vector3(max.X, min.Z, min.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 1.0f)); buff.Add(new Vector3(max.X, max.Z, min.Y));
                buff.SetTextureCoords(new Vector2(0.0f, 1.0f)); buff.Add(new Vector3(min.X, max.Z, min.Y));
            }
            // LEFT            
            if ((flag & BoxFlag.LEFT) != 0)
            {
                buff.SetNormal(new Vector3(-1, 0, 0));
                buff.SetTextureCoords(new Vector2(0.0f, 0.0f)); buff.Add(new Vector3(min.X, min.Z, min.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 0.0f)); buff.Add(new Vector3(min.X, max.Z, min.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 1.0f)); buff.Add(new Vector3(min.X, max.Z, max.Y));
                buff.SetTextureCoords(new Vector2(0.0f, 1.0f)); buff.Add(new Vector3(min.X, min.Z, max.Y));
            }
            // RIGHT                                 
            if ((flag & BoxFlag.RIGHT) != 0)
            {
                buff.SetNormal(new Vector3(1, 0, 0));
                buff.SetTextureCoords(new Vector2(0.0f, 0.0f)); buff.Add(new Vector3(max.X, min.Z, min.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 0.0f)); buff.Add(new Vector3(max.X, max.Z, min.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 1.0f)); buff.Add(new Vector3(max.X, max.Z, max.Y));
                buff.SetTextureCoords(new Vector2(0.0f, 1.0f)); buff.Add(new Vector3(max.X, min.Z, max.Y));
            }
            // TOP
            if ((flag & BoxFlag.TOP) != 0)
            {
                buff.SetNormal(new Vector3(0, 1, 0));
                buff.SetTextureCoords(new Vector2(0.0f, 0.0f)); buff.Add(new Vector3(min.X, max.Z, min.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 0.0f)); buff.Add(new Vector3(max.X, max.Z, min.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 1.0f)); buff.Add(new Vector3(max.X, max.Z, max.Y));
                buff.SetTextureCoords(new Vector2(0.0f, 1.0f)); buff.Add(new Vector3(min.X, max.Z, max.Y));
            }
            // Bottom
            if ((flag & BoxFlag.BOTTOM) != 0)
            {
                buff.SetNormal(new Vector3(0, -1, 0));
                buff.SetTextureCoords(new Vector2(0.0f, 0.0f)); buff.Add(new Vector3(min.X, min.Z, min.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 0.0f)); buff.Add(new Vector3(max.X, min.Z, min.Y));
                buff.SetTextureCoords(new Vector2(1.0f, 1.0f)); buff.Add(new Vector3(max.X, min.Z, max.Y));
                buff.SetTextureCoords(new Vector2(0.0f, 1.0f)); buff.Add(new Vector3(min.X, min.Z, max.Y));
            }
        }

    }

    public class CubeMeshCombine<T> : Disposable
    {
        public struct Cube
        {
            public int X, Y, Z;
            public T ColorIndex;
        }
        private int totalCount = 0;
        private HashMap<int, HashMap<int, HashMap<int, Cube>>> mapX = new HashMap<int, HashMap<int, HashMap<int, Cube>>>();
        public int TotalCount { get => totalCount; }
        protected override void Disposing()
        {
            mapX.Clear();
            totalCount = 0;
        }
        public void AddCube(int x, int y, int z, T color)
        {
            var mapY = mapX.GetOrAdd(x, xx => new HashMap<int, HashMap<int, Cube>>());
            var mapZ = mapY.GetOrAdd(y, zz => new HashMap<int, Cube>());
            mapZ.TryAdd(z, new Cube() { X = x, Y = y, Z = z, ColorIndex = color });
            totalCount++;
        }
        public bool RemoveCube(int x, int y, int z, out Cube cube)
        {
            if (mapX.TryGetValue(x, out var mapY))
            {
                if (mapY.TryGetValue(y, out var mapZ))
                {
                    if (mapZ.TryRemove(z, out cube))
                    {
                        totalCount--;
                        return true;
                    }
                }
            }
            cube = default(Cube);
            return false;
        }
        public bool TryGetCube(int x, int y, int z, out Cube cube)
        {
            if (mapX.TryGetValue(x, out var mapY))
            {
                if (mapY.TryGetValue(y, out var mapZ))
                {
                    if (mapZ.TryGetValue(z, out cube))
                    {
                        return true;
                    }
                }
            }
            cube = default(Cube);
            return false;
        }
        public void ForEachCubes(Action<Cube> action)
        {
            foreach (var mapY in mapX.Values)
            {
                foreach (var mapZ in mapY.Values)
                {
                    foreach (var cube in mapZ.Values)
                    {
                        action(cube);
                    }
                }
            }
        }
        public int Combine()
        {
            var oldCount = TotalCount;
            var array = ToArray();
            var removing = new List<Cube>();
            foreach (var c in array)
            {
                if (TryGetCube(c.X, c.Y - 1, c.Z, out var _top) &&
                    TryGetCube(c.X, c.Y + 1, c.Z, out var _bottom) &&
                    TryGetCube(c.X - 1, c.Y, c.Z, out var _left) &&
                    TryGetCube(c.X + 1, c.Y, c.Z, out var _right) &&
                    TryGetCube(c.X, c.Y, c.Z - 1, out var _fort) &&
                    TryGetCube(c.X, c.Y, c.Z + 1, out var _back))
                {
                    removing.Add(c);
                }
            }
            foreach (var r in removing)
            {
                RemoveCube(r.X, r.Y, r.Z, out var obj);
            }
            var newCount = TotalCount;
            return oldCount - newCount;
        }
        public Cube[] ToArray()
        {
            var array = new List<Cube>();
            ForEachCubes(c => array.Add(c));
            return array.ToArray();
        }
    }
}
