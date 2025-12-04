using Domain.Driven.Design.Domain.Entities;

namespace Domain.Driven.Design.Tests.Unit.Factories;

public static class ParticipantFactory
{
    public static Participant CreateParticipant(Guid? id = null, Guid? userId = null) => 
        new(userId: userId ?? Constants.User.Id, id: id ?? Constants.Participant.Id);
}
