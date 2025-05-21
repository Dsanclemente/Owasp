using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using SecureApp.Domain.Users.Enums;
using SecureApp.WebApi.DTOs.Usuarios;
using SecureApp.WebApi.DTOs.Auth;

namespace SecureApp.WebApi.SwaggerExamples;

public class UsuarioExamples : IExamplesProvider<UsuarioResponse>
{
    public UsuarioResponse GetExamples()
    {
        return new UsuarioResponse
        {
            Id = Guid.NewGuid(),
            Nombre = "Juan Pérez",
            Email = "juan.perez@example.com",
            Rol = RolUsuario.Usuario.ToString()
        };
    }
}

public class LoginRequestExamples : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new LoginRequest
        {
            Email = "juan.perez@example.com",
            Password = "Contraseña123!"
        };
    }
}

public class CreateUsuarioRequestExamples : IExamplesProvider<CreateUsuarioRequest>
{
    public CreateUsuarioRequest GetExamples()
    {
        return new CreateUsuarioRequest
        {
            Nombre = "Juan Pérez",
            Email = "juan.perez@example.com",
            Password = "Contraseña123!",
            Rol = RolUsuario.Usuario.ToString()
        };
    }
}

public class UpdateUsuarioRequestExamples : IExamplesProvider<UpdateUsuarioRequest>
{
    public UpdateUsuarioRequest GetExamples()
    {
        return new UpdateUsuarioRequest
        {
            Nombre = "Juan Pérez Actualizado",
            Email = "juan.perez.actualizado@example.com",
            Rol = RolUsuario.Admin.ToString()
        };
    }
} 