using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DeepCore.Xml;
using DeepCore.Reflection;
using MaterialSkin.Controls;
using DeepEditor.Common.G2D;

namespace DeepEditor.Common.G2D.DataGrid
{
    public partial class G2DFieldEditor : G2DBaseForm
    {
        //private object edit_data;

        public G2DFieldEditor(Type dataType, object data, bool enableClear, params IG2DPropertyAdapter[] adapters)
        {
            InitializeComponent();
            this.buttonClear.Visible = enableClear;
            object edit_data;
            if (data != null)
            {
                edit_data = XmlUtil.CloneObject(data);
            }
            else
            {
                var instance = G2DCreateInstanceDialog.ShowCreateInstanceDialog(dataType, this);
                edit_data = instance;// ReflectionUtil.CreateInstance(dataType);
            }
            this.propertyGrid1.SelectedObject = G2DTypeDescriptor.CreateDescriptor(edit_data, adapters);
            this.propertyGrid1.PropertySort = PropertySort.Categorized;
        }

        public object EditObject
        {
            get
            {
                if (this.propertyGrid1.SelectedObject is G2DTypeDescriptor desc)
                {
                    return desc.EditData;
                }
                return null;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = null;
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

    }
}
