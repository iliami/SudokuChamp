using System.Text.Json;
using System.Text.Json.Serialization;
using SudokuChamp.API.DAL.Repo.Abstract;
using SudokuChamp.Server.DTO.Sudoku;
using SudokuChamp.Server.Services.Abstract;

namespace SudokuChamp.Server.Services
{
    public class SudokuService : ISudokuService
    {
        private readonly ISudokuRepo sudokuRepo;

        public SudokuService(ISudokuRepo sudokuRepo)
        {
            this.sudokuRepo = sudokuRepo;
        }

        public async Task<SudokuDTO> Get(Guid sudokuId)
        {
            var sudoku = await sudokuRepo.Get(sudokuId)
                ?? throw new Exception($"Нет судоку с Id {sudokuId}");
            sudoku.TotalAttemps += 1;
            var res = new SudokuDTO
            {
                Id = sudoku.Id,
                Board = JsonSerializer.Deserialize<int[][]>(sudoku.Board)!,
                Difficulty = sudoku.Difficulty,
                TotalAttemps = sudoku.TotalAttemps
            };
            return res;
        }

        public async Task<IEnumerable<SudokuDTO>> GetByPage(int page)
        {
            var sudokus = await sudokuRepo.GetWithPagination(page);
            var res = new List<SudokuDTO>();

            foreach (var sudoku in sudokus)
            {
                res.Add(new SudokuDTO
                {
                    Id = sudoku.Id,
                    Board = JsonSerializer.Deserialize<int[][]>(sudoku.Board)!,
                    Difficulty = sudoku.Difficulty,
                    TotalAttemps = sudoku.TotalAttemps
                });
            }

            return res;
        }
    }
}
