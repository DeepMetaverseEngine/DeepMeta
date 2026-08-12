using ST.Library.UI.NodeEditor;
using System.Drawing;

namespace DeepEditor.Common.EventEditor.BehaviorEditor
{
    public class STBehaviorField : STNodeControl
    {
        protected internal override void OnPaint(DrawingTools dt)
        {
            base.OnPaint(dt);
        }


        //         override 
        //         protected override void OnPaint(DrawingTools dt)
        //         {
        //             //base.OnPaint(dt);
        //             Graphics g = dt.Graphics;
        //             g.FillRectangle(Brushes.Gray, 0, 5, 10, 10);
        //             m_sf.Alignment = StringAlignment.Near;
        //             g.DrawString(this.Text, this.Font, Brushes.LightGray, new Rectangle(15, 0, this.Width - 20, 20), m_sf);
        //             if (this.Checked)
        //             {
        //                 g.FillRectangle(Brushes.Black, 2, 7, 6, 6);
        //             }
        //         }
    }
}
