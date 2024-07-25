using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SudokuChamp.API.DAL.Entities;
using SudokuChamp.Server.Utils.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SudokuChamp.Server.Utils
{
    public class JwtProvider : IJwtProvider
    {
        public JwtOptions Options { get; }

        public JwtProvider(IOptions<JwtOptions> options)
        {
            Options = options.Value;
        }


        public string CreateToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            Claim[] claims = [new("userId", user.Id.ToString())];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(Options.ExpiresHours)
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
