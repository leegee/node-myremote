using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using DotNetEnv;

namespace MyRemote
{
    public enum VirtualKeyCode
    {
        UNKNOWN = 0, // Fallback value for invalid keys

        // Letters
        VK_A = 65,
        VK_B = 66,
        VK_C = 67,
        VK_D = 68,
        VK_E = 69,
        VK_F = 70,
        VK_G = 71,
        VK_H = 72,
        VK_I = 73,
        VK_J = 74,
        VK_K = 75,
        VK_L = 76,
        VK_M = 77,
        VK_N = 78,
        VK_O = 79,
        VK_P = 80,
        VK_Q = 81,
        VK_R = 82,
        VK_S = 83,
        VK_T = 84,
        VK_U = 85,
        VK_V = 86,
        VK_W = 87,
        VK_X = 88,
        VK_Y = 89,
        VK_Z = 90,

        // Numbers (Main row)
        VK_1 = 49,
        VK_2 = 50,
        VK_3 = 51,
        VK_4 = 52,
        VK_5 = 53,
        VK_6 = 54,
        VK_7 = 55,
        VK_8 = 56,
        VK_9 = 57,
        VK_0 = 48,

        // Punctuation and special characters
        VK_OEM_PLUS = 187, // Equals/Plus '=' key
        VK_OEM_COMMA = 188, // Comma ',' key
        VK_OEM_MINUS = 189, // Minus/Underscore '-' key
        VK_OEM_PERIOD = 190, // Period/Greater-than '.' key
        VK_OEM_2 = 191, // Slash/Question mark '/' key
        VK_OEM_1 = 192, // Tilde/Grave accent '`' key
        VK_OEM_3 = 192, // Grave accent '`' key (same as VK_OEM_1)
        VK_OEM_4 = 219, // Left square bracket '[' key
        VK_OEM_5 = 220, // Backslash '\' key
        VK_OEM_6 = 221, // Right square bracket ']' key
        VK_OEM_7 = 222, // Single quote/Double quote "'" key

        // Function keys
        VK_F1 = 112,
        VK_F2 = 113,
        VK_F3 = 114,
        VK_F4 = 115,
        VK_F5 = 116,
        VK_F6 = 117,
        VK_F7 = 118,
        VK_F8 = 119,
        VK_F9 = 120,
        VK_F10 = 121,
        VK_F11 = 122,
        VK_F12 = 123,

        // Control keys
        VK_ESCAPE = 27,
        VK_SPACE = 32,
        VK_ENTER = 13,
        VK_TAB = 9,
        VK_BACK = 8,
        VK_SHIFT = 16,
        VK_CONTROL = 17,
        VK_MENU = 18, // Alt key
        VK_PAUSE = 19,
        VK_CAPITAL = 20, // Caps Lock key
        VK_ESC = 27, // Escape key (same as VK_ESCAPE)

        // Arrow keys
        VK_LEFT = 37,
        VK_UP = 38,
        VK_RIGHT = 39,
        VK_DOWN = 40,

        // Numeric keypad
        VK_NUMPAD0 = 96,
        VK_NUMPAD1 = 97,
        VK_NUMPAD2 = 98,
        VK_NUMPAD3 = 99,
        VK_NUMPAD4 = 100,
        VK_NUMPAD5 = 101,
        VK_NUMPAD6 = 102,
        VK_NUMPAD7 = 103,
        VK_NUMPAD8 = 104,
        VK_NUMPAD9 = 105,
        VK_NUMPAD_ADD = 107,
        VK_NUMPAD_SUBTRACT = 109,
        VK_NUMPAD_MULTIPLY = 106,
        VK_NUMPAD_DIVIDE = 111,
        VK_NUMPAD_DECIMAL = 110,
        VK_NUMPAD_ENTER = 13,

        // Modifier keys
        VK_SHIFT_LEFT = 160,
        VK_SHIFT_RIGHT = 161,
        VK_CONTROL_LEFT = 162,
        VK_CONTROL_RIGHT = 163,
        VK_MENU_LEFT = 164,
        VK_MENU_RIGHT = 165,

        // Miscellaneous keys
        VK_INSERT = 45,
        VK_HOME = 36,
        VK_PAGEUP = 33,
        VK_PAGEDOWN = 34,
        VK_END = 35,
        VK_DELETE = 46,

        // OEM specific keys
        VK_OEM_8 = 223, // Varies by keyboard layout
        VK_OEM_102 = 226, // "<>" and "\|" key (non-US keyboard layouts)

        // Special keys (sometimes assigned depending on the system or keyboard layout)
        VK_LWIN = 91, // Left Windows key
        VK_RWIN = 92, // Right Windows key
        VK_APPS = 93, // Applications key (Menu key)
        VK_SLEEP = 95, // Sleep key

        // Media keys
        VK_MEDIA_PLAY_PAUSE = 179,
        VK_MEDIA_STOP = 178,
        VK_MEDIA_PREV_TRACK = 177,
        VK_MEDIA_NEXT_TRACK = 176,
        VK_VOLUME_MUTE = 173,
        VK_VOLUME_DOWN = 174,
        VK_VOLUME_UP = 175,
        VK_LAUNCH_MAIL = 180,
        VK_LAUNCH_MEDIA_SELECT = 181,
        VK_LAUNCH_APP1 = 182,
        VK_LAUNCH_APP2 = 183,
    }

    public static class MessageHandler
    {
        [DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        static MessageHandler()
        {
            // Load environment variables to access TARGET_APP_REGEX
            Env.Load("../../.env");
        }

        // Main function to process incoming message
        public static void ProcessMessage(string message)
        {
            Console.WriteLine("Processing message: " + message);

            JsonDocument doc = JsonDocument.Parse(message);
            JsonElement root = doc.RootElement;
            string? key = null;
            VirtualKeyCode? virtualKey = null;

            // Attempt to get the "key" property
            if (root.TryGetProperty("key", out var keyProperty))
            {
                key = keyProperty.GetString();

                // If the key is null, just return without doing anything
                if (key == null)
                {
                    Console.WriteLine("Key is null, skipping processing.");
                    return;
                }

                virtualKey = ConvertKey(key);
            }
            else
            {
                Console.WriteLine("Key property not found.");
                return;
            }

            string[] modifiers = Array.Empty<string>();

            // Attempt to get the "modifiers" property
            if (root.TryGetProperty("modifiers", out var modifiersProperty))
            {
                // Check if the modifiers property is an array
                if (modifiersProperty.ValueKind == JsonValueKind.Array)
                {
                    modifiers = modifiersProperty
                        .EnumerateArray()
                        .Select(m => m.GetString())
                        .Where(m => m != null)
                        .Cast<string>()
                        .ToArray();

                    // If the array is empty, log and return
                    if (modifiers.Length == 0)
                    {
                        Console.WriteLine("Modifiers array is empty, skipping processing.");
                        return;
                    }

                    // Proceed with the modifiers logic
                    Console.WriteLine("Modifiers: " + string.Join(", ", modifiers));
                    // Further logic...
                }
                else
                {
                    Console.WriteLine("Modifiers property is not an array.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Modifiers property not found.");
                return;
            }

            // Create KeySimulator and simulate the key press
            var keySimulator = new KeySimulator();
            keySimulator.SimulateKeyPress(key, modifiers);
        }

        private static VirtualKeyCode ConvertKey(string key)
        {
            try
            {
                return (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), key.ToUpper(), true);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Message handler got an invalid key: {key}");
                return VirtualKeyCode.UNKNOWN; // Default fallback key. Maybe should die here?
            }
        }

        // Bring the target application to the foreground based on regex pattern
        private static bool BringAppToForeground(string appPattern)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (Regex.IsMatch(process.ProcessName, appPattern, RegexOptions.IgnoreCase))
                {
                    IntPtr hWnd = process.MainWindowHandle;
                    if (hWnd != IntPtr.Zero)
                    {
                        SetForegroundWindow(hWnd);
                        FlashWindow(hWnd, true);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
