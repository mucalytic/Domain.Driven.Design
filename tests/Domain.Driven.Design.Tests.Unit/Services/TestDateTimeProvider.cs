using Domain.Driven.Design.Domain.Interfaces;

namespace Domain.Driven.Design.Tests.Unit.Services;

public class TestDateTimeProvider(DateTime? fixedDateTime = null) : IDateTimeProvider
{
    public DateTime UtcNow => fixedDateTime ?? DateTime.UtcNow;
}
