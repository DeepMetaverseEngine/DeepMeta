using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace DeepEditor.Common.Windows
{
#if true
    public static class ShutdownHook
    {
        public static event Action<CtrlType> ApplicationClosing
        {
            add { { AddShutdownHook(value); } }
            remove { RemoveShutdownHook(value); }
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);
        private static List<Action<CtrlType>> event_OnCloseEvent;
        private static object lockObject = new object();
        private static void Handler(CtrlType sig)
        {
            var evt = new List<Action<CtrlType>>();
            lock (lockObject)
            {
                if (event_OnCloseEvent != null)
                {
                    evt.AddRange(event_OnCloseEvent);
                    event_OnCloseEvent.Clear();
                }
            }
            foreach (var e in evt)
            {
                try
                {
                    e.Invoke(sig);
                }
                catch (Exception err)
                {
                    Console.Error.WriteLine(err.Message + Environment.NewLine + err.StackTrace);
                }
            }
        }
        static private void AddShutdownHook(Action<CtrlType> action)
        {
            lock (lockObject)
            {
                if (event_OnCloseEvent == null)
                {
                    event_OnCloseEvent = new List<Action<CtrlType>>();
                    SetConsoleCtrlHandler((sig) =>
                    {
                        Console.WriteLine($"ConsoleCtrlHandler {sig}");
                        switch (sig)
                        {
                            case CtrlType.CTRL_CLOSE_EVENT:
                            case CtrlType.CTRL_C_EVENT:
                                Handler(sig);
                                return false;
                        }
                        return false;
                    }, true);
                    AppDomain.CurrentDomain.ProcessExit += (sender, evt) =>
                    {
                        try
                        {
                            Console.WriteLine("AppDomain.CurrentDomain.ProcessExit");
                        }
                        catch { }
                        Handler(CtrlType.APP_DOMAIN_PROCESS_EXIT);
                    };
                    Process.GetCurrentProcess().EnableRaisingEvents = true;
                }
                event_OnCloseEvent.Add(action);
            }
        }
        static private void RemoveShutdownHook(Action<CtrlType> action)
        {
            lock (lockObject)
            {
                if (event_OnCloseEvent != null)
                {
                    event_OnCloseEvent.Remove(action);
                }
            }
        }
        public enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6,
            APP_DOMAIN_PROCESS_EXIT = 1000,
        }
        public static void DisableConsoleCloseButton()
        {
            ConsoleWin32Helper.DisableCloseButton(Process.GetCurrentProcess().MainWindowHandle);
        }
    }

    /*
* 控制台禁用关闭按钮并最小化到系统托盘演示
*
* 通过ConsoleWin32类来进行控制
* 添加引用 System.Runtime.InteropServices; 和 System.Threading; 用于禁用关闭按钮
* 添加引用 System.Drawing; 和 System.Windows.Forms; 用于系统托盘
*
*/


    //         class Program
    //         {
    //             static bool _IsExit = false;
    // 
    //             static void Main(string[] args)
    //             {
    //                 Console.Title = " TestConsoleLikeWin32 ";
    //                 ConsoleWin32Helper.ShowNotifyIcon();
    //                 ConsoleWin32Helper.DisableCloseButton(Console.Title);
    // 
    //                 Thread threadMonitorInput = new Thread(new ThreadStart(MonitorInput));
    //                 threadMonitorInput.Start();
    // 
    //                 while (true)
    //                 {
    //                     Application.DoEvents();
    //                     if (_IsExit)
    //                     {
    //                         break;
    //                     }
    //                 }
    //             }
    // 
    //             static void MonitorInput()
    //             {
    //                 while (true)
    //                 {
    //                     string input = Console.ReadLine();
    //                     if (input == " exit ")
    //                     {
    //                         _IsExit = true;
    //                         Thread.CurrentThread.Abort();
    //                     }
    //                 }
    //             }
    //         }

    class ConsoleWin32Helper
    {
        static ConsoleWin32Helper()
        {
            //                 _NotifyIcon.Icon = new Icon(@" G:\BruceLi Test\ConsoleAppTest\ConsoleApps\Tray\small.ico ");
            //                 _NotifyIcon.Visible = false;
            //                 _NotifyIcon.Text = " tray ";
            // 
            //                 ContextMenu menu = new ContextMenu();
            //                 MenuItem item = new MenuItem();
            //                 item.Text = " 右键菜单，还没有添加事件 ";
            //                 item.Index = 0;
            // 
            //                 menu.MenuItems.Add(item);
            //                 _NotifyIcon.ContextMenu = menu;
            // 
            //                 _NotifyIcon.MouseDoubleClick += new MouseEventHandler(_NotifyIcon_MouseDoubleClick);

        }

        //             static void _NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        //             {
        //                 Console.WriteLine(" 托盘被双击. ");
        //             }

        #region 禁用关闭按钮

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        /// <summary>
        /// 禁用关闭按钮
        /// </summary>
        /// <param name="consoleName"> 控制台名字 </param>
        public static void DisableCloseButton(string title)
        {
            try
            {
                // 线程睡眠，确保closebtn中能够正常FindWindow，否则有时会Find失败。。
                Thread.Sleep(100);

                IntPtr windowHandle = FindWindow(null, title);
                IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
                uint SC_CLOSE = 0xF060;
                RemoveMenu(closeMenu, SC_CLOSE, 0x0);
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
        }
        public static void DisableCloseButton(IntPtr windowHandle)
        {
            try
            {
                // 线程睡眠，确保closebtn中能够正常FindWindow，否则有时会Find失败。。
                Thread.Sleep(100);

                //IntPtr windowHandle = FindWindow(null, title);
                IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
                uint SC_CLOSE = 0xF060;
                RemoveMenu(closeMenu, SC_CLOSE, 0x0);
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
        }
        public static bool IsExistsConsole(string title)
        {
            try
            {
                IntPtr windowHandle = FindWindow(null, title);
                if (windowHandle.Equals(IntPtr.Zero)) return false;
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
            return true;
        }
        #endregion

        #region 托盘图标
        //             static NotifyIcon _NotifyIcon = new NotifyIcon();
        //             public static void ShowNotifyIcon()
        //             {
        //                 _NotifyIcon.Visible = true;
        //                 _NotifyIcon.ShowBalloonTip(3000, "", " 我是托盘图标，用右键点击我试试，还可以双击看看。 ", ToolTipIcon.None);
        //             }
        //             public static void HideNotifyIcon()
        //             {
        //                 _NotifyIcon.Visible = false;
        //             }
        #endregion
    }


#else
    ///<summary>
    /// Provides all c# console application shutdown scenarios in a single handler
    ///</summary>
    public static class ShutdownEventCatcher
    {
        public static event Action<ShutdownEventArgs> Shutdown;
        static void RaiseShutdownEvent(ShutdownEventArgs args)
        {
            if (null != Shutdown)
                Shutdown(args);
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(Kernel32ShutdownHandler handler, bool add);

        private delegate bool Kernel32ShutdownHandler(ShutdownReason reason);

        /// <summary>
        /// Constructor attaches the shutdown event handlers immediately
        /// </summary>
        static ShutdownEventCatcher()
        {
            SetConsoleCtrlHandler(new Kernel32ShutdownHandler(Kernel32_ProcessShuttingDown), true);
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            var args = new ShutdownEventArgs(ShutdownReason.ReachEndOfMain);
            RaiseShutdownEvent(args);
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var args = new ShutdownEventArgs(e.ExceptionObject as Exception);
            RaiseShutdownEvent(args);
        }
        static bool Kernel32_ProcessShuttingDown(ShutdownReason sig)
        {
            ShutdownEventArgs args = new ShutdownEventArgs(sig);
            RaiseShutdownEvent(args);
            return false;
        }
    }

    public enum ShutdownReason
    {
        /// <summary>
        /// Source is Kernel 32
        /// User has pressed ^C
        /// </summary>
        PressCtrlC = 0,

        /// <summary>
        /// Source is Kernel 32
        /// User has pressed ^Break
        /// </summary>
        PressCtrlBreak = 1,

        /// <summary>
        /// Source is Kernel 32
        /// User has clicked the big "X" to close the console window or a windows message has been sent to the console
        /// </summary>
        ConsoleClosing = 2,

        /// <summary>
        /// Source is Kernel 32
        /// Windows is logging off
        /// </summary>
        WindowsLogOff = 5,

        /// <summary>
        /// Source is Kernel 32
        /// Windows is shutting down
        /// </summary>
        WindowsShutdown = 6,

        /// <summary>
        /// Source is Kernel 32
        /// Program has finished executing
        /// </summary>
        ReachEndOfMain = 1000,

        /// <summary>
        /// Source is AppDomain
        /// Unhandled exception in the program
        /// </summary>
        Exception = 1001
    }

    public class ShutdownEventArgs
    {
        public readonly Exception Exception;
        public readonly ShutdownReason Reason;

        public ShutdownEventArgs(ShutdownReason reason)
        {
            Reason = reason;
            Exception = null;
        }

        public ShutdownEventArgs(Exception exception)
        {
            Reason = ShutdownReason.Exception;
            Exception = exception;
        }
    }

#endif


}
