using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace StudentBlogAPI.Data.Health;

public class DatabaseHealthCheck(StudentBlogDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        try
        {
            if (await dbContext.Database.CanConnectAsync(cancellationToken))
                return HealthCheckResult.Healthy("Database connection is healthy");
            
            return HealthCheckResult.Unhealthy("Database connection is unhealthy");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("Database connection failed!", exception);
        }
    }
}