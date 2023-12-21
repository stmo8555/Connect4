using System.Runtime.CompilerServices;

namespace MessageLib
{
    public class IdMsg : IMessage
    {
        public string Id { get; private set; }
        
        public IdMsg Set(string id)
        {
            Id = id;
            return this;
        }
        

        public static IdMsg ToObject(int headerLength, string msg)
        {
            if (msg.Length <= headerLength)
                return null;

            msg = msg.Substring(headerLength - 1);
            return string.IsNullOrWhiteSpace(msg) ? null : new IdMsg().Set(msg);
        }

        public override string ToString()
        {
            return Id;
        }
    }
}