using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Server;

namespace ConnectFour
{
    public partial class Form1 : Form
    {
        private PictureBox[,] _boxes = new PictureBox[6, 7];
        private Board _board = new Board();

        public Form1()
        {
            InitializeComponent();
            InitBoxes();
            //var server = new Server();
        }
        
        private void PaintBox(object sender, PaintEventArgs e)
        {
            var item = sender as PictureBox;
            e.Graphics.FillEllipse(Brushes.Cyan, 0,0, item.Width, item.Height);
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
    }
}