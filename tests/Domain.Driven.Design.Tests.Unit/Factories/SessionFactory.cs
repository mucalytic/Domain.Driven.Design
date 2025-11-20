using Domain.Driven.Design.Domain.Objects;

namespace Domain.Driven.Design.Tests.Unit.Factories;

public static class SessionFactory
{
    public static Session CreateSession(
        DateOnly? date = null,
        TimeRange? time = null,
        int maxParticipants = Constants.Session.MaxParticipants,
        Guid? id = null)
    {
        return new Session(
            date ?? Constants.Session.Date,
            time ?? Constants.Session.Time,
            maxParticipants,
            trainerId: Constants.Trainer.Id,
            id: id ?? Constants.Session.Id);
    }
}
