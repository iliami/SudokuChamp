using SudokuChamp.API.DAL.Repo.Abstract;
using SudokuChamp.Server.DTO.Auth;
using SudokuChamp.Server.Services.Abstract;
using SudokuChamp.Server.Utils;
using SudokuChamp.Server.Utils.Abstract;

namespace SudokuChamp.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo userRepo;
        private readonly IJwtProvider jwtProvider;

        public AuthService(IUserRepo userRepo, IJwtProvider jwtProvider)
        {
            this.userRepo = userRepo;
            this.jwtProvider = jwtProvider;
        }

        public async Task Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var user = await userRepo.GetUserByName(registrationRequestDTO.UserName);
            if (user is not null)
            {
                throw new Exception("Пользователь с таким именем уже существует");
            }

            var passwordHash = registrationRequestDTO.Password.GetHash();

            await userRepo.Register(registrationRequestDTO.UserName, registrationRequestDTO.Email, passwordHash);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await userRepo.GetUserByName(loginRequestDTO.UserName)
                ?? throw new Exception("Пользователь с таким именем не зарегистрирован");

            if (!user.PasswordHash.IsEqualToHashOf(loginRequestDTO.Password))
            {
                throw new Exception("Неверный пароль");
            }

            var token = jwtProvider.CreateToken(user);

            var response = new LoginResponseDTO { Token = token };

            return response;
        }
    }
}
