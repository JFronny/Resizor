using System.Drawing;
using System.Linq;
using CC_Functions.W32;

namespace Resizor
{
    internal class WindowSizeSetter
    {
        public readonly Wnd32 Window;
        public Rectangle Pos;

        private WindowSizeSetter(Wnd32 window, Rectangle pos)
        {
            Window = window;
            Pos = pos;
        }

        public static void Make(Wnd32 window, Rectangle pos)
        {
            WindowSizeSetter[] match = Program.Ctx.WindowSizeSetters.Where(s => s.Window == window).ToArray();
            switch (match.Length)
            {
                case 0:
                    Program.Ctx.WindowSizeSetters.Add(new WindowSizeSetter(window, pos));
                    break;
                case 1:
                    match[0].Pos = pos;
                    break;
                default:
                    for (int i = 0; i < match.Length; i++)
                        if (i == match.Length - 1)
                            match[0].Pos = pos;
                        else
                            Program.Ctx.WindowSizeSetters.Remove(match[i]);
                    break;
            }
        }

        public static void TryRemove(Wnd32 window)
        {
            WindowSizeSetter[] match = Program.Ctx.WindowSizeSetters.Where(s => s.Window == window).ToArray();
            if (match.Length > 0)
                Program.Ctx.WindowSizeSetters.RemoveAll(s => s.Window == window);
        }
    }
}