using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using SecureApp.Domain.Vulnerabilities;
using SecureApp.WebApi.DTOs.Vulnerabilidades;
using SecureApp.Domain.Vulnerabilities.Enums;

namespace SecureApp.WebApi.SwaggerExamples;

public class VulnerabilidadExamples : IExamplesProvider<VulnerabilidadResponse>
{
    public VulnerabilidadResponse GetExamples()
    {
        return new VulnerabilidadResponse
        {
            Id = Guid.NewGuid(),
            Titulo = "Inyección SQL en formulario de búsqueda",
            Descripcion = "Se detectó una vulnerabilidad de inyección SQL en el formulario de búsqueda que permite a atacantes ejecutar consultas SQL maliciosas.",
            DescubiertaPor = Guid.NewGuid(),
            FechaDescubrimiento = DateTime.UtcNow,
            Severidad = SeveridadVulnerabilidad.Alta.ToString(),
            Estado = EstadoVulnerabilidad.Reportada.ToString()
        };
    }
}

public class CreateVulnerabilidadRequestExamples : IExamplesProvider<DTOs.Vulnerabilidades.CreateVulnerabilidadRequest>
{
    public DTOs.Vulnerabilidades.CreateVulnerabilidadRequest GetExamples()
    {
        return new DTOs.Vulnerabilidades.CreateVulnerabilidadRequest
        {
            Titulo = "Vulnerabilidad de Inyección SQL",
            Descripcion = "Se detectó una posible vulnerabilidad de inyección SQL en el endpoint de búsqueda de usuarios.",
            Severidad = SeveridadVulnerabilidad.Alta.ToString()
        };
    }
}

public class UpdateVulnerabilidadRequestExamples : IExamplesProvider<DTOs.Vulnerabilidades.UpdateVulnerabilidadRequest>
{
    public DTOs.Vulnerabilidades.UpdateVulnerabilidadRequest GetExamples()
    {
        return new DTOs.Vulnerabilidades.UpdateVulnerabilidadRequest
        {
            Estado = EstadoVulnerabilidad.EnAnalisis.ToString(),
            Severidad = SeveridadVulnerabilidad.Media.ToString()
        };
    }
} 