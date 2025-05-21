using SecureApp.Domain.Common;

namespace SecureApp.Domain.Users.Services;

public interface IPasswordValidationService
{
    Result ValidatePassword(string password);
} 