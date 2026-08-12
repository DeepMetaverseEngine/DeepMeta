using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepEditor.Common.G3D
{
    public static class GeometryUtils
    {
        public static OpenTK.Mathematics.Vector3 ToGL(this DeepCore.Geometry.Vector3 value)
        {
            return new OpenTK.Mathematics.Vector3(value.X, value.Y, value.Z);
        }
        public static DeepCore.Geometry.Vector3 ToGeometry(this OpenTK.Mathematics.Vector3 value)
        {
            return new DeepCore.Geometry.Vector3(value.X, value.Y, value.Z);
        }
        public static OpenTK.Mathematics.Vector2 ToGL(this DeepCore.Geometry.Vector2 value)
        {
            return new OpenTK.Mathematics.Vector2(value.X, value.Y);
        }
        public static DeepCore.Geometry.Vector2 ToGeometry(this OpenTK.Mathematics.Vector2 value)
        {
            return new DeepCore.Geometry.Vector2(value.X, value.Y);
        }
        public static Vector3 MoveTowards(this Vector3 current, Vector3 target, float maxDistanceDelta, bool limit = true)
        {
            float num = target.X - current.X;
            float num2 = target.Y - current.Y;
            float num3 = target.Z - current.Z;
            float num4 = num * num + num2 * num2 + num3 * num3;
            if (num4 == 0f)
            {
                return target;
            }
            if (limit && (maxDistanceDelta >= 0f && num4 <= maxDistanceDelta * maxDistanceDelta))
            {
                return target;
            }
            float num5 = (float)Math.Sqrt(num4);
            return new Vector3(current.X + num / num5 * maxDistanceDelta, current.Y + num2 / num5 * maxDistanceDelta, current.Z + num3 / num5 * maxDistanceDelta);
        }
        public static Vector2 MoveTowards(this Vector2 current, Vector2 target, float maxDistanceDelta, bool limit = true)
        {
            float num = target.X - current.X;
            float num2 = target.Y - current.Y;
            float num4 = num * num + num2 * num2;
            if (num4 == 0f)
            {
                return target;
            }
            if (limit && (maxDistanceDelta >= 0f && num4 <= maxDistanceDelta * maxDistanceDelta))
            {
                return target;
            }
            float num5 = (float)Math.Sqrt(num4);
            return new Vector2(current.X + num / num5 * maxDistanceDelta, current.Y + num2 / num5 * maxDistanceDelta);
        }
    }
    public static class GLUtils
    {
        public static Color4 Argb2Color4(int argb)
        {
            Color4 ret = Color4.Transparent;
            ret.A = ((argb & 0xFF000000L) >> 24) / 255.0f;
            ret.R = ((argb & 0x00FF0000) >> 16) / 255.0f;
            ret.G = ((argb & 0x0000FF00) >> 8) / 255.0f;
            ret.B = ((argb & 0x000000FF) >> 0) / 255.0f;
            return ret;
        }
        public static Color4 Argb2Color4(uint argb)
        {
            Color4 ret = Color4.Transparent;
            ret.A = ((argb & 0xFF000000L) >> 24) / 255.0f;
            ret.R = ((argb & 0x00FF0000) >> 16) / 255.0f;
            ret.G = ((argb & 0x0000FF00) >> 8) / 255.0f;
            ret.B = ((argb & 0x000000FF) >> 0) / 255.0f;
            return ret;
        }
        public static Color4 Argb2Color4(byte a, byte r, byte g, byte b)
        {
            return new Color4(r, g, b, a);
        }
        public static Color4 SetAlpha(this Color4 src, float alpha)
        {
            var a = src;
            a.A = alpha;
            return a;
        }
        public static Color4 Mul(this Color4 src, float dark)
        {
            var c = src;
            c.R *= dark;
            c.G *= dark;
            c.B *= dark;
            return c;
        }
        public static Color4 Add(this Color4 src, float value)
        {
            var c = src;
            c.R += value;
            c.G += value;
            c.B += value;
            return c;
        }
    }
    public static class Glu
    {
        public static Vector2 To2D(this Vector3 src)
        {
            return new Vector2(src.X, src.Y);
        }
        public static Vector3 To3D(this Vector4 src)
        {
            return new Vector3(src.X, src.Y, src.Z);
        }
        /// <summary>
        /// 世界到屏幕
        /// </summary>
        /// <param name="objPos"></param>
        /// <param name="matWorldViewProjection"></param>
        /// <param name="viewport"></param>
        /// <param name="screenPos"></param>
        /// <returns></returns>
        public static Vector3 Project(Vector3 objPos, Matrix4 matWorldView, Matrix4 matProjection, Rectangle viewport)
        {
            try
            {
                OpenTK.Mathematics.Vector4 _in;
                _in.X = objPos.X;
                _in.Y = objPos.Y;
                _in.Z = objPos.Z;
                _in.W = 1f;
                Vector3 screenPos;
                //Vector4 _out = OpenTK.Mathematics.Vector4.Transform(_in, matWorldView * matProjection);
                Vector4 _out = OpenTK.Mathematics.Vector4.TransformRow(_in, matWorldView * matProjection);
                if (_out.W <= 0.0)
                {
                    screenPos = OpenTK.Mathematics.Vector3.Zero;
                    return screenPos;
                }
                _out.X /= _out.W;
                _out.Y /= _out.W;
                _out.Z /= _out.W;
                /* Map x, y and z to range 0-1 */
                _out.X = _out.X * 0.5f + 0.5f;
                _out.Y = -_out.Y * 0.5f + 0.5f;
                _out.Z = _out.Z * 0.5f + 0.5f;
                /* Map x,y to viewport */
                _out.X = _out.X * viewport.Width + viewport.X;
                _out.Y = _out.Y * viewport.Height + viewport.Y;

                screenPos.X = _out.X;
                screenPos.Y = _out.Y;
                screenPos.Z = _out.Z;
                return screenPos;
            }
            catch
            {
                return new Vector3(0, 0, 0);
            }
        }
        public static Vector3 UnProject(Vector3 screen, Matrix4 matWorldView, Matrix4 matProjection, Rectangle viewport)
        {
            Vector4 pos = new Vector4();
            try
            {
                // Map x and y from window coordinates, map to range -1 to 1 
                pos.X = (screen.X - (float)viewport.X) / (float)viewport.Width * 2.0f - 1.0f;
                pos.Y = 1 - (screen.Y - (float)viewport.Y) / (float)viewport.Height * 2.0f;
                pos.Z = screen.Z * 2.0f - 1.0f;
                pos.W = 1.0f;

                //Vector4 pos2 = Vector4.Transform(pos, Matrix4.Invert(matWorldView * matProjection));
                Vector4 pos2 = Vector4.TransformRow(pos, Matrix4.Invert(matWorldView * matProjection));
                Vector3 pos_out = new Vector3(pos2.X, pos2.Y, pos2.Z);

                return pos_out / pos2.W;
            }
            catch
            {
                return new Vector3(0, 0, 0);
            }
        }
        public static Vector3 Project(Vector3 objPos)
        {
            int[] viewport = new int[4];
            Matrix4 modelViewMatrix, projectionMatrix;
            GL.GetFloat(GetPName.ModelviewMatrix, out modelViewMatrix);
            GL.GetFloat(GetPName.ProjectionMatrix, out projectionMatrix);
            GL.GetInteger(GetPName.Viewport, viewport);
            return Project(objPos, modelViewMatrix, projectionMatrix, new Rectangle(viewport[0], viewport[1], viewport[2], viewport[3]));
        }
        public static Vector3 UnProject(Vector3 screen)
        {
            int[] viewport = new int[4];
            Matrix4 modelViewMatrix, projectionMatrix;
            GL.GetFloat(GetPName.ModelviewMatrix, out modelViewMatrix);
            GL.GetFloat(GetPName.ProjectionMatrix, out projectionMatrix);
            GL.GetInteger(GetPName.Viewport, viewport);
            return UnProject(screen, modelViewMatrix, projectionMatrix, new Rectangle(viewport[0], viewport[1], viewport[2], viewport[3]));
        }
        public static Vector3 ScreenPointToOrgin(Vector2 mouseLocation, Matrix4 matWorldView, Matrix4 matProjection, Rectangle viewport)
        {
            var near = UnProject(new Vector3(mouseLocation.X, mouseLocation.Y, 0), matWorldView, matProjection, viewport);
            var origin = near;
            return new Vector3(origin.X, origin.Y, origin.Z);
        }
        public static Ray ScreenPointToRay(Vector2 mouseLocation, Matrix4 matWorldView, Matrix4 matProjection, Rectangle viewport, float zfar = 1f)
        {
            var near = UnProject(new Vector3(mouseLocation.X, mouseLocation.Y, 0), matWorldView, matProjection, viewport);
            var far = UnProject(new Vector3(mouseLocation.X, mouseLocation.Y, zfar), matWorldView, matProjection, viewport);
            var origin = near;
            var direction = (far - near).Normalized();
            return new Ray()
            {
                center = new Vector3(origin.X, origin.Y, origin.Z),
                normal = new Vector3(direction.X, direction.Y, direction.Z)
            };
        }
        public static Ray ScreenPointToRay(Vector2 mouseLocation, float zfar = 1f)
        {
            var near = UnProject(new Vector3(mouseLocation.X, mouseLocation.Y, 0));
            var far = UnProject(new Vector3(mouseLocation.X, mouseLocation.Y, zfar));
            var origin = near;
            var direction = (far - near).Normalized();
            return new Ray()
            {
                center = new Vector3(origin.X, origin.Y, origin.Z),
                normal = new Vector3(direction.X, direction.Y, direction.Z)
            };
        }

        public static Vector3 RayPlaneIntersection(Ray ray, Plane plane)
        {
            Vector3 p;
            float t;
            t = (Vector3.Dot(plane.normal, plane.point) - Vector3.Dot(plane.normal, ray.center)) / Vector3.Dot(plane.normal, ray.normal);
            p = ray.center + t * ray.normal;
            return p;
        }
        public static Vector3? RayBoundingBoxIntersection(Ray ray, BoundingBox box)
        {
            const float Epsilon = 1e-6f;

            float? tMin = null, tMax = null;

            if (Math.Abs(ray.normal.X) < Epsilon)
            {
                if (ray.center.X < box.min.X || ray.center.X > box.max.X)
                    return null;
            }
            else
            {
                tMin = (box.min.X - ray.center.X) / ray.normal.X;
                tMax = (box.max.X - ray.center.X) / ray.normal.X;

                if (tMin > tMax)
                {
                    var temp = tMin;
                    tMin = tMax;
                    tMax = temp;
                }
            }

            if (Math.Abs(ray.normal.Y) < Epsilon)
            {
                if (ray.center.Y < box.min.Y || ray.center.Y > box.max.Y)
                    return null;
            }
            else
            {
                var tMinY = (box.min.Y - ray.center.Y) / ray.normal.Y;
                var tMaxY = (box.max.Y - ray.center.Y) / ray.normal.Y;

                if (tMinY > tMaxY)
                {
                    var temp = tMinY;
                    tMinY = tMaxY;
                    tMaxY = temp;
                }

                if ((tMin.HasValue && tMin > tMaxY) || (tMax.HasValue && tMinY > tMax))
                    return null;

                if (!tMin.HasValue || tMinY > tMin) tMin = tMinY;
                if (!tMax.HasValue || tMaxY < tMax) tMax = tMaxY;
            }

            if (Math.Abs(ray.normal.Z) < Epsilon)
            {
                if (ray.center.Z < box.min.Z || ray.center.Z > box.max.Z)
                    return null;
            }
            else
            {
                var tMinZ = (box.min.Z - ray.center.Z) / ray.normal.Z;
                var tMaxZ = (box.max.Z - ray.center.Z) / ray.normal.Z;

                if (tMinZ > tMaxZ)
                {
                    var temp = tMinZ;
                    tMinZ = tMaxZ;
                    tMaxZ = temp;
                }

                if ((tMin.HasValue && tMin > tMaxZ) || (tMax.HasValue && tMinZ > tMax))
                    return null;

                if (!tMin.HasValue || tMinZ > tMin) tMin = tMinZ;
                if (!tMax.HasValue || tMaxZ < tMax) tMax = tMaxZ;
            }

            // having a positive tMin and a negative tMax means the ray is inside the box
            // we expect the intesection distance to be 0 in that case
            if ((tMin.HasValue && tMin < 0) && tMax > 0) return ray.center;

            // a negative tMin means that the intersection point is behind the ray's origin
            // we discard these as not hitting the AABB
            if (tMin < 0) return null;

            //return tMin;
            var src = ray.center;
            var dis = Vector3.CalculateAngle(ray.center, ray.center + ray.normal);
            var ret = Vector3.Lerp(ray.center, ray.center + ray.normal, tMin.Value / dis);
            return ret;
        }
        //一点，和一个方向向量（两点求差）确定一条射线，
        public struct Ray
        {
            public Vector2? screen;
            public Vector3 center;
            public Vector3 normal;
            public Ray(Vector3 c, Vector3 t)
            {
                this.center = c;
                this.normal = t;
            }
            public override string ToString()
            {
                return $"center:{center} normal:{normal}";
            }
        }

        //一点，和一个法向量确定一个平面
        public struct Plane
        {
            public Vector3 point;
            public Vector3 normal;
            public Plane(Vector3 p, Vector3 n)
            {
                this.point = p;
                this.normal = n;
            }
        }
        public struct BoundingBox
        {
            public Vector3 min;
            public Vector3 max;
            public BoundingBox(Vector3 min, Vector3 max)
            {
                this.min = min;
                this.max = max;
            }
            public static implicit operator BoundingBox(in DeepCore.Geometry.BoundingBox value)
            {
                return new BoundingBox(value.Min.ToGL(), value.Max.ToGL());
            }
            public static implicit operator DeepCore.Geometry.BoundingBox(in BoundingBox value)
            {
                return new DeepCore.Geometry.BoundingBox(value.min.ToGeometry(), value.max.ToGeometry());
            }
        }

    }




}
