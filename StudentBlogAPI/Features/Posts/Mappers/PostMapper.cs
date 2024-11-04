using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;

namespace StudentBlogAPI.Features.Posts.Mappers;

public class PostMapper : IMapper<Post, PostDTO>
{
    public PostDTO MapToDTO(Post post)
    {
        return new PostDTO
        {
            Id = post.Id,
            UserId = post.UserId,
            Title = post.Title,
            Content = post.Content,
            DatePosted = post.DatePosted
        };
    }

    public Post MapToModel(PostDTO postDTO)
    {
        return new Post
        {
            Id = postDTO.Id,
            UserId = postDTO.UserId,
            Title = postDTO.Title,
            Content = postDTO.Content,
            DatePosted = postDTO.DatePosted
        };
    }
}