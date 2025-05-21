using System.ComponentModel.DataAnnotations;

namespace SecureApp.WebApi.Models;

public class CreateUsuarioRequest
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    [StringLength(100, ErrorMessage = "El email no puede tener más de 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
        ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una minúscula, un número y un carácter especial")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es requerido")]
    [RegularExpression("^(Admin|Usuario)$", ErrorMessage = "El rol debe ser 'Admin' o 'Usuario'")]
    public string Rol { get; set; } = string.Empty;
} 