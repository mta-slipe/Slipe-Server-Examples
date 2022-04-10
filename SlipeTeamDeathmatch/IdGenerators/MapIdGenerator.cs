using SlipeServer.Server.Elements.IdGeneration;

namespace SlipeTeamDeathmatch.IdGenerators;

public class MapIdGenerator : IElementIdGenerator
{
    private readonly uint start;
    private readonly uint stop;
    private uint idCounter;

    public MapIdGenerator(uint start, uint stop)
    {
        this.idCounter = start;
        this.start = start;
        this.stop = stop;
    }

    public uint GetId()
    {
        this.idCounter++;
        if (this.idCounter > this.stop)
            this.idCounter = this.start;

        return this.idCounter;
    }
}
