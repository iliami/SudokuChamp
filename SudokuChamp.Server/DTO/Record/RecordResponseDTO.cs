namespace SudokuChamp.Server.DTO.Record
{
    public class RecordResponseDTO
    {
        public required string UserName { get; set; }
        public required int TotalGames { get; set; }
        public required int BestTimeMilliseconds { get; set; }
    }
}
