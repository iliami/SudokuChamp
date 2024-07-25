using SudokuChamp.API.DAL.Entities;
using SudokuChamp.API.DAL.Json;
using SudokuChamp.API.DAL.Repo.Abstract;

namespace SudokuChamp.API.DAL.Repo.Json
{
    public class RecordRepoJson : IRecordRepo
    {
        private readonly JsonContext context;

        public RecordRepoJson(JsonContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<GameRecord>> GetBySudokuId(Guid sudokuId)
        {
            var all = await context.Games.GetAll();

            var res = all.Where(g => g.SudokuId == sudokuId)
                .OrderBy(g => g.TimeMilliseconds)
                .Take(20)
                .ToList();

            return res;
        }
        public async Task AddRecord(GameRecord game)
        {
            game.Id = Guid.NewGuid();

            await context.Games.Add(game);

            var all = await context.Sudokus.GetAll();
            var item = all.First(s => s.Id == game.SudokuId);
            item.TotalAttemps += 1;
            await context.Sudokus.Update(item.Id, item);
        }
    }
}
