using SudokuChamp.API.DAL.Entities.Abstract;

namespace SudokuChamp.API.DAL.Entities
{
    public class Sudoku : IIdentifiable
    {
        public Guid Id { get; set; }
        public required string Board { get; set; }
        public SudokuDifficulty Difficulty { get; set; }
        public int TotalAttemps { get; set; }
    }

    public enum SudokuDifficulty
    {
        Easy,
        Medium,
        Hard
    }
}
