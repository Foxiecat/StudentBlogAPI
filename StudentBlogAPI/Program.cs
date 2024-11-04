using Serilog;

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
            builder.AddServices();

            WebApplication app = builder.Build();
            app.ConfigureHttpRequest();
            
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

