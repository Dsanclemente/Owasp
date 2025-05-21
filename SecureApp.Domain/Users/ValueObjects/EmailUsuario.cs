using System.Text.RegularExpressions;
using SecureApp.Domain.Common;

namespace SecureApp.Domain.Users.ValueObjects;

public class EmailUsuario : ValueObject
{
    public string Value { get; }

    private EmailUsuario(string value)
    {
        Value = value;
    }

    public static Result<EmailUsuario> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<EmailUsuario>("El email no puede estar vacío");

        if (!IsValidEmail(email))
            return Result.Failure<EmailUsuario>("El formato del email no es válido");

        return Result.Success<EmailUsuario>(new EmailUsuario(email));
    }

    private static bool IsValidEmail(string email)
    {
        const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
} 