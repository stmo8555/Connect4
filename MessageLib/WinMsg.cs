namespace MessageLib
{
    public class WinMsg : IMessage
    {
        private string _player;

        public WinMsg Set(string player)
        {
            _player = player;
            return this;
        }
        
        public override string ToString()
        {
            return _player;
        }
        
        public static WinMsg ToObject(int headerLength,string msg)
        {
            if (msg.Length <= headerLength)
                return null;
            
            msg = msg.Substring(headerLength - 1);
            return string.IsNullOrWhiteSpace(msg) ? null : new WinMsg().Set(msg);
        }
    }
}