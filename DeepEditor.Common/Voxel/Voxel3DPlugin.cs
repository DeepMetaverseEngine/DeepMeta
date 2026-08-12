using DeepCore;
using DeepCore.IO;
using DeepCore.SharpZipLib;
using DeepCore.Voxel;
using DeepCore.Voxel.Data;
using DeepCore.Voxel.Extensions.MagicaVoxel;
using DeepCore.Xml;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel.Display3D;
using DeepTools.Voxel;
using G3D.ObjRenderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeepEditor.Common.Voxel
{
    public abstract class Voxel3DPlugin
    {
        private class Default : Voxel3DPlugin { }
        public static Voxel3DPlugin Instance { get; private set; } = new Default();
        public Voxel3DPlugin()
        {
            Instance = this;
        }

        public virtual VoxelBuildConfig CreateVoxelBuildConfig(VoxelTerrainData tdata = null)
        {
            return VoxelTerrainData.CreateVoxelBuildConfig(tdata);
        }
        public virtual string GetSubVoxelFileName(string voxelFileName)
        {
            return DeepCore.IO.Resource.FormatPath(voxelFileName.Substring(Environment.CurrentDirectory.Length));
        }

        public static void OpenVoxelTest(string voxelBinFile)
        {
            //var voxelBinFile = Main.Editor.GetResourceFullPath(VoxelFileName);
            if (!File.Exists(voxelBinFile))
            {
                MessageBox.Show("体素文件不存在: " + voxelBinFile);
                return;
            }
            var form = new FormVoxelWorldPathTest();
            form.Text = voxelBinFile;
            form.Load += new EventHandler((s2, e2) =>
            {
                form.Enabled = false;
                Task.Run(() =>
                {
                    try
                    {
                        //                         var bin = File.ReadAllBytes(voxelBinFile);
                        //                         var wd = VoxelWorld.LoadFromBin(bin);
                        var wd = VoxelWorld.LoadFromFile(voxelBinFile);
                        form.Invoke(new System.Action(() =>
                        {
                            form.Enabled = true;
                            form.View3D.LoadVoxelWorld(wd, voxelBinFile);
                        }));
                    }
                    catch (Exception err)
                    {
                        form.Invoke(new System.Action(() =>
                        {
                            form.Enabled = true;
                            MessageBox.Show(err.Message);
                        }));
                    }
                });
            });
            form.Show();
        }


        public static bool TryCreateVoxelBuildConfig(IWin32Window window, ref VoxelBuildConfig prop)
        {
            if (prop == null)
            {
                prop = Instance.CreateVoxelBuildConfig(null);
            }
            var pdialog = new G2DDataDialog.G2DObjectDialog<VoxelBuildConfig>(prop);
            if (pdialog.ShowDialog(window) == DialogResult.OK)
            {
                prop = pdialog.SelectedObject;
                if (prop.StepIntercept > prop.VoxelMinDistance)
                {
                    MessageBox.Show(window, "阶梯高度不得大于最小体素间距");
                    return false;
                }
                return true;
            }
            return false;
        }

        public static ZipUtil.OpenStream LoadZipEntry(string path, out string ext)
        {
            var _ext = string.Empty;
            var _ret = ZipUtil.LoadZipEntry(path, e =>
            {
                var name = e.Name.ToLower();
                if (name.EndsWith(".voxt")) { _ext = ".voxt"; return true; }
                if (name.EndsWith(".xml")) { _ext = ".xml"; return true; }
                //if (ext.EndsWith(".json")) { return true; }
                return false;
            });
            ext = _ext;
            return _ret;
        }
        public static ZipUtil.OpenStream LoadZipEntry(byte[] bytes, out string ext)
        {
            var _ext = string.Empty;
            var _ret = ZipUtil.LoadZipEntry(bytes, e =>
            {
                var name = e.Name.ToLower();
                if (name.EndsWith(".voxt")) { _ext = ".voxt"; return true; }
                if (name.EndsWith(".xml")) { _ext = ".xml"; return true; }
                //if (ext.EndsWith(".json")) { return true; }
                return false;
            });
            ext = _ext;
            return _ret;
        }

        public static VoxelTerrainData LoadTerrainDataFromZipFile(string path)
        {
            using (var zstream = LoadZipEntry(path, out var ext))
            {
                switch (ext)
                {
                    case ".voxt":
                        using (var reader = new StreamReader(zstream))
                        {
                            return VoxelTerrainData.LoadFromText(reader);
                        }
                    case ".xml":
                        {
                            var doc = XmlUtil.LoadXML(zstream);
                            return VoxelTerrainData.LoadFromXML(doc);
                        }
                }
                return null;
            }
        }

        public static VoxelTerrainData LoadTerrainData(string path, string ext, Stream stream)
        {
            //var ext = Path.GetExtension(path).ToLower();
            if (ext == ".voxt")
            {
                using (var reader = new StreamReader(stream))
                {
                    return VoxelTerrainData.LoadFromText(reader);
                }
            }
            else if (ext == ".xml")
            {
                var doc = XmlUtil.LoadXML(stream);
                return VoxelTerrainData.LoadFromXML(doc);
            }
            else if (ext == ".zip")
            {
                using (var reader = new StreamReader(stream))
                {
                    return VoxelTerrainData.LoadFromText(reader);
                }
            }
            else if (ext == ".tif")
            {
                var tif = System.Drawing.Image.FromStream(stream);
                var map = tif.AsBitmap();
                var raw = new float[map.Width, map.Height];
                var terrain = new VoxelTerrainData()
                {
                    GridSize = 1,
                    Grids = new VoxelNodeData[map.Width, map.Height][]
                };
                terrain.Grids.InitArray2D(0, (st, x, y) =>
                {
                    var pixel = map.GetPixel(x, y);
                    return new VoxelNodeData[] {
                                    new VoxelNodeData()
                                    {
                                        Color = VoxelTerrainData.FromRGB(pixel.R,pixel.G,pixel.B),
                                        Upward = -10000 + (pixel.R * 256 * 256 + pixel.G * 256 + pixel.B) * 0.1f,
                                        Downward = -10000,
                                    }
                                };
                });
                return terrain;
            }
            else if (ext == ".vox")
            {
                var vox = MagicaVoxelFile.Load(stream);
                return VoxelConverter.ConvertMagicaVoxelFileToVoxelTerrainData(vox);
            }
            return null;
        }
        public static VoxelTerrainData LoadTerrainData(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            if (ext == ".voxt")
            {
                using (var fs = File.OpenRead(path))
                {
                    return LoadTerrainData(path, ext, fs);
                }
            }
            else if (ext == ".xml")
            {
                using (var fs = File.OpenRead(path))
                {
                    return LoadTerrainData(path, ext, fs);
                }
            }
            else if (ext == ".zip")
            {
                using (var zstream = LoadZipEntry(path, out var e_ext))
                {
                    return LoadTerrainData(path, e_ext, zstream);
                }
            }
            else if (ext == ".tif")
            {
                using (var fs = File.OpenRead(path))
                {
                    return LoadTerrainData(path, ext, fs);
                }
            }
            else if (ext == ".vox")
            {
                using (var fs = File.OpenRead(path))
                {
                    return LoadTerrainData(path, ext, fs);
                }
            }
            return null;
        }





        public static bool TryLoadTerrainDataDialog(string path, out VoxelTerrainData data)
        {
            var ext = Path.GetExtension(path).ToLower();
            var title = "解析 : " + Path.GetFileName(path);
            data = null;
            try
            {
                if (ext == ".xml")
                {
                    var bytes = File.ReadAllBytes(path);
                    new G2DProgressDialog<VoxelTerrainData>(new StreamProgress(title, new System.IO.MemoryStream(bytes),
                        (stream) => LoadTerrainData(path, ext, stream))).ShowDialog(out data);
                }
                else if (ext == ".voxt")
                {
                    var bytes = File.ReadAllBytes(path);
                    new G2DProgressDialog<VoxelTerrainData>(new StreamProgress(title, new System.IO.MemoryStream(bytes),
                        (stream) => LoadTerrainData(path, ext, stream))).ShowDialog(out data);
                }
                else if (ext == ".zip")
                {
                    var bytes = File.ReadAllBytes(path);
                    using (var zstream = LoadZipEntry(bytes, out var e_ext))
                    {
                        new G2DProgressDialog<VoxelTerrainData>(new StreamProgress(title, zstream,
                            (stream) => LoadTerrainData(path, e_ext, stream))).ShowDialog(out data);
                    }
                }
                else if (ext == ".tif")
                {
                    var bytes = File.ReadAllBytes(path);
                    new G2DProgressDialog<VoxelTerrainData>(new StreamProgress(title, new System.IO.MemoryStream(bytes),
                        (stream) => LoadTerrainData(path, ext, stream))).ShowDialog(out data);
                }
                else if (ext == ".vox")
                {
                    new G2DProgressDialog<VoxelTerrainData>(title, progress =>
                    {
                        var vox = MagicaVoxelFile.Load(new FileInfo(path));
                        return VoxelConverter.ConvertMagicaVoxelFileToVoxelTerrainData(vox, progress);
                    }).ShowDialog(out data);
                }
            }
            catch (Exception e)
            {
                e.ShowMessageBox();
            }
            return data != null;
        }
        public static bool TryLoadTerrainDataDialog(IWin32Window window, out string voxelFile, out VoxelTerrainData voxelData)
        {
            var fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.Filter =
                "所有文件|*.*|" +
                "体素打包ZIP|*.zip|" +
                "体素导出XML|*.xml|" +
                "体素导出JSON|*.json|" +
                "体素导出VOXT|*.voxt|" +
                "高程位图GTIFF|*.tif|" +
                "MagicaVoxel|*.vox|" +
                "所有文件|*.*";
            fd.DefaultExt = "*";
            fd.InitialDirectory = Environment.CurrentDirectory;
            if (fd.ShowDialog(window) == DialogResult.OK)
            {
                try
                {
                    voxelFile = fd.FileName;
                    return TryLoadTerrainDataDialog(voxelFile, out voxelData);
                }
                catch (Exception err)
                {
                    err.ShowMessageBox(window);
                }
            }
            voxelFile = null;
            voxelData = null;
            return false;
        }

        public static bool TryLoadNewVoxelWorldDialog(IWin32Window window, out string voxelXmlFile, out string voxelBinFile, out VoxelWorld voxelWorld)
        {
            voxelWorld = null;
            voxelBinFile = null;
            voxelXmlFile = null;
            try
            {
                if (TryLoadTerrainDataDialog(window, out voxelXmlFile, out var tdata))
                {
                    var prop = Instance.CreateVoxelBuildConfig(tdata);
                    if (TryCreateVoxelBuildConfig(window, ref prop))
                    {
                        return TryConvertWorldDialog(window, voxelXmlFile, tdata, prop, out voxelBinFile, out voxelWorld);
                    }
                }
            }
            catch (Exception err)
            {
                err.ShowMessageBox(window);
            }
            return false;
        }
        public static bool TryConvertWorldDialog(IWin32Window window, string voxelXmlFile, VoxelBuildConfig prop, out string voxelBinFile, out VoxelWorld voxelWorld)
        {
            return TryConvertWorldDialog(window, voxelXmlFile, null, prop, out voxelBinFile, out voxelWorld);
        }
        public static bool TryConvertWorldDialog(IWin32Window window, string voxelXmlFile, VoxelTerrainData data, VoxelBuildConfig prop, out string voxelBinFile, out VoxelWorld voxelWorld)
        {
            FileInfo vf = new FileInfo(voxelXmlFile);
            VoxelWorld retWorld = null;
            var progress = new G2DProgressDialog<string>(("体素转换中 : " + vf.Name), ((percent) =>
            {
                percent.SetMin(0);
                percent.SetMax(1);
                try
                {
                    if (data == null)
                    {
                        data = LoadTerrainData(voxelXmlFile);
                        percent.SetValue(1);
                    }
                    var terrain = new VoxelTerrain3D(data, prop, percent);
                    data = null;

                    var astar = VoxelWorldManager.Instance.CreateVoxelAstar(terrain, percent);
                    retWorld = new VoxelWorld(voxelXmlFile, terrain, astar);

                    TryCombineMesh(voxelXmlFile, retWorld);

                    var voxelFileName = Path.GetFullPath(vf.Directory.FullName + Path.DirectorySeparatorChar + vf.Name + VoxelWorld.FILE_EXT);
                    VoxelWorld.SaveToFile(retWorld, voxelFileName);

                    return Instance.GetSubVoxelFileName(voxelFileName);
                    //return DeepCore.IO.Resource.FormatPath(voxelFileName.Substring(Main.Editor.EditorRootDir.Length));
                }
                catch (Exception err)
                {
                    if (window is Control form)
                    {
                        form.Invoke(new System.Action(() =>
                        {
                            err.ShowMessageBox(window);
                        }));
                    }
                }
                return null;
            }));
            if (progress.ShowDialog(window, out voxelBinFile) == DialogResult.OK)
            {
                voxelWorld = retWorld;
                return voxelBinFile != null;
            }
            voxelWorld = null;
            return false;
        }

        public static bool TryRebuildWorldDialog(IWin32Window window, DirectoryInfo vd, VoxelWorld wd, out VoxelWorld voxelWorld)
        {
            var progress = new G2DProgressDialog<VoxelWorld>(("体素转换中 : " + wd.FileName), ((percent) =>
            {
                percent.SetMin(0);
                percent.SetMax(1);
                try
                {
                    var terrain = wd.Terrain;

                    var astar = VoxelWorldManager.Instance.CreateVoxelAstar(terrain, percent);
                    var retWorld = new VoxelWorld(wd.FileName, terrain, astar);

                    var voxelFileName = Path.GetFullPath(vd.FullName + Path.DirectorySeparatorChar + wd.FileName + VoxelWorld.FILE_EXT);
                    VoxelWorld.SaveToFile(retWorld, voxelFileName);

                    return retWorld;
                }
                catch (Exception err)
                {
                    if (window is Control form)
                    {
                        form.Invoke(new System.Action(() =>
                        {
                            err.ShowMessageBox(window);
                        }));
                    }
                }
                return null;
            }));
            if (progress.ShowDialog(window, out voxelWorld) == DialogResult.OK)
            {
                return voxelWorld != null;
            }
            voxelWorld = null;
            return false;
        }

        public static bool TryCombineMesh(string voxelXmlFile, VoxelWorld world)
        {
            if (world.TryGetAttributeAs<ObjLoaderConfig>(nameof(ObjLoaderConfig), out var meshcfg) == false)
            {
                meshcfg = new ObjLoaderConfig()
                {
                    ScaleX = -1,
                    ScaleZ = -1,
                    TranslationZ = -(world.Terrain.TotalSizeY),
                    TerrainWidth = (world.Terrain.TotalSizeX),
                    TerrainHeight = (world.Terrain.TotalSizeY),
                };
            }
            if (voxelXmlFile.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                var objstream = ZipUtil.LoadZipEntry(voxelXmlFile, e => e.Name.EndsWith(".obj", StringComparison.OrdinalIgnoreCase));
                if (objstream != null)
                {
                    using (objstream)
                    {
                        var mesh = ObjLoader.Load(objstream, meshcfg);
                        var triangles = mesh.ToVoxelTriangles(world.Terrain);
                        world.PathMap.CombineMesh(triangles, meshcfg.PathWeight);
                        world.Attributes.Put(nameof(ObjLoaderConfig), meshcfg);
                        return true;
                    }
                }
            }
            return false;
        }
        /*
        public static bool TryCombineMeshDialog(string voxelXmlFile, VoxelWorld world)
        {
            if (voxelXmlFile.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                var objstream = ZipUtil.LoadZipEntry(voxelXmlFile, e => e.Name.EndsWith(".obj", StringComparison.OrdinalIgnoreCase));
                if (objstream != null)
                {
                    var terrain = world.Terrain;
                    var meshcfg = new G3D.ObjRenderer.ObjLoaderConfig()
                    {
                        ScaleX = -1,
                        ScaleZ = -1,
                        TranslationZ = -(terrain.TotalSizeY),
                        TerrainWidth = (terrain.TotalSizeX),
                        TerrainHeight = (terrain.TotalSizeY),
                    };
                    using (objstream)
                    {
                        var pdialog = new G2DObjectDialog<ObjLoaderConfig>(meshcfg);
                        if (pdialog.ShowDialog() == DialogResult.OK)
                        {
                            meshcfg = pdialog.SelectedObject;
                            try
                            {
                                var mesh = ObjLoader.Load(objstream, meshcfg);
                                var triangles = mesh.ToVoxelTriangles(terrain);
                                world.PathFinder.CombineMesh(triangles, meshcfg.TerrainPathWeight);
                                return true;
                            }
                            catch (Exception err)
                            {
                                err.ShowMessageBox();
                            }
                        }
                    }
                }
            }
            return false;
        }
        */

        public static VoxelTerrainData GenRandomTerrain(int totalW, int totalH, float maxAltitude, float maxDeepness, float grid = 1f)
        {
            var ret = new VoxelTerrainData();
            ret.GridSize = grid;
            ret.Grids = new VoxelNodeData[totalW, totalH][];
            CUtils.ForEach2D(totalW, totalH, (cx, cy) =>
            {

            });
            return ret;
        }
    }
}
