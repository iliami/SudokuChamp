using SudokuChamp.API.DAL.Entities.Abstract;

namespace SudokuChamp.API.DAL.Entities
{
    public class GameRecord : IIdentifiable
    {
        public Guid Id { get; set; }
        public Guid SudokuId { get; set; }
        public Guid UserId { get; set; }
        public int TimeMilliseconds { get; set; }
    }
}
