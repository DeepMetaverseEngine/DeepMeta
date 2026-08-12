using DeepCore;
using DeepCore.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DeepTools.MPQ
{
    public static class Build
    {
        public static void xcopy(DirectoryInfo workDir, string srcName, string dstName, params string[] types)
        {
            var src = new DirectoryInfo(workDir.FullName + Path.DirectorySeparatorChar + srcName);
            var dst = new DirectoryInfo(workDir.FullName + Path.DirectorySeparatorChar + dstName);
            CFiles.CreateDir(dst);
            if (types.Length == 0)
            {
                var start = new ProcessStartInfo();
                start.WorkingDirectory = workDir.FullName;
                start.FileName = "xcopy";
                start.Arguments = $"/S/Y  \"{src.FullName}\\*.*\"  \"{dst.FullName}\"";
                start.UseShellExecute = false;
                //start.RedirectStandardOutput = true;
                var p = Process.Start(start);
                p.WaitForExit();
            }
            else
            {
                foreach (var type in types)
                {
                    var start = new ProcessStartInfo();
                    start.WorkingDirectory = workDir.FullName;
                    start.FileName = "xcopy";
                    start.Arguments = $"/S/Y  \"{src.FullName}\\{type}\"  \"{dst.FullName}\"";
                    //Console.WriteLine($"{start.FileName} {start.Arguments}");
                    start.UseShellExecute = false;
                    //start.RedirectStandardOutput = true;
                    var p = Process.Start(start);
                    p.WaitForExit();
                }
            }
        }
        public static void copy(DirectoryInfo workDir, string src, string dst)
        {
            CFiles.FileCopyTo(
                Path.Combine(workDir.FullName, src),
                Path.Combine(workDir.FullName, dst), true);
        }

        public static void mpq_make_update(DirectoryInfo workDir, DirectoryInfo binDir, string srcName, string dstName, string dstDirName, FileInfo filterFile, string replace = "")
        {
            var dstDir = workDir.FullName + Path.DirectorySeparatorChar + dstDirName;
            CFiles.CreateDir(dstDir);
            var filter = File.ReadAllText(filterFile.FullName);
            filter = filter.Replace("\r\n", "");
            {
                var start = new ProcessStartInfo();
                start.WorkingDirectory = workDir.FullName;
                start.FileName = "java";
                start.Arguments = $" -classpath {binDir}\\g2d_studio.jar -Xmx1024m FilePackerBatchZlib U {srcName} {dstName} {dstDirName} {filterFile} \"{replace}\"";
                start.UseShellExecute = false;
                var p = Process.Start(start);
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    throw new Exception("FilePackerBatchZlib failed.");
                }
            }
            {
                var start = new ProcessStartInfo();
                start.WorkingDirectory = workDir.FullName;
                start.FileName = "java";
                start.Arguments = $" -classpath {binDir}\\g2d_studio.jar -Xmx1024m GenMD5 --md5 -verbos:s -srcDir:{dstDirName} -dstFile:{dstDirName}/update_version.txt -dstEnc:UTF-8 -filter:+.mpq;+.dir;-update_version.txt;{filter}";
                start.UseShellExecute = false;
                var p = Process.Start(start);
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    throw new Exception("GenMD5 failed.");
                }
            }
            {
                var start = new ProcessStartInfo();
                start.WorkingDirectory = workDir.FullName;
                start.FileName = "java";
                start.Arguments = $" -classpath {binDir}\\g2d_studio.jar -Xmx1024m MakeBeginEnd {dstDirName}/update_version.txt BEGIN END";
                start.UseShellExecute = false;
                var p = Process.Start(start);
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    throw new Exception("MakeBeginEnd failed.");
                }
            }
            {
                var update_version = new List<string>(Resource.LoadAllLines($"{dstDir}/update_version.txt"));
                update_version.Insert(1, $"TIME_UTC {CUtils.FormatTime(DateTime.UtcNow)}");
                CFiles.WriteAllLines($"{dstDir}/update_version.txt", update_version.ToArray(), CUtils.UTF8);
            }
            /*
@echo ----------------------------------------------------------------
@echo 创建所有M3Z更新包 FileSystem
@echo ----------------------------------------------------------------
@echo - 差异化文件更新系统
@echo - 每次点击 update_packer.bat 将自动生成补丁包供客户端下载。
@echo ----------------------------------------------------------------

@SET LIB=%~dp0/../lib

@if not exist updates_png @md updates_png 
@if not exist updates_etc @md updates_etc 
@if not exist updates_pvr @md updates_pvr 
@java -classpath %LIB%/g2d_studio.jar -Xmx1024m FilePackerBatchZlib U mpq ./updates_etc/mpq.mpq ./updates_etc ./filter_update_android.txt "etc.m3z>png;etc.m3z>jpg"
@java -classpath %LIB%/g2d_studio.jar -Xmx1024m FilePackerBatchZlib U mpq ./updates_pvr/mpq.mpq ./updates_pvr ./filter_update_ios.txt     "pvr.m3z>png;pvr.m3z>jpg"

@echo ----------------------------------------------------------------
@echo - 生成更新配置文件 update_version.txt
@echo ----------------------------------------------------------------
@SET SUFFIX=+.mpq;+.assetBundles;+.unity3d;+.ogg;+.ogv;+.mp3;+.mp4;+.wav;+.assetbundle;+.assetbundles;+standalonewindows;+ios;+android;

@java -classpath %LIB%/g2d_studio.jar -Xmx1024m GenMD5 --md5 -verbos:s -srcDir:updates_etc -dstFile:./updates_etc/update_version.txt -dstEnc:UTF-8 -filter:%SUFFIX%
@java -classpath %LIB%/g2d_studio.jar -Xmx1024m GenMD5 --md5 -verbos:s -srcDir:updates_pvr -dstFile:./updates_pvr/update_version.txt -dstEnc:UTF-8 -filter:%SUFFIX%

@java -classpath %LIB%/g2d_studio.jar MakeBeginEnd ./updates_etc/update_version.txt BEGIN END
@java -classpath %LIB%/g2d_studio.jar MakeBeginEnd ./updates_pvr/update_version.txt BEGIN END

@echo ----------------------------------------------------------------
@echo 全部完成！
@echo ----------------------------------------------------------------

pause
             */

        }
    }
}
