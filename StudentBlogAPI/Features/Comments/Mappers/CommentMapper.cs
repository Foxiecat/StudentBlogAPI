using StudentBlogAPI.Features.Comments.DTO;
using StudentBlogAPI.Features.Common.Interfaces;

namespace StudentBlogAPI.Features.Comments.Mappers;

public class CommentMapper : IMapper<Comment, CommentResponse>
{
    public CommentResponse MapToResponse(Comment model)
    {
        return new CommentResponse
        {
            Id = model.Id,
            PostId = model.PostId,
            UserId = model.UserId,
            Content = model.Content
        };
    }

    public Comment MapToModel(CommentResponse response)
    {
        return new Comment
        {
            Id = response.Id,
            PostId = response.PostId,
            UserId = response.UserId,
            Content = response.Content
        };
    }
}