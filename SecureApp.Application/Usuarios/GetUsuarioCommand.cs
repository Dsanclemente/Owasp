namespace SecureApp.Application.Usuarios;

public class GetUsuarioCommand
{
    public Guid Id { get; set; }
}

public class GetUsuarioResult
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
} 