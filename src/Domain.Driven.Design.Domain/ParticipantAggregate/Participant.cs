using Domain.Driven.Design.Domain.SessionAggregate;
using Domain.Driven.Design.Domain.Common.Entities;
using Domain.Driven.Design.Domain.Common;
using ErrorOr;

namespace Domain.Driven.Design.Domain.ParticipantAggregate;

public class Participant(Guid userId, Guid? id = null) : AggregateRoot(id)
{
    private readonly Schedule _schedule = Schedule.Empty();
    private readonly List<Guid> _sessionIds = [];

    public ErrorOr<Success> AddToSchedule(Session session)
    {
        if (_sessionIds.Contains(session.Id))
        {
            return Error.Conflict(description: "Session already exists in participant's schedule");
        }
        var bookTimeSlotResult = _schedule.BookTimeSlot(session.Date, session.Time);
        if (bookTimeSlotResult.IsError)
        {
            return bookTimeSlotResult.FirstError.Type == ErrorType.Conflict
                ? ParticipantErrors.CannotHaveTwoOrMoreOverlappingSessions
                : bookTimeSlotResult.Errors;
        }
        _sessionIds.Add(session.Id);
        return Result.Success;
    }
}
