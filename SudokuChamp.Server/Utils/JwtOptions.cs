namespace SudokuChamp.Server.Utils
{
    public class JwtOptions
    {
        public required string SecretKey { get; set; }
        public required int ExpiresHours { get; set; }
    }
}
