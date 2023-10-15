namespace ComputerSystem.Commodore64
{
    partial class FormC64Screen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormC64Screen));
            pScreen = new System.Windows.Forms.PictureBox();
            statusMain = new System.Windows.Forms.StatusStrip();
            lblStatusCpu = new System.Windows.Forms.ToolStripStatusLabel();
            lblClockSpeed = new System.Windows.Forms.ToolStripStatusLabel();
            lblClockSpeedReal = new System.Windows.Forms.ToolStripStatusLabel();
            lblClockSpeedRealPercent = new System.Windows.Forms.ToolStripStatusLabel();
            lblCpuClockSpeedMultiplier = new System.Windows.Forms.ToolStripStatusLabel();
            lblCycles = new System.Windows.Forms.ToolStripStatusLabel();
            lblInstructions = new System.Windows.Forms.ToolStripStatusLabel();
            lblIllegalInstructions = new System.Windows.Forms.ToolStripStatusLabel();
            toolMain = new System.Windows.Forms.ToolStrip();
            btnRestart = new System.Windows.Forms.ToolStripButton();
            btnReset = new System.Windows.Forms.ToolStripButton();
            btnPause = new System.Windows.Forms.ToolStripButton();
            separator1 = new System.Windows.Forms.ToolStripSeparator();
            btnOpen = new System.Windows.Forms.ToolStripButton();
            btnSave = new System.Windows.Forms.ToolStripButton();
            separator7 = new System.Windows.Forms.ToolStripSeparator();
            btnInsertCartridge = new System.Windows.Forms.ToolStripButton();
            separator2 = new System.Windows.Forms.ToolStripSeparator();
            btnUseCrtFilter = new System.Windows.Forms.ToolStripButton();
            btnShowOnScreenDisplay = new System.Windows.Forms.ToolStripButton();
            btnToggleFullscreen = new System.Windows.Forms.ToolStripButton();
            separator3 = new System.Windows.Forms.ToolStripSeparator();
            btnCopyOutput = new System.Windows.Forms.ToolStripSplitButton();
            btnCopyRawOutput = new System.Windows.Forms.ToolStripMenuItem();
            separator4 = new System.Windows.Forms.ToolStripSeparator();
            lblDebug = new System.Windows.Forms.ToolStripLabel();
            btnDebugger = new System.Windows.Forms.ToolStripButton();
            btnClockSpeedSlowerSlower = new System.Windows.Forms.ToolStripButton();
            btnClockSpeedSlower = new System.Windows.Forms.ToolStripButton();
            btnClockSpeedDefault = new System.Windows.Forms.ToolStripButton();
            btnClockSpeedFaster = new System.Windows.Forms.ToolStripButton();
            btnClockSpeedFasterFaster = new System.Windows.Forms.ToolStripButton();
            separator5 = new System.Windows.Forms.ToolStripSeparator();
            lblVicIiDebugging = new System.Windows.Forms.ToolStripLabel();
            btnShowVideoFrameOutlines = new System.Windows.Forms.ToolStripButton();
            btnShowScanLinePosition = new System.Windows.Forms.ToolStripButton();
            btnShowRasterLineInterrupt = new System.Windows.Forms.ToolStripButton();
            btnShowFullFrameVideo = new System.Windows.Forms.ToolStripButton();
            separator6 = new System.Windows.Forms.ToolStripSeparator();
            btnToggleSound = new System.Windows.Forms.ToolStripButton();
            ofdOpenFile = new System.Windows.Forms.OpenFileDialog();
            sfd = new System.Windows.Forms.SaveFileDialog();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            lblStatusVic = new System.Windows.Forms.ToolStripStatusLabel();
            lblFps = new System.Windows.Forms.ToolStripStatusLabel();
            lblVicCycles = new System.Windows.Forms.ToolStripStatusLabel();
            lblVicCurrentLine = new System.Windows.Forms.ToolStripStatusLabel();
            lblVicCurrentLineCycle = new System.Windows.Forms.ToolStripStatusLabel();
            lblVicGraphicsMode = new System.Windows.Forms.ToolStripStatusLabel();
            lblVicScreenOn = new System.Windows.Forms.ToolStripStatusLabel();
            ofdInsertCartridge = new System.Windows.Forms.OpenFileDialog();
            menuStrip = new System.Windows.Forms.MenuStrip();
            mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            mnuColors = new System.Windows.Forms.ToolStripMenuItem();
            mnuKernalWhiteTextColor = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            mnuBorderColor = new System.Windows.Forms.ToolStripMenuItem();
            mnuBackgroundColor = new System.Windows.Forms.ToolStripMenuItem();
            mnuTextColor = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            mnuPalettes = new System.Windows.Forms.ToolStripMenuItem();
            mnuOpenPaletteFolder = new System.Windows.Forms.ToolStripMenuItem();
            mnuImportVICEPaletteFile = new System.Windows.Forms.ToolStripMenuItem();
            ofdImportVicePaletteFile = new System.Windows.Forms.OpenFileDialog();
            fsw = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)pScreen).BeginInit();
            statusMain.SuspendLayout();
            toolMain.SuspendLayout();
            statusStrip1.SuspendLayout();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fsw).BeginInit();
            SuspendLayout();
            // 
            // pScreen
            // 
            pScreen.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pScreen.Location = new System.Drawing.Point(0, 49);
            pScreen.Margin = new System.Windows.Forms.Padding(0);
            pScreen.Name = "pScreen";
            pScreen.Size = new System.Drawing.Size(832, 624);
            pScreen.TabIndex = 1;
            pScreen.TabStop = false;
            pScreen.DragDrop += pScreen_DragDropAsync;
            pScreen.DragEnter += pScreen_DragEnter;
            pScreen.Paint += PScreen_Paint;
            pScreen.DoubleClick += pScreen_DoubleClick;
            pScreen.MouseMove += pScreen_MouseMove;
            pScreen.Resize += PScreen_Resize;
            // 
            // statusMain
            // 
            statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblStatusCpu, lblClockSpeed, lblClockSpeedReal, lblClockSpeedRealPercent, lblCpuClockSpeedMultiplier, lblCycles, lblInstructions, lblIllegalInstructions });
            statusMain.Location = new System.Drawing.Point(0, 701);
            statusMain.Name = "statusMain";
            statusMain.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusMain.Size = new System.Drawing.Size(832, 24);
            statusMain.TabIndex = 2;
            statusMain.Text = "statusStrip1";
            // 
            // lblStatusCpu
            // 
            lblStatusCpu.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblStatusCpu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblStatusCpu.Name = "lblStatusCpu";
            lblStatusCpu.Size = new System.Drawing.Size(34, 19);
            lblStatusCpu.Text = "CPU";
            // 
            // lblClockSpeed
            // 
            lblClockSpeed.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblClockSpeed.Name = "lblClockSpeed";
            lblClockSpeed.Size = new System.Drawing.Size(34, 19);
            lblClockSpeed.Text = "0 Hz";
            // 
            // lblClockSpeedReal
            // 
            lblClockSpeedReal.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblClockSpeedReal.Name = "lblClockSpeedReal";
            lblClockSpeedReal.Size = new System.Drawing.Size(34, 19);
            lblClockSpeedReal.Text = "0 Hz";
            // 
            // lblClockSpeedRealPercent
            // 
            lblClockSpeedRealPercent.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblClockSpeedRealPercent.Name = "lblClockSpeedRealPercent";
            lblClockSpeedRealPercent.Size = new System.Drawing.Size(30, 19);
            lblClockSpeedRealPercent.Text = "0 %";
            // 
            // lblCpuClockSpeedMultiplier
            // 
            lblCpuClockSpeedMultiplier.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblCpuClockSpeedMultiplier.Name = "lblCpuClockSpeedMultiplier";
            lblCpuClockSpeedMultiplier.Size = new System.Drawing.Size(23, 19);
            lblCpuClockSpeedMultiplier.Text = "1x";
            // 
            // lblCycles
            // 
            lblCycles.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblCycles.Name = "lblCycles";
            lblCycles.Size = new System.Drawing.Size(52, 19);
            lblCycles.Text = "0 cycles";
            // 
            // lblInstructions
            // 
            lblInstructions.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new System.Drawing.Size(82, 19);
            lblInstructions.Text = "0 instructions";
            // 
            // lblIllegalInstructions
            // 
            lblIllegalInstructions.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblIllegalInstructions.Name = "lblIllegalInstructions";
            lblIllegalInstructions.Size = new System.Drawing.Size(82, 19);
            lblIllegalInstructions.Text = "0 instructions";
            // 
            // toolMain
            // 
            toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnRestart, btnReset, btnPause, separator1, btnOpen, btnSave, separator7, btnInsertCartridge, separator2, btnUseCrtFilter, btnShowOnScreenDisplay, btnToggleFullscreen, separator3, btnCopyOutput, separator4, lblDebug, btnDebugger, btnClockSpeedSlowerSlower, btnClockSpeedSlower, btnClockSpeedDefault, btnClockSpeedFaster, btnClockSpeedFasterFaster, separator5, lblVicIiDebugging, btnShowVideoFrameOutlines, btnShowScanLinePosition, btnShowRasterLineInterrupt, btnShowFullFrameVideo, separator6, btnToggleSound });
            toolMain.Location = new System.Drawing.Point(0, 24);
            toolMain.Name = "toolMain";
            toolMain.Size = new System.Drawing.Size(832, 25);
            toolMain.TabIndex = 0;
            toolMain.Text = "toolStrip1";
            // 
            // btnRestart
            // 
            btnRestart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnRestart.Image = (System.Drawing.Image)resources.GetObject("btnRestart.Image");
            btnRestart.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnRestart.Name = "btnRestart";
            btnRestart.Size = new System.Drawing.Size(23, 22);
            btnRestart.Text = "Restart";
            btnRestart.Click += BtnRestart_Click;
            // 
            // btnReset
            // 
            btnReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnReset.Image = (System.Drawing.Image)resources.GetObject("btnReset.Image");
            btnReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnReset.Name = "btnReset";
            btnReset.Size = new System.Drawing.Size(23, 22);
            btnReset.Text = "Reset";
            btnReset.Click += BtnReset_Click;
            // 
            // btnPause
            // 
            btnPause.CheckOnClick = true;
            btnPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnPause.Image = (System.Drawing.Image)resources.GetObject("btnPause.Image");
            btnPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnPause.Name = "btnPause";
            btnPause.Size = new System.Drawing.Size(23, 22);
            btnPause.Text = "Pause";
            btnPause.Click += BtnPause_ClickAsync;
            // 
            // separator1
            // 
            separator1.Name = "separator1";
            separator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOpen
            // 
            btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnOpen.Image = (System.Drawing.Image)resources.GetObject("btnOpen.Image");
            btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new System.Drawing.Size(23, 22);
            btnOpen.Text = "Open";
            btnOpen.Click += BtnOpen_Click;
            // 
            // btnSave
            // 
            btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
            btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(23, 22);
            btnSave.Text = "Save";
            btnSave.Click += BtnSave_Click;
            // 
            // separator7
            // 
            separator7.Name = "separator7";
            separator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnInsertCartridge
            // 
            btnInsertCartridge.Image = (System.Drawing.Image)resources.GetObject("btnInsertCartridge.Image");
            btnInsertCartridge.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnInsertCartridge.Name = "btnInsertCartridge";
            btnInsertCartridge.Size = new System.Drawing.Size(23, 22);
            btnInsertCartridge.Click += btnInsertCartridge_ClickAsync;
            // 
            // separator2
            // 
            separator2.Name = "separator2";
            separator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUseCrtFilter
            // 
            btnUseCrtFilter.Checked = true;
            btnUseCrtFilter.CheckOnClick = true;
            btnUseCrtFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            btnUseCrtFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnUseCrtFilter.Image = (System.Drawing.Image)resources.GetObject("btnUseCrtFilter.Image");
            btnUseCrtFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnUseCrtFilter.Name = "btnUseCrtFilter";
            btnUseCrtFilter.Size = new System.Drawing.Size(23, 22);
            btnUseCrtFilter.Text = "CRT filter";
            // 
            // btnShowOnScreenDisplay
            // 
            btnShowOnScreenDisplay.Checked = true;
            btnShowOnScreenDisplay.CheckOnClick = true;
            btnShowOnScreenDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            btnShowOnScreenDisplay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnShowOnScreenDisplay.Image = (System.Drawing.Image)resources.GetObject("btnShowOnScreenDisplay.Image");
            btnShowOnScreenDisplay.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnShowOnScreenDisplay.Name = "btnShowOnScreenDisplay";
            btnShowOnScreenDisplay.Size = new System.Drawing.Size(23, 22);
            btnShowOnScreenDisplay.Text = "CRT filter";
            btnShowOnScreenDisplay.Click += btnShowOnScreenDisplay_Click;
            // 
            // btnToggleFullscreen
            // 
            btnToggleFullscreen.CheckOnClick = true;
            btnToggleFullscreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnToggleFullscreen.Image = (System.Drawing.Image)resources.GetObject("btnToggleFullscreen.Image");
            btnToggleFullscreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnToggleFullscreen.Name = "btnToggleFullscreen";
            btnToggleFullscreen.Size = new System.Drawing.Size(23, 22);
            btnToggleFullscreen.Text = "Toggle Fullscreen";
            btnToggleFullscreen.Click += btnToggleFullscreen_Click;
            // 
            // separator3
            // 
            separator3.Name = "separator3";
            separator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCopyOutput
            // 
            btnCopyOutput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnCopyOutput.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btnCopyRawOutput });
            btnCopyOutput.Image = (System.Drawing.Image)resources.GetObject("btnCopyOutput.Image");
            btnCopyOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnCopyOutput.Name = "btnCopyOutput";
            btnCopyOutput.Size = new System.Drawing.Size(32, 22);
            btnCopyOutput.Text = "Copy";
            btnCopyOutput.ToolTipText = "Copy screen";
            btnCopyOutput.ButtonClick += BtnCopyOutput_ButtonClick;
            // 
            // btnCopyRawOutput
            // 
            btnCopyRawOutput.Name = "btnCopyRawOutput";
            btnCopyRawOutput.Size = new System.Drawing.Size(124, 22);
            btnCopyRawOutput.Text = "Copy raw";
            btnCopyRawOutput.ToolTipText = "Copy raw screen";
            btnCopyRawOutput.Click += BtnCopyRawOutput_Click;
            // 
            // separator4
            // 
            separator4.Name = "separator4";
            separator4.Size = new System.Drawing.Size(6, 25);
            // 
            // lblDebug
            // 
            lblDebug.Name = "lblDebug";
            lblDebug.Size = new System.Drawing.Size(33, 22);
            lblDebug.Text = "CPU:";
            lblDebug.Visible = false;
            // 
            // btnDebugger
            // 
            btnDebugger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnDebugger.Image = (System.Drawing.Image)resources.GetObject("btnDebugger.Image");
            btnDebugger.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnDebugger.Name = "btnDebugger";
            btnDebugger.Size = new System.Drawing.Size(23, 22);
            btnDebugger.Text = "Debugger";
            btnDebugger.Click += BtnMemoryWatch_Click;
            // 
            // btnClockSpeedSlowerSlower
            // 
            btnClockSpeedSlowerSlower.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnClockSpeedSlowerSlower.Image = (System.Drawing.Image)resources.GetObject("btnClockSpeedSlowerSlower.Image");
            btnClockSpeedSlowerSlower.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnClockSpeedSlowerSlower.Name = "btnClockSpeedSlowerSlower";
            btnClockSpeedSlowerSlower.Size = new System.Drawing.Size(23, 22);
            btnClockSpeedSlowerSlower.Text = "Speed [-]";
            btnClockSpeedSlowerSlower.ToolTipText = "Clock Speed -";
            btnClockSpeedSlowerSlower.Click += btnClockSpeedSlowerSlower_Click;
            // 
            // btnClockSpeedSlower
            // 
            btnClockSpeedSlower.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnClockSpeedSlower.Image = (System.Drawing.Image)resources.GetObject("btnClockSpeedSlower.Image");
            btnClockSpeedSlower.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnClockSpeedSlower.Name = "btnClockSpeedSlower";
            btnClockSpeedSlower.Size = new System.Drawing.Size(23, 22);
            btnClockSpeedSlower.Text = "Speed [-]";
            btnClockSpeedSlower.ToolTipText = "Clock Speed -";
            btnClockSpeedSlower.Click += btnSlowDown_Click;
            // 
            // btnClockSpeedDefault
            // 
            btnClockSpeedDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnClockSpeedDefault.Image = (System.Drawing.Image)resources.GetObject("btnClockSpeedDefault.Image");
            btnClockSpeedDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnClockSpeedDefault.Name = "btnClockSpeedDefault";
            btnClockSpeedDefault.Size = new System.Drawing.Size(23, 22);
            btnClockSpeedDefault.Text = "Speed [-]";
            btnClockSpeedDefault.ToolTipText = "Clock Speed -";
            btnClockSpeedDefault.Click += btnClockSpeedDefault_Click;
            // 
            // btnClockSpeedFaster
            // 
            btnClockSpeedFaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnClockSpeedFaster.Image = (System.Drawing.Image)resources.GetObject("btnClockSpeedFaster.Image");
            btnClockSpeedFaster.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnClockSpeedFaster.Name = "btnClockSpeedFaster";
            btnClockSpeedFaster.Size = new System.Drawing.Size(23, 22);
            btnClockSpeedFaster.Text = "Speed [+]";
            btnClockSpeedFaster.ToolTipText = "Clock Speed +";
            btnClockSpeedFaster.Click += btnClockSpeedFaster_Click;
            // 
            // btnClockSpeedFasterFaster
            // 
            btnClockSpeedFasterFaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnClockSpeedFasterFaster.Image = (System.Drawing.Image)resources.GetObject("btnClockSpeedFasterFaster.Image");
            btnClockSpeedFasterFaster.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnClockSpeedFasterFaster.Name = "btnClockSpeedFasterFaster";
            btnClockSpeedFasterFaster.Size = new System.Drawing.Size(23, 22);
            btnClockSpeedFasterFaster.Text = "Speed [+]";
            btnClockSpeedFasterFaster.ToolTipText = "Clock Speed +";
            btnClockSpeedFasterFaster.Click += btnClockSpeedFasterFaster_Click;
            // 
            // separator5
            // 
            separator5.Name = "separator5";
            separator5.Size = new System.Drawing.Size(6, 25);
            // 
            // lblVicIiDebugging
            // 
            lblVicIiDebugging.Name = "lblVicIiDebugging";
            lblVicIiDebugging.Size = new System.Drawing.Size(39, 22);
            lblVicIiDebugging.Text = "VIC-II:";
            lblVicIiDebugging.Visible = false;
            // 
            // btnShowVideoFrameOutlines
            // 
            btnShowVideoFrameOutlines.CheckOnClick = true;
            btnShowVideoFrameOutlines.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnShowVideoFrameOutlines.Image = (System.Drawing.Image)resources.GetObject("btnShowVideoFrameOutlines.Image");
            btnShowVideoFrameOutlines.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnShowVideoFrameOutlines.Name = "btnShowVideoFrameOutlines";
            btnShowVideoFrameOutlines.Size = new System.Drawing.Size(23, 22);
            btnShowVideoFrameOutlines.Text = "Outlines";
            // 
            // btnShowScanLinePosition
            // 
            btnShowScanLinePosition.CheckOnClick = true;
            btnShowScanLinePosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnShowScanLinePosition.Image = (System.Drawing.Image)resources.GetObject("btnShowScanLinePosition.Image");
            btnShowScanLinePosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnShowScanLinePosition.Name = "btnShowScanLinePosition";
            btnShowScanLinePosition.Size = new System.Drawing.Size(23, 22);
            btnShowScanLinePosition.Text = "Scanline Position";
            // 
            // btnShowRasterLineInterrupt
            // 
            btnShowRasterLineInterrupt.CheckOnClick = true;
            btnShowRasterLineInterrupt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnShowRasterLineInterrupt.Image = (System.Drawing.Image)resources.GetObject("btnShowRasterLineInterrupt.Image");
            btnShowRasterLineInterrupt.Name = "btnShowRasterLineInterrupt";
            btnShowRasterLineInterrupt.Size = new System.Drawing.Size(23, 22);
            btnShowRasterLineInterrupt.Text = "Show Raster Line Interrupt";
            // 
            // btnShowFullFrameVideo
            // 
            btnShowFullFrameVideo.CheckOnClick = true;
            btnShowFullFrameVideo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnShowFullFrameVideo.Image = (System.Drawing.Image)resources.GetObject("btnShowFullFrameVideo.Image");
            btnShowFullFrameVideo.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnShowFullFrameVideo.Name = "btnShowFullFrameVideo";
            btnShowFullFrameVideo.Size = new System.Drawing.Size(23, 22);
            btnShowFullFrameVideo.Text = "Show Full Frame";
            btnShowFullFrameVideo.Click += btnShowFullFrameVideo_Click;
            // 
            // separator6
            // 
            separator6.Name = "separator6";
            separator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnToggleSound
            // 
            btnToggleSound.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnToggleSound.Image = (System.Drawing.Image)resources.GetObject("btnToggleSound.Image");
            btnToggleSound.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnToggleSound.Name = "btnToggleSound";
            btnToggleSound.Size = new System.Drawing.Size(23, 22);
            btnToggleSound.Text = "toolStripButton1";
            btnToggleSound.Click += btnShowSidDebugWindow_Click;
            // 
            // ofdOpenFile
            // 
            ofdOpenFile.Filter = "C64-files|*.prg;*.seq;*.crt;*.bin;*.vpl|PRG-files|*.prg|SEQ-files|*.seq|Cartridges|*.crt;*.bin|Cartridge|*.crt|Raw Cartridge|*.bin|VICE Palette File|*.vpl|All files|*.*";
            // 
            // sfd
            // 
            sfd.Filter = "PRG-files|*.prg";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblStatusVic, lblFps, lblVicCycles, lblVicCurrentLine, lblVicCurrentLineCycle, lblVicGraphicsMode, lblVicScreenOn });
            statusStrip1.Location = new System.Drawing.Point(0, 677);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(832, 24);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatusVic
            // 
            lblStatusVic.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblStatusVic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblStatusVic.Name = "lblStatusVic";
            lblStatusVic.Size = new System.Drawing.Size(43, 19);
            lblStatusVic.Text = "VIC-II";
            // 
            // lblFps
            // 
            lblFps.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblFps.Name = "lblFps";
            lblFps.Size = new System.Drawing.Size(36, 19);
            lblFps.Text = "0 fps";
            // 
            // lblVicCycles
            // 
            lblVicCycles.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblVicCycles.Name = "lblVicCycles";
            lblVicCycles.Size = new System.Drawing.Size(52, 19);
            lblVicCycles.Text = "0 cycles";
            // 
            // lblVicCurrentLine
            // 
            lblVicCurrentLine.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblVicCurrentLine.Name = "lblVicCurrentLine";
            lblVicCurrentLine.Size = new System.Drawing.Size(36, 19);
            lblVicCurrentLine.Text = "Line:";
            // 
            // lblVicCurrentLineCycle
            // 
            lblVicCurrentLineCycle.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblVicCurrentLineCycle.Name = "lblVicCurrentLineCycle";
            lblVicCurrentLineCycle.Size = new System.Drawing.Size(33, 19);
            lblVicCurrentLineCycle.Text = "Pos:";
            // 
            // lblVicGraphicsMode
            // 
            lblVicGraphicsMode.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblVicGraphicsMode.Name = "lblVicGraphicsMode";
            lblVicGraphicsMode.Size = new System.Drawing.Size(45, 19);
            lblVicGraphicsMode.Text = "Mode:";
            // 
            // lblVicScreenOn
            // 
            lblVicScreenOn.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            lblVicScreenOn.Name = "lblVicScreenOn";
            lblVicScreenOn.Size = new System.Drawing.Size(49, 19);
            lblVicScreenOn.Text = "Screen:";
            // 
            // ofdInsertCartridge
            // 
            ofdInsertCartridge.Filter = "Cartridges|*.crt;*.bin|Cartridge|*.crt|Raw Cartridge|*.bin|All files|*.*";
            // 
            // menuStrip
            // 
            menuStrip.BackColor = System.Drawing.SystemColors.Control;
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFile, mnuColors });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(832, 24);
            menuStrip.TabIndex = 5;
            menuStrip.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuOpen });
            mnuFile.Name = "mnuFile";
            mnuFile.Size = new System.Drawing.Size(37, 20);
            mnuFile.Text = "File";
            // 
            // mnuOpen
            // 
            mnuOpen.Name = "mnuOpen";
            mnuOpen.Size = new System.Drawing.Size(103, 22);
            mnuOpen.Text = "Open";
            mnuOpen.Click += mnuOpen_Click;
            // 
            // mnuColors
            // 
            mnuColors.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuKernalWhiteTextColor, toolStripSeparator2, mnuBorderColor, mnuBackgroundColor, mnuTextColor, toolStripSeparator1, mnuPalettes, mnuOpenPaletteFolder, mnuImportVICEPaletteFile });
            mnuColors.Name = "mnuColors";
            mnuColors.Size = new System.Drawing.Size(53, 20);
            mnuColors.Text = "Colors";
            mnuColors.DropDownOpening += mnuColors_DropDownOpening;
            // 
            // mnuKernalWhiteTextColor
            // 
            mnuKernalWhiteTextColor.Name = "mnuKernalWhiteTextColor";
            mnuKernalWhiteTextColor.Size = new System.Drawing.Size(197, 22);
            mnuKernalWhiteTextColor.Text = "White Text Color Patch";
            mnuKernalWhiteTextColor.Click += mnuKernalWhiteTextColor_ClickAsync;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(194, 6);
            // 
            // mnuBorderColor
            // 
            mnuBorderColor.BackColor = System.Drawing.SystemColors.Control;
            mnuBorderColor.Name = "mnuBorderColor";
            mnuBorderColor.Size = new System.Drawing.Size(197, 22);
            mnuBorderColor.Text = "Border Color";
            // 
            // mnuBackgroundColor
            // 
            mnuBackgroundColor.BackColor = System.Drawing.SystemColors.Control;
            mnuBackgroundColor.Name = "mnuBackgroundColor";
            mnuBackgroundColor.Size = new System.Drawing.Size(197, 22);
            mnuBackgroundColor.Text = "Background Color";
            // 
            // mnuTextColor
            // 
            mnuTextColor.Name = "mnuTextColor";
            mnuTextColor.Size = new System.Drawing.Size(197, 22);
            mnuTextColor.Text = "Text Color";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
            // 
            // mnuPalettes
            // 
            mnuPalettes.Name = "mnuPalettes";
            mnuPalettes.Size = new System.Drawing.Size(197, 22);
            mnuPalettes.Text = "Palettes";
            // 
            // mnuOpenPaletteFolder
            // 
            mnuOpenPaletteFolder.Name = "mnuOpenPaletteFolder";
            mnuOpenPaletteFolder.Size = new System.Drawing.Size(197, 22);
            mnuOpenPaletteFolder.Text = "Open Palette Folder";
            mnuOpenPaletteFolder.Click += mnuOpenPaletteFolder_Click;
            // 
            // mnuImportVICEPaletteFile
            // 
            mnuImportVICEPaletteFile.Name = "mnuImportVICEPaletteFile";
            mnuImportVICEPaletteFile.Size = new System.Drawing.Size(197, 22);
            mnuImportVICEPaletteFile.Text = "Import VICE Palette File";
            mnuImportVICEPaletteFile.Click += mnuImportVICEPaletteFile_Click;
            // 
            // ofdImportVicePaletteFile
            // 
            ofdImportVicePaletteFile.Filter = "VICE Palette File|*.vpl";
            // 
            // fsw
            // 
            fsw.EnableRaisingEvents = true;
            fsw.NotifyFilter = System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName | System.IO.NotifyFilters.LastWrite | System.IO.NotifyFilters.CreationTime;
            fsw.SynchronizingObject = this;
            fsw.Changed += fsw_Changed;
            // 
            // FormC64Screen
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(832, 725);
            Controls.Add(statusStrip1);
            Controls.Add(toolMain);
            Controls.Add(statusMain);
            Controls.Add(menuStrip);
            Controls.Add(pScreen);
            DoubleBuffered = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormC64Screen";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Retrocomputing.NET - Commodore 64";
            FormClosing += FormC64Screen_FormClosing;
            Load += FormC64Screen_Load;
            KeyDown += FormC64Screen_KeyDown;
            KeyUp += FormC64Screen_KeyUp;
            ((System.ComponentModel.ISupportInitialize)pScreen).EndInit();
            statusMain.ResumeLayout(false);
            statusMain.PerformLayout();
            toolMain.ResumeLayout(false);
            toolMain.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)fsw).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuBorderColor;
        private System.Windows.Forms.ToolStripMenuItem mnuBackgroundColor;
        private System.Windows.Forms.ToolStripMenuItem mnuTextColor;
    }
}