using System;

namespace rori.Drawing
{
    [Flags]
    public enum CaptureMode
    {
        Region,
        Screen,
        Window,
        ActiveWindow
    }
}