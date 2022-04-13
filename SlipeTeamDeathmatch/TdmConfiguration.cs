using SlipeServer.Server;

namespace SlipeTeamDeathmatch;

public static class TdmConfiguration
{
    public static Configuration Config { get; } = new Configuration()
    {
        ServerName = "Slipe Team Death Match Example Server",
    };
}
