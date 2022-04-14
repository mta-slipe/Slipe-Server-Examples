namespace SlipeTeamDeathmatch.Services;

public interface IPasswordService
{
    string Hash(string password);
    bool Matches(string source, string hash);
}