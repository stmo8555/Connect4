using System.Configuration;
using System.Data;

namespace MessageLib
{
    public class MoveMsg : IMessage
    {
        private const char Delimiter = ',';
        public int Row { get; private set; }
        public int Column { get; private set; }
        public string Player { get; private set; }

        public MoveMsg Set(int row, int column, string player = null)
        {
            Row = row;
            Column = column;
            Player = player;
            return this;
        }

        public override string ToString()
        {
            var player = "";
            if (Player != null)
                player = Delimiter + Player;
            return Row + Delimiter + Column + player;
        }
        
        public static MoveMsg ToObject(int headerLength,string msg)
        {
            if (msg.Length <= headerLength)
                return null;

            msg = msg.Substring(headerLength - 1);
            var items = msg.Split(Delimiter);
            if (items.Length < 2)
                return null;

            if (!int.TryParse(items[0], out var row))
                return null;

            if (!int.TryParse(items[1], out var column))
                return null;
            
            return new MoveMsg().Set(row,column, items.Length == 3 ? items[2] : null );
        }
        
    }
}