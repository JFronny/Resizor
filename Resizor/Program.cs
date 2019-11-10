﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
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
        public static immResize rez;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
            string mutexId = string.Format("Global\\{{{0}}}", appGuid);
            bool createdNew;
            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            using (var mutex = new Mutex(false, mutexId, out createdNew, securitySettings))
            {
                var hasHandle = false;
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
                    notifyIcon = new NotifyIcon();
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
                    notifyIcon.Icon = Resources.Resizor;
                    notifyIcon.Text = "Resizor";
                    notifyIcon.ContextMenu = contextMenu;
                    notifyIcon.Visible = true;
                    kh = new KeyboardHook();
                    kh.OnKeyPress += keyDown;
                    ctx = new NIApplicationContext();
                    Application.Run(ctx);
                    kh.Dispose();
                    notifyIcon.Visible = false;
                }
                finally
                {
                    if (hasHandle)
                        mutex.ReleaseMutex();
                }
            }
        }

        private static void keyDown(KeyboardHookEventArgs e)
        {
            if (e.Key == Settings.Default.ImmediateResizeKey && (rez == null || rez.IsDisposed))
            {
                rez = new immResize();
                rez.Show();
            }
        }

        private static NotifyIcon notifyIcon;
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
            }
        }
    }
}
