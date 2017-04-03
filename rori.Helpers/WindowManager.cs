using rori.Helpers.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace rori.Helpers
{
    public static class WindowManager
    {
        public static IDictionary<IntPtr, string> GetOpenWindows()
        {
            var windows = new Dictionary<IntPtr, string>();
            var shellWindow = NativeMethods.GetShellWindow();

            NativeMethods.EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                int size = NativeMethods.GetWindowTextLength(hWnd);

                if (hWnd == shellWindow) return true;
                if (size++ > 0 && NativeMethods.IsWindowVisible(hWnd))
                {
                    var sb = new StringBuilder(size);
                    NativeMethods.GetWindowText(hWnd, sb, size);

                    if (sb.ToString() != "Start") windows.Add(hWnd, sb.ToString());
                }

                return true;
            }, IntPtr.Zero);

            return windows;
        }

        public static void ForceForegroundWindow(IntPtr hWnd)
        {
            var foreThread = NativeMethods.GetWindowThreadProcessId(NativeMethods.GetForegroundWindow(), IntPtr.Zero);

            var appThread = NativeMethods.GetCurrentThreadId();

            if (foreThread != appThread)
            {
                NativeMethods.AttachThreadInput(foreThread, appThread, true);
                NativeMethods.BringWindowToTop(hWnd);
                if (!isWindowMaximized(hWnd))
                {
                    NativeMethods.ShowWindow(hWnd, ShowWindowCommands.Show);
                }
                NativeMethods.AttachThreadInput(foreThread, appThread, false);
            }
            else
            {
                NativeMethods.BringWindowToTop(hWnd);
                if (!isWindowMaximized(hWnd))
                {
                    NativeMethods.ShowWindow(hWnd, ShowWindowCommands.Show);
                }
            }
        }

        public static void HideTaskbar(bool visibility)
        {
            if (visibility)
            {
                NativeMethods.ShowWindow(NativeMethods.FindWindowByCaption(IntPtr.Zero, "Shell_TrayWnd"), ShowWindowCommands.Hide);
            }
            else
            {
                NativeMethods.ShowWindow(NativeMethods.FindWindowByCaption(IntPtr.Zero, "Shell_TrayWnd"), ShowWindowCommands.Show);
            }
        }

        private static WindowPlacement GetWindowPlacementByHandle(IntPtr window)
        {
            var placement = new WindowPlacement();
            placement.length = Marshal.SizeOf(placement);
            NativeMethods.GetWindowPlacement(window, ref placement);
            return placement;
        }

        private static bool isWindowMaximized(IntPtr handle)
        {
            return GetWindowPlacementByHandle(handle).showCmd == ShowWindowCommands.Maximize;
        }
    }
}