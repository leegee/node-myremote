using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyRemote
{
    public interface IWebServer
    {
        Task StartAsync(int port);
    }

    public class WebServer : IWebServer
    {
        private readonly string httpDocRoot;
        private readonly byte[] buffer;

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
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://+:{port}/");

            try
            {
                listener.Start();
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

            Console.WriteLine($"Webserver listening on {port}");

            try
            {
                while (true)
                {
                    var context = await listener.GetContextAsync();
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
    }
}
