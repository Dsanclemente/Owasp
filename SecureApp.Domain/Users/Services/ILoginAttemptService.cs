using SecureApp.Domain.Common;
using SecureApp.Domain.Users.ValueObjects;

namespace SecureApp.Domain.Users.Services;

public interface ILoginAttemptService
{
    Task<bool> IsAccountLockedAsync(EmailUsuario email);
    Task RecordFailedAttemptAsync(EmailUsuario email);
    Task ResetFailedAttemptsAsync(EmailUsuario email);
} 