using SudokuChamp.API.DAL.Entities;

namespace SudokuChamp.API.DAL.Repo.Abstract
{
    public interface IUserRepo
    {
        Task<User?> GetUserByName(string userName);
        Task<User?> GetUserById(Guid userId);
        Task Register(string userName, string email, string passwordHash);
        Task Update(Guid Id, User user);
    }
}
