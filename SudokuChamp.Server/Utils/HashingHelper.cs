using static BCrypt.Net.BCrypt;

namespace SudokuChamp.Server.Utils
{
    public static class HashingHelper
    {
        public static string GetHash(this string password)
            => EnhancedHashPassword(password);

        public static bool IsEqualToHashOf(this string passwordHash, string password)
            => EnhancedVerify(password, passwordHash);
    }
}
