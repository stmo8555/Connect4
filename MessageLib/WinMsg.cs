namespace MessageLib
{
    public struct WinMsg : IMessage
    {
        private string _player;

        public void Set(string player)
        {
            _player = player;
        }

        public bool Deserialize(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return false;
            _player = msg;
            return true;
        }

        public string Serialize()
        {
            return _player;
        }
    }
}