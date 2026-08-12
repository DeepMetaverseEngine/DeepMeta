using DeepCore.Voxel.StreamingVoxel.Data;
using DeepEditor.Common.G3D;
using DeepTools.Voxel;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.DisplaySCVX
{

    public class StreamingVoxelCanvas : GLView
    {
        //-----------------------------------------------------------------------------------------------------------------
        private Shader TintShader;
        public StreamingVoxelCanvas(GLControl control, Timer timer) : base(control, timer)
        {
            this.OnRender += StreamingVoxelCanvas_OnRender;
        }
        protected override void GlControl_Load(object sender, EventArgs e)
        {
            base.GlControl_Load(sender, e);
            this.BackColor = Color4.DeepSkyBlue;
            this.TintShader = Shaders.GetOrAdd("LightingShader", n =>
            {
                var ret = new LightingShader();
                ret.OnShaderBegin += (s, e) =>
                {
                    ret.LightPosition = Camera.CamPosition + new Vector3(0, 10, 0);
                };
                return ret;
            });
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
            if (Chunk != null)
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

        public StreamingChunk Chunk;
        private DeepCore.Geometry.BoundingBox box;
        private VertexElementObject viewBuffer;
        public void InitWorld(StreamingChunk chunk)
        {
            this.Chunk = chunk;

            var mesh = StreamingConverter.ConvertToMesh(this.Chunk);
            using (var ms = new MemoryStream())
            {
                StreamingMeshFile.Save(new StreamingMeshFile(mesh), ms);
                ms.Position = 0;
                var meshf = StreamingMeshFile.Load<StreamingMeshFile>(ms);
                mesh = meshf.Chunk;
            }
            this.viewBuffer = new VertexElementObject(
                OpenTK.Graphics.OpenGL.PrimitiveType.Triangles,
                mesh.vertices.ConvertAll(v => v.ToGL()).ToArray(),
                mesh.normals.ConvertAll(v => v.ToGL()).ToArray(),
                mesh.uv.ConvertAll(v => v.ToGL()).ToArray(),
                mesh.colors.ConvertAll(v => new Color4(v.X, v.Y, v.Z, v.W)).ToArray(),
                mesh.triangles.ConvertAll(v => (uint)v).ToArray());
            this.viewBuffer.SetShader(TintShader);
            this.box = new DeepCore.Geometry.BoundingBox(
                new DeepCore.Geometry.Vector3(0, 0, 0),
                new DeepCore.Geometry.Vector3(
                    chunk.ChunkSize.X * chunk.GridCellSize,
                    chunk.ChunkSize.Y * chunk.GridCellSize,
                    chunk.ChunkSize.Z * chunk.GridCellSize));

            ResetCameraPos();
        }
        private void StreamingVoxelCanvas_OnRender(GLView sender, PaintEventArgs3D e)
        {
            DrawingVoxelObject.DrawVoxelAnchor(Vector3.Zero);
            DrawingVoxelObject.DrawGridLines(Color4.White, -2048, -2048, 256, 256, 4096 / 256, 4096 / 256);
            viewBuffer.Draw(e);
        }

        //-----------------------------------------------------------------------------------------

    }

    public abstract class DisplayStreamingObject : GLViewObject3D
    {
        public StreamingVoxelCanvas World { get => base.View as StreamingVoxelCanvas; }

    }

}
