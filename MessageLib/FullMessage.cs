using System;

namespace MessageLib
{
    public class FullMessage : IMessage
    {
        private const char Delimiter = '|';
        public Commands Command { get; private set; }
        public IMessage Message { get; private set; }


        public IMessage Set(Commands command, IMessage message)
        {
            Command = command;
            Message = message;
            return this;
        }

        public IMessage Deserialize(string msg)
        {
            var items = msg.Split(Delimiter);
            if (items.Length != 2)
                return null;
            if (!Enum.TryParse(items[0], out Commands command))
                return null;

            switch (Command)
            {
                case Commands.Id:
                    Message = new IdMsg().Deserialize(msg);
                    break;
                case Commands.Move:
                    Message = new IdMsg().Deserialize(msg);
                    break;
                case Commands.Players:
                    Message = new IdMsg().Deserialize(msg);
                    break;
                case Commands.Start:
                    return null;
                case Commands.Win:
                    Message = new IdMsg().Deserialize(msg);
                    break;
                case Commands.Disqualified:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return Message == null ? null : this;
        }

        public string Serialize()
        {
            return Command + "|" + Message.Serialize();
        }
    }
}