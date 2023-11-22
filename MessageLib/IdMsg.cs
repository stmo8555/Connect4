namespace MessageLib
{
    public class IdMsg : IMessage
    {
        public string Id { get; private set; }

        public IMessage Set(string id)
        {
            Id = id;
            return this;
        }
        

        public IMessage Deserialize(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return null;
            Id = msg;
            return this;
        }

        public string Serialize()
        {
            return Id;
        }
    }
}