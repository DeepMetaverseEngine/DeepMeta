namespace DeepEditor.Plugin3D.BattleClient
{
    partial class FormBattleView3D
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
            panel = new PanelBattleView3D();
            SuspendLayout();
            // 
            // panel
            // 
            panel.BackColor = Color.FromArgb(242, 242, 242);
            panel.Dock = DockStyle.Fill;
            panel.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            panel.ForeColor = Color.FromArgb(0, 0, 0);
            panel.Margin = new Padding(3, 4, 3, 4);
            panel.Name = "panel";
            panel.Size = new Size(1631, 980);
            panel.TabIndex = 0;
            // 
            // FormBattleView3D
            // 
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1643, 1026);
            Controls.Add(panel);
            Name = "FormBattleView3D";
            Text = "FormBattleView3D";
            ResumeLayout(false);
        }

        #endregion

        private DeepEditor.Plugin3D.BattleClient.PanelBattleView3D panel;
    }
}