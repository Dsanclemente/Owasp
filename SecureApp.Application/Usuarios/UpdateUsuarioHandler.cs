using SecureApp.Domain.Common;
using SecureApp.Domain.Users;
using SecureApp.Domain.Users.Enums;
using SecureApp.Domain.Users.ValueObjects;

namespace SecureApp.Application.Usuarios;

public class UpdateUsuarioHandler
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UpdateUsuarioHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Result<UpdateUsuarioResult>> Handle(UpdateUsuarioCommand command)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(UsuarioId.Create(command.Id));
        if (usuario == null)
            return Result.Failure<UpdateUsuarioResult>("Usuario no encontrado.");

        // Validar que el email no esté duplicado
        var existingUsuario = await _usuarioRepository.GetByEmailAsync(command.Email);
        if (existingUsuario != null && existingUsuario.Id.Value != command.Id)
            return Result.Failure<UpdateUsuarioResult>("El email ya está en uso.");

        var result = usuario.Actualizar(
            command.Nombre,
            command.Email,
            command.Rol
        );

        if (result.IsFailure)
            return Result.Failure<UpdateUsuarioResult>(result.Error);

        await _usuarioRepository.UpdateAsync(usuario);

        return Result.Success(new UpdateUsuarioResult
        {
            Id = usuario.Id.Value,
            Nombre = usuario.Nombre.Value,
            Email = usuario.Email.Value,
            Rol = usuario.Rol.ToString()
        });
    }
} 