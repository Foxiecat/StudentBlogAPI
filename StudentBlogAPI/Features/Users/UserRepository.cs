using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StudentBlogAPI.Database;
using StudentBlogAPI.Features.Posts;
using StudentBlogAPI.Features.Users.Interfaces;

namespace StudentBlogAPI.Features.Users;

public class UserRepository(ILogger<UserRepository> logger, StudentBlogDbContext dbContext) : IUserRepository
{
    public async Task<User?> AddAsync(User model)
    {
        bool checkIfUserExists = dbContext.Users.Any(user => user.Id == model.Id);
        if (checkIfUserExists)
        {
            logger.LogWarning("User with ID {UserId} already exists.", model.Id);
            return null;
        }
        
        await dbContext.Users.AddAsync(model);
        await dbContext.SaveChangesAsync();
        
        return model;
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<IEnumerable<User>> GetPagedAsync(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;
        
        List<User> users = await dbContext.Users
            .OrderBy(user => user.UserName)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return users;
    }

    public async Task<IEnumerable<Post>> GetUserPostsAsync(Guid userId)
    {
        List<Post> posts = await dbContext.Posts
            .Where(post => post.UserId == userId)
            .ToListAsync();

        return posts;
    }

    public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
    {
        return await dbContext.Users
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<User?> UpdateAsync(User model)
    {
        User? existingUser = await dbContext.Users.FindAsync(model.Id);

        if (existingUser == null)
        {
            logger.LogWarning("Could not find user: {UserId}", model.Id);
            return null;
        }
        
        existingUser.FirstName = model.FirstName;
        existingUser.LastName = model.LastName;
        existingUser.UserName = model.UserName;
        existingUser.Email = model.Email;
        existingUser.Updated = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        return existingUser;
    }

    public async Task<User?> DeleteByIdAsync(Guid id)
    {
        User? deletedUser = await dbContext.Users.FindAsync(id);

        if (deletedUser is null)
        {
            logger.LogWarning("Could not find user: {UserId}, deletion aborted.", id);
            return null;
        }

        dbContext.Users.Remove(deletedUser);
        await dbContext.SaveChangesAsync();
        
        return deletedUser;
    }
}