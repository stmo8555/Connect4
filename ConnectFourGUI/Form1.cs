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
        
        private readonly Dictionary<string, Socket> _players = new Dictionary<string, Socket>();
        public readonly string[,] _board = new string[Rows,Columns];
        
        private readonly Brush _p1Brush = Brushes.Red;
        private readonly Brush _p2Brush = Brushes.Blue;
        
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
                switch (playerData)
                {
                    case "1":
                        brush = _p1Brush;
                        break;
                    case "2":
                        brush = _p2Brush;
                        break;
                }
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
                ParseData(data);
            }
        }

        private void ParseData(string msg)
        {
            // 111221|221NNN
            var boardRows = msg.Split('|');
            if (boardRows.Length != 6)
                return;
            for (var i = 0; i < boardRows.Length; i++)
            {
                var row = boardRows[i];
                for (var j = 0; j < boardRows[i].Length; j++)
                {
                    _board[i, j] = row[j].ToString();
                }
            }


            
            Invoke(new MethodInvoker(() => markerHolder.Refresh()));
        }
    }
}