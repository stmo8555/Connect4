using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MessageLib;
using ServerClientLib;
using Message = MessageLib.Message;

namespace ConnectFour
{
    public partial class Form1 : Form
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private readonly Client _client = new Client();
        private readonly string[,] _board = new string[Rows, Columns];
        private string _p1;
        private string _p2;

        private readonly PictureBox[,] _boxes = new PictureBox[6, 7];

        public Form1()
        {
            InitializeComponent();
            InitBoxes();
            _client.ReceivedMessage += ParseData;
        }

        private void PaintBox(object sender, PaintEventArgs e)
        {
            var item = sender as PictureBox;
            var brush = Brushes.Gray;
            if (item?.Tag is Tuple<int, int> index2d)
            {
                var playerData = _board[index2d.Item1, index2d.Item2];
                if (playerData == _p1 & _p1 != null)
                    brush = Brushes.Blue;
                else if (playerData == _p2 & _p2 != null)
                    brush = Brushes.Red;
            }

            if (item != null)
                e.Graphics.FillEllipse(brush, 0, 0, item.Width, item.Height);
        }


        private void InitBoxes()
        {
            var counter = 1;
            for (var i = 0; i < _boxes.GetLength(0); i++)
            {
                for (var j = 0; j < _boxes.GetLength(1); j++)
                {
                    var pictureBoxName = "pictureBox" + counter;
                    if (Controls.Find(pictureBoxName, true).FirstOrDefault() is PictureBox pictureBox)
                    {
                        pictureBox.Tag = new Tuple<int, int>(i, j);
                        _boxes[i, j] = pictureBox;
                        pictureBox.Paint += PaintBox;
                        pictureBox.Resize += TriggerNewRender;
                    }

                    counter++;
                }
            }
        }

        private void Send(IMessage msg)
        {
            _client.Send(msg.ToString());
        }

        private void TriggerNewRender(object sender, EventArgs e)
        {
            var p = sender as PictureBox;
            p?.Invalidate();
        }

        private void ParseData()
        {
            
            var obj = Message.ToObject(_client.GetMessage());

            if (obj == null)
            {
                Console.WriteLine(@"Failed to parse message");
                return;
            }
            


            switch (obj.MessageType())
            {
                case nameof(PlayerMsg):
                    HandlePlayersMsg(obj);
                    break;
                case nameof(IdMsg):
                    HandleIdMsg(obj);
                    break;
                case nameof(MoveMsg):
                    HandleMoveMsg(obj);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // to players
            // var boardRows = msg.Split('|');
            // if (boardRows.Length != 6)
            //     return;
            // for (var i = 0; i < boardRows.Length; i++)
            // {
            //     var row = boardRows[i];
            //     for (var j = 0; j < boardRows[i].Length; j++)
            //     {
            //         _board[i, j] = row[j].ToString();
            //     }
            // }


            //Invoke(new MethodInvoker(() => markerHolder.Refresh()));
        }

        private void HandlePlayersMsg(Message obj)
        {
            var player = obj.Convert<PlayerMsg>();
            MessageBox.Show(player.Player);
        }

        private void HandleIdMsg(Message obj)
        {
            Send(new IdMsg().Set("GUI"));
        }

        private void HandleMoveMsg(Message obj)
        {
            var move = obj.Convert<MoveMsg>();
            _board[move.Row, move.Column] = move.Player;
            _boxes[move.Row, move.Column].Refresh();
        }

        private void GetPlayerBtn_Click(object sender, EventArgs e)
        {
            Send(new PlayerMsg());
        }
    }
}