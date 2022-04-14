using System.Linq.Expressions;

namespace SlipeTeamDeathmatch.Persistence.Repositories;

public interface IDbRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity?> GetAsync(uint id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter);
    Task CreateAync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
}