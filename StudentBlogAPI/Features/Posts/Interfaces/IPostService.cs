using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;

namespace StudentBlogAPI.Features.Posts.Interfaces;

public interface IPostService : IService<PostDTO>
{
    Task<PostDTO?> CreatePostAsync(CreatePostDTO createPostDTO);
    Task<IEnumerable<PostDTO>> FindAsync();
}