
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    partial class G2DTaskDialog
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
            richTextBox1 = new RichTextBox();
            splitContainer1 = new SplitContainer();
            btn_cancel = new Button();
            btn_close = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.Color.Black;
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
            richTextBox1.Location = new System.Drawing.Point(0, 0);
            richTextBox1.Margin = new Padding(2, 2, 2, 2);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new System.Drawing.Size(877, 577);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.Location = new System.Drawing.Point(3, 0);
            splitContainer1.Margin = new Padding(2, 2, 2, 2);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(richTextBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btn_cancel);
            splitContainer1.Panel2.Controls.Add(btn_close);
            splitContainer1.Size = new System.Drawing.Size(877, 672);
            splitContainer1.SplitterDistance = 577;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 1;
            // 
            // btn_cancel
            // 
            btn_cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_cancel.Location = new System.Drawing.Point(8, 38);
            btn_cancel.Margin = new Padding(2, 2, 2, 2);
            btn_cancel.Name = "btn_cancel";
            btn_cancel.Size = new System.Drawing.Size(129, 39);
            btn_cancel.TabIndex = 1;
            btn_cancel.Text = "Cancel";
            btn_cancel.UseVisualStyleBackColor = true;
            btn_cancel.Click += btn_cancel_Click;
            // 
            // btn_close
            // 
            btn_close.Anchor = AnchorStyles.Bottom;
            btn_close.Location = new System.Drawing.Point(299, 38);
            btn_close.Margin = new Padding(2, 2, 2, 2);
            btn_close.Name = "btn_close";
            btn_close.Size = new System.Drawing.Size(290, 39);
            btn_close.TabIndex = 0;
            btn_close.Text = "Done";
            btn_close.UseVisualStyleBackColor = true;
            btn_close.Click += btn_close_Click;
            // 
            // G2DTaskDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(882, 674);
            ControlBox = false;
            Controls.Add(splitContainer1);

            Margin = new Padding(2, 2, 2, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "G2DTaskDialog";
            Padding = new Padding(3, 0, 2, 2);
            Text = "FormSendTransaction";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private SplitContainer splitContainer1;
        private Button btn_close;
        private Button btn_cancel;
    }
}