using DeepCore.Geometry;
using DeepCore.GUI.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common
{
    public class EditorPictureBox : PictureBox
    {
        public EditorPictureBox()
        {
            this.BackgroundImage = DeepEditor.Common.Properties.Resources.canvasBg2;
            this.Canvas = new PictureBoxCanvas(this);
            this.Root = new Win32DisplayRoot(Canvas);
        }
        public PictureBoxCanvas Canvas { get; }
        public Win32DisplayRoot Root { get; }
        public bool RepaintOnMouseHold
        {
            get => Root.RepaintOnMouseHold;
            set => Root.RepaintOnMouseHold = value;
        }
        public Vector3 RootMousePoint
        {
            get
            {
                var e = this.GetMousePoint();
                var pos = new DeepCore.Geometry.Vector3(e.X, e.Y, 0);
                return Root.CanvasLocationToRoot(pos);
            }
        }
      

    }
}
