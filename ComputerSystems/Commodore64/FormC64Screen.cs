using Commodore64;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using Extensions.Byte;
using Extensions.Enums;

namespace ComputerSystem.Commodore64 {
    public partial class FormC64Screen : Form {

        public C64 C64 { get; set; }

        private readonly Stopwatch _stopWatch = new Stopwatch();

        private double _fpsActual = 0.0f;
        private double _screenRefreshRate = 1000.0f / 60.0f; // 60 fps

        private readonly Bitmap _bC64ScreenBuffer;

        private Bitmap _bC64ScreenOutputBuffer;
        private Graphics _gC64ScreenOutputBuffer;

        private readonly Color[] _screenBufferPixels;
        private readonly Pen _penScanLine;
        private readonly Pen _penScanLine2;

        public FormC64Screen(C64 c64) {
            InitializeComponent();

            C64 = c64;

            _bC64ScreenBuffer = new Bitmap(320, 200, PixelFormat.Format24bppRgb);
            _bC64ScreenOutputBuffer = new Bitmap(pScreen.Width, pScreen.Height);
            _gC64ScreenOutputBuffer = Graphics.FromImage(_bC64ScreenOutputBuffer);
            _screenBufferPixels = new Color[_bC64ScreenBuffer.Width * _bC64ScreenBuffer.Height];
            _penScanLine = new Pen(Color.FromArgb(100, 127, 127, 127));
            _penScanLine2 = new Pen(Color.FromArgb(20, 127, 127, 127));
        }

        private void FormC64Screen_Load(object sender, EventArgs e) {
            new Thread(InvalidateScreen).Start();
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

        public void UpdateScreenBuffer() {
            var bgColor = Colors.FromByte((byte)(C64.Memory[C64MemoryLocations.SCREEN_BACKGROUND_COLOR] & 0b00001111));

            for (var i = 0; i < 1000; i++) {
                var petsciiCode = C64.Memory[C64MemoryOffsets.SCREEN_BUFFER + i];
                var fgColor = Colors.FromByte((byte)(C64.Memory[C64MemoryOffsets.SCREEN_COLOR_RAM + i] & 0b00001111));

                var line = (i / 40);
                var characterInLine = i % 40;
                var indexLineOffset = (2560 * line) + (8 * characterInLine);

                for (int row = 0; row <= 7; row++) {
                    var charRow = C64.Memory._romCharacter.Read((petsciiCode * 8) + row);

                    // TODO: Don't read directly from the character ROM, needs some CIA logic for it to work I think
                    //var charRow = C64.Memory.Read(0xD000 + (petsciiCode * 8) + row);

                    var indexRowOffset = indexLineOffset + (320 * row);

                    for (int col = 0; col <= 7; col++) {
                        var indexPixelOffset = indexRowOffset + col;

                        _screenBufferPixels[indexPixelOffset] = charRow.IsBitSet(7 - (BitIndex)col) ? fgColor : bgColor;
                    }

                }

            }

            SetPixels(_bC64ScreenBuffer, _screenBufferPixels);
            _gC64ScreenOutputBuffer.DrawImage(_bC64ScreenBuffer, 0, 0, _bC64ScreenOutputBuffer.Width, _bC64ScreenOutputBuffer.Height);
        }

        public void ApplyCrtFilter() {
            for (int i = 0; i < _bC64ScreenOutputBuffer.Width; i += (int)(_penScanLine2.Width * 2)) {
                _gC64ScreenOutputBuffer.DrawLine(_penScanLine2, i, 0, i, _bC64ScreenOutputBuffer.Height);
            }

            for (int i = 0; i < _bC64ScreenOutputBuffer.Height; i += (int)(_penScanLine.Width * 2)) {
                _gC64ScreenOutputBuffer.DrawLine(_penScanLine, 0, i, _bC64ScreenOutputBuffer.Width, i);
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

        private void PScreen_Paint(object sender, PaintEventArgs e) {
            UpdateScreenBuffer();
            if (btnUseCrtFilter.Checked) ApplyCrtFilter();

            e.Graphics.DrawImage(_bC64ScreenOutputBuffer, 0, 0, pScreen.Width, pScreen.Height);

            _stopWatch.Stop();

            _fpsActual = 1000f / _stopWatch.Elapsed.TotalMilliseconds;
            lblFps.Text = $"{_fpsActual:F1} fps";

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
            _bC64ScreenBuffer.Dispose();
            _gC64ScreenOutputBuffer.Dispose();
            _bC64ScreenOutputBuffer.Dispose();

            C64.Stop();
        }

        private async void BtnRestart_Click(object sender, EventArgs e) {
            var res = await C64.Stop();

            if (res) {
                C64 = new C64();
                C64.Run();
            }
        }

        private void BtnCopyScreenBuffer_Click(object sender, EventArgs e) {
            Clipboard.SetImage(_bC64ScreenBuffer);
        }

        private void BtnCopyScaledScreenBuffer_Click(object sender, EventArgs e) {
            Clipboard.SetImage(_bC64ScreenOutputBuffer);
        }
    }
}
