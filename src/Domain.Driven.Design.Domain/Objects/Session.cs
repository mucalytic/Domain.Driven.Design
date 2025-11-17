using Domain.Driven.Design.Domain.Interfaces;
using ErrorOr;

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

    public ErrorOr<Success> ReserveSpot(Participant participant)
    {
        if (_participantIds.Count >= maxParticipants)
        {
            return Error.Validation(
                code: nameof(Constants.ErrorCodes.MaximumNumberOfParticipantsReached),
                description: Constants.ErrorCodes.MaximumNumberOfParticipantsReached);
        }
        _participantIds.Add(participant.Id);
        return Result.Success;
    }

    public ErrorOr<Success> CancelReservation(Participant participant, IDateTimeProvider dateTimeProvider)
    {
        if (IsTooCloseToSession(dateTimeProvider.UtcNow))
        {
            return Error.Validation(
                code: nameof(Constants.ErrorCodes.CancellationIsTooCloseToSession),
                description: Constants.ErrorCodes.CancellationIsTooCloseToSession);
        }
        if (!_participantIds.Remove(participant.Id))
        {
            return Error.Validation(
                code: nameof(Constants.ErrorCodes.ReservationNotFound),
                description: Constants.ErrorCodes.ReservationNotFound);
        }
        return Result.Success;
    }

    private bool IsTooCloseToSession(DateTime utcNow)
    {
        const int minHours = 24;
        // session time - current time < 24 hours
        return (date.ToDateTime(startTime) - utcNow).TotalHours < minHours;
    }
}
