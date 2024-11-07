using WindowsInput;
using WindowsInput.Native;
using System;

namespace MyRemote
{
    public class KeySimulator
    {
        private readonly InputSimulator _inputSimulator;

        // Define the Modifier enum, as it is not part of WindowsInput.
        public enum Modifier
        {
            SHIFT,
            CONTROL,
            ALT,
            WIN
        }

        public KeySimulator()
        {
            _inputSimulator = new InputSimulator();
        }

        // Main method to simulate key press with optional modifiers
        public void SimulateKeyPress(string key, string[] modifiers)
        {
            // Simulate the modifiers being pressed
            if (modifiers != null)
            {
                foreach (var modifier in modifiers)
                {
                    if (Enum.TryParse(modifier, true, out Modifier parsedModifier))
                    {
                        _inputSimulator.Keyboard.KeyDown(ConvertModifierToVirtualKey(parsedModifier));
                    }
                    else
                    {
                        Console.WriteLine($"Invalid modifier: {modifier}");
                    }
                }
            }

            // Simulate the key press
            if (!string.IsNullOrEmpty(key))
            {
                var virtualKey = ConvertKey(key);
                _inputSimulator.Keyboard.KeyPress(virtualKey);
            }

            // Release all modifiers
            if (modifiers != null)
            {
                foreach (var modifier in modifiers)
                {
                    if (Enum.TryParse(modifier, true, out Modifier parsedModifier))
                    {
                        _inputSimulator.Keyboard.KeyUp(ConvertModifierToVirtualKey(parsedModifier));
                    }
                }
            }
        }

        // Converts a key string (e.g., "VK_S") to VirtualKeyCode
        private WindowsInput.Native.VirtualKeyCode ConvertKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            }

            // Attempt to parse the key as VirtualKeyCode
            if (Enum.TryParse(key, true, out WindowsInput.Native.VirtualKeyCode virtualKey))
            {
                return virtualKey;
            }
            else
            {
                // If the key is invalid, throw an exception
                throw new ArgumentOutOfRangeException(nameof(key), $"Invalid key: {key}");
            }
        }

        // Converts Modifier enum to VirtualKeyCode
        private WindowsInput.Native.VirtualKeyCode ConvertModifierToVirtualKey(Modifier modifier)
        {
            switch (modifier)
            {
                case Modifier.SHIFT: return WindowsInput.Native.VirtualKeyCode.SHIFT;
                case Modifier.CONTROL: return WindowsInput.Native.VirtualKeyCode.CONTROL;
                case Modifier.ALT: return WindowsInput.Native.VirtualKeyCode.MENU; // Alt key
                case Modifier.WIN: return WindowsInput.Native.VirtualKeyCode.LWIN; // Windows key
                default: 
                    throw new ArgumentException($"Invalid modifier: {modifier}", nameof(modifier));
            }
        }
    }
}
