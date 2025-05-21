namespace SecureApp.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
} 