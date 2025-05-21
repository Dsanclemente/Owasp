using SecureApp.Domain.Common;

namespace SecureApp.Domain.Users.ValueObjects;

public class NombreUsuario : ValueObject
{
    public string Value { get; }

    private NombreUsuario(string value)
    {
        Value = value;
    }

    public static Result<NombreUsuario> Create(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return Result.Failure<NombreUsuario>("El nombre no puede estar vacío");

        if (nombre.Length < 3)
            return Result.Failure<NombreUsuario>("El nombre debe tener al menos 3 caracteres");

        if (nombre.Length > 50)
            return Result.Failure<NombreUsuario>("El nombre no puede tener más de 50 caracteres");

        return Result.Success<NombreUsuario>(new NombreUsuario(nombre));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
} 