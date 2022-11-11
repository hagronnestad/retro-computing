using Commodore64;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ComputerSystem.Commodore64 {
    public partial class FormSimpleCharacterBufferViewer : Form {

        public C64 C64 { get; set; }

        private Stopwatch sw = new Stopwatch();
        private Stopwatch sw2 = new Stopwatch();

        private double _lastFrameTime = 0.0f;
        private double _fpsActual = 0.0f;
        private double _fpsAdjusted = 0.0f;
        private double _fpsTarget = 60.0f;
        private double _fpsWaitTime = 0.0f;

        private Bitmap bBuffer;

        public Graphics g;

        private Pen pWhite = new Pen(Color.White);
        private Pen pScanLine = new Pen(Color.FromArgb(120, 0, 0, 0), 2);

        private SolidBrush bWhite = new SolidBrush(Color.White);

        private Font fFont = new Font("Consolas", 16);

        public FormSimpleCharacterBufferViewer(C64 c64) {
            InitializeComponent();

            C64 = c64;

            bBuffer = new Bitmap(40 * 16, 25 * 16);
            g = Graphics.FromImage(bBuffer);
        }

        private void FormSimpleCharacterBufferViewer_Load(object sender, EventArgs e) {
            new Thread(() => {
                while (true) {
                    if (!Visible) return;

                    sw2.Reset();
                    sw2.Start();
                    BeginInvoke(new MethodInvoker(() => { Invalidate(); }));
                    Thread.Sleep(TimeSpan.FromMilliseconds(_fpsWaitTime));

                    sw2.Stop();

                    _fpsAdjusted = 1000f / sw2.Elapsed.TotalMilliseconds;

                    BeginInvoke(new MethodInvoker(() => { Text = $"{_fpsActual:F0} fps max, {_fpsAdjusted:F0} fps adjusted"; }));
                }   
            }).Start();
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            base.OnPaintBackground(e);

            sw.Reset();
            sw.Start();

            Update();
            e.Graphics.DrawImage(bBuffer, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            sw.Stop();

            _lastFrameTime = sw.Elapsed.TotalMilliseconds;

            _fpsActual = 1000f / _lastFrameTime;

            if (_fpsActual > _fpsTarget) {
                _fpsWaitTime = (1000f / _fpsTarget) - _lastFrameTime;
            }
        }

        public new void Update() {
            g.Clear(Color.Blue);

            for (int i = 0; i < 1000; i++) {
                byte data = C64.Cpu.Memory[(ushort)(C64MemoryOffsets.SCREEN_BUFFER + i)];

                if (data < 0x20) data += 0x40;

                var x = (i % 40) * 16;
                var y = (i / 40) * 16;

                // 0xA0 is the cursor character
                // Let's just cheat and draw it as a filled rectangle
                if (data == 0xA0) {
                    g.FillRectangle(bWhite, x, y, 16, 16);

                // Draw other characters as ASCII
                } else {
                    g.DrawString(new string((char)data, 1), fFont, bWhite, x - 2, y - 5);
                }
            }

            // Let's make some fake scanlines for fun 😎
            for (int i = 0; i < bBuffer.Height; i += 3) {
                g.DrawLine(pScanLine, 0, i, bBuffer.Width, i);
            }
        }
    }
}
