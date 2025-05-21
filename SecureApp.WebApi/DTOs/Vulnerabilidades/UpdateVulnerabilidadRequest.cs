namespace SecureApp.WebApi.DTOs.Vulnerabilidades;

public class UpdateVulnerabilidadRequest
{
    public string Estado { get; set; } = string.Empty;
    public string? Severidad { get; set; }
} 