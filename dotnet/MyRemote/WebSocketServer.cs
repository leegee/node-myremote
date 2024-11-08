using System;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyRemote
{
    public class WebSocketServer
    {
        public async Task StartAsync(int port)
        {
            try
            {
                string? publicIp = await GetPublicIPAsync();
                if (publicIp == null)
                {
                    MessageBox.Show(
                        "Unable to retrieve public IP. Please check your network.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                string url = $"http://+:{port}/";

                HttpListener listener = new HttpListener();
                listener.Prefixes.Add(url);
                listener.Start();
                Console.WriteLine($"WebSocket server listening on {url}");

                var cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                // Start the listener loop in a separate task for async handling
                Task.Run(async () => await ListenForConnectionsAsync(listener, token), token);
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
                        Task.Run(() => HandleWebSocketRequest(context, token));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error accepting connection: " + ex.Message);
                }
            }
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

        private static async Task<string?> GetPublicIPAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://icanhazip.com");
                    response.EnsureSuccessStatusCode();
                    string ip = (await response.Content.ReadAsStringAsync()).Trim();
                    return ip;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting public IP: " + ex.Message);
                return null;
            }
        }
    }
}
