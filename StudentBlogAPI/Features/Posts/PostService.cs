using StudentBlogAPI.Features.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;
using StudentBlogAPI.Features.Posts.Interfaces;
using StudentBlogAPI.Features.Users;

namespace StudentBlogAPI.Features.Posts;

public class PostService(ILogger<PostService> logger,
                         IMapper<Post, PostResponse> postMapper,
                         IMapper<Post, PostRequest> createPostMapper,
                         IPostRepository postRepository,
                         IHttpContextAccessor httpContextAccessor) : IPostService
{
    public async Task<PostResponse?> CreatePostAsync(PostRequest postRequest)
    {
        Post post = createPostMapper.MapToModel(postRequest);
        string? loggedInUserIdString = httpContextAccessor.HttpContext?.Items["UserId"] as string;

        if (loggedInUserIdString == null)
        {
            logger.LogWarning("UserId {UserId} not found", loggedInUserIdString);
            return null;
        }
        
        Guid loggedInUserId = Guid.Parse(loggedInUserIdString);
        
        post.Id = Guid.NewGuid();
        post.UserId = loggedInUserId;
        post.Title = postRequest.Title;
        post.Content = postRequest.Content;
        post.DatePosted = DateTime.UtcNow;
        
        Post? addedPost = await postRepository.AddAsync(post);

        return addedPost is null
            ? null
            : postMapper.MapToResponse(addedPost);
    }
    
    public async Task<PostResponse?> AddAsync(PostResponse postResponse)
    {
        Post model = postMapper.MapToModel(postResponse);
        Post? modelResponse = await postRepository.AddAsync(model);
        
        return modelResponse is null
            ? null
            : postMapper.MapToResponse(modelResponse);
    }

    
    public async Task<PostResponse?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            logger.LogWarning("Id {Id} not valid or is missing: Provide an existing Post Id:Guid", id);
            return null;
        }
        
        Post? post = await postRepository.GetByIdAsync(id);
        
        return post is null
            ? null
            : postMapper.MapToResponse(post);
    }

    public async Task<IEnumerable<PostResponse>> GetPagedAsync(int pageNumber, int pageSize)
    {
        IEnumerable<Post>? posts = await postRepository.GetPagedAsync(pageNumber, pageSize);
        
        return posts.Select(postMapper.MapToResponse).ToList();
    }
    
    
    public async Task<PostResponse?> UpdateAsync(PostResponse postResponse)
    {
        Post post = postMapper.MapToModel(postResponse);
        string? loggedInUserId = httpContextAccessor.HttpContext?.Items["UserId"] as string;

        if (loggedInUserId == null)
        {
            logger.LogWarning("Logged in User {UserId} not found", loggedInUserId);
            return null;
        }
        
        Guid id = Guid.Parse(loggedInUserId);
        if (!id.ToString().Equals(post.UserId.ToString()))
        {
            logger.LogWarning("User {id} not authorized to update this post {UserId}", id, post.UserId);
            return null;
        }
        
        
        logger.LogDebug("Updating post {PostId}", post.Id);
        Post? updatedPost = await postRepository.UpdateAsync(post);

        if (updatedPost is null)
        {
            logger.LogWarning("Post {PostId} failed to update", post.Id);
            return null;
        }
        
        return postMapper.MapToResponse(updatedPost);
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        Post? post = await postRepository.GetByIdAsync(id);

        if (post is null)
        {
            logger.LogWarning("Post {PostId} not found", id);
            return false;
        }
        
        string? loggedInUserId = httpContextAccessor.HttpContext?.Items["UserId"] as string;
        if (loggedInUserId == null)
        {
            logger.LogWarning("Logged in User {UserId} not found", loggedInUserId);
            return false;
        }
        
        Guid userId = Guid.Parse(loggedInUserId);
        if (!userId.ToString().Equals(post.UserId.ToString()))
        {
            logger.LogWarning("User {id} not authorized to delete this post {UserId}", id, post.UserId);
            return false;
        }
        
        string? isLoggedInUserAdmin = httpContextAccessor.HttpContext?.Items["IsAdminUser"] as string;

        if (isLoggedInUserAdmin != null && isLoggedInUserAdmin.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            await postRepository.DeleteByIdAsync(id);
            return true;
        }
        
        logger.LogDebug("Deleting post {PostId}", id);
        Post? deletedPost = await postRepository.DeleteByIdAsync(id);

        if (deletedPost is null)
        {
            logger.LogWarning("Could not delete post {PostId}", id);
            return false;
        }
        
        logger.LogDebug("Post {PostId} deleted", id);
        return true;
    }

    public async Task<IEnumerable<PostResponse>> FindAsync()
    {
        throw new NotImplementedException();
    }
}