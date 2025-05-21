using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureApp.Application.Auth;
using SecureApp.WebApi.DTOs.Auth;
using SecureApp.WebApi.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace SecureApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginHandler _loginHandler;

    public AuthController(LoginHandler loginHandler)
    {
        _loginHandler = loginHandler;
    }

    /// <summary>
    /// Inicia sesión en el sistema
    /// </summary>
    /// <remarks>
    /// Este endpoint implementa las siguientes medidas de seguridad OWASP:
    /// - Validación de credenciales
    /// - Prevención de ataques de fuerza bruta
    /// - Tokens JWT seguros
    /// </remarks>
    /// <param name="request">Credenciales de acceso</param>
    /// <returns>Token JWT de autenticación</returns>
    /// <response code="200">Login exitoso</response>
    /// <response code="400">Credenciales inválidas</response>
    /// <response code="401">Credenciales incorrectas</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExamples))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password
        };
        var result = await _loginHandler.Handle(command);
        if (result == null)
            return Unauthorized(new { message = "Credenciales inválidas" });
        return Ok(result);
    }
} 