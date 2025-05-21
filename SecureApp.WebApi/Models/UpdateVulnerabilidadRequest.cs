using System.ComponentModel.DataAnnotations;

namespace SecureApp.WebApi.Models;

public class UpdateVulnerabilidadRequest
{
    [Required(ErrorMessage = "El estado es requerido")]
    [RegularExpression("^(Nueva|EnProceso|Resuelta|Cerrada)$", 
        ErrorMessage = "El estado debe ser 'Nueva', 'EnProceso', 'Resuelta' o 'Cerrada'")]
    public string Estado { get; set; } = string.Empty;

    [RegularExpression("^(Alta|Media|Baja)$", ErrorMessage = "La severidad debe ser 'Alta', 'Media' o 'Baja'")]
    public string? Severidad { get; set; }
} 