using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace GATShipD365TS.App_Code
{
    public class Window
    {
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void Show()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOW); // To show
        }

        public static void Hide()
        {
            Thread.Sleep(2000);
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE); // To hide
        }
    }
}
