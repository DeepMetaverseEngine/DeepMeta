using DeepCore.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DeepEditor.Common.WinConsole.ShareDllImport;


namespace DeepEditor.Common.WinConsole
{
    public partial class WinConsole : UserControl
    {
        public WinConsole()
        {
            InitializeComponent();
            this.Load += WinConsole_Load;
            this.Resize += WinConsole_Resize;
            this.VisibleChanged += WinConsole_VisibleChanged;
            this.Enter += WinConsole_Enter;
            this.MouseDown += WinConsole_MouseDown;
            this.MouseEnter += WinConsole_MouseEnter;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
//             if (console_handle != IntPtr.Zero)
//             {
//                 if (CanFocus)
//                 {
//                     SetFocus(console_handle);
//                 }
//             }
        }
        private void WinConsole_MouseEnter(object sender, EventArgs e)
        {
//             if (console_handle != IntPtr.Zero)
//             {
//                 SetFocus(console_handle);
//             }
        }

        private void WinConsole_MouseDown(object sender, MouseEventArgs e)
        {
//             if (console_handle != IntPtr.Zero)
//             {
//                 SetFocus(console_handle);
//             }
        }

        private void WinConsole_Enter(object sender, EventArgs e)
        {
//             if (console_handle != IntPtr.Zero)
//             {
//                 SetFocus(console_handle);
//             }
        }

        private void WinConsole_VisibleChanged(object sender, EventArgs e)
        {
            if (console_handle != IntPtr.Zero)
            {
                if (!this.Visible)
                {
                    ShowWindow(console_handle, 0); // 隐藏
                }
                else
                {
                    ShowWindow(console_handle, 3); // 显示
                    MoveWindow(console_handle, 0, 0, this.Width, this.Height, true);
                }
            }
        }

        private void WinConsole_Resize(object sender, EventArgs e)
        {
            if (console_handle != IntPtr.Zero)
            {
                MoveWindow(console_handle, 0, 0, this.Width, this.Height, true);
            }
        }

        private void WinConsole_Load(object sender, EventArgs e)
        {
            //this.resizeDebounce = new DebounceDispatcher();
            //this.AddConsole();
        }
        //private DebounceDispatcher resizeDebounce;
        private IntPtr console_handle;
        public void BindConsole()
        {
            try
            {
                console_handle = GetConsoleWindow();
                if (console_handle == IntPtr.Zero)
                {
                    Win32.CreateConsole();
                }

                console_handle = GetConsoleWindow();
                if (console_handle == IntPtr.Zero)
                {
                    return;
                }

                // 设置容器
                SetParent(console_handle, this.Handle);

                // 去掉边框, 最大化, 最小化, 大小调节
                SetWindowLongPtr(console_handle, GWL_STYLE,
                    GetWindowLongPtr(console_handle, GWL_STYLE) - WS_CAPTION - WS_SIZEBOX - WS_MAXIMIZEBOX - WS_MINIMIZEBOX);

                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        await Task.Delay(1000);
                        ShowWindow(console_handle, 3);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }).ConfigureAwait(false);
            }
            catch (System.Exception err)
            {

            }
        }

        //         private void mainboardTabControl_SizeChanged(object sender, EventArgs e)
        //         {
        //             resizeDebounce.Debounce(() =>
        //             {
        //                 //                 ShowWindow(console_handle, 0); // 隐藏
        //                 //                 ShowWindow(console_handle, 3); // 最大化显示
        //                 //                 Logger.Log4Net.AppLog.Info("Info");
        //                 //                 Logger.Log4Net.AppLog.Warn("Warn");
        //                 //                 Logger.Log4Net.AppLog.Error("Error");
        //             }, 200);
        //         }

    }
}
