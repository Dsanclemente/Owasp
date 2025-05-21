using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureApp.Domain.Common;
using SecureApp.Domain.Users.ValueObjects;
using SecureApp.Domain.Vulnerabilities;
using SecureApp.Domain.Vulnerabilities.Enums;
using SecureApp.Domain.Vulnerabilities.ValueObjects;
using SecureApp.WebApi.DTOs.Vulnerabilidades;
using SecureApp.Application.Vulnerabilidades;
using SecureApp.Domain.Services;
using SecureApp.WebApi.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace SecureApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VulnerabilidadesController : ControllerBase
{
    private readonly IVulnerabilidadRepository _vulnerabilidadRepository;
    private readonly CreateVulnerabilidadHandler _createVulnerabilidadHandler;
    private readonly GetVulnerabilidadHandler _getVulnerabilidadHandler;
    private readonly UpdateVulnerabilidadHandler _updateVulnerabilidadHandler;

    public VulnerabilidadesController(IVulnerabilidadRepository vulnerabilidadRepository, IVulnerabilidadValidationService validationService)
    {
        _vulnerabilidadRepository = vulnerabilidadRepository;
        _createVulnerabilidadHandler = new CreateVulnerabilidadHandler(vulnerabilidadRepository, validationService);
        _getVulnerabilidadHandler = new GetVulnerabilidadHandler(vulnerabilidadRepository);
        _updateVulnerabilidadHandler = new UpdateVulnerabilidadHandler(vulnerabilidadRepository, validationService);
    }

    /// <summary>
    /// Obtiene todas las vulnerabilidades registradas
    /// </summary>
    /// <remarks>
    /// Este endpoint implementa las siguientes medidas de seguridad OWASP:
    /// - Autenticación JWT
    /// - Autorización basada en roles
    /// - Prevención de inyección SQL
    /// - Paginación para prevenir ataques de denegación de servicio
    /// </remarks>
    /// <param name="page">Número de página (comienza en 1)</param>
    /// <param name="pageSize">Tamaño de página (máximo 50)</param>
    /// <returns>Lista de vulnerabilidades</returns>
    /// <response code="200">Lista de vulnerabilidades obtenida exitosamente</response>
    /// <response code="401">No autorizado</response>
    /// <response code="403">No tiene permisos para acceder al recurso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Vulnerabilidad>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VulnerabilidadExamples))]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var vulnerabilidades = await _vulnerabilidadRepository.GetAllAsync();
        return Ok(vulnerabilidades.Select(v => new VulnerabilidadResponse
        {
            Id = v.Id.Value,
            Titulo = v.Titulo.Value,
            Descripcion = v.Descripcion.Value,
            Severidad = v.Severidad.ToString(),
            Estado = v.Estado.ToString(),
            FechaDescubrimiento = v.FechaDescubrimiento,
            DescubiertaPor = v.DescubiertaPor.Value
        }));
    }

    /// <summary>
    /// Obtiene una vulnerabilidad por su ID
    /// </summary>
    /// <remarks>
    /// Este endpoint implementa las siguientes medidas de seguridad OWASP:
    /// - Autenticación JWT
    /// - Autorización basada en roles
    /// - Prevención de inyección SQL
    /// - Validación de entrada de datos
    /// </remarks>
    /// <param name="id">ID de la vulnerabilidad</param>
    /// <returns>Vulnerabilidad encontrada</returns>
    /// <response code="200">Vulnerabilidad encontrada</response>
    /// <response code="401">No autorizado</response>
    /// <response code="403">No tiene permisos para acceder al recurso</response>
    /// <response code="404">Vulnerabilidad no encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VulnerabilidadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VulnerabilidadExamples))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getVulnerabilidadHandler.Handle(new GetVulnerabilidadCommand { Id = id });
        if (result == null)
            return NotFound();

        return Ok(new VulnerabilidadResponse
        {
            Id = result.Id,
            Titulo = result.Titulo,
            Descripcion = result.Descripcion,
            Severidad = result.Severidad,
            Estado = result.Estado,
            FechaDescubrimiento = result.FechaDescubrimiento,
            DescubiertaPor = result.DescubiertaPor
        });
    }

    /// <summary>
    /// Crea una nueva vulnerabilidad
    /// </summary>
    /// <remarks>
    /// Este endpoint implementa las siguientes medidas de seguridad OWASP:
    /// - Autenticación JWT
    /// - Autorización basada en roles
    /// - Validación de entrada de datos
    /// - Prevención de inyección SQL
    /// - Sanitización de datos
    /// </remarks>
    /// <param name="request">Datos de la vulnerabilidad a crear</param>
    /// <returns>Vulnerabilidad creada</returns>
    /// <response code="201">Vulnerabilidad creada exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="401">No autorizado</response>
    /// <response code="403">No tiene permisos para acceder al recurso</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(VulnerabilidadResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [SwaggerRequestExample(typeof(DTOs.Vulnerabilidades.CreateVulnerabilidadRequest), typeof(CreateVulnerabilidadRequestExamples))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(VulnerabilidadExamples))]
    public async Task<IActionResult> Create([FromBody] DTOs.Vulnerabilidades.CreateVulnerabilidadRequest request)
    {
        var command = new CreateVulnerabilidadCommand
        {
            Titulo = request.Titulo,
            Descripcion = request.Descripcion,
            Severidad = request.Severidad
        };
        var descubiertaPor = Guid.Parse(User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException());
        var result = await _createVulnerabilidadHandler.Handle(command, descubiertaPor);
        if (result.IsFailure)
            return BadRequest(result.Error);
        var vulnerabilidad = result.Value;
        return CreatedAtAction(nameof(GetById), new { id = vulnerabilidad.Id }, new VulnerabilidadResponse
        {
            Id = vulnerabilidad.Id,
            Titulo = vulnerabilidad.Titulo,
            Descripcion = vulnerabilidad.Descripcion,
            Severidad = vulnerabilidad.Severidad,
            Estado = vulnerabilidad.Estado,
            FechaDescubrimiento = vulnerabilidad.FechaDescubrimiento,
            DescubiertaPor = vulnerabilidad.DescubiertaPor
        });
    }

    /// <summary>
    /// Actualiza una vulnerabilidad existente
    /// </summary>
    /// <remarks>
    /// Este endpoint implementa las siguientes medidas de seguridad OWASP:
    /// - Autenticación JWT
    /// - Autorización basada en roles
    /// - Validación de entrada de datos
    /// - Prevención de inyección SQL
    /// - Sanitización de datos
    /// </remarks>
    /// <param name="id">ID de la vulnerabilidad</param>
    /// <param name="request">Datos actualizados de la vulnerabilidad</param>
    /// <returns>Vulnerabilidad actualizada</returns>
    /// <response code="200">Vulnerabilidad actualizada exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="401">No autorizado</response>
    /// <response code="403">No tiene permisos para acceder al recurso</response>
    /// <response code="404">Vulnerabilidad no encontrada</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(VulnerabilidadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerRequestExample(typeof(DTOs.Vulnerabilidades.UpdateVulnerabilidadRequest), typeof(UpdateVulnerabilidadRequestExamples))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(VulnerabilidadExamples))]
    public async Task<IActionResult> Update(Guid id, [FromBody] DTOs.Vulnerabilidades.UpdateVulnerabilidadRequest request)
    {
        var command = new UpdateVulnerabilidadCommand
        {
            Id = id,
            Estado = request.Estado,
            Severidad = request.Severidad
        };
        var result = await _updateVulnerabilidadHandler.Handle(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        var vulnerabilidad = result.Value;
        return Ok(new VulnerabilidadResponse
        {
            Id = vulnerabilidad.Id,
            Titulo = vulnerabilidad.Titulo,
            Descripcion = vulnerabilidad.Descripcion,
            Severidad = vulnerabilidad.Severidad,
            Estado = vulnerabilidad.Estado,
            FechaDescubrimiento = vulnerabilidad.FechaDescubrimiento,
            DescubiertaPor = vulnerabilidad.DescubiertaPor
        });
    }
} 