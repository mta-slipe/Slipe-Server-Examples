using SlipeServer.Server;

namespace SlipeFreeroam;

public static class FreeroamConfiguration
{
    public static Configuration Config { get; } = new Configuration()
    {
        ServerName = "Slipe Freeroam Server",
    };
}
