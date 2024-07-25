using SudokuChamp.API.DAL.Repo;
using SudokuChamp.API.SudokuGame;
using SudokuChamp.Server.Services.Abstract;

namespace SudokuChamp.Server.Services
{
    public class SoloGameService : ISoloGameService
    {
        public SoloGameService()
        {
        }

        public int[,] CreateEasySudoku()
        {
            var game = Sudoku.CreateWithEasyDifficulty();
            return game.GetBoard();
        }

        public int[,] CreateMediumSudoku()
        {
            var game = Sudoku.CreateWithMediumDifficulty();
            return game.GetBoard();
        }

        public int[,] CreateHardSudoku()
        {
            var game = Sudoku.CreateWithHardDifficulty();
            return game.GetBoard();
        }

        public bool IsTrueSolved(int[,] sudokuBoard)
        {
            var validator = SudokuValidator.Create(sudokuBoard);
            return validator.IsTrueSolved;
        }
    }
}
