using System.Collections.Generic;
using WindowsInput.Native;

public static class KeyMapping
{
    public static readonly Dictionary<string, VirtualKeyCode> RobotJsToVirtualKeyCode =
        new Dictionary<string, VirtualKeyCode>
        {
            { "backspace", VirtualKeyCode.BACK },
            { "delete", VirtualKeyCode.DELETE },
            { "enter", VirtualKeyCode.RETURN },
            { "tab", VirtualKeyCode.TAB },
            { "escape", VirtualKeyCode.ESCAPE },
            { "up", VirtualKeyCode.UP },
            { "down", VirtualKeyCode.DOWN },
            { "right", VirtualKeyCode.RIGHT },
            { "left", VirtualKeyCode.LEFT },
            { "home", VirtualKeyCode.HOME },
            { "end", VirtualKeyCode.END },
            { "pageup", VirtualKeyCode.PRIOR },
            { "pagedown", VirtualKeyCode.NEXT },
            { "f1", VirtualKeyCode.F1 },
            { "f2", VirtualKeyCode.F2 },
            { "f3", VirtualKeyCode.F3 },
            { "f4", VirtualKeyCode.F4 },
            { "f5", VirtualKeyCode.F5 },
            { "f6", VirtualKeyCode.F6 },
            { "f7", VirtualKeyCode.F7 },
            { "f8", VirtualKeyCode.F8 },
            { "f9", VirtualKeyCode.F9 },
            { "f10", VirtualKeyCode.F10 },
            { "f11", VirtualKeyCode.F11 },
            { "f12", VirtualKeyCode.F12 },
            { "command", VirtualKeyCode.LWIN },
            { "alt", VirtualKeyCode.MENU },
            { "control", VirtualKeyCode.CONTROL },
            { "shift", VirtualKeyCode.SHIFT },
            { "right_shift", VirtualKeyCode.RSHIFT },
            { "space", VirtualKeyCode.SPACE },
            { "printscreen", VirtualKeyCode.SNAPSHOT },
            { "insert", VirtualKeyCode.INSERT },
            // Audio control keys
            { "audio_mute", VirtualKeyCode.VOLUME_MUTE },
            { "audio_vol_down", VirtualKeyCode.VOLUME_DOWN },
            { "audio_vol_up", VirtualKeyCode.VOLUME_UP },
            { "audio_play", VirtualKeyCode.MEDIA_PLAY_PAUSE },
            { "audio_stop", VirtualKeyCode.MEDIA_STOP },
            { "audio_pause", VirtualKeyCode.MEDIA_PLAY_PAUSE }, // Pause is often mapped to play/pause toggle
            { "audio_prev", VirtualKeyCode.MEDIA_PREV_TRACK },
            { "audio_next", VirtualKeyCode.MEDIA_NEXT_TRACK },
            // Numpad keys
            { "numpad_0", VirtualKeyCode.NUMPAD0 },
            { "numpad_1", VirtualKeyCode.NUMPAD1 },
            { "numpad_2", VirtualKeyCode.NUMPAD2 },
            { "numpad_3", VirtualKeyCode.NUMPAD3 },
            { "numpad_4", VirtualKeyCode.NUMPAD4 },
            { "numpad_5", VirtualKeyCode.NUMPAD5 },
            { "numpad_6", VirtualKeyCode.NUMPAD6 },
            { "numpad_7", VirtualKeyCode.NUMPAD7 },
            { "numpad_8", VirtualKeyCode.NUMPAD8 },
            { "numpad_9", VirtualKeyCode.NUMPAD9 },

            // Special keys (unavailable on Windows platform in VirtualKeyCode)
            // { "lights_mon_up", null }, // Brightness controls not supported on Windows
            // { "lights_mon_down", null },
            // { "lights_kbd_toggle", null },
            // { "lights_kbd_up", null },
            // { "lights_kbd_down", null }
        };

    public static VirtualKeyCode GetVirtualKeyCode(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Key cannot be null or empty", nameof(key));
        }

        // Check if the key is a single character
        if (key.Length == 1)
        {
            string formattedKey = "VK_" + key.ToUpper();

            // Try parsing as a single-character key with "VK_" prefix
            if (Enum.TryParse(formattedKey, true, out VirtualKeyCode virtualKey))
            {
                return virtualKey;
            }
        }

        // If it's a special key, try to look it up in the dictionary
        if (RobotJsToVirtualKeyCode.TryGetValue(key.ToLower(), out var mappedKey))
        {
            return mappedKey;
        }

        // If no valid key was found, throw an exception
        throw new ArgumentOutOfRangeException(nameof(key), $"Invalid key mapping: {key}");
    }
}
