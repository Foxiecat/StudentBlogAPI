using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;
using StudentBlogAPI.Features.Posts.Interfaces;
using StudentBlogAPI.Features.Users;

namespace StudentBlogAPI.Features.Posts;

public class PostService(ILogger<PostService> logger,
                         IMapper<Post, PostDTO> postMapper,
                         IMapper<Post, CreatePostDTO> createPostMapper,
                         IPostRepository postRepository,
                         IHttpContextAccessor httpContextAccessor) : IPostService
{
    public async Task<PostDTO?> CreatePostAsync(CreatePostDTO createPostDTO)
    {
        Post post = createPostMapper.MapToModel(createPostDTO);
        string? loggedInUserIdString = httpContextAccessor.HttpContext?.Items["UserId"] as string;

        if (loggedInUserIdString == null)
        {
            logger.LogWarning("UserId {UserId} not found", loggedInUserIdString);
            return null;
        }
        
        Guid loggedInUserId = Guid.Parse(loggedInUserIdString);
        
        post.Id = Guid.NewGuid();
        post.UserId = loggedInUserId;
        post.Title = createPostDTO.Title;
        post.Content = createPostDTO.Content;
        post.DatePosted = DateTime.UtcNow;
        
        Post? addedPost = await postRepository.AddAsync(post);

        return addedPost is null
            ? null
            : postMapper.MapToDTO(addedPost);
    }
    
    public async Task<PostDTO?> AddAsync(PostDTO postDTO)
    {
        Post model = postMapper.MapToModel(postDTO);
        Post? modelResponse = await postRepository.AddAsync(model);
        
        return modelResponse is null
            ? null
            : postMapper.MapToDTO(modelResponse);
    }

    
    public async Task<PostDTO?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            logger.LogWarning("Id {Id} not valid or is missing: Provide an existing Post Id:Guid", id);
            return null;
        }
        
        Post? post = await postRepository.GetByIdAsync(id);
        
        return post is null
            ? null
            : postMapper.MapToDTO(post);
    }

    public async Task<IEnumerable<PostDTO>> GetPagedAsync(int pageNumber, int pageSize)
    {
        IEnumerable<Post>? posts = await postRepository.GetPagedAsync(pageNumber, pageSize);
        
        return posts.Select(postMapper.MapToDTO).ToList();
    }
    
    
    public async Task<PostDTO?> UpdateAsync(PostDTO postDTO)
    {
        Post post = postMapper.MapToModel(postDTO);
        
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
            logger.LogWarning("Post {PostId} not found", post.Id);
            return null;
        }
        
        logger.LogDebug("Updating post {PostId}", post.Id);
        return postMapper.MapToDTO(updatedPost);
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

    public async Task<IEnumerable<PostDTO>> FindAsync()
    {
        throw new NotImplementedException();
    }
}