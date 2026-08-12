namespace DeepEditor.Common.G2D.DataGrid
{
    partial class G2DCollectionEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(G2DCollectionEditor));
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer3 = new System.Windows.Forms.SplitContainer();
            listView1 = new G2DBaseListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            btn_MoveDownItem = new G2DBaseButton();
            btn_MoveUpItem = new G2DBaseButton();
            btn_DuplicateItem = new G2DBaseButton();
            btn_DelItem = new G2DBaseButton();
            btn_AddItem = new G2DBaseButton();
            propertyGrid1 = new G2DPropertyGrid();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            buttonCancel = new G2DBaseButton();
            buttonOK = new G2DBaseButton();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel1.Controls.Add(splitContainer3);
            splitContainer1.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer1.Panel2.Controls.Add(propertyGrid1);
            splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer1.Size = new System.Drawing.Size(1254, 553);
            splitContainer1.SplitterDistance = 575;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 0;
            // 
            // splitContainer3
            // 
            splitContainer3.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer3.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer3.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer3.Location = new System.Drawing.Point(0, 0);
            splitContainer3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer3.Panel1.Controls.Add(listView1);
            splitContainer3.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer3.Panel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer3.Panel2.Controls.Add(tableLayoutPanel1);
            splitContainer3.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer3.Panel2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer3.Size = new System.Drawing.Size(575, 553);
            splitContainer3.SplitterDistance = 389;
            splitContainer3.SplitterWidth = 3;
            splitContainer3.TabIndex = 1;
            // 
            // listView1
            // 
            listView1.AutoSizeTable = true;
            listView1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2 });
            listView1.CustomBackColor = null;
            listView1.CustomForeColor = null;
            listView1.Depth = 0;
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            listView1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            listView1.FullRowSelect = true;
            listView1.Location = new System.Drawing.Point(0, 0);
            listView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            listView1.MinimumSize = new System.Drawing.Size(200, 100);
            listView1.MouseLocation = new System.Drawing.Point(-1, -1);
            listView1.MouseState = MaterialSkin.MouseState.OUT;
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.OwnerDraw = true;
            listView1.Size = new System.Drawing.Size(389, 553);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.ItemSelectionChanged += listView1_ItemSelectionChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Index";
            columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Data";
            columnHeader2.Width = 299;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(btn_MoveDownItem, 0, 5);
            tableLayoutPanel1.Controls.Add(btn_MoveUpItem, 0, 4);
            tableLayoutPanel1.Controls.Add(btn_DuplicateItem, 0, 2);
            tableLayoutPanel1.Controls.Add(btn_DelItem, 0, 1);
            tableLayoutPanel1.Controls.Add(btn_AddItem, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            tableLayoutPanel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(183, 553);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // btn_MoveDownItem
            // 
            btn_MoveDownItem.AutoSize = false;
            btn_MoveDownItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btn_MoveDownItem.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_MoveDownItem.CustomBackColor = null;
            btn_MoveDownItem.CustomForeColor = null;
            btn_MoveDownItem.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btn_MoveDownItem.Depth = 0;
            btn_MoveDownItem.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_MoveDownItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btn_MoveDownItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_MoveDownItem.HighEmphasis = true;
            btn_MoveDownItem.Icon = (System.Drawing.Image)resources.GetObject("btn_MoveDownItem.Icon");
            btn_MoveDownItem.Image = (System.Drawing.Image)resources.GetObject("btn_MoveDownItem.Image");
            btn_MoveDownItem.Location = new System.Drawing.Point(2, 152);
            btn_MoveDownItem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            btn_MoveDownItem.MouseState = MaterialSkin.MouseState.HOVER;
            btn_MoveDownItem.Name = "btn_MoveDownItem";
            btn_MoveDownItem.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_MoveDownItem.Size = new System.Drawing.Size(179, 28);
            btn_MoveDownItem.TabIndex = 3;
            btn_MoveDownItem.Text = "向下";
            btn_MoveDownItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            btn_MoveDownItem.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            btn_MoveDownItem.UseAccentColor = false;
            btn_MoveDownItem.UseVisualStyleBackColor = false;
            btn_MoveDownItem.Click += btn_MoveDownItem_Click;
            // 
            // btn_MoveUpItem
            // 
            btn_MoveUpItem.AutoSize = false;
            btn_MoveUpItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btn_MoveUpItem.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_MoveUpItem.CustomBackColor = null;
            btn_MoveUpItem.CustomForeColor = null;
            btn_MoveUpItem.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btn_MoveUpItem.Depth = 0;
            btn_MoveUpItem.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_MoveUpItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btn_MoveUpItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_MoveUpItem.HighEmphasis = true;
            btn_MoveUpItem.Icon = (System.Drawing.Image)resources.GetObject("btn_MoveUpItem.Icon");
            btn_MoveUpItem.Image = (System.Drawing.Image)resources.GetObject("btn_MoveUpItem.Image");
            btn_MoveUpItem.Location = new System.Drawing.Point(2, 120);
            btn_MoveUpItem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            btn_MoveUpItem.MouseState = MaterialSkin.MouseState.HOVER;
            btn_MoveUpItem.Name = "btn_MoveUpItem";
            btn_MoveUpItem.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_MoveUpItem.Size = new System.Drawing.Size(179, 28);
            btn_MoveUpItem.TabIndex = 2;
            btn_MoveUpItem.Text = "向上";
            btn_MoveUpItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            btn_MoveUpItem.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            btn_MoveUpItem.UseAccentColor = false;
            btn_MoveUpItem.UseVisualStyleBackColor = false;
            btn_MoveUpItem.Click += btn_MoveUpItem_Click;
            // 
            // btn_DuplicateItem
            // 
            btn_DuplicateItem.AutoSize = false;
            btn_DuplicateItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btn_DuplicateItem.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_DuplicateItem.CustomBackColor = null;
            btn_DuplicateItem.CustomForeColor = null;
            btn_DuplicateItem.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btn_DuplicateItem.Depth = 0;
            btn_DuplicateItem.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_DuplicateItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btn_DuplicateItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_DuplicateItem.HighEmphasis = true;
            btn_DuplicateItem.Icon = (System.Drawing.Image)resources.GetObject("btn_DuplicateItem.Icon");
            btn_DuplicateItem.Image = (System.Drawing.Image)resources.GetObject("btn_DuplicateItem.Image");
            btn_DuplicateItem.Location = new System.Drawing.Point(2, 74);
            btn_DuplicateItem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            btn_DuplicateItem.MouseState = MaterialSkin.MouseState.HOVER;
            btn_DuplicateItem.Name = "btn_DuplicateItem";
            btn_DuplicateItem.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_DuplicateItem.Size = new System.Drawing.Size(179, 28);
            btn_DuplicateItem.TabIndex = 6;
            btn_DuplicateItem.Text = "复制";
            btn_DuplicateItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            btn_DuplicateItem.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            btn_DuplicateItem.UseAccentColor = false;
            btn_DuplicateItem.UseVisualStyleBackColor = false;
            btn_DuplicateItem.Click += btn_DuplicateItem_Click;
            // 
            // btn_DelItem
            // 
            btn_DelItem.AutoSize = false;
            btn_DelItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btn_DelItem.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_DelItem.CustomBackColor = null;
            btn_DelItem.CustomForeColor = null;
            btn_DelItem.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btn_DelItem.Depth = 0;
            btn_DelItem.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_DelItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btn_DelItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_DelItem.HighEmphasis = true;
            btn_DelItem.Icon = (System.Drawing.Image)resources.GetObject("btn_DelItem.Icon");
            btn_DelItem.Image = (System.Drawing.Image)resources.GetObject("btn_DelItem.Image");
            btn_DelItem.Location = new System.Drawing.Point(2, 42);
            btn_DelItem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            btn_DelItem.MouseState = MaterialSkin.MouseState.HOVER;
            btn_DelItem.Name = "btn_DelItem";
            btn_DelItem.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_DelItem.Size = new System.Drawing.Size(179, 28);
            btn_DelItem.TabIndex = 4;
            btn_DelItem.Text = "删除";
            btn_DelItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            btn_DelItem.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            btn_DelItem.UseAccentColor = false;
            btn_DelItem.UseVisualStyleBackColor = false;
            btn_DelItem.Click += btn_DelItem_Click;
            // 
            // btn_AddItem
            // 
            btn_AddItem.AutoSize = false;
            btn_AddItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btn_AddItem.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            btn_AddItem.CustomBackColor = null;
            btn_AddItem.CustomForeColor = null;
            btn_AddItem.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btn_AddItem.Depth = 0;
            btn_AddItem.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_AddItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btn_AddItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            btn_AddItem.HighEmphasis = true;
            btn_AddItem.Icon = (System.Drawing.Image)resources.GetObject("btn_AddItem.Icon");
            btn_AddItem.Image = (System.Drawing.Image)resources.GetObject("btn_AddItem.Image");
            btn_AddItem.Location = new System.Drawing.Point(2, 2);
            btn_AddItem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            btn_AddItem.MouseState = MaterialSkin.MouseState.HOVER;
            btn_AddItem.Name = "btn_AddItem";
            btn_AddItem.NoAccentTextColor = System.Drawing.Color.Empty;
            btn_AddItem.Size = new System.Drawing.Size(179, 36);
            btn_AddItem.TabIndex = 5;
            btn_AddItem.Text = "添加";
            btn_AddItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            btn_AddItem.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            btn_AddItem.UseAccentColor = false;
            btn_AddItem.UseVisualStyleBackColor = false;
            btn_AddItem.Click += btn_AddItem_Click;
            // 
            // propertyGrid1
            // 
            propertyGrid1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            propertyGrid1.CategoryForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            propertyGrid1.CategorySplitterColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            propertyGrid1.CommandsBackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            propertyGrid1.CommandsBorderColor = System.Drawing.Color.FromArgb(242, 242, 242);
            propertyGrid1.CommandsForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            propertyGrid1.CustomBackColor = null;
            propertyGrid1.CustomForeColor = null;
            propertyGrid1.DescriptionAreaHeight = 59;
            propertyGrid1.DescriptionAreaLineCount = 2;
            propertyGrid1.DisabledItemForeColor = System.Drawing.Color.Gray;
            propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            propertyGrid1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            propertyGrid1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            propertyGrid1.HelpBackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            propertyGrid1.HelpBorderColor = System.Drawing.Color.FromArgb(30, 0, 0, 0);
            propertyGrid1.HelpForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            propertyGrid1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            propertyGrid1.LineColor = System.Drawing.Color.LightGray;
            propertyGrid1.Location = new System.Drawing.Point(0, 0);
            propertyGrid1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            propertyGrid1.MinDescriptionAreaLineCount = 5;
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            propertyGrid1.Size = new System.Drawing.Size(676, 553);
            propertyGrid1.TabIndex = 0;
            propertyGrid1.ViewBackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            propertyGrid1.ViewBorderColor = System.Drawing.Color.FromArgb(242, 242, 242);
            propertyGrid1.ViewForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            propertyGrid1.PropertyValueChanged += propertyGrid1_PropertyValueChanged;
            propertyGrid1.Click += propertyGrid1_Click;
            // 
            // splitContainer2
            // 
            splitContainer2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer2.Location = new System.Drawing.Point(3, 24);
            splitContainer2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer2.Panel1.Controls.Add(splitContainer1);
            splitContainer2.Panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer2.Panel1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            splitContainer2.Panel2.Controls.Add(buttonCancel);
            splitContainer2.Panel2.Controls.Add(buttonOK);
            splitContainer2.Panel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            splitContainer2.Panel2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            splitContainer2.Size = new System.Drawing.Size(1254, 666);
            splitContainer2.SplitterDistance = 553;
            splitContainer2.SplitterWidth = 3;
            splitContainer2.TabIndex = 1;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonCancel.AutoSize = false;
            buttonCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            buttonCancel.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            buttonCancel.CustomBackColor = null;
            buttonCancel.CustomForeColor = null;
            buttonCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonCancel.Depth = 0;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            buttonCancel.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            buttonCancel.HighEmphasis = true;
            buttonCancel.Icon = null;
            buttonCancel.Location = new System.Drawing.Point(1032, 49);
            buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            buttonCancel.MouseState = MaterialSkin.MouseState.HOVER;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonCancel.Size = new System.Drawing.Size(88, 45);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "取消";
            buttonCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonCancel.UseAccentColor = false;
            buttonCancel.UseVisualStyleBackColor = false;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonOK
            // 
            buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonOK.AutoSize = false;
            buttonOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            buttonOK.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            buttonOK.CustomBackColor = null;
            buttonOK.CustomForeColor = null;
            buttonOK.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonOK.Depth = 0;
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOK.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            buttonOK.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            buttonOK.HighEmphasis = true;
            buttonOK.Icon = null;
            buttonOK.Location = new System.Drawing.Point(1147, 49);
            buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            buttonOK.MouseState = MaterialSkin.MouseState.HOVER;
            buttonOK.Name = "buttonOK";
            buttonOK.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonOK.Size = new System.Drawing.Size(88, 45);
            buttonOK.TabIndex = 0;
            buttonOK.Text = "确定";
            buttonOK.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonOK.UseAccentColor = false;
            buttonOK.UseVisualStyleBackColor = false;
            buttonOK.Click += buttonOK_Click;
            // 
            // G2DCollectionEditor
            // 
            AcceptButton = buttonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(1259, 692);
            Controls.Add(splitContainer2);
            Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            Name = "G2DCollectionEditor";
            Padding = new System.Windows.Forms.Padding(3, 24, 2, 2);
            Text = "G2DCollectionEditor";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DeepEditor.Common.G2D.DataGrid.G2DPropertyGrid propertyGrid1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DeepEditor.Common.G2D.G2DBaseButton buttonCancel;
        private DeepEditor.Common.G2D.G2DBaseButton buttonOK;
        private DeepEditor.Common.G2D.G2DBaseButton btn_AddItem;
        private DeepEditor.Common.G2D.G2DBaseButton btn_DelItem;
        private DeepEditor.Common.G2D.G2DBaseButton btn_MoveDownItem;
        private DeepEditor.Common.G2D.G2DBaseButton btn_MoveUpItem;
        private DeepEditor.Common.G2D.G2DBaseListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private DeepEditor.Common.G2D.G2DBaseButton btn_DuplicateItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}