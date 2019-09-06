using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CC_Functions.W32;
using Resizor.Properties;
using Timer = System.Windows.Forms.Timer;

namespace Resizor
{
    static class Program
    {
        public static KeyboardHook kh;
        public static NIApplicationContext ctx;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            notifyIcon1 = new NotifyIcon();
            ContextMenu contextMenu = new ContextMenu();
            MenuItem settings = new MenuItem();
            MenuItem exititem = new MenuItem();
            contextMenu.MenuItems.AddRange(new MenuItem[] { settings, exititem });
            settings.Index = 0;
            settings.Text = "Settings";
            settings.Click += new EventHandler(openSettings);
            exititem.Index = 1;
            exititem.Text = "Exit";
            exititem.Click += new EventHandler(exit);
            notifyIcon1.Icon = Resources.Resizor;
            notifyIcon1.Text = "Resizor";
            notifyIcon1.ContextMenu = contextMenu;
            notifyIcon1.Visible = true;
            kh = new KeyboardHook();
            kh.OnKeyPress += keyDown;
            ctx = new NIApplicationContext();
            Application.Run(ctx);
            kh.Dispose();

        }

        private static void keyDown(KeyboardHookEventArgs e)
        {
            if (e.Key == Settings.Default.ImmediateResizeKey)
            {
                new immResize().Show();
            }
        }

        private static NotifyIcon notifyIcon1;
        private static void openSettings(object sender, EventArgs e) => new SettingsForm().Show();
        private static void exit(object Sender, EventArgs e) => Application.Exit();
        public class NIApplicationContext : ApplicationContext
        {
            public List<WindowSizeSetter> windowSizeSetters = new List<WindowSizeSetter>();
            Timer tim;
            public NIApplicationContext()
            {
                tim = new Timer();
                tim.Enabled = true;
                tim.Interval = 100;
                tim.Tick += tick;
            }
            private void tick(object sender, EventArgs e)
            {
                List<int> toRemove = new List<int>();
                for (int i = 0; i < windowSizeSetters.Count; i++)
                {
                    if (windowSizeSetters[i].Window.stillExists)
                        windowSizeSetters[i].Window.position = windowSizeSetters[i].Pos;
                    else
                        toRemove.Add(i);
                }
                for (int i = 0; i < toRemove.Count; i++)
                    windowSizeSetters.RemoveAt(toRemove[i]);
                Console.WriteLine(windowSizeSetters.Count.ToString());
            }
        }
    }

    class WindowSizeSetter
    {
        public readonly Wnd32 Window;
        public Rectangle Pos;
        WindowSizeSetter(Wnd32 window, Rectangle pos)
        {
            Window = window;
            Pos = pos;
        }

        public static void make(Wnd32 window, Rectangle pos)
        {
            WindowSizeSetter[] match = Program.ctx.windowSizeSetters.Where(Window => Window.Window == window).ToArray();
            switch (match.Length)
            {
                case 0:
                    Program.ctx.windowSizeSetters.Add(new WindowSizeSetter(window, pos));
                    break;
                case 1:
                    match[0].Pos = pos;
                    break;
                default:
                    for (int i = 0; i < match.Length; i++)
                    {
                        if (i == match.Length - 1)
                            match[0].Pos = pos;
                        else
                            Program.ctx.windowSizeSetters.Remove(match[i]);
                    }
                    break;
            }
        }

        public static void TryRemove(Wnd32 window)
        {
            WindowSizeSetter[] match = Program.ctx.windowSizeSetters.Where(Window => Window.Window == window).ToArray();
            if (match.Length > 0)
                Program.ctx.windowSizeSetters.RemoveAll(Window => Window.Window == window);
        }
    }
}
