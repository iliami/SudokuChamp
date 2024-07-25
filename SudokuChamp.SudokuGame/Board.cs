using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SudokuChamp.API.SudokuGame
{
    internal class Board
    {
        private Cell?[,] cells = new Cell?[SudokuConstants.BOARD_SIZE, SudokuConstants.BOARD_SIZE];

        public bool IsEditableAt(int x, int y)
        {
            ValidateCoordinates(x, y);
            return cells[x,y]?.IsEditable ?? false;
        }
        public bool TrySetValueAt(int x, int y, int value)
        {
            ValidateCoordinates(x, y);
            cells[x, y]?.SetValue(value); 
            return IsEditableAt(x, y);
        }

        public Cell? this[int x, int y]
        {
            get => cells[x, y];
            set 
            { 
                ValidateCoordinates(x, y);
                cells[x, y] = value;
            }
        }

        public (int cellRow, int cellCol) FindFirstEmptyCell()
        {
            for (int row = 0; row < SudokuConstants.BOARD_SIZE; row++)
            {
                for (int col = 0; col < SudokuConstants.BOARD_SIZE; col++)
                {
                    if (row == 1 && col == 8)
                    { }
                    if (cells[row, col] == null)
                        return (row, col);
                }
            }
            return (-1, -1);
        }

        private void ValidateCoordinates(int x, int y)
        {
            if (x < 0)
                throw new ArgumentOutOfRangeException(nameof(x), "x должен быть неотрицательным");
            if (x >= SudokuConstants.BOARD_SIZE)
                throw new ArgumentOutOfRangeException(nameof(x), $"x должен быть меньше размера игрового поля, равного {SudokuConstants.BOARD_SIZE}");
           
            if (y < 0)
                throw new ArgumentOutOfRangeException(nameof(y), "y должен быть неотрицательным");
            if (y >= SudokuConstants.BOARD_SIZE)
                throw new ArgumentOutOfRangeException(nameof(y), $"y должен быть меньше размера игрового поля, равного {SudokuConstants.BOARD_SIZE}");
        }
    }
}
