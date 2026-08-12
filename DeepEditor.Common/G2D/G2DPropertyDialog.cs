using DeepEditor.Common.G2D.DataGrid;
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
    public partial class G2DPropertyDialog : G2DBaseForm
    {
        public G2DPropertyDialog(object data, params IG2DPropertyAdapter[] adds)
        {
            InitializeComponent();
            if (data != null)
            {
                this.g2DPropertyGrid1.SetSelectedObject(data, adds);
                this.g2DPropertyGrid1.Focus();
            }
        }
        public G2DPropertyDialog(G2DTypeDescriptor desc)
        {
            InitializeComponent();
            this.g2DPropertyGrid1.SelectedObject = desc;
            this.g2DPropertyGrid1.Focus();
        }

        public G2DPropertyGrid PropertyGrid
        {
            get { return this.g2DPropertyGrid1; }
        }
        public object SelectedObject
        {
            get { return this.g2DPropertyGrid1.GetSelectedValue(); }
        }

        //----------------------------------------------------------------------------------


    }

    public class G2DPropertyDialog<T> : G2DPropertyDialog
        where T : class, new()
    {
        public G2DPropertyDialog(T data, params IG2DPropertyAdapter[] adds) : base(data, adds)
        {
            if (data == null)
            {
                data = new T();
                this.PropertyGrid.SetSelectedObject(data, adds);
                this.PropertyGrid.Focus();
            }
        }
        public G2DPropertyDialog(G2DTypeDescriptor desc) : base(desc)
        {
        }

        new public T SelectedObject
        {
            get { return this.PropertyGrid.GetSelectedValue() as T; }
        }

        //----------------------------------------------------------------------------------

        public static T Show(string title, T data, params IG2DPropertyAdapter[] adds)
        {
            var dialog = new G2DPropertyDialog<T>(data, adds);
            dialog.Text = title;
            DialogResult res = dialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedObject;
            }
            return null;
        }
        public static T Show(IWin32Window window, string title, T data, params IG2DPropertyAdapter[] adds)
        {
            var dialog = new G2DPropertyDialog<T>(data, adds);
            dialog.Text = title;
            DialogResult res = dialog.ShowDialog(window);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedObject;
            }
            return null;
        }

    }


}