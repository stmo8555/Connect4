using System;
using System.Collections.Generic;
using MessageLib;
using ServerClientLib;
using ServerClientLib.Utils;

namespace ConnectFourServer
{
    public class CommunicationManager
    {
        private bool _playersConnected = false;
        private readonly Server _server;
        private string _p1;
        private string _p2;
        private readonly Dictionary<string, Connection> _connections = new Dictionary<string, Connection>();
        private Connection GuiConnection { get; set; }
        
        public delegate void MoveData(string player, int row, int column);

        public event MoveData NewMove;

        public CommunicationManager(Server server)
        {
            _server = server;
            server.MaxConnectionReached += ServerOnMaxConnectionReached;
            server.ReceivedMessage += OnReceivedData;
            server.NewConnection += AskForId;
        }

        private void AskForId(Connection connection)
        {
            _server.Send(new Message(new IdMsg()).ToString(), connection);
        }

        public void SendToGui(IMessage msg)
        {
            _server.Send(msg.ToString(), GuiConnection);
        }

        public void SendToP1(IMessage msg)
        {
            _server.Send(msg.ToString(), _connections[_p1]);
        }

        public void SendToP2(IMessage msg)
        {
            _server.Send(msg.ToString(), _connections[_p1]);
        }

        private void OnReceivedData(Connection sender)
        {
            ParseData(sender, _server.GetMessage());
        }

        private void ServerOnMaxConnectionReached()
        {
            //
        }

        private void ParseData(Connection connection, string msg)
        {
            var obj = Message.ToObject(msg);

            if (obj == null)
            {
                Console.WriteLine("Failed to parse message");
                return;
            }
            
            switch (obj.MessageType())
            {
                case nameof(PlayerMsg):
                    HandlePlayerMsg(obj);
                    break;
                case nameof(IdMsg):
                    HandleIdMsg(connection, obj);
                    break;
                case nameof(MoveMsg):
                    HandleMoveMsg(connection, obj);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandlePlayerMsg(Message obj)
        {
            var player = obj.Convert<PlayerMsg>();
            
            SendToGui(new Message(new PlayerMsg().Set(player.Player)));
        }

        private void HandleIdMsg(Connection connection, Message obj)
        {
            var idMsg = obj.Convert<IdMsg>();
            
            var id = idMsg.Id;
            if (id == "GUI" && GuiConnection == null)
            {
                GuiConnection = connection;
                GuiConnection.Id = id;
                return;
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                connection.Close();
                return;
            }
            
            _connections.Add(id, connection);
        }

        private void HandleMoveMsg(Connection connection, Message obj)
        {
            var moveMsg = obj.Convert<MoveMsg>();
            NewMove?.Invoke(connection.Id, moveMsg.Row, moveMsg.Column);
        }
    }
}