using SudokuChamp.API.DAL.Entities;
using SudokuChamp.API.DAL.Json.JsonSets.Abstract;

namespace SudokuChamp.API.DAL.Json.JsonSets
{
    public class UserSet : JsonSet<User>
    {
        public UserSet() : base(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"SudokuChamp\users.txt"))
        {
        }
    }
}
