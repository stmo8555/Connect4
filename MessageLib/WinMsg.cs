namespace MessageLib
{
    public class WinMsg : BaseMessage, IMessage
    {
        private string _player;

        public IMessage Set(string player)
        {
            SetCommand(Commands.Win);
            _player = player;
            return this;
        }

        public new IMessage Deserialize(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return null;
            _player = msg;
            return this;
        }

        public new string Serialize()
        {
            return _player;
        }
    }
}