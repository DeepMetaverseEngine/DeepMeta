using DeepCore;
using DeepCore.Reflection;
using DeepCore.Voxel.Data;
using DeepEditor.Common.G3D;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.DisplayTerrainData
{

    public class TerrainDataCanvas : GLView
    {
        [Desc("显示Chunk边框", "show", true)]
        public bool ShowChunkBounding { get; set; } = false;
        [Desc("显示地面网格", "show", true)]
        public bool ShowGridLines { get; set; } = true;
        //-----------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------
        public TerrainDataCanvas(GLControl control, Timer timer) : base(control, timer)
        {
            this.OnRender += Canvas_OnRender;
            this.OnBeginRender += Canvas_OnBeginRender;
            this.BackColor = Color4.SkyBlue;
        }
//         protected override void GlControl_Load(object sender, EventArgs e)
//         {
//             base.GlControl_Load(sender, e);
//         }
//         protected override void GlControl_Disposed(object sender, EventArgs e)
//         {
//             base.GlControl_Disposed(sender, e);
//         }
        protected virtual void Canvas_OnBeginRender(GLView sender, PaintEventArgs3D e)
        {
            var cpos = Camera.CamPosition;
        }
        protected virtual void Canvas_OnRender(GLView sender, PaintEventArgs3D e)
        {
            DrawGrids(e);
            DrawingVoxelObject.DrawVoxelAnchor(Vector3.Zero);
            if (ShowGridLines)
            {
                DrawingVoxelObject.DrawGridLines(Color4.White, -2048, -2048, 256, 256, 4096 / 256, 4096 / 256);
            }
            if (ShowChunkBounding)
            {
                DrawingVoxelObject.DrawBoundingBox(Color4.Yellow, Bounding);
            }
        }
        //-----------------------------------------------------------------------------------------------------------------
        public void ResetCameraPos()
        {
            this.ResetCameraPos(this.Camera);
        }
        protected override void OnCreateCameraControl(CameraControl c)
        {
            this.ResetCameraPos(c);
            base.OnCreateCameraControl(c);
        }
        public virtual void ResetCameraPos(CameraControl camera)
        {
            if (Data != null)
            {
                var size = Bounding.Size;
                camera.ShiftAddSpeedRate = Math.Max(10, 256f / 100);
                camera.ResetCameraFar(Math.Max(this.Camera.CameraFar, size.X + size.Y));
                camera.SetTerrain(Bounding);
            }
            else
            {
                var zoneSize = 256f;
                var tf = 4096f;
                camera.ShiftAddSpeedRate = Math.Max(10, zoneSize / 100);
                camera.ResetCameraFar(Math.Max(this.Camera.CameraFar, tf));
                camera.SetLookTarget(Vector3.Zero, tf);
            }
        }
        //-----------------------------------------------------------------------------------------------------------------

        public VoxelTerrainData Data { get; private set; }
        public float GridSize { get; private set; } = 1;
        public DeepCore.Geometry.BoundingBox Bounding { get; private set; }
        public void InitTerrain(VoxelTerrainData data)
        {
            this.Data = data;
            this.GridSize = data.GridSize;
            this.Bounding = new DeepCore.Geometry.BoundingBox(
                new DeepCore.Geometry.Vector3(0, 0, 0),
                new DeepCore.Geometry.Vector3(data.XCount * data.GridSize, data.YCount * data.GridSize, 10));
            InitGrids(data);
            ResetCameraPos();
        }

        //-----------------------------------------------------------------------------------------
        private VertexArrayObject Grids;
        private CubesInstancedVertexArrayObject Cubes;
        private void ClearGrids()
        {
            if (Grids != null)
            {
                Grids.Dispose();
                Grids = null;
            }
            if (Cubes != null)
            {
                Cubes.Dispose();
                Cubes = null;
            }
        }
        private void InitGrids(VoxelTerrainData data)
        {
            ClearGrids();
#if GPU_INSTANCING
            if (Grids == null)
            {
                Cubes = new CubesVertexArrayObject(GridSize);
                data.Grids.ForEachArray2D((c, x, y) =>
                {
                    foreach (var o in c)
                    {
                        Colors.DecodeARGB(o.Color, out float r, out float g, out float b, out float a);
                        Cubes.AddCube2D(
                            new Vector3(x * GridSize, y * GridSize, o.Upward - GridSize), 
                            new Color4(r, g, b, a));
                    }
                });
            }
#endif
            if (Cubes == null)
            {
                Grids = new VertexArrayObject(PrimitiveType.Quads);
                Grids.EnableUV = false;
                Grids.EnableNormal = false;
                Grids.SetShader(Shaders.GetOrAdd("Grids", n =>
                {
                    var ret = new TintShader();
                    ret.OnShaderBegin += (s, e) =>
                    {
                        //ret.LightPosition = e.Camera.CamPosition;
                    };
                    return ret;
                }));
                data.Grids.ForEachArray2D(0,(st,c, x, y) =>
                {
                    foreach (var o in c)
                    {
                        var min = new Vector3(
                            x * GridSize,
                            y * GridSize,
                            o.Downward);
                        var max = new Vector3(
                            x * GridSize + GridSize,
                            y * GridSize + GridSize,
                            o.Upward);
                        Colors.DecodeARGB(o.Color, out float r, out float g, out float b, out float a);
                        Grids.AddBox2DShadow(min, max, new Color4(r, g, b, a));
                    }
                });
            }

        }
        private void DrawGrids(PaintEventArgs3D e)
        {
            if (Grids != null)
            {
                Grids.Draw(e);
            }
            if (Cubes != null)
            {
                Cubes.Draw(e);
            }
        }
        //-----------------------------------------------------------------------------------------
    }

    public abstract class DisplayTerrainObject : GLViewObject3D
    {
        public TerrainDataCanvas World { get => base.View as TerrainDataCanvas; }

    }

}
