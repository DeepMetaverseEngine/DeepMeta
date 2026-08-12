using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using DeepCore;

namespace System.Windows.Forms
{
    public static class Mouse
    {
        public static bool IsMouseDown(MouseButtons buttons)
        {
            return (Control.MouseButtons & buttons) != 0;
        }
        public static System.Drawing.Point MousePoint
        {
            get => Control.MousePosition;
        }
        public static System.Drawing.Point GetMousePoint(this Control control)
        {
            return control.PointToClient(Control.MousePosition);
        }
        public static bool ContainsMousePoint(this Control control)
        {
            return control.RectangleToScreen(new Rectangle(0, 0, control.Width, control.Height)).Contains(Control.MousePosition);
        }
    }

}
namespace DeepEditor.Common
{


    public static class Keyboard
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern short GetKeyState(int virtualKeyCode);

        public const int VK_SHIFT = 0x10;
        public const int VK_CTRL = 0x11;
        public const int VK_ALT = 0x12;

        public static bool IsShiftDown
        {
            get
            {
                return (GetKeyState(VK_SHIFT) & 0x80) != 0;
            }
        }
        public static bool IsCtrlDown
        {
            get
            {
                return (GetKeyState(VK_CTRL) & 0x80) != 0;
            }
        }
        public static bool IsAltDown
        {
            get
            {
                return (GetKeyState(VK_ALT) & 0x80) != 0;
            }
        }
        public static bool GetKeyState(System.Windows.Forms.Keys keys)
        {
            return ((GetKeyState((int)keys) & 0x8000) != 0) ? true : false;
        }
        public static bool IsKeyDown(System.Windows.Forms.Keys keys)
        {
            return ((GetKeyState((int)keys) & 0x8000) != 0) ? true : false;
        }
    }

    public static class WindowsMessage
    {
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;


    }

    public class QueryKey : IDisposable, IMessageFilter
    {
        private Control component;

        private HashMap<MouseButtons, System.Windows.Forms.Message> mousestate = new HashMap<MouseButtons, System.Windows.Forms.Message>();
        private HashMap<MouseButtons, System.Windows.Forms.Message> mousestate_down = new HashMap<MouseButtons, System.Windows.Forms.Message>();
        private HashMap<MouseButtons, System.Windows.Forms.Message> mousestate_up = new HashMap<MouseButtons, System.Windows.Forms.Message>();
        private HashMap<MouseButtons, System.Windows.Forms.Message> mousestate_query_down = new HashMap<MouseButtons, System.Windows.Forms.Message>();
        private HashMap<MouseButtons, System.Windows.Forms.Message> mousestate_query_up = new HashMap<MouseButtons, System.Windows.Forms.Message>();


        private Point mouse_pos;

        public QueryKey(Control component)
        {
            this.component = component;
            Application.AddMessageFilter(this);
        }
        public void Dispose()
        {
            Application.RemoveMessageFilter(this);
        }

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            try
            {
                switch (m.Msg)
                {
                    case WindowsMessage.WM_MOUSEMOVE:
                        mouse_pos = component.PointToClient(Control.MousePosition);
                        break;

                    case WindowsMessage.WM_LBUTTONDOWN:
                        mousestate_down.Put(MouseButtons.Left, m);
                        mousestate.Put(MouseButtons.Left, m);
                        break;
                    case WindowsMessage.WM_RBUTTONDOWN:
                        mousestate_down.Put(MouseButtons.Right, m);
                        mousestate.Put(MouseButtons.Right, m);
                        break;

                    case WindowsMessage.WM_LBUTTONUP:
                        mousestate_up.Put(MouseButtons.Left, m);
                        mousestate.Remove(MouseButtons.Left);
                        break;
                    case WindowsMessage.WM_RBUTTONUP:
                        mousestate_up.Put(MouseButtons.Right, m);
                        mousestate.Remove(MouseButtons.Right);
                        break;

                    case WindowsMessage.WM_LBUTTONDBLCLK:
                    case WindowsMessage.WM_RBUTTONDBLCLK:
                        mouse_pos = component.PointToClient(Control.MousePosition);
                        break;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message + "\n" + err.StackTrace);
            }

            return false;
        }

        /// <summary>
        /// 用收集到的按键替换查询的按键,并清理收集到的按键,重新监测下一帧的情况
        /// </summary>
        public void Update()
        {
            mousestate_query_down.Clear();
            mousestate_query_down.PutAll(mousestate_down);
            mousestate_query_up.Clear();
            mousestate_query_up.PutAll(mousestate_up);

            mousestate_down.Clear();
            mousestate_up.Clear();
        }

        public bool IsMouseHold(params MouseButtons[] button)
        {
            foreach (MouseButtons b in button)
            {
                if (mousestate.ContainsKey(b))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsMouseDown(params MouseButtons[] button)
        {
            foreach (MouseButtons b in button)
            {
                if (mousestate_query_down.ContainsKey(b))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsMouseUp(params MouseButtons[] button)
        {
            foreach (MouseButtons b in button)
            {
                if (mousestate_query_up.ContainsKey(b))
                {
                    return true;
                }
            }
            return false;
        }

        public int MouseX
        {
            get
            {
                return mouse_pos.X;
            }
        }

        public int MouseY
        {
            get
            {
                return mouse_pos.Y;
            }
        }


    }
}
