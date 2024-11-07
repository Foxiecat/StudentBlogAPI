using Microsoft.AspNetCore.Mvc;
using StudentBlogAPI.Features.Comments.DTO;
using StudentBlogAPI.Features.Comments.Interfaces;

namespace StudentBlogAPI.Features.Comments;

[ApiController]
[Route("api/v1/comments")]
public class CommentController(ILogger<CommentController> logger, ICommentService commentService) : ControllerBase
{
    [HttpPost("{postId:guid}", Name = "AddCommentAsync")]
    public async Task<ActionResult<CommentResponse>> AddCommentAsync(Guid postId, [FromBody] CommentRequest commentRequest)
    {
        CommentResponse? addedComment = await commentService.AddCommentAsync(postId, commentRequest);

        return addedComment is null
            ? BadRequest("Comment could not be created")
            : Ok(addedComment);
    }
    
    [HttpGet(Name = "GetCommentsAsync")]
    public async Task<ActionResult<IEnumerable<CommentResponse>>> GetCommentsAsync([FromQuery] int pageNumber = 1, 
                                                                                   [FromQuery] int pageSize = 10)
    {
        IEnumerable<CommentResponse> commentResponses = await commentService.GetPagedAsync(pageNumber, pageSize);
        return Ok(commentResponses);
    }
    
    [HttpGet("{postId:guid}/comments", Name = "GetAllCommentsFromPostIdAsync")]
    public async Task<ActionResult<IEnumerable<CommentResponse>>> GetAllFromPostIdAsync(Guid postId)
    {
        IEnumerable<CommentResponse>? commentResponse = await commentService.GetAllFromPostIdAsync(postId);
        
        return Ok(commentResponse);
    }
    
    [HttpPut("{commentId:guid}", Name = "UpdateCommentAsync")]
    public async Task<ActionResult<CommentResponse>> UpdateCommentAsync(Guid commentId,
                                                          [FromBody] CommentRequest commentRequest)
    {
        CommentResponse? commentResponse = await commentService.GetByIdAsync(commentId);
        
        if (commentResponse is null)
            return NotFound($"Failed to find comment with id: {commentId}");
        
        commentResponse.Content = commentRequest.Content;
        
        CommentResponse? updatedComment = await commentService.UpdateAsync(commentResponse);
        return Ok(updatedComment);
    }

    [HttpDelete("{commentId:guid}", Name = "DeleteCommentAsync")]
    public async Task<ActionResult<CommentResponse>> DeleteCommentAsync(Guid commentId)
    {
        bool result = await commentService.DeleteByIdAsync(commentId);
        
        return result
            ? Ok(result)
            : BadRequest();
    }
}