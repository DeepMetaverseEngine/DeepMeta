namespace DeepEditor.Common.G2D
{
    partial class G2DPasswordDialog
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            buttonOkey = new DeepEditor.Common.G2D.G2DBaseButton();
            buttonCancel = new DeepEditor.Common.G2D.G2DBaseButton();
            textBox1 = new DeepEditor.Common.G2D.G2DBaseTextBox();
            SuspendLayout();
            // 
            // buttonOkey
            // 
            buttonOkey.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonOkey.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOkey.Location = new System.Drawing.Point(289, 138);
            buttonOkey.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            buttonOkey.Name = "buttonOkey";
            buttonOkey.Size = new System.Drawing.Size(92, 36);
            buttonOkey.TabIndex = 1;
            buttonOkey.Text = "OK";
            buttonOkey.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Location = new System.Drawing.Point(193, 138);
            buttonCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(92, 36);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            textBox1.Location = new System.Drawing.Point(12, 84);
            textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            textBox1.Name = "textBox1";
            textBox1.PasswordChar = '*';
            textBox1.Size = new System.Drawing.Size(369, 28);
            textBox1.TabIndex = 0;
            // 
            // G2DPasswordDialog
            // 
            AcceptButton = buttonOkey;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(404, 192);
            Controls.Add(textBox1);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOkey);
            Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            
            Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            MinimumSize = new System.Drawing.Size(388, 153);
            Name = "G2DPasswordDialog";
            Padding = new System.Windows.Forms.Padding(3, 24, 2, 2);
            Text = "Input Passphrase";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DeepEditor.Common.G2D.G2DBaseButton buttonOkey;
        private DeepEditor.Common.G2D.G2DBaseButton buttonCancel;
        private DeepEditor.Common.G2D.G2DBaseTextBox textBox1;
    }
}