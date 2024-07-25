using SudokuChamp.API.DAL.Json.JsonSets;

namespace SudokuChamp.API.DAL.Json
{
    public class JsonContext
    {
        public UserSet Users { get; set; } = new();
        public SudokuSet Sudokus { get; set; } = new();
        public GameSet Games { get; set; } = new();
    }
}
