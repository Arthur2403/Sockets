using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncServer
{
    public class AsyncTcpServer
    {
        public static async Task StartServerAsync()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8080);
            server.Start();
            Console.WriteLine("Async сервер запущено...");

            try
            {
                while (true)
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    Console.WriteLine("Клієнт підключився!");

                    _ = Task.Run(async () => await HandleClientAsync(client));
                }
            }
            finally
            {
                server.Stop();
            }
        }
        private static async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];

                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string currentTime = DateTime.Now.ToString("HH:mm");
                Console.WriteLine($"О {currentTime} від {((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString} отримано рядок: {data}");

                string response = $"Привіт, клієнт!";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка обробки клієнта: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}