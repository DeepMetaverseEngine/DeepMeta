using DeepCore;
using DeepCore.FuncData;
using DeepCore.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DeepEditor.Common.FuncEditor
{
    public partial class FormSelectFuncID : Form
    {
        private LazyLogger log = new LazyLogger("FormSelectFuncID");
        private static FormSelectFuncID sInstance;
        private IFuncData CurrentFuncData;
        private bool ResultOK = false;
        private Action EndAction;
        private bool Opening = true;
        //----------------------------------------------------------------------------------------------
        private FormSelectFuncID()
        {
            sInstance = this;
            InitializeComponent();
            this.treeViewXLS.ImageList = FuncEditorPlugin.Instance.GetTempaltesImageList();//Editor.Instance.TempaltesImageList;
            this.treeViewXLS.NodeMouseClick += this.TreeViewXLS_NodeMouseClick;
            this.tabControlFiles.SelectedIndexChanged += this.TabPage_SelectedIndexChanged;
            InitTables();
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            RefreshStatus(null);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            EndAction?.Invoke();
            EndAction = null;
            CurrentFuncData = null;
            e.Cancel = true;
            ForEachFuncViews((view) => { view.OnClose(); });
            this.Hide();
        }
        protected bool ShowDialog<T>(Func<T> action, out T ret)
        {
            var dst = default(T);
            this.ResultOK = false;
            this.EndAction = () => { dst = action(); };
            var rst = base.ShowDialog();
            ret = dst;
            return ResultOK;
        }
        //----------------------------------------------------------------------------------------------
        private void InitTables()
        {
            try
            {
                this.tabControlFiles.TabPages.Clear();
                this.treeViewXLS.Nodes.Clear();
                this.Opening = true;
                this.SuspendLayout();
                foreach (var func in FuncDataManager.Instance.GetAllTemplates())
                {
                    GetOrAddFileTab(func, out var fileNode);
                    GetOrAddSheetTable(func, fileNode, out var sheetNode);
                    var item = sheetNode.FuncView.CreateFuncItem(func);
                    sheetNode.FuncView.FuncListView.Items.Add(item);
                }
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            finally
            {
                this.ResumeLayout();
                this.Opening = false;
            }
            void GetOrAddFileTab(FuncDataTemplate temp, out FileTreeNode fileNode)
            {
                fileNode = treeViewXLS.Nodes[temp.FilePath] as FileTreeNode;
                if (fileNode == null)
                {
                    var fileTab = new TabControl();
                    fileTab.Name = temp.FilePath;
                    fileTab.Dock = DockStyle.Fill;
                    fileTab.Alignment = TabAlignment.Bottom;
                    fileTab.SelectedIndexChanged += TabPage_SelectedIndexChanged;

                    var filePage = new TabPage(temp.FileName);
                    filePage.BorderStyle = BorderStyle.Fixed3D;
                    filePage.Dock = DockStyle.Fill;
                    filePage.Name = temp.FilePath;
                    filePage.ToolTipText = temp.FilePath;
                    filePage.Controls.Add(fileTab);
                    tabControlFiles.TabPages.Add(filePage);

                    fileNode = new FileTreeNode(temp, fileTab, filePage);
                    treeViewXLS.Nodes.Add(fileNode);
                    filePage.Tag = fileNode;
                }
            }
            void GetOrAddSheetTable(FuncDataTemplate temp, FileTreeNode fileNode, out SheetTreeNode sheetNode)
            {
                sheetNode = fileNode.Nodes[temp.SheetName] as SheetTreeNode;
                if (sheetNode == null)
                {
                    var sheetTab = new FuncTableView(temp);
                    sheetTab.Dock = DockStyle.Fill;
                    sheetTab.FuncListView.SelectedIndexChanged += ListView1_SelectedIndexChanged;
                    sheetTab.FuncListView.ItemChecked += ListView1_ItemChecked;
                    sheetTab.FuncListView.MouseDown += ListView1_MouseDown;

                    var sheetPage = new TabPage(temp.SheetName);
                    sheetPage.Text = temp.SheetName;
                    sheetPage.Name = temp.SheetName;
                    sheetPage.Controls.Add(sheetTab);
                    fileNode.FileTab.TabPages.Add(sheetPage);

                    sheetNode = new SheetTreeNode(temp, fileNode, sheetTab, sheetPage);
                    fileNode.Nodes.Add(sheetNode);
                    sheetPage.Tag = sheetNode;
                }
            }
        }
        class FileTreeNode : TreeNode
        {
            public FuncDataTemplate Temp { get; }
            public TabControl FileTab { get; }
            public TabPage FilePage { get; }
            public FileTreeNode(FuncDataTemplate temp, TabControl fileTab, TabPage filePage) : base(temp.FileName)
            {
                this.Temp = temp;
                this.Name = temp.FilePath;
                this.ToolTipText = temp.FilePath;
                this.ImageKey = this.SelectedImageKey = FuncEditorPlugin.Instance.GetFileTreeNodeImageKey(); //"icons_tool_bar2.png";
                this.FileTab = fileTab;
                this.FilePage = filePage;
            }
        }
        class SheetTreeNode : TreeNode
        {
            public FuncDataTemplate Temp { get; }
            public FileTreeNode ParentFile { get; }
            public FuncTableView FuncView { get; }
            public TabPage SheetPage { get; }
            public SheetTreeNode(FuncDataTemplate temp, FileTreeNode parent, FuncTableView funcView, TabPage sheetPage) : base(temp.SheetName)
            {
                this.Temp = temp;
                this.Name = temp.SheetName;
                this.ImageKey = this.SelectedImageKey = FuncEditorPlugin.Instance.GetSheetTreeNodeImageKey();//"icons_tool_bar1.png";
                this.ParentFile = parent;
                this.FuncView = funcView;
                this.SheetPage = sheetPage;
            }
        }
        private void TreeViewXLS_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is FileTreeNode fileNode)
            {
                tabControlFiles.SelectedTab = fileNode.FilePage;
            }
            else if (e.Node is SheetTreeNode sheetNode)
            {
                tabControlFiles.SelectedTab = sheetNode.ParentFile.FilePage;
                sheetNode.ParentFile.FileTab.SelectedTab = sheetNode.SheetPage;
            }
        }
        private void TabPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is TabControl tab)
            {
                try
                {
                    var page = tab.SelectedTab;
                    if (page.Tag is FileTreeNode fileNode)
                    {
                        treeViewXLS.SelectedNode = fileNode;
                    }
                    else if (page.Tag is SheetTreeNode sheetNode)
                    {
                        treeViewXLS.SelectedNode = sheetNode;
                    }
                }
                catch { }
            }
        }
        public void ForEachFuncViews(Action<FuncTableView> action)
        {
            foreach (var tn in treeViewXLS.GetAllNodes())
            {
                if (tn is SheetTreeNode sn)
                {
                    action(sn.FuncView);
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------------
        private void RefreshOnlyShowID()
        {
            var exists = chk_OnlyShowID.Checked ? new HashMap<string, FuncDataTemplate>() : null;
            ForEachFuncViews((sheetView) =>
            {
                sheetView.RefreshOnlyShowID(exists);
            });
        }
        private void RefreshStatus(FuncTableView.FuncListViewItem funcItem)
        {
            if (!Opening)
            {
                try
                {
                    var dtype = CurrentFuncData?.GetType();
                    var list = new List<FuncTable.FuncFields>();
                    ForEachFuncViews((sheetView) =>
                    {
                        try
                        {
                            sheetView.GetSelectFields(dtype, list);
                        }
                        catch (Exception err)
                        {
                            log.Error(err);
                        }
                    });
                    txt_Status.Text = CUtils.ListToString<FuncTable.FuncFields>(list, (a) => a.ToString(true), " | ");
                    //                     foreach (var func in list)
                    //                     {
                    //                         //dataGridView1.Rows.Add(new object[] { func.ID, func.Level });
                    //                     }
                }
                catch (Exception err)
                {
                    log.Error(err);
                }
            }
        }

        public static void ShowTable()
        {
            try
            {
                var dialog = sInstance != null ? sInstance : new FormSelectFuncID();
                dialog.chk_OnlyShowID.Visible = false;
                dialog.chk_OnlyShowID.Checked = false;
                dialog.Show();
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
        }
        public static bool ShowDialog(IFuncData src, out FuncTable dst)
        {
            try
            {
                var dialog = sInstance != null ? sInstance : new FormSelectFuncID();
                dialog.chk_OnlyShowID.Visible = true;
                dialog.chk_OnlyShowID.Checked = true;
                dialog.SetSelectedTable(src);
                if (dialog.ShowDialog(() => dialog.GetSelectedTable(src), out dst))
                {
                    return true;
                }
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            dst = null;
            return false;
        }
        public static bool ShowOwnerDialog(HashMap<string, int> src, out HashMap<string, int> dst)
        {
            try
            {
                var dialog = sInstance != null ? sInstance : new FormSelectFuncID();
                dialog.chk_OnlyShowID.Visible = false;
                dialog.chk_OnlyShowID.Checked = false;
                dialog.SetOwnerFunc(src);
                if (dialog.ShowDialog(() => dialog.GetSelectedOwnerTable(), out dst))
                {
                    return true;
                }
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            dst = null;
            return false;
        }
        public static bool ShowFillDialog(IList list, Type listType, Type elementType, out IList olist)
        {
            try
            {
                var dialog = sInstance != null ? sInstance : new FormSelectFuncID();
                dialog.chk_OnlyShowID.Visible = false;
                dialog.chk_OnlyShowID.Checked = false;
                dialog.SetFillFunc(list, elementType);
                if (dialog.ShowDialog(() => dialog.GetSelectedFillTable(elementType), out var funcs))
                {
                    olist = (IList)Activator.CreateInstance(listType);
                    if (funcs != null)
                    {
                        foreach (var fd in funcs)
                        {
                            var element = Activator.CreateInstance(elementType) as IFuncData;
                            element.FuncID = new FuncTable(fd);
                            FuncDataManager.Instance.FillFromFuncID(element);
                            olist.Add(element);
                        }
                    }
                    return true;
                }
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            olist = null;
            return false;
        }
        public static bool ShowFillDialog(Type srcType, ref object value)
        {
            try
            {
                var dialog = sInstance != null ? sInstance : new FormSelectFuncID();
                dialog.chk_OnlyShowID.Visible = true;
                dialog.chk_OnlyShowID.Checked = true;
                dialog.SetFillFunc(value);
                if (value != null) { srcType = value.GetType(); }
                if (dialog.ShowDialog(() => dialog.GetSelectedFillTable(srcType), out var funcs))
                {
                    value = Activator.CreateInstance(srcType);
                    if (funcs != null)
                    {
                        FuncDataManager.Instance.FillFromFuncID(value);
                    }
                    return true;
                }
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            return false;
        }

        //----------------------------------------------------------------------------------------------

        private void SetSelectedTable(IFuncData funcData)
        {
            try
            {
                this.Opening = true;
                this.CurrentFuncData = funcData;
                ForEachFuncViews((sheetView) =>
                {
                    sheetView.SetSelectFieldIndex(funcData);
                });
            }
            finally
            {
                this.Opening = false;
            }
        }
        private void SetOwnerFunc(HashMap<string, int> src)
        {
            try
            {
                this.Opening = true;
                ForEachFuncViews((sheetView) =>
                {
                    sheetView.SetSelectOwnerFuncs(src);
                });
            }
            finally
            {
                this.Opening = false;
            }
        }
        private void SetFillFunc(IList list, Type elementType)
        {
            try
            {
                this.Opening = true;
                ForEachFuncViews((sheetView) =>
                {
                    sheetView.SetSelectFillFuncs(list, elementType);
                });
            }
            finally
            {
                this.Opening = false;
            }
        }
        private void SetFillFunc(object src)
        {
            try
            {
                this.Opening = true;
                ForEachFuncViews((sheetView) =>
                {
                    sheetView.SetSelectFillFuncs(src);
                });
            }
            finally
            {
                this.Opening = false;
            }
        }
        private FuncTable GetSelectedTable(IFuncData funcData)
        {
            var ret = new SortedDictionary<string, FuncTable.FuncFields>();
            ForEachFuncViews((sheetView) =>
            {
                sheetView.GetSelectFields(funcData, ret);
            });
            if (ret.Count > 0)
            {
                return new FuncTable()
                {
                    FuncID = ret.Values.ToArray()
                };
            }
            else
            {
                return null;
            }
        }
        private HashMap<string, int> GetSelectedOwnerTable()
        {
            var ret = new HashMap<string, int>();
            ForEachFuncViews((sheetView) =>
            {
                sheetView.GetSelectOwnerFuncs(ret);
            });
            if (ret.Count > 0)
            {
                return ret;
            }
            else
            {
                return null;
            }
        }
        private List<FuncTable.FuncFields> GetSelectedFillTable(Type fillType)
        {
            var ret = new List<FuncTable.FuncFields>();
            ForEachFuncViews((sheetView) =>
            {
                sheetView.GetSelectFields(fillType, ret);
            });
            if (ret.Count > 0)
            {
                return ret;
            }
            else
            {
                return null;
            }
        }

        //------------------------------------------------------------------------------------------------------------------

        private void ListView1_MouseDown(object sender, MouseEventArgs e)
        {
            //RefreshStatus();
        }
        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RefreshStatus();
        }
        private void ListView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item is FuncTableView.FuncListViewItem funcItem)
            {
                RefreshStatus(funcItem);
            }
        }

        //------------------------------------------------------------------------------------------------------------------



        private void Btn_OK_Click(object sender, EventArgs e)
        {
            this.ResultOK = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            this.tabControlFiles.TabPages.Clear();
            this.treeViewXLS.Nodes.Clear();
            FuncEditorPlugin.Instance.BuildFuncDatas();
            //             EditorPlugin.BuildFunc();
            FuncDataManager.Instance.RefreshFromEditor(FuncEditorPlugin.Instance.GetEditorTemplatesData());
            InitTables();
            RefreshOnlyShowID();
        }

        private void Chk_OnlyShowID_CheckedChanged(object sender, EventArgs e)
        {
            RefreshOnlyShowID();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
