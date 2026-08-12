using DeepCore;
using DeepCore.Components;
using DeepCore.FuncData;
using DeepCore.Reflection;
using DeepCore.Xml;
using DeepEditor.Common.Properties;
using DeepEditor.Common.Windows;
using MaterialSkin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using static DeepEditor.Common.G2D.DataGrid.G2DTypeDescriptor;

namespace DeepEditor.Common.G2D.DataGrid
{
    public class G2DPropertyGrid : System.Windows.Forms.PropertyGrid, IG2DBaseComponent
    {
        #region Fields
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }

        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        private Control propertyGridView = null;
        private ToolStrip tools;
        private object selectRootObject = null;

        private int descriptionAreaLineCount = 4;
        private int descriptionAreaLineCountMin = 5;
        private Control docComment = null;
        private Type docCommentType = null;
        private PropertyInfo linesProperty;

        private bool sizeChangeIsFromUser = true;
        private bool inited = false;
        private Type last_field_decleard_type = null;

        private G2DBaseToolStripButton btn_collapse_all;
        private G2DBaseToolStripButton btn_expand_all;
        private G2DBaseToolStripButton btn_copy;
        private G2DBaseToolStripButton btn_paste;
        private G2DBaseToolStripDropDownButton btn_paste_plus;
        private G2DBaseToolStripButton btn_cut;
        private G2DBaseToolStripButton btn_levels;
        private G2DBaseToolStripButton btn_delete;
        private G2DBaseToolStripButton btn_display;
        private G2DBaseToolStripButton btn_search;
        private G2DBaseContextMenuStrip docMenuStrip;
        private G2DBaseToolStripMenuItem docMenuCopyDesc;
        private G2DBaseToolStripMenuItem docMenuCopyFuncField;
        private G2DBaseToolStripMenuItem docMenuCopyFuncField2;
        private G2DBaseToolStripLabel lbl_copying;

        //搜索对话框
        private G2DSearchDialog mSearchDialog;
        //上一次搜索词
        private string mLastSearchText = "";
        //当前搜索到的行
        private GridItem mCurrSearchItem = null;
        //顶层属性
        private GridItem mTopItem = null;
        private List<GridItem> mAllItemsList = new List<GridItem>();
        private object mLastSearchObject = null;
        #endregion

        /// <summary>
        /// Initializes a new instance of the CustomPropertyGrid class.
        /// </summary>
        public G2DPropertyGrid()
        {
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ImeMode = ImeMode.NoControl;
            this.MinDescriptionAreaLineCount = 5;
            foreach (Control control in this.Controls)
            {
                Type controlType = control.GetType();
                if (controlType.Name == "HelpPane")
                {
                    this.docCommentType = controlType;
                    this.docComment = control;
                    this.linesProperty = this.docCommentType.GetProperty("Lines");
                    var userSizedField = this.docCommentType.GetField(
                        "UserSized",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    userSizedField?.SetValue(this.docComment, true);
                    this.docComment.SizeChanged += this.HandleDocCommentSizeChanged;
                }
                else if (controlType.Name.EndsWith("PropertyGridView"))
                {
                    this.propertyGridView = control;
                }
                else if (controlType.Name.EndsWith("ToolStrip"))
                {
                    this.tools = control as ToolStrip;
                }
            }
            this.InitializeDesc();
            if (tools != null)
            {
                InitTools();
            }
            try
            {
                this.Controls[1].Height -= 30;
                this.Controls[3].Top -= 30;
                this.Controls[3].Height += 30;
                this.Controls[3].Font = new Font(this.Controls[3].Font, FontStyle.Bold);
            }
            catch { }
            this.SelectedGridItemChanged += G2DPropertyGrid_SelectedGridItemChanged;
            this.InitMenu();
            //this.propertyGridView.MouseDown += PropertyGridView_MouseDown;
        }

        public List<GridItem> TopLevelGridEntries
        {
            get
            {
                var ret = new List<GridItem>();
                try
                {
                    var func = propertyGridView.GetType().GetMethod("GetAllGridEntries",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    //                     var prop = propertyGridView.GetType().GetProperty("TopLevelGridEntries",
                    //                         BindingFlags.Instance | BindingFlags.NonPublic);
                    //                    var grids = (IEnumerable)prop.GetValue(this.propertyGridView);
                    var grids = (IEnumerable)func.Invoke(this.propertyGridView, new object[] { true });
                    if (grids != null)
                    {
                        foreach (var grid in grids)
                        {
                            ret.Add((GridItem)grid);
                        }
                    }
                }
                catch { }
                return ret;
            }
        }
        public G2DTypeDescriptor SelectedDescriptorObject
        {
            get { return base.SelectedObject as G2DTypeDescriptor; }
        }
        public object SelectedDataObject
        {
            get
            {
                if (base.SelectedObject is G2DTypeDescriptor)
                {
                    return (base.SelectedObject as G2DTypeDescriptor).EditData;
                }
                return base.SelectedObject;
            }
        }
        public object SelectedRootObject
        {
            get
            {
                if (selectRootObject == null) return SelectedDataObject;
                return selectRootObject;
            }
            set => selectRootObject = value;
        }
        public MemberInfo SelectedField
        {
            get
            {
                var grid = this.SelectedGridItem;
                if (grid != null && grid.PropertyDescriptor is G2DOwnerPropertyDescriptor fieldDesc)
                {
                    return fieldDesc.FieldMember as MemberInfo;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        var g = this.FindGridItem(grid =>
                        {
                            if (grid.PropertyDescriptor is G2DOwnerPropertyDescriptor fieldDesc)
                            {
                                if (object.Equals(fieldDesc.FieldMember, value))
                                {
                                    return true;
                                }
                            }
                            return false;
                        });
                        this.ExpandTo(g);
                    }
                    catch
                    {

                    }
                }
            }
        }
        public G2DFieldElementDesc SelectedElementDesc
        {
            get
            {
                var grid = this.SelectedGridItem;
                if (grid != null && grid.PropertyDescriptor is G2DOwnerPropertyDescriptor fieldDesc)
                {
                    return fieldDesc.ToFieldDesc(this.SelectedRootObject, grid);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        var g = this.FindGridItem(grid =>
                        {
                            if (grid.PropertyDescriptor is G2DOwnerPropertyDescriptor fieldDesc)
                            {
                                var f = fieldDesc.ToFieldDesc(this.SelectedRootObject, grid);
                                if (f.FieldDecleardType == value.FieldDecleardType
                                && f.ComponentData.GetType() == value.ComponentData.GetType()
                                && f.FieldMember == value.FieldMember
                                && f.FieldName == value.FieldName)
                                {
                                    //fieldDesc.SetValue(grid.Parent.Value, value.FieldValue);
                                    return true;
                                }
                            }
                            return false;
                        });
                        this.ExpandTo(g);
                    }
                    catch
                    {

                    }
                }
            }
        }
        public FieldOwnerValue SelectedFieldDesc
        {
            get
            {
                var grid = this.SelectedGridItem;
                if (grid != null && grid.PropertyDescriptor is G2DOwnerPropertyDescriptor fieldDesc && fieldDesc.FieldMember is FieldInfo member)
                {
                    return new FieldOwnerValue(SelectedRootObject, member, fieldDesc.FieldValue, fieldDesc.ComponentData);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        var g = this.FindGridItem(grid =>
                        {
                            if (grid.PropertyDescriptor is G2DOwnerPropertyDescriptor fieldDesc)
                            {
                                if (Object.Equals(fieldDesc.FieldMember, value.Field))
                                {
                                    if (Object.Equals(fieldDesc.ComponentData, value.FieldOwner))
                                    {
                                        if (value.FieldValue == null || Object.Equals(fieldDesc.FieldValue, value.FieldValue))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                            return false;
                        });
                        this.ExpandTo(g);
                    }
                    catch
                    {

                    }
                }
            }
        }
        public G2DFieldDescValue SelectedFieldValue
        {
            get
            {
                var grid = this.SelectedGridItem;
                if (grid.PropertyDescriptor is G2DOwnerPropertyDescriptor md)
                {
                    return md.ToFieldDesc();
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        var g = this.FindGridItem(grid =>
                        {
                            if (grid.PropertyDescriptor is G2DOwnerPropertyDescriptor fieldDesc)
                            {
                                if (Object.Equals(fieldDesc.FieldMember, value.FieldMember))
                                {
                                    if (Object.Equals(fieldDesc.ComponentData, value.ComponentData))
                                    {
                                        if (Object.Equals(grid.Value, value.FieldValue))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                            return false;
                        });
                        this.ExpandTo(g);
                    }
                    catch
                    {

                    }
                }
            }
        }

        public void ExpandTo(GridItem grid)
        {
            if (grid != null)
            {
                //this.SelectedGridItem = grid;
                var stak = new List<GridItem>();
                while (grid != null)
                {
                    stak.Add(grid);
                    grid = grid.Parent;
                }
                for (int i = stak.Count - 1; i >= 0; --i)
                {
                    stak[i].Expanded = true;
                    stak[i].Select();
                }
            }
        }
        public List<GridItem> GetAllGridItems()
        {
            var ret = new List<GridItem>();
            GetAllGridItems(ret);
            return ret;
        }
        public void GetAllGridItems(List<GridItem> list)
        {
            var tops = TopLevelGridEntries;
            if (tops != null)
            {
                foreach (GridItem grid in tops)
                {

                    list.Add(grid);
                    FormUtils.GetAllSubItems(grid, list);
                }
            }
            else
            {
                var topItem = this.SelectedGridItem;
                while (topItem != null && topItem.Parent != null)
                {
                    topItem = topItem.Parent;
                }
                list.Add(topItem);
                FormUtils.GetAllSubItems(topItem, list);
            }
        }
        public GridItem FindGridItem(Predicate<GridItem> find)
        {
            var all = GetAllGridItems();
            foreach (var e in all)
            {
                if (find(e)) return e;
            }
            return null;
        }

        public G2DTypeDescriptor SetSelectedObject(object value, params IG2DPropertyAdapter[] adds)
        {
            if (value is G2DTypeDescriptor desc)
            {
                desc.AddPropertyAdapter(adds);
                base.SelectedObject = desc;
                return desc;
            }
            else if (value != null)
            {
                var ret = G2DTypeDescriptor.CreateDescriptor(value, adds);
                base.SelectedObject = ret;
                return ret;
            }
            else
            {
                base.SelectedObject = null;
                return null;
            }
        }
        public object GetSelectedValue()
        {
            if (base.SelectedObject is G2DTypeDescriptor)
            {
                return (base.SelectedObject as G2DTypeDescriptor).EditData;
            }
            return base.SelectedObject;
        }
        public T GetSelectedValueAs<T>()
        {
            return (T)GetSelectedValue();
        }
        public void AppendCurrentToHistory()
        {
            var obj = GetSelectedValue();
            var desc = SelectedDescriptorObject;
            if (obj != null && desc != null)
            {
                desc.AppendOptionalsFromHistoryObject(obj);
            }
        }
        public void SetLevels() { }
        //         private void PropertyGridView_MouseDown(object sender, MouseEventArgs e)
        //         {
        //             if (e.Button == MouseButtons.Right)
        //             {
        //                 var grid = this.SelectedGridItem;
        //                 if (grid != null)
        //                 {
        //                     this.gridMenuStrip.Show(propertyGridView, e.Location);
        //                 }
        //             }
        //         }
        protected override void OnSelectedObjectsChanged(EventArgs e)
        {
            if (!inited)
            {
                inited = true;
                this.DescriptionAreaLineCount = 5;
                this.DescriptionAreaHeight = 120;
                this.DescriptionAreaLineCount = Math.Max(5, this.DescriptionAreaLineCount);
            }
            base.OnSelectedObjectsChanged(e);
            if (SelectedDescriptorObject is G2DTypeDescriptor g2d)
            {
                g2d.OnCommit += G2d_OnCommit;
                g2d.OnSetValue += G2d_SetValue;
            }
        }
        protected override void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
        {
            base.OnPropertyValueChanged(e);
            this.Refresh();
        }
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);

        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
        }

        //         protected override void OnMouseDown(MouseEventArgs me)
        //         {
        //             base.OnMouseDown(me);
        //         }
        public event OnCommitHandler OnCommit;
        public delegate void OnCommitHandler(object sender, CommitEventArgs args);
        public event OnSetValueHandler OnSetValue;
        public delegate void OnSetValueHandler(object sender, CommitEventArgs args);
        private void G2d_OnCommit(G2DPropertyDescriptor prop, object component, object value)
        {
            OnCommit?.Invoke(this, new CommitEventArgs()
            {
                TypeDescriptor = this.SelectedDescriptorObject,
                PropertyDescriptor = prop,
                Component = component,
                Value = value,
            });
        }
        private void G2d_SetValue(G2DPropertyDescriptor prop, object component, object value)
        {
            OnSetValue?.Invoke(this, new CommitEventArgs()
            {
                TypeDescriptor = this.SelectedDescriptorObject,
                PropertyDescriptor = prop,
                Component = component,
                Value = value,
            });
        }
        public class CommitEventArgs : EventArgs
        {
            public G2DTypeDescriptor TypeDescriptor { get; internal set; }
            public G2DPropertyDescriptor PropertyDescriptor { get; internal set; }
            public object Component { get; internal set; }
            public object Value { get; internal set; }
        }
        private void G2DPropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            this.OnRefreshHistoryValiable(SelectedGridItem);
        }
        //------------------------------------------------------------------------------------------------------
        #region _key_events_
        const int WM_MOUSEWHEEL = (0x020A);//: 垂直滚轮滚动。
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEWHEEL)
            {
                // it can not be handle this msg
            }
            base.WndProc(ref m);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys kd)
        {
            if (Keyboard.IsCtrlDown)
            {
                Keys keyData = kd ^ Keys.Control;
                switch (keyData)
                {
                    case Keys.C:
                        if (ProcessKeyDown_CtrlC != null)
                        {
                            ProcessKeyDown_CtrlC.Invoke(this, new KeyEventArgs(keyData));
                        }
                        DoCopy();
                        return true;
                    case Keys.V:
                        if (ProcessKeyDown_CtrlV != null)
                        {
                            ProcessKeyDown_CtrlV.Invoke(this, new KeyEventArgs(keyData));
                        }
                        DoPaste();
                        return true;
                    case Keys.X:
                        if (ProcessKeyDown_CtrlX != null)
                        {
                            ProcessKeyDown_CtrlX.Invoke(this, new KeyEventArgs(keyData));
                        }
                        DoCut();
                        return true;
                }
            }
            switch (kd)
            {
                case Keys.Delete:
                    if (ProcessKeyDown_Delete != null)
                    {
                        ProcessKeyDown_Delete.Invoke(this, new KeyEventArgs(kd));
                    }
                    DoDelete();
                    return true;
                case Keys.F3:       //查找
                    //正向查找
                    if (SelectedGridItem != null)
                    {
                        mCurrSearchItem = this.SelectedGridItem;
                    }
                    findNextItem(mLastSearchText);
                    return true;
                case Keys.F3 | Keys.Shift:
                    //反向查找
                    if (SelectedGridItem != null)
                    {
                        mCurrSearchItem = this.SelectedGridItem;
                    }
                    findPrevItem(mLastSearchText);
                    return true;
                case Keys.F | Keys.Control:
                    showSearch();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, kd);
        }

        public delegate void KeyDownProcessHandler(object sender, KeyEventArgs e);
        /// <summary>
        /// 键盘Ctrl+C触发
        /// </summary>
        public event KeyDownProcessHandler ProcessKeyDown_CtrlC;
        /// <summary>
        /// 键盘Ctrl+V触发
        /// </summary>
        public event KeyDownProcessHandler ProcessKeyDown_CtrlV;
        /// <summary>
        /// 键盘Ctrl+X触发
        /// </summary>
        public event KeyDownProcessHandler ProcessKeyDown_CtrlX;
        /// <summary>
        /// 键盘Del触发
        /// </summary>
        public event KeyDownProcessHandler ProcessKeyDown_Delete;

        #endregion
        //------------------------------------------------------------------------------------------------------
        #region _grid_menu_
        public delegate bool TrySetFuncIDAction(G2DPropertyGrid prop, G2DFieldElementDesc grid, out object newValue);
        public static event TrySetFuncIDAction SetFuncIDCall;

        public delegate bool TryClearFuncIDAction(G2DPropertyGrid prop, G2DFieldElementDesc grid, out object newValue);
        public static event TryClearFuncIDAction ClearFuncIDCall;

        public delegate bool TryAcceptFieldMenuStrip(G2DPropertyGrid prop, G2DFieldElementDesc grid);
        public event TryAcceptFieldMenuStrip AcceptMenuStripOpening;

        public G2DBaseContextMenuStrip GridMenuStrip { get { return gridMenuStrip; } }
        public G2DFieldElementDesc PopupMenuFieldDesc => gridMenuStrip.Tag as G2DFieldElementDesc;

        private G2DBaseContextMenuStrip gridMenuStrip;
        private G2DBaseToolStripMenuItem gridMenuHead;
        private G2DBaseToolStripMenuItem gridMenuSetFuncID;
        private G2DBaseToolStripMenuItem gridMenuClearFuncID;
        private G2DBaseToolStripMenuItem gridAddFavorite;
        private void InitMenu()
        {
            try
            {
                gridMenuStrip = new G2DBaseContextMenuStrip();
                gridMenuStrip.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
                gridMenuStrip.CustomBackColor = null;
                gridMenuStrip.CustomForeColor = null;
                gridMenuStrip.Depth = 0;
                gridMenuStrip.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
                gridMenuStrip.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                gridMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
                {
                    {
                        gridAddFavorite = new G2DBaseToolStripMenuItem()
                        {
                            Text = "添加到搜藏夹",
                            ImageOrigin = Resources.bookmark
                        };
                        gridAddFavorite.Click += GridAddFavorite_Click;
                    }
                    {
                        gridMenuHead = new G2DBaseToolStripMenuItem() { Text = "Commet" };
                        gridMenuHead.Enabled = false;
                    }
                    {
                        gridMenuSetFuncID = new G2DBaseToolStripMenuItem()
                        {
                            Text = "字段绑定词缀",
                            ImageOrigin = Resources.todo_list
                        };
                        gridMenuSetFuncID.Click += GridMenuSetFuncID_Click;
                    }
                    {
                        gridMenuClearFuncID = new G2DBaseToolStripMenuItem()
                        {
                            Text = "字段移除词缀",
                            ImageOrigin = Resources.trash
                        };
                        gridMenuClearFuncID.Click += GridMenuClearFuncID_Click;
                    }
                    gridMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { gridAddFavorite, gridMenuHead, gridMenuSetFuncID, gridMenuClearFuncID });
                }
                gridMenuStrip.MouseState = MaterialSkin.MouseState.HOVER;
                gridMenuStrip.Name = "gridMenu";
                gridMenuStrip.Size = new System.Drawing.Size(249, 229);
                gridMenuStrip.Opening += GridMenuStrip_Opening;
                this.ContextMenuStrip = gridMenuStrip;
            }
            catch { }
        }
        private void GridMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var cancel = false;
            if (SelectedGridItem != null && SelectedElementDesc is G2DFieldElementDesc desc)
            {
                gridMenuHead.Text = $"字段: {desc.FieldName}";
                gridAddFavorite.Tag = desc;
                gridMenuStrip.Tag = desc;
                if (desc.ComponentData is IFuncData fdata && desc.FieldDecleardType.IsPrimitiveData())
                {
                    gridMenuSetFuncID.Visible = true;
                    gridMenuSetFuncID.Tag = desc;
                    gridMenuClearFuncID.Visible = true;
                    gridMenuClearFuncID.Tag = desc;
                }
                else
                {
                    gridMenuSetFuncID.Visible = false;
                    gridMenuClearFuncID.Visible = false;
                }
            }
            else
            {
                cancel = true;
            }
            if (AcceptMenuStripOpening != null && AcceptMenuStripOpening.Invoke(this, SelectedElementDesc))
            {
                cancel = false;
            }
            e.Cancel = cancel;
        }
        private void GridMenuSetFuncID_Click(object sender, EventArgs e)
        {
            if (gridMenuSetFuncID.Tag is G2DFieldElementDesc desc)
            {
                var oldvalue = desc.FieldValue;
                if (SetFuncIDCall != null && SetFuncIDCall.Invoke(this, desc, out var newValue))
                {
                    try
                    {
                        desc.Cell.PropertyDescriptor.SetValue(desc.ComponentData, newValue);
                    }
                    catch { }
                    this.OnPropertyValueChanged(new PropertyValueChangedEventArgs(desc.Cell, oldvalue));
                    //this.Invalidate();
                }
            }
        }
        private void GridMenuClearFuncID_Click(object sender, EventArgs e)
        {
            if (gridMenuClearFuncID.Tag is G2DFieldElementDesc desc)
            {
                var oldvalue = desc.FieldValue;
                if (ClearFuncIDCall != null && ClearFuncIDCall.Invoke(this, desc, out var newValue))
                {
                    try
                    {
                        desc.Cell.PropertyDescriptor.SetValue(desc.ComponentData, newValue);
                    }
                    catch { }
                    this.OnPropertyValueChanged(new PropertyValueChangedEventArgs(desc.Cell, oldvalue));
                    //this.Invalidate();
                }
            }
        }
        private void GridAddFavorite_Click(object sender, EventArgs e)
        {
            if (gridAddFavorite.Tag is G2DFieldElementDesc desc)
            {
                GridFavoriteManager.Instance.AddFavorite(desc);
            }
        }

        #endregion
        //------------------------------------------------------------------------------------------------------
        #region _tools_
        private void InitTools()
        {
            tools.CanOverflow = false;
            tools.LayoutStyle = ToolStripLayoutStyle.Flow;
            //             try
            //             {
            //                 FieldInfo btnViewPropertyPagesField = base.GetType().GetField("btnViewPropertyPages", BindingFlags.Instance);
            //                 ToolStripButton btnViewPropertyPages = btnViewPropertyPagesField.GetValue(this) as ToolStripButton;
            //                 btnViewPropertyPages.Visible = false;
            //             }
            //             catch (Exception err) { }         
            tools.Items.Add(btn_collapse_all = new G2DBaseToolStripButton()
            {
                Text = "Collapse All",
                Image = global::DeepEditor.Common.Properties.Resources.compress,
                ImageOrigin = global::DeepEditor.Common.Properties.Resources.compress,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
            });
            tools.Items.Add(btn_expand_all = new G2DBaseToolStripButton()
            {
                Text = "Expand All",
                Image = global::DeepEditor.Common.Properties.Resources.resize_diagonal,
                ImageOrigin = global::DeepEditor.Common.Properties.Resources.resize_diagonal,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
            });
            tools.Items.Add(btn_display = new G2DBaseToolStripButton()
            {
                Text = "显示",
                Image = global::DeepEditor.Common.Properties.Resources.translation,
                ImageOrigin = global::DeepEditor.Common.Properties.Resources.translation,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
            });
            tools.Items.Add(btn_search = new G2DBaseToolStripButton()
            {
                Text = "搜索",
                Image = global::DeepEditor.Common.Properties.Resources.search,
                ImageOrigin = global::DeepEditor.Common.Properties.Resources.search,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
            });
            tools.Items.Add(new ToolStripSeparator());
            tools.Items.Add(btn_levels = new G2DBaseToolStripButton()
            {
                Text = "设置等级",
                Image = global::DeepEditor.Common.Properties.Resources.grid_3,
                ImageOrigin = global::DeepEditor.Common.Properties.Resources.grid_3,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
            });
            tools.Items.Add(btn_copy = new G2DBaseToolStripButton()
            {
                Text = "复制",
                Image = global::DeepEditor.Common.Properties.Resources.copy,
                ImageOrigin = global::DeepEditor.Common.Properties.Resources.copy,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
            });
            tools.Items.Add(btn_paste = new G2DBaseToolStripButton()
            {
                Text = "粘贴",
                Image = global::DeepEditor.Common.Properties.Resources.paste,
                ImageOrigin = global::DeepEditor.Common.Properties.Resources.paste,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
            });
            tools.Items.Add(btn_cut = new G2DBaseToolStripButton()
            {
                Text = "剪切",
                Image = global::DeepEditor.Common.Properties.Resources.cut,
                ImageOrigin = global::DeepEditor.Common.Properties.Resources.cut,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
            });
            tools.Items.Add(btn_delete = new G2DBaseToolStripButton()
            {
                Text = "删除",
                Image = global::DeepEditor.Common.Properties.Resources.eraser,
                ImageOrigin = global::DeepEditor.Common.Properties.Resources.eraser,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
            });
            tools.Items.Add(new ToolStripSeparator());
            tools.Items.Add(btn_paste_plus = new G2DBaseToolStripDropDownButton()
            {
                Text = "",
                Image = global::DeepEditor.Common.Properties.Resources.import,
                ImageOrigin = global::DeepEditor.Common.Properties.Resources.import
            });
            tools.Items.Add(lbl_copying = new G2DBaseToolStripLabel() { Text = "" });

            btn_paste_plus.ToolTipText = "粘贴自";
            btn_paste_plus.ForeColor = System.Drawing.Color.Blue;
            btn_paste_plus.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            btn_paste_plus.DropDownItemClicked += Btn_paste_plus_DropDownItemClicked;
            btn_paste_plus.DropDownOpening += Btn_paste_plus_DropDownOpening;
            btn_paste_plus.AutoToolTip = true;
            btn_paste_plus.AutoSize = true;

            lbl_copying.ForeColor = System.Drawing.Color.Blue;
            lbl_copying.Overflow = ToolStripItemOverflow.Never;
            lbl_copying.AutoToolTip = true;

            btn_delete.Visible = false;

            btn_collapse_all.Click += Btn_collapse_all_Click;
            btn_expand_all.Click += Btn_expand_all_Click;
            btn_levels.Click += Btn_levels_Click;
            btn_cut.Click += Btn_cut_Click;
            btn_copy.Click += Btn_copy_Click;
            btn_paste.Click += Btn_paste_Click;
            btn_delete.Click += Btn_delete_Click;
            btn_display.Click += Btn_display_Click;
            btn_search.Click += Btn_search_Click;

            OnRefreshDisplay();
        }

        private void Btn_expand_all_Click(object sender, EventArgs e)
        {
            this.ExpandAllGridItems();
        }

        private void Btn_collapse_all_Click(object sender, EventArgs e)
        {
            this.CollapseAllGridItems();
        }

        private void RefreshToolTips(List<CopyHistory.Entry> list)
        {
            if (list != null && list.Count > 0)
            {
                CopyHistory.Entry current = list[0];
                this.btn_paste_plus.Text = list.Count.ToString();
                this.btn_paste_plus.ToolTipText = "粘贴自: " + current.lbl_text;
                this.lbl_copying.Text = current.lst_text;
                this.lbl_copying.ToolTipText = current.lbl_text;

                this.btn_paste_plus.Visible = true;
                this.lbl_copying.Visible = true;
            }
            else
            {
                this.btn_paste_plus.Visible = false;
                this.lbl_copying.Visible = false;
            }
        }

        /// <summary>
        /// 刷新粘贴可用项目
        /// </summary>
        /// <param name="item"></param>
        private void OnRefreshHistoryValiable(GridItem item)
        {
            if (item != null && item.PropertyDescriptor is G2DOwnerPropertyDescriptor)
            {
                Type decleard_type = (item.PropertyDescriptor as G2DOwnerPropertyDescriptor).DecleardFieldType;
                if (!decleard_type.Equals(this.last_field_decleard_type))
                {
                    this.last_field_decleard_type = decleard_type;
                    if (this.btn_paste_plus != null)
                    {
                        this.btn_paste_plus.DropDownItems.Clear();
                        List<CopyHistory.Entry> list = CopyHistory.GetHistoryList(decleard_type);
                        if (list != null && list.Count > 0)
                        {
                            foreach (var h in list)
                            {
                                var add = new G2DBaseToolStripButton();
                                add.Text = h.lst_text;
                                add.AutoSize = true;
                                add.AutoToolTip = true;
                                add.ToolTipText = h.lbl_text;
                                add.DisplayStyle = ToolStripItemDisplayStyle.Text;
                                add.ForeColor = (add.Enabled) ? SkinManager.TextMediumEmphasisColor : System.Drawing.Color.Gray;
                                add.Tag = h;
                                btn_paste_plus.DropDownItems.Add(add);
                            }
                            RefreshToolTips(list);
                            return;
                        }
                        else
                        {
                            RefreshToolTips(null);
                        }
                    }
                }
            }
            else
            {
                this.last_field_decleard_type = null;
                RefreshToolTips(null);
            }
        }

        private void OnRefreshHistoryAdded(GridItem item, bool new_item, List<CopyHistory.Entry> list)
        {
            if (list != null && list.Count > 0)
            {
                if (new_item)
                {
                    CopyHistory.Entry current = list[0];
                    var add = new G2DBaseToolStripButton();
                    add.Text = current.lst_text;
                    add.AutoSize = true;
                    add.AutoToolTip = true;
                    add.ToolTipText = current.lbl_text;
                    add.DisplayStyle = ToolStripItemDisplayStyle.Text;
                    add.ForeColor = (add.Enabled) ? SkinManager.TextMediumEmphasisColor : System.Drawing.Color.Gray;
                    add.Tag = current;
                    btn_paste_plus.DropDownItems.Insert(0, add);
                }
                RefreshToolTips(list);
            }
            else
            {
                RefreshToolTips(null);
            }
        }

        private void OnRefreshDisplay()
        {
            switch (G2DTypeDescriptor.SHOW_DISPLAY_NAME)
            {
                case G2DPropertyFieldDisplayStyle.FieldName:
                    btn_display.ToolTipText = btn_display.Text = "字段名";
                    break;
                case G2DPropertyFieldDisplayStyle.FieldName_DescName:
                    btn_display.ToolTipText = btn_display.Text = "字段名-注释名";
                    break;
                case G2DPropertyFieldDisplayStyle.DescName:
                    btn_display.ToolTipText = btn_display.Text = "注释名";
                    break;
            }
        }



        private void showSearch()
        {
            if (SelectedObject == null)
            {
                return;
            }

            //打开搜索框时强制重新获取item列表
            mLastSearchObject = null;

            if (mSearchDialog == null)
            {

                mSearchDialog = new G2DSearchDialog();
                mSearchDialog.FindPrevClicked += findPrevItem;
                mSearchDialog.FindNextClicked += findNextItem;
                mSearchDialog.FindClicked += findItem;
                mSearchDialog.CloseClicked += onSearchClosed;

                mCurrSearchItem = null;
            }

            string title = "搜索【" + SelectedObject.ToString() + "】的属性";
            mSearchDialog.SetTitle(title);
            mSearchDialog.VisibleChanged += new EventHandler((s, e) =>
            {
                if (mSearchDialog != null && !mSearchDialog.IsDisposed && mSearchDialog.Visible)
                {
                    var pos = this.RectangleToScreen(new System.Drawing.Rectangle(0, 0, Width, Height));
                    mSearchDialog.SetDesktopLocation(Math.Max(pos.X - mSearchDialog.Width, 0), pos.Y);
                }
            });
            mSearchDialog.ShowDialog();

        }

        private void Btn_levels_Click(object sender, EventArgs e)
        {
            SetLevels();
        }

        private void Btn_display_Click(object sender, EventArgs e)
        {
            switch (G2DTypeDescriptor.SHOW_DISPLAY_NAME)
            {
                case G2DPropertyFieldDisplayStyle.FieldName:
                    G2DTypeDescriptor.SHOW_DISPLAY_NAME = G2DPropertyFieldDisplayStyle.FieldName_DescName;
                    break;
                case G2DPropertyFieldDisplayStyle.FieldName_DescName:
                    G2DTypeDescriptor.SHOW_DISPLAY_NAME = G2DPropertyFieldDisplayStyle.DescName;
                    break;
                case G2DPropertyFieldDisplayStyle.DescName:
                    G2DTypeDescriptor.SHOW_DISPLAY_NAME = G2DPropertyFieldDisplayStyle.FieldName;
                    break;
            }
            this.OnRefreshDisplay();
            this.Refresh();
        }

        //搜索按钮被点击
        private void Btn_search_Click(object sender, EventArgs e)
        {
            showSearch();
        }

        private void Btn_delete_Click(object sender, EventArgs e)
        {
            DoDelete();
        }
        private void Btn_paste_Click(object sender, EventArgs e)
        {
            DoPaste();
        }
        private void Btn_copy_Click(object sender, EventArgs e)
        {
            DoCopy();
        }
        private void Btn_cut_Click(object sender, EventArgs e)
        {
            DoCut();
        }
        private void Btn_paste_plus_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var history = e.ClickedItem.Tag as CopyHistory.Entry;
            if (history != null)
            {
                DoPastePlus(history);
            }
        }
        private void Btn_paste_plus_DropDownOpening(object sender, EventArgs e)
        {
        }
        #endregion
        //------------------------------------------------------------------------------------------------------
        #region _copying_and_pasting_

        public static int CopyHistoryLimit
        {
            get { return CopyHistory.CopyHistoryLimit; }
            set { CopyHistory.CopyHistoryLimit = value; }
        }

        //----------------------------------------------------------------------------------
        public static void PushCopy(Type declearType, object value)
        {
            CopyHistory.PushCopy(declearType, value, out var list);
        }
        private void DoCopy()
        {
            GridItem item = SelectedGridItem;
            if (item != null && item.PropertyDescriptor != null && item.Value != null)
            {
                var new_item = CopyHistory.Copy(SelectedObject, item, out var list);
                this.OnRefreshHistoryAdded(item, new_item, list);
            }
        }
        private void DoPaste()
        {
            GridItem item = SelectedGridItem;
            if (item != null && item.PropertyDescriptor is G2DOwnerPropertyDescriptor g2dpp)
            {
                var current = CopyHistory.Paste(g2dpp.DecleardFieldType);
                if (current != null)
                {
                    DoPastePlus(current);
                }
            }
        }
        private void DoPastePlus(CopyHistory.Entry copying)
        {
            GridItem item = SelectedGridItem;
            if (item != null && (copying != null) && (item.PropertyDescriptor != null) && (!item.PropertyDescriptor.IsReadOnly))
            {
                if (item.PropertyDescriptor is G2DOwnerPropertyDescriptor g2dpp)
                {
                    try
                    {
                        var target = copying.CloneData(g2dpp.DecleardFieldType);
                        if (target != null)
                        {
                            var oldValue = item.Value;
                            item.PropertyDescriptor.SetValue(g2dpp.ComponentData, target);
                            this.OnPropertyValueChanged(new PropertyValueChangedEventArgs(item, oldValue));
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
            }
        }


        private void DoDelete()
        {
            GridItem item = SelectedGridItem;
            if (item != null && (item.PropertyDescriptor != null) && (!item.PropertyDescriptor.IsReadOnly))
            {
                var oldValue = item.Value;
                if (item.PropertyDescriptor is G2DOwnerPropertyDescriptor g2dpp)
                {
                    if (!g2dpp.NotNull)
                    {
                        try
                        {
                            item.PropertyDescriptor.SetValue(g2dpp.ComponentData, null);
                            this.OnPropertyValueChanged(new PropertyValueChangedEventArgs(item, oldValue));
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }
                    }
                    else if (g2dpp.FieldValue != null)
                    {
                        if (g2dpp.DecleardFieldType.IsArray)
                        {
                            var array = (Array)g2dpp.FieldValue;
                            Array.Clear(array, 0, array.Length);
                        }
                        else if (g2dpp.DecleardFieldType.GetInterface(typeof(IDictionary).Name) != null)
                        {
                            var map = (IDictionary)g2dpp.FieldValue;
                            map.Clear();
                        }
                        else if (g2dpp.DecleardFieldType.GetInterface(typeof(IList).Name) != null)
                        {
                            var list = (IList)g2dpp.FieldValue;
                            list.Clear();
                        }
                        try
                        {
                            item.PropertyDescriptor.SetValue(g2dpp.ComponentData, g2dpp.FieldValue);
                            this.OnPropertyValueChanged(new PropertyValueChangedEventArgs(item, oldValue));
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("字段不可删除！");
                    }
                }
            }
        }
        private void DoCut()
        {
            DoCopy();
            DoDelete();
        }


        #endregion
        //------------------------------------------------------------------------------------------------------
        #region _description_

        public ContextMenuStrip DescMenuStrip { get => docMenuStrip; }
        /// <summary>
        /// Occurs when the description area size is changed by the user.
        /// </summary>
        public event EventHandler UserChangedDescriptionAreaSize;

        /// <summary>
        /// 设置最小默认注释行数量
        /// </summary>
        [Browsable(false)]
        public int MinDescriptionAreaLineCount
        {
            get
            {
                return this.descriptionAreaLineCountMin;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The value cannot be less than zero.");
                }
                this.descriptionAreaLineCountMin = value;
            }
        }
        /// <summary>
        /// Gets or sets the description area line count.
        /// </summary>
        /// <value>The description area line count.</value>
        /// <exception cref="ArgumentException"> If value is less than zero.</exception>
        /// <exception cref="TypeLoadException"> If not of the all objects required to set the field were found.</exception>
        [Browsable(false)]
        public int DescriptionAreaLineCount
        {
            get
            {
                return this.descriptionAreaLineCount;
            }

            set
            {
                if (!inited)
                {
                    return;
                }
                if (value < MinDescriptionAreaLineCount)
                {
                    throw new ArgumentException("The value cannot be less than " + MinDescriptionAreaLineCount + ".");
                }

                if (this.propertyGridView == null)
                {
                    throw new TypeLoadException("Not all of the objects required to set the field were found.");
                }
                if (this.docComment != null)
                {
                    try
                    {
                        int oldDocCommentHeight = this.docComment.Height;
                        int oldValue = this.DescriptionAreaLineCount;
                        this.linesProperty.SetValue(this.docComment, value, null);
                        int difference = this.docComment.Height - oldDocCommentHeight;
                        if (this.docComment.Top - difference > this.propertyGridView.Top)
                        {
                            this.sizeChangeIsFromUser = false;
                            this.propertyGridView.Height -= difference;
                            this.docComment.Top -= difference;
                            this.descriptionAreaLineCount = value;
                            this.sizeChangeIsFromUser = true;
                        }
                        else
                        {
                            this.linesProperty.SetValue(this.docComment, oldValue, null);
                        }
                    }
                    catch (TargetInvocationException)
                    {
                    }
                }
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the height of the description area.
        /// </summary>
        /// <value>The height of the description area.</value>
        [Browsable(false)]
        public int DescriptionAreaHeight
        {
            get
            {
                return this.docComment.Height;
            }

            set
            {
                if (!inited)
                {
                    return;
                }
                if (this.docComment != null)
                {
                    int difference = value - this.docComment.Height;
                    if (this.docComment.Top - difference > this.propertyGridView.Top)
                    {
                        this.docComment.Height = value;
                        this.docComment.Top -= difference;
                        this.propertyGridView.Height -= difference;
                        this.Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// Raises the UserChangedDescriptionAreaSize event.
        /// </summary>
        /// <param name="e">The System.EventArgs instance containing the event data.</param>
        protected void OnUserChangedDescriptionAreaSize(EventArgs e)
        {
            EventHandler handler = this.UserChangedDescriptionAreaSize;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Handles this.docComment.SizeChanged.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The System.EventArgs instance containing the event data.</param>
        private void HandleDocCommentSizeChanged(object sender, EventArgs e)
        {
            if (this.sizeChangeIsFromUser)
            {
                try
                {
                    if (this.docComment != null)
                    {
                        this.descriptionAreaLineCount = (int)this.linesProperty.GetValue(this.docComment, null);
                    }
                    this.OnUserChangedDescriptionAreaSize(EventArgs.Empty);
                }
                catch (TargetInvocationException)
                {
                }
            }
        }

        private void InitializeDesc()
        {
            {
                this.docMenuCopyDesc = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
                this.docMenuCopyDesc.Size = new System.Drawing.Size(255, 30);
                this.docMenuCopyDesc.Text = "拷贝文本";
                this.docMenuCopyDesc.Click += DocMenuCopyDesc_Click;
            }
            {
                this.docMenuCopyFuncField = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
                this.docMenuCopyFuncField.Size = new System.Drawing.Size(255, 30);
                this.docMenuCopyFuncField.Text = "拷贝FuncField@Type";
                this.docMenuCopyFuncField.Click += DocMenuCopyFuncField_Click;
            }
            {
                this.docMenuCopyFuncField2 = new DeepEditor.Common.G2D.G2DBaseToolStripMenuItem();
                this.docMenuCopyFuncField2.Size = new System.Drawing.Size(255, 30);
                this.docMenuCopyFuncField2.Text = "拷贝FuncField";
                this.docMenuCopyFuncField2.Click += DocMenuCopyFuncField2_Click;
            }
            this.docMenuStrip = new DeepEditor.Common.G2D.G2DBaseContextMenuStrip();
            this.docMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.docMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.docMenuCopyDesc, docMenuCopyFuncField, docMenuCopyFuncField2 });
            this.docMenuStrip.Size = new System.Drawing.Size(256, 154);
            if (docComment != null)
            {
                this.docComment.ContextMenuStrip = this.docMenuStrip;
            }
        }
        private void DocMenuCopyDesc_Click(object sender, EventArgs e)
        {
            GridItem item = SelectedGridItem;
            if (item != null && item.PropertyDescriptor != null)
            {
                var name = item.PropertyDescriptor.Name;
                var desc = item.PropertyDescriptor.Description;
                var text = name + Environment.NewLine + desc;
                try
                {
                    Win32.SetClipboard(text.Trim());
                }
                catch { }
            }
        }
        private void DocMenuCopyFuncField_Click(object sender, EventArgs e)
        {
            GridItem item = SelectedGridItem;
            if (item != null && item.PropertyDescriptor != null)
            {
                if (item.PropertyDescriptor is G2DTypeDescriptor.MemberPropertyDescriptor desc)
                {
                    try
                    {
                        Win32.SetClipboard(desc.GetFuncDesc(true));
                    }
                    catch { }
                }
                else
                {
                    var text =
                        item.PropertyDescriptor.DisplayName + Environment.NewLine +
                        '.' + item.PropertyDescriptor.Name + Environment.NewLine +
                        item.PropertyDescriptor.ComponentType.Name;
                    try
                    {
                        Win32.SetClipboard(text.Trim());
                    }
                    catch { }
                }

            }
        }
        private void DocMenuCopyFuncField2_Click(object sender, EventArgs e)
        {
            GridItem item = SelectedGridItem;
            if (item != null && item.PropertyDescriptor != null)
            {
                if (item.PropertyDescriptor is G2DTypeDescriptor.MemberPropertyDescriptor desc)
                {
                    try
                    {
                        Win32.SetClipboard(desc.GetFuncDesc(false));
                    }
                    catch { }
                }
                else
                {
                    var text =
                        item.PropertyDescriptor.DisplayName + Environment.NewLine +
                        '.' + item.PropertyDescriptor.Name + Environment.NewLine +
                        item.PropertyDescriptor.ComponentType.Name;
                    try
                    {
                        Win32.SetClipboard(text.Trim());
                    }
                    catch { }
                }

            }
        }
        #endregion
        //------------------------------------------------------------------------------------------------------



        #region 搜索
        //查找下一个
        private GridItem findNextItem(string text)
        {
            if (text == "")
            {
                return null;
            }

            if (SelectedObject != mLastSearchObject)
            {
                mLastSearchObject = SelectedObject;
                mCurrSearchItem = null;
                mAllItemsList.Clear();
                GetAllGridItems(mAllItemsList);
                if (mAllItemsList.Count > 0)
                {
                    mTopItem = mAllItemsList[0];
                }
            }

            if (mLastSearchText != text)
            {
                mCurrSearchItem = mTopItem;
                mLastSearchText = text;
            }

            var findItem = FormUtils.FindPropertyGridItem(mAllItemsList, mCurrSearchItem, text, true);
            if (findItem == null && mCurrSearchItem != null)
            {
                //再从头搜索一次
                findItem = FormUtils.FindPropertyGridItem(mAllItemsList, null, text, true);
            }
            SetSelectItem(findItem);

            return findItem;
        }

        //查找上一个
        private GridItem findPrevItem(string text)
        {
            if (text == "")
            {
                return null;
            }

            if (SelectedObject != mLastSearchObject)
            {
                mLastSearchObject = SelectedObject;
                mCurrSearchItem = null;
                mAllItemsList.Clear();
                GetAllGridItems(mAllItemsList);
                if (mAllItemsList.Count > 0)
                {
                    mTopItem = mAllItemsList[0];
                }
            }

            if (mLastSearchText != text)
            {
                mCurrSearchItem = mTopItem;
                mLastSearchText = text;
            }

            var findItem = FormUtils.FindPropertyGridItem(mAllItemsList, mCurrSearchItem, text, false);
            SetSelectItem(findItem);

            return findItem;
        }

        //查找
        private GridItem findItem(string text)
        {
            if (text == "")
            {
                return null;
            }
            mLastSearchText = "";//重置搜索词，达到从头搜索的目的
            var item = findNextItem(text);
            SetSelectItem(item);
            return item;
        }

        //设置选择项
        public void SetSelectItem(GridItem item)
        {
            if (item == null)
            {
                return;
            }
            mCurrSearchItem = item;
            //展开
            var expandItem = item.Parent;
            while (expandItem != null)
            {
                if (expandItem.Expandable)
                {
                    expandItem.Expanded = true;
                }
                expandItem = expandItem.Parent;
            }
            //隐藏搜索框
            if (mSearchDialog != null)
            {
                // mSearchDialog.Hide();
            }
            //设置焦点
            item.Select();
            this.Focus();
        }

        private void onSearchClosed()
        {
            mSearchDialog = null;
            this.Focus();
        }
        #endregion




    }

    public static class PropertyGridExt
    {

    }
}
