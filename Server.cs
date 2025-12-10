using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class TCPServer
    {
        public static void StartServer()
        {
            Socket serverSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8080);
            try
            {
                serverSocket.Bind(endPoint);

                serverSocket.Listen(10);

                Console.WriteLine("Сервер запущено на порту 8080");

                while (true)
                {
                    Socket clientSocket = serverSocket.Accept();
                    Console.WriteLine("Клієнт підключився!");

                    byte[] buffer = new byte[1024];
                    int bytesReceived = clientSocket.Receive(buffer);
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    string currentTime = DateTime.Now.ToString("HH:mm");
                    Console.WriteLine($"О {currentTime} від {((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString()} отримано рядок: {data}");

                    string response = $"Привіт, клієнт!";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    clientSocket.Send(responseBytes);

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            finally
            {
                serverSocket.Close();
            }
        }
    }
}
