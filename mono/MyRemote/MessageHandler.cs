using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using DotNetEnv;
using WindowsInput;
using WindowsInput.Native; 

namespace MyRemote
{
    public static class MessageHandler
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        static MessageHandler()
        {
            // Load the environment variables to access TARGET_APP_REGEX
            Env.Load("../../.env");
        }

        public static void ProcessMessage(string message)
        {
            Console.WriteLine("Processing message: " + message);

            // Retrieve target app regex from environment variables
            string targetAppPattern = Env.GetString("VITE_APP_RE", "Cubase");

            // Attempt to bring the target app to the foreground
            if (BringAppToForeground(targetAppPattern))
            {
                // Create an InputSimulator object
                var sim = new InputSimulator();

                // Simulate pressing CONTROL + S
                sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_S);
                Console.WriteLine("Sent CTRL+S to " + targetAppPattern);
            }
            else
            {
                Console.WriteLine("Application not found or could not be focused.");
            }
        }

        private static bool BringAppToForeground(string appPattern)
        {
            // Find the target window using a regex pattern on process names
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
