using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SlipeServer.LuaControllers;
using SlipeServer.Packets.Definitions.Sync;
using SlipeServer.Resources.Reload;
using SlipeServer.Server;
using SlipeServer.Server.ElementCollections;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.IdGeneration;
using SlipeServer.Server.Loggers;
using SlipeServer.Server.PacketHandling.Handlers.Middleware;
using SlipeServer.Server.ServerBuilders;
using SlipeTeamDeathmatch;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.IdGenerators;
using SlipeTeamDeathmatch.Logic;
using SlipeTeamDeathmatch.Middleware;
using SlipeTeamDeathmatch.Persistence;
using SlipeTeamDeathmatch.Persistence.Repositories;
using SlipeTeamDeathmatch.Services;

// this is neccesary for docker support
Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!);

var server = MtaServer.Create<TdmPlayer>(builder =>
{
    builder.UseConfiguration(TdmConfiguration.Config);
    builder.AddDefaults(
        exceptBehaviours: 
            ServerBuilderDefaultBehaviours.DefaultChatBehaviour | 
            ServerBuilderDefaultBehaviours.LightSyncBehaviour
        , 
        exceptMiddleware: 
            ServerBuilderDefaultMiddleware.PlayerPureSyncPacketMiddleware | 
            ServerBuilderDefaultMiddleware.KeySyncPacketMiddleware |
            ServerBuilderDefaultMiddleware.ProjectileSyncPacketMiddleware
    );

    builder.AddLuaControllers();

    builder.AddLogic<TdmResourceLogic>();
    builder.AddLogic<ChatLogic>();

    builder.AddReloadResource();

    builder.ConfigureServices(services =>
    {
        services.AddDbContext<TdmContext>();
        services.AddTransient(typeof(IDbRepository<>), typeof(DbRepository<>));

        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddSingleton(typeof(ISyncHandlerMiddleware<>), typeof(MatchMiddleware<>));

        services.AddSingleton<IElementIdGenerator, PlayerIdGenerator>(x =>
            new PlayerIdGenerator(x.GetRequiredService<IElementCollection>(), IdGeneratorConstants.PlayerIdStart, IdGeneratorConstants.PlayerIdStop)
        );

        services.AddTransient<IPasswordService, PasswordService>();
        services.AddTransient<AccountService>();
        services.AddSingleton<MapService>(x => new MapService(x.GetRequiredService<RootElement>(), "Maps"));
        services.AddSingleton<MatchService>();

#if DEBUG
        // there is no debug net dll for x64
        if (!Environment.Is64BitProcess)
            builder.AddNetWrapper(dllPath: "net_d", port: 50667);
#endif
    });
});
server.GameType = "Slipe:TDM";
server.Start();
await Task.Delay(-1);