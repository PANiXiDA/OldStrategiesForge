using Common.SearchParams.Core;
using Microsoft.EntityFrameworkCore;

namespace BaseDAL;

public interface IBaseDal<TDbContext, TDbObject, TEntity, TObjectId, TSearchParams, TConvertParams>
    where TDbContext : DbContext
    where TDbObject : class, new()
    where TEntity : class
    where TSearchParams : BaseSearchParams
    where TConvertParams : class
{
    Task<TObjectId> AddOrUpdateAsync(TEntity entity);

    Task<IList<TObjectId>> AddOrUpdateAsync(IList<TEntity> entity);

    Task<bool> ExistsAsync(TObjectId id);

    Task<bool> ExistsAsync(TSearchParams searchParams);

    Task<TEntity> GetAsync(TObjectId id, TConvertParams? convertParams = null);

    Task<SearchResult<TEntity>> GetAsync(TSearchParams searchParams, TConvertParams? convertParams = null);

    Task<bool> DeleteAsync(TObjectId id);
}
