using SecureApp.Domain.Common;

namespace SecureApp.Domain.Vulnerabilities.ValueObjects;

public class DescripcionVulnerabilidad : ValueObject
{
    public string Value { get; }

    private DescripcionVulnerabilidad(string value)
    {
        Value = value;
    }

    public static Result<DescripcionVulnerabilidad> Create(string descripcion)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            return Result.Failure<DescripcionVulnerabilidad>("La descripción no puede estar vacía");

        if (descripcion.Length < 20)
            return Result.Failure<DescripcionVulnerabilidad>("La descripción debe tener al menos 20 caracteres");

        if (descripcion.Length > 1000)
            return Result.Failure<DescripcionVulnerabilidad>("La descripción no puede tener más de 1000 caracteres");

        return Result.Success<DescripcionVulnerabilidad>(new DescripcionVulnerabilidad(descripcion));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
} 