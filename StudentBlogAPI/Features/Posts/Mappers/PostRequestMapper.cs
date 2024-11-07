using StudentBlogAPI.Features.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;

namespace StudentBlogAPI.Features.Posts.Mappers;

public class PostRequestMapper : IMapper<Post, PostRequest>
{
    public PostRequest MapToResponse(Post model)
    {
        return new PostRequest
        {
            Title = model.Title,
            Content = model.Content
        };
    }

    public Post MapToModel(PostRequest request)
    {
        return new Post
        {
            Title = request.Title,
            Content = request.Content
        };
    }
}