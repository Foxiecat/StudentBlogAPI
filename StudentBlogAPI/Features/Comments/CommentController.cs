using Microsoft.AspNetCore.Mvc;
using StudentBlogAPI.Features.Comments.Interfaces;
using StudentBlogAPI.Features.Comments.Models.Responses;

namespace StudentBlogAPI.Features.Comments;

[Route("api/v1/comments")]
public class CommentController(ILogger<CommentController> logger, ICommentService commentService) : ControllerBase
{
    [HttpPost("{postId:guid}", Name = "AddCommentAsync")]
    public async Task<ActionResult> AddCommentAsync(Guid postId, [FromBody] AddCommentResponse addCommentResponse)
    {
        CommentResponse? addedComment = await commentService.AddCommentAsync(postId, addCommentResponse);

        return addedComment is null
            ? BadRequest("Comment could not be created")
            : Ok(addedComment);
    }
}