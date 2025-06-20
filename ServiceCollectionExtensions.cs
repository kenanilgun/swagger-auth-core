using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SwaggerAuth.Core;

/// <summary>
/// Extension methods for configuring Swagger authentication services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Swagger authentication services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Action to configure the Swagger authentication options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSwaggerAuth(this IServiceCollection services, Action<SwaggerAuthOptions>? configureOptions = null)
    {
        var options = new SwaggerAuthOptions();
        configureOptions?.Invoke(options);
        
        services.AddSingleton(options);
        return services;
    }

    /// <summary>
    /// Adds Swagger authentication services to the service collection with default options.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSwaggerAuth(this IServiceCollection services)
    {
        return services.AddSwaggerAuth(null);
    }
}

/// <summary>
/// Extension methods for configuring Swagger authentication middleware.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds Swagger authentication middleware to the application pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder for chaining.</returns>
    public static IApplicationBuilder UseSwaggerAuth(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SwaggerAuthMiddleware>();
    }
} 