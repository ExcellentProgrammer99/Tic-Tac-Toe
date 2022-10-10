using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tictactoe
{
   public class Board
    {
        public int Size { get; set; }
        public string[,] boardvalues { get; set; }
        public Board(int Size)
            {
            this.Size = Size;
            boardvalues = new string[Size, Size];
            
            }
        public int Search(int a, int b)
        {
            int counter = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (j == a && i == b)
                        return counter;
                    counter++;
                }
            }
            throw new Exception();
        }
    }
}
