using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeepEditor.Common.WinConsole
{
    /// <summary>
    /// 防抖调度器 确保函数在一定时间间隔内以固定的频率执行
    /// </summary>
    public class DebounceDispatcher
    {
        private Timer timer;

        /// <summary>
        /// 函数防抖
        /// </summary>
        /// <param name="action">需要执行的<see cref="Action"/></param>
        /// <param name="delay">时间间隔/毫秒</param>
        public void Debounce(Action action, int delay)
        {
            timer?.Dispose();
            timer = new Timer((_) =>
            {
                action?.Invoke();
            }, null, delay, Timeout.Infinite);
        }
    }


    public static class ShareDllImport
    {
        /// <summary>
        /// 开启控制台
        /// </summary>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool AllocConsole();//开启

        /// <summary>
        /// 获取当前进程的控制台
        /// </summary>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public extern static IntPtr GetConsoleWindow();

        /// <summary>
        /// 释放控制台
        /// </summary>
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void FreeConsole();//关闭

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);//找出运行的窗口   

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        public extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert); //取出窗口运行的菜单   

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        public extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags); //灰掉按钮

        [System.Runtime.InteropServices.DllImport("User32.dll ", EntryPoint = "SetParent")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);//设置父窗体

        [System.Runtime.InteropServices.DllImport("user32.dll ", EntryPoint = "ShowWindow")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);//显示窗口

        [System.Runtime.InteropServices.DllImport("user32.dll ", EntryPoint = "SetWindowLong")]
        public static extern long SetWindowLongPtr(IntPtr hwnd, int index, long style);
        [System.Runtime.InteropServices.DllImport("user32.dll ", EntryPoint = "GetWindowLong")]
        public static extern long GetWindowLongPtr(IntPtr hwnd, int index);


        public const int GWL_EXSTYLE = -20;
        public const int GWLP_HINSTANCE = -6;
        public const int GWLP_HWNDPARENT = -8;
        public const int GWLP_ID = -12;
        public const int GWL_STYLE = -16;
        public const int GWLP_USERDATA = -21;
        public const int GWLP_WNDPROC = -4;

        /// <summary>
        /// WS_CAPTION<br/>
        /// https://learn.microsoft.com/zh-cn/windows/win32/winmsg/window-styles
        /// </summary>
        public const long WS_CAPTION = 0x00C00000L;
        public const long WS_SIZEBOX = 0x00040000L;
        public const long WS_MINIMIZEBOX = 0x00020000L;
        public const long WS_MAXIMIZEBOX = 0x00010000L;


        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);
    }

}
