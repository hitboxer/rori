using rori.Helpers.Native;
using System;
using System.Drawing;

namespace rori.Helpers
{
    public static class NativeMethodsManager
    {
        public static Icon GetApplicationIconSmall(IntPtr handle)
        {
            var iconHandle = IntPtr.Zero;

            NativeMethods.SendMessageTimeout(handle, (int)WindowsMessages.WM_GETICON, NativeConstants.ICON_SMALL2, 0, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out iconHandle);

            if (iconHandle == IntPtr.Zero)
            {
                NativeMethods.SendMessageTimeout(handle, (int)WindowsMessages.WM_GETICON, NativeConstants.ICON_SMALL, 0, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out iconHandle);

                if (iconHandle == IntPtr.Zero)
                {
                    iconHandle = GetClassLongPtrSafe(handle, NativeConstants.GCL_HICONSM);

                    if (iconHandle == IntPtr.Zero)
                    {
                        NativeMethods.SendMessageTimeout(handle, (int)WindowsMessages.WM_QUERYDRAGICON, 0, 0, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out iconHandle);
                    }
                }
            }

            if (iconHandle != IntPtr.Zero)
            {
                return Icon.FromHandle(iconHandle);
            }

            return null;
        }

        public static IntPtr GetClassLongPtrSafe(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size > 4)
            {
                return GetClassLongPtr(hWnd, nIndex);
            }

            return new IntPtr(NativeMethods.GetClassLong(hWnd, nIndex));
        }

        public static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size > 4)
                return NativeMethods.GetClassLongPtr64(hWnd, nIndex);
            else
                return new IntPtr(NativeMethods.GetClassLongPtr32(hWnd, nIndex));
        }
    }
}