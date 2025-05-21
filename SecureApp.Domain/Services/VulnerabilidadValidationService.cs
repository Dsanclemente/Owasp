using SecureApp.Domain.Vulnerabilities;
using SecureApp.Domain.Common;
using SecureApp.Domain.Vulnerabilities.Enums;

namespace SecureApp.Domain.Services;

public class VulnerabilidadValidationService : IVulnerabilidadValidationService
{
    private readonly IVulnerabilidadRepository _vulnerabilidadRepository;

    public VulnerabilidadValidationService(IVulnerabilidadRepository vulnerabilidadRepository)
    {
        _vulnerabilidadRepository = vulnerabilidadRepository;
    }

    public async Task<bool> IsDuplicateAsync(string titulo)
    {
        var vulnerabilidades = await _vulnerabilidadRepository.GetAllAsync();
        return vulnerabilidades.Any(v => v.Titulo.Value.Equals(titulo, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> ValidateRulesAsync(Vulnerabilidad vulnerabilidad)
    {
        // Ejemplo de regla: la severidad debe ser 'Alta' si el título contiene 'crítico'
        if (vulnerabilidad.Titulo.Value.Contains("crítico", StringComparison.OrdinalIgnoreCase) && 
            vulnerabilidad.Severidad != SeveridadVulnerabilidad.Alta)
            return false;
        return true;
    }

    public async Task<Result> ValidateVulnerabilidad(Vulnerabilidad vulnerabilidad)
    {
        if (string.IsNullOrWhiteSpace(vulnerabilidad.Titulo.Value))
        {
            return Result.Failure("El título es requerido");
        }

        if (string.IsNullOrWhiteSpace(vulnerabilidad.Descripcion.Value))
        {
            return Result.Failure("La descripción es requerida");
        }

        if (vulnerabilidad.Severidad == SeveridadVulnerabilidad.Critica && 
            vulnerabilidad.Estado == EstadoVulnerabilidad.Reportada)
        {
            await Task.Delay(100);
            return Result.Failure("Las vulnerabilidades críticas no pueden estar en estado reportada");
        }

        return Result.Success();
    }

    public async Task<bool> ValidateVulnerabilidadAsync(Vulnerabilidad vulnerabilidad)
    {
        // Validaciones básicas
        if (string.IsNullOrWhiteSpace(vulnerabilidad.Titulo.Value))
            return false;

        if (string.IsNullOrWhiteSpace(vulnerabilidad.Descripcion.Value))
            return false;

        // Validar estado
        if (!IsValidEstado(vulnerabilidad.Estado))
            return false;

        // Validar severidad
        if (!IsValidSeveridad(vulnerabilidad.Severidad))
            return false;

        // Validar fechas
        if (vulnerabilidad.FechaDescubrimiento > DateTime.UtcNow)
            return false;

        if (vulnerabilidad.FechaCorreccion.HasValue && 
            vulnerabilidad.FechaCorreccion.Value > DateTime.UtcNow)
            return false;

        return true;
    }

    private bool IsValidEstado(EstadoVulnerabilidad estado)
    {
        return Enum.IsDefined(typeof(EstadoVulnerabilidad), estado);
    }

    private bool IsValidSeveridad(SeveridadVulnerabilidad severidad)
    {
        return Enum.IsDefined(typeof(SeveridadVulnerabilidad), severidad);
    }
} 