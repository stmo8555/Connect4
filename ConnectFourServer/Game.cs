using System;
using MessageLib;
using ServerClientLib;

namespace ConnectFourServer
{
    public class Game
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private readonly string[,] _board = new string[Rows, Columns];
        private readonly CommunicationManager _communicationManager;

        public Game()
        {
            InitBoard();
            _communicationManager = new CommunicationManager(new Server(3));
            _communicationManager.NewMove += LegalMove;
        }

        private void LegalMove(string player, int row, int column)
        {
            if (OutsideBoundary(row, column))
            {
                OnInvalidMove("Outside boundary");
                return;
            }

            if (CellOccupied(row, column))
            {
                OnInvalidMove("Cell occupied");
                return;
            }

            if (!HaveFloor(row, column))
            {
                OnInvalidMove("No floor");
                return;
            }

            InsertMove(player, row, column);

            _communicationManager.SendToGui(
                new FullMessage().Set(Commands.Move, new MoveMsg().Set(row, column, player)));

            if (GameWon())
                _communicationManager.SendToGui(new FullMessage().Set(Commands.Win, new WinMsg().Set(player)));
        }

        private bool GameWon()
        {
            for (var i = 0; true; i++)
            {
                for (var j = 0; true; j++)
                {
                    var cell = _board[i, j];
                    if (cell == "N")
                        return false;
                    return VerticalWin(i, j, cell) >= 4 || HorizontalWin(i, j, cell) >= 4 ||
                           DiagonalWin(i, j, cell) >= 4;
                }
            }
        }

        private int DiagonalWin(int row, int column, string player)
        {
            var nextRow = ++row;
            var nextColumn = ++column;
            if (OutsideBoundary(nextRow, nextColumn))
                return 0;
            if (_board[nextRow, nextColumn] != player)
                return 0;
            return 1 + DiagonalWin(nextRow, nextColumn, player);
        }

        private int HorizontalWin(int row, int column, string player)
        {
            var nextColumn = ++column;
            if (OutsideColumnBoundary(row, nextColumn))
                return 0;
            if (_board[row, nextColumn] != player)
                return 0;
            return 1 + HorizontalWin(row, nextColumn, player);
        }

        private int VerticalWin(int row, int column, string player)
        {
            var nextRow = ++row;
            if (OutsideColumnBoundary(nextRow, column))
                return 0;
            if (_board[nextRow, column] != player)
                return 0;
            return 1 + VerticalWin(nextRow, column, player);
        }

        private bool OutsideBoundary(int row, int column)
        {
            return OutsideRowBoundary(row) || OutsideColumnBoundary(row, column);
        }

        private static bool OutsideColumnBoundary(int row, int column)
        {
            return row < 0 || column > Columns;
        }

        private static bool OutsideRowBoundary(int row)
        {
            return row < 0 || row > Rows;
        }

        private void OnInvalidMove(string msg)
        {
            Console.WriteLine(msg);
        }

        private bool HaveFloor(int row, int column)
        {
            if (row == 0)
                return true;
            return _board[row - 1, column] != "N";
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

        private void InsertMove(string player, int row, int column)
        {
            _board[row, column] = player;
        }


        private bool CellOccupied(int row, int column)
        {
            return _board[row, column] != "N";
        }
    }
}