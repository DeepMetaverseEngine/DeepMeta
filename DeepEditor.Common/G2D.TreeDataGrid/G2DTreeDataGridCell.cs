using AdvancedDataGridView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Common.Utils.TreeGrid
{
    public class G2DTreeGridValueColumn : DataGridViewTextBoxColumn
    {
        public G2DTreeGridValueColumn()
        {
            this.CellTemplate = new G2DTreeGridValueCell();
        }
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                G2DTreeGridValueCell cell = value as G2DTreeGridValueCell;
                if (value != null && cell == null)
                {
                    throw new InvalidCastException("Value provided for CellTemplate must be of type TEditNumDataGridViewCell or derive from it.");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class G2DTreeGridValueCell : DataGridViewTextBoxCell
    {
        public G2DTreeGridValueCell()
        {

        }

        private Type editType = typeof(TextG2DTreeGridValueEditingControl);
        private Type valueType = typeof(string);

        public void SetValueEditType(Type vtype, Type etype = null)
        {
            this.valueType = vtype;
            if (etype != null)
            {
                this.editType = etype;
            }
            else
            {
                if (valueType.IsEnum)
                {
                    this.editType = typeof(EnumG2DTreeGridValueEditingControl);
                }
                else if (typeof(bool).IsAssignableFrom(ValueType))
                {
                    this.editType = typeof(BoolG2DTreeGridValueEditingControl);
                }
                else
                {
                    this.editType = typeof(TextG2DTreeGridValueEditingControl);
                }
            }
        }

        public override Type EditType
        {
            get { return editType; }
        }
        public override Type ValueType
        {
            get { return valueType; }
        }
        public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter formattedValueTypeConverter, System.ComponentModel.TypeConverter valueTypeConverter)
        {
            return base.ParseFormattedValue(formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
        }
    }


    public class TextG2DTreeGridValueEditingControl : DataGridViewTextBoxEditingControl
    {
        public TextG2DTreeGridValueEditingControl()
        {
        }
    }
    public class EnumG2DTreeGridValueEditingControl : DataGridViewComboBoxEditingControl
    {
        public EnumG2DTreeGridValueEditingControl()
        {
        }
        public void SetEnumType(Type type)
        {
            if (type.IsEnum)
            {
                base.Items.Clear();
                foreach (object obj in Enum.GetValues(type))
                {
                    base.Items.Add(obj.ToString());
                }
            }
        }
        public G2DTreeGridValueCell CurrentCell
        {
            get
            {
                return EditingControlDataGridView[2, EditingControlRowIndex] as G2DTreeGridValueCell;
            }
        }
        public override void PrepareEditingControlForEdit(bool selectAll)
        {
            this.Text = CurrentCell.Value + "";
            SetEnumType(CurrentCell.ValueType);
        }
    }
    public class BoolG2DTreeGridValueEditingControl : DataGridViewComboBoxEditingControl
    {
        public BoolG2DTreeGridValueEditingControl()
        {
            base.Items.Add(true.ToString());
            base.Items.Add(false.ToString());
        }
        public G2DTreeGridValueCell CurrentCell
        {
            get
            {
                return EditingControlDataGridView[2, EditingControlRowIndex] as G2DTreeGridValueCell;
            }
        }
        public override void PrepareEditingControlForEdit(bool selectAll)
        {
            this.Text = CurrentCell.Value + "";
        }
    }



    public class MoneyTextBoxDataGridViewEditingControl : TextBox, IDataGridViewEditingControl
    {
        private DataGridView dataGridView;  // grid owning this editing control
        private bool valueChanged;  // editing control's value has changed or not
        private int rowIndex;  //  row index in which the editing control resides

        #region IDataGridViewEditingControl 成员

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            if (dataGridViewCellStyle.BackColor.A < 255)
            {
                // The NumericUpDown control does not support transparent back colors
                Color opaqueBackColor = Color.FromArgb(255, dataGridViewCellStyle.BackColor);
                this.BackColor = opaqueBackColor;
                this.dataGridView.EditingPanel.BackColor = opaqueBackColor;
            }
            else
            {
                this.BackColor = dataGridViewCellStyle.BackColor;
            }
            this.ForeColor = dataGridViewCellStyle.ForeColor;
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return this.dataGridView;
            }
            set
            {
                this.dataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
            }
            set
            {
                this.Text = (string)value;
            }
        }

        public int EditingControlRowIndex
        {
            get
            {
                return this.rowIndex;
            }
            set
            {
                this.rowIndex = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get { return this.valueChanged; }
            set { this.valueChanged = value; }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Down:
                case Keys.Up:
                case Keys.Home:
                case Keys.End:
                case Keys.Delete:
                    return true;
            }
            return !dataGridViewWantsInputKey;
        }

        public Cursor EditingPanelCursor
        {
            get { return Cursors.Default; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Text;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
            {
                this.SelectAll();
            }
            else
            {
                this.SelectionStart = this.Text.Length;
            }
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        #endregion
    }
}
