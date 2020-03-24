using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using CC_Functions.W32.Hooks;
using Resizor.Properties;
using Timer = System.Windows.Forms.Timer;

namespace Resizor
{
    internal static class Program
    {
        public static KeyboardHook Kh;
        public static NiApplicationContext Ctx;
        private static ImmResize _rez;

        private static NotifyIcon _notifyIcon;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string appGuid = ((GuidAttribute) Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;
            MutexAccessRule allowEveryoneRule = new MutexAccessRule(
                new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl,
                AccessControlType.Allow);
            MutexSecurity securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            using Mutex mutex = new Mutex(false, $"Global\\{{{appGuid}}}", out bool _);
            bool hasHandle = false;
            try
            {
                try
                {
                    hasHandle = mutex.WaitOne(5000, false);
                    if (hasHandle == false)
                        throw new TimeoutException("Timeout waiting for exclusive access");
                }
                catch (AbandonedMutexException)
                {
#if DEBUG
                    Console.WriteLine("Mutex abandoned");
#endif
                    hasHandle = true;
                }
                _notifyIcon = new NotifyIcon();
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Settings").Click += OpenSettings;
                contextMenu.Items.Add("Exit").Click += Exit;
                _notifyIcon.Icon = Resources.Resizor;
                _notifyIcon.Text = "Resizor";
                _notifyIcon.ContextMenuStrip = contextMenu;
                _notifyIcon.Visible = true;
                Kh = new KeyboardHook();
                Kh.OnKeyPress += KeyDown;
                Ctx = new NiApplicationContext();
                Application.Run(Ctx);
                Kh.Dispose();
                _notifyIcon.Visible = false;
            }
            finally
            {
                if (hasHandle)
                    mutex.ReleaseMutex();
            }
        }

        private static void KeyDown(KeyboardHookEventArgs e)
        {
            if (e.Key != Settings.ImmediateResizeKey || (_rez != null && !_rez.IsDisposed)) return;
            _rez = new ImmResize();
            _rez.Show();
        }

        private static void OpenSettings(object sender, EventArgs e) => new SettingsForm().Show();
        private static void Exit(object sender, EventArgs e) => Application.Exit();

        public class NiApplicationContext : ApplicationContext
        {
            public readonly List<WindowSizeSetter> WindowSizeSetters = new List<WindowSizeSetter>();

            public NiApplicationContext()
            {
                Timer tim = new Timer {Enabled = true, Interval = 100};
                tim.Tick += Tick;
            }

            private void Tick(object sender, EventArgs e)
            {
                List<int> toRemove = new List<int>();
                for (int i = 0; i < WindowSizeSetters.Count; i++)
                    if (WindowSizeSetters[i].Window.StillExists)
                        WindowSizeSetters[i].Window.Position = WindowSizeSetters[i].Pos;
                    else
                        toRemove.Add(i);
                for (int i = 0; i < toRemove.Count; i++)
                    WindowSizeSetters.RemoveAt(toRemove[i]);
            }
        }
    }
}