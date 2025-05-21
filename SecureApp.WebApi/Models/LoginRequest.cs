using System.ComponentModel.DataAnnotations;

namespace SecureApp.WebApi.Models;

public class LoginRequest
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    public string Password { get; set; } = string.Empty;
} 