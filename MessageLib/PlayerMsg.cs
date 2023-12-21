using System.Linq;

namespace MessageLib
{
    public class PlayerMsg : IMessage
    {
        private const char Delimiter = ',';
        public string Player { get; private set; }
        
        
        public PlayerMsg Set(string player)
        {
            Player = player;
            return this;
        }
        
        public override string ToString()
        {
            return Player;
        }
        public static PlayerMsg ToObject(int headerLength,string msg)
        {
            if (msg.Length <= headerLength)
                return null;
            
            msg = msg.Substring(headerLength - 1);
            
            return string.IsNullOrWhiteSpace(msg) ? null : new PlayerMsg().Set(msg);
        }
    }
}