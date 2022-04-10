using SlipeServer.Server.Elements.IdGeneration;
using SlipeServer.Server.Repositories;

namespace SlipeTeamDeathmatch.IdGenerators;

public class PlayerIdGenerator : IElementIdGenerator
{
    private readonly IElementRepository elementRepository;
    private readonly uint start;
    private readonly uint stop;
    private uint idCounter;

    public PlayerIdGenerator(IElementRepository elementRepository, uint start, uint stop)
    {
        this.idCounter = start;
        this.elementRepository = elementRepository;
        this.start = start;
        this.stop = stop;
    }

    public uint GetId()
    {
        var start = this.idCounter;
        while (this.elementRepository.Get(this.idCounter) != null)
        {
            this.idCounter++;
            if (this.idCounter > this.stop)
                this.idCounter = this.start;
            if (this.idCounter == start)
                throw new ElementIdsExhaustedException();
        }

        return this.idCounter;
    }
}
