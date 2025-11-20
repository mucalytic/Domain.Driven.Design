using Domain.Driven.Design.Domain.Errors;
using ErrorOr;

namespace Domain.Driven.Design.Domain.Objects;

public class Trainer(Guid userId, Schedule? schedule = null, Guid? id = null)
{
    private readonly List<Guid> _sessionIds = [];
    private readonly Guid _id = id ?? Guid.NewGuid();
    private readonly Schedule _schedule = schedule ?? Schedule.Empty();

    public ErrorOr<Success> AddSessionToSchedule(Session session)
    {
        if (_sessionIds.Contains(session.Id))
        {
            return Error.Conflict(description: "Session already exists in trainer's schedule");
        }
        var bookTimeSlotResult = _schedule.BookTimeSlot(session.Date, session.Time);
        if (bookTimeSlotResult is { IsError: true, FirstError.Type: ErrorType.Conflict })
        {
            return TrainerErrors.CannotHaveTwoOrMoreOverlappingSessions;
        }
        _sessionIds.Add(session.Id);
        return Result.Success;
    }
}
