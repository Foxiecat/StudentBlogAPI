using StudentBlogAPI.Features.Comments.DTO;
using StudentBlogAPI.Features.Comments.Interfaces;
using StudentBlogAPI.Features.Common.Interfaces;
using StudentBlogAPI.Features.Users;
using StudentBlogAPI.Features.Users.Interfaces;

namespace StudentBlogAPI.Features.Comments;

public class CommentService(ILogger<CommentService> logger,
                            IMapper<Comment, CommentResponse> commentMapper,
                            IMapper<Comment, CommentRequest> commentRequestMapper,
                            ICommentRepository commentRepository,
                            IUserRepository userRepository,
                            IHttpContextAccessor httpContextAccessor) : ICommentService
{
    public async Task<CommentResponse?> AddCommentAsync(Guid postId, CommentRequest commentRequest)
    {
        Comment comment = commentRequestMapper.MapToModel(commentRequest);
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
        comment.Content = commentRequest.Content;
        comment.DateCommented = DateTime.UtcNow;

        Comment? commentAdded = await commentRepository.AddAsync(comment);
        
        return commentAdded is null
            ? null
            : commentMapper.MapToResponse(commentAdded);
    }
    
    public async Task<CommentResponse?> AddAsync(CommentResponse model)
    {
        Comment comment = commentMapper.MapToModel(model);
        Comment? commentAdded = await commentRepository.AddAsync(comment);
        
        return commentAdded is null
            ? null
            : commentMapper.MapToResponse(commentAdded);
    }

    // Unused
    public async Task<CommentResponse?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            logger.LogWarning("Id {Id} not found", id);
            return null;
        }
        
        Comment? comment = await commentRepository.GetByIdAsync(id);

        return comment is null
            ? null
            : commentMapper.MapToResponse(comment);
    }

    public async Task<IEnumerable<CommentResponse>> GetPagedAsync(int pageNumber, int pageSize)
    {
        IEnumerable<Comment> comments = await commentRepository.GetPagedAsync(pageNumber, pageSize);
        
        return comments.Select(commentMapper.MapToResponse);
    }
    
    public async Task<IEnumerable<CommentResponse>> GetAllFromPostIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            logger.LogWarning("Id {Id} not valid or is missing: Provide an existing Comment Id", id);
            return null;
        }
        
        IEnumerable<Comment>? comments = await commentRepository.GetAllFromPostIdAsync(id);
        
        return comments.Select(commentMapper.MapToResponse).ToList();
    }
    
    public async Task<CommentResponse?> UpdateAsync(CommentResponse commentResponse)
    {
        Comment comment = commentMapper.MapToModel(commentResponse);
        string? loggedInUserId = httpContextAccessor.HttpContext?.Items["UserId"] as string;
        
        if (loggedInUserId == null)
        {
            logger.LogWarning("Logged in User {UserId} not found", loggedInUserId);
            return null;
        }
        
        Guid id = Guid.Parse(loggedInUserId);
        if (!id.ToString().Equals(comment.UserId.ToString()))
        {
            logger.LogWarning("User {id} not authorized to update this comment {UserId}", id, comment.UserId);
            return null;
        }
        
        
        logger.LogDebug("Updating comment {CommentId}", comment.Id);
        Comment? updatedComment = await commentRepository.UpdateAsync(comment);

        if (updatedComment is null)
        {
            logger.LogWarning("Comment {CommentId} not found", comment.Id);
            return null;
        }
        
        return commentMapper.MapToResponse(updatedComment);
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        Comment? comment = await commentRepository.GetByIdAsync(id);
        
        if (comment is null)
        {
            logger.LogWarning("comment {CommentId} not found", id);
            return false;
        }
        
        string? loggedInUserId = httpContextAccessor.HttpContext?.Items["UserId"] as string;
        
        if (loggedInUserId == null)
        {
            logger.LogWarning("Logged in User {UserId} not found", loggedInUserId);
            return false;
        }
        
        Guid userId = Guid.Parse(loggedInUserId);
        
        if (!userId.ToString().Equals(comment.UserId.ToString()))
        {
            logger.LogWarning("User {id} not authorized to delete this comment {UserId}", userId, comment.UserId);
            return false;
        }
        
        
        logger.LogDebug("Deleting comment {CommentId}", comment.Id);
        Comment? deletedComment = await commentRepository.DeleteByIdAsync(id);

        if (deletedComment is null)
        {
            logger.LogWarning("Could not delete comment {CommentId}", comment.Id);
            return false;
        }
        
        logger.LogDebug("Comment {CommentId} deleted", comment.Id);
        return true;
    }
}