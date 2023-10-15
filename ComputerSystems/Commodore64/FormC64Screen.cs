using Commodore64;
using Commodore64.Cartridge;
using Commodore64.Cartridge.FileFormats.Crt;
using Commodore64.Cartridge.FileFormats.Raw;
using Commodore64.Keyboard;
using Commodore64.Properties;
using Commodore64.Sid.Debug;
using Commodore64.Vic;
using Commodore64.Vic.Colors;
using Debugger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Image = System.Drawing.Image;
using Path = System.IO.Path;
using Rectangle = System.Drawing.Rectangle;
using Timer = System.Threading.Timer;

namespace ComputerSystem.Commodore64
{
    public partial class FormC64Screen : Form, IC64KeyboardInputProvider
    {
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

        /// <summary>
        /// Keeps track of the currently pressed keys, used by IC64KeyboardInputProvider
        /// </summary>
        private List<Keys> _keysDown = new List<Keys>();


        public FormC64Screen(C64 c64)
        {
            InitializeComponent();

            _osdManager = new OsdManager();
            _osdManager.AddItem("Power On");

            C64 = c64;
            C64.C64KeyboardInputProvider = this;

            _crtImage = Image.FromFile("Images\\crt-overlay-03.png");

            _bC64ScreenBuffer = new Bitmap(VicIi.FULL_WIDTH, VicIi.FULL_HEIGHT_PAL, PixelFormat.Format24bppRgb);
            _gC64ScreenBuffer = Graphics.FromImage(_bC64ScreenBuffer);
            _bC64ScreenOutputBuffer = new Bitmap(pScreen.Width, pScreen.Height, PixelFormat.Format24bppRgb);
            _gC64ScreenOutputBuffer = Graphics.FromImage(_bC64ScreenOutputBuffer);

            _uiRefreshTimer = new Timer((e) =>
            {

                try
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        lblClockSpeed.Text = $"{c64.CpuClockSpeedHz / 1000000:F4} MHz";
                        lblClockSpeedReal.Text = $"{c64.CpuClockSpeedRealHz / 1000000:F4} MHz";
                        lblClockSpeedRealPercent.Text = $"{c64.CpuClockSpeedPercent:F2} %";
                        lblCpuClockSpeedMultiplier.Text = $"x{c64.CpuClockSpeedMultiplier:F2}";

                        lblCycles.Text = $"{c64.Cpu.TotalCycles:N0} cycles";
                        lblInstructions.Text = $"{c64.Cpu.TotalInstructions:N0} instructions";
                        lblIllegalInstructions.Text = $"{c64.Cpu.TotalIllegalInstructions:N0} illegal instructions";

                        lblFps.Text = $"{((int)_fpsActual):D2} fps";
                        lblVicCycles.Text = $"{c64.Vic.TotalCycles:N0} cycles";

                        lblVicCurrentLine.Text = $"Line: {c64.Vic.CurrentLine:D3}";
                        lblVicCurrentLineCycle.Text = $"Pos: {c64.Vic.CurrentLineCycle:D2}";

                        lblVicGraphicsMode.Text = "Mode: " + C64.Vic.GetCurrentGraphicsMode().ToString();
                        lblVicScreenOn.Text = C64.Vic.ScreenControlRegisterScreenOffOn ? "Screen: On" : "Screen: Off";
                    }));
                }
                catch { }

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

        private void FormC64Screen_Load(object sender, EventArgs e)
        {
            LoadSettings();

            pScreen.AllowDrop = true;

            _invalidateScreenThread = new Thread(InvalidateScreen);
            _invalidateScreenThread.Start();
        }

        private void InvalidateScreen()
        {
            var sw = Stopwatch.StartNew();

            var period = 1000.0f / _fpsTarget;

            while (!_formIsClosing)
            {
                if (!Visible || WindowState == FormWindowState.Minimized) continue;

                sw.Restart();

                try
                {
                    BeginInvoke(new MethodInvoker(() =>
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

        public void ApplyCrtFilter()
        {
            _gC64ScreenOutputBuffer.DrawImage(_crtImage, 0, 0,
                _bC64ScreenOutputBuffer.Width + 1, _bC64ScreenOutputBuffer.Height + 1);
        }

        private void PScreen_Paint(object sender, PaintEventArgs e)
        {
            SetPixels(_bC64ScreenBuffer, C64.Vic.ScreenBufferPixels);

            if (btnShowVideoFrameOutlines.Checked)
            {
                _gC64ScreenBuffer.DrawRectangle(_penWhite, C64.Vic.FullFrame.X, C64.Vic.FullFrame.Y, C64.Vic.FullFrame.Width, C64.Vic.FullFrame.Height);
                _gC64ScreenBuffer.DrawRectangle(_penWhite, C64.Vic.BorderFrame.X, C64.Vic.BorderFrame.Y, C64.Vic.BorderFrame.Width, C64.Vic.BorderFrame.Height);
                _gC64ScreenBuffer.DrawRectangle(_penWhite, C64.Vic.DisplayFrame.X, C64.Vic.DisplayFrame.Y, C64.Vic.DisplayFrame.Width, C64.Vic.DisplayFrame.Height);
            }

            if (btnShowRasterLineInterrupt.Checked && C64.Vic.InterruptControlRegisterRasterInterruptEnabled)
            {
                _gC64ScreenBuffer.DrawLine(_penRaster, 0, C64.Vic.RasterLineToGenerateInterruptAt, C64.Vic.FullFrame.Width, C64.Vic.RasterLineToGenerateInterruptAt);
            }

            if (btnShowScanLinePosition.Checked)
            {
                var p = C64.Vic.GetScanlinePoint();
                _gC64ScreenBuffer.DrawLine(_penWhite, p.X, p.Y, p.X + 8, p.Y);
            }

            if (btnShowFullFrameVideo.Checked)
            {
                _gC64ScreenOutputBuffer.DrawImage(_bC64ScreenBuffer, 0, 0, _bC64ScreenOutputBuffer.Width, _bC64ScreenOutputBuffer.Height);
            }
            else
            {
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

        private void PScreen_Resize(object sender, EventArgs e)
        {
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

        private void FormC64Screen_FormClosing(object sender, FormClosingEventArgs e)
        {
            _formIsClosing = true;

            _gC64ScreenOutputBuffer.Dispose();
            _bC64ScreenOutputBuffer.Dispose();

            C64.PowerOff();

            SaveSettings();
        }

        private async void BtnRestart_Click(object sender, EventArgs e)
        {
            _osdManager.AddItem("Power Cycle");
            await C64.PowerOff();
            C64.Sid.Stop();
            C64.Sid.Reset();
            C64.PowerOn();
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var basicAreaLength = C64MemoryOffsets.DEFAULT_BASIC_AREA_END - C64MemoryOffsets.DEFAULT_BASIC_AREA_START;
                var data = new List<byte>();

                data.AddRange(BitConverter.GetBytes((ushort)0x0801));

                for (int i = 0; i < basicAreaLength; i++)
                {
                    data.Add(C64.Memory[C64MemoryOffsets.DEFAULT_BASIC_AREA_START + i]);

                    // TODO: Fix this horrible check
                    // The BASIC program ends with 3 NULL-bytes, so break when we find them
                    if (data.Count >= 3 && data.Skip(data.Count - 3).Take(3).All(x => x == 0x00))
                    {
                        break;
                    }
                }

                File.WriteAllBytes(sfd.FileName, data.ToArray());
            }
        }

        private void BtnCopyOutput_ButtonClick(object sender, EventArgs e)
        {
            Clipboard.SetImage(_bC64ScreenOutputBuffer);
        }

        private void BtnCopyRawOutput_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(_bC64ScreenBuffer);
        }

        private void BtnMemoryWatch_Click(object sender, EventArgs e)
        {
            var f = new FormDebugger(C64.Cpu, C64.Memory);
            f.Show();
        }

        private async void BtnPause_ClickAsync(object sender, EventArgs e)
        {

            if (btnPause.Checked)
            {
                C64.Sid.Pause();
                var r = await C64.Cpu.Pause();
                _osdManager.AddItem("Pause");

            }
            else
            {
                C64.Cpu.Resume();
                C64.Sid.Play();
                _osdManager.AddItem("Resume");
            }

        }

        private async void BtnReset_Click(object sender, EventArgs e)
        {
            await C64.Cpu.Pause();
            C64.Cpu.Reset();
            C64.Sid.Reset();

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
                case ".prg": // Programs
                    await C64.Cpu.Pause();
                    LoadPrg(fileName, true);
                    C64.Cpu.Resume();
                    fsw.Path = Path.GetDirectoryName(fileName);
                    break;

                case ".crt": // Cartridges
                case ".bin":
                    await InsertCartridge(fileName);
                    break;

                case ".seq": // Sequential files
                    await LoadSequentialFile(fileName);
                    break;

                case ".vpl": // VICE Palette file
                    ColorManager.LoadPalette(PaletteDefinition.FromVicePaletteFile(fileName));
                    Settings.Default.CurrentColorPalette = $"{Path.GetFileNameWithoutExtension(fileName)}.json";
                    break;

                case ".json": // Just color palettes for now
                    ColorManager.LoadPalette(PaletteDefinition.FromFile(fileName));
                    break;

                default:
                    // Using Task.Run to prevent a bug where the window locks up when using drag and drop
                    await Task.Run(() => MessageBox.Show("Unknown file format.", "Unknown", MessageBoxButtons.OK, MessageBoxIcon.Warning)); return;
            }

            _osdManager.AddItem($"Loaded {Path.GetFileName(fileName)}");
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
                C64.Memory._memory[0x00C6] = 4; // Keyboard buffer length
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

        public void SetPixels(Bitmap b, Color[,] pixels)
        {
            var width = b.Width;
            var height = b.Height;

            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = data.Stride;

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;

                for (int y = 0; y < height; y++)
                {

                    for (int x = 0; x < width; x++)
                    {

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

        private void btnSlowDown_Click(object sender, EventArgs e)
        {
            AdjustSpeedMultiplier(-0.01f);
        }

        private void btnClockSpeedFaster_Click(object sender, EventArgs e)
        {
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

        private void pScreen_DoubleClick(object sender, EventArgs e)
        {
            ResizeToCorrectAspectRatio();
        }


        private void ResizeToCorrectAspectRatio()
        {
            var height = (int)((pScreen.Width / 4.0f) * 3.0f);

            if (height < pScreen.Height) Height -= pScreen.Height - height;
            if (height > pScreen.Height) Height += height - pScreen.Height;

            // Compensate for full frame mode
            if (btnShowFullFrameVideo.Checked)
            {
                Width += C64.Vic.FullFrame.Width - C64.Vic.BorderFrame.Width;
            }
        }

        private void ToggleFullscreen()
        {
            FormBorderStyle = FormBorderStyle == FormBorderStyle.Sizable ? FormBorderStyle.None : FormBorderStyle.Sizable;
            WindowState = WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal;

            menuStrip.Visible = FormBorderStyle == FormBorderStyle.None ? false : true;
            toolMain.Visible = FormBorderStyle == FormBorderStyle.None ? false : true;
            statusMain.Visible = FormBorderStyle == FormBorderStyle.None ? false : true;
            statusStrip1.Visible = FormBorderStyle == FormBorderStyle.None ? false : true;
            pScreen.Dock = FormBorderStyle == FormBorderStyle.None ? DockStyle.Fill : DockStyle.None;
            pScreen.Anchor = FormBorderStyle != FormBorderStyle.None ? AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right : AnchorStyles.Top | AnchorStyles.Left;
        }

        private void pScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (FormBorderStyle == FormBorderStyle.None)
            {
                toolMain.Visible = e.Y < 5 ? true : false;
                statusMain.Visible = e.Y > pScreen.Height - 5 ? true : false;
                statusStrip1.Visible = e.Y > pScreen.Height - 5 ? true : false;
            }
        }

        private void btnToggleFullscreen_Click(object sender, EventArgs e)
        {
            ToggleFullscreen();
        }

        private void btnShowFullFrameVideo_Click(object sender, EventArgs e)
        {
            ResizeToCorrectAspectRatio();
        }

        private async Task PasteText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            // Remove \n chars
            text = text.Replace("\r\n", "\r");

            // 0x0277: Keyboard buffer (10 bytes, 10 entries).
            int chunkLength = 10;
            int offset = 0;

            while (offset < text.Length)
            {
                if (offset + chunkLength > text.Length) chunkLength = text.Length - offset;
                Array.Copy(Encoding.ASCII.GetBytes(text), offset, C64.Memory._memory, 0x0277, chunkLength);

                // Wait for BASIC to process the keyboard buffer
                C64.Memory._memory[0x00C6] = (byte)chunkLength;
                while (C64.Memory._memory[0x00C6] != 0)
                {
                    await Task.Delay(1);
                }

                offset += chunkLength;
            }
        }

        private async Task LoadSequentialFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) return;

            var f = new FormLoadSequentialFileOptions();
            if (f.ShowDialog() != DialogResult.OK) return;

            var data = File.ReadAllBytes(file).ToList();

            var bytesToRemove = f.BytesToRemoveFromEnd;
            if (bytesToRemove > data.Count) bytesToRemove = data.Count;
            if (bytesToRemove > 0) data = data.SkipLast(bytesToRemove).ToList();

            if (f.ClearScreen) data.Insert(0, 147); // SHIFT+CLR/HOME Clears screen

            // 0x0277: Keyboard buffer (10 bytes, 10 entries).
            int chunkLength = 10;
            int offset = 0;

            while (offset < data.Count)
            {
                if (offset + chunkLength > data.Count) chunkLength = data.Count - offset;
                Array.Copy(data.ToArray(), offset, C64.Memory._memory, 0x0277, chunkLength);

                // Wait for BASIC to process the keyboard buffer
                C64.Memory._memory[0x00C6] = (byte)chunkLength;
                while (C64.Memory._memory[0x00C6] != 0)
                {
                    await Task.Delay(1);
                }

                offset += chunkLength;
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

            // Create border color items
            mnuBorderColor.DropDownItems.Clear();
            for (int i = 0; i < 16; i++)
            {
                // Needed to capture the value of i instead of a reference
                var colorIndex = (byte)i;

                mnuBorderColor.DropDownItems.Add(
                    new ToolStripMenuItem(
                        ColorManager.ColorNames[colorIndex],
                        null,
                        (s, e) =>
                        {
                            C64.Memory[53280] = colorIndex;
                        }
                    )
                    {
                        Checked = C64.Memory[53280] == colorIndex,
                        Image = CreateColorImage(ColorManager.FromByte(colorIndex)),
                        ImageScaling = ToolStripItemImageScaling.SizeToFit,
                        ImageAlign = ContentAlignment.MiddleCenter
                    }
                );
            }

            // Create background color items
            mnuBackgroundColor.DropDownItems.Clear();
            for (int i = 0; i < 16; i++)
            {
                // Needed to capture the value of i instead of a reference
                var colorIndex = (byte)i;

                mnuBackgroundColor.DropDownItems.Add(
                    new ToolStripMenuItem(
                        ColorManager.ColorNames[colorIndex],
                        null,
                        (s, e) =>
                        {
                            C64.Memory[53281] = colorIndex;
                        }
                    )
                    {
                        Checked = C64.Memory[53281] == colorIndex,
                        Image = CreateColorImage(ColorManager.FromByte(colorIndex)),
                        ImageScaling = ToolStripItemImageScaling.SizeToFit,
                        ImageAlign = ContentAlignment.MiddleCenter
                    }
                );
            }

            // Create text color items
            mnuTextColor.DropDownItems.Clear();
            for (int i = 0; i < 16; i++)
            {
                // Needed to capture the value of i instead of a reference
                var colorIndex = (byte)i;

                mnuTextColor.DropDownItems.Add(
                    new ToolStripMenuItem(
                        ColorManager.ColorNames[colorIndex],
                        null,
                        (s, e) =>
                        {
                            C64.Memory[646] = colorIndex;
                        }
                    )
                    {
                        Checked = C64.Memory[646] == colorIndex,
                        Image = CreateColorImage(ColorManager.FromByte(colorIndex)),
                        ImageScaling = ToolStripItemImageScaling.SizeToFit,
                        ImageAlign = ContentAlignment.MiddleCenter
                    }
                );
            }
        }

        private Bitmap CreateColorImage(Color color)
        {
            var image = new Bitmap(16, 16);
            using var g = Graphics.FromImage(image);
            g.Clear(color);
            g.DrawRectangle(new Pen(Color.Black, 1), 0, 0, image.Width - 1, image.Height - 1);
            return image;
        }

        private async void mnuImportVICEPaletteFile_Click(object sender, EventArgs e)
        {
            if (ofdImportVicePaletteFile.ShowDialog() == DialogResult.OK)
            {
                await HandleFileOpen(ofdImportVicePaletteFile.FileName);
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

            await Task.Run(async () =>
            {
                if (MessageBox.Show(
                    "This setting requires a restart. Do you want to restart now?",
                    "Restart",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    await C64.PowerOff();
                    C64.PowerOn();
                }
            });
        }

        private void btnShowSidDebugWindow_Click(object sender, EventArgs e)
        {
            new FormSidDebug(C64.Sid).Show(this);
        }

        private void btnShowOnScreenDisplay_Click(object sender, EventArgs e)
        {
            if (btnShowOnScreenDisplay.Checked) _osdManager.AddItem("On Screen Display On");
        }

        private void FormC64Screen_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle pasting
            if (e.Control && e.KeyCode == Keys.V)
            {
                var text = Clipboard.GetText();
                _ = PasteText(text);
                e.SuppressKeyPress = true;
                return;
            }

            // Handle up arrow key
            if (e.KeyCode == Keys.Up)
            {
                if (!_keysDown.Contains(Keys.ShiftKey)) _keysDown.Add(Keys.ShiftKey);
                if (!_keysDown.Contains(Keys.Down)) _keysDown.Add(Keys.Down);
            }
            // Handle left arrow key
            else if (e.KeyCode == Keys.Left)
            {
                if (!_keysDown.Contains(Keys.ShiftKey)) _keysDown.Add(Keys.ShiftKey);
                if (!_keysDown.Contains(Keys.Right)) _keysDown.Add(Keys.Right);
            }
            // Handle insert key
            else if (e.KeyCode == Keys.Insert)
            {
                if (!_keysDown.Contains(Keys.ShiftKey)) _keysDown.Add(Keys.ShiftKey);
                if (!_keysDown.Contains(Keys.Back)) _keysDown.Add(Keys.Back);
            }
            // Handle 1-to-1 mapped keys
            else
            {
                if (!_keysDown.Contains(e.KeyCode)) _keysDown.Add(e.KeyCode);
            }

            // Suppress the Alt (Menu) key to prevent menu bar focus
            if (e.Alt) e.SuppressKeyPress = true;

            // Debug.WriteLine("KeyDown: " + string.Join(", ", _keysDown.Select(x => x.ToString())));
        }

        private void FormC64Screen_KeyUp(object sender, KeyEventArgs e)
        {
            // Handle up arrow key
            if (e.KeyCode == Keys.Up)
            {
                _keysDown.Remove(Keys.ShiftKey);
                _keysDown.Remove(Keys.Down);
            }
            // Handle left arrow key
            else if (e.KeyCode == Keys.Left)
            {
                _keysDown.Remove(Keys.ShiftKey);
                _keysDown.Remove(Keys.Right);
            }
            // Handle insert key
            else if (e.KeyCode == Keys.Insert)
            {
                _keysDown.Remove(Keys.ShiftKey);
                _keysDown.Remove(Keys.Back);
            }
            // Handle 1-to-1 mapped keys
            else
            {
                _keysDown.Remove(e.KeyCode);
            }

            // Suppress the Alt (Menu) key to prevent menu bar focus
            if (e.Alt) e.SuppressKeyPress = true;

            // Debug.WriteLine("KeyUp: " + string.Join(", ", _keysDown.Select(x => x.ToString())));
        }

        /// <summary>
        /// IC64KeyboardInputProvider Implementation
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
            return _keysDown.Contains(key);
        }

        private async void mnuOpen_Click(object sender, EventArgs e)
        {
            if (ofdOpenFile.ShowDialog() == DialogResult.OK)
            {
                await HandleFileOpen(ofdOpenFile.FileName);
            }
        }
    }
}
