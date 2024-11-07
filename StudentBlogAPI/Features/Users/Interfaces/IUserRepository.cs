using StudentBlogAPI.Features.Common.Interfaces;
using StudentBlogAPI.Features.Posts;

namespace StudentBlogAPI.Features.Users.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<Post>> GetUserPostsAsync(Guid userId);
}