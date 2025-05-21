using SecureApp.Domain.Common;

namespace SecureApp.Domain.Users.ValueObjects;

public class UsuarioId : ValueObject
{
    public Guid Value { get; }

    private UsuarioId(Guid value)
    {
        Value = value;
    }

    public static UsuarioId CreateUnique()
    {
        return new UsuarioId(Guid.NewGuid());
    }

    public static UsuarioId Create(Guid value)
    {
        return new UsuarioId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
} 