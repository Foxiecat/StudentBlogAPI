using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;

namespace StudentBlogAPI.Features.Posts.Mappers;

public class CreatePostMapper : IMapper<Post, CreatePostDTO>
{
    public CreatePostDTO MapToDTO(Post model)
    {
        return new CreatePostDTO
        {
            Title = model.Title,
            Content = model.Content
        };
    }

    public Post MapToModel(CreatePostDTO dto)
    {
        return new Post
        {
            Title = dto.Title,
            Content = dto.Content
        };
    }
}