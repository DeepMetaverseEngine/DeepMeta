
namespace DeepEditor.Plugin3D.BattleServer.Slave
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
            components = new System.ComponentModel.Container();
            label1 = new Common.G2D.G2DBaseLabel();
            txt_GameDataRoot = new Common.G2D.G2DBaseRichTextBox();
            label2 = new Common.G2D.G2DBaseLabel();
            txt_PlayerUUID = new Common.G2D.G2DBaseRichTextBox();
            label3 = new Common.G2D.G2DBaseLabel();
            label4 = new Common.G2D.G2DBaseLabel();
            label5 = new Common.G2D.G2DBaseLabel();
            txt_ConnectString = new Common.G2D.G2DBaseRichTextBox();
            num_IntervalMS = new NumericUpDown();
            label6 = new Common.G2D.G2DBaseLabel();
            num_SyncRange = new NumericUpDown();
            label7 = new Common.G2D.G2DBaseLabel();
            btn_Connect = new Common.G2D.G2DBaseButton();
            txt_NetDirver = new Common.G2D.G2DBaseComboBox();
            txt_UnitTemplateID = new Common.G2D.G2DBaseRichTextBox();
            label8 = new Common.G2D.G2DBaseLabel();
            txt_Force = new Common.G2D.G2DBaseRichTextBox();
            label9 = new Common.G2D.G2DBaseLabel();
            txt_SceneID = new Common.G2D.G2DBaseRichTextBox();
            label10 = new Common.G2D.G2DBaseLabel();
            timer1 = new System.Windows.Forms.Timer(components);
            chk_IsProxy = new Common.G2D.G2DBaseCheckBox();
            txt_ProxyConnectString = new Common.G2D.G2DBaseRichTextBox();
            label11 = new Common.G2D.G2DBaseLabel();
            txt_RoomID = new Common.G2D.G2DBaseRichTextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)num_IntervalMS).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_SyncRange).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(242, 242, 242);
            label1.CustomBackColor = null;
            label1.CustomForeColor = null;
            label1.Depth = 0;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label1.Location = new Point(3, 0);
            label1.MouseState = MaterialSkin.MouseState.HOVER;
            label1.Name = "label1";
            label1.RightToLeft = RightToLeft.Yes;
            label1.Size = new Size(121, 32);
            label1.TabIndex = 0;
            label1.Text = "GameDataRoot";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txt_GameDataRoot
            // 
            txt_GameDataRoot.BackColor = Color.FromArgb(255, 255, 255);
            txt_GameDataRoot.BorderStyle = BorderStyle.None;
            txt_GameDataRoot.CustomBackColor = null;
            txt_GameDataRoot.CustomForeColor = null;
            txt_GameDataRoot.Dock = DockStyle.Left;
            txt_GameDataRoot.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txt_GameDataRoot.ForeColor = Color.FromArgb(222, 0, 0, 0);
            txt_GameDataRoot.Location = new Point(130, 4);
            txt_GameDataRoot.Margin = new Padding(3, 4, 3, 4);
            txt_GameDataRoot.MaxLength = 50;
            txt_GameDataRoot.Multiline = false;
            txt_GameDataRoot.Name = "txt_GameDataRoot";
            txt_GameDataRoot.Size = new Size(1014, 24);
            txt_GameDataRoot.TabIndex = 18;
            txt_GameDataRoot.Text = "/GameEditor/data";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(242, 242, 242);
            label2.CustomBackColor = null;
            label2.CustomForeColor = null;
            label2.Depth = 0;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label2.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label2.Location = new Point(3, 32);
            label2.MouseState = MaterialSkin.MouseState.HOVER;
            label2.Name = "label2";
            label2.RightToLeft = RightToLeft.Yes;
            label2.Size = new Size(121, 32);
            label2.TabIndex = 2;
            label2.Text = "PlayerUUID";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txt_PlayerUUID
            // 
            txt_PlayerUUID.BackColor = Color.FromArgb(255, 255, 255);
            txt_PlayerUUID.BorderStyle = BorderStyle.None;
            txt_PlayerUUID.CustomBackColor = null;
            txt_PlayerUUID.CustomForeColor = null;
            txt_PlayerUUID.Dock = DockStyle.Left;
            txt_PlayerUUID.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txt_PlayerUUID.ForeColor = Color.FromArgb(222, 0, 0, 0);
            txt_PlayerUUID.Location = new Point(130, 36);
            txt_PlayerUUID.Margin = new Padding(3, 4, 3, 4);
            txt_PlayerUUID.MaxLength = 50;
            txt_PlayerUUID.Multiline = false;
            txt_PlayerUUID.Name = "txt_PlayerUUID";
            txt_PlayerUUID.Size = new Size(1014, 24);
            txt_PlayerUUID.TabIndex = 3;
            txt_PlayerUUID.Text = "玩家名字";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(242, 242, 242);
            label3.CustomBackColor = null;
            label3.CustomForeColor = null;
            label3.Depth = 0;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label3.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label3.Location = new Point(3, 64);
            label3.MouseState = MaterialSkin.MouseState.HOVER;
            label3.Name = "label3";
            label3.RightToLeft = RightToLeft.Yes;
            label3.Size = new Size(121, 32);
            label3.TabIndex = 4;
            label3.Text = "RoomID";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(242, 242, 242);
            label4.CustomBackColor = null;
            label4.CustomForeColor = null;
            label4.Depth = 0;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label4.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label4.Location = new Point(3, 128);
            label4.MouseState = MaterialSkin.MouseState.HOVER;
            label4.Name = "label4";
            label4.RightToLeft = RightToLeft.Yes;
            label4.Size = new Size(121, 58);
            label4.TabIndex = 8;
            label4.Text = "NetDirver";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(242, 242, 242);
            label5.CustomBackColor = null;
            label5.CustomForeColor = null;
            label5.Depth = 0;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label5.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label5.Location = new Point(3, 186);
            label5.MouseState = MaterialSkin.MouseState.HOVER;
            label5.Name = "label5";
            label5.RightToLeft = RightToLeft.Yes;
            label5.Size = new Size(121, 32);
            label5.TabIndex = 9;
            label5.Text = "ConnectString";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txt_ConnectString
            // 
            txt_ConnectString.BackColor = Color.FromArgb(255, 255, 255);
            txt_ConnectString.BorderStyle = BorderStyle.None;
            txt_ConnectString.CustomBackColor = null;
            txt_ConnectString.CustomForeColor = null;
            txt_ConnectString.Dock = DockStyle.Left;
            txt_ConnectString.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txt_ConnectString.ForeColor = Color.FromArgb(222, 0, 0, 0);
            txt_ConnectString.Location = new Point(130, 190);
            txt_ConnectString.Margin = new Padding(3, 4, 3, 4);
            txt_ConnectString.MaxLength = 50;
            txt_ConnectString.Multiline = false;
            txt_ConnectString.Name = "txt_ConnectString";
            txt_ConnectString.Size = new Size(1014, 24);
            txt_ConnectString.TabIndex = 10;
            txt_ConnectString.Text = "192.168.1.95:35000";
            // 
            // num_IntervalMS
            // 
            num_IntervalMS.BackColor = Color.FromArgb(242, 242, 242);
            num_IntervalMS.Dock = DockStyle.Left;
            num_IntervalMS.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            num_IntervalMS.ForeColor = Color.FromArgb(222, 0, 0, 0);
            num_IntervalMS.Location = new Point(130, 222);
            num_IntervalMS.Margin = new Padding(3, 4, 3, 4);
            num_IntervalMS.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            num_IntervalMS.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_IntervalMS.Name = "num_IntervalMS";
            num_IntervalMS.Size = new Size(140, 25);
            num_IntervalMS.TabIndex = 11;
            num_IntervalMS.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(242, 242, 242);
            label6.CustomBackColor = null;
            label6.CustomForeColor = null;
            label6.Depth = 0;
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label6.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label6.Location = new Point(3, 218);
            label6.MouseState = MaterialSkin.MouseState.HOVER;
            label6.Name = "label6";
            label6.RightToLeft = RightToLeft.Yes;
            label6.Size = new Size(121, 33);
            label6.TabIndex = 12;
            label6.Text = "IntervalMS";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // num_SyncRange
            // 
            num_SyncRange.BackColor = Color.FromArgb(242, 242, 242);
            num_SyncRange.Dock = DockStyle.Left;
            num_SyncRange.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            num_SyncRange.ForeColor = Color.FromArgb(222, 0, 0, 0);
            num_SyncRange.Location = new Point(130, 255);
            num_SyncRange.Margin = new Padding(3, 4, 3, 4);
            num_SyncRange.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            num_SyncRange.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_SyncRange.Name = "num_SyncRange";
            num_SyncRange.Size = new Size(140, 25);
            num_SyncRange.TabIndex = 13;
            num_SyncRange.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(242, 242, 242);
            label7.CustomBackColor = null;
            label7.CustomForeColor = null;
            label7.Depth = 0;
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label7.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label7.Location = new Point(3, 251);
            label7.MouseState = MaterialSkin.MouseState.HOVER;
            label7.Name = "label7";
            label7.RightToLeft = RightToLeft.Yes;
            label7.Size = new Size(121, 33);
            label7.TabIndex = 14;
            label7.Text = "SyncRange";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btn_Connect
            // 
            btn_Connect.AutoSize = false;
            btn_Connect.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btn_Connect.BackColor = Color.FromArgb(242, 242, 242);
            btn_Connect.CustomBackColor = null;
            btn_Connect.CustomForeColor = null;
            btn_Connect.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btn_Connect.Depth = 0;
            btn_Connect.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btn_Connect.ForeColor = Color.FromArgb(222, 0, 0, 0);
            btn_Connect.HighEmphasis = true;
            btn_Connect.Icon = null;
            btn_Connect.Location = new Point(130, 421);
            btn_Connect.Margin = new Padding(3, 4, 3, 4);
            btn_Connect.MouseState = MaterialSkin.MouseState.HOVER;
            btn_Connect.Name = "btn_Connect";
            btn_Connect.NoAccentTextColor = Color.Empty;
            btn_Connect.Size = new Size(142, 49);
            btn_Connect.TabIndex = 15;
            btn_Connect.Text = "Connect";
            btn_Connect.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btn_Connect.UseAccentColor = false;
            btn_Connect.UseVisualStyleBackColor = false;
            btn_Connect.Click += btn_Connect_Click;
            // 
            // txt_NetDirver
            // 
            txt_NetDirver.AutoResize = true;
            txt_NetDirver.BackColor = Color.FromArgb(242, 242, 242);
            txt_NetDirver.CustomBackColor = null;
            txt_NetDirver.CustomForeColor = null;
            txt_NetDirver.Depth = 0;
            txt_NetDirver.Dock = DockStyle.Left;
            txt_NetDirver.DrawMode = DrawMode.OwnerDrawVariable;
            txt_NetDirver.DropDownHeight = 174;
            txt_NetDirver.DropDownStyle = ComboBoxStyle.DropDownList;
            txt_NetDirver.DropDownWidth = 300;
            txt_NetDirver.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txt_NetDirver.ForeColor = Color.FromArgb(222, 0, 0, 0);
            txt_NetDirver.FormattingEnabled = true;
            txt_NetDirver.IntegralHeight = false;
            txt_NetDirver.ItemHeight = 43;
            txt_NetDirver.Items.AddRange(new object[] { "DeepCore.Net.Sockets.NetSession" });
            txt_NetDirver.Location = new Point(130, 132);
            txt_NetDirver.Margin = new Padding(3, 4, 3, 4);
            txt_NetDirver.MaxDropDownItems = 4;
            txt_NetDirver.MouseState = MaterialSkin.MouseState.OUT;
            txt_NetDirver.Name = "txt_NetDirver";
            txt_NetDirver.Size = new Size(300, 49);
            txt_NetDirver.StartIndex = 0;
            txt_NetDirver.TabIndex = 16;
            // 
            // txt_UnitTemplateID
            // 
            txt_UnitTemplateID.BackColor = Color.FromArgb(255, 255, 255);
            txt_UnitTemplateID.BorderStyle = BorderStyle.None;
            txt_UnitTemplateID.CustomBackColor = null;
            txt_UnitTemplateID.CustomForeColor = null;
            txt_UnitTemplateID.Dock = DockStyle.Left;
            txt_UnitTemplateID.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txt_UnitTemplateID.ForeColor = Color.FromArgb(222, 0, 0, 0);
            txt_UnitTemplateID.Location = new Point(130, 288);
            txt_UnitTemplateID.Margin = new Padding(3, 4, 3, 4);
            txt_UnitTemplateID.MaxLength = 50;
            txt_UnitTemplateID.Multiline = false;
            txt_UnitTemplateID.Name = "txt_UnitTemplateID";
            txt_UnitTemplateID.Size = new Size(1014, 24);
            txt_UnitTemplateID.TabIndex = 1;
            txt_UnitTemplateID.Text = "11001";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.FromArgb(242, 242, 242);
            label8.CustomBackColor = null;
            label8.CustomForeColor = null;
            label8.Depth = 0;
            label8.Dock = DockStyle.Fill;
            label8.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label8.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label8.Location = new Point(3, 284);
            label8.MouseState = MaterialSkin.MouseState.HOVER;
            label8.Name = "label8";
            label8.RightToLeft = RightToLeft.Yes;
            label8.Size = new Size(121, 32);
            label8.TabIndex = 17;
            label8.Text = "UnitTemplateID";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txt_Force
            // 
            txt_Force.BackColor = Color.FromArgb(255, 255, 255);
            txt_Force.BorderStyle = BorderStyle.None;
            txt_Force.CustomBackColor = null;
            txt_Force.CustomForeColor = null;
            txt_Force.Dock = DockStyle.Left;
            txt_Force.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txt_Force.ForeColor = Color.FromArgb(222, 0, 0, 0);
            txt_Force.Location = new Point(130, 320);
            txt_Force.Margin = new Padding(3, 4, 3, 4);
            txt_Force.MaxLength = 50;
            txt_Force.Multiline = false;
            txt_Force.Name = "txt_Force";
            txt_Force.Size = new Size(1014, 24);
            txt_Force.TabIndex = 20;
            txt_Force.Text = "0";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.FromArgb(242, 242, 242);
            label9.CustomBackColor = null;
            label9.CustomForeColor = null;
            label9.Depth = 0;
            label9.Dock = DockStyle.Fill;
            label9.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label9.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label9.Location = new Point(3, 316);
            label9.MouseState = MaterialSkin.MouseState.HOVER;
            label9.Name = "label9";
            label9.RightToLeft = RightToLeft.Yes;
            label9.Size = new Size(121, 32);
            label9.TabIndex = 19;
            label9.Text = "Force";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txt_SceneID
            // 
            txt_SceneID.BackColor = Color.FromArgb(255, 255, 255);
            txt_SceneID.BorderStyle = BorderStyle.None;
            txt_SceneID.CustomBackColor = null;
            txt_SceneID.CustomForeColor = null;
            txt_SceneID.Dock = DockStyle.Left;
            txt_SceneID.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txt_SceneID.ForeColor = Color.FromArgb(222, 0, 0, 0);
            txt_SceneID.Location = new Point(130, 100);
            txt_SceneID.Margin = new Padding(3, 4, 3, 4);
            txt_SceneID.MaxLength = 50;
            txt_SceneID.Multiline = false;
            txt_SceneID.Name = "txt_SceneID";
            txt_SceneID.Size = new Size(1014, 24);
            txt_SceneID.TabIndex = 22;
            txt_SceneID.Text = "0";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.FromArgb(242, 242, 242);
            label10.CustomBackColor = null;
            label10.CustomForeColor = null;
            label10.Depth = 0;
            label10.Dock = DockStyle.Fill;
            label10.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label10.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label10.Location = new Point(3, 96);
            label10.MouseState = MaterialSkin.MouseState.HOVER;
            label10.Name = "label10";
            label10.RightToLeft = RightToLeft.Yes;
            label10.Size = new Size(121, 32);
            label10.TabIndex = 21;
            label10.Text = "SceneID";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // chk_IsProxy
            // 
            chk_IsProxy.AutoSize = true;
            chk_IsProxy.BackColor = Color.FromArgb(242, 242, 242);
            chk_IsProxy.CustomBackColor = null;
            chk_IsProxy.CustomForeColor = null;
            chk_IsProxy.Depth = 0;
            chk_IsProxy.Dock = DockStyle.Left;
            chk_IsProxy.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            chk_IsProxy.ForeColor = Color.FromArgb(222, 0, 0, 0);
            chk_IsProxy.Location = new Point(127, 348);
            chk_IsProxy.Margin = new Padding(0);
            chk_IsProxy.MouseLocation = new Point(-1, -1);
            chk_IsProxy.MouseState = MaterialSkin.MouseState.HOVER;
            chk_IsProxy.Name = "chk_IsProxy";
            chk_IsProxy.ReadOnly = false;
            chk_IsProxy.Ripple = true;
            chk_IsProxy.Size = new Size(163, 37);
            chk_IsProxy.TabIndex = 23;
            chk_IsProxy.Text = "代理模式（网关）";
            chk_IsProxy.UseVisualStyleBackColor = false;
            chk_IsProxy.CheckedChanged += Chk_IsProxy_CheckedChanged;
            // 
            // txt_ProxyConnectString
            // 
            txt_ProxyConnectString.BackColor = Color.FromArgb(255, 255, 255);
            txt_ProxyConnectString.BorderStyle = BorderStyle.None;
            txt_ProxyConnectString.CustomBackColor = null;
            txt_ProxyConnectString.CustomForeColor = null;
            txt_ProxyConnectString.Dock = DockStyle.Left;
            txt_ProxyConnectString.Enabled = false;
            txt_ProxyConnectString.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txt_ProxyConnectString.ForeColor = Color.FromArgb(222, 0, 0, 0);
            txt_ProxyConnectString.Location = new Point(130, 389);
            txt_ProxyConnectString.Margin = new Padding(3, 4, 3, 4);
            txt_ProxyConnectString.MaxLength = 50;
            txt_ProxyConnectString.Multiline = false;
            txt_ProxyConnectString.Name = "txt_ProxyConnectString";
            txt_ProxyConnectString.Size = new Size(1014, 24);
            txt_ProxyConnectString.TabIndex = 24;
            txt_ProxyConnectString.Text = "192.168.1.81:18888";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.FromArgb(242, 242, 242);
            label11.CustomBackColor = null;
            label11.CustomForeColor = null;
            label11.Depth = 0;
            label11.Dock = DockStyle.Fill;
            label11.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label11.ForeColor = Color.FromArgb(222, 0, 0, 0);
            label11.Location = new Point(3, 385);
            label11.MouseState = MaterialSkin.MouseState.HOVER;
            label11.Name = "label11";
            label11.RightToLeft = RightToLeft.Yes;
            label11.Size = new Size(121, 32);
            label11.TabIndex = 25;
            label11.Text = "网关地址";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txt_RoomID
            // 
            txt_RoomID.BackColor = Color.FromArgb(255, 255, 255);
            txt_RoomID.BorderStyle = BorderStyle.None;
            txt_RoomID.CustomBackColor = null;
            txt_RoomID.CustomForeColor = null;
            txt_RoomID.Dock = DockStyle.Left;
            txt_RoomID.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txt_RoomID.ForeColor = Color.FromArgb(222, 0, 0, 0);
            txt_RoomID.Location = new Point(130, 68);
            txt_RoomID.Margin = new Padding(3, 4, 3, 4);
            txt_RoomID.MaxLength = 50;
            txt_RoomID.Multiline = false;
            txt_RoomID.Name = "txt_RoomID";
            txt_RoomID.Size = new Size(1014, 24);
            txt_RoomID.TabIndex = 26;
            txt_RoomID.Text = "房间ID";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(242, 242, 242);
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(btn_Connect, 1, 12);
            tableLayoutPanel1.Controls.Add(txt_ProxyConnectString, 1, 11);
            tableLayoutPanel1.Controls.Add(label11, 0, 11);
            tableLayoutPanel1.Controls.Add(txt_RoomID, 1, 2);
            tableLayoutPanel1.Controls.Add(txt_GameDataRoot, 1, 0);
            tableLayoutPanel1.Controls.Add(chk_IsProxy, 1, 10);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(txt_Force, 1, 9);
            tableLayoutPanel1.Controls.Add(txt_SceneID, 1, 3);
            tableLayoutPanel1.Controls.Add(label9, 0, 9);
            tableLayoutPanel1.Controls.Add(txt_PlayerUUID, 1, 1);
            tableLayoutPanel1.Controls.Add(txt_UnitTemplateID, 1, 8);
            tableLayoutPanel1.Controls.Add(label10, 0, 3);
            tableLayoutPanel1.Controls.Add(label8, 0, 8);
            tableLayoutPanel1.Controls.Add(label3, 0, 2);
            tableLayoutPanel1.Controls.Add(num_SyncRange, 1, 7);
            tableLayoutPanel1.Controls.Add(label7, 0, 7);
            tableLayoutPanel1.Controls.Add(label4, 0, 4);
            tableLayoutPanel1.Controls.Add(label5, 0, 5);
            tableLayoutPanel1.Controls.Add(label6, 0, 6);
            tableLayoutPanel1.Controls.Add(txt_ConnectString, 1, 5);
            tableLayoutPanel1.Controls.Add(num_IntervalMS, 1, 6);
            tableLayoutPanel1.Controls.Add(txt_NetDirver, 1, 4);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            tableLayoutPanel1.ForeColor = Color.FromArgb(222, 0, 0, 0);
            tableLayoutPanel1.Location = new Point(2, 23);
            tableLayoutPanel1.Margin = new Padding(2, 3, 2, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 13;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(908, 644);
            tableLayoutPanel1.TabIndex = 27;
            // 
            // FormLauncher
            // 
            AcceptButton = btn_Connect;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(912, 670);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(17, 567);
            Name = "FormLauncher";
            Padding = new Padding(2, 23, 2, 3);
            Text = "FormLauncher";
            FormClosed += FormLauncher_FormClosed;
            Load += FormLauncher_Load;
            Shown += FormLauncher_Shown;
            ((System.ComponentModel.ISupportInitialize)num_IntervalMS).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_SyncRange).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DeepEditor.Common.G2D.G2DBaseLabel label1;
        private DeepEditor.Common.G2D.G2DBaseRichTextBox txt_GameDataRoot;
        private DeepEditor.Common.G2D.G2DBaseLabel label2;
        private DeepEditor.Common.G2D.G2DBaseRichTextBox txt_PlayerUUID;
        private DeepEditor.Common.G2D.G2DBaseLabel label3;
        private DeepEditor.Common.G2D.G2DBaseLabel label4;
        private DeepEditor.Common.G2D.G2DBaseLabel label5;
        private DeepEditor.Common.G2D.G2DBaseRichTextBox txt_ConnectString;
        private NumericUpDown num_IntervalMS;
        private DeepEditor.Common.G2D.G2DBaseLabel label6;
        private NumericUpDown num_SyncRange;
        private DeepEditor.Common.G2D.G2DBaseLabel label7;
        private DeepEditor.Common.G2D.G2DBaseButton btn_Connect;
        private DeepEditor.Common.G2D.G2DBaseComboBox txt_NetDirver;
        private DeepEditor.Common.G2D.G2DBaseRichTextBox txt_UnitTemplateID;
        private DeepEditor.Common.G2D.G2DBaseLabel label8;
        private DeepEditor.Common.G2D.G2DBaseRichTextBox txt_Force;
        private DeepEditor.Common.G2D.G2DBaseLabel label9;
        private DeepEditor.Common.G2D.G2DBaseRichTextBox txt_SceneID;
        private DeepEditor.Common.G2D.G2DBaseLabel label10;
        private System.Windows.Forms.Timer timer1;
        private DeepEditor.Common.G2D.G2DBaseCheckBox chk_IsProxy;
        private DeepEditor.Common.G2D.G2DBaseRichTextBox txt_ProxyConnectString;
        private DeepEditor.Common.G2D.G2DBaseLabel label11;
        private DeepEditor.Common.G2D.G2DBaseRichTextBox txt_RoomID;
        private TableLayoutPanel tableLayoutPanel1;
    }
}