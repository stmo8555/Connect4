using System.Configuration;
using System.Data;

namespace MessageLib
{
    public class MoveMsg : IMessage
    {
        private const char Delimiter = ',';
        public int Row { get;  private set; }
        public int Column { get; private set; }
        public string Player { get; private set; }

        public MoveMsg Set(int row, int column, string player = null)
        {
            Row = row;
            Column = column;
            Player = player;
            return this;
        }
        public string MessageType()
        {
            return nameof(MoveMsg);
        }

        public override string ToString()
        {
            var player = "";
            if (Player != null)
                player = Delimiter + Player;
            return Row.ToString() + Delimiter + Column + player;
        }
        
        public static MoveMsg ToObject(string msg)
        {
            var items = msg.Split(Delimiter);
            if (items.Length < 2)
                return null;

            if (!int.TryParse(items[0], out var row))
                return null;

            if (!int.TryParse(items[1], out var column))
                return null;
            
            return new MoveMsg().Set(row,column, items.Length == 3 ? items[2] : null );
        }
        
        // For UnitTest
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (MoveMsg)obj;
            
            return Player == other.Player && Row == other.Row && Column == other.Column;
        }
    }
}