using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ConnectFour;

namespace ConnectFourGui
{
    public partial class Form1 : Form
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private Socket _mysock;
        public readonly string[,] _board = new string[Rows, Columns];
        private string _p1;
        private string _p2;

        private readonly PictureBox[,] _boxes = new PictureBox[6, 7];

        public Form1()
        {
            InitializeComponent();
            InitBoxes();
            new Thread(StartSocket).Start();
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

        private void StartSocket()
        {
            using (var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                s.Connect("127.0.0.1", 5000);
                _mysock = s;
                OnConnected(s);
            }
        }

        private void OnConnected(Socket handler)
        {
            while (true)
            {
                var buffer = new byte[1_024];
                var received = handler.Receive(buffer, SocketFlags.None);
                var data = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine(data);
                ParseData(data);
            }
        }

        private void ParseData(string msg)
        {
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
                    _mysock.Send(Encoding.UTF8.GetBytes("id|GUI"));
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
            _mysock.Send(Encoding.UTF8.GetBytes("players|123"));
        }
    }
}