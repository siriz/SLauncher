using System;
using System.Runtime.InteropServices;

namespace SLauncher.Classes
{
    /// <summary>
    /// Global hotkey manager using Win32 RegisterHotKey API
    /// </summary>
    public class GlobalHotkeyManager : IDisposable
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Modifiers
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;
        private const uint MOD_NOREPEAT = 0x4000;

        // Virtual key codes
        private const uint VK_SPACE = 0x20;

        // Window messages
        private const uint WM_HOTKEY = 0x0312;

        // Hotkey ID
        private const int HOTKEY_ID = 9000;

        private IntPtr _windowHandle;
        private Action _onHotkeyPressed;
        private IntPtr _oldWndProc;
        private WndProcDelegate _newWndProc;

        private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public GlobalHotkeyManager(IntPtr windowHandle)
        {
            _windowHandle = windowHandle;
        }

        /// <summary>
        /// Register Ctrl+Space hotkey
        /// </summary>
        public bool RegisterCtrlSpace(Action onHotkeyPressed)
        {
            return RegisterHotkey("Ctrl+Space", onHotkeyPressed);
        }

        /// <summary>
        /// Register a hotkey with custom modifiers and key
        /// </summary>
        public bool RegisterHotkey(string hotkeyString, Action onHotkeyPressed)
        {
            _onHotkeyPressed = onHotkeyPressed;

            // Parse hotkey string (e.g., "Ctrl+Space", "Ctrl+Alt+F1")
            uint modifiers = MOD_NOREPEAT;
            uint vk = 0;

            string[] parts = hotkeyString.Split('+');
            for (int i = 0; i < parts.Length - 1; i++)
            {
                string part = parts[i].Trim();
                if (part == "Ctrl" || part == "Control")
                    modifiers |= MOD_CONTROL;
                else if (part == "Alt")
                    modifiers |= MOD_ALT;
                else if (part == "Shift")
                    modifiers |= MOD_SHIFT;
                else if (part == "Win" || part == "Windows")
                    modifiers |= MOD_WIN;
            }

            // Get virtual key code
            string key = parts[parts.Length - 1].Trim();
            vk = GetVirtualKeyCode(key);

            if (vk == 0)
            {
                System.Diagnostics.Debug.WriteLine($"Unknown key: {key}");
                return false;
            }

            // Register hotkey
            bool success = RegisterHotKey(_windowHandle, HOTKEY_ID, modifiers, vk);

            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                System.Diagnostics.Debug.WriteLine($"Failed to register hotkey. Error: {error}");
                return false;
            }

            // Subclass window to receive WM_HOTKEY messages
            _newWndProc = new WndProcDelegate(WndProc);
            _oldWndProc = SetWindowLongPtr(_windowHandle, GWL_WNDPROC,
                Marshal.GetFunctionPointerForDelegate(_newWndProc));

            System.Diagnostics.Debug.WriteLine($"{hotkeyString} hotkey registered successfully");
            return true;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const int GWL_WNDPROC = -4;

        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WM_HOTKEY)
            {
                int hotkeyId = wParam.ToInt32();
                if (hotkeyId == HOTKEY_ID)
                {
                    System.Diagnostics.Debug.WriteLine("Ctrl+Space hotkey triggered!");
                    _onHotkeyPressed?.Invoke();
                }
            }

            // Call original window procedure
            return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
        }

        private uint GetVirtualKeyCode(string key)
        {
            switch (key.ToLower())
            {
                case "space": return 0x20;
                case "tab": return 0x09;
                case "enter": case "return": return 0x0D;
                case "esc": case "escape": return 0x1B;
                case "f1": return 0x70;
                case "f2": return 0x71;
                case "f3": return 0x72;
                case "f4": return 0x73;
                case "f5": return 0x74;
                case "f6": return 0x75;
                case "f7": return 0x76;
                case "f8": return 0x77;
                case "f9": return 0x78;
                case "f10": return 0x79;
                case "f11": return 0x7A;
                case "f12": return 0x7B;
                default: return 0;
            }
        }

        public void Dispose()
        {
            // Unregister hotkey
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            System.Diagnostics.Debug.WriteLine("Ctrl+Space hotkey unregistered");

            // Restore original window procedure
            if (_oldWndProc != IntPtr.Zero && _windowHandle != IntPtr.Zero)
            {
                SetWindowLongPtr(_windowHandle, GWL_WNDPROC, _oldWndProc);
            }
        }
    }
}
