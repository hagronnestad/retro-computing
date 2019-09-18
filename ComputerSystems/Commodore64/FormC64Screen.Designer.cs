namespace ComputerSystem.Commodore64 {
    partial class FormC64Screen {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReset = new System.Windows.Forms.ToolStripMenuItem();
            this.pScreen = new System.Windows.Forms.PictureBox();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSystem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(656, 24);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuSystem
            // 
            this.mnuSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuReset});
            this.mnuSystem.Name = "mnuSystem";
            this.mnuSystem.Size = new System.Drawing.Size(57, 20);
            this.mnuSystem.Text = "System";
            // 
            // mnuReset
            // 
            this.mnuReset.Name = "mnuReset";
            this.mnuReset.Size = new System.Drawing.Size(180, 22);
            this.mnuReset.Text = "Reset";
            this.mnuReset.Click += new System.EventHandler(this.MnuReset_Click);
            // 
            // pScreen
            // 
            this.pScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pScreen.Location = new System.Drawing.Point(0, 24);
            this.pScreen.Name = "pScreen";
            this.pScreen.Size = new System.Drawing.Size(656, 441);
            this.pScreen.TabIndex = 1;
            this.pScreen.TabStop = false;
            this.pScreen.Paint += new System.Windows.Forms.PaintEventHandler(this.PScreen_Paint);
            // 
            // FormC64Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 465);
            this.Controls.Add(this.pScreen);
            this.Controls.Add(this.mnuMain);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "FormC64Screen";
            this.Text = "FormC64Screen";
            this.Load += new System.EventHandler(this.FormC64Screen_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormC64Screen_KeyDown);
            this.Resize += new System.EventHandler(this.FormC64Screen_Resize);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pScreen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuSystem;
        private System.Windows.Forms.ToolStripMenuItem mnuReset;
        private System.Windows.Forms.PictureBox pScreen;
    }
}