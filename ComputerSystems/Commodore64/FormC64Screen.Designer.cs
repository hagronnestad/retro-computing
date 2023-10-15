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
            this.lblClockSpeedReal = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblClockSpeedRealPercent = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCpuClockSpeedMultiplier = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCycles = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblInstructions = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblIllegalInstructions = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.btnRestart = new System.Windows.Forms.ToolStripButton();
            this.btnReset = new System.Windows.Forms.ToolStripButton();
            this.btnPause = new System.Windows.Forms.ToolStripButton();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.separator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnInsertCartridge = new System.Windows.Forms.ToolStripButton();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUseCrtFilter = new System.Windows.Forms.ToolStripButton();
            this.btnShowOnScreenDisplay = new System.Windows.Forms.ToolStripButton();
            this.btnToggleFullscreen = new System.Windows.Forms.ToolStripButton();
            this.separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCopyOutput = new System.Windows.Forms.ToolStripSplitButton();
            this.btnCopyRawOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.separator4 = new System.Windows.Forms.ToolStripSeparator();
            this.lblDebug = new System.Windows.Forms.ToolStripLabel();
            this.btnDebugger = new System.Windows.Forms.ToolStripButton();
            this.btnClockSpeedSlowerSlower = new System.Windows.Forms.ToolStripButton();
            this.btnClockSpeedSlower = new System.Windows.Forms.ToolStripButton();
            this.btnClockSpeedDefault = new System.Windows.Forms.ToolStripButton();
            this.btnClockSpeedFaster = new System.Windows.Forms.ToolStripButton();
            this.btnClockSpeedFasterFaster = new System.Windows.Forms.ToolStripButton();
            this.separator5 = new System.Windows.Forms.ToolStripSeparator();
            this.lblVicIiDebugging = new System.Windows.Forms.ToolStripLabel();
            this.btnShowVideoFrameOutlines = new System.Windows.Forms.ToolStripButton();
            this.btnShowScanLinePosition = new System.Windows.Forms.ToolStripButton();
            this.btnShowRasterLineInterrupt = new System.Windows.Forms.ToolStripButton();
            this.btnShowFullFrameVideo = new System.Windows.Forms.ToolStripButton();
            this.separator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToggleSound = new System.Windows.Forms.ToolStripButton();
            this.ofdOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatusVic = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFps = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVicCycles = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVicCurrentLine = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVicCurrentLineCycle = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVicGraphicsMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVicScreenOn = new System.Windows.Forms.ToolStripStatusLabel();
            this.ofdInsertCartridge = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.mnuColors = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuKernalWhiteTextColor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPalettes = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenPaletteFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuImportVICEPaletteFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdImportVicePaletteFile = new System.Windows.Forms.OpenFileDialog();
            this.fsw = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.pScreen)).BeginInit();
            this.statusMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fsw)).BeginInit();
            this.SuspendLayout();
            // 
            // pScreen
            // 
            this.pScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pScreen.Location = new System.Drawing.Point(0, 49);
            this.pScreen.Margin = new System.Windows.Forms.Padding(0);
            this.pScreen.Name = "pScreen";
            this.pScreen.Size = new System.Drawing.Size(832, 624);
            this.pScreen.TabIndex = 1;
            this.pScreen.TabStop = false;
            this.pScreen.Click += new System.EventHandler(this.pScreen_Click);
            this.pScreen.DragDrop += new System.Windows.Forms.DragEventHandler(this.pScreen_DragDropAsync);
            this.pScreen.DragEnter += new System.Windows.Forms.DragEventHandler(this.pScreen_DragEnter);
            this.pScreen.Paint += new System.Windows.Forms.PaintEventHandler(this.PScreen_Paint);
            this.pScreen.DoubleClick += new System.EventHandler(this.pScreen_DoubleClick);
            this.pScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pScreen_MouseMove);
            this.pScreen.Resize += new System.EventHandler(this.PScreen_Resize);
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusCpu,
            this.lblClockSpeed,
            this.lblClockSpeedReal,
            this.lblClockSpeedRealPercent,
            this.lblCpuClockSpeedMultiplier,
            this.lblCycles,
            this.lblInstructions,
            this.lblIllegalInstructions});
            this.statusMain.Location = new System.Drawing.Point(0, 701);
            this.statusMain.Name = "statusMain";
            this.statusMain.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusMain.Size = new System.Drawing.Size(832, 24);
            this.statusMain.TabIndex = 2;
            this.statusMain.Text = "statusStrip1";
            // 
            // lblStatusCpu
            // 
            this.lblStatusCpu.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblStatusCpu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
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
            // lblClockSpeedReal
            // 
            this.lblClockSpeedReal.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblClockSpeedReal.Name = "lblClockSpeedReal";
            this.lblClockSpeedReal.Size = new System.Drawing.Size(34, 19);
            this.lblClockSpeedReal.Text = "0 Hz";
            // 
            // lblClockSpeedRealPercent
            // 
            this.lblClockSpeedRealPercent.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblClockSpeedRealPercent.Name = "lblClockSpeedRealPercent";
            this.lblClockSpeedRealPercent.Size = new System.Drawing.Size(30, 19);
            this.lblClockSpeedRealPercent.Text = "0 %";
            // 
            // lblCpuClockSpeedMultiplier
            // 
            this.lblCpuClockSpeedMultiplier.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblCpuClockSpeedMultiplier.Name = "lblCpuClockSpeedMultiplier";
            this.lblCpuClockSpeedMultiplier.Size = new System.Drawing.Size(23, 19);
            this.lblCpuClockSpeedMultiplier.Text = "1x";
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
            // toolMain
            // 
            this.toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRestart,
            this.btnReset,
            this.btnPause,
            this.separator1,
            this.btnOpen,
            this.btnSave,
            this.separator7,
            this.btnInsertCartridge,
            this.separator2,
            this.btnUseCrtFilter,
            this.btnShowOnScreenDisplay,
            this.btnToggleFullscreen,
            this.separator3,
            this.btnCopyOutput,
            this.separator4,
            this.lblDebug,
            this.btnDebugger,
            this.btnClockSpeedSlowerSlower,
            this.btnClockSpeedSlower,
            this.btnClockSpeedDefault,
            this.btnClockSpeedFaster,
            this.btnClockSpeedFasterFaster,
            this.separator5,
            this.lblVicIiDebugging,
            this.btnShowVideoFrameOutlines,
            this.btnShowScanLinePosition,
            this.btnShowRasterLineInterrupt,
            this.btnShowFullFrameVideo,
            this.separator6,
            this.btnToggleSound});
            this.toolMain.Location = new System.Drawing.Point(0, 24);
            this.toolMain.Name = "toolMain";
            this.toolMain.Size = new System.Drawing.Size(832, 25);
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
            // separator7
            // 
            this.separator7.Name = "separator7";
            this.separator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnInsertCartridge
            // 
            this.btnInsertCartridge.Image = ((System.Drawing.Image)(resources.GetObject("btnInsertCartridge.Image")));
            this.btnInsertCartridge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInsertCartridge.Name = "btnInsertCartridge";
            this.btnInsertCartridge.Size = new System.Drawing.Size(23, 22);
            this.btnInsertCartridge.Click += new System.EventHandler(this.btnInsertCartridge_ClickAsync);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUseCrtFilter
            // 
            this.btnUseCrtFilter.Checked = true;
            this.btnUseCrtFilter.CheckOnClick = true;
            this.btnUseCrtFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnUseCrtFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUseCrtFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnUseCrtFilter.Image")));
            this.btnUseCrtFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUseCrtFilter.Name = "btnUseCrtFilter";
            this.btnUseCrtFilter.Size = new System.Drawing.Size(23, 22);
            this.btnUseCrtFilter.Text = "CRT filter";
            // 
            // btnShowOnScreenDisplay
            // 
            this.btnShowOnScreenDisplay.Checked = true;
            this.btnShowOnScreenDisplay.CheckOnClick = true;
            this.btnShowOnScreenDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnShowOnScreenDisplay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowOnScreenDisplay.Image = ((System.Drawing.Image)(resources.GetObject("btnShowOnScreenDisplay.Image")));
            this.btnShowOnScreenDisplay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowOnScreenDisplay.Name = "btnShowOnScreenDisplay";
            this.btnShowOnScreenDisplay.Size = new System.Drawing.Size(23, 22);
            this.btnShowOnScreenDisplay.Text = "CRT filter";
            this.btnShowOnScreenDisplay.Click += new System.EventHandler(this.btnShowOnScreenDisplay_Click);
            // 
            // btnToggleFullscreen
            // 
            this.btnToggleFullscreen.CheckOnClick = true;
            this.btnToggleFullscreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToggleFullscreen.Image = ((System.Drawing.Image)(resources.GetObject("btnToggleFullscreen.Image")));
            this.btnToggleFullscreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToggleFullscreen.Name = "btnToggleFullscreen";
            this.btnToggleFullscreen.Size = new System.Drawing.Size(23, 22);
            this.btnToggleFullscreen.Text = "Toggle Fullscreen";
            this.btnToggleFullscreen.Click += new System.EventHandler(this.btnToggleFullscreen_Click);
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
            // lblDebug
            // 
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(33, 22);
            this.lblDebug.Text = "CPU:";
            this.lblDebug.Visible = false;
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
            // btnClockSpeedSlowerSlower
            // 
            this.btnClockSpeedSlowerSlower.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClockSpeedSlowerSlower.Image = ((System.Drawing.Image)(resources.GetObject("btnClockSpeedSlowerSlower.Image")));
            this.btnClockSpeedSlowerSlower.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClockSpeedSlowerSlower.Name = "btnClockSpeedSlowerSlower";
            this.btnClockSpeedSlowerSlower.Size = new System.Drawing.Size(23, 22);
            this.btnClockSpeedSlowerSlower.Text = "Speed [-]";
            this.btnClockSpeedSlowerSlower.ToolTipText = "Clock Speed -";
            this.btnClockSpeedSlowerSlower.Click += new System.EventHandler(this.btnClockSpeedSlowerSlower_Click);
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
            // btnClockSpeedDefault
            // 
            this.btnClockSpeedDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClockSpeedDefault.Image = ((System.Drawing.Image)(resources.GetObject("btnClockSpeedDefault.Image")));
            this.btnClockSpeedDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClockSpeedDefault.Name = "btnClockSpeedDefault";
            this.btnClockSpeedDefault.Size = new System.Drawing.Size(23, 22);
            this.btnClockSpeedDefault.Text = "Speed [-]";
            this.btnClockSpeedDefault.ToolTipText = "Clock Speed -";
            this.btnClockSpeedDefault.Click += new System.EventHandler(this.btnClockSpeedDefault_Click);
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
            // btnClockSpeedFasterFaster
            // 
            this.btnClockSpeedFasterFaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClockSpeedFasterFaster.Image = ((System.Drawing.Image)(resources.GetObject("btnClockSpeedFasterFaster.Image")));
            this.btnClockSpeedFasterFaster.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClockSpeedFasterFaster.Name = "btnClockSpeedFasterFaster";
            this.btnClockSpeedFasterFaster.Size = new System.Drawing.Size(23, 22);
            this.btnClockSpeedFasterFaster.Text = "Speed [+]";
            this.btnClockSpeedFasterFaster.ToolTipText = "Clock Speed +";
            this.btnClockSpeedFasterFaster.Click += new System.EventHandler(this.btnClockSpeedFasterFaster_Click);
            // 
            // separator5
            // 
            this.separator5.Name = "separator5";
            this.separator5.Size = new System.Drawing.Size(6, 25);
            // 
            // lblVicIiDebugging
            // 
            this.lblVicIiDebugging.Name = "lblVicIiDebugging";
            this.lblVicIiDebugging.Size = new System.Drawing.Size(39, 22);
            this.lblVicIiDebugging.Text = "VIC-II:";
            this.lblVicIiDebugging.Visible = false;
            // 
            // btnShowVideoFrameOutlines
            // 
            this.btnShowVideoFrameOutlines.CheckOnClick = true;
            this.btnShowVideoFrameOutlines.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowVideoFrameOutlines.Image = ((System.Drawing.Image)(resources.GetObject("btnShowVideoFrameOutlines.Image")));
            this.btnShowVideoFrameOutlines.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowVideoFrameOutlines.Name = "btnShowVideoFrameOutlines";
            this.btnShowVideoFrameOutlines.Size = new System.Drawing.Size(23, 22);
            this.btnShowVideoFrameOutlines.Text = "Outlines";
            // 
            // btnShowScanLinePosition
            // 
            this.btnShowScanLinePosition.CheckOnClick = true;
            this.btnShowScanLinePosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowScanLinePosition.Image = ((System.Drawing.Image)(resources.GetObject("btnShowScanLinePosition.Image")));
            this.btnShowScanLinePosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowScanLinePosition.Name = "btnShowScanLinePosition";
            this.btnShowScanLinePosition.Size = new System.Drawing.Size(23, 22);
            this.btnShowScanLinePosition.Text = "Scanline Position";
            // 
            // btnShowRasterLineInterrupt
            // 
            this.btnShowRasterLineInterrupt.CheckOnClick = true;
            this.btnShowRasterLineInterrupt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowRasterLineInterrupt.Image = ((System.Drawing.Image)(resources.GetObject("btnShowRasterLineInterrupt.Image")));
            this.btnShowRasterLineInterrupt.Name = "btnShowRasterLineInterrupt";
            this.btnShowRasterLineInterrupt.Size = new System.Drawing.Size(23, 22);
            this.btnShowRasterLineInterrupt.Text = "Show Raster Line Interrupt";
            // 
            // btnShowFullFrameVideo
            // 
            this.btnShowFullFrameVideo.CheckOnClick = true;
            this.btnShowFullFrameVideo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowFullFrameVideo.Image = ((System.Drawing.Image)(resources.GetObject("btnShowFullFrameVideo.Image")));
            this.btnShowFullFrameVideo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowFullFrameVideo.Name = "btnShowFullFrameVideo";
            this.btnShowFullFrameVideo.Size = new System.Drawing.Size(23, 22);
            this.btnShowFullFrameVideo.Text = "Show Full Frame";
            this.btnShowFullFrameVideo.Click += new System.EventHandler(this.btnShowFullFrameVideo_Click);
            // 
            // separator6
            // 
            this.separator6.Name = "separator6";
            this.separator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnToggleSound
            // 
            this.btnToggleSound.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToggleSound.Image = ((System.Drawing.Image)(resources.GetObject("btnToggleSound.Image")));
            this.btnToggleSound.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToggleSound.Name = "btnToggleSound";
            this.btnToggleSound.Size = new System.Drawing.Size(23, 22);
            this.btnToggleSound.Text = "toolStripButton1";
            this.btnToggleSound.Click += new System.EventHandler(this.btnShowSidDebugWindow_Click);
            // 
            // ofdOpenFile
            // 
            this.ofdOpenFile.Filter = "PRG-files|*.prg|All files|*.*";
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 677);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(832, 24);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatusVic
            // 
            this.lblStatusVic.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblStatusVic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
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
            // ofdInsertCartridge
            // 
            this.ofdInsertCartridge.Filter = "Cartridges|*.crt;*.bin|Cartridge|*.crt|Raw Cartridge|*.bin|All files|*.*";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuColors});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(832, 24);
            this.menuStrip.TabIndex = 5;
            this.menuStrip.Text = "menuStrip1";
            // 
            // mnuColors
            // 
            this.mnuColors.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuKernalWhiteTextColor,
            this.toolStripSeparator2,
            this.mnuPalettes,
            this.mnuOpenPaletteFolder,
            this.toolStripSeparator1,
            this.mnuImportVICEPaletteFile});
            this.mnuColors.Name = "mnuColors";
            this.mnuColors.Size = new System.Drawing.Size(53, 20);
            this.mnuColors.Text = "Colors";
            this.mnuColors.DropDownOpening += new System.EventHandler(this.mnuColors_DropDownOpening);
            // 
            // mnuKernalWhiteTextColor
            // 
            this.mnuKernalWhiteTextColor.Name = "mnuKernalWhiteTextColor";
            this.mnuKernalWhiteTextColor.Size = new System.Drawing.Size(197, 22);
            this.mnuKernalWhiteTextColor.Text = "White Text Color Patch";
            this.mnuKernalWhiteTextColor.Click += new System.EventHandler(this.mnuKernalWhiteTextColor_ClickAsync);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(194, 6);
            // 
            // mnuPalettes
            // 
            this.mnuPalettes.Name = "mnuPalettes";
            this.mnuPalettes.Size = new System.Drawing.Size(197, 22);
            this.mnuPalettes.Text = "Palettes";
            // 
            // mnuOpenPaletteFolder
            // 
            this.mnuOpenPaletteFolder.Name = "mnuOpenPaletteFolder";
            this.mnuOpenPaletteFolder.Size = new System.Drawing.Size(197, 22);
            this.mnuOpenPaletteFolder.Text = "Open Palette Folder";
            this.mnuOpenPaletteFolder.Click += new System.EventHandler(this.mnuOpenPaletteFolder_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
            // 
            // mnuImportVICEPaletteFile
            // 
            this.mnuImportVICEPaletteFile.Name = "mnuImportVICEPaletteFile";
            this.mnuImportVICEPaletteFile.Size = new System.Drawing.Size(197, 22);
            this.mnuImportVICEPaletteFile.Text = "Import VICE Palette File";
            this.mnuImportVICEPaletteFile.Click += new System.EventHandler(this.mnuImportVICEPaletteFile_Click);
            // 
            // ofdImportVicePaletteFile
            // 
            this.ofdImportVicePaletteFile.Filter = "VICE Palette File|*.vpl";
            // 
            // fsw
            // 
            this.fsw.EnableRaisingEvents = true;
            this.fsw.NotifyFilter = ((System.IO.NotifyFilters)((((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName) 
            | System.IO.NotifyFilters.LastWrite) 
            | System.IO.NotifyFilters.CreationTime)));
            this.fsw.SynchronizingObject = this;
            this.fsw.Changed += new System.IO.FileSystemEventHandler(this.fsw_Changed);
            // 
            // FormC64Screen
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 725);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolMain);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.pScreen);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FormC64Screen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Retrocomputing.NET - Commodore 64";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormC64Screen_FormClosing);
            this.Load += new System.EventHandler(this.FormC64Screen_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormC64Screen_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormC64Screen_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pScreen)).EndInit();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.toolMain.ResumeLayout(false);
            this.toolMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fsw)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pScreen;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusCpu;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripButton btnRestart;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.OpenFileDialog ofdOpenFile;
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
        private System.Windows.Forms.ToolStripStatusLabel lblClockSpeedReal;
        private System.Windows.Forms.ToolStripStatusLabel lblIllegalInstructions;
        private System.Windows.Forms.ToolStripSeparator separator5;
        private System.Windows.Forms.ToolStripButton btnShowVideoFrameOutlines;
        private System.Windows.Forms.ToolStripButton btnShowScanLinePosition;
        private System.Windows.Forms.ToolStripButton btnShowFullFrameVideo;
        private System.Windows.Forms.ToolStripLabel lblDebug;
        private System.Windows.Forms.ToolStripLabel lblVicIiDebugging;
        private System.Windows.Forms.ToolStripSeparator separator6;
        private System.Windows.Forms.ToolStripButton btnToggleFullscreen;
        private System.Windows.Forms.ToolStripButton btnShowRasterLineInterrupt;
        private System.Windows.Forms.ToolStripButton btnInsertCartridge;
        private System.Windows.Forms.OpenFileDialog ofdInsertCartridge;
        private System.Windows.Forms.ToolStripSeparator separator7;
        private System.Windows.Forms.ToolStripStatusLabel lblClockSpeed;
        private System.Windows.Forms.ToolStripStatusLabel lblClockSpeedRealPercent;
        private System.Windows.Forms.ToolStripStatusLabel lblCpuClockSpeedMultiplier;
        private System.Windows.Forms.ToolStripButton btnUseCrtFilter;
        private System.Windows.Forms.ToolStripButton btnClockSpeedSlowerSlower;
        private System.Windows.Forms.ToolStripButton btnClockSpeedFasterFaster;
        private System.Windows.Forms.ToolStripButton btnClockSpeedDefault;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuColors;
        private System.Windows.Forms.ToolStripMenuItem mnuPalettes;
        private System.Windows.Forms.ToolStripMenuItem mnuImportVICEPaletteFile;
        private System.Windows.Forms.OpenFileDialog ofdImportVicePaletteFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenPaletteFolder;
        private System.Windows.Forms.ToolStripMenuItem mnuKernalWhiteTextColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnShowOnScreenDisplay;
        private System.IO.FileSystemWatcher fsw;
        private System.Windows.Forms.ToolStripButton btnToggleSound;
    }
}