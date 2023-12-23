namespace MessageLib
{
    public class WinMsg : IMessage
    {
        public string Player { get; private set; }

        public WinMsg Set(string player)
        {
            Player = player;
            return this;
        }
        
        public override string ToString()
        {
            return Player;
        }
        
        public string MessageType()
        {
            return nameof(WinMsg);
        }
        
        public static WinMsg ToObject(string msg)
        {
            return string.IsNullOrWhiteSpace(msg) ? null : new WinMsg().Set(msg);
        }

        // For UnitTest
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (WinMsg)obj;
            
            return Player == other.Player;
        }
    }
}