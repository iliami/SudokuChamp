using SudokuChamp.API.DAL.Entities.Abstract;

namespace SudokuChamp.API.DAL.Entities
{
    public class User : IIdentifiable
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public int TotalGames { get; set; }
        public int BestTimeMilliseconds { get; set; }
    }
}
