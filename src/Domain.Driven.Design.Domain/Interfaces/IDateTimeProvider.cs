namespace Domain.Driven.Design.Domain.Interfaces;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
