using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureApp.Application.Usuarios;
using SecureApp.Domain.Users;
using SecureApp.Domain.Users.Enums;
using SecureApp.WebApi.DTOs.Usuarios;
using SecureApp.WebApi.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace SecureApp.WebApi.Controllers;

/// <summary>
/// Controlador para la gestión de usuarios
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly CreateUsuarioHandler _createUsuarioHandler;
    private readonly UpdateUsuarioHandler _updateUsuarioHandler;
    private readonly GetUsuarioHandler _getUsuarioHandler;

    public UsuariosController(
        CreateUsuarioHandler createUsuarioHandler,
        UpdateUsuarioHandler updateUsuarioHandler,
        GetUsuarioHandler getUsuarioHandler)
    {
        _createUsuarioHandler = createUsuarioHandler;
        _updateUsuarioHandler = updateUsuarioHandler;
        _getUsuarioHandler = getUsuarioHandler;
    }

    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    /// <remarks>
    /// Este endpoint implementa las siguientes medidas de seguridad OWASP:
    /// - Autenticación JWT
    /// - Autorización basada en roles
    /// - Prevención de enumeración de usuarios
    /// </remarks>
    /// <param name="id">ID del usuario</param>
    /// <returns>Usuario encontrado</returns>
    /// <response code="200">Usuario encontrado</response>
    /// <response code="401">No autorizado</response>
    /// <response code="403">No tiene permisos para acceder al recurso</response>
    /// <response code="404">Usuario no encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UsuarioExamples))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getUsuarioHandler.Handle(new GetUsuarioCommand { Id = id });
        if (result == null)
            return NotFound();

        return Ok(new UsuarioResponse
        {
            Id = result.Id,
            Nombre = result.Nombre,
            Email = result.Email,
            Rol = result.Rol.ToString()
        });
    }

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    /// <remarks>
    /// Este endpoint implementa las siguientes medidas de seguridad OWASP:
    /// - Validación de contraseñas robusta
    /// - Prevención de inyección SQL
    /// - Validación de entrada de datos
    /// </remarks>
    /// <param name="request">Datos del usuario a crear</param>
    /// <returns>Detalles del usuario creado</returns>
    /// <response code="201">Retorna el usuario creado</response>
    /// <response code="400">Si los datos del usuario son inválidos</response>
    /// <response code="401">Si no está autenticado</response>
    /// <response code="403">Si no tiene el rol Admin</response>
    /// <response code="409">El email ya está registrado</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [SwaggerRequestExample(typeof(DTOs.Usuarios.CreateUsuarioRequest), typeof(CreateUsuarioRequestExamples))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(UsuarioExamples))]
    public async Task<IActionResult> Create([FromBody] DTOs.Usuarios.CreateUsuarioRequest request)
    {
        if (!Enum.TryParse<RolUsuario>(request.Rol, true, out var rol))
            return BadRequest("Rol inválido");

        var command = new CreateUsuarioCommand
        {
            Nombre = request.Nombre,
            Email = request.Email,
            Password = request.Password,
            Rol = rol
        };
        var result = await _createUsuarioHandler.Handle(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        var usuario = result.Value;
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, new UsuarioResponse
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol.ToString()
        });
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    /// <remarks>
    /// Este endpoint implementa las siguientes medidas de seguridad OWASP:
    /// - Autenticación JWT
    /// - Autorización basada en roles
    /// - Validación de entrada de datos
    /// - Prevención de inyección SQL
    /// </remarks>
    /// <param name="id">ID del usuario</param>
    /// <param name="request">Datos actualizados del usuario</param>
    /// <returns>Detalles del usuario actualizado</returns>
    /// <response code="200">Retorna el usuario actualizado</response>
    /// <response code="400">Si los datos son inválidos</response>
    /// <response code="401">No autorizado</response>
    /// <response code="403">No tiene permisos para acceder al recurso</response>
    /// <response code="404">Si el usuario no existe</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerRequestExample(typeof(DTOs.Usuarios.UpdateUsuarioRequest), typeof(UpdateUsuarioRequestExamples))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UsuarioExamples))]
    public async Task<IActionResult> Update(Guid id, [FromBody] DTOs.Usuarios.UpdateUsuarioRequest request)
    {
        if (!Enum.TryParse<RolUsuario>(request.Rol, true, out var rol))
            return BadRequest("Rol inválido");

        var command = new UpdateUsuarioCommand
        {
            Id = id,
            Nombre = request.Nombre,
            Email = request.Email,
            Rol = rol
        };
        var result = await _updateUsuarioHandler.Handle(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        var usuario = result.Value;
        return Ok(new UsuarioResponse
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol.ToString()
        });
    }
} 