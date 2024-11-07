using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StudentBlogAPI.Database;
using StudentBlogAPI.Features.Comments.Interfaces;

namespace StudentBlogAPI.Features.Comments;

public class CommentRepository(ILogger<CommentRepository> logger, StudentBlogDbContext dbContext) : ICommentRepository
{
    public async Task<Comment?> AddAsync(Comment comment)
    {
        bool checkIfAnyExists = dbContext.Comments.Any(c => c.Id == comment.Id);
        if (checkIfAnyExists)
        {
            logger.LogWarning("Comment with id: {Id} already exists.", comment.Id);
            return null;
        }
        
        await dbContext.Comments.AddAsync(comment);
        await dbContext.SaveChangesAsync();

        return comment;
    }

    public async Task<IEnumerable<Comment>> GetAllFromPostIdAsync(Guid id)
    {
        List<Comment> comments = await dbContext.Comments
            .OrderByDescending(c => c.DateCommented)
            .Where(c => c.PostId == id)
            .ToListAsync();

        return comments;
    }

    public async Task<IEnumerable<Comment>> GetPagedAsync(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        List<Comment> comments = await dbContext.Comments
            .OrderBy(comment => comment.Content)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return comments;
    }

    public async Task<Comment?> UpdateAsync(Comment comment)
    {
        Comment? commentToUpdate = await dbContext.Comments.FindAsync(comment.Id);

        if (commentToUpdate is null)
        {
            logger.LogWarning("Comment with id: {Id} not found.", comment.Id);
            return null;
        }
        
        commentToUpdate.Content = comment.Content;
        await dbContext.SaveChangesAsync();
        
        return commentToUpdate;
    }

    public async Task<Comment?> DeleteByIdAsync(Guid id)
    {
        Comment? commentToDelete = await dbContext.Comments.FindAsync(id);

        await dbContext.Comments
            .Where(comment => comment.Id == id)
            .ExecuteDeleteAsync();
        
        await dbContext.SaveChangesAsync();
        return commentToDelete;
    }
    
    // Unused
    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        return await dbContext.Comments
            .FirstOrDefaultAsync(comment => comment.Id == id);
    }
    
    // Unused
    public Task<IEnumerable<Comment>> FindAsync(Expression<Func<Comment, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}