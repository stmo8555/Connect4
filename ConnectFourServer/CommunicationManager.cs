using System;
using MessageLib;
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
            server.ReceivedMessage += OnReceivedData;
            server.NewConnection += AskForId;
        }

        private void AskForId(Connection connection)
        {
            _server.Send("id|123", connection);
        }

        public void SendToGui(IMessage msg)
        {
            _server.Send(msg.Serialize(), GuiConnection);
        }

        public void SendToP1(IMessage msg)
        {
            _server.Send(msg.Serialize(), P1Connection);
        }

        public void SendToP2(IMessage msg)
        {
            _server.Send(msg.Serialize(), P2Connection);
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
            if (!(new FullMessage().Deserialize(msg) is FullMessage fullMessage))
                return;

            switch (fullMessage.Command)
            {
                case Commands.Players:
                    
                    if (P1Connection == null || P2Connection == null)
                    {
                        Console.WriteLine("no name set for one of the players");
                        return;
                    }
                    SendToGui(new FullMessage().Set(Commands.Players, new PlayersMsg().Set(P1Connection.Id, P2Connection.Id)));
                    break;

                case Commands.Id:
                    var id = (fullMessage.Message as IdMsg)?.Id;
                    if (id == null) 
                        break;
                    
                    if (id == "GUI" && GuiConnection == null)
                    {
                        GuiConnection = connection;
                        GuiConnection.Id = id;
                        break;
                    }

                    if (P1Connection == null)
                    {
                        P1Connection = connection;
                        P1Connection.Id = id;
                        break;
                    }

                    if (P2Connection == null)
                    {
                        P2Connection = connection;
                        P2Connection.Id = id;
                    }

                    break;
                case Commands.Move:
                    if (fullMessage.Message is MoveMsg move) 
                        NewMove?.Invoke(connection.Id, move.Row, move.Column);
                    break;
                case Commands.Start:
                    break;
                case Commands.Win:
                    break;
                case Commands.Disqualified:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}