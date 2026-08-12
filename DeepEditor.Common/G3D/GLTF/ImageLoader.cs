using FreeImageAPI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DeepEditor.Common.G3D.GLTF
{

    public class FileGroup
    {
        public string[] filenames;
        private FileGroup(int size)
        {
            filenames = new string[size];
        }

        public static FileGroup groupImagesCubeMap()
        {
            FileGroup result = new FileGroup(6);
            result.filenames[0] = "right.jpg";
            result.filenames[1] = "left.jpg";
            result.filenames[2] = "top.jpg";
            result.filenames[3] = "bottom.jpg";
            result.filenames[4] = "front.jpg";//this bitmap actually is back. +z
            result.filenames[5] = "back.jpg";
            return result;
        }
    }

    public class ImageLoader
    {
        public static ImageData2D Load(byte[] imgdata)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            memoryStream.Write(imgdata, 0, imgdata.Length);
            FIBITMAP fibmp = FreeImage.LoadFromStream(memoryStream);
            var width = FreeImage.GetWidth(fibmp);
            var height = FreeImage.GetHeight(fibmp);
            var format = FreeImage.GetPixelFormat(fibmp);
            Bitmap bmp = FreeImage.GetBitmap(fibmp);
            ImageData2D imgData = new ImageData2D(width,height,format);
            imgData.FillData(bmp);
            memoryStream.Close();
            return imgData;
        }

        public static ImageData2D[] LoadCubeMap(string path, FileGroup fg)
        {
            ImageData2D[] imgCubeMap = new ImageData2D[6];
            if(!path.EndsWith('\\'))
                path+='\\';
            for(int i =0;i<6;i++)
            {
                string filename = fg.filenames[i];
                FIBITMAP fibmp = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_JPEG,path + filename,FREE_IMAGE_LOAD_FLAGS.DEFAULT);
                var width = FreeImage.GetWidth(fibmp);
                var height = FreeImage.GetHeight(fibmp);
                var format = FreeImage.GetPixelFormat(fibmp);
                Bitmap bmp = FreeImage.GetBitmap(fibmp);
                ImageData2D imgData = new ImageData2D(width,height,format);
                imgData.FillData(bmp);
                imgCubeMap[i] = imgData;
            }
            return imgCubeMap;
        }

        //ProcessEquirectangularMap
        public static ImageData2D LoadHDRFile(string fileHDR)
        {
            FIBITMAP fibmp = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_HDR,fileHDR,FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            var width = FreeImage.GetWidth(fibmp);
            var height = FreeImage.GetHeight(fibmp);
            var format = FreeImage.GetPixelFormat(fibmp);
            var line = FreeImage.GetLine(fibmp);
            //Bitmap bmp = FreeImage.GetBitmap(fibmp);
            ImageData2D imgData = new ImageData2D(width,height,line);
            for(int i =0;i<height;i++)
            {
                var ptr = FreeImage.GetScanLine(fibmp,i);
                Marshal.Copy(ptr,imgData.pixels,(int)(i * line),(int)line);
            }
            return imgData;
        }

        private static void SavePNG_rg16f(byte[] data,int w,int h, string filename)
        {
            FIBITMAP dib;
            // dib = FreeImage.Allocate(w,h,16);
            dib = FreeImage.ConvertFromRawBits(data,w,h,w*2,16,0xff,0xff00,0xff0000,false);
            FreeImage.Save(FREE_IMAGE_FORMAT.FIF_PNG,dib,filename,FREE_IMAGE_SAVE_FLAGS.DEFAULT);
        }

        public static void SavePNG(byte[] data,int w,int h, string filename, OpenTK.Graphics.OpenGL4.PixelInternalFormat pif)
        {
            switch(pif)
            {
                case OpenTK.Graphics.OpenGL4.PixelInternalFormat.Rg16f:
                SavePNG_rg16f(data,w,h,filename);
                break;
            }

        }
    }
}