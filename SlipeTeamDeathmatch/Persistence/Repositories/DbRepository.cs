using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SlipeTeamDeathmatch.Persistence.Repositories;

public class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class, IEntity
{
    private readonly IServiceProvider serviceProvider;

    public DbRepository(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<TEntity?> GetAsync(uint id)
    {
        var context = this.serviceProvider.GetRequiredService<TdmContext>();
        return await context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
    {
        var context = this.serviceProvider.GetRequiredService<TdmContext>();
        return await context.Set<TEntity>().SingleOrDefaultAsync(filter);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
    {
        var context = this.serviceProvider.GetRequiredService<TdmContext>();
        return await context.Set<TEntity>().Where(filter).ToArrayAsync();
    }

    public Task CreateAync(TEntity entity)
    {
        var context = this.serviceProvider.GetRequiredService<TdmContext>();
        context.Set<TEntity>().Add(entity);
        return context.SaveChangesAsync();
    }

    public Task UpdateAsync(TEntity entity)
    {
        var context = this.serviceProvider.GetRequiredService<TdmContext>();
        context.Set<TEntity>().Attach(entity);
        context.Set<TEntity>().Update(entity);
        return context.SaveChangesAsync();
    }

    public Task DeleteAsync(TEntity entity)
    {
        var context = this.serviceProvider.GetRequiredService<TdmContext>();
        context.Set<TEntity>().Attach(entity);
        context.Set<TEntity>().Remove(entity);
        return context.SaveChangesAsync();
    }
}
