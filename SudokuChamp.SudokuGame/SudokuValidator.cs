using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuChamp.API.SudokuGame
{
    public class SudokuValidator
    {
        private int[,]? sudoku;
        private SudokuValidator()
        {
        }

        public static SudokuValidator Create(int[,] sudoku)
        {
            var validator = new SudokuValidator();
            validator.sudoku = sudoku;
            validator.ValidateSudoku();
            return validator;
        }

        public bool IsTrueSolved { get; private set; } = true;
        public string ErrorMessage { get; private set; } = string.Empty;
        private void ValidateSudoku()
        {
            var rows = SudokuConstants.BOARD_SIZE;
            var cols = SudokuConstants.BOARD_SIZE;

            var reqSum = 0;
            for (int x = 1; x <= rows; x++) reqSum += x;

            for (int row = 0; row < rows; row++)
            {
                int sumInRow = 0;
                for (int col = 0; col < cols; col++) sumInRow += sudoku![row, col];
                if (sumInRow != reqSum)
                {
                    IsTrueSolved = false;
                    ErrorMessage = $"Row: {row}";
                    return;
                }
            }

            for (int col = 0; col < cols; col++)
            {
                int sumInCol = 0;
                for (int row = 0; row < rows; row++) sumInCol += sudoku![row, col];
                if (sumInCol != reqSum)
                {
                    IsTrueSolved = false;
                    ErrorMessage = $"Col: {col}";
                    return;
                }
            }

            for (int block = 0; 
                block < (cols * rows) / (SudokuConstants.BLOCK_SIZE * SudokuConstants.BLOCK_SIZE); 
                block++)
            {
                int firstCellRowInBlock = SudokuConstants.BLOCK_SIZE * (block / SudokuConstants.BLOCK_SIZE);
                int firstCellColInBlock = block * SudokuConstants.BLOCK_SIZE % cols;

                int sumInBlock = 0;
                for (int row = firstCellRowInBlock; row < firstCellRowInBlock + SudokuConstants.BLOCK_SIZE; row++)
                    for (int col = firstCellColInBlock; col < firstCellColInBlock + SudokuConstants.BLOCK_SIZE; col++)
                        sumInBlock += sudoku![row, col];
                if (sumInBlock != reqSum)
                {
                    IsTrueSolved = false;
                    ErrorMessage = $"Block: {block}";
                    return;
                }
            }
        }
    }
}
