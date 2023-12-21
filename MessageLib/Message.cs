using System;
using System.CodeDom;
using System.Runtime.Serialization;

namespace MessageLib
{
    public class Message : IMessage
    {
        private readonly string _data;
        public Header Header { get; }

        public Message(IMessage data, string msgType)
        {
            _data = data.ToString();
            Header = new Header(msgType);
        }

        private Message(Header header, string data)
        {
            Header = header;
            _data = data;
        }

        public T Convert<T>() where T : class
        {
            switch (Header.MsgType)
            {
                case nameof(IdMsg):
                    return IdMsg.ToObject(Header.HeaderLength, _data) as T;
                case nameof(MoveMsg):
                    return MoveMsg.ToObject(Header.HeaderLength, _data) as T;
                case nameof(PlayerMsg):
                    return PlayerMsg.ToObject(Header.HeaderLength, _data) as T;
                case nameof(WinMsg):
                    return WinMsg.ToObject(Header.HeaderLength, _data) as T;
            }

            return null;
        }

        public static Message ToObject(string msg)
        {
            var header = Header.ToObject(msg);
            return header == null ? null : new Message(header, msg.Substring(1, header.HeaderLength - 1));
        }

        public override string ToString()
        {
            return Header + _data;
        }
    }

    public class Header
    {
        public string MsgType { get; private set; }
        public int HeaderLength { get; }

        internal Header(string msgType)
        {
            MsgType = msgType;
            HeaderLength = MsgType.Length;
        }

        public override string ToString()
        {
            return HeaderLength + MsgType;
        }

        public static Header ToObject(string msg)
        {
            if (!int.TryParse(msg[0].ToString(), out var headerLength))
                return null;
            
            return msg.Length >= headerLength ? new Header(msg.Substring(1,  headerLength)) : null;
        }
    }
    
}