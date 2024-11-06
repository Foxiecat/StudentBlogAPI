using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Comments.Models;
using StudentBlogAPI.Features.Comments.Models.Responses;

namespace StudentBlogAPI.Features.Comments.Mappers;

public class CreateCommentMapper : IMapper<Comment, AddCommentResponse>
{
    public AddCommentResponse MapToDTO(Comment model)
    {
        return new AddCommentResponse
        {
            Content = model.Content
        };
    }

    public Comment MapToModel(AddCommentResponse response)
    {
        return new Comment
        {
            Content = response.Content
        };
    }
}