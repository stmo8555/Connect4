using System;
using System.CodeDom;
using System.Linq;
using System.Runtime.Serialization;

namespace MessageLib
{
    public class Message : IMessage
    {
        private readonly string _data;
        private readonly Header _header;

        public Message(IMessage data)
        {
            _data = data.ToString();
            _header = new Header(data.MessageType());
        }

        private Message(Header header, string data)
        {
            _header = header;
            _data = data;
        }
        public string MessageType()
        {
            return _header.MsgType;
        }

        public T Convert<T>() where T : class
        {
            switch (_header.MsgType)
            {
                case nameof(IdMsg):
                    return IdMsg.ToObject(_data) as T;
                case nameof(MoveMsg):
                    return MoveMsg.ToObject(_data) as T;
                case nameof(PlayerMsg):
                    return PlayerMsg.ToObject(_data) as T;
                case nameof(WinMsg):
                    return WinMsg.ToObject(_data) as T;
            }

            return null;
        }

        public static Message ToObject(string msg)
        {
            var header = Header.ToObject(msg);
            return header == null ? null : new Message(header, msg.Substring(header.HeaderLength));
        }

        public override string ToString()
        {
            return _header + _data;
        }
    }

    public class Header
    {
        public string MsgType { get; private set; }
        public int HeaderLength { get; }

        internal Header(string msgType)
        {
            MsgType = msgType;
            HeaderLength = $"{MsgType.Length + 1}{MsgType}".Length;
        }

        public override string ToString()
        {
            return HeaderLength + MsgType;
        }

        public static Header ToObject(string msg)
        {
            var count = 0;

            foreach (var c in msg)
            {
                if (char.IsDigit(c))
                    count++;
                else
                    break;
            }

            if (!int.TryParse(msg.Substring(0, count), out var headerLength))
                return null;
            
            return msg.Length >= headerLength ? new Header(msg.Substring(count,  headerLength - count)) : null;
        }
    }
    
}