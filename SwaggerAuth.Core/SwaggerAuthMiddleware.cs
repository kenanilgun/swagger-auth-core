using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace SwaggerAuth.Core;

/// <summary>
/// Middleware for securing Swagger documentation with Basic Authentication.
/// Supports multiple document authentication with configurable credentials.
/// </summary>
public class SwaggerAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly SwaggerAuthOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="SwaggerAuthMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="options">The Swagger authentication options.</param>
    public SwaggerAuthMiddleware(RequestDelegate next, IConfiguration configuration, SwaggerAuthOptions options)
    {
        _next = next;
        _configuration = configuration;
        _options = options;
    }

    /// <summary>
    /// Processes the HTTP request and applies authentication if required.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();

        // Check if the request is for Swagger documentation
        if (path != null && path.Contains("/swagger/"))
        {
            var docName = ExtractDocNameFromPath(path);
            
            if (!string.IsNullOrEmpty(docName) && IsDocumentConfiguredForAuth(docName))
            {
                if (!await IsAuthorizedForDocAsync(context, docName))
                {
                    context.Response.StatusCode = 401;
                    context.Response.Headers.Add("WWW-Authenticate", $"Basic realm=\"{docName} API Documentation\"");
                    await context.Response.WriteAsync($"Unauthorized access to {docName} API documentation. Please provide valid credentials.");
                    return;
                }
            }
        }

        await _next(context);
    }

    /// <summary>
    /// Extracts the document name from the Swagger URL path.
    /// </summary>
    /// <param name="path">The request path.</param>
    /// <returns>The document name if found; otherwise, null.</returns>
    private string? ExtractDocNameFromPath(string path)
    {
        // /swagger/v1-pinsoft-admin/swagger.json -> v1-pinsoft-admin
        if (path.Contains("/swagger/") && path.Contains("/swagger.json"))
        {
            var startIndex = path.IndexOf("/swagger/") + "/swagger/".Length;
            var endIndex = path.IndexOf("/swagger.json");
            
            if (startIndex > 0 && endIndex > startIndex)
            {
                return path.Substring(startIndex, endIndex - startIndex);
            }
        }
        
        return null;
    }

    /// <summary>
    /// Checks if a document is configured for authentication.
    /// </summary>
    /// <param name="docName">The document name.</param>
    /// <returns>True if the document requires authentication; otherwise, false.</returns>
    private bool IsDocumentConfiguredForAuth(string docName)
    {
        // Check if credentials exist for this document in configuration
        var username = _configuration[$"{_options.ConfigurationSection}:{docName}:Username"];
        var password = _configuration[$"{_options.ConfigurationSection}:{docName}:Password"];
        
        return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
    }

    /// <summary>
    /// Validates Basic Authentication credentials for a specific document.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="docName">The document name.</param>
    /// <returns>True if authentication is successful; otherwise, false.</returns>
    private Task<bool> IsAuthorizedForDocAsync(HttpContext context, string docName)
    {
        // Basic Authentication validation
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic "))
        {
            return Task.FromResult(false);
        }

        try
        {
            var encodedCredentials = authHeader.Substring("Basic ".Length);
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var parts = credentials.Split(':', 2);

            if (parts.Length != 2)
            {
                return Task.FromResult(false);
            }

            var username = parts[0];
            var password = parts[1];

            // Get document credentials from configuration
            var docUsername = _configuration[$"{_options.ConfigurationSection}:{docName}:Username"];
            var docPassword = _configuration[$"{_options.ConfigurationSection}:{docName}:Password"];

            return Task.FromResult(username == docUsername && password == docPassword);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
} 