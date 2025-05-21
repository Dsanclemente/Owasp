namespace SecureApp.Domain.Users;

public record LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public record CreateUsuarioRequest
{
    public required string Nombre { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public record UpdateUsuarioRequest
{
    public required string Nombre { get; init; }
    public required string Email { get; init; }
} 