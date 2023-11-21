namespace MessageLib
{
    public struct IdMsg : IMessage
    {
        private string _id;

        public void Set(string id)
        {
            _id = id;
        }

        public bool Deserialize(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return false;
            _id = msg;
            return true;
        }

        public string Serialize()
        {
            return _id;
        }
    }
}