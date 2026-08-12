using System.Drawing;

namespace MaterialSkin
{
    /// <summary>
    /// Defines the <see cref="IMaterialControl" />
    /// </summary>
    public interface IMaterialControl
    {
        int Depth { get; set; }

        MaterialSkinManager SkinManager { get; }

        MouseState MouseState { get; set; }
    }

    /// <summary>
    /// Defines the MouseState
    /// </summary>
    public enum MouseState
    {
        /// <summary>
        /// Defines the HOVER
        /// </summary>
        HOVER,

        /// <summary>
        /// Defines the DOWN
        /// </summary>
        DOWN,

        /// <summary>
        /// Defines the OUT
        /// </summary>
        OUT
    }
}