using SecureApp.Domain.Common;

namespace SecureApp.Domain.Users.ValueObjects;

public class PasswordHash : ValueObject
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        Value = value;
    }

    public static Result<PasswordHash> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return Result.Failure<PasswordHash>("La contraseña no puede estar vacía");

        if (password.Length < 8)
            return Result.Failure<PasswordHash>("La contraseña debe tener al menos 8 caracteres");

        if (!password.Any(char.IsUpper))
            return Result.Failure<PasswordHash>("La contraseña debe contener al menos una mayúscula");

        if (!password.Any(char.IsLower))
            return Result.Failure<PasswordHash>("La contraseña debe contener al menos una minúscula");

        if (!password.Any(char.IsDigit))
            return Result.Failure<PasswordHash>("La contraseña debe contener al menos un número");

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            return Result.Failure<PasswordHash>("La contraseña debe contener al menos un carácter especial");

        return Result.Success<PasswordHash>(new PasswordHash(password));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
} 