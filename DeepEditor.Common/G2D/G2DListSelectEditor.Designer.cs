using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    partial class G2DListSelectEditor
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
            treeView1 = new G2DTreeView();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            buttonCancel = new G2DBaseButton();
            buttonOK = new G2DBaseButton();
            SuspendLayout();
            // 
            // treeView1
            // 
            treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeView1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            treeView1.CustomBackColor = null;
            treeView1.CustomForeColor = null;
            treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeView1.EnableCopyPaste = false;
            treeView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            treeView1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            treeView1.FullRowSelect = true;
            treeView1.HideSelection = false;
            treeView1.LineColor = System.Drawing.Color.Gray;
            treeView1.Location = new System.Drawing.Point(14, 62);
            treeView1.Name = "treeView1";
            treeView1.PathSeparator = "/";
            treeView1.Size = new System.Drawing.Size(803, 805);
            treeView1.TabIndex = 1;
            // 
            // listView1
            // 
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listView1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            listView1.BorderStyle = BorderStyle.None;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            listView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            listView1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            listView1.FullRowSelect = true;
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView1.LabelWrap = false;
            listView1.Location = new System.Drawing.Point(14, 62);
            listView1.Margin = new Padding(3, 4, 3, 4);
            listView1.MinimumSize = new System.Drawing.Size(314, 141);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(803, 806);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Items";
            columnHeader1.Width = 1000;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.AutoSize = false;
            buttonCancel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            buttonCancel.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            buttonCancel.CustomBackColor = null;
            buttonCancel.CustomForeColor = null;
            buttonCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonCancel.Depth = 0;
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            buttonCancel.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            buttonCancel.HighEmphasis = true;
            buttonCancel.Icon = null;
            buttonCancel.Location = new System.Drawing.Point(496, 886);
            buttonCancel.Margin = new Padding(3, 4, 3, 4);
            buttonCancel.MouseState = MaterialSkin.MouseState.HOVER;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonCancel.Size = new System.Drawing.Size(119, 55);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "Cancel";
            buttonCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonCancel.UseAccentColor = false;
            buttonCancel.UseVisualStyleBackColor = false;
            // 
            // buttonOK
            // 
            buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonOK.AutoSize = false;
            buttonOK.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            buttonOK.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            buttonOK.CustomBackColor = null;
            buttonOK.CustomForeColor = null;
            buttonOK.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            buttonOK.Depth = 0;
            buttonOK.DialogResult = DialogResult.OK;
            buttonOK.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            buttonOK.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            buttonOK.HighEmphasis = true;
            buttonOK.Icon = null;
            buttonOK.Location = new System.Drawing.Point(708, 886);
            buttonOK.Margin = new Padding(3, 4, 3, 4);
            buttonOK.MouseState = MaterialSkin.MouseState.HOVER;
            buttonOK.Name = "buttonOK";
            buttonOK.NoAccentTextColor = System.Drawing.Color.Empty;
            buttonOK.Size = new System.Drawing.Size(119, 55);
            buttonOK.TabIndex = 0;
            buttonOK.Text = "OK";
            buttonOK.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            buttonOK.UseAccentColor = false;
            buttonOK.UseVisualStyleBackColor = false;
            // 
            // G2DListSelectEditor
            // 
            AcceptButton = buttonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(840, 955);
            Controls.Add(listView1);
            Controls.Add(treeView1);
            Controls.Add(buttonOK);
            Controls.Add(buttonCancel);
            Margin = new Padding(3, 4, 3, 4);
            Name = "G2DListSelectEditor";
            Padding = new Padding(5, 34, 3, 3);
            Text = "G2DListSelect";
            FormClosing += G2DListSelectEditor_FormClosing;
            Load += G2DListSelectEditor_Load;
            ResumeLayout(false);
        }

        #endregion
        protected DeepEditor.Common.G2D.G2DBaseButton buttonCancel;
        protected DeepEditor.Common.G2D.G2DBaseButton buttonOK;
        protected ListView listView1;
        protected G2DTreeView treeView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}