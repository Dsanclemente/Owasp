namespace SecureApp.WebApi.DTOs.Auth;

public class LoginResponse
{
    public required string Token { get; init; }
    public required DateTime ExpiresAt { get; init; }
    public required string Rol { get; init; }
} 