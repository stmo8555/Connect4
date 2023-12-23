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
        

        public static IdMsg ToObject(string msg)
        {
            return string.IsNullOrWhiteSpace(msg) ? null : new IdMsg().Set(msg);
        }

        public override string ToString()
        {
            return Id;
        }

        public string MessageType()
        {
            return nameof(IdMsg);
        }
        
        // For UnitTest
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (IdMsg)obj;

            return Id == other.Id;
        }
    }
}