using SudokuChamp.API.DAL.Entities;
using SudokuChamp.API.DAL.Json.JsonSets.Abstract;

namespace SudokuChamp.API.DAL.Json.JsonSets
{
    public class GameSet : JsonSet<GameRecord>
    {
        public GameSet() : base(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"SudokuChamp\games.txt"))
        {
        }
    }
}
