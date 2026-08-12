using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DeepCore.Xml;
using DeepCore.Reflection;
using Newtonsoft.Json;
using MaterialSkin.Controls;
using DeepEditor.Common.G2D;

namespace DeepEditor.Common.G2D.DataGrid
{
    public partial class G2DXmlEditor : G2DBaseForm
    {
        private Type dataType;
        public G2DXmlEditor(Type dataType, object data, bool enableClear)
        {
            InitializeComponent();
            this.buttonClear.Visible = enableClear;
            this.dataType = dataType;
            if (data == null)
            {
                if (dataType.IsValueType)
                {
                    data = DeepActivator.CreateInstance(dataType);
                }
                else
                {
                    data = G2DCreateInstanceDialog.ShowCreateInstanceDialog(dataType, this);
                }
            }
            this.text.Text = XmlUtil.DataToXmlText(data);
        }

        public object EditObject
        {
            get
            {
                try
                {
                    return XmlUtil.XmlTextToObject(this.text.Text, dataType);
                }
                catch { }
                return null;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.text.Text = string.Empty;
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}
