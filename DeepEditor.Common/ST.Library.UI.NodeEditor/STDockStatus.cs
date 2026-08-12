using System.Drawing;
using System.Drawing.Drawing2D;

namespace ST.Library.UI.NodeEditor
{
    public enum STDockStatus
    {
        NA = 0,
        Input = 1,
        Output = 2,
        All = 3,
    }
    public enum STOptionDotStyle
    {
        Empty = 0,
        Fill = 1,
        Draw = 2,
    }
    public delegate void DrawOptionDot(DrawingTools dt, STNodeOption op, Rectangle bounds);
    public delegate void DrawOptionText(DrawingTools dt, STNodeOption op, Rectangle bounds);
    public delegate void DrawOptionBezier(DrawingTools dt, STNodeOption op, STNodeOption next);


    //     public interface ISTGroupNode
    //     {
    //         STNode Node { get; }
    //     }
}
