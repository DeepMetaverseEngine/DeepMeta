using DeepCore.GUI.Display;
using DeepEditor.Common;
using System.Threading.Tasks;

namespace DeepCore.GUI.Win32
{
    public class Win32Image : Image
    {
        internal readonly System.Drawing.Bitmap src;
        public System.Drawing.Bitmap Src { get => src; }
        public Win32Image(System.Drawing.Image dimg, string fileName)
        {
            this.FileName = fileName;
            this.src = dimg.AsBitmap();
        }
        protected override void Disposing()
        {
            this.src.Dispose();
        }
        public override float MaxU
        {
            get { return 1; }
        }
        public override float MaxV
        {
            get { return 1; }
        }
        public override int Width
        {
            get { return src.Width; }
        }
        public override int Height
        {
            get { return src.Height; }
        }
        public override void CopyPixels(Image src, int sx, int sy, int sw, int sh, int dx, int dy)
        {
            Win32Image srci = src as Win32Image;
            if (sx + sw <= srci.Width && sy + sh <= srci.Height &&
                dx + sw <= this.Width && dy + sh <= this.Height)
            {
                for (int x = 0; x < sw; x++)
                {
                    for (int y = 0; y < sh; y++)
                    {
                        this.src.SetPixel(dx + x, dy + y, srci.src.GetPixel(sx + x, sy + y));
                    }
                }
            }
        }
        public override void Flush()
        {

        }

    }
}
