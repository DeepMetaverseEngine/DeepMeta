using MaterialSkin.Controls; using DeepEditor.Common.G2D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public partial class G2DPasswordDialog : G2DBaseForm
    {
        public G2DPasswordDialog(string value)
        {
            InitializeComponent();
            this.textBox1.Text = value;
            this.textBox1.SelectAll();
            this.textBox1.Focus();
        }


        public string GetText()
        {
            return textBox1.Text;
        }

        //----------------------------------------------------------------------------------
        public static string ShowPassword(string value, string title, Size size)
        {
            var dialog = new G2DPasswordDialog(value);
            if (title != null) { dialog.Text = title; }
            if (!size.IsEmpty) { dialog.Size = size; }
            DialogResult res = dialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(dialog.GetText()))
                {
                    return dialog.GetText();
                }
            }
            return null;
        }
        public static string ShowPassword(string value, string title)
        {
            return ShowPassword(value, title, Size.Empty);
        }
        public static string ShowPassword(string value)
        {
            return ShowPassword(value, value);
        }

        //----------------------------------------------------------------------------------
    }
}