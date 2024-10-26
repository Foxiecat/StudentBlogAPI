namespace StudentBlogAPI.Configuration.Authentication;

public class BasicAuthenticationOptions
{
    public List<string> ExcludePatterns { get; set; } = new List<string>();
}