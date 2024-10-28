using System.Linq.Expressions;
using StudentBlogAPI.Data;
using StudentBlogAPI.Features.Users.Interfaces;

namespace StudentBlogAPI.Features.Users;

public class UserRepository(ILogger<UserRepository> logger, StudentBlogDbContext dbContext) : IUserRepository
{
    public async Task<User?> AddAsync(User model)
    {
        await dbContext.Users.AddAsync(model);
        await dbContext.SaveChangesAsync();
        
        return model;
    }

    public Task<User?> UpdateAsync(User model)
    {
        throw new NotImplementedException();
    }

    public Task<User?> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}