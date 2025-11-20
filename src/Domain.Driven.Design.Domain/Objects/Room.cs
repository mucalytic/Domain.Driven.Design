using Domain.Driven.Design.Domain.Errors;
using ErrorOr;

namespace Domain.Driven.Design.Domain.Objects;

public class Room(int maxDailySessions, Guid gymId, Schedule? schedule = null, Guid? id = null)
{
    private readonly List<Guid> _sessionIds = [];
    private readonly Schedule _schedule = schedule ?? Schedule.Empty();

    public Guid Id { get; } = id ?? Guid.NewGuid();

    public ErrorOr<Success> ScheduleSession(Session session)
    {
        if (_sessionIds.Any(id => id == session.Id))
        {
            return Error.Conflict(description: "Session already exists in room");
        }
        if (_sessionIds.Count >= maxDailySessions)
        {
            return RoomErrors.CannotHaveMoreSessionThanSubscriptionAllows;
        }
        var addEventResult = _schedule.BookTimeSlot(session.Date, session.Time);
        if (addEventResult is { IsError: true, FirstError.Type: ErrorType.Conflict })
        {
            return RoomErrors.CannotHaveTwoOrMoreOverlappingSessions;
        }
        _sessionIds.Add(session.Id);
        return Result.Success;
    }
}
