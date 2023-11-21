using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ServerClientLib;

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

        private void TriggerNewRender(object sender, EventArgs e)
        {
            var p = sender as PictureBox;
            p?.Invalidate();
        }
        
        private void ParseData()
        {
            var msg = _client.GetMessage();
            // 111221|221NNN
            // commmand|data
            var strings = msg.Split('|');
            if (strings.Length != 2)
                return;

            var command = strings[0].ToLower();
            var data = strings[1];

            switch (command)
            {
                case "id":
                    _client.Send("id|GUI");
                    break;
                case "players":
                    var players = data.Split(',');
                    if (players.Length != 2)
                        return;
                    _p1 = players[0];
                    _p2 = players[1];
                    P1Label.Text += $@" {players[0]}";
                    P2Label.Text += $@" {players[1]}";
                    break;
                case "move":
                    // player, 1, 2
                    var index = data.Split(',');
                    if (index.Length != 3)
                        return;

                    if (!int.TryParse(index[1], out var row))
                        return;
                    if (!int.TryParse(index[2], out var column))
                        return;

                    _board[row, column] = index[0];
                    _boxes[row, column].Refresh();
                    break;
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

        private void GetPlayerBtn_Click(object sender, EventArgs e)
        {
            _client.Send("players|123");
        }
    }
}