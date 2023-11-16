using System;
using ServerClientLib;

namespace ConnectFourServer
{
    public class Game
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private readonly string[,] _board = new string[Rows, Columns];
        private CommunicationManager _communicationManager;
        
        public Game()
        {
            InitBoard();
            _communicationManager = new CommunicationManager(new Server(3));
            _communicationManager.NewMove += LegalMove;
        }

        private void LegalMove(string player, int row, int column)
        {
            InsertMove(player, row, column);
            // replace with only send the latest move
            _communicationManager.SendToGui(GetSerializedBoard());
        }

        private void InitBoard()
        {
            for (var i = 0; i < _board.GetLength(0); i++)
            {
                for (var j = 0; j < _board.GetLength(1); j++)
                {
                    _board[i, j] = "N";
                }
            }
        }
        private string GetSerializedBoard()
        {
            var data = "";
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    data += _board[i, j];
                }
                
                if (i != Rows - 1)
                    data += "|";
            }

            return data;
        }

        private void InsertMove(string player,int row, int column)
        {
            _board[row, column] = player;
        }
    }
}