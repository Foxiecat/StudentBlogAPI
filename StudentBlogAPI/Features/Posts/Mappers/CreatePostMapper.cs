using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;

namespace StudentBlogAPI.Features.Posts.Mappers;

public class CreatePostMapper : IMapper<Post, PostRequest>
{
    public PostRequest MapToDTO(Post model)
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