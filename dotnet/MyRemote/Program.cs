using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
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
            string envPath = ".env";
            Env.Load(envPath);

            string appTitle = Env.GetString("VITE_APP_TITLE", "MyRemote");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int wsPort = int.Parse(Env.GetString("VITE_WS_PORT", "8081"));
            int httpPort = int.Parse(Env.GetString("VITE_HTTP_PORT", "8080"));
            string httpUrl = GetHttpURL(httpPort, wsPort);

            Task.Run(() => StartWebSocketServer(wsPort));
            Task.Run(() => StartWebServer(httpPort));

            NotifyIcon trayIcon = new NotifyIcon()
            {
                Icon = new Icon("systray-icon.ico"),
                Visible = true,
                Text = appTitle,
            };

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add(
                "Open",
                null,
                (sender, e) =>
                {
                    OpenPageOnPublicIp(httpUrl);
                }
            );
            contextMenu.Items.Add(
                "Exit",
                null,
                (sender, e) =>
                {
                    trayIcon.Visible = false;
                    Application.Exit();
                }
            );
            trayIcon.ContextMenuStrip = contextMenu;

            // Run the application
            Application.Run();
        }

        private static async Task StartWebServer(int port)
        {
            var webServer = new WebServer(".");
            await webServer.StartAsync(port);
        }

        private static async Task StartWebSocketServer(int port)
        {
            WebSocketServer webSocketServer = new WebSocketServer();
            await webSocketServer.StartAsync(port);
        }

        public static string GetPublicIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host
                .AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)
                .ToString();
        }

        public static void OpenPageOnPublicIp(string url)
        {
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        }

        public static string GetHttpURL(int httpPort, int wsPort)
        {
            var publicIp = GetPublicIp();
            return "http://"
                + publicIp
                + ':'
                + httpPort
                + '?'
                + Uri.EscapeDataString("http://" + publicIp + ':' + wsPort);
        }
    }
}
