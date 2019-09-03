﻿using Commodore64;
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

        private Stopwatch sw = new Stopwatch();
        private Stopwatch sw2 = new Stopwatch();

        private double _lastFrameTime = 0.0f;
        private double _fpsActual = 0.0f;
        private double _fpsAdjusted = 0.0f;
        private double _fpsTarget = 60.0f;
        private double _fpsWaitTime = 0.0f;

        private Bitmap bBuffer;
        Color[] pixels;
        private Pen pScanLine;

        public FormC64Screen(C64 c64) {
            InitializeComponent();

            C64 = c64;

            bBuffer = new Bitmap(320, 200, PixelFormat.Format24bppRgb);
            pixels = new Color[bBuffer.Width * bBuffer.Height];
            pScanLine = new Pen(Color.FromArgb(100, 127, 127, 127));
        }

        private void FormSimpleCharacterBufferViewer_Load(object sender, EventArgs e) {
            new Thread(() => {
                while (true) {
                    if (!Visible) return;

                    sw2.Reset();
                    sw2.Start();
                    Invoke(new Action(() => { Invalidate(); }));
                    Thread.Sleep(TimeSpan.FromMilliseconds(_fpsWaitTime));

                    sw2.Stop();

                    _fpsAdjusted = 1000f / sw2.Elapsed.TotalMilliseconds;

                    Invoke(new Action(() => { Text = $"{_fpsActual:F0} fps max, {_fpsAdjusted:F0} fps adjusted"; }));
                }   
            }).Start();
        }

        protected override void OnPaint(PaintEventArgs e) {
            sw.Reset();
            sw.Start();

            //base.OnPaint(e);

            Update();

            e.Graphics.InterpolationMode = InterpolationMode.Low;
            e.Graphics.DrawImage(bBuffer, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            // Let's make some fake scanlines for fun 😎
            for (int i = 0; i < ClientRectangle.Height; i += (int)(pScanLine.Width * 2)) {
                e.Graphics.DrawLine(pScanLine, 0, i, ClientRectangle.Width, i);
            }

            sw.Stop();

            _lastFrameTime = sw.Elapsed.TotalMilliseconds;

            _fpsActual = 1000f / _lastFrameTime;

            if (_fpsActual > _fpsTarget) {
                _fpsWaitTime = (1000f / _fpsTarget) - _lastFrameTime;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            base.OnPaintBackground(e);
        }

        
        public new void Update() {

            for (var i = 0; i < 1000; i++) {
                var petsciiCode = C64.Memory[ScreenBufferOffset + i];

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

                        pixels[indexPixelOffset] = pixelSet ? Color.FromArgb(0, 136, 255) : Color.FromArgb(0, 0, 170);
                    }

                }

            }

            SetPixels(bBuffer, pixels);
        }

        public void SetPixels(Bitmap b, Color[] pixels) {
            BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = data.Stride;

            unsafe {
                byte* ptr = (byte*)data.Scan0;

                for (int y = 0; y < b.Height; y++) {

                    for (int x = 0; x < b.Width; x++) {

                        var index = (y * b.Width) + x;

                        ptr[(x * 3) + y * stride] = pixels[index].B;
                        ptr[(x * 3) + y * stride + 1] = pixels[index].G;
                        ptr[(x * 3) + y * stride + 2] = pixels[index].R;
                    }

                }
            }

            b.UnlockBits(data);
        }

        private void FormC64Screen_Resize(object sender, EventArgs e) {
            pScanLine.Width = (int)(ClientRectangle.Height * 0.005);
        }
    }
}
