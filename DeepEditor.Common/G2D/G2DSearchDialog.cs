using MaterialSkin.Controls;
using DeepEditor.Common.G2D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public partial class G2DSearchDialog : G2DBaseForm
    {
        private static string last_find_text = "";
        public G2DSearchDialog(string title = "")
        {
            InitializeComponent();

            SetTitle(title);
            this.Activated += G2DSearchDialog_Activated;
            this.FormClosing += G2DSearchDialog_FormClosing; ;
        }

        public void Resume()
        {
            this.textBox1.Text = last_find_text;
            this.textBox1.SelectAll();
            this.textBox1.Focus();
        }

        private void G2DSearchDialog_Activated(object sender, EventArgs e)
        {
            Resume();
        }
        private void G2DSearchDialog_Shown(object sender, EventArgs e)
        {
            Resume();
        }
        private void G2DSearchDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            last_find_text = textBox1.Text;
        }
        private void G2DSearchDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            last_find_text = textBox1.Text;
        }

        public string GetText()
        {
            return textBox1.Text;
        }

        private void findOver(object finded)
        {
            if (finded == null)
            {
                lbl_status.ForeColor = Color.Red;
                lbl_status.Text = "未找到";
            }
            else
            {
                lbl_status.ForeColor = SkinManager.TextHighEmphasisColor;
                lbl_status.Text = finded.ToString();
            }
        }

        //----------------------------------------------------------------------------------

        public delegate object OnFindNextClicked(string text);
        public delegate object OnFindPrevClicked(string text);
        public delegate object OnFindClicked(string text);
        public delegate void OnFindCloseClicked();

        public event OnFindNextClicked FindNextClicked;
        public event OnFindPrevClicked FindPrevClicked;
        public event OnFindClicked FindClicked;
        public event OnFindCloseClicked CloseClicked;

        private void button_next_Click(object sender, EventArgs e)
        {
            if (FindNextClicked != null)
            {
                object finded = FindNextClicked.Invoke(GetText());
                findOver(finded);
            }
        }
        private void button_prev_Click(object sender, EventArgs e)
        {
            if (FindPrevClicked != null)
            {
                object finded = FindPrevClicked.Invoke(GetText());
                findOver(finded);
            }
        }

        private void button_find_Click(object sender, EventArgs e)
        {
            if (FindClicked != null)
            {
                object finded = FindClicked.Invoke(GetText());
                findOver(finded);
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            this.Close();

            if (CloseClicked != null)
            {
                CloseClicked.Invoke();
            }
        }

        private void G2DSearchDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.Enter)
            {
                if (FindClicked != null)
                {
                    object finded = FindClicked.Invoke(GetText());
                    findOver(finded);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys kd)
        {
            switch (kd)
            {
                case Keys.F3:       //查找
                    //正向查找
                    if (FindNextClicked != null)
                    {
                        object finded = FindNextClicked.Invoke(GetText());
                        findOver(finded);
                    }
                    return true;
                case Keys.F3 | Keys.Shift:
                    //反向查找
                    if (FindPrevClicked != null)
                    {
                        object finded = FindPrevClicked.Invoke(GetText());
                        findOver(finded);
                    }
                    return true;
            }

            return base.ProcessCmdKey(ref msg, kd);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            {
                if (CloseClicked != null)
                {
                    CloseClicked.Invoke();
                }
            }

            base.WndProc(ref m);
        }
        //----------------------------------------------------------------------------------


        public void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return;
            }
            this.Text = title;
        }
    }
}