using DeepCore.Geometry;
using DeepCore.GUI.SceneGraph;
using System;
using System.Windows.Forms;

namespace DeepCore.GUI.Win32
{
    public class Win32PictureBox : PictureBox
    {
        public Win32DisplayRoot RootNode { get; }
        public bool RepaintOnMouseHold
        {
            get => RootNode.RepaintOnMouseHold;
            set => RootNode.RepaintOnMouseHold = value;
        }
        public Vector3 RootMousePoint
        {
            get
            {
                var e = this.GetMousePoint();
                var pos = new DeepCore.Geometry.Vector3(e.X, e.Y, 0);
                return RootNode.CanvasLocationToRoot(pos);
            }
        }
        public Win32PictureBox()
        {
            this.BackgroundImage = DeepEditor.Common.Properties.Resources.canvasBg2;
            this.RootNode = new Win32DisplayRoot(new PictureBoxCanvas(this));
        }
        //-----------------------------------------------------------------------------------------------------
//         public event Action<DisplayNode, object> OnPostToEditor;
//         public void PostToEditor(DisplayNode e, object arg)
//         {
//             OnPostToEditor?.Invoke(e, arg);
//         }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------


    }
}
