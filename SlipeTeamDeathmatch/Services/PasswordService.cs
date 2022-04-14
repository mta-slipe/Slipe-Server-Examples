namespace SlipeTeamDeathmatch.Services;

public class PasswordService : IPasswordService
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Matches(string source, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(source, hash);
    }
}
