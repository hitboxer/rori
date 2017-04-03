using rori.Helpers;
using rori.Helpers.Native;
using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Size = System.Drawing.Size;

namespace rori.Drawing
{
    public class Capture
    {
        public CaptureMode Mode { get; private set; }

        public Capture(CaptureMode mode)
        {
            Mode = mode;
        }

        public Image GetScreenshot(IntPtr window = default(IntPtr))
        {
            var screenshot = default(Image);

            switch (Mode)
            {
                case CaptureMode.Region:
                    screenshot = CreateScreenshot();
                    break;

                case CaptureMode.Screen:
                    screenshot = CreateScreenshot((int)SystemParameters.VirtualScreenLeft, (int)SystemParameters.VirtualScreenTop, (int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight);
                    break;

                case CaptureMode.ActiveWindow:
                    if (window != null) { WindowManager.ForceForegroundWindow(window); Thread.Sleep(500); }
                    screenshot = CreateScreenshot(NativeMethods.GetForegroundWindow());
                    break;

                case CaptureMode.Window:
                    if (window != null)
                        screenshot = CreateScreenshot(window);
                    break;
            }

            return screenshot;
        }

        private static Bitmap CreateScreenshot()
        {
            using (var bmp = new Bitmap((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight))
            {
                using (var graphic = Graphics.FromImage(bmp))
                {
                    graphic.CopyFromScreen((int)SystemParameters.VirtualScreenLeft, (int)SystemParameters.VirtualScreenTop, 0, 0, bmp.Size);

                    var hBitmap = bmp.GetHbitmap();

                    var selection = new SelectionWindow(new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())));
                    if (selection.ShowDialog() != true) return default(Bitmap);

                    var result = new Bitmap(selection.Result.Width, selection.Result.Height, PixelFormat.Format24bppRgb);
                    using (var resultgraphic = Graphics.FromImage(result))
                    {
                        resultgraphic.DrawImage(bmp, new Rectangle(0, 0, selection.Result.Width, selection.Result.Height), selection.Result, GraphicsUnit.Pixel);
                    }

                    NativeMethods.DeleteObject(hBitmap);
                    return result;
                }
            }
        }

        private static Bitmap CreateScreenshot(int left, int top, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            using (var graphic = Graphics.FromImage(bmp))
            {
                graphic.CopyFromScreen(left, top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
            }
            return bmp;
        }

        private static Bitmap CreateScreenshot(IntPtr handle)
        {
            var rect = new NativeTypes.RECT();
            NativeMethods.GetWindowRect(handle, ref rect);
            return CreateScreenshot(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }
    }
}