using Commodore64;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Commodore64.Properties;
using Timer = System.Threading.Timer;
using Debugger;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Commodore64.Vic;
using Commodore64.Cartridge.FileFormats.Crt;
using System.Threading.Tasks;
using Commodore64.Cartridge.FileFormats.Raw;
using Commodore64.Cartridge;
using Commodore64.Vic.Colors;

namespace ComputerSystem.Commodore64 {
    public partial class FormC64Screen : Form {

        public C64 C64 { get; set; }

        private readonly Stopwatch _stopWatch = new Stopwatch();

        private Thread _invalidateScreenThread;

        private double _fpsTarget = 50.0f;
        private double _fpsActual = 0.0f;
        private Queue<double> _fpsValues = new Queue<double>();

        private Bitmap _bC64ScreenOutputBuffer;
        private Graphics _gC64ScreenOutputBuffer;

        private readonly Pen _penWhite = new Pen(Color.White) { DashStyle = DashStyle.Dot };
        private readonly Pen _penRaster = new Pen(Color.Red) { DashStyle = DashStyle.Dash };

        private Timer _uiRefreshTimer;
        private Bitmap _bC64ScreenBuffer;
        private Graphics _gC64ScreenBuffer;
        private Image _crtImage;
        private OsdManager _osdManager;

        private bool _isInitialized = false;
        private bool _formIsClosing = false;
        private bool _fswBusy = false;


        public FormC64Screen(C64 c64) {
            InitializeComponent();

            _osdManager = new OsdManager();
            _osdManager.AddItem("Power On");

            C64 = c64;

            _crtImage = Image.FromFile("Images\\crt-overlay-03.png");

            _bC64ScreenBuffer = new Bitmap(VicIi.FULL_WIDTH, VicIi.FULL_HEIGHT_PAL, PixelFormat.Format24bppRgb);
            _gC64ScreenBuffer = Graphics.FromImage(_bC64ScreenBuffer);
            _bC64ScreenOutputBuffer = new Bitmap(pScreen.Width, pScreen.Height, PixelFormat.Format24bppRgb);
            _gC64ScreenOutputBuffer = Graphics.FromImage(_bC64ScreenOutputBuffer);

            _uiRefreshTimer = new Timer((e) => {

                try {
                    Invoke(new Action(() => {
                        lblClockSpeed.Text = $"{c64.CpuClockSpeedHz / 1000000:F4} MHz";
                        lblClockSpeedReal.Text = $"{c64.CpuClockSpeedRealHz / 1000000:F4} MHz";
                        lblClockSpeedRealPercent.Text = $"{c64.CpuClockSpeedPercent:F2} %";
                        lblCpuClockSpeedMultiplier.Text = $"x{c64.CpuClockSpeedMultiplier:F2}";

                        lblCycles.Text = $"{c64.Cpu.TotalCycles:N0} cycles";
                        lblInstructions.Text = $"{c64.Cpu.TotalInstructions:N0} instructions";
                        lblIllegalInstructions.Text = $"{c64.Cpu.TotalIllegalInstructions:N0} illegal instructions";
                        lblKeyboardDisabled.Visible = !c64.KeyboardActivated;

                        lblFps.Text = $"{((int)_fpsActual):D2} fps";
                        lblVicCycles.Text = $"{c64.Vic.TotalCycles:N0} cycles";

                        lblVicCurrentLine.Text = $"Line: {c64.Vic.CurrentLine:D3}";
                        lblVicCurrentLineCycle.Text = $"Pos: {c64.Vic.CurrentLineCycle:D2}";

                        lblVicGraphicsMode.Text = "Mode: " + C64.Vic.GetCurrentGraphicsMode().ToString();
                        lblVicScreenOn.Text = C64.Vic.ScreenControlRegisterScreenOffOn ? "Screen: On" : "Screen: Off";
                    }));
                } catch { }

            }, null, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(50));

            c64.PowerOn();

            _isInitialized = true;
        }

        private void LoadSettings()
        {
            mnuKernalWhiteTextColor.Checked = Settings.Default.KernalWhiteTextColor;
            btnUseCrtFilter.Checked = Settings.Default.UseCrtFilter;
            btnShowOnScreenDisplay.Checked = Settings.Default.ShowOnScreenDisplay;
        }

        private void SaveSettings()
        {
            Settings.Default.KernalWhiteTextColor = mnuKernalWhiteTextColor.Checked;
            Settings.Default.UseCrtFilter = btnUseCrtFilter.Checked;
            Settings.Default.ShowOnScreenDisplay = btnShowOnScreenDisplay.Checked;
            Settings.Default.Save();
        }

        private void FormC64Screen_Load(object sender, EventArgs e) {
            LoadSettings();

            pScreen.AllowDrop = true;

            _invalidateScreenThread = new Thread(InvalidateScreen);
            _invalidateScreenThread.Start();
        }

        private void InvalidateScreen() {
            var sw = Stopwatch.StartNew();

            var period = 1000.0f / _fpsTarget;

            while (!_formIsClosing) {
                if (!Visible || WindowState == FormWindowState.Minimized) continue;

                sw.Restart();

                try
                {
                    Invoke(new Action(() =>
                    {
                        pScreen.Invalidate();
                    }));
                }
                catch (Exception)
                {
                }

                while (sw.Elapsed.TotalMilliseconds < period)
                {
                }
            }
        }

        public void ApplyCrtFilter() {
            _gC64ScreenOutputBuffer.DrawImage(_crtImage, 0, 0,
                _bC64ScreenOutputBuffer.Width + 1, _bC64ScreenOutputBuffer.Height + 1);
        }

        private void PScreen_Paint(object sender, PaintEventArgs e) {
            SetPixels(_bC64ScreenBuffer, C64.Vic.ScreenBufferPixels);

            if (btnShowVideoFrameOutlines.Checked) {
                _gC64ScreenBuffer.DrawRectangle(_penWhite, C64.Vic.FullFrame.X, C64.Vic.FullFrame.Y, C64.Vic.FullFrame.Width, C64.Vic.FullFrame.Height);
                _gC64ScreenBuffer.DrawRectangle(_penWhite, C64.Vic.BorderFrame.X, C64.Vic.BorderFrame.Y, C64.Vic.BorderFrame.Width, C64.Vic.BorderFrame.Height);
                _gC64ScreenBuffer.DrawRectangle(_penWhite, C64.Vic.DisplayFrame.X, C64.Vic.DisplayFrame.Y, C64.Vic.DisplayFrame.Width, C64.Vic.DisplayFrame.Height);
            }

            if (btnShowRasterLineInterrupt.Checked && C64.Vic.InterruptControlRegisterRasterInterruptEnabled) {
                _gC64ScreenBuffer.DrawLine(_penRaster, 0, C64.Vic.RasterLineToGenerateInterruptAt, C64.Vic.FullFrame.Width, C64.Vic.RasterLineToGenerateInterruptAt);
            }

            if (btnShowScanLinePosition.Checked) {
                var p = C64.Vic.GetScanlinePoint();
                _gC64ScreenBuffer.DrawLine(_penWhite, p.X, p.Y, p.X + 8, p.Y);
            }

            if (btnShowFullFrameVideo.Checked) {
                _gC64ScreenOutputBuffer.DrawImage(_bC64ScreenBuffer, 0, 0, _bC64ScreenOutputBuffer.Width, _bC64ScreenOutputBuffer.Height);
            } else {
                _gC64ScreenOutputBuffer.DrawImage(_bC64ScreenBuffer, new Rectangle(0, 0, _bC64ScreenOutputBuffer.Width, _bC64ScreenOutputBuffer.Height),
                    new Rectangle(C64.Vic.BorderFrame.X, C64.Vic.BorderFrame.Y, C64.Vic.BorderFrame.Width, C64.Vic.BorderFrame.Height), GraphicsUnit.Pixel);
            }

            if (btnShowOnScreenDisplay.Checked) _gC64ScreenOutputBuffer.DrawImage(_osdManager.OsdBitmap, 0, 0, pScreen.Width, pScreen.Height);
            if (btnUseCrtFilter.Checked) ApplyCrtFilter();

            e.Graphics.CompositingMode = CompositingMode.SourceCopy;
            e.Graphics.DrawImage(_bC64ScreenOutputBuffer, 0, 0, pScreen.Width, pScreen.Height);

            _stopWatch.Stop();
            _fpsValues.Enqueue(_stopWatch.Elapsed.TotalMilliseconds);
            _stopWatch.Restart();
            
            if (_fpsValues.Count > 15) _fpsValues.Dequeue();
            _fpsActual = 1000f / _fpsValues.Average(x => x);
        }

        private void PScreen_Resize(object sender, EventArgs e) {
            if (!_isInitialized) return;
            if (WindowState == FormWindowState.Minimized) return;

            _bC64ScreenOutputBuffer.Dispose();
            _gC64ScreenOutputBuffer.Dispose();

            _bC64ScreenOutputBuffer = new Bitmap(pScreen.Width, pScreen.Height, PixelFormat.Format24bppRgb);
            _gC64ScreenOutputBuffer = Graphics.FromImage(_bC64ScreenOutputBuffer);

            //_gC64ScreenOutputBuffer.SmoothingMode = SmoothingMode.AntiAlias;
            //_gC64ScreenOutputBuffer.InterpolationMode = InterpolationMode.NearestNeighbor;
            //_gC64ScreenOutputBuffer.PixelOffsetMode = PixelOffsetMode.HighSpeed;
        }

        private void FormC64Screen_FormClosing(object sender, FormClosingEventArgs e) {
            _formIsClosing = true;

            _gC64ScreenOutputBuffer.Dispose();
            _bC64ScreenOutputBuffer.Dispose();

            C64.PowerOff();

            SaveSettings();
        }

        private async void BtnRestart_Click(object sender, EventArgs e) {
            _osdManager.AddItem("Power Cycle");
            await C64.PowerOff();
            C64.PowerOn();
        }


        private void BtnSave_Click(object sender, EventArgs e) {
            if (sfd.ShowDialog() == DialogResult.OK) {

                var basicAreaLength = C64MemoryOffsets.DEFAULT_BASIC_AREA_END - C64MemoryOffsets.DEFAULT_BASIC_AREA_START;
                var data = new List<byte>();

                data.AddRange(BitConverter.GetBytes((ushort)0x0801));

                for (int i = 0; i < basicAreaLength; i++) {
                    data.Add(C64.Memory[C64MemoryOffsets.DEFAULT_BASIC_AREA_START + i]);

                    // TODO: Fix this horrible check
                    // The BASIC program ends with 3 NULL-bytes, so break when we find them
                    if (data.Count >= 3 && data.Skip(data.Count - 3).Take(3).All(x => x == 0x00)) {
                        break;
                    }
                }

                File.WriteAllBytes(sfd.FileName, data.ToArray());
            }
        }

        private void BtnCopyOutput_ButtonClick(object sender, EventArgs e) {
            Clipboard.SetImage(_bC64ScreenOutputBuffer);
        }

        private void BtnCopyRawOutput_Click(object sender, EventArgs e) {
            Clipboard.SetImage(_bC64ScreenBuffer);
        }

        private void BtnMemoryWatch_Click(object sender, EventArgs e) {
            var f = new FormDebugger(C64.Cpu, C64.Memory);
            f.Show();
        }

        private async void BtnPause_ClickAsync(object sender, EventArgs e) {

            if (btnPause.Checked) {
                var r = await C64.Cpu.Pause();
                _osdManager.AddItem("Pause");

            } else {

                C64.Cpu.Resume();
                _osdManager.AddItem("Resume");
            }

        }

        private void FormC64Screen_Activated(object sender, EventArgs e) {
            C64.KeyboardActivated = true;
        }

        private void FormC64Screen_Deactivate(object sender, EventArgs e) {
            C64.KeyboardActivated = false;
        }

        private async void BtnReset_Click(object sender, EventArgs e) {
            await C64.Cpu.Pause();
            C64.Cpu.Reset();
            C64.Cpu.Resume();

            _osdManager.AddItem("Reset");
        }

        private async void pScreen_DragDropAsync(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] d && d.Length > 0)
            {
                var fileName = d.First();
                if (!File.Exists(fileName)) return;

                await HandleFileOpen(fileName);
            }
        }

        private void pScreen_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private async void BtnOpen_Click(object sender, EventArgs e)
        {
            if (ofdOpenFile.ShowDialog() == DialogResult.OK)
            {
                await HandleFileOpen(ofdOpenFile.FileName);
            }
        }

        private async void btnInsertCartridge_ClickAsync(object sender, EventArgs e)
        {
            if (btnInsertCartridge.Checked)
            {
                await RemoveCartridge();
                return;
            }

            if (ofdInsertCartridge.ShowDialog() == DialogResult.OK)
            {
                await HandleFileOpen(ofdInsertCartridge.FileName);
            }
        }

        private async Task HandleFileOpen(string fileName)
        {
            var ext = Path.GetExtension(fileName);

            switch (ext.ToLower())
            {
                case ".prg":
                    await C64.Cpu.Pause();
                    LoadPrg(fileName, true);
                    C64.Cpu.Resume();
                    _osdManager.AddItem($"Loaded {Path.GetFileName(fileName)}");
                    fsw.Path = Path.GetDirectoryName(fileName);
                    break;

                case ".crt":
                case ".bin":
                    await InsertCartridge(fileName);
                    _osdManager.AddItem($"Inserted {Path.GetFileName(fileName)}");
                    break;

                case ".vpl": // VICE Palette file
                    ColorManager.LoadPalette(PaletteDefinition.FromVicePaletteFile(fileName));
                    _osdManager.AddItem($"Loaded {Path.GetFileName(fileName)}");
                    break;

                case ".json": // Just color palettes for now
                    ColorManager.LoadPalette(PaletteDefinition.FromFile(fileName));
                    _osdManager.AddItem($"Loaded {Path.GetFileName(fileName)}");
                    break;

                default:
                    // Using Task.Run to prevent a bug where the window locks up when using drag and drop
                    _osdManager.AddItem($"Unknown file format!");
                    await Task.Run(() => MessageBox.Show("Unknown file format.", "Unknown", MessageBoxButtons.OK, MessageBoxIcon.Warning));
                    return;
            }
        }

        private async void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            if (_fswBusy) return;
            if (e.ChangeType != WatcherChangeTypes.Changed) return;

            _fswBusy = true;

            var ext = Path.GetExtension(e.Name);

            switch (ext.ToLower())
            {
                case ".prg":
                    await C64.PowerOff();
                    C64.PowerOn();

                    // TODO: Fix hacky solution to wait for the BASIC prompt before continuing
                    await Task.Delay(3000);

                    await C64.Cpu.Pause();
                    LoadPrg(e.FullPath, true);
                    C64.Cpu.Resume();

                    _osdManager.AddItem($"Loaded {Path.GetFileName(e.FullPath)}");
                    break;

                default:
                    break;
            }

            _fswBusy = false;
        }

        private void LoadPrg(string fileName, bool executeRun)
        {
            var file = File.ReadAllBytes(fileName);

            var address = BitConverter.ToUInt16(file, 0);
            var data = file.Skip(2).ToArray();

            for (int i = 0; i < data.Length; i++)
            {
                C64.Memory._memory[address + i] = data[i];
            }

            if (executeRun)
            {
                // Put "RUN" + {RETURN} directly into the BASIC keyboard
                // buffer and set the buffer length, BASIC does the rest!
                C64.Memory._memory[0x0277] = (byte)'R';
                C64.Memory._memory[0x0278] = (byte)'U';
                C64.Memory._memory[0x0279] = (byte)'N';
                C64.Memory._memory[0x027A] = 13; // {RETURN}
                C64.Memory._memory[0x00C6] = 4;
            }
        }

        private async Task<ICartridge> InsertCartridge(string fileName)
        {
            var ext = Path.GetExtension(fileName);

            ICartridge crt;

            switch (ext.ToLower())
            {
                case ".crt":
                    crt = CartridgeCrt.FromFile(fileName);
                    C64.Cartridge = crt;
                    break;

                case ".bin":
                    crt = CartridgeRaw.FromFile(fileName);
                    C64.Cartridge = crt;
                    break;

                default:
                    return null;

            }

            btnInsertCartridge.Text = $"{crt.Name}";
            btnInsertCartridge.Checked = true;

            await C64.PowerOff();
            C64.PowerOn();

            return crt;
        }

        private async Task RemoveCartridge()
        {
            C64.Cartridge = null;
            btnInsertCartridge.Text = "";
            btnInsertCartridge.Checked = false;

            await C64.PowerOff();
            C64.PowerOn();
        }

        public void SetPixels(Bitmap b, Color[,] pixels) {
            var width = b.Width;
            var height = b.Height;

            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = data.Stride;

            unsafe {
                byte* ptr = (byte*)data.Scan0;

                for (int y = 0; y < height; y++) {

                    for (int x = 0; x < width; x++) {

                        var index = (y * width) + x;

                        ptr[(x * 3) + y * stride] = pixels[y, x].B;
                        ptr[(x * 3) + y * stride + 1] = pixels[y, x].G;
                        ptr[(x * 3) + y * stride + 2] = pixels[y, x].R;
                    }

                }
            }

            b.UnlockBits(data);
        }

        private void AdjustSpeedMultiplier(float value)
        {
            C64.CpuClockSpeedMultiplier += value;

            if (C64.CpuClockSpeedMultiplier <= C64.CpuClockSpeedMultiplierMin)
            {
                C64.CpuClockSpeedMultiplier = C64.CpuClockSpeedMultiplierMin;
                return;
            }

            if (C64.CpuClockSpeedMultiplier >= C64.CpuClockSpeedMultiplierMax)
            {
                C64.CpuClockSpeedMultiplier = C64.CpuClockSpeedMultiplierMax;
                return;
            }
        }

        private void btnSlowDown_Click(object sender, EventArgs e) {
            AdjustSpeedMultiplier(-0.01f);
        }

        private void btnClockSpeedFaster_Click(object sender, EventArgs e) {
            AdjustSpeedMultiplier(0.01f);
        }

        private void btnClockSpeedSlowerSlower_Click(object sender, EventArgs e)
        {
            AdjustSpeedMultiplier(-0.10f);
        }

        private void btnClockSpeedFasterFaster_Click(object sender, EventArgs e)
        {
            AdjustSpeedMultiplier(0.10f);
        }
        private void btnClockSpeedDefault_Click(object sender, EventArgs e)
        {
            C64.CpuClockSpeedMultiplier = 1f;
        }

        private void pScreen_DoubleClick(object sender, EventArgs e) {
            ResizeToCorrectAspectRatio();
        }


        private void ResizeToCorrectAspectRatio() {
            var height = (int)((pScreen.Width / 4.0f) * 3.0f);

            if (height < pScreen.Height) Height -= pScreen.Height - height;
            if (height > pScreen.Height) Height += height - pScreen.Height;

            // Compensate for full frame mode
            if (btnShowFullFrameVideo.Checked) {
                Width += C64.Vic.FullFrame.Width - C64.Vic.BorderFrame.Width;
            }
        }

        private void ToggleFullscreen() {
            FormBorderStyle = FormBorderStyle == FormBorderStyle.Sizable ? FormBorderStyle.None : FormBorderStyle.Sizable;
            WindowState = WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal;

            menuStrip.Visible = FormBorderStyle == FormBorderStyle.None ? false : true;
            toolMain.Visible = FormBorderStyle == FormBorderStyle.None ? false : true;
            statusMain.Visible = FormBorderStyle == FormBorderStyle.None ? false : true;
            statusStrip1.Visible = FormBorderStyle == FormBorderStyle.None ? false : true;
            pScreen.Dock = FormBorderStyle == FormBorderStyle.None ? DockStyle.Fill : DockStyle.None;
            pScreen.Anchor = FormBorderStyle != FormBorderStyle.None ? AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right : AnchorStyles.Top | AnchorStyles.Left;            
        }

        private void pScreen_MouseMove(object sender, MouseEventArgs e) {
            if (FormBorderStyle == FormBorderStyle.None) {
                toolMain.Visible = e.Y < 5 ? true : false;
                statusMain.Visible = e.Y > pScreen.Height - 5 ? true : false;
                statusStrip1.Visible = e.Y > pScreen.Height - 5 ? true : false;
            }
        }

        private void btnToggleFullscreen_Click(object sender, EventArgs e) {
            ToggleFullscreen();
        }

        private void pScreen_Click(object sender, EventArgs e) {

        }

        private void btnShowFullFrameVideo_Click(object sender, EventArgs e) {
            ResizeToCorrectAspectRatio();
        }

        private void FormC64Screen_KeyUp(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.V) {
                var text = Clipboard.GetText();
                PasteText(text);
            }
        }

        private void PasteText(string text) {
            if (string.IsNullOrWhiteSpace(text)) return;

            for (int i = 0; i < text.Length; i++) {
                C64.Memory._memory[0x0277] = (byte)text[i];
                C64.Memory._memory[0x00C6] = 1;

                while (C64.Memory._memory[0x00C6] != 0)
                {
                    Thread.Sleep(1);
                }
            }
        }

        private void mnuColors_DropDownOpening(object sender, EventArgs e)
        {
            mnuPalettes.DropDownItems.Clear();

            var paths = Directory.GetFiles("Palettes", "*.json");

            foreach (var path in paths)
            {
                var fileName = Path.GetFileName(path);

                mnuPalettes.DropDownItems.Add(
                    new ToolStripMenuItem(
                        Path.GetFileNameWithoutExtension(path),
                        null,
                        (s, e) =>
                        {
                            ColorManager.LoadPalette(PaletteDefinition.FromFile(fileName));
                            Settings.Default.CurrentColorPalette = fileName;
                        }
                    )
                    {
                        Checked = fileName == Settings.Default.CurrentColorPalette
                    }
                );
            }
        }

        private void mnuImportVICEPaletteFile_Click(object sender, EventArgs e)
        {
            if (ofdImportVicePaletteFile.ShowDialog() == DialogResult.OK)
            {
                var fn = ofdImportVicePaletteFile.FileName;
                var pd = PaletteDefinition.ImportVicePaletteFile(fn);
                ColorManager.LoadPalette(pd);
                Settings.Default.CurrentColorPalette = $"{Path.GetFileNameWithoutExtension(fn)}.json";
            }
        }

        private void mnuOpenPaletteFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Path.GetFullPath(@"Palettes\"));
        }

        private async void mnuKernalWhiteTextColor_ClickAsync(object sender, EventArgs e)
        {
            Settings.Default.KernalWhiteTextColor = !Settings.Default.KernalWhiteTextColor;
            mnuKernalWhiteTextColor.Checked = Settings.Default.KernalWhiteTextColor;

            await Task.Run(async () => {
                if (MessageBox.Show(
                    "This setting requires a restart. Do you want to restart now?",
                    "Restart",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes) {

                    await C64.PowerOff();
                    C64.PowerOn();
                }
            });


        }

        private void btnShowOnScreenDisplay_Click(object sender, EventArgs e)
        {
            if (btnShowOnScreenDisplay.Checked) _osdManager.AddItem("On Screen Display On");
        }
    }
}
