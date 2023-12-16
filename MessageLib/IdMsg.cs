using System.Runtime.CompilerServices;

namespace MessageLib
{
    public class IdMsg : BaseMessage, IMessage
    {
        public string Id { get; private set; }

        public IMessage Set(string id)
        {
            SetCommand(Commands.Id);
            Id = id;
            return this;
        }
        
        public new IMessage Deserialize(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return null;
            
            Id = msg;
            return this;
        }

        public new string Serialize()
        {
            return Id;
        }
    }
}