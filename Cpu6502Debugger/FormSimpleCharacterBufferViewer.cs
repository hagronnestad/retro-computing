using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace Cpu6502Debugger {
    public partial class FormSimpleCharacterBufferViewer : Form {

        public byte[] Memory;
        public int ScreenBufferOffset = 0x0400;

        private Stopwatch sw = new Stopwatch();
        private Stopwatch sw2 = new Stopwatch();

        private double lastFrameTime = 0.0f;
        private double fpsActual = 0.0f;
        private double fpsAdjusted = 0.0f;
        private double fpsTarget = 60.0f;
        private double fpsWaitTime = 0.0f;

        private Bitmap bBuffer;

        public Graphics g;

        private Pen pWhite = new Pen(Color.White);
        private Pen pScanLine = new Pen(Color.FromArgb(120, 0, 0, 0), 2);

        private SolidBrush bWhite = new SolidBrush(Color.White);

        private Font fFont = new Font("Consolas", 16);

        public FormSimpleCharacterBufferViewer(byte[] memory) {
            InitializeComponent();

            Memory = memory;

            bBuffer = new Bitmap(40 * 16, 25 * 16);

            g = Graphics.FromImage(bBuffer);
        }

        private void FormSimpleCharacterBufferViewer_Load(object sender, EventArgs e) {
            new Thread(() => {
                while (true) {
                    if (!Visible) return;

                    sw2.Reset();
                    sw2.Start();
                    Invoke(new Action(() => { Invalidate(); }));
                    Thread.Sleep(TimeSpan.FromMilliseconds(fpsWaitTime));

                    sw2.Stop();

                    fpsAdjusted = 1000f / sw2.Elapsed.TotalMilliseconds;

                    Invoke(new Action(() => { Text = $"{fpsActual:F0} fps max, {fpsAdjusted:F0} fps adjusted"; }));
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

            lastFrameTime = sw.Elapsed.TotalMilliseconds;

            fpsActual = 1000f / lastFrameTime;

            if (fpsActual > fpsTarget) {
                fpsWaitTime = (1000f / fpsTarget) - lastFrameTime;
            }
        }

        public new void Update() {
            g.Clear(Color.Blue);

            for (int i = 0; i < 1000; i++) {
                byte data = Memory[(ushort)(ScreenBufferOffset + i)];

                if (data < 0x20) data += 0x40;

                var x = (i % 40) * 16;
                var y = (i / 40) * 16;

                g.DrawString(new string((char)data, 1), fFont, bWhite, x - 2, y - 5);
            }

            if (Memory[0x00CC] == 0) // 0 == cursor is visible
            {
                int x = Memory[0x00CA] * 8;
                int y = Memory[0x00C9] * 8;

                g.DrawRectangle(pWhite, x * 2, y * 2, 16, 16);
            }

            // Let's make some fake scanlines for fun 😎
            for (int i = 0; i < bBuffer.Height; i += 3) {
                g.DrawLine(pScanLine, 0, i, bBuffer.Width, i);
            }
        }
    }
}
