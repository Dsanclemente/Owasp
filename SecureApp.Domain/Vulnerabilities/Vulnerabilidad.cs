using SecureApp.Domain.Common;
using SecureApp.Domain.Users.ValueObjects;
using SecureApp.Domain.Vulnerabilities.Enums;
using SecureApp.Domain.Vulnerabilities.ValueObjects;

namespace SecureApp.Domain.Vulnerabilities;

public class Vulnerabilidad : Entity
{
    public VulnerabilidadId Id { get; private set; }
    public TituloVulnerabilidad Titulo { get; private set; }
    public DescripcionVulnerabilidad Descripcion { get; private set; }
    public UsuarioId DescubiertaPor { get; private set; }
    public EstadoVulnerabilidad Estado { get; private set; }
    public SeveridadVulnerabilidad Severidad { get; private set; }
    public DateTime FechaDescubrimiento { get; private set; }
    public DateTime? FechaCorreccion { get; private set; }
    public string? Solucion { get; private set; }

    private Vulnerabilidad() { } // Para EF Core

    public Vulnerabilidad(
        TituloVulnerabilidad titulo,
        DescripcionVulnerabilidad descripcion,
        UsuarioId descubiertaPor,
        SeveridadVulnerabilidad severidad)
    {
        Titulo = titulo;
        Descripcion = descripcion;
        DescubiertaPor = descubiertaPor;
        Estado = EstadoVulnerabilidad.Reportada;
        Severidad = severidad;
        FechaDescubrimiento = DateTime.UtcNow;
    }

    public static Result<Vulnerabilidad> Create(
        string titulo,
        string descripcion,
        SeveridadVulnerabilidad severidad,
        UsuarioId descubiertaPor)
    {
        var tituloResult = TituloVulnerabilidad.Create(titulo);
        if (tituloResult.IsFailure)
            return Result.Failure<Vulnerabilidad>(tituloResult.Error);

        var descripcionResult = DescripcionVulnerabilidad.Create(descripcion);
        if (descripcionResult.IsFailure)
            return Result.Failure<Vulnerabilidad>(descripcionResult.Error);

        return Result.Success<Vulnerabilidad>(new Vulnerabilidad(
            tituloResult.Value,
            descripcionResult.Value,
            descubiertaPor,
            severidad));
    }

    public Result CambiarEstado(EstadoVulnerabilidad nuevoEstado)
    {
        if (Estado == nuevoEstado)
            return Result.Failure("La vulnerabilidad ya está en este estado");

        if (Estado == EstadoVulnerabilidad.Corregida)
            return Result.Failure("No se puede cambiar el estado de una vulnerabilidad corregida");

        Estado = nuevoEstado;
        return Result.Success();
    }

    public Result ActualizarSeveridad(SeveridadVulnerabilidad nuevaSeveridad)
    {
        if (Severidad == nuevaSeveridad)
            return Result.Failure("La vulnerabilidad ya tiene esta severidad");

        if (Estado == EstadoVulnerabilidad.Corregida)
            return Result.Failure("No se puede cambiar la severidad de una vulnerabilidad corregida");

        Severidad = nuevaSeveridad;
        return Result.Success();
    }

    public void MarcarComoEnAnalisis()
    {
        if (Estado != EstadoVulnerabilidad.Reportada)
        {
            throw new InvalidOperationException("Solo las vulnerabilidades reportadas pueden ser marcadas como en análisis.");
        }

        Estado = EstadoVulnerabilidad.EnAnalisis;
    }

    public void MarcarComoCorregida(string solucion)
    {
        if (Estado != EstadoVulnerabilidad.EnAnalisis)
        {
            throw new InvalidOperationException("Solo las vulnerabilidades en análisis pueden ser marcadas como corregidas.");
        }

        Estado = EstadoVulnerabilidad.Corregida;
        Solucion = solucion;
        FechaCorreccion = DateTime.UtcNow;
    }

    public void MarcarComoVerificada()
    {
        if (Estado != EstadoVulnerabilidad.Corregida)
        {
            throw new InvalidOperationException("Solo las vulnerabilidades corregidas pueden ser marcadas como verificadas.");
        }

        Estado = EstadoVulnerabilidad.Verificada;
    }

    public void Cerrar()
    {
        if (Estado != EstadoVulnerabilidad.Verificada)
        {
            throw new InvalidOperationException("Solo las vulnerabilidades verificadas pueden ser cerradas.");
        }

        Estado = EstadoVulnerabilidad.Cerrada;
    }
} 