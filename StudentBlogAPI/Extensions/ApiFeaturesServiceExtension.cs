using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Posts;
using StudentBlogAPI.Features.Posts.DTOs;
using StudentBlogAPI.Features.Posts.Interfaces;
using StudentBlogAPI.Features.Posts.Mappers;
using StudentBlogAPI.Features.Users;
using StudentBlogAPI.Features.Users.DTOs;
using StudentBlogAPI.Features.Users.Interfaces;
using StudentBlogAPI.Features.Users.Mappers;

namespace StudentBlogAPI.Extensions;

public static class ApiFeaturesServiceExtension
{
    public static void AddApiFeaturesServices(this IServiceCollection services)
    {
        // User services
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IMapper<User, UserResponse>, UserMapper>()
            .AddScoped<IMapper<User, UserRequest>, RegistrationMapper>();
        
        // Post services
        services
            .AddScoped<IPostRepository, PostRepository>()
            .AddScoped<IPostService, PostService>()
            .AddScoped<IMapper<Post, PostResponse>, PostMapper>()
            .AddScoped<IMapper<Post, PostRequest>, CreatePostMapper>();
        
        // Comment services
            
    }
}