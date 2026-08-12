using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public class G2DSortedListView : ListView
    {
        private int columnHeaderIndex = 0;
        public int SelectedColumnHeaderIndex { get { return columnHeaderIndex; } }

        public G2DSortedListView()
        {
            this.ColumnClick += ListView1_ColumnClick;
        }

        public void SetColumnComparer(params IComparer<ListViewItem>[] sorter)
        {
            this.ListViewItemSorter = new DefaultColumnComparer(this, sorter);
        }
        public void SetColumnComparison(params Comparison<ListViewItem>[] sorter)
        {
            this.ListViewItemSorter = new DefaultColumnComparison(this, sorter);
        }
        public void SetColumnItemTagComparison<T>(params Comparison<T>[] sorter)
        {
            this.ListViewItemSorter = new DefaultColumnItemTagComparison<T>(this, sorter);
        }

        private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.SuspendLayout();
                if (columnHeaderIndex == e.Column)
                {
                    this.columnHeaderIndex = e.Column;
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
                this.columnHeaderIndex = e.Column;
                this.Sort();
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        public class DefaultColumnComparer : IComparer
        {
            private G2DSortedListView view;
            private IComparer<ListViewItem>[] columnSorters;
            public DefaultColumnComparer(G2DSortedListView view, params IComparer<ListViewItem>[] sorter)
            {
                this.view = view;
                this.columnSorters = sorter;
            }
            public virtual int Compare(object x, object y)
            {
                if (view.Sorting == SortOrder.Descending) { DeepCore.CUtils.Swap(ref x, ref y); }
                var ex = (x as ListViewItem);
                var ey = (y as ListViewItem);
                if (view.SelectedColumnHeaderIndex < columnSorters.Length)
                {
                    return columnSorters[view.SelectedColumnHeaderIndex].Compare(ex, ey);
                }
                return ex.Text.CompareTo(ey.Text);
            }
        }
        public class DefaultColumnComparison : IComparer
        {
            private G2DSortedListView view;
            private Comparison<ListViewItem>[] columnSorters;
            public DefaultColumnComparison(G2DSortedListView view, params Comparison<ListViewItem>[] sorter)
            {
                this.view = view;
                this.columnSorters = sorter;
            }
            public virtual int Compare(object x, object y)
            {
                if (view.Sorting == SortOrder.Descending) { DeepCore.CUtils.Swap(ref x, ref y); }
                var ex = (x as ListViewItem);
                var ey = (y as ListViewItem);
                if (view.SelectedColumnHeaderIndex < columnSorters.Length)
                {
                    return columnSorters[view.SelectedColumnHeaderIndex](ex, ey);
                }
                return ex.Text.CompareTo(ey.Text);
            }
        }
        public class DefaultColumnItemTagComparison<T> : IComparer
        {
            private G2DSortedListView view;
            private Comparison<T>[] columnSorters;
            public DefaultColumnItemTagComparison(G2DSortedListView view, params Comparison<T>[] sorter)
            {
                this.view = view;
                this.columnSorters = sorter;
            }
            public virtual int Compare(object x, object y)
            {
                if (view.Sorting == SortOrder.Descending) { DeepCore.CUtils.Swap(ref x, ref y); }
                var ex = (x as ListViewItem);
                var ey = (y as ListViewItem);
                if (view.SelectedColumnHeaderIndex < columnSorters.Length)
                {
                    T tx = (T)Convert.ChangeType(ex.Tag, typeof(T));
                    T ty = (T)Convert.ChangeType(ey.Tag, typeof(T));
                    return columnSorters[view.SelectedColumnHeaderIndex](tx, ty);
                }
                return ex.Text.CompareTo(ey.Text);
            }
        }
    }

 
}
