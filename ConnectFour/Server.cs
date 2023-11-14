using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConnectFour
{
    internal class Server
    {
        private readonly TcpListener _listener;

        public Server()
        {
            _listener = new TcpListener(IPAddress.Any, 5555);
            var listenerThread = new Thread(new ThreadStart(ListenForClients));
            listenerThread.Start();
        }

        private void ListenForClients()
        {
            _listener.Start();

            while (true)
            {
                // blocks until a client has connected to the server
                var client = _listener.AcceptTcpClient();

                // create a thread to handle communication with the connected client
                var clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object clientObj)
        {
            var tcpClient = (TcpClient)clientObj;
            var clientStream = tcpClient.GetStream();

            var message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                    break;

                var clientMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
                Console.WriteLine("Received: " + clientMessage);

                var response = Encoding.ASCII.GetBytes("Server: " + clientMessage);
                clientStream.Write(response, 0, response.Length);
                clientStream.Flush();
            }

            tcpClient.Close();
        }
    }
}