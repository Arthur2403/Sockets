using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class TCPClient
    {
        public static void ConnectToServer()
        {
            Socket clientSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            try
            {
                IPEndPoint serverEndPoint = new IPEndPoint(
                    IPAddress.Parse("127.0.0.1"),
                    8080
                );

                clientSocket.Connect(serverEndPoint);
                Console.WriteLine("Підключено до сервера! Ведіть ваш запрос(time or date)");

                string message = Console.ReadLine();
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                clientSocket.Send(messageBytes);

                byte[] buffer = new byte[1024];
                int bytesReceived = clientSocket.Receive(buffer);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                Console.WriteLine($"Отримано рядок: {response}");

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
