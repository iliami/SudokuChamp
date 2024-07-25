using SudokuChamp.API.DAL.Entities;
using SudokuChamp.API.DAL.Json.JsonSets.Abstract;

namespace SudokuChamp.API.DAL.Json.JsonSets
{
    public class SudokuSet : JsonSet<Sudoku>
    {
        public SudokuSet() : base(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"SudokuChamp\sudokus.txt"))
        {
        }
    }
}
