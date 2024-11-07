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
        // Add other VirtualKeyCode values here...
    }

    public static class MessageHandler
    {
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

                // Proceed with the key processing logic (like calling ConvertKey)
                VirtualKeyCode virtualKey = ConvertKey(key);
                // Further logic...
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
                // Try parsing the key into the enum
                return (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), key, true);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Invalid key: {key}");
                return VirtualKeyCode.UNKNOWN; // Default fallback key
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
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
