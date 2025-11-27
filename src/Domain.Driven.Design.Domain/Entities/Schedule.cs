using Domain.Driven.Design.Domain.Objects;
using Domain.Driven.Design.Domain.Common;
using ErrorOr;

namespace Domain.Driven.Design.Domain.Entities;

public class Schedule(Dictionary<DateOnly, List<TimeRange>>? calendar = null, Guid? id = null) :  Entity<Guid>(id ?? Guid.NewGuid())
{
    private readonly Dictionary<DateOnly, List<TimeRange>> _calendar = calendar ?? new Dictionary<DateOnly, List<TimeRange>>();

    public static Schedule Empty() => new(id: Guid.NewGuid());

    internal bool CanBookTimeSlot(DateOnly date, TimeRange time)
    {
        if (!_calendar.TryGetValue(date, out var timeSlots))
        {
            return true;
        }

        return !timeSlots.Any(timeSlot => timeSlot.OverlapsWith(time));
    }

    internal ErrorOr<Success> BookTimeSlot(DateOnly date, TimeRange time)
    {
        if (!_calendar.TryGetValue(date, out var timeSlots))
        {
            _calendar[date] = [time];
            return Result.Success;
        }
        if (timeSlots.Any(timeSlot => timeSlot.OverlapsWith(time)))
        {
            return Error.Conflict();
        }
        timeSlots.Add(time);
        return Result.Success;
    }

    internal ErrorOr<Success> RemoveBooking(DateOnly date, TimeRange time)
    {
        if (!_calendar.TryGetValue(date, out var timeSlots) || !timeSlots.Contains(time))
        {
            return Error.NotFound(description: "Booking not found");
        }
        if (!timeSlots.Remove(time))
        {
            return Error.Unexpected();
        }
        return Result.Success;
    }
}
