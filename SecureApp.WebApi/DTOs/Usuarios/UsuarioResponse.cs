namespace SecureApp.WebApi.DTOs.Usuarios;

public class UsuarioResponse
{
    public required Guid Id { get; init; }
    public required string Nombre { get; init; }
    public required string Email { get; init; }
    public required string Rol { get; init; }
} 