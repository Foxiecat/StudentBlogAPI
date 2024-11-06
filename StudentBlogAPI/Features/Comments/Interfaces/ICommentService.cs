using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Comments.Models.Responses;

namespace StudentBlogAPI.Features.Comments.Interfaces;

public interface ICommentService : IService<CommentResponse>
{
    Task<CommentResponse?> AddCommentAsync(Guid postId, AddCommentResponse addCommentResponse);
    Task<IEnumerable<CommentResponse>> FindAsync();
}