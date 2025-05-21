namespace SecureApp.WebApi.DTOs.Vulnerabilidades;

public class CreateVulnerabilidadRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Severidad { get; set; } = string.Empty;
} 