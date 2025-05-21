namespace SecureApp.Application.Vulnerabilidades;

public class GetVulnerabilidadCommand
{
    public Guid Id { get; set; }
}

public class GetVulnerabilidadResult
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Severidad { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaDescubrimiento { get; set; }
    public Guid DescubiertaPor { get; set; }
} 