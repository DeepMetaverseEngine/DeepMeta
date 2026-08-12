
namespace DeepEditor.Plugin3D.BattleServer.Host
{
    partial class FormServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServer));
            toolStrip1 = new Common.G2D.G2DBaseToolStrip();
            toolStripDropDownButton1 = new Common.G2D.G2DBaseToolStripDropDownButton();
            btn_AddPlayer = new Common.G2D.G2DBaseToolStripMenuItem();
            btn_addPlayerStandalone = new Common.G2D.G2DBaseToolStripMenuItem();
            btn_EmulateDelay = new Common.G2D.G2DBaseToolStripMenuItem();
            btn_ShowMsgBytes = new Common.G2D.G2DBaseToolStripMenuItem();
            disconnectAllToolStripMenuItem = new Common.G2D.G2DBaseToolStripMenuItem();
            btn_Bots = new Common.G2D.G2DBaseToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            btn_GC = new Common.G2D.G2DBaseToolStripMenuItem();
            toolStripDropDownButton2 = new Common.G2D.G2DBaseToolStripDropDownButton();
            chk_ServerVisible = new Common.G2D.G2DBaseToolStripMenuItem();
            chk_EnableAOI = new Common.G2D.G2DBaseToolStripMenuItem();
            battlePanel = new BattleClient.PanelBattleView3D();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.FromArgb(242, 242, 242);
            toolStrip1.CustomBackColor = null;
            toolStrip1.CustomForeColor = null;
            toolStrip1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            toolStrip1.ForeColor = Color.FromArgb(0, 0, 0);
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton2 });
            toolStrip1.Location = new Point(4, 32);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(0, 0, 4, 0);
            toolStrip1.Size = new Size(1862, 29);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.CustomBackColor = null;
            toolStripDropDownButton1.CustomForeColor = null;
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { btn_AddPlayer, btn_addPlayerStandalone, btn_EmulateDelay, btn_ShowMsgBytes, disconnectAllToolStripMenuItem, btn_Bots, toolStripMenuItem1, btn_GC });
            toolStripDropDownButton1.ForeColor = Color.FromArgb(0, 0, 0);
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(67, 24);
            toolStripDropDownButton1.Text = "Menu";
            // 
            // btn_AddPlayer
            // 
            btn_AddPlayer.CustomBackColor = null;
            btn_AddPlayer.CustomForeColor = null;
            btn_AddPlayer.ForeColor = Color.FromArgb(0, 0, 0);
            btn_AddPlayer.Name = "btn_AddPlayer";
            btn_AddPlayer.Size = new Size(283, 34);
            btn_AddPlayer.Text = "AddPlayer";
            btn_AddPlayer.Click += btn_AddPlayer_Click;
            // 
            // btn_addPlayerStandalone
            // 
            btn_addPlayerStandalone.CustomBackColor = null;
            btn_addPlayerStandalone.CustomForeColor = null;
            btn_addPlayerStandalone.ForeColor = Color.FromArgb(0, 0, 0);
            btn_addPlayerStandalone.Name = "btn_addPlayerStandalone";
            btn_addPlayerStandalone.Size = new Size(283, 34);
            btn_addPlayerStandalone.Text = "AddPlayer(Standalone)";
            btn_addPlayerStandalone.Click += btn_addPlayerStandalone_Click;
            // 
            // btn_EmulateDelay
            // 
            btn_EmulateDelay.CustomBackColor = null;
            btn_EmulateDelay.CustomForeColor = null;
            btn_EmulateDelay.ForeColor = Color.FromArgb(0, 0, 0);
            btn_EmulateDelay.Name = "btn_EmulateDelay";
            btn_EmulateDelay.Size = new Size(283, 34);
            btn_EmulateDelay.Text = "Emulate Net Delay";
            btn_EmulateDelay.Click += btn_EmulateDelay_Click;
            // 
            // btn_ShowMsgBytes
            // 
            btn_ShowMsgBytes.CustomBackColor = null;
            btn_ShowMsgBytes.CustomForeColor = null;
            btn_ShowMsgBytes.ForeColor = Color.FromArgb(0, 0, 0);
            btn_ShowMsgBytes.Name = "btn_ShowMsgBytes";
            btn_ShowMsgBytes.Size = new Size(283, 34);
            btn_ShowMsgBytes.Text = "Show Message Type Bytes";
            btn_ShowMsgBytes.Click += btn_ShowMsgBytes_Click;
            // 
            // disconnectAllToolStripMenuItem
            // 
            disconnectAllToolStripMenuItem.CustomBackColor = null;
            disconnectAllToolStripMenuItem.CustomForeColor = null;
            disconnectAllToolStripMenuItem.ForeColor = Color.FromArgb(0, 0, 0);
            disconnectAllToolStripMenuItem.Name = "disconnectAllToolStripMenuItem";
            disconnectAllToolStripMenuItem.Size = new Size(283, 34);
            disconnectAllToolStripMenuItem.Text = "Disconnect All";
            disconnectAllToolStripMenuItem.Click += btn_disconnectAll_Click;
            // 
            // btn_Bots
            // 
            btn_Bots.CustomBackColor = null;
            btn_Bots.CustomForeColor = null;
            btn_Bots.ForeColor = Color.FromArgb(0, 0, 0);
            btn_Bots.Name = "btn_Bots";
            btn_Bots.Size = new Size(283, 34);
            btn_Bots.Text = "Bots";
            btn_Bots.Click += btn_Bots_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.BackColor = Color.FromArgb(242, 242, 242);
            toolStripMenuItem1.ForeColor = Color.FromArgb(30, 0, 0, 0);
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(280, 6);
            // 
            // btn_GC
            // 
            btn_GC.CustomBackColor = null;
            btn_GC.CustomForeColor = null;
            btn_GC.ForeColor = Color.FromArgb(0, 0, 0);
            btn_GC.Name = "btn_GC";
            btn_GC.ShortcutKeys = Keys.F2;
            btn_GC.Size = new Size(283, 34);
            btn_GC.Text = "GC";
            btn_GC.Click += btn_GC_Click;
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.CustomBackColor = null;
            toolStripDropDownButton2.CustomForeColor = null;
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { chk_ServerVisible, chk_EnableAOI });
            toolStripDropDownButton2.ForeColor = Color.FromArgb(0, 0, 0);
            toolStripDropDownButton2.Image = (Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new Size(80, 24);
            toolStripDropDownButton2.Text = "Options";
            // 
            // chk_ServerVisible
            // 
            chk_ServerVisible.Checked = true;
            chk_ServerVisible.CheckOnClick = true;
            chk_ServerVisible.CheckState = CheckState.Checked;
            chk_ServerVisible.CustomBackColor = null;
            chk_ServerVisible.CustomForeColor = null;
            chk_ServerVisible.ForeColor = Color.FromArgb(0, 0, 0);
            chk_ServerVisible.Name = "chk_ServerVisible";
            chk_ServerVisible.Size = new Size(197, 34);
            chk_ServerVisible.Text = "Server Visible";
            chk_ServerVisible.Click += chk_ServerVisible_Click;
            // 
            // chk_EnableAOI
            // 
            chk_EnableAOI.Checked = true;
            chk_EnableAOI.CheckOnClick = true;
            chk_EnableAOI.CheckState = CheckState.Checked;
            chk_EnableAOI.CustomBackColor = null;
            chk_EnableAOI.CustomForeColor = null;
            chk_EnableAOI.ForeColor = Color.FromArgb(0, 0, 0);
            chk_EnableAOI.Name = "chk_EnableAOI";
            chk_EnableAOI.Size = new Size(197, 34);
            chk_EnableAOI.Text = "Enable AOI";
            chk_EnableAOI.Click += chk_EnableAOI_Click;
            // 
            // battleView
            // 
            battlePanel.BackColor = Color.FromArgb(242, 242, 242);
            battlePanel.Dock = DockStyle.Fill;
            battlePanel.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            battlePanel.ForeColor = Color.FromArgb(0, 0, 0);
            battlePanel.Location = new Point(4, 61);
            battlePanel.Margin = new Padding(4, 5, 4, 5);
            battlePanel.Name = "battleView";
            battlePanel.Size = new Size(1862, 1275);
            battlePanel.TabIndex = 3;
            // 
            // FormServer
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1870, 1340);
            Controls.Add(battlePanel);
            Controls.Add(toolStrip1);
            Margin = new Padding(5);
            Name = "FormServer";
            Padding = new Padding(4, 32, 4, 4);
            Text = "FormServer";
            Load += FormServer_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DeepEditor.Common.G2D.G2DBaseToolStrip toolStrip1;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton toolStripDropDownButton1;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_AddPlayer;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_EmulateDelay;
        private DeepEditor.Common.G2D.G2DBaseToolStripDropDownButton toolStripDropDownButton2;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem chk_ServerVisible;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_ShowMsgBytes;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_GC;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem disconnectAllToolStripMenuItem;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem chk_EnableAOI;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_Bots;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private DeepEditor.Common.G2D.G2DBaseToolStripMenuItem btn_addPlayerStandalone;
        private BattleClient.PanelBattleView3D battlePanel;
    }
}

