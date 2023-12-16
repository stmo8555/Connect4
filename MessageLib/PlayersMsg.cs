namespace MessageLib
{
    public class PlayersMsg : BaseMessage, IMessage
    {
        private const char Delimiter = ',';
        public string P1 { get; private set; }
        public string P2 { get; private set; }

        public IMessage Set(string p1, string p2)
        {
            SetCommand(Commands.Players);
            P1 = p1;
            P2 = p2;
            return this;
        }

        public new IMessage Deserialize(string msg)
        {
            var items = msg.Split(Delimiter);
            if (items.Length != 2)
                return null;

            P1 = items[0];
            P2 = items[1];
            return this;
        }

        public new string Serialize()
        {
            return P1 + Delimiter + P2;
        }
    }
}