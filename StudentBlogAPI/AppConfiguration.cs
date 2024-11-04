using StudentBlogAPI.Middleware;

namespace StudentBlogAPI;

public static class AppConfiguration
{
    public static void ConfigureHttpRequest(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/error");
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection()
            .UseMiddleware<BasicAuthentication>()
            .UseAuthorization();

        app.MapControllers();
    }
}