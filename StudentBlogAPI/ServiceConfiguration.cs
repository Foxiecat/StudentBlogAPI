using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StudentBlogAPI.Auth;
using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Data;
using StudentBlogAPI.Data.Health;
using StudentBlogAPI.Extensions;
using StudentBlogAPI.Features.Comments;
using StudentBlogAPI.Features.Comments.Interfaces;
using StudentBlogAPI.Features.Posts;
using StudentBlogAPI.Features.Posts.DTOs;
using StudentBlogAPI.Features.Posts.Interfaces;
using StudentBlogAPI.Features.Posts.Mappers;
using StudentBlogAPI.Features.Users;
using StudentBlogAPI.Features.Users.DTOs;
using StudentBlogAPI.Features.Users.Interfaces;
using StudentBlogAPI.Features.Users.Mappers;
using StudentBlogAPI.Middleware;

namespace StudentBlogAPI;

public static class ServiceConfiguration
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddStudentBlogServices();
        builder.Services.AddFluentValidation();
        builder.Services.AddAuthentication(builder.Configuration);
        builder.Services.AddExceptionHandler<GlobalExceptionHandling>();
        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.AddSwagger();
        builder.Host.SerilogConfiguration();
    }

    private static void AddStudentBlogServices(this IServiceCollection services)
    {
        // User services
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IMapper<User, UserDTO>, UserMapper>()
            .AddScoped<IMapper<User, RegistrationDTO>, RegistrationMapper>();
        
        // Post services
        services
            .AddScoped<IPostRepository, PostRepository>()
            .AddScoped<IPostService, PostService>()
            .AddScoped<IMapper<Post, PostDTO>, PostMapper>()
            .AddScoped<IMapper<Post, CreatePostDTO>, CreatePostMapper>();
        
        // Comment services
            
    }

    private static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>()
            .AddFluentValidationAutoValidation(config
                => config.DisableDataAnnotationsValidation = true);
    }

    private static void AddAuthentication(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddScoped<BasicAuthentication>()
            .Configure<BasicAuthenticationOptions>(configuration.GetSection("BasicAuthenticationOptions"));
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database health check");
        
        services.AddDbContext<StudentBlogDbContext>(options =>
            options.UseMySql(configuration.GetConnectionString(name:"DefaultConnection"),
                new MySqlServerVersion(new Version(8, 4, 2))));
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services
            .AddHttpContextAccessor()
            .AddEndpointsApiExplorer()
            .AddSwaggerBasicAuthentication();
    }

    private static void SerilogConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
    }
}