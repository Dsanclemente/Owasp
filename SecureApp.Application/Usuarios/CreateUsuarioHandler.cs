using SecureApp.Domain.Common;
using SecureApp.Domain.Users;
using SecureApp.Domain.Users.Enums;
using SecureApp.Domain.Users.Services;
using SecureApp.Domain.Users.ValueObjects;

namespace SecureApp.Application.Usuarios;

public class CreateUsuarioHandler
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordValidationService _passwordValidationService;

    public CreateUsuarioHandler(
        IUsuarioRepository usuarioRepository,
        IPasswordValidationService passwordValidationService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordValidationService = passwordValidationService;
    }

    public async Task<Result<CreateUsuarioResult>> Handle(CreateUsuarioCommand command)
    {
        var emailResult = EmailUsuario.Create(command.Email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<CreateUsuarioResult>(emailResult.Error);
        }

        var existingUser = await _usuarioRepository.GetByEmailAsync(emailResult.Value.Value);
        if (existingUser != null)
        {
            return Result.Failure<CreateUsuarioResult>("El email ya está registrado");
        }

        var passwordValidation = _passwordValidationService.ValidatePassword(command.Password);
        if (passwordValidation.IsFailure)
        {
            return Result.Failure<CreateUsuarioResult>(passwordValidation.Error);
        }

        var usuarioResult = Usuario.Create(
            command.Nombre,
            command.Email,
            command.Password,
            command.Rol);

        if (usuarioResult.IsFailure)
        {
            return Result.Failure<CreateUsuarioResult>(usuarioResult.Error);
        }

        await _usuarioRepository.AddAsync(usuarioResult.Value);

        return Result.Success(new CreateUsuarioResult
        {
            Id = usuarioResult.Value.Id.Value,
            Nombre = usuarioResult.Value.Nombre.Value,
            Email = usuarioResult.Value.Email.Value,
            Rol = usuarioResult.Value.Rol.ToString()
        });
    }
} 