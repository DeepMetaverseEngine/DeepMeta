using DeepCore.Astar;
using DeepCore.Reflection;
using DeepCore.Voxel.Data;
using DeepEditor.Common.Controls;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel.Display3D;
using G3D.ObjRenderer;
using OpenTK.Mathematics;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.Display3D
{
    public partial class FormVoxelWorldPathTest : DeepEditor.Common.G2D.G2DBaseForm
    {
        public PanelVoxelWorldPathTest View3D { get => this.panelVoxelView3d1; }
        public FormVoxelWorldPathTest()
        {
            InitializeComponent();
        }

    }
}
