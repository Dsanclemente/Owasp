namespace SecureApp.WebApi.Configuration;

public class CorsSettings
{
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
} 