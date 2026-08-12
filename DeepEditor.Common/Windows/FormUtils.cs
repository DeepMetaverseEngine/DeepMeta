using DeepCore;
using DeepCore.Log;
using DeepCore.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common
{

    //可搜索的
    public interface ISearchable
    {
        bool Contains(string text);
    }

    public static class FormUtils
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(
int hWnd, // window handle
int hWndInsertAfter, // placement-order handle
int X, // horizontal position
int Y, // vertical position
int cx, // width
int cy, // height
uint uFlags); // window positioning flags

        const uint SWP_NOSIZE = 0x1;
        const uint SWP_NOMOVE = 0x2;
        const uint SWP_SHOWWINDOW = 0x40;
        const uint SWP_NOACTIVATE = 0x10;

        static public void InsertAfterZOrder(IntPtr hWnd, IntPtr insertAfter)
        {
            SetWindowPos((int)hWnd,
                (int)insertAfter,
                0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW | SWP_NOACTIVATE);
        }


        [DllImport("user32.dll", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool turnOn);

        /// <summary>
        /// 切换到当前窗口
        /// </summary>
        /// <param name="form"></param>
        /// <param name="turnOn"></param>
        static public void SwithToThisForm(Form form, bool turnOn)
        {
            SwitchToThisWindow(form.Handle, turnOn);
        }

        /// <summary>
        /// 判断ListView里面的所有Tag是否和list一致，通常用于数据层刷新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="view"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool ListViewItemTagEquals<T>(ListView view, ICollection<T> list) where T : class
        {
            if (view.Items.Count != list.Count)
            {
                return false;
            }
            foreach (ListViewItem item in view.Items)
            {
                T it = item.Tag as T;
                if (!list.Contains(it))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断ListView里面的所有Tag是否和list一致，通常用于数据层刷新
        /// </summary>
        /// <param name="view"></param>
        /// <param name="list"></param>
        /// <param name="compare">参数1表示list中的单位，参数2表示ListView里的Tag</param>
        /// <returns></returns>
        public static bool ListViewItemTagEquals(ListView view, IList list, IComparer compare)
        {
            if (view.Items.Count != list.Count)
            {
                return false;
            }
            foreach (ListViewItem item in view.Items)
            {
                bool finded = false;
                foreach (object tag in list)
                {
                    if (compare.Compare(tag, item.Tag) == 0)
                    {
                        finded = true;
                        break;
                    }
                }
                if (!finded)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 保持ListView所有Item和List中的Tag一致，通过Tag检测。
        /// </summary>
        /// <param name="view"></param>
        /// <param name="tags"></param>
        /// <param name="equal"></param>
        /// <param name="add"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static void ListViewItemTagRefresh(ListView view, IList tags, Func<ListViewItem, object, bool> equal, Func<object, ListViewItem> add = null, Action<ListViewItem> remove = null)
        {
            List<object> addlist = new List<object>();
            foreach (var tag in tags) { addlist.Add(tag); }

            List<ListViewItem> removelist = new List<ListViewItem>();
            foreach (ListViewItem item in view.Items)
            {
                if (item.Tag != null)
                {
                    var tag = addlist.Find((e) => { return equal(item, e); });
                    if (tag != null)
                    {
                        addlist.Remove(tag);
                    }
                    else
                    {
                        removelist.Add(item);
                    }
                }
                else
                {
                    removelist.Add(item);
                }
            }
            foreach (ListViewItem item in removelist)
            {
                view.Items.Remove(item);
                if (remove != null)
                    remove.Invoke(item);
            }
            foreach (var tag in addlist)
            {
                ListViewItem item = null;
                if (add != null)
                {
                    item = add.Invoke(tag);
                }
                else
                {
                    item = new ListViewItem(tag.ToString());
                    item.Tag = tag;
                }
                view.Items.Add(item);
            }
        }
        public static void ListViewItemTagRefresh(ListView view, IList tags, Func<object, ListViewItem> add = null, Action<ListViewItem> remove = null)
        {
            ListViewItemTagRefresh(view, tags, (item, e) => { return item.Tag == e; }, add, remove);
        }

        public static void GetAllTreeNodes(TreeNodeCollection nodes, List<TreeNode> allnodes)
        {
            foreach (TreeNode node in nodes)
            {
                allnodes.Add(node);
                GetAllTreeNodes(node.Nodes, allnodes);
            }
        }

        public static TreeNode FindTreeNodeByText(TreeNodeCollection nodes, string text, TreeNode start = null, bool include = false)
        {
            List<TreeNode> allnodes = new List<TreeNode>();
            GetAllTreeNodes(nodes, allnodes);
            int begin = allnodes.IndexOf(start);
            if (begin < 0)
            {
                begin = 0;
            }
            else if (!include)
            {
                begin += 1;
            }
            for (int i = begin; i < allnodes.Count; i++)
            {
                TreeNode tn = allnodes[i];
                if (tn is ISearchable searchable)
                {
                    if (searchable.Contains(text))
                    {
                        return tn;
                    }
                }
                else if (tn.Text.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    return tn;
                }
            }
            return null;
        }
        public static TreeNode FindLastTreeNodeByText(TreeNodeCollection nodes, string text, TreeNode start = null, bool include = false)
        {
            List<TreeNode> allnodes = new List<TreeNode>();
            GetAllTreeNodes(nodes, allnodes);
            int end = allnodes.IndexOf(start);
            if (end < 0)
            {
                end = allnodes.Count - 1;
            }
            else if (!include)
            {
                end -= 1;
            }
            for (int i = end; i >= 0; i--)
            {
                TreeNode tn = allnodes[i];
                if (tn is ISearchable searchable)
                {
                    if (searchable.Contains(text))
                    {
                        return tn;
                    }
                }
                else if (tn.Text.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    return tn;
                }
            }
            return null;
        }

        public static Task InvokeTCSAsync(this Form form, Action action)
        {
            var tcs = new TaskCompletionSource<int>();
            form.Invoke(new System.Action(() =>
            {
                try
                {
                    action();
                    tcs.TrySetResult(1);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }));
            return tcs.Task;
        }
        public static Task InvokeTCSAsync<T>(this Form form, T state, Action<T> action)
        {
            var tcs = new TaskCompletionSource<int>();
            form.Invoke(new System.Action(() =>
            {
                try
                {
                    action(state);
                    tcs.TrySetResult(1);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }));
            return tcs.Task;
        }
        public static Task<R> InvokeTCSAsync<R>(this Form form, Func<R> action)
        {
            var tcs = new TaskCompletionSource<R>();
            form.Invoke(new System.Action(() =>
            {
                try
                {
                    var result = action();
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }));
            return tcs.Task;
        }
        public static Task<R> InvokeTCSAsync<T, R>(this Form form, T state, Func<T, R> action)
        {
            var tcs = new TaskCompletionSource<R>();
            form.Invoke(new System.Action(() =>
            {
                try
                {
                    var result = action(state);
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }));
            return tcs.Task;
        }

        public static GridItem FindPropertyGridItem(List<GridItem> list, GridItem currItem, string findText, bool findNext)
        {

            findText = findText.ToLower();//不区分大小写
            int startIndex = 0;
            int invalidIndex = list.Count;
            int stepValue = 1;
            if (false == findNext)
            {
                //往前查找
                startIndex = list.Count - 1;
                invalidIndex = -1;
                stepValue = -1;
            }
            var find = currItem == null;
            for (var i = startIndex; i != invalidIndex; i += stepValue)
            {
                var item = list[i];
                if (find == false)
                {
                    if (item == currItem)
                    {
                        find = true;
                    }
                    continue;
                }
                if (item.Label.Contains(findText))
                {
                    return item;
                }
                if (item.Value != null && item.Value.ToString().Contains(findText))
                {
                    return item;
                }

                if (item.PropertyDescriptor != null)
                {
                    var str = item.PropertyDescriptor.Description;
                    if (str.Contains(findText))
                    {
                        return item;
                    }
                }
            }
            return null;
        }



        public static void GetAllSubItems(GridItem currItem, List<GridItem> list)
        {
            for (var i = 0; i < currItem.GridItems.Count; i++)
            {
                var item = currItem.GridItems[i];
                list.Add(item);
                GetAllSubItems(item, list);
            }
        }

        public static bool TryGetSelectedItem<T>(this ListView view, out T selected) where T : ListViewItem
        {
            foreach (ListViewItem item in view.SelectedItems)
            {
                if (item is T t && item.Selected)
                {
                    selected = t;
                    return true;
                }
            }
            selected = null;
            return false;
        }
        public static bool TryGetSelectedItems<T>(this ListView view, out T[] selected) where T : ListViewItem
        {
            var items = new List<T>();
            foreach (ListViewItem item in view.SelectedItems)
            {
                if (item is T t && item.Selected)
                {
                    items.Add(t);
                }
            }
            selected = items.ToArray();
            return (items.Count > 0);
        }
        public static bool TryGetCheckedItem<T>(this ListView view, out T selected) where T : ListViewItem
        {
            foreach (ListViewItem item in view.Items)
            {
                if (item is T t && item.Checked)
                {
                    selected = t;
                    return true;
                }
            }
            selected = null;
            return false;
        }
        public static bool TryGetCheckedItems<T>(this ListView view, out T[] selected) where T : ListViewItem
        {
            var items = new List<T>();
            foreach (ListViewItem item in view.Items)
            {
                if (item is T t && item.Checked)
                {
                    items.Add(t);
                }
            }
            selected = items.ToArray();
            return (items.Count > 0);
        }

        public static void MeasureColumnsWidth(this ListView view, int space = 10)
        {
            using (var gfx = view.CreateGraphics())
            {
                foreach (ColumnHeader column in view.Columns)
                {
                    column.Width = (int)gfx.MeasureString(column.Text, view.Font).Width + space;
                }
            }
        }

        public static void ShowDialog<F>(this F form, Action<Form, DialogResult> action, IWin32Window owner = null) where F : Form
        {
            form.FormClosed += (sender, e) =>
            {
                try
                {
                    action(form, form.DialogResult);
                }
                catch (Exception err)
                {
                    err.ShowMessageBox();
                }
            };
            if (form.Visible)
            {
                form.Show();
                form.BringToFront();
            }
            else
            {
                form.Show(owner);
            }
            //form.ShowDialogAsync(owner).ContinueWith();
        }
    }

}

namespace System.Windows.Forms
{
    public static class FormExt
    {
        private static Logger log = new LazyLogger("Exception");

        public static void ShowMessageBox(this Exception err, IWin32Window owner = null)
        {
            log.Error(err);
            if (owner != null)
            {
                MessageBox.Show(owner, err.Message + Environment.NewLine + err.StackTrace, err.Message);
            }
            else
            {
                MessageBox.Show(err.Message + Environment.NewLine + err.StackTrace, err.Message);
            }
        }
        public static void ShowMessageBox(this Exception err, string prefix, IWin32Window owner = null)
        {
            log.Error(prefix + err.Message, err);
            if (owner != null)
            {
                MessageBox.Show(owner, err.Message + Environment.NewLine + err.StackTrace, prefix);
            }
            else
            {
                MessageBox.Show(err.Message + Environment.NewLine + err.StackTrace, prefix);
            }
        }
        struct LockControl : IDisposable
        {
            private Control control;
            public LockControl(Control ctl)
            {
                control = ctl;
                control.Enabled = false;
            }
            public void Dispose()
            {
                control.Enabled = true;
            }
        }

        public static IDisposable Lock(this Control control)
        {
            return new LockControl(control);
        }

        public static void Lock(this Control control, Action action)
        {
            control.Enabled = false;
            try
            {
                action();
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            finally
            {
                control.Enabled = true;
            }
        }
        public static async Task LockAsync(this Control control, Func<Task> action)
        {
            control.Enabled = false;
            try
            {
                await action();
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            finally
            {
                control.Enabled = true;
            }
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(this Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(this Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }



        extension(ListView listView)
        {
            public ListViewItem SelectedItem => listView.SelectedItems.Count > 0 ? listView.SelectedItems[0] : null;
        }
    }
}