namespace DeepCore.GUI.Win32
{
    partial class Win32SceneCanvas
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Win32SceneCanvas));
            canvas = new Win32PictureBox();
            timer = new System.Windows.Forms.Timer(components);
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            txtStatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
            txtStatusMouse = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // canvas
            // 
            canvas.BackgroundImage = DeepEditor.Common.Properties.Resources.canvasBg2;
            canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            canvas.InitialImage = (System.Drawing.Image)resources.GetObject("canvas.InitialImage");
            canvas.Location = new System.Drawing.Point(0, 0);
            canvas.Margin = new System.Windows.Forms.Padding(5);
            canvas.Name = "canvas";
            canvas.RepaintOnMouseHold = true;
            canvas.Size = new System.Drawing.Size(1596, 1260);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            // 
            // timer
            // 
            timer.Enabled = true;
            timer.Interval = 33;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { txtStatusInfo, txtStatusMouse });
            statusStrip1.Location = new System.Drawing.Point(0, 1260);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(1596, 31);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // txtStatusInfo
            // 
            txtStatusInfo.Name = "txtStatusInfo";
            txtStatusInfo.Size = new System.Drawing.Size(0, 24);
            // 
            // txtStatusMouse
            // 
            txtStatusMouse.Name = "txtStatusMouse";
            txtStatusMouse.Size = new System.Drawing.Size(72, 24);
            txtStatusMouse.Text = "Mouse:";
            // 
            // Win32SceneCanvas
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlDarkDark;
            Controls.Add(canvas);
            Controls.Add(statusStrip1);
            Margin = new System.Windows.Forms.Padding(5);
            Name = "Win32SceneCanvas";
            Size = new System.Drawing.Size(1596, 1291);
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        protected Win32PictureBox canvas;
        protected System.Windows.Forms.Timer timer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel txtStatusMouse;
        private System.Windows.Forms.ToolStripStatusLabel txtStatusInfo;
    }
}
