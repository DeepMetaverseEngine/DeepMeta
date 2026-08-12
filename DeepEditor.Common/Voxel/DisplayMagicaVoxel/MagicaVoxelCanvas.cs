using DeepCore;
using DeepCore.Reflection;
using DeepCore.Voxel.Extensions.MagicaVoxel;
using DeepEditor.Common.G3D;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.DisplayMagicaVoxel
{

    public class MagicaVoxelCanvas : GLView
    {
        [Desc("显示Chunk边框", "show", true)]
        public bool ShowChunkBounding { get; set; } = false;
        [Desc("显示地面网格", "show", true)]
        public bool ShowGridLines { get; set; } = true;
        //-----------------------------------------------------------------------------------------------------------------
        private LightingShader lightingShader;
        //-----------------------------------------------------------------------------------------------------------------
        public MagicaVoxelCanvas(GLControl control, Timer timer) : base(control, timer)
        {
            this.OnRender += MagicaVoxelCanvas_OnRender;
            this.OnBeginRender += MagicaVoxelCanvas_OnBeginRender;
            this.OnPaintGDI += MagicaVoxelCanvas_OnPaintGDI;
            this.BackColor = Color4.SkyBlue;
        }
        private void MagicaVoxelCanvas_OnPaintGDI(object sender, PaintEventArgs e)
        {
            if (VOX != null)
            {
                e.Graphics.DrawDebugTextHUD($"Bounding:{Bounding}", 1, 1);
            }

        }
        protected override void GlControl_Load(object sender, EventArgs e)
        {
            base.GlControl_Load(sender, e);
            this.lightingShader = Shaders.GetOrAdd("LightingShader", n => new LightingShader());
        }
        protected override void Disposing()
        {
            foreach (var d in chunkCaches.Values)
            {
                d.Dispose();
            }
            base.Disposing();
        }
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
            if (VOX != null)
            {
                var size = box.Size;
                camera.ShiftAddSpeedRate = Math.Max(10, 256f / 100);
                camera.ResetCameraFar(Math.Max(this.Camera.CameraFar, size.X + size.Y));
                camera.SetTerrain(box);
                //lightingShader.LightPosition = new Vector3(size.X, CMath.Max(size.X, size.Y, size.Z) * 2, size.Y);
            }
            else
            {
                var zoneSize = 256f;
                var tf = 4096f;
                camera.ShiftAddSpeedRate = Math.Max(10, zoneSize / 100);
                camera.ResetCameraFar(Math.Max(this.Camera.CameraFar, tf));
                camera.SetLookTarget(Vector3.Zero, tf);
                //lightingShader.LightPosition = new Vector3(0, tf, 0);
            }
        }
        //-----------------------------------------------------------------------------------------------------------------
        public MagicaVoxelFile VOX { get; private set; }
        private HashMap<int, VertexArrayObject> chunkCaches = new HashMap<int, VertexArrayObject>();
        private DeepCore.Geometry.BoundingBox box;
        public DeepCore.Geometry.BoundingBox Bounding { get => box; }
        protected virtual void MagicaVoxelCanvas_OnBeginRender(GLView sender, PaintEventArgs3D e)
        {
            var cpos = Camera.CamPosition;
            //cpos.Y += 1024;
            lightingShader.LightPosition = cpos;
        }
        protected virtual void MagicaVoxelCanvas_OnRender(GLView sender, PaintEventArgs3D e)
        {
            DrawingVoxelObject.DrawVoxelAnchor(Vector3.Zero);
            if (ShowGridLines)
            {
                DrawingVoxelObject.DrawGridLines(Color4.White, -2048, -2048, 256, 256, 4096 / 256, 4096 / 256);
            }
            if (ShowChunkBounding)
            {
                DrawingVoxelObject.DrawBoundingBox(Color4.Yellow, box);
            }
        }
        public void InitVOX(MagicaVoxelFile vox)
        {
            this.VOX = vox;
            InitVOXChilds();
            ResetCameraPos();
        }
#if false
        private void InitVOXChilds(MagicaVoxelFile.SceneGraph root)
        {
            InitSceneGraph(RootObject, root);
        }
        private void InitSceneGraph(GLViewObject3D parent, MagicaVoxelFile.SceneGraph node)
        {
            var t = node.Transform.Translation;
            var r = node.Transform.Rotation;
            var xt = Matrix4.CreateTranslation(t.X, t.Z, -t.Y);
            var transform = xt;
            if (node.Shape != null)
            {
                foreach (var model in node.Shape.Models)
                {
                    var chunk = node.Transform.Owner.Main.Models[model.ModelID];
                    var display = new DisplayShapeObject(node, model.ModelID, chunk);
                    display.Transform = transform;
                    parent.AddChild(display);
                }
            }
            else if (node.Group != null)
            {
                var display = new DisplayGroupObject(node);
                display.Transform = transform;
                parent.AddChild(display);
                foreach (var child in node.GroupChilds)
                {
                    InitSceneGraph(display, child);
                }
            }
        }
        class DisplayGroupObject : DisplayMagicaVoxelObject
        {
            public MagicaVoxelFile.SceneGraph Node { get; private set; }
            public DisplayGroupObject(MagicaVoxelFile.SceneGraph node)
            {
                Node = node;
            }
        }
        class DisplayShapeObject : DisplayMagicaVoxelObject
        {
            private TintVertexArrayObject buffer;
            public MagicaVoxelFile.SceneGraph Node { get; private set; }
            public MagicaVoxelFile.Model Chunk { get; private set; }
            public int ModelID { get; private set; }
            public DisplayShapeObject(MagicaVoxelFile.SceneGraph node, int modelID, MagicaVoxelFile.Model chunk)
            {
                Node = node;
                ModelID = modelID;
                Chunk = chunk;
            }
            protected override void OnAdded()
            {
                base.OnAdded();
                if (Chunk.XYZI.NumVoxels > 0)
                {
                    if (World.chunkCaches.TryGetValue(ModelID, out var cache))
                    {
                        buffer = cache;
                    }
                    else
                    {
                        var palette = Chunk.XYZI.Owner.Main.Palette;
                        buffer = new TintVertexArrayObject(PrimitiveType.Quads);
                        buffer.SetShader(World.Shaders.GetOrAdd("Cubes", n => TintVertexArrayObject.CreateDefaultShader()));
                        foreach (var c in Chunk.XYZI.Voxels)
                        {
                            var color = palette.GetColor(c.ColorIndex);
                            buffer.SetColor(
                                new Color4(color.R, color.G, color.B, color.A));
                            buffer.AddBox2D(
                                new Vector3(c.X, c.Y, c.Z),
                                new Vector3(c.X + 1, c.Y + 1, c.Z + 1));
                        }
                        World.chunkCaches.Add(ModelID, buffer);
                    }
                }
            }
            protected override void Disposing()
            {
            }
            protected override void OnRender(PaintEventArgs3D e)
            {
                base.OnRender(e);
                if (World.ShowChunkBounding)
                {
                    var size = Chunk.Size;
                    DrawingObject.DrawBoundingBox(PrimitiveType.LineLoop, Color4.Black,
                        Vector3.Zero, new Vector3(size.SizeX, size.SizeY, size.SizeZ));
                }
                if (buffer != null)
                {
                    var r = Node.Transform.Rotation;
                    var sx = (Chunk.Size.SizeX / 2f);
                    var sy = (Chunk.Size.SizeY / 2f);
                    var sz = (Chunk.Size.SizeZ / 2f);
                    var local_t = Matrix4.CreateTranslation(new Vector3(-sx, -sz, -sy));
                    var local_r = new Matrix4(
                               r.M11, r.M21, r.M31, 0.00f,
                               r.M12, r.M22, r.M32, 0.00f,
                               r.M13, r.M23, r.M33, 0.00f,
                               0.00f, 0.00f, 0.00f, 1.00f);
                    e.MultiplyMatrix(local_t * local_r);
                    buffer?.Draw(e);
                }
            }
        }
#else
        private void InitVOXChilds()
        {
            this.AddDisplayObject(new DisplayShapeObject());
        }
        class DisplayShapeObject : DisplayMagicaVoxelObject
        {
            private VertexArrayObject buffer;
            public DisplayShapeObject()
            {
            }
            protected override void OnAdded()
            {
                base.OnAdded();
                World.VOX.GetAABB(out int minX, out int minY, out int minZ, out int maxX, out int maxY, out int maxZ);
                var xlen = maxX - minX + 1;
                var ylen = maxY - minY + 1;
                var zlen = maxZ - minZ + 1;
                this.World.box = new DeepCore.Geometry.BoundingBox(
                    new DeepCore.Geometry.Vector3(0, 0, 0),
                    new DeepCore.Geometry.Vector3(xlen, ylen, zlen));
                var palette = World.VOX.Main.Palette;
                buffer = new VertexArrayObject(PrimitiveType.Quads);
                buffer.SetShader(World.lightingShader);
                using (var combine = new CubeMeshCombine<int>())
                {
                    World.VOX.ForEachVoxels(c =>
                    {
                        combine.AddCube(c.X, c.Y, c.Z, c.ColorIndex);
                    });
                    combine.Combine();
                    combine.ForEachCubes(c =>
                    {
                        var color = palette.GetColor(c.ColorIndex);
                        var vX = c.X - minX;
                        var vY = ylen - (c.Y - minY);
                        var vZ = c.Z - minZ;
                        buffer.SetColor(
                            new Color4(color.R, color.G, color.B, color.A));
                        buffer.AddBox2D(
                            new Vector3(vX, vY, vZ),
                            new Vector3(vX + 1, vY + 1, vZ + 1));

                    });
                }
            }
            protected override void Disposing()
            {
                buffer?.Dispose();
            }
            protected override void OnRender(PaintEventArgs3D e)
            {
                base.OnRender(e);
                if (buffer != null)
                {
                    buffer?.Draw(e);
                }
            }
        }
#endif
        //-----------------------------------------------------------------------------------------


        //-----------------------------------------------------------------------------------------
    }

    public abstract class DisplayMagicaVoxelObject : GLViewObject3D
    {
        public MagicaVoxelCanvas World { get => base.View as MagicaVoxelCanvas; }

    }

}
