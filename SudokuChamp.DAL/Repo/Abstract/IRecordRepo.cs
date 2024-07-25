using SudokuChamp.API.DAL.Entities;

namespace SudokuChamp.API.DAL.Repo.Abstract
{
    public interface IRecordRepo
    {
        Task<IEnumerable<GameRecord>> GetBySudokuId(Guid userId);
        Task AddRecord(GameRecord game);
    }
}
