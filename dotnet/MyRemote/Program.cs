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
        static WebSocketServer? webSocketServer;
        static WebServer? webServer;

        [STAThread]
        static void Main()
        {
            string envPath = ".env";
            Env.Load(envPath);

            string appTitle = Env.GetString("VITE_APP_TITLE", "MyRemote");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int wsPort = int.Parse(Env.GetString("VITE_WS_PORT", "8223"));
            int httpPort = int.Parse(Env.GetString("VITE_HTTP_PORT", "8224"));
            string httpUrl = GetHttpURL(httpPort, wsPort);

            Task.Run(() => StartWebSocketServer(wsPort));
            Task.Run(() => StartWebServer(httpPort));

            NotifyIcon trayIcon = new NotifyIcon()
            {
                Icon = new Icon("systray-icon.ico"),
                Visible = true,
                Text = appTitle,
            };

            trayIcon.Text = "Right-click to open control page or scan QR";

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add(
                $"Open {appTitle}",
                null,
                (sender, e) =>
                {
                    OpenPageOnPublicIp(httpUrl);
                }
            );

            contextMenu.Items.Add("Show QR Code", null, (s, e) =>
            {
                new QRForm(httpUrl).Show();
            });

            contextMenu.Items.Add(new ToolStripSeparator());

            contextMenu.Items.Add(
                "Exit",
                null,
                async (sender, e) =>
                {
                    trayIcon.Visible = false;

                    if (webSocketServer != null)
                        await webSocketServer.StopAsync();

                    if (webServer != null)
                        await webServer.StopAsync();

                    Application.Exit();
                }
            );

            trayIcon.ContextMenuStrip = contextMenu;

            Application.ApplicationExit += async (sender, e) =>
            {
                if (webSocketServer != null)
                    await webSocketServer.StopAsync();

                if (webServer != null)
                    await webServer.StopAsync();
            };
                
            Application.Run();
        }

        private static async Task StartWebServer(int port)
        {
            webServer = new WebServer(".");
            await webServer.StartAsync(port);
        }

        private static async Task StartWebSocketServer(int port)
        {
            webSocketServer = new WebSocketServer();
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
