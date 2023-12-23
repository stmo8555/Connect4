using NUnit.Framework;
using MessageLib;
namespace UnitTest
{
        [TestFixture]
        public class MessageTest
        {
            private MoveMsg _moveMsg;
            private string _moveMsgStr = Row.ToString() + ',' + Column + ',' + Player;
            private MoveMsg _moveMsg2;
            private string _moveMsgStr2 = Row.ToString() + ',' + Column;
            private IdMsg _idMsg;
            private string _idMsgStr = Player;
            private PlayerMsg _playerMsg;
            private string _playerMsgStr = Player;
            private WinMsg _winMsg;
            private string _winMsgStr = Player;
            private const int Row = 1;
            private const int Column = 0;
            private const string Player = "Putte";
            
            [SetUp]
            public void SetUp()
            {
                _moveMsg = new MoveMsg().Set(Row, Column, Player);
                _moveMsg2 = new MoveMsg().Set(Row, Column);
                _idMsg = new IdMsg().Set(Player);
                _playerMsg = new PlayerMsg().Set(Player);
                _winMsg = new WinMsg().Set(Player);
            }
            [Test]
            public void MoveMsgToString()
            {
                Assert.AreEqual(_moveMsgStr, _moveMsg.ToString());
                Assert.AreEqual(_moveMsgStr2, _moveMsg2.ToString());
            }
            
            [Test]
            public void MoveMsgToObject()
            {
                Assert.AreEqual(_moveMsg, MoveMsg.ToObject(_moveMsgStr));
                Assert.AreEqual(_moveMsg2, MoveMsg.ToObject(_moveMsgStr2));
            }
            
            [Test]
            public void MoveMsgToObjectToString()
            {
                Assert.AreEqual(_moveMsgStr, MoveMsg.ToObject(_moveMsgStr).ToString());
                Assert.AreEqual(_moveMsgStr2, MoveMsg.ToObject(_moveMsgStr2).ToString());
            }
            
            [Test]
            public void IdMsgToString()
            {
                Assert.AreEqual(_idMsgStr, _idMsg.ToString());
            }
            
            [Test]
            public void IdMsgToObject()
            {
                Assert.AreEqual(_idMsg, IdMsg.ToObject(_idMsgStr));
            }
            
            [Test]
            public void IdMsgToObjectToString()
            {
                Assert.AreEqual(_idMsgStr, IdMsg.ToObject(_idMsgStr).ToString());
            }
            
            [Test]
            public void PlayerMsgToString()
            {
                Assert.AreEqual(_playerMsgStr, _playerMsg.ToString());
            }
            
            [Test]
            public void PlayerMsgToObject()
            {
                Assert.AreEqual(_playerMsg, PlayerMsg.ToObject(_playerMsgStr));
            }
            
            [Test]
            public void PlayerMsgToObjectToString()
            {
                Assert.AreEqual(_playerMsgStr, PlayerMsg.ToObject(_playerMsgStr).ToString());
            }
            
            [Test]
            public void WinMsgToString()
            {
                Assert.AreEqual(_winMsgStr, _winMsg.ToString());
            }
            
            [Test]
            public void WinMsgToObject()
            {
                Assert.AreEqual(_winMsg, WinMsg.ToObject(_winMsgStr));
            }
            
            [Test]
            public void WinMsgToObjectToString()
            {
                Assert.AreEqual(_winMsgStr, IdMsg.ToObject(_winMsgStr).ToString());
            }
            
            [Test]
            public void MessageToString()
            {
                var ans = ('x' + nameof(IdMsg)).Length + nameof(IdMsg) + _idMsgStr;
                var msg = new Message(_idMsg).ToString();
                Assert.AreEqual(ans, msg);
                ans = ("xx" + nameof(PlayerMsg)).Length + nameof(PlayerMsg) + _playerMsgStr;
                msg = new Message(_playerMsg).ToString();
                Assert.AreEqual(ans, msg);
                ans = ('x' + nameof(MoveMsg)).Length + nameof(MoveMsg) + _moveMsgStr;
                msg = new Message(_moveMsg).ToString();
                Assert.AreEqual(ans, msg);
                ans = ('x' + nameof(MoveMsg)).Length + nameof(MoveMsg) + _moveMsgStr2;
                msg = new Message(_moveMsg2).ToString();
                Assert.AreEqual(ans, msg);
                ans = ('x' + nameof(WinMsg)).Length + nameof(WinMsg) + _winMsgStr;
                msg = new Message(_winMsg).ToString();
                Assert.AreEqual(ans, msg);
            }
            
            [Test]
            public void MessageToObjectToString()
            {
                var ans = ('x' + nameof(IdMsg)).Length + nameof(IdMsg) + _idMsgStr;
                Assert.AreEqual(ans, Message.ToObject(ans).ToString());
                ans = ("xx" + nameof(PlayerMsg)).Length + nameof(PlayerMsg) + _idMsgStr;
                Assert.AreEqual(ans, Message.ToObject(ans).ToString());
                ans = ('x' + nameof(MoveMsg)).Length + nameof(MoveMsg) + _moveMsgStr;
                Assert.AreEqual(ans, Message.ToObject(ans).ToString());
                ans = ('x' + nameof(MoveMsg)).Length + nameof(MoveMsg) + _moveMsgStr2;
                Assert.AreEqual(ans, Message.ToObject(ans).ToString());
                ans = ('x' + nameof(WinMsg)).Length + nameof(WinMsg) + _winMsgStr;
                Assert.AreEqual(ans, Message.ToObject(ans).ToString());
            }
            
            [Test]
            public void MessageConvert()
            {
                var ans = ('x' + nameof(IdMsg)).Length + nameof(IdMsg) + _idMsgStr;
                Assert.AreEqual(_idMsg, Message.ToObject(ans).Convert<IdMsg>());
                ans = ("xx" + nameof(PlayerMsg)).Length + nameof(PlayerMsg) + _playerMsgStr;
                Assert.AreEqual(_playerMsg, Message.ToObject(ans).Convert<PlayerMsg>());
                ans = ('x' + nameof(MoveMsg)).Length + nameof(MoveMsg) + _moveMsgStr;
                Assert.AreEqual(_moveMsg, Message.ToObject(ans).Convert<MoveMsg>());
                ans = ('x' + nameof(MoveMsg)).Length + nameof(MoveMsg) + _moveMsg2;
                Assert.AreEqual(_moveMsg2, Message.ToObject(ans).Convert<MoveMsg>());
                ans = ('x' + nameof(WinMsg)).Length + nameof(WinMsg) + _winMsgStr;
                Assert.AreEqual(_winMsg, Message.ToObject(ans).Convert<WinMsg>());
            }
            
        }
}