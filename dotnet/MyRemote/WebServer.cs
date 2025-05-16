using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MyRemote
{
    public interface IWebServer
    {
        Task StartAsync(int port);
        Task StopAsync();
    }

    public class WebServer : IWebServer
    {
        private readonly string httpDocRoot;
        private readonly byte[] buffer;
        private HttpListener? _listener;

        public WebServer(string _httpDocRoot)
        {
            httpDocRoot = _httpDocRoot;
            var filePath = Path.Combine(httpDocRoot, "index.html");
            var fileContents = File.ReadAllText(filePath);
            buffer = System.Text.Encoding.UTF8.GetBytes(fileContents);
        }

        public async Task StartAsync(int port)
        {
            Console.WriteLine($"Webserver start on {port}");
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://+:{port}/");

            try
            {
                _listener.Start();
                Console.WriteLine($"Webserver listening on {port}");
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine(
                    "Failed to start listener. Check if you have the necessary permissions."
                );
                Console.WriteLine(ex.Message);
                return;
            }

            try
            {
                while (_listener.IsListening)
                {
                    var context = await _listener.GetContextAsync();
                    var response = context.Response;

                    try
                    {
                        response.ContentLength64 = buffer.Length;
                        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error sending response: " + ex.Message);
                    }
                    finally
                    {
                        response.Close();
                    }
                }
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine("Listener stopped unexpectedly: " + ex.Message);
            }
        }

        public Task StopAsync()
        {
            if (_listener != null && _listener.IsListening)
            {
                Console.WriteLine("Stopping web server...");
                _listener.Stop();
                _listener.Close();
                _listener = null;
                Console.WriteLine("Web server stopped.");
            }

            return Task.CompletedTask;
        }
    }
}
