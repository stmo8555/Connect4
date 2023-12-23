using System.Linq;

namespace MessageLib
{
    public class PlayerMsg : IMessage
    {
        public string Player { get; private set; }
        
        
        public PlayerMsg Set(string player)
        {
            Player = player;
            return this;
        }
        public string MessageType()
        {
            return nameof(PlayerMsg);
        }
        
        public override string ToString()
        {
            return Player;
        }
        public static PlayerMsg ToObject(string msg)
        {
            return string.IsNullOrWhiteSpace(msg) ? null : new PlayerMsg().Set(msg);
        }
        
        // For UnitTest
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (PlayerMsg)obj;
            
            return Player == other.Player;
        }
    }
}