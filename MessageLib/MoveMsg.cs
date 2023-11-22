using System.Configuration;

namespace MessageLib
{
    public class MoveMsg : IMessage
    {
        private const char Delimiter = ',';
        public int Row { get; private set; }
        public int Column { get; private set; }
        public string Player { get; private set; }

        public IMessage Set(int row, int column, string player = null)
        {
            Row = row;
            Column = column;
            Player = player;
            return this;
        }

        public IMessage Deserialize(string msg)
        {
            var items = msg.Split(Delimiter);
            if (items.Length < 2)
                return null;

            if (!int.TryParse(items[0], out var row))
                return null;
            Row = row;

            if (!int.TryParse(items[1], out var column))
                return null;
            Column = column;
            if (items.Length == 3)
                Player = items[2];
            return this;
        }

        public string Serialize()
        {
            var player = "";
            if (Player != null)
                player = Delimiter + Player;
            return Row.ToString() + Delimiter + Column + player;
        }
    }
}