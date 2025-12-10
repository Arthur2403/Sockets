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

                Socket clientSocket = serverSocket.Accept();
                Console.WriteLine("Клієнт підключився!");

                byte[] buffer = new byte[1024];
                int bytesReceived = clientSocket.Receive(buffer);
                string data = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                Console.WriteLine($"Отримано рядок: {data}");

                if (data == "date")
                {
                    var response = DateTime.Today.ToString("dd.MM.yyyy");
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    clientSocket.Send(responseBytes);
                }
                else if (data == "time")
                {
                    var response = DateTime.Now.ToString("HH:mm:ss");
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    clientSocket.Send(responseBytes);
                }
                else
                {
                    var response = "Wrong request";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    clientSocket.Send(responseBytes);
                }

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                
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