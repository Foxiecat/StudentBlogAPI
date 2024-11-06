using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Comments.Interfaces;
using StudentBlogAPI.Features.Comments.Models;
using StudentBlogAPI.Features.Comments.Models.Responses;

namespace StudentBlogAPI.Features.Comments;

public class CommentService(ILogger<CommentService> logger,
                            IMapper<Comment, CommentResponse> commentMapper,
                            IMapper<Comment, AddCommentResponse> createCommentMapper,
                            ICommentRepository commentRepository,
                            IHttpContextAccessor httpContextAccessor) : ICommentService
{
    public async Task<CommentResponse?> AddCommentAsync(Guid postId, AddCommentResponse addCommentResponse)
    {
        Comment comment = createCommentMapper.MapToModel(addCommentResponse);
        string? loggedInUserId = httpContextAccessor.HttpContext?.Items["UserId"] as string;
        
        if (loggedInUserId == null)
        {
            logger.LogWarning("UserId {UserId} not found", loggedInUserId);
            return null;
        }
        
        Guid userId = Guid.Parse(loggedInUserId);
        
        comment.Id = Guid.NewGuid();
        comment.PostId = postId;
        comment.UserId = userId;
        comment.Content = addCommentResponse.Content;
        comment.DateCommented = DateTime.UtcNow;

        Comment? commentAdded = await commentRepository.AddAsync(comment);
        
        return commentAdded is null
            ? null
            : commentMapper.MapToDTO(commentAdded);
    }
    
    public Task<CommentResponse?> AddAsync(CommentResponse model)
    {
        throw new NotImplementedException();
    }

    public Task<CommentResponse?> UpdateAsync(CommentResponse user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<CommentResponse?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CommentResponse>> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CommentResponse>> FindAsync()
    {
        throw new NotImplementedException();
    }
}