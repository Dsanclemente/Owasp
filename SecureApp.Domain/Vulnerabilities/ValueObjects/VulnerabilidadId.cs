using SecureApp.Domain.Common;

namespace SecureApp.Domain.Vulnerabilities.ValueObjects;

public class VulnerabilidadId : ValueObject
{
    public Guid Value { get; }

    private VulnerabilidadId(Guid value)
    {
        Value = value;
    }

    public static VulnerabilidadId CreateUnique()
    {
        return new VulnerabilidadId(Guid.NewGuid());
    }

    public static VulnerabilidadId Create(Guid value)
    {
        return new VulnerabilidadId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
} 