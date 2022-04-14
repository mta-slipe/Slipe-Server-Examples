using SlipeServer.Server.Events;
using SlipeServer.Server.Services;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.LuaValues;
using SlipeTeamDeathmatch.Services;

namespace SlipeTeamDeathmatch.Logic;

public class AuthenticationLogic
{
    private readonly LuaEventService luaEventService;
    private readonly AccountService accountService;

    public AuthenticationLogic(
        LuaEventService luaEventService,
        AccountService accountService)
    {
        this.luaEventService = luaEventService;
        this.accountService = accountService;

        luaEventService.AddEventHandler("Slipe.TeamDeathMatch.Login", HandleLoginRequest);
        luaEventService.AddEventHandler("Slipe.TeamDeathMatch.Logout", HandleLogoutRequest);
        luaEventService.AddEventHandler("Slipe.TeamDeathMatch.Register", HandleRegisterRequest);
    }

    private async void HandleLoginRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;
        var luaValue = new LoginLuaValue();
        luaValue.Parse(luaEvent.Parameters[0]);

        await this.accountService.LoginAsync(player, luaValue.Username, luaValue.Password);
    }

    private async void HandleLogoutRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;

        await this.accountService.LogoutAsync(player);
    }

    private async void HandleRegisterRequest(LuaEvent luaEvent)
    {
        var player = (luaEvent.Player as TdmPlayer)!;
        var luaValue = new RegisterLuaValue();
        luaValue.Parse(luaEvent.Parameters[0]);

        await this.accountService.RegisterAsync(player, luaValue.Username, luaValue.Password);
    }
}
