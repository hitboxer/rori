namespace rori.Helpers.Native
{
    public static class NativeConstants
    {
        public const int GCL_HICONSM = -34;
        public const int GCL_HICON = -14;
        public const int ICON_SMALL = 0;
        public const int ICON_BIG = 1;
        public const int ICON_SMALL2 = 2;
        public const int SC_MINIMIZE = 0xF020;
        public const int HT_CAPTION = 2;
        public const int CURSOR_SHOWING = 1;
        public const int GWL_STYLE = -16;
        public const int DWM_TNP_RECTDESTINATION = 0x1;
        public const int DWM_TNP_RECTSOURCE = 0x2;
        public const int DWM_TNP_OPACITY = 0x4;
        public const int DWM_TNP_VISIBLE = 0x8;
        public const int DWM_TNP_SOURCECLIENTAREAONLY = 0x10;
        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE_LL = 14;
        public const int ULW_ALPHA = 0x02;
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;

        public const int DI_MASK = 0x0001;

        public const int DI_IMAGE = 0x0002;

        public const int DI_NORMAL = 0x0003;

        public const int DI_COMPAT = 0x0004;

        public const int DI_DEFAULTSIZE = 0x0008;

        public const int DI_NOMIRROR = 0x0010;

        public const uint ECM_FIRST = 0x1500;
        public const uint EM_SETCUEBANNER = ECM_FIRST + 1;
        public const uint MA_ACTIVATE = 1;
        public const uint MA_ACTIVATEANDEAT = 2;
        public const uint MA_NOACTIVATE = 3;
        public const uint MA_NOACTIVATEANDEAT = 4;
        public const uint MOUSE_MOVE = 0xF012;
    }
}