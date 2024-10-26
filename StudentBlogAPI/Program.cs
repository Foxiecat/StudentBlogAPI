using Microsoft.EntityFrameworkCore;
using Serilog;
using StudentBlogAPI.Configuration.Authentication;
using StudentBlogAPI.Configuration.Database;
using StudentBlogAPI.Extensions;
using StudentBlogAPI.Features.Users;
using StudentBlogAPI.Features.Users.Interfaces;
using StudentBlogAPI.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddScoped<IUserService, UserService>();

builder.Services
    .AddScoped<BasicAuthentication>()
    .Configure<BasicAuthenticationOptions>(builder.Configuration.GetSection("BasicAuthenticationOptions"));

// Database connection
builder.Services.AddDbContext<StudentBlogDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8,4,2))));


builder.Services
    .AddHttpContextAccessor()
    .AddEndpointsApiExplorer()
    .AddSwaggerBasicAuthentication();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseMiddleware<BasicAuthentication>()
    .UseAuthorization();

app.MapControllers();

app.Run();