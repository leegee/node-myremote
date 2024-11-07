using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetEnv;

namespace MyRemote
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            string envPath = "../../.env";  // Adjust the path to your .env file here
            Env.Load(envPath);

            string appTitle = Env.GetString("VITE_APP_TITLE", "Default App Title");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NotifyIcon trayIcon = new NotifyIcon()
            {
                Icon = new Icon("../../src/assets/systray-icon.ico"),
                Visible = true,
                Text = appTitle
            };

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Exit", null, (sender, e) =>
            {
                trayIcon.Visible = false;
                Application.Exit();
            });
            trayIcon.ContextMenuStrip = contextMenu;

            // Start WebSocket server in a background task
            int port = int.Parse(Env.GetString("VITE_WS_PORT", "8080"));
            Task.Run(() => StartWebSocketServer(port));

            // Run the application
            Application.Run();
        }

        private static async Task StartWebSocketServer(int port)
        {
            WebSocketServer webSocketServer = new WebSocketServer();
            await webSocketServer.StartAsync(port);
        }
    }
}
