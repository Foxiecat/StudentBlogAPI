using StudentBlogAPI.Features.Comments.DTO;
using StudentBlogAPI.Features.Common.Interfaces;

namespace StudentBlogAPI.Features.Comments.Interfaces;

public interface ICommentService : IService<CommentResponse>
{
    Task<CommentResponse?> AddCommentAsync(Guid postId, CommentRequest commentRequest);
    public Task<IEnumerable<CommentResponse>> GetAllFromPostIdAsync(Guid id);
}