using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server;

namespace ConnectFour
{
    public partial class Form1 : Form
    {
        private readonly Brush _p1Brush = Brushes.Red;
        private readonly Brush _p2Brush = Brushes.Blue;

        private readonly PictureBox[,] _boxes = new PictureBox[6, 7];
        private readonly Board _board = new Board();

        public Form1()
        {
            InitializeComponent();
            InitBoxes();
            _board.TestData();
            new Thread(StartSocket);
        }

        private void PaintBox(object sender, PaintEventArgs e)
        {
            var item = sender as PictureBox;
            var brush = Brushes.Gray;
            if (item?.Tag is Tuple<int, int> index2d)
            {
                var playerData = _board._pos[index2d.Item1, index2d.Item2];
                switch (playerData)
                {
                    case "p1":
                        brush = _p1Brush;
                        break;
                    case "p2":
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
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Bind(new IPEndPoint(0, 1234));
            s.Listen(2);
            var handler = s.Accept();
            while (true)
            {
                var buffer = new byte[1_024];
                var received = handler.Receive(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);

                Console.WriteLine(response);

                var msg = "Hello World!";
                var msgBuffer = Encoding.UTF8.GetBytes(msg);
                handler.Send(msgBuffer);
            }
        }
    }
}