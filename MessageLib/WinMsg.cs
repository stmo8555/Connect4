namespace MessageLib
{
    public class WinMsg : IMessage
    {
        private string _player;

        public IMessage Set(string player)
        {
            _player = player;
            return this;
        }

        public IMessage Deserialize(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return null;
            _player = msg;
            return this;
        }

        public string Serialize()
        {
            return _player;
        }
    }
}