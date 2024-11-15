using System.Linq.Expressions;

namespace StudentBlogAPI.Features.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> AddAsync(T model);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> UpdateAsync(T model);
    Task<T?> DeleteByIdAsync(Guid id);
}