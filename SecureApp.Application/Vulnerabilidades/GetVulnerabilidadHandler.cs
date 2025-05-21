using SecureApp.Domain.Vulnerabilities;
using SecureApp.Domain.Vulnerabilities.ValueObjects;

namespace SecureApp.Application.Vulnerabilidades;

public class GetVulnerabilidadHandler
{
    private readonly IVulnerabilidadRepository _vulnerabilidadRepository;

    public GetVulnerabilidadHandler(IVulnerabilidadRepository vulnerabilidadRepository)
    {
        _vulnerabilidadRepository = vulnerabilidadRepository;
    }

    public async Task<GetVulnerabilidadResult?> Handle(GetVulnerabilidadCommand command)
    {
        var vulnerabilidad = await _vulnerabilidadRepository.GetByIdAsync(VulnerabilidadId.Create(command.Id));
        if (vulnerabilidad == null)
            return null;

        return new GetVulnerabilidadResult
        {
            Id = vulnerabilidad.Id.Value,
            Titulo = vulnerabilidad.Titulo.Value,
            Descripcion = vulnerabilidad.Descripcion.Value,
            Severidad = vulnerabilidad.Severidad.ToString(),
            Estado = vulnerabilidad.Estado.ToString(),
            FechaDescubrimiento = vulnerabilidad.FechaDescubrimiento,
            DescubiertaPor = vulnerabilidad.DescubiertaPor.Value
        };
    }
} 