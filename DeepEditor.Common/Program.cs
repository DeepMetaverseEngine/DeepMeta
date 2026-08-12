using DeepCore.IO;
using DeepCore.Voxel.Data;
using DeepCore.Voxel.Extensions.MagicaVoxel;
using DeepCore.Voxel.StreamingVoxel.Data;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Space;
using DeepEditor.Common.Voxel.Display3D;
using DeepEditor.Common.Voxel.DisplayMagicaVoxel;
using DeepEditor.Common.Voxel.DisplaySCVX;
using DeepEditor.Common.Voxel.DisplayTerrainData;
using DeepTools.Voxel;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    var prop = DeepCore.Properties.ParseArgs(args);
                    if (TryFuckBaidu(args[0], prop))
                    {
                    }
                }
                catch (Exception err)
                {
                    err.PrintStackTrace();
                }
            }
            Console.WriteLine("Press Any Key !!!");
            Console.ReadLine();
        }

        public static bool TryFuckBaidu(string cmd, DeepCore.Properties prop)
        {
            if (cmd == "baidu")
            {
                Console.WriteLine(cmd);
                return FuckBaiduNetdisk.Main();
            }
            return false;
        }


        public static bool TryConvertFile(string cmd, DeepCore.Properties prop)
        {
            if (cmd == "voxsnaps")
            {
                return voxsnaps(prop);
            }
            return false;
        }
        public static bool TryOpenFile(string cmd, DeepCore.Properties prop)
        {
            try
            {
                var file = new FileInfo(cmd);
                if (file.Exists)
                {
                    Environment.CurrentDirectory = file.Directory.FullName;
                    using (var fs = new InputStream(new System.IO.MemoryStream(File.ReadAllBytes(file.FullName)), Codec.Instance))
                    {
                        //----------------------------------------------------------------------------------
                        if (fs.TryPickFileHeadASCII(MagicaVoxelFile.FILE_HEAD))
                        {
                            var vox = MagicaVoxelFile.Load(fs);
#if USE_VOXW
                            var world = vox.ConvertMagicaVoxelFileToVoxelWorld();
                            var dialog = new FormVoxelViewer();
                            dialog.Load += (s, e) =>
                            {
                                dialog.LoadVoxelWorld(world);
                                dialog.Text = file.FullName;
                            };
                            Application.Run(dialog);
#else
                            var dialog = new FormMagicaVoxelViewer();
                            dialog.Load += (s, e) =>
                            {
                                dialog.InitVoxel(vox);
                                dialog.Text = file.FullName;
                            };
                            Application.Run(dialog);
#endif
                            return true;
                        }
                        //----------------------------------------------------------------------------------
                        else if (fs.TryPickFileHead(VoxelWorld.FILE_HEAD))
                        {
                            var world = VoxelWorld.LoadFromStream(fs);
                            var dialog = new FormVoxelViewer();
                            dialog.Load += (s, e) =>
                            {
                                dialog.LoadVoxelWorld(world);
                                dialog.Text = file.FullName;
                            };
                            Application.Run(dialog);
                            return true;
                        }
                        //----------------------------------------------------------------------------------
                        else if (fs.TryPickFileHeadASCII(StreamingVoxChunkFile.FILE_HEAD))
                        {
                            var scvx = StreamingVoxChunkFile.Load<StreamingVoxChunkFile>(fs);
                            var dialog = new FormStreamingVoxelViewer();
                            dialog.Load += (s, e) =>
                            {
                                dialog.InitVoxelAdapter(scvx.Chunk);
                                dialog.Text = file.FullName;
                            };
                            Application.Run(dialog);
                            return true;
                        }
                        //----------------------------------------------------------------------------------
                        else if (file.Extension.ToLower() == ".zip")
                        {
                            var dialog = new FormVoxelCreater();
                            dialog.Load += (s, e) =>
                            {
                                dialog.LoadZipFile(file);
                                dialog.Text = file.FullName;
                            };
                            Application.Run(dialog);
                            return true;
                        }
                        //----------------------------------------------------------------------------------
                        else if (file.Extension.ToLower() == ".png" || file.Extension.ToLower() == ".jpg")
                        {
                            var img = Image.FromFile(file.FullName);
                            var dialog = new FormCarmackScroll2D();
                            dialog.Load += (s, e) =>
                            {
                                dialog.SetScrollImage(img);
                                dialog.Text = file.FullName;
                            };
                            Application.Run(dialog);
                            return true;
                        }
                        //----------------------------------------------------------------------------------
                        else if (file.Extension.ToLower() == ".glb")
                        {
                            //GLTF.LoadFile(file.FullName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.PrintStackTrace();
            }
            return false;
        }

        public static bool voxsnaps(DeepCore.Properties prop)
        {
            var ext = ".vox";
            if (prop.TryGetValue(ext, out var _ext))
            {
                ext = _ext;
            }
            var files = CFiles.ListAllFiles(new DirectoryInfo(Environment.CurrentDirectory));
            foreach (var f in files.ToArray())
            {
                if (f.Extension != ext)
                {
                    files.Remove(f);
                }
            }
            var viewer = new FormVoxelViewer();
            viewer.Load += (s, e) =>
            {
                new G2DProgressDialog("voxsnaps", range =>
                {

                    range.SetRange(0, files.Count, 0);
                    foreach (var f in files)
                    {
                        Console.WriteLine(f.FullName); try
                        {


                            try
                            {
                                using (var fs = f.OpenInputStream())
                                {
                                    VoxelWorld world = null;
                                    if (fs.TryPickFileHead(VoxelWorld.FILE_HEAD))
                                    {
                                        world = VoxelWorld.LoadFromStream(fs);
                                    }
                                    else if (fs.TryPickFileHeadASCII(MagicaVoxelFile.FILE_HEAD))
                                    {
                                        var vox = MagicaVoxelFile.Load(fs);
                                        world = vox.ConvertMagicaVoxelFileToVoxelWorld();
                                    }
                                    if (world != null)
                                    {
                                        var tcs = new TaskCompletionSource<Bitmap>();
                                        viewer.InvokeAsync(() =>
                                        {
                                            try
                                            {
                                                viewer.LoadVoxelWorld(world);
                                            }
                                            catch (Exception e1)
                                            {
                                                tcs.TrySetException(e1);
                                            }
                                        }).Wait();
                                        Task.Delay(100).ContinueWith(t =>
                                        {
                                            viewer.InvokeAsync(() =>
                                            {
                                                try
                                                {
                                                    var bitmap = viewer.Viewer.Canvas.TakeSnap();
                                                    bitmap.Save(f.FullName + ".png", ImageFormat.Png);
                                                    tcs.TrySetResult(bitmap);
                                                }
                                                catch (Exception e2)
                                                {
                                                    tcs.TrySetException(e2);
                                                }
                                            });
                                        }).Wait();
                                        tcs.Task.Wait();
                                    }
                                }
                            }
                            finally
                            {
                                range.Add(1);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.PrintStackTrace();
                        }
                    }
                    viewer.InvokeAsync(() =>
                    {
                        viewer.Dispose();
                    });
                }).Show();
            };
            Application.Run(viewer);
            return true;
        }
    }
}
