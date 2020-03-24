using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Resizor
{
    internal static class Settings
    {
        private static bool loaded;

        private static readonly string dir = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
            "Settings.xml");

        private static Keys immediateResizeKey;
        private static Point resizeDividor;

        public static Keys ImmediateResizeKey
        {
            get
            {
                if (!loaded)
                    Load();
                return immediateResizeKey;
            }
            set
            {
                if (!loaded)
                    Load();
                immediateResizeKey = value;
            }
        }

        public static Point ResizeDividor
        {
            get
            {
                if (!loaded)
                    Load();
                return resizeDividor;
            }
            set
            {
                if (!loaded)
                    Load();
                resizeDividor = value;
            }
        }

        public static void Save() =>
            new XElement("settings",
                    new XElement("ImmediateResizeKey", ImmediateResizeKey),
                    new XElement("ResizeDividorX", ResizeDividor.X),
                    new XElement("ResizeDividorY", ResizeDividor.Y))
                .Save(dir);

        private static void Load()
        {
            if (!File.Exists(dir))
                Reset();
            else
                try
                {
                    XElement settings = XDocument.Load(dir).Element("settings");
                    immediateResizeKey = Enum.Parse<Keys>(settings.Element("ImmediateResizeKey").Value);
                    resizeDividor = new Point(
                        int.Parse(settings.Element("ResizeDividorX").Value),
                        int.Parse(settings.Element("ResizeDividorY").Value));
                }
                catch
                {
                    MessageBox.Show("Something went wrong while loading. Resetting to defaults", "Resizor Settings");
                    Reset();
                }
            loaded = true;
        }

        private static void Reset()
        {
            immediateResizeKey = Keys.NumPad4;
            resizeDividor = new Point(4, 4);
        }
    }
}