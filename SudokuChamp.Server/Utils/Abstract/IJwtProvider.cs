using SudokuChamp.API.DAL.Entities;

namespace SudokuChamp.Server.Utils.Abstract
{
    public interface IJwtProvider
    {
        string CreateToken(User user);
    }
}
