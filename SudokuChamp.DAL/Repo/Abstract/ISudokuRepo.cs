using SudokuChamp.API.DAL.Entities;

namespace SudokuChamp.API.DAL.Repo.Abstract
{
    public interface ISudokuRepo
    {
        Task<Sudoku?> Get(Guid Id);
        Task<IEnumerable<Sudoku>> GetWithPagination(int page);
    }
}
