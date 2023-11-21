using System.Configuration;

namespace MessageLib
{
    public struct MoveMsg : IMessage
    {
        private const char Delimiter = ',';
        private int _row;
        private int _column;

        public void Set(int row, int column)
        {
            _row = row;
            _column = column;
        }

        public bool Deserialize(string msg)
        {
            var items = msg.Split(Delimiter);
            if (items.Length != 2)
                return false;

            if (!int.TryParse(items[0], out var row))
                return false;
            _row = row;

            if (!int.TryParse(items[1], out var column))
                return false;
            _column = column;
            return true;
        }

        public string Serialize()
        {
            return _row.ToString() + Delimiter + _column;
        }
    }
}