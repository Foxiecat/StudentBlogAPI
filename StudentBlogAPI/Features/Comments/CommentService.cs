using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Comments.DTOs;
using StudentBlogAPI.Features.Comments.Interfaces;

namespace StudentBlogAPI.Features.Comments;

public class CommentService(ILogger<CommentService> logger,
                            IMapper<Comment, CommentDTO> commentMapper,
                            ICommentRepository commentRepository,
                            IHttpContextAccessor httpContextAccessor) : ICommentService
{
    public Task<CommentDTO> CreateCommentAsync(Guid postId, string content)
    {
        throw new NotImplementedException();
    }
    
    public Task<CommentDTO?> AddAsync(CommentDTO model)
    {
        throw new NotImplementedException();
    }

    public Task<CommentDTO?> UpdateAsync(CommentDTO user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<CommentDTO?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CommentDTO>> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CommentDTO>> FindAsync()
    {
        throw new NotImplementedException();
    }
}