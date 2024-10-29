namespace StudentBlogAPI.Common.Interfaces;


public interface IService<T> where T : class
{
    Task<T?> AddAsync(T model);
    Task<T?> UpdateAsync(T user);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
}