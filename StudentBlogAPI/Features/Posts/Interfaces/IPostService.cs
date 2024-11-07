using StudentBlogAPI.Features.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;

namespace StudentBlogAPI.Features.Posts.Interfaces;

public interface IPostService : IService<PostResponse>
{
    Task<PostResponse?> CreatePostAsync(PostRequest postRequest);
    Task<IEnumerable<PostResponse>> FindAsync();
}