using Microsoft.AspNetCore.Mvc;
using SudokuChamp.Server.DTO.Auth;
using SudokuChamp.Server.Services.Abstract;

namespace SudokuChamp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                await authService.Register(registrationRequestDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var response = await authService.Login(loginRequestDTO);
                var cookieOptions = new CookieOptions
                {
                    Path = "/",
                    MaxAge = TimeSpan.FromHours(12)
                    // Другие опции...
                };
                HttpContext.Response.Cookies.Append("what-is-that", response.Token, cookieOptions);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
