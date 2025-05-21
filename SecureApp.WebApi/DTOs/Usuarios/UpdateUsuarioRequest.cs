using SecureApp.Domain.Users.Enums;

namespace SecureApp.WebApi.DTOs.Usuarios;

public class UpdateUsuarioRequest
{
    public required string Nombre { get; init; }
    public required string Email { get; init; }
    public required string Rol { get; init; }
} 