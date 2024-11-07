using StudentBlogAPI.Features.Common.Interfaces;

namespace StudentBlogAPI.Features.Comments.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    public Task<IEnumerable<Comment>> GetAllFromPostIdAsync(Guid id);
}