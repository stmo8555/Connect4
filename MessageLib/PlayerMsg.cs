using System.Collections.Generic;
using System.Linq;

namespace MessageLib
{
    public class PlayerMsg : IMessage
    {
        private const char Delimiter = ',';
        public List<string> Players { get; private set; }
        
        
        public PlayerMsg Set(List<string> players)
        {
            Players = players;
            return this;
        }
        public string MessageType()
        {
            return nameof(PlayerMsg);
        }
        
        public override string ToString()
        {
            var str = "";
            for (var i = 0; i < Players.Count; i++)
            {
                str += Players[i];
                if (i != Players.Count - 1)
                    str += Delimiter;
            }
            
            return str;
        }
        public static PlayerMsg ToObject(string msg)
        {
            var players = msg.Split(Delimiter).ToList();
            
            
            return new PlayerMsg().Set(players);
        }
        
        // For UnitTest
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (PlayerMsg)obj;
            
            return Players.SequenceEqual(other.Players);
        }
    }
}