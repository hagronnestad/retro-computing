using Commodore64;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using Extensions.Byte;
using Extensions.Enums;
using System.IO;
using System.Linq;

namespace ComputerSystem.Commodore64 {
    public partial class FormC64Screen : Form {

        public C64 C64 { get; set; }

        private readonly Stopwatch _stopWatch = new Stopwatch();

        private double _fpsActual = 0.0f;
        private double _screenRefreshRate = 1000.0f / 60.0f; // 60 fps

        private readonly Bitmap bBuffer;
        readonly Color[] screenBufferPixels;
        private readonly Pen penScanLine;
        private readonly Pen penScanLine2;

        public FormC64Screen(C64 c64) {
            InitializeComponent();

            C64 = c64;

            bBuffer = new Bitmap(320, 200, PixelFormat.Format24bppRgb);
            screenBufferPixels = new Color[bBuffer.Width * bBuffer.Height];
            penScanLine = new Pen(Color.FromArgb(100, 127, 127, 127));
            penScanLine2 = new Pen(Color.FromArgb(20, 127, 127, 127));
        }

        private void FormC64Screen_Load(object sender, EventArgs e) {
            new Thread(() => {
                while (true) {
                    if (!Visible || WindowState == FormWindowState.Minimized) {
                        Thread.Sleep(1000);
                        continue;
                    }

                    Invoke(new Action(() => { pScreen.Invalidate(); }));

                    Thread.Sleep((int)_screenRefreshRate);
                }   
            }).Start();

            OnResize(null);
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            
        }
        
        public new void Update() {
            var bgColor = Colors.FromByte((byte) (C64.Memory[C64MemoryLocations.SCREEN_BACKGROUND_COLOR] & 0b00001111));

            for (var i = 0; i < 1000; i++) {
                var petsciiCode = C64.Memory[C64MemoryOffsets.SCREEN_BUFFER + i];
                var fgColor = Colors.FromByte((byte) (C64.Memory[C64MemoryOffsets.SCREEN_COLOR_RAM + i] & 0b00001111));

                var line = (i / 40);
                var characterInLine = i % 40;
                var indexLineOffset = (2560 * line) + (8 * characterInLine);

                for (int row = 0; row <= 7; row++) {
                    var charRow = C64.Memory._romCharacter.Read((petsciiCode * 8) + row);

                    // TODO: Don't read directly from the character ROM, needs some CIA logic for it to work I think
                    //var charRow = C64.Memory.Read(0xD000 + (petsciiCode * 8) + row);

                    var indexRowOffset = indexLineOffset + (320 * row) ;

                    for (int col = 0; col <= 7; col++) {
                        var indexPixelOffset = indexRowOffset + col;

                        screenBufferPixels[indexPixelOffset] = charRow.IsBitSet(7 - (BitIndex)col) ? fgColor : bgColor;
                    }

                }

            }

            SetPixels(bBuffer, screenBufferPixels);
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

        private void FormC64Screen_Resize(object sender, EventArgs e) {
            penScanLine.Width = (int)(ClientRectangle.Height * 0.005);
            penScanLine2.Width = (int)(ClientRectangle.Width * 0.0025);
        }

        private void MnuReset_Click(object sender, EventArgs e) {
            C64.Cpu.Reset();
        }

        private void PScreen_Paint(object sender, PaintEventArgs e) {
            if (C64.Cpu.Memory[C64MemoryLocations.CURRENT_OUTPUT_DEVICE] != C64MemoryValues.CURRENT_OUTPUT_DEVICE_SCREEN) {
                return;
            }

            Update();

            e.Graphics.InterpolationMode = InterpolationMode.Low;
            e.Graphics.DrawImage(bBuffer, 0, 0, pScreen.Width, pScreen.Height);

            // Let's make some fake scanlines for fun 😎
            for (int i = 0; i < pScreen.Width; i += (int)(penScanLine2.Width * 2)) {
                e.Graphics.DrawLine(penScanLine2, i, 0, i, pScreen.Height);
            }

            for (int i = 0; i < pScreen.Height; i += (int)(penScanLine.Width * 2)) {
                e.Graphics.DrawLine(penScanLine, 0, i, pScreen.Width, i);
            }


            _stopWatch.Stop();

            _fpsActual = 1000f / _stopWatch.Elapsed.TotalMilliseconds;
            lblFps.Text = $"{_fpsActual:F1} fps";

            _stopWatch.Restart();
        }
    }
}
