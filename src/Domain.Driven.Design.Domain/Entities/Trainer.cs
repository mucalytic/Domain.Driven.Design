using Domain.Driven.Design.Domain.Common;
using Domain.Driven.Design.Domain.Errors;
using ErrorOr;

namespace Domain.Driven.Design.Domain.Entities;

public class Trainer(Guid userId, Schedule? schedule = null, Guid? id = null) : Entity<Guid>(id ?? Guid.NewGuid())
{
    private readonly Schedule _schedule = schedule ?? Schedule.Empty();
    private readonly List<Guid> _sessionIds = [];

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
