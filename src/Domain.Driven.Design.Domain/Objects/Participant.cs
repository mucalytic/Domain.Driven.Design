namespace Domain.Driven.Design.Domain.Objects;

public class Participant(Guid userId, Guid? id = null)
{
    private readonly List<Guid> _sessionIds = [];

    public Guid Id { get; } = id ?? Guid.NewGuid();
}
