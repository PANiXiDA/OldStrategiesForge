using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;
using Common.SearchParams.Core;

namespace BaseDAL;

public abstract class BaseDAL<TDbContext, TDbObject, TEntity, TObjectId, TSearchParams, TConvertParams> : IBaseDal<TDbContext, TDbObject, TEntity, TObjectId, TSearchParams, TConvertParams>
      where TDbContext : DbContext, new()
      where TDbObject : class, new()
      where TEntity : class
      where TObjectId : notnull
      where TSearchParams : BaseSearchParams
      where TConvertParams : class
{
    protected TDbContext _context;

    protected abstract bool RequiresUpdatesAfterObjectSaving { get; }

    protected internal BaseDAL(TDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual Task<TObjectId> AddOrUpdateAsync(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        return AddOrUpdateAsync(entity, true);
    }

    protected virtual async Task<TObjectId> AddOrUpdateAsync(TEntity entity, bool forceSave)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var data = GetContext() ?? throw new InvalidOperationException("Context is not available");
        try
        {
            var objects = data.Set<TDbObject>();
            var dbObject = await objects.FirstOrDefaultAsync(GetCheckDbObjectIdExpression(GetIdByEntity(entity)));
            var exists = dbObject != null;
            dbObject ??= new TDbObject();

            await UpdateBeforeSavingAsync(data, entity, dbObject, exists);

            if (!exists)
            {
                objects.Add(dbObject);
            }

            if (RequiresUpdatesAfterObjectSaving || forceSave)
            {
                await data.SaveChangesAsync();
            }

            if (RequiresUpdatesAfterObjectSaving)
            {
                await UpdateAfterSavingAsync(data, entity, dbObject, exists);

                if (forceSave)
                {
                    await data.SaveChangesAsync();
                }
            }

            return GetIdByDbObject(dbObject);
        }
        finally
        {
            await DisposeContextAsync(data);
        }
    }

    public virtual Task<IList<TObjectId>> AddOrUpdateAsync(IList<TEntity> entities)
    {
        if (entities == null || !entities.Any()) throw new ArgumentNullException(nameof(entities));
        return AddOrUpdateAsync(entities, true);
    }

    protected virtual async Task<IList<TObjectId>> AddOrUpdateAsync(IList<TEntity> entities, bool forceSave)
    {
        if (entities == null || !entities.Any()) throw new ArgumentNullException(nameof(entities));

        var data = GetContext() ?? throw new InvalidOperationException("Context is not available");
        try
        {
            var entitiesIdArray = entities.Select(GetIdByEntity).ToArray();
            var dbSet = data.Set<TDbObject>();
            var dbObjectsDictionary = await dbSet.Where(GetCheckDbObjectIdInArrayExpression(entitiesIdArray)).ToDictionaryAsync(GetIdByDbObjectExpression().Compile());

            var existingSet = new HashSet<TObjectId>();
            var dbObjects = new List<TDbObject>();
            var addedObjects = new List<TDbObject>();

            foreach (var entity in entities)
            {
                var id = GetIdByEntity(entity);
                var exists = dbObjectsDictionary.TryGetValue(id, out var dbObject);
                dbObject ??= new TDbObject();

                if (exists)
                    existingSet.Add(id);

                await UpdateBeforeSavingAsync(data, entity, dbObject, exists);
                dbObjects.Add(dbObject);

                if (!exists)
                {
                    addedObjects.Add(dbObject);
                }
            }

            dbSet.AddRange(addedObjects);

            if (RequiresUpdatesAfterObjectSaving || forceSave)
            {
                await data.SaveChangesAsync();
            }

            if (RequiresUpdatesAfterObjectSaving)
            {
                for (var i = 0; i < dbObjects.Count; i++)
                {
                    var dbObject = dbObjects[i];
                    var id = GetIdByDbObject(dbObject);
                    await UpdateAfterSavingAsync(data, entities[i], dbObject, existingSet.Contains(id));
                }
            }

            if (forceSave)
            {
                await data.SaveChangesAsync();
            }

            return dbObjects.Select(GetIdByDbObject).ToList();
        }
        finally
        {
            await DisposeContextAsync(data);
        }
    }

    public virtual async Task<TEntity> GetAsync(TObjectId id, TConvertParams? convertParams = null)
    {
        var data = GetContext() ?? throw new InvalidOperationException("Context is not available");
        try
        {
            var dbObjects = data.Set<TDbObject>().Where(GetCheckDbObjectIdExpression(id)).Take(1);
            return (await BuildEntitiesListAsync(data, dbObjects, convertParams, true)).FirstOrDefault()
                   ?? throw new InvalidOperationException("Entity not found.");
        }
        finally
        {
            await DisposeContextAsync(data);
        }
    }

    public virtual Task<bool> ExistsAsync(TObjectId id)
    {
        return ExistsAsync(GetCheckDbObjectIdExpression(id));
    }

    public virtual async Task<bool> ExistsAsync(TSearchParams searchParams)
    {
        if (searchParams == null) throw new ArgumentNullException(nameof(searchParams));

        var data = GetContext() ?? throw new InvalidOperationException("Context is not available");
        try
        {
            var objects = data.Set<TDbObject>().AsNoTracking();
            return await BuildDbQuery(data, objects, searchParams).AnyAsync();
        }
        finally
        {
            await DisposeContextAsync(data);
        }
    }

    protected virtual async Task<bool> ExistsAsync(Expression<Func<TDbObject, bool>> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        var data = GetContext() ?? throw new InvalidOperationException("Context is not available");
        try
        {
            return await data.Set<TDbObject>().Where(predicate).AnyAsync();
        }
        finally
        {
            await DisposeContextAsync(data);
        }
    }

    public virtual Task<bool> DeleteAsync(TObjectId id)
    {
        return DeleteAsync(GetCheckDbObjectIdExpression(id));
    }

    protected virtual async Task<bool> DeleteAsync(Expression<Func<TDbObject, bool>> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        var data = GetContext() ?? throw new InvalidOperationException("Context is not available");
        try
        {
            return await data.Set<TDbObject>().Where(predicate).DeleteAsync() > 0;
        }
        finally
        {
            await DisposeContextAsync(data);
        }
    }

    public virtual async Task<SearchResult<TEntity>> GetAsync(TSearchParams searchParams, TConvertParams? convertParams = null)
    {
        if (searchParams == null) throw new ArgumentNullException(nameof(searchParams));

        var data = GetContext() ?? throw new InvalidOperationException("Context is not available");
        try
        {
            var objects = data.Set<TDbObject>().AsNoTracking();
            objects = BuildDbQuery(data, objects, searchParams);

            var visitor = new OrderedQueryableVisitor();
            visitor.Visit(objects.Expression);

            objects = visitor.IsOrdered
                ? (objects as IOrderedQueryable<TDbObject>)?.ThenBy(GetIdByDbObjectExpression()) ?? throw new InvalidOperationException("Failed to order objects.")
                : objects.OrderBy(GetIdByDbObjectExpression());

            var result = new SearchResult<TEntity>
            {
                Total = await objects.CountAsync(),
                Objects = new List<TEntity>(),
                RequestedObjectsCount = searchParams.ObjectsCount,
                RequestedStartIndex = searchParams.StartIndex,
            };

            if (searchParams.ObjectsCount == 0)
            {
                return result;
            }

            objects = objects.Skip(searchParams.StartIndex);
            if (searchParams.ObjectsCount.HasValue)
                objects = objects.Take(searchParams.ObjectsCount.Value);

            result.Objects = await BuildEntitiesListAsync(data, objects, convertParams, false);
            return result;
        }
        finally
        {
            await DisposeContextAsync(data);
        }
    }

    protected virtual async Task<IList<TEntity>> GetAsync(Expression<Func<TDbObject, bool>> predicate, TConvertParams? convertParams = null)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        var data = GetContext() ?? throw new InvalidOperationException("Context is not available");
        try
        {
            return await BuildEntitiesListAsync(data, data.Set<TDbObject>().Where(predicate), convertParams, false);
        }
        finally
        {
            await DisposeContextAsync(data);
        }
    }

    protected abstract Task UpdateBeforeSavingAsync(TDbContext context, TEntity entity, TDbObject dbObject, bool exists);

    protected virtual Task UpdateAfterSavingAsync(TDbContext context, TEntity entity, TDbObject dbObject, bool exists)
    {
        return Task.CompletedTask;
    }

    protected abstract IQueryable<TDbObject> BuildDbQuery(TDbContext context, IQueryable<TDbObject> dbObjects, TSearchParams searchParams);

    protected abstract Task<IList<TEntity>> BuildEntitiesListAsync(TDbContext context, IQueryable<TDbObject> dbObjects, TConvertParams? convertParams, bool isFull);

    protected abstract Expression<Func<TDbObject, TObjectId>> GetIdByDbObjectExpression();

    protected MemberInfo? GetDbObjectIdMember()
    {
        return (GetIdByDbObjectExpression().Body as MemberExpression)?.Member;
    }

    protected TObjectId GetIdByDbObject(TDbObject dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));
        return GetIdByDbObjectExpression().Compile()(dbObject);
    }

    protected Expression<Func<TDbObject, bool>> GetCheckDbObjectIdExpression(TObjectId objectId)
    {
        var memberInfo = GetDbObjectIdMember() ?? throw new InvalidOperationException("ID member not found.");
        var p = Expression.Parameter(typeof(TDbObject));
        return Expression.Lambda<Func<TDbObject, bool>>(
            Expression.Equal(
                Expression.MakeMemberAccess(p, memberInfo),
                Expression.Constant(objectId)
            ), p);
    }

    protected Expression<Func<TDbObject, bool>> GetCheckDbObjectIdInArrayExpression(TObjectId[] arr)
    {
        if (arr == null) throw new ArgumentNullException(nameof(arr));

        var memberInfo = GetDbObjectIdMember() ?? throw new InvalidOperationException("ID member not found.");
        var p = Expression.Parameter(typeof(TDbObject));

        return Expression.Lambda<Func<TDbObject, bool>>(
            Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.Contains),
                new[] { typeof(TObjectId) },
                Expression.Constant(arr),
                Expression.MakeMemberAccess(p, memberInfo)
            ), p);
    }

    protected abstract Expression<Func<TEntity, TObjectId>> GetIdByEntityExpression();

    protected TObjectId GetIdByEntity(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        return GetIdByEntityExpression().Compile()(entity);
    }

    protected TDbContext GetContext()
    {
        return _context ?? new TDbContext();
    }

    protected async Task<bool> DisposeContextAsync(TDbContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (ReferenceEquals(context, _context)) return false;
        await context.DisposeAsync();
        return true;
    }
}