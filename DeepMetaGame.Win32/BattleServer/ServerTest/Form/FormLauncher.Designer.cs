namespace DeepEditor.Plugin.ServerTest
{
    partial class FormLauncher
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_GameDataRoot = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_PlayerUUID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_ConnectString = new System.Windows.Forms.TextBox();
            this.num_IntervalMS = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.num_SyncRange = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.txt_NetDirver = new System.Windows.Forms.ComboBox();
            this.txt_UnitTemplateID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_Force = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_SceneID = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chk_IsProxy = new System.Windows.Forms.CheckBox();
            this.txt_ProxyConnectString = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_RoomID = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.num_IntervalMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_SyncRange)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "GameDataRoot";
            // 
            // txt_GameDataRoot
            // 
            this.txt_GameDataRoot.Location = new System.Drawing.Point(218, 48);
            this.txt_GameDataRoot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_GameDataRoot.Name = "txt_GameDataRoot";
            this.txt_GameDataRoot.Size = new System.Drawing.Size(588, 28);
            this.txt_GameDataRoot.TabIndex = 18;
            this.txt_GameDataRoot.Text = "/GameEditor/data";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 93);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "PlayerUUID";
            // 
            // txt_PlayerUUID
            // 
            this.txt_PlayerUUID.Location = new System.Drawing.Point(218, 88);
            this.txt_PlayerUUID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_PlayerUUID.Name = "txt_PlayerUUID";
            this.txt_PlayerUUID.Size = new System.Drawing.Size(588, 28);
            this.txt_PlayerUUID.TabIndex = 3;
            this.txt_PlayerUUID.Text = "玩家名字";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(140, 132);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(62, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "RoomID";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 214);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(89, 18);
            this.label4.TabIndex = 8;
            this.label4.Text = "NetDirver";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(76, 260);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(125, 18);
            this.label5.TabIndex = 9;
            this.label5.Text = "ConnectString";
            // 
            // txt_ConnectString
            // 
            this.txt_ConnectString.Location = new System.Drawing.Point(218, 255);
            this.txt_ConnectString.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_ConnectString.Name = "txt_ConnectString";
            this.txt_ConnectString.Size = new System.Drawing.Size(588, 28);
            this.txt_ConnectString.TabIndex = 10;
            this.txt_ConnectString.Text = "192.168.1.95:35000";
            // 
            // num_IntervalMS
            // 
            this.num_IntervalMS.Location = new System.Drawing.Point(218, 296);
            this.num_IntervalMS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.num_IntervalMS.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.num_IntervalMS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_IntervalMS.Name = "num_IntervalMS";
            this.num_IntervalMS.Size = new System.Drawing.Size(180, 28);
            this.num_IntervalMS.TabIndex = 11;
            this.num_IntervalMS.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(104, 298);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label6.Size = new System.Drawing.Size(98, 18);
            this.label6.TabIndex = 12;
            this.label6.Text = "IntervalMS";
            // 
            // num_SyncRange
            // 
            this.num_SyncRange.Location = new System.Drawing.Point(218, 336);
            this.num_SyncRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.num_SyncRange.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.num_SyncRange.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_SyncRange.Name = "num_SyncRange";
            this.num_SyncRange.Size = new System.Drawing.Size(180, 28);
            this.num_SyncRange.TabIndex = 13;
            this.num_SyncRange.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(112, 339);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(89, 18);
            this.label7.TabIndex = 14;
            this.label7.Text = "SyncRange";
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(214, 531);
            this.btn_Connect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(183, 52);
            this.btn_Connect.TabIndex = 15;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // txt_NetDirver
            // 
            this.txt_NetDirver.FormattingEnabled = true;
            this.txt_NetDirver.Items.AddRange(new object[] {
            "DeepCore.Net.Sockets.NetSession"});
            this.txt_NetDirver.Location = new System.Drawing.Point(218, 210);
            this.txt_NetDirver.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_NetDirver.Name = "txt_NetDirver";
            this.txt_NetDirver.Size = new System.Drawing.Size(588, 26);
            this.txt_NetDirver.TabIndex = 16;
            this.txt_NetDirver.Text = "DeepCore.Net.Sockets.NetSession";
            // 
            // txt_UnitTemplateID
            // 
            this.txt_UnitTemplateID.Location = new System.Drawing.Point(218, 376);
            this.txt_UnitTemplateID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_UnitTemplateID.Name = "txt_UnitTemplateID";
            this.txt_UnitTemplateID.Size = new System.Drawing.Size(588, 28);
            this.txt_UnitTemplateID.TabIndex = 1;
            this.txt_UnitTemplateID.Text = "11001";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(68, 381);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label8.Size = new System.Drawing.Size(134, 18);
            this.label8.TabIndex = 17;
            this.label8.Text = "UnitTemplateID";
            // 
            // txt_Force
            // 
            this.txt_Force.Location = new System.Drawing.Point(218, 417);
            this.txt_Force.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Force.Name = "txt_Force";
            this.txt_Force.Size = new System.Drawing.Size(588, 28);
            this.txt_Force.TabIndex = 20;
            this.txt_Force.Text = "0";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(148, 422);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label9.Size = new System.Drawing.Size(53, 18);
            this.label9.TabIndex = 19;
            this.label9.Text = "Force";
            // 
            // txt_SceneID
            // 
            this.txt_SceneID.Location = new System.Drawing.Point(218, 170);
            this.txt_SceneID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_SceneID.Name = "txt_SceneID";
            this.txt_SceneID.Size = new System.Drawing.Size(588, 28);
            this.txt_SceneID.TabIndex = 22;
            this.txt_SceneID.Text = "0";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(130, 174);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label10.Size = new System.Drawing.Size(71, 18);
            this.label10.TabIndex = 21;
            this.label10.Text = "SceneID";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // chk_IsProxy
            // 
            this.chk_IsProxy.AutoSize = true;
            this.chk_IsProxy.Location = new System.Drawing.Point(218, 458);
            this.chk_IsProxy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chk_IsProxy.Name = "chk_IsProxy";
            this.chk_IsProxy.Size = new System.Drawing.Size(178, 22);
            this.chk_IsProxy.TabIndex = 23;
            this.chk_IsProxy.Text = "代理模式（网关）";
            this.chk_IsProxy.UseVisualStyleBackColor = true;
            // 
            // txt_ProxyConnectString
            // 
            this.txt_ProxyConnectString.Location = new System.Drawing.Point(218, 490);
            this.txt_ProxyConnectString.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_ProxyConnectString.Name = "txt_ProxyConnectString";
            this.txt_ProxyConnectString.Size = new System.Drawing.Size(588, 28);
            this.txt_ProxyConnectString.TabIndex = 24;
            this.txt_ProxyConnectString.Text = "192.168.1.81:18888";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(122, 495);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label11.Size = new System.Drawing.Size(80, 18);
            this.label11.TabIndex = 25;
            this.label11.Text = "网关地址";
            // 
            // txt_RoomID
            // 
            this.txt_RoomID.Location = new System.Drawing.Point(218, 128);
            this.txt_RoomID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_RoomID.Name = "txt_RoomID";
            this.txt_RoomID.Size = new System.Drawing.Size(588, 28);
            this.txt_RoomID.TabIndex = 26;
            this.txt_RoomID.Text = "房间ID";
            // 
            // FormLauncher
            // 
            this.AcceptButton = this.btn_Connect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1330, 610);
            this.Controls.Add(this.txt_RoomID);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txt_ProxyConnectString);
            this.Controls.Add(this.chk_IsProxy);
            this.Controls.Add(this.txt_SceneID);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txt_Force);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txt_UnitTemplateID);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txt_NetDirver);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.num_SyncRange);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.num_IntervalMS);
            this.Controls.Add(this.txt_ConnectString);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_PlayerUUID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_GameDataRoot);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormLauncher";
            this.Text = "FormLauncher";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormLauncher_FormClosed);
            this.Load += new System.EventHandler(this.FormLauncher_Load);
            this.Shown += new System.EventHandler(this.FormLauncher_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.num_IntervalMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_SyncRange)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_GameDataRoot;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_PlayerUUID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_ConnectString;
        private System.Windows.Forms.NumericUpDown num_IntervalMS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown num_SyncRange;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.ComboBox txt_NetDirver;
        private System.Windows.Forms.TextBox txt_UnitTemplateID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_Force;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_SceneID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox chk_IsProxy;
        private System.Windows.Forms.TextBox txt_ProxyConnectString;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_RoomID;
    }
}