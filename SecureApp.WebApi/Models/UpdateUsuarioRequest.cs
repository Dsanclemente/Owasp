using System.ComponentModel.DataAnnotations;

namespace SecureApp.WebApi.Models;

public class UpdateUsuarioRequest
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    [StringLength(100, ErrorMessage = "El email no puede tener más de 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es requerido")]
    [RegularExpression("^(Admin|Usuario)$", ErrorMessage = "El rol debe ser 'Admin' o 'Usuario'")]
    public string Rol { get; set; } = string.Empty;
} 