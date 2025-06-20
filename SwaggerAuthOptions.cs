namespace SwaggerAuth.Core;

/// <summary>
/// Configuration options for Swagger authentication middleware.
/// </summary>
public class SwaggerAuthOptions
{
    /// <summary>
    /// Gets or sets the configuration section name where authentication credentials are stored.
    /// Default value is "Swagger:Auth".
    /// </summary>
    /// <example>
    /// For configuration like:
    /// {
    ///   "Swagger": {
    ///     "Auth": {
    ///       "v1-admin": {
    ///         "Username": "admin",
    ///         "Password": "password123"
    ///       }
    ///     }
    ///   }
    /// }
    /// The ConfigurationSection should be "Swagger:Auth"
    /// </example>
    public string ConfigurationSection { get; set; } = "Swagger:Auth";

    /// <summary>
    /// Gets or sets whether to enable authentication for all Swagger documents by default.
    /// If false, only documents with explicit configuration will require authentication.
    /// Default value is false.
    /// </summary>
    public bool RequireAuthForAllDocuments { get; set; } = false;

    /// <summary>
    /// Gets or sets the custom error message to display when authentication fails.
    /// If null, a default message will be used.
    /// </summary>
    public string? CustomErrorMessage { get; set; }
} 