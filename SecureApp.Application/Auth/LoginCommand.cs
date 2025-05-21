namespace SecureApp.Application.Auth;

public class LoginCommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResult
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
} 