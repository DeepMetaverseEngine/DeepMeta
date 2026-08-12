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
    public partial class G2DTextDialog : G2DBaseForm
    {
        public G2DTextDialog(string value)
        {
            InitializeComponent();
            this.textBox1.Text = value;
            this.Shown += G2DTextDialog_Shown;
        }

        private void G2DTextDialog_Shown(object sender, EventArgs e)
        {
            this.textBox1.SelectAll();
            this.textBox1.Focus();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public string GetText()
        {
            return textBox1.Text.Trim();
        }

        //----------------------------------------------------------------------------------


        public static string Show(string value, string title, Size size)
        {
            var dialog = new G2DTextDialog(value);
            if (title != null) { dialog.Text = title; }
            if (!size.IsEmpty) { dialog.Size = size; }
            var res = dialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(dialog.GetText()))
                {
                    return dialog.GetText();
                }
            }
            return null;
        }
        public static string Show(string value, string title)
        {
            return Show(value, title, Size.Empty);
        }
        public static string Show(string value)
        {
            return Show(value, value);
        }

        //----------------------------------------------------------------------------------

        public delegate bool TryConvert<T>(string value, out T result);

        public static bool TryShow<T>(string value, out T result, TryConvert<T> tostring, string title, Size size)
        {
            var txt = Show(value, title, size);
            if (txt != null && tostring(txt, out result))
            {
                return true;
            }
            result = default(T);
            return false;
        }
        public static bool TryShow<T>(string value, out T result, TryConvert<T> tostring, string title)
        {
            return TryShow<T>(value, out result, tostring, title, Size.Empty);
        }
        public static bool TryShow<T>(string value, out T result, TryConvert<T> tostring)
        {
            return TryShow<T>(value, out result, tostring, value);
        }

        public static T Show<T>(string value, TryConvert<T> tostring, string title, Size size)
        {
            var txt = Show(value, title, size);
            if (txt != null && tostring(txt, out var result))
            {
                return result;
            }
            return default(T);
        }
        public static T Show<T>(string value, TryConvert<T> tostring, string title)
        {
            return Show<T>(value, tostring, title, Size.Empty);
        }
        public static T Show<T>(string value, TryConvert<T> tostring)
        {
            return Show<T>(value, tostring, value);
        }

        //----------------------------------------------------------------------------------
    }
}