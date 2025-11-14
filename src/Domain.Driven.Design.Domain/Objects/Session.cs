using Domain.Driven.Design.Domain.Interfaces;

namespace Domain.Driven.Design.Domain.Objects;

public class Session(
    DateOnly date,
    TimeOnly startTime,
    TimeOnly endTime,
    int maxParticipants,
    Guid trainerId,
    Guid? id = null)
{
    private readonly List<Guid> _participantIds = [];
    private readonly Guid _id = id ?? Guid.NewGuid();

    public void ReserveSpot(Participant participant)
    {
        if (_participantIds.Count >= maxParticipants)
        {
            throw new InvalidOperationException("Maximum number of participants reached");
        }
        _participantIds.Add(participant.Id);
    }

    public void CancelReservation(Participant participant, IDateTimeProvider dateTimeProvider)
    {
        if (IsTooCloseToSession(dateTimeProvider.UtcNow))
        {
            throw new InvalidOperationException("Cancellation is too close to session");
        }
        if (!_participantIds.Remove(participant.Id))
        {
            throw new  InvalidOperationException("Reservation not found");
        }
    }

    private bool IsTooCloseToSession(DateTime utcNow)
    {
        const int minHours = 24;
        // session time - current time < 24 hours
        return (date.ToDateTime(startTime) - utcNow).TotalHours < minHours;
    }
}
