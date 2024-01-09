using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Persistence.Repository;

public interface IBaseRepository<T> where T : class
{
	Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		List<Expression<Func<T, object>>>? includes = null,
		bool disableTracking = false);

	IQueryable<T> Get(Expression<Func<T, bool>>? predicate = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		List<Expression<Func<T, object>>>? includes = null,
		bool disableTracking = false);

	Task<T?> GetByIdAsync(object id);

	Task<T> AddAsync(T entity);

	Task UpdateAsync(T entity);

	Task DeleteAsync(T entity);

	Task AddRange(IEnumerable<T> entities);

	Task DeleteRange(IEnumerable<T> entities);

	Task DeleteAsync(object id);

	bool Any();
	Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
	bool IsExisted(Expression<Func<T, bool>>? predicate = null);
}
