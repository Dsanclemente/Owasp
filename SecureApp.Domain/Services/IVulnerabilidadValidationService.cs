using SecureApp.Domain.Vulnerabilities;

namespace SecureApp.Domain.Services;

public interface IVulnerabilidadValidationService
{
    Task<bool> IsDuplicateAsync(string titulo);
    Task<bool> ValidateRulesAsync(Vulnerabilidad vulnerabilidad);
} 