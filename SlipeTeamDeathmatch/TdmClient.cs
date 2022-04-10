using SlipeServer.Net.Wrappers;
using SlipeServer.Server;
using SlipeTeamDeathmatch.Elements;

namespace SlipeTeamDeathmatch;
public class TdmClient : Client
{
    public TdmClient(uint address, INetWrapper netWrapper) : base(address, netWrapper)
    {
        this.Player = new TdmPlayer(this);
    }
}