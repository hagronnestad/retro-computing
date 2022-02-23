using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Timers;

namespace Commodore64
{
    internal class OsdManager
    {
        private Dictionary<DateTime, string> _osdItems { get; set; } = new Dictionary<DateTime, string>();

        private Image _b;
        private Graphics _g;

        private Font _osdFont;
        private Brush _osdBrush;
        private Pen _osdPen;

        private Timer _t;


        public OsdManager()
        {
            _b = new Bitmap(450, 300);
            _g = Graphics.FromImage(_b);
            _g.SmoothingMode = SmoothingMode.AntiAlias;

            _osdFont = new Font(FontFamily.GenericMonospace, 16, FontStyle.Bold);
            _osdBrush = new SolidBrush(Color.White);
            _osdPen = new Pen(Color.Black, 2)
            {
                LineJoin = LineJoin.Round
            };

            _t = new Timer
            {
                Interval = 500
            };
            _t.Elapsed += (s, e) => ClearOldItems();
            _t.Start();
        }


        public TimeSpan ItemTimeout { get; set; } = TimeSpan.FromSeconds(3);
        public Image OsdBitmap => _b;
        public string OsdText => string.Join(Environment.NewLine, _osdItems.Select(x => x.Value.ToUpper()));
        public bool HasItems => _osdItems.Count > 0;


        public void AddItem(string text)
        {
            _osdItems.Add(DateTime.Now, text);
            UpdateOsdBitmap();
        }

        public void ClearOldItems()
        {
            var itemsToRemove = _osdItems
                .Where(x => DateTime.Now - x.Key >= ItemTimeout)
                .Select(x => x.Key)
                .ToList();

            foreach (var item in itemsToRemove)
            {
                _osdItems.Remove(item);
            }

            if (itemsToRemove.Count > 0)
            {
                UpdateOsdBitmap();
            }
        }

        private void UpdateOsdBitmap()
        {
            _g.Clear(Color.Transparent);
            var osdPath = new GraphicsPath();
            osdPath.AddString(OsdText, _osdFont.FontFamily, (int)_osdFont.Style,
                _osdFont.Size, new Point(10, 10), StringFormat.GenericDefault);
            _g.DrawPath(_osdPen, osdPath);
            _g.FillPath(_osdBrush, osdPath);
        }
    }
}
