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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormC64Screen));
            this.pScreen = new System.Windows.Forms.PictureBox();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblFps = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCycles = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblInstructions = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblKeyboardDisabled = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.btnRestart = new System.Windows.Forms.ToolStripButton();
            this.btnReset = new System.Windows.Forms.ToolStripButton();
            this.btnPause = new System.Windows.Forms.ToolStripButton();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUseCrtFilter = new System.Windows.Forms.ToolStripButton();
            this.separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCopyOutput = new System.Windows.Forms.ToolStripSplitButton();
            this.btnCopyRawOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.separator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDebugger = new System.Windows.Forms.ToolStripButton();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pScreen)).BeginInit();
            this.statusMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pScreen
            // 
            this.pScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pScreen.Location = new System.Drawing.Point(0, 25);
            this.pScreen.Margin = new System.Windows.Forms.Padding(0);
            this.pScreen.Name = "pScreen";
            this.pScreen.Size = new System.Drawing.Size(640, 400);
            this.pScreen.TabIndex = 1;
            this.pScreen.TabStop = false;
            this.pScreen.DragDrop += new System.Windows.Forms.DragEventHandler(this.pScreen_DragDropAsync);
            this.pScreen.DragEnter += new System.Windows.Forms.DragEventHandler(this.pScreen_DragEnter);
            this.pScreen.Paint += new System.Windows.Forms.PaintEventHandler(this.PScreen_Paint);
            this.pScreen.Resize += new System.EventHandler(this.PScreen_Resize);
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFps,
            this.lblCycles,
            this.lblInstructions,
            this.lblKeyboardDisabled});
            this.statusMain.Location = new System.Drawing.Point(0, 423);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(640, 24);
            this.statusMain.TabIndex = 2;
            this.statusMain.Text = "statusStrip1";
            // 
            // lblFps
            // 
            this.lblFps.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(36, 19);
            this.lblFps.Text = "0 fps";
            // 
            // lblCycles
            // 
            this.lblCycles.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblCycles.Name = "lblCycles";
            this.lblCycles.Size = new System.Drawing.Size(52, 19);
            this.lblCycles.Text = "0 cycles";
            // 
            // lblInstructions
            // 
            this.lblInstructions.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(82, 19);
            this.lblInstructions.Text = "0 instructions";
            // 
            // lblKeyboardDisabled
            // 
            this.lblKeyboardDisabled.ForeColor = System.Drawing.Color.Red;
            this.lblKeyboardDisabled.Name = "lblKeyboardDisabled";
            this.lblKeyboardDisabled.Size = new System.Drawing.Size(455, 19);
            this.lblKeyboardDisabled.Spring = true;
            this.lblKeyboardDisabled.Text = "Keyboard Disabled";
            this.lblKeyboardDisabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolMain
            // 
            this.toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRestart,
            this.btnReset,
            this.btnPause,
            this.separator1,
            this.btnOpen,
            this.btnSave,
            this.separator2,
            this.btnUseCrtFilter,
            this.separator3,
            this.btnCopyOutput,
            this.separator4,
            this.btnDebugger});
            this.toolMain.Location = new System.Drawing.Point(0, 0);
            this.toolMain.Name = "toolMain";
            this.toolMain.Size = new System.Drawing.Size(640, 25);
            this.toolMain.TabIndex = 0;
            this.toolMain.Text = "toolStrip1";
            // 
            // btnRestart
            // 
            this.btnRestart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRestart.Image = ((System.Drawing.Image)(resources.GetObject("btnRestart.Image")));
            this.btnRestart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(23, 22);
            this.btnRestart.Text = "Restart";
            this.btnRestart.Click += new System.EventHandler(this.BtnRestart_Click);
            // 
            // btnReset
            // 
            this.btnReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(23, 22);
            this.btnReset.Text = "Reset";
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnPause
            // 
            this.btnPause.CheckOnClick = true;
            this.btnPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
            this.btnPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(23, 22);
            this.btnPause.Text = "Pause";
            this.btnPause.Click += new System.EventHandler(this.BtnPause_ClickAsync);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 22);
            this.btnOpen.Text = "Open";
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUseCrtFilter
            // 
            this.btnUseCrtFilter.Checked = global::Commodore64.Properties.Settings.Default.ApplyCrtFilter;
            this.btnUseCrtFilter.CheckOnClick = true;
            this.btnUseCrtFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnUseCrtFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUseCrtFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnUseCrtFilter.Image")));
            this.btnUseCrtFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUseCrtFilter.Name = "btnUseCrtFilter";
            this.btnUseCrtFilter.Size = new System.Drawing.Size(23, 22);
            this.btnUseCrtFilter.Text = "CRT filter";
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            this.separator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCopyOutput
            // 
            this.btnCopyOutput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopyOutput.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCopyRawOutput});
            this.btnCopyOutput.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyOutput.Image")));
            this.btnCopyOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyOutput.Name = "btnCopyOutput";
            this.btnCopyOutput.Size = new System.Drawing.Size(32, 22);
            this.btnCopyOutput.Text = "Copy";
            this.btnCopyOutput.ToolTipText = "Copy screen";
            this.btnCopyOutput.ButtonClick += new System.EventHandler(this.BtnCopyOutput_ButtonClick);
            // 
            // btnCopyRawOutput
            // 
            this.btnCopyRawOutput.Name = "btnCopyRawOutput";
            this.btnCopyRawOutput.Size = new System.Drawing.Size(124, 22);
            this.btnCopyRawOutput.Text = "Copy raw";
            this.btnCopyRawOutput.ToolTipText = "Copy raw screen";
            this.btnCopyRawOutput.Click += new System.EventHandler(this.BtnCopyRawOutput_Click);
            // 
            // separator4
            // 
            this.separator4.Name = "separator4";
            this.separator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDebugger
            // 
            this.btnDebugger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDebugger.Image = ((System.Drawing.Image)(resources.GetObject("btnDebugger.Image")));
            this.btnDebugger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDebugger.Name = "btnDebugger";
            this.btnDebugger.Size = new System.Drawing.Size(23, 22);
            this.btnDebugger.Text = "Debugger";
            this.btnDebugger.Click += new System.EventHandler(this.BtnMemoryWatch_Click);
            // 
            // ofd
            // 
            this.ofd.Filter = "PRG-files|*.prg";
            // 
            // sfd
            // 
            this.sfd.Filter = "PRG-files|*.prg";
            // 
            // FormC64Screen
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 447);
            this.Controls.Add(this.toolMain);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.pScreen);
            this.DoubleBuffered = true;
            this.Name = "FormC64Screen";
            this.Text = "Retrocomputing.NET - Commodore 64";
            this.Activated += new System.EventHandler(this.FormC64Screen_Activated);
            this.Deactivate += new System.EventHandler(this.FormC64Screen_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormC64Screen_FormClosing);
            this.Load += new System.EventHandler(this.FormC64Screen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pScreen)).EndInit();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.toolMain.ResumeLayout(false);
            this.toolMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pScreen;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblFps;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripButton btnUseCrtFilter;
        private System.Windows.Forms.ToolStripButton btnRestart;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.ToolStripSplitButton btnCopyOutput;
        private System.Windows.Forms.ToolStripMenuItem btnCopyRawOutput;
        private System.Windows.Forms.ToolStripSeparator separator1;
        private System.Windows.Forms.ToolStripSeparator separator2;
        private System.Windows.Forms.ToolStripSeparator separator3;
        private System.Windows.Forms.ToolStripStatusLabel lblCycles;
        private System.Windows.Forms.ToolStripStatusLabel lblInstructions;
        private System.Windows.Forms.ToolStripButton btnDebugger;
        private System.Windows.Forms.ToolStripButton btnPause;
        private System.Windows.Forms.ToolStripStatusLabel lblKeyboardDisabled;
        private System.Windows.Forms.ToolStripButton btnReset;
        private System.Windows.Forms.ToolStripSeparator separator4;
    }
}