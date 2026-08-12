using DeepCore;
using DeepCore.Concurrent;
using DeepCore.Reflection;
using DeepCore.Voxel.Data;
using DeepCore.Voxel.Data.PathFinder;
using DeepEditor.Common.G3D;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.Display3D
{
    public class DisplayVoxelWorld3D : DisplayTerrain3D
    {
        private bool mShowPathFinder = false;
        private bool mShowPathFinderBlock = true;

        [Desc("显示寻路网格", "show", true)]
        public bool ShowPathFinder { get => mShowPathFinder; set { if (value != mShowPathFinder) { mShowPathFinder = value; RebuildVoxelTerrain(); } } }

        [Desc("显示寻路网格阻挡", "show", true)]
        public bool ShowPathFinderBlock { get => mShowPathFinderBlock; set { if (value != mShowPathFinderBlock) { mShowPathFinderBlock = value; } } }

        public VoxelWorld World3D { get; private set; }

        public DisplayVoxelWorld3D(GLControl control, Timer timer) : base(control, timer)
        {
        }
//         protected override void Disposing()
//         {
//             base.Disposing();
//             //World3D?.Dispose();
//         }
        protected override void RebuildVoxelTerrain()
        {
            this.InitVoxelTerrain(World3D?.Terrain);
            this.InitVoxelAstar(World3D?.PathMap, World3D?.PathFinder);
        }
        public virtual void InitVoxelWorld(VoxelWorld world)
        {
            this.ClearObjects();
            this.World3D = world;
            this.InitVoxelTerrain(world?.Terrain);
            this.InitVoxelAstar(world?.PathMap, world?.PathFinder);
        }
        public IVoxelAstarMap VoxelPathMap { get => World3D?.PathMap; }
        public IVoxelAstar VoxelPathFinder { get => World3D?.PathFinder; }

        private VertexArrayObject VoxelAstarVBO = new VertexArrayObject(PrimitiveType.Lines, Color4.Wheat);
        private VertexArrayObject VoxelCrossVBO = new VertexArrayObject(PrimitiveType.Lines, Color4.Purple);
        private VertexArrayObject VoxelWeightVBO = new VertexArrayObject(PrimitiveType.Lines, Color4.LightBlue);
        private HashMap<int, VertexArrayObject> VoxelAstarQuardsVBO = new HashMap<int, VertexArrayObject>();
        private HashMap<int, VertexArrayObject> VoxelAstarQuardsPVBO = new HashMap<int, VertexArrayObject>();
        private static readonly Color4[] AreaColorTable = new Color4[] {
            Color4.Cyan,
            Color4.Red,
            Color4.Orange,
            Color4.Yellow,
            Color4.Green,
            Color4.Blue,
            Color4.Purple };
        public virtual void InitVoxelAstar(IVoxelAstarMap map, IVoxelAstar meta)
        {
            this.VoxelWeightVBO.Flush();
            this.VoxelAstarVBO.Flush();
            this.VoxelCrossVBO.Flush();
            this.VoxelAstarQuardsVBO.RunAndClear(e => e.Value.Flush());
            this.VoxelAstarQuardsPVBO.RunAndClear(e => e.Value.Flush());
            if (ShowPathFinder && meta != null && this.VoxelTerrain != null)
            {
                var area_total = new HashMap<int, AtomicInteger>();
                var cellsize = this.VoxelTerrain.GridCellSize;
                var cellr = cellsize / 2f;
                var cellq = cellsize / 8f;
                var steph = World3D.Terrain.BuildConfig.StepIntercept;
                meta.ForEachNodes(0,(node,st) =>
                {
                    {
                        var area = node.CloseAreaIndex;
                        var ai = ((area) % AreaColorTable.Length);
                        var color = AreaColorTable[ai];
                        var src = node.Position;
                        var rect = node.Range.GetRangeSize(cellsize);
                        var list1 = VoxelAstarQuardsVBO.GetOrAdd(ai, flag => new VertexArrayObject(PrimitiveType.Lines, color));
                        list1.AddPlaneLines2D(rect.Location.ToGL(), (rect.Location + rect.Size).ToGL(), src.Z);
                        var list2 = VoxelAstarQuardsPVBO.GetOrAdd(ai, flag => new VertexArrayObject(PrimitiveType.Lines, color.SetAlpha(0.2f)));
                        list2.AddPlane2D(rect.Location.ToGL(), (rect.Location + rect.Size).ToGL(), src.Z);
                        if (node.HasWeight)
                        {
                            var star = DeepCore.Geometry.VectorDrawing.ToStar(src, cellsize);
                            for (int i = 0; i < star.Length; i++)
                            {
                                VoxelWeightVBO.Add2D(star[i].ToGL());
                                VoxelWeightVBO.Add2D(star[(i + 1) % star.Length].ToGL());
                            }
                        }
                    }
                    {
                        node.ForEachNextLinks(0,(next, thisLink, nextLink, st) =>
                        {
                            var src = thisLink.UpwardCenterPos;
                            var dst = nextLink.UpwardCenterPos;
                            VoxelAstarVBO.Add(new Vector3(src.X, src.Z, src.Y));
                            VoxelAstarVBO.Add(new Vector3(dst.X, dst.Z, dst.Y));
                            VoxelAstarVBO.AddPlaneLines2D(
                                (src - new DeepCore.Geometry.Vector3(cellq, cellq, 0)).ToGL().To2D(),
                                (src + new DeepCore.Geometry.Vector3(cellq, cellq, 0)).ToGL().To2D(),
                                src.Z);
                        });
                    }
                });
            }
        }
        protected override void DrawTerrain3D(PaintEventArgs3D e)
        {
            base.DrawTerrain3D(e);
            DrawAstar(e);
            DrawAstarCross(e);
        }

        protected virtual void DrawAstar(PaintEventArgs3D e)
        {
            if (ShowPathFinder && VoxelPathFinder != null)
            {
                foreach (var list in VoxelAstarQuardsVBO.Values)
                {
                    list.Draw();
                }
                foreach (var list in VoxelAstarQuardsPVBO.Values)
                {
                    list.Draw();
                }
                VoxelAstarVBO.Draw();
                VoxelWeightVBO.Draw();
            }
        }
        protected virtual void DrawAstarCross(PaintEventArgs3D e)
        {
            if (ShowPathFinderBlock && VoxelPathFinder is IVoxelAstar meta)
            {
                var cellsize = this.VoxelTerrain.GridCellSize;
                var cellr = cellsize / 2f;
                var cellq = cellsize / 8f;
                var steph = World3D.Terrain.BuildConfig.StepIntercept;
                foreach(var block in meta.GetBlockMapNodes())
                {
                    if (block is IVoxelMapNode node)
                    {
                        var pos = node.Position;
                        var rect = node.Range.GetRangeSize(cellsize);
                        var min = new Vector3(rect.x, rect.y, pos.Z);
                        var max = new Vector3(rect.x + rect.width, rect.y + rect.height, pos.Z + node.Height);
                        DrawingVoxelObject.FillBoundingBox(Color4.Red, min, max);
                    }
                }
            }
        }
        public void AddObject(VoxelObject obj)
        {
            this.AddDisplayVoxel(new DisplayVoxelObject3D<VoxelObject>(obj));
        }
        public void AddDisplayVoxel<T>(DisplayVoxelObject3D<T> obj) where T : VoxelObject
        {
            this.AddDisplayObject(obj);
        }


    }


    //     public class ActorCameraControl : CameraControl
    //     {
    //         protected override void ProcessMouseMove(float xoffset, float yoffset)
    //         {
    //             throw new NotImplementedException();
    //         }
    //         protected override void ProcessMouseWheel(float delta)
    //         {
    //             throw new NotImplementedException();
    //         }
    //         protected override void ProcessQueryKey(TimeSpan elapsed)
    //         {
    //             throw new NotImplementedException();
    //         }
    //     }
}
