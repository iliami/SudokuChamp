using SudokuChamp.API.DAL.Entities;

namespace SudokuChamp.Server.DTO.Sudoku
{
    public class SudokuDTO
    {
        public Guid Id { get; set; }
        public required int[][] Board { get; set; }
        public required SudokuDifficulty Difficulty { get; set; }
        public required int TotalAttemps { get; set; }
    }
}
