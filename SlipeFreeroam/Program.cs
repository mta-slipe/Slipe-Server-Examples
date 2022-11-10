using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SlipeFreeroam;
using SlipeFreeroam.Elements;
using SlipeFreeroam.Logic;
using SlipeFreeroam.Services;
using SlipeServer.LuaControllers;
using SlipeServer.Server;
using SlipeServer.Server.Behaviour;
using SlipeServer.Server.Loggers;
using SlipeServer.Server.ServerBuilders;

var server = MtaServer.CreateWithDiSupport<FreeroamPlayer>(builder =>
{
    builder.UseConfiguration(FreeroamConfiguration.Config);

    var exceptBehaviours = ServerBuilderDefaultBehaviours.DefaultChatBehaviour;
#if DEBUG
    exceptBehaviours |= ServerBuilderDefaultBehaviours.MasterServerAnnouncementBehaviour;
    builder.AddBehaviour<EventLoggingBehaviour>();
#endif

    builder.AddDefaults(exceptBehaviours: exceptBehaviours);

    builder.ConfigureServices(services =>
    {
        services.AddSingleton<ClothingService>();
        services.AddSingleton<FreeroamClientSettings>();
        services.AddSingleton<ILogger, ConsoleLogger>();
    });

    builder.AddLogic<FreeroamResourceLogic>();
    builder.AddLogic<SpawnLogic>();
    builder.AddLogic<ChatLogic>();

    builder.AddLuaControllers();
});

server.GameType = "Slipe:Freeroam";
server.Start();
server.GetRequiredService<ILogger>().LogInformation("Server started.");

await Task.Delay(-1);
