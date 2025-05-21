using SecureApp.Domain.Common;
using SecureApp.Domain.Users.Enums;
using SecureApp.Domain.Users.ValueObjects;

namespace SecureApp.Domain.Users;

public class Usuario : Entity
{
    public UsuarioId Id { get; private set; }
    public NombreUsuario Nombre { get; private set; }
    public EmailUsuario Email { get; private set; }
    public RolUsuario Rol { get; private set; }
    public PasswordHash Password { get; private set; }

    private Usuario() { } // Para EF Core

    private Usuario(
        UsuarioId id,
        NombreUsuario nombre,
        EmailUsuario email,
        RolUsuario rol,
        PasswordHash password)
    {
        Id = id;
        Nombre = nombre;
        Email = email;
        Rol = rol;
        Password = password;
    }

    public static Result<Usuario> Create(
        string nombre,
        string email,
        string password,
        RolUsuario rol)
    {
        var nombreResult = NombreUsuario.Create(nombre);
        if (nombreResult.IsFailure)
            return Result.Failure<Usuario>(nombreResult.Error);

        var emailResult = EmailUsuario.Create(email);
        if (emailResult.IsFailure)
            return Result.Failure<Usuario>(emailResult.Error);

        var passwordResult = PasswordHash.Create(password);
        if (passwordResult.IsFailure)
            return Result.Failure<Usuario>(passwordResult.Error);

        return Result.Success<Usuario>(new Usuario(
            UsuarioId.CreateUnique(),
            nombreResult.Value,
            emailResult.Value,
            rol,
            passwordResult.Value));
    }

    public Result CambiarRol(RolUsuario nuevoRol)
    {
        if (Rol == nuevoRol)
            return Result.Failure("El usuario ya tiene asignado este rol");

        Rol = nuevoRol;
        return Result.Success();
    }

    public Result ActualizarPassword(string nuevoPassword)
    {
        var passwordResult = PasswordHash.Create(nuevoPassword);
        if (passwordResult.IsFailure)
            return Result.Failure(passwordResult.Error);

        Password = passwordResult.Value;
        return Result.Success();
    }

    public Result Actualizar(string nombre, string email, RolUsuario rol)
    {
        var nombreResult = NombreUsuario.Create(nombre);
        if (nombreResult.IsFailure)
            return Result.Failure(nombreResult.Error);

        var emailResult = EmailUsuario.Create(email);
        if (emailResult.IsFailure)
            return Result.Failure(emailResult.Error);

        Nombre = nombreResult.Value;
        Email = emailResult.Value;
        Rol = rol;

        return Result.Success();
    }
} 