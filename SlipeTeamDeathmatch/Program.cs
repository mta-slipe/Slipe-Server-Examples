using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.IdGeneration;
using SlipeServer.Server.Loggers;
using SlipeServer.Server.PacketHandling.Handlers.Middleware;
using SlipeServer.Server.Repositories;
using SlipeServer.Server.ServerBuilders;
using SlipeTeamDeathmatch;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.IdGenerators;
using SlipeTeamDeathmatch.Logic;
using SlipeTeamDeathmatch.Middleware;
using SlipeTeamDeathmatch.Services;

var server = new MtaServer<TdmPlayer>(builder =>
{
    builder.UseConfiguration(TdmConfiguration.Config);
    builder.AddDefaults();

    builder.AddLogic<MatchLogic>();

    builder.ConfigureServices(services =>
    {
        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddSingleton(typeof(ISyncHandlerMiddleware<>), typeof(MatchMiddleware<>));

        services.AddSingleton<IElementIdGenerator, PlayerIdGenerator>(x =>
            new PlayerIdGenerator(x.GetRequiredService<IElementRepository>(), IdGeneratorConstants.PlayerIdStart, IdGeneratorConstants.PlayerIdStop)
        );

        services.AddSingleton<MapService>(x => new MapService(x.GetRequiredService<RootElement>(), "Maps"));

#if DEBUG
        builder.AddNetWrapper(dllPath: "net_d", port: 50667);
#endif
    });
})
{
    GameType = "Slipe:TDM"
};

server.Start();
await Task.Delay(-1);
