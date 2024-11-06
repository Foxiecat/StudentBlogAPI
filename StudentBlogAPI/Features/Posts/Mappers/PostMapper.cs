using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;

namespace StudentBlogAPI.Features.Posts.Mappers;

public class PostMapper : IMapper<Post, PostResponse>
{
    public PostResponse MapToDTO(Post post)
    {
        return new PostResponse
        {
            Id = post.Id,
            UserId = post.UserId,
            Title = post.Title,
            Content = post.Content
        };
    }

    public Post MapToModel(PostResponse postResponse)
    {
        return new Post
        {
            Id = postResponse.Id,
            UserId = postResponse.UserId,
            Title = postResponse.Title,
            Content = postResponse.Content
        };
    }
}