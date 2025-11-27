using Domain.Driven.Design.Domain.Interfaces;
using Domain.Driven.Design.Domain.Objects;
using Domain.Driven.Design.Domain.Common;
using Domain.Driven.Design.Domain.Errors;
using ErrorOr;

namespace Domain.Driven.Design.Domain.Entities;

public class Session(DateOnly date, TimeRange time, int maxParticipants, Guid trainerId, Guid? id = null) : Entity<Guid>(id ?? Guid.NewGuid())
{
    private readonly List<Guid> _participantIds = [];
    private readonly Guid _trainerId = trainerId;

    public DateOnly Date  { get; } = date;
    public TimeRange Time { get; } = time;

    public ErrorOr<Success> CancelReservation(Participant participant, IDateTimeProvider dateTimeProvider)
    {
        if (IsTooCloseToSession(dateTimeProvider.UtcNow))
        {
            return SessionErrors.CannotCancelReservationTooCloseToSession;
        }
        if (!_participantIds.Remove(participant.Id))
        {
            return Error.NotFound(description: "Participant not found");
        }
        return Result.Success;
    }

    private bool IsTooCloseToSession(DateTime utcNow)
    {
        const int minHours = 24;
        return (Date.ToDateTime(Time.Start) - utcNow).TotalHours < minHours;
    }

    public ErrorOr<Success> ReserveSpot(Participant participant)
    {
        if (_participantIds.Count >= maxParticipants)
        {
            return SessionErrors.CannotHaveMoreReservationsThanParticipants;
        }
        if (_participantIds.Contains(participant.Id))
        {
            return Error.Conflict(description: "Participants cannot reserve twice to the same session");
        }
        _participantIds.Add(participant.Id);
        return Result.Success;
    }
}
