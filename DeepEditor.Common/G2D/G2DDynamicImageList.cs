using DeepEditor.Common.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public static class G2DDynamicImageList
    {

        public static ImageList CreateImageList(Size size, params ValueTuple<string, Image>[] images)
        {
            var imageList = new ImageList();
            imageList.ImageSize = size;
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            foreach (var img in images)
            {
                imageList.Images.Add(img.Item1, img.Item2);
            }
            return imageList;
        }
        public static ImageList Clone(this ImageList clone, Size size)
        {
            var imageList = new ImageList();
            imageList.TransparentColor = clone.TransparentColor;
            imageList.ColorDepth = clone.ColorDepth;
            imageList.ImageSize = size;
            foreach (var key in clone.Images.Keys)
            {
                imageList.Images.Add(key, clone.Images[key]);
            }
            return imageList;
        }
        public static ImageList Clone(this ImageList clone)
        {
            var imageList = new ImageList();
            imageList.TransparentColor = clone.TransparentColor;
            imageList.ColorDepth = clone.ColorDepth;
            imageList.ImageSize = clone.ImageSize;
            foreach (var key in clone.Images.Keys)
            {
                imageList.Images.Add(key, clone.Images[key]);
            }
            return imageList;
        }
    }


}
