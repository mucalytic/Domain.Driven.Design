using ErrorOr;

namespace Domain.Driven.Design.Domain.Errors;

public static class ParticipantErrors
{
    public static readonly Error CannotHaveTwoOrMoreOverlappingSessions =
        Error.Validation(
            "Participant.CannotHaveTwoOrMoreOverlappingSessions",
            "A participant cannot have two or more overlapping sessions");
}
