using Domain.Driven.Design.Domain;
using Domain.Driven.Design.Domain.Objects;

namespace Domain.Driven.Design.Tests.Unit.Factories;

public static class ParticipantFactory
{
    public static Participant CreateParticipant(Guid? userId, Guid? id = null) =>
        new(userId ?? Constants.User.Id, id ?? Constants.Participant.Id);
}
