using System.ComponentModel.DataAnnotations;

namespace SecureApp.WebApi.Models;

public class CreateVulnerabilidadRequest
{
    [Required(ErrorMessage = "El título es requerido")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "El título debe tener entre 5 y 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 1000 caracteres")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La severidad es requerida")]
    [RegularExpression("^(Alta|Media|Baja)$", ErrorMessage = "La severidad debe ser 'Alta', 'Media' o 'Baja'")]
    public string Severidad { get; set; } = string.Empty;
} 