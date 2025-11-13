using Domain.Driven.Design.Domain;

namespace Domain.Driven.Design.Tests.Unit.Factories;

public static class ParticipantFactory
{
    public static Participant CreateParticipant(Guid? userId, Guid? id = null) =>
        new(userId ?? Constants.Constants.User.Id, id ?? Constants.Constants.Participant.Id);
}
