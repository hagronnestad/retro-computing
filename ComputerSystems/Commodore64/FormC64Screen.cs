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

        private Timer _uiRefreshTimer;
        private Bitmap _bC64ScreenBuffer;

        public FormC64Screen(C64 c64) {
            InitializeComponent();

            C64 = c64;

            _bC64ScreenBuffer = new Bitmap(VicIi.USABLE_WIDTH_BORDER, VicIi.USABLE_HEIGHT_BORDER, PixelFormat.Format24bppRgb);
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
                        lblKeyboardDisabled.Visible = !c64.KeyboardActivated;

                        lblFps.Text = $"{((int)_fpsActual):D3} fps";
                        lblVicCycles.Text = $"{c64.Vic.TotalCycles:N0} cycles";

                        lblVicCurrentLine.Text = $"Line: {c64.Vic.CurrentLine:D3}";
                        lblVicCurrentLineCycle.Text = $"Pos: {c64.Vic.CurrentLineCycle:D2}";

                        lblVicGraphicsMode.Text = C64.Vic.ScreenControlRegisterTextModeBitmapMode ? "Mode: Bitmap" : "Mode: Text";
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
            for (int i = 0; i < _bC64ScreenOutputBuffer.Width; i += (int)(_penScanLine2.Width * 2)) {
                _gC64ScreenOutputBuffer.DrawLine(_penScanLine2, i, 0, i, _bC64ScreenOutputBuffer.Height);
            }

            for (int i = 0; i < _bC64ScreenOutputBuffer.Height; i += (int)(_penScanLine.Width * 2)) {
                _gC64ScreenOutputBuffer.DrawLine(_penScanLine, 0, i, _bC64ScreenOutputBuffer.Width, i);
            }
        }

        private void PScreen_Paint(object sender, PaintEventArgs e) {
            SetPixels(_bC64ScreenBuffer, C64.Vic.ScreenBufferPixels);
            _gC64ScreenOutputBuffer.DrawImage(_bC64ScreenBuffer, 0, 0, _bC64ScreenOutputBuffer.Width, _bC64ScreenOutputBuffer.Height);

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

        private void LoadPrg(string fileName) {
            var file = File.ReadAllBytes(fileName);

            var address = BitConverter.ToUInt16(file, 0);
            var data = file.Skip(2).ToArray();

            for (int i = 0; i < data.Length; i++) {
                C64.Memory[address + i] = data[i];
            }
        }

        private void BtnOpen_Click(object sender, EventArgs e) {
            if (ofd.ShowDialog() == DialogResult.OK) {
                LoadPrg(ofd.FileName);
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
                if (!File.Exists(d.First())) return;

                await C64.Cpu.Pause();
                LoadPrg(d.First());
                C64.Cpu.Resume();

                // TODO: Fix this RUN hack
                while (!Focused) {
                    Focus();
                }
                SendKeys.SendWait("r");
                SendKeys.SendWait("u");
                SendKeys.SendWait("n");
                SendKeys.SendWait("{ENTER}");
            }
        }

        private void pScreen_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            }
        }

        public void SetPixels(Bitmap b, Color[] pixels) {
            var width = b.Width;
            var height = b.Height;

            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = data.Stride;

            unsafe {
                byte* ptr = (byte*)data.Scan0;

                for (int y = 0; y < height; y++) {

                    for (int x = 0; x < width; x++) {

                        var index = (y * width) + x;

                        ptr[(x * 3) + y * stride] = pixels[index].B;
                        ptr[(x * 3) + y * stride + 1] = pixels[index].G;
                        ptr[(x * 3) + y * stride + 2] = pixels[index].R;
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
    }
}
