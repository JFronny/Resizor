﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CC_Functions.W32.Hooks;
using Microsoft.Win32;

namespace Resizor
{
    public partial class SettingsForm : Form
    {
        private const string AppName = "Resizor";
        private readonly RegistryKey _rkApp;

        public SettingsForm()
        {
            InitializeComponent();
            Program.Kh = new KeyboardHook();
            keySelectButton.Text = Settings.ImmediateResizeKey.ToString();
            keySelectButton.Tag = false;
            rowsSelect.Value = Settings.ResizeDividor.Y;
            columnsSelect.Value = Settings.ResizeDividor.X;
            _rkApp = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            startupBox.Checked = _rkApp.GetValue(AppName) != null;
        }

        private void KeySelectButton_Click(object sender, EventArgs e)
        {
            if ((bool) keySelectButton.Tag)
            {
                Program.Kh.OnKeyPress -= Hook_OnKeyPress;
                keySelectButton.BackColor = SystemColors.Control;
                keySelectButton.Tag = false;
                keySelectButton.Text = Settings.ImmediateResizeKey.ToString();
            }
            else
            {
                keySelectButton.BackColor = Color.Red;
                keySelectButton.Text = "Cancel";
                keySelectButton.Tag = true;
                Program.Kh.OnKeyPress += Hook_OnKeyPress;
            }
        }

        private void Hook_OnKeyPress(KeyboardHookEventArgs e)
        {
            Program.Kh.OnKeyPress -= Hook_OnKeyPress;
            keySelectButton.BackColor = SystemColors.Control;
            if (e.Key != Keys.Escape)
            {
                Settings.ImmediateResizeKey = e.Key;
                Settings.Save();
            }
            keySelectButton.Text = Settings.ImmediateResizeKey.ToString();
            keySelectButton.Tag = false;
        }

        private void RowsSelect_ValueChanged(object sender, EventArgs e)
        {
            Point tmp = Settings.ResizeDividor;
            tmp.Y = (int) rowsSelect.Value;
            Settings.ResizeDividor = tmp;
            Settings.Save();
        }

        private void ColumnsSelect_ValueChanged(object sender, EventArgs e)
        {
            Point tmp = Settings.ResizeDividor;
            tmp.X = (int) columnsSelect.Value;
            Settings.ResizeDividor = tmp;
            Settings.Save();
        }

        private void StartupBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (startupBox.Checked)
                    _rkApp.SetValue(AppName, Path.ChangeExtension(Application.ExecutablePath, ".exe"));
                else
                    _rkApp.DeleteValue(AppName, false);
                startupBox.Checked = _rkApp.GetValue(AppName) != null;
            }
            catch (Exception e1)
            {
                startupBox.Checked = _rkApp.GetValue(AppName) != null;
                MessageBox.Show(e1.ToString(), "Failed");
            }
        }
    }
}