using DeepCore.Log;
using DeepCore.MPQ;
using DeepCore.MPQ.Updater;
using DeepCore.SharpZipLib;
using DeepCore.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace DeepTools.MPQUpdate
{
    public partial class FormUpdater : Form
    {
        private static Logger log = new LazyLogger(nameof(FormUpdater));
        private SaveData save = new SaveData();
        private MPQUpdater updater;
        private MPQFileSystem filesystem;

        public string DefaultBaseURL
        {
            get { return comboBox_RemoteDir.Text; }
            set { comboBox_RemoteDir.Text = value; }
        }
        public string DefaultRemoteURL
        {
            get { return comboBox_RemoteUrl.Text; }
            set { comboBox_RemoteUrl.Text = value; }
        }
        public string DefaultSavePath
        {
            get { return textBox_SaveRoot.Text; }
            set { textBox_SaveRoot.Text = value; }
        }
        public string DefaultZIPType
        {
            get { return comboBox_zipType.Text; }
            set { comboBox_zipType.Text = value; }
        }
        public string DefaultMPQType
        {
            get { return comboBox_mpqType.Text; }
            set { comboBox_mpqType.Text = value; }
        }
        public FormUpdater()
        {
            InitializeComponent();
            try
            {
                if (File.Exists(Application.StartupPath + "/save.xml"))
                {
                    XmlDocument xml = XmlUtil.LoadXML(Application.StartupPath + "/save.xml");
                    save = (SaveData)XmlUtil.XmlToObject(xml);
                    if (!string.IsNullOrEmpty(save.DEFAULT_URL))
                    {
                        comboBox_RemoteDir.Items.Add(save.DEFAULT_URL);
                        comboBox_RemoteDir.Text = save.DEFAULT_URL;
                    }
                    if (!string.IsNullOrEmpty(save.DEFAULT_SUFFIX))
                    {
                        comboBox_RemoteUrl.Items.Add(save.DEFAULT_SUFFIX);
                        comboBox_RemoteUrl.Text = save.DEFAULT_SUFFIX;
                    }
                    if (!string.IsNullOrEmpty(save.SAVE_PATH))
                    {
                        textBox_SaveRoot.Text = save.SAVE_PATH;
                    }
                    if (!string.IsNullOrEmpty(save.ZIP_TYPE))
                    {
                        comboBox_zipType.Items.Add(save.ZIP_TYPE);
                        comboBox_zipType.Text = save.ZIP_TYPE;
                    }
                    if (!string.IsNullOrEmpty(save.MPQ_TYPE))
                    {
                        comboBox_mpqType.Items.Add(save.MPQ_TYPE);
                        comboBox_mpqType.Text = save.MPQ_TYPE;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            this.Refresh();
        }

        private void FormUpdater_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (filesystem != null)
            {
                filesystem.Dispose();
            }
            if (updater != null)
            {
                updater.Dispose();
            }
        }
        public class SaveData
        {
            public string DEFAULT_URL;
            public string DEFAULT_SUFFIX;
            public string SAVE_PATH;
            public string ZIP_TYPE;
            public string MPQ_TYPE;
        }
        private void SaveHistory()
        {
            try
            {
                save.DEFAULT_URL = comboBox_RemoteDir.Text;
                save.DEFAULT_SUFFIX = comboBox_RemoteUrl.Text;
                save.SAVE_PATH = textBox_SaveRoot.Text;
                save.ZIP_TYPE = comboBox_zipType.Text;
                save.MPQ_TYPE = comboBox_mpqType.Text;
                XmlDocument xml = XmlUtil.ObjectToXml(save);
                XmlUtil.SaveXML(Application.StartupPath + "/save.xml", xml);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        //--------------------------------------------------------------------------------------------
        #region AUTO_UPDATER

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (updater != null)
            {
                updater.Update();
                try
                {
                    progressBar_Download.Minimum = 0;
                    progressBar_Download.Maximum = (int)(updater.TotalDownloadBytes / 1024);
                    progressBar_Download.Value = (int)(updater.CurrentDownloadBytes / 1024);
                    label_Download.Text = updater.CurrentDownloadFile +
                        " (" + updater.CurrentDownloadBytes + "/" + updater.TotalDownloadBytes + ") " +
                        " " + (updater.CurrentDownloadSpeed / 1024) + "KB/S";

                    progressBar_Decompress.Minimum = 0;
                    progressBar_Decompress.Maximum = (int)(updater.TotalUnzipBytes / 1024);
                    progressBar_Decompress.Value = (int)(updater.CurrentUnzipBytes / 1024);
                    label_Unzip.Text = updater.CurrentUnzipFile +
                        " (" + updater.CurrentUnzipBytes + "/" + updater.TotalUnzipBytes + ") " +
                        " " + (updater.CurrentUnzipSpeed / 1024) + "KB/S";
                }
                catch (Exception err)
                {
                    log.Error(err);
                }

                textBox_VersionText.Lines = updater.VersionText.Split(new char[] { '\n' });
                progressBar_Running.Visible = updater.IsRunning;
            }
            else
            {
                progressBar_Download.Minimum = 0;
                progressBar_Download.Maximum = 1;
                progressBar_Download.Value = 0;
                label_Download.Text = "";

                progressBar_Decompress.Minimum = 0;
                progressBar_Decompress.Maximum = 1;
                progressBar_Decompress.Value = 0;
                label_Unzip.Text = "";

                textBox_VersionText.Text = "";

                progressBar_Running.Visible = false;
            }
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            if (updater != null)
            {
                updater.Dispose();
            }

            try
            {
                progressBar_Download.Minimum = 0;
                progressBar_Download.Maximum = 1;
                progressBar_Download.Value = 0;
                label_Download.Text = "";

                MPQDriverFactory.CreateUnziper = (dir) => new SharpZipLibMPQDriver();
                Uri url = new Uri(comboBox_RemoteDir.Text);
                updater = new MPQUpdater(
                   new string[] { comboBox_RemoteDir.Text },
                   new Uri(comboBox_RemoteUrl.Text),
                   new DirectoryInfo(textBox_SaveRoot.Text),
                   new DirectoryInfo(textBox_BundleDir.Text),
                   true);
                updater.DoNotUnzip = chk_DoNotUnzip.Checked;
                updater.DoNotDownloadZip = chk_DoNotDownloadZip.Checked;
                updater.OnEvent += updater_OnEvent;
                MPQUpdater.MPQ_EXT = comboBox_mpqType.Text.Trim();
                MPQUpdater.ZIP_EXT = comboBox_zipType.Text.Trim();
                //updater.RedirectDownloadSingle = RunDownloadSingle;
                //updater.RedirectUnzipSingle = RunUnzipSingle;
                updater.Start();

                SaveHistory();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void updater_OnEvent(MPQUpdater sender, MPQUpdaterEvent e)
        {
            if (e.EventType == MPQUpdaterEventType.TYPE_COMPLETE)
            {
                RefreshMPQ();
            }
            else if (e.EventType == MPQUpdaterEventType.TYPE_ERROR || e.EventType == MPQUpdaterEventType.TYPE_NOT_ENOUGH_SPACE)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /*
private bool RunDownloadSingle(MPQUpdater updater, MPQUpdater.RemoteFileInfo inf, long exist_size, long need_bytes, AtomicLong process)
{
   Uri url = new Uri(updater.UrlRoots[0] + inf.key);
   var webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
   webRequest.AddRange((int)exist_size, (int)(exist_size + need_bytes));
   webRequest.Method = "GET";
   WebResponse webResponse = webRequest.GetResponse();
   try
   {
       if (webResponse.ContentLength == need_bytes)
       {
           byte[] io_buffer = new byte[1024 * 4];
           using (FileStream fos = new FileStream(inf.file.FullName, FileMode.Append, FileAccess.Write))
           {
               Stream input = webResponse.GetResponseStream();
               try
               {
                   long total_readed = 0;
                   while (total_readed < need_bytes)
                   {
                       if (updater.IsDisposing) return false;
                       int readed = input.Read(io_buffer, 0, (int)Math.Min(io_buffer.Length, need_bytes - total_readed));
                       total_readed += readed;
                       process += readed;
                       fos.Write(io_buffer, 0, readed);
                   }
                   fos.Flush();
               }
               finally
               {
                   fos.Close();
               }
           }
       }
       else
       {
           throw new Exception("Bad response with ContentLength=" + webResponse.ContentLength);
       }
       return true;
   }
   finally
   {
       webResponse.Close();
   }
}
private bool RunUnzipSingle(MPQUpdater updater, MPQUpdater.RemoteFileInfo zip, MPQUpdater.RemoteFileInfo mpq, AtomicLong process)
{
   byte[] io_buffer = new byte[1024 * 4];
   using (FileStream fis = new FileStream(zip.file.FullName, FileMode.Open, FileAccess.Read))
   {
       using (FileStream fos = new FileStream(mpq.file.FullName, FileMode.Create, FileAccess.Write))
       {
           GZipStream gstream = new GZipStream(fis, CompressionMode.Decompress);
           long total_readed = 0;
           long total_size = mpq.size;
           while (total_readed < total_size)
           {
               if (updater.IsDisposing) return false;
               int readed = gstream.Read(io_buffer, 0, (int)Math.Min(io_buffer.Length, total_size - total_readed));
               total_readed += readed;
               process += readed;
               fos.Write(io_buffer, 0, readed);
           }
           fos.Flush();
           gstream.Close();
           fos.Close();
           fis.Close();
       }
   }
   return true;
}
*/
        private void button_Stop_Click(object sender, EventArgs e)
        {
            if (updater != null)
            {
                updater.Dispose();
                updater = null;
            }
            UnloadMPQ();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            try
            {
                if (updater != null)
                {
                    updater.Dispose();
                    updater = null;
                }
                UnloadMPQ();
                DirectoryInfo local = new DirectoryInfo(textBox_SaveRoot.Text);
                if (local.Exists)
                {
                    local.Delete(true);
                    local.Create();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }


        #endregion

        //--------------------------------------------------------------------------------------------
        private void UnloadMPQ()
        {
            if (this.filesystem != null)
            {
                TreeNode root = treeView_MPQ.Nodes[0];
                root.Tag = null;
                root.Nodes.Clear();

                this.filesystem.Dispose();
                this.filesystem = null;
            }
        }

        private void RefreshMPQ()
        {
            this.Enabled = false;
            try
            {
                if (filesystem != null)
                {
                    filesystem.Dispose();
                }
                if (updater != null)
                {
                    this.filesystem = new MPQFileSystem();
                    this.filesystem.Init(updater);
                    //                     {
                    //                         var dir = this.filesystem.GetDirectory("/GameEditor/func/lua/npc/functions.xlsx");
                    //                         var files = dir.GetFiles();
                    //                         files.ToString();
                    //                     }
                    InitUpdateInfo(filesystem);
                }
                RefreshUpdateInfo();
                RefreshFileSystemView();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            finally
            {
                this.Enabled = true;
                treeView_MPQ.Invalidate(true);
            }
        }

        //--------------------------------------------------------------------------------------------
        #region TAB_UPDATE_INFO

        private void RefreshUpdateInfo()
        {
            TreeNode root = treeView_MPQ.Nodes[0];
            root.Nodes.Clear();

            if (filesystem != null && root.Tag is MPQFileSystemInfo)
            {
                MPQFileSystemInfo fsinfo = (MPQFileSystemInfo)root.Tag;
                foreach (var rmf in fsinfo.files.Keys)
                {
                    MPQFileInfo mpq_info = fsinfo.files[rmf];

                    TreeNode mpq_file_node = new TreeNode(rmf.key);
                    mpq_file_node.Tag = mpq_info;
                    foreach (var e in mpq_info.entries)
                    {
                        bool is_old = false;
                        var exist_fe = filesystem.FindEntry(e.Key);
                        if (exist_fe != null)
                        {
                            is_old = !e.Equals(exist_fe);
                        }
                        if (!toolStripButton_ViewReplaced.Checked || is_old)
                        {
                            TreeNode enode = new TreeNode(e.Key);
                            enode.Tag = e;
                            if (is_old)
                            {
                                enode.Text = "(old) " + e.Key;
                                enode.ForeColor = Color.Gray;
                            }
                            enode.ContextMenuStrip = this.menu_EntryNode;
                            mpq_file_node.Nodes.Add(enode);
                        }
                    }
                    mpq_file_node.Text += " (" + (mpq_info.EntryCount - mpq_info.ReplacedFileCount) + "/" + mpq_info.EntryCount + ")";
                    root.Nodes.Add(mpq_file_node);
                }
            }
        }

        private void InitUpdateInfo(MPQFileSystem fs)
        {
            TreeNode root = treeView_MPQ.Nodes[0];
            root.Tag = null;
            root.Nodes.Clear();

            MPQFileSystemInfo fsinfo = new MPQFileSystemInfo();
            root.Tag = fsinfo;
            foreach (var rmf in updater.GetAllRemoteFiles())
            {
                if (rmf.key.EndsWith(MPQUpdater.MPQ_EXT))
                {
                    MPQFileInfo mpq_info = new MPQFileInfo();
                    mpq_info.fileinfo = rmf.file;
                    mpq_info.entries = fs.LoadEntries(rmf.file);
                    foreach (MPQFileSystem.MPQFileEntry e in mpq_info.entries)
                    {
                        var exist_fe = fs.FindEntry(e.Key);
                        if (exist_fe != null)
                        {
                            bool is_old = !e.Equals(exist_fe);
                            if (is_old)
                            {
                                mpq_info.replaced_size += e.Size;
                                mpq_info.replaced_count += 1;
                            }
                        }
                    }
                    fsinfo.files[rmf] = mpq_info;
                }
            }
        }

        private void toolStripMenuItem_RefreshMPQ_Click(object sender, EventArgs e)
        {
            RefreshMPQ();
        }

        private void 导出文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode sn = treeView_MPQ.SelectedNode;
            if (sn != null && sn.Tag is MPQFileSystem.MPQFileEntry entry)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                string name = entry.Key.Replace('\\', '/');
                int index = entry.Key.LastIndexOf('/');
                if (index >= 0) { sfd.FileName = entry.Key.Substring(index + 1); }
                else { sfd.FileName = entry.Key; }
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    byte[] data = filesystem.GetEntryData(entry);
                    using (Stream stream = sfd.OpenFile())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
        }


        private void treeView_MPQ_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is MPQFileSystemInfo)
            {
                MPQFileSystemInfo mpq = (MPQFileSystemInfo)e.Node.Tag;
                property_MPQInfo.SelectedObject = mpq;
            }
            else if (e.Node.Tag is MPQFileInfo)
            {
                MPQFileInfo mpq = (MPQFileInfo)e.Node.Tag;
                property_MPQInfo.SelectedObject = mpq;
            }
            else if (e.Node.Tag is MPQFileSystem.MPQFileEntry mpq)
            {
                property_MPQInfo.SelectedObject = mpq;
            }
        }
        private void treeView_MPQ_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is MPQFileSystem.MPQFileEntry mpq)
            {
                try
                {
                    new FormEntry(mpq, filesystem).ShowDialog();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }


        private void toolStripButton_ViewReplaced_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            try
            {
                RefreshUpdateInfo();
            }
            finally
            {
                this.Enabled = true;
                treeView_MPQ.Refresh();
            }
        }

        public class MPQFileSystemInfo
        {
            internal Dictionary<MPQUpdater.RemoteFileInfo, MPQFileInfo> files =
                new Dictionary<MPQUpdater.RemoteFileInfo, MPQFileInfo>();

            [Description("文件容量")]
            public long FileSize
            {
                get
                {
                    long ret = 0;
                    foreach (MPQFileInfo f in files.Values)
                    {
                        ret += f.FileSize;
                    }
                    return ret;
                }
                set { }
            }
            [Description("文件个数")]
            public int EntryCount
            {
                get
                {
                    int ret = 0;
                    foreach (MPQFileInfo f in files.Values)
                    {
                        ret += f.EntryCount;
                    }
                    return ret;
                }
                set { }
            }
            [Description("冗余容量")]
            public long ReplacedSize
            {
                get
                {
                    long ret = 0;
                    foreach (MPQFileInfo f in files.Values)
                    {
                        ret += f.ReplacedSize;
                    }
                    return ret;
                }
                set { }
            }
            [Description("冗余文件数")]
            public int ReplacedFileCount
            {
                get
                {
                    int ret = 0;
                    foreach (MPQFileInfo f in files.Values)
                    {
                        ret += f.ReplacedFileCount;
                    }
                    return ret;
                }
                set { }
            }
        }
        public class MPQFileInfo
        {
            internal List<MPQFileSystem.MPQFileEntry> entries = new List<MPQFileSystem.MPQFileEntry>();
            internal FileInfo fileinfo;
            internal int replaced_count;
            internal long replaced_size;

            public FileInfo File
            {
                get { return fileinfo; }
                set { }
            }

            [Description("文件容量")]
            public long FileSize
            {
                get { return fileinfo.Length; }
                set { }
            }
            [Description("文件个数")]
            public int EntryCount
            {
                get { return entries.Count; }
                set { }
            }
            [Description("冗余容量")]
            public long ReplacedSize
            {
                get { return replaced_size; }
                set { }
            }
            [Description("冗余文件数")]
            public int ReplacedFileCount
            {
                get { return replaced_count; }
                set { }
            }
        }

        #endregion
        //--------------------------------------------------------------------------------------------
        #region TAB_FILE_STRUCT

        private void RefreshFileSystemView()
        {
            treeView_FS.Nodes.Clear();
            if (filesystem != null)
            {
                var root = filesystem.RootDirectory;
                if (root != null)
                {
                    addDir(treeView_FS.Nodes, root);
                }
            }
            void addDir(TreeNodeCollection root, MPQFileSystem.MPQDirectoryInfo dir)
            {
                var rd = new FSDirectoryNode(dir);
                root.Add(rd);
                foreach (var sub in dir.GetDirectories())
                {
                    addDir(rd.Nodes, sub);
                }
                foreach (var sf in dir.GetFiles())
                {
                    rd.Nodes.Add(new FSFileNode(sf));
                }
            }
        }
        public class FSDirectoryNode : TreeNode
        {
            public MPQFileSystem.MPQDirectoryInfo Directory { get; }
            public FSDirectoryNode(MPQFileSystem.MPQDirectoryInfo dir)
            {
                this.Text = dir.Name;
                this.Directory = dir;
                this.ToolTipText = dir.FullPath;
                this.SelectedImageIndex = this.ImageIndex = 0;
            }
        }
        public class FSFileNode : TreeNode
        {
            public MPQFileSystem.MPQFileInfo File { get; }
            public FSFileNode(MPQFileSystem.MPQFileInfo file)
            {
                this.Text = file.Name;
                this.File = file;
                this.ToolTipText = file.FullPath;
                this.SelectedImageIndex = this.ImageIndex = 1;
            }
        }
        #endregion
        //--------------------------------------------------------------------------------------------
    }
}
