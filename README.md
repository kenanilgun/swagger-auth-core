# SwaggerAuth.Core

A flexible and secure Swagger authentication middleware for ASP.NET Core Web APIs. Supports multiple document authentication with configurable credentials.

**Author:** Kenan İLGÜN  
**GitHub:** [github.com/kenanilgun/swagger-auth-core](https://github.com/kenanilgun/swagger-auth-core)

## Features

- 🔐 **Basic Authentication** for Swagger documentation
- 📚 **Multiple Document Support** - Secure different API groups separately
- ⚙️ **Flexible Configuration** - Configure authentication per document
- 🚀 **Easy Integration** - Simple extension methods for quick setup
- 🔒 **Security First** - Only configured documents require authentication
- 📖 **Well Documented** - Comprehensive XML documentation

## Installation

```bash
dotnet add package SwaggerAuth.Core
```

## Quick Start

### 1. Configure Authentication Credentials

Add your Swagger authentication configuration to `appsettings.json`:

```json
{
  "Swagger": {
    "Auth": {
      "v1-admin": {
        "Username": "admin",
        "Password": "K9#mP2$vL8nQ"
      },
      "v1-public": {
        "Username": "public",
        "Password": "X7@jR5&hF3wE"
      }
    }
  }
}
```

### 2. Register Services

In your `Program.cs` or `Startup.cs`:

```csharp
using SwaggerAuth.Core;

// Add Swagger authentication services
builder.Services.AddSwaggerAuth();

// Or with custom options
builder.Services.AddSwaggerAuth(options =>
{
    options.ConfigurationSection = "Swagger:Auth";
    options.RequireAuthForAllDocuments = false;
    options.CustomErrorMessage = "Access denied to API documentation";
});
```

### 3. Add Middleware

```csharp
// Add the middleware to your pipeline
app.UseSwaggerAuth();
```

### 4. Configure Swagger Documents

```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1-admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
    c.SwaggerDoc("v1-public", new OpenApiInfo { Title = "Public API", Version = "v1" });
    c.SwaggerDoc("v1-internal", new OpenApiInfo { Title = "Internal API", Version = "v1" });
});
```

## Configuration Options

### SwaggerAuthOptions

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ConfigurationSection` | string | `"Swagger:Auth"` | Configuration section where credentials are stored |
| `RequireAuthForAllDocuments` | bool | `false` | Whether to require auth for all documents by default |
| `CustomErrorMessage` | string? | `null` | Custom error message for authentication failures |

### Configuration Structure

```json
{
  "Swagger": {
    "Auth": {
      "document-name": {
        "Username": "username",
        "Password": "password"
      }
    }
  }
}
```

## Usage Examples

### Basic Setup

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddSwaggerAuth();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1-admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
    c.SwaggerDoc("v1-public", new OpenApiInfo { Title = "Public API", Version = "v1" });
});

var app = builder.Build();

// Add middleware
app.UseSwaggerAuth();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
```

### Advanced Configuration

```csharp
builder.Services.AddSwaggerAuth(options =>
{
    options.ConfigurationSection = "MyApi:Documentation:Auth";
    options.RequireAuthForAllDocuments = true;
    options.CustomErrorMessage = "Please contact administrator for access";
});
```

### Custom Configuration Section

```json
{
  "MyApi": {
    "Documentation": {
      "Auth": {
        "v1-secure": {
          "Username": "secureuser",
          "Password": "securepass123"
        }
      }
    }
  }
}
```

## Security Best Practices

1. **Use Strong Passwords**: Use complex passwords with special characters
2. **Environment-Specific Configuration**: Use different credentials for different environments
3. **Regular Password Rotation**: Change passwords periodically
4. **HTTPS Only**: Always use HTTPS in production
5. **Minimal Access**: Only secure documents that need protection

## Examples

### Multiple API Groups

```json
{
  "Swagger": {
    "Auth": {
      "v1-admin": {
        "Username": "admin",
        "Password": "AdminPass123!"
      },
      "v1-partner": {
        "Username": "partner",
        "Password": "PartnerPass456!"
      },
      "v1-internal": {
        "Username": "internal",
        "Password": "InternalPass789!"
      }
    }
  }
}
```

### Public and Private APIs

```csharp
builder.Services.AddSwaggerGen(c =>
{
    // Public API - no authentication required
    c.SwaggerDoc("v1-public", new OpenApiInfo { Title = "Public API", Version = "v1" });
    
    // Private API - authentication required
    c.SwaggerDoc("v1-private", new OpenApiInfo { Title = "Private API", Version = "v1" });
});
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For support and questions, please open an issue on GitHub or contact Kenan İLGÜN. 