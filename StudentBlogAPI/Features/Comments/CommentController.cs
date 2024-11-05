using Microsoft.AspNetCore.Mvc;
using StudentBlogAPI.Features.Comments.DTOs;
using StudentBlogAPI.Features.Comments.Interfaces;

namespace StudentBlogAPI.Features.Comments;

[Route("api/v1/comments")]
public class CommentController(ILogger<CommentController> logger, ICommentService commentService) : ControllerBase
{
    [HttpPost("{postId:guid}", Name = "AddCommentAsync")]
    public async Task<ActionResult> AddCommentAsync(Guid postId, [FromBody] string content = "Content")
    {
        CommentDTO? addedComment = await commentService.CreateCommentAsync(postId, content);

        return addedComment is null
            ? BadRequest("Comment could not be created")
            : Ok(addedComment);
    }
}