using DeepCore.IO;
using DeepCore.MPQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace DeepTools.MPQ
{
    public partial class FormUnarchive : Form
    {
        public static DirectoryInfo ToDefaultOutputDir(FileInfo mpq_file)
        {
            return new DirectoryInfo(mpq_file.Directory.FullName + Path.DirectorySeparatorChar +
                   mpq_file.Name.Substring(0, mpq_file.Name.Length - mpq_file.Extension.Length));
        }
        public static DirectoryInfo ToDefaultOutputDir(DirectoryInfo root, FileInfo mpq_file)
        {
            return new DirectoryInfo(root.FullName + Path.DirectorySeparatorChar +
                   mpq_file.Name.Substring(0, mpq_file.Name.Length - mpq_file.Extension.Length));
        }

        public static FormUnarchive OpenUnarchive(FileInfo mpq_file)
        {
            return OpenUnarchive(mpq_file, ToDefaultOutputDir(mpq_file));
        }
        public static FormUnarchive OpenUnarchive(FileInfo mpq_file, DirectoryInfo output_dir)
        {
            var mpq_fs = new MPQFileSystem();
            mpq_fs.LoadMPQ(mpq_file);
            var ret = new FormUnarchive(mpq_fs.ListEntrys(), output_dir);
            ret.Disposed += (sender, e) => { mpq_fs.Dispose(); };
            return ret;
        }
        public static FormUnarchive OpenUnarchive(MPQFileSystem mpq_fs, DirectoryInfo output_dir)
        {
            var ret = new FormUnarchive(mpq_fs.ListEntrys(), output_dir);
            return ret;
        }
        public static FormUnarchive OpenUnarchive(List<MPQFileSystem.MPQFileEntry> entries, DirectoryInfo output_dir)
        {
            var ret = new FormUnarchive(entries, output_dir);
            return ret;
        }
        public static FormUnarchive OpenUnarchive(MPQFileSystem.MPQFileEntry[] entries, DirectoryInfo output_dir)
        {
            var ret = new FormUnarchive(new List<MPQFileSystem.MPQFileEntry>(entries), output_dir);
            return ret;
        }


        private FormUnarchive(List<MPQFileSystem.MPQFileEntry> entries, DirectoryInfo output_dir)
        {
            InitializeComponent();
            this.output_dir = output_dir;
            this.entries = entries;
            Thread t = new Thread(run_extract);
            t.Start();
        }


        private DirectoryInfo output_dir;
        private List<MPQFileSystem.MPQFileEntry> entries;
        private bool is_done = false;
        private Exception error;
        private string efile;
        private int max = 100;
        private int cur = 0;
        private void run_extract()
        {
            try
            {
                max = entries.Count;
                foreach (var e in entries)
                {
                    efile = e.Key;
                    byte[] data = e.GetFileData();
                    FileInfo save = new FileInfo(output_dir.FullName + Path.DirectorySeparatorChar + e.Key);
                    CFiles.CreateFile(save);
                    File.WriteAllBytes(save.FullName, data);
                    cur++;
                }
            }
            catch (Exception err)
            {
                error = err;
            }
            finally
            {
                is_done = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (error != null)
            {
                MessageBox.Show(error.Message);
            }
            progressBar1.Maximum = max;
            progressBar1.Value = cur;
            label1.Text = efile;
            if (is_done)
            {
                this.Close();
            }
        }
    }
}
