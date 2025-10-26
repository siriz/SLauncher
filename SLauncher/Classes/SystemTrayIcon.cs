using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace SLauncher.Classes
{
    /// <summary>
    /// Simple system tray icon helper using Win32 API
    /// </summary>
    public class SystemTrayIcon : IDisposable
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern bool Shell_NotifyIcon(NotifyIconAction dwMessage, ref NOTIFYICONDATA lpData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool AppendMenu(IntPtr hMenu, uint uFlags, UIntPtr uIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        private static extern bool TrackPopupMenuEx(IntPtr hMenu, uint uFlags, int x, int y, IntPtr hWnd, IntPtr lptpm);

        [DllImport("user32.dll")]
        private static extern bool DestroyMenu(IntPtr hMenu);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        private const int GWL_WNDPROC = -4;
        private const int WM_LBUTTONDBLCLK = 0x0203;
        private const int WM_RBUTTONUP = 0x0205;
        private const uint WM_USER = 0x0400;
        private const uint WM_TRAYICON = WM_USER + 1;
        private const uint WM_COMMAND = 0x0111;

        // Menu IDs
        private const uint MENU_OPEN = 1000;
        private const uint MENU_SETTINGS = 1001;
        private const uint MENU_EXIT = 1002;

        // Menu flags
        private const uint MF_STRING = 0x00000000;
        private const uint MF_SEPARATOR = 0x00000800;
        private const uint TPM_BOTTOMALIGN = 0x0020;
        private const uint TPM_LEFTALIGN = 0x0000;

        private NOTIFYICONDATA _iconData;
        private IntPtr _windowHandle;
        private Action _onLeftClick;
        private Action _onOpenMenu;
        private Action _onSettingsMenu;
        private Action _onExitMenu;
        private IntPtr _oldWndProc;
        private WndProcDelegate _newWndProc;

        // Delegate for window procedure
        private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        public SystemTrayIcon(IntPtr windowHandle, string iconPath, string tooltip)
        {
            _windowHandle = windowHandle;

            _iconData = new NOTIFYICONDATA();
            _iconData.cbSize = Marshal.SizeOf(_iconData);
            _iconData.hWnd = windowHandle;
            _iconData.uID = 1;
            _iconData.uFlags = NotifyIconFlags.NIF_ICON | NotifyIconFlags.NIF_MESSAGE | NotifyIconFlags.NIF_TIP;
            _iconData.uCallbackMessage = WM_TRAYICON;

            // Load icon
            _iconData.hIcon = LoadIcon(iconPath);

            // Set tooltip
            _iconData.szTip = tooltip;

            // Subclass the window to receive tray icon messages
            _newWndProc = new WndProcDelegate(WndProc);
            _oldWndProc = SetWindowLongPtr(windowHandle, GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(_newWndProc));

            // Add tray icon
            Shell_NotifyIcon(NotifyIconAction.NIM_ADD, ref _iconData);
        }

        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WM_TRAYICON)
            {
                switch ((int)lParam)
                {
                    case WM_LBUTTONDBLCLK:
                        _onLeftClick?.Invoke();
                        break;
                    case WM_RBUTTONUP:
                        ShowContextMenu();
                        break;
                }
            }
            else if (msg == WM_COMMAND)
            {
                uint menuId = (uint)(wParam.ToInt32() & 0xFFFF);
                switch (menuId)
                {
                    case MENU_OPEN:
                        _onOpenMenu?.Invoke();
                        break;
                    case MENU_SETTINGS:
                        _onSettingsMenu?.Invoke();
                        break;
                    case MENU_EXIT:
                        _onExitMenu?.Invoke();
                        break;
                }
            }

            // Call the original window procedure
            return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
        }

        private void ShowContextMenu()
        {
            // Create popup menu
            IntPtr hMenu = CreatePopupMenu();

            // Add menu items
            AppendMenu(hMenu, MF_STRING, (UIntPtr)MENU_OPEN, "Open SLauncher");
            AppendMenu(hMenu, MF_STRING, (UIntPtr)MENU_SETTINGS, "Settings");
            AppendMenu(hMenu, MF_SEPARATOR, UIntPtr.Zero, null);
            AppendMenu(hMenu, MF_STRING, (UIntPtr)MENU_EXIT, "Exit");

            // Get cursor position
            GetCursorPos(out POINT cursorPos);

            // Set foreground window (required for TrackPopupMenuEx to work properly)
            SetForegroundWindow(_windowHandle);

            // Show menu
            TrackPopupMenuEx(hMenu, TPM_BOTTOMALIGN | TPM_LEFTALIGN, cursorPos.X, cursorPos.Y, _windowHandle, IntPtr.Zero);

            // Clean up
            DestroyMenu(hMenu);
        }

        private IntPtr LoadIcon(string iconPath)
        {
            try
            {
                if (System.IO.File.Exists(iconPath))
                {
                    return LoadImage(IntPtr.Zero, iconPath, 1, 0, 0, 0x00000010);
                }
            }
            catch { }

            // Return default application icon if file not found
            return LoadIcon(IntPtr.Zero, new IntPtr(32512)); // IDI_APPLICATION
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        public void SetOnLeftClick(Action action)
        {
            _onLeftClick = action;
        }

        public void SetOnOpenMenu(Action action)
        {
            _onOpenMenu = action;
        }

        public void SetOnSettingsMenu(Action action)
        {
            _onSettingsMenu = action;
        }

        public void SetOnExitMenu(Action action)
        {
            _onExitMenu = action;
        }

        public void Dispose()
        {
            // Restore original window procedure
            if (_oldWndProc != IntPtr.Zero && _windowHandle != IntPtr.Zero)
            {
                SetWindowLongPtr(_windowHandle, GWL_WNDPROC, _oldWndProc);
            }

            Shell_NotifyIcon(NotifyIconAction.NIM_DELETE, ref _iconData);

            if (_iconData.hIcon != IntPtr.Zero)
            {
                DestroyIcon(_iconData.hIcon);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr hIcon);

        private enum NotifyIconAction
        {
            NIM_ADD = 0x00000000,
            NIM_MODIFY = 0x00000001,
            NIM_DELETE = 0x00000002,
        }

        [Flags]
        private enum NotifyIconFlags
        {
            NIF_MESSAGE = 0x00000001,
            NIF_ICON = 0x00000002,
            NIF_TIP = 0x00000004,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct NOTIFYICONDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uID;
            public NotifyIconFlags uFlags;
            public uint uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szTip;
        }
    }
}
