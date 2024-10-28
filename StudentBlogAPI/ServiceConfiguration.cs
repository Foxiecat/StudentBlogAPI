using Microsoft.EntityFrameworkCore;
using Serilog;
using StudentBlogAPI.Auth;
using StudentBlogAPI.Data;
using StudentBlogAPI.Extensions;
using StudentBlogAPI.Features.Common.Interfaces;
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
        builder.Services.AddAuthServices(builder.Configuration);
        builder.Services.AddExceptionHandler<GlobalExceptionHandling>();
        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.AddSwagger();
        builder.Host.SerilogConfiguration();
    }

    private static void AddStudentBlogServices(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IMapper<User, UserDTO>, UserMapper>()
            .AddScoped<IMapper<User, RegistrationDTO>, RegistrationMapper>()
            .AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddAuthServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddScoped<BasicAuthentication>()
            .Configure<BasicAuthenticationOptions>(configuration.GetSection("BasicAuthenticationOptions"));
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
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