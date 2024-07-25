using SudokuChamp.API.DAL.Entities;
using SudokuChamp.API.DAL.Json;
using SudokuChamp.API.DAL.Repo.Abstract;

namespace SudokuChamp.API.DAL.Repo.Json
{
    public class UserRepoJson : IUserRepo
    {
        private readonly JsonContext context;

        public UserRepoJson(JsonContext context)
        {
            this.context = context;
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            var all = await context.Users.GetAll();
            var res = all.FirstOrDefault(u => u.Id == userId);
            return res;
        }
        public async Task<User?> GetUserByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return null;

            var all = await context.Users.GetAll();
            var res = all.FirstOrDefault(u => u.UserName == userName.ToLower());

            return res;
        }
        public async Task Register(string userName, string email, string passwordHash)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = userName.ToLower(),
                Email = email,
                PasswordHash = passwordHash
            };

            await context.Users.Add(user);
        }

        public async Task Update(Guid Id, User user)
        {
            await context.Users.Update(Id, user);
        }
    }
}
