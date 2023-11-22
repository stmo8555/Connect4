using System.Configuration;

namespace MessageLib
{
    public interface IMessage
    {
        IMessage Deserialize(string msg);
        string Serialize();
    }
}