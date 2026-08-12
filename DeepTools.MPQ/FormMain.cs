using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeepTools.MPQ
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        public void LoadMPQ(FileInfo file)
        {
            entriesPanel1.LoadMPQ(file);
        }
        private void btnOpenMPQ_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MPQ文件(*.mpq)|*.mpq|所有文件(*.*)|*.*";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    entriesPanel1.LoadMPQ(new FileInfo(ofd.FileName));
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }
        }
        private void btnExtractTo_Click(object sender, EventArgs e)
        {
            if (entriesPanel1.SelectedMPQFile != null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.SelectedPath = entriesPanel1.SelectedMPQFile.DirectoryName;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FormUnarchive.OpenUnarchive(entriesPanel1.AllEntries, FormUnarchive.ToDefaultOutputDir(
                        new DirectoryInfo(fbd.SelectedPath), entriesPanel1.SelectedMPQFile)).ShowDialog();
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message);
                    }
                }
            }
        }

        private void btnExtractSelectedTo_Click(object sender, EventArgs e)
        {
            if (entriesPanel1.SelectedMPQFile != null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.SelectedPath = entriesPanel1.SelectedMPQFile.DirectoryName;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FormUnarchive.OpenUnarchive(entriesPanel1.CheckedEntries, FormUnarchive.ToDefaultOutputDir(
                        new DirectoryInfo(fbd.SelectedPath), entriesPanel1.SelectedMPQFile)).ShowDialog();
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message);
                    }
                }
            }
        }
    }
}
