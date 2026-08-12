using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DeepCore;
using DeepCore.Reflection.Modeling;
using DeepCore.Reflection;
using System.Reflection;

namespace DeepEditor.Common.G2D.DataGrid
{
    public partial class G2DDataGrid : UserControl
    {
        private List<object> mDatas;
        private HashMap<object, UmlDocument> mDataMap = new HashMap<object, UmlDocument>();
        private List<List<UmlNode>> mDataGrid = new List<List<UmlNode>>();

        private bool mIsValueChanged = false;

        public G2DDataGrid(params object[] datas) : this(new List<object>(datas)) { }
        public G2DDataGrid(IList<object> datas)
        {
            InitializeComponent();

            mDatas = new List<object>(datas);

            CellClickEvent cell_click = new CellClickEvent(this); 
            SourceGrid.Cells.Views.Cell[] views = new SourceGrid.Cells.Views.Cell[2];
            {
                SourceGrid.Cells.Views.Cell view1 = new SourceGrid.Cells.Views.Cell();
                view1.BackColor = Color.FromArgb(0xFF, 0xF0, 0xFF, 0xF0);
                SourceGrid.Cells.Views.Cell view2 = new SourceGrid.Cells.Views.Cell();
                view2.BackColor = Color.FromArgb(0xFF, 0xF0, 0xF0, 0xFF);
                views[0] = view1;
                views[1] = view2;
            }
            SourceGrid.Cells.Views.RowHeader hd_view_row = new SourceGrid.Cells.Views.RowHeader();
            hd_view_row.ForeColor = Color.FromArgb(0xFF, 0, 0, 0);
            hd_view_row.BackColor = Color.FromArgb(0xFF, 0x80, 0x80, 0xFF);
            SourceGrid.Cells.Views.ColumnHeader hd_view_col = new SourceGrid.Cells.Views.ColumnHeader();
            hd_view_col.ForeColor = Color.FromArgb(0xFF, 0, 0, 0);
            hd_view_col.BackColor = Color.FromArgb(0xFF, 0x80, 0x80, 0xFF);

            if (datas.Count > 0)
            {
                foreach (object data in datas)
                {
                    UmlDocument root = new UmlDocument(data);
                    mDataMap.Put(data, root);
                    List<UmlNode> rows = UmlUtils.ListElements(root);
//                     if (mDataGrid.Count > 0 && !UmlUtils.StructEquals(mDataGrid[0], rows))
//                     {
//                         throw new Exception("结构不一致");
//                     }
                    mDataGrid.Add(rows);
                }

                grid1.Redim(mDataGrid[0].Count + 1, datas.Count + 2);
                grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
                grid1.FixedColumns = 1;
                grid1.FixedRows = 1;
                grid1[0, 0] = new SourceGrid.Cells.ColumnHeader("Properties");
                for (int c = 1; c <= mDataGrid.Count; c++)
                {
                    int index = c - 1;
                    SourceGrid.Cells.ColumnHeader data_hd = new SourceGrid.Cells.ColumnHeader(datas[index].ToString());
                    data_hd.AutomaticSortEnabled = false;
                    data_hd.Tag = datas[index];
                    data_hd.View = hd_view_col;
                    grid1[0, c] = data_hd;
                }
                List<UmlNode> head = mDataGrid[0];
                for (int r = 1; r <= head.Count; r++)
                {
                    UmlNode rowHead = head[r - 1];
                    SourceGrid.Cells.RowHeader field_hd = new SourceGrid.Cells.RowHeader(GetDepthText(rowHead) + rowHead.Name);
                    field_hd.Tag = rowHead;
                    field_hd.View = hd_view_row;
                    field_hd.AddController(cell_click);
                    grid1[r, 0] = field_hd;
                    for (int c = 1; c <= mDataGrid.Count; c++)
                    {
                        UmlNode cell = mDataGrid[c - 1][r - 1];
                        if (cell is UmlValueNode)
                        {
                            UmlValueNode cellvalue = cell as UmlValueNode;
                            if (cellvalue.FieldDesc != null && !cellvalue.FieldDesc.Editable)
                            {
                                continue;
                            }
                            else if (cellvalue.IsLeaf)
                            {
                                SourceGrid.Cells.Cell cc = new SourceGrid.Cells.Cell(cellvalue.Value, cellvalue.ValueType);
                                cc.Tag = cellvalue;
                                cc.View = views[c % 2];
                                cc.AddController(cell_click);
                                grid1[r, c] = cc;
                            }
                        }
                    }
                }

            }
        }

        static public string GetDepthText(UmlNode e)
        {
            StringBuilder ret = new StringBuilder();
            UmlNode node = e;
            while (node.ParentNode != null)
            {
                ret.Append("  ");
                node = node.ParentNode;
            }
            return ret.ToString();
        }

        private class CellClickEvent : SourceGrid.Cells.Controllers.ControllerBase
        {
            public readonly G2DDataGrid Owner;

            public CellClickEvent(G2DDataGrid owner)
            {
                this.Owner = owner;
            }

            public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnClick(sender, e);
                if (sender.Cell is SourceGrid.Cells.Cell)
                {
                    SourceGrid.Cells.Cell scell = sender.Cell as SourceGrid.Cells.Cell;
                    if (scell.Tag is UmlValueNode)
                    {
                        UmlValueNode cellvalue = scell.Tag as UmlValueNode;
                        if (cellvalue.FieldDesc != null)
                        {
                            FieldInfo field = cellvalue.OwnerIndexer as FieldInfo;
                            Owner.textDesc.Text =
                                "描述: " + cellvalue.FieldDesc.Desc + "\n" +
                                "类型: " + cellvalue.ValueType + "\n";
                        }
                    }
                }
                //MessageBox.Show(sender.Grid, sender.DisplayText);
            }
            public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnValueChanged(sender, e);

                Owner.TryChangeValue();
            }
        }

        private void TryChangeValue()
        {
            this.mIsValueChanged = true;

            for (int c = 0; c < mDataGrid.Count; c++)
            {
                for (int r = 0; r < mDataGrid[c].Count; r++)
                {
                    SourceGrid.Cells.Cell cc = grid1[r + 1, c + 1] as SourceGrid.Cells.Cell;
                    if (cc != null)
                    {
                        UmlValueNode cellvalue = cc.Tag as UmlValueNode;
                        if (cc.Value != cellvalue.Value)
                        {
                            if (OnDataChanged != null)
                            {
                                OnDataChanged.Invoke(this, cellvalue, cc.Value);
                            }
                            return;
                        }
                    }
                }
            }

            this.mIsValueChanged = false;
        }

        public bool IsDataChanged
        {
            get
            {
                return mIsValueChanged;
            }
        }

        public void SaveAll()
        {
            for (int c = 0; c < mDataGrid.Count; c++)
            {
                for (int r = 0; r < mDataGrid[c].Count; r++)
                {
                    SourceGrid.Cells.Cell cc = grid1[r + 1, c + 1] as SourceGrid.Cells.Cell;
                    if (cc != null)
                    {
                        UmlValueNode cellvalue = cc.Tag as UmlValueNode;
                        if (cc.Value != cellvalue.Value)
                        {
                            cellvalue.SetValue(cc.Value);
                        }
                    }
                }
            }
        }


        public delegate void DataChangedHandler(G2DDataGrid sender, UmlValueNode cell, object new_value);
        public event DataChangedHandler OnDataChanged;
    }
}
