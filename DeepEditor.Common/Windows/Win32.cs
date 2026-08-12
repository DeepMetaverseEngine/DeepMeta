using Microsoft.Win32.SafeHandles;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DeepEditor.Common.G2D.G2DDirectoryTreeView;

namespace DeepEditor.Common
{
    public static class Win32
    {
        public static Encoding Encoding { get; set; }

        static Win32()
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding = System.Text.Encoding.GetEncoding("GB2312");
            }
            catch (Exception err)
            {
                err.PrintStackTrace();
            }
            finally
            {
                if (Encoding == null)
                {
                    Encoding = Encoding.Default;
                }
            }
        }

        [DllImport("kernel32.dll",
                EntryPoint = "GetStdHandle",
                SetLastError = true,
                CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        //public static int STD_OUTPUT_HANDLE = -11;
        //         public static int MY_CODE_PAGE = 437;
        public static StreamWriter CreateSTDOutput(Encoding encoding = null, int STD_OUTPUT_HANDLE = -11)
        {
            encoding = encoding ?? Encoding;
            var stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            var safeFileHandle = new SafeFileHandle(stdHandle, true);
            var fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            var standardOutput = new StreamWriter(fileStream, encoding);
            standardOutput.AutoFlush = true;
            return standardOutput;
        }
        public static StreamWriter CreateConsole(Encoding encoding = null, int STD_OUTPUT_HANDLE = -11)
        {
            var ptr = AllocConsole();
            var standardOutput = CreateSTDOutput(encoding, STD_OUTPUT_HANDLE);
            Console.SetOut(standardOutput);
            Console.SetError(standardOutput);
            return standardOutput;
        }
        //         public static void CreateConsole()
        //         {
        //             AllocConsole();
        //             //encoding = encoding ?? Encoding;
        //             //             var stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
        //             //             var safeFileHandle = new SafeFileHandle(stdHandle, true);
        //             //             var fileStream = new FileStream(safeFileHandle, FileAccess.Write);
        //             //var standardOutput = new StreamWriter(fileStream, encoding);
        //             //standardOutput.AutoFlush = true;
        //             //             Console.SetOut(standardOutput);
        //             //             Console.SetError(standardOutput);
        //         }

        public static void SetClipboard(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch { }
        }


        public static FileSystemInfo GetFileSystemInfo(object data)
        {
            if (data is IFileTreeNode ftv)
            {
                if (ftv.FileTag is FileInfo file)
                {
                    return file;
                }
                else if (ftv.FileTag is DirectoryInfo dir)
                {
                    return dir;
                }
            }
            else if (data is TreeNode tn)
            {
                if (tn.Tag is FileInfo file)
                {
                    return file;
                }
                else if (tn.Tag is DirectoryInfo dir)
                {
                    return dir;
                }
            }
            else if (data is FileInfo file)
            {
                return file;
            }
            else if (data is DirectoryInfo dir)
            {
                return dir;
            }
            else if (data is string path)
            {
                if (File.Exists(path))
                {
                    return new FileInfo(path);
                }
                else if (Directory.Exists(path))
                {
                    return new DirectoryInfo(path);
                }
            }
            return null;
        }

        public static Process ShowInFolder(object data)
        {
            var fs = GetFileSystemInfo(data);

            if (fs is FileInfo file)
            {
                return System.Diagnostics.Process.Start("explorer.exe", $"/select, {file.FullName}");
            }
            else if (fs is DirectoryInfo dir)
            {
                return System.Diagnostics.Process.Start("explorer.exe", $"{dir.FullName}");
            }

            return null;
        }

        public static Process OpenFile(object data)
        {
            var fs = GetFileSystemInfo(data);
            if (fs is FileInfo file)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = $"\"{file.FullName}\"",
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }
            else if (fs is DirectoryInfo dir)
            {
                return System.Diagnostics.Process.Start("explorer.exe", $"{dir.FullName}");
            }
            return null;
        }





        //POST方法
        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            Encoding encoding = Encoding.UTF8;
            byte[] postData = encoding.GetBytes(postDataStr);
            request.ContentLength = postData.Length;
            Stream myRequestStream = request.GetRequestStream();
            myRequestStream.Write(postData, 0, postData.Length);
            myRequestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        //GET方法
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public static bool DetermineIfThisIsInDesignMode(this Control ctx)
        {
            // This works on .NET Framework but no longer seems to work reliably on .NET Core.
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return true;

            // Try walking the control tree to see if any ancestors are in DesignMode.
            for (Control control = ctx; control != null; control = control.Parent)
            {
                if (control.Site != null && control.Site.DesignMode)
                    return true;
            }

            // Last-ditch attempt:  Is the process named `devenv` or `VisualStudio`?
            // These are bad, hacky tests, but they *can* work sometimes.
            if (System.Reflection.Assembly.GetExecutingAssembly().Location.Contains("VisualStudio", StringComparison.OrdinalIgnoreCase))
                return true;
            if (string.Equals(System.Diagnostics.Process.GetCurrentProcess().ProcessName, "devenv", StringComparison.OrdinalIgnoreCase))
                return true;

            // Nope.  Not design mode.  Probably.  Maybe.
            return false;
        }
        // 
        //         public static void SendMailLocalhost()
        //         {
        //             System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
        //             msg.To.Add("a@a.com");
        //             msg.To.Add("b@b.com");
        //             /* msg.To.Add("b@b.com");  
        //             * msg.To.Add("b@b.com");  
        //             * msg.To.Add("b@b.com");可以发送给多人  
        //             */
        //             msg.CC.Add(c@c.com);
        //             /*  
        //             * msg.CC.Add("c@c.com");  
        //             * msg.CC.Add("c@c.com");可以抄送给多人  
        //             */
        //             msg.From = new MailAddress("a@a.com", "AlphaWu", System.Text.Encoding.UTF8);
        //             /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
        //             msg.Subject = "这是测试邮件";//邮件标题  
        //             msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码  
        //             msg.Body = "邮件内容";//邮件内容  
        //             msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码  
        //             msg.IsBodyHtml = false;//是否是HTML邮件  
        //             msg.Priority = MailPriority.High;//邮件优先级 
        // 
        //             SmtpClient client = new SmtpClient();
        //             client.Host = "localhost";
        //             object userState = msg;
        //             try
        //             {
        //                 client.SendAsync(msg, userState);
        //                 //简单一点儿可以client.Send(msg);  
        //                 MessageBox.Show("发送成功");
        //             }
        //             catch (System.Net.Mail.SmtpException ex)
        //             {
        //                 MessageBox.Show(ex.Message, "发送邮件出错");
        //             }
        //         }
        //         public static void SendMailLocalhost()
        //         {
        //             System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
        //             msg.To.Add("a@a.com");
        //             msg.To.Add("b@b.com");
        //             /* msg.To.Add("b@b.com");  
        //             * msg.To.Add("b@b.com");  
        //             * msg.To.Add("b@b.com");可以发送给多人  
        //             */
        //             msg.CC.Add(c@c.com);
        //             /*  
        //             * msg.CC.Add("c@c.com");  
        //             * msg.CC.Add("c@c.com");可以抄送给多人  
        //             */
        //             msg.From = new MailAddress(master@boys90.com, "dulei", System.Text.Encoding.UTF8);
        //             /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
        //             msg.Subject = "这是测试邮件";//邮件标题  
        //             msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码  
        //             msg.Body = "邮件内容";//邮件内容  
        //             msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码  
        //             msg.IsBodyHtml = false;//是否是HTML邮件  
        //             msg.Priority = MailPriority.High;//邮件优先级 
        //             SmtpClient client = new SmtpClient();
        //             client.Host = "localhost";
        //             object userState = msg;
        //             try
        //             {
        //                 client.SendAsync(msg, userState);
        //                 //简单一点儿可以client.Send(msg);  
        //                 MessageBox.Show("发送成功");
        //             }
        //             catch (System.Net.Mail.SmtpException ex)
        //             {
        //                 MessageBox.Show(ex.Message, "发送邮件出错");
        //             }
        //         }
        // 在程序初始化（创建窗口前）调用这个方法
        public static void SetupCustomGlfwErrorCallback()
        {
            // 设置自定义错误回调
            GLFWProvider.SetErrorCallback((errorCode, description) =>
            {
                // 过滤掉 "Failed to clear current context" 这个特定错误
                string desc = description;
                if (!desc.Contains("Failed to clear current context"))
                {
                    // 其他错误正常打印（方便排查真问题）
                    Console.WriteLine($"GLFW Error [{errorCode}]: {desc}");
                }
                // 对于目标错误，不做任何处理，避免抛出异常
            });
        }
    }
    /// <summary>
    /// Wraps necessary Shell32.dll structures and functions required to retrieve Icon Handles using SHGetFileInfo. Code
    /// courtesy of MSDN Cold Rooster Consulting case study.
    /// </summary>
    /// 

    // This code has been left largely untouched from that in the CRC example. The main changes have been moving
    // the icon reading code over to the IconReader type.
    public class Shell32
    {

        public const int MAX_PATH = 256;
        [StructLayout(LayoutKind.Sequential)]
        public struct SHITEMID
        {
            public ushort cb;
            [MarshalAs(UnmanagedType.LPArray)]
            public byte[] abID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ITEMIDLIST
        {
            public SHITEMID mkid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BROWSEINFO
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            public IntPtr pszDisplayName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszTitle;
            public uint ulFlags;
            public IntPtr lpfn;
            public int lParam;
            public IntPtr iImage;
        }

        // Browsing for directory.
        public const uint BIF_RETURNONLYFSDIRS = 0x0001;
        public const uint BIF_DONTGOBELOWDOMAIN = 0x0002;
        public const uint BIF_STATUSTEXT = 0x0004;
        public const uint BIF_RETURNFSANCESTORS = 0x0008;
        public const uint BIF_EDITBOX = 0x0010;
        public const uint BIF_VALIDATE = 0x0020;
        public const uint BIF_NEWDIALOGSTYLE = 0x0040;
        public const uint BIF_USENEWUI = (BIF_NEWDIALOGSTYLE | BIF_EDITBOX);
        public const uint BIF_BROWSEINCLUDEURLS = 0x0080;
        public const uint BIF_BROWSEFORCOMPUTER = 0x1000;
        public const uint BIF_BROWSEFORPRINTER = 0x2000;
        public const uint BIF_BROWSEINCLUDEFILES = 0x4000;
        public const uint BIF_SHAREABLE = 0x8000;

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public const int NAMESIZE = 80;
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAMESIZE)]
            public string szTypeName;
        };

        public const uint SHGFI_ICON = 0x000000100;     // get icon
        public const uint SHGFI_DISPLAYNAME = 0x000000200;     // get display name
        public const uint SHGFI_TYPENAME = 0x000000400;     // get type name
        public const uint SHGFI_ATTRIBUTES = 0x000000800;     // get attributes
        public const uint SHGFI_ICONLOCATION = 0x000001000;     // get icon location
        public const uint SHGFI_EXETYPE = 0x000002000;     // return exe type
        public const uint SHGFI_SYSICONINDEX = 0x000004000;     // get system icon index
        public const uint SHGFI_LINKOVERLAY = 0x000008000;     // put a link overlay on icon
        public const uint SHGFI_SELECTED = 0x000010000;     // show icon in selected state
        public const uint SHGFI_ATTR_SPECIFIED = 0x000020000;     // get only specified attributes
        public const uint SHGFI_LARGEICON = 0x000000000;     // get large icon
        public const uint SHGFI_SMALLICON = 0x000000001;     // get small icon
        public const uint SHGFI_OPENICON = 0x000000002;     // get open icon
        public const uint SHGFI_SHELLICONSIZE = 0x000000004;     // get shell size icon
        public const uint SHGFI_PIDL = 0x000000008;     // pszPath is a pidl
        public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;     // use passed dwFileAttribute
        public const uint SHGFI_ADDOVERLAYS = 0x000000020;     // apply the appropriate overlays
        public const uint SHGFI_OVERLAYINDEX = 0x000000040;     // Get the index of the overlay

        public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags
            );




    }

    /// <summary>
    /// Wraps necessary functions imported from User32.dll. Code courtesy of MSDN Cold Rooster Consulting example.
    /// </summary>
    public class User32
    {
        /// <summary>
        /// Provides access to function required to delete handle. This method is used internally
        /// and is not required to be called separately.
        /// </summary>
        /// <param name="hIcon">Pointer to icon handle.</param>
        /// <returns>N/A</returns>
        [DllImport("User32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);
    }


    public interface IFileTreeNode
    {
        public FileSystemInfo FileTag { get; }
    }

}
