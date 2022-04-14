using SlipeTeamDeathmatch.Persistence;

namespace SlipeTeamDeathmatch.Models;
public class Account : IEntity
{
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int MatchCount { get; set; }
    public int KillCount { get; set; }
    public int DeathCount { get; set; }
    public bool IsGuest { get; set; }

    protected Account()
    {
        this.Name = "";
        this.Password = "";
    }

    public Account(string name, string password)
    {
        this.Name = name;
        this.Password = password;
    }

    public static Account CreateGuest()
    {
        return new Account(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
        {
            IsGuest = true
        };
    }
}

