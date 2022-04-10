using System.Numerics;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Enums;

namespace SlipeTeamDeathmatch.Models;

public struct SpawnPoint
{
    public Vector3 Position { get; init; }
    public float Rotation { get; init; }
    public Team Team { get; init; }
    public Dictionary<WeaponId, ushort> Weapons { get; init; }


    public SpawnPoint(Vector3 position, float rotation, Team team, Dictionary<WeaponId, ushort> weapons)
    {
        this.Position = position;
        this.Rotation = rotation;
        this.Team = team;
        this.Weapons = weapons;
    }
}

