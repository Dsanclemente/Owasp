using SecureApp.Domain.Users;
using SecureApp.Domain.Users.ValueObjects;

namespace SecureApp.Application.Usuarios;

public class GetUsuarioHandler
{
    private readonly IUsuarioRepository _usuarioRepository;

    public GetUsuarioHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<GetUsuarioResult?> Handle(GetUsuarioCommand command)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(UsuarioId.Create(command.Id));
        if (usuario == null)
            return null;

        return new GetUsuarioResult
        {
            Id = usuario.Id.Value,
            Nombre = usuario.Nombre.Value,
            Email = usuario.Email.Value,
            Rol = usuario.Rol.ToString()
        };
    }
} 