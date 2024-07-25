using SudokuChamp.API.DAL.Entities;
using SudokuChamp.API.DAL.Repo.Abstract;
using SudokuChamp.Server.DTO.Record;
using SudokuChamp.Server.Services.Abstract;

namespace SudokuChamp.Server.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepo recordRepo;
        private readonly IUserRepo userRepo;

        public RecordService(IRecordRepo recordRepo, IUserRepo userRepo)
        {
            this.recordRepo = recordRepo;
            this.userRepo = userRepo;
        }

        public async Task<RecordResponseDTO> Get(string userName)
        {
            var user = await userRepo.GetUserByName(userName)
                ?? throw new Exception("Такой пользователь не зарегистрирован");

            var response = new RecordResponseDTO
            {
                UserName = user.UserName,
                TotalGames = user.TotalGames,
                BestTimeMilliseconds = user.BestTimeMilliseconds
            };

            return response;
        }

        public async Task<IEnumerable<SudokuRecordResponseDTO>> GetBySudokuId(Guid sudokuId)
        {
            var games = await recordRepo.GetBySudokuId(sudokuId)
                ?? throw new Exception("Такого судоку не существует");

            List<SudokuRecordResponseDTO> sudokuRecordResponseDTOs = [];
            foreach (var game in games)
            {
                var user = await userRepo.GetUserById(game.UserId);
                if (user is not null)
                {
                    var res = new SudokuRecordResponseDTO
                    {
                        UserName = user.UserName,
                        TimeMilliseconds = game.TimeMilliseconds
                    };
                    sudokuRecordResponseDTOs.Add(res);
                }
            }

            return sudokuRecordResponseDTOs;
        }

        public async Task AddRecord(Guid userId, Guid sudokuId, int ms)
        {
            var record = new GameRecord
            {
                UserId = userId,
                SudokuId = sudokuId,
                TimeMilliseconds = ms
            };


            var user = await userRepo.GetUserById(userId)
                ?? throw new Exception("Такой пользователь не зарегистрирован");

            user.TotalGames += 1;
            if (user.BestTimeMilliseconds == 0 || user.BestTimeMilliseconds > ms)
            {
                user.BestTimeMilliseconds = ms;
            }
            await userRepo.Update(user.Id, user);
            await recordRepo.AddRecord(record);
        }
    }
}
