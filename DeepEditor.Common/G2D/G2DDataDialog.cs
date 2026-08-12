using DeepCore;
using DeepCore.IO;
using DeepCore.Reflection;
using DeepEditor.Common.G2D.DataGrid;
using MaterialSkin.Controls;
using DeepEditor.Common.G2D;
using System;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    abstract public partial class G2DDataDialog : G2DBaseForm
    {
        public G2DDataDialog(object data, params IG2DPropertyAdapter[] adds)
        {
            InitializeComponent();
            this.g2DPropertyGrid1.SetSelectedObject(data, adds);
            this.g2DPropertyGrid1.Focus();
        }
        public G2DDataDialog(G2DTypeDescriptor desc)
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
            get
            {
                var ret = this.g2DPropertyGrid1.GetSelectedValue();
                return ret;
            }
        }

        public static T ShowObjectDialog<T>(T data, params IG2DPropertyAdapter[] adds) where T : class, new()
        {
            var dialog = new G2DObjectDialog<T>(data, adds);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedObject;
            }
            return default;
        }
        public static T ShowObjectDialog<T>(IWin32Window win, T data, params IG2DPropertyAdapter[] adds) where T : class, new()
        {
            var dialog = new G2DObjectDialog<T>(data, adds);
            if (dialog.ShowDialog(win) == DialogResult.OK)
            {
                return dialog.SelectedObject;
            }
            return default;
        }
        public static T ShowStructDialog<T>(T data, params IG2DPropertyAdapter[] adds) where T : struct
        {
            var dialog = new G2DStructDialog<T>(data, adds);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedObject;
            }
            return default;
        }
        public static T ShowStructDialog<T>(IWin32Window win, T data, params IG2DPropertyAdapter[] adds) where T : struct
        {
            var dialog = new G2DStructDialog<T>(data, adds);
            if (dialog.ShowDialog(win) == DialogResult.OK)
            {
                return dialog.SelectedObject;
            }
            return default;
        }


        abstract public partial class G2DDataDialogT<T> : G2DDataDialog
        {
            public G2DDataDialogT(T data, params IG2DPropertyAdapter[] adds) : base(data, adds) { }
            public G2DDataDialogT(G2DTypeDescriptor desc) : base(desc) { }
            new public T SelectedObject
            {
                get
                {
                    var ret = base.SelectedObject;
                    return (T)ret;
                }
            }
        }

        public partial class G2DObjectDialog<T> : G2DDataDialogT<T> where T : class, new()
        {
            public G2DObjectDialog(T data, params IG2DPropertyAdapter[] adds) : base(Clone(data), adds) { }
            public G2DObjectDialog(G2DTypeDescriptor desc) : base(desc) { }
            private static T Clone(T src)
            {
                if (src is ICloneable c)
                {
                    return (T)c.Clone();
                }
                else
                {
                    var ret = new T();
                    if (src != null)
                    {
                        PropertyUtil.CopyFieldsTo(src, ret);
                    }
                    return ret;
                }
            }
        }

        public partial class G2DStructDialog<T> : G2DDataDialogT<T> where T : struct
        {
            public G2DStructDialog(T data, params IG2DPropertyAdapter[] adds) : base(data, adds) { }
            public G2DStructDialog(G2DTypeDescriptor desc) : base(desc) { }
        }
    }
}