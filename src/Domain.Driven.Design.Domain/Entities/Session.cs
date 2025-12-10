using Domain.Driven.Design.Domain.ValueObjects;
using Domain.Driven.Design.Domain.Interfaces;
using Domain.Driven.Design.Domain.Common;
using Domain.Driven.Design.Domain.Errors;
using ErrorOr;

namespace Domain.Driven.Design.Domain.Entities;

public class Session(DateOnly date, TimeRange time, int maxParticipants, Guid trainerId, Guid? id = null) : GuidEntity(id)
{
    private readonly List<Reservation> _reservations = [];
    private readonly Guid _trainerId = trainerId;

    public DateOnly Date  { get; } = date;
    public TimeRange Time { get; } = time;

    public ErrorOr<Success> CancelReservation(Participant participant, IDateTimeProvider dateTimeProvider)
    {
        if (IsTooCloseToSession(dateTimeProvider.UtcNow))
        {
            return SessionErrors.CannotCancelReservationTooCloseToSession;
        }
        var reservation = _reservations.Find(r => r.ParticipantId == participant.Id);
        if (reservation is null)
        {
            return Error.NotFound(description: "Participant not found");
        }
        _reservations.Remove(reservation);
        return Result.Success;
    }

    private bool IsTooCloseToSession(DateTime utcNow)
    {
        const int minHours = 24;
        return (Date.ToDateTime(Time.Start) - utcNow).TotalHours < minHours;
    }

    public ErrorOr<Success> ReserveSpot(Participant participant)
    {
        if (_reservations.Count >= maxParticipants)
        {
            return SessionErrors.CannotHaveMoreReservationsThanParticipants;
        }
        if (_reservations.Any(r => r.ParticipantId == participant.Id))
        {
            return Error.Conflict(description: "Participants cannot reserve twice to the same session");
        }
        _reservations.Add(new Reservation(participant.Id));
        return Result.Success;
    }
}
