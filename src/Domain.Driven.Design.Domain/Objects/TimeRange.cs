using ErrorOr;
using Throw;

namespace Domain.Driven.Design.Domain.Objects;

public class TimeRange(TimeOnly start, TimeOnly end)
{
    public TimeOnly Start { get; init; } = start.Throw().IfGreaterThanOrEqualTo(end);
    public TimeOnly End   { get; init; } = end;

    public static ErrorOr<TimeRange> FromDateTimes(DateTime start, DateTime end)
    {
        if (start.Date != end.Date || start >= end)
        {
            return Error.Validation();
        }
        return new TimeRange(TimeOnly.FromDateTime(start), TimeOnly.FromDateTime(end));
    }

    public bool OverlapsWith(TimeRange other)
    {
        if (Start >= other.End) return false;
        if (other.Start >= End) return false;
        return true;
    }
}
