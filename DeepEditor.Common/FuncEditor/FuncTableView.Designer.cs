
namespace DeepEditor.Common.FuncEditor
{
    partial class FuncTableView
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnFuncID = new System.Windows.Forms.ColumnHeader();
            this.columnFuncLevel = new System.Windows.Forms.ColumnHeader();
            this.columnFuncName = new System.Windows.Forms.ColumnHeader();
            this.columnFuncDesc = new System.Windows.Forms.ColumnHeader();
            this.menu_Field = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_Field_Exclude = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_FieldOP = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Field_OP_Set = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Field_OP_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Field_OP_Sub = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Field.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFuncID,
            this.columnFuncLevel,
            this.columnFuncName,
            this.columnFuncDesc});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.listView1.Name = "listView1";
            this.listView1.OwnerDraw = true;
            this.listView1.Size = new System.Drawing.Size(994, 720);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView1_DrawColumnHeader);
            this.listView1.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listView1_DrawItem);
            this.listView1.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView1_DrawSubItem);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // columnFuncID
            // 
            this.columnFuncID.Text = "FuncID";
            this.columnFuncID.Width = 93;
            // 
            // columnFuncLevel
            // 
            this.columnFuncLevel.Text = "FuncLevel";
            this.columnFuncLevel.Width = 97;
            // 
            // columnFuncName
            // 
            this.columnFuncName.Text = "FuncName";
            this.columnFuncName.Width = 112;
            // 
            // columnFuncDesc
            // 
            this.columnFuncDesc.Text = "FuncDesc";
            this.columnFuncDesc.Width = 115;
            // 
            // menu_Field
            // 
            this.menu_Field.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menu_Field.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_Field_Exclude,
            this.menu_FieldOP});
            this.menu_Field.Name = "contextMenuStrip1";
            this.menu_Field.Size = new System.Drawing.Size(101, 48);
            // 
            // menu_Field_Exclude
            // 
            this.menu_Field_Exclude.CheckOnClick = true;
            this.menu_Field_Exclude.Name = "menu_Field_Exclude";
            this.menu_Field_Exclude.Size = new System.Drawing.Size(100, 22);
            this.menu_Field_Exclude.Text = "禁用";
            this.menu_Field_Exclude.Click += new System.EventHandler(this.menu_Field_Exclude_Click);
            // 
            // menu_FieldOP
            // 
            this.menu_FieldOP.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_Field_OP_Set,
            this.menu_Field_OP_Add,
            this.menu_Field_OP_Sub});
            this.menu_FieldOP.Name = "menu_FieldOP";
            this.menu_FieldOP.Size = new System.Drawing.Size(100, 22);
            this.menu_FieldOP.Text = "行为";
            // 
            // menu_Field_OP_Set
            // 
            this.menu_Field_OP_Set.Name = "menu_Field_OP_Set";
            this.menu_Field_OP_Set.Size = new System.Drawing.Size(128, 22);
            this.menu_Field_OP_Set.Text = "SET(=)";
            this.menu_Field_OP_Set.Click += new System.EventHandler(this.menu_Field_OP_Set_Click);
            // 
            // menu_Field_OP_Add
            // 
            this.menu_Field_OP_Add.Name = "menu_Field_OP_Add";
            this.menu_Field_OP_Add.Size = new System.Drawing.Size(128, 22);
            this.menu_Field_OP_Add.Text = "ADD(+=)";
            this.menu_Field_OP_Add.Click += new System.EventHandler(this.menu_Field_OP_Add_Click);
            // 
            // menu_Field_OP_Sub
            // 
            this.menu_Field_OP_Sub.Name = "menu_Field_OP_Sub";
            this.menu_Field_OP_Sub.Size = new System.Drawing.Size(128, 22);
            this.menu_Field_OP_Sub.Text = "SUB(-=)";
            this.menu_Field_OP_Sub.Click += new System.EventHandler(this.menu_Field_OP_Sub_Click);
            // 
            // FuncTableView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "FuncTableView";
            this.Size = new System.Drawing.Size(994, 720);
            this.menu_Field.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColumnHeader columnFuncID;
        private System.Windows.Forms.ColumnHeader columnFuncLevel;
        private System.Windows.Forms.ColumnHeader columnFuncName;
        private System.Windows.Forms.ColumnHeader columnFuncDesc;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ContextMenuStrip menu_Field;
        private System.Windows.Forms.ToolStripMenuItem menu_Field_Exclude;
        private System.Windows.Forms.ToolStripMenuItem menu_FieldOP;
        private System.Windows.Forms.ToolStripMenuItem menu_Field_OP_Set;
        private System.Windows.Forms.ToolStripMenuItem menu_Field_OP_Add;
        private System.Windows.Forms.ToolStripMenuItem menu_Field_OP_Sub;
    }
}
