using Serilog;
using StudentBlogAPI.Auth;
using StudentBlogAPI.Database.Health;
using StudentBlogAPI.Extensions;
using StudentBlogAPI.Middleware;

namespace StudentBlogAPI;

internal class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .CreateBootstrapLogger();

        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();
            builder.Services.AddApiFeaturesServices();
            
            builder.Services.AddDatabaseServices(builder.Configuration);
            
            builder.Services
                .AddScoped<BasicAuthentication>()
                .Configure<BasicAuthenticationOptions>(builder.Configuration.GetSection("BasicAuthenticationOptions"));
            
            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });
            
            builder.Services
                .AddHttpContextAccessor()
                .AddEndpointsApiExplorer()
                .AddSwaggerBasicAuthentication();

            
            // Http Configuration
            WebApplication app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middleware
            app.UseHttpsRedirection()
                .UseHealthChecks("/_health")
                .UseMiddleware<BasicAuthentication>()
                .UseAuthorization();

            app.MapControllers();
            
            app.Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Application terminated unexpectedly!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

