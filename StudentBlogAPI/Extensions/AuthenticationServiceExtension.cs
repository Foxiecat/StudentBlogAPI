using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace StudentBlogAPI.Extensions;

public static class AuthenticationServiceExtension
{
    public static void AddSwaggerBasicAuthentication(this IServiceCollection services)
    {
        services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme."
            });
            
            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        },
                        Scheme = "basic",
                        Name = "basic",
                        In = ParameterLocation.Header
                    },
                    []
                }
            });
        });
    }
}