using SudokuChamp.Server.DTO.Auth;

namespace SudokuChamp.Server.Services.Abstract
{
    public interface IAuthService
    {
        Task Register(RegistrationRequestDTO registrationRequestDTO);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}
