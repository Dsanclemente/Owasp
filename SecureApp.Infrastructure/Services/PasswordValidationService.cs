using SecureApp.Domain.Common;
using SecureApp.Domain.Users.Services;

namespace SecureApp.Infrastructure.Services;

public class PasswordValidationService : IPasswordValidationService
{
    public Result ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return Result.Failure("La contraseña no puede estar vacía.");

        if (password.Length < 8)
            return Result.Failure("La contraseña debe tener al menos 8 caracteres.");

        if (!password.Any(char.IsUpper))
            return Result.Failure("La contraseña debe contener al menos una letra mayúscula.");

        if (!password.Any(char.IsLower))
            return Result.Failure("La contraseña debe contener al menos una letra minúscula.");

        if (!password.Any(char.IsDigit))
            return Result.Failure("La contraseña debe contener al menos un número.");

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            return Result.Failure("La contraseña debe contener al menos un carácter especial.");

        return Result.Success();
    }
} 