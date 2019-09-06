using CC_Functions.W32;
using System;
using System.Drawing;
using System.Windows.Forms;
using Resizor.Properties;
using Microsoft.Win32;

namespace Resizor
{
    public partial class SettingsForm : Form
    {
        RegistryKey rkApp;
        string appName = "Rasizor";
        public SettingsForm()
        {
            InitializeComponent();
            Program.kh = new KeyboardHook();
            keySelectButton.Text = Settings.Default.ImmediateResizeKey.ToString();
            keySelectButton.Tag = false;
            rowsSelect.Value = Settings.Default.ResizeDividor.Y;
            columnsSelect.Value = Settings.Default.ResizeDividor.X;
            rkApp = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            startupBox.Checked = rkApp.GetValue(appName) != null;
        }

        private void KeySelectButton_Click(object sender, EventArgs e)
        {
            if ((bool)keySelectButton.Tag)
            {
                Program.kh.OnKeyPress -= Hook_OnKeyPress;
                keySelectButton.BackColor = SystemColors.Control;
                keySelectButton.Tag = false;
                keySelectButton.Text = Settings.Default.ImmediateResizeKey.ToString();
            }
            else
            {
                keySelectButton.BackColor = Color.Red;
                keySelectButton.Text = "Cancel";
                keySelectButton.Tag = true;
                Program.kh.OnKeyPress += Hook_OnKeyPress;
            }
        }

        private void Hook_OnKeyPress(KeyboardHookEventArgs e)
        {
            Program.kh.OnKeyPress -= Hook_OnKeyPress;
            keySelectButton.BackColor = SystemColors.Control;
            if (e.Key != Keys.Escape)
            {
                Settings.Default.ImmediateResizeKey = e.Key;
                Settings.Default.Save();
            }
            keySelectButton.Text = Settings.Default.ImmediateResizeKey.ToString();
            keySelectButton.Tag = false;
        }

        private void RowsSelect_ValueChanged(object sender, EventArgs e)
        {
            Point tmp = Settings.Default.ResizeDividor;
            tmp.Y = (int)rowsSelect.Value;
            Settings.Default.ResizeDividor = tmp;
            Settings.Default.Save();
        }

        private void ColumnsSelect_ValueChanged(object sender, EventArgs e)
        {
            Point tmp = Settings.Default.ResizeDividor;
            tmp.X = (int)columnsSelect.Value;
            Settings.Default.ResizeDividor = tmp;
            Settings.Default.Save();
        }

        private void StartupBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (startupBox.Checked)
                    rkApp.SetValue(appName, Application.ExecutablePath.ToString());
                else
                    rkApp.DeleteValue(appName, false);
                startupBox.Checked = rkApp.GetValue(appName) != null;
            }
            catch (Exception e1)
            {
                startupBox.Checked = rkApp.GetValue(appName) != null;
                MessageBox.Show(e1.ToString(), "Failed");
            }
        }
    }
}
