using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Packets.Lua.Camera;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Enums;
using SlipeServer.Server.Extensions;
using SlipeTeamDeathmatch.Elements;

namespace SlipeTeamDeathmatch.Models;
public class Match
{
    public int Id { get; }
    public string Name { get; }

    private readonly List<TdmPlayer> players;
    public IReadOnlyCollection<TdmPlayer> Players => this.players.AsReadOnly();

    public MatchState State { get; set; }
    public Map? Map { get; set; }

    public MatchResult Result { get; private set; }

    public TdmPlayer Host { get; set; }

    private readonly List<Death> deaths;
    public IReadOnlyCollection<Death> Deaths => this.deaths.AsReadOnly();

    public bool CanStart => this.State == MatchState.Lobby && this.Map != null;

    public Match(int id, string name, TdmPlayer host)
    {
        this.Id = id;
        this.Name = name;
        this.Host = host;

        this.players = new();
        this.deaths = new();

        this.AddPlayer(host);
    }

    public void AddPlayer(TdmPlayer player)
    {
        if (this.State != MatchState.Lobby)
            return;

        this.players.Add(player);
        player.Match = this;
        player.Disconnected += HandlePlayerDisconnect;
        player.Wasted += HandlePlayerWasted;

        this.Map?.Elements.CreateFor(new Player[] { player });
    }

    public void RemovePlayer(TdmPlayer player)
    {
        if (!this.players.Contains(player))
            return;

        this.players.Remove(player);
        player.Match = null;
        player.Disconnected -= HandlePlayerDisconnect;
        player.Wasted -= HandlePlayerWasted;
        player.Team = null;

        this.Map?.Elements.DestroyFor(new Player[] { player });

        CheckForWin();

        if (!this.players.Any())
            this.Abandoned?.Invoke(this, new());
    }

    public void SetMap(Map? map)
    {
        if (this.State != MatchState.Lobby)
            return;

        if (this.Map != null)
            this.Map.Elements.DestroyFor(this.players);

        this.Map = map;

        if (this.Map != null)
            this.Map.Elements.CreateFor(this.players);

        var x = this.Map!.Elements.Select(x => x.Id);
    }

    public void Start()
    {
        if (!this.CanStart)
            return;

        this.State = MatchState.InProgress;
        this.Started?.Invoke(this, EventArgs.Empty);

        foreach (var player in this.players)
            player.Team = this.Map!.Teams.OrderBy(x => x.Players.Count).First();

        foreach (var team in this.Map!.Teams)
        {
            var players = this.players.Where(x => x.Team == team).ToArray();
            var spawnPoints = this.Map.SpawnPoints.Where(x => x.Team == team).ToArray();

            for (int i = 0; i < players.Length; i++)
            {
                var spawnPoint = spawnPoints[i % spawnPoints.Length];
                var player = players[i];

                player.Spawn(spawnPoint.Position, spawnPoint.Rotation, 7, 0, 0);
                player.Camera.Target = player;
                player.Camera.Fade(CameraFade.In);
                foreach (var weapon in spawnPoint.Weapons)
                    player.Weapons.Add(new(weapon.Key, weapon.Value));

                player.SendStart();
                player.Account.MatchCount++;
            }
        }
    }

    public void Stop()
    {
        this.State = MatchState.Finished;

        foreach (var player in this.players.ToArray())
            RemovePlayer(player);
    }

    public void Reset()
    {
        this.State = MatchState.Lobby;
        this.deaths.Clear();

        foreach (var player in this.players)
            player.SendMatch(this);
    }

    public void EndMatch(MatchResult result)
    {
        this.Result = result;
        this.Ended?.Invoke(result);
        this.State = MatchState.Review;

        foreach (var player in this.players)
        {
            player.SendMatch(this);
            player.Camera.Fade(CameraFade.Out, 5);
        }

        Task.Run(async () =>
        {
            await Task.Delay(10000);
            Reset();
        });
    }

    public LuaValue GetLuaValue()
    {
        return new LuaValue(new Dictionary<LuaValue, LuaValue>()
        {
            ["id"] = this.Id,
            ["name"] = this.Name,
            ["players"] = this.Players.Select(x => x.GetLuaValue()).ToArray(),
            ["mapName"] = this.Map?.Name ?? "N/A",
            ["state"] = this.State.ToString(),
            ["host"] = this.Host.GetLuaValue(),
            ["deaths"] = this.Deaths.Select(x => x.GetLuaValue()).ToArray(),
        });
    }

    private void CheckForWin()
    {
        var livingPlayers = this.players.Where(x => x.IsAlive && x.Team != null);
        if (!livingPlayers.Any())
        {
            this.EndMatch(new MatchResult());
            return;
        }

        var livingTeams = livingPlayers
            .Select(x => x.Team)
            .Distinct();

        if (livingTeams.Count() == 1)
            this.EndMatch(new MatchResult(livingTeams.Single(), this.deaths));
    }

    private void HandlePlayerWasted(Ped sender, PedWastedEventArgs e)
    {
        var killer = e.Killer as TdmPlayer;
        if (e.Source is not TdmPlayer victim)
            return;

        victim.Account.DeathCount++;

        if (killer != null)
            killer.Account.KillCount++;

        this.deaths.Add(new Death(victim, killer, (WeaponId)e.WeaponType));

        CheckForWin();
    }

    private void HandlePlayerDisconnect(Player player, PlayerQuitEventArgs _)
    {
        this.RemovePlayer((player as TdmPlayer)!);
        CheckForWin();
    }

    public event EventHandler<EventArgs>? Started;
    public event Action<MatchResult>? Ended;
    public event EventHandler<EventArgs>? Abandoned;
}

