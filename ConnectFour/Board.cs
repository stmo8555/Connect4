using System;
using System.Runtime.InteropServices;

namespace Server
{
    public class Board
    {
        public readonly string[,] _pos;
        private const int Rows = 6;
        private const int Columns = 7;
        
        

        public Board()
        {
            _pos = new string[Rows, Columns];
        }

        public void TestData()
        {
            // for (var i = 0; i < Rows; i++)
            // {
            //     for (var j = 0; j < Columns; j++)
            //     {
            //         if (j % 2 == 0)
            //             _pos[i, j] = "Green";
            //         else
            //         {
            //             _pos[i, j] = "Red";
            //         }
            //     }
            // }
            _pos[0, 1] = "p1";
            _pos[2, 5] = "p2";
        }
        
    }
}