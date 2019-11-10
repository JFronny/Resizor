using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using CC_Functions.W32;
using Resizor.Properties;

namespace Resizor
{
    public partial class immResize : Form
    {
        bool down;
        Point startP;
        Wnd32 window;
        Rectangle prevR;
        Rectangle screen = Screen.PrimaryScreen.WorkingArea;
        public immResize()
        {
            prevR = new Rectangle();
            window = Wnd32.foreground();
            InitializeComponent();
            Program.kh.OnKeyPress += onKeyDown;
            Rectangle tmp = window.position;
            forcePos.Location = new Point((tmp.X + (tmp.Width / 2)) - (forcePos.Width / 2), tmp.Y);
            forcePos.Checked = Program.ctx.windowSizeSetters.Where(Window => Window.Window == window).ToArray().Length > 0;
        }

        private void onKeyDown(KeyboardHookEventArgs _args)
        {
            if (_args.Key == Keys.Escape)
                Close();
        }

        private void ImmResize_FormClosed(object sender, FormClosedEventArgs e) => Program.kh.OnKeyPress -= onKeyDown;

        private void Form1_Load(object sender, EventArgs e)
        {
            Wnd32 self = Wnd32.fromForm(this);
            self.MakeOverlay();
            if (self != window)
                self.isForeground = true;
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
            Rectangle rect;
            if (down)
                rect = FRect();
            else
                rect = CRect();
            g.FillRectangle(new SolidBrush(Color.LightBlue), rect);
            Pen gridPen = new Pen(Color.Black, 2);
            PointF div = getDiv();
            for (int x = 0; x < divisor.X; x++)
            {
                g.DrawLine(gridPen, x * div.X, 0, x * div.X, screen.Height);
            }
            for (int y = 0; y < divisor.Y; y++)
            {
                g.DrawLine(gridPen, 0, y * div.Y, screen.Width, y * div.Y);
            }
            g.DrawRectangle(new Pen(Color.Blue, 2), rect);
            g.DrawRectangle(new Pen(Color.Red, 2), window.position);
        }
        PointF getDiv() => new PointF(screen.Width / Settings.Default.ResizeDividor.X, screen.Height / Settings.Default.ResizeDividor.Y);
        Rectangle CRect() => p2r(f2s(MousePosition, getDiv()), c2s(MousePosition, getDiv()));
        Rectangle FRect()
        {
            Point min = f2s(new Point(Math.Min(MousePosition.X, startP.X), Math.Min(MousePosition.Y, startP.Y)), getDiv());
            Point max = c2s(new Point(Math.Max(MousePosition.X, startP.X), Math.Max(MousePosition.Y, startP.Y)), getDiv());
            return p2r(min, max);
        }
        Point f2s(Point p, PointF step) => new Point(f2s(p.X, step.X), f2s(p.Y, step.Y));
        Point c2s(Point p, PointF step) => new Point(c2s(p.X, step.X), c2s(p.Y, step.Y));
        Rectangle p2r(Point p1, Point p2) => new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        int f2s(int f, double step) => (int)d2f(Math.Floor(f / step) * step);
        int c2s(int f, double step) => (int)d2f(Math.Ceiling(f / step) * step);
        float d2f(double f)
        {
            float result = (float)f;
            if (float.IsPositiveInfinity(result))
                return float.MaxValue;
            else if (float.IsNegativeInfinity(result))
                return float.MinValue;
            return result;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            down = true;
            startP = MousePosition;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle rect;
            if (down)
                rect = FRect();
            else
                rect = CRect();
            if (prevR != rect)
                Invalidate();
            prevR = rect;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            window.position = FRect();
            Close();
        }

        private void ForcePos_CheckedChanged(object sender, EventArgs e)
        {
            if (forcePos.Checked)
            {
                if (Program.ctx.windowSizeSetters.Where(Window => Window.Window == window).ToArray().Length == 0)
                {
                    WindowSizeSetter.make(window, window.position);
                    Close();
                }
            }
            else
            {
                if (Program.ctx.windowSizeSetters.Where(Window => Window.Window == window).ToArray().Length > 0)
                {
                    WindowSizeSetter.TryRemove(window);
                    Close();
                }
            }
        }
    }
}
