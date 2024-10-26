using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using StudentBlogAPI.Configuration.Authentication;
using StudentBlogAPI.Features.Users.Interfaces;

namespace StudentBlogAPI.Middleware;

public class BasicAuthentication : IMiddleware
{
    private readonly ILogger<BasicAuthentication> _logger;
    private readonly IUserService _userService;
    private readonly List<Regex> _excludePatterns;

    public BasicAuthentication(
        ILogger<BasicAuthentication> logger,
        IUserService userService,
        IOptions<BasicAuthenticationOptions> options)
    {
        _logger = logger;
        _userService = userService;
        
        _excludePatterns = options.Value.ExcludePatterns.Select(pattern => new Regex(pattern)).ToList();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (_excludePatterns.Any(regex => regex.IsMatch(context.Request.Path)))
        {
            await next(context);
        }
        
        string authenticationHeader = context.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(authenticationHeader))
        {
            _logger.LogWarning("Authentication header is missing.");
            throw new UnauthorizedAccessException("Authentication header is missing.");
        }

        if (!authenticationHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Authentication header is invalid.");
            throw new UnauthorizedAccessException("Authentication header is invalid.");
        }

        SplitString(authenticationHeader, " ", out string basic, out string base64String);
        if (string.IsNullOrWhiteSpace(base64String) || string.IsNullOrWhiteSpace(basic))
        {
            _logger.LogWarning("Authentication header is empty");
            throw new UnauthorizedAccessException("Authentication header is empty.");
        }
        
        // Decode base64-string to username and password
        string userName, password;
        try
        {
            string userNamePassword = ExtractBase64String(base64String);
            SplitString(userNamePassword, ":", out userName, out password);

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Missing username and/or password.");
                throw new UnauthorizedAccessException("Missing username and/or password.");
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Authentication header is invalid.");
            throw new UnauthorizedAccessException("Authentication header is invalid.", e);
        }

        Guid userId = await _userService.AuthenticateUserAsync(userName, password);
        if (userId == Guid.Empty)
        {
            _logger.LogWarning("Username or password is incorrect");
            throw new UnauthorizedAccessException("Username or password is incorrect.");
        }
        
        context.Items["UserId"] = userId.ToString();
        await next(context);
    }

    private string ExtractBase64String(string base64String)
    {
        byte[] base64Bytes = Convert.FromBase64String(base64String);
        string userNamePassword = Encoding.UTF8.GetString(base64Bytes);
        
        return userNamePassword;
    }

    private void SplitString(string authenticationHeader, string seperator, out string left, out string right)
    {
        left = right = string.Empty;
        string[] array = authenticationHeader.Split(seperator);

        if (array is not [var a, var b]) return;
        left = a;
        right = b;
    }
}