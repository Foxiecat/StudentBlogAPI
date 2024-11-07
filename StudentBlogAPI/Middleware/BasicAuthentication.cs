using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using StudentBlogAPI.Auth;
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
        
        _excludePatterns = options.Value.ExcludePatterns
            .Select(pattern => new Regex(pattern)).ToList();
    }

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        string authenticationHeader = httpContext.Request.Headers.Authorization.ToString();

        if (_excludePatterns.Any(excludePattern => excludePattern.IsMatch(httpContext.Request.Path)))
        {
            await next(httpContext);
            return;
        }
        
        
        // Checks Authentication Header:
        if (string.IsNullOrWhiteSpace(authenticationHeader))
        {
            _logger.LogWarning("Authentication header is missing.");
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Authentication header is missing.");
            return;
        }

        if (!authenticationHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Authentication header is invalid.");
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Authentication header is invalid.");
            return;
        }

        SplitString(authenticationHeader, " ", out string basic, out string base64String);
        if (string.IsNullOrWhiteSpace(base64String) || string.IsNullOrWhiteSpace(basic))
        {
            _logger.LogWarning("Authentication header is empty");
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Authentication header is empty");
            return;
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
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Missing username and/or password.");
                return;
            }
        }
        catch (FormatException)
        {
            _logger.LogWarning("Authentication header is invalid.");
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Authentication header is invalid.");
            return;
        }

        
        Guid userId = await _userService.AuthenticateUserAsync(userName, password);
        if (userId == Guid.Empty)
        {
            _logger.LogWarning("Username or password is incorrect");
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Username or password is incorrect");
            return;
        }
        
        httpContext.Items["UserId"] = userId.ToString();
        
        // Next middleware
        await next(httpContext);
    }

    private static string ExtractBase64String(string base64String)
    {
        byte[] base64Bytes = Convert.FromBase64String(base64String);
        return Encoding.UTF8.GetString(base64Bytes);
    }

    private static void SplitString(string authenticationHeader, string separator, out string left, out string right)
    {
        left = right = string.Empty;
        string[] array = authenticationHeader.Split(separator);

        if (array is not [var a, var b]) return;
        left = a;
        right = b;

    }
}