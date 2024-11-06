using Microsoft.AspNetCore.Mvc;
using StudentBlogAPI.Features.Posts.DTOs;
using StudentBlogAPI.Features.Posts.Interfaces;

namespace StudentBlogAPI.Features.Posts;

[ApiController]
[Route("api/v1/posts")]
public class PostController(ILogger<PostController> logger, IPostService postService) : ControllerBase
{
    [HttpPost(Name = "CreatePostAsync")]
    public async Task<ActionResult> CreatePostAsync(PostRequest postRequest)
    {
        PostResponse? post = await postService.CreatePostAsync(postRequest);
        
        return post is null
            ? BadRequest("Failed to create post")
            : Ok(post);
    }

    
    [HttpGet("{id:Guid}", Name = "GetPostByIdAsync")]
    public async Task<ActionResult> GetPostByIdAsync(Guid id)
    {
        PostResponse? postDTO = await postService.GetByIdAsync(id);
        
        return postDTO is null
            ? BadRequest($"Failed to get post with id: {id}")
            : Ok(postDTO);
    }

    
    [HttpGet(Name = "GetPostsAsync")]
    public async Task<ActionResult> GetPostsAsync([FromQuery] int pageNumber = 1,
                                                     [FromQuery] int pageSize = 10)
    {
        IEnumerable<PostResponse> postDTOs = await postService.GetPagedAsync(pageNumber, pageSize);
        
        return Ok(postDTOs);
    }

    
    [HttpPut("{postId:Guid}", Name = "UpdatePostAsync")]
    public async Task<ActionResult> UpdatePostAsync(Guid postId,
                                                    [FromBody] PostRequest updatePostBody)
    {
        PostResponse? postDTO = await postService.GetByIdAsync(postId);
        
        if (postDTO is null)
            return NotFound($"Failed to find post with id: {postId}");
        
        postDTO.Title = updatePostBody.Title;
        postDTO.Content = updatePostBody.Content;
        
        PostResponse? updatedPost = await postService.UpdateAsync(postDTO);

        return Ok(updatedPost);
    }

    [HttpDelete("{postId:Guid}", Name = "DeletePostAsync")]
    public async Task<ActionResult> DeletePostAsync(Guid postId)
    {
        bool result = await postService.DeleteByIdAsync(postId);
        
        return result
            ? Ok($"Successfully deleted post with id: {postId}")
            : BadRequest();
    }
}