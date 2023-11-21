namespace MessageLib
{
    public interface IMessage
    {
        bool Deserialize(string msg);
        string Serialize();
    }
}