using Microsoft.EntityFrameworkCore;
using StudentBlogAPI.Data;
using StudentBlogAPI.Data.Health;

namespace StudentBlogAPI.Extensions;

public static class DatabaseServiceExtension
{
    public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database health check");
        
        services.AddDbContext<StudentBlogDbContext>(options =>
            options.UseMySql(configuration.GetConnectionString(name:"DefaultConnection"),
                new MySqlServerVersion(new Version(8, 4, 2))));
    }
}