using Common.SearchParams.Core;

namespace EmailService.BL.BL.Interfaces.Extensions
{
    public interface ICrudBL<TEntity, TSearchParams, TConvertParams>
        where TEntity : class
        where TSearchParams : class, new()
        where TConvertParams : class
    {
        Task<int> AddOrUpdateAsync(TEntity entity);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(TSearchParams searchParams);
        Task<TEntity> GetAsync(int id, TConvertParams? convertParams = null);
        Task<bool> DeleteAsync(int id);
        Task<SearchResult<TEntity>> GetAsync(TSearchParams searchParams, TConvertParams? convertParams = null);
    }
}
