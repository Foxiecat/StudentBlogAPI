using Microsoft.EntityFrameworkCore;
using StudentBlogAPI.Features.Comments.Models;
using StudentBlogAPI.Features.Posts.Models;
using StudentBlogAPI.Features.Users.Models;

namespace StudentBlogAPI.Configuration.Database;

public class StudentBlogDbContext(DbContextOptions<StudentBlogDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(property => property.Email).IsRequired();
            
            entity.HasIndex(user => user.Email).IsUnique();
            entity.HasIndex(user => user.UserName).IsUnique();
        });
    }
}