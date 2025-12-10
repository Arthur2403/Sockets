using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncClient
{
    public class AsyncTcpClient
    {
        public static async Task ConnectAsync()
        {
            try
            {
                TcpClient client = new TcpClient();
                await client.ConnectAsync("127.0.0.1", 8080);
                Console.WriteLine("Підключено!");
                NetworkStream stream = client.GetStream();

                string message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                Console.WriteLine($"Відправлено: {message}");

                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string currentTime = DateTime.Now.ToString("HH:mm");
                Console.WriteLine($"О {currentTime} від {((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()} отримано рядок: {response}");

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}