using SecureApp.Domain.Common;

namespace SecureApp.Domain.Vulnerabilities.ValueObjects;

public class TituloVulnerabilidad : ValueObject
{
    public string Value { get; }

    private TituloVulnerabilidad(string value)
    {
        Value = value;
    }

    public static Result<TituloVulnerabilidad> Create(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Failure<TituloVulnerabilidad>("El título no puede estar vacío");

        if (titulo.Length < 5)
            return Result.Failure<TituloVulnerabilidad>("El título debe tener al menos 5 caracteres");

        if (titulo.Length > 100)
            return Result.Failure<TituloVulnerabilidad>("El título no puede tener más de 100 caracteres");

        return Result.Success<TituloVulnerabilidad>(new TituloVulnerabilidad(titulo));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
} 