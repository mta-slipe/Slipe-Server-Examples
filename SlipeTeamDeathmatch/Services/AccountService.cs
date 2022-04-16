using Microsoft.Extensions.Logging;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Events;
using SlipeTeamDeathmatch.Elements;
using SlipeTeamDeathmatch.Models;
using SlipeTeamDeathmatch.Persistence.Repositories;

namespace SlipeTeamDeathmatch.Services;
public class AccountService
{
    private readonly ILogger logger;
    private readonly IDbRepository<Account> accountRepository;
    private readonly IPasswordService passwordService;

    public AccountService(
        MtaServer<TdmPlayer> mtaServer, 
        ILogger logger,
        IDbRepository<Account> accountRepository,
        IPasswordService passwordService
    )
    {
        this.logger = logger;
        this.accountRepository = accountRepository;
        this.passwordService = passwordService;

        mtaServer.PlayerJoined += HandlePlayerJoined;

        // call this to "wake" entity framework so the first request does not take long.
        this.accountRepository.GetAsync(0);
    }

    private void HandlePlayerJoined(TdmPlayer player)
    {
        player.Disconnected += HandlePlayerDisconnect;
    }

    private void HandlePlayerDisconnect(Player sender, PlayerQuitEventArgs e)
    {
        Task.Run(async () =>
        {
            var player = (TdmPlayer)sender;
            await LogoutAsync(player);

            player.Disconnected -= HandlePlayerDisconnect;
        });
    }

    public async Task<Account?> RegisterAsync(TdmPlayer player, string username, string password)
    {
        if (!player.Account.IsGuest)
        {
            player.SendErrorMessage("You are already logged in.");
            return null;
        }

        if (await this.accountRepository.GetAsync(x => x.Name == username) != null)
        {
            player.SendErrorMessage("This username is already in use.");
            return null;
        }

        var account = new Account(username, this.passwordService.Hash(password))
        {
            MatchCount = player.Account.MatchCount,
            DeathCount = player.Account.DeathCount,
            KillCount = player.Account.KillCount,
        };

        await this.accountRepository.CreateAync(account);

        this.logger.LogInformation("\"{player}\" has registered account \"{account}\"", player.Name, username);
        player.Account = account;
        player.SendLoggedIn();

        return account;
    }

    public async Task<Account?> LoginAsync(TdmPlayer player, string username, string password)
    {
        if (!player.Account.IsGuest)
        {
            player.SendErrorMessage("You are already logged in.");
            return null;
        }

        var account = await this.accountRepository.GetAsync(x => x.Name == username);
        if (account == null)
        {
            player.SendErrorMessage("These credentials do not match our records.");
            return null;
        }

        if (!this.passwordService.Matches(password, account.Password))
        {
            player.SendErrorMessage("These credentials do not match our records.");
            return null;
        }

        this.logger.LogInformation("\"{player}\" has logged into account \"{account}\"", player.Name, username);
        player.Account = account;
        player.SendLoggedIn();

        return account;
    }

    public async Task LogoutAsync(TdmPlayer player)
    {
        if (player.Account.IsGuest)
            return;

        await this.accountRepository.UpdateAsync(player.Account);
        this.logger.LogInformation("\"{player}\" has logged out of account \"{account}\"", player.Name, player.Account.Name);
        player.Account = Account.CreateGuest();
        player.SendLoggedOut();
    }
}
