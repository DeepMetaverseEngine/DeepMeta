using DeepCore.Reflection;
using DeepEditor.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeepTools.MPQUpdate
{
    class Program
    { 
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ReflectionUtil.LoadDlls(new DirectoryInfo(Application.StartupPath));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Win32.CreateConsole();
            Application.Run(new FormUpdater());
            //new Loader().run();
        }

    }
}
