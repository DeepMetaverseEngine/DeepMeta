using DeepCore;
using DeepCore.XCSV;
using glTFLoader.Schema;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DeepEditor.Common.G3D
{
    //---------------------------------------------------------------------------------------------------------------------------------
    public enum CameraMode
    {
        Perspective, Orthographic
    }
    public enum CameraType
    {
        Camera2D, Camera3D,
    }
    public abstract class CameraControl : Disposable
    {
        //位置
        protected Vector3 mCamPosition;
        //摇头
        protected float mCamYaw;
        //点头
        protected float mCamPitch;

        protected Matrix4 mtx_project = Matrix4.Identity;
        protected Matrix4 mtx_modelview = Matrix4.Identity;

        private Vector3 camFront;
        private Vector3 camUp;
        private Vector3 camRight;
        private float maxConstrainPitch = CMath.ToPI(89.0f);
        private CameraMode mode = CameraMode.Perspective;
        //-------------------------------------------------------------------------
        public float MouseSensitivity { get; set; } = 0.001f;
        public float MovementSpeed { get; set; } = 0.4f;
        public float ZoomSpeed { get; set; } = 8f;
        public bool ConstrainPitch { get; set; } = true;
        public float ShiftAddSpeedRate { get; set; } = 10f;
        public abstract CameraType CamType { get; }
        public float CameraFar { get; private set; } = 10000f;
        public Rectangle ViewPort { get; private set; }
        public CameraMode Mode { get => mode; }
        public Matrix4 ViewMatrix { get => mtx_modelview; }
        public Matrix4 ProjectionMatrix { get => mtx_project; }
        /// <summary>
        /// 时实摄像机位置
        /// </summary>
        public virtual Vector3 CamPosition { get => mCamPosition; set => mCamPosition = value; }
        /// <summary>
        /// 时实摄像机前方
        /// </summary>
        public virtual Vector3 CamFront { get => camFront; }
        /// <summary>
        /// 时实摄像机上方
        /// </summary>
        public virtual Vector3 CamUp { get => camUp; }
        /// <summary>
        /// 时实摄像机右方
        /// </summary>
        public virtual Vector3 CamRight { get => camRight; }
        //-------------------------------------------------------------------------
        public void ResetViewPort(Size size)
        {
            this.ResetViewPort(new Rectangle(0, 0, size.Width, size.Height));
        }
        public void ResetViewPort(Rectangle viewPort)
        {
            if (this.ViewPort != viewPort)
            {
                this.ViewPort = viewPort;
                GL.Viewport(viewPort.X, viewPort.Y, viewPort.Width, viewPort.Height);
            }
        }
        public void ResetCameraFar(float far)
        {
            far = Math.Min(1000000, Math.Max(1000, far));
            if (this.CameraFar != far)
            {
                this.CameraFar = far;
            }
        }
        public void ResetCameraMode(CameraMode mode)
        {
            if (this.mode != mode)
            {
                this.mode = mode;
            }
        }

        //---------------------------------------------------------------------------------------------------------------

        public Glu.Ray ScreenToWorldRay(Point vec, float farZ = 1f)
        {
            return this.ScreenToWorldRay(new Vector2(vec.X, vec.Y), farZ);
        }
        public virtual Glu.Ray ScreenToWorldRay(Vector2 vec, float farZ = 1f)
        {
            var ray = Glu.ScreenPointToRay(vec, mtx_modelview, mtx_project, this.ViewPort, farZ);
            ray.screen = vec;
            return ray;
        }
        //---------------------------------------------------------------------------------------------------------------
        public Vector3 ScreenRaycastZeroPlane(Point vec, float farZ = 1f)
        {
            var ray = this.ScreenToWorldRay(new Vector2(vec.X, vec.Y), farZ);
            var t2 = Glu.RayPlaneIntersection(ray, new Glu.Plane(Vector3.Zero, CamFront));
            return t2;
        }
        public Vector3 ScreenRaycastZeroPlane(Vector2 vec, float farZ = 1f)
        {
            var ray = this.ScreenToWorldRay(vec, farZ);
            var t2 = Glu.RayPlaneIntersection(ray, new Glu.Plane(Vector3.Zero, CamFront));
            return t2;
        }
        //---------------------------------------------------------------------------------------------------------------
        public Vector3 ScreenRaycastCameraPlane(Point vec, float farZ = 1f)
        {
            var ray = this.ScreenToWorldRay(new Vector2(vec.X, vec.Y), farZ);
            var t2 = Glu.RayPlaneIntersection(ray, new Glu.Plane(CamPosition, CamFront));
            return t2;
        }
        public Vector3 ScreenRaycastCameraPlane(Vector2 vec, float farZ = 1f)
        {
            var ray = this.ScreenToWorldRay(vec, farZ);
            var t2 = Glu.RayPlaneIntersection(ray, new Glu.Plane(CamPosition, CamFront));
            return t2;
        }
        //---------------------------------------------------------------------------------------------------------------
        public Vector3 ScreenRaycastPlane(Point vec, Glu.Plane plane, float farZ = 1f)
        {
            var ray = this.ScreenToWorldRay(new Vector2(vec.X, vec.Y), farZ);
            var t2 = Glu.RayPlaneIntersection(ray, plane);
            return t2;
        }
        public Vector3 ScreenRaycastPlane(Vector2 vec, Glu.Plane plane, float farZ = 1f)
        {
            var ray = this.ScreenToWorldRay(vec, farZ);
            var t2 = Glu.RayPlaneIntersection(ray, plane);
            return t2;
        }
        //---------------------------------------------------------------------------------------------------------------
        public float ScreenToWorldSize(Vector3 worldPos, float size)
        {
            var ray1 = ScreenToWorldRay(Vector2.Zero);
            var t1 = Glu.RayPlaneIntersection(ray1, new Glu.Plane(worldPos, CamFront));
            var ray2 = ScreenToWorldRay(new Vector2(size, 0));
            var t2 = Glu.RayPlaneIntersection(ray2, new Glu.Plane(worldPos, CamFront));
            return Vector3.Distance(t1, t2);
        }

        //---------------------------------------------------------------------------------------------------------------
        public virtual Vector3 ScreenToWorldOrgin(Vector2 vec)
        {
            var pos = Glu.ScreenPointToOrgin(new Vector2(vec.X, vec.Y), mtx_modelview, mtx_project, this.ViewPort);
            return new Vector3(pos.X, pos.Y, pos.Z);
        }
        public virtual Vector3 WorldToScreen(Vector3 vec)
        {
            var pos = Glu.Project(vec, mtx_modelview, mtx_project, this.ViewPort);
            return new Vector3(pos.X, pos.Y, pos.Z);
        }
        public virtual float ScreenToWorldSize(float v)
        {
            var r1 = Glu.ScreenPointToOrgin(Vector2.Zero, mtx_modelview, mtx_project, this.ViewPort);
            var r2 = Glu.ScreenPointToOrgin(new Vector2(v, 0), mtx_modelview, mtx_project, this.ViewPort);
            return CMath.GetDirect(v) * Vector3.Distance(r1, r2);
        }
        public virtual float WorldToScreenSize(float v)
        {
            var r1 = Glu.Project(mCamPosition, mtx_modelview, mtx_project, this.ViewPort);
            var r2 = Glu.Project(mCamPosition + (camRight * v), mtx_modelview, mtx_project, this.ViewPort);
            return CMath.GetDirect(v) * Vector2.Distance(r1.Xy, r2.Xy);
        }
        //---------------------------------------------------------------------------------------------------------------
        public abstract bool IsObjectInCamera(Vector3 vec, float radius = 0);

        public virtual void Update(GLControl control, TimeSpan elapsed)
        {
            if (control.Focused)
            {
                this.ProcessQueryKey(control, elapsed);
            }
        }
        public virtual void BeginLookAt(GLControl control, TimeSpan elapsed)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            if (mode == CameraMode.Perspective)
            {
                float w = ViewPort.Width;
                float h = ViewPort.Height;
                var AspectRatio = w / (float)h;
                this.mtx_project = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, AspectRatio, 1f, CameraFar);
                GL.LoadMatrix(ref mtx_project);
            }
            else if (mode == CameraMode.Orthographic)
            {
                float w = ViewPort.Width;
                float h = ViewPort.Height;
                this.mtx_project = Matrix4.CreateOrthographicOffCenter(-w, w, -h, h, -CameraFar, CameraFar);
                GL.LoadMatrix(ref mtx_project);
            }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            this.InternalLookAt();
        }
        protected virtual void InternalLookAt()
        {
            this.mtx_modelview = Matrix4.LookAt(mCamPosition, mCamPosition + camFront, camUp);
            GL.LoadMatrix(ref mtx_modelview);
        }
        public virtual void EndLookAt()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
        }
        public virtual void SetCamera(Vector3 pos, float yaw, float pitch)
        {
            this.mCamPosition = pos;
            this.mCamYaw = yaw;
            this.mCamPitch = pitch;
            this.UpdateCameraVectors();
        }
        public virtual void SetTarget(Vector3 pos)
        {
            Vector3 front = mCamPosition - pos;
            this.mCamYaw = CMath.GetDegree(front.Z, front.X);
            this.mCamPitch = CMath.GetDegree(front.Z, -front.Y);
            this.UpdateCameraVectors();
        }
        public virtual void SetTerrain(float tw, float th)
        {
            this.mCamPosition = new Vector3(tw * 0.5f, th / 2, th);
            SetTarget(new Vector3(tw * 0.5f, 0, 0));
            if (this is FreeCameraControl3D free)
            {
                free.Forward(-Math.Max(tw, th));
            }
        }
        public virtual void SetTerrain(DeepCore.Geometry.BoundingBox box)
        {
            var boxA = box;
            boxA.Min.Z = box.Min.Y;
            boxA.Min.Y = box.Min.Z;
            boxA.Max.Z = box.Max.Y;
            boxA.Max.Y = box.Max.Z;

            var boxB = boxA;
            var tx = Math.Abs(boxA.Max.X - boxA.Min.X);
            var ty = Math.Abs(boxA.Max.Y - boxA.Min.Y);
            var tz = Math.Abs(boxA.Max.Z - boxA.Min.Z);

            boxB.Min.Y = Math.Min(boxA.Min.X, Math.Min(boxA.Min.Y, boxA.Min.Z));
            boxB.Min.Z = Math.Min(boxA.Min.X, Math.Min(boxA.Min.Y, boxA.Min.Z));
            boxB.Max.Y = Math.Max(boxA.Max.X, Math.Max(boxA.Max.Y, boxA.Max.Z));
            boxB.Max.Z = Math.Max(boxA.Max.X, Math.Max(boxA.Max.Y, boxA.Max.Z));

            this.mCamPosition = new Vector3(
                boxB.Max.X - tx * 0.5f,
                boxB.Max.Y,
                boxB.Max.Z);
            SetTarget(new Vector3(
                boxA.Min.X + tx * 0.5f,
                boxA.Min.Y + ty * 0.5f,
                boxA.Min.Z + tz * 0.5f));
            if (this is FreeCameraControl3D free)
            {
                free.Forward(-CMath.Max(tz, tx, ty) * 1.25f);
            }
        }
        public virtual void SetLookTarget(Vector3 target, float len)
        {
            this.mCamPosition = new Vector3(target.X, target.Y + len, target.Z + len);
            SetTarget(target);
        }
        public void LookAt(Vector3 target)
        {
            SetTarget(target);
        }
        protected virtual void UpdateCameraVectors()
        {
            if (ConstrainPitch)
            {
                if (this.mCamPitch > maxConstrainPitch)
                    this.mCamPitch = maxConstrainPitch;
                if (this.mCamPitch < -maxConstrainPitch)
                    this.mCamPitch = -maxConstrainPitch;
            }
            //更新摄像机向量
            Vector3 front;
            front.X = (float)(-Math.Sin(mCamYaw)); //cos(glm::radians(Yaw)) * cos(glm::radians(Pitch));
            front.Y = (float)(Math.Sin(mCamPitch));
            front.Z = (float)(-Math.Cos(mCamPitch) * Math.Cos(mCamYaw));// sin(glm::radians(Yaw)) * cos(glm::radians(Pitch));
            this.camFront = front.Normalized();
            this.camRight = Vector3.Cross(camFront, Vector3.UnitY).Normalized();
            this.camUp = Vector3.Cross(camRight, camFront).Normalized();//glm::normalize(glm::cross(Right, Front));
        }
        internal void OnKeyDown(GLControl control, KeyEventArgs e)
        {
            this.ProcessKeyDown(control, e);
        }
        internal void OnKeyUp(GLControl control, KeyEventArgs e)
        {
            this.ProcessKeyUp(control, e);
        }
        internal void OnMouseDown(GLControl control, MouseEventArgs e)
        {
            this.mouse_lastMouseDown = new MouesMoveArgs()
            {
                RaycastOrgin = ScreenToWorldRay(e.Location),
                CameraOrgin = CamPosition,
                ScreenOrgin = new Vector2(e.X, e.Y),
                ScreenPos = new Vector2(e.X, e.Y),
                ScreenOffset = new Vector2(0, 0),
                ScreenLastPos = new Vector2(e.X, e.Y),
            };
            this.ProcessMouseDown(control, e);
        }
        internal void OnMouseUp(GLControl control, MouseEventArgs e)
        {
            this.ProcessMouseUp(control, e);
        }
        internal void OnMouseMove(GLControl control, MouseEventArgs e)
        {
            //鼠标输入处理
            this.mouse_lastMouseDown.ScreenPos = new Vector2(e.X, e.Y);
            this.mouse_lastMouseDown.ScreenOffset = mouse_lastMouseDown.ScreenLastPos - mouse_lastMouseDown.ScreenPos;
            this.mouse_lastMouseDown.ScreenLastPos = mouse_lastMouseDown.ScreenPos;
            if (e.Button != 0)
            {
                this.ProcessMouseDrag(control, e, mouse_lastMouseDown);
            }
            else
            {
                this.ProcessMouseMove(control, e, mouse_lastMouseDown);
            }
            this.UpdateCameraVectors();
        }
        internal void OnMouseWheel(GLControl control, MouseEventArgs e)
        {
            float delta = e.Delta;
            this.ProcessMouseWheel(control, e, delta);
        }

        protected virtual void ProcessKeyDown(GLControl control, KeyEventArgs e) { }
        protected virtual void ProcessKeyUp(GLControl control, KeyEventArgs e) { }
        protected virtual void ProcessQueryKey(GLControl control, TimeSpan elapsed) { }
        protected virtual void ProcessMouseDown(GLControl control, MouseEventArgs e) { }
        protected virtual void ProcessMouseUp(GLControl control, MouseEventArgs e) { }
        protected virtual void ProcessMouseWheel(GLControl control, MouseEventArgs e, float delta) { }
        protected virtual void ProcessMouseDrag(GLControl control, MouseEventArgs e, MouesMoveArgs args) { }
        protected virtual void ProcessMouseMove(GLControl control, MouseEventArgs e, MouesMoveArgs args) { }

        private MouesMoveArgs mouse_lastMouseDown;
        public struct MouesMoveArgs
        {
            public Glu.Ray RaycastOrgin;
            public Vector3 CameraOrgin;
            public Vector2 ScreenOrgin;

            public Vector2 ScreenPos;
            public Vector2 ScreenOffset;
            public Vector2 ScreenLastPos;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------
    public class FreeCameraControl3D : CameraControl
    {
        private float movementSpeedAdd;
        //private Vector2 mouse_ScreenCameraSizeRateOrgin = Vector2.One;
        private Vector3 mouse_down_camera_pos;
        private Vector3 mouse_down_hit_camera_pos;
        public override CameraType CamType => CameraType.Camera3D;
        protected override void Disposing()
        {

        }
        protected override void ProcessMouseDown(GLControl control, MouseEventArgs e)
        {
            if (Mouse.IsMouseDown(MouseButtons.Middle))
            {
                this.mouse_down_camera_pos = this.mCamPosition;
                this.mouse_down_hit_camera_pos = ScreenRaycastZeroPlane(e.Location);
                //                 var sp1 = ScreenToWorldOrgin(new Vector2(control.Width / 2f, control.Height / 2f)) - CamFront;
                //                 var sp2 = ScreenToWorldOrgin(new Vector2(control.Width / 2f, control.Height / 2f + 1f)) - CamFront;
                //                 var sp3 = ScreenToWorldOrgin(new Vector2(control.Width / 2f + 1f, control.Height / 2f)) - CamFront;
                //                 mouse_ScreenCameraSizeRateOrgin = new Vector2(
                //                     Vector3.Distance(sp1, sp3),
                //                     Vector3.Distance(sp1, sp2));
                //Console.WriteLine($"{mouse_ScreenCameraSizeRateOrgin}");
            }
            //             var _a = ScreenToWorldOrgin(new Vector2(control.Width / 2f, control.Height / 2f));
            //             Console.WriteLine($"{_a} : {mCamPosition} : {Vector3.Distance(_a, mCamPosition)}");
            base.ProcessMouseDown(control, e);
        }
        protected override void ProcessMouseDrag(GLControl control, MouseEventArgs e, MouesMoveArgs args)
        {
            if (Mouse.IsMouseDown(MouseButtons.Right))
            {
                var o = args.ScreenOffset * MouseSensitivity;
                this.mCamYaw += o.X;
                this.mCamPitch += o.Y;
            }
            else if (Mouse.IsMouseDown(MouseButtons.Middle))
            {
                //                 var ray1 = ScreenToWorldRay(args.ScreenPos);
                //                 var offset = args.RaycastOrgin.center - ray1.center;
                //                 var dlen = Vector3.Distance(args.RaycastOrgin.center, args.CameraOrgin);
                //                 Console.WriteLine($"{dlen}");
                //                var offset = args.ScreenPos - args.ScreenOrgin;
                var sp1 = ScreenRaycastZeroPlane(args.ScreenOrgin);
                var sp2 = ScreenRaycastZeroPlane(args.ScreenPos);
                // var sp1 = mouse_down_hit_camera_pos;
                // var sp2 = ScreenRaycastZeroPlane(e.Location);
                var offset = sp2 - sp1;
                //Console.WriteLine($"{offset}");
                if (offset.Length > 0)
                {
                    var camNew = mouse_down_camera_pos - offset;
                    //                 camNew -= (CamRight * (offset.X ));
                    //                 camNew -= (CamUp * (offset.Y ));
                    //camNew -= offset;
                    this.mCamPosition = camNew;
                }


                //                 var ray2 = ScreenToWorldRay(orgin + offset);
                //                 var _a = Vector3.Lerp(ray1.center, -ray1.normal, 1f);
                //                 var _b = Vector3.Lerp(ray2.center, -ray2.normal, 1f);
                //                 this.mCamPosition += (_b - _a);

                //                 //this.mCamPosition -= (CamRight * (_a.X - _b.X));
                //                 //this.mCamPosition -= (CamUp * (_a.Y - _b.Y));
                //                 Console.WriteLine($"{_a} : {_b} : {(_a - _b)}");

                //                 this.mCamPosition += (CamRight * (offset.X)) * 0.1f;
                //                 this.mCamPosition -= (CamUp * (offset.Y)) * 0.1f;

                //                 if (yoffset > 0)
                //                     mtx *= Matrix4.CreateRotationX(MathF.PI);
                //                 if (yoffset < 0)
                //                     mtx *= Matrix4.CreateRotationX(-MathF.PI);
                //                 if (xoffset > 0)
                //                     mtx *= Matrix4.CreateRotationY(MathF.PI);
                //                 if (xoffset < 0)
                //                     mtx *= Matrix4.CreateRotationY(-MathF.PI);
            }
        }
        protected override void ProcessMouseWheel(GLControl control, MouseEventArgs e, float delta)
        {
            if (delta != 0)
            {
                var sp1 = ScreenRaycastZeroPlane(new Point(0, 0));
                var sp2 = ScreenRaycastZeroPlane(new Point(4 * control.DeviceDpi, 0));
                var distance = Vector3.Distance(sp1, sp2);
                var add = Keyboard.IsShiftDown ? ShiftAddSpeedRate : 1.0f;
                var spd = Math.Min(MovementSpeed, 0.1f);

                var offset = 0f;
                if (delta > 0)
                {
                    offset = distance * (spd * add);
                }
                else if (delta < 0)
                {
                    offset = -distance * (spd * add);
                }
                //                 if (this.backDistanec < 1)
                //                 {
                //                     this.backDistanec = 1;
                //                 }
                //var add = Keyboard.IsShiftDown ? ShiftAddSpeedRate : 1.0f;
                //var offset = (DeepCore.CMath.GetDirect(delta) * ZoomSpeed * add);
                this.mCamPosition += this.CamFront * offset;
                //this.mCamPosition = Vector3.Lerp(this.mCamPosition, this.CamFront, offset);
            }
        }
        protected override void ProcessQueryKey(GLControl control, TimeSpan elapsed)
        {
            if (!Keyboard.IsCtrlDown)
            {
                var add = Keyboard.IsShiftDown ? ShiftAddSpeedRate : 1.0f;
                var speed = movementSpeedAdd * add;
                // move up or down
                if (Keyboard.IsKeyDown(Keys.Space) || Keyboard.IsKeyDown(Keys.Q))
                {
                    this.mCamPosition.Y += speed;
                }
                if (Keyboard.IsKeyDown(Keys.E))
                {
                    this.mCamPosition.Y -= speed;
                }
                // move forward or backward
                if (Keyboard.IsKeyDown(Keys.W))
                {
                    this.mCamPosition += CamFront * speed;
                    this.movementSpeedAdd *= 1.01f;
                }
                else if (Keyboard.IsKeyDown(Keys.S))
                {
                    this.mCamPosition -= CamFront * speed;
                    this.movementSpeedAdd *= 1.01f;
                }
                else
                {
                    this.movementSpeedAdd = this.MovementSpeed;
                }
                // move left or right
                if (Keyboard.IsKeyDown(Keys.A))
                {
                    this.mCamPosition -= CamRight * speed;
                }
                if (Keyboard.IsKeyDown(Keys.D))
                {
                    this.mCamPosition += CamRight * speed;
                }
            }
        }
        public void Forward(float distance)
        {
            this.mCamPosition += CamFront * distance;
        }
        public override bool IsObjectInCamera(Vector3 vec, float radius = 0)
        {
            var viewPos = this.WorldToScreen(vec);
            if (viewPos.Z < 0) return false;
            if (viewPos.Z > CameraFar) return false;
            if (viewPos.X < 0 || viewPos.Y < 0 || viewPos.X > ViewPort.Width || viewPos.Y > ViewPort.Height) return false;
            return true;
        }

    }
    //--------------------------------------------------------------------------------------------------------------------------------------
    public class FreeCameraControl2D : CameraControl
    {
        private float camZoom = 1.0f;
        private Vector3 camPos = new Vector3(0, 2, 0);
        private Vector4 camBounding = Vector4.Zero;
        protected override void Disposing()
        {

        }
        public override CameraType CamType => CameraType.Camera2D;
        public override Vector3 CamPosition => camPos;
        public override Vector3 CamFront => -Vector3.UnitY;

        public override void SetTerrain(float tw, float th)
        {
            camPos.X = tw / 2;
            camPos.Z = th / 2;
        }
        public override void SetCamera(Vector3 pos, float yaw, float pitch)
        {
            camPos = new Vector3(pos.X, CameraFar / camZoom, pos.Z);
        }
        public override void SetTarget(Vector3 pos)
        {
            camPos = new Vector3(pos.X, CameraFar / camZoom, pos.Z);
        }
        public override bool IsObjectInCamera(Vector3 vec, float radius = 0)
        {
            return CMath.IntersectRect(
                camBounding.X, camBounding.Y,
                camBounding.Z, camBounding.W,
                vec.X - radius, vec.Z - radius,
                vec.X + radius, vec.Z + radius);
        }
        //----------------------------------------------------------------------------------------
        public override Vector3 WorldToScreen(Vector3 vec)
        {
            return new Vector3(
                (vec.X - camPos.X) * camZoom + ViewPort.Width / 2,
                (vec.Z - camPos.Z) * camZoom + ViewPort.Height / 2,
                1);
        }
        public override Glu.Ray ScreenToWorldRay(Vector2 vec, float farZ = 1f)
        {
            var p = ScreenToWorldOrgin(vec);
            return new Glu.Ray(p, new Vector3(0, -farZ, 0).Normalized())
            {
                screen = vec,
            };
        }
        public override float ScreenToWorldSize(float v)
        {
            return v / camZoom;
        }
        public override float WorldToScreenSize(float v)
        {
            return v * camZoom;
        }
        public override Vector3 ScreenToWorldOrgin(Vector2 vec)
        {
            return new Vector3(
                camPos.X + (vec.X - ViewPort.Width / 2f) / camZoom,
                0,
                camPos.Z + (vec.Y - ViewPort.Height / 2f) / camZoom);
        }
        //----------------------------------------------------------------------------------------
        public override void Update(GLControl control, TimeSpan elapsed)
        {
            this.ProcessQueryKey(control, elapsed);
        }
        public override void BeginLookAt(GLControl control, TimeSpan elapsed)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            {
                float w = ViewPort.Width;
                float h = ViewPort.Height;
                this.mtx_project = Matrix4.CreateOrthographicOffCenter(-w / 2, w / 2, -h / 2, h / 2, -CameraFar * 2, CameraFar * 2);
                GL.LoadMatrix(ref mtx_project);
            }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                var d = CameraFar / camZoom;
                camPos.Y = d;
                this.mtx_modelview = Matrix4.LookAt(camPos, camPos + new Vector3(0, -d, 0), new Vector3(0, 0, -1));
                this.mtx_modelview *= Matrix4.CreateScale(camZoom);
                GL.LoadMatrix(ref mtx_modelview);
                {
                    var sw1 = ScreenToWorldOrgin(new Vector2(0, 0));
                    var sw2 = ScreenToWorldOrgin(new Vector2(ViewPort.Width, ViewPort.Height));
                    camBounding = new Vector4(sw1.X, sw1.Z, sw2.X, sw2.Z);
                }
            }
        }
        protected override void UpdateCameraVectors()
        {

        }
        protected override void ProcessMouseWheel(GLControl control, MouseEventArgs e, float delta)
        {
            var add = CMath.GetDirect(delta);
            if (add > 0)
            {
                this.camZoom *= 1.1f; ;
            }
            else if (add < 0)
            {
                this.camZoom /= 1.1f;
            }
            this.camZoom = Math.Min(camZoom, 100000.0f);
            this.camZoom = Math.Max(camZoom, 0.000001f);
        }
        protected override void ProcessMouseDrag(GLControl control, MouseEventArgs e, MouesMoveArgs args)
        {
            if ((e.Button & (MouseButtons.Right | MouseButtons.Middle)) != 0)
            {
                camPos.X += args.ScreenOffset.X / camZoom;
                camPos.Z += args.ScreenOffset.Y / camZoom;
            }
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------
    public interface ILockCameraActor
    {
        bool IsActive { get; }
        float Direction { get; }
        Vector3 Position { get; }
        float BodyHeight { get; }
        void Jump();
        void MoveAxis(Vector3 axis);
        void FaceTo(float dir);
    }
    public class LockActorCamera3D : FreeCameraControl3D
    {
        private float backDistanec = 20f;
        private float backYaw = 0f;
        private float backPitch = -MathHelper.Pi / 4;
        private float camDir;
        private float moveDir;
        private float moveDistance;
        private Vector3 targetTo = Vector3.Zero;
        public ILockCameraActor Actor { get; private set; }
        public LockActorCamera3D(ILockCameraActor actor)
        {
            this.Actor = actor;
            this.backYaw = actor.Direction;
        }
        protected override void Disposing()
        {
            this.Actor = null;
        }
        protected override void InternalLookAt()
        {
            if (Actor.IsActive)
            {
                var actor_pos = Actor.Position;
                actor_pos.Y += Actor.BodyHeight;
                Vector3 front;
                front.X = backDistanec * (float)(-Math.Sin(backYaw));
                front.Y = backDistanec * (float)(Math.Sin(backPitch));
                front.Z = backDistanec * (float)(-Math.Cos(backPitch) * Math.Cos(backYaw));
                var camRight = Vector3.Cross(front, Vector3.UnitY).Normalized();
                var camUp = Vector3.Cross(camRight, front).Normalized();
                var target = actor_pos;
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
        protected override void ProcessQueryKey(GLControl control, TimeSpan elapsed)
        {
            if (Actor.IsActive)
            {
                Vector3 offset = Vector3.Zero;
                float angle = 0;
                float distance = 0;
                if (Mouse.IsMouseDown(MouseButtons.Left) && Mouse.IsMouseDown(MouseButtons.Right))
                {
                    angle = 0;
                    distance = 1;
                }
                else
                {
                    if (Keyboard.IsKeyDown(Keys.W)) { offset.X += 1; }
                    if (Keyboard.IsKeyDown(Keys.S)) { offset.X -= 1; }
                    if (Keyboard.IsKeyDown(Keys.A)) { offset.Z -= 1; }
                    if (Keyboard.IsKeyDown(Keys.D)) { offset.Z += 1; }
                    if (Keyboard.IsKeyDown(Keys.Q)) { offset.Y += 1; }
                    if (Keyboard.IsKeyDown(Keys.E)) { offset.Y -= 1; }
                    angle = CMath.GetDegree(offset.X, offset.Z);
                    distance = CMath.GetDistance(0, 0, offset.X, offset.Z);
                }
                this.moveDistance = distance;
                this.moveDir = camDir + angle;
                if (distance > 0)
                {
                    var dx = (float)Math.Cos(moveDir) * distance;
                    var dz = (float)Math.Sin(moveDir) * distance;
                    var axis = new Vector3(dx, offset.Y, dz);
                    Actor.MoveAxis(Vector3.Normalize(axis));
                }
                Actor.FaceTo(camDir);

            }
            else
            {
                base.ProcessQueryKey(control, elapsed);
            }
        }
        protected override void ProcessKeyDown(GLControl control, KeyEventArgs e)
        {
            if (Actor.IsActive)
            {
                if (e.KeyCode == Keys.Space)
                {
                    Actor.Jump();
                }
            }
            else
            {
                base.ProcessKeyDown(control, e);
            }
        }
        protected override void ProcessMouseDown(GLControl control, MouseEventArgs e)
        {
            if (Actor.IsActive)
            {

            }
            else
            {
                base.ProcessMouseDown(control, e);
            }
        }
        protected override void ProcessMouseWheel(GLControl control, MouseEventArgs e, float delta)
        {
            if (Actor.IsActive)
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
        protected override void ProcessMouseDrag(GLControl control, MouseEventArgs e, MouesMoveArgs args)
        {
            if (Actor.IsActive)
            {
                if ((e.Button & (MouseButtons.Right | MouseButtons.Middle)) != 0)
                {
                    var o = args.ScreenOffset * MouseSensitivity;
                    this.backYaw += o.X;
                    this.backPitch += o.Y;
                }
            }
            else
            {
                base.ProcessMouseDrag(control, e, args);
            }
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------------------------------
    public class LockActorCamera2D : FreeCameraControl2D
    {
        private float moveDir;
        private float moveDistance;
        public ILockCameraActor Actor { get; private set; }
        public LockActorCamera2D(ILockCameraActor actor)
        {
            this.Actor = actor;
        }
        protected override void Disposing()
        {
            this.Actor = null;
        }
        public override void BeginLookAt(GLControl control, TimeSpan elapsed)
        {
            if (Actor.IsActive)
            {
                var pos = Actor.Position;
                pos.Y += Actor.BodyHeight;
                base.SetTarget(pos);
            }
            base.BeginLookAt(control, elapsed);
        }

        protected override void ProcessQueryKey(GLControl control, TimeSpan elapsed)
        {
            if (Actor.IsActive)
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
                    if (control.ContainsMousePoint())
                    {
                        var mx = (float)Math.Cos(moveDir);
                        var mz = (float)Math.Sin(moveDir);
                        var axis = new Vector3(mx, 0, mz);
                        Actor.MoveAxis(Vector3.Normalize(axis));
                    }
                    Actor.FaceTo(angle);
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
                    if (control.ContainsMousePoint())
                    {
                        if (move_distance > 0)
                        {
                            var mx = (float)Math.Cos(moveDir);
                            var mz = (float)Math.Sin(moveDir);
                            var axis = new Vector3(mx, 0, mz);
                            Actor.MoveAxis(Vector3.Normalize(axis));
                        }
                        Actor.FaceTo(angle);
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
            if (Actor.IsActive)
            {
                if (e.KeyCode == Keys.Space)
                {
                    Actor.Jump();
                }
            }
            else
            {
                base.ProcessKeyDown(control, e);
            }
        }
        protected override void ProcessMouseDown(GLControl control, MouseEventArgs e)
        {
            if (Actor.IsActive)
            {

            }
            else
            {
                base.ProcessMouseDown(control, e);
            }
        }
        protected override void ProcessMouseDrag(GLControl control, MouseEventArgs e, MouesMoveArgs args)
        {
            if (Actor.IsActive)
            {

            }
            else
            {
                base.ProcessMouseDrag(control, e, args);
            }
        }
    }
}
