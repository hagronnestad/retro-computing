using Commodore64;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace ComputerSystem.Commodore64 {
    public partial class FormC64Screen : Form {

        public C64 C64 { get; set; }

        public int ScreenBufferOffset = 0x0400;

        private readonly Stopwatch _stopWatch = new Stopwatch();

        private double _lastFrameTime = 0.0f;
        private double _fpsActual = 0.0f;

        private readonly Bitmap bBuffer;
        readonly Color[] screenBufferPixels;
        private readonly Pen penScanLine;

        public FormC64Screen(C64 c64) {
            InitializeComponent();

            C64 = c64;

            bBuffer = new Bitmap(320, 200, PixelFormat.Format24bppRgb);
            screenBufferPixels = new Color[bBuffer.Width * bBuffer.Height];
            penScanLine = new Pen(Color.FromArgb(100, 127, 127, 127));
        }

        private void FormSimpleCharacterBufferViewer_Load(object sender, EventArgs e) {
            new Thread(() => {
                while (true) {
                    if (!Visible) return;

                    Invoke(new Action(() => { Invalidate(); }));
                }   
            }).Start();

            OnResize(null);
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            if (C64.Cpu.Memory[0x009A] != 0x03) { // Current output device number. Default: $03, screen.
                base.OnPaintBackground(e);
                Text = "Screen off!";
                return;
            }

            _stopWatch.Reset();
            _stopWatch.Start();

            //base.OnPaintBackground(e);

            Update();

            e.Graphics.InterpolationMode = InterpolationMode.Low;
            e.Graphics.DrawImage(bBuffer, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            // Let's make some fake scanlines for fun 😎
            for (int i = 0; i < ClientRectangle.Height; i += (int)(penScanLine.Width * 2)) {
                e.Graphics.DrawLine(penScanLine, 0, i, ClientRectangle.Width, i);
            }

            _stopWatch.Stop();

            _lastFrameTime = _stopWatch.Elapsed.TotalMilliseconds;
            _fpsActual = 1000f / _lastFrameTime;
            Text = $"{_fpsActual:F1} fps";
        }
        
        public new void Update() {
            var bgColor = Colors.FromByte(C64.Memory[0xD021]);

            for (var i = 0; i < 1000; i++) {
                var petsciiCode = C64.Memory[ScreenBufferOffset + i];
                var fgColor = Colors.FromByte(C64.Memory[0xD800 + i]);

                var line = (i / 40);
                var characterInLine = i % 40;
                var indexLineOffset = (2560 * line) + (8 * characterInLine);

                for (int row = 0; row <= 7; row++) {
                    var charRow = C64.Memory._romCharacter.Read((petsciiCode * 8) + row);

                    var indexRowOffset = indexLineOffset + (320 * row) ;

                    for (int col = 0; col <= 7; col++) {
                        var indexPixelOffset = indexRowOffset + col;

                        var pixelSet = (charRow & 0x80) == 0x80;
                        charRow <<= 1;

                        screenBufferPixels[indexPixelOffset] = pixelSet ? fgColor : bgColor;
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
        }
    }
}
