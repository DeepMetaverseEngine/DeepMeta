using DeepCore;
using DeepCore.MPQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DeepTools.MPQ
{
    public partial class EntriesPanel : UserControl
    {
        private MPQFileSystem mpq;

        public EntriesPanel()
        {
            InitializeComponent();
            this.Load += EntriesPanel_Load;
            this.Disposed += EntriesPanel_Disposed;
        }

        private void EntriesPanel_Load(object sender, EventArgs e)
        {
            this.listView1.SetColumnItemTagComparison<MPQFileSystem.MPQFileEntry>(
                (x, y) => { return x.Key.CompareTo(y.Key); },
                (x, y) => { return x.Size.CompareTo(y.Size); },
                (x, y) => { return x.Date.CompareTo(y.Date); }
                );
        }
        private void EntriesPanel_Disposed(object sender, EventArgs e)
        {
            if (mpq != null)
            {
                mpq.Dispose();
            }
        }

        public void LoadMPQ(FileInfo file)
        {
            try
            {
                this.SuspendLayout();
                mpq = new MPQFileSystem();
                mpq.LoadMPQ(file);
                listView1.Items.Clear();
                List<ListViewItem> adding = new List<ListViewItem>();
                foreach (var e in mpq.ListEntrys())
                {
                    ListViewItem item = new ListViewItem(new string[] { e.Key, e.Size.ToString(), e.Date.ToString() });
                    item.Tag = e;
                    adding.Add(item);
                }
                listView1.Items.AddRange(adding.ToArray());
                listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                this.SelectedMPQFile = file;
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        public FileInfo SelectedMPQFile
        {
            get; private set;
        }
        public MPQFileSystem.MPQFileEntry[] AllEntries
        {
            get
            {
                using (var list = new ArrayList<MPQFileSystem.MPQFileEntry>())
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        list.Add(item.Tag as MPQFileSystem.MPQFileEntry);
                    }
                    return list.ToArray();
                }
            }
        }
        public MPQFileSystem.MPQFileEntry[] CheckedEntries
        {
            get
            {
                using (var list = new ArrayList<MPQFileSystem.MPQFileEntry>())
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Checked)
                        {
                            list.Add(item.Tag as MPQFileSystem.MPQFileEntry);
                        }
                    }
                    return list.ToArray();
                }
            }
        }
    }
}
