using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using CC_Functions.W32;
using CC_Functions.W32.Hooks;
using Resizor.Properties;

namespace Resizor
{
    public partial class ImmResize : Form
    {
        private readonly Wnd32 _window;
        private bool _down;
        private Rectangle _prevR;
        private Rectangle _screen = Screen.PrimaryScreen.WorkingArea;
        private Point _startP;

        public ImmResize()
        {
            _prevR = new Rectangle();
            _window = Wnd32.Foreground;
            InitializeComponent();
            Program.Kh.OnKeyPress += OnKeyDown;
            Rectangle tmp = _window.Position;
            forcePos.Location = new Point((tmp.X + (tmp.Width / 2)) - (forcePos.Width / 2), tmp.Y);
            forcePos.Checked = Program.Ctx.WindowSizeSetters.Where(s => s.Window == _window).ToArray().Length > 0;
        }

        private void OnKeyDown(KeyboardHookEventArgs args)
        {
            if (args.Key == Keys.Escape)
                Close();
        }

        private void ImmResize_FormClosed(object sender, FormClosedEventArgs e) => Program.Kh.OnKeyPress -= OnKeyDown;

        private void Form1_Load(object sender, EventArgs e)
        {
            Wnd32 self = this.GetWnd32();
            self.Overlay = true;
            if (self != _window)
                self.IsForeground = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.Low;
            g.CompositingMode = CompositingMode.SourceCopy;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.PixelOffsetMode = PixelOffsetMode.None;
            PointF divisor = Settings.Default.ResizeDividor;
            Rectangle rect = _down ? FRect() : CRect();
            g.FillRectangle(new SolidBrush(Color.LightBlue), rect);
            Pen gridPen = new Pen(Color.Black, 2);
            PointF div = GetDiv();
            for (int x = 0; x < divisor.X; x++) g.DrawLine(gridPen, x * div.X, 0, x * div.X, _screen.Height);
            for (int y = 0; y < divisor.Y; y++) g.DrawLine(gridPen, 0, y * div.Y, _screen.Width, y * div.Y);
            g.DrawRectangle(new Pen(Color.Blue, 2), rect);
            g.DrawRectangle(new Pen(Color.Red, 2), _window.Position);
        }

        private PointF GetDiv() => new PointF(_screen.Width / (float) Settings.Default.ResizeDividor.X,
            _screen.Height / (float) Settings.Default.ResizeDividor.Y);

        private Rectangle CRect() => P2R(F2S(MousePosition, GetDiv()), C2S(MousePosition, GetDiv()));

        private Rectangle FRect()
        {
            Point min = F2S(new Point(Math.Min(MousePosition.X, _startP.X), Math.Min(MousePosition.Y, _startP.Y)),
                GetDiv());
            Point max = C2S(new Point(Math.Max(MousePosition.X, _startP.X), Math.Max(MousePosition.Y, _startP.Y)),
                GetDiv());
            return P2R(min, max);
        }

        private static Point F2S(Point p, PointF step) => new Point(F2S(p.X, step.X), F2S(p.Y, step.Y));
        private static Point C2S(Point p, PointF step) => new Point(C2S(p.X, step.X), C2S(p.Y, step.Y));

        private static Rectangle P2R(Point p1, Point p2) => new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y),
            Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));

        private static int F2S(int f, double step) => (int) D2F(Math.Floor(f / step) * step);
        private static int C2S(int f, double step) => (int) D2F(Math.Ceiling(f / step) * step);

        private static float D2F(double f)
        {
            float result = (float) f;
            return float.IsPositiveInfinity(result) ? float.MaxValue :
                float.IsNegativeInfinity(result) ? float.MinValue : result;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            _down = true;
            _startP = MousePosition;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle rect = _down ? FRect() : CRect();
            if (_prevR != rect)
                Invalidate();
            _prevR = rect;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            _window.Position = FRect();
            Close();
        }

        private void ForcePos_CheckedChanged(object sender, EventArgs e)
        {
            if (forcePos.Checked)
            {
                if (Program.Ctx.WindowSizeSetters.Any(s => s.Window == _window)) return;
                WindowSizeSetter.Make(_window, _window.Position);
                Close();
            }
            else
            {
                if (Program.Ctx.WindowSizeSetters.All(s => s.Window != _window)) return;
                WindowSizeSetter.TryRemove(_window);
                Close();
            }
        }
    }
}