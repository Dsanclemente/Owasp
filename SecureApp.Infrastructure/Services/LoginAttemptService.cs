using Microsoft.Extensions.Caching.Memory;
using SecureApp.Domain.Users.Services;
using SecureApp.Domain.Users.ValueObjects;

namespace SecureApp.Infrastructure.Services;

public class LoginAttemptService : ILoginAttemptService
{
    private readonly IMemoryCache _cache;
    private const int MaxFailedAttempts = 5;
    private const int LockoutMinutes = 15;
    private const string AttemptsKeyPrefix = "login_attempts_";
    private const string LockoutKeyPrefix = "account_locked_";

    public LoginAttemptService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<bool> IsAccountLockedAsync(EmailUsuario email)
    {
        var lockoutKey = $"{LockoutKeyPrefix}{email.Value}";
        return Task.FromResult(_cache.TryGetValue(lockoutKey, out _));
    }

    public Task RecordFailedAttemptAsync(EmailUsuario email)
    {
        var attemptsKey = $"{AttemptsKeyPrefix}{email.Value}";
        var attempts = _cache.GetOrCreate(attemptsKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(LockoutMinutes);
            return 0;
        });

        attempts++;

        if (attempts >= MaxFailedAttempts)
        {
            var lockoutKey = $"{LockoutKeyPrefix}{email.Value}";
            _cache.Set(lockoutKey, true, TimeSpan.FromMinutes(LockoutMinutes));
            _cache.Remove(attemptsKey);
        }
        else
        {
            _cache.Set(attemptsKey, attempts, TimeSpan.FromMinutes(LockoutMinutes));
        }

        return Task.CompletedTask;
    }

    public Task ResetFailedAttemptsAsync(EmailUsuario email)
    {
        var attemptsKey = $"{AttemptsKeyPrefix}{email.Value}";
        var lockoutKey = $"{LockoutKeyPrefix}{email.Value}";

        _cache.Remove(attemptsKey);
        _cache.Remove(lockoutKey);

        return Task.CompletedTask;
    }
} 