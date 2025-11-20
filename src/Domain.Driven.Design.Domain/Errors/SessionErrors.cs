using ErrorOr;

namespace Domain.Driven.Design.Domain.Errors;

public static class SessionErrors
{
    public static readonly Error CannotHaveMoreReservationsThanParticipants =
        Error.Validation(
            "Session.CannotHaveMoreReservationsThanParticipants",
            "Cannot have more reservations than participants");

    public static readonly Error CannotCancelReservationTooCloseToSession =
        Error.Validation(
            "Session.CannotCancelReservationTooCloseToSession",
            "Cannot cancel reservation too close to session start time");
}
