using DeepCore;
using DeepEditor.Common.G2D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeepEditor.Common.Windows
{
    public static class IconReader
    {
        /// <summary>
        /// Options to specify the size of icons to return.
        /// </summary>
        public enum IconSize
        {
            /// <summary>
            /// Specify large icon - 32 pixels by 32 pixels.
            /// </summary>
            Large = 0,
            /// <summary>
            /// Specify small icon - 16 pixels by 16 pixels.
            /// </summary>
            Small = 1
        }

        /// <summary>
        /// Options to specify whether folders should be in the open or closed state.
        /// </summary>
        public enum FolderType
        {
            /// <summary>
            /// Specify open folder.
            /// </summary>
            Open = 0,
            /// <summary>
            /// Specify closed folder.
            /// </summary>
            Closed = 1
        }

        /// <summary>
        /// Returns an icon for a given file - indicated by the name parameter.
        /// </summary>
        /// <param name="name">Pathname for file.</param>
        /// <param name="size">Large or small</param>
        /// <param name="linkOverlay">Whether to include the link icon</param>
        /// <returns>System.Drawing.Icon</returns>
        public static System.Drawing.Icon GetFileIcon(string name, IconSize size, bool linkOverlay)
        {
            Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
            uint flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

            if (true == linkOverlay) flags += Shell32.SHGFI_LINKOVERLAY;

            /* Check the size specified for return. */
            if (IconSize.Small == size)
            {
                flags += Shell32.SHGFI_SMALLICON;
            }
            else
            {
                flags += Shell32.SHGFI_LARGEICON;
            }

            Shell32.SHGetFileInfo(name,
                Shell32.FILE_ATTRIBUTE_NORMAL,
                ref shfi,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                flags);

            // Copy (clone) the returned icon to a new object, thus allowing us to clean-up properly
            System.Drawing.Icon icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
            User32.DestroyIcon(shfi.hIcon);     // Cleanup
            return icon;
        }

        /// <summary>
        /// Used to access system folder icons.
        /// </summary>
        /// <param name="size">Specify large or small icons.</param>
        /// <param name="folderType">Specify open or closed FolderType.</param>
        /// <returns>System.Drawing.Icon</returns>
        public static System.Drawing.Icon GetFolderIcon(IconSize size, FolderType folderType)
        {
            // Need to add size check, although errors generated at present!
            uint flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

            if (FolderType.Open == folderType)
            {
                flags += Shell32.SHGFI_OPENICON;
            }

            if (IconSize.Small == size)
            {
                flags += Shell32.SHGFI_SMALLICON;
            }
            else
            {
                flags += Shell32.SHGFI_LARGEICON;
            }

            // Get the folder icon
            Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
            Shell32.SHGetFileInfo(@"Folder",
                Shell32.FILE_ATTRIBUTE_DIRECTORY,
                ref shfi,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                flags);

            System.Drawing.Icon.FromHandle(shfi.hIcon); // Load the icon from an HICON handle

            // Now clone the icon, so that it can be successfully stored in an ImageList
            System.Drawing.Icon icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();

            User32.DestroyIcon(shfi.hIcon);     // Cleanup
            return icon;
        }

        public static System.Drawing.Icon GetDriveIcon(IconSize size, FolderType folderType)
        {
            // Need to add size check, although errors generated at present!
            uint flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

            if (FolderType.Open == folderType)
            {
                flags += Shell32.SHGFI_OPENICON;
            }

            if (IconSize.Small == size)
            {
                flags += Shell32.SHGFI_SMALLICON;
            }
            else
            {
                flags += Shell32.SHGFI_LARGEICON;
            }

            // Get the folder icon
            Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
            Shell32.SHGetFileInfo(null,
                Shell32.FILE_ATTRIBUTE_DIRECTORY,
                ref shfi,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                flags);

            System.Drawing.Icon.FromHandle(shfi.hIcon); // Load the icon from an HICON handle

            // Now clone the icon, so that it can be successfully stored in an ImageList
            System.Drawing.Icon icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();

            User32.DestroyIcon(shfi.hIcon);     // Cleanup
            return icon;
        }


        private static HashMap<string, Image> icons = new HashMap<string, Image>();
        public static bool TryFeatchSystemIcon(string file, out Image icon, out string iconKey)
        {
            iconKey = Path.GetExtension(file);
            {
                if (!string.IsNullOrEmpty(iconKey))
                {
                    if (icons.TryGetValue(iconKey, out icon))
                    {
                        return icon != null;
                    }
                    else
                    {
                        try
                        {
                            icon = IconReader.GetFileIcon(file, IconReader.IconSize.Large, false).ToBitmap();
                            icons.Add(iconKey, icon);
                            return icon != null;
                        }
                        catch
                        {
                            icons.Add(iconKey, null);
                        }
                    }
                }
            }
            icon = null;
            return false;
        }

        public static bool TryFeatchSystemIcon(this TreeView tree, string file, out Image icon, out string iconKey)
        {
            iconKey = Path.GetExtension(file);
            var imageList = tree.ImageList;
            if (imageList != null)
            {
                if (!string.IsNullOrEmpty(iconKey))
                {
                    if (!already.ContainsKey(iconKey))
                    {
                        already.Add(iconKey, null);
                        Console.WriteLine("TryFeatchSystemIcon : " + file);
                        icon = imageList.Images[iconKey];
                        if (icon == null)
                        {
                            try
                            {
                                icon = IconReader.GetFileIcon(file, IconReader.IconSize.Large, false).ToBitmap();
                                already.Put(iconKey, icon);
                                if (!tree.IsHandleCreated)
                                {
                                    imageList.Images.Add(iconKey, icon);
                                }
                                else
                                {
                                    imageList.Images.Add(iconKey, icon);
                                }
                            }
                            catch
                            {
                            }
                        }
                        return icon != null;
                    }
                }
            }
            icon = null;
            return false;
        }

        public static bool TryFeatchSystemIcon(this TreeView tree, TreeNode node, string file, out string iconName)
        {
            iconName = Path.GetExtension(file);
            var imageList = tree.ImageList;
            if (imageList != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(iconName))
                    {
                        if (node.ImageKey != iconName)
                        {
                            if (!already.ContainsKey(iconName))
                            {
                                already.Add(iconName, null);
                                Console.WriteLine("TryFeatchSystemIcon : " + file);
                                if (!imageList.Images.ContainsKey(iconName))
                                {
                                    try
                                    {
                                        var icon = IconReader.GetFileIcon(file, IconReader.IconSize.Large, false).ToBitmap();
                                        already.Put(iconName, icon);
                                        if (!tree.IsHandleCreated)
                                        {
                                            imageList.Images.Add(iconName, icon);
                                        }
                                        else
                                        {
                                            imageList.Images.Add(iconName, icon);
                                        }
                                        node.ImageKey = node.SelectedImageKey = iconName;
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            //                         else
                            //                         {
                            //                             node.ImageKey = node.SelectedImageKey = iconName;
                            //                         }
                            return true;
                        }
                    }
                }
                finally
                {
                    node.ImageKey = node.SelectedImageKey = iconName;
                }
            }
            return false;
        }

        private static HashMap<string, Image> already = new HashMap<string, Image>();
    }



}
