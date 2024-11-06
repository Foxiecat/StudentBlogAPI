using System.Linq.Expressions;
using StudentBlogAPI.Data;
using StudentBlogAPI.Features.Comments.Interfaces;
using StudentBlogAPI.Features.Comments.Models;

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

    public Task<Comment?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Comment>> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Comment>> FindAsync(Expression<Func<Comment, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<Comment?> UpdateAsync(Comment model)
    {
        throw new NotImplementedException();
    }

    public Task<Comment?> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}