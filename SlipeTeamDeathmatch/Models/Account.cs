namespace SlipeTeamDeathmatch.Models;
public class Account
{
    public uint Id { get; set; }
    public string Name { get; set; }
    public int MatchCount { get; set; }
    public int KillCount { get; set; }
    public int DeathCount { get; set; }
    public bool IsGuest { get; set; }

    protected Account()
    {
        this.Name = "";
    }

    public Account(string name)
    {
        this.Name = name;
    }
}

