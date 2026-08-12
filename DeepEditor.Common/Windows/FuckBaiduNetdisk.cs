using Microsoft.Win32;
using System;
using System.Windows.Forms;

//<requestedExecutionLevel level = "requireAdministrator" uiAccess="false" />
public static class FuckBaiduNetdisk
{
    public static string FUCK = @"
....................../´¯/)
....................,/¯../
.................../..../
............./´¯/'...'/´¯¯`·¸
........../'/.../..../......./¨¯\
........('(...´...´.... ¯~/'...')
.........\.................'...../
..........''...\.......... _.·´
............\..............(
..............\.............\..
";
    public static void Launch()
    {
        try
        {
            var exe_file = new System.IO.FileInfo(typeof(FuckBaiduNetdisk).Assembly.Location);
            //创建启动对象
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = exe_file.FullName.Replace(".dll", ".exe");
            startInfo.Arguments = "baidu";
            //设置启动动作,确保以管理员身份运行
            startInfo.Verb = "runas";
            Console.WriteLine(startInfo.FileName);
            System.Diagnostics.Process.Start(startInfo);
        }
        catch (Exception err)
        {
            err.PrintStackTrace();
        }
    }

    public static bool Main()
    {
        //.WorkspaceExt0]
        //.WorkspaceExt1]
        //.WorkspaceExt2]
        //.WorkspaceExt3]
        //.WorkspaceExt4]
        try
        {
            /**
             * 当前用户是管理员的时候，直接启动应用程序
             * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
             */
            //获得当前登录的Windows用户标示
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            //判断当前登录用户是否为管理员
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                //如果是管理员，则直接运行
                DeleteKey();
                return true;
            }
            else
            {
                Console.WriteLine("restart " + Application.ExecutablePath);
                //创建启动对象
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = Application.ExecutablePath;
                //设置启动动作,确保以管理员身份运行
                startInfo.Verb = "runas";
                try
                {
                    System.Diagnostics.Process.Start(startInfo);
                }
                catch
                {
                }
            }
        }
        catch (Exception err)
        {
            Console.Error.WriteLine(err.Message);
        }
        return false;
    }

    const string BaiduKeyName = ".WorkspaceExt";
    public static void DeleteKey()
    {
        //计算机\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ShellIconOverlayIdentifiers
        var key = Registry.LocalMachine;
        var shellIconOverlayIdentifiers = key
            .OpenSubKey("SOFTWARE", true)
            .OpenSubKey("Microsoft", true)
            .OpenSubKey("Windows", true)
            .OpenSubKey("CurrentVersion", true)
            .OpenSubKey("Explorer", true)
            .OpenSubKey("ShellIconOverlayIdentifiers", true);
        var subkeyNames = shellIconOverlayIdentifiers.GetSubKeyNames(); 
        Console.WriteLine($"Find all [{BaiduKeyName}]");
        foreach (string keyName in subkeyNames)
        {
            if (keyName.Trim().ToLower().Contains(BaiduKeyName.ToLower()))
            {
                shellIconOverlayIdentifiers.DeleteSubKey(keyName, true);
                Console.WriteLine($"Delete [{keyName}]");
            }
        }
        key.Close();
        Console.WriteLine(FUCK);
    }
}
