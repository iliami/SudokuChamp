using SudokuChamp.Server.DTO.Record;

namespace SudokuChamp.Server.Services.Abstract
{
    public interface IRecordService
    {
        Task<RecordResponseDTO> Get(string userName);
        Task<IEnumerable<SudokuRecordResponseDTO>> GetBySudokuId(Guid sudokuId);
        Task AddRecord(Guid userId, Guid sudokuId, int ms);
    }
}
