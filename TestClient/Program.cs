using System;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using MessageLib;
using ServerClientLib;

namespace TestClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var client = new Client();
            client.ReceivedMessage += () => Console.WriteLine(client.GetMessage());


            while (true)
            {
                var command = "";
                var data = "";
                Console.WriteLine("Command:");
                command = Console.ReadLine();
                Console.WriteLine("Data;");
                data = Console.ReadLine();

                switch (command)
                {
                    case "Id":
                        client.Send(new Message(new IdMsg().Set(data)).ToString());
                        break;
                    case "Move":
                        if (data != null)
                        {
                            var split = data.Split(',');
                            client.Send(new Message(new MoveMsg().Set(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]))).ToString());
                        }
                        break;
                }
            }
        }
    }
}