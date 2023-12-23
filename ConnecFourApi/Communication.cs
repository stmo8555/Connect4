using System;
using System.Collections.Generic;
using MessageLib;
using ServerClientLib;

namespace ConnectFourApi
{
    public class Communication
    {
        private string _identifier;
        private readonly Client _client = new Client();

        public Communication(string identifier)
        {
            _identifier = identifier;
            _client.ReceivedMessage += ParseData;
        }

        public Action<List<string>> PlayersReceived;
        public Action<int, int, string> MoveReceived;
        public Action<string> WinReceived;
        
        private void ParseData()
        {
            var obj = Message.ToObject(_client.GetMessage());

            if (obj == null)
            {
                Console.WriteLine(@"Failed to parse message");
                return;
            }


            switch (obj.MessageType())
            {
                case nameof(PlayerMsg):
                    HandlePlayerMsg(obj);
                    break;
                case nameof(IdMsg):
                    HandleIdMsg();
                    break;
                case nameof(MoveMsg):
                    HandleMoveMsg(obj);
                    break;
                case nameof(WinMsg):
                    HandleWinMsg(obj);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleWinMsg(Message obj)
        {
            var msg = obj.Convert<WinMsg>();
            WinReceived?.Invoke(msg.Player);
        }

        private void HandleMoveMsg(Message obj)
        {
            var msg = obj.Convert<MoveMsg>();
            MoveReceived?.Invoke(msg.Row, msg.Column, msg.Player);
        }

        private void HandlePlayerMsg(Message obj)
        {
            var msg = obj.Convert<PlayerMsg>();
            PlayersReceived?.Invoke(msg.Players);
        }

        private void HandleIdMsg()
        {
            var msg = new Message(new IdMsg().Set(_identifier));
            _client.Send(msg.ToString());
        }

        public void PlayerRequest()
        {
            var msg = new Message(new PlayerMsg().Set(new List<string>()));
            _client.Send(msg.ToString());
        }

        public void StartRequest(string player1, string player2)
        {
            throw new NotImplementedException();
        }
    }
}