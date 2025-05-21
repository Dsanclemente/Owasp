using SecureApp.Domain.Common;
using SecureApp.Domain.Services;
using SecureApp.Domain.Vulnerabilities;
using SecureApp.Domain.Vulnerabilities.Enums;
using SecureApp.Domain.Users.ValueObjects;

namespace SecureApp.Application.Vulnerabilidades;

public class CreateVulnerabilidadHandler
{
    private readonly IVulnerabilidadRepository _vulnerabilidadRepository;
    private readonly IVulnerabilidadValidationService _validationService;

    public CreateVulnerabilidadHandler(IVulnerabilidadRepository vulnerabilidadRepository, IVulnerabilidadValidationService validationService)
    {
        _vulnerabilidadRepository = vulnerabilidadRepository;
        _validationService = validationService;
    }

    public async Task<Result<CreateVulnerabilidadResult>> Handle(CreateVulnerabilidadCommand command, Guid descubiertaPor)
    {
        // Validar duplicados
        if (await _validationService.IsDuplicateAsync(command.Titulo))
            return Result.Failure<CreateVulnerabilidadResult>("Ya existe una vulnerabilidad con el mismo título.");

        var result = Vulnerabilidad.Create(
            command.Titulo,
            command.Descripcion,
            Enum.Parse<SeveridadVulnerabilidad>(command.Severidad),
            UsuarioId.Create(descubiertaPor)
        );

        if (result.IsFailure)
            return Result.Failure<CreateVulnerabilidadResult>(result.Error);

        var vulnerabilidad = result.Value;

        // Validar reglas de negocio
        if (!await _validationService.ValidateRulesAsync(vulnerabilidad))
            return Result.Failure<CreateVulnerabilidadResult>("La vulnerabilidad no cumple con las reglas de negocio.");

        await _vulnerabilidadRepository.AddAsync(vulnerabilidad);

        return Result.Success(new CreateVulnerabilidadResult
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