namespace DeepTools.MPQUpdate
{
    partial class FormUpdater
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("ROOT", new System.Windows.Forms.TreeNode[] { treeNode1 });
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdater));
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("/");
            menu_MPQTreeNode = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripMenuItem_RefreshMPQ = new System.Windows.Forms.ToolStripMenuItem();
            button_Start = new System.Windows.Forms.Button();
            button_Stop = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            textBox_BundleDir = new System.Windows.Forms.TextBox();
            progressBar_Download = new System.Windows.Forms.ProgressBar();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            progressBar_Decompress = new System.Windows.Forms.ProgressBar();
            timer1 = new System.Windows.Forms.Timer(components);
            label_Download = new System.Windows.Forms.Label();
            label_Unzip = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            comboBox_zipType = new System.Windows.Forms.ComboBox();
            comboBox_mpqType = new System.Windows.Forms.ComboBox();
            textBox_SaveRoot = new System.Windows.Forms.TextBox();
            comboBox_RemoteUrl = new System.Windows.Forms.ComboBox();
            comboBox_RemoteDir = new System.Windows.Forms.ComboBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            chk_DoNotDownloadZip = new System.Windows.Forms.CheckBox();
            chk_DoNotUnzip = new System.Windows.Forms.CheckBox();
            button_clear = new System.Windows.Forms.Button();
            progressBar_Running = new System.Windows.Forms.ProgressBar();
            groupBox3 = new System.Windows.Forms.GroupBox();
            split_Tree = new System.Windows.Forms.SplitContainer();
            treeView_MPQ = new System.Windows.Forms.TreeView();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButton_ViewReplaced = new System.Windows.Forms.ToolStripButton();
            groupBox5 = new System.Windows.Forms.GroupBox();
            split_Info = new System.Windows.Forms.SplitContainer();
            property_MPQInfo = new System.Windows.Forms.PropertyGrid();
            textBox_EntryInfo = new System.Windows.Forms.RichTextBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            textBox_VersionText = new System.Windows.Forms.RichTextBox();
            tabPage3 = new System.Windows.Forms.TabPage();
            treeView_FS = new System.Windows.Forms.TreeView();
            toolStrip2 = new System.Windows.Forms.ToolStrip();
            tabPage2 = new System.Windows.Forms.TabPage();
            menu_EntryNode = new System.Windows.Forms.ContextMenuStrip(components);
            导出文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            imageList1 = new System.Windows.Forms.ImageList(components);
            menu_MPQTreeNode.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)split_Tree).BeginInit();
            split_Tree.Panel1.SuspendLayout();
            split_Tree.Panel2.SuspendLayout();
            split_Tree.SuspendLayout();
            toolStrip1.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)split_Info).BeginInit();
            split_Info.Panel1.SuspendLayout();
            split_Info.Panel2.SuspendLayout();
            split_Info.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage2.SuspendLayout();
            menu_EntryNode.SuspendLayout();
            SuspendLayout();
            // 
            // menu_MPQTreeNode
            // 
            menu_MPQTreeNode.ImageScalingSize = new System.Drawing.Size(24, 24);
            menu_MPQTreeNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem_RefreshMPQ });
            menu_MPQTreeNode.Name = "contextMenuStrip1";
            menu_MPQTreeNode.Size = new System.Drawing.Size(117, 34);
            // 
            // toolStripMenuItem_RefreshMPQ
            // 
            toolStripMenuItem_RefreshMPQ.Name = "toolStripMenuItem_RefreshMPQ";
            toolStripMenuItem_RefreshMPQ.Size = new System.Drawing.Size(116, 30);
            toolStripMenuItem_RefreshMPQ.Text = "刷新";
            toolStripMenuItem_RefreshMPQ.Click += toolStripMenuItem_RefreshMPQ_Click;
            // 
            // button_Start
            // 
            button_Start.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button_Start.Location = new System.Drawing.Point(884, 280);
            button_Start.Margin = new System.Windows.Forms.Padding(6);
            button_Start.Name = "button_Start";
            button_Start.Size = new System.Drawing.Size(138, 47);
            button_Start.TabIndex = 0;
            button_Start.Text = "开始";
            button_Start.UseVisualStyleBackColor = true;
            button_Start.Click += button_Start_Click;
            // 
            // button_Stop
            // 
            button_Stop.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button_Stop.Location = new System.Drawing.Point(1033, 280);
            button_Stop.Margin = new System.Windows.Forms.Padding(6);
            button_Stop.Name = "button_Stop";
            button_Stop.Size = new System.Drawing.Size(138, 47);
            button_Stop.TabIndex = 1;
            button_Stop.Text = "中止";
            button_Stop.UseVisualStyleBackColor = true;
            button_Stop.Click += button_Stop_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(16, 42);
            label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(118, 24);
            label1.TabIndex = 3;
            label1.Text = "远程目录地址";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(16, 96);
            label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(118, 24);
            label2.TabIndex = 4;
            label2.Text = "更新配置文件";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(16, 150);
            label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(118, 24);
            label3.TabIndex = 5;
            label3.Text = "本地存储地址";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(16, 203);
            label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(118, 24);
            label4.TabIndex = 6;
            label4.Text = "本地资源地址";
            // 
            // textBox_BundleDir
            // 
            textBox_BundleDir.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox_BundleDir.Location = new System.Drawing.Point(168, 198);
            textBox_BundleDir.Margin = new System.Windows.Forms.Padding(6);
            textBox_BundleDir.Name = "textBox_BundleDir";
            textBox_BundleDir.Size = new System.Drawing.Size(1132, 30);
            textBox_BundleDir.TabIndex = 9;
            textBox_BundleDir.Text = "./res";
            // 
            // progressBar_Download
            // 
            progressBar_Download.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar_Download.Location = new System.Drawing.Point(14, 72);
            progressBar_Download.Margin = new System.Windows.Forms.Padding(6);
            progressBar_Download.Name = "progressBar_Download";
            progressBar_Download.Size = new System.Drawing.Size(1313, 16);
            progressBar_Download.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            progressBar_Download.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(11, 42);
            label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(82, 24);
            label5.TabIndex = 11;
            label5.Text = "下载进度";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(11, 144);
            label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(82, 24);
            label6.TabIndex = 13;
            label6.Text = "解压进度";
            // 
            // progressBar_Decompress
            // 
            progressBar_Decompress.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar_Decompress.Location = new System.Drawing.Point(14, 174);
            progressBar_Decompress.Margin = new System.Windows.Forms.Padding(6);
            progressBar_Decompress.Name = "progressBar_Decompress";
            progressBar_Decompress.Size = new System.Drawing.Size(1313, 16);
            progressBar_Decompress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            progressBar_Decompress.TabIndex = 12;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
            // 
            // label_Download
            // 
            label_Download.AutoSize = true;
            label_Download.Location = new System.Drawing.Point(11, 95);
            label_Download.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label_Download.Name = "label_Download";
            label_Download.Size = new System.Drawing.Size(61, 24);
            label_Download.TabIndex = 14;
            label_Download.Text = "status";
            // 
            // label_Unzip
            // 
            label_Unzip.AutoSize = true;
            label_Unzip.Location = new System.Drawing.Point(11, 196);
            label_Unzip.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label_Unzip.Name = "label_Unzip";
            label_Unzip.Size = new System.Drawing.Size(61, 24);
            label_Unzip.TabIndex = 15;
            label_Unzip.Text = "status";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(comboBox_zipType);
            groupBox1.Controls.Add(comboBox_mpqType);
            groupBox1.Controls.Add(textBox_SaveRoot);
            groupBox1.Controls.Add(comboBox_RemoteUrl);
            groupBox1.Controls.Add(comboBox_RemoteDir);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(textBox_BundleDir);
            groupBox1.Location = new System.Drawing.Point(6, 12);
            groupBox1.Margin = new System.Windows.Forms.Padding(6);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(6);
            groupBox1.Size = new System.Drawing.Size(1340, 374);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "参数";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(38, 268);
            label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(100, 24);
            label7.TabIndex = 15;
            label7.Text = "包文件类型";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(16, 320);
            label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(118, 24);
            label8.TabIndex = 16;
            label8.Text = "压缩文件类型";
            // 
            // comboBox_zipType
            // 
            comboBox_zipType.FormattingEnabled = true;
            comboBox_zipType.Items.AddRange(new object[] { ".zip", ".mgz", ".z", ".gz" });
            comboBox_zipType.Location = new System.Drawing.Point(168, 313);
            comboBox_zipType.Margin = new System.Windows.Forms.Padding(6);
            comboBox_zipType.Name = "comboBox_zipType";
            comboBox_zipType.Size = new System.Drawing.Size(218, 32);
            comboBox_zipType.TabIndex = 14;
            comboBox_zipType.Text = ".zip";
            // 
            // comboBox_mpqType
            // 
            comboBox_mpqType.FormattingEnabled = true;
            comboBox_mpqType.Items.AddRange(new object[] { ".mpq" });
            comboBox_mpqType.Location = new System.Drawing.Point(168, 263);
            comboBox_mpqType.Margin = new System.Windows.Forms.Padding(6);
            comboBox_mpqType.Name = "comboBox_mpqType";
            comboBox_mpqType.Size = new System.Drawing.Size(218, 32);
            comboBox_mpqType.TabIndex = 13;
            comboBox_mpqType.Text = ".mpq";
            // 
            // textBox_SaveRoot
            // 
            textBox_SaveRoot.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox_SaveRoot.Location = new System.Drawing.Point(168, 144);
            textBox_SaveRoot.Margin = new System.Windows.Forms.Padding(6);
            textBox_SaveRoot.Name = "textBox_SaveRoot";
            textBox_SaveRoot.Size = new System.Drawing.Size(1132, 30);
            textBox_SaveRoot.TabIndex = 12;
            textBox_SaveRoot.Text = "./http_res";
            // 
            // comboBox_RemoteUrl
            // 
            comboBox_RemoteUrl.AllowDrop = true;
            comboBox_RemoteUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBox_RemoteUrl.FormattingEnabled = true;
            comboBox_RemoteUrl.Location = new System.Drawing.Point(168, 90);
            comboBox_RemoteUrl.Margin = new System.Windows.Forms.Padding(6);
            comboBox_RemoteUrl.Name = "comboBox_RemoteUrl";
            comboBox_RemoteUrl.Size = new System.Drawing.Size(1132, 32);
            comboBox_RemoteUrl.TabIndex = 11;
            comboBox_RemoteUrl.Text = "http://192.168.1.101/morefun/res_expantion/updates__pvr_m3z/update_version.txt";
            // 
            // comboBox_RemoteDir
            // 
            comboBox_RemoteDir.AllowDrop = true;
            comboBox_RemoteDir.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBox_RemoteDir.FormattingEnabled = true;
            comboBox_RemoteDir.Location = new System.Drawing.Point(168, 37);
            comboBox_RemoteDir.Margin = new System.Windows.Forms.Padding(6);
            comboBox_RemoteDir.Name = "comboBox_RemoteDir";
            comboBox_RemoteDir.Size = new System.Drawing.Size(1132, 32);
            comboBox_RemoteDir.TabIndex = 10;
            comboBox_RemoteDir.Text = "http://192.168.1.101/morefun/res_expantion";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox2.Controls.Add(chk_DoNotDownloadZip);
            groupBox2.Controls.Add(chk_DoNotUnzip);
            groupBox2.Controls.Add(button_clear);
            groupBox2.Controls.Add(progressBar_Running);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(button_Stop);
            groupBox2.Controls.Add(progressBar_Download);
            groupBox2.Controls.Add(label_Unzip);
            groupBox2.Controls.Add(button_Start);
            groupBox2.Controls.Add(progressBar_Decompress);
            groupBox2.Controls.Add(label_Download);
            groupBox2.Controls.Add(label6);
            groupBox2.Location = new System.Drawing.Point(6, 398);
            groupBox2.Margin = new System.Windows.Forms.Padding(6);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(6);
            groupBox2.Size = new System.Drawing.Size(1340, 359);
            groupBox2.TabIndex = 17;
            groupBox2.TabStop = false;
            groupBox2.Text = "自动更新";
            // 
            // chk_DoNotDownloadZip
            // 
            chk_DoNotDownloadZip.AutoSize = true;
            chk_DoNotDownloadZip.Location = new System.Drawing.Point(140, 294);
            chk_DoNotDownloadZip.Margin = new System.Windows.Forms.Padding(6);
            chk_DoNotDownloadZip.Name = "chk_DoNotDownloadZip";
            chk_DoNotDownloadZip.Size = new System.Drawing.Size(126, 28);
            chk_DoNotDownloadZip.TabIndex = 19;
            chk_DoNotDownloadZip.Text = "不下载压缩";
            chk_DoNotDownloadZip.UseVisualStyleBackColor = true;
            // 
            // chk_DoNotUnzip
            // 
            chk_DoNotUnzip.AutoSize = true;
            chk_DoNotUnzip.Location = new System.Drawing.Point(20, 294);
            chk_DoNotUnzip.Margin = new System.Windows.Forms.Padding(6);
            chk_DoNotUnzip.Name = "chk_DoNotUnzip";
            chk_DoNotUnzip.Size = new System.Drawing.Size(108, 28);
            chk_DoNotUnzip.TabIndex = 18;
            chk_DoNotUnzip.Text = "不解压缩";
            chk_DoNotUnzip.UseVisualStyleBackColor = true;
            // 
            // button_clear
            // 
            button_clear.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button_clear.Location = new System.Drawing.Point(1181, 280);
            button_clear.Margin = new System.Windows.Forms.Padding(6);
            button_clear.Name = "button_clear";
            button_clear.Size = new System.Drawing.Size(138, 47);
            button_clear.TabIndex = 17;
            button_clear.Text = "清理";
            button_clear.UseVisualStyleBackColor = true;
            button_clear.Click += button_clear_Click;
            // 
            // progressBar_Running
            // 
            progressBar_Running.Enabled = false;
            progressBar_Running.Location = new System.Drawing.Point(14, 248);
            progressBar_Running.Margin = new System.Windows.Forms.Padding(6);
            progressBar_Running.Name = "progressBar_Running";
            progressBar_Running.Size = new System.Drawing.Size(286, 20);
            progressBar_Running.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            progressBar_Running.TabIndex = 16;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(split_Tree);
            groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox3.Location = new System.Drawing.Point(6, 6);
            groupBox3.Margin = new System.Windows.Forms.Padding(6);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(6);
            groupBox3.Size = new System.Drawing.Size(1346, 1446);
            groupBox3.TabIndex = 18;
            groupBox3.TabStop = false;
            groupBox3.Text = "MPQ文件系统";
            // 
            // split_Tree
            // 
            split_Tree.Dock = System.Windows.Forms.DockStyle.Fill;
            split_Tree.Location = new System.Drawing.Point(6, 29);
            split_Tree.Margin = new System.Windows.Forms.Padding(6);
            split_Tree.Name = "split_Tree";
            // 
            // split_Tree.Panel1
            // 
            split_Tree.Panel1.Controls.Add(treeView_MPQ);
            split_Tree.Panel1.Controls.Add(toolStrip1);
            // 
            // split_Tree.Panel2
            // 
            split_Tree.Panel2.Controls.Add(groupBox5);
            split_Tree.Size = new System.Drawing.Size(1334, 1411);
            split_Tree.SplitterDistance = 635;
            split_Tree.SplitterWidth = 8;
            split_Tree.TabIndex = 2;
            // 
            // treeView_MPQ
            // 
            treeView_MPQ.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView_MPQ.Location = new System.Drawing.Point(0, 33);
            treeView_MPQ.Margin = new System.Windows.Forms.Padding(6);
            treeView_MPQ.Name = "treeView_MPQ";
            treeNode1.Name = "节点1";
            treeNode1.Text = "节点1";
            treeNode2.ContextMenuStrip = menu_MPQTreeNode;
            treeNode2.Name = "节点0";
            treeNode2.Text = "ROOT";
            treeView_MPQ.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { treeNode2 });
            treeView_MPQ.Size = new System.Drawing.Size(635, 1378);
            treeView_MPQ.TabIndex = 1;
            treeView_MPQ.NodeMouseClick += treeView_MPQ_NodeMouseClick;
            treeView_MPQ.NodeMouseDoubleClick += treeView_MPQ_NodeMouseDoubleClick;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton_ViewReplaced });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            toolStrip1.Size = new System.Drawing.Size(635, 33);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_ViewReplaced
            // 
            toolStripButton_ViewReplaced.CheckOnClick = true;
            toolStripButton_ViewReplaced.Image = (System.Drawing.Image)resources.GetObject("toolStripButton_ViewReplaced.Image");
            toolStripButton_ViewReplaced.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButton_ViewReplaced.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton_ViewReplaced.Name = "toolStripButton_ViewReplaced";
            toolStripButton_ViewReplaced.Size = new System.Drawing.Size(100, 28);
            toolStripButton_ViewReplaced.Text = "显示冗余";
            toolStripButton_ViewReplaced.ToolTipText = "显示冗余";
            toolStripButton_ViewReplaced.Click += toolStripButton_ViewReplaced_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(split_Info);
            groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox5.Location = new System.Drawing.Point(0, 0);
            groupBox5.Margin = new System.Windows.Forms.Padding(6);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new System.Windows.Forms.Padding(6);
            groupBox5.Size = new System.Drawing.Size(691, 1411);
            groupBox5.TabIndex = 0;
            groupBox5.TabStop = false;
            groupBox5.Text = "文件信息";
            // 
            // split_Info
            // 
            split_Info.Dock = System.Windows.Forms.DockStyle.Fill;
            split_Info.Location = new System.Drawing.Point(6, 29);
            split_Info.Margin = new System.Windows.Forms.Padding(6);
            split_Info.Name = "split_Info";
            split_Info.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_Info.Panel1
            // 
            split_Info.Panel1.Controls.Add(property_MPQInfo);
            // 
            // split_Info.Panel2
            // 
            split_Info.Panel2.Controls.Add(textBox_EntryInfo);
            split_Info.Size = new System.Drawing.Size(679, 1376);
            split_Info.SplitterDistance = 1187;
            split_Info.SplitterWidth = 8;
            split_Info.TabIndex = 2;
            // 
            // property_MPQInfo
            // 
            property_MPQInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            property_MPQInfo.Location = new System.Drawing.Point(0, 0);
            property_MPQInfo.Margin = new System.Windows.Forms.Padding(6);
            property_MPQInfo.Name = "property_MPQInfo";
            property_MPQInfo.Size = new System.Drawing.Size(679, 1187);
            property_MPQInfo.TabIndex = 1;
            // 
            // textBox_EntryInfo
            // 
            textBox_EntryInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_EntryInfo.Location = new System.Drawing.Point(0, 0);
            textBox_EntryInfo.Margin = new System.Windows.Forms.Padding(6);
            textBox_EntryInfo.Name = "textBox_EntryInfo";
            textBox_EntryInfo.Size = new System.Drawing.Size(679, 181);
            textBox_EntryInfo.TabIndex = 0;
            textBox_EntryInfo.Text = "";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Margin = new System.Windows.Forms.Padding(6);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1366, 1495);
            tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(textBox_VersionText);
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Location = new System.Drawing.Point(4, 33);
            tabPage1.Margin = new System.Windows.Forms.Padding(6);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(6);
            tabPage1.Size = new System.Drawing.Size(1358, 1458);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "自动更新参数";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox_VersionText
            // 
            textBox_VersionText.AcceptsTab = true;
            textBox_VersionText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox_VersionText.BackColor = System.Drawing.SystemColors.Control;
            textBox_VersionText.ForeColor = System.Drawing.SystemColors.ControlText;
            textBox_VersionText.Location = new System.Drawing.Point(6, 769);
            textBox_VersionText.Margin = new System.Windows.Forms.Padding(6);
            textBox_VersionText.Name = "textBox_VersionText";
            textBox_VersionText.ReadOnly = true;
            textBox_VersionText.Size = new System.Drawing.Size(1340, 677);
            textBox_VersionText.TabIndex = 0;
            textBox_VersionText.Text = "";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(treeView_FS);
            tabPage3.Controls.Add(toolStrip2);
            tabPage3.Location = new System.Drawing.Point(4, 33);
            tabPage3.Margin = new System.Windows.Forms.Padding(6);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new System.Drawing.Size(1358, 1458);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "目录结构";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // treeView_FS
            // 
            treeView_FS.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView_FS.FullRowSelect = true;
            treeView_FS.HideSelection = false;
            treeView_FS.ImageIndex = 0;
            treeView_FS.ImageList = imageList1;
            treeView_FS.Location = new System.Drawing.Point(0, 25);
            treeView_FS.Margin = new System.Windows.Forms.Padding(6);
            treeView_FS.Name = "treeView_FS";
            treeNode3.Name = "ROOT_FS";
            treeNode3.Text = "/";
            treeView_FS.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { treeNode3 });
            treeView_FS.SelectedImageIndex = 0;
            treeView_FS.ShowNodeToolTips = true;
            treeView_FS.Size = new System.Drawing.Size(1358, 1433);
            treeView_FS.TabIndex = 0;
            // 
            // toolStrip2
            // 
            toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            toolStrip2.Location = new System.Drawing.Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            toolStrip2.Size = new System.Drawing.Size(1358, 25);
            toolStrip2.TabIndex = 1;
            toolStrip2.Text = "toolStrip2";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox3);
            tabPage2.Location = new System.Drawing.Point(4, 33);
            tabPage2.Margin = new System.Windows.Forms.Padding(6);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(6);
            tabPage2.Size = new System.Drawing.Size(1358, 1458);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "MPQ文件报告";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // menu_EntryNode
            // 
            menu_EntryNode.ImageScalingSize = new System.Drawing.Size(24, 24);
            menu_EntryNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 导出文件ToolStripMenuItem });
            menu_EntryNode.Name = "menu_EntryNode";
            menu_EntryNode.Size = new System.Drawing.Size(153, 34);
            // 
            // 导出文件ToolStripMenuItem
            // 
            导出文件ToolStripMenuItem.Name = "导出文件ToolStripMenuItem";
            导出文件ToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            导出文件ToolStripMenuItem.Text = "导出文件";
            导出文件ToolStripMenuItem.Click += 导出文件ToolStripMenuItem_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "folder_7795785.png");
            imageList1.Images.SetKeyName(1, "login_btn_agreement.png");
            // 
            // FormUpdater
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1366, 1495);
            Controls.Add(tabControl1);
            Margin = new System.Windows.Forms.Padding(6);
            Name = "FormUpdater";
            Text = "MPQ文件检测器";
            FormClosing += FormUpdater_FormClosing;
            menu_MPQTreeNode.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            split_Tree.Panel1.ResumeLayout(false);
            split_Tree.Panel1.PerformLayout();
            split_Tree.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split_Tree).EndInit();
            split_Tree.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            groupBox5.ResumeLayout(false);
            split_Info.Panel1.ResumeLayout(false);
            split_Info.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split_Info).EndInit();
            split_Info.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage2.ResumeLayout(false);
            menu_EntryNode.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_BundleDir;
        private System.Windows.Forms.ProgressBar progressBar_Download;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar_Decompress;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label_Download;
        private System.Windows.Forms.Label label_Unzip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer split_Tree;
        private System.Windows.Forms.TreeView treeView_MPQ;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RichTextBox textBox_EntryInfo;
        private System.Windows.Forms.ContextMenuStrip menu_MPQTreeNode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_RefreshMPQ;
        private System.Windows.Forms.SplitContainer split_Info;
        private System.Windows.Forms.PropertyGrid property_MPQInfo;
        private System.Windows.Forms.ComboBox comboBox_RemoteUrl;
        private System.Windows.Forms.ComboBox comboBox_RemoteDir;
        private System.Windows.Forms.TextBox textBox_SaveRoot;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_ViewReplaced;
        private System.Windows.Forms.RichTextBox textBox_VersionText;
        private System.Windows.Forms.ProgressBar progressBar_Running;
        private System.Windows.Forms.ContextMenuStrip menu_EntryNode;
        private System.Windows.Forms.ToolStripMenuItem 导出文件ToolStripMenuItem;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox_zipType;
        private System.Windows.Forms.ComboBox comboBox_mpqType;
        private System.Windows.Forms.CheckBox chk_DoNotUnzip;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TreeView treeView_FS;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.CheckBox chk_DoNotDownloadZip;
        private System.Windows.Forms.ImageList imageList1;
    }
}