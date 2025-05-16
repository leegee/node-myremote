using System;
using System.Net;
using System.Net.Sockets;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace MyRemote
{
    public class WebSocketServer
    {

        private readonly List<Task> _activeConnections = new();
        private HttpListener? _listener;
        private CancellationTokenSource? _cts;

        public async Task StartAsync(int port)
        {
            try
            {
                string? publicIp = await GetIpAddress();
                if (publicIp == null)
                {
                    MessageBox.Show(
                        "Unable to retrieve your intranet IP. Please check your network.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                string url = $"http://+:{port}/";

                _listener = new HttpListener();
                _listener.Prefixes.Add(url);
                _listener.Start();
                Console.WriteLine($"WebSocket server listening on {url}");

                _cts = new CancellationTokenSource();
                CancellationToken token = _cts.Token;

                // Start the listener loop in a separate task for async handling - not awaiting
                try
                {
                    _ = Task.Run(() => ListenForConnectionsAsync(_listener, token), token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to start listener loop: " + ex.Message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error starting WebSocket server: " + ex.Message);
            }
        }

        private async Task ListenForConnectionsAsync(HttpListener listener, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Use await to prevent blocking
                    var context = await listener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {
                        // Handle each connection in a separate task
                        #pragma warning disable CS4014
                        var wsTask = Task.Run(() => HandleWebSocketRequest(context, token), token);
                        #pragma warning restore CS4014

                        lock (_activeConnections)
                        {
                            _activeConnections.Add(wsTask);
                        }
                        
                        #pragma warning disable CS4014
                        wsTask.ContinueWith(t =>
                        {
                            lock (_activeConnections)
                            {
                                _activeConnections.Remove(wsTask);
                            }
                        }, TaskScheduler.Default);
                        #pragma warning restore CS4014

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error accepting connection: " + ex.Message);
                }
            }
        }

        public async Task StopAsync()
        {
            if (_cts == null || _listener == null)
                return;

            Console.WriteLine("Shutting down WebSocket server...");

            _cts.Cancel();

            try
            {
                _listener.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error stopping listener: " + ex.Message);
            }

            Task[] tasksCopy;
            lock (_activeConnections)
            {
                tasksCopy = _activeConnections.ToArray();
            }

            try
            {
                await Task.WhenAll(tasksCopy);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error waiting for connections to close: " + ex.Message);
            }

            Console.WriteLine("Server shutdown complete.");
        }


        private async Task HandleWebSocketRequest(
            HttpListenerContext context,
            CancellationToken token
        )
        {
            try
            {
                var wsContext = await context.AcceptWebSocketAsync(null);
                if (
                    wsContext?.WebSocket is WebSocket webSocket
                    && webSocket.State == WebSocketState.Open
                )
                {
                    Console.WriteLine("WebSocket connection established");

                    var buffer = new byte[1024];
                    while (webSocket.State == WebSocketState.Open && !token.IsCancellationRequested)
                    {
                        var receiveResult = await webSocket.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            token
                        );
                        if (receiveResult.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(
                                WebSocketCloseStatus.NormalClosure,
                                "Closing",
                                token
                            );
                            Console.WriteLine("WebSocket connection closed");
                        }
                        else
                        {
                            string message = System.Text.Encoding.UTF8.GetString(
                                buffer,
                                0,
                                receiveResult.Count
                            );
                            Console.WriteLine("Received: " + message);

                            MessageHandler.ProcessMessage(message);

                            await webSocket.SendAsync(
                                new ArraySegment<byte>(
                                    System.Text.Encoding.UTF8.GetBytes("Echo: " + message)
                                ),
                                WebSocketMessageType.Text,
                                true,
                                token
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling WebSocket request: " + ex.Message);
            }
        }

        public static Task<string?> GetIpAddress()
        {
            foreach (var ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != System.Net.NetworkInformation.OperationalStatus.Up)
                    continue;

                var ipProps = ni.GetIPProperties();
                foreach (var addr in ipProps.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork && 
                        !IPAddress.IsLoopback(addr.Address))
                    {
                        return Task.FromResult<string?>(addr.Address.ToString());
                    }
                }
            }
            return Task.FromResult<string?>(null);
        }

    }
}
