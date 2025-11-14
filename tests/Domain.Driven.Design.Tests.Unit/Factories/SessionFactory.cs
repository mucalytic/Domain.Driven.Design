using Domain.Driven.Design.Domain.Objects;

namespace Domain.Driven.Design.Tests.Unit.Factories;

public static class SessionFactory
{
    public static Session CreateSession(
        DateOnly? date = null,
        TimeOnly? startTime = null,
        TimeOnly? endTime = null,
        int maxParticipants = Constants.Session.MaxParticipants,
        Guid? id = null) =>
        new(date ?? Constants.Session.Date,
            startTime ?? Constants.Session.StartTime,
            endTime ?? Constants.Session.EndTime,
            maxParticipants,
            trainerId: Constants.Trainer.Id,
            id: id ?? Constants.Session.Id);
}
