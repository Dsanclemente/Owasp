using SecureApp.Domain.Common;
using SecureApp.Domain.Services;
using SecureApp.Domain.Vulnerabilities;
using SecureApp.Domain.Vulnerabilities.Enums;
using SecureApp.Domain.Vulnerabilities.ValueObjects;

namespace SecureApp.Application.Vulnerabilidades;

public class UpdateVulnerabilidadHandler
{
    private readonly IVulnerabilidadRepository _vulnerabilidadRepository;
    private readonly IVulnerabilidadValidationService _validationService;

    public UpdateVulnerabilidadHandler(IVulnerabilidadRepository vulnerabilidadRepository, IVulnerabilidadValidationService validationService)
    {
        _vulnerabilidadRepository = vulnerabilidadRepository;
        _validationService = validationService;
    }

    public async Task<Result<UpdateVulnerabilidadResult>> Handle(UpdateVulnerabilidadCommand command)
    {
        var vulnerabilidad = await _vulnerabilidadRepository.GetByIdAsync(VulnerabilidadId.Create(command.Id));
        if (vulnerabilidad == null)
            return Result.Failure<UpdateVulnerabilidadResult>("Vulnerabilidad no encontrada.");

        var estadoResult = vulnerabilidad.CambiarEstado(Enum.Parse<EstadoVulnerabilidad>(command.Estado));
        if (estadoResult.IsFailure)
            return Result.Failure<UpdateVulnerabilidadResult>(estadoResult.Error);

        if (command.Severidad != null)
        {
            var severidadResult = vulnerabilidad.ActualizarSeveridad(Enum.Parse<SeveridadVulnerabilidad>(command.Severidad));
            if (severidadResult.IsFailure)
                return Result.Failure<UpdateVulnerabilidadResult>(severidadResult.Error);
        }

        // Validar reglas de negocio
        if (!await _validationService.ValidateRulesAsync(vulnerabilidad))
            return Result.Failure<UpdateVulnerabilidadResult>("La vulnerabilidad no cumple con las reglas de negocio.");

        await _vulnerabilidadRepository.UpdateAsync(vulnerabilidad);

        return Result.Success(new UpdateVulnerabilidadResult
        {
            Id = vulnerabilidad.Id.Value,
            Titulo = vulnerabilidad.Titulo.Value,
            Descripcion = vulnerabilidad.Descripcion.Value,
            Severidad = vulnerabilidad.Severidad.ToString(),
            Estado = vulnerabilidad.Estado.ToString(),
            FechaDescubrimiento = vulnerabilidad.FechaDescubrimiento,
            DescubiertaPor = vulnerabilidad.DescubiertaPor.Value
        });
    }
} 