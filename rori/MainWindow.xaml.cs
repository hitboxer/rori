using rori.Helpers;
using rori.Helpers.Extension;
using System;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace rori
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ContextMenuStrip TrayIconMenu = new ContextMenuStrip();

        private static NotifyIcon TrayIcon = new NotifyIcon()
        {
            Text = "rori",
            Visible = true,
            ContextMenuStrip = TrayIconMenu,
            Icon = Properties.Resources.rori
        };

        public MainWindow()
        {
            InitializeComponent();
            LoadComponents();
        }

        private async void TestUpload(byte[] file)
        {
            var x = await new Network.Upload.UploadService(new Network.Upload.CustomUploadService("nnlv", "file", new Uri("http://f.nn.lv/"), null, null, @"(http:\/\/nn\.lv\/\w*)")).Upload(file, "testxxx.png");
            System.Windows.MessageBox.Show(x.Result.ToString());
            System.Windows.Clipboard.SetText(x.Result.ToString());
        }

        private void TestRegionScreenshot()
        {
            using (var x = new Drawing.Capture(Drawing.CaptureMode.Region).GetScreenshot())
            {
                if (x != null) TestUpload(x.ToByteArray());
            }
        }

        private ToolStripMenuItem _captureWindowMenuItem = new ToolStripMenuItem("Window");

        private void LoadComponents()
        {
            var exitMenuItem = new ToolStripMenuItem("Exit", null, delegate { Application.Current.Shutdown(); });
            var captureFullscreenMenuItem = new ToolStripMenuItem("Fullscreen");
            var captureMonitorMenuItem = new ToolStripMenuItem("Monitor");
            var captureRegionMenuItem = new ToolStripMenuItem("Region", null, delegate { TestRegionScreenshot(); });
            var captureMenuItem = new ToolStripMenuItem("Capture", null, new ToolStripItem[] { captureFullscreenMenuItem, _captureWindowMenuItem, captureRegionMenuItem });

            TrayIconMenu.Items.AddRange(new ToolStripItem[] { captureMenuItem, new ToolStripSeparator(), exitMenuItem });

            Application.Current.Exit += Current_Exit;
            TrayIcon.MouseClick += TrayIcon_MouseClick;
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _captureWindowMenuItem.DropDownItems.Clear();
                AddDropDownWindows(_captureWindowMenuItem);
            }
        }

        private void AddDropDownWindows(ToolStripMenuItem menuItem)
        {
            foreach (var item in WindowManager.GetOpenWindows())
            {
                menuItem.DropDownItems.Add(item.Value, NativeMethodsManager.GetApplicationIconSmall(item.Key).ToBitmap(), delegate
                {
                    using (var x = new Drawing.Capture(Drawing.CaptureMode.ActiveWindow).GetScreenshot(item.Key))
                    {
                        TestUpload(x.ToByteArray());
                    }
                });
            }
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            TrayIconMenu.Dispose();
            TrayIcon.Dispose();
        }
    }
}