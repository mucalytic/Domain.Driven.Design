using Domain.Driven.Design.Domain;

namespace Domain.Driven.Design.Tests.Unit.Factories;

public static class SessionFactory
{
    public static Session CreateSession(int maxParticipants, Guid? id = null) =>
        new(maxParticipants, Constants.Constants.Trainer.Id, id ?? Constants.Constants.Session.Id);
}
