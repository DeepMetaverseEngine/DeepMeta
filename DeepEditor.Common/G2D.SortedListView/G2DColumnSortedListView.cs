using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    /// <summary>
    /// 要求 ColumnItem 实现 IComparer
    /// </summary>
    public class G2DColumnSortedListView : ListView
    {
        public int SelectedColumnHeaderIndex { get; private set; }
        public G2DColumnSortedListView()
        {
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e); 
            if (this.ListViewItemSorter == null)
            {
                try
                {
                    this.ListViewItemSorter = this.Columns[0] as IComparer;
                }
                catch { }
            }
        }
        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);
            try
            {
                this.SuspendLayout();
                if (this.SelectedColumnHeaderIndex == e.Column)
                {
                    switch (this.Sorting)
                    {
                        case SortOrder.Ascending:
                            this.Sorting = SortOrder.Descending;
                            break;
                        case SortOrder.Descending:
                            this.Sorting = SortOrder.Ascending;
                            break;
                        case SortOrder.None:
                            this.Sorting = SortOrder.Ascending;
                            break;
                    }
                }
                this.SelectedColumnHeaderIndex = e.Column;
                this.ListViewItemSorter = this.Columns[e.Column] as IComparer;
            }
            finally
            {
                this.ResumeLayout();
            }
            this.Sort();
        }


    }
}
