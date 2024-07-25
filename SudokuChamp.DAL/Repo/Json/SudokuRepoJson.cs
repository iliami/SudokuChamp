using SudokuChamp.API.DAL.Entities;
using SudokuChamp.API.DAL.Json;
using SudokuChamp.API.DAL.Repo.Abstract;

namespace SudokuChamp.API.DAL.Repo.Json
{
    public class SudokuRepoJson : ISudokuRepo
    {
        private readonly JsonContext context;

        public SudokuRepoJson(JsonContext context)
        {
            this.context = context;
        }

        public async Task<Sudoku?> Get(Guid Id)
        {
            var all = await context.Sudokus.GetAll();
            var res = all.FirstOrDefault(s => s.Id == Id);
            return res;
        }

        public async Task<IEnumerable<Sudoku>> GetWithPagination(int page)
        {
            var all = await context.Sudokus.GetAll();
            var sudokus = all.Skip(( page - 1 ) * 10)
                .Take(10)
                .ToList();
            return sudokus;
        }
    }
}
