using System;
using ServerClientLib;
using ServerClientLib.Utils;

namespace ConnectFourServer
{
    public class CommunicationManager
    {
        private bool _playersConnected = false;
        private readonly Server _server;
        private Connection GuiConnection { get; set; }
        private Connection P1Connection { get; set; }
        private Connection P2Connection { get; set; }

        public delegate void MoveData(string player, int row, int column);

        public event MoveData NewMove;

        public CommunicationManager(Server server)
        {
            _server = server;
            server.MaxConnectionReached += ServerOnMaxConnectionReached;
            server.ReceivedMessage += OnReceived;
            server.NewConnection += AskForId;
        }

        private void AskForId(Connection connection)
        {
            _server.Send("id|123", connection);
        }

        public void SendToGui(string msg)
        {
            _server.Send(msg, GuiConnection);
        }

        public void SendToP1(string msg)
        {
            _server.Send(msg, P1Connection);
        }

        public void SendToP2(string msg)
        {
            _server.Send(msg, P2Connection);
        }

        private void OnReceived(Connection sender)
        {
            ParseData(sender, _server.GetMessage());
        }

        private void ServerOnMaxConnectionReached()
        {
            SendToGui("connected|");
        }

        private void ParseData(Connection connection, string msg)
        {
            // command|data
            var strings = msg.Split('|');
            if (strings.Length != 2)
                return;

            var command = strings[0].ToLower();
            var data = strings[1];

            switch (command)
            {
                case "players":
                    if (P1Connection == null || P2Connection == null)
                    {
                        Console.WriteLine("no name set for one of the players");
                        return;
                    }
                    SendToGui($"players|{P1Connection.Id},{P2Connection.Id}");
                    break;
                   
                case "id":

                    if (data == "GUI")
                    {
                        GuiConnection = connection;
                        GuiConnection.Id = data;
                        break;
                    }

                    if (P1Connection == null)
                    {
                        P1Connection = connection;
                        P1Connection.Id = data;
                        break;
                    }

                    if (P2Connection == null)
                    {
                        P2Connection = connection;
                        P2Connection.Id = data;
                    }
                    break;
                case "move":
                    // 1,2
                    var index = data.Split(',');
                    if (index.Length != 2)
                        return;

                    if (!int.TryParse(index[0], out var row))
                        return;
                    if (!int.TryParse(index[1], out var column))
                        return;

                    NewMove?.Invoke(connection.Id, row, column);
                    break;
            }
        }
    }
}