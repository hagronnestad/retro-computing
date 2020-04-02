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
            this.lblStatusCpu = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblClockSpeed = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCycles = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblInstructions = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblIllegalInstructions = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblKeyboardDisabled = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.btnRestart = new System.Windows.Forms.ToolStripButton();
            this.btnReset = new System.Windows.Forms.ToolStripButton();
            this.btnPause = new System.Windows.Forms.ToolStripButton();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCopyOutput = new System.Windows.Forms.ToolStripSplitButton();
            this.btnCopyRawOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.separator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDebugger = new System.Windows.Forms.ToolStripButton();
            this.btnClockSpeedSlower = new System.Windows.Forms.ToolStripButton();
            this.btnClockSpeedFaster = new System.Windows.Forms.ToolStripButton();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatusVic = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFps = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVicCycles = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVicCurrentLine = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVicCurrentLineCycle = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVicGraphicsMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVicScreenOn = new System.Windows.Forms.ToolStripStatusLabel();
            this.separator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUseCrtFilter = new System.Windows.Forms.ToolStripButton();
            this.btnShowVideoFrameOutlines = new System.Windows.Forms.ToolStripButton();
            this.btnShowScanLinePosition = new System.Windows.Forms.ToolStripButton();
            this.btnShowFullFrameVideo = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.pScreen)).BeginInit();
            this.statusMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
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
            this.pScreen.Size = new System.Drawing.Size(1008, 624);
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
            this.lblStatusCpu,
            this.lblClockSpeed,
            this.lblCycles,
            this.lblInstructions,
            this.lblIllegalInstructions,
            this.lblKeyboardDisabled});
            this.statusMain.Location = new System.Drawing.Point(0, 673);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(1008, 24);
            this.statusMain.TabIndex = 2;
            this.statusMain.Text = "statusStrip1";
            // 
            // lblStatusCpu
            // 
            this.lblStatusCpu.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblStatusCpu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusCpu.Name = "lblStatusCpu";
            this.lblStatusCpu.Size = new System.Drawing.Size(34, 19);
            this.lblStatusCpu.Text = "CPU";
            // 
            // lblClockSpeed
            // 
            this.lblClockSpeed.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblClockSpeed.Name = "lblClockSpeed";
            this.lblClockSpeed.Size = new System.Drawing.Size(34, 19);
            this.lblClockSpeed.Text = "0 Hz";
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
            // lblIllegalInstructions
            // 
            this.lblIllegalInstructions.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblIllegalInstructions.Name = "lblIllegalInstructions";
            this.lblIllegalInstructions.Size = new System.Drawing.Size(82, 19);
            this.lblIllegalInstructions.Text = "0 instructions";
            // 
            // lblKeyboardDisabled
            // 
            this.lblKeyboardDisabled.ForeColor = System.Drawing.Color.Red;
            this.lblKeyboardDisabled.Name = "lblKeyboardDisabled";
            this.lblKeyboardDisabled.Size = new System.Drawing.Size(709, 19);
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
            this.btnDebugger,
            this.btnClockSpeedSlower,
            this.btnClockSpeedFaster,
            this.separator5,
            this.btnShowVideoFrameOutlines,
            this.btnShowScanLinePosition,
            this.btnShowFullFrameVideo});
            this.toolMain.Location = new System.Drawing.Point(0, 0);
            this.toolMain.Name = "toolMain";
            this.toolMain.Size = new System.Drawing.Size(1008, 25);
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
            // btnClockSpeedSlower
            // 
            this.btnClockSpeedSlower.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClockSpeedSlower.Image = ((System.Drawing.Image)(resources.GetObject("btnClockSpeedSlower.Image")));
            this.btnClockSpeedSlower.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClockSpeedSlower.Name = "btnClockSpeedSlower";
            this.btnClockSpeedSlower.Size = new System.Drawing.Size(23, 22);
            this.btnClockSpeedSlower.Text = "Speed [-]";
            this.btnClockSpeedSlower.ToolTipText = "Clock Speed -";
            this.btnClockSpeedSlower.Click += new System.EventHandler(this.btnSlowDown_Click);
            // 
            // btnClockSpeedFaster
            // 
            this.btnClockSpeedFaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClockSpeedFaster.Image = ((System.Drawing.Image)(resources.GetObject("btnClockSpeedFaster.Image")));
            this.btnClockSpeedFaster.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClockSpeedFaster.Name = "btnClockSpeedFaster";
            this.btnClockSpeedFaster.Size = new System.Drawing.Size(23, 22);
            this.btnClockSpeedFaster.Text = "Speed [+]";
            this.btnClockSpeedFaster.ToolTipText = "Clock Speed +";
            this.btnClockSpeedFaster.Click += new System.EventHandler(this.btnClockSpeedFaster_Click);
            // 
            // ofd
            // 
            this.ofd.Filter = "PRG-files|*.prg";
            // 
            // sfd
            // 
            this.sfd.Filter = "PRG-files|*.prg";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusVic,
            this.lblFps,
            this.lblVicCycles,
            this.lblVicCurrentLine,
            this.lblVicCurrentLineCycle,
            this.lblVicGraphicsMode,
            this.lblVicScreenOn});
            this.statusStrip1.Location = new System.Drawing.Point(0, 649);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 24);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatusVic
            // 
            this.lblStatusVic.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblStatusVic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusVic.Name = "lblStatusVic";
            this.lblStatusVic.Size = new System.Drawing.Size(43, 19);
            this.lblStatusVic.Text = "VIC-II";
            // 
            // lblFps
            // 
            this.lblFps.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(36, 19);
            this.lblFps.Text = "0 fps";
            // 
            // lblVicCycles
            // 
            this.lblVicCycles.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblVicCycles.Name = "lblVicCycles";
            this.lblVicCycles.Size = new System.Drawing.Size(52, 19);
            this.lblVicCycles.Text = "0 cycles";
            // 
            // lblVicCurrentLine
            // 
            this.lblVicCurrentLine.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblVicCurrentLine.Name = "lblVicCurrentLine";
            this.lblVicCurrentLine.Size = new System.Drawing.Size(36, 19);
            this.lblVicCurrentLine.Text = "Line:";
            // 
            // lblVicCurrentLineCycle
            // 
            this.lblVicCurrentLineCycle.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblVicCurrentLineCycle.Name = "lblVicCurrentLineCycle";
            this.lblVicCurrentLineCycle.Size = new System.Drawing.Size(33, 19);
            this.lblVicCurrentLineCycle.Text = "Pos:";
            // 
            // lblVicGraphicsMode
            // 
            this.lblVicGraphicsMode.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblVicGraphicsMode.Name = "lblVicGraphicsMode";
            this.lblVicGraphicsMode.Size = new System.Drawing.Size(45, 19);
            this.lblVicGraphicsMode.Text = "Mode:";
            // 
            // lblVicScreenOn
            // 
            this.lblVicScreenOn.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblVicScreenOn.Name = "lblVicScreenOn";
            this.lblVicScreenOn.Size = new System.Drawing.Size(49, 19);
            this.lblVicScreenOn.Text = "Screen:";
            // 
            // separator5
            // 
            this.separator5.Name = "separator5";
            this.separator5.Size = new System.Drawing.Size(6, 25);
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
            // btnShowVideoFrameOutlines
            // 
            this.btnShowVideoFrameOutlines.Checked = global::Commodore64.Properties.Settings.Default.ShowVideoFrameOutlines;
            this.btnShowVideoFrameOutlines.CheckOnClick = true;
            this.btnShowVideoFrameOutlines.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnShowVideoFrameOutlines.Image = ((System.Drawing.Image)(resources.GetObject("btnShowVideoFrameOutlines.Image")));
            this.btnShowVideoFrameOutlines.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowVideoFrameOutlines.Name = "btnShowVideoFrameOutlines";
            this.btnShowVideoFrameOutlines.Size = new System.Drawing.Size(55, 22);
            this.btnShowVideoFrameOutlines.Text = "Outlines";
            // 
            // btnShowScanLinePosition
            // 
            this.btnShowScanLinePosition.Checked = global::Commodore64.Properties.Settings.Default.ShowScanLinePosition;
            this.btnShowScanLinePosition.CheckOnClick = true;
            this.btnShowScanLinePosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnShowScanLinePosition.Image = ((System.Drawing.Image)(resources.GetObject("btnShowScanLinePosition.Image")));
            this.btnShowScanLinePosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowScanLinePosition.Name = "btnShowScanLinePosition";
            this.btnShowScanLinePosition.Size = new System.Drawing.Size(101, 22);
            this.btnShowScanLinePosition.Text = "Scanline Position";
            // 
            // btnShowFullFrameVideo
            // 
            this.btnShowFullFrameVideo.Checked = global::Commodore64.Properties.Settings.Default.ShowFullFrameVideo;
            this.btnShowFullFrameVideo.CheckOnClick = true;
            this.btnShowFullFrameVideo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnShowFullFrameVideo.Image = ((System.Drawing.Image)(resources.GetObject("btnShowFullFrameVideo.Image")));
            this.btnShowFullFrameVideo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowFullFrameVideo.Name = "btnShowFullFrameVideo";
            this.btnShowFullFrameVideo.Size = new System.Drawing.Size(98, 22);
            this.btnShowFullFrameVideo.Text = "Show Full Frame";
            // 
            // FormC64Screen
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 697);
            this.Controls.Add(this.statusStrip1);
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
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pScreen;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusCpu;
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
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblFps;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusVic;
        private System.Windows.Forms.ToolStripStatusLabel lblVicCycles;
        private System.Windows.Forms.ToolStripStatusLabel lblVicGraphicsMode;
        private System.Windows.Forms.ToolStripStatusLabel lblVicScreenOn;
        private System.Windows.Forms.ToolStripStatusLabel lblVicCurrentLine;
        private System.Windows.Forms.ToolStripStatusLabel lblVicCurrentLineCycle;
        private System.Windows.Forms.ToolStripButton btnClockSpeedSlower;
        private System.Windows.Forms.ToolStripButton btnClockSpeedFaster;
        private System.Windows.Forms.ToolStripStatusLabel lblClockSpeed;
        private System.Windows.Forms.ToolStripStatusLabel lblIllegalInstructions;
        private System.Windows.Forms.ToolStripSeparator separator5;
        private System.Windows.Forms.ToolStripButton btnShowVideoFrameOutlines;
        private System.Windows.Forms.ToolStripButton btnShowScanLinePosition;
        private System.Windows.Forms.ToolStripButton btnShowFullFrameVideo;
    }
}