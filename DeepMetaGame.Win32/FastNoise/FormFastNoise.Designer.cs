namespace DeepMetaGame.Win32.FastNoise
{
    partial class FormFastNoise
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
            splitContainer1 = new SplitContainer();
            tabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            tabPage1 = new TabPage();
            pictureBox1 = new PictureBox();
            tabPage2 = new TabPage();
            glControl1 = new OpenTK.WinForms.GLControl();
            materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            splitContainer2 = new SplitContainer();
            g2dPropertyGrid1 = new DeepEditor.Common.G2D.DataGrid.G2DPropertyGrid();
            btn_Gen = new DeepEditor.Common.G2D.G2DBaseButton();
            timer1 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = Color.FromArgb(50, 50, 50);
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            splitContainer1.ForeColor = Color.FromArgb(255, 255, 255);
            splitContainer1.Location = new Point(6, 44);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = Color.FromArgb(50, 50, 50);
            splitContainer1.Panel1.Controls.Add(tabControl1);
            splitContainer1.Panel1.Controls.Add(materialTabSelector1);
            splitContainer1.Panel1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            splitContainer1.Panel1.ForeColor = Color.FromArgb(255, 255, 255);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = Color.FromArgb(50, 50, 50);
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Panel2.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            splitContainer1.Panel2.ForeColor = Color.FromArgb(255, 255, 255);
            splitContainer1.Size = new Size(1571, 1125);
            splitContainer1.SplitterDistance = 727;
            splitContainer1.SplitterWidth = 8;
            splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Depth = 0;
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.ForeColor = Color.FromArgb(255, 255, 255);
            tabControl1.Location = new Point(0, 50);
            tabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(727, 1075);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.FromArgb(50, 50, 50);
            tabPage1.Controls.Add(pictureBox1);
            tabPage1.ForeColor = Color.FromArgb(255, 255, 255);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(719, 1042);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Texture2D";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(50, 50, 50);
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            pictureBox1.ForeColor = Color.FromArgb(255, 255, 255);
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(713, 1036);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.FromArgb(50, 50, 50);
            tabPage2.Controls.Add(glControl1);
            tabPage2.ForeColor = Color.FromArgb(255, 255, 255);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(719, 1042);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "View3D";
            // 
            // glControl1
            // 
            glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl1.APIVersion = new Version(3, 3, 0, 0);
            glControl1.BackColor = Color.FromArgb(50, 50, 50);
            glControl1.Dock = DockStyle.Fill;
            glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            glControl1.ForeColor = Color.FromArgb(255, 255, 255);
            glControl1.IsDesignRender = false;
            glControl1.IsEventDriven = true;
            glControl1.Location = new Point(3, 3);
            glControl1.Name = "glControl1";
            glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            glControl1.Size = new Size(713, 1036);
            glControl1.TabIndex = 0;
            glControl1.Text = "glControl1";
            // 
            // materialTabSelector1
            // 
            materialTabSelector1.BackColor = Color.FromArgb(50, 50, 50);
            materialTabSelector1.BaseTabControl = tabControl1;
            materialTabSelector1.CharacterCasing = MaterialSkin.Controls.MaterialTabSelector.CustomCharacterCasing.Normal;
            materialTabSelector1.Depth = 0;
            materialTabSelector1.Dock = DockStyle.Top;
            materialTabSelector1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialTabSelector1.ForeColor = Color.FromArgb(255, 255, 255);
            materialTabSelector1.Location = new Point(0, 0);
            materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            materialTabSelector1.Name = "materialTabSelector1";
            materialTabSelector1.Size = new Size(727, 50);
            materialTabSelector1.TabIndex = 2;
            materialTabSelector1.Text = "materialTabSelector1";
            // 
            // splitContainer2
            // 
            splitContainer2.BackColor = Color.FromArgb(50, 50, 50);
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.FixedPanel = FixedPanel.Panel2;
            splitContainer2.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            splitContainer2.ForeColor = Color.FromArgb(255, 255, 255);
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.BackColor = Color.FromArgb(50, 50, 50);
            splitContainer2.Panel1.Controls.Add(g2dPropertyGrid1);
            splitContainer2.Panel1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            splitContainer2.Panel1.ForeColor = Color.FromArgb(255, 255, 255);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.BackColor = Color.FromArgb(50, 50, 50);
            splitContainer2.Panel2.Controls.Add(btn_Gen);
            splitContainer2.Panel2.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            splitContainer2.Panel2.ForeColor = Color.FromArgb(255, 255, 255);
            splitContainer2.Size = new Size(836, 1125);
            splitContainer2.SplitterDistance = 990;
            splitContainer2.SplitterWidth = 8;
            splitContainer2.TabIndex = 0;
            // 
            // g2dPropertyGrid1
            // 
            g2dPropertyGrid1.BackColor = Color.FromArgb(50, 50, 50);
            g2dPropertyGrid1.CategoryForeColor = Color.FromArgb(255, 255, 255);
            g2dPropertyGrid1.CategorySplitterColor = Color.FromArgb(30, 255, 255, 255);
            g2dPropertyGrid1.CommandsBackColor = Color.FromArgb(50, 50, 50);
            g2dPropertyGrid1.CommandsBorderColor = Color.FromArgb(50, 50, 50);
            g2dPropertyGrid1.CommandsForeColor = Color.FromArgb(255, 255, 255);
            g2dPropertyGrid1.CustomBackColor = null;
            g2dPropertyGrid1.CustomForeColor = null;
            g2dPropertyGrid1.DescriptionAreaHeight = 88;
            g2dPropertyGrid1.DescriptionAreaLineCount = 4;
            g2dPropertyGrid1.DisabledItemForeColor = Color.Gray;
            g2dPropertyGrid1.Dock = DockStyle.Fill;
            g2dPropertyGrid1.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            g2dPropertyGrid1.ForeColor = Color.FromArgb(255, 255, 255);
            g2dPropertyGrid1.HelpBackColor = Color.FromArgb(50, 50, 50);
            g2dPropertyGrid1.HelpBorderColor = Color.FromArgb(30, 255, 255, 255);
            g2dPropertyGrid1.HelpForeColor = Color.FromArgb(255, 255, 255);
            g2dPropertyGrid1.ImeMode = ImeMode.NoControl;
            g2dPropertyGrid1.LineColor = Color.FromArgb(80, 80, 80);
            g2dPropertyGrid1.Location = new Point(0, 0);
            g2dPropertyGrid1.MinDescriptionAreaLineCount = 5;
            g2dPropertyGrid1.Name = "g2dPropertyGrid1";
            g2dPropertyGrid1.SelectedElementDesc = null;
            g2dPropertyGrid1.SelectedField = null;
            g2dPropertyGrid1.SelectedFieldDesc = null;
            g2dPropertyGrid1.SelectedRootObject = null;
            g2dPropertyGrid1.Size = new Size(836, 990);
            g2dPropertyGrid1.TabIndex = 0;
            g2dPropertyGrid1.ViewBackColor = Color.FromArgb(50, 50, 50);
            g2dPropertyGrid1.ViewBorderColor = Color.FromArgb(50, 50, 50);
            g2dPropertyGrid1.ViewForeColor = Color.FromArgb(255, 255, 255);
            // 
            // btn_Gen
            // 
            btn_Gen.AutoSize = false;
            btn_Gen.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btn_Gen.BackColor = Color.FromArgb(50, 50, 50);
            btn_Gen.CustomBackColor = null;
            btn_Gen.CustomForeColor = null;
            btn_Gen.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btn_Gen.Depth = 0;
            btn_Gen.Dock = DockStyle.Fill;
            btn_Gen.Font = new Font("Microsoft YaHei UI", 9F);
            btn_Gen.ForeColor = Color.FromArgb(255, 255, 255);
            btn_Gen.HighEmphasis = true;
            btn_Gen.Icon = null;
            btn_Gen.Location = new Point(0, 0);
            btn_Gen.Margin = new Padding(4, 6, 4, 6);
            btn_Gen.MouseState = MaterialSkin.MouseState.HOVER;
            btn_Gen.Name = "btn_Gen";
            btn_Gen.NoAccentTextColor = Color.Empty;
            btn_Gen.Size = new Size(836, 127);
            btn_Gen.TabIndex = 0;
            btn_Gen.Text = "Generate";
            btn_Gen.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btn_Gen.UseAccentColor = false;
            btn_Gen.UseVisualStyleBackColor = false;
            btn_Gen.Click += btn_Gen_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 33;
            timer1.Tick += timer1_Tick;
            // 
            // FormFastNoise
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1583, 1175);
            Controls.Add(splitContainer1);
            Name = "FormFastNoise";
            Text = "FormFastNoise";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabPage2.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private PictureBox pictureBox1;
        private DeepEditor.Common.G2D.DataGrid.G2DPropertyGrid g2dPropertyGrid1;
        private DeepEditor.Common.G2D.G2DBaseButton btn_Gen;
        private MaterialSkin.Controls.MaterialTabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private OpenTK.WinForms.GLControl glControl1;
        private System.Windows.Forms.Timer timer1;
    }
}