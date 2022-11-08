using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SlipeFreeroam.Elements;
using SlipeServer.Packets.Definitions.Entities.Structs;
using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Enums;
using SlipeServer.Server.Mappers;

namespace SlipeFreeroam.Services;

public struct PlayerClothingArticle
{
    public string model;
    public string texture;
}

public struct ClothingArticle : ILuaMappable
{
    public int id;
    public string model;
    public string texture;

    public LuaValue ToLuaValue()
    {
        return new Dictionary<LuaValue, LuaValue>()
        {
            ["id"] = this.id,
            ["model"] = this.model,
            ["texture"] = this.texture,
        };
    }
}

public struct ClothingGroup : ILuaMappable
{
    [JsonProperty("1")]
    public string group;
    public int type;
    public string name;
    public ClothingArticle[] children;

    public LuaValue ToLuaValue()
    {
        return new Dictionary<LuaValue, LuaValue>()
        {
            [1] = this.group,
            ["type"] = this.type,
            ["name"] = this.name,
            ["children"] = this.children.Select(x => x.ToLuaValue()).ToArray()
        };
    }
}

public struct ClothingData
{
    public Dictionary<int, PlayerClothingArticle> playerClothes;
    public IEnumerable<ClothingGroup> allClothes;
}


public class ClothingService
{
    private IEnumerable<ClothingGroup> allClothes;

    public ClothingService()
    {
        this.allClothes = Array.Empty<ClothingGroup>();

        this.LoadClothingGroups();
    }

    public void AddClothingToPlayer(FreeroamPlayer player, string model, string texture)
    {
        var clothing = this.GetClothingId(model, texture);
        if (clothing == null)
            return;

        player.Clothing.AddClothing(new PedClothing()
        {
            Type = (byte)clothing.Value.type,
            Texture = texture,
            Model = model
        });
    }

    public void RemoveClothingFromPlayer(FreeroamPlayer player, ClothingType type)
    {
        player.Clothing.RemoveClothing(type);
    }

    public ClothingData GetClothingData(FreeroamPlayer player)
    {
        return new ClothingData()
        {
            playerClothes = GetPlayerClothes(player),
            allClothes = this.allClothes,
        };
    }

    public (ClothingType type, int id)? GetClothingId(string model, string texture)
    {
        foreach (var group in this.allClothes)
            foreach (var item in group.children)
                if (item.model == model && item.texture == texture)
                    return ((ClothingType)group.type, item.id);

        return null;
    }

    private Dictionary<int, PlayerClothingArticle> GetPlayerClothes(FreeroamPlayer player)
    {
        var dictionary = new Dictionary<int, PlayerClothingArticle>();

        foreach (var clothing in player.Clothing.GetClothing())
        {
            dictionary[clothing.Type] = new PlayerClothingArticle()
            {
                texture = clothing.Texture,
                model = clothing.Model
            };
        }

        return dictionary;
    }

    private void LoadClothingGroups()
    {
        var file = GetEmbeddedFile("SlipeFreeroam.Data.clothes.json");
        var text = Encoding.UTF8.GetString(file).Trim(new char[] { '\uFEFF', '\u200B' });
        var clothes = JsonConvert.DeserializeObject<ClothingGroup[]>(text);

        if (clothes != null)
            this.allClothes = clothes;
    }

    private static byte[] GetEmbeddedFile(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(name);

        if (stream == null)
            throw new FileNotFoundException($"File \"{name}\" not found in embedded resources.");

        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        return buffer;
    }
}
