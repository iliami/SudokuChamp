using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuChamp.API.SudokuGame
{
    public class Sudoku
    {
        private Board Board { get; set; }
        private Sudoku()
        {
        }
        public static Sudoku CreateWithEasyDifficulty()
        {
            var sudokuGame = new Sudoku();
            var generator = new BoardGenerator();

            sudokuGame.Board = generator.Generate(SudokuDifficulty.Easy);

            return sudokuGame;
        }
        public static Sudoku CreateWithMediumDifficulty()
        {
            var sudokuGame = new Sudoku();
            var generator = new BoardGenerator();

            sudokuGame.Board = generator.Generate(SudokuDifficulty.Medium);

            return sudokuGame;
        }
        public static Sudoku CreateWithHardDifficulty()
        {
            var sudokuGame = new Sudoku();
            var generator = new BoardGenerator();

            sudokuGame.Board = generator.Generate(SudokuDifficulty.Hard);

            return sudokuGame;
        }

        public int[,] GetBoard()
        {
            int[,] values = new int[SudokuConstants.BOARD_SIZE, SudokuConstants.BOARD_SIZE];
            for (int x = 0; x < SudokuConstants.BOARD_SIZE; x++)
                for (int y = 0; y < SudokuConstants.BOARD_SIZE; y++)
                    values[x, y] = Board[x, y]?.Value ?? 0;

            return values;
        }
    }
}
