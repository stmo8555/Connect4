using System;
using System.Text;
using ServerClientLib;
using ServerClientLib.Utils;

namespace ConnectFourServer
{
    public class CommunicationManager
    {
        private bool _playersConnected = false;
        private readonly Server _server;

        public delegate void MoveData(string player, int row, int column);

        public event MoveData NewMove;

        public CommunicationManager(Server server)
        {
            _server = server;
            server.MaxConnectionReached += ServerOnMaxConnectionReached;
            server.ReceivedMessage += OnReceived;
        }

        public void SendToGui(string msg)
        {
            _server.Send(msg, _server.GetConnection[2]);
        }

        private void OnReceived(Connection sender)
        {
            ParseData(sender.Id, _server.GetMessage());
            _server.Send("sender", sender);
        }

        private void ServerOnMaxConnectionReached()
        {
            _playersConnected = true;
        }

        private void ParseData(string player, string msg)
        {
            // 1,2
            var index = msg.Split(',');
            if (index.Length != 2)
                return;

            if (!int.TryParse(index[0], out var row))
                return;
            if (!int.TryParse(index[1], out var column))
                return;

            NewMove?.Invoke(player, row, column);
        }
    }
}