using System;

namespace MessageLib
{
    public class BaseMessage
    {
        public Commands Command { get; private set; }
        internal readonly int HeaderLength;

        public BaseMessage()
        {
        }

        internal BaseMessage(Commands command)
        {
            Command = command;
            HeaderLength = Command.ToString().Length + 1;
        }
        
        public virtual bool Deserialize(string msg)
        {
            if (!int.TryParse(msg[0].ToString(), out var val))
                return false;

            if (!Enum.TryParse(msg.Substring(1, HeaderLength - 1), out Commands command))
                return false;
            Command = command;

            return true;
        }

        public virtual string Serialize()
        {
            return HeaderLength + Command.ToString();
        }
    }
}