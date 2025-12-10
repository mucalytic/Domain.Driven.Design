using Domain.Driven.Design.Domain.Common;
using ErrorOr;
using Throw;

namespace Domain.Driven.Design.Domain.ValueObjects;

public class TimeRange(TimeOnly start, TimeOnly end) : ValueObject<TimeRange>
{
    public TimeOnly Start { get; } = start.Throw().IfGreaterThanOrEqualTo(end);
    public TimeOnly End   { get; } = end;

    public static ErrorOr<TimeRange> FromDateTimes(DateTime start, DateTime end)
    {
        if (start.Date != end.Date || start >= end) return Error.Validation();
        return new TimeRange(TimeOnly.FromDateTime(start), TimeOnly.FromDateTime(end));
    }

    public bool OverlapsWith(TimeRange other)
    {
        if (Start >= other.End) return false;
        if (other.Start >= End) return false;
        return true;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}
