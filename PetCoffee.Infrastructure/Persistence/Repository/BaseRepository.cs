using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using System.Linq.Expressions;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly DbContext _dbContext;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<T> AddAsync(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbContext.Set<T>().Attach(entity);
        }
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task AddRange(IEnumerable<T> entities)
    {
        var listEntities = entities.ToList();
        listEntities.ForEach(entity =>
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbContext.Set<T>().Attach(entity);
            }
        });
        await _dbContext.Set<T>().AddRangeAsync(listEntities);
    }

    public bool Any()
    {
        return _dbContext.Set<T>().Any();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        return await query.CountAsync();
    }

    public Task DeleteAsync(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbContext.Set<T>().Attach(entity);
        }

        _dbContext.Set<T>().Remove(entity);

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(object id)
    {
        var entityToDelete = await _dbContext.Set<T>().FindAsync(id);
        if (entityToDelete != null)
        {
            await DeleteAsync(entityToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }

    public Task DeleteRange(IEnumerable<T> entities)
    {
        var listEntities = entities.ToList();
        listEntities.ForEach(entity =>
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbContext.Set<T>().Attach(entity);
            }
        });

        _dbContext.Set<T>().RemoveRange(listEntities);

        return Task.CompletedTask;
    }

    public IQueryable<T> Get(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool disableTracking = false)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        return query;
    }

    public Task<IQueryable<T>> GetAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        List<Expression<Func<T, object>>>? includes = null,
        bool disableTracking = false)
    {
        return Task.FromResult(Get(predicate, orderBy, includes, disableTracking));
    }

    public async Task<T?> GetByIdAsync(object id)
    {

        return await _dbContext.Set<T>().FindAsync(id);
    }

    public bool IsExisted(Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        return query.Any();
    }

    public Task UpdateAsync(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbContext.Set<T>().Attach(entity);
        }

        _dbContext.Entry(entity).State = EntityState.Modified;

        _dbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }
}
