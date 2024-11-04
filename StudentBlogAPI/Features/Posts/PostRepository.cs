using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StudentBlogAPI.Data;
using StudentBlogAPI.Features.Posts.Interfaces;

namespace StudentBlogAPI.Features.Posts;

public class PostRepository(ILogger<PostRepository> logger, StudentBlogDbContext dbContext) : IPostRepository
{
    public async Task<Post?> AddAsync(Post post)
    {
        bool checkIfPostExists = dbContext.Posts.Any(p => p.Id == post.Id);
        if (checkIfPostExists)
        {
            logger.LogWarning("Post with ID: {PostId} already exists.", post.Id);
            return null;
        }
        
        await dbContext.Posts.AddAsync(post);
        await dbContext.SaveChangesAsync();

        return post;
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        return await dbContext.Posts
            .FirstOrDefaultAsync(post => post.Id == id);
    }

    public async Task<IEnumerable<Post>> GetPagedAsync(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        List<Post> posts = await dbContext.Posts
            .OrderBy(post => post.Title)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return posts;
    }

    public async Task<IEnumerable<Post>> FindAsync(Expression<Func<Post, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<Post?> UpdateAsync(Post post)
    {
        Post? existingPost = await dbContext.Posts.FindAsync(post.Id);

        if (existingPost == null)
        {
            logger.LogWarning("Post with ID: {PostId} does not exist.", post.Id);
            return null;
        }
        
        existingPost.Title = post.Title;
        existingPost.Content = post.Content;
        
        await dbContext.SaveChangesAsync();
        return existingPost;
    }

    public async Task<Post?> DeleteByIdAsync(Guid id)
    {
        Post? deletingPost = await dbContext.Posts.FindAsync(id);

        await dbContext.Posts
            .Where(post => post.Id == id)
            .ExecuteDeleteAsync();
        
        await dbContext.SaveChangesAsync();
        return deletingPost;
    }
}