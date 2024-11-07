using StudentBlogAPI.Features.Comments.DTO;
using StudentBlogAPI.Features.Common.Interfaces;

namespace StudentBlogAPI.Features.Comments.Mappers;

public class CommentRequestMapper : IMapper<Comment, CommentRequest>
{
    public CommentRequest MapToResponse(Comment model)
    {
        return new CommentRequest
        {
            Content = model.Content
        };
    }

    public Comment MapToModel(CommentRequest request)
    {
        return new Comment
        {
            Content = request.Content
        };
    }
}