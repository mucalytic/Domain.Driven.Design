using Domain.Driven.Design.Domain.Errors;
using Domain.Driven.Design.Domain.Interfaces;
using ErrorOr;

namespace Domain.Driven.Design.Domain.Objects;

public class Session(DateOnly date, TimeRange time, int maxParticipants, Guid trainerId, Guid? id = null)
{
    private readonly Guid _trainerId = trainerId;
    private readonly List<Guid> _participantIds = [];

    public Guid Id        { get; } = id ?? Guid.NewGuid();
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
