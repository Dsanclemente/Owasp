namespace SecureApp.WebApi.DTOs.Usuarios;

public class CreateUsuarioRequest
{
    public required string Nombre { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Rol { get; init; }
} 