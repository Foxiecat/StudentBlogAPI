using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Comments.Models;

namespace StudentBlogAPI.Features.Comments.Interfaces;

public interface ICommentRepository : IRepository<Comment>;