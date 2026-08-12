using DeepCore.Game3D.Slave.Layer;
using DeepMetaGame.Data;
using DeepMetaGame.Slave.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DeepGameEditor3D.Common
{
    public static class LayerDisplay
    {


        public static string ToStatusText(this LayerUnit unit) => ZoneGUIExt.ToStatusText(unit);
    }
}
