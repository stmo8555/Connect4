using System.Drawing;
using System.Windows.Forms;
using Server;

namespace ConnectFour
{
    public partial class Form1 : Form
    {
        private Board _board = new Board();
        public Form1()
        {
            InitializeComponent();
            var server = new Server();
        }

        private void RenderGameStart(object sender, PaintEventArgs e)
        {
            _board.TestData();
            var eGraphics = e.Graphics;
            var canvasHeight = Canvas.Height;
            var canvasWidth = Canvas.Width;
            const int columns = 7;
            const int rows = 6;
            var circleDiameter = canvasHeight / columns - 20;

            var spaceColumns = (canvasWidth - circleDiameter * columns) / 8;
            var spaceRows = (canvasHeight - circleDiameter * rows) / 8;

            
            var y = canvasHeight - spaceRows * 2;
            for (var i = 0; i < rows; i++)
            {   
                var x = 0;
                y -= i == 0 ? circleDiameter : (circleDiameter + spaceRows);
                for (var j = 0; j < columns; j++)
                {
                    var color = _board._pos[i,j];
                    x += j == 0 ? circleDiameter : circleDiameter + spaceColumns;
                    var brush = Brushes.Gray;
                    switch (color)
                    {
                        case "Green":
                            brush = Brushes.Green;
                            break;
                        case "Red":
                            brush = Brushes.Blue;
                            break;
                    }
                        
                        
                    eGraphics.FillEllipse(brush, x, y, circleDiameter, circleDiameter);
    
                }
            }
        }
    }
}