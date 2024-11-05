using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Comments.DTOs;

namespace StudentBlogAPI.Features.Comments.Interfaces;

public interface ICommentService : IService<CommentDTO>
{
    Task<CommentDTO> CreateCommentAsync(Guid postId, string content);
    Task<IEnumerable<CommentDTO>> FindAsync();
}