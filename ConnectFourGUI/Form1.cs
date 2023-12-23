using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ConnectFourApi;
using MessageLib;
using ServerClientLib;
using Message = MessageLib.Message;

namespace ConnectFour
{
    public partial class Form1 : Form
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private readonly string[,] _board = new string[Rows, Columns];
        private string _p1;
        private string _p2;
        private readonly PictureBox[,] _boxes = new PictureBox[6, 7];
        private readonly GuiApi _api;

        public Form1()
        {
            InitializeComponent();
            InitBoxes();
            _api = new GuiApi(OnMoveReceived, OnWinReceived);
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

        private void OnMoveReceived(int row, int column, string player)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => OnMoveReceived(row, column, player)));
            }
            else
            {
                _board[row, column] = player;
                _boxes[row, column].Refresh();
            }
        }
        
        private void OnWinReceived(string player)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => OnWinReceived(player)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void GetPlayerBtn_Click(object sender, EventArgs e)
        {
            var players = _api.GetPlayers();
            MessageBox.Show(string.Join(",", players));
        }
    }
}