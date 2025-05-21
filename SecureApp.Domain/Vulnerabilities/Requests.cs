using SecureApp.Domain.Vulnerabilities.Enums;

namespace SecureApp.Domain.Vulnerabilities;

public record CreateVulnerabilidadRequest
{
    public required string Titulo { get; init; }
    public required string Descripcion { get; init; }
    public required string DescubiertaPor { get; init; }
    public required SeveridadVulnerabilidad Severidad { get; init; }
}

public record UpdateVulnerabilidadRequest
{
    public required string Titulo { get; init; }
    public required string Descripcion { get; init; }
    public required EstadoVulnerabilidad Estado { get; init; }
} 