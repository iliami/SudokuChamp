using SudokuChamp.Server.DTO.Sudoku;

namespace SudokuChamp.Server.Services.Abstract
{
    public interface ISudokuService
    {
        Task<SudokuDTO> Get(Guid sudokuId);
        Task<IEnumerable<SudokuDTO>> GetByPage(int page);
    }
}
