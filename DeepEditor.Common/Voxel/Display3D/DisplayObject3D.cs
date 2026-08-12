using DeepEditor.Common.G3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepEditor.Common.Voxel.Display3D
{
    public abstract class DisplayObject3D : GLViewObject3D
    {
        new public DisplayTerrain3D View { get => base.View as DisplayTerrain3D; }
    }
}
