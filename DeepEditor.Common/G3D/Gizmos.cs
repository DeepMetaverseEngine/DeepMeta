using DeepCore;
using DeepEditor.Common.Voxel;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepEditor.Common.G3D
{

    public class GizmosTouchInfo
    {
        public Vector3 worldCenter;
        public Vector3 worldX;
        public Vector3 worldY;
        public Vector3 worldZ;
        public Vector3 rayTouchX;
        public Vector3 rayTouchY;
        public Vector3 rayTouchZ;

        public float screenRadius;
        public Vector2 screenCenter;
        public Vector2 screenX;
        public Vector2 screenY;
        public Vector2 screenZ;
        public bool touchX;
        public bool touchY;
        public bool touchZ;
        public bool touched { get => touchX || touchY || touchZ; }
        public Glu.Plane planeUp { get => new Glu.Plane() { point = worldX, normal = Vector3.UnitY }; }
        public Glu.Plane planeFront { get => new Glu.Plane() { point = worldY, normal = Vector3.UnitZ }; }
        public Glu.Plane planeLeft { get => new Glu.Plane() { point = worldZ, normal = Vector3.UnitY }; }
        public Vector3? rayTouchPos
        {
            get
            {
                if (touchX) { return rayTouchX; }
                if (touchY) { return rayTouchY; }
                if (touchZ) { return rayTouchZ; }
                return null;
            }
        }

        public static bool GetGizmosPosition(CameraControl camera, Vector3 wp,
            float handlerScreenDistance,
            out Vector3 wx, out Vector3 wy, out Vector3 wz,
            out Vector2 dc, out Vector2 dx, out Vector2 dy, out Vector2 dz)
        {
            var dis = camera.ScreenToWorldSize(wp, handlerScreenDistance);
            wx = wp + new Vector3(dis, 0, 0);
            wy = wp + new Vector3(0, dis, 0);
            wz = wp + new Vector3(0, 0, dis);
            var sc = camera.WorldToScreen(wp).Xy;
            var sX = camera.WorldToScreen(wx).Xy;
            var sY = camera.WorldToScreen(wy).Xy;
            var sZ = camera.WorldToScreen(wz).Xy;
            dc = sc;
            //             {
            //                 var sD = 50;
            //                 dx = sX = Vector2.Lerp(sc, sc + (sX - sc).Normalized(), sD);
            //                 dy = sY = Vector2.Lerp(sc, sc + (sY - sc).Normalized(), sD);
            //                 dz = sZ = Vector2.Lerp(sc, sc + (sZ - sc).Normalized(), sD);
            //             }
            {
                dx = sX;
                dy = sY;
                dz = sZ;
            }
            return true;
        }

        public static GizmosTouchInfo PickGizmos(Vector2 mouse, CameraControl camera, Vector3 wp, float handlerScreenDistance, float handlerScreenRadius)
        {
            var info = new GizmosTouchInfo();
            var sR = info.screenRadius = handlerScreenRadius;
            GetGizmosPosition(camera, wp, handlerScreenDistance,
                out var wx, out var wy, out var wz,
                out var sc, out var dx, out var dy, out var dz);
            info.worldCenter = wp;
            info.worldX = wx;
            info.worldY = wy;
            info.worldZ = wz;
            info.screenCenter = sc;
            info.screenX = dx;
            info.screenY = dy;
            info.screenZ = dz;
            info.touchX = CMath.IncludeRoundPoint(dx.X, dx.Y, sR, mouse.X, mouse.Y);
            info.touchY = CMath.IncludeRoundPoint(dy.X, dy.Y, sR, mouse.X, mouse.Y);
            info.touchZ = CMath.IncludeRoundPoint(dz.X, dz.Y, sR, mouse.X, mouse.Y);
         
            var ray = camera.ScreenToWorldRay(mouse);
            info.rayTouchX = Glu.RayPlaneIntersection(ray, info.planeUp);
            info.rayTouchY = Glu.RayPlaneIntersection(ray, info.planeFront);
            info.rayTouchZ = Glu.RayPlaneIntersection(ray, info.planeLeft);

            if (camera.CamType == CameraType.Camera2D)
            {
                info.touchY = false;
            }
            return info;
        }

        public static void RayCast(in GizmosTouchInfo gizmos, Glu.Ray ray, out Vector3 touchX, out Vector3 touchY, out Vector3 touchZ)
        {
            touchX = Glu.RayPlaneIntersection(ray, gizmos.planeUp);
            touchY = Glu.RayPlaneIntersection(ray, gizmos.planeFront);
            touchZ = Glu.RayPlaneIntersection(ray, gizmos.planeLeft);
        }
        public static bool RayCastLast(in GizmosTouchInfo gizmos, Glu.Ray ray, out Vector3? touchPos)
        {
            if (gizmos.touchX) { touchPos = Glu.RayPlaneIntersection(ray, gizmos.planeUp); return true; }
            if (gizmos.touchY) { touchPos = Glu.RayPlaneIntersection(ray, gizmos.planeFront); return true; }
            if (gizmos.touchZ) { touchPos = Glu.RayPlaneIntersection(ray, gizmos.planeLeft); return true; }
            touchPos = null;
            return false;
        }

        public static void DrawGizmosHUD(in GizmosTouchInfo gizmos, CameraControl camera, bool forceX = false, bool forceY = false, bool forceZ = false)
        {
            var cX = Color4.Red;
            var cY = Color4.Green;
            var cZ = Color4.Blue;

            DrawingHUD.FillCycle(cX, gizmos.screenX, gizmos.screenRadius + 1);
            DrawingHUD.FillCycle(cZ, gizmos.screenZ, gizmos.screenRadius + 1);          
            if (forceX || gizmos.touchX) { cX = cX.Add(0.75f); DrawingHUD.FillCycle(cX, gizmos.screenX, gizmos.screenRadius); }
            if (forceZ || gizmos.touchZ) { cZ = cZ.Add(0.85f); DrawingHUD.FillCycle(cZ, gizmos.screenZ, gizmos.screenRadius); }
            DrawingHUD.DrawLine(cX, gizmos.screenCenter, gizmos.screenX);
            DrawingHUD.DrawLine(cZ, gizmos.screenCenter, gizmos.screenZ);

            if (camera.CamType == CameraType.Camera3D)
            {
                DrawingHUD.FillCycle(cY, gizmos.screenY, gizmos.screenRadius + 1);
                if (forceY || gizmos.touchY) { cY = cY.Add(0.75f); DrawingHUD.FillCycle(cY, gizmos.screenY, gizmos.screenRadius); }
                DrawingHUD.DrawLine(cY, gizmos.screenCenter, gizmos.screenY);

            }
        }

    }

}
