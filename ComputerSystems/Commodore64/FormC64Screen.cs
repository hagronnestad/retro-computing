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

namespace ComputerSystem.Commodore64 {
    public partial class FormC64Screen : Form {

        public C64 C64 { get; set; }

        private readonly Stopwatch _stopWatch = new Stopwatch();

        private double _fpsActual = 0.0f;

        private double _screenRefreshRate = 1000.0f / 60.0f; // 60 fps
        private Bitmap _bC64ScreenOutputBuffer;
        private Graphics _gC64ScreenOutputBuffer;

        private readonly Pen _penScanLine;
        private readonly Pen _penScanLine2;
        private readonly Pen _penWhite = new Pen(Color.White) { DashStyle = DashStyle.Dot };
        private readonly Pen _penRaster = new Pen(Color.Red) { DashStyle = DashStyle.Dash };

        private Timer _uiRefreshTimer;
        private Bitmap _bC64ScreenBuffer;
        private Graphics _gC64ScreenBuffer;

        public FormC64Screen(C64 c64) {
            InitializeComponent();

            C64 = c64;

            _bC64ScreenBuffer = new Bitmap(VicIi.FULL_WIDTH, VicIi.FULL_HEIGHT_PAL, PixelFormat.Format24bppRgb);
            _gC64ScreenBuffer = Graphics.FromImage(_bC64ScreenBuffer);
            _bC64ScreenOutputBuffer = new Bitmap(pScreen.Width, pScreen.Height);
            _gC64ScreenOutputBuffer = Graphics.FromImage(_bC64ScreenOutputBuffer);
            _penScanLine = new Pen(Color.FromArgb(100, 127, 127, 127));
            _penScanLine2 = new Pen(Color.FromArgb(20, 127, 127, 127));


            _uiRefreshTimer = new Timer((e) => {

                try {
                    Invoke(new Action(() => {
                        lblClockSpeed.Text = $"{1 / c64.CpuClockSpeed:F0} Hz";

                        lblCycles.Text = $"{c64.Cpu.TotalCycles:N0} cycles";
                        lblInstructions.Text = $"{c64.Cpu.TotalInstructions:N0} instructions";
                        lblIllegalInstructions.Text = $"{c64.Cpu.TotalIllegalInstructions:N0} illegal instructions";
                        lblKeyboardDisabled.Visible = !c64.KeyboardActivated;

                        lblFps.Text = $"{((int)_fpsActual):D3} fps";
                        lblVicCycles.Text = $"{c64.Vic.TotalCycles:N0} cycles";

                        lblVicCurrentLine.Text = $"Line: {c64.Vic.CurrentLine:D3}";
                        lblVicCurrentLineCycle.Text = $"Pos: {c64.Vic.CurrentLineCycle:D2}";

                        lblVicGraphicsMode.Text = "Mode: " + C64.Vic.GetCurrentGraphicsMode().ToString();
                        lblVicScreenOn.Text = C64.Vic.ScreenControlRegisterScreenOffOn ? "Screen: On" : "Screen: Off";
                    }));
                } catch { }

            }, null, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(50));

            c64.PowerOn();
        }

        private void FormC64Screen_Load(object sender, EventArgs e) {
            pScreen.AllowDrop = true;

            C64.Vic.OnLastScanLine += Vic_OnLastScanLine;

            new Thread(InvalidateScreen).Start();
        }

        private void Vic_OnLastScanLine(object sender, EventArgs e) {

        }


        private void InvalidateScreen() {
            while (true) {
                if (!Visible || WindowState == FormWindowState.Minimized) {
                    Thread.Sleep(1000);
                    continue;
                }

                try {
                    Invoke(new Action(() => {
                        pScreen.Invalidate();
                    }));

                } catch (Exception) {
                    return;
                }

                Thread.Sleep((int)_screenRefreshRate);
            }
        }

        public void ApplyCrtFilter() {
            var width = _bC64ScreenOutputBuffer.Width;
            var height = _bC64ScreenOutputBuffer.Height;
            var penWidth = (int)(_penScanLine.Width * 2);
            var penWidth2 = (int)(_penScanLine2.Width * 2);

            for (int i = 0; i < width; i += penWidth2) {
                _gC64ScreenOutputBuffer.DrawLine(_penScanLine2, i, 0, i, height);
            }

            for (int i = 0; i < height; i += penWidth) {
                _gC64ScreenOutputBuffer.DrawLine(_penScanLine, 0, i, width, i);
            }
        }

        private void PScreen_Paint(object sender, PaintEventArgs e) {
            SetPixels(_bC64ScreenBuffer, C64.Vic.ScreenBufferPixels);

            if (btnShowVideoFrameOutlines.Checked) {
                _gC64ScreenBuffer.DrawRectangle(_penWhite, C64.Vic.FullFrame.X, C64.Vic.FullFrame.Y, C64.Vic.FullFrame.Width, C64.Vic.FullFrame.Height);
                _gC64ScreenBuffer.DrawRectangle(_penWhite, C64.Vic.BorderFrame.X, C64.Vic.BorderFrame.Y, C64.Vic.BorderFrame.Width, C64.Vic.BorderFrame.Height);
                _gC64ScreenBuffer.DrawRectangle(_penWhite, C64.Vic.DisplayFrame.X, C64.Vic.DisplayFrame.Y, C64.Vic.DisplayFrame.Width, C64.Vic.DisplayFrame.Height);
            }

            if (btnShowRasterLineInterrupt.Checked && C64.Vic.InterruptControlRegisterRasterInterruptEnabled) {
                _gC64ScreenBuffer.DrawLine(_penRaster, 0, C64.Vic._rasterLineToGenerateInterruptAt, C64.Vic.FullFrame.Width, C64.Vic._rasterLineToGenerateInterruptAt);
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

            if (btnUseCrtFilter.Checked) ApplyCrtFilter();

            e.Graphics.DrawImage(_bC64ScreenOutputBuffer, 0, 0, pScreen.Width, pScreen.Height);

            _stopWatch.Stop();

            _fpsActual = 1000f / _stopWatch.Elapsed.TotalMilliseconds;

            _stopWatch.Restart();
        }

        private void PScreen_Resize(object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized) return;

            _penScanLine.Width = (int)(pScreen.Height * 0.005);
            _penScanLine2.Width = (int)(pScreen.Width * 0.0025);

            _bC64ScreenOutputBuffer.Dispose();
            _gC64ScreenOutputBuffer.Dispose();
            _bC64ScreenOutputBuffer = new Bitmap(pScreen.Width, pScreen.Height);
            _gC64ScreenOutputBuffer = Graphics.FromImage(_bC64ScreenOutputBuffer);
        }

        private void FormC64Screen_FormClosing(object sender, FormClosingEventArgs e) {
            _gC64ScreenOutputBuffer.Dispose();
            _bC64ScreenOutputBuffer.Dispose();

            C64.PowerOff();

            Settings.Default.Save();
        }

        private async void BtnRestart_Click(object sender, EventArgs e) {
            await C64.PowerOff();
            C64.PowerOn();
        }

        private void LoadPrg(string fileName, bool executeRun) {
            var file = File.ReadAllBytes(fileName);

            var address = BitConverter.ToUInt16(file, 0);
            var data = file.Skip(2).ToArray();

            for (int i = 0; i < data.Length; i++) {
                C64.Memory._memory[address + i] = data[i];
            }

            if (executeRun) {
                // Put "RUN" + {RETURN} directly into the BASIC keyboard
                // buffer and set the buffer length, BASIC does the rest!
                C64.Memory._memory[0x0277] = (byte)'R';
                C64.Memory._memory[0x0278] = (byte)'U';
                C64.Memory._memory[0x0279] = (byte)'N';
                C64.Memory._memory[0x027A] = 13; // {RETURN}
                C64.Memory._memory[0x00C6] = 4;
            }
        }

        private async void BtnOpen_ClickAsync(object sender, EventArgs e) {
            if (ofd.ShowDialog() == DialogResult.OK) {
                await C64.Cpu.Pause();
                LoadPrg(ofd.FileName, true);
                C64.Cpu.Resume();
            }
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

            } else {

                C64.Cpu.Resume();
            }

        }

        private void FormC64Screen_Activated(object sender, EventArgs e) {
            C64.KeyboardActivated = true;
        }

        private void FormC64Screen_Deactivate(object sender, EventArgs e) {
            C64.KeyboardActivated = false;
        }

        private void BtnReset_Click(object sender, EventArgs e) {
            C64.Cpu.Reset();
        }


        private async void pScreen_DragDropAsync(object sender, DragEventArgs e) {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] d && d.Length > 0) {

                var fileName = d.First();
                if (!File.Exists(fileName)) return;

                switch (Path.GetExtension(fileName).ToLower()) {
                    case "":
                    case ".prg":
                        await C64.Cpu.Pause();
                        LoadPrg(fileName, true);
                        C64.Cpu.Resume();
                        break;

                    case ".crt":
                        await InsertCartridge(fileName);
                        break;
                }
            }
        }

        private void pScreen_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            }
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

        private void btnSlowDown_Click(object sender, EventArgs e) {
            C64.CpuClockSpeed *= 10.0d;
        }

        private void btnClockSpeedFaster_Click(object sender, EventArgs e) {
            C64.CpuClockSpeed /= 10.0d;
        }

        private void pScreen_DoubleClick(object sender, EventArgs e) {
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

        private async void btnInsertCartridge_ClickAsync(object sender, EventArgs e) {
            if (!btnInsertCartridge.Checked) {
                if (ofdInsertCartridge.ShowDialog() == DialogResult.OK) {
                    await InsertCartridge(ofdInsertCartridge.FileName);
                }

            } else {
                await RemoveCartridge();
            }
        }

        private async Task InsertCartridge(string fileName) {
            var crt = CrtFile.FromFile(fileName);
            C64.Cartridge = crt;
            btnInsertCartridge.Text = $"{crt.Name}";
            btnInsertCartridge.Checked = true;

            await C64.PowerOff();
            C64.PowerOn();
        }

        private async Task RemoveCartridge() {
            C64.Cartridge = null;
            btnInsertCartridge.Text = "";
            btnInsertCartridge.Checked = false;

            await C64.PowerOff();
            C64.PowerOn();
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

                while (C64.Memory._memory[0x00C6] != 0) {
                    Application.DoEvents();
                }
            }
        }

    }
}
