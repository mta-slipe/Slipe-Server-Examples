using Microsoft.EntityFrameworkCore;
using SlipeTeamDeathmatch.Models;

namespace SlipeTeamDeathmatch.Persistence;
public interface IEntity
{
    public uint Id { get; set; }
}
