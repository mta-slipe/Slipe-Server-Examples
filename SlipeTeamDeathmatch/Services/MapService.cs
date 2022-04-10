using SlipeServer.Server.Elements;
using SlipeTeamDeathmatch.Models;

namespace SlipeTeamDeathmatch.Services;
public class MapService
{
    private readonly RootElement root;
    private readonly string directory;

    private readonly HashSet<string> availableMaps;
    public IReadOnlyCollection<string> AvailableMaps => this.availableMaps.ToList().AsReadOnly();

    public MapService(RootElement root, string directory)
    {
        this.root = root;
        this.directory = directory;
        this.availableMaps = new();

        IndexMaps();
    }

    private void IndexMaps()
    {
        this.availableMaps.Clear();
        var files = Directory.GetFiles(this.directory, "*.map");
        foreach (var file in files)
            this.availableMaps.Add(Path.GetFileNameWithoutExtension(file));
    }

    public Map GetMap(string name)
    {
        if (!this.availableMaps.Contains(name))
            throw new NotSupportedException();

        var map = new Map(name);
        map.LoadFromFile(Path.Join(this.directory, $"{name}.map"), this.root);
        return map;
    }
}
