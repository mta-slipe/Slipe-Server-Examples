using Microsoft.EntityFrameworkCore;
using SlipeTeamDeathmatch.Models;

namespace SlipeTeamDeathmatch.Persistence;
public class TdmContext : DbContext
{
    public DbSet<Account> Accounts { get; set; } = null!;

    public TdmContext()
    {
        this.Database.Migrate();
    }

    public TdmContext(DbContextOptions options) : base(options)
    {
        this.Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlite("Data Source=database.db");
    }
}
