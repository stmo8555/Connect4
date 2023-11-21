namespace MessageLib
{
    public struct PlayersMsg : IMessage
    {
        private const char Delimiter = ',';
        private string _p1;
        private string _p2;

        public void Set(string p1, string p2)
        {
            _p1 = p1;
            _p2 = p2;
        }

        public bool Deserialize(string msg)
        {
            var items = msg.Split(Delimiter);
            if (items.Length != 2)
                return false;

            _p1 = items[0];
            _p2 = items[1];
            return true;
        }

        public string Serialize()
        {
            return _p1 + Delimiter + _p2;
        }
    }
}