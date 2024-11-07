// MessageHandler.cs
using System;

namespace MyRemote
{
    public static class MessageHandler
    {
        // This method processes each received message
        public static void ProcessMessage(string message)
        {
            // For now, log the message to the console
            Console.WriteLine("MessageHandler received: " + message);

            // In the future, implement logic here to send keystrokes to an application
            // using the `message` content.
        }
    }
}
