using System.Linq.Expressions;

namespace StudentBlogAPI.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> AddAsync(T model);
    Task<T?> UpdateAsync(T model);
    Task<T?> DeleteByIdAsync(Guid id);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}