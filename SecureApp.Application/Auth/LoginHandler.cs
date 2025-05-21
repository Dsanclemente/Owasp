using BCrypt.Net;
using SecureApp.Domain.Users;

namespace SecureApp.Application.Auth;

public class LoginHandler
{
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<LoginResult?> Handle(LoginCommand command)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(command.Email);
        if (usuario == null)
            return null;

        // Validar el hash de la contraseña
        bool passwordValid = BCrypt.Net.BCrypt.Verify(command.Password, usuario.Password.Value);
        if (!passwordValid)
            return null;

        return new LoginResult
        {
            Id = usuario.Id.Value,
            Email = usuario.Email.Value,
            Rol = usuario.Rol.ToString(),
            PasswordHash = usuario.Password.Value
        };
    }
} 