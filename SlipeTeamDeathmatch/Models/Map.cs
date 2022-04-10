using System.Drawing;
using System.Globalization;
using System.Numerics;
using System.Xml;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Enums;
using SlipeTeamDeathmatch.IdGenerators;

namespace SlipeTeamDeathmatch.Models;

public class Map
{
    public string Name { get; init; }

    private readonly List<WorldObject> objects;
    public IReadOnlyCollection<WorldObject> Objects => this.objects.AsReadOnly();


    private readonly List<Team> teams;
    public IReadOnlyCollection<Team> Teams => this.teams.AsReadOnly();


    public IEnumerable<Element> Elements => (Array.Empty<Element>()).Concat(this.objects).Concat(this.teams);


    private readonly List<SpawnPoint> spawnPoints;
    public IReadOnlyCollection<SpawnPoint> SpawnPoints => this.spawnPoints.AsReadOnly();


    public Map(string name)
    {
        this.Name = name;
        this.objects = new();
        this.teams = new();
        this.spawnPoints = new();
    }

    public void LoadFromFile(string path, Element parent)
    {
        var document = new XmlDocument();
        document.Load(path);

        var root = document.ChildNodes[0];

        var teams = root.ChildNodes
            .Cast<XmlNode>()
            .Where(x => x != null && x.Attributes != null)
            .Where(x => x.Name == "team");

        foreach (var teamNode in teams)
        {
            var team = new Team(
                teamNode.Attributes["name"].Value,
                Color.FromArgb(
                    255,
                    byte.Parse(teamNode.Attributes["red"].Value),
                    byte.Parse(teamNode.Attributes["green"].Value),
                    byte.Parse(teamNode.Attributes["blue"].Value)
                )
            )
            {
                Parent = parent
            };
            this.teams.Add(team);

            var spawnpoints = teamNode.ChildNodes
                .Cast<XmlNode>()
                .Where(x => x != null && x.Attributes != null)
                .Where(x => x.Name == "spawnpoint");

            foreach (var spawnpointNode in spawnpoints)
            {
                var weapons = spawnpointNode.ChildNodes
                    .Cast<XmlNode>()
                    .Where(x => x != null && x.Attributes != null)
                    .Where(x => x.Name == "weapon")
                    .ToDictionary(
                        x => Enum.Parse<WeaponId>(x.Attributes["type"].Value.Replace(" ", ""), true),
                        x => ushort.Parse(x.Attributes["ammo"].Value)
                    );

                this.spawnPoints.Add(new SpawnPoint(
                    new Vector3(
                        float.Parse(spawnpointNode.Attributes["x"].Value, CultureInfo.InvariantCulture),
                        float.Parse(spawnpointNode.Attributes["y"].Value, CultureInfo.InvariantCulture),
                        float.Parse(spawnpointNode.Attributes["z"].Value, CultureInfo.InvariantCulture)
                    ), float.Parse(spawnpointNode.Attributes["rotation"].Value, CultureInfo.InvariantCulture),
                    team, weapons
                ));
            }
        }

        var objects = root.ChildNodes
            .Cast<XmlNode>()
            .Where(x => x != null && x.Attributes != null)
            .Where(x => x.Name == "object");

        foreach (var objectNode in objects)
        {
            this.objects.Add(new WorldObject(
                    ushort.Parse(objectNode.Attributes["model"].Value),
                    new Vector3(
                        float.Parse(objectNode.Attributes["posX"].Value, CultureInfo.InvariantCulture),
                        float.Parse(objectNode.Attributes["posY"].Value, CultureInfo.InvariantCulture),
                        float.Parse(objectNode.Attributes["posZ"].Value, CultureInfo.InvariantCulture)
                    )
                )
            {
                Parent = parent,
                Rotation = new Vector3(
                        float.Parse(objectNode.Attributes["rotX"].Value, CultureInfo.InvariantCulture),
                        float.Parse(objectNode.Attributes["rotY"].Value, CultureInfo.InvariantCulture),
                        float.Parse(objectNode.Attributes["rotZ"].Value, CultureInfo.InvariantCulture)
                    )
            });
        }

        AssignElementIds();
    }

    private void AssignElementIds()
    {
        var generator = new MapIdGenerator(IdGeneratorConstants.MapIdStart, IdGeneratorConstants.MapIdStop);

        var elements = Array.Empty<Element>()
            .Concat(this.teams)
            .Concat(this.objects);

        foreach (var element in elements)
            element.Id = generator.GetId();
    }
}


