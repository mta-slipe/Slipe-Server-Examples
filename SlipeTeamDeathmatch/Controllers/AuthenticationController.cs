using SlipeServer.LuaControllers;
using SlipeServer.LuaControllers.Attributes;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.LuaValues;
using SlipeTeamDeathmatch.Services;

namespace SlipeTeamDeathmatch.Controllers;

[LuaController("Slipe.TeamDeathMatch.")]
public class AuthenticationController : BaseLuaController<TdmPlayer>
{
    private readonly AccountService accountService;

    public AuthenticationController(
        AccountService accountService)
    {
        this.accountService = accountService;
    }

    public async void Login(string username, string password)
    {
        await this.accountService.LoginAsync(this.Context.Player, username, password);
    }

    public async void Logout()
    {
        await this.accountService.LogoutAsync(this.Context.Player);
    }

    public async void Register(string username, string password)
    {
        await this.accountService.RegisterAsync(this.Context.Player, username, password);
    }
}
