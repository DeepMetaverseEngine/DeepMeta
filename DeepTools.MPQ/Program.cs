using DeepCore.IO;
using System;
using System.IO;
using System.Windows.Forms;

namespace DeepTools.MPQ
{
    static class Program
    {
        public static string Usage
        {
            get { return @"Usage key: 
    E InputMPQFile
    解压缩MPQ文件

    Z InputDirectory OutputZipFile FileFilter
    压缩过滤器符合的文件
"; }
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            try
            {
                if (args.Length >= 1)
                {
                    switch (args[0].ToUpper())
                    {
                        case "E":
                            {
                                var file = args[1];
                                if (File.Exists(file) && file.ToLower().EndsWith(".mpq"))
                                {
                                    Application.EnableVisualStyles();
                                    Application.SetCompatibleTextRenderingDefault(false);
                                    Application.Run(FormUnarchive.OpenUnarchive(new FileInfo(file)));
                                    return;
                                }
                            }
                            break;
                        case "Z":
                            {
                                if (Directory.Exists(args[1]) && File.Exists(args[2]) && File.Exists(args[3]))
                                {
                                    ZipFiles(new DirectoryInfo(args[1]), new FileFilters(File.ReadAllText(args[2])), new FileInfo(args[3]));
                                    return;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var main = new FormMain();
                if (args.Length > 0)
                {
                    if (File.Exists(args[0]))
                    {
                        main.LoadMPQ(new FileInfo(args[0]));
                    }
                }
                Application.Run(main);
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
            Console.WriteLine(Usage);
        }
        public static void ZipFiles(DirectoryInfo root, FileFilters filters, FileInfo dst)
        {
            var files = CFiles.ListAllFiles(root, filters);
            using (var s = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(dst.OpenWrite()))
            {
                s.SetLevel(9);
                byte[] buffer = new byte[4096];
                foreach (var file in files)
                {
                    var entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(root.GetSuffixPath(file));
                    entry.DateTime = DateTime.MinValue;
                    s.PutNextEntry(entry);
                    Console.WriteLine("PutNextEntry : " + entry.Name);
                    using (var fs = file.OpenRead())
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Close();
            }
        }
    }


}
